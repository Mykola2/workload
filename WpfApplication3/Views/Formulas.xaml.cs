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
    /// Interaction logic for Formulas.xaml
    /// </summary>
    public partial class Formulas : Window
    {
        public Formulas()
        {
            InitializeComponent();
            this.DataContext = new FormulaViewModel();
        }
    }
}
