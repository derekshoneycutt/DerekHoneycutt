using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IcdDatabaseBuilder.CmsFiles
{
    /// <summary>
    /// Class to handle Additional Codes section in a Tabular XML file from the CMS
    /// </summary>
    public class TabularAddCodes
    {
        /// <summary>
        /// Gets the Additional Codes associated to the object
        /// </summary>
        public IEnumerable<string> AdditionalCodes { get; private set; }
        /// <summary>
        /// Gets the "Code Also" codes associated to the object
        /// </summary>
        public IEnumerable<string> CodeAlso { get; private set; }
        /// <summary>
        /// Gets the "Code First" codes associated to the object
        /// </summary>
        public IEnumerable<string> CodeFirst { get; private set; }

        /// <summary>
        /// Gets the "Excludes 1" codes associated to the object
        /// </summary>
        public IEnumerable<string> Excludes1 { get; private set; }
        /// <summary>
        /// Gets the "Excludes 2" codes associated to the object
        /// </summary>
        public IEnumerable<string> Excludes2 { get; private set; }

        /// <summary>
        /// Clean up any extraneous data at the beginning or end of a code in a string
        /// </summary>
        /// <param name="inCode">Code text to begin with</param>
        /// <returns>Properly cleaned string containing the code</returns>
        private string CleanCode(string inCode)
        {
            var tempCode = inCode.Trim();
            while (tempCode.EndsWith("-") || tempCode.EndsWith(".") || tempCode.EndsWith(","))
            {
                tempCode = tempCode.Substring(0, tempCode.Length - 1);
            }
            while (tempCode.StartsWith("-") || tempCode.StartsWith(".") || tempCode.StartsWith(","))
            {
                tempCode = tempCode.Substring(1);
            }
            return tempCode;
        }

        /// <summary>
        /// Get the codes from a specific string, which may be a range of options
        /// </summary>
        /// <param name="tempCode">Code or Range of Codes held in the string</param>
        /// <param name="codes">Collection that code strings should be added to</param>
        private void BuildCodesFromStr(string tempCode, ICollection<string> codes)
        {
            var hyphLocat = tempCode.IndexOf('-');
            if (hyphLocat >= 0)
            {
                var firstPart = CleanCode(tempCode.Substring(0, hyphLocat));
                var endPart = CleanCode(tempCode.Substring(hyphLocat + 1));

                int offset = 1;
                if (firstPart.Contains('.') || endPart.Contains('.'))
                {
                    if (endPart.Length < 3)
                    {
                        endPart = String.Format("{0}.{1}", firstPart.Substring(0, 3), endPart);
                    }
                    
                    if (firstPart.Substring(0, 3).Equals(endPart.Substring(0, 3), StringComparison.CurrentCultureIgnoreCase))
                    {
                        offset = 5;
                        while (firstPart.Substring(0, offset).Equals(endPart.Substring(0, offset), StringComparison.CurrentCultureIgnoreCase))
                        {
                            ++offset;
                        }
                        --offset;
                    }
                    else
                    {
                        firstPart = firstPart.Substring(0, 3);
                        endPart = endPart.Substring(0, 3);
                    }
                }
                var startChar = tempCode.Substring(0, offset);
                var startCodeStr = CleanCode(firstPart.Substring(offset));
                if (String.IsNullOrWhiteSpace(startCodeStr))
                {
                    startCodeStr = new String('0', endPart.Length - offset);
                }
                var endCodeStr = CleanCode(endPart.Substring(offset));

                var startCode = Convert.ToInt32(startCodeStr);
                int endCode = startCode;
                bool includeAdditionalA = false;
                if (endCodeStr.EndsWith("A", StringComparison.CurrentCultureIgnoreCase))
                {
                    endCode = (Convert.ToInt32(endCodeStr.Substring(0, 1)) * 10) + 9;
                    includeAdditionalA = true;
                }
                else
                {
                    endCode = Convert.ToInt32(endCodeStr);
                }

                for (int codeOn = startCode; codeOn <= endCode; ++codeOn)
                {
                    switch (endCodeStr.Length)
                    {
                        case 1:
                            codes.Add(String.Format("{0}{1:0}", startChar, codeOn));
                            break;
                        case 2:
                            codes.Add(String.Format("{0}{1:00}", startChar, codeOn));
                            break;
                        case 3:
                            codes.Add(String.Format("{0}{1:000}", startChar, codeOn));
                            break;
                        case 4:
                            codes.Add(String.Format("{0}{1:0000}", startChar, codeOn));
                            break;
                        case 5:
                            codes.Add(String.Format("{0}{1:00000}", startChar, codeOn));
                            break;
                        default:
                            codes.Add(String.Format("{0}{1}", startChar, codeOn));
                            break;
                    }
                }
                if (includeAdditionalA)
                {
                    codes.Add(String.Format("{0}{1}", startChar, endCodeStr));
                }
            }
            else
            {
                codes.Add(tempCode);
            }
        }

        /// <summary>
        /// Get all of the codes associated to the string passed, if there are any
        /// </summary>
        /// <param name="textLine">Line of text that the codes are expected to be in somewhere</param>
        /// <returns>Collection of strings containing the codes that were able to be processed</returns>
        private IEnumerable<string> GetCodesFromString(string textLine)
        {
            var codes = new List<string>();

            if (textLine.EndsWith(")"))
            {
                var startLocat = textLine.LastIndexOf('(');
                var useCodes = textLine.Substring(startLocat + 1, textLine.Length - startLocat - 2);

                var codeList = useCodes.Split(new string[] { ", ", ". " }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var code in codeList)
                {
                    var tempCode = CleanCode(code);
                    var locateWith = tempCode.IndexOf("with", StringComparison.CurrentCultureIgnoreCase);
                    bool restIsWith = true;
                    if (locateWith != 0)
                    {
                        if (locateWith >= 0)
                        {
                            tempCode = CleanCode(tempCode.Substring(0, locateWith).Trim());
                        }
                        else
                        {
                            restIsWith = false;
                        }

                        BuildCodesFromStr(tempCode, codes);
                    }

                    if (restIsWith)
                    {
                        break;
                    }
                }
            }
            else if (textLine.StartsWith("code from ", StringComparison.CurrentCultureIgnoreCase))
            {
                int startWord = 2;
                var words = textLine.Split(' ');
                if (words[startWord].Equals("category", StringComparison.CurrentCultureIgnoreCase))
                {
                    ++startWord;
                }
                codes.Add(CleanCode(words[startWord]));
                if (words.Length > startWord + 1)
                {
                    if (words[startWord + 1].Equals("or", StringComparison.CurrentCultureIgnoreCase))
                    {
                        codes.Add(CleanCode(words[startWord + 2]));
                    }
                }
            }

            return codes.AsReadOnly();
        }

        /// <summary>
        /// Initialize the TabularAddCodes object by concatenating information from two previous objects
        /// </summary>
        /// <param name="codes1">First TabularAddCodes object to build off of</param>
        /// <param name="codes2">Second TabularAddCodes object to concatenate objects from</param>
        public TabularAddCodes(TabularAddCodes codes1, TabularAddCodes codes2)
        {
            if ((codes1 != null) || (codes2 != null))
            {
                if (codes1 == null)
                {
                    AdditionalCodes = codes2.AdditionalCodes;
                    CodeAlso = codes2.CodeAlso;
                    CodeFirst = codes2.CodeFirst;

                    Excludes1 = codes2.Excludes1;
                    Excludes2 = codes2.Excludes2;
                }
                else if (codes2 == null)
                {
                    AdditionalCodes = codes1.AdditionalCodes;
                    CodeAlso = codes1.CodeAlso;
                    CodeFirst = codes1.CodeFirst;

                    Excludes1 = codes1.Excludes1;
                    Excludes2 = codes1.Excludes2;
                }
                else
                {
                    AdditionalCodes = codes1.AdditionalCodes.Union(codes2.AdditionalCodes);
                    CodeAlso = codes1.CodeAlso.Union(codes2.CodeAlso);
                    CodeFirst = codes1.CodeFirst.Union(codes2.CodeFirst);

                    Excludes1 = codes1.Excludes1.Union(codes2.Excludes1);
                    Excludes2 = codes1.Excludes2.Union(codes2.Excludes2);
                }
            }
            else
            {
                AdditionalCodes = Enumerable.Empty<string>();
                CodeAlso = Enumerable.Empty<string>();
                CodeFirst = Enumerable.Empty<string>();

                Excludes1 = Enumerable.Empty<string>();
                Excludes2 = Enumerable.Empty<string>();
            }
        }

        /// <summary>
        /// Initialize TabularAddCodes object based off of a Tabular XML Element
        /// </summary>
        /// <param name="parentEl">The Parent Tabular XML Element to build off of</param>
        public TabularAddCodes(XElement parentEl)
        {
            SetToElement(parentEl);
        }

        /// <summary>
        /// Initialize an empty TabularAddCodes object
        /// </summary>
        public TabularAddCodes()
        {
            AdditionalCodes = Enumerable.Empty<string>();
            CodeAlso = Enumerable.Empty<string>();
            CodeFirst = Enumerable.Empty<string>();

            Excludes1 = Enumerable.Empty<string>();
            Excludes2 = Enumerable.Empty<string>();
        }

        /// <summary>
        /// Set the Additional Codes properties (all 3) to be from a given XML element
        /// </summary>
        /// <param name="parentEl">The XML element to pull the additional codes from</param>
        public void SetToElement(XElement parentEl)
        {
            AdditionalCodes = (from addit in parentEl.Elements("useAdditionalCode")
                               from note in addit.Elements("note")
                               from codes in GetCodesFromString(note.Value)
                               select codes).Distinct();
            CodeAlso = (from addit in parentEl.Elements("codeAlso")
                        from note in addit.Elements("note")
                        from codes in GetCodesFromString(note.Value)
                        select codes).Distinct();
            CodeFirst = (from addit in parentEl.Elements("codeFirst")
                         from note in addit.Elements("note")
                         from codes in GetCodesFromString(note.Value)
                         select codes).Distinct();

            Excludes1 = (from addit in parentEl.Elements("excludes1")
                         from note in addit.Elements("note")
                         from codes in GetCodesFromString(note.Value)
                         select codes).Distinct();
            Excludes2 = (from addit in parentEl.Elements("excludes2")
                         from note in addit.Elements("note")
                         from codes in GetCodesFromString(note.Value)
                         select codes).Distinct();
        }
    }
}
