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
    
    public partial class Vacancy
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public int StatusID { get; set; }
        public Nullable<System.DateTime> ClosingDate { get; set; }
        public int RecruitmentProcessID { get; set; }
        public string Salary { get; set; }
        public string Location { get; set; }
        public Nullable<int> BusinessID { get; set; }
        public Nullable<int> DivisionID { get; set; }
        public Nullable<int> PoolID { get; set; }
        public Nullable<int> FunctionID { get; set; }
        public string JobDescription { get; set; }
        public int HiringLeadID { get; set; }
        public bool MustUploadCoverLetter { get; set; }
        public bool MustUploadResumeCV { get; set; }
        public string ApplicationFormPathOriginal { get; set; }
        public string ApplicationFormPath { get; set; }
        public bool Question1On { get; set; }
        public string Question1Text { get; set; }
        public bool Question2On { get; set; }
        public string Question2Text { get; set; }
        public bool Question3On { get; set; }
        public string Question3Text { get; set; }
        public bool Question4On { get; set; }
        public string Question4Text { get; set; }
        public bool Question5On { get; set; }
        public string Question5Text { get; set; }
        public Nullable<int> SourceID { get; set; }
        public bool Archived { get; set; }
        public Nullable<int> UserIDCreatedBy { get; set; }
        public Nullable<int> UserIDLastModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
        public bool IsRead { get; set; }
        public string ApprovalStatus { get; set; }
        public Nullable<System.DateTime> ApprovalDate { get; set; }
        public Nullable<bool> IsReadVacancy { get; set; }
    }
}