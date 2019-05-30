using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareTechnikProjekt
{
    class ApplicationConstants
    {
        public enum ModuleContainer {Open, Planned, Finished };
        public static List<double> Grades { get => GetGrades(); }
        private static List<double> _grades;

        private static List<double> GetGrades()
        {
            if (_grades == null)
            {
                _grades = new List<double> { 1.0, 1.3, 1.7, 2.0, 2.3, 2.7, 3.0, 3.3, 3.7, 4.0, 5.0 };
            }

            return _grades;
        }
    }
}
