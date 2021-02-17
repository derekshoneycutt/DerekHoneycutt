using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using VagabondLib.Utils;

namespace VagabondLib.Behaviors
{
    /// <summary>
    /// Class that provides extra functionality to help with the RichTextBox WPF control
    /// </summary>
    public class RichTextBoxes : DependencyObject
    {
        /// <summary>
        /// Used to prevent recursive calls in the DocumentRtf elements
        /// </summary>
        private static HashSet<Thread> m_RecursProtectDocRtf = new HashSet<Thread>();

        /// <summary>
        /// Property for handling DocumentRtf on RichTextBox controls
        /// </summary>
        public static readonly DependencyProperty DocumentRtfProperty =
                DependencyProperty.RegisterAttached("DocumentRtf", typeof(string), typeof(RichTextBoxes),
                                                        new FrameworkPropertyMetadata(String.Empty,
                                                            FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                            RtfMetadataBinding));

        /// <summary>
        /// Method that is called when the DocumentRtf property is changed
        /// </summary>
        /// <param name="d">RichTextBox being acted upon</param>
        /// <param name="e">Data concerning the change of the property</param>
        private static void RtfMetadataBinding(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (m_RecursProtectDocRtf.Contains(Thread.CurrentThread))
            {
                return;
            }

            var trueBox = (RichTextBox)d;
            trueBox.TextChanged -= trueBox_TextChanged;

            var textRange = new TextRange(trueBox.Document.ContentStart, trueBox.Document.ContentEnd);
            var rtfStream = new MemoryStream(Encoding.UTF8.GetBytes(RtfHelper.FormatPlainToRtf((string)e.NewValue)));
            textRange.Load(rtfStream, DataFormats.Rtf);

            trueBox.TextChanged += trueBox_TextChanged;
        }

        /// <summary>
        /// Event that is fired for handling when a RichTextBox's text is changed
        /// </summary>
        /// <param name="sender">RichTextBox that is being changed</param>
        /// <param name="e">Appropriate, new information</param>
        private static void trueBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var trueBox = sender as RichTextBox;
            if (trueBox != null)
            {
                var textRange = new TextRange(trueBox.Document.ContentStart, trueBox.Document.ContentEnd);
                var rtfStream = new MemoryStream();
                textRange.Save(rtfStream, DataFormats.Rtf);
                string rtfText = Encoding.UTF8.GetString(rtfStream.ToArray());
                SetDocumentRtf(trueBox, rtfText);
            }
        }

        /// <summary>
        /// Get the DocumentRtf for a given RichTextBox
        /// </summary>
        /// <param name="obj">RichTextBox to get the DocumentRtf from</param>
        /// <returns>RTF formatted string from the RichTextBox</returns>
        public static string GetDocumentRtf(DependencyObject obj)
        {
            return (string)obj.GetValue(DocumentRtfProperty);
        }

        /// <summary>
        /// Sets the DocumentRtf for a given RichTextBox
        /// </summary>
        /// <param name="obj">RichTextBox to set the DocumentRtf value for</param>
        /// <param name="value">New RTF String value to set</param>
        public static void SetDocumentRtf(DependencyObject obj, string value)
        {
            m_RecursProtectDocRtf.Add(Thread.CurrentThread);
            obj.SetValue(DocumentRtfProperty, value);
            m_RecursProtectDocRtf.Remove(Thread.CurrentThread);
        }


        /// <summary>
        /// Used to prevent recursive calls in the DocumentSelectedRtf elements
        /// </summary>
        private static HashSet<Thread> m_RecursProtectSelDocRtf = new HashSet<Thread>();

        /// <summary>
        /// Property for handling DocumentSelectedRtf on RichTextBox controls
        /// </summary>
        public static readonly DependencyProperty DocumentSelectedRtfProperty =
                DependencyProperty.RegisterAttached("DocumentSelectedRtf", typeof(string), typeof(RichTextBoxes),
                                                        new FrameworkPropertyMetadata(String.Empty,
                                                            FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                            SelectedRtfMetadataBinding));

