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
    
    public partial class GetAllPublicHolidayByEmployee_JobCountry_Result
    {
        public int PublicHolidayId { get; set; }
        public string HolidayName { get; set; }
        public Nullable<System.DateTime> HolidayDate { get; set; }
        public Nullable<int> UserIDCreatedBy { get; set; }
        public Nullable<int> UserIDLastModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
        public int PublicHolidayCountryID { get; set; }
        public Nullable<int> employeePublicHolidayId { get; set; }
        public Nullable<int> EmployeeHolidayEmployeeId { get; set; }
        public Nullable<int> EmployeePublicHolidayCountryId { get; set; }
        public Nullable<System.DateTime> EffecitveFrom { get; set; }
        public Nullable<System.DateTime> EffectiveTo { get; set; }
        public Nullable<System.DateTime> employeeHolidayCreatedDate { get; set; }
        public Nullable<int> EmployeeCreatedBy { get; set; }
    }
}