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
using System.Windows.Shapes;
using WpfApplication3.ViewModels;

namespace WpfApplication3.Views
{
    /// <summary>
    /// Interaction logic for AssignTeacherWindow.xaml
    /// </summary>
    public partial class AssignTeacherWindow : Window
    {
        private object o;

        public AssignTeacherWindow(object o)
        {
           
            InitializeComponent();
            TeacherViewModel vm = new TeacherViewModel(o);
            this.DataContext = vm;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(this.Close);
        }

        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Id")
            {
                e.Cancel = true;

            }
        }
    }
}
