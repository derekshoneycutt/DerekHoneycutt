using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IcdDatabaseBuilder.CmsFiles
{
    internal class CmsData
    {
        public IEnumerable<Models.IcdCmSection> Sections { get; set; }

        public IEnumerable<Models.IcdCmTabCodes> TabularCodes { get; set; }
    }

    internal static class CmsBuilder
    {
        private static string FormatSectionTitle(string title)
        {
            if (title.EndsWith(")"))
            {
                return title.Substring(0, title.LastIndexOf(' '));
            }
            else
            {
                return title;
            }
        }

        private static IEnumerable<Models.IcdCmTabCodes> GetCodes(XElement parentXml, Models.IcdCmSection section, TabularAddCodes parentAddCodes, string parentCode = null)
        {
            var codes = from el in parentXml.Elements("diag")
                        let addCodes = new TabularAddCodes(el)
                        select new
                        {
                            Element = el,
                            Code = new Models.IcdCmTabCodes()
                                   {
                                       SectionRange = section.CodeRange,
                                       Code = el.Element("name").Value,
                                       InclusionTerms = from inclusTerms in el.Elements("inclusionTerm")
                                                        from notes in inclusTerms.Elements("note")
                                                        select notes.Value.Replace("(", String.Empty)
                                                                          .Replace(")", String.Empty)
                                                                          .Replace("[", String.Empty)
                                                                          .Replace("]", String.Empty),
                                       AddCodes = new TabularAddCodes(parentAddCodes, addCodes),
                                       Title = el.Element("desc").Value,
                                       ParentCode = parentCode
                                   }
                        };

            var children = from code in codes
                           from ret in GetCodes(code.Element, section, code.Code.AddCodes, code.Code.Code)
                           select ret;

            return codes.Select(c => c.Code).Concat(children);
        }

        public static CmsData BuildFromTabularFile(string filename)
        {
            var ret = new CmsData()
                        {
                            Sections = Enumerable.Empty<Models.IcdCmSection>(),
                            TabularCodes = Enumerable.Empty<Models.IcdCmTabCodes>()
                        };

            var tabXml = XDocument.Load(filename);

            var mainEl = tabXml.Elements().FirstOrDefault();

            var chapters = from el in mainEl.Elements("chapter")
                           select new
                           {
                               Element = el,
                               AddCodes = new TabularAddCodes(el)
                           };

            var sections = from chap in chapters
                           from el in chap.Element.Elements("section")
                           let elAddCodes = new TabularAddCodes(el)
                           select new
                           {
                               Element = el,
                               Section = new Models.IcdCmSection()
                                            {
                                                AddCodes = new TabularAddCodes(chap.AddCodes, elAddCodes),
                                                CodeRange = el.Attribute("id").Value,
                                                Title = FormatSectionTitle(el.Element("desc").Value)
                                            }
                           };

            var codes = from sect in sections
                        from code in GetCodes(sect.Element, sect.Section, sect.Section.AddCodes)
                        select code;

            ret.Sections = from sect in sections select sect.Section;
            ret.TabularCodes = codes;

            return ret;
        }
    }
}
