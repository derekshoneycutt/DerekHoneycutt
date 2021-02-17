using ICD.DataAccess.ObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IcdDatabaseBuilder.CmsFiles
{
    public static class XmlListBuilder
    {
        private static readonly string MainEl = "SearchIcd10List";
        private static readonly string ListEl = "OrderedList";
        private static readonly string ItemEl = "Item";
        private static readonly string TypeAttr = "type";
        private static readonly string IdAttr = "id";

        //private static readonly string DividerType = "Divider";
        //private static readonly string CodeType = "Code";

        public static IEnumerable<IcdListItem> GetFromFile(string filename)
        {
            if (!System.IO.File.Exists(filename))
            {
                yield break;
            }

            var doc = XDocument.Load(filename);

            var firstEl = doc.Element(MainEl);
            var listEl = firstEl.Element(ListEl);

            var listId = listEl.Attribute(IdAttr).Value;
            
            var elements = from el in listEl.Elements(ItemEl)
                           select new {
                               Value = el.Value,
                               Type = el.Attribute(TypeAttr).Value
                           };

            var orderNum = 0;
            foreach (var el in elements)
            {
                yield return new IcdListItem()
                        {
                            Id = null,
                            Data = el.Value,
                            ListId = listId,
                            ListOrder = orderNum++,
                            Type = el.Type
                        };
            }
        }
    }
}
