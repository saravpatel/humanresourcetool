using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRTool.CommanMethods;
namespace HRTool.Models.Resources
{
    public class EmployeeProjectPlannerViewModel
    {
        public EmployeeProjectPlannerViewModel()
        {
            CountryList = new List<SelectListItem>();
            WorkPatternList = new List<SelectListItem>();
            ProjectList = new List<SelectListItem>();
        }

        public int yearId { get; set; }
        public int currentMonth { get; set; }
        public int EmployeeId { get; set; }
        public Nullable<int> flag { get; set; }
        public IList<SelectListItem> CountryList { get; set; }
        public IList<SelectListItem> ProjectList { get; set; }
        public List<KeyValue> ListOfYear { get; set; }
        public int LastCountryId { get; set; }
        public string LoginUserName { get; set; }
        public IList<SelectListItem> WorkPatternList { get; set; }
        public int WorkPattern { get; set; }
        public Nullable<int> cflag { get; set; }

        public decimal ProjectbookDays { get; set; }
        public decimal ProjectRemainingDays { get; set; }

        public decimal TimeSheetbookDays { get; set; }
        public decimal TimeSheetRemainingDays { get; set; }

        public decimal UpliftbookDays { get; set; }
        public decimal UpliftRemainingDays { get; set; }

        public decimal TravelDays { get; set; }
        public decimal TravelWorkingDays { get; set; }

        public decimal ProjectDayWorkedInYear { get; set; }
        public decimal ProjectDayWorkedsinceYear { get; set; }
        public decimal ProjectDayPlanned { get; set; }
        public decimal ProjectTotalDays { get; set; }

        public decimal TravelDayInYear { get; set; }
        public decimal TravelDaySinceYear { get; set; }
        public decimal TravelDayPlanned { get; set; }
        public decimal TravelTotalDays { get; set; }

        public decimal UpliftDayInYear { get; set; }
        public decimal UpliftDaySinceYear { get; set; }
        public decimal UpliftTotalDays { get; set; }

        public decimal TimesheetApprovedIntheYear { get; set; }
        public decimal TimesheetApprovedSinceYear { get; set; }
        public decimal TimesheetAwaitingForApproval { get; set; }

    }

    public class EmployeeProjectPlannerYearViewModel
    {
        public EmployeeProjectPlannerYearViewModel()
        {
            MonthList = new List<EmployeeProjectPlannerMonthViewModel>();
        }
        public int yearId { get; set; }
        public IList<EmployeeProjectPlannerMonthViewModel> MonthList { get; set; }
    }

    public class EmployeeProjectPlannerMonthViewModel
    {
        public EmployeeProjectPlannerMonthViewModel()
        {
            DayList = new List<EmployeeProjectPlannerDayViewModel>();
        }
        public int monthId { get; set; }
        public int yearId { get; set; }
        public string MonthName { get; set; }
        public string TimesheetCount { get; set; }
        public decimal TravelCount { get; set; }

        public IList<EmployeeProjectPlannerDayViewModel> DayList { get; set; }
        public decimal SumAnnualLeave { get; set; }
        public int sumLateLeave { get; set; }
    }

    public class EmployeeProjectPlannerDayViewModel
    {
        public int day { get; set; }
        public string DayName { get; set; }
        public int yearId { get; set; }
        public int monthId { get; set; }
        public int TimeSheetId { get; set; }
        public int TravelLeaveId { get; set; }
        public int ScheduleId { get; set; }
        public int UpliftId { get; set; }
        public bool isLeave { get; set; }
        public bool isTimeSheetTaken { get; set; }
        public bool isTravelLeaveTaken { get; set; }
        public bool isSchedulingLeaveTaken { get; set; }
        public bool isWorkPatternLeaveTaken { get; set; }
        public bool isWorkPatternExist { get; set; }
        public bool isUpliftTaken { get; set; }
        public Nullable<int> Flag { get; set; }
    }

    public class EmployeeProjectPlanner_TimeSheetViewModel
    {
        public EmployeeProjectPlanner_TimeSheetViewModel()
        {
            DocumentList = new List<EmployeeProjectPlanner_TimeSheet_DocumentsViewModel>();
            DetailList = new List<EmployeeProjectPlanner_TimeSheet_DetailViewModel>();
        }
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Date { get; set; }
        public string Comment { get; set; }
        public int yearId { get; set; }
        public int monthId { get; set; }
        public int day { get; set; }
        public int HolidayCountryID { get; set; }
        public string jsonDocumentList { get; set; }
        public string jsonDetailList { get; set; }
        public IList<EmployeeProjectPlanner_TimeSheet_DocumentsViewModel> DocumentList { get; set; }
        public IList<EmployeeProjectPlanner_TimeSheet_DetailViewModel> DetailList { get; set; }
        public Nullable<int> Flag { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<int> timehseetDrillDown { get; set; }
        public Nullable<int> isMonth { get; set; }
        public Nullable<TimeSpan> totoalHrInMonth { get; set; }
        public Nullable<TimeSpan> totalHrOfWeek { get; set; }
        public Nullable<TimeSpan> totalHrToday { get; set; }
    }

