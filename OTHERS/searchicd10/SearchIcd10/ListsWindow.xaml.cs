using SearchIcd10.Behaviors;
using SearchIcd10.Utils;
using SearchIcd10.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SearchIcd10
{
    /// <summary>
    /// Interaction logic for ListsWindow.xaml
    /// </summary>
    public partial class ListsWindow : Window
    {
        public ScrollViewerFocusHandler focusHandler { get; set; }

        public ListsWindow()
        {
            focusHandler = new ScrollViewerFocusHandler();
            InitializeComponent();
            focusHandler.ScrollView = scrollViewer;
            focusHandler.ExceptedTextBox = SearchBox;
            Icon = Properties.Resources.DmoDb.ToImageSource();

            this.Closing += ListsWindow_Closing;
            this.Loaded += ListsWindow_Loaded;
        }

        void ListsWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //When the window is closing, wait on the ViewModel to be sure it's not still processing something
            ((SearchIcd10.ViewModels.ListVM)this.DataContext).WaitForExit();
        }

        void ListsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(App.StartupArgs.SearchForText))
            {
                SearchBox.Focus();
            }
            else
            {
                var vm = (ListVM)DataContext;
                vm.SearchTerms = App.StartupArgs.SearchForText;
                var auto = new ButtonAutomationPeer(SearchButton);
                var invokeProvider = auto.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProvider.Invoke();
            }
        }

        void IcdList_ScrollHint(object sender, IcdListScrollHintEventArgs e)
        {
            var animate = new DoubleAnimation();
            animate.From = scrollViewer.VerticalOffset;
            animate.To = e.Hint;
            animate.DecelerationRatio = 0.2;
            animate.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            var story = new Storyboard();
            story.Children.Add(animate);
            Storyboard.SetTarget(animate, scrollViewer);
            Storyboard.SetTargetProperty(animate, new PropertyPath(Behaviors.ScrollViewerBehavior.VerticalOffsetProperty));
            story.Begin();
        }

        void IcdList_ItemsReset(object sender, IcdItemsResetEventArgs e)
        {
            if (!e.IsReset)
            {
                return;
            }
            System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    System.Threading.Thread.Sleep(100);
                    this.Dispatcher.Invoke(() => scrollViewer.ScrollToHome());
                });
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            focusHandler.FocusScrollViewer();
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                var auto = new ButtonAutomationPeer(SearchButton);
                var invokeProvider = auto.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProvider.Invoke();
            }
        }


        /*
         * SCROLLVIEW NICE TO DRAGON FUNCTIONS
         * Review the ScrollViewerFocusHandler class for information, as this just routes the calls there
         */

        private void Focus_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            focusHandler.PreviewMouseDown(sender, e);
        }

        private void Focus_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            focusHandler.PreviewMouseUp(sender, e);
        }

        private void Focus_LostFocus(object sender, RoutedEventArgs e)
        {
            focusHandler.LostFocus(sender, e);
        }

        private void scrollViewer_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            focusHandler.IsVisibleChanged(sender, e);
        }

        /*
         * END SCROLLING FUNCTIONS
         */
    }
}
