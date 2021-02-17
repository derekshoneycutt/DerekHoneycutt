using SearchIcd10.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
using SearchIcd10.Utils;

namespace SearchIcd10
{
    /// <summary>
    /// Interaction logic for OutputWindow.xaml
    /// </summary>
    public partial class OutputWindow : Window
    {
        public OutputWindow(string startText)
        {
            InitializeComponent();
            Icon = Properties.Resources.DmoDb.ToImageSource();
            ((OutputVM)this.DataContext).Text = startText;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
