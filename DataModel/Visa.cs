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
    
    public partial class Visa
    {
        public int Id { get; set; }
        public Nullable<int> AssignedToEmployeeId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> RelationToCSEmployeeID { get; set; }
        public string Description { get; set; }
        public Nullable<int> Country { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<int> VisaType { get; set; }
        public Nullable<int> ServiceAgency { get; set; }
        public string Number { get; set; }
        public Nullable<int> AlertBeforeDays { get; set; }
        public Nullable<System.DateTime> DueDateChangedOn { get; set; }
        public Nullable<bool> Archived { get; set; }
        public Nullable<int> CSUserIDCreatedBy { get; set; }
        public Nullable<int> CSUserIDLastModifiedBy { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
    }
}
