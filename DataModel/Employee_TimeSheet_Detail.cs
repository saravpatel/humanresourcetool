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
    
    public partial class Employee_TimeSheet_Detail
    {
        public int Id { get; set; }
        public Nullable<int> TimeSheetId { get; set; }
        public Nullable<int> InTimeHr { get; set; }
        public Nullable<int> InTimeMin { get; set; }
        public Nullable<int> EndTimeHr { get; set; }
        public Nullable<int> EndTimeMin { get; set; }
        public Nullable<int> Project { get; set; }
        public Nullable<int> CostCode { get; set; }
        public string Customer { get; set; }
        public Nullable<int> Asset { get; set; }
        public Nullable<bool> Archived { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public bool IsRead { get; set; }
        public string ApprovalStatus { get; set; }
        public Nullable<System.DateTime> ApprovalDate { get; set; }
        public Nullable<bool> IsReadHR { get; set; }
        public Nullable<bool> IsReadAddRep { get; set; }
    }
}
