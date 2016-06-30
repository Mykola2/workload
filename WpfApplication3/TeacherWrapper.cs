using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication3.Models;

namespace WpfApplication3
{
    class TeacherWrapper
    {
        public Teacher teacher;

        public TeacherWrapper(Teacher teacher)
        {
            this.teacher = teacher;
            throw new NotImplementedException();
        }

        public bool ValidateNorms()
        {
            return false;
        }

        public int CountHours()
        {
            int result = 0;

            foreach (var item in teacher.ModuleStudyAction)
            {
                // magic here
            }

            return result;
        }
    }
}
