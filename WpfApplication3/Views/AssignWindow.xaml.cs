using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Windows.Controls;
using WpfApplication3.Models;
using WpfApplication3.ViewModels;

namespace WpfApplication3.Views
{
    /// <summary>
    /// Interaction logic for AssignWindow.xaml
    /// </summary>
    public partial class AssignWindow : Window
    {
        public AssignWindow(int moduleId)
        {
            InitializeComponent();
            DataContext = new AssignViewModel(moduleId);
        }

   
        private void e(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs dataGridAutoGeneratingColumnEventArgs)
        {
            
        }
    }
}
