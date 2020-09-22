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
    
    public partial class Benefit
    {
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public int BenefitID { get; set; }
        public Nullable<System.DateTime> DateAwarded { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<decimal> FixedAmount { get; set; }
        public Nullable<int> Currency { get; set; }
        public bool RecoverOnTermination { get; set; }
        public string Comments { get; set; }
        public bool Archived { get; set; }
        public Nullable<int> UserIDCreatedBy { get; set; }
        public Nullable<int> UserIDLastModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
    }
}