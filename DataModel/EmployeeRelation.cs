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
    
    public partial class EmployeeRelation
    {
        public int EmployeeRelationID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> Reportsto { get; set; }
        public Nullable<int> BusinessID { get; set; }
        public Nullable<int> DivisionID { get; set; }
        public Nullable<int> PoolID { get; set; }
        public Nullable<int> FunctionID { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}
