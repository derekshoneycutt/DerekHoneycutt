using ICD.DataAccess;
using ICD.DataAccess.ObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IcdDatabaseBuilder.CmsFiles
{
    internal class PcsTabularCode
    {
        public string ParentCode { get; set; }
        public string Code { get; set; }
        public string Text { get; set; }
    }

    internal class PcsData
    {
        public IEnumerable<PcsTabularCode> ParentCodes { get; set; }
        public IEnumerable<IcdTerm> Terms { get; set; }
    }

    public class PcsOrderTxtLineData
    {
        public string Code { get; set; }
        public bool IsFull { get; set; }
        public string Abbrev { get; set; }
        public string FullText { get; set; }

        public PcsOrderTxtLineData(string inLine)
        {
            if (String.IsNullOrWhiteSpace(inLine)) throw new System.ArgumentException("TxtLineData in Icd10PcsTabular received an invalid string");

            Code = inLine.Substring(6, 8).Trim();
            IsFull = String.Equals("1", inLine.Substring(14, 1));
            Abbrev = inLine.Substring(16, 60).Trim();
            FullText = inLine.Substring(77).Trim();
        }
    }

    internal static class PcsBuilder
    {
        private static PcsData BuildDataFromAxis(XElement axisEl, PcsTabularCode parentCode, bool append, Dictionary<string, PcsOrderTxtLineData> txtLines)
        {
            var ret = new PcsData()
                        {
                            ParentCodes = Enumerable.Empty<PcsTabularCode>(),
                            Terms = Enumerable.Empty<IcdTerm>()
                        };


            var labels = from el in axisEl.Elements()
                         where el.Name.LocalName.Equals("label", StringComparison.CurrentCultureIgnoreCase)
                         let elCode = parentCode != null ? parentCode.Code + el.Attribute("code").Value : el.Attribute("code").Value
                         let titleAndAppend = txtLines.ContainsKey(elCode) ?
                                                new Tuple<string, bool>(txtLines[elCode].FullText, false) :
                                                append ?
                                                    new Tuple<string, bool>(", " + el.Value, true) :
                                                    new Tuple<string, bool>(el.Value, false)
                         let title = titleAndAppend.Item2 ? parentCode.Text + titleAndAppend.Item1 : titleAndAppend.Item1
                         select new
                         {
                             ParentCodes = new PcsTabularCode()
                                            {
                                                ParentCode = parentCode != null ? parentCode.Code : String.Empty,
                                                Code = elCode,
                                                Text = title
                                            },
                             Term = new IcdTerm() 
                                        {
                                            ParentCode = parentCode != null ? parentCode.Code : String.Empty,
                                            Code = elCode,
                                            Section = "",
                                            Title = title,
                                            Type = IcdCodeStrings.CodeType_Procedure
                                        }
                         };

            ret.ParentCodes = from label in labels select label.ParentCodes;
            ret.Terms = from label in labels select label.Term;

            return ret;
        }

        private static PcsData ProcessAxis(IEnumerable<PcsTabularCode> parentCodes, IEnumerable<IGrouping<string, XElement>> axi, string useCode, bool append, Dictionary<string, PcsOrderTxtLineData> txtLines)
        {
            var ret = new PcsData()
                        {
                            ParentCodes = Enumerable.Empty<PcsTabularCode>(),
                            Terms = Enumerable.Empty<IcdTerm>()
                        };

            var useAxi = from elGroup in axi
                         where elGroup.Key == useCode
                         from el in elGroup
                         select el;

            var data = Enumerable.Empty<PcsData>();

            var parentsArray = parentCodes.ToArray();
            
            if (parentsArray.Length > 0)
            {
                data = (from parent in parentsArray
                        select from axis in useAxi
                               select BuildDataFromAxis(axis, parent, append, txtLines)).SelectMany(d => d);
            }
            else
            {
                data = from axis in useAxi
                       select BuildDataFromAxis(axis, null, false, txtLines);
            }

            var dataArray = data.ToArray();

            ret.ParentCodes = from dataPieces in dataArray
                              from piece in dataPieces.ParentCodes
                              select piece;
            ret.Terms = from dataPieces in dataArray
                        from piece in dataPieces.Terms
                        select piece;

            return ret;
        }

        public static Dictionary<string, PcsOrderTxtLineData> GetOrderLineData(string filename)
        {
            return TextFiles.ReadFileLines(filename)
                            .Select((line) => new PcsOrderTxtLineData(line))
                            .OrderBy((line) => line.Code)
                            .ToDictionary((line) => line.Code);
        }

        public static IEnumerable<IcdTerm> BuildWithOrderData(string xmlTabularFile, Dictionary<string, PcsOrderTxtLineData> txtLines)
        {
            var xmlDoc = XDocument.Load(xmlTabularFile);
            var baseEl = xmlDoc.Elements().FirstOrDefault();
            if (baseEl == null)
            {
                yield break;
            }

            var tables = from el in baseEl.Elements()
                         where el.Name.LocalName.Equals("pcsTable", StringComparison.CurrentCultureIgnoreCase)
                         select new
                         {
                             Element = el,
                             Axi = from axis in el.Elements()
                                   where axis.Name.LocalName.Equals("axis", StringComparison.CurrentCultureIgnoreCase)
                                   group axis by axis.Attribute("pos").Value into axisGroup
                                   select axisGroup,
                             Rows = from row in el.Elements()
                                    where row.Name.LocalName.Equals("pcsRow", StringComparison.CurrentCultureIgnoreCase)
                                    select new
                                    {
                                        Element = row,
                                        Axi = from axis in row.Elements()
                                              where axis.Name.LocalName.Equals("axis", StringComparison.CurrentCultureIgnoreCase)
                                              group axis by axis.Attribute("pos").Value into axisGroup
                                              select axisGroup
                                    }
                         };

            foreach (var table in tables)
            {
                var currAdded = ProcessAxis(Enumerable.Empty<PcsTabularCode>(), table.Axi, "1", false, txtLines);
                foreach (var term in currAdded.Terms)
                {
                    yield return term;
                }
                currAdded = ProcessAxis(currAdded.ParentCodes, table.Axi, "2", false, txtLines);
                foreach (var term in currAdded.Terms)
                {
                    yield return term;
                }
                currAdded = ProcessAxis(currAdded.ParentCodes, table.Axi, "3", true, txtLines);
                foreach (var term in currAdded.Terms)
                {
                    yield return term;
                }

                var level3Added = currAdded;
                foreach (var row in table.Rows)
                {
                    currAdded = ProcessAxis(level3Added.ParentCodes, row.Axi, "4", true, txtLines);
                    foreach (var term in currAdded.Terms)
                    {
                        yield return term;
                    }
                    currAdded = ProcessAxis(currAdded.ParentCodes, row.Axi, "5", true, txtLines);
                    foreach (var term in currAdded.Terms)
                    {
                        yield return term;
                    }
                    currAdded = ProcessAxis(currAdded.ParentCodes, row.Axi, "6", true, txtLines);
                    foreach (var term in currAdded.Terms)
                    {
                        yield return term;
                    }
                    currAdded = ProcessAxis(currAdded.ParentCodes, row.Axi, "7", true, txtLines);
                    foreach (var term in currAdded.Terms)
                    {
                        yield return term;
                    }
                }
            }
        }

        public static IEnumerable<IcdTerm> BuildFromFiles(string xmlTabularFile, string txtOrderFile)
        {
            Dictionary<string, PcsOrderTxtLineData> txtLines = GetOrderLineData(txtOrderFile);

            return BuildWithOrderData(xmlTabularFile, txtLines);
        }
    }
}
