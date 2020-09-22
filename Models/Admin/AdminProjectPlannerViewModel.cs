using HRTool.CommanMethods;
using HRTool.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace HRTool.Models.Admin
{
    public class AdminProjectPlannerViewModel
    {
        public AdminProjectPlannerViewModel()
        {
            ListOfSelectedYear_Month = new List<SelectListItem>();
        }
        public int ID { get; set; }

        public bool IsSchedule { get; set; }
        public bool IsTimeSheet { get; set; }
        public bool IsTravel { get; set; }
        public bool IsUplift { get; set; }
        public List<KeyValue> listOfBusiness { get; set; }
        public List<KeyValue> listOfDivision { get; set; }
        public List<KeyValue> listOfFunction { get; set; }
        public List<KeyValue> listOfPool { get; set; }
        public List<KeyValue> listOfEmployee { get; set; }
        public List<KeyValue> listOfProject { get; set; }
        public List<KeyValue> ListOfYear { get; set; }        
        public Nullable<int> Flag { get; set; }
        public List<SelectListItem> ListOfSelectedYear_Month { get; set; }
        public AdminProjectPlannerYearModel AdminPlannerYearModel { get; set; }
        public AdminProjectPlannerYearModel AdminPlannerMonth_YearModel { get; set; }
    }

    public class AdminProjectPlannerRequestModel
    {
        public int ID { get; set; }
        public int Year { get; set; }
        public bool IsTimeSheet { get; set; }
        public bool IsSchedule { get; set; }
        public bool IsTravel { get; set; }
        public bool IsUplift { get; set; }
        public int BusinessId { get; set; }
        public int DivisionId { get; set; }
        public int PoolId { get; set; }
        public int FunctionId { get; set; }
        public int ResourceId { get; set; }
        public int ProjectId { get; set; }
        public int YearId { get; set; }
        public int MonthId { get; set; }
    }

    public class AdminProjectPlannerYearModel
    {
        public AdminProjectPlannerYearModel()
        {
            listOfEmployeeScheduling = new List<SchedulingProjectPlannerViewModel>();
            listOfMonthModel = new List<AdminProjectPlannerMonthModel>();
            listOfDayModel = new List<AdminProjectPlannerDayModel>();
        }

        public int YearId { get; set; }
        public bool IsTimeSheet { get; set; }
        public bool IsSchedule { get; set; }
        public bool IsTravel { get; set; }
        public bool IsUplift { get; set; }
        public List<SchedulingProjectPlannerViewModel> listOfEmployeeScheduling { get; set; }
        public List<TimeSheetProjectPlannerViewModel> listOfEmployeeTimeSheet { get; set; }
        public List<TravelProjectPlannerViewModel> listOfEmployeeTravelLeave { get; set; }
        public List<UpliftProjectPlannerViewModel> listOfEmployeeUpliftLeave { get; set; }
        public List<AdminProjectPlannerMonthModel> listOfMonthModel { get; set; }
        public List<AdminProjectPlannerDayModel> listOfDayModel { get; set; }
        public AdminProjectPlannerDayModel DayModel { get; set; }
        public Nullable<int> isExistWorkPattern { get; set; }
        public Nullable<int> isExistWorkPatternLeave { get; set; }
    }
    public class AdminProjectPlannerMonthModel
    {
        public AdminProjectPlannerMonthModel()
        {
            listOfEmployeeScheduling = new List<SchedulingProjectPlannerViewModel>();
            listOfDayModel = new List<AdminProjectPlannerDayModel>();
        }
        public int MonthId { get; set; }
        public int YearId { get; set; }
        public string MonthName { get; set; }
        public bool IsTimeSheet { get; set; }
        public bool IsSchedule { get; set; }
        public bool IsTravel { get; set; }
        public bool IsUplift { get; set; }
        public Nullable<decimal> TotalTimesheet { get; set; }
        public Nullable<decimal> TotalUplift { get; set; }

        public List<AdminProjectPlannerDayModel> listOfDayModel { get; set; }
        public List<AdminProjectPlannerDayModel> listOfDay_MonthModel { get; set; }
        public List<TimeSheetProjectPlannerViewModel> listOfEmployeeTimeSheet { get; set; }
        public List<TravelProjectPlannerViewModel> listOfEmployeeTravelLeave { get; set; }
        public List<UpliftProjectPlannerViewModel> listOfEmployeeUpliftLeave { get; set; }
        public Nullable<int> isExistWorkPattern { get; set; }
        public Nullable<int> isExistWorkPatternLeave { get; set; }
        public List<SchedulingProjectPlannerViewModel> listOfEmployeeScheduling { get; set; }
    }

    public class AdminProjectPlannerDayModel
    {
        public AdminProjectPlannerDayModel()
        {
        }
        public int DayId { get; set; }
        public int YearId { get; set; }
        public int MonthId { get; set; }
        public string DayName { get; set; }
        public int StartDay { get; set; }
        public bool IsSheduling { get; set; }
        public bool IsTravel { get; set; }
        public bool IsTimeSheet { get; set; }
        public bool IsUplift { get; set; }
        public int LeaveId { get; set; }
        public string MonthName { get; set; }
        public List<SchedulingProjectPlannerViewModel> listOfEmployeeScheduling { get; set; }
    }
}