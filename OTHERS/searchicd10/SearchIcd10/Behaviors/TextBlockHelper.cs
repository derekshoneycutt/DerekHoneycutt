using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SearchIcd10.Behaviors
{
    /// <summary>
    /// Help class to handle special bindings of TextBlock objects in WPF
    /// <para>Binds FormattedText to take an IEnumerable of Inline objects that define the TextBlock's Text, formatted specially</para>
    /// </summary>
    public static class TextBlockHelper
    {
        public static readonly DependencyProperty FormattedTextProperty =
            DependencyProperty.RegisterAttached("FormattedText",
            typeof(IEnumerable<Inline>),
            typeof(TextBlockHelper),
            new UIPropertyMetadata(null, FormattedTextChanged));

        public static string GetFormattedText(DependencyObject obj)
        {
            return (string)obj.GetValue(FormattedTextProperty);
        }

        public static void SetFormattedText(DependencyObject obj, string value)
        {
            obj.SetValue(FormattedTextProperty, value);
        }

        private static void FormattedTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var value = e.NewValue as IEnumerable<Inline>;
            var textBlock = sender as TextBlock;

            if (textBlock != null)
            {
                textBlock.Inlines.Clear();
                textBlock.Inlines.AddRange(value);
            }
        }
    }
}
