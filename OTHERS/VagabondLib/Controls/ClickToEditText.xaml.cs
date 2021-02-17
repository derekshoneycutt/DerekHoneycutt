using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VagabondLib.Controls
{
    /// <summary>
    /// Interaction logic for ClickToEditText.xaml
    /// </summary>
    public partial class ClickToEditText : UserControl
    {
        public static DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ClickToEditText),
                new UIPropertyMetadata(String.Empty));

        /// <summary>
        /// Gets or Sets the Text associated to the control
        /// </summary>
        public string Text
        {
            get
            {
                string ret = (string)GetValue(TextProperty);
                if (ret == null)
                {
                    return String.Empty;
                }
                return ret;
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public static DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(ClickToEditText),
                new UIPropertyMetadata(TextWrapping.NoWrap));

        /// <summary>
        /// Gets or Sets the TextWrapping for the control
        /// </summary>
        public TextWrapping TextWrapping
        {
            get
            {
                return (TextWrapping)GetValue(TextWrappingProperty);
            }
            set
            {
                SetValue(TextWrappingProperty, value);
            }
        }

        public static DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(ClickToEditText),
                new UIPropertyMetadata(TextAlignment.Left));

        /// <summary>
        /// Gets or Sets the Text Alignment for the control
        /// </summary>
        public TextAlignment TextAlignment
        {
            get
            {
                return (TextAlignment)GetValue(TextAlignmentProperty);
            }
            set
            {
                SetValue(TextAlignmentProperty, value);
            }
        }

        public ClickToEditText()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Causes the text to go into edit mode, and attempts to take Focus
        /// </summary>
        public void EditText()
        {
            textBox.Visibility = System.Windows.Visibility.Visible;
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                System.Threading.Thread.Sleep(100);
                this.Dispatcher.Invoke(() =>
                {
                    textBox.Focus();
                    textBox.SelectAll();
                });
            });
        }

        private void textBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //If double click, call EditText to start text editing
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (e.ClickCount == 2)
                {
                    EditText();
                }
            }
        }

        private void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            //If TextBox has lost focus, hide it
            textBox.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {
            //If the Enter key is pressed on the textbox, hide the textbox to finish the text
            if (e.Key == Key.Enter)
            {
                textBox.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
