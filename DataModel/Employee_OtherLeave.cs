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
    
    public partial class Employee_OtherLeave
    {
        public int Id { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public Nullable<int> ReasonForLeaveId { get; set; }
        public Nullable<bool> IsLessThenADay { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<decimal> Duration { get; set; }
        public Nullable<int> Hour { get; set; }
        public Nullable<int> Min { get; set; }
        public string Comment { get; set; }
        public Nullable<int> PartOfTheDay { get; set; }
        public Nullable<bool> Archived { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public string ApprovalStatus { get; set; }
        public Nullable<System.DateTime> ApprovalDate { get; set; }
        public bool IsRead { get; set; }
        public Nullable<bool> IsReadHR { get; set; }
        public Nullable<bool> IsReadAddRep { get; set; }
    }
}
