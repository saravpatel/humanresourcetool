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
    
    public partial class Employee_Skills
    {
        public int Id { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public string TechnicalSkillsName { get; set; }
        public string GeneralSkillsName { get; set; }
        public Nullable<bool> TechnicalSkillsArchived { get; set; }
        public Nullable<bool> GeneralSkillsArchived { get; set; }
        public Nullable<int> UserIDCreatedBy { get; set; }
        public Nullable<int> UserIDLastModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
        public string SkillType { get; set; }
        public Nullable<bool> IsRead { get; set; }
    }
}
