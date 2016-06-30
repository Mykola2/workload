using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WpfApplication3.Models;
using Group = WpfApplication3.Models.Group;


namespace WpfApplication3
{
    using System;
    using LinqToExcel;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data.Entity.Validation;
    public class ModelContext : TeachersModelContainer
    {

        public override int SaveChanges()
        {
            int result = base.SaveChanges();
            ((IObjectContextAdapter)this).ObjectContext.AcceptAllChanges();
            return result;
        }
    }

    public static class Offset
    {
        public static int Title = 1;
        public static int Credits = 3;
        public static int Lections = 6;
        public static int Practices = 7;
        public static int Labs = 8;
        public static int Self = 9;
        public static int Exam = 10;
        public static int Zalik = 11;
        public static int Module_tests = 12;
        public static int Course_project = 13;
        public static int Course_work = 14;
        public static int RGR = 15;
        public static int DKR = 16;
        public static int Referat = 17;
        public static int Groups = 18;
    }

    public static class Global
    {
        public static int year = 2016;
        public static string yearStr { get { return year.ToString(); } }
    }

    /// <summary>
    /// Main class to import groups and modules
    /// </summary>
    public class XLSLoader
    {
        public struct ImportResults
        {
            public List<Module> Modules;
            public List<Group> Groups;
            public Dictionary<Module, List<ModuleStudyAction>> ModuleStudyActions;
        }

        public string SheetName { get; private set; }

        public XLSLoader(string fileName,
            string sheetName = "бакалавр",
            TeachersModelContainer context = null)
        {
            if (context == null)
            {
                _context = new ModelContext();
            }
            else
            {
                _context = context;
            }
            _excel = new ExcelQueryFactory(fileName);
            SheetName = sheetName;
            _rawData = (from c in _excel.WorksheetNoHeader(SheetName)
                        select c).ToList();
            _moduleOffsets = new Dictionary<Module, List<int>>();
            _groupOffsets = new Dictionary<Group, int>();
        }

        public ImportResults DoImport()
        {
            var result = new ImportResults();
            result.Groups = this.GetGroups();
            result.Modules = this.GetModules();
            result.ModuleStudyActions = this.GetModuleStudyActionsDict(result.Modules, result.Groups);
            return result;
        }

        public void Save(List<Module> modules,
                         List<Group> groups)
        {
            modules.ForEach(module => _context.ModuleSet.Add(module));
            groups.ForEach(group => _context.GroupSet.Add(group));
            _context.SaveChanges();
        }

        public void SaveModuleStudyActions(
            Dictionary<Module, List<ModuleStudyAction>> moduleSturyActions)
        {
            foreach (var kvPair in moduleSturyActions)
            {
                //kvPair.Value.ForEach(msa => calculate(msa));
                kvPair.Value.ForEach(msa => _context.ModuleStudyActionSet.Add(msa));
            }
            
            _context.SaveChanges();
        }

       /* public void calculate(ModuleStudyAction msa)
        {
            int budget_count = msa.Group.Budget_students;
            int contract_count = msa.Group.Contract_students;
            if (msa.StudyActions.Name == "Заліки")
            {
                msa.Hours = 0.25*(budget_count + contract_count);
            }
        }*/

