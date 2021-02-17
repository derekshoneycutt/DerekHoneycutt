using ICD.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchIcd10.DataProviders
{
    internal class LocalSqliteDataProvider : IAppDataProvider
    {
        private static readonly string m_LoadFile = "NewTest.db";

        public bool TryExistingSession(string startSession = null)
        {
            return true;
        }

        public void CloseSession()
        {
        }

        public void NewSession(string username, string password)
        {
        }

        public List<ICD.DataAccess.ObjectModels.IcdCode> GetSearch(string terms, int skip, int take)
        {
            var fact = new DataProviderFactory();
            var dp = fact.GetSqliteIcdDataProvider(m_LoadFile);
            return dp.GetSearch(terms, skip, take);
        }

        public List<ICD.DataAccess.ObjectModels.IcdCode> GetCode(string code, string codeType)
        {
            var fact = new DataProviderFactory();
            var dp = fact.GetSqliteIcdDataProvider(m_LoadFile);
            return dp.GetCode(code, codeType);
        }

        public List<ICD.DataAccess.ObjectModels.IcdCode> GetAllCode(string code)
        {
            var fact = new DataProviderFactory();
            var dp = fact.GetSqliteIcdDataProvider(m_LoadFile);
            return dp.GetAllCode(code);
        }

        public List<ICD.DataAccess.ObjectModels.IcdCode> GetChildren(string code, string codeType)
        {
            var fact = new DataProviderFactory();
            var dp = fact.GetSqliteIcdDataProvider(m_LoadFile);
            return dp.GetChildren(code, codeType);
        }

        public List<ICD.DataAccess.ObjectModels.IcdCode> GetList(string listId)
        {
            var fact = new DataProviderFactory();
            var dp = fact.GetSqliteIcdDataProvider(m_LoadFile);
            return dp.GetList(listId);
        }
    }
}
