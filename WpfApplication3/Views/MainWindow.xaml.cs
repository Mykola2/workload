using System.Windows;
using System.Windows.Controls;
using WpfApplication3.ViewModels;

namespace WpfApplication3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = new ViewModel();
            this.DataContext = vm;
        }

      

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
           
        }

     
    }
}
