using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICD.DataAccess
{
    /// <summary>
    /// Interface describing a Factory to create IICDDataProvider Objects
    /// </summary>
    public interface IDataProviderFactory
    {
        /// <summary>
        /// Get a new IICDDataProvider object
        /// </summary>
        /// <returns>A newly created IICDDataProvider object</returns>
        IICDDataProvider GetIcdDataProvider();
    }
}