        /// <summary>
        /// Tries to parse given sheet in given xls to extract Modules.
        /// </summary>
        /// <returns>List of modules</returns>
        public List<Module> GetModules()
        {
            ModuleImport importer = new ModuleImport();
            List<Module> result = new List<Module>();
            foreach (var row in _rawData)
            {
                var module = importer.ParseModule(row);
                if (module != null)
                {
                    result.Add(module);
                    _moduleOffsets[module] = new List<int>();
                    for (int i = Offset.Groups; i < row.Count; i += 8)
                    {
                        bool b1 = row[i].Value as string != null;
                        bool b2 = (i + 4) < row.Count && row[i + 4].Value as string != null;
                        if (b1 || b2)
                        {
                            _moduleOffsets[module].Add(i);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Tries to parse given sheet in given xls to extract Groups.
        /// </summary>
        /// <returns>List of credits</returns>
        public List<Group> GetGroups()
        {
            List<Group> result = new List<Group>();
            // Skip the header
            int verticalOffset = 0;
            foreach (var row in _rawData)
            {
                if (row[18] == _groupListHeader)
                    break;
                verticalOffset++;
            }
            var courseRow = _rawData[verticalOffset + 1];
            var titleRow = _rawData[verticalOffset + 2];
            for (int i = 0; i < courseRow.Count && i < titleRow.Count; i++)
            {
                if (courseRow[i] != "" && titleRow[i] != "")
                {
                    GroupImporter _g = new GroupImporter();
                    var group = _g.ImportGroup(courseRow, titleRow, i);
                    result.Add(group);
                    _groupOffsets[group] = i;
                }
            }
            return result;
        }

        /// <summary>
        /// Get ModuleStudyActions for given list of modules
        /// </summary>
        public Dictionary<Module, List<ModuleStudyAction>> GetModuleStudyActionsDict(
            List<Module> modules,
            List<Group> groups)
        {
            var result = new Dictionary<Module, List<ModuleStudyAction>>();
            foreach (var module in modules)
            {
                result[module] = GetModuleStudyActions(module, groups);
            }
            return result;
        }

        /// <summary>
        /// Get ModuleStudyActions for one module
        /// </summary>
        public List<ModuleStudyAction> GetModuleStudyActions(
            Module module,
            List<Group> groups)
        {
            List<ModuleStudyAction> result = new List<ModuleStudyAction>();
            ModuleStudyActionImport importer = new ModuleStudyActionImport(_context, null);
            foreach (var group in groups)
            {
                int groupOffset = _groupOffsets[group];
                if (_moduleOffsets[module].Contains(groupOffset))
                {
                    result.AddRange(importer.DoImport(group, module));
                }
            }
            return result;
        }

        /// <summary>
        /// Generate ModuleStudyAction from given data
        /// </summary>
        private ModuleStudyAction GetModuleStudyAction(Module module,
            Group group,
            bool failSilently = false)
        {
            ModuleStudyAction _m = null;
            GroupWrapper _group = group as GroupWrapper;
            if (_group == null && !failSilently)
            {
                //throw new ArgumentException("Group has wrong type");
            }
            else
            {
                _m.Group = group;
            }
            return _m;
        }

        /// <summary>
        /// ExcelQueryFactory used to import data
        /// </summary>
        private ExcelQueryFactory _excel;

        /// <summary>
        /// Imported data represented as list.
        /// </summary>
        private List<RowNoHeader> _rawData;

        /// <summary>
        /// Subheader for group import
        /// </summary>
        private string _groupListHeader = "Кількість годин аудиторних занять на тиждень за семестрами";

        private TeachersModelContainer _context;

        private Dictionary<Group, int> _groupOffsets;
        private Dictionary<Module, List<int>> _moduleOffsets;
    }

    /// <summary>
    /// Utility class for module importing
    /// </summary>
    public class ModuleImport
    {
        public ModuleImport() { }

        /// <summary>
        /// Checks if imported row is valid: Name, Department, 
        /// HomeworkHours exist and are of correct types 
        /// </summary>
        /// <param name="row">Row to be checked</param>
        /// <returns>True if row is valid</returns>
        public static bool IsValidRow(RowNoHeader row)
        {
            //Check if crucial cells are empty
            //1 being name, 2 being department and 9 being homework hours
            if (row[1] == "" || row[2] == "" || row[9] == "")
                return false;
            //Check if a credit, not just some row
            int parseInt;
            string name = row[1].ToString().Trim();
            string department = row[2].ToString().Trim();
            string homeworkHours = row[9].ToString().Trim();
            //homeworkHours must be a number, whereas name and department should not
            if (int.TryParse(name, out parseInt)
                || int.TryParse(department, out parseInt)
                || !int.TryParse(homeworkHours, out parseInt)
                || !isValidDepartment(department))
                return false;

            return true;
        }

        /// <summary>
        /// Check if given string represents a valid department name
        /// </summary>
        /// <param name="department">A cell from .xls plan</param>
        /// <returns></returns>
        private static bool isValidDepartment(string department)
        {
            string valid1 = "Програмного забезпечення комп'ютерних систем";
            string valid2 = "ПЗКС";
            var result = department.Equals(valid1, StringComparison.InvariantCultureIgnoreCase)
                || department.Equals(valid2, StringComparison.InvariantCultureIgnoreCase);
            return result;
        }

        /// <summary>
        /// Convert given string to an int representation. 
        /// If conversion fails - return 0 to represent empty.
        /// </summary>
        /// <param name="c">Cell from row</param>
        /// <returns>int representation</returns>
        private int cellToInt(Cell c)
        {
            try
            {
                return int.Parse(c);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public Module ParseModule(RowNoHeader row)
        {
            if (row == null || !IsValidRow(row))
                return null;
            var module = new Module();
            module.Title = row[Offset.Title];
            module.Credits = double.Parse(row[Offset.Credits]); // Guaranteed to be an double
            module.Lections = cellToInt(row[Offset.Lections]);
            module.Practices = cellToInt(row[Offset.Practices]);
            module.Labs = cellToInt(row[Offset.Labs]);
            //module.Self = cellToInt(row[Offset.Self]);
            module.Exam = cellToInt(row[Offset.Exam]);
            module.ZALIK = row[Offset.Zalik];
            module.Module_tests = cellToInt(row[Offset.Module_tests]);
            module.Course_project = cellToInt(row[Offset.Course_project]);
            module.Course_work = cellToInt(row[Offset.Course_work]);
            module.RGR = cellToInt(row[Offset.RGR]);
            module.DKR = cellToInt(row[Offset.DKR]);
            module.Referat = cellToInt(row[Offset.Referat]);
            return module;
        }
    }

    public class GroupWrapper : Group
    {
        /// <summary>
        /// Horizontal offset in xls this group was extracted from
        /// </summary>
        public int HorizontalOffset { get; set; }

        public GroupWrapper() : base()
        {
            HorizontalOffset = 0;
        }

        public GroupWrapper(int horizontalOffset) : base()
        {
            HorizontalOffset = horizontalOffset;
        }
    }

    public class ModuleWrapper : Module
    {
        public List<int> GroupFlags = new List<int>();
    }

    public class GroupImporter
    {
        public bool IsValid(Group group)
        {
            return _regex.IsMatch(group.Name);
        }

        public Group ImportGroup(RowNoHeader courseRow,
                                 RowNoHeader titleRow,
                                 int offset)
        {
            Group group = new Group();
            parseTitle(group, titleRow[offset]);
            group.Course = courseRow[offset];
            group.Year = 2015;
            return group;
        }

        private void parseTitle(Group group, string titleRow)
        {
            var match = _regex.Match(titleRow);
            var matchGroups = match.Groups;
            if (matchGroups.Count >= 3)
            {
                group.Name = matchGroups[1].Value;
                group.Budget_students = Convert.ToInt32(matchGroups[2].Value);
                group.Contract_students = Convert.ToInt32(matchGroups[3].Value);
            }
        }

        //Matches group title, which looks like "КП-21(20+4)"
        private Regex _regex = new Regex(@"(.[А-Я]-\d+)\((\d+)\+(\d+)\)");
    }

    public class ModuleStudyActionImport
    {
        TeachersModelContainer context;
        RowNoHeader row;

        public ModuleStudyActionImport(TeachersModelContainer context,
                                       RowNoHeader row)
        {
            this.context = context;
            this.row = row;

        }

        public List<ModuleStudyAction> DoImport(Group group, Module module)
        {
            List<ModuleStudyAction> result = new List<ModuleStudyAction>();
            // For the sake of convenience
            Action<ModuleStudyAction> add = x => { if (x != null) result.Add(x); };

            add(_doLections(group, module));
            add(_doPractices(group, module));
            add(_doLabs(group, module));
            add(_doSelf(group, module));
            add(_doExam(group, module));
            add(_doZALIK(group, module));
            add(_doModule_tests(group, module));
            add(_doCourse_project(group, module));
            add(_doCourse_work(group, module));
            add(_doRGR(group, module));
            add(_doDKR(group, module));
            add(_doReferat(group, module));

            return result;
        }

        private ModuleStudyAction _getItem(
            string actionName, int hours,
            Group group, Module module)
        {
            ModuleStudyAction result = new ModuleStudyAction();
            result.Group = group;
            result.StudyActions = getStudyAction(actionName);
            result.Module = module;
            result.Hours = hours;
            result.Year = Global.yearStr;
            return result;
        }

        /// <summary>
        /// Return appropriate StudyAction
        /// </summary>
        private StudyAction getStudyAction(string name)
        {
            var item = (from s in context.StudyActionSet
                        where name.Equals(s.Name, StringComparison.InvariantCultureIgnoreCase)
                        select s).FirstOrDefault();
            context.StudyActionSet.Attach(item);
            return item;
        }

        #region Import convenience methods
        ModuleStudyAction _doLections(Group group, Module module)
        {
            ModuleStudyAction result = null;
            if (module.Lections != null && module.Lections > 0)
            {
                result = _getItem("Лекції", (int)module.Lections, group, module);
            }
            return result;
        }

        ModuleStudyAction _doPractices(Group group, Module module)
        {
            ModuleStudyAction result = null;
            if (module.Practices != null && module.Practices > 0)
            {
                result = _getItem("Практичні", (int)module.Practices, group, module);
            }
            return result;
        }

        ModuleStudyAction _doLabs(Group group, Module module)
        {
            ModuleStudyAction result = null;
            if (module.Labs != null && module.Labs > 0)
            {
                result = _getItem("Лабораторні", (int)module.Labs, group, module);
            }
            return result;
        }

        ModuleStudyAction _doSelf(Group group, Module module)
        {
            ModuleStudyAction result = null;
            if (module.Self != null && module.Self > 0)
            {
                result = _getItem("Самостійна робота студентів", (int)module.Self, group, module);
            }
            return result;
        }

        ModuleStudyAction _doExam(Group group, Module module)
        {
            ModuleStudyAction result = null;
            if (module.Exam != null && module.Exam > 0)
            {
                result = _getItem("Екзамени", (int)module.Exam, group, module);
            }
            return result;
        }

        ModuleStudyAction _doZALIK(Group group, Module module)
        {
            ModuleStudyAction result = null;
            if (module.ZALIK != null)
            {
                // String like "(%d)(д)", i.e. "1", "8", "4д"
                var capture = Regex.Match(module.ZALIK, @"\d+");
                if (capture.Success)
                {
                    int value = Convert.ToInt32(capture.Value);
                    result = _getItem("Заліки", value, group, module);
                }
            }
            return result;
        }

        ModuleStudyAction _doModule_tests(Group group, Module module)
        {
            ModuleStudyAction result = null;
            if (module.Module_tests != null && module.Module_tests > 0)
            {
                result = _getItem("Модульні", (int)module.Module_tests, group, module);
            }
            return result;
        }

        ModuleStudyAction _doCourse_project(Group group, Module module)
        {
            ModuleStudyAction result = null;
            if (module.Course_project != null && module.Course_project > 0)
            {
                result = _getItem("Курсові  проекти", (int)module.Course_project, group, module);
            }
            return result;
        }

        ModuleStudyAction _doCourse_work(Group group, Module module)
        {
            ModuleStudyAction result = null;
            if (module.Course_work != null && module.Course_work > 0)
            {
                result = _getItem("Курсові роботи", (int)module.Course_work, group, module);
            }
            return result;
        }

        ModuleStudyAction _doRGR(Group group, Module module)
        {
            ModuleStudyAction result = null;
            if (module.RGR != null && module.RGR > 0)
            {
                result = _getItem("РГР", (int)module.RGR, group, module);
            }
            return result;
        }

        ModuleStudyAction _doDKR(Group group, Module module)
        {
            ModuleStudyAction result = null;
            if (module.DKR != null && module.DKR > 0)
            {
                result = _getItem("ДКР", (int)module.DKR, group, module);
            }
            return result;
        }

        ModuleStudyAction _doReferat(Group group, Module module)
        {
            ModuleStudyAction result = null;
            if (module.Referat != null && module.Referat > 0)
            {
                result = _getItem("Реферати", (int)module.Referat, group, module);
            }
            return result;
        }
        #endregion
    }
}
