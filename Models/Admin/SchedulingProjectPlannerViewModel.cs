using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.Models.Admin
{
    public class SchedulingProjectPlannerViewModel
    {
        public int Id { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public bool IsDayOrMore { get; set; }
        public bool IsLessThenADay { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<decimal> DurationDays { get; set; }
        public string CustomerId { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public Nullable<int> AssetId { get; set; }
        public string ApprovalStatus { get; set; }
        public Nullable<System.DateTime> ApprovalDate { get; set; }

        public int? BusinessId { get; set; }
        public int? DivisionId { get; set; }
        public int? PoolId { get; set; }
        public int? FunctionId { get; set; }

        public int Year { get; set; }
    }
}