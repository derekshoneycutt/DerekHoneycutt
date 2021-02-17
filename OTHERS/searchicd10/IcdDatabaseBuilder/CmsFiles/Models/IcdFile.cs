using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcdDatabaseBuilder.CmsFiles.Models
{
    public class IcdFile : NotifyingModel
    {
        public static readonly PropertyChangedEventArgs FilenameChangedArgs = new PropertyChangedEventArgs("Filename");
        public static readonly PropertyChangedEventArgs TypeChangedArgs = new PropertyChangedEventArgs("Type");

        private string m_Filename;
        public string Filename
        {
            get { return m_Filename; }
            set { SetValue(ref m_Filename, value, FilenameChangedArgs); }
        }

        private IcdCmsFileType m_Type;
        public IcdCmsFileType Type
        {
            get { return m_Type; }
            set { SetValue(ref m_Type, value, TypeChangedArgs); }
        }

        public IcdFile()
        {
            m_Filename = String.Empty;
            m_Type = IcdCmsFileType.None;
        }
    }
}
