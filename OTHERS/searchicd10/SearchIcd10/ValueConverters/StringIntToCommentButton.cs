using System;
using System.Windows.Data;

namespace SearchIcd10.ValueConverters
{
    /// <summary>
    /// Converts a nullable Int32 and String to the text to display on a ICD Item's Comment button
    /// <para>Values should always be the Nullable Int32 representing the item's displayed number, followed by the Item's Current Comment Text</para>
    /// <para>If Number is null (or translates to null), ArgumentException will be thrown</para>
    /// <para>If Comment text is NullOrWhiteSpace, returns "Comment {Num}"; otherwise "Remove Comment {Num}"</para>
    /// </summary>
    public class StringIntToCommentButton : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var num = values[0] as int?;
            var comment = values[1] as string;
            if ((num != null))
            {
                return String.Format("Comment {0}", num);
            }
            throw new ArgumentException("Values to StringIntToCommentButton must be an integer and string consecutively");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return new[] { Binding.DoNothing, Binding.DoNothing };
        }
    }

    public class IntToCommentButton : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var num = value as int?;
            if (num != null)
            {
                return String.Format("Comment {0}", num.Value);
            }
            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
