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
    
    public partial class Cases_Comments
    {
        public int Id { get; set; }
        public int CaseID { get; set; }
        public string Description { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDateTime { get; set; }
        public bool Archived { get; set; }
        public Nullable<int> UserIDCreatedBy { get; set; }
        public Nullable<int> UserIDLastModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
    }
}
