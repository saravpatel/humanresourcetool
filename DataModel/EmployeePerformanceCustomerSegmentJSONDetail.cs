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
    using System.Collections.Generic;
    
    public partial class EmployeePerformanceCustomerSegmentJSONDetail
    {
        public int Id { get; set; }
        public Nullable<int> PerCustomerdetailId { get; set; }
        public Nullable<int> CustomerSegId { get; set; }
        public Nullable<int> QueId { get; set; }
        public string Score { get; set; }
        public string Comments { get; set; }
        public Nullable<bool> Archived { get; set; }
        public Nullable<int> UserIdCreatedBy { get; set; }
        public Nullable<System.DateTime> UserIdCreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
