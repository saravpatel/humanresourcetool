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
    
    public partial class Employee_AddEndrosementSkills
    {
        public int Id { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public string AssignSkillId { get; set; }
        public Nullable<int> SkilsId { get; set; }
        public string SkillsName { get; set; }
        public Nullable<bool> Archived { get; set; }
        public Nullable<int> UserIDCreatedBy { get; set; }
        public Nullable<int> UserIDLastModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
        public string SkillType { get; set; }
        public bool IsRead { get; set; }
    }
}
