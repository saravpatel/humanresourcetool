using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.Models.Admin
{
    public class TimeSheetProjectPlannerViewModel
    {
        public int Id { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Comments { get; set; }
        public Nullable<bool> Archived { get; set; }

        public int? BusinessId { get; set; }
        public int? DivisionId { get; set; }
        public int? PoolId { get; set; }
        public int? FunctionId { get; set; }
        public int? ProjectId { get; set; }
        public int Year { get; set; }

    }
}