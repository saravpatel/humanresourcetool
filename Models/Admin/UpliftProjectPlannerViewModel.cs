using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.Models.Admin
{
    public class UpliftProjectPlannerViewModel
    {
        public int Id { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public System.DateTime Date { get; set; }
        public Nullable<int> UpliftPostionId { get; set; }

        public int? BusinessId { get; set; }
        public int? DivisionId { get; set; }
        public int? PoolId { get; set; }
        public int? FunctionId { get; set; }
        public int? ProjectId { get; set; }
        public int Year { get; set; }
    }
}