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
    
    public partial class Employee_Salary
    {
        public int Id { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<System.DateTime> EffectiveFrom { get; set; }
        public Nullable<int> SalaryType { get; set; }
        public Nullable<int> PaymentFrequency { get; set; }
        public string Amount { get; set; }
        public Nullable<int> Currency { get; set; }
        public string TotalSalary { get; set; }
        public Nullable<int> ReasonforChange { get; set; }
        public string Comments { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> LastModificationDate { get; set; }
        public Nullable<int> UserIDCreatedBy { get; set; }
        public Nullable<bool> Archived { get; set; }
    }
}