    public class EmployeeProjectPlanner_TimeSheet_DetailViewModel
    {
        public EmployeeProjectPlanner_TimeSheet_DetailViewModel()
        {
            ProjectList = new List<SelectListItem>();
            CostCodeList = new List<SelectListItem>();
            CustomerList = new List<SelectListItem>();
            AssetList = new List<SelectListItem>();
            HoursList = new List<SelectListItem>();
            MinutesList = new List<SelectListItem>();
            ResourceList = new List<SelectListItem>();
            AllResourceCustomerList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public Nullable<int> TimeSheetId { get; set; }
        public Nullable<int> InTimeHr { get; set; }
        public Nullable<int> InTimeMin { get; set; }
        public Nullable<int> EndTimeHr { get; set; }
        public Nullable<int> EndTimeMin { get; set; }
        public IList<SelectListItem> HoursList { get; set; }
        public IList<SelectListItem> MinutesList { get; set; }
        public IList<SelectListItem> ProjectList { get; set; }
        public Nullable<int> Project { get; set; }
        public IList<SelectListItem> CostCodeList { get; set; }
        public Nullable<int> CostCode { get; set; }
        public IList<SelectListItem> CustomerList { get; set; }
        public IList<SelectListItem> ResourceList { get; set; }
        public IList<SelectListItem> AllResourceCustomerList { get; set; }
        public string Customer { get; set; }
        public string CustomerId { get; set; }
        public IList<SelectListItem> AssetList { get; set; }
        public Nullable<int> Asset { get; set; }
        public Nullable<int> FlagD { get; set; }

    }

    public class EmployeeProjectPlanner_TimeSheet_DocumentsViewModel
    {
        public int Id { get; set; }
        public int TravelLeaveId { get; set; }
        public string originalName { get; set; }
        public string newName { get; set; }
        public string description { get; set; }
    }

    public class EmployeeProjectPlanner_TravelLeaveViewModel
    {
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
        public Nullable<int> InTimeHr { get; set; }
        public Nullable<int> InTimeMin { get; set; }
        public Nullable<int> EndTimeHr { get; set; }
        public Nullable<int> EndTimeMin { get; set; }
        public Nullable<int> IsTravellDrillDown { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
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
        public string CustomerId { get; set; }
        public IList<SelectListItem> ProjectList { get; set; }
        public Nullable<int> Project { get; set; }
        public IList<SelectListItem> CostCodeList { get; set; }
        public Nullable<int> CostCode { get; set; }

        public IList<EmployeeProjectPlanner_TravelLeave_DocumentsViewModel> DocumentList { get; set; }
        public int yearId { get; set; }
        public int monthId { get; set; }
        public int day { get; set; }
        public int HolidayCountryID { get; set; }
        public string jsonDocumentList { get; set; }
        public Nullable<int> flag { get; set; }
        public Nullable<int> isMonth { get; set; }

    }

    public class EmployeeProjectPlanner_TravelLeave_DocumentsViewModel
    {
        public int Id { get; set; }
        public int TravelLeaveId { get; set; }
        public string originalName { get; set; }
        public string newName { get; set; }
        public string description { get; set; }
    }

    public class EmployeeProjectPlanner_Scheduling_DocumentsViewModel
    {
        public EmployeeProjectPlanner_Scheduling_DocumentsViewModel()
        {
            ProjectList = new List<SelectListItem>();
            CustomerList = new List<SelectListItem>();
            AssetList = new List<SelectListItem>();
            HoursList = new List<SelectListItem>();
            MinutesList = new List<SelectListItem>();
        }
        public IList<SelectListItem> HoursList { get; set; }
        public IList<SelectListItem> MinutesList { get; set; }
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public bool IsDayOrMore { get; set; }
        public bool IsLessThenADay { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Nullable<decimal> DurationDays { get; set; }

        public Nullable<int> InTimeHr { get; set; }
        public Nullable<int> InTimeMin { get; set; }
        public Nullable<int> EndTimeHr { get; set; }
        public Nullable<int> EndTimeMin { get; set; }

        public Nullable<decimal> DurationHr { get; set; }

        public IList<SelectListItem> ProjectList { get; set; }
        public Nullable<int> Project { get; set; }
        public IList<SelectListItem> CustomerList { get; set; }
        public string customerName { get; set; }
        public string Customer { get; set; }
        public string CustomerId { get; set; }
        public string EmployeeName { get; set; }
        public IList<SelectListItem> AssetList { get; set; }
        public Nullable<int> Asset { get; set; }
        public string Comments { get; set; }
        public int yearId { get; set; }
        public int monthId { get; set; }
        public int day { get; set; }
        public int HolidayCountryID { get; set; }
        public Nullable<int> isScheduling { get; set; }
        public Nullable<int> isMonth { get; set; }
        public Nullable<int> flag { get; set; }
        public Nullable<int> isWorkPatternExist { get; set; }
        public Nullable<int> isWorkPatternLeave { get; set; }
    }

