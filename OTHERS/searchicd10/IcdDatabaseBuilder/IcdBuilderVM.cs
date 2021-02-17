using ICD.DataAccess;
using ICD.DataAccess.Mapping;
using ICD.DataAccess.ObjectModels;
using IcdDatabaseBuilder.CmsFiles;
using IcdDatabaseBuilder.CmsFiles.Models;
//using Microsoft.Data.ConnectionUI;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace IcdDatabaseBuilder
{
    internal static class IcdFileHelper
    {
        public static string ShowOpenDialog(bool xml, bool txt)
        {
            var openFileDlg = new OpenFileDialog();
            var filterBuilder = new StringBuilder();
            if (xml)
            {
                filterBuilder.Append("XML Files (*.xml)|*.xml|");
            }
            if (txt)
            {
                filterBuilder.Append("Text Files (*.txt)|*.txt|");
            }
            filterBuilder.Append("All Files (*.*)|*.*");
            openFileDlg.Filter = filterBuilder.ToString();
            openFileDlg.ShowDialog();
            return openFileDlg.FileName;
        }

        public static string ShowSaveDialog(bool xml = false)
        {
            var saveFileDlg = new SaveFileDialog();
            var filterBuilder = new StringBuilder();
            if (xml)
            {
                filterBuilder.Append("XML Files (*.xml)|*.xml|");
            }
            filterBuilder.Append("All Files (*.*)|*.*");
            saveFileDlg.Filter = filterBuilder.ToString();
            saveFileDlg.ShowDialog();

            return saveFileDlg.FileName;
        }

        //Disabling SQL Server Connections
        /*public static string ShowSqlServerConnnectionDialog()
        {
            var dlg = new DataConnectionDialog();
            DataSource.AddStandardDataSources(dlg);
            if (DataConnectionDialog.Show(dlg) == System.Windows.Forms.DialogResult.OK)
            {
                return dlg.ConnectionString;
            }
            return String.Empty;
        }*/
    }

    public class IcdFileVm
    {
        public IcdFile File { get; set; }

        public ICommand SetFile { get; private set; }

        public bool IsValidFile { get; set; }

        private void SetFilename()
        {
            bool xml = false;
            bool txt = false;

            switch (File.Type)
            {
                case IcdCmsFileType.CmsIndexXml:
                case IcdCmsFileType.CmsTabularXml:
                case IcdCmsFileType.PcsIndexXml:
                case IcdCmsFileType.PcsTabularXml:
                case IcdCmsFileType.XmlSpecialList:
                    xml = true;
                    File.Filename = IcdFileHelper.ShowOpenDialog(xml, txt);
                    break;
                case IcdCmsFileType.CmsAlternateGemTxt:
                case IcdCmsFileType.CmsAlternateTxt:
                case IcdCmsFileType.CmsGemsTxt:
                case IcdCmsFileType.PcsAlternateGemTxt:
                case IcdCmsFileType.PcsAlternateTxt:
                case IcdCmsFileType.PcsGemsTxt:
                case IcdCmsFileType.PcsOrderTxt:
                    txt = true;
                    File.Filename = IcdFileHelper.ShowOpenDialog(xml, txt);
                    break;
                case IcdCmsFileType.SqliteFileSave:
                    File.Filename = IcdFileHelper.ShowSaveDialog();
                    break;
                /*case IcdCmsFileType.SqlServerSave:
                    File.Filename = IcdFileHelper.ShowSqlServerConnnectionDialog();
                    break;*/
                default:
                    return;
            }

        }

        public IcdFileVm(IcdFile model)
        {
            File = model;
            SetFile = new ActionCommand(SetFilename);
            IsValidFile = true;
        }
    }

    public class IcdBuilderVM : NotifyingModel
    {
        public ObservableCollection<IcdFileVm> IcdFiles { get; private set; }

        private static readonly string[] m_FileTypes = new string[] 
            {
                "",
                "Diagnosis Tabular XML",
                "Diagnosis Index XML",
                "Diagnosis Alternate Text",
                "Diagnosis GEMs Text",
                "Diagnosis Alternate through GEMs Text",
                "Procedures Tabular XML",
                "Procedures Order Text",
                "Procedures Index XML",
                "Procedures Alternate Text",
                "Procedures GEMs Text",
                "Procedures Alternate through GEMs Text",
                "Special XML Code List",
                "Save to SQLite File",
                //Disable sql server option
                //"Save to SQL Server Connection"
            };
        public IEnumerable<string> FileTypes { get { return m_FileTypes; } }

        public static readonly PropertyChangedEventArgs ProgBarVisibleChangedArgs = new PropertyChangedEventArgs("ProgBarVisible");
        private bool m_ProgBarVisible;
        public bool ProgBarVisible
        {
            get { return m_ProgBarVisible; }
            set { SetValue(ref m_ProgBarVisible, value, ProgBarVisibleChangedArgs); }
        }


        public static readonly PropertyChangedEventArgs ProgBarMaxChangedArgs = new PropertyChangedEventArgs("ProgBarMax");
        private int m_ProgBarMax;
        public int ProgBarMax
        {
            get { return m_ProgBarMax; }
            set { SetValue(ref m_ProgBarMax, value, ProgBarMaxChangedArgs); }
        }

        public static readonly PropertyChangedEventArgs ProgBarValueChangedArgs = new PropertyChangedEventArgs("ProgBarValue");
        private int m_ProgBarValue;
        public int ProgBarValue
        {
            get { return m_ProgBarValue; }
            set { SetValue(ref m_ProgBarValue, value, ProgBarValueChangedArgs); }
        }

        public static readonly PropertyChangedEventArgs ProgBarOnChangedArgs = new PropertyChangedEventArgs("ProgBarOn");
        private string m_ProgBarOn;
        public string ProgBarOn
        {
            get { return m_ProgBarOn; }
            set { SetValue(ref m_ProgBarOn, value, ProgBarOnChangedArgs); }
        }

        public ICommand BuildDatabase { get; private set; }

        public ICommand AddNewFile { get; private set; }

        public ICommand SaveFileList { get; private set; }
        public ICommand OpenFileList { get; private set; }

        public ICommand RemoveItem { get; private set; }

        private static readonly string Table_TermsFts = "SearchIcd_TermsFts";
        private static readonly string Table_LinkedTitlesFts = "SearchIcd_LinkedTitlesFts";

        private class DatabaseConnections : IList<ICDDbContext>, IDisposable
        {
            private class DbContextAndSuch
            {
                public ICDDbContext Database { get; set; }
                public DbContextTransaction CurrentTransact { get; set; }
            }

            private List<DbContextAndSuch> m_Store;

            public bool IsDisposed { get; private set; }

            public DatabaseConnections()
            {
                m_Store = new List<DbContextAndSuch>();
            }

            ~DatabaseConnections()
            {
                Dispose(false);
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void DisposeItem(DbContextAndSuch item)
            {
                if (item != null)
                {
                    if (item.CurrentTransact != null)
                    {
                        item.CurrentTransact.Dispose();
                    }
                    if (item.Database != null)
                    {
                        item.Database.Dispose();
                    }
                }
            }

            protected virtual void Dispose(bool disposing)
            {
                if (IsDisposed)
                {
                    return;
                }

                if (disposing)
                {
                    foreach (var item in m_Store)
                    {
                        DisposeItem(item);
                    }
                }
                m_Store.Clear();

                IsDisposed = true;
            }

            public int IndexOf(ICDDbContext item)
            {
                throw new NotImplementedException();
            }

            public void Insert(int index, ICDDbContext item)
            {
                if (item != null)
                {
                    m_Store.Insert(index,
                        new DbContextAndSuch()
                        {
                            Database = item,
                            CurrentTransact = item.Database.BeginTransaction()
                        });
                }
            }

            public void RemoveAt(int index)
            {
                m_Store.RemoveAt(index);
            }

            public ICDDbContext this[int index]
            {
                get
                {
                    return m_Store[index].Database;
                }
                set
                {
                    DisposeItem(m_Store[index]);
                    var db = value;
                    if (db != null)
                    {
                        m_Store[index] = new DbContextAndSuch()
                            {
                                Database = db,
                                CurrentTransact = db.Database.BeginTransaction()
                            };
                    }
                }
            }

            public void Add(ICDDbContext item)
            {
                if (item != null)
                {
                    m_Store.Add(new DbContextAndSuch()
                    {
                        Database = item,
                        CurrentTransact = item.Database.BeginTransaction()
                    });
                }
            }

            public void AddRange(IEnumerable<ICDDbContext> collection)
            {
                m_Store.AddRange(from item in collection
                                 where item != null
                                 select new DbContextAndSuch()
                                    {
                                        Database = item,
                                        CurrentTransact = item.Database.BeginTransaction()
                                    });
            }

            public void Clear()
            {
                foreach (var item in m_Store)
                {
                    DisposeItem(item);
                }
                m_Store.Clear();
            }

            public bool Contains(ICDDbContext item)
            {
                return m_Store.Any(context => (context != null) ? Object.ReferenceEquals(context.Database, item) : false);
            }

            public void CopyTo(ICDDbContext[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public int Count
            {
                get { return m_Store.Count; }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            public bool Remove(ICDDbContext item)
            {
                var match = m_Store.FirstOrDefault(context => (context != null) ? Object.ReferenceEquals(context.Database, item) : false);
                if (match != null)
                {
                   return m_Store.Remove(match);
                }
                return false;
            }

            public void RollbackTransactions()
            {
                foreach (var item in m_Store)
                {
                    if (item != null)
                    {
                        if (item.CurrentTransact != null)
                        {
                            item.CurrentTransact.Rollback();
                            item.CurrentTransact.Dispose();
                        }
                        item.CurrentTransact = item.Database.Database.BeginTransaction();
                    }
                }
            }

            public void CommitTransactions()
            {
                foreach (var item in m_Store)
                {
                    if (item != null)
                    {
                        if (item.CurrentTransact != null)
                        {
                            item.CurrentTransact.Commit();
                            item.CurrentTransact.Dispose();
                        }
                        item.CurrentTransact = item.Database.Database.BeginTransaction();
                    }
                }
            }

            public IEnumerator<ICDDbContext> GetEnumerator()
            {
                return (from item in m_Store
                        select item.Database)
                        .GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        private void InsertNewFileAtEnd()
        {
            IcdFiles.Insert(IcdFiles.Count - 1, new IcdFileVm(new IcdFile()) { IsValidFile = true });
        }

        public IcdBuilderVM()
        {
            ProgBarVisible = false;

            IcdFiles = new ObservableCollection<IcdFileVm>();
            IcdFiles.Add(new IcdFileVm(new IcdFile()) { IsValidFile = false });

            BuildDatabase = new ActionCommand(Build);

            AddNewFile = new ActionCommand(InsertNewFileAtEnd);

            SaveFileList = new ActionCommand(SaveList);
            OpenFileList = new ActionCommand(OpenList);

            RemoveItem = new ActionCommand<IcdFileVm>(Remove);
        }

        public void Remove(IcdFileVm item)
        {
            IcdFiles.Remove(item);
        }

        public void SaveList()
        {
            var saveToFile = IcdFileHelper.ShowSaveDialog(true);
            if (!String.IsNullOrWhiteSpace(saveToFile))
            {
                var doc = new XDocument(
                                new XElement("SearchIcdFileList",
                                    from file in IcdFiles
                                    where file.IsValidFile
                                    select new XElement("IcdFile",
                                                new XAttribute("type", (int)file.File.Type),
                                                file.File.Filename)));
                doc.Save(saveToFile);
            }
        }

        public void OpenList()
        {
            var openFile = IcdFileHelper.ShowOpenDialog(true, false);
            if (!String.IsNullOrWhiteSpace(openFile))
            {
                if (System.IO.File.Exists(openFile))
                {
                    var doc = XDocument.Load(openFile);
                    var mainEl = doc.Element("SearchIcdFileList");
                    IcdFiles.Clear();
                    var newFiles = mainEl.Elements("IcdFile");
                    foreach (var newFile in newFiles)
                    {
                        IcdFiles.Add(new IcdFileVm(new IcdFile() { Filename = newFile.Value, Type = (IcdCmsFileType)Int32.Parse(newFile.Attribute("type").Value) }) { IsValidFile = true });
                    }
                    IcdFiles.Add(new IcdFileVm(new IcdFile()) { IsValidFile = false });
                }
            }
        }


        private string BuildSQLite_DropTable(string table)
        {
            return String.Format("DROP TABLE IF EXISTS {0};", table);
        }

        private string BuildSQLite_CreateTable(string table, Dictionary<string, string> columns)
        {
            var strings = from pair in columns
                          select String.Format("{0} {1}", pair.Key, pair.Value);
            return String.Format("CREATE TABLE {0} ({1});", table, String.Join(", ", strings));
        }

        private string BuildSQLite_CreateTableFts(string table, string contentTable, Dictionary<string, string> columns)
        {
            var strings = from pair in columns
                          select String.Format("{0} {1}", pair.Key, pair.Value);
            return String.Format("CREATE VIRTUAL TABLE {0} USING fts4(content='{1}', tokenize=porter, {2});",
                                    table, contentTable, String.Join(", ", strings));
        }

        private void SQLite_Setup(SQLiteConnection m_Connection)
        {
            var dropTable = BuildSQLite_DropTable(ConstData.Table_Terms);
            var cmnd = new SQLiteCommand(dropTable, m_Connection);
            cmnd.ExecuteNonQuery();
            dropTable = BuildSQLite_DropTable(Table_TermsFts);
            cmnd = new SQLiteCommand(dropTable, m_Connection);
            cmnd.ExecuteNonQuery();

            var createTable = BuildSQLite_CreateTable(ConstData.Table_Terms, new Dictionary<string, string>()
                                                                            {
                                                                                {ConstData.Table_Terms_Id, "INTEGER PRIMARY KEY"},
                                                                                {ConstData.Table_Terms_Type, "COLLATE NOCASE"},
                                                                                {ConstData.Table_Terms_Section, "COLLATE NOCASE"},
                                                                                {ConstData.Table_Terms_Code, "COLLATE NOCASE"},
                                                                                {ConstData.Table_Terms_Title, "COLLATE NOCASE"},
                                                                                {ConstData.Table_Terms_ParentCode, "COLLATE NOCASE"},
                                                                                {"CONSTRAINT", 
                                                                                    String.Format("unq UNIQUE ({0}, {1})", 
                                                                                        ConstData.Table_Terms_Type, ConstData.Table_Terms_Code)}
                                                                            });
            cmnd = new SQLiteCommand(createTable, m_Connection);
            cmnd.ExecuteNonQuery();
            createTable = BuildSQLite_CreateTableFts(Table_TermsFts, ConstData.Table_Terms, new Dictionary<string, string>()
                                                                                                    {
                                                                                                        {ConstData.Table_Terms_Title, String.Empty}
                                                                                                    });
            cmnd = new SQLiteCommand(createTable, m_Connection);
            cmnd.ExecuteNonQuery();

            dropTable = BuildSQLite_DropTable(ConstData.Table_AddCodes);
            cmnd = new SQLiteCommand(dropTable, m_Connection);
            cmnd.ExecuteNonQuery();

            createTable = BuildSQLite_CreateTable(ConstData.Table_AddCodes, new Dictionary<string, string>()
                                                                            {
                                                                                {ConstData.Table_AddCodes_Id, "INTEGER PRIMARY KEY"},
                                                                                {ConstData.Table_AddCodes_Type, "COLLATE NOCASE"},
                                                                                {ConstData.Table_AddCodes_AddType, "COLLATE NOCASE"},
                                                                                {ConstData.Table_AddCodes_ParentCode, "COLLATE NOCASE"},
                                                                                {ConstData.Table_AddCodes_Code, "COLLATE NOCASE"},
                                                                                {"CONSTRAINT", 
                                                                                    String.Format("unq UNIQUE ({0}, {1}, {2}, {3})", 
                                                                                        ConstData.Table_AddCodes_Type, ConstData.Table_AddCodes_AddType, ConstData.Table_AddCodes_ParentCode, ConstData.Table_AddCodes_Code)}
                                                                            });
            cmnd = new SQLiteCommand(createTable, m_Connection);
            cmnd.ExecuteNonQuery();

            dropTable = BuildSQLite_DropTable(ConstData.Table_LinkedTitles);
            cmnd = new SQLiteCommand(dropTable, m_Connection);
            cmnd.ExecuteNonQuery();
            dropTable = BuildSQLite_DropTable(Table_LinkedTitlesFts);
            cmnd = new SQLiteCommand(dropTable, m_Connection);
            cmnd.ExecuteNonQuery();

            createTable = BuildSQLite_CreateTable(ConstData.Table_LinkedTitles, new Dictionary<string, string>()
                                                                            {
                                                                                {ConstData.Table_LinkedTitles_Id, "INTEGER PRIMARY KEY"},
                                                                                {ConstData.Table_LinkedTitles_Type, "COLLATE NOCASE"},
                                                                                {ConstData.Table_LinkedTitles_Title, "COLLATE NOCASE"},
                                                                                {ConstData.Table_LinkedTitles_Code, "COLLATE NOCASE"},
                                                                                {"CONSTRAINT", 
                                                                                    String.Format("unq UNIQUE ({0}, {1}, {2})", 
                                                                                        ConstData.Table_LinkedTitles_Type, ConstData.Table_LinkedTitles_Title, ConstData.Table_LinkedTitles_Code)}
                                                                            });
            cmnd = new SQLiteCommand(createTable, m_Connection);
            cmnd.ExecuteNonQuery();
            createTable = BuildSQLite_CreateTableFts(Table_LinkedTitlesFts, ConstData.Table_LinkedTitles, new Dictionary<string, string>()
                                                                                                    {
                                                                                                        {ConstData.Table_LinkedTitles_Title, String.Empty}
                                                                                                    });
            cmnd = new SQLiteCommand(createTable, m_Connection);
            cmnd.ExecuteNonQuery();

            createTable = BuildSQLite_CreateTable(ConstData.Table_Lists, new Dictionary<string,string>()
                                                                            {
                                                                                {ConstData.Table_Lists_Id, "INTEGER PRIMARY KEY"},
                                                                                {ConstData.Table_Lists_Type, "COLLATE NOCASE"},
                                                                                {ConstData.Table_Lists_Data, "COLLATE NOCASE"},
                                                                                {ConstData.Table_Lists_ListOrder, "INTEGER"},
                                                                                {ConstData.Table_Lists_ListId, "COLLATE NOCASE"}
                                                                            });
            cmnd = new SQLiteCommand(createTable, m_Connection);
            cmnd.ExecuteNonQuery();

        }

        private SQLiteConnection GetSqliteFile(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                System.IO.File.Delete(filename);
            }

            SQLiteConnection conn = null;
            try
            {
                SQLiteConnection.CreateFile(filename);

                conn = new SQLiteConnection(String.Format("Data Source={0};Version=3;", filename));
                conn.Open();
                SQLite_Setup(conn);
            }
            catch
            {
                if (conn != null)
                {
                    conn.Dispose();
                }
                throw;
            }

            return conn;
        }

        private SqlConnection GetSqlServer(string connStr)
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
            }
            catch
            {
                if (conn != null)
                {
                    conn.Dispose();
                }
                throw;
            }

            return conn;
        }

        private class TermEqualityComparer : IEqualityComparer<IcdTerm>
        {
            private static TermEqualityComparer m_Static = new TermEqualityComparer();
            public static TermEqualityComparer Static { get { return m_Static; } }

            public bool Equals(IcdTerm x, IcdTerm y)
            {
                if (Object.ReferenceEquals(x, y))
                {
                    return true;
                }
                return String.Equals(x.Type, y.Type, StringComparison.CurrentCultureIgnoreCase) &&
                        String.Equals(x.Title, y.Title, StringComparison.CurrentCultureIgnoreCase) &&
                        String.Equals(x.Code, y.Code, StringComparison.CurrentCultureIgnoreCase);
            }

            public int GetHashCode(IcdTerm obj)
            {
                int hashcode = 0;

                hashcode = (hashcode * 397) ^ (obj.Type != null ? obj.Type.ToLower().GetHashCode() : 0);
                hashcode = (hashcode * 397) ^ (obj.Title != null ? obj.Title.ToLower().GetHashCode() : 0);
                hashcode = (hashcode * 397) ^ (obj.Code != null ? obj.Code.ToLower().GetHashCode() : 0);

                return hashcode;
            }
        }

        private class LinkedTitleEqualityComparer : IEqualityComparer<IcdLinkedTitle>
        {
            private static LinkedTitleEqualityComparer m_Static = new LinkedTitleEqualityComparer();
            public static LinkedTitleEqualityComparer Static { get { return m_Static; } }

            public bool Equals(IcdLinkedTitle x, IcdLinkedTitle y)
            {
                if (Object.ReferenceEquals(x, y))
                {
                    return true;
                }
                return String.Equals(x.Code, y.Code, StringComparison.CurrentCultureIgnoreCase) && 
                        String.Equals(x.Title, y.Title, StringComparison.CurrentCultureIgnoreCase) && 
                        String.Equals(x.Type, y.Type, StringComparison.CurrentCultureIgnoreCase);
            }

            public int GetHashCode(IcdLinkedTitle obj)
            {
                int hashcode = 0;

                hashcode = (hashcode * 397) ^ (obj.Title != null ? obj.Title.ToLower().GetHashCode() : 0);
                hashcode = (hashcode * 397) ^ (obj.Code != null ? obj.Code.ToLower().GetHashCode() : 0);
                hashcode = (hashcode * 397) ^ (obj.Type != null ? obj.Type.ToLower().GetHashCode() : 0);

                return hashcode;
            }
        }

        private class AddCodeEqualityComparer : IEqualityComparer<IcdAddCode>
        {
            private static AddCodeEqualityComparer m_Static = new AddCodeEqualityComparer();
            public static AddCodeEqualityComparer Static { get { return m_Static; } }

            public bool Equals(IcdAddCode x, IcdAddCode y)
            {
                if (Object.ReferenceEquals(x, y))
                {
                    return true;
                }
                return String.Equals(x.Code, y.Code, StringComparison.CurrentCultureIgnoreCase) &&
                        String.Equals(x.ParentCode, y.ParentCode, StringComparison.CurrentCultureIgnoreCase) &&
                        String.Equals(x.Type, y.Type, StringComparison.CurrentCultureIgnoreCase) &&
                        String.Equals(x.AddType, y.AddType, StringComparison.CurrentCultureIgnoreCase);
            }

            public int GetHashCode(IcdAddCode obj)
            {
                int hashcode = 0;

                hashcode = (hashcode * 397) ^ (obj.AddType != null ? obj.AddType.ToLower().GetHashCode() : 0);
                hashcode = (hashcode * 397) ^ (obj.Code != null ? obj.Code.ToLower().GetHashCode() : 0);
                hashcode = (hashcode * 397) ^ (obj.ParentCode != null ? obj.ParentCode.ToLower().GetHashCode() : 0);
                hashcode = (hashcode * 397) ^ (obj.Type != null ? obj.Type.ToLower().GetHashCode() : 0);

                return hashcode;
            }
        }

        private IEnumerable<IcdLinkedTitle> BuildGemsAndAlternatives(bool diagnosis)
        {
            var gemsFiles = from file in IcdFiles
                            where file.File.Type == (diagnosis ? IcdCmsFileType.CmsGemsTxt : IcdCmsFileType.PcsGemsTxt)
                            select file.File.Filename;
            var gemsDict = new Dictionary<string, List<string>>();
            foreach (var file in gemsFiles)
            {
                var newGems = GemsBuilder.GetGemsDictionary(file, diagnosis);

                foreach (var gem in newGems)
                {
                    List<string> workGems = null;
                    if (gemsDict.TryGetValue(gem.Key, out workGems))
                    {
                        var unique = workGems.Union(gem.Value, StringComparer.CurrentCultureIgnoreCase);
                        workGems.Clear();
                        workGems.AddRange(unique);
                    }
                    else
                    {
                        gemsDict.Add(gem.Key, gem.Value);
                    }
                }
            }


            var alternGems = from file in IcdFiles
                             where file.File.Type == (diagnosis ? IcdCmsFileType.CmsAlternateGemTxt : IcdCmsFileType.PcsAlternateGemTxt)
                             select file.File.Filename;
            foreach (var alternGem in alternGems)
            {
                var rets = GemsBuilder.GetLinkedTitles(alternGem, gemsDict, diagnosis ? IcdCodeStrings.CodeType_Diagnosis : IcdCodeStrings.CodeType_Procedure);
                foreach (var ret in rets)
                {
                    yield return ret;
                }
            }


            var alterns = from file in IcdFiles
                          where file.File.Type == (diagnosis ? IcdCmsFileType.CmsAlternateTxt : IcdCmsFileType.PcsAlternateTxt)
                          select file.File.Filename;
            foreach (var altern in alterns)
            {
                var rets = GemsBuilder.GetLinkedTitlesNoGem(altern, diagnosis ? IcdCodeStrings.CodeType_Diagnosis : IcdCodeStrings.CodeType_Procedure);
                foreach (var ret in rets)
                {
                    yield return ret;
                }
            }

            var indices = from file in IcdFiles
                          where file.File.Type == (diagnosis ? IcdCmsFileType.CmsIndexXml : IcdCmsFileType.PcsIndexXml)
                          select file.File.Filename;
            foreach (var index in indices)
            {
                var rets = IndexBuilder.GetFromFile(index, diagnosis ? IcdCodeStrings.CodeType_Diagnosis : IcdCodeStrings.CodeType_Procedure);
                foreach (var ret in rets)
                {
                    yield return ret;
                }
            }
        }

        private IEnumerable<IcdTerm> CopyTerms(IEnumerable<IcdTerm> inTerms)
        {
            foreach (var term in inTerms)
            {
                yield return new IcdTerm()
                {
                    Id = null,
                    Code = term.Code,
                    ParentCode = term.ParentCode,
                    Section = term.Section,
                    Title = term.Title,
                    Type = term.Type
                };
            }
        }

        private IEnumerable<IcdLinkedTitle> CopyLinkedTitles(IEnumerable<IcdLinkedTitle> inTitles)
        {
            foreach (var term in inTitles)
            {
                yield return new IcdLinkedTitle()
                {
                    Id = null,
                    Code = term.Code,
                    Title = term.Title,
                    Type = term.Type
                };
            }
        }

        private IEnumerable<IcdAddCode> CopyAddCodes(IEnumerable<IcdAddCode> inAddCodes)
        {
            foreach (var term in inAddCodes)
            {
                yield return new IcdAddCode()
                {
                    Id = null,
                    Code = term.Code,
                    Type = term.Type,
                    AddType = term.AddType,
                    ParentCode = term.ParentCode
                };
            }
        }

        private IEnumerable<IcdListItem> CopyListItems(IEnumerable<IcdListItem> inAddCodes)
        {
            foreach (var term in inAddCodes)
            {
                yield return new IcdListItem()
                {
                    Id = null,
                    Type = term.Type,
                    Data = term.Data,
                    ListId = term.ListId,
                    ListOrder = term.ListOrder
                };
            }
        }

        private IcdLinkedTitle CleanSingleLinkedTitle(IcdLinkedTitle title)
        {
            var wordSet = new HashSet<string>(StringComparer.CurrentCultureIgnoreCase);

            var words = title.Title.Split(null);
            foreach (var word in words.OrderBy(s => s, StringComparer.CurrentCultureIgnoreCase))
            {
                if (!String.IsNullOrWhiteSpace(word))
                {
                    var useWord = new string(word.Where(c => !char.IsPunctuation(c)).ToArray());
                    if (!wordSet.Contains(useWord))
                    {
                        wordSet.Add(useWord);
                    }
                }
            }

            return new IcdLinkedTitle()
            {
                Code = title.Code,
                Id = title.Id,
                Type = title.Type,
                Title = String.Join(" ", wordSet)
            };
        }

        private List<IcdLinkedTitle> CleanLinkedTitles(IEnumerable<IcdLinkedTitle> titles)
        {
            var linkedDict = new Dictionary<string, IcdLinkedTitle>(StringComparer.CurrentCultureIgnoreCase);
            foreach (var title in titles)
            {
                var useKey = title.Code;
                if (linkedDict.ContainsKey(useKey))
                {
                    linkedDict[useKey].Title = String.Format("{0} {1}", linkedDict[useKey].Title, title.Title);
                }
                else
                {
                    linkedDict[useKey] = title;
                }
            }
            return linkedDict.Select(p => CleanSingleLinkedTitle(p.Value)).ToList();
        }


        private List<List<T>> SplitChunks<T>(IEnumerable<T> inData, int size)
        {
            var ret = new List<List<T>>();
            using (var iter = inData.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    var newList = new List<T>();
                    newList.Add(iter.Current);
                    for (int iterOn = 1; (iterOn < size) && iter.MoveNext(); ++iterOn)
                    {
                        newList.Add(iter.Current);
                    }
                    ret.Add(newList);
                }
            }
            return ret;
        }

        private void AddFromCmsTabular(DatabaseConnections dbConns)
        {
            var AddCodes = new HashSet<IcdAddCode>(AddCodeEqualityComparer.Static);
            try
            {
                var tabXmlFiles = (from file in IcdFiles
                                   where file.File.Type == IcdCmsFileType.CmsTabularXml
                                   select file.File.Filename).ToList();

                ProgBarMax += ((2 * dbConns.Count) - 1) + (tabXmlFiles.Count * dbConns.Count * 3);

                int currOn = 0;
                var LinkedTitles = new HashSet<IcdLinkedTitle>(LinkedTitleEqualityComparer.Static);
                foreach (var tabXmlFile in tabXmlFiles)
                {
                    var data = CmsBuilder.BuildFromTabularFile(tabXmlFile);
                    ProgBarOn = String.Format("Adding Terms for Diagnosis Tabular File {0}/{1}", ++currOn, tabXmlFiles.Count);

                    var terms = (from code in data.TabularCodes
                                 select new IcdTerm()
                                 {
                                     Id = null,
                                     Code = code.Code,
                                     Section = code.SectionRange,
                                     Title = code.Title,
                                     Type = IcdCodeStrings.CodeType_Diagnosis,
                                     ParentCode = code.ParentCode
                                 }).ToList();
                    foreach (var db in dbConns)
                    {
                        db.IcdTerms.AddRange(terms);
                        db.SaveChanges();
                        ++ProgBarValue;
                    }
                    terms = null;

                    ProgBarOn = String.Format("Preparing Linked Titles from Diagnosis Tabular File {0}/{1}", currOn, tabXmlFiles.Count);

                    var newLinked = from code in data.TabularCodes
                                    from linked in code.InclusionTerms
                                    select new IcdLinkedTitle()
                                    {
                                        Id = null,
                                        Code = code.Code.Trim(),
                                        Title = linked.Trim(),
                                        Type = IcdCodeStrings.CodeType_Diagnosis.Trim()
                                    };
                    foreach (var linked in newLinked)
                    {
                        LinkedTitles.Add(linked);
                    }
                    ++ProgBarValue;

                    ProgBarOn = String.Format("Preparing Additional Codes from Diagnosis Tabular File {0}/{1}", currOn, tabXmlFiles.Count);

                    var newAddCodes = (from code in data.TabularCodes
                                       let CodeTable = new
                                       {
                                           CodeFirst = from codes in code.AddCodes.CodeFirst
                                                       select new IcdAddCode()
                                                       {
                                                           Id = null,
                                                           Type = IcdCodeStrings.CodeType_Diagnosis,
                                                           AddType = IcdCodeStrings.ChildType_CodeFirst,
                                                           ParentCode = code.Code,
                                                           Code = codes
                                                       },
                                           CodeAlso = from codes in code.AddCodes.CodeAlso
                                                      select new IcdAddCode()
                                                      {
                                                          Id = null,
                                                          Type = IcdCodeStrings.CodeType_Diagnosis,
                                                          AddType = IcdCodeStrings.ChildType_CodeAlso,
                                                          ParentCode = code.Code,
                                                          Code = codes
                                                      },
                                           CodeAdditional = from codes in code.AddCodes.AdditionalCodes
                                                            select new IcdAddCode()
                                                            {
                                                                Id = null,
                                                                Type = IcdCodeStrings.CodeType_Diagnosis,
                                                                AddType = IcdCodeStrings.ChildType_CodeAdditional,
                                                                ParentCode = code.Code,
                                                                Code = codes
                                                            },
                                           Excludes1 = from codes in code.AddCodes.Excludes1
                                                       select new IcdAddCode()
                                                       {
                                                           Id = null,
                                                           Type = IcdCodeStrings.CodeType_Diagnosis,
                                                           AddType = IcdCodeStrings.ChildType_Excludes1,
                                                           ParentCode = code.Code,
                                                           Code = codes
                                                       },
                                           Excludes2 = from codes in code.AddCodes.Excludes2
                                                       select new IcdAddCode()
                                                       {
                                                           Id = null,
                                                           Type = IcdCodeStrings.CodeType_Diagnosis,
                                                           AddType = IcdCodeStrings.ChildType_Excludes2,
                                                           ParentCode = code.Code,
                                                           Code = codes
                                                       }
                                       }
                                       from codes in CodeTable.CodeFirst.Concat(CodeTable.CodeAlso).Concat(CodeTable.CodeAdditional)//.Concat(CodeTable.Excludes1).Concat(CodeTable.Excludes2)
                                       select codes);
                    foreach (var addCode in newAddCodes)
                    {
                        AddCodes.Add(addCode);
                    }
                    ++ProgBarValue;
                }

                ProgBarOn = "Adding All Diagnosis Linked Titles...";
                foreach (var newLinked in BuildGemsAndAlternatives(true))
                {
                    LinkedTitles.Add(newLinked);
                }

                var trueLinkedTitles = CleanLinkedTitles(LinkedTitles);
                foreach (var db in dbConns)
                {
                    db.IcdLinkedTitles.AddRange(trueLinkedTitles);
                    db.SaveChanges();
                    ++ProgBarValue;
                }
                LinkedTitles = null;
                trueLinkedTitles = null;

                ProgBarOn = "Adding All Diagnosis Additional Codes...";

                /*System.Diagnostics.Debug.Print("Total Number: {0}", AddCodes.Count);
                int onNum = 0;
                int onDb = 0;
                var insertCmd = String.Format(
@"INSERT INTO {0}
    ({1}, {2}, {3}, {4})
    VALUES (@NewType, @NewAddType, @NewParentCode, @NewCode);",
                                            ConstData.Table_AddCodes,
                                            ConstData.Table_AddCodes_Type, ConstData.Table_AddCodes_AddType, ConstData.Table_AddCodes_ParentCode, ConstData.Table_AddCodes_Code); 
                foreach (var db in dbConns)
                {
                    ++onDb;
                    foreach (var addCode in AddCodes)
                    {
                        ++onNum;
                        if (onNum % 100000 == 0)
                        {
                            System.Diagnostics.Debug.Print("Db {0}, {1}/{2}", onDb, onNum, AddCodes.Count);
                        }
                        var query = db.Database.ExecuteSqlCommand(
                                        insertCmd,
                                        new SQLiteParameter("@NewType", addCode.Type),
                                        new SQLiteParameter("@NewAddType", addCode.AddType),
                                        new SQLiteParameter("@NewParentCode", addCode.ParentCode),
                                        new SQLiteParameter("@NewCode", addCode.Code));
                    }
                    ++ProgBarValue;
                }
                AddCodes = null;*/
                var addCodeChunks = SplitChunks(AddCodes, 500000);
                AddCodes = null;
                int numOn = 0;
                foreach (var addCodeChunk in addCodeChunks)
                {
                    System.Diagnostics.Debug.Print("Currently On: {0}/{1}", ++numOn, addCodeChunks.Count);
                    foreach (var db in dbConns)
                    {
                        db.IcdAddCodes.AddRange(addCodeChunk);
                        db.SaveChanges();
                    }
                    addCodeChunk.Clear();
                }
                ProgBarValue += dbConns.Count;
                addCodeChunks = null;
            }
            catch (System.Exception e)
            {
                ExpandException(e);
                throw;
            }
        }

        private void ExpandException(System.Exception e)
        {
            System.Diagnostics.Debug.Print("Exception: {0}\r\n{1}", e.Message, e.StackTrace);

            if (e.InnerException != null)
            {
                ExpandException(e.InnerException);
            }
        }

        private void AddFromPcsTabularAndOrder(DatabaseConnections dbConns)
        {
            var orderFiles = (from file in IcdFiles
                              where file.File.Type == IcdCmsFileType.PcsOrderTxt
                              select file.File.Filename).ToList();
            var orderDict = new Dictionary<string, PcsOrderTxtLineData>();

            ProgBarMax += (orderFiles.Count - 1) + (dbConns.Count * 2);

            ProgBarOn = "Processing all Procedure Order Files...";
            
            foreach (var orderFile in orderFiles)
            {
                var newDict = PcsBuilder.GetOrderLineData(orderFile);
                foreach (var line in newDict)
                {
                    orderDict[line.Key] = line.Value;
                }
                ++ProgBarValue;
            }

            ProgBarOn = "Adding All Procedure Terms...";
            
            var terms = (from file in IcdFiles
                         where file.File.Type == IcdCmsFileType.PcsTabularXml
                         from term in PcsBuilder.BuildWithOrderData(file.File.Filename, orderDict)
                         select term)
                         .Distinct(TermEqualityComparer.Static)
                         .ToList();
            foreach (var db in dbConns)
            {
                db.IcdTerms.AddRange(CopyTerms(terms).ToList());
                db.SaveChanges();
                ++ProgBarValue;
            }

            ProgBarOn = "Add All Procedure Linked Titles...";

            var linkedTitles = CleanLinkedTitles(BuildGemsAndAlternatives(false)
                                    .Distinct(LinkedTitleEqualityComparer.Static));
            foreach (var db in dbConns)
            {
                db.IcdLinkedTitles.AddRange(CopyLinkedTitles(linkedTitles).ToList());
                db.SaveChanges();
                ++ProgBarValue;
            }
        }

        private void AddLists(DatabaseConnections dbConns)
        {
            var listItems = (from file in IcdFiles
                             where file.File.Type == IcdCmsFileType.XmlSpecialList
                             from item in XmlListBuilder.GetFromFile(file.File.Filename)
                             select item).ToList();
            ProgBarMax += dbConns.Count - 1;
            ProgBarOn = "Adding All Special Lists...";
            
            foreach (var db in dbConns)
            {
                db.IcdListItems.AddRange(CopyListItems(listItems).ToList());
                db.SaveChanges();
                ++ProgBarValue;
            }
        }

        private void BuildSQLite_Fts(SQLiteConnection dbconn, string ftsTable, string fromTable, string idCol, params string[] otherCols)
        {
            if (otherCols.Length < 1) return;

            var cmdStr = String.Format("INSERT INTO {0} (docid, {3}) SELECT {2}, {3} FROM {1};",
                                        ftsTable, fromTable, idCol, String.Join(", ", otherCols));
            var insertCmd = new SQLiteCommand(cmdStr, dbconn);
            insertCmd.ExecuteNonQuery();
        }

        private void CreateSqliteIndices(SQLiteConnection conn)
        {
            var cmnd = new SQLiteCommand(
                    String.Format("CREATE INDEX TermsCodes ON {0} ({1} COLLATE NOCASE);",
                        ConstData.Table_Terms, ConstData.Table_Terms_Code),
                    conn);
            cmnd.ExecuteNonQuery();
            cmnd = new SQLiteCommand(
                    String.Format("CREATE INDEX TermsTitles ON {0} ({1} COLLATE NOCASE);",
                        ConstData.Table_Terms, ConstData.Table_Terms_Title),
                    conn);
            cmnd.ExecuteNonQuery();
            cmnd = new SQLiteCommand(
                    String.Format("CREATE INDEX TermsTypes ON {0} ({1} COLLATE NOCASE);",
                        ConstData.Table_Terms, ConstData.Table_Terms_Type),
                    conn);
            cmnd.ExecuteNonQuery();
            cmnd = new SQLiteCommand(
                    String.Format("CREATE INDEX AddCodesAddTypes ON {0} ({1} COLLATE NOCASE);",
                        ConstData.Table_AddCodes, ConstData.Table_AddCodes_AddType),
                    conn);
            cmnd.ExecuteNonQuery();
            cmnd = new SQLiteCommand(
                    String.Format("CREATE INDEX AddCodesTypes ON {0} ({1} COLLATE NOCASE);",
                        ConstData.Table_AddCodes, ConstData.Table_AddCodes_Type),
                    conn);
            cmnd.ExecuteNonQuery();
            cmnd = new SQLiteCommand(
                    String.Format("CREATE INDEX AddCodesParentCodes ON {0} ({1} COLLATE NOCASE);",
                        ConstData.Table_AddCodes, ConstData.Table_AddCodes_ParentCode),
                    conn);
            cmnd.ExecuteNonQuery();
            cmnd = new SQLiteCommand(
                    String.Format("CREATE INDEX AddCodesCodes ON {0} ({1} COLLATE NOCASE);",
                        ConstData.Table_AddCodes, ConstData.Table_AddCodes_Code),
                    conn);
            cmnd.ExecuteNonQuery();
            cmnd = new SQLiteCommand(
                    String.Format("CREATE INDEX LinkedTitlesCodes ON {0} ({1} COLLATE NOCASE);",
                        ConstData.Table_LinkedTitles, ConstData.Table_LinkedTitles_Code),
                    conn);
            cmnd.ExecuteNonQuery();
            cmnd = new SQLiteCommand(
                    String.Format("CREATE INDEX LinkedTitlesTitles ON {0} ({1} COLLATE NOCASE);",
                        ConstData.Table_LinkedTitles, ConstData.Table_LinkedTitles_Title),
                    conn);
            cmnd.ExecuteNonQuery();
            cmnd = new SQLiteCommand(
                    String.Format("CREATE INDEX LinkedTitlesTypes ON {0} ({1} COLLATE NOCASE);",
                        ConstData.Table_LinkedTitles, ConstData.Table_LinkedTitles_Type),
                    conn);
            cmnd.ExecuteNonQuery();

            BuildSQLite_Fts(conn, Table_TermsFts, ConstData.Table_Terms, ConstData.Table_Terms_Id, ConstData.Table_Terms_Title);

            BuildSQLite_Fts(conn, Table_LinkedTitlesFts, ConstData.Table_LinkedTitles, ConstData.Table_LinkedTitles_Id, ConstData.Table_LinkedTitles_Title);
        }

        public void TrueBuild()
        {
            try
            {
                ProgBarVisible = true;
                ProgBarMax = 6;
                ProgBarValue = 0;
                ProgBarOn = "Building Database...";

                using (var dbConns = new DatabaseConnections())
                {
                    ProgBarOn = "Opening and preparing Databases...";
                    var sqliteFiles = (from file in IcdFiles
                                       where file.File.Type == IcdCmsFileType.SqliteFileSave
                                       select GetSqliteFile(file.File.Filename))
                                       .ToList();
                    dbConns.AddRange(sqliteFiles.Select(f => new ICDDbContext(f, true)));

                    //SQL Server is disabledf
                    /*dbConns.AddRange(from file in IcdFiles
                                     where file.File.Type == IcdCmsFileType.SqlServerSave
                                     select new ICDDbContext(GetSqlServer(file.File.Filename), true));*/

                    ++ProgBarValue;

                    ProgBarOn = "Starting Diagnosis Files...";
                    AddFromCmsTabular(dbConns);

                    ProgBarOn = "Starting Procedure Files...";
                    AddFromPcsTabularAndOrder(dbConns);

                    ProgBarOn = "Building Database Indices...";
                    foreach (var sqlite in sqliteFiles)
                    {
                        CreateSqliteIndices(sqlite);
                    }
                    ++ProgBarValue;

                    ProgBarOn = "Starting Special Lists...";
                    AddLists(dbConns);

                    ProgBarOn = "Committing All Updates...";
                    dbConns.CommitTransactions();
                    ++ProgBarValue;
                }

                ProgBarVisible = false;
            }
            catch (System.Exception e)
            {
                ExpandException(e);
                throw;
            }
        }

        public void Build()
        {
            Task.Factory.StartNew(TrueBuild);
        }
    }
}
