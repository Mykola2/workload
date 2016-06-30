using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WpfApplication3.Models;
using WpfApplication3.Commands;

namespace WpfApplication3.ViewModels
{ 
    class TeacherViewModel : ViewModel
    {
       

        public TeacherViewModel(object o)
        {
            CurrentMSA = (ModuleStudyAction)o;
            FillTeachers();
        }

        public TeacherViewModel()
        {
            FillTeachers();
        }

        private void FillTeachers()
        {
            var q = (from a in GlobalContext.context.TeacherSet select a).ToList();
            this.Teachers = new ObservableCollection<Teacher>(q);
            Teachers.CollectionChanged += ContentCollectionChanged;

        }
        private ObservableCollection<Teacher> _teachers;
        public ObservableCollection<Teacher> Teachers
        {
            get
            {
                return _teachers;
            }

            set
            {
                _teachers = value;
                RaisePropertyChangedEvent("Teachers");

            }
        }
        private Teacher _selectedTeacher;
        public Teacher SelectedTeacher
        {
            get { return _selectedTeacher; }
            set
            {
                if (_selectedTeacher != value)
                {
                    _selectedTeacher = value;
                    RaisePropertyChangedEvent("SelectedTeacher");
                }
            }
        }
        public Action CloseAction { get; set; }
        public void ContentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                GlobalContext.context.TeacherSet.Add(((ObservableCollection<Teacher>)sender).Last());

            }
        }

        public ModuleStudyAction CurrentMSA { get; set; }

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new CommandHandler(Save, true));
            }
        }

        private ICommand _assignCommand;
        public ICommand AssignCommand
        {
            get
            {
                return _saveCommand ?? (_assignCommand = new CommandHandler(AssignTeacherCommand, true));
            }
        }

        private void Save(object sender)
        {
            GlobalContext.context.SaveChanges();
        }

        private void AssignTeacherCommand(object sender)
        {

            Teacher teacher = (Teacher) sender;
            if (!teacher.ModuleStudyAction.Contains(CurrentMSA))
            {
                GlobalContext.context.TeacherSet.Find(teacher.Id).ModuleStudyAction.Add(CurrentMSA);
                GlobalContext.context.ModuleStudyActionSet.Find(CurrentMSA.Id).Teacher = teacher;
                bool exist = false;
                if (teacher.TeacherWorkload != null)
                {
                    foreach (var workload in teacher.TeacherWorkload)
                    {
                        if (workload.Year.ToString().Equals(CurrentMSA.Year))
                        {
                            workload.Amount += CurrentMSA.Hours;
                            exist = true;
                        }
                    }
                    if (!exist)
                    {
                        TeacherWorkload tw = new TeacherWorkload();
                        tw.Teacher = teacher;
                        tw.Year = int.Parse(CurrentMSA.Year);
                        tw.Amount += CurrentMSA.Hours;
                        teacher.TeacherWorkload.Add(tw);
                    }
                }
                else
                {
                    TeacherWorkload tw = new TeacherWorkload();
                    tw.Teacher = teacher;
                    tw.Year = int.Parse(CurrentMSA.Year);
                    tw.Amount += CurrentMSA.Hours;
                    teacher.TeacherWorkload.Add(tw);
                }
                GlobalContext.context.SaveChanges();
            }
            CloseAction();


        }


        public new event PropertyChangedEventHandler PropertyChanged;
        protected new void RaisePropertyChangedEvent(string propertyName)

        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
