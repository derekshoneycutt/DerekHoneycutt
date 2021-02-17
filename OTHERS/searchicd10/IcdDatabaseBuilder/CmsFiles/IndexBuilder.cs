using ICD.DataAccess.ObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IcdDatabaseBuilder.CmsFiles
{
    internal static class IndexBuilder
    {
        /// <summary>
        /// Remove Parenthesised strings from a string
        /// </summary>
        /// <param name="inStr">Input string possibly containing the parts to remove</param>
        /// <returns>New string with all parenthesised strings removed</returns>
        private static string RemoveParens(string inStr)
        {
            string workStr = inStr;
            int startLocat = -1;
            while ((startLocat = workStr.IndexOf('(')) > -1)
            {
                int endLocat = workStr.IndexOf(')', startLocat);
                if (endLocat < 0) break;

                var parenStr = workStr.Substring(startLocat, (endLocat - startLocat) + 1);
                while (parenStr.Count((c) => c == '(') > parenStr.Count((c) => c == ')'))
                {
                    endLocat = workStr.IndexOf(')', endLocat + 1);
                    if (endLocat < 0)
                    {
                        parenStr = null;
                        break;
                    }
                    parenStr = workStr.Substring(startLocat, (endLocat - startLocat) + 1);
                }
                if (String.IsNullOrEmpty(parenStr)) break;

                workStr = workStr.Remove(startLocat, (endLocat - startLocat) + 1);
            }
            return workStr;
        }

        /// <summary>
        /// Get the full titles of a term according to the element presented
        /// </summary>
        /// <param name="termEl">Element to get the full title of</param>
        /// <returns>The full titles of the desired element</returns>
        private static IEnumerable<string> GetIndexTitle(XElement termEl)
        {
            if (termEl == null)
            {
                yield break;
            }

            if (termEl.Name.LocalName.Equals("mainTerm", StringComparison.CurrentCultureIgnoreCase))
            {
                var titleEl = termEl.Element("title");
                if (titleEl == null) yield break;
                if (!String.IsNullOrWhiteSpace(titleEl.Value))
                {
                    var useStr = titleEl.Value;
                    var nemod = titleEl.Elements("nemod");
                    if (nemod != null)
                    {
                        foreach (var el in nemod)
                        {
                            useStr = useStr.Replace(el.Value, String.Format(" {0} ", el.Value));
                        }
                    }

                    yield return useStr.Replace("(", String.Empty).Replace(")", String.Empty);
                    yield return RemoveParens(useStr);
                }
            }
            else if (termEl.Name.LocalName.Equals("term", StringComparison.CurrentCultureIgnoreCase))
            {
                var stringRets = new string[2];
                var strBuild = new StringBuilder();
                var parentTitles = GetIndexTitle(termEl.Parent);
                var titleEl = termEl.Element("title");

                string useTitleStr = String.Empty;
                if (titleEl != null)
                {
                    useTitleStr = titleEl.Value;
                    var nemod = titleEl.Elements("nemod");
                    if (nemod != null)
                    {
                        foreach (var el in nemod)
                        {
                            useTitleStr = useTitleStr.Replace(el.Value, String.Format(" {0} ", el.Value));
                        }
                    }
                }

                foreach (var title in parentTitles)
                {
                    if (!String.IsNullOrWhiteSpace(title))
                    {
                        strBuild.Append(title).Append(", ");
                    }
                    strBuild.Append(useTitleStr.Replace("(", String.Empty).Replace(")", String.Empty));
                    yield return strBuild.ToString();

                    strBuild.Clear();
                    if (!String.IsNullOrWhiteSpace(title))
                    {
                        strBuild.Append(title).Append(", ");
                    }
                    strBuild.Append(RemoveParens(useTitleStr));
                    yield return strBuild.ToString();

                    strBuild.Clear();
                }
            }
            else
            {
                yield break;
            }
        }

        /// <summary>
        /// Get the "code"/"codes"/"tab" element children from an XElement
        /// </summary>
        /// <param name="parentEl">Parent XElement expected to contain the code elements</param>
        /// <returns>Enumerable of the code elements discovered within the element</returns>
        private static IEnumerable<XElement> GetCodeElements(XElement parentEl)
        {
            if (parentEl == null)
            {
                return Enumerable.Empty<XElement>();
            }
            return from el in parentEl.Elements()
                   where el.Name.LocalName.Equals("code") ||
                         el.Name.LocalName.Equals("codes") ||
                         el.Name.LocalName.Equals("tab")
                   select el;
        }

        /// <summary>
        /// Get all of the elements that contain a code from the term element given, including all children
        /// </summary>
        /// <param name="term">Term element to get all matching elements from</param>
        /// <returns></returns>
        private static IEnumerable<XElement> GetAllWithCode(XElement term)
        {
            if (term == null)
            {
                yield break;
            }

            if (GetCodeElements(term).Count() > 0)
            {
                yield return term;
            }
            var alsos = from el in term.Elements()
                        where (el.Name.LocalName.Equals("see") ||
                               el.Name.LocalName.Equals("seeAlso") ||
                               el.Name.LocalName.Equals("use")) &&
                              (GetCodeElements(el).Count() > 0)
                        select el;
            foreach (var also in alsos)
            {
                yield return also;
            }
            var children = from el in term.Elements("term")
                           from coded in GetAllWithCode(el)
                           select coded;
            foreach (var child in children)
            {
                yield return child;
            }
        }

        /// <summary>
        /// Return a new string with anything unnecessary from a code in the index trimmed off (some contain extra '-' characters we don't need)
        /// </summary>
        /// <param name="inCode">Code to trim</param>
        /// <returns>A new, trimmed up code</returns>
        private static string CleanCode(string inCode)
        {
            string workCode = inCode.Trim();
            while (workCode.EndsWith("-") || workCode.EndsWith("."))
            {
                workCode = workCode.Substring(0, workCode.Length - 1).Trim();
            }
            while (workCode.StartsWith("-") || workCode.StartsWith("."))
            {
                workCode = workCode.Substring(0, workCode.Length - 1).Trim();
            }
            return workCode;
        }

        private static IEnumerable<IcdLinkedTitle> ProcessTerm(XElement termEl, string forType)
        {
            var thisTitles = GetIndexTitle(termEl);

            var codes = from el in GetAllWithCode(termEl)
                        from code in GetCodeElements(el)
                        select CleanCode(code.Value);

            foreach (var code in codes)
            {
                foreach (var title in thisTitles)
                {
                    yield return new IcdLinkedTitle()
                                 {
                                     Id = null,
                                     Code = code.Trim(),
                                     Title = title.Trim(),
                                     Type = forType.Trim()
                                 };
                }
            }

            foreach (var childTerm in termEl.Elements("term"))
            {
                foreach (var ret in ProcessTerm(childTerm, forType))
                {
                    yield return ret;
                }
            }
        }

        public static IEnumerable<IcdLinkedTitle> GetFromFile(string file, string forType)
        {
            var indexXml = XDocument.Load(file);
            var mainTerms = from letter in indexXml.Elements().FirstOrDefault().Elements("letter")
                            from mainTerm in letter.Elements("mainTerm")
                            select mainTerm;

            foreach (var mainTerm in mainTerms)
            {
                foreach (var ret in ProcessTerm(mainTerm, forType))
                {
                    yield return ret;
                }
            }
        }
    }
}
