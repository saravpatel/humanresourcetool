using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRTool.CommanMethods;

namespace HRTool.Models.Admin
{
    public class AdminPlannerViewModel
    {
        public AdminPlannerViewModel()
        {
            ListOfSelectedYear_Month = new List<SelectListItem>();
        }
        public int ID { get; set; }

        public bool IsSchedule { get; set; }
        public bool IsTimeSheet { get; set; }
        public bool IsTravel { get; set; }
        public bool IsUplift { get; set; }
        public Nullable<int> Flag { get; set; }
        public List<KeyValue> listOfBusiness { get; set; }
        public List<KeyValue> listOfDivision { get; set; }
        public List<KeyValue> listOfProject { get; set; }
        public List<KeyValue> listOfPool { get; set; }
        public List<KeyValue> listOfFunction { get; set;}
        public List<KeyValue> listOfEmployee { get; set; }
        public List<KeyValue> ListOfYear { get; set; }
        public List<SelectListItem> ListOfSelectedYear_Month { get; set; }
        public AdminPlannerYearModel AdminPlannerYearModel { get; set; }
        public AdminPlannerYearModel AdminPlannerMonth_YearModel { get; set; }
        public string HolidayYear { get; set; }
        //Sick Leave Hit Map

        public decimal MondayDays { get; set; }
        public decimal TuesdayDays { get; set; }
        public decimal WednessdayDays { get; set; }
        public decimal ThursdayDays { get; set; }
        public decimal FridayDays { get; set; }
        public decimal SaturdayDays { get; set; }
        public decimal SundayDays { get; set; }
        

    }
    public class AdminPlannerYearModel
    {
        public AdminPlannerYearModel()
        {
            //listOfEmployeeScheduling = new List<SchedulingProjectPlannerViewModel>();
            listOfMonthModel = new List<AdminPlannerMonthModel>();
            listOfDayModel = new List<AdminPlannerDayModel>();
        }

        public int YearId { get; set; }
        public bool IsTimeSheet { get; set; }
        public bool IsTravel { get; set; }
        public bool IsAnualLeave { get; set; }
        public bool IsOtherLeave { get; set; }
        public bool IsSickLeave { get; set; }
        public bool IsLateLeave { get; set; }
        public bool IsPublicHolidays { get; set; }
        public bool IsMatPatLeave { get; set; }
        public Nullable<int> IsTimesheetDrillDown { get; set; }
        public List<TimeSheetProjectPlannerViewModel> listOfEmployeeTimeSheet { get; set; }
        public List<TravelProjectPlannerViewModel> listOfEmployeeTravelLeave { get; set; }
        public List<AnnualLeavePlannerViewModel> listOfEmployeeAnnualLeave { get; set; }
        public List<OtherLeavePlannerViewModel> listOfEmployeeOtherLeave { get; set; }
        public List<SickLeavePlanner> listOfEmployeeSickLeave { get; set; }
        public List<LateLeavePlannerViewModel> listOfEmployeeLateLeave { get; set; }
        public List<MaternityLeaveViewModel> listOfEmployeeMaternityLeave { get; set; }
        public List<PublicHolidayViewModel> listOfEmployeePublicHoliday { get; set; }
        public List<AdminPlannerMonthModel> listOfMonthModel { get; set; }
        public List<AdminPlannerDayModel> listOfDayModel { get; set; }
        public AdminPlannerDayModel DayModel { get; set; }
    }
    public class AdminPlannerDayModel
    {
        public AdminPlannerDayModel()
        {
        }
        public int DayId { get; set; }
        public int YearId { get; set; }
        public int MonthId { get; set; }
        public string DayName { get; set; }
        public int StartDay { get; set; }
        public bool IsTimeSheet { get; set; }
        public bool IsAnualLeave { get; set; }
        public bool IsTravel { get; set; }
        public bool IsOtherLeave { get; set; }
        public bool IsSickLeave { get; set; }
        public bool IsLate { get; set; }
        public bool IsPublicHolidays { get; set; }
        public bool IsMatPatLeave { get; set; }
        public int LeaveId { get; set; }
        public string MonthName { get; set; }
        public List<SchedulingProjectPlannerViewModel> listOfEmployeeScheduling { get; set; }
    }
    public class AdminPlannerRequestModel
    {
        public int ID { get; set; }
        public int Year { get; set; }
        public bool IsTimeSheet { get; set; }
        public bool IsAnualLeave { get; set; }
        public bool IsTravel { get; set; }
        public bool IsOtherLeave { get; set; }
        public bool IsSickLeave { get; set; }
        public bool IsLate { get; set; }
        public bool IsPublicHolidays { get; set; }
        public bool IsMatPatLeave { get; set; }
        public int BusinessId { get; set; }
        public int DivisionId { get; set; }
        public int PoolId { get; set; }
        public int FunctionId { get; set; }
        public int ResourceId { get; set; }
        public int ProjectId { get; set; }
        public int YearId { get; set; }
        public int MonthId { get; set; }
    }
    public class AdminPlannerMonthModel
    {
        public AdminPlannerMonthModel()
        {
            listOfEmployeeScheduling = new List<SchedulingProjectPlannerViewModel>();
            listOfDayModel = new List<AdminPlannerDayModel>();
        }
        public int MonthId { get; set; }
        public int YearId { get; set; }
        public string MonthName { get; set; }
        public bool IsTimeSheet { get; set; }
        public bool IsAnualLeave { get; set; }
        public bool IsTravel { get; set; }
        public bool IsOtherLeave { get; set; }
        public bool IsSickLeave { get; set; }
        public bool IsLate { get; set; }
        public bool IsPublicHolidays { get; set; }
        public bool IsMatPatLeave { get; set; }
        public Nullable<decimal> TotalTravelLeave { get; set; }
        public Nullable<int> TotalMaternityLeave { get; set; }
        public Nullable<int> TotalLateLeave { get; set; }
        public Nullable<int> TotalSickLeave { get; set; }
        public Nullable<int> TotalOtherLeave { get; set; }
        public Nullable<int> TotalAnnualLeave { get; set; }
        public Nullable<decimal> TotalTimesheet { get; set; }
        public List<AdminPlannerDayModel> listOfDayModel { get; set; }
        public List<AdminPlannerDayModel> listOfDay_MonthModel { get; set; }
        public List<TimeSheetProjectPlannerViewModel> listOfEmployeeTimeSheet { get; set; }
        public List<TravelProjectPlannerViewModel> listOfEmployeeTravelLeave { get; set; }
        public List<UpliftProjectPlannerViewModel> listOfEmployeeUpliftLeave { get; set; }

        public List<SchedulingProjectPlannerViewModel> listOfEmployeeScheduling { get; set; }
    }


}