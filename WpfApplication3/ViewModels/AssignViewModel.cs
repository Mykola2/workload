using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using WpfApplication3.Models;
using WpfApplication3.Commands;
using WpfApplication3.Views;

namespace WpfApplication3.ViewModels
{
    class AssignViewModel : ViewModel
    {
        public AssignViewModel(int moduleId)
        {
            FillStudyActions(moduleId);
        }


        private void FillStudyActions(int moduleId)
        {
           
            Teachers = new ObservableCollection<string>();
            //GlobalContext.context.TeacherSet.ForEachAsync(teacher => Teachers.Add(teacher.Name));
            var msa = GlobalContext.context.ModuleSet.Find(moduleId).ModuleStudyActions;
            ModuleStudyActions = new ObservableCollection<ModuleStudyAction>(msa);


        }

        private ObservableCollection<ModuleStudyAction> _moduleStudyActions;
        public ObservableCollection<ModuleStudyAction> ModuleStudyActions
        {
            get { return _moduleStudyActions; }
            set { _moduleStudyActions = value; RaisePropertyChangedEvent("ModuleStudyActions"); }
        }

        private ModuleStudyAction _selectedModuleStudyAction;

        public ModuleStudyAction SelectedModuleStudyAction
        {
            get { return _selectedModuleStudyAction; }
            set { _selectedModuleStudyAction = value; RaisePropertyChangedEvent("SelectedModuleStudyAction"); }
        }

        private ObservableCollection<string> _teachers;
        public new ObservableCollection<string> Teachers
        {
            get { return _teachers; }
            set { _teachers = value; RaisePropertyChangedEvent("Teachers"); }
        }

        private ICommand _assignTeacherCommand;
        public ICommand AssignTeacherCommand
        {
            get
            {
                return _assignTeacherCommand ?? (_assignTeacherCommand = new CommandHandler(param => Assign(param), true));
            }
        }

        
        private void Assign(object o)
        {
           
            AssignTeacherWindow win = new AssignTeacherWindow(o);
            win.Show();
        }
    }
}
