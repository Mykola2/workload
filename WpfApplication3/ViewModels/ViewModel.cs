using System;
using System.Collections.Generic;
using WpfApplication3.Views;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using WpfApplication3.Models;
using WpfApplication3.Commands;
using Expression = NCalc.Expression;

namespace WpfApplication3.ViewModels
{
    class ViewModel : INotifyPropertyChanged
    {

      
        public ViewModel()
        {
            _canExecute = true;
            FillK3B();  
        }

     

        private void FillK3B()
        {
            var q = (from a in GlobalContext.context.ModuleSet select a).ToList();
            Modules = new ObservableCollection<Module>(q);
            Modules.CollectionChanged += ContentCollectionChanged;
            var qt = (from t in GlobalContext.context.TeacherSet select t).ToList();
            Teachers = new ObservableCollection<Teacher>(qt);
        }

        private ObservableCollection<Module> _modules;
        public ObservableCollection<Module> Modules
        {
            get
            {
                return _modules;
            }

            set
            {
                _modules = value;
                RaisePropertyChangedEvent("Modules");
            }
        }

        private Module _selectedModule;
        public Module SelectedModule
        {
            get
            {
                return _selectedModule;
            }
            set
            {
                _selectedModule = value;
                RaisePropertyChangedEvent("SelectedModule");

            }
        }

        private ObservableCollection<Teacher> _teachers;

        public void ContentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                GlobalContext.context.ModuleSet.Add(((ObservableCollection<Module>) sender).Last());
                
            }
        }

        private ICommand _importCommand;
         public ICommand ImportCommand
         {
             get
             {
                 return _importCommand ?? (_importCommand = new CommandHandler(Import, _canExecute));
             }
         }

        private void Import(object sender)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xls)|*.xls|(*.xlsx)|*.xlsx";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.ShowDialog();
            /*Fixtures.InstallFixtures();

            XLSLoader loader = new XLSLoader(openFileDialog.FileName);
            var modules = loader.GetModules();
            var groups = loader.GetGroups();
            var msaDict = loader.GetModuleStudyActionsDict(modules, groups);
          
            loader.SaveStuff(modules, groups);
            loader.SaveModuleStudyActions(msaDict);*/
            var context = new ModelContext();
            var fixtures = new Fixtures(context);
            fixtures.InstallFixtures();

            XLSLoader loader = new XLSLoader(openFileDialog.FileName, context: context);
            var modules = loader.GetModules();
            var groups = loader.GetGroups();
            var msaDict = loader.GetModuleStudyActionsDict(modules, groups);

            loader.Save(modules, groups);
            loader.SaveModuleStudyActions(msaDict);
            FillK3B();
           
        }
    

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new CommandHandler(Save, _canExecute));
            }
        }

        private ICommand _openTeachersCommand;
        public ICommand OpenTeachersCommand
        {
            get
            {
                return _openTeachersCommand ?? (_openTeachersCommand = new CommandHandler(OpenTeachers, _canExecute));
            }
        }

        private ICommand _openFormulasCommand;
        public ICommand OpenFormulasCommand
        {
            get
            {
                return _openFormulasCommand ?? (_openFormulasCommand = new CommandHandler(OpenFormulas, _canExecute));
            }
        }

        private ICommand _assignCommand;
        public ICommand AssignCommand
        {
            get
            {
                return _assignCommand ?? (_assignCommand = new CommandHandler(param => Assign(param), _canExecute));
            }
        }

        public ObservableCollection<Teacher> Teachers
        {
            get { return _teachers; }
            set { _teachers = value; }
        }

        private void Assign(object sender)
        {
           
            var module = (Module) sender;
            int moduleId = module.Id;
            AssignWindow win = new AssignWindow(moduleId);
            win.Title = module.Title;
            win.Show();

        }

        private void OpenTeachers(object sender)
        {

          Teachers win = new Teachers();
            win.Show();
        }

        private void OpenFormulas(object sender)
        {
            Views.Formulas win = new Views.Formulas();
            win.Show();
        }

        private void Save(object sender)
        {
            GlobalContext.context.SaveChanges();
        }

        private bool _canExecute;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChangedEvent(string propertyName)
            
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public void Calculate()
        {
            Expression e = new Expression("2+2");
        }
    }
}
