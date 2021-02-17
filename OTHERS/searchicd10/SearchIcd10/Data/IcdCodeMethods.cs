using ICD.DataAccess.ObjectModels;
using System;
using System.Collections.Generic;

namespace SearchIcd10.Data
{
    /// <summary>
    /// Extensions to the IcdCode class
    /// </summary>
    internal static class IcdCodeMethods
    {
        /// <summary>
        /// Get the Children of an IcdCode object with deferred once execution
        /// Does not execute retrieving the children until first enumeration; after, uses the first retrieved collection
        /// </summary>
        /// <param name="fromParent">Parent IcdCode to get the Children from</param>
        /// <param name="useProvider">Data Provider to get the Children from</param>
        /// <returns>Deferred Once Enumerable of IcdCodeItem objects representing the children</returns>
        public static IEnumerable<IcdCodeItem> DeferredOnceChildren(this IcdCode fromParent, DataProviders.IAppDataProvider useProvider)
        {
            return new DeferredOnceChildren(fromParent, useProvider);
        }

        /// <summary>
        /// Wrap an IcdCode object into an IcdCodeItem object
        /// </summary>
        /// <param name="fromCode">IcdCode object to wrap</param>
        /// <param name="useProvider">Data Provider that additional information on the IcdCode will be retrieved from</param>
        /// <returns>New IcdCodeItem object wrapping the original IcdCode object</returns>
        public static IcdCodeItem ToIcdCodeItem(this IcdCode fromCode, DataProviders.IAppDataProvider useProvider)
        {
            return new IcdCodeItem()
                    {
                        Code = fromCode,
                        Comment = String.Empty,
                        Enabled = false,
                        Children = fromCode.DeferredOnceChildren(useProvider)
                    };
        }

        /// <summary>
        /// Copy an IcdCode object to a new instance
        /// </summary>
        /// <param name="fromCode">Code to copy</param>
        /// <returns>A new exact copy of the original IcdCode object</returns>
        public static IcdCode Copy(this IcdCode fromCode)
        {
            return new IcdCode()
                {
                    ChildType = fromCode.ChildType,
                    Code = fromCode.Code,
                    CodeType = fromCode.CodeType,
                    RetrievedFrom = fromCode.RetrievedFrom,
                    Title = fromCode.Title
                };
        }

        /// <summary>
        /// Copy an enumerable of IcdCode objects
        /// </summary>
        /// <param name="fromCodes">Enumerable of IcdCode objects to copy</param>
        /// <returns>A new set of new IcdCode objects, exactly copied from the previous</returns>
        public static IEnumerable<IcdCode> Copy(this IEnumerable<IcdCode> fromCodes)
        {
            foreach (var code in fromCodes)
            {
                yield return code.Copy();
            }
        }

        /// <summary>
        /// Copy an enumerable of IcdCode objects
        /// </summary>
        /// <param name="fromCodes">Enumerable of IcdCode objects to copy</param>
        /// <param name="modifyNew">Action used to modify each new item before it is returned</param>
        /// <returns>A new set of IcdCode objects, with modifications made by modifyNew action to each item</returns>
        public static IEnumerable<IcdCode> Copy(this IEnumerable<IcdCode> fromCodes, Action<IcdCode> modifyNew)
        {
            foreach (var code in fromCodes)
            {
                var ret = code.Copy();
                modifyNew(ret);
                yield return ret;
            }
        }

        /// <summary>
        /// Create a new Divider IcdCode item with a given text
        /// </summary>
        /// <param name="text">Text to base the new divider on</param>
        /// <returns>A new IcdCode object set as a Divider type, with the given text</returns>
        public static IcdCode NewDivider(string text)
        {
            return new IcdCode()
                {
                    ChildType = IcdCodeStrings.ChildType_Parent,
                    Code = String.Empty,
                    CodeType = IcdCodeStrings.CodeType_Divider,
                    RetrievedFrom = IcdCodeStrings.RetrievedFrom_WholeWord,
                    Title = text
                };
        }
    }
}
