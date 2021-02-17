using SearchIcd10.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace SearchIcd10
{
    /// <summary>
    /// Interaction logic for WFTextBox.xaml
    /// </summary>
    public partial class WFTextBox : UserControl
    {
        private class NativeMethods
        {
            [DllImport("user32.dll")]
            public static extern IntPtr SetFocus(IntPtr hWnd);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(WFTextBox),
                new FrameworkPropertyMetadata(String.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnTextPropChanged)));

        /// <summary>
        /// Gets or Sets the Text within the TextBox
        /// </summary>
        public string Text
        {
            get
            {
                var ret = (string)GetValue(TextProperty);
                if (String.IsNullOrEmpty(ret))
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
        private static void OnTextPropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var real = (WFTextBox)d;
            real.winFormText.Text = (string)e.NewValue;
        }

        public event EventHandler TextChanged;

        public static readonly DependencyProperty AcceptsReturnProperty =
            DependencyProperty.Register("AcceptsReturn", typeof(bool), typeof(WFTextBox),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(AcceptsReturnChanged)));

        /// <summary>
        /// Gets or Sets whether the TextBox accepts the Return key entry
        /// </summary>
        public bool AcceptsReturn
        {
            get
            {
                return (bool)GetValue(AcceptsReturnProperty);
            }
            set
            {
                SetValue(AcceptsReturnProperty, value);
            }
        }

        private static void AcceptsReturnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var real = (WFTextBox)d;
            real.winFormText.AcceptsReturn = (bool)e.NewValue;
        }

        public WFTextBox()
        {
            InitializeComponent();

            winFormText.TextChanged += winFormText_TextChanged;
            this.Loaded += WFTextBox_Loaded;
        }

        void WFTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            if (win != null)
            {
                win.Closed += win_Closed;
            }
            this.Loaded -= WFTextBox_Loaded;
        }

        void win_Closed(object sender, EventArgs e)
        {
            winFormText.TextChanged -= winFormText_TextChanged;
            winFormHost.Dispose();
        }

        void winFormText_TextChanged(object sender, EventArgs e)
        {
            Text = winFormText.Text;
            var text = winFormText.Text;
            if (String.IsNullOrEmpty(text))
            {
                text = " ";
            }
            var s = System.Windows.Forms.TextRenderer.MeasureText(text, winFormText.Font, winFormText.ClientRectangle.Size, System.Windows.Forms.TextFormatFlags.WordBreak);
            winFormText.Height = s.Height + 8;
            if (TextChanged != null)
            {
                TextChanged(this, e);
            }
        }

        /// <summary>
        /// Focuses on the TextBox
        /// <para>Overrides the usual with the forced TakeFocus</para>
        /// </summary>
        public new void Focus()
        {
            TakeFocus();
        }

        /// <summary>
        /// Force the focus to go to the TextBox
        /// </summary>
        public void TakeFocus()
        {
            base.Focus();
            winFormHost.Focus();

            //The below seems to work better to actually get focus. A simple Focus() call does wonky things with the focus
            //It's a hack of pure magical proportions
            NativeMethods.SetFocus(winFormText.Handle);
            Window parentWin = Window.GetWindow(this);
            parentWin.Focus();
            NativeMethods.SetFocus(winFormText.Handle);

            //Also going to make the selection go to the end
            winFormText.SelectionStart = winFormText.Text.Length;
        }
        
        /// <summary>
        /// Select a specific range of text within the textbox
        /// </summary>
        /// <param name="start">The start location to start highlighting from</param>
        /// <param name="length">The length of text to highlight</param>
        public void Select(int start, int length)
        {
            winFormText.Select(start, length);
        }
    }
}
