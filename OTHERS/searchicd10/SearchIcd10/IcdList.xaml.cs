using SearchIcd10.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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

namespace SearchIcd10
{
    /// <summary>
    /// Event Arguments for when the Items on an ICD List are reset
    /// </summary>
    public sealed class IcdItemsResetEventArgs : EventArgs
    {
        /// <summary>
        /// Gets whether the list was entirely reset, or just partially updated
        /// </summary>
        public bool IsReset { get; private set; }

        public IcdItemsResetEventArgs(bool isreset = true)
        {
            IsReset = isreset;
        }
    }

    /// <summary>
    /// Event Arguments for passing a Scrolling Hint through an ICD List
    /// </summary>
    public sealed class IcdListScrollHintEventArgs : EventArgs
    {
        /// <summary>
        /// Gets a hinted value for scrolling on the ICD List
        /// <para>Represents a visual location of the item to scroll to</para>
        /// </summary>
        public double Hint { get; private set; }

        public IcdListScrollHintEventArgs(double hint)
        {
            Hint = hint;
        }
    }

    /// <summary>
    /// Interaction logic for IcdList.xaml
    /// </summary>
    public partial class IcdList : UserControl
    {
        public static readonly DependencyProperty DividerBackBrushProperty =
            DependencyProperty.Register("DividerBackBrush", typeof(Brush), typeof(IcdList));
        /// <summary>
        /// Gets or Sets a Brush to draw the background of Dividers on the list
        /// </summary>
        public Brush DividerBackBrush
        {
            get { return (Brush)GetValue(DividerBackBrushProperty); }
            set { SetValue(DividerBackBrushProperty, value); }
        }

        /// <summary>
        /// Event that is raised when items on the list are somehow reset
        /// </summary>
        public event EventHandler<IcdItemsResetEventArgs> ItemsReset;
        /// <summary>
        /// Event that is raised when a scrolling is expected on the list
        /// </summary>
        public event EventHandler<IcdListScrollHintEventArgs> ScrollHint;
        /// <summary>
        /// Event that is raised when focus is not necessarily lost, but is somehow changed
        /// <para>Could be lost, or many other potential things</para>
        /// </summary>
        public event EventHandler<RoutedEventArgs> FocusChanged;

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(IcdList),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(ItemsSourceChanged)));

        /// <summary>
        /// Gets or Sets the Items that the list should be built off of
        /// </summary>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //When the items list changes, we need to track this for handling CollectionChanged events and raising the ItemsReset event
            var real = d as IcdList;
            if (real != null)
            {
                var old = e.OldValue as INotifyCollectionChanged;
                if (old != null)
                {
                    old.CollectionChanged -= real.notifyingCollection_CollectionChanged;
                }
                if (e.NewValue != null)
                {
                    var notifyingCollection = e.NewValue as INotifyCollectionChanged;
                    if (notifyingCollection != null)
                    {
                        notifyingCollection.CollectionChanged += real.notifyingCollection_CollectionChanged;
                    }

                    if (real.ItemsReset != null)
                    {
                        real.ItemsReset(real, new IcdItemsResetEventArgs());
                    }
                }
            }
        }
        void notifyingCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (ItemsReset != null)
            {
                ItemsReset(this, new IcdItemsResetEventArgs((e.Action == NotifyCollectionChangedAction.Reset)));
            }
        }

        public static readonly DependencyProperty MoreCommandProperty =
            DependencyProperty.Register("MoreCommand", typeof(ICommand), typeof(IcdList));
        /// <summary>
        /// Gets or Sets a command to run for clicking on a "More..." button if present
        /// </summary>
        public ICommand MoreCommand
        {
            get { return (ICommand)GetValue(MoreCommandProperty); }
            set { SetValue(MoreCommandProperty, value); }
        }

        public IcdList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Raise the FocusChanged event
        /// </summary>
        /// <param name="e">Event arguments to pass to the event</param>
        private void RaiseFocusChanged(RoutedEventArgs e)
        {
            if (FocusChanged != null)
            {
                FocusChanged(this, e);
            }
        }

        /// <summary>
        /// Focus on a textbox, and hint scrolling to the Grid containing that TextBox
        /// </summary>
        /// <param name="sender">TextBox to focus on</param>
        private void FocusOnTextbox(object sender)
        {
            var textbox = sender as TextBox;
            if (textbox != null)
            {
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    System.Threading.Thread.Sleep(100);
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (textbox.IsVisible)
                        {
                            textbox.Focus();
                            textbox.CaretIndex = textbox.Text.Length;

                            var parentGrid = UIElementHelper.FindParentOf<Grid>(textbox);
                            if (parentGrid != null)
                            {
                                var position = parentGrid.TranslatePoint(new Point(0, 0), this);
                                if (ScrollHint != null)
                                {
                                    ScrollHint(this, new IcdListScrollHintEventArgs(position.Y));
                                }
                            }
                        }
                    }));
                });
            }
        }

        private void TextBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            FocusOnTextbox(sender);
        }

        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            FocusOnTextbox(sender);
        }

        private void IcdList_ScrollHint(object sender, IcdListScrollHintEventArgs e)
        {
            //When a child list sends a scroll hint, propogate this up, with an adjusted hint value
            var icdlist = sender as IcdList;
            if (icdlist != null)
            {
                var position = icdlist.TranslatePoint(new Point(0, 0), this);
                if (ScrollHint != null)
                {
                    ScrollHint(this, new IcdListScrollHintEventArgs(position.Y + e.Hint));
                }
            }
        }

        private void IcdList_ItemsReset(object sender, IcdItemsResetEventArgs e)
        {
            //Do nothing...
        }

        private void CommentButton_Click(object sender, RoutedEventArgs e)
        {
            //The comment button will go into or out of the Comment Box
            var button = sender as UIElement;
            if (button != null)
            {
                var textbox = UIElementHelper.FindNear<TextBox>(button);
                if (textbox != null)
                {
                    if (textbox.IsVisible)
                    {
                        if (textbox.IsFocused)
                        {
                            button.Focus();
                            RaiseFocusChanged(new RoutedEventArgs());
                        }
                        else
                        {
                            textbox.Focus();
                            textbox.CaretIndex = textbox.Text.Length;
                            RaiseFocusChanged(new RoutedEventArgs());
                        }
                    }
                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            RaiseFocusChanged(e);
        }

        private void IcdList_FocusChanged(object sender, RoutedEventArgs e)
        {
            RaiseFocusChanged(e);
        }
    }
}
