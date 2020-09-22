using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Resources
{
    public class EmployeePlannerViewModel
    {
        public EmployeePlannerViewModel()
        {
            CountryList = new List<SelectListItem>();
            WorkPatternList = new List<SelectListItem>();
        }
        public int yearId { get; set; }
        public int currentMonth { get; set; }
        public int EmployeeId { get; set; }
        public string LoginUserName { get; set; }
        public IList<SelectListItem> CountryList { get; set; }
        public int LastCountryId { get; set; }
        public Nullable<int> flag { get; set; }
        public IList<SelectListItem> WorkPatternList { get; set; }
        public int WorkPattern { get; set; }

        // Graph Holiday
        public decimal bookDays { get; set; }
        public decimal RemainingDays { get; set; }
        // Graph Sickleave
        public decimal SickLeavesDays { get; set; }
        public decimal WorkingDays { get; set; }
        //Bradford Factor
        public decimal Point { get; set; }

        public decimal MondayDays { get; set; }
        public decimal TuesdayDays { get; set; }
        public decimal WednessdayDays { get; set; }
        public decimal ThursdayDays { get; set; }
        public decimal FridayDays { get; set; }
        public decimal SaturdayDays { get; set; }
        public decimal SundayDays { get; set; }

        public int LowerValue1 { get; set; }

        public int UpperValue1 { get; set; }
        public int LowerValue2 { get; set; }

        public int UpperValue2 { get; set; }
        public int LowerValue3 { get; set; }

        public int UpperValue3 { get; set; }
        public int LowerValue4 { get; set; }

        public int UpperValue4 { get; set; }

        public int minValue { get; set; }
        public int maxValue { get; set; }
        public decimal BradfordFactor { get; set; }

        public string HolidayYear { get; set; }
        //Sick Leave Hit Map
        public int Sunday { get; set; }
        public int Monday { get; set; }
        public int Tuesday { get; set; }
        public int Wednesday { get; set; }
        public int Thursday { get; set; }
        public int Friday { get; set; }
        public int Saturday { get; set; }

    }

    public class EmployeePlannerYearViewModel
    {
        public EmployeePlannerYearViewModel()
        {
            MonthList = new List<EmployeePlannerMonthViewModel>();
        }
        public int yearId { get; set; }
        public IList<EmployeePlannerMonthViewModel> MonthList { get; set; }
    }

    public class EmployeePlannerMonthViewModel
    {
        public EmployeePlannerMonthViewModel()
        {
            DayList = new List<EmployeePlannerDayViewModel>();
        }
        public int monthId { get; set; }
        public int yearId { get; set; }
        public string MonthName { get; set; }
        public IList<EmployeePlannerDayViewModel> DayList { get; set; }
        public decimal SumAnnualLeave { get; set; }
        public decimal sumLateLeave { get; set; }

        public string TimesheetCount { get; set; }
        public decimal TravelCount { get; set; }
        // public int AnnualLeaveCount { get; set; }
        public decimal OtherLeaveCount { get; set; }
        public decimal SickLeaveCount { get; set; }
        // public int LateCount { get; set; }
        public string PublicHolidaysCount { get; set; }
        public decimal MaternityorPaternity { get; set; }
    }

    public class EmployeePlannerDayViewModel
    {
        public int day { get; set; }
        public string DayName { get; set; }
        public int yearId { get; set; }
        public int monthId { get; set; }
        public int TimeSheetId { get; set; }
        public int AnnualLeaveId { get; set; }
        public int LateLeaveId { get; set; }
        public int OtherLeaveId { get; set; }
        public int TravelLeaveId { get; set; }
        public int PublicholidayId { get; set; }
        public int SickLeaveId { get; set; }
        public int MaternityLeaveId { get; set; }
        public bool isLeave { get; set; }
        public bool isAnnualLeaveTaken { get; set; }
        public bool isLateLeaveTaken { get; set; }
        public bool isOtherLeaveTaken { get; set; }
        public bool isTravelLeaveTaken { get; set; }
        public bool isTimeSheetTaken { get; set; }
        public bool isPublicHoliday { get; set; }
        public bool isSickLeaveTaken { get; set; }
        public bool isWorkPatternLeaveTaken { get; set; }
        public bool isMaternityPaternityLeaveTaken { get; set; }
        public Nullable<int> Flag { get; set; }
    }

    public class EmployeePlanner_AnnualLeaveViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Nullable<bool> IsLessThenADay { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Nullable<decimal> Duration { get; set; }
        public string Comment { get; set; }
        public Nullable<bool> TOIL { get; set; }
        public Nullable<int> PartOfTheDay { get; set; }
        public int yearId { get; set; }
        public int monthId { get; set; }
        public int day { get; set; }
        public int HolidayCountry { get; set; }
    }

    public class EmployeePlanner_LateLeaveViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string LateDate { get; set; }
        public string Comment { get; set; }
        public Nullable<int> LateHr { get; set; }
        public Nullable<int> LateMin { get; set; }
        public int yearId { get; set; }
        public int monthId { get; set; }
        public int day { get; set; }
        public int HolidayCountryID { get; set; }
        public Nullable<int> InTimeHr { get; set; }
        public Nullable<int> InTimeMin { get; set; }
        public IList<SelectListItem> HoursList { get; set; }
        public IList<SelectListItem> MinutesList { get; set; }
        public EmployeePlanner_LateLeaveViewModel()
        {
            HoursList = new List<SelectListItem>();
            MinutesList = new List<SelectListItem>();
        }

    }

    public class EmployeePlanner_OtherLeaveViewModel
    {
        public EmployeePlanner_OtherLeaveViewModel()
        {
            ReasonForLeaveList = new List<SelectListItem>();
            DocumentList = new List<EmployeePlanner_OtherLeave_DocumentsViewModel>();
            HoursList = new List<SelectListItem>();
            MinutesList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Nullable<bool> IsLessThenADay { get; set; }
        public Nullable<int> ReasonForLeaveId { get; set; }
        public String ReasonForLeave { get; set; }
        public IList<SelectListItem> ReasonForLeaveList { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Nullable<decimal> Duration { get; set; }
        public string Comment { get; set; }
        public Nullable<int> PartOfTheDay { get; set; }
        public int Hour { get; set; }
        public int Min { get; set; }
        public Nullable<int> InTimeHr { get; set; }
        public Nullable<int> InTimeMin { get; set; }
        public IList<SelectListItem> HoursList { get; set; }
        public IList<SelectListItem> MinutesList { get; set; }
        public IList<EmployeePlanner_OtherLeave_DocumentsViewModel> DocumentList { get; set; }
        public int yearId { get; set; }
        public int monthId { get; set; }
        public int day { get; set; }
        public int HolidayCountryID { get; set; }
        public string jsonDocumentList { get; set; }
    }

    public class EmployeePlanner_OtherLeave_DocumentsViewModel
    {
        public int Id { get; set; }
        public int OtherLeaveId { get; set; }
        public string originalName { get; set; }
        public string newName { get; set; }
        public string description { get; set; }
    }

    public class EmployeePlanner_TravelLeaveViewModel
    {
        public IList<SelectListItem> HoursList { get; set; }
        public IList<SelectListItem> MinutesList { get; set; }
        public Nullable<decimal> DurationHr { get; set; }
        public Nullable<int> InTimeHr { get; set; }
        public Nullable<int> InTimeMin { get; set; }
        public Nullable<int> EndTimeHr { get; set; }
        public Nullable<int> EndTimeMin { get; set; }
        public EmployeePlanner_TravelLeaveViewModel()
        {
            DocumentList = new List<EmployeePlanner_TravelLeave_DocumentsViewModel>();
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
        public int EmployeeId { get; set; }
        public Nullable<int> IsTravellDrillDown { get; set; }
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
        public string EmployeeName { get; set; }
        public IList<EmployeePlanner_TravelLeave_DocumentsViewModel> DocumentList { get; set; }
        public int yearId { get; set; }
        public int monthId { get; set; }
        public int day { get; set; }
        public int HolidayCountryID { get; set; }
        public string jsonDocumentList { get; set; }
        public Nullable<int> flag { get; set; }
        public Nullable<int> isMonth { get; set; }
    }

    public class EmployeePlanner_TravelLeave_DocumentsViewModel
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
        public int EmployeeId { get; set; }
        public string Date { get; set; }
        public string Comment { get; set; }
        public int yearId { get; set; }
        public int monthId { get; set; }
        public int day { get; set; }
        public int HolidayCountryID { get; set; }
        public string jsonDocumentList { get; set; }
        public string jsonDetailList { get; set; }
        public Nullable<int> Flag { get; set; }
        public Nullable<int> timehseetDrillDown { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<int> isMonth { get; set; }
        public Nullable<TimeSpan> totoalHrInMonth { get; set; }
        public Nullable<TimeSpan> totalHrOfWeek { get; set; }
        public Nullable<TimeSpan> totalHrToday { get; set; }
        public IList<EmployeePlanner_TimeSheet_DocumentsViewModel> DocumentList { get; set; }
        public IList<EmployeePlanner_TimeSheet_DetailViewModel> DetailList { get; set; }
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
        public IList<SelectListItem> ProjectList { get; set; }
        public Nullable<int> Project { get; set; }
        public IList<SelectListItem> CostCodeList { get; set; }
        public Nullable<int> CostCode { get; set; }
        public IList<SelectListItem> CustomerList { get; set; }
        public string Customer { get; set; }
        public string CustomerId { get; set; }
        public Nullable<int> FlagD { get; set; }
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


    public class EmployeePlanner_publicHolidayCounty
    {
        public EmployeePlanner_publicHolidayCounty()
        {
            holidayList = new List<EmployeePlanner_publicHoliday_CountyWiseViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public String EffavtiveFromDate { get; set; }
        public IList<EmployeePlanner_publicHoliday_CountyWiseViewModel> holidayList { get; set; }
    }

    public class EmployeePlanner_publicHoliday_CountyWiseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public int countryId { get; set; }
    }

    //Sick Leave
    public class EmployeePlanner_SickLeaves
    {
        public EmployeePlanner_SickLeaves()
        {
            CommentList = new List<SickLeavesCommentViewModel>();
            DocumentList = new List<SickLeavesDocumentViewModel>();
            ReasonSickLeaveList = new List<SelectListItem>();
            //HoursList = new List<SelectListItem>();
            //MinutesList = new List<SelectListItem>();
            TimeList = new List<SelectListItem>();
        }
        public int day { get; set; }
        public int yearId { get; set; }
        public int monthId { get; set; }
        public int HolidayCountryID { get; set; }
        // public IList<SelectListItem> HoursList { get; set; }
        //  public IList<SelectListItem> MinutesList { get; set; }
        public IList<SelectListItem> TimeList { get; set; }
        public IList<SelectListItem> ReasonSickLeaveList { get; set; }
        public IList<SickLeavesCommentViewModel> CommentList { get; set; }
        public IList<SickLeavesDocumentViewModel> DocumentList { get; set; }

        //Step 1
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public bool IsADayOrMore { get; set; }
        public bool IsHalfDay { get; set; }
        public bool IsHours { get; set; }
        public int Reason { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal DurationDays { get; set; }

        // public string InTimeHrSD { get; set; }
        // public string InTimeMinSD { get; set; }
        //  public string InTimeHrED { get; set; }
        //  public string InTimeMinED { get; set; }
        public string PartOfDay { get; set; }
        public decimal DurationHours { get; set; }
        public bool EmergencyLeave { get; set; }

        //Step 2

        public bool ConfirmedbyHR { get; set; }
        public bool SelfCertificationFormRequired { get; set; }
        public string SelfCertificateReceivedDate { get; set; }
        public bool BackToWorkInterviewRequired { get; set; }
        public string InterviewDate { get; set; }
        public string InterviewConductedBy { get; set; }
        public bool IsPaid { get; set; }
        public bool IsPaidatotherrate { get; set; }
        public bool IsUnpaid { get; set; }

        //Step 3
        public bool DoctorConsulted { get; set; }
        public string DoctorName { get; set; }
        public string DateOfVisit { get; set; }
        //public string TimeOfVisitStartHr { get; set; }
        //public string TimeOfVisitEndHr { get; set; }
        //public string TimeOfVisitStartMin { get; set; }
        //public string TimeOfVisitEndMin { get; set; }
        public string TimeOfVisit { get; set; }
        public string MedicalCertificateIssuedDate { get; set; }
        public string DoctorAdvice { get; set; }
        public string MedicationPrescribed { get; set; }
        public string WhyDoctorNotConsulted { get; set; }
        public bool IsDuaToAccident { get; set; }

        public string CommentListString { get; set; }
        public string DocumentListString { get; set; }

    }

    public class SickLeavesCommentViewModel
    {
        public int Id { get; set; }
        public int SickLeaveID { get; set; }
        public string comment { get; set; }
        public string commentBy { get; set; }
        public string commentTime { get; set; }
    }

    public class SickLeavesDocumentViewModel
    {
        public int Id { get; set; }
        public int SickLeaveID { get; set; }
        public string originalName { get; set; }
        public string newName { get; set; }
        public string description { get; set; }
    }

    //Maternity-Paternity Leaves
    public class EmployeePlanner_MaternityPaternityViewModel
    {
        public EmployeePlanner_MaternityPaternityViewModel()
        {
            CommentList = new List<MaternityPaternityCommentViewModel>();
            DocumentList = new List<MaternityPaternityDocumentViewModel>();
        }
        public IList<MaternityPaternityCommentViewModel> CommentList { get; set; }
        public IList<MaternityPaternityDocumentViewModel> DocumentList { get; set; }

        public string CommentListString { get; set; }
        public string DocumentListString { get; set; }
        public int day { get; set; }
        public int yearId { get; set; }
        public int monthId { get; set; }
        public int HolidayCountryID { get; set; }
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public string DueDate { get; set; }

        public string Lengthofemployment { get; set; }
        public string ExptectedBirthWeekStartDate { get; set; }
        public string ExptectedBirthWeekEndDate { get; set; }
        public string OrdinaryMaternityLeaveStartDate { get; set; }
        public string OrdinaryMaternityLeaveEndDate { get; set; }
        public string AdditionalMaternityLeaveStartDate { get; set; }
        public string AdditionalMaternityLeaveEndDate { get; set; }
        public string EarliestBirthWeekStartDate { get; set; }
        public string ActualStartDate { get; set; }
        public string ActualEndDate { get; set; }

    }

    public class MaternityPaternityCommentViewModel
    {
        public int Id { get; set; }
        public int MaternityOrPaternityID { get; set; }
        public string comment { get; set; }
        public string commentBy { get; set; }
        public string commentTime { get; set; }
    }

    public class MaternityPaternityDocumentViewModel
    {
        public int Id { get; set; }
        public int MaternityOrPaternityID { get; set; }
        public string originalName { get; set; }
        public string newName { get; set; }
        public string description { get; set; }
    }

    // settings
    public class EmployeeAllowanceSettings
    {
        public EmployeeAllowanceSettings()
        {
            holidayYearList = new List<SelectListItem>();
            MeasuredList = new List<SelectListItem>();
            authorisationsList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public int HolidayYear { get; set; }
        public int MeasuredIn { get; set; }

        public int Thisyear { get; set; }
        public int Nextyear { get; set; }
        public int TOIL { get; set; }
        public int CarriedOver { get; set; }
        public string AuthoriseUserId { get; set; }

        public bool EntitlementIncludesPublicHoliday { get; set; }

        public bool AutoApproveHolidays { get; set; }
        public bool ExceedAllowance { get; set; }
        public bool UseVirtualClock { get; set; }
        public string authorisationEmployeeName { get; set; }
        public IList<SelectListItem> authorisationsList { get; set; }
        public IList<SelectListItem> holidayYearList { get; set; }

        public IList<SelectListItem> MeasuredList { get; set; }

    }

    public class EmployeeTOILModelView
    {
        public EmployeeTOILModelView()
        {

        }

        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int Balance { get; set; }
        public bool AddEdit { get; set; }
        public int DurationDays { get; set; }
        public string ExpiryDate { get; set; }
        public string StartDate { get; set; }
        public string SupportingComments { get; set; }


    }

    //Work Pattern
    public class EmployeeWorkPatternViewModel
    {
        public EmployeeWorkPatternViewModel()
        {

        }

        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int WorkPatternId { get; set; }
        public string EffectiveFrom { get; set; }
        public int CurrentWeekWorkPatternDetailID { get; set; }
        public int HolidayCountryID { get; set; }
    }

    //Work Pattern History 

    public class EmployeeWorkPatternHistoryViewModel
    {
        public EmployeeWorkPatternHistoryViewModel()
        {

        }
        public int Id { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string ChangeDate { get; set; }
        public string ChangeBy { get; set; }

    }

    //Print 

    public class EmployeePrintPdfViewModel
    {
        public int EmployeeId { get; set; }
        public int day { get; set; }
        public int yearId { get; set; }
        public int monthId { get; set; }
        public bool StartDateError { get; set; }
        public string StartDate { get; set; }
        public bool EndDateError { get; set; }
        public string EndDate { get; set; }
        public bool Absence { get; set; }
        public bool Holidays { get; set; }
    }

    public class EmployeeAbsenceLeavesPdfViewModel
    {
        public EmployeeAbsenceLeavesPdfViewModel()
        {
            DetailList = new List<EmployeeAbsenceLeaveList>();
        }
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string HireDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public IList<EmployeeAbsenceLeaveList> DetailList { get; set; }
    }
    public class EmployeeAbsenceLeaveList
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal Duration { get; set; }
        public string Comments { get; set; }
        public string Paid { get; set; }
        public string UnPaid { get; set; }
        public string PaidAtOtherRate { get; set; }
        public string EmergencyLeave { get; set; }
    }

    public class EmployeeHolidayLeavesPdfViewModel
    {
        public EmployeeHolidayLeavesPdfViewModel()
        {
            DetailList = new List<EmployeeHolidaysLeaveList>();
        }
        public int Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeStartDate { get; set; }
        public string EmployeeEndDate { get; set; }
        public int Entitlement { get; set; }
        public decimal Booked { get; set; }
        public int Remaining { get; set; }

        public IList<EmployeeHolidaysLeaveList> DetailList { get; set; }

    }

    public class EmployeeHolidaysLeaveList
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Duration { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string ApprovedBy { get; set; }
        public string DateRequested { get; set; }
        public string Comments { get; set; }
    }

    public class WorkpatternWeekend
    {
        public int Id { get; set; }
        public Nullable<decimal> MondayDays { get; set; }
        public Nullable<decimal> TuesdayDays { get; set; }
        public Nullable<decimal> WednessdayDays { get; set; }
        public Nullable<decimal> ThursdayDays { get; set; }
        public Nullable<decimal> FridayDays { get; set; }
        public Nullable<decimal> SaturdayDays { get; set; }
        public Nullable<decimal> SundayDays { get; set; }

    }
}