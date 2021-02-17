using ICD.DataAccess.ObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcdDatabaseBuilder.CmsFiles
{
    internal static class GemsBuilder
    {
        private static readonly HashSet<string> NoGemEquiv =
            new HashSet<string>(StringComparer.CurrentCultureIgnoreCase)
            {
                "NoPCS",
                "NoDx"
            };

        private static Tuple<string, string> SplitByFirstWhitespace(string line)
        {
            var firstSpace = line.Trim().TakeWhile(c => !Char.IsWhiteSpace(c)).Count();
            var first = line.Substring(0, firstSpace);
            var second = line.Substring(firstSpace).Trim();

            return new Tuple<string, string>(first, second);
        }

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
        /// Get the full titles of a term according to the string presented
        /// </summary>
        /// <param name="titleStr">String to get the full title of</param>
        /// <returns>The full titles of the desired string</returns>
        public static IEnumerable<string> GetIndexTitle(string titleStr)
        {
            if (!String.IsNullOrWhiteSpace(titleStr))
            {
                var inList = new SortedList<string, bool>(StringComparer.CurrentCultureIgnoreCase);
                var newStr = titleStr.Replace("(", String.Empty).Replace(")", String.Empty);
                inList.Add(newStr, true);
                yield return newStr;
                newStr = RemoveParens(titleStr);
                if (inList.ContainsKey(newStr))
                {
                    yield return newStr;
                }
            }
        }

        public static Dictionary<string, List<string>> GetGemsDictionary(string file, bool addPeriodToCode)
        {
            var ret = new Dictionary<string, List<string>>(StringComparer.CurrentCultureIgnoreCase);

            if (!String.IsNullOrWhiteSpace(file))
            {
                if (System.IO.File.Exists(file))
                {
                    var icd9Lines = TextFiles.ReadFileLines(file);
                    foreach (var line in icd9Lines)
                    {
                        var firstSplit = SplitByFirstWhitespace(line);
                        var secondSplit = SplitByFirstWhitespace(firstSplit.Item2);

                        if (!NoGemEquiv.Contains(secondSplit.Item1))
                        {
                            List<string> workGem = null;
                            var code = secondSplit.Item1;
                            if ((code.Length > 3) && addPeriodToCode)
                            {
                                code = code.Insert(3, ".");
                            }

                            if (ret.TryGetValue(firstSplit.Item1, out workGem))
                            {
                                workGem.Add(code);
                            }
                            else
                            {
                                ret.Add(firstSplit.Item1, new List<string>() { code });
                            }
                        }
                    }
                }
            }

            return ret;
        }

        public static IEnumerable<IcdLinkedTitle> GetLinkedTitles(string filename, Dictionary<string, List<string>> gemsDict, string forType)
        {
            if (!String.IsNullOrWhiteSpace(filename) && (gemsDict.Count > 0))
            {
                if (System.IO.File.Exists(filename))
                {
                    var icd9Lines = TextFiles.ReadFileLines(filename);
                    foreach (var line in icd9Lines)
                    {
                        var codeSplit = SplitByFirstWhitespace(line);
                        var codeTitles = GetIndexTitle(codeSplit.Item2);

                        List<string> workGem = null;
                        if (gemsDict.TryGetValue(codeSplit.Item1, out workGem))
                        {
                            if (workGem == null)
                            {
                                continue;
                            }
                            foreach (var code in workGem)
                            {
                                foreach (var title in codeTitles)
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
                        }
                        else if (!String.IsNullOrWhiteSpace(codeSplit.Item1))
                        {
                            var matchingCodes = (from term in gemsDict
                                                 where term.Key.StartsWith(codeSplit.Item1)
                                                 from matchCode in term.Value
                                                 select matchCode)
                                                 .Distinct()
                                                 .ToList();
                            var trimmedMatches = from match in matchingCodes
                                                 let trueMatch = match.Replace(".", "")
                                                 where !matchingCodes.Any(s => trueMatch.Substring(0, trueMatch.Length - 1).StartsWith(s.Replace(".", "")))
                                                 select match;
                            foreach (var code in trimmedMatches)
                            {
                                foreach (var title in codeTitles)
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
                        }
                    }
                }
            }
        }

        public static IEnumerable<IcdLinkedTitle> GetLinkedTitlesNoGem(string filename, string forType)
        {
            if (!String.IsNullOrWhiteSpace(filename))
            {
                if (System.IO.File.Exists(filename))
                {
                    var icd10Lines = TextFiles.ReadFileLines(filename);
                    foreach (var line in icd10Lines)
                    {
                        var codeSplit = SplitByFirstWhitespace(line);
                        var codeTitles = GetIndexTitle(codeSplit.Item2);
                        foreach (var title in codeTitles)
                        {
                            yield return new IcdLinkedTitle()
                            {
                                Id = null,
                                Code = codeSplit.Item1.Trim(),
                                Title = title.Trim(),
                                Type = forType.Trim()
                            };
                        }
                    }
                }
            }
        }
    }
}