        /// <summary>
        /// Method that is called when the DocumentSelectedRtf property is changed
        /// </summary>
        /// <param name="d">RichTextBox being acted upon</param>
        /// <param name="e">Data concerning the change of the property</param>
        private static void SelectedRtfMetadataBinding(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (m_RecursProtectSelDocRtf.Contains(Thread.CurrentThread))
            {
                return;
            }

            var trueBox = (RichTextBox)d;
            trueBox.SelectionChanged -= trueBox_SelectionChanged;

            var textRange = new TextRange(trueBox.Selection.Start, trueBox.Selection.End);
            var rtfStream = new MemoryStream(Encoding.UTF8.GetBytes(RtfHelper.FormatPlainToRtf((string)e.NewValue)));
            textRange.Load(rtfStream, DataFormats.Rtf);

            trueBox.SelectionChanged += trueBox_SelectionChanged;
        }

        /// <summary>
        /// Event that is fired for handling when a RichTextBox's Selection is changed
        /// </summary>
        /// <param name="sender">RichTextBox that is being changed</param>
        /// <param name="e">Appropriate, new information</param>
        static void trueBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var trueBox = sender as RichTextBox;
            if (trueBox != null)
            {
                var textRange = new TextRange(trueBox.Selection.Start, trueBox.Selection.End);
                var rtfStream = new MemoryStream();
                textRange.Save(rtfStream, DataFormats.Rtf);
                string rtfText = Encoding.UTF8.GetString(rtfStream.ToArray());
                SetDocumentSelectedRtf(trueBox, rtfText);
            }
        }

        /// <summary>
        /// Get the DocumentSelectedRtf for a given RichTextBox
        /// </summary>
        /// <param name="obj">RichTextBox to get the DocumentRtf from</param>
        /// <returns>RTF formatted string from the RichTextBox</returns>
        public static string GetDocumentSelectedRtf(DependencyObject obj)
        {
            return (string)obj.GetValue(DocumentSelectedRtfProperty);
        }

        /// <summary>
        /// Sets the DocumentSelectedRtf for a given RichTextBox
        /// </summary>
        /// <param name="obj">RichTextBox to set the DocumentRtf value for</param>
        /// <param name="value">New RTF String value to set</param>
        public static void SetDocumentSelectedRtf(DependencyObject obj, string value)
        {
            m_RecursProtectSelDocRtf.Add(Thread.CurrentThread);
            obj.SetValue(DocumentSelectedRtfProperty, value);
            m_RecursProtectSelDocRtf.Remove(Thread.CurrentThread);
        }

        /// <summary>
        /// Property to add behavior for moving the cursor to the very end of a RichTextBox
        /// </summary>
        public static readonly DependencyProperty SelectToEndProperty =
                DependencyProperty.RegisterAttached("SelectToEnd", typeof(bool), typeof(RichTextBoxes),
                                                        new FrameworkPropertyMetadata(false,
                                                            FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                            SelectToEndMetadata));

        /// <summary>
        /// When the SelectToEnd property is changed, move the cursor to the end, always
        /// </summary>
        private static void SelectToEndMetadata(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rtfBox = (RichTextBox)d;
            rtfBox.Selection.Select(rtfBox.Document.ContentEnd, rtfBox.Document.ContentEnd);
        }

        /// <summary>
        /// Gets the current SelectToEnd value : Should be pretty pointless
        /// </summary>
        /// <param name="obj">RichTextBox object to set the value for</param>
        /// <returns>The currently set value</returns>
        public static bool GetSelectToEnd(DependencyObject obj)
        {
            return (bool)obj.GetValue(SelectToEndProperty);
        }

        /// <summary>
        /// Sets the SelectToEnd value : Always moves the selection to the end of a RichTextBox
        /// </summary>
        /// <param name="obj">RichTextBox to send the cursor to the end of</param>
        /// <param name="value">New value to set the property to : True/False doesn't matter</param>
        public static void SetSelectToEnd(DependencyObject obj, bool value)
        {
            obj.SetValue(SelectToEndProperty, value);
        }
    }
}
