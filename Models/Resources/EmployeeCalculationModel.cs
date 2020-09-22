using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.Models.Resources
{
    public class EmployeeCalculationModel
    {
        public int EmployeeId { get; set; }

        public int EmployeeLength { get; set; }

        public string EployeeAbsence { get; set; }

        public string EmployeeHoliday { get; set; }

        public string EmployeeContractDay { get; set; }

        public string EmployeeDayworkInYear { get; set; }

        public string EmployeeDayworkedSinceYear { get; set; }


    }
}