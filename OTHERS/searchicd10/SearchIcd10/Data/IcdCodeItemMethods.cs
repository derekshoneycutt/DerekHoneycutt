using ICD.DataAccess.ObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchIcd10.Data
{
    /// <summary>
    /// Extensions to the IcdCodeItem class
    /// </summary>
    internal static class IcdCodeItemMethods
    {
        /// <summary>
        /// Create a new Divider IcdCodeItem object with the given text
        /// </summary>
        /// <param name="text">The new text to use for the divider</param>
        /// <returns>A new IcdCodeItem object set as a divider with the given text</returns>
        public static IcdCodeItem NewDividerItem(string text)
        {
            return new IcdCodeItem()
            {
                Children = null,
                Comment = String.Empty,
                Enabled = false,
                Code = IcdCodeMethods.NewDivider(text)
            };
        }

        /// <summary>
        /// Create a new "More" IcdCodeItem object with the given text
        /// </summary>
        /// <returns>A new IcdCodeItem object set as a "More" with the given text</returns>
        public static IcdCodeItem NewMoreItem()
        {
            return new IcdCodeItem()
            {
                Children = Enumerable.Empty<IcdCodeItem>(),
                Comment = String.Empty,
                Enabled = false,
                Code = new IcdCode()
                {
                    ChildType = String.Empty,
                    Code = String.Empty,
                    CodeType = Lists.SearchList.MoreItemType,
                    RetrievedFrom = IcdCodeStrings.RetrievedFrom_WholeWord,
                    Title = String.Empty
                }
            };
        }

        /// <summary>
        /// Copy a IcdCodeItem object to a new instance
        /// NOTE: Children collection is not copied to a new instance
        /// </summary>
        /// <param name="fromItem">Item to Copy</param>
        /// <returns>A new IcdCodeItem copied from the original</returns>
        public static IcdCodeItem Copy(this IcdCodeItem fromItem)
        {
            return new IcdCodeItem()
            {
                Children = fromItem.Children,
                Code = fromItem.Code.Copy(),
                Comment = fromItem.Comment,
                Enabled = fromItem.Enabled
            };
        }

        /// <summary>
        /// Copy a collection of IcdCodeItem objects to a new collection, each receiving a new instance
        /// NOTE: Children collections of each item are not copied to a new instance
        /// </summary>
        /// <param name="fromItems">Collection of items to copy</param>
        /// <returns>A new collection of new IcdCodeItem objects copied from the original</returns>
        public static IEnumerable<IcdCodeItem> Copy(this IEnumerable<IcdCodeItem> fromItems)
        {
            foreach (var fromItem in fromItems)
            {
                yield return fromItem.Copy();
            }
        }

        /// <summary>
        /// Copy a collection of IcdCodeItem objects to a new collection, each receiving a new instance
        /// NOTE: Children collections of each item are not copied to a new instance by default
        /// </summary>
        /// <param name="fromItems">Collection of items to copy</param>
        /// <param name="modifyNew">Action to modify each new copy before it is returned</param>
        /// <returns>A new collection of new IcdCodeItem objects copied from the original, modified per modifyNew Action</returns>
        public static IEnumerable<IcdCodeItem> Copy(this IEnumerable<IcdCodeItem> fromItems, Action<IcdCodeItem> modifyNew)
        {
            foreach (var fromItem in fromItems)
            {
                var ret = fromItem.Copy();
                modifyNew(ret);
                yield return ret;
            }
        }
    }
}
