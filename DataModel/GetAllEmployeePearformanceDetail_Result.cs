//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRTool.DataModel
{
    using System;
    
    public partial class GetAllEmployeePearformanceDetail_Result
    {
        public int ReviewId { get; set; }
        public int PerformanceId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeImage { get; set; }
        public Nullable<int> ReportToId { get; set; }
        public Nullable<int> BussinessId { get; set; }
        public Nullable<int> DivisionId { get; set; }
        public Nullable<int> PoolId { get; set; }
        public Nullable<int> FunctionId { get; set; }
        public Nullable<int> JobTitleId { get; set; }
        public int PerfDetailId { get; set; }
        public string OverAllScore { get; set; }
        public string JobRoleScore { get; set; }
        public string CoWorkerScore { get; set; }
        public string CustomerScore { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}