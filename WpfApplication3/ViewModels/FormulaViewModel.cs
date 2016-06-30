using System.Collections.ObjectModel;
using System.Linq;
using WpfApplication3.Models;

namespace WpfApplication3.ViewModels
{
    class FormulaViewModel : ViewModel
    {
        public FormulaViewModel()
        {
            FillFormulas();
        }

        private void FillFormulas()
        {
            var q = (from a in GlobalContext.context.FormulaSet select a).ToList();
            this.Formulas = new ObservableCollection<Formula>(q);

        }
        private ObservableCollection<Formula> _formulas;

        public ObservableCollection<Formula> Formulas
        {
            get
            {
                return _formulas;
            }

            set
            {
                _formulas = value;
            }
        }
    }
}

