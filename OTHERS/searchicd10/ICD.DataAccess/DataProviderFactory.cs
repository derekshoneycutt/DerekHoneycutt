using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICD.DataAccess
{
    /// <summary>
    /// Factory class creating new IICDDataProvider objects
    /// </summary>
    public class DataProviderFactory : IDataProviderFactory
    {
        /// <summary>
        /// Get a new IICDDataProvider object
        /// </summary>
        /// <returns>A newly created IICDDataProvider object</returns>
        public IICDDataProvider GetIcdDataProvider()
        {
            return new ICDDataProvider(ICDDbContext.Create());
        }

        public IICDDataProvider GetSqliteIcdDataProvider(string filename)
        {
            return new ICDSqliteProvider(ICDDbContext.CreateSqlite(filename));
        }
    }
}