    //Uplift
    public class EmployeeProjectPlanner_UpliftViewModel
    {
        public EmployeeProjectPlanner_UpliftViewModel()
        {
            DocumentList = new List<EmployeeProjectPlanner_Uplift_DocumentsViewModel>();
            DetailList = new List<EmployeeProjectPlanner_Uplift_DetailViewModel>();
            JobTitleList = new List<SelectListItem>();
            CustomerList = new List<SelectListItem>();
            ProjectList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Date { get; set; }
        public string Comment { get; set; }
        public int yearId { get; set; }
        public int monthId { get; set; }
        public int day { get; set; }
        public int HolidayCountryID { get; set; }
        public IList<SelectListItem> JobTitleList { get; set; }
        public IList<SelectListItem> CustomerList { get; set; }
        public IList<SelectListItem> ProjectList { get; set; }
        public Nullable<TimeSpan> totoalHrInMonth { get; set; }
        public Nullable<TimeSpan> totalHrOfWeek { get; set; }
        public Nullable<TimeSpan> totalHrToday { get; set; }
        public int UpliftPostionId { get; set; }
       public string Customer { get; set; }
        public string CustomerId { get; set; }
        public int ProjectId { get; set; }
        public Nullable<int> isUpliftDrillDown { get; set; }
        public string jsonDocumentList { get; set; }
        public string jsonDetailList { get; set; }
        public IList<EmployeeProjectPlanner_Uplift_DocumentsViewModel> DocumentList { get; set; }
        public IList<EmployeeProjectPlanner_Uplift_DetailViewModel> DetailList { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<int> FlagD { get; set; }
        public Nullable<int> isMonth { get; set; }
        public Nullable<int> isWorkPatternExist { get; set; }
        public Nullable<int> isWorkPatternLeave { get; set; }
    }

    public class EmployeeProjectPlanner_Uplift_DetailViewModel
    {
        public EmployeeProjectPlanner_Uplift_DetailViewModel()
        {
            HoursList = new List<SelectListItem>();
            MinutesList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public Nullable<int> UpliftId { get; set; }
        public Nullable<int> InTimeHr { get; set; }
        public Nullable<int> InTimeMin { get; set; }
        public Nullable<int> EndTimeHr { get; set; }
        public Nullable<int> EndTimeMin { get; set; }
        public IList<SelectListItem> HoursList { get; set; }
        public IList<SelectListItem> MinutesList { get; set; }
        public decimal WorkerRate { get; set; }
        public decimal WorkerRatePer { get; set; }
        public decimal CustomerRate { get; set; }
        public decimal CustomerRatePer { get; set; }
        public Nullable<int> FlagD { get; set; }
    }

    public class EmployeeProjectPlanner_Uplift_DocumentsViewModel
    {
        public int Id { get; set; }
        public int UpliftId { get; set; }
        public string originalName { get; set; }
        public string newName { get; set; }
        public string description { get; set; }
    }
    public class ResourcesAsminProjectPlannerViewModel
    {
        public ResourcesAsminProjectPlannerViewModel()
        {
            GetAllList = new List<ResourcesAsminProjectPlannerViewModel>();
        }
        public int EmployeeId { get; set; }
        public int travelId { get; set; }
        public string Resource_Name_SSO { get; set; }
        public string jobtitle { get; set; }
        public string Days { get; set; }
        public string Business { get; set; }
        public string Division { get; set; }
        public string Pool { get; set; }
        public string Project { get; set; }
        public string Customer { get; set; }
        public string AssetName{get;set;}
        public string CostCode { get; set; }
        public string tavelType { get; set; }
        public bool IsSchdule { get; set; }
        public bool IsTravel { get; set; }
        public bool IsTimeshhet { get; set; }
        public bool IsUplift { get; set; }
        public string AssetType { get; set; }
        public string Status { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string FunctionId { get; set; }
        public Nullable<TimeSpan> Duration { get; set; }
        public string DurationHR { get; set; }
        public Nullable<int> isMonth { get; set; }
        public IList<ResourcesAsminProjectPlannerViewModel> GetAllList { get; set; }                                
    }

}