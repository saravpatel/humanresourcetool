
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Web.Mvc;

namespace HRTool.Models.BulkActions
{
    public class AllDataList
    {
        public AllDataList()
        {
            BusinesList = new List<SelectListItem>();
            DivisionList = new List<SelectListItem>();
            PoolList = new List<SelectListItem>();
            FunctionList = new List<SelectListItem>();
            JobTitleList = new List<SelectListItem>();
            GetCountryList = new List<SelectListItem>();
        }
        public int EmpId { get; set; }
        public string EmployeeName { get; set; }
        public int busId { get; set; }
        public string BusinessName { get; set; }
        public int divId { get; set; }
        public string DivisionName { get; set; }
        public string poolId { get; set; }
        public string PoolName { get; set; }
        public string FunctionName { get; set; }
        public int jobID { get; set; }
        public string JobTitle { get; set; }
        public int countryId { get; set; }
        public string CountryOfR { get; set; }
        public int SchadulId { get; set; }
        public int TimesheetId { get; set; }
        public int TravelId { get; set; }
        public int BenifitId { get; set; }
        public int ActivityTypeId { get; set; }
        public int SalaryId { get; set; }
        public int AccessSetupId { get; set; }
        public int HolidayEntitlementsThisYearId { get; set; }
        public int HolidayEntitlementsNextYearId { get; set; }
        public int TrainingId { get; set; }
        public int SendCVResumeId { get; set; }
        public int HolidayAccrualsId { get; set; }
        public int BookHolidayId { get; set; }
        public int ResourceSettingsId { get; set; }
        public int UploadResourcesId { get; set; }
        public IList<SelectListItem> JobTitleList { get; set; }
        public IList<SelectListItem> BusinesList { get; set; }
        public IList<SelectListItem> DivisionList { get; set; }
        public IList<SelectListItem> PoolList { get; set; }
        public IList<SelectListItem> FunctionList { get; set; }
        public IList<SelectListItem> GetCountryList { get; set; }

    }
    public class EmployeeHolidayEntitNextYear
    {
        //     public string id { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public Nullable<int> current { get; set; }
        public int newVal { get; set; }

    }
    public class employeeReqNextYear
    {
        public string EmpId { get; set; }
        public string Year { get; set; }
    }
    public class EmployeeHolidayEntitlementsThisYear
    {
   //     public string id { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public int current { get; set; }
        public int newVal { get; set;}

    }
    public class employeeRequestThisYear
    {
        public string EmpId { get; set; }
        public string Year { get; set; }
    }
    public class EmployeeProjectPlanner_Scheduling_DocumnetsViewModel
    {
        public EmployeeProjectPlanner_Scheduling_DocumnetsViewModel()
        {
            ProjectList = new List<SelectListItem>();
            CustomerList = new List<SelectListItem>();
            AssetList = new List<SelectListItem>();
            HoursList = new List<SelectListItem>();
            MinutesList = new List<SelectListItem>();
        }
        public string EmpId { get; set; }
        public int Id { get; set; }
        public bool IsDayOrMore { get; set; }
        public Nullable<bool> IsLessThenADay { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Nullable<decimal> Duration { get; set; }
        public Nullable<int> InTimeHr { get; set; }
        public Nullable<int> InTimeMin { get; set; }
        public Nullable<int> EndTimeHr { get; set; }
        public Nullable<int> EndTimeMin { get; set; }
        public IList<SelectListItem> HoursList { get; set; }
        public IList<SelectListItem> MinutesList { get; set; }
        public Nullable<decimal> DurationHr { get; set; }
        public string duration { get; set; }
        public IList<SelectListItem> ProjectList { get; set; }
        public Nullable<int> Project { get; set; }
        public IList<SelectListItem> CustomerList { get; set; }
        public Nullable<int> Customer { get; set; }
        public IList<SelectListItem> AssetList { get; set; }
        public Nullable<int> Asset { get; set; }
        public string Comments { get; set; }
        public int yearId { get; set; }
        public int monthId { get; set; }
        public int day { get; set; }
    }
    public class SaveSchedulingData
    {
        public bool isLessDay { get; set; }
        public string EmpId { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string duration { get; set; }
        public int customer { get; set; }
        public int project { get; set; }
        public int asset { get; set; }
        public string comment { get; set; }
    }
    public class EmployeeProjectPlanner_TravelLeave_DocumnetsViewModel
    {
        public int Id { get; set; }
        public int TravelLeaveId { get; set; }
        public string originalName { get; set; }
        public string newName { get; set; }
        public string description { get; set; }
    }
    public class EmployeeProjectPlanner_TravelLeaveViewModel
    {            
        public string EmployeeId { get; set; }
        public Nullable<int> InTimeHr { get; set; }
        public Nullable<int> InTimeMin { get; set; }
        public Nullable<int> EndTimeHr { get; set; }
        public Nullable<int> EndTimeMin { get; set; }
        public EmployeeProjectPlanner_TravelLeaveViewModel()
        {
            DocumentList = new List<EmployeeProjectPlanner_TravelLeave_DocumentsViewModel>();
            FromCountryList = new List<SelectListItem>();
            FromStateList = new List<SelectListItem>();
            FromCityList = new List<SelectListItem>();
            FromAirportList = new List<SelectListItem>();
            ToCountryList = new List<SelectListItem>();
            ToStateList = new List<SelectListItem>();
            ToCityList = new List<SelectListItem>();
            ToAirportList = new List<SelectListItem>();
            ReasonForTravelList = new List<SelectListItem>();
            TravelTypeList = new List<SelectListItem>();
            CostCodeList = new List<SelectListItem>();
            ProjectList = new List<SelectListItem>();
            CustomerList = new List<SelectListItem>();
            HoursList = new List<SelectListItem>();
            MinutesList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public IList<SelectListItem> HoursList { get; set; }
        public IList<SelectListItem> MinutesList { get; set; }
        public Nullable<decimal> DurationHr { get; set; }
        public IList<SelectListItem> FromCountryList { get; set; }
        public IList<SelectListItem> FromStateList { get; set; }
        public IList<SelectListItem> FromCityList { get; set; }
        public IList<SelectListItem> FromAirportList { get; set; }
        public Nullable<int> FromCountryId { get; set; }
        public Nullable<int> FromStateId { get; set; }
        public Nullable<int> FromTownId { get; set; }
        public Nullable<int> FromAirportId { get; set; }

        public IList<SelectListItem> ToCountryList { get; set; }
        public IList<SelectListItem> ToStateList { get; set; }
        public IList<SelectListItem> ToCityList { get; set; }
        public IList<SelectListItem> ToAirportList { get; set; }
        public Nullable<int> ToCountryId { get; set; }
        public Nullable<int> ToStateId { get; set; }
        public Nullable<int> ToTownId { get; set; }
        public Nullable<int> ToAirportId { get; set; }

        public IList<SelectListItem> ReasonForTravelList { get; set; }
        public Nullable<int> ReasonForTravelId { get; set; }
        public Nullable<bool> IsLessThenADay { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Nullable<decimal> Duration { get; set; }
        public Nullable<int> Hour { get; set; }
        public Nullable<int> Min { get; set; }
        public string Comment { get; set; }
        public IList<SelectListItem> TravelTypeList { get; set; }
        public Nullable<int> Type { get; set; }
        public IList<SelectListItem> CustomerList { get; set; }
        public string Customer { get; set; }
        public IList<SelectListItem> ProjectList { get; set; }
        public Nullable<int> Project { get; set; }
        public IList<SelectListItem> CostCodeList { get; set; }
        public Nullable<int> CostCode { get; set; }

        public IList<EmployeeProjectPlanner_TravelLeave_DocumentsViewModel> DocumentList { get; set; }
        public int yearId { get; set; }
        public int monthId { get; set; }
        public int day { get; set; }
        public string jsonDocumentList { get; set; }
    }
    public class EmployeeProjectPlanner_TravelLeave_DocumentsViewModel
    {
        public int Id { get; set; }
        public int TravelLeaveId { get; set; }
        public string originalName { get; set; }
        public string newName { get; set; }
        public string description { get; set; }
    }
    public class EmployeePlanner_TimeSheetViewModel
    {
        public EmployeePlanner_TimeSheetViewModel()
        {
            DocumentList = new List<EmployeePlanner_TimeSheet_DocumentsViewModel>();
            DetailList = new List<EmployeePlanner_TimeSheet_DetailViewModel>();
        }
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string Date { get; set; }
        public string Comment { get; set; }
        public int yearId { get; set; }
        public int monthId { get; set; }
        public int day { get; set; }
        public string jsonDocumentList { get; set; }
        public string jsonDetailList { get; set; }
        public IList<EmployeePlanner_TimeSheet_DocumentsViewModel> DocumentList { get; set; }
        public IList<EmployeePlanner_TimeSheet_DetailViewModel> DetailList { get; set; }
        public TimeSpan totoalHrInMonth { get; set; }
        public TimeSpan totalHrOfWeek { get; set; }
    }
    public class EmployeePlanner_TimeSheet_DetailViewModel
    {
        public EmployeePlanner_TimeSheet_DetailViewModel()
        {
            ProjectList = new List<SelectListItem>();
            CostCodeList = new List<SelectListItem>();
            CustomerList = new List<SelectListItem>();
            AssetList = new List<SelectListItem>();
            HoursList = new List<SelectListItem>();
            MinutesList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public Nullable<int> TimeSheetId { get; set; }
        public Nullable<int> InTimeHr { get; set; }
        public Nullable<int> InTimeMin { get; set; }
        public Nullable<int> EndTimeHr { get; set; }
        public Nullable<int> EndTimeMin { get; set; }
        public IList<SelectListItem> HoursList { get; set; }
        public IList<SelectListItem> MinutesList { get; set; }
        public Nullable<decimal> DurationHr { get; set; }
        public IList<SelectListItem> ProjectList { get; set; }
        public Nullable<int> Project { get; set; }
        public IList<SelectListItem> CostCodeList { get; set; }
        public Nullable<int> CostCode { get; set; }
        public IList<SelectListItem> CustomerList { get; set; }
        public string Customer { get; set; }
        public IList<SelectListItem> AssetList { get; set; }
        public Nullable<int> Asset { get; set; }

    }

    public class EmployeePlanner_TimeSheet_DocumentsViewModel
    {
        public int Id { get; set; }
        public int TravelLeaveId { get; set; }
        public string originalName { get; set; }
        public string newName { get; set; }
        public string description { get; set; }
    }
    public class BenefitsViewModel
    {
        public BenefitsViewModel()
        {
            BenefitList = new List<SelectListItem>();
            BenefitDocumentList = new List<BenefitsDocumentViewModel>();
            CurrencyList = new List<SelectListItem>();
        }

        public int Id { get; set; }
        public string EmployeeID { get; set; }
        public int BenefitID { get; set; }

        public int Currency { get; set; }
        public string BenefitValue { get; set; }
        public string DateAwarded { get; set; }
        public string ExpiryDate { get; set; }
        public decimal FixedAmount { get; set; }
        public bool RecoverOnTermination { get; set; }
        public string Comments { get; set; }
        public string StatusValue { get; set; }
        public IList<SelectListItem> BenefitList { get; set; }
        public IList<BenefitsDocumentViewModel> BenefitDocumentList { get; set; }
        public IList<SelectListItem> CurrencyList { get; set; }
        public string DocumentListString { get; set; }
    }

 
    public class BenefitsDocumentViewModel
    {
        public int Id { get; set; }
        public int BenifitId { get; set; }
        public string originalName { get; set; }
        public string newName { get; set; }
        public string description { get; set; }
    }
    public class AddSalaryViewModel
    {
        public string SelectedFromCurrency { get; set; }
        public string SelectedToCurrency { get; set; }
        
        public double ToAmount { get; set; }
        public Nullable<decimal> FixedRate { get; set; }
        public Nullable<decimal> LiveRate { get; set; }
        public AddSalaryViewModel()
        {
            SalaryTypeList = new List<SelectListItem>();
            PaymentFrequencyList = new List<SelectListItem>();
            CurrencyList = new List<SelectListItem>();
            ReasonforChangeList = new List<SelectListItem>();
            SalaryList = new List<AddSalaryViewModel>();
        }
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string EffectiveFrom { get; set; }
        public IList<SelectListItem> SalaryTypeList { get; set; }
        public int SalaryTypeID { get; set; }
        public IList<SelectListItem> PaymentFrequencyList { get; set; }
        public int PaymentFrequencyID { get; set; }
        public IList<SelectListItem> CurrencyList { get; set; }
        public int CurrencyID { get; set; }
        public Double Amount { get; set; }
        public string TotalSalary { get; set; }
        public IList<SelectListItem> ReasonforChangeList { get; set; }
        public int ReasonforChange { get; set; }
        public string ReasonforChangeName { get; set; }
        public string Comments { get; set; }
        public IList<AddSalaryViewModel> SalaryList { get; set; }
        public bool Tempmode { get; set; }
        public string CurrentUserId { get; set; }
        public int TableId { get; set; }

    }
    public class CurrencyConverterModel
    {

        public CurrencyConverterModel()
        {
            BindCurrencyList = new List<SelectListItem>();
            CurrencyList = new List<CurrencyConverterModel>();
        }
        public IList<SelectListItem> BindCurrencyList { get; set; }
        public string SelectedFromCurrency { get; set; }
        public string SelectedToCurrency { get; set; }
        public double FromAmount { get; set; }
        public double ToAmount { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Nullable<decimal> FixedRate { get; set; }
        public Nullable<decimal> LiveRate { get; set; }
        public Nullable<System.DateTime> FreezingDate { get; set; }
        public IList<CurrencyConverterModel> CurrencyList { get; set; }

        public bool IsFixed { get; set; }

    }

    public class HolidayViewModel
    {
        public HolidayViewModel()
        {
            TimeList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public string EmployeeId { get; set; }

        public string startDate { get; set; }
        public string endDate { get; set; }

        //public bool TOIL { get; set; }
        public decimal Duration { get; set; }
        public bool IsLessThenADay { get; set; }
        public string comment { get; set; }
        public string PartOfDay { get; set; }
        public IList<SelectListItem> TimeList { get; set; }
    }
    public class AddSalaryDeductionViewModel
    {

        public AddSalaryDeductionViewModel()
        {
            SalaryTypeList = new List<SelectListItem>();
            //SalaryDeductionList = new List<AddSalaryDeductionViewModel>();
        }
        public int Id { get; set; }
        public string EmployeeID { get; set; }
        public int EmployeeSalaryID { get; set; }
        public string TotalSalary { get; set; }
        public string SalaryType { get; set; }
        public int DeductionID { get; set; }
        public string Deduction { get; set; }
        public decimal FixedAmount { get; set; }
        public decimal PercentOfSalary { get; set; }
        public bool IncludeInSalary { get; set; }
        public string Comments { get; set; }

        //Salary Details
        public string EffectiveFrom { get; set; }
        public int SalaryTypeID { get; set; }
        public int PaymentFrequencyID { get; set; }
        public string Amount { get; set; }
        public int CurrencyID { get; set; }
        public int ReasonforChange { get; set; }
        public string SalaryComments { get; set; }

        public IList<SelectListItem> SalaryTypeList { get; set; }
        // public IList<AddSalaryDeductionViewModel> SalaryDeductionList { get; set; }
    }

    public class AddSalaryDeductionViewModelTemp
    {

        public AddSalaryDeductionViewModelTemp()
        {
            SalaryTypeList = new List<SelectListItem>();
            //SalaryDeductionList = new List<AddSalaryDeductionViewModel>();
        }
        public int Id { get; set; }
        public int DeductionID { get; set; }
        public string Deduction { get; set; }
        public decimal FixedAmount { get; set; }
        public decimal PercentOfSalary { get; set; }
        public bool IncludeInSalary { get; set; }
        public string Comments { get; set; }

        //Salary Details

        public string EmployeeID { get; set; }
        public int EmployeeSalaryID { get; set; }
        public string EffectiveFrom { get; set; }
        public int SalaryTypeID { get; set; }
        public string SalaryType { get; set; }
        public int PaymentFrequencyID { get; set; }
        public string Amount { get; set; }
        public int CurrencyID { get; set; }
        public int ReasonforChange { get; set; }
        public string TotalSalary { get; set; }
        public string SalaryComments { get; set; }
        public IList<SelectListItem> SalaryTypeList { get; set; }
        // public IList<AddSalaryDeductionViewModel> SalaryDeductionList { get; set; }
    }

    public class AddSalaryEntitlementViewModel
    {
        public AddSalaryEntitlementViewModel()
        {
            SalaryTypeList = new List<SelectListItem>();
            // SalaryEntitlementList = new List<AddSalaryEntitlementViewModel>();
        }
        public int Id { get; set; }
        public string EmployeeID { get; set; }
        public int EmployeeSalaryID { get; set; }
        public int EntitlementID { get; set; }
        public string SalaryType { get; set; }
        public string TotalSalary { get; set; }
        public string Entitlement { get; set; }
        public decimal FixedAmount { get; set; }
        public decimal PercentOfSalary { get; set; }

        public bool IncludeInSalary { get; set; }

        public string Comments { get; set; }
        public IList<SelectListItem> SalaryTypeList { get; set; }
        // public IList<AddSalaryEntitlementViewModel> SalaryEntitlementList { get; set; }
    }

    public class AddResourceBulk
    {
        public AddResourceBulk()
        {
            userList = new List<AddResourceBulk>();
            emptyuserList = new List<AddResourceBulk>();
            alreadyExistUserList = new List<AddResourceBulk>();
        }
        public int CreateEmpId { get; set; }
        public string FilePath { get; set; }
        public string FileOriginalPath { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public string UserName { get; set; }
        public int Gender { get; set; }
        public int Title { get; set; }
        public Nullable<int> NameTitle { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }       
        public string SSOID { get; set; }        
        
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        
        public Nullable<System.DateTime> StartDate { get; set; }       
        
        public string ResourceSheetFormPath { get; set; }

        public IList<AddResourceBulk> userList { get; set; }
        public IList<AddResourceBulk> emptyuserList { get; set; }
        public IList<AddResourceBulk> alreadyExistUserList { get; set; }

    }
    public class AddbulknewtaskRecordmodel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Assign { get; set; }
        public string DueDate { get; set; }
        public int Status { get; set; }
        public int AlertBeforeDays { get; set; }
    }

    public class SendCVResumeToCustomer
    {
        public SendCVResumeToCustomer()
        {
            CustomerList = new List<SelectListItem>();
            //CustomerEmailList = new List<SelectListItem>();
        }
        public string EmpId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerMail { get; set; }
        
        public IList<SelectListItem> CustomerList { get; set; }
        //public IList<SelectListItem> CustomerEmailList { get; set; }
        public string CustomerEmail { get; set; }

    }
    //public class EmployeeResumePDFViewModel
    //{
    //    public EmployeeResumePDFViewModel()
    //    {
    //        AllDetails = new EmployeeResumeViewModel();
    //    }
    //    public int EmployeeID { get; set; }
    //    public string PhotoPath { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string JobTitle { get; set; }
    //    public string Address { get; set; }
    //    public string PersonalPhone { get; set; }
    //    public string PersonalEmail { get; set; }
    //    public string Email { get; set; }
    //    public string ResumeText { get; set; }
    //    public EmployeeResumeViewModel AllDetails { get; set; }

    //}

    public class BulkEmployeeSetting
    {
        public BulkEmployeeSetting()
        {
            CompanyList = new List<SelectListItem>();
            Location = new List<SelectListItem>();
            NoticePeriod = new List<SelectListItem>();
            ResourceType = new List<SelectListItem>();
            ReportstoList = new List<SelectListItem>();
            AdditionalReportstoList = new List<SelectListItem>();
            HRResponsibleList = new List<SelectListItem>();
            JobTitle = new List<SelectListItem>();
            BusinessList = new List<SelectListItem>();
            DivisionList = new List<SelectListItem>();
            PoolList = new List<SelectListItem>();
            FunctionList = new List<SelectListItem>();
        }
        public string EmpId { get; set; }
        public IList<SelectListItem> FunctionList { get; set; }
        public IList<SelectListItem> PoolList { get; set; }
        public IList<SelectListItem> BusinessList { get; set; }
        public IList<SelectListItem> DivisionList { get; set; }
        public IList<SelectListItem> CompanyList { get; set; }
        public IList<SelectListItem> Location { get; set; }
        public IList<SelectListItem> NoticePeriod { get; set; }
        public IList<SelectListItem> ResourceType { get; set; }
        public IList<SelectListItem> ReportstoList { get; set; }
        public IList<SelectListItem> AdditionalReportstoList { get; set; }
        public IList<SelectListItem> HRResponsibleList { get; set; }
        public IList<SelectListItem> JobTitle { get; set; }

        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> LocationId { get; set; }
        public Nullable<int> BusinessId { get; set; }
        public Nullable<int> DivisonId { get; set; }
        public Nullable<int> PoolId { get; set; }
        public Nullable<int> FunctionId { get; set; }
        public Nullable<int> NoticePeriodId { get; set; }
        public Nullable<int> ResourceTypeId { get; set; }
        public Nullable<int> ReportId { get; set; }
        public Nullable<int> AdditinalReportId { get; set; }
        public Nullable<int> HRRepoId { get; set; }
        public Nullable<int> JobTitleId { get; set; }

    }

    public class BulkAccessSetup
    {
        public BulkAccessSetup()
        {
            AspnetUsersRoleList = new List<SelectListItem>();
        }
        public string EmployeeId { get; set; }
        public int AspnetRoleId{get;set;}
        public IList<SelectListItem> AspnetUsersRoleList { get; set; } 
    }

}




