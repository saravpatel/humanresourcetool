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
    
    public partial class GetAllOverAllScorePerformance_Result
    {
        public int ReviewId { get; set; }
        public int PerfReviewId { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeImage { get; set; }
        public string InviteCustomerName { get; set; }
        public string InviteCustomerImage { get; set; }
        public string ReviewStatus { get; set; }
        public Nullable<int> EmployeePoolId { get; set; }
        public Nullable<int> JobTitleId { get; set; }
        public Nullable<int> EmployeeReportToId { get; set; }
        public string CustomerInviteStatus { get; set; }
        public string CoWorkerInviteStatus { get; set; }
        public string OverAllScore { get; set; }
    }
}
