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
    
    public partial class EmployeeTraining
    {
        public int Id { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public Nullable<int> TrainingNameId { get; set; }
        public string Description { get; set; }
        public Nullable<int> Importance { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> Progress { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public string Provider { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public string Notes { get; set; }
        public string CustomFieldsJSON { get; set; }
        public Nullable<bool> Archived { get; set; }
        public Nullable<int> UserIDCreatedBy { get; set; }
        public Nullable<int> UserIDLastModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string ApprovalStatus { get; set; }
        public Nullable<System.DateTime> ApprovalDate { get; set; }
        public bool IsRead { get; set; }
        public Nullable<bool> IsReadWorker { get; set; }
        public Nullable<bool> IsReadHR { get; set; }
        public Nullable<bool> IsReadAddRep { get; set; }
    }
}