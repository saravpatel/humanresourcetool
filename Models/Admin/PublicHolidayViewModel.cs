using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.Models.Admin
{
    public class PublicHolidayViewModel
    {
        public int Id { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public int? BusinessId { get; set; }
        public int? DivisionId { get; set; }
        public int? PoolId { get; set; }
        public int? FunctionId { get; set; }
        public int? ProjectId { get; set; }
        public int Year { get; set; }
    }
}