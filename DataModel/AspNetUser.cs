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
    
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            this.AspNetUserClaims = new HashSet<AspNetUserClaim>();
            this.AspNetUserLogins = new HashSet<AspNetUserLogin>();
            this.AspNetUserRoles = new HashSet<AspNetUserRole>();
        }
    
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string Discriminator { get; set; }
        public Nullable<int> NameTitle { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string KnownAs { get; set; }
        public string SSOID { get; set; }
        public string IMAddress { get; set; }
        public Nullable<int> Gender { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public Nullable<int> Nationality { get; set; }
        public string NINumberSSN { get; set; }
        public string image { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> ContinuousServiceDate { get; set; }
        public Nullable<int> ResourceType { get; set; }
        public Nullable<int> AdditionalReportsto { get; set; }
        public Nullable<int> HRResponsible { get; set; }
        public Nullable<int> JobTitle { get; set; }
        public Nullable<int> JobContryID { get; set; }
        public Nullable<int> Company { get; set; }
        public Nullable<int> Location { get; set; }
        public Nullable<System.DateTime> ProbationEndDate { get; set; }
        public Nullable<System.DateTime> NextProbationReviewDate { get; set; }
        public Nullable<int> NoticePeriod { get; set; }
        public Nullable<System.DateTime> FixedTermEndDate { get; set; }
        public string MethodofRecruitmentSetup { get; set; }
        public string RecruitmentCost { get; set; }
        public Nullable<int> Thisyear { get; set; }
        public Nullable<int> Nextyear { get; set; }
        public string WorkPhone { get; set; }
        public string WorkMobile { get; set; }
        public string CustomeNewTask { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> HasLeft { get; set; }
        public Nullable<int> EducationID { get; set; }
        public Nullable<int> LanguageID { get; set; }
        public Nullable<int> QualificationID { get; set; }
        public Nullable<int> WorkExperienceID { get; set; }
        public string ResumeText { get; set; }
        public Nullable<int> HolidayYear { get; set; }
        public Nullable<int> MeasuredIn { get; set; }
        public Nullable<int> InculudedCarriedOver { get; set; }
        public Nullable<int> TOIL { get; set; }
        public string AuthorisorEmployeeID { get; set; }
        public Nullable<bool> EntitlementIncludesPublicHoliday { get; set; }
        public Nullable<bool> AutoApproveHolidays { get; set; }
        public Nullable<bool> ExceedAllowance { get; set; }
        public Nullable<bool> UseVirtualClock { get; set; }
        public string SelectCustomerCompanyId { get; set; }
        public string CustomerCareID { get; set; }
        public Nullable<bool> Archived { get; set; }
        public Nullable<int> workPatternId { get; set; }
        public Nullable<int> CurrenciesId { get; set; }
        public Nullable<bool> IncludePublicHoliday { get; set; }
        public Nullable<int> HolidayEntitlement { get; set; }
        public Nullable<bool> IsReadAddReport { get; set; }
        public Nullable<bool> IsReadHRRespo { get; set; }
        public Nullable<bool> IsReadArchived { get; set; }
        public Nullable<int> ActivityType { get; set; }
        public Nullable<decimal> RecovryRate { get; set; }
        public Nullable<bool> IsReadNewResource { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> LastModifyBy { get; set; }
    
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
    }
}