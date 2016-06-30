using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToExcel;

namespace WpfApplication3
{
    public class Credit
    {
        //TODO: import aggregated hour info to check validity
        public int? Id { get; set; }
        public string CreditName { get; set; }
        public string Department { get; set; }
        public int? ETCSCreditCount { get; set; }
        public int? LectionHourCount { get; set; }
        public int? SeminarHourCount { get; set; }
        public int? LabHourCount { get; set; }
        //TODO: choose proper name
        public int? HomeworkHourCount { get; set; }

        public Credit(RowNoHeader row)
        {
            if (row == null)
                return;
            Id = cellToInt(row[0]);
            CreditName = row[1];
            Department = row[2];
            ETCSCreditCount = cellToInt(row[3]);
            LectionHourCount = cellToInt(row[6]);
            SeminarHourCount = cellToInt(row[7]);
            LabHourCount = cellToInt(row[8]);
            HomeworkHourCount = cellToInt(row[9]);
        }

        public Credit() : this(null)
        { }

        public bool IsValid
        {
            get
            {
                bool has_hours = LectionHourCount != null || SeminarHourCount != null || LabHourCount == null
                    || HomeworkHourCount == null;
                if (CreditName == null && Department == null && !has_hours)
                    return false;
                return true;
            }
        }

        public override string ToString()
        {
            return String.Format("{0} {1} {2}", Id, CreditName, Department);
        }

        private int? cellToInt(Cell c)
        {
            try
            {
                return int.Parse(c);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
