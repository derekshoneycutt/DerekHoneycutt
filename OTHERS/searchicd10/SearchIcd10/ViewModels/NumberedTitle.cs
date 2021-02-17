using SearchIcd10.Utils;
using System;
using System.ComponentModel;

namespace SearchIcd10.ViewModels
{
    /// <summary>
    /// Class defining a Title that is numbered by some index
    /// </summary>
    public class NumberedTitle : NotifyingModel
    {
        public static readonly PropertyChangedEventArgs NumberChangedArgs = new PropertyChangedEventArgs("Number");
        public static readonly PropertyChangedEventArgs TitleChangedArgs = new PropertyChangedEventArgs("Title");
        public static readonly PropertyChangedEventArgs FullTitleChangedArgs = new PropertyChangedEventArgs("FullTitle");

        private int m_Number;
        /// <summary>
        /// Gets or Sets the number associated to the title
        /// </summary>
        public int Number
        {
            get
            {
                return m_Number;
            }
            set
            {
                SetValue(ref m_Number, value, NumberChangedArgs, FullTitleChangedArgs);
            }
        }

        private string m_Title;
        /// <summary>
        /// Gets or Sets the simple text title
        /// </summary>
        public string Title
        {
            get
            {
                return m_Title;
            }
            set
            {
                SetValue(ref m_Title, value, TitleChangedArgs, FullTitleChangedArgs);
            }
        }

        /// <summary>
        /// Gets the actual full title of the object
        /// </summary>
        public string FullTitle
        {
            get
            {
                return String.Format("{0}. {1}", Number, Title);
            }
        }

        public NumberedTitle(int num, string title)
        {
            Number = num;
            Title = title;
        }

        public override string ToString()
        {
            return FullTitle;
        }
    }
}
