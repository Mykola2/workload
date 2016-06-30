using WpfApplication3.Models;

namespace WpfApplication3
{

    using System;
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// Use this class to fill dictionary-like tables FacultySet, DepartmentSet and StudyActionSet
    /// </summary>
    public class Fixtures
    {
        TeachersModelContainer context;

        public Fixtures(TeachersModelContainer context)
        {
            this.context = context;
        }

        /// <summary>
        /// Install initial fixtures. Does not create duplicates, only installs missing ones.
        /// Use this method to install all fixtures, or other methods to install individual.
        /// </summary>
        public void InstallFixtures()
        {
            using (var context = new TeachersModelContainer())
            {
                InstallFacultyFixtures(context);
                InstallDepartmentFixtures(context);
                InstallStudyActionFixtures(context);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Install fixtures for FacultySet. Does not save changes to context.
        /// </summary>
        /// <param name="context">DbContext instance.</param>
        public static void InstallFacultyFixtures(TeachersModelContainer context)
        {
            List<Faculty> _toSave = new List<Faculty>();
            foreach (var fixture in _facultyFixtures)
            {
                var faculty = (from f in context.FacultySet
                               where f.Name.Equals(fixture, StringComparison.InvariantCultureIgnoreCase)
                               select f).FirstOrDefault();
                if (faculty == null)
                {
                    faculty = new Faculty();
                    faculty.Name = fixture;
                    _toSave.Add(faculty);
                }
            }
            foreach (var item in _toSave)
            {
                context.FacultySet.Add(item);
            }
        }

        /// <summary>
        /// Install fixtures for DepartmentSet. Does not save changes to context.
        /// </summary>
        /// <param name="context">DbContext instance.</param>
        public static void InstallDepartmentFixtures(TeachersModelContainer context)
        {
            Faculty defaultFaculty = context.FacultySet.FirstOrDefault();
            List<Department> _toSave = new List<Department>();
            foreach (var fixture in _departmentFixtures)
            {
                var department = (from f in context.DepartmentSet
                                  where f.Name.Equals(fixture, StringComparison.InvariantCultureIgnoreCase)
                                  select f).FirstOrDefault();
                if (department == null)
                {
                    department = new Department();
                    department.Name = fixture;
                    department.Faculty = defaultFaculty;
                    _toSave.Add(department);
                }
            }
            foreach (var item in _toSave)
            {
                context.DepartmentSet.Add(item);
            }
        }

        /// <summary>
        /// Install fixtures for StudyActionSet. Does not save changes to context.
        /// </summary>
        /// <param name="context">DbContext instance.</param>
        public static void InstallStudyActionFixtures(TeachersModelContainer context)
        {
            List<StudyAction> _toSave = new List<StudyAction>();
            foreach (var kvPair in _studyActionFixtures)
            {
                var studyAction = (from s in context.StudyActionSet
                                   where (s.Name.Equals(kvPair.Key, StringComparison.InvariantCultureIgnoreCase)
                                          && s.IsIndividual == kvPair.Value)
                                   select s).FirstOrDefault();
                if (studyAction == null)
                {
                    studyAction = new StudyAction();
                    studyAction.Name = kvPair.Key;
                    studyAction.IsIndividual = kvPair.Value;
                    _toSave.Add(studyAction);
                }
            }
            foreach (var item in _toSave)
            {
                context.StudyActionSet.Add(item);
            }
        }

        /// <summary>
        /// (Not implemented yet!)
        /// Install fixtures for StudyActionSet. Does not save changes to context.
        /// </summary>
        /// <param name="context">DbContext instance.</param>
        public static void InstallNormFixtures(TeachersModelContainer context)
        {
            throw new NotImplementedException();
        }

        #region fixture data
        static string[] _facultyFixtures = new string[] { "ФПМ" };
        static string[] _departmentFixtures = new string[]
        {
            "Програмного забезпечення комп'ютерних систем",
            "Прикладної математики",
            "Системного програмування і спеціалізованих комп’ютерних систем"
        };
        static Dictionary<string, bool> _studyActionFixtures = new Dictionary<string, bool>
        {
            { "Лекції", false },
            { "Практичні", false },
            { "Лабораторні", false },
            { "Самостійна робота студентів", false },
            { "Екзамени", false },
            { "Заліки", false },
            { "Модульні", false },
            { "Курсові проекти", true },
            { "Курсові роботи", true },
            { "РГР", false },
            { "ДКР", false },
            { "Реферати", true }
        };
        #endregion
    }
}
