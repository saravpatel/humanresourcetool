using HRTool.CommanMethods.Resources;
using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.IO;
using HRTool.CommanMethods.RolesManagement;
using HRTool.Models.Settings;
using Rotativa;
using Rotativa.Options;
using HRTool.CommanMethods;
using static HRTool.CommanMethods.Enums;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class MeEmployeeProjectPlannerController : Controller
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        EmployeeProjectPlannerMethod _employeeProjectPlannerMethod = new EmployeeProjectPlannerMethod();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        ProjectSettindsMethod _projectSettindsMethod = new ProjectSettindsMethod();
        EmployeeMethod _employeeMethod = new EmployeeMethod();
        EmployeePlannerMethod _employeePlannerMethod = new EmployeePlannerMethod();
        RolesManagementMethod _RolesManagementMethod = new RolesManagementMethod();
        HolidayNAbsenceMethod _holidayNAbsenceMethod = new HolidayNAbsenceMethod();
        private string inputFormat = "dd-MM-yyyy";
        private string outputFormat = "yyyy-MM-dd HH:mm:ss";

        #endregion

        #region Index
        public ActionResult Index(int EmployeeId)
        {
            EmployeeProjectPlannerViewModel model = new EmployeeProjectPlannerViewModel();
            model.yearId = DateTime.Now.Year;
            model.currentMonth = DateTime.Now.Month;
            model.EmployeeId = EmployeeId;
            var userID = SessionProxy.UserId;
            var employee = _employeeMethod.getEmployeeById(userID);
            var WorkPattern = _employeePlannerMethod.GetNoorderEmployeeWorkPatternListByEmployeeId(EmployeeId).OrderByDescending(x => x.EffectiveFrom).FirstOrDefault();
            if (WorkPattern != null)
            {
                model.WorkPattern = WorkPattern.WorkPatternID;
            }
            else
            {
                var Default = _holidayNAbsenceMethod.getAllHolidaysNAbsenceSettingList();
                if (Default != null)
                {
                    model.WorkPattern = Convert.ToInt16(Default.WorkPattern);
                }
                else
                {
                    model.WorkPattern = 0;
                }
            }
            model.LoginUserName = employee.FirstName + " " + employee.LastName;
            //   var CountryList = _holidayNAbsenceMethod.getAllCountryList();
            var employeeData = _db.AspNetUsers.Where(x => x.Id == EmployeeId).ToList();
            if (employeeData.FirstOrDefault().JobContryID != null && employeeData.FirstOrDefault().JobContryID != 0)
            {
                var countryList = _holidayNAbsenceMethod.getHolidayAndAbsenceByEmployee(EmployeeId);
                foreach (var item in countryList)
                {
                    model.LastCountryId = Convert.ToInt32(item.JobContryID);
                }
            }
            var CountryList = _holidayNAbsenceMethod.getAllCountryList();
            foreach (var item in CountryList)
            {
                model.CountryList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            var AssignCountry = _employeePlannerMethod.publicHolidayListByEmployeeId(EmployeeId);
            model.WorkPatternList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in _holidayNAbsenceMethod.getAllWorkPattern())
            {
                model.WorkPatternList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            var data = _db.AspNetUserRoles.Where(x => x.UserId == EmployeeId && x.RoleId == 1).ToList();
            if (data.Count > 0 && data != null)
            {
                model.flag = 1;
            }
            else
            {
                model.flag = 0;
            }
            if (AssignCountry.Count > 0)
            {
                var Details = _employeePlannerMethod.publicHolidayListByEmployeeId(EmployeeId).OrderByDescending(x => x.EffecitveFrom).FirstOrDefault();
                var Country = _holidayNAbsenceMethod.GetPublicHolidayCountryById((int)Details.PublicHolidayCountryId);
                if(Country != null)
                {
                    model.LastCountryId = Country.Id;
                }
                //AssignCountry.OrderByDescending(x => x.Id).FirstOrDefault().Id;
            }
            else
            {
                if (employeeData.FirstOrDefault().JobContryID != null && employeeData.FirstOrDefault().JobContryID != 0)
                {
                    int JobCId = Convert.ToInt32(employeeData.FirstOrDefault().JobContryID);
                    var Country = _holidayNAbsenceMethod.GetPublicHolidayCountryById(JobCId);
                    if(Country != null)
                    {
                        model.LastCountryId = Country.Id;
                    }
                }
                else
                {
                    var Defaults = _holidayNAbsenceMethod.getAllHolidaysNAbsenceSettingList();
                    if (Defaults != null)
                    {
                        model.LastCountryId = Convert.ToInt16(Defaults.PublicHolidayTemplate);
                    }
                    else
                    {
                        model.LastCountryId = 0;
                    }
                }
            }
            HolidayYearAndMonthSetting HolidayYearAndMonth = _db.HolidayYearAndMonthSettings.Where(x => x.IsActive == true).FirstOrDefault();
            List<Employee_ProjectPlanner_TravelLeave> TravelProjectPlanner = _db.Employee_ProjectPlanner_TravelLeave.Where(x => x.Archived == false && x.EmployeeId == EmployeeId).ToList();

            List<Employee_ProjectPlanner_Uplift> UpliftProjectPlanner = _db.Employee_ProjectPlanner_Uplift.Where(x => x.Archived == false && x.EmployeeId == EmployeeId).ToList();

            List<Employee_ProjectPlanner_TimeSheet> ProjectDayTimesheet = _db.Employee_ProjectPlanner_TimeSheet.Where(x => x.Archived == false && x.EmployeeId == EmployeeId).ToList();

            var TimesheetProjectPlanner = (from timesheet in _db.Employee_ProjectPlanner_TimeSheet
                                           join timesheetDetails in _db.Employee_ProjectPlanner_TimeSheet_Detail
                                           on timesheet.Id equals timesheetDetails.TimeSheetId
                                           where timesheet.EmployeeId == EmployeeId
                                           select new
                                           {
                                               Id = timesheet.Id,
                                               EmployeeId = timesheet.EmployeeId,
                                               Date = timesheet.Date,
                                               Comments = timesheet.Comments,
                                               Archived = timesheet.Archived,
                                               CreatedDate = timesheet.CreatedDate,
                                               CreatedBy = timesheet.CreatedBy,
                                               LastModifiedBy = timesheet.LastModifiedBy,
                                               LastModifiedDate = timesheet.LastModifiedDate,
                                               ApproveStatus = timesheetDetails.ApprovalStatus
                                           }).ToList();

            _db.Employee_ProjectPlanner_TimeSheet.Where(x => x.Archived == false && x.EmployeeId == EmployeeId).ToList();

            model.TravelDayInYear = _employeePlannerMethod.GetTravelInYearCount(TravelProjectPlanner, HolidayYearAndMonth);
            model.TravelDaySinceYear = _employeePlannerMethod.GetTravelsinceEverCount(TravelProjectPlanner);
            model.TravelTotalDays = 0;

            model.UpliftDayInYear = _employeePlannerMethod.GetUpliftDayInYearCount(UpliftProjectPlanner, HolidayYearAndMonth);
            model.UpliftDaySinceYear = UpliftProjectPlanner.Count();

            List<Employee_ProjectPlanner_TimeSheet> ApprovedTimeSheet = TimesheetProjectPlanner.Where(x => x.ApproveStatus != null ? x.ApproveStatus.Trim() != "Pending" : x.ApproveStatus == "Pending").Select(xx => new Employee_ProjectPlanner_TimeSheet
            {
                Archived = xx.Archived,
                Date = xx.Date,
                Id = xx.Id,
                CreatedBy = xx.CreatedBy,
                CreatedDate = xx.CreatedDate
            }).ToList();

            List<Employee_ProjectPlanner_TimeSheet> PendingTimeSheet = TimesheetProjectPlanner.Where(x => x.ApproveStatus != null ? x.ApproveStatus.Trim() == "Pending" : x.ApproveStatus == "Pending").Select(xx => new Employee_ProjectPlanner_TimeSheet
            {
                Archived = xx.Archived,
                Date = xx.Date,
                Id = xx.Id,
                CreatedBy = xx.CreatedBy,
                CreatedDate = xx.CreatedDate
            }).ToList();

            model.TimesheetApprovedIntheYear = _employeePlannerMethod.GetTimeSheetDayInYearCount(ApprovedTimeSheet, HolidayYearAndMonth);
            model.TimesheetApprovedSinceYear = ApprovedTimeSheet.Count();
            model.TimesheetAwaitingForApproval = PendingTimeSheet.Count();

            model.ProjectDayWorkedInYear = _employeePlannerMethod.GetTimeSheetDayInYearCount(ProjectDayTimesheet, HolidayYearAndMonth);
            model.ProjectDayWorkedsinceYear = ProjectDayTimesheet.Count();

            return View(model);
        }

        #endregion

        #region Common Method
        public ActionResult ListOfMonth(int year, int EmployeeId, int HolidayCountryId)
        {
            EmployeeProjectPlannerYearViewModel yearModel = new EmployeeProjectPlannerYearViewModel();
          
             var workPattenList = _holidayNAbsenceMethod.getAllWorkPattern();
            var employeeWorkPatternList = _employeePlannerMethod.getAllEmployeeWorkPattern().Where(x => x.EmployeeID == EmployeeId).ToList();
            var rotatingWorkPatternList = _holidayNAbsenceMethod.allWorkPatternDetail();
            var scheduling = _employeeProjectPlannerMethod.getAllSchedulingLeves().Where(x => x.EmployeeId == EmployeeId).ToList();
            var timeSheet = _employeeProjectPlannerMethod.getAllTimeSheet().Where(x => x.EmployeeId == EmployeeId).ToList();
            var travelLeave = _employeeProjectPlannerMethod.getAllTravelLeave().Where(x => x.EmployeeId == EmployeeId).ToList();
            var upliftList = _employeeProjectPlannerMethod.getAllUplift().Where(x => x.EmployeeId == EmployeeId).ToList();
            yearModel.yearId = year;
            int TotalMonths = 12;
            for (int i = 1; i <= TotalMonths; i++)
            {
                EmployeeProjectPlannerMonthViewModel monthModel = new EmployeeProjectPlannerMonthViewModel();
                monthModel.monthId = i;
                monthModel.yearId = year;
                monthModel.MonthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(i);
                var startDate = new DateTime(year, i, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                monthModel.TimesheetCount = _employeePlannerMethod.GetProjectPlannerTimeSheetTotalTimeInMonth(EmployeeId, startDate, endDate);
                monthModel.TravelCount = _employeePlannerMethod.GetAllProjrctPlannerTravelLeavesMonthWiseCount(EmployeeId, startDate, endDate);
                //monthModel.SumAnnualLeave = (decimal)annualLeave.Where(x=>x).Select(x => x.Duration).Sum();
                //monthModel.sumLateLeave = lateLeave.Count();
                int totalDays = DateTime.DaysInMonth(year, i);
                for (int j = 1; j <= totalDays; j++)
                {
                    EmployeeProjectPlannerDayViewModel dayModel = new EmployeeProjectPlannerDayViewModel();
                    dayModel.day = j;
                    var date = new DateTime(year, i, j);
                    dayModel.DayName = date.DayOfWeek.ToString();
                    dayModel.monthId = i;
                    dayModel.yearId = year;
                    dayModel.TimeSheetId = 0;
                    dayModel.ScheduleId = 0;
                    dayModel.TravelLeaveId = 0;
                    dayModel.UpliftId = 0;
                    int maxRotaingCount = 0;
                    int currentRotatingWeekDays = 0;

                    #region Work Pattern

                    if (employeeWorkPatternList.Count == 1)
                    {
                        Employee_WorkPattern saveWorkPattern = employeeWorkPatternList.FirstOrDefault();
                        if (date.Date >= saveWorkPattern.EffectiveFrom.Date)
                        {
                            dayModel.Flag = 0;
                            WorkPattern workPatternDetail = workPattenList.Where(x => x.Id == saveWorkPattern.WorkPatternID).FirstOrDefault();
                            if (workPatternDetail != null)
                            {
                                if (workPatternDetail.IsRotating == false)
                                {
                                    dayModel.isWorkPatternLeaveTaken = false;
                                    #region Switch Case

                                    switch (dayModel.DayName.ToLower())
                                    {
                                        case "monday":
                                            if (workPatternDetail.MondayHours == null)
                                            {
                                                dayModel.isWorkPatternLeaveTaken = true;
                                            }
                                            break;
                                        case "tuesday":
                                            if (workPatternDetail.TuesdayHours == null)
                                            {
                                                dayModel.isWorkPatternLeaveTaken = true;
                                            }
                                            break;
                                        case "wednesday":
                                            if (workPatternDetail.WednessdayHours == null)
                                            {
                                                dayModel.isWorkPatternLeaveTaken = true;
                                            }
                                            break;
                                        case "thursday":
                                            if (workPatternDetail.ThursdayHours == null)
                                            {
                                                dayModel.isWorkPatternLeaveTaken = true;
                                            }
                                            break;
                                        case "friday":
                                            if (workPatternDetail.FridayHours == null)
                                            {
                                                dayModel.isWorkPatternLeaveTaken = true;
                                            }
                                            break;
                                        case "saturday":
                                            if (workPatternDetail.SaturdayHours == null)
                                            {
                                                dayModel.isWorkPatternLeaveTaken = true;
                                            }
                                            break;
                                        case "sunday":
                                            if (workPatternDetail.SundayHours == null)
                                            {
                                                dayModel.isWorkPatternLeaveTaken = true;
                                            }
                                            break;
                                        default:
                                            break;


                                    }
                                    #endregion
                                }
                                else
                                {
                                    var rotatingWorkpatternDetail = rotatingWorkPatternList.Where(x => x.WorkPatternID == saveWorkPattern.WorkPatternID).OrderBy(x => x.Id).ToList();
                                    maxRotaingCount = rotatingWorkpatternDetail.Count();


                                    #region Switch Case

                                    var currentWorkPatternDetail = rotatingWorkpatternDetail[currentRotatingWeekDays];
                                    switch (dayModel.DayName.ToLower())
                                    {
                                        case "monday":
                                            if (currentWorkPatternDetail.MondayHours == null)
                                            {
                                                dayModel.isWorkPatternLeaveTaken = true;
                                            }
                                            break;
                                        case "tuesday":
                                            if (currentWorkPatternDetail.TuesdayHours == null)
                                            {
                                                dayModel.isWorkPatternLeaveTaken = true;
                                            }
                                            break;
                                        case "wednesday":
                                            if (currentWorkPatternDetail.WednessdayHours == null)
                                            {
                                                dayModel.isWorkPatternLeaveTaken = true;
                                            }
                                            break;
                                        case "thursday":
                                            if (currentWorkPatternDetail.ThursdayHours == null)
                                            {
                                                dayModel.isWorkPatternLeaveTaken = true;
                                            }
                                            break;
                                        case "friday":
                                            if (currentWorkPatternDetail.FridayHours == null)
                                            {
                                                dayModel.isWorkPatternLeaveTaken = true;
                                            }
                                            break;
                                        case "saturday":
                                            if (currentWorkPatternDetail.SaturdayHours == null)
                                            {
                                                dayModel.isWorkPatternLeaveTaken = true;
                                            }
                                            break;
                                        case "sunday":
                                            if (currentWorkPatternDetail.SundayHours == null)
                                            {
                                                dayModel.isWorkPatternLeaveTaken = true;

                                            }
                                            if (rotatingWorkpatternDetail.Count > 1)
                                            {
                                                if (currentRotatingWeekDays < maxRotaingCount - 1)
                                                    currentRotatingWeekDays = currentRotatingWeekDays + 1;
                                                else
                                                    currentRotatingWeekDays = 0;
                                            }
                                            break;
                                        default:
                                            break;



                                    }
                                    #endregion
                                }
                            }
                        }
                        else
                        {
                            dayModel.Flag = 1;
                        }
                    }
                    else if (employeeWorkPatternList.Count > 1)
                    {
                        foreach (var item in employeeWorkPatternList)
                        {
                            if (date.Date >= item.EffectiveFrom.Date)
                            {
                                dayModel.Flag = 0;
                                currentRotatingWeekDays++;
                                WorkPattern workPatternDetail = workPattenList.Where(x => x.Id == item.WorkPatternID).FirstOrDefault();
                                if (workPatternDetail != null)
                                {
                                    if (workPatternDetail.IsRotating == false)
                                    {
                                        dayModel.isWorkPatternLeaveTaken = false;

                                        #region Switch Case

                                        switch (dayModel.DayName.ToLower())
                                        {
                                            case "monday":
                                                if (workPatternDetail.MondayHours == null)
                                                {
                                                    dayModel.isWorkPatternLeaveTaken = true;
                                                }
                                                break;
                                            case "tuesday":
                                                if (workPatternDetail.TuesdayHours == null)
                                                {
                                                    dayModel.isWorkPatternLeaveTaken = true;
                                                }
                                                break;
                                            case "wednesday":
                                                if (workPatternDetail.WednessdayHours == null)
                                                {
                                                    dayModel.isWorkPatternLeaveTaken = true;
                                                }
                                                break;
                                            case "thursday":
                                                if (workPatternDetail.ThursdayHours == null)
                                                {
                                                    dayModel.isWorkPatternLeaveTaken = true;
                                                }
                                                break;
                                            case "friday":
                                                if (workPatternDetail.FridayHours == null)
                                                {
                                                    dayModel.isWorkPatternLeaveTaken = true;
                                                }
                                                break;
                                            case "saturday":
                                                if (workPatternDetail.SaturdayHours == null)
                                                {
                                                    dayModel.isWorkPatternLeaveTaken = true;
                                                }
                                                break;
                                            case "sunday":
                                                if (workPatternDetail.SundayHours == null)
                                                {
                                                    dayModel.isWorkPatternLeaveTaken = true;
                                                }
                                                break;
                                            default:
                                                break;


                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        var rotatingWorkpatternDetail = rotatingWorkPatternList.Where(x => x.WorkPatternID == item.WorkPatternID).OrderBy(x => x.Id).ToList();
                                        maxRotaingCount = rotatingWorkpatternDetail.Count();
                                        for (int ii = 0; ii < rotatingWorkpatternDetail.Count; ii++)
                                        {
                                            var currentWorkPatternDetail = rotatingWorkpatternDetail[ii];
                                            #region Switch Case
                                            //var currentWorkPatternDetail = rotatingWorkpatternDetail[currentRotatingWeekDays];

                                            switch (dayModel.DayName.ToLower())
                                            {
                                                case "monday":
                                                    if (currentWorkPatternDetail.MondayHours == null)
                                                    {
                                                        dayModel.isWorkPatternLeaveTaken = true;
                                                    }
                                                    break;
                                                case "tuesday":
                                                    if (currentWorkPatternDetail.TuesdayHours == null)
                                                    {
                                                        dayModel.isWorkPatternLeaveTaken = true;
                                                    }
                                                    break;
                                                case "wednesday":
                                                    if (currentWorkPatternDetail.WednessdayHours == null)
                                                    {
                                                        dayModel.isWorkPatternLeaveTaken = true;
                                                    }
                                                    break;
                                                case "thursday":
                                                    if (currentWorkPatternDetail.ThursdayHours == null)
                                                    {
                                                        dayModel.isWorkPatternLeaveTaken = true;
                                                    }
                                                    break;
                                                case "friday":
                                                    if (currentWorkPatternDetail.FridayHours == null)
                                                    {
                                                        dayModel.isWorkPatternLeaveTaken = true;
                                                    }
                                                    break;
                                                case "saturday":
                                                    if (currentWorkPatternDetail.SaturdayHours == null)
                                                    {
                                                        dayModel.isWorkPatternLeaveTaken = true;
                                                    }
                                                    break;
                                                case "sunday":
                                                    if (currentWorkPatternDetail.SundayHours == null)
                                                    {
                                                        dayModel.isWorkPatternLeaveTaken = true;

                                                    }
                                                    if (rotatingWorkpatternDetail.Count > 1)
                                                    {
                                                        if (currentRotatingWeekDays < maxRotaingCount - 1)
                                                            currentRotatingWeekDays = currentRotatingWeekDays + 1;
                                                        else
                                                            currentRotatingWeekDays = 0;
                                                    }
                                                    break;
                                                default:
                                                    break;



                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                            else
                            {
                                dayModel.Flag = 1;
                            }

                        }

                    }
                    else
                    {

                    }

                    #endregion

                    var timeSheetTaken = timeSheet.Where(x => date.Date == x.Date.Value.Date).FirstOrDefault();
                    if (timeSheetTaken != null)
                    {
                        dayModel.TimeSheetId = timeSheetTaken.Id;
                        dayModel.isTimeSheetTaken = true;
                    }

                    var travelLeaveTaken = travelLeave.Where(x => date.Date >= x.StartDate.Value.Date && date.Date <= x.EndDate.Value.Date).FirstOrDefault();
                    if (travelLeaveTaken != null)
                    {
                        dayModel.TravelLeaveId = travelLeaveTaken.Id;
                        dayModel.isTravelLeaveTaken = true;
                    }

                   
                    var schedulingTaken = scheduling.Where(x => date.Date >= (x.StartDate != null ? x.StartDate.Value.Date : DateTime.Now)  && date.Date <= (x.EndDate != null ?  x.EndDate.Value.Date : DateTime.Now)).FirstOrDefault();
                   
                    if (schedulingTaken != null)
                    {
                        dayModel.ScheduleId = schedulingTaken.Id;
                        dayModel.isSchedulingLeaveTaken = true;
                    }
                    var UpliftTaken = upliftList.Where(x => date.Date >= x.Date.Date && date.Date <= x.Date.Date).FirstOrDefault();
                    if (UpliftTaken != null)
                    {
                        dayModel.UpliftId = UpliftTaken.Id;
                        dayModel.isUpliftTaken = true;
                    }

                    if (dayModel.isTimeSheetTaken || dayModel.isTravelLeaveTaken || dayModel.isSchedulingLeaveTaken || dayModel.isUpliftTaken)
                    {
                        dayModel.isLeave = true;
                    }

                    monthModel.DayList.Add(dayModel);
                }
                yearModel.MonthList.Add(monthModel);
            }
            return PartialView("_partialListOfMonthYearWise", yearModel);
        }

        public List<EmployeeProjectPlannerDayViewModel> returnDayListTable(int MonthId, int YearId, int EmployeeId,int HolidayCountryID)
        {
            List<EmployeeProjectPlannerDayViewModel> model = new List<EmployeeProjectPlannerDayViewModel>();
            var workPatternList = _employeePlannerMethod.getAllEmployeeWorkPattern().Where(x => x.EmployeeID == EmployeeId).ToList();
            var scheduling = _employeeProjectPlannerMethod.getAllSchedulingLeves().Where(x => x.EmployeeId == EmployeeId).ToList();
            var timeSheet = _employeeProjectPlannerMethod.getAllTimeSheet().Where(x => x.EmployeeId == EmployeeId).ToList();
            var travelLeave = _employeeProjectPlannerMethod.getAllTravelLeave().Where(x => x.EmployeeId == EmployeeId).ToList();
            var upliftList = _employeeProjectPlannerMethod.getAllUplift().Where(x => x.EmployeeId == EmployeeId).ToList();
            int totalDays = DateTime.DaysInMonth(YearId, MonthId);
            for (int j = 1; j <= totalDays; j++)
            {
                EmployeeProjectPlannerDayViewModel dayModel = new EmployeeProjectPlannerDayViewModel();
                dayModel.day = j;
                var date = new DateTime(YearId, MonthId, j);                
                dayModel.DayName = date.DayOfWeek.ToString();
                dayModel.monthId = MonthId;
                dayModel.yearId = YearId;
                dayModel.TimeSheetId = 0;
                dayModel.ScheduleId = 0;
                dayModel.TravelLeaveId = 0;
                dayModel.UpliftId = 0;

                #region Work Pattern

                if (workPatternList.Count == 1)
                {
                    Employee_WorkPattern saveWorkPattern = workPatternList.FirstOrDefault();
                    if (date.Date >= saveWorkPattern.EffectiveFrom.Date)
                    {
                        WorkPattern workPatternDetail = _holidayNAbsenceMethod.getWorkPatternById(saveWorkPattern.WorkPatternID);
                        if (workPatternDetail != null)
                        {
                            if (workPatternDetail.IsRotating == false)
                            {
                                dayModel.isWorkPatternLeaveTaken = false;
                                switch (dayModel.DayName.ToLower())
                                {
                                    case "monday":
                                        if (workPatternDetail.MondayHours == null)
                                        {
                                            dayModel.isWorkPatternLeaveTaken = true;
                                        }
                                        break;
                                    case "tuesday":
                                        if (workPatternDetail.TuesdayHours == null)
                                        {
                                            dayModel.isWorkPatternLeaveTaken = true;
                                        }
                                        break;
                                    case "wednesday":
                                        if (workPatternDetail.WednessdayHours == null)
                                        {
                                            dayModel.isWorkPatternLeaveTaken = true;
                                        }
                                        break;
                                    case "thursday":
                                        if (workPatternDetail.ThursdayHours == null)
                                        {
                                            dayModel.isWorkPatternLeaveTaken = true;
                                        }
                                        break;
                                    case "friday":
                                        if (workPatternDetail.FridayHours == null)
                                        {
                                            dayModel.isWorkPatternLeaveTaken = true;
                                        }
                                        break;
                                    case "saturday":
                                        if (workPatternDetail.SaturdayHours == null)
                                        {
                                            dayModel.isWorkPatternLeaveTaken = true;
                                        }
                                        break;
                                    case "sunday":
                                        if (workPatternDetail.SundayHours == null)
                                        {
                                            dayModel.isWorkPatternLeaveTaken = true;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else { }
                        }
                    }
                }
                else if (workPatternList.Count > 1)
                {
                    foreach (var item in workPatternList)
                    {

                        if (date.Date >= item.EffectiveFrom.Date)
                        {
                            WorkPattern workPatternDetail = _holidayNAbsenceMethod.getWorkPatternById(item.WorkPatternID);
                            if (workPatternDetail != null)
                            {
                                if (workPatternDetail.IsRotating == false)
                                {
                                    dayModel.isWorkPatternLeaveTaken = false;
                                    switch (dayModel.DayName.ToLower())
                                    {
                                        case "monday":
                                            if (workPatternDetail.MondayHours == null)
                                            {
                                                dayModel.isWorkPatternLeaveTaken = true;
                                            }
                                            break;
                                        case "tuesday":
                                            if (workPatternDetail.TuesdayHours == null)
                                            {
                                                dayModel.isWorkPatternLeaveTaken = true;
                                            }
                                            break;
                                        case "wednesday":
                                            if (workPatternDetail.WednessdayHours == null)
                                            {
                                                dayModel.isWorkPatternLeaveTaken = true;
                                            }
                                            break;
                                        case "thursday":
                                            if (workPatternDetail.ThursdayHours == null)
                                            {
                                                dayModel.isWorkPatternLeaveTaken = true;
                                            }
                                            break;
                                        case "friday":
                                            if (workPatternDetail.FridayHours == null)
                                            {
                                                dayModel.isWorkPatternLeaveTaken = true;
                                            }
                                            break;
                                        case "saturday":
                                            if (workPatternDetail.SaturdayHours == null)
                                            {
                                                dayModel.isWorkPatternLeaveTaken = true;
                                            }
                                            break;
                                        case "sunday":
                                            if (workPatternDetail.SundayHours == null)
                                            {
                                                dayModel.isWorkPatternLeaveTaken = true;
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else { }
                            }
                        }
                    }
                }

                #endregion

                var timeSheetTaken = timeSheet.Where(x => date.Date == x.Date.Value.Date).FirstOrDefault();
                if (timeSheetTaken != null)
                {
                    dayModel.TimeSheetId = timeSheetTaken.Id;
                    dayModel.isTimeSheetTaken = true;
                }

                var travelLeaveTaken = travelLeave.Where(x => date.Date >= x.StartDate.Value.Date && date.Date <= x.EndDate.Value.Date).FirstOrDefault();
                if (travelLeaveTaken != null)
                {
                    dayModel.TravelLeaveId = travelLeaveTaken.Id;
                    dayModel.isTravelLeaveTaken = true;
                }
                var schedulingTaken = scheduling.Where(x => date.Date >= x.StartDate.Value.Date && date.Date <= x.EndDate.Value.Date).FirstOrDefault();
                if (schedulingTaken != null)
                {
                    dayModel.ScheduleId = schedulingTaken.Id;
                    dayModel.isSchedulingLeaveTaken = true;
                }
                var UpliftTaken = upliftList.Where(x => date.Date >= x.Date.Date && date.Date <= x.Date.Date).FirstOrDefault();
                if (UpliftTaken != null)
                {
                    dayModel.UpliftId = UpliftTaken.Id;
                    dayModel.isUpliftTaken = true;
                }

                if (dayModel.isTimeSheetTaken || dayModel.isTravelLeaveTaken || dayModel.isSchedulingLeaveTaken || dayModel.isUpliftTaken)
                {
                    dayModel.isLeave = true;
                }

                model.Add(dayModel);
            }
            return model;
        }

        #endregion

        #region Work Pattern
        public WorkPatternViewModel returnModel(int Id)
        {
            WorkPatternViewModel returnModel = new WorkPatternViewModel();
            var data = _holidayNAbsenceMethod.getWorkPatternById(Id);
            returnModel.Id = Id;
            returnModel.Name = data.Name;
            returnModel.IsRotating = data.IsRotating;
            if (data.IsRotating)
            {
                foreach (var item in _holidayNAbsenceMethod.workPatternDetaiiById(Id))
                {
                    WorkPatternListViewModel detailData = new WorkPatternListViewModel();
                    detailData.Id = item.Id;
                    detailData.WorkPatternID = item.WorkPatternID;
                    detailData.MondayHours = item.MondayHours;
                    detailData.MondayDays = item.MondayDays;
                    detailData.MondayStart = String.Format("{0:HH:mm}", item.MondayStart);
                    detailData.MondayEnd = String.Format("{0:HH:mm}", item.MondayEnd);
                    detailData.MondayBreakMins = item.MondayBreakMins;
                    detailData.TuesdayHours = item.TuesdayHours;
                    detailData.TuesdayDays = item.TuesdayDays;
                    detailData.TuesdayStart = String.Format("{0:HH:mm}", item.TuesdayStart);
                    detailData.TuesdayEnd = String.Format("{0:HH:mm}", item.TuesdayEnd);
                    detailData.TuesdayBreakMins = item.TuesdayBreakMins;
                    detailData.WednessdayHours = item.WednessdayHours;
                    detailData.WednessdayDays = item.WednessdayDays;
                    detailData.WednessdayStart = String.Format("{0:HH:mm}", item.WednessdayStart);
                    detailData.WednessdayEnd = String.Format("{0:HH:mm}", item.WednessdayEnd);
                    detailData.WednessdayBreakMins = item.WednessdayBreakMins;
                    detailData.ThursdayHours = item.ThursdayHours;
                    detailData.ThursdayDays = item.ThursdayDays;
                    detailData.ThursdayStart = String.Format("{0:HH:mm}", item.ThursdayStart);
                    detailData.ThursdayEnd = String.Format("{0:HH:mm}", item.ThursdayEnd);
                    detailData.ThursdayBreakMins = item.ThursdayBreakMins;
                    detailData.FridayHours = item.FridayHours;
                    detailData.FridayDays = item.FridayDays;
                    detailData.FridayStart = String.Format("{0:HH:mm}", item.FridayStart);
                    detailData.FridayEnd = String.Format("{0:HH:mm}", item.FridayEnd);
                    detailData.FridayBreakMins = item.FridayBreakMins;
                    detailData.SaturdayHours = item.SaturdayHours;
                    detailData.SaturdayDays = item.SaturdayDays;
                    detailData.SaturdayStart = String.Format("{0:HH:mm}", item.SaturdayStart);
                    detailData.SaturdayEnd = String.Format("{0:HH:mm}", item.SaturdayEnd);
                    detailData.SaturdayBreakMins = item.SaturdayBreakMins;
                    detailData.SundayHours = item.SundayHours;
                    detailData.SundayDays = item.SundayDays;
                    detailData.SundayStart = String.Format("{0:HH:mm}", item.SundayStart);
                    detailData.SundayEnd = String.Format("{0:HH:mm}", item.SundayEnd);
                    detailData.SundayBreakMins = item.SundayBreakMins;
                    returnModel.WorkPatternList.Add(detailData);
                }
            }
            else
            {
                returnModel.MondayHours = data.MondayHours;
                returnModel.MondayDays = data.MondayDays;
                returnModel.MondayStart = Convert.ToDecimal(data.MondayStart);
                returnModel.MondayEnd = Convert.ToDecimal(data.MondayEnd);
                returnModel.MondayBreakMins = data.MondayBreakMins;
                returnModel.TuesdayHours = data.TuesdayHours;
                returnModel.TuesdayDays = data.TuesdayDays;
                returnModel.TuesdayStart = Convert.ToDecimal(data.TuesdayStart);
                returnModel.TuesdayEnd = Convert.ToDecimal(data.TuesdayEnd);
                returnModel.TuesdayBreakMins = data.TuesdayBreakMins;
                returnModel.WednessdayHours = data.WednessdayHours;
                returnModel.WednessdayDays = data.WednessdayDays;
                returnModel.WednessdayStart = Convert.ToDecimal(data.WednessdayStart);
                returnModel.WednessdayEnd = Convert.ToDecimal(data.WednessdayEnd);
                returnModel.WednessdayBreakMins = data.WednessdayBreakMins;
                returnModel.ThursdayHours = data.ThursdayHours;
                returnModel.ThursdayDays = data.ThursdayDays;
                returnModel.ThursdayStart = Convert.ToDecimal(data.ThursdayStart);
                returnModel.ThursdayEnd = Convert.ToDecimal(data.ThursdayEnd);
                returnModel.ThursdayBreakMins = data.ThursdayBreakMins;
                returnModel.FridayHours = data.FridayHours;
                returnModel.FridayDays = data.FridayDays;
                returnModel.FridayStart = Convert.ToDecimal(data.FridayStart);
                returnModel.FridayEnd = Convert.ToDecimal(data.FridayEnd);
                returnModel.FridayBreakMins = data.FridayBreakMins;
                returnModel.SaturdayHours = data.SaturdayHours;
                returnModel.SaturdayDays = data.SaturdayDays;
                returnModel.SaturdayStart = Convert.ToDecimal(data.SaturdayStart);
                returnModel.SaturdayEnd = Convert.ToDecimal(data.SaturdayEnd);
                returnModel.SaturdayBreakMins = data.SaturdayBreakMins;
                returnModel.SundayHours = data.SundayHours;
                returnModel.SundayDays = data.SundayDays;
                returnModel.SundayStart = Convert.ToDecimal(data.SundayStart);
                returnModel.SundayEnd = Convert.ToDecimal(data.SundayEnd);
                returnModel.SundayBreakMins = data.SundayBreakMins;
            }

            return returnModel;
        }

        public ActionResult workPatten(int Id)
        {
            WorkPatternViewModel model = new WorkPatternViewModel();
            if (Id > 0)
            {
                model = _holidayNAbsenceMethod.returnModel(Id);
                return PartialView("_partialReadOnlyWorkPattern", model);
            }
            else
            {
                model.IsRotating = false;
                model.Id = 0;
                return PartialView("_partialNewWorkPattern", model);
            }

        }

        public ActionResult TrueIsRotating(int workPatternId)
        {
            List<WorkPatternListViewModel> model = new List<WorkPatternListViewModel>();
            if (workPatternId > 0) { }
            else
            {
                WorkPatternListViewModel oneModel = new WorkPatternListViewModel();
                oneModel.WorkPatternID = workPatternId;
                oneModel.Id = 0;
                model.Add(oneModel);
            }
            return PartialView("_partialTrueIsRotating", model);
        }


        public ActionResult FalseIsRotating()
        {
            WorkPatternViewModel model = new WorkPatternViewModel();
            return PartialView("_partialFalseIsRotating", model);
        }

        public ActionResult SaveFalseRoatingData(string modelString)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            WorkPatternViewModel ValueArray = js.Deserialize<WorkPatternViewModel>(modelString);
            _holidayNAbsenceMethod.SaveFalseRoatingData(ValueArray);
            return null;

        }


        public ActionResult SaveTrueRoatingData(int Id, string Name, bool IsRotating, string modelString)
        {
            WorkPatternViewModel model = new WorkPatternViewModel();
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<WorkPatternListViewModel> ValueArray = js.Deserialize<List<WorkPatternListViewModel>>(modelString);
            model.Id = Id;
            model.Name = Name;
            model.IsRotating = IsRotating;
            model.WorkPatternList = ValueArray;
            _holidayNAbsenceMethod.SaveTrueRoatingData(model);
            return null;

        }

        [HttpPost]
        public ActionResult SaveEmployeeWorkPattern(EmployeeWorkPatternViewModel model)
        {
            var Userid = SessionProxy.UserId;
            //var data = _employeePlannerMethod.SaveDataEmployeeWorkPattern(model, Userid);
            //    if(data)
            //    {
            //        return Json("Successes",JsonRequestBehavior.AllowGet);
            //    }
            //    else
            //    {
            //       return Json("Error",JsonRequestBehavior.AllowGet);
            //    }
            var EffactiveDateToString = DateTime.ParseExact(model.EffectiveFrom, inputFormat, CultureInfo.InvariantCulture);
            DateTime effDate = Convert.ToDateTime(EffactiveDateToString.ToString(outputFormat));

            Employee_WorkPattern employeeList = _employeePlannerMethod.getAllEmployeeWorkPattern().Where(x => x.EmployeeID == model.EmployeeId).OrderBy(x => x.Id).LastOrDefault();

            if (employeeList != null)
            {
                if (employeeList.EffectiveFrom.Date < effDate.Date)
                {
                    _employeePlannerMethod.SaveDataEmployeeWorkPattern(model, Userid);

                    var currentYear = DateTime.Now.Year;
                    var currentMonth = DateTime.Now.Month;
                    List<EmployeeProjectPlannerDayViewModel> returnModel = returnDayListTable(currentMonth, currentYear, model.EmployeeId,model.HolidayCountryID);
                    return PartialView("_partialListOfDayMonthYearWise", returnModel);
                }
                else
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var currentYear = DateTime.Now.Year;
                var currentMonth = DateTime.Now.Month;
                _employeePlannerMethod.SaveDataEmployeeWorkPattern(model, Userid);
                List<EmployeeProjectPlannerDayViewModel> returnModel = returnDayListTable(currentMonth, currentYear, model.EmployeeId,model.HolidayCountryID);
                return PartialView("_partialListOfDayMonthYearWise", returnModel);
            }

        }


        public ActionResult WorkPatternHistory(int EmployeeId)
        {
            List<EmployeeWorkPatternHistoryViewModel> Model = new List<EmployeeWorkPatternHistoryViewModel>();
            var details = _employeePlannerMethod.GetEmployeeWorkPatternListByEmployeeId(EmployeeId);
            foreach (var item in details)
            {
                if (item.NewWorkPatternID != null)
                {
                    EmployeeWorkPatternHistoryViewModel mm = new EmployeeWorkPatternHistoryViewModel();
                    var oldworkpatterndetails = _holidayNAbsenceMethod.getWorkPatternById(item.WorkPatternID);
                    var newworkpatterndetails = _holidayNAbsenceMethod.getWorkPatternById((int)item.NewWorkPatternID);
                    var name = _employeeMethod.getEmployeeById(item.UserIDLastModifiedBy);
                    mm.OldValue = oldworkpatterndetails.Name;
                    mm.ChangeDate = String.Format("{0:ddd, d MMM yyyy}", item.LastModified);
                    mm.ChangeBy = name.FirstName + " " + name.LastName;
                    mm.NewValue = newworkpatterndetails.Name;
                    Model.Add(mm);
                }

            }


            return PartialView("_partialHistoryWorkPatternView", Model);
        }

        #endregion

        #region Settings
        public ActionResult AnnualSettings(int Id)
        {
            EmployeeAllowanceSettings model = new EmployeeAllowanceSettings();
            var data = _employeeMethod.getEmployeeById(Id);
            model.Id = data.Id;
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Holiday Year List"))
            {
                model.holidayYearList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Measured List"))
            {
                model.MeasuredList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            foreach (var item in _RolesManagementMethod.GetEmployeeManagerList())
            {
                if (item.Id != Id)
                {
                    model.authorisationsList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
                }
            }
            if (data.AuthorisorEmployeeID != null && data.AuthorisorEmployeeID != "")
            {
                int AuthorisorEmpId = Convert.ToInt32(data.AuthorisorEmployeeID);
                var employeeData = _db.AspNetUsers.Where(x => x.Id == AuthorisorEmpId && x.Archived == false).FirstOrDefault();
                model.authorisationEmployeeName = employeeData.FirstName + employeeData.LastName + "-" + employeeData.SSOID;
            }
            if (data.AuthorisorEmployeeID != null)
            {
                model.AuthoriseUserId = data.AuthorisorEmployeeID;
            }
            if (data.TOIL != null)
            {
                model.TOIL = (int)data.TOIL;
            }
            if (data.InculudedCarriedOver != null)
            {
                model.CarriedOver = (int)data.InculudedCarriedOver;
            }
            if (data.HolidayYear != null)
            {
                model.HolidayYear = (int)data.HolidayYear;
            }
            if (data.MeasuredIn != null)
            {
                model.MeasuredIn = (int)data.MeasuredIn;
            }
            if (data.Thisyear != null)
            {
                model.Thisyear = (int)data.Thisyear;
            }
            if (data.Nextyear != null)
            {
                model.Nextyear = (int)data.Nextyear;
            }
            if (data.EntitlementIncludesPublicHoliday != null)
            {
                model.EntitlementIncludesPublicHoliday = (bool)data.EntitlementIncludesPublicHoliday;
            }
            if (data.AutoApproveHolidays != null)
            {
                model.AutoApproveHolidays = (bool)data.AutoApproveHolidays;
            }
            if (data.ExceedAllowance != null)
            {
                model.ExceedAllowance = (bool)data.ExceedAllowance;
            }
            if (data.UseVirtualClock != null)
            {
                model.UseVirtualClock = (bool)data.UseVirtualClock;
            }
            return PartialView("_PartialAllowanceSettingsView", model);
        }

        public ActionResult SaveAnnualSettings(EmployeeAllowanceSettings data)
        {
            AspNetUser model = _db.AspNetUsers.Where(x => x.Id == data.Id).FirstOrDefault();
            if (model != null)
            {
                model.AuthorisorEmployeeID = data.AuthoriseUserId;

                model.HolidayYear = data.HolidayYear;

                model.MeasuredIn = data.MeasuredIn;

                model.Thisyear = data.Thisyear;

                model.Nextyear = data.Nextyear;

                model.EntitlementIncludesPublicHoliday = data.EntitlementIncludesPublicHoliday;

                model.AutoApproveHolidays = data.AutoApproveHolidays;

                model.ExceedAllowance = data.ExceedAllowance;

                _db.SaveChanges();

            }
            return Index(data.Id);
        }

        public ActionResult AddEditTOIL(int Id)
        {

            EmployeeTOILModelView model = new EmployeeTOILModelView();
            var data = _db.AspNetUsers.Where(x => x.Id == Id).FirstOrDefault();
            if (data.TOIL != null)
            {
                model.Balance = (int)data.TOIL;
            }
            if (data.StartDate != null)
            {
                model.StartDate = String.Format("{0: dd-MM-yyyy}", data.StartDate);
            }
            model.ExpiryDate = String.Format("{0: dd-MM-yyyy}", DateTime.Now);
            model.AddEdit = false;
            model.Id = 0;

            return PartialView("_PartialAddEditTOILView", model);
        }

        public ActionResult SaveTOIL(EmployeeTOILModelView Model)
        {
            int userid = SessionProxy.UserId;
            var Save = _employeePlannerMethod.SaveEmployeeAddEditTOIL(Model, userid);
            return AnnualSettings(Model.EmployeeId);
        }
        #endregion

        #region TimeSheet

        public ActionResult AddEdit_TimeSheet(int Id, int Date, int Month, int Year)
        {
            EmployeeProjectPlanner_TimeSheetViewModel model = new EmployeeProjectPlanner_TimeSheetViewModel();
            DateTime date = new DateTime(Year, Month, Date);
            model.Id = Id;
            model.yearId = Year;
            model.monthId = Month;
            model.day = Date;

            if (Id > 0)
            {
                var data = _employeeProjectPlannerMethod.getTimeSheetById(Id);
                model.Date = String.Format("{0:dd-MM-yyy}", data.Date);
                model.Comment = data.Comments;

                var timeSheetDetail = _employeeProjectPlannerMethod.getAllTimeSheetDetail(Id);
                foreach (var timeSheet in timeSheetDetail)
                {
                    EmployeeProjectPlanner_TimeSheet_DetailViewModel detailModel = new EmployeeProjectPlanner_TimeSheet_DetailViewModel();
                    detailModel.CostCodeList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
                    foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Cost Code List"))
                    {
                        detailModel.CostCodeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
                    }
                    int CmpId = Convert.ToInt32(timeSheet.Customer);
                    if (CmpId != 0 && timeSheet.Customer != null)
                    {
                        var Employee = _db.AspNetUsers.Where(x => x.Id == CmpId).FirstOrDefault();
                        detailModel.Customer = Employee.FirstName + " " + Employee.LastName + " " + Employee.SSOID;
                        detailModel.CustomerId = timeSheet.Customer;
                    }
                    var projectList = _projectSettindsMethod.getAllList().Where(x => x.Archived == false).ToList();
                    detailModel.ProjectList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
                    foreach (var item in projectList)
                    {
                        detailModel.ProjectList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
                    }

                    detailModel.CustomerList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
                    foreach (var item in _employeeMethod.GetAllCoustomerEmployeeList().Where(x => x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList())
                    {
                        detailModel.CustomerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
                    }
                    
                    detailModel.AssetList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
                    foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Asset Type List"))
                    {
                        detailModel.AssetList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
                    }
                    for (int i = 1; i <= 60; i++)
                    {
                        detailModel.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                    }
                    for (int i = 1; i <= 24; i++)
                    {
                        detailModel.HoursList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                    }
                    detailModel.Id = timeSheet.Id;
                    detailModel.InTimeHr = timeSheet.InTimeHr;
                    detailModel.InTimeMin = timeSheet.InTimeMin;
                    detailModel.EndTimeHr = timeSheet.EndTimeHr;
                    detailModel.EndTimeMin = timeSheet.EndTimeMin;
                    detailModel.Project = timeSheet.Project;
                    detailModel.CostCode = timeSheet.CostCode;
                //    detailModel.Customer = timeSheet.Customer;
                    detailModel.Asset = timeSheet.Asset;
                    model.DetailList.Add(detailModel);
                }

                var timeSheetDoument = _employeeProjectPlannerMethod.getAllTimeSheetDocument(Id);
                foreach (var item in timeSheetDoument)
                {
                    EmployeeProjectPlanner_TimeSheet_DocumentsViewModel docModel = new EmployeeProjectPlanner_TimeSheet_DocumentsViewModel();
                    docModel.Id = item.Id;
                    docModel.originalName = item.OriginalName;
                    docModel.newName = item.NewName;
                    docModel.description = item.Description;
                    model.DocumentList.Add(docModel);
                }
            }
            else
            {
                model.Date = String.Format("{0:dd-MM-yyy}", date);
            }
            return PartialView("_partialAdd_TimeSheet", model);
        }
        public ActionResult getEmployee(int Id)
        {
            int userId = SessionProxy.UserId;

            EmployeeProjectPlanner_TimeSheet_DetailViewModel detailModel = new EmployeeProjectPlanner_TimeSheet_DetailViewModel();
            detailModel.CostCodeList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
             foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Cost Code List"))
             {
                 detailModel.CostCodeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
                }
                //model.ReportstoList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
                //model.AdditionalReportstoList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
                //model.HRResponsibleList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            //foreach (var item in _employeeMethod.GetAllCoustomerEmployeeList().Where(x => x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList())
            //{
            //    detailModel.CustomerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            //}
            var List = _db.AspNetUsers.Where(x => x.SSOID.Contains("C") && x.Archived == false).ToList();
            foreach (var item in List)
            {
                detailModel.CustomerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }

            return Json(detailModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWorkPatternData(EmployeePlanner_TimeSheetViewModel model)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<EmployeePlanner_TimeSheet_DetailViewModel> listDetail = js.Deserialize<List<EmployeePlanner_TimeSheet_DetailViewModel>>(model.jsonDetailList);
            model.DetailList = listDetail;
            string inputFormat = "dd-MM-yyyy";
            var wData = _employeeMethod.getWorkPatternById(model.EmployeeId).OrderByDescending(x => x.EffectiveFrom).FirstOrDefault();
            DateTime dt = DateTime.ParseExact(model.Date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            bool flag = false;
            foreach (var data in model.DetailList)
            {
                string strSt = data.InTimeHr + "." + data.InTimeMin;
                decimal st_Time = Convert.ToDecimal(strSt);
                string strEt = data.EndTimeHr + "." + data.EndTimeMin;
                decimal et_Time = Convert.ToDecimal(strEt);

                if (dt.DayOfWeek == DayOfWeek.Sunday)
                {
                    if ((st_Time >= wData.SundayStart && st_Time <= wData.SundayEnd) && (et_Time >= wData.SundayStart && et_Time <= wData.SundayEnd))
                    {
                        decimal TotalSundayTime = Convert.ToDecimal(wData.SundayStart) - Convert.ToDecimal(wData.SundayEnd);
                        if (wData.sundayBreak != null && wData.sundayBreak != 0)
                        {
                            decimal sundayBreakTime = Convert.ToDecimal(wData.sundayBreak);
                            decimal totalTime = TotalSundayTime - sundayBreakTime;
                        }
                        flag = true;
                    }
                    //if((st_Time>=wData.SundayStart&& st_Time>=wData.SundayEnd)&&(et_Time>=wData.SundayStart&& et_Time<=wData.SundayEnd))
                    //{
                    //    flag = false;
                    //}
                    //if((st_Time>=wData.SundayStart&&st_Time<=wData.SundayEnd)&&(et_Time>=wData.SundayEnd&&et_Time<=wData.SundayEnd))
                    //{
                    //    flag = false;
                    //}
                }
                else if (dt.DayOfWeek == DayOfWeek.Monday)
                {
                    if ((st_Time >= wData.MondayStart && st_Time <= wData.MondayEnd && (et_Time >= wData.MondayStart && et_Time <= wData.MondayEnd)))
                    {
                        decimal TotalMondayTime = Convert.ToDecimal(wData.MondayStart) - Convert.ToDecimal(wData.MondayEnd);
                        if (wData.mondayBreak != null && wData.mondayBreak != 0)
                        {
                            decimal mondayBreakTime = Convert.ToDecimal(wData.sundayBreak);
                            decimal totalTime = TotalMondayTime - mondayBreakTime;
                        }
                        flag = true;
                    }
                }
                else if (dt.DayOfWeek == DayOfWeek.Tuesday)
                {
                    if ((st_Time >= wData.TuesdayStart && st_Time <= wData.TuesdayEnd && (et_Time >= wData.TuesdayStart && et_Time <= wData.TuesdayEnd)))
                    {
                        decimal TotalTime = Convert.ToDecimal(wData.TuesdayStart) - Convert.ToDecimal(wData.TuesdayEnd);
                        if (wData.mondayBreak != null && wData.mondayBreak != 0)
                        {
                            decimal BreakTime = Convert.ToDecimal(wData.sundayBreak);
                            decimal totalTime = TotalTime - BreakTime;
                        }
                        flag = true;
                    }
                }
                else if (dt.DayOfWeek == DayOfWeek.Wednesday)
                {
                    if ((st_Time >= wData.WednessdayStart && st_Time <= wData.WednessdayEnd && (et_Time >= wData.WednessdayStart && et_Time <= wData.WednessdayEnd)))
                    {
                        decimal TotalTime = Convert.ToDecimal(wData.WednessdayStart) - Convert.ToDecimal(wData.WednessdayEnd);
                        if (wData.wednessdayBreak != null && wData.wednessdayBreak != 0)
                        {
                            decimal BreakTime = Convert.ToDecimal(wData.wednessdayBreak);
                            decimal totalTime = TotalTime - BreakTime;
                        }
                        flag = true;
                    }
                }
                else if (dt.DayOfWeek == DayOfWeek.Thursday)
                {
                    if ((st_Time >= wData.ThursdayStart && st_Time <= wData.ThursdayEnd && (et_Time >= wData.ThursdayStart && et_Time <= wData.ThursdayEnd)))
                    {
                        decimal TotalTime = Convert.ToDecimal(wData.ThursdayStart) - Convert.ToDecimal(wData.ThursdayEnd);
                        if (wData.thursdayBreak != null && wData.thursdayBreak != 0)
                        {
                            decimal BreakTime = Convert.ToDecimal(wData.thursdayBreak);
                            decimal totalTime = TotalTime - BreakTime;
                        }
                        flag = true;
                    }
                }
                else if (dt.DayOfWeek == DayOfWeek.Friday)
                {
                    if ((st_Time >= wData.FridayStart && st_Time <= wData.FridayEnd && (et_Time >= wData.FridayStart && et_Time <= wData.FridayEnd)))
                    {
                        decimal TotalTime = Convert.ToDecimal(wData.FridayStart) - Convert.ToDecimal(wData.FridayEnd);
                        if (wData.fridayBreak != null && wData.fridayBreak != 0)
                        {
                            decimal BreakTime = Convert.ToDecimal(wData.fridayBreak);
                            decimal totalTime = TotalTime - BreakTime;
                        }
                        flag = true;
                    }
                }
                else if (dt.DayOfWeek == DayOfWeek.Saturday)
                {
                    if ((st_Time >= wData.SaturdayStart && st_Time <= wData.SaturdayEnd && (et_Time >= wData.SaturdayStart && et_Time <= wData.SaturdayEnd)))
                    {
                        decimal TotalTime = Convert.ToDecimal(wData.SaturdayStart) - Convert.ToDecimal(wData.SaturdayEnd);
                        if (wData.SaturdayStart != null && wData.saturdayBreak != 0)
                        {
                            decimal BreakTime = Convert.ToDecimal(wData.saturdayBreak);
                            decimal totalTime = TotalTime - BreakTime;
                        }
                        flag = true;
                    }
                }
            }
                return Json(flag, JsonRequestBehavior.AllowGet);

        }

        public ActionResult AddEdit_TimeSheet_Detail()
        {
            EmployeeProjectPlanner_TimeSheet_DetailViewModel model = new EmployeeProjectPlanner_TimeSheet_DetailViewModel();

            model.CostCodeList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Cost Code List"))
            {
                model.CostCodeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }

            var projectList = _projectSettindsMethod.getAllList().Where(x => x.Archived == false).ToList();
            model.ProjectList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in projectList)
            {
                model.ProjectList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            //var customerList = _projectSettindsMethod.getAllList().Where(x => x.Archived == false).ToList();
            //foreach (var item in customerList)
            //{
            //    model.CustomerList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            //}
            var customerList = _projectSettindsMethod.getAllCustomer().Where(x => x.Archived == false).ToList();
            model.CustomerList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in customerList)
            {
                model.CustomerList.Add(new SelectListItem() { Text = item.FirstName + " " + item.LastName, Value = item.Id.ToString() });
            }

            model.AssetList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Asset Type List"))
            {
                model.AssetList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            for (int i = 1; i <= 60; i++)
            {
                model.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            for (int i = 1; i <= 24; i++)
            {
                model.HoursList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            return PartialView("_partialAdd_TimeSheet_Detail", model);
        }

        public ActionResult SaveData_TimeSheet(EmployeeProjectPlanner_TimeSheetViewModel model)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<EmployeeProjectPlanner_TimeSheet_DocumentsViewModel> listDocument = js.Deserialize<List<EmployeeProjectPlanner_TimeSheet_DocumentsViewModel>>(model.jsonDocumentList);
            model.DocumentList = listDocument;
            List<EmployeeProjectPlanner_TimeSheet_DetailViewModel> listDetail = js.Deserialize<List<EmployeeProjectPlanner_TimeSheet_DetailViewModel>>(model.jsonDetailList);
            model.DetailList = listDetail;


            _employeeProjectPlannerMethod.TimeSheet_SaveData(model, SessionProxy.UserId);

            List<EmployeeProjectPlannerDayViewModel> returnModel = returnDayListTable(model.monthId, model.yearId, model.EmployeeId,model.HolidayCountryID);
            return PartialView("_partialListOfDayMonthYearWise", returnModel);
        }

        [HttpPost]
        public ActionResult TimeSheetImageData()
        {
            string FilePath = string.Empty;
            string fileName = string.Empty;
            string originalFileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                FilePath = ConfigurationManager.AppSettings["ProjectPlanner_TimeSheet"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
        }

        #endregion

        #region Travel Leave

        public ActionResult AddEdit_Travel(int Id, int Date, int Month, int Year)
        {
            EmployeeProjectPlanner_TravelLeaveViewModel model = new EmployeeProjectPlanner_TravelLeaveViewModel();
            DateTime date = new DateTime(Year, Month, Date);
            model.Id = Id;
            model.yearId = Year;
            model.monthId = Month;
            model.day = Date;
            model.Hour = 0;
            model.Min = 0;

            model.FromCountryList = _employeeMethod.BindCountryDropdown();
            model.ToCountryList = _employeeMethod.BindCountryDropdown();

            model.ReasonForTravelList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Travel Leave Reason List"))
            {
                model.ReasonForTravelList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }

            model.TravelTypeList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Travel Type List"))
            {
                model.TravelTypeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }

            model.CostCodeList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Cost Code List"))
            {
                model.CostCodeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }

            var projectList = _projectSettindsMethod.getAllList().Where(x => x.Archived == false).ToList();
            model.ProjectList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in projectList)
            {
                model.ProjectList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }

            model.CustomerList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in _employeeMethod.GetAllCoustomerEmployeeList().Where(x => x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList())
            {
                model.CustomerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }
            for (int i = 1; i <= 60; i++)
            {
                model.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            for (int i = 1; i <= 24; i++)
            {
                model.HoursList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            if (Id > 0)
            {
                var data = _employeeProjectPlannerMethod.getTravelLeaveById(Id);

                if (data.FromCountryId > 0)
                {
                    model.FromStateList = _employeeMethod.BindStateDropdown((int)data.FromCountryId);
                    model.FromAirportList = _employeeMethod.BindAirportDropdown((int)data.FromCountryId);
                    model.FromCityList = _employeeMethod.BindCityDropdown((int)data.FromStateId);
                }
                else
                {
                    model.FromStateList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
                    model.FromAirportList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
                    model.FromCityList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
                }

                if (data.ToCountryId > 0)
                {
                    model.ToStateList = _employeeMethod.BindStateDropdown((int)data.ToCountryId);
                    model.ToAirportList = _employeeMethod.BindAirportDropdown((int)data.ToCountryId);
                    model.ToCityList = _employeeMethod.BindCityDropdown((int)data.ToStateId);
                }
                else
                {
                    model.ToStateList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
                    model.ToAirportList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
                    model.ToCityList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
                }

                model.FromCountryId = data.FromCountryId;
                model.FromStateId = data.FromStateId;
                model.FromTownId = data.FromTownId;
                model.FromAirportId = data.FromAirportId;
                model.ToCountryId = data.ToCountryId;
                model.ToStateId = data.ToStateId;
                model.ToTownId = data.ToTownId;
                model.ToAirportId = data.ToAirportId;
                model.ReasonForTravelId = data.ReasonForTravelId;
                model.StartDate = String.Format("{0:dd-MM-yyy}", data.StartDate);
                model.EndDate = String.Format("{0:dd-MM-yyy}", data.EndDate);
                model.Duration = data.Duration;
                model.IsLessThenADay = data.IsLessThenADay;
                if (model.IsLessThenADay == true)
                {
                    model.InTimeHr = (int)data.StartTimeHour;
                    model.InTimeMin = (int)data.StartTimeMin;
                    model.EndTimeHr = (int)data.EndTimeHour;
                    model.EndTimeMin = (int)data.EndTimeMin;
                    model.DurationHr = data.DurationHr;
                }
                model.Comment = data.Comment;
                model.Type = data.Type;
                model.Project = data.Project;
                int CmpId = Convert.ToInt32(data.Customer);
                if (CmpId != 0 && data.Customer != null)
                {

                    var Employee = _db.AspNetUsers.Where(x => x.Id == CmpId).FirstOrDefault();
                    model.Customer = Employee.FirstName + " " + Employee.LastName + " " + Employee.SSOID;
                    model.CustomerId = data.Customer;
                }
                //   model.Customer = data.Customer;
                model.CostCode = data.CostCode;

                var otherLeaveDoument = _employeeProjectPlannerMethod.getAllTravelLeaveDocument(Id);
                foreach (var item in otherLeaveDoument)
                {
                    EmployeeProjectPlanner_TravelLeave_DocumentsViewModel docModel = new EmployeeProjectPlanner_TravelLeave_DocumentsViewModel();
                    docModel.Id = item.Id;
                    docModel.originalName = item.OriginalName;
                    docModel.newName = item.NewName;
                    docModel.description = item.Description;
                    model.DocumentList.Add(docModel);
                }
            }
            else
            {
                model.StartDate = String.Format("{0:dd-MM-yyy}", date);
                model.EndDate = String.Format("{0:dd-MM-yyy}", date);
                model.FromStateList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
                model.ToStateList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
                model.FromCityList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
                model.ToCityList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
                model.FromAirportList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
                model.ToAirportList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
                model.Duration = 1;
            }
            return PartialView("_partialAdd_TravelLeave", model);
        }
        public ActionResult BindStateDropdown(int countryId)
        {
            try
            {
                var state = _employeeMethod.BindStateDropdown(countryId);
                return Json(state, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }
        public ActionResult BindCityDropdown(int stateId)
        {
            try
            {
                var city = _employeeMethod.BindCityDropdown(stateId);
                return Json(city, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }

        }
        public ActionResult BindAirportDropdown(int countryId)
        {
            try
            {
                var state = _employeeMethod.BindAirportDropdown(countryId);
                return Json(state, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }
        [HttpPost]
        public ActionResult TravelLeaveImageData()
        {
            string FilePath = string.Empty;
            string fileName = string.Empty;
            string originalFileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                FilePath = ConfigurationManager.AppSettings["ProjectPlanner_TravelLeave"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
        }
        public ActionResult SaveData_TravelLeave(EmployeeProjectPlanner_TravelLeaveViewModel model)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<EmployeeProjectPlanner_TravelLeave_DocumentsViewModel> listDocument = js.Deserialize<List<EmployeeProjectPlanner_TravelLeave_DocumentsViewModel>>(model.jsonDocumentList);
            model.DocumentList = listDocument;

            _employeeProjectPlannerMethod.TravelLeave_SaveData(model, SessionProxy.UserId);

            List<EmployeeProjectPlannerDayViewModel> returnModel = returnDayListTable(model.monthId, model.yearId, model.EmployeeId,model.HolidayCountryID);
            return PartialView("_partialListOfDayMonthYearWise", returnModel);
        }

        #endregion

        #region Scheduling

        public ActionResult AddEdit_Scheduling(int Id, int Date, int Month, int Year)
        {
            EmployeeProjectPlanner_Scheduling_DocumentsViewModel model = new EmployeeProjectPlanner_Scheduling_DocumentsViewModel();
            DateTime date = new DateTime(Year, Month, Date);
            model.Id = Id;
            model.yearId = Year;
            model.monthId = Month;
            model.day = Date;

            model.AssetList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Asset Type List"))
            {
                model.AssetList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }

            var projectList = _projectSettindsMethod.getAllList().Where(x => x.Archived == false).ToList();
            model.ProjectList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in projectList)
            {
                model.ProjectList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            for (int i = 1; i <= 60; i++)
            {
                model.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            for (int i = 1; i <= 24; i++)
            {
                model.HoursList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            model.CustomerList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });

            foreach (var item in _employeeMethod.GetAllCoustomerEmployeeList().Where(x => x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList())
            {
                model.CustomerList.Add(new SelectListItem() { Text = item.FirstName + " " + item.LastName + "-" + item.SSOID, Value = Convert.ToString(item.Id) });
            }
            if (Id > 0)
            {
                var details = _employeeProjectPlannerMethod.getSchedulingLeaveById(Id);
                model.Asset = details.AssetId;
                model.Comments = details.Comment;
                //  model.Customer = details.CustomerId;
                int CmpId = Convert.ToInt32(details.CustomerId);
                if (CmpId != 0 && details.CustomerId != null)
                {
                    var Employee = _db.AspNetUsers.Where(x => x.Id == CmpId).FirstOrDefault();
                    model.Customer = Employee.FirstName + " " + Employee.LastName + " " + Employee.SSOID;
                    model.CustomerId = details.CustomerId;
                }
                model.Project = details.ProjectId;
                model.IsDayOrMore = details.IsDayOrMore;
                model.StartDate = String.Format("{0:dd-MM-yyyy}", details.StartDate);
                if (details.IsDayOrMore == true)
                {
                    model.EndDate = String.Format("{0:dd-MM-yyyy}", details.EndDate);
                    model.DurationDays = details.DurationDays;
                }
                else
                {
                    model.InTimeHr = (int)details.InTimeHr;
                    model.InTimeMin = (int)details.InTimeMin;
                    model.EndTimeHr = (int)details.EndTimeHr;
                    model.EndTimeMin = (int)details.EndTimeMin;
                    model.DurationHr = details.DurationHr;
                }
            }
            else
            {
                model.StartDate = String.Format("{0:dd-MM-yyy}", date);
                model.EndDate = String.Format("{0:dd-MM-yyy}", date);
                model.DurationDays = 1;
                model.IsDayOrMore = true;
                model.IsLessThenADay = false;
            }
            return PartialView("_partialAdd_Scheduling", model);
        }

        public ActionResult SaveData_SchedulingLeaves(EmployeeProjectPlanner_Scheduling_DocumentsViewModel model)
        {
            var Userid = SessionProxy.UserId;
            var data = _employeeProjectPlannerMethod.SaveData_SchedulingLeave(model, Userid);
            if (data)
            {
                List<EmployeeProjectPlannerDayViewModel> returnModel = returnDayListTable(model.monthId, model.yearId, model.EmployeeId,model.HolidayCountryID);
                return PartialView("_partialListOfDayMonthYearWise", returnModel);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

        #region Print Pdf


        public ActionResult BindAbsencePdfView(int EmployeeId)
        {
            EmployeePrintPdfViewModel model = new EmployeePrintPdfViewModel();
            model.EmployeeId = EmployeeId;
            model.day = DateTime.Now.Day;
            model.yearId = DateTime.Now.Year;
            model.monthId = DateTime.Now.Month;
            model.StartDate = String.Format("{0: dd-MM-yyyy}", DateTime.Now);
            model.EndDate = String.Format("{0: dd-MM-yyyy}", DateTime.Now);
            model.Absence = true;
            model.StartDateError = false;
            model.EndDateError = false;
            return PartialView("_partialPrintPdfAbsenceLeavesView", model);
        }

        public ActionResult PrintPdfAbsenceLeaves(EmployeePrintPdfViewModel dataModel)
        {


            EmployeeHolidayLeavesPdfViewModel model = new EmployeeHolidayLeavesPdfViewModel();
            var EmployeeDetails = _employeeMethod.getEmployeeById(dataModel.EmployeeId);
            model.EmployeeId = dataModel.EmployeeId;
            DateTime currentDate = DateTime.Now;


            if (dataModel.Absence)
            {


                string newfileName = string.Format("" + EmployeeDetails.FirstName + " " + EmployeeDetails.LastName + " " + "Absence Report" + ".pdf", currentDate.Date);
                return new ActionAsPdf("AbsenceLeavesPDFView", dataModel)
                {
                    PageSize = Size.A4,
                    PageOrientation = Orientation.Landscape,
                    FileName = newfileName
                };

            }
            else
            {
                string newfileName = string.Format("" + EmployeeDetails.FirstName + " " + EmployeeDetails.LastName + " " + "Holiday Report" + ".pdf", currentDate.Date);
                return new ActionAsPdf("HolidayLeavesPDFView", dataModel)
                {
                    PageSize = Size.A4,
                    PageOrientation = Orientation.Landscape,
                    FileName = newfileName
                };

            }

        }

        public ActionResult AbsenceLeavesPDFView(EmployeeAbsenceLeavesPdfViewModel dataModel)
        {
            EmployeeAbsenceLeavesPdfViewModel mm = new EmployeeAbsenceLeavesPdfViewModel();
            var EmployeeDetails = _employeeMethod.getEmployeeById(dataModel.EmployeeId);
            var OtherLeaves = _employeePlannerMethod.getOtherLeaveByEmployeeId(dataModel.EmployeeId, dataModel.StartDate, dataModel.EndDate);
            var SickLeaves = _employeePlannerMethod.getSickLeaveByEmployeeId(dataModel.EmployeeId, dataModel.StartDate, dataModel.EndDate);
            mm.HireDate = String.Format("{0:dd-MMM-yyyy}", EmployeeDetails.StartDate);
            mm.EmployeeName = EmployeeDetails.FirstName + " " + EmployeeDetails.LastName;

            foreach (var item in OtherLeaves)
            {
                EmployeeAbsenceLeaveList list = new EmployeeAbsenceLeaveList();
                list.EndDate = String.Format("{0:dd-MM-yyyy}", EmployeeDetails.FixedTermEndDate);
                list.StartDate = String.Format("{0:dd-MM-yyyy}", item.StartDate);
                //dataModel.EndDate = String.Format("{0:dd-MM-yyyy}", item.EndDate);
                list.Duration = (decimal)item.Duration;
                list.Comments = item.Comment;
                mm.DetailList.Add(list);

            }
            foreach (var item in SickLeaves)
            {
                EmployeeAbsenceLeaveList list = new EmployeeAbsenceLeaveList();
                list.EndDate = String.Format("{0:dd-MM-yyyy}", EmployeeDetails.FixedTermEndDate);
                list.StartDate = String.Format("{0:dd-MM-yyyy}", item.StartDate);
                if (item.IsADayOrMore == true)
                {
                    list.EndDate = String.Format("{0:dd-MM-yyy}", item.EndDate);
                    list.Duration = (decimal)item.DurationDays;
                }
                else
                {
                    list.Duration = (decimal)item.DurationHours;
                }
                list.EmergencyLeave = (item.EmergencyLeave == true ? "Yes" : "No");
                list.Paid = (item.IsPaid == true ? "Yes" : "No");
                list.PaidAtOtherRate = (item.IsPaidatotherrate == true ? "Yes" : "No");
                list.UnPaid = (item.IsUnpaid == true ? "Yes" : "No");
                mm.DetailList.Add(list);
            }
            return View(mm);
        }

        public ActionResult HolidayLeavesPDFView(EmployeeHolidayLeavesPdfViewModel dataModel)
        {
            EmployeeHolidayLeavesPdfViewModel Model = new EmployeeHolidayLeavesPdfViewModel();
            var EmployeeDetails = _employeeMethod.getEmployeeById(dataModel.EmployeeId);
            Model.EmployeeStartDate = String.Format("{0:dd-MMM-yyyy}", EmployeeDetails.StartDate);
            Model.EmployeeName = EmployeeDetails.FirstName + " " + EmployeeDetails.LastName;
            Model.EmployeeEndDate = String.Format("{0:dd-MMM-yyyy}", EmployeeDetails.FixedTermEndDate);
            Model.Entitlement = (int)EmployeeDetails.Nextyear;
            Model.Remaining = (int)EmployeeDetails.Thisyear;


            return View(Model);
        }

        #endregion

        #region Public Holiday Country

        public ActionResult BindPublicHolidayTemplate(int CountryId)
        {

            EmployeePlanner_publicHolidayCounty model = new EmployeePlanner_publicHolidayCounty();
            var countryData = _holidayNAbsenceMethod.getCountryById(CountryId);
            model.Id = countryData.Id;
            model.Name = countryData.Name;

            foreach (var item in _holidayNAbsenceMethod.getHolidayListByCountryId(CountryId))
            {
                EmployeePlanner_publicHoliday_CountyWiseViewModel holidayModel = new EmployeePlanner_publicHoliday_CountyWiseViewModel();
                holidayModel.Id = item.Id;
                holidayModel.Name = item.Name;
                holidayModel.Date = String.Format("{0:dd-MM-yyy}", item.Date);
                model.holidayList.Add(holidayModel);
            }
            return PartialView("_partial_Effactive_PublicHoliday", model);
        }

        public ActionResult SavePublicHolidayTemplate(int CountryId, string EffactiveDate, int EmployeeId,int HolidayCountryID)
        {
            var EffactiveDateToString = DateTime.ParseExact(EffactiveDate, inputFormat, CultureInfo.InvariantCulture);
            DateTime effDate = Convert.ToDateTime(EffactiveDateToString.ToString(outputFormat));

            Employee_PublicHoliday employeeList = _employeePlannerMethod.publicHolidayList().Where(x => x.EmployeeId == EmployeeId).OrderBy(x => x.Id).LastOrDefault();
            if (employeeList != null)
            {
                if (employeeList.EffecitveFrom.Value.Date < effDate.Date)
                {
                    _employeePlannerMethod.SaveData_EffactivePublicHoliday(CountryId, EffactiveDate, EmployeeId, SessionProxy.UserId);

                    var currentYear = DateTime.Now.Year;
                    var currentMonth = DateTime.Now.Month;
                    List<EmployeeProjectPlannerDayViewModel> returnModel = returnDayListTable(currentMonth, currentYear, EmployeeId, HolidayCountryID);
                    return PartialView("_partialListOfDayMonthYearWise", returnModel);
                }
                else
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var currentYear = DateTime.Now.Year;
                var currentMonth = DateTime.Now.Month;
                _employeePlannerMethod.SaveData_EffactivePublicHoliday(CountryId, EffactiveDate, EmployeeId, SessionProxy.UserId);
                List<EmployeeProjectPlannerDayViewModel> returnModel = returnDayListTable(currentMonth, currentYear, EmployeeId, HolidayCountryID);
                return PartialView("_partialListOfDayMonthYearWise", returnModel);
            }

        }

        public ActionResult GetHolidayDetail(int Id, int Date, int Month, int Year)
        {
            var holiDay = _holidayNAbsenceMethod.getHolidayListByCountryId(Id);
            var currentHoliDay = holiDay.Where(x => x.Date.Value.Month == Month && x.Date.Value.Day == Date).FirstOrDefault();
            EmployeePlanner_publicHoliday_CountyWiseViewModel holidayModel = new EmployeePlanner_publicHoliday_CountyWiseViewModel();
            if (currentHoliDay != null)
            {
                holidayModel.Id = currentHoliDay.Id;
                holidayModel.Name = currentHoliDay.Name;
                holidayModel.Date = String.Format("{0:dd-MM-yyy}", currentHoliDay.Date);
            }
            return PartialView("_partialAdd_PublicHoliday", holidayModel);
        }

        #endregion


        #region Uplift

        public ActionResult AddEdit_Uplift(int Id, int Date, int Month, int Year)
        {
            EmployeeProjectPlanner_UpliftViewModel model = new EmployeeProjectPlanner_UpliftViewModel();
            DateTime date = new DateTime(Year, Month, Date);
            model.Id = Id;
            model.yearId = Year;
            model.monthId = Month;
            model.day = Date;
            var jobTitle = _otherSettingMethod.getAllSystemValueListByKeyName("Job Title List");
            model.JobTitleList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in jobTitle)
            {
                model.JobTitleList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var projectList = _projectSettindsMethod.getAllList().Where(x => x.Archived == false).ToList();
            model.ProjectList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in projectList)
            {
                model.ProjectList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }

            model.CustomerList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in _employeeMethod.GetAllCoustomerEmployeeList().Where(x => x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList())
            {
                model.CustomerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }

            if (Id > 0)
            {
                var data = _employeeProjectPlannerMethod.getUpliftById(Id);
                model.Date = String.Format("{0:dd-MM-yyy}", data.Date);
                model.Comment = data.Comments;
                model.UpliftPostionId = (int)data.UpliftPostionId;

                model.ProjectId = (int)data.ProjectId;


             //   model.CustomerId = data.CustomerId;

                int CmpId = Convert.ToInt32(data.CustomerId);
                if (CmpId != 0 && data.CustomerId != null)
                {
                    var Employee = _db.AspNetUsers.Where(x => x.Id == CmpId).FirstOrDefault();
                    model.Customer = Employee.FirstName + " " + Employee.LastName + " " + Employee.SSOID;
                    model.CustomerId = data.CustomerId;
                }

                var UpliftDetail = _employeeProjectPlannerMethod.getAllUpliftDetail(Id);
                foreach (var Uplift in UpliftDetail)
                {
                    EmployeeProjectPlanner_Uplift_DetailViewModel detailModel = new EmployeeProjectPlanner_Uplift_DetailViewModel();
                    detailModel.Id = Uplift.Id;
                    detailModel.InTimeHr = Uplift.InTimeHr;
                    detailModel.InTimeMin = Uplift.InTimeMin;
                    detailModel.EndTimeHr = Uplift.OutTimeHr;
                    detailModel.EndTimeMin = Uplift.OutTimeMin;
                    if (Uplift.WorkerRate != null)
                    {
                        detailModel.WorkerRate = (decimal)Uplift.WorkerRate;
                    }
                    if (Uplift.WorkerRatePer != null)
                    {
                        detailModel.WorkerRatePer = (decimal)Uplift.WorkerRatePer;
                    }
                    if (Uplift.CustomerRate != null)
                    {
                        detailModel.CustomerRate = (decimal)Uplift.CustomerRate;
                    }
                    if (Uplift.CustomerRatePer != null)
                    {
                        detailModel.CustomerRatePer = (decimal)Uplift.CustomerRatePer;
                    }
                    for (int i = 1; i <= 60; i++)
                    {
                        detailModel.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                    }
                    for (int i = 1; i <= 24; i++)
                    {
                        detailModel.HoursList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                    }
                    model.DetailList.Add(detailModel);
                }

                var UpliftDoument = _employeeProjectPlannerMethod.getAllUpliftDocument(Id);
                foreach (var item in UpliftDoument)
                {
                    EmployeeProjectPlanner_Uplift_DocumentsViewModel docModel = new EmployeeProjectPlanner_Uplift_DocumentsViewModel();
                    docModel.Id = item.Id;
                    docModel.originalName = item.OriginalName;
                    docModel.newName = item.NewName;
                    docModel.description = item.Description;
                    model.DocumentList.Add(docModel);
                }
            }
            else
            {
                model.Date = String.Format("{0:dd-MM-yyy}", date);
                model.CustomerId = "0";
                model.ProjectId = 0;

            }
            return PartialView("_partialAdd_Uplift", model);
        }

        public ActionResult AddEdit_Uplift_Detail()
        {
            EmployeeProjectPlanner_Uplift_DetailViewModel model = new EmployeeProjectPlanner_Uplift_DetailViewModel();
            for (int i = 1; i <= 60; i++)
            {
                model.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            for (int i = 1; i <= 24; i++)
            {
                model.HoursList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            return PartialView("_partialAdd_Uplift_Detail", model);
        }

        public ActionResult SaveData_Uplift(EmployeeProjectPlanner_UpliftViewModel model)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<EmployeeProjectPlanner_Uplift_DocumentsViewModel> listDocument = js.Deserialize<List<EmployeeProjectPlanner_Uplift_DocumentsViewModel>>(model.jsonDocumentList);
            model.DocumentList = listDocument;
            List<EmployeeProjectPlanner_Uplift_DetailViewModel> listDetail = js.Deserialize<List<EmployeeProjectPlanner_Uplift_DetailViewModel>>(model.jsonDetailList);
            model.DetailList = listDetail;


            _employeeProjectPlannerMethod.Uplift_SaveData(model, SessionProxy.UserId);

            List<EmployeeProjectPlannerDayViewModel> returnModel = returnDayListTable(model.monthId, model.yearId, model.EmployeeId,model.HolidayCountryID);
            return PartialView("_partialListOfDayMonthYearWise", returnModel);
        }

        [HttpPost]
        public ActionResult UpliftImageData()
        {
            string FilePath = string.Empty;
            string fileName = string.Empty;
            string originalFileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                FilePath = ConfigurationManager.AppSettings["ProjectPlanner_Uplift"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
        }

        #endregion
    }
}