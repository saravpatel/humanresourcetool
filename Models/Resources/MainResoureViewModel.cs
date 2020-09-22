using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Resources
{
    public class MainResoureViewModel
    {
        public MainResoureViewModel()
        {
            TitleList = new List<SelectListItem>();
            GenderList = new List<SelectListItem>();
            LocationList = new List<SelectListItem>();
            BusinessList = new List<SelectListItem>();
            DivisionList = new List<SelectListItem>();
            PoolList = new List<SelectListItem>();
            FunctionList = new List<SelectListItem>();
            NationalityList = new List<SelectListItem>();
            CopyFromList = new List<SelectListItem>();
            ReportstoList = new List<SelectListItem>();
            AdditionalReportstoList = new List<SelectListItem>();
            HRResponsibleList = new List<SelectListItem>();
            JobTitleList = new List<SelectListItem>();
            CountryDropdown = new List<SelectListItem>();
            JobCountryList = new List<SelectListItem>();
            ResourceTypeList = new List<SelectListItem>();
            StateDropdown = new List<SelectListItem>();
            CityDropdown = new List<SelectListItem>();
            AirportList = new List<SelectListItem>();
            NoticePeriodList = new List<SelectListItem>();
            AssignList = new List<SelectListItem>();
            StatusList = new List<SelectListItem>();
            CustomerList = new List<MainResoureViewModel>();
            ResoureceList = new List<MainResoureViewModel>();
            CompanyList = new List<SelectListItem>();
            CountingList = new List<SelectListItem>();
            AllListData = new List<MainResoureViewModel>();
            CoustomerCompanyList = new List<SelectListItem>();
            NewsTasklistRecord = new List<AddNewTaskListViewModel>();
            CustomerCareList = new List<SelectListItem>();
            CurruencyCodeList = new List<SelectListItem>();
            EmployeeCustomerCare = new List<SelectListItem>();
        }
        public Nullable<int> cFlag { get; set; }
        public string CustomerCareId { get; set; }
        public string curruencyCode { get; set; }
        public IList<MainResoureViewModel> CustomerList { get; set; }
        public IList<MainResoureViewModel> ResoureceList { get; set; }
        public string SWIFT_Code { get; set; }
        public string IBAN_Number { get; set; }
        public bool RoleType { get; set; }
        public string Picture { get; set; }
        public int TempSalaryID { get; set; }
        public int LoginUserID { get; set; }
        //public int? CustomerCareID { get; set; }
        public string CustomerCare { get; set; }
        public IList<SelectListItem> TitleList { get; set; }
        public IList<SelectListItem> GenderList { get; set; }
        public IList<SelectListItem> LocationList { get; set; }
        public IList<SelectListItem> BusinessList { get; set; }
        public IList<SelectListItem> PoolList { get; set; }
        public IList<SelectListItem> FunctionList { get; set; }
        public IList<SelectListItem> NationalityList { get; set; }
        public IList<SelectListItem> DivisionList { get; set; }
        public IList<SelectListItem> CopyFromList { get; set; }
        public IList<SelectListItem> ReportstoList { get; set; }
        public IList<SelectListItem> CustomerCareList { get; set; }
        public IList<SelectListItem> AdditionalReportstoList { get; set; }
        public IList<SelectListItem> HRResponsibleList { get; set; }
        public IList<SelectListItem> JobTitleList { get; set; }
        public IList<SelectListItem> JobCountryList { get; set; }
        public IList<SelectListItem> CompanyList { get; set; }
        public IList<SelectListItem> CountryDropdown { get; set; }
        public IList<SelectListItem> StateDropdown { get; set; }
        public IList<SelectListItem> CityDropdown { get; set; }
        public IList<SelectListItem> ResourceTypeList { get; set; }
        public IList<SelectListItem> AirportList { get; set; }
        public IList<SelectListItem> NoticePeriodList { get; set; }
        public IList<SelectListItem> AssignList { get; set; }
        public IList<SelectListItem> StatusList { get; set; }
        public IList<SelectListItem> CountingList { get; set; }
        public IList<SelectListItem> CoustomerCompanyList { get; set; }
        public IList<MainResoureViewModel> AllListData { get; set; }
        public IList<SelectListItem> CurruencyCodeList { get; set; }
        public IList<SelectListItem> EmployeeCustomerCare { get; set; }
        //public int ApplicantID { get; set; }
        public int AlertBeforeDays { get; set; }
        public IList<AddNewTaskListViewModel> NewsTasklistRecord { get; set; }
        public string DueDate { get; set; }
        public string CheckRecord { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string Discriminator { get; set; }
        public Nullable<int> UserType { get; set; }
        public int CurrentUserId { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
        public string SelectCustomerCompanyId { get; set; }
        public int TaskId { get; set; }
        public int FullTime { get; set; }
        public int PartTime { get; set; }
        public int TemporaryTime { get; set; }
        public int VacancyId { get; set; }
        public int TotalCustomer { get; set; }
        public int TotalCoumpany { get; set; }
        public string JsonNewtaskList { get; set; }
        public int NameTitle { get; set; }
        //step 1
        public int Id { get; set; }
        public Nullable<int> Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string KnownAs { get; set; }
        public string UserNameEmail { get; set; }
        public string SSO { get; set; }
        public string IMAddress { get; set; }
        public Nullable<int> Gender { get; set; }
        public string DateOfBirth { get; set; }
        public Nullable<int> Nationality { get; set; }

        public string NationalityName { get; set; }
        public string NIN_SSN { get; set; }
        public string ImageURL { get; set; }
        public string PhotoPath { get; set; }
        public string ContinuousServiceDate { get; set; }

        //step 2

        //ResourceType Add in database
        public string StartDate { get; set; }
        public Nullable<int> ResourceType { get; set; }
        public string ResourceTypeName { get; set; }
        public int? Reportsto { get; set; }
        public string ReportToName { get; set; }
        public int? AdditionalReportsto { get; set; }
        public string AdditionalReportToName { get; set; }
        public int? HRResponsible { get; set; }
        public string HRResponsibleName { get; set; }
        public Nullable<int> JobTitle { get; set; }
        public string JobTitleName { get; set; }
        public Nullable<int> JobCountrID { get; set; }
        public Nullable<int> Location { get; set; }

        public string LocationName { get; set; }
        public Nullable<int> BusinessID { get; set; }
        public string BusinessName { get; set; }
        public Nullable<int> DivisionID { get; set; }
        public string DivisionName { get; set; }
        public Nullable<int> PoolID { get; set; }
        public string PoolName { get; set; }
        public Nullable<int> FunctionID { get; set; }
        public string FunctionName { get; set; }


        //Step 3
        public string ProbationEndDate { get; set; }
        public string NextProbationReviewDate { get; set; }
        public string FixedTermEndDate { get; set; }
        public Nullable<int> NoticePeriodID { get; set; }
        public Nullable<decimal> RecruitmentCost { get; set; }
        public string MethodofRecruitmentSetup { get; set; }
        public Nullable<decimal> HolidaysThisYear { get; set; }
        public Nullable<decimal> HolidaysNextYear { get; set; }
        public string IncludePublicHoliday { get; set; }
        public Nullable<int> HolidayEntit { get; set; }
        //  step 4

        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityyId { get; set; }
        public int AirportId { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public string WorkPhone { get; set; }
        public string WorkMobile { get; set; }
        public string PersonalPhone { get; set; }
        public string PersonalMobile { get; set; }
        public string PersonalEmail { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string AccountNumber { get; set; }
        public string OtherAccountInformation { get; set; }
        public string AccountName { get; set; }
        public string BankAddress { get; set; }

        //Other Record

        public string OverallScoreAverageScore { get; set; }
        public string CoreStrengthsAverageScore { get; set; }
        public string NumberofSkills { get; set; }
        public string NumberofSkillsEndorsed { get; set; }
        public string CountryofResidence { get; set; }
        public string LengthofService { get; set; }
        public string TotalSkill { get; set; }
        public string NoOfEndorsmntReceive { get; set; }
        public string ContryName { get; set; }
        public string copyName { get; set; }
        public string RepoempName { get; set; }
        public Nullable<int> RepoempId { get; set; }
        public string AddReName { get; set; }
        public Nullable<int> AddReId { get; set; }
        public Nullable<int> HrId { get; set; }

        public Nullable<int> CSEmployeeIDOfUser { get; set; }
        public Nullable<int> CSCustomerIDOfUser { get; set; }
        public Nullable<bool> Archived { get; set; }
        public Nullable<bool> EmailConfirmed { get; set; }
        public Nullable<int> JobRole { get; set; }
        public Nullable<int> Company { get; set; }

        public string CompanyName { get; set; }
        public Nullable<int> Code { get; set; }
        public Nullable<int> ApplicantID { get; set; }
        public Nullable<int> WorkPatternID { get; set; }
        public Nullable<decimal> Salary { get; set; }
        public Nullable<int> CurrencyID { get; set; }
        public Nullable<bool> HasLeft { get; set; }
        public Nullable<int> EducationID { get; set; }
        public Nullable<int> LanguageID { get; set; }
        public Nullable<int> QualificationID { get; set; }
        public Nullable<int> WorkExperienceID { get; set; }
        public Nullable<int> InculudedCarriedOver { get; set; }
        public Nullable<int> TOIL { get; set; }
        public Nullable<int> AuthorisorEmployeeID { get; set; }
        public Nullable<bool> EntitlementIncludesPublicHoliday { get; set; }
        public Nullable<bool> AutoApproveHolidays { get; set; }
        public Nullable<bool> ExceedAllowance { get; set; }
        public string CustomerCareName { get; set; }

    }

    public class AddNewTaskListViewModel
    {
        public AddNewTaskListViewModel()
        {
            AssignList = new List<SelectListItem>();
            StatusList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Assign { get; set; }
        public string DueDate { get; set; }
        public int Status { get; set; }
        public int AlertBeforeDays { get; set; }
        public IList<SelectListItem> StatusList { get; set; }
        public IList<SelectListItem> AssignList { get; set; }
        public int IdRecord { get; set; }
        public int IsTemp { get; set; }
    }

    public class AddSalaryViewModel
    {

        public AddSalaryViewModel()
        {
            SalaryTypeList = new List<SelectListItem>();
            PaymentFrequencyList = new List<SelectListItem>();
            CurrencyList = new List<SelectListItem>();
            ReasonforChangeList = new List<SelectListItem>();
            SalaryList = new List<AddSalaryViewModel>();
        }
        public int Id { get; set; }
        public int OriginalId { get; set; }
        public bool Tempmode { get; set; }
        public int? EmployeeId { get; set; }
        public int TableId { get; set; }
        public string EffectiveFrom { get; set; }
        public IList<SelectListItem> SalaryTypeList { get; set; }
        public int SalaryTypeID { get; set; }
        public IList<SelectListItem> PaymentFrequencyList { get; set; }
        public int PaymentFrequencyID { get; set; }
        public IList<SelectListItem> CurrencyList { get; set; }
        public int CurrencyID { get; set; }
        public string Amount { get; set; }
        public string TotalSalary { get; set; }
        public IList<SelectListItem> ReasonforChangeList { get; set; }
        public int ReasonforChange { get; set; }
        public string ReasonforChangeName { get; set; }
        public string Comments { get; set; }
        public IList<AddSalaryViewModel> SalaryList { get; set; }
        public int CurrentUserId { get; set; }
        public int curruencyCode { get; set; }
    }

    public class AddnewtaskRecordmodel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Assign { get; set; }
        public string DueDate { get; set; }
        public int Status { get; set; }
        public int AlertBeforeDays { get; set; }
    }

    public class HelpmecalculateviewModel
    {
        public HelpmecalculateviewModel()
        {
                
        }
        public int EmployeeID { get; set; }
        public string StartDate { get; set; }
        public int CountryId { get; set;}
        public string FullTimeEntitlement { get; set; }
        public string DaysPerWeek { get; set; }
        public string IncludePublicHolidays { get; set; }
    }

    public class HelpmeCalculeteModel 
    {
        public HelpmeCalculeteModel()
        {
                
        }

        public int totalWorkingDays { get; set; }
        public int remainiingDays { get; set; }
        public int TotalHolidayYear { get; set; }
        public int TotalRemainingHolidays { get; set; }
        public int ExpiredHolidays { get; set; }

    }

}