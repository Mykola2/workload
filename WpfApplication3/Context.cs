using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication3.Models;

namespace WpfApplication3
{
    public static class GlobalContext
    {
        public static TeachersModelContainer context = new TeachersModelContainer();
    }
}
