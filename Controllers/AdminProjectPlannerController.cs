using HRTool.CommanMethods;
using HRTool.DataModel;
using HRTool.Models.Admin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRTool.Models.Resources;
using HRTool.CommanMethods.Resources;
using HRTool.CommanMethods.Settings;
using static HRTool.CommanMethods.Enums;
using HRTool.CommanMethods.Admin;
using ClosedXML.Excel;
using System.IO;
using System.Data;
using System.Configuration;
using System.Web.Script.Serialization;

namespace HRTool.Controllers
{

    [CustomAuthorize]
    public class AdminProjectPlannerController : Controller
    {
        #region Constant
        EvolutionEntities _db = new EvolutionEntities();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        ProjectSettindsMethod _projectSettindsMethod = new ProjectSettindsMethod();
        EmployeeMethod _employeeMethod = new EmployeeMethod();
        AdminProjectPlanner _adminProjectPlanner = new AdminProjectPlanner();
        EmployeeProjectPlannerMethod _employeeProjectPlannerMethod = new EmployeeProjectPlannerMethod();
        HolidayNAbsenceMethod _holidayNAbsenceMethod = new HolidayNAbsenceMethod();
        #endregion
        EmployeePlannerMethod _employeePlannerMethod = new EmployeePlannerMethod();
        //
        // GET: /AdminProjectPlanner/
        //WorkPattern
        public Boolean getEmployeeWorkPatternById(int EmpId, DateTime date)
        {
            var workPattenList = _holidayNAbsenceMethod.getAllWorkPattern();
            var employeeWorkPatternList = _employeePlannerMethod.getAllEmployeeWorkPattern().Where(x => x.EmployeeID == EmpId && x.EffectiveTo==null && x.NewWorkPatternID==null).FirstOrDefault();
            Boolean workPatternExist = false;
            if (workPattenList.Count > 0)
            {
                if (employeeWorkPatternList!=null)
                {
                    
                        //Employee_WorkPattern saveWorkPattern = employeeWorkPatternList.FirstOrDefault();
                        if (date.Date >= employeeWorkPatternList.EffectiveFrom.Date)
                        {
                            workPatternExist = true;
                        }
                        else { workPatternExist = false; }
                    
                }
                return workPatternExist;
            }
            else
            {
                return workPatternExist;
            }
        }
        public Boolean getWorkPattenLeavebyId(int EmpId, DateTime date)
        {
            var workPattenList = _holidayNAbsenceMethod.getAllWorkPattern();
            var employeeWorkPatternList = _employeePlannerMethod.getAllEmployeeWorkPattern().Where(x => x.EmployeeID == EmpId && x.EffectiveTo == null && x.NewWorkPatternID == null).FirstOrDefault();
            var rotatingWorkPatternList = _holidayNAbsenceMethod.allWorkPatternDetail();
            EmployeeProjectPlannerDayViewModel dayModel = new EmployeeProjectPlannerDayViewModel();
            int maxRotaingCount = 0;
            int currentRotatingWeekDays = 0;
            if (employeeWorkPatternList != null)
            {
                #region Work Pattern
                dayModel.day = date.Day;
                dayModel.DayName = date.DayOfWeek.ToString();
                dayModel.monthId = date.Month;
                dayModel.yearId = date.Year;
              
                if (date.Date >= employeeWorkPatternList.EffectiveFrom.Date)
                {
                    dayModel.isWorkPatternExist = true;
                    WorkPattern workPatternDetail = workPattenList.Where(x => x.Id == employeeWorkPatternList.WorkPatternID).FirstOrDefault();
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
                            var rotatingWorkpatternDetail = rotatingWorkPatternList.Where(x => x.WorkPatternID == employeeWorkPatternList.WorkPatternID).OrderBy(x => x.Id).ToList();
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
                                    if (rotatingWorkpatternDetail != null)
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
            return dayModel.isWorkPatternLeaveTaken;
            #endregion
            }
            else
            {
                return dayModel.isWorkPatternLeaveTaken;
            }
        }
       //Index
        public ActionResult Index(AdminProjectPlannerRequestModel _requestModel)
        {
            int empId = SessionProxy.UserId;
            AdminProjectPlannerViewModel _AdminProjectPlannerViewModel = new AdminProjectPlannerViewModel();
            var admindata = _db.AspNetUserRoles.Where(x => x.UserId == empId && x.RoleId == 1).ToList();
            var data = _db.AspNetUsers.Where(x => x.Id == empId && x.SSOID.StartsWith("W")).FirstOrDefault();
            var employeeRelation = _db.EmployeeRelations.Where(x => x.UserID == empId && x.IsActive == true).FirstOrDefault();

            try
            {
                _requestModel.IsSchedule = true;
                //AdminProjectPlannerViewModel _AdminProjectPlannerViewModel = returnResultList(_requestModel);
                if (admindata.Count > 0 && admindata != null)
                {
                    _requestModel.IsTimeSheet = true;
                    _AdminProjectPlannerViewModel.listOfBusiness = _db.Businesses.Where(x => x.Archived == false).Select(xx => new KeyValue()
                    {
                        Key = xx.Id,
                        Value = xx.Name
                    }).ToList();                   
                    _AdminProjectPlannerViewModel.Flag = 0;
                }
                else
                {
                    _AdminProjectPlannerViewModel.listOfBusiness = _db.Businesses.Where(x => x.Archived == false && x.Id == employeeRelation.BusinessID).Select(xx => new KeyValue()
                    {
                        Key = xx.Id,
                        Value = xx.Name
                    }).ToList();
                    _AdminProjectPlannerViewModel.listOfDivision = _db.Divisions.Where(x => x.Archived == false && x.Id == employeeRelation.DivisionID).Select(xx => new KeyValue()
                    {
                        Key = xx.Id,
                        Value = xx.Name
                    }).ToList();
                    _AdminProjectPlannerViewModel.listOfFunction = _db.Functions.Where(x => x.Archived == false && x.Id == employeeRelation.FunctionID).Select(xx => new KeyValue()
                    {
                        Key = xx.Id,
                        Value = xx.Name
                    }).ToList();
                    _AdminProjectPlannerViewModel.listOfPool = _db.Pools.Where(x => x.Archived == false && x.Id == employeeRelation.PoolID).Select(xx => new KeyValue()
                    {
                        Key = xx.Id,
                        Value = xx.Name
                    }).ToList();
                    _AdminProjectPlannerViewModel.Flag = 1;
                    List<KeyValue> listOfKeyValueEmp = new List<KeyValue>();
                    var EmpRelation = _db.EmployeeRelations.Where(x => x.FunctionID == employeeRelation.FunctionID && x.PoolID == employeeRelation.PoolID && x.IsActive == true && x.Reportsto == empId || x.UserID==empId).ToList();
                    foreach (var item in EmpRelation)
                    {
                        KeyValue KeyValue = new KeyValue();
                        var AspNetUser = _db.AspNetUsers.Where(x => x.Id == item.UserID).FirstOrDefault();
                        KeyValue.Key = AspNetUser.Id;
                        KeyValue.Value = AspNetUser.FirstName + ' ' + AspNetUser.LastName;
                        listOfKeyValueEmp.Add(KeyValue);
                    }
                    _AdminProjectPlannerViewModel.listOfEmployee = listOfKeyValueEmp;
                }
                _AdminProjectPlannerViewModel.listOfProject = _db.Projects.Where(x => x.Archived == false).Select(xx => new KeyValue()
                {
                    Key = xx.Id,
                    Value = xx.Name
                }).ToList();
                List<KeyValue> listOfKeyValue = new List<KeyValue>();
                for (int i = 0; i < 10; i++)
                {
                    KeyValue _KeyValue = new KeyValue();
                    _KeyValue.Key = DateTime.Now.AddYears(-i).Year;
                    _KeyValue.Value = Convert.ToString(DateTime.Now.AddYears(-i).Year);
                    listOfKeyValue.Add(_KeyValue);
                }
                _AdminProjectPlannerViewModel.ListOfYear = listOfKeyValue;
                var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                _AdminProjectPlannerViewModel.ListOfSelectedYear_Month.Add(new SelectListItem() { Text = "--Select Month", Value = "0" });
                for (int i = 0; i < months.Length-1; i++)
                {
                    _AdminProjectPlannerViewModel.ListOfSelectedYear_Month.Add(new SelectListItem() { Text= months[i] , Value=i.ToString()});
                }                
                if (_requestModel.Year == 0 || _requestModel.Year == null)
                {
                    _requestModel.Year = DateTime.Now.Year;
                }
                if(_requestModel.MonthId==0 || _requestModel.MonthId==null)
                {
                    _requestModel.MonthId = DateTime.Now.Month;
                }
                _AdminProjectPlannerViewModel.AdminPlannerYearModel = ListOfMonth(_requestModel.Year, _requestModel);
                //_AdminProjectPlannerViewModel.AdminPlannerMonth_yearModel = ListOfMonth_Year(_requestModel.Year,_requestModel.MonthId, _requestModel);
                return View(_AdminProjectPlannerViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetResultByFilter_Month(AdminProjectPlannerRequestModel _requestModel)
        {
            AdminProjectPlannerMonthModel DayModel = new AdminProjectPlannerMonthModel();
            DayModel = ListOfMonth_Year(_requestModel.Year, _requestModel.MonthId, _requestModel);
            return PartialView("_ProjectDayPartial", DayModel);
        }
        public ActionResult GetResultByFilter(AdminProjectPlannerRequestModel _requestModel)
        {
            AdminProjectPlannerYearModel YearModel = new AdminProjectPlannerYearModel();
            YearModel = ListOfMonth(_requestModel.Year, _requestModel);
            return PartialView("_ProjectCalenderPartial", YearModel);
        }
        public AdminProjectPlannerMonthModel ListOfMonth_Year(int year,int month, AdminProjectPlannerRequestModel _requestModel)
        {
            AdminProjectPlannerMonthModel listOfMothn = new AdminProjectPlannerMonthModel();
            try
            {
                listOfMothn.YearId = year;
                listOfMothn.MonthId = _requestModel.MonthId;
                listOfMothn.IsSchedule = false;
                listOfMothn.IsTimeSheet = false;
                listOfMothn.IsTravel = false;
                listOfMothn.IsUplift = false;
                listOfMothn.MonthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(_requestModel.MonthId);
                if (_requestModel.IsSchedule)
                {
                    listOfMothn.listOfEmployeeScheduling = (from Scheduling in _db.Employee_ProjectPlanner_Scheduling
                                                            join aspnetUser in _db.AspNetUsers on Scheduling.EmployeeId equals aspnetUser.Id
                                                            join relation in _db.EmployeeRelations on aspnetUser.Id equals relation.UserID
                                                            where relation.IsActive == true &&aspnetUser.Archived == false && Scheduling.StartDate != null ? Scheduling.StartDate.Value.Year == _requestModel.Year : false || Scheduling.EndDate != null ? Scheduling.EndDate.Value.Year == _requestModel.Year : false
                                                            select new
                                                            {
                                                                Id = Scheduling.Id,
                                                                StartDate = Scheduling.StartDate,
                                                                EndDate = Scheduling.EndDate,
                                                                EmployeeId = Scheduling.EmployeeId,
                                                                BusinessId = relation.BusinessID,
                                                                DivisionId = relation.DivisionID,
                                                                PoolId = relation.PoolID,
                                                                FunctionId = relation.FunctionID,
                                                                ProjectId=Scheduling.ProjectId
                                                            }).ToList()
                                 .Select(x => new SchedulingProjectPlannerViewModel()
                                 {
                                     Id = x.Id,
                                     StartDate = x.StartDate,
                                     EndDate = x.EndDate,
                                     EmployeeId = x.EmployeeId,
                                     BusinessId = x.BusinessId,
                                     DivisionId = x.DivisionId,
                                     PoolId = x.PoolId,
                                     FunctionId = x.FunctionId,
                                     ProjectId=x.ProjectId
                                 }).ToList();
                    if (_requestModel.BusinessId > 0)
                    {
                        listOfMothn.listOfEmployeeScheduling = listOfMothn.listOfEmployeeScheduling.Where(x => x.BusinessId == _requestModel.BusinessId).ToList();
                    }
                    if (_requestModel.DivisionId > 0)
                    {
                        listOfMothn.listOfEmployeeScheduling = listOfMothn.listOfEmployeeScheduling.Where(x => x.DivisionId == _requestModel.DivisionId).ToList();
                    }
                    if (_requestModel.PoolId > 0)
                    {
                        listOfMothn.listOfEmployeeScheduling = listOfMothn.listOfEmployeeScheduling.Where(x => x.PoolId == _requestModel.PoolId).ToList();
                    }
                    if (_requestModel.FunctionId > 0)
                    {
                        listOfMothn.listOfEmployeeScheduling = listOfMothn.listOfEmployeeScheduling.Where(x => x.FunctionId == _requestModel.FunctionId).ToList();
                    }
                    if (_requestModel.ResourceId > 0)
                    {
                        listOfMothn.listOfEmployeeScheduling = listOfMothn.listOfEmployeeScheduling.Where(x => x.EmployeeId == _requestModel.ResourceId).ToList();
                    }
                    if (_requestModel.ProjectId > 0)
                    {
                        listOfMothn.listOfEmployeeScheduling = listOfMothn.listOfEmployeeScheduling.Where(x => x.ProjectId == _requestModel.ProjectId).ToList();

                    }
                    if (_requestModel.Year != null)
                    {
                        //listOfYearModel.listOfEmployeeScheduling = listOfYearModel.listOfEmployeeScheduling.Where(x => x.Year == _requestModel.Year).ToList();
                    }
                    listOfMothn.IsSchedule = true;
                }
                else if (_requestModel.IsTimeSheet)
                {
                    //listOfYearModel.listOfEmployeeTimeSheet = _db.Employee_ProjectPlanner_TimeSheet.Where(x => x.Date != null ? x.Date.Value.Year == _requestModel.Year : false).ToList();

                    listOfMothn.listOfEmployeeTimeSheet = (from timeSheet in _db.Employee_ProjectPlanner_TimeSheet
                                                               join aspnetUser in _db.AspNetUsers on timeSheet.EmployeeId equals aspnetUser.Id
                                                               join relation in _db.EmployeeRelations on aspnetUser.Id equals relation.UserID
                                                               join timesheetDetail in _db.Employee_ProjectPlanner_TimeSheet_Detail on timeSheet.Id equals timesheetDetail.TimeSheetId                 
                                                               where relation.IsActive == true && aspnetUser.Archived == false && timeSheet.Date != null ? timeSheet.Date.Value.Year == _requestModel.Year : false
                                                               select new
                                                               {
                                                                   Id = timeSheet.Id,
                                                                   Date = timeSheet.Date,
                                                                   EmployeeId = timeSheet.EmployeeId,
                                                                   BusinessId = relation.BusinessID,
                                                                   DivisionId = relation.DivisionID,
                                                                   PoolId = relation.PoolID,
                                                                   FunctionId = relation.FunctionID,
                                                                   ProjectId=timesheetDetail.Project
                                                               }).ToList()
                                                                .Select(x => new TimeSheetProjectPlannerViewModel()
                                                                {
                                                                    Id = x.Id,
                                                                    Date = x.Date,
                                                                    EmployeeId = x.EmployeeId,
                                                                    BusinessId = x.BusinessId,
                                                                    DivisionId = x.DivisionId,
                                                                    PoolId = x.PoolId,
                                                                    FunctionId = x.FunctionId,
                                                                    ProjectId=x.ProjectId
                                                                }).ToList();

                    if (_requestModel.BusinessId > 0)
                    {
                        listOfMothn.listOfEmployeeTimeSheet = listOfMothn.listOfEmployeeTimeSheet.Where(x => x.BusinessId == _requestModel.BusinessId).ToList();
                    }
                    if (_requestModel.DivisionId > 0)
                    {
                        listOfMothn.listOfEmployeeTimeSheet = listOfMothn.listOfEmployeeTimeSheet.Where(x => x.DivisionId == _requestModel.DivisionId).ToList();
                    }
                    if (_requestModel.PoolId > 0)
                    {
                        listOfMothn.listOfEmployeeTimeSheet = listOfMothn.listOfEmployeeTimeSheet.Where(x => x.PoolId == _requestModel.PoolId).ToList();
                    }
                    if (_requestModel.FunctionId > 0)
                    {
                        listOfMothn.listOfEmployeeTimeSheet = listOfMothn.listOfEmployeeTimeSheet.Where(x => x.FunctionId == _requestModel.FunctionId).ToList();
                    }
                    if (_requestModel.ResourceId > 0)
                    {
                        listOfMothn.listOfEmployeeTimeSheet = listOfMothn.listOfEmployeeTimeSheet.Where(x => x.EmployeeId == _requestModel.ResourceId).ToList();
                    }
                    if (_requestModel.ProjectId > 0)
                    {
                        listOfMothn.listOfEmployeeTimeSheet = listOfMothn.listOfEmployeeTimeSheet.Where(x => x.ProjectId == _requestModel.ProjectId).ToList();
                    }
                    if (_requestModel.Year != null)
                    {

                    }

                    listOfMothn.IsTimeSheet = true;
                }
                else if (_requestModel.IsTravel)
                {
                    //listOfYearModel.listOfEmployeeTravelLeave = _db.Employee_ProjectPlanner_TravelLeave.Where(x => x.StartDate != null ? x.StartDate.Value.Year == _requestModel.Year : false || x.EndDate != null ? x.EndDate.Value.Year == _requestModel.Year : false).ToList();

                    listOfMothn.listOfEmployeeTravelLeave = (from Travel in _db.Employee_ProjectPlanner_TravelLeave
                                                                 join aspnetUser in _db.AspNetUsers on Travel.EmployeeId equals aspnetUser.Id
                                                                 join relation in _db.EmployeeRelations on aspnetUser.Id equals relation.UserID
                                                                 where relation.IsActive == true && aspnetUser.Archived == false && Travel.StartDate != null ? Travel.StartDate.Value.Year == _requestModel.Year : false || Travel.EndDate != null ? Travel.EndDate.Value.Year == _requestModel.Year : false
                                                                 select new
                                                                 {
                                                                     Id = Travel.Id,
                                                                     StartDate = Travel.StartDate,
                                                                     EndDate = Travel.EndDate,
                                                                     EmployeeId = Travel.EmployeeId,
                                                                     BusinessId = relation.BusinessID,
                                                                     DivisionId = relation.DivisionID,
                                                                     PoolId = relation.PoolID,
                                                                     FunctionId = relation.FunctionID,
                                                                     ProjectId=Travel.Project
                                                                 }).ToList()
                                .Select(x => new TravelProjectPlannerViewModel()
                                {
                                    Id = x.Id,
                                    StartDate = x.StartDate,
                                    EndDate = x.EndDate,
                                    EmployeeId = x.EmployeeId,
                                    BusinessId = x.BusinessId,
                                    DivisionId = x.DivisionId,
                                    PoolId = x.PoolId,
                                    FunctionId = x.FunctionId,
                                    ProjectId=x.ProjectId
                                }).ToList();

                    if (_requestModel.BusinessId > 0)
                    {
                        listOfMothn.listOfEmployeeTravelLeave = listOfMothn.listOfEmployeeTravelLeave.Where(x => x.BusinessId == _requestModel.BusinessId).ToList();
                    }
                    if (_requestModel.DivisionId > 0)
                    {
                        listOfMothn.listOfEmployeeTravelLeave = listOfMothn.listOfEmployeeTravelLeave.Where(x => x.DivisionId == _requestModel.DivisionId).ToList();
                    }
                    if (_requestModel.PoolId > 0)
                    {
                        listOfMothn.listOfEmployeeTravelLeave = listOfMothn.listOfEmployeeTravelLeave.Where(x => x.PoolId == _requestModel.PoolId).ToList();
                    }
                    if (_requestModel.FunctionId > 0)
                    {
                        listOfMothn.listOfEmployeeTravelLeave = listOfMothn.listOfEmployeeTravelLeave.Where(x => x.FunctionId == _requestModel.FunctionId).ToList();
                    }
                    if (_requestModel.ResourceId > 0)
                    {
                        listOfMothn.listOfEmployeeTravelLeave = listOfMothn.listOfEmployeeTravelLeave.Where(x => x.EmployeeId == _requestModel.ResourceId).ToList();
                    }
                    if (_requestModel.ProjectId > 0)
                    {
                        listOfMothn.listOfEmployeeTravelLeave = listOfMothn.listOfEmployeeTravelLeave.Where(x => x.ProjectId == _requestModel.ProjectId).ToList();
                    }
                    if (_requestModel.Year != null)
                    {

                    }

                    listOfMothn.IsTravel = true;
                }
                else if (_requestModel.IsUplift)
                {
                    //listOfYearModel.listOfEmployeeUpliftLeave = _db.Employee_ProjectPlanner_Uplift.Where(x => x.Date != null ? x.Date.Year == _requestModel.Year : false).ToList();

                    listOfMothn.listOfEmployeeUpliftLeave = (from uplift in _db.Employee_ProjectPlanner_Uplift
                                                                 join aspnetUser in _db.AspNetUsers on uplift.EmployeeId equals aspnetUser.Id
                                                                 join relation in _db.EmployeeRelations on aspnetUser.Id equals relation.UserID
                                                                 where relation.IsActive == true && aspnetUser.Archived == false && uplift.Date != null ? uplift.Date.Year == _requestModel.Year : false
                                                                 select new
                                                                 {
                                                                     Id = uplift.Id,
                                                                     Date = uplift.Date,
                                                                     EmployeeId = uplift.EmployeeId,
                                                                     BusinessId = relation.BusinessID,
                                                                     DivisionId = relation.DivisionID,
                                                                     PoolId = relation.PoolID,
                                                                     FunctionId = relation.FunctionID,
                                                                     ProjectId=uplift.ProjectId
                                                                 }).ToList()
                                                                .Select(x => new UpliftProjectPlannerViewModel()
                                                                {
                                                                    Id = x.Id,
                                                                    Date = x.Date,
                                                                    EmployeeId = x.EmployeeId,
                                                                    BusinessId = x.BusinessId,
                                                                    DivisionId = x.DivisionId,
                                                                    PoolId = x.PoolId,
                                                                    FunctionId = x.FunctionId,
                                                                    ProjectId=x.ProjectId
                                                                }).ToList();

                    if (_requestModel.BusinessId > 0)
                    {
                        listOfMothn.listOfEmployeeUpliftLeave = listOfMothn.listOfEmployeeUpliftLeave.Where(x => x.BusinessId == _requestModel.BusinessId).ToList();
                    }
                    if (_requestModel.DivisionId > 0)
                    {
                        listOfMothn.listOfEmployeeUpliftLeave = listOfMothn.listOfEmployeeUpliftLeave.Where(x => x.DivisionId == _requestModel.DivisionId).ToList();
                    }
                    if (_requestModel.PoolId > 0)
                    {
                        listOfMothn.listOfEmployeeUpliftLeave = listOfMothn.listOfEmployeeUpliftLeave.Where(x => x.PoolId == _requestModel.PoolId).ToList();
                    }
                    if (_requestModel.FunctionId > 0)
                    {
                        listOfMothn.listOfEmployeeUpliftLeave = listOfMothn.listOfEmployeeUpliftLeave.Where(x => x.FunctionId == _requestModel.FunctionId).ToList();
                    }
                    if (_requestModel.ResourceId > 0)
                    {
                        listOfMothn.listOfEmployeeUpliftLeave = listOfMothn.listOfEmployeeUpliftLeave.Where(x => x.EmployeeId == _requestModel.ResourceId).ToList();
                    }
                    if (_requestModel.ProjectId > 0)
                    {
                        listOfMothn.listOfEmployeeUpliftLeave = listOfMothn.listOfEmployeeUpliftLeave.Where(x => x.ProjectId == _requestModel.ProjectId).ToList();
                    }
                    if (_requestModel.Year != null)
                    {

                    }

                    listOfMothn.IsUplift = true;
                }
                int totalDay = DateTime.DaysInMonth(year, month);
                for (int j = 1; j <= totalDay; j++)
                {
                    AdminProjectPlannerDayModel listOfDayModel = new AdminProjectPlannerDayModel();
                    listOfDayModel.DayId = j;
                    listOfDayModel.YearId = year;
                    listOfDayModel.MonthId = month;
                    var date = new DateTime(year, month, j);
                    listOfDayModel.DayName = date.DayOfWeek.ToString();

                    switch (listOfDayModel.DayName)
                    {
                        case "Sunday":
                            listOfDayModel.StartDay = 0;
                            break;
                        case "Monday":
                            listOfDayModel.StartDay = 1;
                            break;
                        case "Tuesday":
                            listOfDayModel.StartDay = 2;
                            break;
                        case "Wednesday":
                            listOfDayModel.StartDay = 3;
                            break;
                        case "Thursday":
                            listOfDayModel.StartDay = 4;
                            break;
                        case "Friday":
                            listOfDayModel.StartDay = 5;
                            break;
                        case "Saturday":
                            listOfDayModel.StartDay = 6;
                            break;
                        default:
                            listOfDayModel.StartDay = 0;
                            break;
                    }
                    if (_requestModel.IsSchedule)
                    {
                        var schedule = listOfMothn.listOfEmployeeScheduling.Where(x => (x.StartDate != null ? date.Date >= x.StartDate.Value.Date : false) && (x.EndDate != null ? date.Date <= x.EndDate.Value.Date : false)).FirstOrDefault();
                        if (schedule != null)
                        {
                            listOfDayModel.LeaveId = schedule.Id;
                            listOfDayModel.IsSheduling = true;
                        }
                    }
                    else if (_requestModel.IsTimeSheet)
                    {
                        var timeSheet = listOfMothn.listOfEmployeeTimeSheet.Where(x => x.Date != null ? x.Date.Value.Date == date.Date : false).FirstOrDefault();
                        if (timeSheet != null)
                        {
                            listOfDayModel.IsTimeSheet = true;
                        }
                    }
                    else if (_requestModel.IsTravel)
                    {
                        var Travel = listOfMothn.listOfEmployeeTravelLeave.Where(x => (x.StartDate != null ? date.Date >= x.StartDate.Value.Date : false) && (x.EndDate != null ? date.Date <= x.EndDate.Value.Date : false)).FirstOrDefault();
                        if (Travel != null)
                        {
                            listOfDayModel.IsTravel = true;
                        }
                    }
                    else if (_requestModel.IsUplift)
                    {
                        var uplift = listOfMothn.listOfEmployeeUpliftLeave.Where(x => x.Date != null ? x.Date.Date == date.Date : false).FirstOrDefault();
                        if (uplift != null)
                        {
                            listOfDayModel.IsUplift = true;
                        }
                    }
                    listOfMothn.listOfDayModel.Add(listOfDayModel);                    
                }
                return listOfMothn;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        public AdminProjectPlannerYearModel ListOfMonth(int year, AdminProjectPlannerRequestModel _requestModel)
        {
            try
            {
                AdminProjectPlannerYearModel listOfYearModel = new AdminProjectPlannerYearModel();
                listOfYearModel.YearId = year;
                int TotalMonth = 12;
                int EmpID = SessionProxy.UserId;
                var data = _db.AspNetUserRoles.Where(x => x.UserId == EmpID && x.RoleId == 1).ToList();
                listOfYearModel.IsSchedule = false;
                listOfYearModel.IsTimeSheet = false;
                listOfYearModel.IsTravel = false;
                listOfYearModel.IsUplift = false;
                if (_requestModel.IsSchedule)
                {
                    if (data != null && data.Count != 0)
                    {
                        listOfYearModel.listOfEmployeeScheduling = (from Scheduling in _db.Employee_ProjectPlanner_Scheduling
                                                                    join aspnetUser in _db.AspNetUsers on Scheduling.EmployeeId equals aspnetUser.Id
                                                                    join relation in _db.EmployeeRelations on aspnetUser.Id equals relation.UserID
                                                                    where relation.IsActive == true && aspnetUser.Archived == false && Scheduling.StartDate != null ? Scheduling.StartDate.Value.Year == _requestModel.Year : false && Scheduling.EndDate != null ? Scheduling.EndDate.Value.Year == _requestModel.Year : false
                                                                    select new
                                                                    {
                                                                        Id = Scheduling.Id,
                                                                        StartDate = Scheduling.StartDate,
                                                                        EndDate = Scheduling.EndDate,
                                                                        EmployeeId = Scheduling.EmployeeId,
                                                                        BusinessId = relation.BusinessID,
                                                                        DivisionId = relation.DivisionID,
                                                                        PoolId = relation.PoolID,
                                                                        FunctionId = relation.FunctionID,
                                                                        ProjectId = Scheduling.ProjectId
                                                                    }).ToList()
                               .Select(x => new SchedulingProjectPlannerViewModel()
                               {
                                   Id = x.Id,
                                   StartDate = x.StartDate,
                                   EndDate = x.EndDate,
                                   EmployeeId = x.EmployeeId,
                                   BusinessId = x.BusinessId,
                                   DivisionId = x.DivisionId,
                                   PoolId = x.PoolId,
                                   FunctionId = x.FunctionId,
                                   ProjectId = x.ProjectId
                               }).ToList();
                    }
                    else
                    {
                        listOfYearModel.listOfEmployeeScheduling = (from Scheduling in _db.Employee_ProjectPlanner_Scheduling
                                                                    join aspnetUser in _db.AspNetUsers on Scheduling.EmployeeId equals aspnetUser.Id
                                                                    join relation in _db.EmployeeRelations on aspnetUser.Id equals relation.UserID
                                                                    where relation.Reportsto==SessionProxy.UserId || Scheduling.EmployeeId==SessionProxy.UserId && relation.IsActive == true && aspnetUser.Archived == false && Scheduling.StartDate != null ? Scheduling.StartDate.Value.Year == _requestModel.Year : false && Scheduling.EndDate != null ? Scheduling.EndDate.Value.Year == _requestModel.Year : false
                                                                    select new
                                                                    {
                                                                        Id = Scheduling.Id,
                                                                        StartDate = Scheduling.StartDate,
                                                                        EndDate = Scheduling.EndDate,
                                                                        EmployeeId = Scheduling.EmployeeId,
                                                                        BusinessId = relation.BusinessID,
                                                                        DivisionId = relation.DivisionID,
                                                                        PoolId = relation.PoolID,
                                                                        FunctionId = relation.FunctionID,
                                                                        ProjectId = Scheduling.ProjectId
                                                                    }).ToList()
                               .Select(x => new SchedulingProjectPlannerViewModel()
                               {
                                   Id = x.Id,
                                   StartDate = x.StartDate,
                                   EndDate = x.EndDate,
                                   EmployeeId = x.EmployeeId,
                                   BusinessId = x.BusinessId,
                                   DivisionId = x.DivisionId,
                                   PoolId = x.PoolId,
                                   FunctionId = x.FunctionId,
                                   ProjectId = x.ProjectId
                               }).ToList();

                    }
                    //listOfYearModel.listOfEmployeeScheduling = _db.Employee_ProjectPlanner_Scheduling.Where(x => x.StartDate != null ? x.StartDate.Value.Year == _requestModel.Year : false || x.EndDate != null ? x.EndDate.Value.Year == _requestModel.Year : false).ToList();
                    if (_requestModel.BusinessId > 0)
                    {
                        listOfYearModel.listOfEmployeeScheduling = listOfYearModel.listOfEmployeeScheduling.Where(x => x.BusinessId == _requestModel.BusinessId).ToList();
                    }
                    if (_requestModel.DivisionId > 0)
                    {
                        listOfYearModel.listOfEmployeeScheduling = listOfYearModel.listOfEmployeeScheduling.Where(x => x.DivisionId == _requestModel.DivisionId).ToList();
                    }
                    if (_requestModel.PoolId > 0)
                    {
                        listOfYearModel.listOfEmployeeScheduling = listOfYearModel.listOfEmployeeScheduling.Where(x => x.PoolId == _requestModel.PoolId).ToList();
                    }
                    if (_requestModel.FunctionId > 0)
                    {
                        listOfYearModel.listOfEmployeeScheduling = listOfYearModel.listOfEmployeeScheduling.Where(x => x.FunctionId == _requestModel.FunctionId).ToList();
                    }
                    if (_requestModel.ResourceId > 0)
                    {
                        listOfYearModel.listOfEmployeeScheduling = listOfYearModel.listOfEmployeeScheduling.Where(x => x.EmployeeId == _requestModel.ResourceId).ToList();
                    }
                    if (_requestModel.ProjectId > 0)
                    {
                        listOfYearModel.listOfEmployeeScheduling = listOfYearModel.listOfEmployeeScheduling.Where(x => x.ProjectId == _requestModel.ProjectId).ToList();
                    }
                    if (_requestModel.Year != null)
                    {
                        //listOfYearModel.listOfEmployeeScheduling = listOfYearModel.listOfEmployeeScheduling.Where(x => x.Year == _requestModel.Year).ToList();
                    }

                    listOfYearModel.IsSchedule = true;
                    
                }
                else if (_requestModel.IsTimeSheet)
                {
                    //listOfYearModel.listOfEmployeeTimeSheet = _db.Employee_ProjectPlanner_TimeSheet.Where(x => x.Date != null ? x.Date.Value.Year == _requestModel.Year : false).ToList();
                    if (data.Count > 0 && data != null)
                    {
                        listOfYearModel.listOfEmployeeTimeSheet = (from timeSheet in _db.Employee_ProjectPlanner_TimeSheet
                                                                   join aspnetUser in _db.AspNetUsers on timeSheet.EmployeeId equals aspnetUser.Id
                                                                   join relation in _db.EmployeeRelations on aspnetUser.Id equals relation.UserID
                                                                   join timesheetDetail in _db.Employee_ProjectPlanner_TimeSheet_Detail on timeSheet.Id equals timesheetDetail.TimeSheetId
                                                                   where relation.IsActive == true && aspnetUser.Archived == false && timeSheet.Date != null ? timeSheet.Date.Value.Year == _requestModel.Year : false
                                                                   select new
                                                                   {
                                                                       Id = timeSheet.Id,
                                                                       Date = timeSheet.Date,
                                                                       EmployeeId = timeSheet.EmployeeId,
                                                                       BusinessId = relation.BusinessID,
                                                                       DivisionId = relation.DivisionID,
                                                                       PoolId = relation.PoolID,
                                                                       FunctionId = relation.FunctionID,
                                                                       ProjectId = timesheetDetail.Project
                                                                   }).ToList()
                                                              .Select(x => new TimeSheetProjectPlannerViewModel()
                                                              {
                                                                  Id = x.Id,
                                                                  Date = x.Date,
                                                                  EmployeeId = x.EmployeeId,
                                                                  BusinessId = x.BusinessId,
                                                                  DivisionId = x.DivisionId,
                                                                  PoolId = x.PoolId,
                                                                  FunctionId = x.FunctionId,
                                                                  ProjectId = x.ProjectId
                                                              }).ToList();
                    }
                    else
                    {
                        listOfYearModel.listOfEmployeeTimeSheet = (from timeSheet in _db.Employee_ProjectPlanner_TimeSheet
                                                                   join aspnetUser in _db.AspNetUsers on timeSheet.EmployeeId equals aspnetUser.Id
                                                                   join relation in _db.EmployeeRelations on aspnetUser.Id equals relation.UserID
                                                                   join timesheetDetail in _db.Employee_ProjectPlanner_TimeSheet_Detail on timeSheet.Id equals timesheetDetail.TimeSheetId
                                                                   where relation.Reportsto==SessionProxy.UserId || timeSheet.EmployeeId==SessionProxy.UserId && relation.IsActive == true && aspnetUser.Archived == false && timeSheet.Date != null ? timeSheet.Date.Value.Year == _requestModel.Year : false
                                                                   select new
                                                                   {
                                                                       Id = timeSheet.Id,
                                                                       Date = timeSheet.Date,
                                                                       EmployeeId = timeSheet.EmployeeId,
                                                                       BusinessId = relation.BusinessID,
                                                                       DivisionId = relation.DivisionID,
                                                                       PoolId = relation.PoolID,
                                                                       FunctionId = relation.FunctionID,
                                                                       ProjectId = timesheetDetail.Project
                                                                   }).ToList()
                                                                    .Select(x => new TimeSheetProjectPlannerViewModel()
                                                                    {
                                                                        Id = x.Id,
                                                                        Date = x.Date,
                                                                        EmployeeId = x.EmployeeId,
                                                                        BusinessId = x.BusinessId,
                                                                        DivisionId = x.DivisionId,
                                                                        PoolId = x.PoolId,
                                                                        FunctionId = x.FunctionId,
                                                                        ProjectId = x.ProjectId
                                                                    }).ToList();
                    }
                    if (_requestModel.BusinessId > 0)
                    {
                        listOfYearModel.listOfEmployeeTimeSheet = listOfYearModel.listOfEmployeeTimeSheet.Where(x => x.BusinessId == _requestModel.BusinessId).ToList();
                    }
                    if (_requestModel.DivisionId > 0)
                    {
                        listOfYearModel.listOfEmployeeTimeSheet = listOfYearModel.listOfEmployeeTimeSheet.Where(x => x.DivisionId == _requestModel.DivisionId).ToList();
                    }
                    if (_requestModel.PoolId > 0)
                    {
                        listOfYearModel.listOfEmployeeTimeSheet = listOfYearModel.listOfEmployeeTimeSheet.Where(x => x.PoolId == _requestModel.PoolId).ToList();
                    }
                    if (_requestModel.FunctionId > 0)
                    {
                        listOfYearModel.listOfEmployeeTimeSheet = listOfYearModel.listOfEmployeeTimeSheet.Where(x => x.FunctionId == _requestModel.FunctionId).ToList();
                    }
                    if (_requestModel.ResourceId > 0)
                    {
                        listOfYearModel.listOfEmployeeTimeSheet = listOfYearModel.listOfEmployeeTimeSheet.Where(x => x.EmployeeId == _requestModel.ResourceId).ToList();
                    }
                    if (_requestModel.ProjectId > 0)
                    {
                        listOfYearModel.listOfEmployeeTimeSheet = listOfYearModel.listOfEmployeeTimeSheet.Where(x => x.ProjectId == _requestModel.ProjectId).ToList();
                    }
                    if (_requestModel.Year != null)
                    {

                    }

                    listOfYearModel.IsTimeSheet = true;
                }
                else if (_requestModel.IsTravel)
                {
                    //listOfYearModel.listOfEmployeeTravelLeave = _db.Employee_ProjectPlanner_TravelLeave.Where(x => x.StartDate != null ? x.StartDate.Value.Year == _requestModel.Year : false || x.EndDate != null ? x.EndDate.Value.Year == _requestModel.Year : false).ToList();
                    if (data.Count > 0 && data != null)
                    {
                        listOfYearModel.listOfEmployeeTravelLeave = (from Travel in _db.Employee_ProjectPlanner_TravelLeave
                                                                     join aspnetUser in _db.AspNetUsers on Travel.EmployeeId equals aspnetUser.Id
                                                                     join relation in _db.EmployeeRelations on aspnetUser.Id equals relation.UserID
                                                                     where relation.IsActive == true && aspnetUser.Archived == false && Travel.StartDate != null ? Travel.StartDate.Value.Year == _requestModel.Year : false && Travel.EndDate != null ? Travel.EndDate.Value.Year == _requestModel.Year : false
                                                                     select new
                                                                     {
                                                                         Id = Travel.Id,
                                                                         StartDate = Travel.StartDate,
                                                                         EndDate = Travel.EndDate,
                                                                         EmployeeId = Travel.EmployeeId,
                                                                         BusinessId = relation.BusinessID,
                                                                         DivisionId = relation.DivisionID,
                                                                         PoolId = relation.PoolID,
                                                                         FunctionId = relation.FunctionID,
                                                                         ProjectId = Travel.Project
                                                                     }).ToList()
                                .Select(x => new TravelProjectPlannerViewModel()
                                {
                                    Id = x.Id,
                                    StartDate = x.StartDate,
                                    EndDate = x.EndDate,
                                    EmployeeId = x.EmployeeId,
                                    BusinessId = x.BusinessId,
                                    DivisionId = x.DivisionId,
                                    PoolId = x.PoolId,
                                    FunctionId = x.FunctionId,
                                    ProjectId = x.ProjectId
                                }).ToList();
                    }
                    else
                    {
                        listOfYearModel.listOfEmployeeTravelLeave = (from Travel in _db.Employee_ProjectPlanner_TravelLeave
                                                                     join aspnetUser in _db.AspNetUsers on Travel.EmployeeId equals aspnetUser.Id
                                                                     join relation in _db.EmployeeRelations on aspnetUser.Id equals relation.UserID
                                                                     where relation.Reportsto==SessionProxy.UserId||Travel.EmployeeId==SessionProxy.UserId && relation.IsActive == true && aspnetUser.Archived == false && Travel.StartDate != null ? Travel.StartDate.Value.Year == _requestModel.Year : false && Travel.EndDate != null ? Travel.EndDate.Value.Year == _requestModel.Year : false
                                                                     select new
                                                                     {
                                                                         Id = Travel.Id,
                                                                         StartDate = Travel.StartDate,
                                                                         EndDate = Travel.EndDate,
                                                                         EmployeeId = Travel.EmployeeId,
                                                                         BusinessId = relation.BusinessID,
                                                                         DivisionId = relation.DivisionID,
                                                                         PoolId = relation.PoolID,
                                                                         FunctionId = relation.FunctionID,
                                                                         ProjectId = Travel.Project
                                                                     }).ToList()
                               .Select(x => new TravelProjectPlannerViewModel()
                               {
                                   Id = x.Id,
                                   StartDate = x.StartDate,
                                   EndDate = x.EndDate,
                                   EmployeeId = x.EmployeeId,
                                   BusinessId = x.BusinessId,
                                   DivisionId = x.DivisionId,
                                   PoolId = x.PoolId,
                                   FunctionId = x.FunctionId,
                                   ProjectId = x.ProjectId
                               }).ToList();
                    }
                    if (_requestModel.BusinessId > 0)
                    {
                        listOfYearModel.listOfEmployeeTravelLeave = listOfYearModel.listOfEmployeeTravelLeave.Where(x => x.BusinessId == _requestModel.BusinessId).ToList();
                    }
                    if (_requestModel.DivisionId > 0)
                    {
                        listOfYearModel.listOfEmployeeTravelLeave = listOfYearModel.listOfEmployeeTravelLeave.Where(x => x.DivisionId == _requestModel.DivisionId).ToList();
                    }
                    if (_requestModel.PoolId > 0)
                    {
                        listOfYearModel.listOfEmployeeTravelLeave = listOfYearModel.listOfEmployeeTravelLeave.Where(x => x.PoolId == _requestModel.PoolId).ToList();
                    }
                    if (_requestModel.FunctionId > 0)
                    {
                        listOfYearModel.listOfEmployeeTravelLeave = listOfYearModel.listOfEmployeeTravelLeave.Where(x => x.FunctionId == _requestModel.FunctionId).ToList();
                    }
                    if (_requestModel.ResourceId > 0)
                    {
                        listOfYearModel.listOfEmployeeTravelLeave = listOfYearModel.listOfEmployeeTravelLeave.Where(x => x.EmployeeId == _requestModel.ResourceId).ToList();
                    }
                    if (_requestModel.ProjectId > 0)
                    {
                        listOfYearModel.listOfEmployeeTravelLeave = listOfYearModel.listOfEmployeeTravelLeave.Where(x => x.ProjectId == _requestModel.ProjectId).ToList();
                    }
                    if (_requestModel.Year != null)
                    {
                    }
                    listOfYearModel.IsTravel = true;
                }
                else if (_requestModel.IsUplift)
                {
                    //listOfYearModel.listOfEmployeeUpliftLeave = _db.Employee_ProjectPlanner_Uplift.Where(x => x.Date != null ? x.Date.Year == _requestModel.Year : false).ToList();
                    if (data.Count > 0 && data != null)
                    {
                        listOfYearModel.listOfEmployeeUpliftLeave = (from uplift in _db.Employee_ProjectPlanner_Uplift
                                                                     join aspnetUser in _db.AspNetUsers on uplift.EmployeeId equals aspnetUser.Id
                                                                     join relation in _db.EmployeeRelations on aspnetUser.Id equals relation.UserID
                                                                     where relation.IsActive == true && aspnetUser.Archived == false && uplift.Date != null ? uplift.Date.Year == _requestModel.Year : false
                                                                     select new
                                                                     {
                                                                         Id = uplift.Id,
                                                                         Date = uplift.Date,
                                                                         EmployeeId = uplift.EmployeeId,
                                                                         BusinessId = relation.BusinessID,
                                                                         DivisionId = relation.DivisionID,
                                                                         PoolId = relation.PoolID,
                                                                         FunctionId = relation.FunctionID,
                                                                         ProjectId = uplift.ProjectId
                                                                     }).ToList()
                                                                .Select(x => new UpliftProjectPlannerViewModel()
                                                                {
                                                                    Id = x.Id,
                                                                    Date = x.Date,
                                                                    EmployeeId = x.EmployeeId,
                                                                    BusinessId = x.BusinessId,
                                                                    DivisionId = x.DivisionId,
                                                                    PoolId = x.PoolId,
                                                                    FunctionId = x.FunctionId,
                                                                    ProjectId = x.ProjectId
                                                                }).ToList();
                    }
                    else {
                        listOfYearModel.listOfEmployeeUpliftLeave = (from uplift in _db.Employee_ProjectPlanner_Uplift
                                                                     join aspnetUser in _db.AspNetUsers on uplift.EmployeeId equals aspnetUser.Id
                                                                     join relation in _db.EmployeeRelations on aspnetUser.Id equals relation.UserID
                                                                     where relation.Reportsto==SessionProxy.UserId||uplift.EmployeeId==SessionProxy.UserId && relation.IsActive == true && aspnetUser.Archived == false && uplift.Date != null ? uplift.Date.Year == _requestModel.Year : false
                                                                     select new
                                                                     {
                                                                         Id = uplift.Id,
                                                                         Date = uplift.Date,
                                                                         EmployeeId = uplift.EmployeeId,
                                                                         BusinessId = relation.BusinessID,
                                                                         DivisionId = relation.DivisionID,
                                                                         PoolId = relation.PoolID,
                                                                         FunctionId = relation.FunctionID,
                                                                         ProjectId = uplift.ProjectId
                                                                     }).ToList()
                                                                    .Select(x => new UpliftProjectPlannerViewModel()
                                                                    {
                                                                        Id = x.Id,
                                                                        Date = x.Date,
                                                                        EmployeeId = x.EmployeeId,
                                                                        BusinessId = x.BusinessId,
                                                                        DivisionId = x.DivisionId,
                                                                        PoolId = x.PoolId,
                                                                        FunctionId = x.FunctionId,
                                                                        ProjectId = x.ProjectId
                                                                    }).ToList();
                    }
                    if (_requestModel.BusinessId > 0)
                    {
                        listOfYearModel.listOfEmployeeUpliftLeave = listOfYearModel.listOfEmployeeUpliftLeave.Where(x => x.BusinessId == _requestModel.BusinessId).ToList();
                    }
                    if (_requestModel.DivisionId > 0)
                    {
                        listOfYearModel.listOfEmployeeUpliftLeave = listOfYearModel.listOfEmployeeUpliftLeave.Where(x => x.DivisionId == _requestModel.DivisionId).ToList();
                    }
                    if (_requestModel.PoolId > 0)
                    {
                        listOfYearModel.listOfEmployeeUpliftLeave = listOfYearModel.listOfEmployeeUpliftLeave.Where(x => x.PoolId == _requestModel.PoolId).ToList();
                    }
                    if (_requestModel.FunctionId > 0)
                    {
                        listOfYearModel.listOfEmployeeUpliftLeave = listOfYearModel.listOfEmployeeUpliftLeave.Where(x => x.FunctionId == _requestModel.FunctionId).ToList();
                    }
                    if (_requestModel.ResourceId > 0)
                    {
                        listOfYearModel.listOfEmployeeUpliftLeave = listOfYearModel.listOfEmployeeUpliftLeave.Where(x => x.EmployeeId == _requestModel.ResourceId).ToList();
                    }
                    if (_requestModel.ProjectId > 0)
                    {
                        listOfYearModel.listOfEmployeeUpliftLeave = listOfYearModel.listOfEmployeeUpliftLeave.Where(x => x.ProjectId == _requestModel.ProjectId).ToList();
                    }
                    if (_requestModel.Year != null)
                    {

                    }
                    listOfYearModel.IsUplift = true;
                }

                for (int i = 1; i <= TotalMonth; i++)
                {
                    AdminProjectPlannerMonthModel listofMonthModel = new AdminProjectPlannerMonthModel();
                    listofMonthModel.MonthId = i;
                    listofMonthModel.YearId = year;
                    listofMonthModel.MonthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(i);
                    listOfYearModel.listOfMonthModel.Add(listofMonthModel);
                    if (listOfYearModel.IsTimeSheet == true)
                    {
                        var tdata = listOfYearModel.listOfEmployeeTimeSheet.Select(x => x.Date).ToList();
                        var disTimesheet = tdata.Distinct().ToList();
                        if (disTimesheet.Count > 0)
                        {
                            decimal totalTimesheet = 0;
                            foreach (var item in disTimesheet)
                            {
                                if (item.Value.Month == i)
                                {
                                    totalTimesheet = totalTimesheet + 1;
                                }
                            }
                            listofMonthModel.TotalTimesheet = totalTimesheet;
                        }
                    }
                    if (listOfYearModel.IsUplift == true)
                    {
                        var tdata = listOfYearModel.listOfEmployeeUpliftLeave.Select(x => x.Date).ToList();
                        var disuplift = tdata.Distinct().ToList();
                        if (disuplift.Count > 0)
                        {
                            decimal totalUplift = 0;
                            foreach (var item in disuplift)
                            {
                                if (item.Date.Month == i)
                                {
                                    totalUplift = totalUplift + 1;
                                }
                            }
                            listofMonthModel.TotalUplift = totalUplift;
                        }
                    }
                    int totalDay = DateTime.DaysInMonth(year, i);
                    for (int j = 1; j <= totalDay; j++)
                    {
                        AdminProjectPlannerDayModel listOfDayModel = new AdminProjectPlannerDayModel();
                        listOfDayModel.DayId = j;
                        listOfDayModel.YearId = year;
                        listOfDayModel.MonthId = i;
                        var date = new DateTime(year, i, j);
                        listOfDayModel.DayName = date.DayOfWeek.ToString();

                        switch (listOfDayModel.DayName)
                        {
                            case "Sunday":
                                listOfDayModel.StartDay = 0;
                                break;
                            case "Monday":
                                listOfDayModel.StartDay = 1;
                                break;
                            case "Tuesday":
                                listOfDayModel.StartDay = 2;
                                break;
                            case "Wednesday":
                                listOfDayModel.StartDay = 3;
                                break;
                            case "Thursday":
                                listOfDayModel.StartDay = 4;
                                break;
                            case "Friday":
                                listOfDayModel.StartDay = 5;
                                break;
                            case "Saturday":
                                listOfDayModel.StartDay = 6;
                                break;
                            default:
                                listOfDayModel.StartDay = 0;
                                break;
                        }

                        if (_requestModel.IsSchedule)
                        {
                            var schedule = listOfYearModel.listOfEmployeeScheduling.Where(x => (x.StartDate != null ? date.Date >= x.StartDate.Value.Date : false) && (x.EndDate != null ? date.Date <= x.EndDate.Value.Date : false)).FirstOrDefault();
                            if (schedule != null)
                            {
                                listOfDayModel.LeaveId = schedule.Id;
                                listOfDayModel.IsSheduling = true;
                            }
                            listofMonthModel.IsSchedule = true;
                        }
                        else if (_requestModel.IsTimeSheet)
                        {
                            var timeSheet = listOfYearModel.listOfEmployeeTimeSheet.Where(x => x.Date != null ? x.Date.Value.Date == date.Date : false).FirstOrDefault();
                            if (timeSheet != null)
                            {
                                listOfDayModel.IsTimeSheet = true;
                            }
                        }
                        else if (_requestModel.IsTravel)
                        {
                            var Travel = listOfYearModel.listOfEmployeeTravelLeave.Where(x => (x.StartDate != null ? date.Date >= x.StartDate.Value.Date : false) && (x.EndDate != null ? date.Date <= x.EndDate.Value.Date : false)).FirstOrDefault();
                            if (Travel != null)
                            {
                                listOfDayModel.IsTravel = true;
                            }
                        }
                        else if (_requestModel.IsUplift)
                        {
                            var uplift = listOfYearModel.listOfEmployeeUpliftLeave.Where(x => x.Date != null ? x.Date.Date == date.Date : false).FirstOrDefault();
                            if (uplift != null)
                            {
                                listOfDayModel.IsUplift = true;
                            }
                        }
                        
                        listofMonthModel.listOfDayModel.Add(listOfDayModel);
                    }
                }
                return listOfYearModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        // Scheduling
        public ActionResult EditResourceScheduling(int EmpId,int Year,int Month,int Day)
        {
            EmployeeProjectPlanner_Scheduling_DocumentsViewModel model = new EmployeeProjectPlanner_Scheduling_DocumentsViewModel();
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
            for (int i = 0; i < 60; i++)
            {
                model.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            for (int i = 0; i < 24; i++)
            {
                model.HoursList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            foreach (var item in _employeeMethod.GetAllCoustomerEmployeeList().Where(x => x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList())
            {
                model.CustomerList.Add(new SelectListItem() { Text = item.FirstName + " " + item.LastName + "-" + item.SSOID, Value = Convert.ToString(item.Id) });
            }
            DateTime expiry = new DateTime(Year,Month,Day);
            string dat = Convert.ToString(expiry);
            string inputFormat = "dd-MM-yyyy";
            var s = expiry.ToString("dd-MM-yyyy");
            DateTime dt = DateTime.ParseExact(s.ToString(), inputFormat, CultureInfo.InvariantCulture);
            var data = _adminProjectPlanner.getSchedulingDataByDate(dt, EmpId);

            foreach (var details in data)
            {
                model.Asset = details.AssetId;
                model.Comments = details.Comment;
                int CmpId = Convert.ToInt32(details.CustomerId);
                if (CmpId != 0 && details.CustomerId != null && details.CustomerId!="")
                {
                    var Employee = _db.AspNetUsers.Where(x => x.Id == CmpId).FirstOrDefault();
                    if (Employee != null)
                    {
                        if (Employee.FirstName != "" && Employee.FirstName != null && Employee.LastName != "" && Employee.LastName != null)
                        {
                            model.Customer = Employee.FirstName + " " + Employee.LastName + "-" + Employee.SSOID;
                            model.CustomerId = details.CustomerId;
                        }
                    }
                }
                if(EmpId!=null)
                {
                    var Resource = _db.AspNetUsers.Where(x => x.Id == EmpId).FirstOrDefault();
                    model.EmployeeId = EmpId;
                    model.EmployeeName = Resource.FirstName + " " + Resource.LastName + "-" + Resource.SSOID;
                }
                //  model.Customer = details.CustomerId;
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
            return PartialView("_partialAdminProjectPlannerAdd_Scheduling",model);
        }
        public ActionResult getAdminProjectPlannerData(string isSchedule,string isTimesheet,int Year,int month,int Day,int isScheduling,int isMonth)
        {
            EmployeeProjectPlanner_Scheduling_DocumentsViewModel model = new EmployeeProjectPlanner_Scheduling_DocumentsViewModel();
            bool b = isSchedule == "1";
            if (b == true)
            {
                DateTime date = new DateTime(Year, month, Day);
                //model.Id = Id;
                model.yearId = Year;
                model.monthId = month;
                model.day = Day;
                if(isScheduling==1)
                {
                    model.isScheduling = 1;
                }
                else if(isScheduling==0)
                {
                    model.isScheduling = 0;
                }
                if(isMonth==0)
                {
                    model.isMonth = 0;
                }
                else if(isMonth==1)
                {
                    model.isMonth=1;
                }
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
                for (int i = 0; i < 60; i++)
                {
                    model.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }
                for (int i = 0; i < 24; i++)
                {
                    model.HoursList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }
                foreach (var item in _employeeMethod.GetAllCoustomerEmployeeList().Where(x => x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList())
                {
                    model.CustomerList.Add(new SelectListItem() { Text = item.FirstName + " " + item.LastName + "-" + item.SSOID, Value = Convert.ToString(item.Id) });
                }
                model.IsDayOrMore = true;
                model.IsLessThenADay = false;
                DateTime datet = new DateTime(Year, month, Day);
                var s = datet.ToString("dd-MM-yyyy");
                model.StartDate = String.Format("{0:dd-MM-yyy}", s);
                model.EndDate = String.Format("{0:dd-MM-yyy}", s);
                model.DurationDays = 1;
                model.flag = 1;
                return PartialView("_partialAdminProjectPlannerAdd_Scheduling", model);
            }
            return PartialView("_partialAdminProjectPlannerAdd_Scheduling", model);
        }
        public ActionResult getEmployee()
        {
            int userId = SessionProxy.UserId;
            
            EmployeeProjectPlanner_TimeSheet_DetailViewModel detailModel = new EmployeeProjectPlanner_TimeSheet_DetailViewModel();
            var empData = _db.AspNetUsers.Where(x => x.Archived == false && x.Id == userId).FirstOrDefault();
            if (empData != null)
            {
                var customerdata = _db.AspNetUsers.Where(x => x.Archived == false && x.Id == userId).ToList();
                if (customerdata != null && customerdata.Count > 0)
                {
                    foreach (var item in customerdata)
                    {
                        detailModel.CustomerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
                    }
                }
            }
            else
            {
                foreach (var item in _employeeMethod.GetAllCoustomerEmployeeList().Where(x => x.SSOID.StartsWith("C") && x.Archived == false).ToList())
                {
                    detailModel.CustomerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
                }
            }
            //foreach (var item in _employeeMethod.GetAllResourceEmployeeList().Where(x => x.SSOID.StartsWith("W") && x.Archived == false).ToList())
            //{
            //    detailModel.ResourceList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            //}
            var data = _db.AspNetUserRoles.Where(x => x.UserId == userId && x.RoleId == 1).ToList();
            if (data.Count > 0 && data != null)
            {
                foreach (var item in _employeeMethod.GetAllResourceEmployeeList().Where(x => x.SSOID.StartsWith("W") && x.Archived == false).ToList())
                {
                    detailModel.ResourceList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
                }
            }
            else if(empData.SSOID.StartsWith("C"))
            {

                foreach (var item in _employeeMethod.getCustomerResponsibleWorker(userId))
                {
                    detailModel.ResourceList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
                }                
            }
            else
            {
                foreach (var item in _employeeMethod.getReportToEmployee(SessionProxy.UserId))
                {
                    detailModel.ResourceList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.ssoId, Value = item.EmployeeId.ToString() });
                }
            }
            return Json(detailModel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCusetomerListData()
        {
            EmployeeProjectPlanner_TimeSheet_DetailViewModel detailModel = new EmployeeProjectPlanner_TimeSheet_DetailViewModel();
            foreach (var item in _employeeMethod.GetAllCoustomerEmployeeList().Where(x => x.SSOID.StartsWith("C") && x.Archived == false).ToList())
            {
                detailModel.CustomerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }
            return Json(detailModel, JsonRequestBehavior.AllowGet);

        }

        public ActionResult getProjectPlannerDrillDownData(string isSchedule, string isTimesheet,int Day,int month,int Year,int? BusiId,int? DiviId,int? PoolId,int? FunctId,int? ProjectId,int isMonth,int? resourceId)
        {
            string inputFormat = "dd-MM-yyyy";
            bool b = isSchedule == "1";
            ResourcesAsminProjectPlannerViewModel model = new ResourcesAsminProjectPlannerViewModel();
            if (b == true)
            {
                DateTime datet = new DateTime(Year, month, Day);
                string dat = Convert.ToString(datet);
                var s = datet.ToString("dd-MM-yyyy");
                DateTime dt = DateTime.ParseExact(s.ToString(), inputFormat, CultureInfo.InvariantCulture);
                int EmpID = SessionProxy.UserId;
                model.IsSchdule = true;
                model.Year = Year;
                model.Month = month;
                model.Day = Day;
                if(isMonth==0)
                {
                    model.isMonth = 0;
                }
                else if(isMonth==1)
                {
                    model.isMonth = 1;
                }
                var data = _adminProjectPlanner.getResourceDataByFilter(dt);
                var admindata = _db.AspNetUserRoles.Where(x => x.UserId == EmpID && x.RoleId == 1).ToList();
                if (admindata.Count > 0 && admindata != null)
                {
                    data = _adminProjectPlanner.getResourceDataByFilter(dt);
                }
                else
                {
                    data = _adminProjectPlanner.getResourceDataByFilter(dt).Where(x => x.ReportToEmloyee == EmpID || x.EmployeeId == EmpID).ToList();
                }
                //if (EmpID != 0)
                //{
                //    data = _adminProjectPlanner.getResourceDataByFilter(dt).Distinct().Where(x => x.StartDate == datet).ToList();
                //}
                if (BusiId != 0 && BusiId != null)
                {
                    data = _adminProjectPlanner.getResourceDataByFilter(dt).Where(x => x.BustinessId == BusiId).ToList();
                }
                if (DiviId != 0 && DiviId != null)
                {
                    data = _adminProjectPlanner.getResourceDataByFilter(dt).Where(x => x.DivisionID == DiviId).ToList();
                }
                if (PoolId != 0 && PoolId != null)
                {
                    data = _adminProjectPlanner.getResourceDataByFilter(dt).Where(x => x.PoolId == PoolId).ToList();
                }
                if (FunctId != 0 && FunctId != null)
                {
                    data = _adminProjectPlanner.getResourceDataByFilter(dt).Where(x => x.FunctionID == FunctId).ToList();
                }
                if (ProjectId != 0 && ProjectId != null)
                {
                    data = _adminProjectPlanner.getResourceDataByFilter(dt).Where(x => x.ProjectID == ProjectId).ToList();
                }
                if(resourceId!=0 && resourceId!=null)
                {
                    data = _adminProjectPlanner.getResourceDataByFilter(dt).Where(x => x.EmployeeId == resourceId).ToList();
                }
                foreach (var item in data)
                {
                    ResourcesAsminProjectPlannerViewModel resource = new ResourcesAsminProjectPlannerViewModel();
                    resource.EmployeeId = Convert.ToInt32(item.EmployeeId);
                    resource.Resource_Name_SSO=item.ResourceName;
                    resource.jobtitle = item.JobTitle;
                    resource.Days = Convert.ToString(item.Days);
                    if (item.InTimeHr != 0 && item.InTimeHr != null && item.EndTimeHr != 0 && item.EndTimeHr != null)
                    {
                        TimeSpan totalTimeDiff = getTimeDiffOfResource(Convert.ToInt32(item.InTimeHr),
                            Convert.ToInt32(item.InTimeMin), Convert.ToInt32(item.EndTimeHr), Convert.ToInt32(item.EndTimeMin));
                        resource.Duration = totalTimeDiff;
                    }
                    else
                    {
                        resource.Duration = null;
                    }
                    resource.Business = item.BusinessName;
                    resource.Division = item.DivisionName;
                    resource.Pool = item.PoolsName;
                    resource.Project = item.ProjectName;
                    resource.Customer = item.CustomerName;
                    resource.AssetName = item.assetName;
                    model.GetAllList.Add(resource);
                }
            }
            return PartialView("_partialAdminProjectPlannerDrillDown_Scheduling",model);
        }
        public ResourcesAsminProjectPlannerViewModel returnList(string isSchedule, string isTimesheet, int Day, int month, int Year, int? BusiId, int? DiviId, int? PoolId, int? FunctId, int? ProjectId)
        {
            string inputFormat = "dd-MM-yyyy";
            bool b = isSchedule == "1";
            ResourcesAsminProjectPlannerViewModel model = new ResourcesAsminProjectPlannerViewModel();
            if (b == true)
            {
                DateTime datet = new DateTime(Year, month, Day);
                string dat = Convert.ToString(datet);
                var s = datet.ToString("dd-MM-yyyy");
                DateTime dt = DateTime.ParseExact(s.ToString(), inputFormat, CultureInfo.InvariantCulture);
                int EmpID = SessionProxy.UserId;
                model.IsSchdule = true;
                model.Year = Year;
                model.Month = month;
                model.Day = Day;
                var admindata = _db.AspNetUserRoles.Where(x => x.UserId == EmpID && x.RoleId == 1).ToList();
                var data = _adminProjectPlanner.getResourceDataByFilter(dt);
                if (admindata.Count > 0 && admindata != null)
                {
                    data = _adminProjectPlanner.getResourceDataByFilter(dt);
                }
                else
                {                    
                    data = _adminProjectPlanner.getResourceDataByFilter(dt).Where(x => x.ReportToEmloyee == EmpID || x.EmployeeId == EmpID).ToList();
                }                
                //if (EmpID != 0)
                //{
                //    data = _adminProjectPlanner.getResourceDataByFilter(dt).Distinct().Where(x => x.StartDate == datet).ToList();
                //}
                if (BusiId != 0 && BusiId != null)
                {
                    data = _adminProjectPlanner.getResourceDataByFilter(dt).Where(x => x.BustinessId == BusiId).ToList();
                }
                if (DiviId != 0 && DiviId != null)
                {
                    data = _adminProjectPlanner.getResourceDataByFilter(dt).Where(x => x.DivisionID == DiviId).ToList();
                }
                if (PoolId != 0 && PoolId != null)
                {
                    data = _adminProjectPlanner.getResourceDataByFilter(dt).Where(x =>x.PoolId == PoolId).ToList();
                }
                if (FunctId != 0 && FunctId != null)
                {
                    data = _adminProjectPlanner.getResourceDataByFilter(dt).Where(x => x.FunctionID == FunctId).ToList();
                }
                if (ProjectId != 0 && ProjectId != null)
                {
                    data = _adminProjectPlanner.getResourceDataByFilter(dt).Where(x => x.ProjectID == ProjectId).ToList();
                }
                foreach (var item in data)
                {
                    ResourcesAsminProjectPlannerViewModel resource = new ResourcesAsminProjectPlannerViewModel();
                    resource.EmployeeId = Convert.ToInt32(item.EmployeeId);
                    resource.Resource_Name_SSO = item.ResourceName;
                    resource.jobtitle = item.JobTitle;
                    resource.Days = Convert.ToString(item.Days);
                    if (item.InTimeHr != 0 && item.InTimeHr != null && item.EndTimeHr != 0 && item.EndTimeHr != null)
                    {
                        TimeSpan totalTimeDiff = getTimeDiffOfResource(Convert.ToInt32(item.InTimeHr),
                            Convert.ToInt32(item.InTimeMin), Convert.ToInt32(item.EndTimeHr), Convert.ToInt32(item.EndTimeMin));
                        resource.Duration = totalTimeDiff;
                    }
                    else
                    {
                        resource.Duration = null;
                    }
                    resource.Business = item.BusinessName;
                    resource.Division = item.DivisionName;
                    resource.Pool = item.PoolsName;
                    resource.Project = item.ProjectName;
                    resource.Customer = item.CustomerName;
                    resource.AssetName = item.assetName;
                    model.GetAllList.Add(resource);
                }
            }
            return model;
        }
        public ActionResult ExportExcelCoustomer(string isSchedule, string isTimesheet, int Day, int month, int Year, int? BusiId, int? DiviId, int? PoolId, int? FunctId, int? ProjectId)
        {
            string ResourceList = "ResourceList";
            ResourcesAsminProjectPlannerViewModel model = returnList(isSchedule, isTimesheet, Day,month,Year, BusiId,DiviId,PoolId,FunctId,ProjectId);
            DataTable dttable = new DataTable("Schedule");
            dttable.Columns.Add("Name", typeof(string));
            dttable.Columns.Add("Job Title", typeof(string));
            dttable.Columns.Add("Days", typeof(string));
            dttable.Columns.Add("Business", typeof(string));
            dttable.Columns.Add("Division", typeof(string));
            dttable.Columns.Add("Pool", typeof(string));
            dttable.Columns.Add("Project", typeof(string));
            dttable.Columns.Add("Customer", typeof(string));
            dttable.Columns.Add("AssetName", typeof(string));
            dttable.Columns.Add("Asset Type", typeof(string));
            dttable.Columns.Add("Time-Written", typeof(string));
            dttable.Columns.Add("Manager", typeof(string));         
            foreach (var item in model.GetAllList)
            {
                List<string> lstStrRow = new List<string>();
                lstStrRow.Add(item.Resource_Name_SSO);
                lstStrRow.Add(item.jobtitle);
                lstStrRow.Add(item.Days);
                lstStrRow.Add(item.Business);
                lstStrRow.Add(item.Division);
                lstStrRow.Add(item.Pool);
                lstStrRow.Add(item.Project);
                lstStrRow.Add(item.Customer);
                lstStrRow.Add(item.AssetName);
                lstStrRow.Add(null);
                lstStrRow.Add(null);
                lstStrRow.Add(null);
                string[] newArray = lstStrRow.ToArray();
                dttable.Rows.Add(newArray);
            }
            #region export file
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dttable);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + ResourceList + "_Skills.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            #endregion
            return View();
        }
        public ActionResult SaveData_SchedulingLeaves(EmployeeProjectPlanner_Scheduling_DocumentsViewModel model)
        {
            int Userid = SessionProxy.UserId;
            var data = _adminProjectPlanner.SaveData_SchedulingLeave(model, Userid);
            AdminProjectPlannerYearModel YearModel = new AdminProjectPlannerYearModel();
            AdminProjectPlannerRequestModel _requestModel = new AdminProjectPlannerRequestModel();
            if (model.isMonth == 1)
            {
                AdminProjectPlannerMonthModel DayModel = new AdminProjectPlannerMonthModel();
                _requestModel.IsSchedule = true;
                _requestModel.MonthId = model.monthId;
                _requestModel.Year = model.yearId;
                DayModel = ListOfMonth_Year(_requestModel.Year, _requestModel.MonthId, _requestModel);
                return PartialView("_ProjectDayPartial", DayModel);
            }
            else
            {
                _requestModel.IsSchedule = true;
                _requestModel.MonthId = model.monthId;
                _requestModel.Year = model.yearId;
                YearModel = ListOfMonth(_requestModel.Year, _requestModel);
                return PartialView("_ProjectCalenderPartial", YearModel);
            }
        }
        public ActionResult validateSchedule_Resource(string EmployeeID, string startDate)
        {
            string inputFormat = "dd-MM-yyyy";
            bool flag = false;
            if (EmployeeID!="" && EmployeeID!=null && Convert.ToInt32(EmployeeID) != 0 )
            {
                DateTime dt = DateTime.ParseExact(startDate, inputFormat, CultureInfo.InvariantCulture);
                flag = _adminProjectPlanner.validateResource_StartDate(dt, Convert.ToInt32(EmployeeID));
            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }
        public ResourcesAsminProjectPlannerViewModel returnTravelList(string isTravel, int Day, int month, int Year, int? BusiId, int? DiviId, int? PoolId, int? FunctId, int? ProjectId)
        {
            string inputFormat = "dd-MM-yyyy";
            ResourcesAsminProjectPlannerViewModel model = new ResourcesAsminProjectPlannerViewModel();
                DateTime datet = new DateTime(Year, month, Day);
                string dat = Convert.ToString(datet);
                var s = datet.ToString("dd-MM-yyyy");
                DateTime dt = DateTime.ParseExact(s.ToString(), inputFormat, CultureInfo.InvariantCulture);
                model.IsSchdule = true;
                model.Year = Year;
                model.Month = month;
                model.Day = Day;
                 int EmpID = SessionProxy.UserId;
                var data = _adminProjectPlanner.getResourceDataByFilterTravel(dt);
                var admindata = _db.AspNetUserRoles.Where(x => x.UserId == EmpID && x.RoleId == 1).ToList();
                if (admindata.Count > 0 && admindata != null)
                {
                    data = _adminProjectPlanner.getResourceDataByFilterTravel(dt);
                }
                else
                {
                    data = _adminProjectPlanner.getResourceDataByFilterTravel(dt).Where(x => x.ReportToEmloyee == EmpID || x.EmployeeId == EmpID).ToList();
                }
                //if (EmpID != 0)
                // {
                // data = _adminProjectPlanner.getResourceDataByFilterTravel(dt).Distinct().Where(x => x.StartDate == datet).ToList();
                // }
                if (BusiId != 0 && BusiId != null)
                 {
                data = _adminProjectPlanner.getResourceDataByFilterTravel(dt).Where(x => x.BusinessID == BusiId).ToList();
                 }
             if (DiviId != 0 && DiviId != null)
            {
                data = _adminProjectPlanner.getResourceDataByFilterTravel(dt).Where(x =>x.DivisionID == DiviId).ToList();
            }
            if (PoolId != 0 && PoolId != null)
            {
                data = _adminProjectPlanner.getResourceDataByFilterTravel(dt).Where(x => x.PoolID == PoolId).ToList();
            }
            if (FunctId != 0 && FunctId != null)
            {
                data = _adminProjectPlanner.getResourceDataByFilterTravel(dt).Where(x =>x.FunctionID == FunctId).ToList();
            }
            if (ProjectId != 0 && ProjectId != null)
            {
                data = _adminProjectPlanner.getResourceDataByFilterTravel(dt).Where(x => x.Project == ProjectId).ToList();
            }
            foreach (var item in data)
                {
                    ResourcesAsminProjectPlannerViewModel resource = new ResourcesAsminProjectPlannerViewModel();
                    resource.EmployeeId = Convert.ToInt32(item.EmployeeId);
                    resource.Resource_Name_SSO = item.ResourceName;
                    resource.jobtitle = item.JobTitle;
                    resource.Days = Convert.ToString(item.Days);
                    resource.DurationHR = Convert.ToString(item.DurationHr);
                    resource.Business = item.BusinessName;
                    resource.Division = item.DivisionName;
                    resource.Pool = item.PoolsName;
                    resource.Project = item.ProjectName;
                    resource.Customer = item.CustomerName;
                    resource.CostCode = item.CostCode;
                     resource.tavelType = item.Type;
                    model.GetAllList.Add(resource);
                }

            return model;
        }
        public ActionResult ExportExcelTravel(int Day, int month, int Year, int? BusiId, int? DiviId, int? PoolId, int? FunctId, int? ProjectId)
        {
            string ResourceList = "ResourceList";
            string isTravel = "1";
            ResourcesAsminProjectPlannerViewModel model = returnTravelList(isTravel,Day, month, Year, BusiId, DiviId, PoolId, FunctId, ProjectId);
            DataTable dttable = new DataTable("Travel");
            dttable.Columns.Add("Name", typeof(string));
            dttable.Columns.Add("Job Title", typeof(string));
            dttable.Columns.Add("Days", typeof(string));
            dttable.Columns.Add("Business", typeof(string));
            dttable.Columns.Add("Division", typeof(string));
            dttable.Columns.Add("Pool", typeof(string));
            dttable.Columns.Add("Project", typeof(string));
            dttable.Columns.Add("Customer", typeof(string));
            dttable.Columns.Add("CostCode", typeof(string));
            dttable.Columns.Add("Type", typeof(string));
            dttable.Columns.Add("Manager", typeof(string));
            foreach (var item in model.GetAllList)
            {
                List<string> lstStrRow = new List<string>();
                lstStrRow.Add(item.Resource_Name_SSO);
                lstStrRow.Add(item.jobtitle);
                lstStrRow.Add(item.Days);
                lstStrRow.Add(item.Business);
                lstStrRow.Add(item.Division);
                lstStrRow.Add(item.Pool);
                lstStrRow.Add(item.Project);
                lstStrRow.Add(item.Customer);
                lstStrRow.Add(item.CostCode);
                lstStrRow.Add(item.tavelType);
                lstStrRow.Add(null);
                string[] newArray = lstStrRow.ToArray();
                dttable.Rows.Add(newArray);
            }
            #region export file
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dttable);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + ResourceList + "_Skills.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            #endregion
            return View();
        }

        //Travel
        public ActionResult getAdminProjectPlannerTravel(string IsTravel, int Year, int month, int Day,int isTravelDrillDown,int isMonth)
        {
            EmployeeProjectPlanner_TravelLeaveViewModel model = new EmployeeProjectPlanner_TravelLeaveViewModel();
            DateTime date = new DateTime(Year, month, Day);
            //model.Id = Id;
            model.yearId = Year;
            model.monthId = month;
            model.day = Day;
            model.Hour = 0;
            model.Min = 0;
            model.flag = 1;
            if(isTravelDrillDown==1)
            {
                model.IsTravellDrillDown = 1;
            }
            else if(isTravelDrillDown==0)
            {
                model.IsTravellDrillDown = 0;
            }
            if (isMonth == 0)
            {
                model.isMonth = 0;
            }
            else if (isMonth == 1)
            {
                model.isMonth = 1;
            }
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
            foreach (var item in _employeeMethod.GetAllCoustomerEmployeeList().Where(x => x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList())
            {
                model.CustomerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }
            for (int i = 0; i < 60; i++)
            {
                model.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            for (int i = 0; i < 24; i++)
            {
                model.HoursList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            DateTime datet = new DateTime(Year, month, Day);
            var s = datet.ToString("dd-MM-yyyy");
            model.StartDate = String.Format("{0:dd-MM-yyy}", s);
            model.EndDate = String.Format("{0:dd-MM-yyy}", s);
            model.Duration = 1;
            return PartialView("_partialAdminProjectPlannerAdd_Travel",model);
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
            _adminProjectPlanner.TravelLeave_SaveData(model, SessionProxy.UserId);
            AdminProjectPlannerYearModel YearModel = new AdminProjectPlannerYearModel();
            AdminProjectPlannerRequestModel _requestModel = new AdminProjectPlannerRequestModel();           
            if (model.isMonth == 1)
            {
                AdminProjectPlannerMonthModel DayModel = new AdminProjectPlannerMonthModel();
                _requestModel.IsTravel = true;
                _requestModel.MonthId = model.monthId;
                _requestModel.Year = model.yearId;
                DayModel = ListOfMonth_Year(_requestModel.Year, _requestModel.MonthId, _requestModel);
                return PartialView("_ProjectDayPartial", DayModel);
            }
            else
            {
                _requestModel.IsTravel = true;
                _requestModel.MonthId = model.monthId;
                _requestModel.Year = model.yearId;
                YearModel = ListOfMonth(_requestModel.Year, _requestModel);
                return PartialView("_ProjectCalenderPartial", YearModel);
            }



            // return PartialView("_partialAdminProjectPlannerAdd_Travel", model);
        }
        public ActionResult getProjectPlannerTravelDrillDownData(string isTravel, int Day, int month, int Year, int? BusiId, int? DiviId, int? PoolId, int? FunctId, int? ProjectId,int isMonth,int? resourceId)
        {
            string inputFormat = "dd-MM-yyyy";
            bool b = isTravel == "1";
            ResourcesAsminProjectPlannerViewModel model = new ResourcesAsminProjectPlannerViewModel();
            if (b == true)
            {
                DateTime datet = new DateTime(Year, month, Day);
                string dat = Convert.ToString(datet);
                var s = datet.ToString("dd-MM-yyyy");
                DateTime dt = DateTime.ParseExact(s.ToString(), inputFormat, CultureInfo.InvariantCulture);
                model.IsSchdule = true;
                model.Year = Year;
                model.Month = month;
                model.Day = Day;
                if(isMonth==0)
                {
                    model.isMonth = 0;
                }
                else if(isMonth==1)
                {
                    model.isMonth = 1;
                }
                int EmpID = SessionProxy.UserId;
                var admindata = _db.AspNetUserRoles.Where(x => x.UserId == EmpID && x.RoleId == 1).ToList();
                var data = _adminProjectPlanner.getResourceDataByFilterTravel(dt);
                if (admindata.Count > 0 && admindata != null)
                {
                    data = _adminProjectPlanner.getResourceDataByFilterTravel(dt);
                }
                else
                {
                    data = _adminProjectPlanner.getResourceDataByFilterTravel(dt).Where(x => x.ReportToEmloyee == EmpID || x.EmployeeId == EmpID).ToList();
                }
                //if (EmpID != 0)
                //{
                //    data = _adminProjectPlanner.getResourceDataByFilterTravel(dt).Distinct().Where(x => x.StartDate == datet).ToList();
                //}
                if (BusiId != 0 && BusiId != null)
                {
                    data = _adminProjectPlanner.getResourceDataByFilterTravel(dt).Where(x => x.BusinessID == BusiId).ToList();
                }
                if (DiviId != 0 && DiviId != null)
                {
                    data = _adminProjectPlanner.getResourceDataByFilterTravel(dt).Where(x => x.DivisionID == DiviId).ToList();
                }
                if (PoolId != 0 && PoolId != null)
                {
                    data = _adminProjectPlanner.getResourceDataByFilterTravel(dt).Where(x =>x.PoolID == PoolId).ToList();
                }
                if (FunctId != 0 && FunctId != null)
                {
                    data = _adminProjectPlanner.getResourceDataByFilterTravel(dt).Where(x => x.FunctionID == FunctId).ToList();
                }
                if (ProjectId != 0 && ProjectId != null)
                {
                    data = _adminProjectPlanner.getResourceDataByFilterTravel(dt).Where(x => x.Project == ProjectId).ToList();
                }
                if(resourceId!=0 && resourceId!=null)
                {
                    data = _adminProjectPlanner.getResourceDataByFilterTravel(dt).Where(x => x.EmployeeId == resourceId).ToList();
                }
                foreach (var item in data)
                {
                    ResourcesAsminProjectPlannerViewModel resource = new ResourcesAsminProjectPlannerViewModel();
                    resource.EmployeeId = Convert.ToInt32(item.EmployeeId);
                    resource.Resource_Name_SSO = item.ResourceName;
                    resource.travelId = item.TravelId;
                    resource.jobtitle = item.JobTitle;
                    resource.Days = Convert.ToString(item.Days);
                    resource.DurationHR =Convert.ToString(item.DurationHr);
                    resource.Business = item.BusinessName;
                    resource.Division = item.DivisionName;
                    resource.Pool = item.PoolsName;
                    resource.Project = item.ProjectName;
                    resource.Customer = item.CustomerName;
                    resource.CostCode = item.CostCode;
                    resource.tavelType = item.Type;
                    model.GetAllList.Add(resource);
                }
            }
            return PartialView("_partialAdminProjectPlannerDrillDown_Traveling", model);
        }
        public ActionResult EditResourceTravel(int EmpId,int TravelId, int Year, int Month, int Day)
        {
            EmployeeProjectPlanner_TravelLeaveViewModel model = new EmployeeProjectPlanner_TravelLeaveViewModel();
            DateTime date = new DateTime(Year, Month, Day);
            model.Id = EmpId;
            model.yearId = Year;
            model.monthId = Month;
            model.day = Day;
            model.Hour = 0;
            model.Min = 0;
            model.FromCountryList = _employeeMethod.BindCountryDropdown();
            model.ToCountryList = _employeeMethod.BindCountryDropdown();
            if (EmpId != null)
            {
                var Resource = _db.AspNetUsers.Where(x => x.Id == EmpId).FirstOrDefault();
                model.EmployeeId = EmpId;
                model.EmployeeName = Resource.FirstName + " " + Resource.LastName + "-" + Resource.SSOID;
            }
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

            foreach (var item in _employeeMethod.GetAllCoustomerEmployeeList().Where(x => x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList())
            {
                model.CustomerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }
            for (int i = 0; i < 60; i++)
            {
                model.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            for (int i = 0; i < 24; i++)
            {
                model.HoursList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
                    
            var data = _adminProjectPlanner.getTravelLeaveByEmployeeId(EmpId, date);
            if (data != null)
            {

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
                    if (model.InTimeHr != null && model.InTimeHr != 0)
                    {
                        model.InTimeHr = (int)data.StartTimeHour;
                        model.InTimeMin = (int)data.StartTimeMin;
                    }
                    if(model.EndTimeHr!=null && model.EndTimeHr!=0)
                    {
                        model.EndTimeHr = (int)data.EndTimeHour;
                        model.EndTimeMin = (int)data.EndTimeMin;
                        model.DurationHr = data.DurationHr;
                    }
                    
                }
                model.Comment = data.Comment;
                model.Type = data.Type;
                model.Project = data.Project;
                int CmpId = Convert.ToInt32(data.Customer);
                if (CmpId != 0 && data.Customer != null && data.Customer!="")
                {

                    var Employee = _db.AspNetUsers.Where(x => x.Id == CmpId).FirstOrDefault();
                    if (Employee != null)
                    {
                        if (Employee.FirstName != "" && Employee.FirstName != null && Employee.LastName != "" && Employee.LastName != null)
                        {
                            model.Customer = Employee.FirstName + " " + Employee.LastName + " " + Employee.SSOID;
                            model.CustomerId = data.Customer;
                        }
                    }
                }
                //   model.Customer = data.Customer;
                model.CostCode = data.CostCode;
                var otherLeaveDoument = _adminProjectPlanner.getAllTravelLeaveDocument(TravelId);
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
            
            return PartialView("_partialAdminProjectPlannerAdd_Travel", model);
        }

        //Timesheet
        public TimeSpan getTotalTimeMonth(int month)
        {
            if(month!=0)
            {
                month = month;   
            }
            else
            {
                month = System.DateTime.Now.Month;
            }
            int Year = DateTime.Now.Year;
            var data = _adminProjectPlanner.getTotalTimeSheetDuration();
            TimeSpan totalTimeOfMonth = new TimeSpan();
            foreach (var item in data)
            {
                DateTime dataMonth = Convert.ToDateTime(item.Date);
                if (month == dataMonth.Month && dataMonth.Year== Year)
                {
                    TimeSpan t = TimeSpan.Parse(item.Hours);
                    totalTimeOfMonth = totalTimeOfMonth.Add(t);
                }
            }
            return totalTimeOfMonth;
        }
        public static int GetWeekNumber(DateTime now)
        {
            CultureInfo ci = CultureInfo.CurrentCulture;
            int weekNumber = ci.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNumber;
        }

        public TimeSpan getTottalTimeWeek(DateTime dateWeek)
        {
            int Curruentweek = GetWeekNumber(dateWeek);
            int month = dateWeek.Month;
            int year = dateWeek.Year;
            var data = _adminProjectPlanner.getTotalTimeSheetDuration();
            TimeSpan totalTimeOfWeek = new TimeSpan();
            foreach (var item in data)
            {
                DateTime dataMonth = Convert.ToDateTime(item.Date);
                int dataWeek = GetWeekNumber(dataMonth);
                if (year == dataMonth.Year && month == dataMonth.Month && Curruentweek == dataWeek)
                {
                    TimeSpan t = TimeSpan.Parse(item.Hours);
                    totalTimeOfWeek = totalTimeOfWeek.Add(t);
                }
            }
            return totalTimeOfWeek;
        }
        public TimeSpan getTotalTimeToday(DateTime date)
        {
            int month = date.Month;
            int year = date.Year;
            int day = date.Day;
            var data = _adminProjectPlanner.getTotalTimeSheetDuration();
            TimeSpan totalTodayTime = new TimeSpan();
            foreach (var item in data)
            {
                DateTime dataMonth = Convert.ToDateTime(item.Date);
                int dataWeek = GetWeekNumber(dataMonth);
                if (date == Convert.ToDateTime(item.Date))
                //if (year == dataMonth.Year && month == dataMonth.Month)
                {
                    TimeSpan t = TimeSpan.Parse(item.Hours);
                    totalTodayTime = totalTodayTime.Add(t);
                }
            }
            return totalTodayTime;
        }

        public ActionResult getAdminTimesheet(int Id,string IsTimeseet, int Year, int month, int Day,int timehseetDrillDown,int isMonth)
        {
            EmployeeProjectPlanner_TimeSheetViewModel model = new EmployeeProjectPlanner_TimeSheetViewModel();
            DateTime date = new DateTime(Year, month, Day);
            model.yearId = Year;
            model.monthId = month;
            model.day = Day;
            if(timehseetDrillDown==1)
            {
                model.timehseetDrillDown = 1;
            }
            else if(timehseetDrillDown==0)
            {
                model.timehseetDrillDown = 0;
            }
            if (isMonth == 0)
            {
                model.isMonth = 0;
            }
            else if (isMonth == 1)
            {
                model.isMonth = 1;
            }
            model.Flag = 1;
            model.totoalHrInMonth = getTotalTimeMonth(month);
            model.totalHrOfWeek = getTottalTimeWeek(date);
            model.totalHrToday = getTotalTimeToday(date);
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
                    int CmpId = Convert.ToInt32(timeSheet.Customer);
                    if (CmpId != 0 && timeSheet.Customer != null && timeSheet.Customer!="")
                    {
                        var Employee = _db.AspNetUsers.Where(x => x.Id == CmpId).FirstOrDefault();
                        if (Employee != null)
                        {
                            if (Employee.FirstName != "" && Employee.FirstName != null && Employee.LastName != "" && Employee.LastName != null)
                            {
                                detailModel.Customer = Employee.FirstName + " " + Employee.LastName + " " + Employee.SSOID;
                                detailModel.CustomerId = timeSheet.Customer;
                            }
                        }
                    }
                    foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Cost Code List"))
                    {
                        detailModel.CostCodeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
                    }

                    var projectList = _projectSettindsMethod.getAllList().Where(x => x.Archived == false).ToList();
                    detailModel.ProjectList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
                    foreach (var item in projectList)
                    {
                        detailModel.ProjectList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
                    }

                    var customerList = _projectSettindsMethod.getAllCustomer().Where(x => x.Archived == false).ToList();

                    foreach (var item in customerList)
                    {
                        detailModel.CustomerList.Add(new SelectListItem() { Text = item.FirstName + "" + item.LastName, Value = item.Id.ToString() });
                    }
                    detailModel.AssetList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
                    foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Asset Type List"))
                    {
                        detailModel.AssetList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
                    }
                    for (int i = 0; i < 60; i++)
                    {
                        detailModel.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                    }
                    for (int i = 0; i < 24; i++)
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
                    //detailModel.Customer = timeSheet.Customer;
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


            model.AssetList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Asset Type List"))
            {
                model.AssetList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            for (int i = 0; i < 60; i++)
            {
                model.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            for (int i = 0; i < 24; i++)
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
            AdminProjectPlannerYearModel YearModel = new AdminProjectPlannerYearModel();
            AdminProjectPlannerRequestModel _requestModel = new AdminProjectPlannerRequestModel();
            DateTime datet = new DateTime(model.yearId,model.monthId,model.day);
            
            //bool test = getWorkPattenById(model.EmployeeId, datet);
            if (model.isMonth == 1)
            {
                AdminProjectPlannerMonthModel DayModel = new AdminProjectPlannerMonthModel();
                _requestModel.IsTimeSheet = true;
                _requestModel.MonthId = model.monthId;
                _requestModel.Year = model.yearId;
                DayModel = ListOfMonth_Year(_requestModel.Year, _requestModel.MonthId, _requestModel);
                return PartialView("_ProjectDayPartial", DayModel);
            }
            else
            {
                _requestModel.IsTimeSheet = true;
                _requestModel.MonthId = model.monthId;
                _requestModel.Year = model.yearId;
                YearModel = ListOfMonth(_requestModel.Year, _requestModel);
                return PartialView("_ProjectCalenderPartial", YearModel);
            }


            // return PartialView("_partialAdd_TimeSheet", model);
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
        public JsonResult GetWorkPatternData(EmployeePlanner_TimeSheetViewModel model)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<EmployeePlanner_TimeSheet_DetailViewModel> listDetail = js.Deserialize<List<EmployeePlanner_TimeSheet_DetailViewModel>>(model.jsonDetailList);
            model.DetailList = listDetail;
            string inputFormat = "dd-MM-yyyy";
            var workData = _employeeMethod.getWorkPatternById(model.EmployeeId);
            DateTime dt = DateTime.ParseExact(model.Date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            bool flag = false;
            foreach (var data in model.DetailList)
            {
                string strSt = data.InTimeHr + "." + data.InTimeMin;
                decimal st_Time = Convert.ToDecimal(strSt);
                string strEt = data.EndTimeHr + "." + data.EndTimeMin;
                decimal et_Time = Convert.ToDecimal(strEt);
                if (workData.Count > 0)
                {
                    foreach (var wData in workData)
                    {
                        if (dt.DayOfWeek == DayOfWeek.Sunday)
                        {
                            if ((st_Time >= wData.SundayStart && st_Time <= wData.SundayEnd) && (et_Time >= wData.SundayStart && et_Time <= wData.SundayEnd))
                            {
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
                                flag = true;
                            }
                        }
                        else if (dt.DayOfWeek == DayOfWeek.Tuesday)
                        {
                            if ((st_Time >= wData.TuesdayStart && st_Time <= wData.TuesdayEnd && (et_Time >= wData.TuesdayStart && et_Time <= wData.TuesdayEnd)))
                            {
                                flag = true;
                            }
                        }
                        else if (dt.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            if ((st_Time >= wData.WednessdayStart && st_Time <= wData.WednessdayEnd && (et_Time >= wData.WednessdayStart && et_Time <= wData.WednessdayEnd)))
                            {
                                flag = true;
                            }
                        }
                        else if (dt.DayOfWeek == DayOfWeek.Thursday)
                        {
                            if ((st_Time >= wData.TuesdayStart && st_Time <= wData.TuesdayEnd && (et_Time >= wData.TuesdayStart && et_Time <= wData.ThursdayEnd)))
                            {
                                flag = true;
                            }
                        }
                        else if (dt.DayOfWeek == DayOfWeek.Friday)
                        {
                            if ((st_Time >= wData.FridayStart && st_Time <= wData.FridayEnd && (et_Time >= wData.FridayStart && et_Time <= wData.FridayEnd)))
                            {
                                flag = true;
                            }
                        }
                        else if (dt.DayOfWeek == DayOfWeek.Saturday)
                        {
                            if ((st_Time >= wData.SaturdayStart && st_Time <= wData.SaturdayEnd && (et_Time >= wData.SaturdayStart && et_Time <= wData.SaturdayEnd)))
                            {
                                flag = true;
                            }
                        }
                    }
                }
            }
            return Json(flag, JsonRequestBehavior.AllowGet);

        }
        public TimeSpan getTimeDiffOfResource(int InTimeHr, int InTimeMin, int EndTimeHr,int EndTimeMin)
        {
            string time1 = InTimeHr + ":" + InTimeMin;
            string time2 = EndTimeHr + ":" + EndTimeMin;
            TimeSpan timeDiff = new TimeSpan();
            if (time1 != "" && time1!=null && time2!="" && time2!=null)
            {
                TimeSpan t1 = TimeSpan.Parse(time1);
                TimeSpan t2 = TimeSpan.Parse(time2);
                timeDiff = t2.Subtract(t1);
            }
            return timeDiff;
        }

        public ActionResult getProjectPlannerTimesheetDrillDownData(string IsTimeSheet, int Day, int month, int Year, int? BusiId, int? DiviId, int? PoolId, int? FunctId, int? ProjectId,int isMonth,int? resourceId)
        {
            string inputFormat = "dd-MM-yyyy";
            bool b = IsTimeSheet == "1";
            ResourcesAsminProjectPlannerViewModel model = new ResourcesAsminProjectPlannerViewModel();
            if (b == true)
            {
                DateTime datet = new DateTime(Year, month, Day);
                string dat = Convert.ToString(datet);
                var s = datet.ToString("dd-MM-yyyy");
                int EmpId = SessionProxy.UserId;
                DateTime dt = DateTime.ParseExact(s.ToString(), inputFormat, CultureInfo.InvariantCulture);
                model.IsTimeshhet = true;
                model.Year = Year;
                model.Month = month;
                model.Day = Day;
                if(isMonth==0)
                {
                    model.isMonth = 0;
                }
                else if(isMonth==1)
                {
                    model.isMonth = 1;
                }
                var admindata = _db.AspNetUserRoles.Where(x => x.UserId == EmpId && x.RoleId == 1).ToList();
                var data = _adminProjectPlanner.getTimesheetDrillDownData(dt);
                if (admindata.Count>0 && admindata!=null)
                {
                    data = _adminProjectPlanner.getTimesheetDrillDownData(dt);
                }
                else
                {
                    data = _adminProjectPlanner.getTimesheetDrillDownData(dt).Where(x => x.ReportToEmloyee == EmpId || x.EmployeeId == EmpId).ToList();
                }
                //if (EmpId != 0)
                //{
                //    data = _adminProjectPlanner.getTimesheetDrillDownData(dt, EmpId).Where(x => x.ReportToEmloyee == EmpId || x.EmployeeId==EmpId).ToList();
                //}
                if (BusiId!=0 && BusiId!=null)
                {
                     data = _adminProjectPlanner.getTimesheetDrillDownData(dt).Where(x=>x.BusinessID==BusiId).ToList();
                }
                if(DiviId!=0 && DiviId!=null)
                {
                    data = _adminProjectPlanner.getTimesheetDrillDownData(dt).Where(x => x.DivisionID==DiviId).ToList();
                }
                if(PoolId!=0 && PoolId!=null)
                {
                    data = _adminProjectPlanner.getTimesheetDrillDownData(dt).Where(x => x.PoolID==PoolId).ToList();
                }
                if(FunctId!=0 && FunctId!=null)
                {
                    data = _adminProjectPlanner.getTimesheetDrillDownData(dt).Where(x => x.FunctionID==FunctId).ToList();
                }
                if(ProjectId!=0 && ProjectId!=null)
                {
                    data = _adminProjectPlanner.getTimesheetDrillDownData(dt).Where(x => x.Project==ProjectId).ToList();
                }
                if(resourceId!=0 && resourceId!=null)
                {
                    data = _adminProjectPlanner.getTimesheetDrillDownData(dt).Where(x => x.EmployeeId == resourceId).ToList();
                }
                foreach (var item in data)
                {
                    ResourcesAsminProjectPlannerViewModel resource = new ResourcesAsminProjectPlannerViewModel();
                    resource.EmployeeId = Convert.ToInt32(item.EmployeeId);
                    resource.Resource_Name_SSO = item.ResourceName;
                    resource.jobtitle = item.JobTitleName;
                    if (item.InTimeHr != 0 && item.InTimeHr != null && item.EndTimeHr!=0 && item.EndTimeHr!=null)
                    {
                        TimeSpan totalTimeDiff = getTimeDiffOfResource(Convert.ToInt32(item.InTimeHr),
                            Convert.ToInt32(item.InTimeMin), Convert.ToInt32(item.EndTimeHr), Convert.ToInt32(item.EndTimeMin));
                        resource.Duration = totalTimeDiff;
                    }
                    else
                    {
                        resource.Duration = null;
                    }
                    resource.Business = item.BusinessName;
                    resource.Division = item.DivisionName;
                    resource.Pool = item.PoolName;
                    resource.Project = item.ProjectName;
                    resource.Customer = item.CustomerName;
                    resource.AssetName = item.AssetValue;
                    resource.CostCode = item.CostCodeValue;
                    resource.Status = item.ApprovalStatus;
                    resource.FunctionId = Convert.ToString(item.FunctionID);
                    model.GetAllList.Add(resource);
                }
            }
            return PartialView("_partialAdminProjectPlannerDrillDown_Timesheet", model);
        }
        public ActionResult editResourceTimesheet(int EmpId, int Year, int Month, int Day)
        {
            EmployeeProjectPlanner_TimeSheetViewModel model = new EmployeeProjectPlanner_TimeSheetViewModel();
            DateTime date = new DateTime(Year, Month, Day);
            model.Id = EmpId;
            model.yearId = Year;
            model.monthId = Month;
            model.day = Day;
            //model.Flag = 1;
            model.totoalHrInMonth = getTotalTimeMonth(Month);
            model.totalHrOfWeek = getTottalTimeWeek(date);
            model.totalHrToday = getTotalTimeToday(date);
            if (EmpId!=0 && EmpId!=null)
            {
                var Employee = _db.AspNetUsers.Where(x => x.Id == EmpId).FirstOrDefault();
                if (Employee != null)
                {
                    if(Employee.FirstName != null && Employee.FirstName != "" && Employee.LastName!=null && Employee.LastName!="")
                    {
                        model.EmployeeName = Employee.FirstName + " " + Employee.LastName + " " + Employee.SSOID;
                        model.EmployeeId = EmpId;
                    }
                }
            }
            if (EmpId > 0)
            {
                var data = _employeeProjectPlannerMethod.getTimeSheetByEmployeeId(EmpId, date);
                model.Date = String.Format("{0:dd-MM-yyy}", data.Date);
                model.Comment = data.Comments;
                int timesheetId = data.Id;

                var timeSheetDetail = _employeeProjectPlannerMethod.getAllTimeSheetDetail(timesheetId);
                foreach (var timeSheet in timeSheetDetail)
                {
                    EmployeeProjectPlanner_TimeSheet_DetailViewModel detailModel = new EmployeeProjectPlanner_TimeSheet_DetailViewModel();
                    detailModel.CostCodeList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
                    detailModel.FlagD = 1;
                    if (timeSheet.Customer != null && timeSheet.Customer!="")
                    {
                        int CmpId = Convert.ToInt32(timeSheet.Customer);
                        if (CmpId != 0 && timeSheet.Customer != null && timeSheet.Customer!="")
                        {
                            var Employee = _db.AspNetUsers.Where(x => x.Id == CmpId).FirstOrDefault();
                            if (Employee != null)
                            {
                                if (Employee.FirstName != "" && Employee.FirstName != null && Employee.LastName != "" && Employee.LastName != null)
                                {
                                    detailModel.Customer = Employee.FirstName + " " + Employee.LastName + " " + Employee.SSOID;
                                    detailModel.CustomerId = timeSheet.Customer;
                                }
                            }
                        }
                    }
                    foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Cost Code List"))
                    {
                        detailModel.CostCodeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
                    }

                    var projectList = _projectSettindsMethod.getAllList().Where(x => x.Archived == false).ToList();
                    detailModel.ProjectList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
                    foreach (var item in projectList)
                    {
                        detailModel.ProjectList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
                    }

                    var customerList = _projectSettindsMethod.getAllCustomer().Where(x => x.Archived == false).ToList();
                    foreach (var item in customerList)
                    {
                        detailModel.CustomerList.Add(new SelectListItem() { Text = item.FirstName + "" + item.LastName, Value = item.Id.ToString() });
                    }
                    detailModel.AssetList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
                    foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Asset Type List"))
                    {
                        detailModel.AssetList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
                    }
                    for (int i = 0; i < 60; i++)
                    {
                        detailModel.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                    }
                    for (int i = 0; i < 24; i++)
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
                    //detailModel.Customer = timeSheet.Customer;
                    detailModel.Asset = timeSheet.Asset;
                    model.DetailList.Add(detailModel);
                }

                var timeSheetDoument = _employeeProjectPlannerMethod.getAllTimeSheetDocument(timesheetId);
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
        public ResourcesAsminProjectPlannerViewModel returnTimesheetList(string isTimehseet, int Day, int month, int Year, int? BusiId, int? DiviId, int? PoolId, int? FunctId, int? ProjectId)
        {
            string inputFormat = "dd-MM-yyyy";
            ResourcesAsminProjectPlannerViewModel model = new ResourcesAsminProjectPlannerViewModel();
            DateTime datet = new DateTime(Year, month, Day);
            string dat = Convert.ToString(datet);
            var s = datet.ToString("dd-MM-yyyy");
            DateTime dt = DateTime.ParseExact(s.ToString(), inputFormat, CultureInfo.InvariantCulture);
            model.IsSchdule = true;
            model.Year = Year;
            model.Month = month;
            model.Day = Day;
            int EmpId = SessionProxy.UserId;
            var admindata = _db.AspNetUserRoles.Where(x => x.UserId == EmpId && x.RoleId == 1).ToList();
            var data = _adminProjectPlanner.getTimesheetDrillDownData(dt);
            if (admindata.Count > 0 && admindata != null)
            {
                data = _adminProjectPlanner.getTimesheetDrillDownData(dt);
            }
            else
            {
                data = _adminProjectPlanner.getTimesheetDrillDownData(dt).Where(x => x.ReportToEmloyee == EmpId || x.EmployeeId == EmpId).ToList();
            }
            //if (EmpId != 0)
            //{
            //    data = _adminProjectPlanner.getTimesheetDrillDownData(dt).Distinct().Where(x => x.Date == datet).ToList();
            //}
            if (BusiId != 0 && BusiId != null)
            {
                data = _adminProjectPlanner.getTimesheetDrillDownData(dt).Where(x => x.BusinessID == BusiId).ToList();
            }
            if (DiviId != 0 && DiviId != null)
            {
                data = _adminProjectPlanner.getTimesheetDrillDownData(dt).Where(x =>x.DivisionID == DiviId).ToList();
            }
            if (PoolId != 0 && PoolId != null)
            {
                data = _adminProjectPlanner.getTimesheetDrillDownData(dt).Where(x =>x.PoolID == PoolId).ToList();
            }
            if (FunctId != 0 && FunctId != null)
            {
                data = _adminProjectPlanner.getTimesheetDrillDownData(dt).Where(x => x.FunctionID == FunctId).ToList();
            }
            if (ProjectId != 0 && ProjectId != null)
            {
                data = _adminProjectPlanner.getTimesheetDrillDownData(dt).Where(x => x.Project == ProjectId).ToList();
            }
            foreach (var item in data)
            {
                ResourcesAsminProjectPlannerViewModel resource = new ResourcesAsminProjectPlannerViewModel();
                resource.EmployeeId = Convert.ToInt32(item.EmployeeId);
                resource.Resource_Name_SSO = item.ResourceName;
                resource.jobtitle = item.JobTitleName;
                if (item.InTimeHr != 0 && item.InTimeHr != null && item.EndTimeHr != 0 && item.EndTimeHr != null)
                {
                    TimeSpan totalTimeDiff = getTimeDiffOfResource(Convert.ToInt32(item.InTimeHr),
                        Convert.ToInt32(item.InTimeMin), Convert.ToInt32(item.EndTimeHr), Convert.ToInt32(item.EndTimeMin));
                    resource.Duration = totalTimeDiff;
                }
                else
                {
                    resource.Duration = null;
                }
                // resource.Days = Convert.ToString(item.Days);
                resource.Business = item.BusinessName;
                resource.Division = item.DivisionName;
                resource.Pool = item.PoolName;
                resource.Project = item.ProjectName;
                resource.Customer = item.CustomerName;
                resource.AssetName = item.AssetValue;
                resource.CostCode = item.CostCodeValue;
                resource.Status = item.ApprovalStatus;
                model.GetAllList.Add(resource);
            }
            return model;
        }
        public ActionResult ExportExcelTimehseet(int Day, int month, int Year, int? BusiId, int? DiviId, int? PoolId, int? FunctId, int? ProjectId)
        {
            string ResourceList = "ResourceList";
            string isTimesheet = "1";
            ResourcesAsminProjectPlannerViewModel model = returnTimesheetList(isTimesheet, Day, month, Year, BusiId, DiviId, PoolId, FunctId, ProjectId);
            DataTable dttable = new DataTable("Timehseet");
            dttable.Columns.Add("Name", typeof(string));
            dttable.Columns.Add("Job Title", typeof(string));
           // dttable.Columns.Add("Days", typeof(string));
            dttable.Columns.Add("Business", typeof(string));
            dttable.Columns.Add("Division", typeof(string));
            dttable.Columns.Add("Pool", typeof(string));
            dttable.Columns.Add("Project", typeof(string));
            dttable.Columns.Add("Customer", typeof(string));
            dttable.Columns.Add("AssetName", typeof(string));
            dttable.Columns.Add("CostCode", typeof(string));
            dttable.Columns.Add("Status", typeof(string));
            foreach (var item in model.GetAllList)
            {
                List<string> lstStrRow = new List<string>();
                lstStrRow.Add(item.Resource_Name_SSO);
                lstStrRow.Add(item.jobtitle);
          //      lstStrRow.Add(item.Days);
                lstStrRow.Add(item.Business);
                lstStrRow.Add(item.Division);
                lstStrRow.Add(item.Pool);
                lstStrRow.Add(item.Project);
                lstStrRow.Add(item.Customer);
                lstStrRow.Add(item.AssetName);
                lstStrRow.Add(item.CostCode);
                lstStrRow.Add(item.Status);
               // lstStrRow.Add(null);
                string[] newArray = lstStrRow.ToArray();
                dttable.Rows.Add(newArray);
            }
            #region export file
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dttable);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + ResourceList + "_Skills.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            #endregion
            return View();
        }


        // Uplift
        public TimeSpan getUpliftTotalTimeMonth(int month)
        {
            if (month != 0)
            {
                month = month;
            }
            else
            {
                month = System.DateTime.Now.Month;
            }
            int year = DateTime.Now.Year;
            var data = _adminProjectPlanner.getTotalUpliftDuration();
            TimeSpan totalTimeOfMonth = new TimeSpan();
            foreach (var item in data)
            {
                DateTime dataMonth = Convert.ToDateTime(item.Date);
                if (month == dataMonth.Month && year==dataMonth.Year)
                {
                    TimeSpan t = TimeSpan.Parse(item.Hours);
                    totalTimeOfMonth = totalTimeOfMonth.Add(t);
                }
            }
            return totalTimeOfMonth;
        }
        public static int GetUpliftWeekNumber(DateTime now)
        {
            CultureInfo ci = CultureInfo.CurrentCulture;
            int weekNumber = ci.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNumber;
        }

        public TimeSpan getUpliftTottalTimeWeek(DateTime dateWeek)
        {
            int Curruentweek = GetWeekNumber(dateWeek);
            int month = dateWeek.Month;
            int year = dateWeek.Year;
            var data = _adminProjectPlanner.getTotalUpliftDuration();
            TimeSpan totalTimeOfWeek = new TimeSpan();
            foreach (var item in data)
            {
                DateTime dataMonth = Convert.ToDateTime(item.Date);
                int dataWeek = GetWeekNumber(dataMonth);
                if (year == dataMonth.Year && month == dataMonth.Month && Curruentweek == dataWeek)
                {
                    TimeSpan t = TimeSpan.Parse(item.Hours);
                    totalTimeOfWeek = totalTimeOfWeek.Add(t);
                }
            }
            return totalTimeOfWeek;
        }
        public TimeSpan getUpliftTotalTimeToday(DateTime date)
        {
            int month = date.Month;
            int year = date.Year;
            int day = date.Day;
            var data = _adminProjectPlanner.getTotalUpliftDuration();
            TimeSpan totalTodayTime = new TimeSpan();
            foreach (var item in data)
            {
                DateTime dataMonth = Convert.ToDateTime(item.Date);
                int dataWeek = GetWeekNumber(dataMonth);
                if (date == Convert.ToDateTime(item.Date))
                //if (year == dataMonth.Year && month == dataMonth.Month)
                {
                    TimeSpan t = TimeSpan.Parse(item.Hours);
                    totalTodayTime = totalTodayTime.Add(t);
                }
            }
            return totalTodayTime;
        }
        public ActionResult AddEdit_Uplift(int Id, int Date, int Month, int Year,int isUpliftDrillDown,int isMonth)
        {
            EmployeeProjectPlanner_UpliftViewModel model = new EmployeeProjectPlanner_UpliftViewModel();
            DateTime date = new DateTime(Year, Month, Date);
            model.Id = Id;
            model.yearId = Year;
            model.monthId = Month;
            model.day = Date;
            model.FlagD = 1;
            if (isUpliftDrillDown==1)
            {
                model.isUpliftDrillDown = 1;
            }
            else if(isUpliftDrillDown==0)
            {
                model.isUpliftDrillDown = 0;
            }
            if (isMonth == 0)
            {
                model.isMonth = 0;
            }
            else if (isMonth == 1)
            {
                model.isMonth = 1;
            }
            model.totoalHrInMonth = getUpliftTotalTimeMonth(Month);
            model.totalHrOfWeek = getUpliftTottalTimeWeek(date);
            model.totalHrToday = getUpliftTotalTimeToday(date);
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
                model.CustomerId = data.CustomerId;
                int CmpId = Convert.ToInt32(data.CustomerId);
                if (CmpId != 0 && data.CustomerId != null)
                {
                    var Employee = _db.AspNetUsers.Where(x => x.Id == CmpId).FirstOrDefault();
                    if (Employee != null)
                    {
                        if (Employee.FirstName != "" && Employee.FirstName != null && Employee.LastName != "" && Employee.LastName != null)
                        {
                            model.Customer = Employee.FirstName + " " + Employee.LastName + " " + Employee.SSOID;
                            model.CustomerId = data.CustomerId;
                        }
                    }
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
                    for (int i = 0; i < 60; i++)
                    {
                        detailModel.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                    }
                    for (int i = 0; i < 24; i++)
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
            for (int i = 0; i < 60; i++)
            {
                model.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            for (int i = 0; i < 24; i++)
            {
                model.HoursList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            return PartialView("_partialAdd_Uplift_Detail", model);
        }

        public Boolean EmployeeWorkPatternExist(string EmployeeId,string sdate)
        {    
            string inputFormat = "dd-MM-yyyy";
            bool isWorkPatternExist = false;
            if (!string.IsNullOrEmpty(EmployeeId) && !string.IsNullOrEmpty(sdate))
            {
                DateTime date = DateTime.ParseExact(sdate, inputFormat, CultureInfo.InvariantCulture);
                isWorkPatternExist = getEmployeeWorkPatternById(Convert.ToInt32(EmployeeId), date);
                if (isWorkPatternExist == true)
                {
                    bool isWorkPatternLeave = getWorkPattenLeavebyId(Convert.ToInt32(EmployeeId), date);
                    if (isWorkPatternLeave == false)
                    {
                        isWorkPatternExist = true;
                    }
                    else
                    {
                        isWorkPatternExist = false;
                    }
                }
                else
                {
                    isWorkPatternExist = false;
                }
                return isWorkPatternExist;
            }
            else
            {
                isWorkPatternExist = false;
                return isWorkPatternExist;
            }
        }

        public ActionResult SaveData_Uplift(EmployeeProjectPlanner_UpliftViewModel model)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<EmployeeProjectPlanner_Uplift_DocumentsViewModel> listDocument = js.Deserialize<List<EmployeeProjectPlanner_Uplift_DocumentsViewModel>>(model.jsonDocumentList);
            model.DocumentList = listDocument;
            List<EmployeeProjectPlanner_Uplift_DetailViewModel> listDetail = js.Deserialize<List<EmployeeProjectPlanner_Uplift_DetailViewModel>>(model.jsonDetailList);
            model.DetailList = listDetail;
            _employeeProjectPlannerMethod.Uplift_SaveData(model, SessionProxy.UserId);
            AdminProjectPlannerYearModel YearModel = new AdminProjectPlannerYearModel();
            AdminProjectPlannerRequestModel _requestModel = new AdminProjectPlannerRequestModel();
            if (model.isMonth == 1)
            {
                AdminProjectPlannerMonthModel DayModel = new AdminProjectPlannerMonthModel();
                _requestModel.IsUplift = true;
                _requestModel.MonthId = model.monthId;
                _requestModel.Year = model.yearId;
                DayModel = ListOfMonth_Year(_requestModel.Year, _requestModel.MonthId, _requestModel);
                return PartialView("_ProjectDayPartial", DayModel);
            }
            else
            {
                _requestModel.IsUplift = true;
                _requestModel.MonthId = model.monthId;
                _requestModel.Year = model.yearId;
                YearModel = ListOfMonth(_requestModel.Year, _requestModel);
                return PartialView("_ProjectCalenderPartial", YearModel);
            }
            // return PartialView("_partialAdd_Uplift", model);
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
        public ActionResult getUpliftDrillDownData(string IsUplift, int Day, int month, int Year, int? BusiId, int? DiviId, int? PoolId, int? FunctId, int? ProjectId,int isMonth,int? resourceId)
        {
            string inputFormat = "dd-MM-yyyy";
            bool b = IsUplift == "1";
            ResourcesAsminProjectPlannerViewModel model = new ResourcesAsminProjectPlannerViewModel();
            if (b == true)
            {
                DateTime datet = new DateTime(Year, month, Day);
                string dat = Convert.ToString(datet);
                var s = datet.ToString("dd-MM-yyyy");
                DateTime dt = DateTime.ParseExact(s.ToString(), inputFormat, CultureInfo.InvariantCulture);
                model.IsTimeshhet = true;
                model.Year = Year;
                model.Month = month;
                model.Day = Day;
                if(isMonth==0)
                {
                    model.isMonth = 0;
                }
                else if(isMonth==1)
                {
                    model.isMonth = 1;
                }
                int EmpID = SessionProxy.UserId;
                var data = _adminProjectPlanner.getUpliftDrillDownData(dt);
                var admindata = _db.AspNetUserRoles.Where(x => x.UserId == EmpID && x.RoleId == 1).ToList();
                if (admindata.Count > 0 && admindata != null)
                {
                    data = _adminProjectPlanner.getUpliftDrillDownData(dt);
                }
                else
                {
                    data = _adminProjectPlanner.getUpliftDrillDownData(dt).Where(x => x.ReportToEmloyee == EmpID || x.EmployeeId == EmpID).ToList();
                }
                //if (EmpID != 0)
                //{
                //    data = _adminProjectPlanner.getUpliftDrillDownData(dt).Distinct().Where(x => x.Date == datet).ToList();
                //}
                if (BusiId != 0 && BusiId!=null)
                {
                    data = _adminProjectPlanner.getUpliftDrillDownData(dt).Where(x => x.BusinessID == BusiId).ToList();
                }
                if (DiviId != 0 && DiviId!=null)
                {
                    data = _adminProjectPlanner.getUpliftDrillDownData(dt).Where(x => x.DivisionID == DiviId).ToList();
                }
                if (PoolId != 0 && PoolId!=null)
                {
                    data = _adminProjectPlanner.getUpliftDrillDownData(dt).Where(x => x.PoolID == PoolId).ToList();
                }
                if (FunctId != 0 && FunctId!=null)
                {
                    data = _adminProjectPlanner.getUpliftDrillDownData(dt).Where(x =>x.FunctionID == FunctId).ToList();
                }
                if (ProjectId != 0 && ProjectId!=null)
                {
                    data = _adminProjectPlanner.getUpliftDrillDownData(dt).Where(x =>x.ProjectId == ProjectId).ToList();
                }
                if(resourceId!=0 && resourceId!=null)
                {
                    data = _adminProjectPlanner.getUpliftDrillDownData(dt).Where(x => x.EmployeeId == resourceId).ToList();
                }
                foreach (var item in data)
                {
                    ResourcesAsminProjectPlannerViewModel resource = new ResourcesAsminProjectPlannerViewModel();
                    resource.EmployeeId = Convert.ToInt32(item.EmployeeId);
                    resource.Resource_Name_SSO = item.ResourceName;
                    resource.jobtitle = item.JobTitleName;
                    //resource.Days = Convert.ToString(item.Days);
                    if (item.InTimeHr != 0 && item.InTimeHr != null && item.EndtimeHr != 0 && item.EndtimeHr != null)
                    {
                        TimeSpan totalTimeDiff = getTimeDiffOfResource(Convert.ToInt32(item.InTimeHr),
                            Convert.ToInt32(item.InTimeMin), Convert.ToInt32(item.EndtimeHr), Convert.ToInt32(item.EndTimeMin));
                        resource.Duration = totalTimeDiff;
                    }
                    else
                    {
                        resource.Duration = null;
                    }
                    resource.Business = item.BusinessName;
                    resource.Division = item.DivisionName;
                    resource.Pool = item.PoolName;
                    resource.Project = item.ProjectName;
                    resource.Customer = item.CustomerName;
                    //resource.AssetName = item.AssetValue;
                    //resource.CostCode = item.CostCodeValue;
                    resource.Status = item.ApprovalStatus;
                    model.GetAllList.Add(resource);
                }
            }
            return PartialView("_partialAdminProjectPlannerDrillDown_Uplift", model);
        }
        public ResourcesAsminProjectPlannerViewModel returnUpliftList(string isUplift, int Day, int month, int Year, int? BusiId, int? DiviId, int? PoolId, int? FunctId, int? ProjectId)
        {
            string inputFormat = "dd-MM-yyyy";
            ResourcesAsminProjectPlannerViewModel model = new ResourcesAsminProjectPlannerViewModel();
            DateTime datet = new DateTime(Year, month, Day);
            string dat = Convert.ToString(datet);
            var s = datet.ToString("dd-MM-yyyy");
            DateTime dt = DateTime.ParseExact(s.ToString(), inputFormat, CultureInfo.InvariantCulture);
            model.IsSchdule = true;
            model.Year = Year;
            model.Month = month;
            model.Day = Day;
            int EmpID = SessionProxy.UserId;
            var data = _adminProjectPlanner.getUpliftDrillDownData(dt);            
            var admindata = _db.AspNetUserRoles.Where(x => x.UserId == EmpID && x.RoleId == 1).ToList();
            if (admindata.Count > 0 && admindata != null)
            {
                data = _adminProjectPlanner.getUpliftDrillDownData(dt);
            }
            else
            {
                data = _adminProjectPlanner.getUpliftDrillDownData(dt).Where(x => x.ReportToEmloyee == EmpID || x.EmployeeId == EmpID).ToList();
            }
            //if (EmpID != 0)
            //{
            //    data = _adminProjectPlanner.getUpliftDrillDownData(dt).Distinct().Where(x => x.Date == datet).ToList();
            //}
            if (BusiId != 0 && BusiId != null)
            {
                data = _adminProjectPlanner.getUpliftDrillDownData(dt).Where(x => x.BusinessID == BusiId).ToList();
            }
            if (DiviId != 0 && DiviId != null)
            {
                data = _adminProjectPlanner.getUpliftDrillDownData(dt).Where(x => x.DivisionID == DiviId).ToList();
            }
            if (PoolId != 0 && PoolId != null)
            {
                data = _adminProjectPlanner.getUpliftDrillDownData(dt).Where(x => x.PoolID == PoolId).ToList();
            }
            if (FunctId != 0 && FunctId != null)
            {
                data = _adminProjectPlanner.getUpliftDrillDownData(dt).Where(x =>x.FunctionID == FunctId).ToList();
            }
            if (ProjectId != 0 && ProjectId != null)
            {
                data = _adminProjectPlanner.getUpliftDrillDownData(dt).Where(x => x.ProjectId == ProjectId).ToList();
            }
            foreach (var item in data)
            {
                ResourcesAsminProjectPlannerViewModel resource = new ResourcesAsminProjectPlannerViewModel();
                resource.EmployeeId = Convert.ToInt32(item.EmployeeId);
                resource.Resource_Name_SSO = item.ResourceName;
                resource.jobtitle = item.JobTitleName;
                if (item.InTimeHr != 0 && item.InTimeHr != null && item.EndtimeHr != 0 && item.EndtimeHr != null)
                {
                    TimeSpan totalTimeDiff = getTimeDiffOfResource(Convert.ToInt32(item.InTimeHr),
                        Convert.ToInt32(item.InTimeMin), Convert.ToInt32(item.EndtimeHr), Convert.ToInt32(item.EndTimeMin));
                    resource.Duration = totalTimeDiff;
                }
                else
                {
                    resource.Duration = null;
                }
                // resource.Days = Convert.ToString(item.Days);
                resource.Business = item.BusinessName;
                resource.Division = item.DivisionName;
                resource.Pool = item.PoolName;
                resource.Project = item.ProjectName;
                resource.Customer = item.CustomerName;
                //resource.AssetName = item.AssetValue;
              //  resource.CostCode = item.CostCodeValue;
                resource.Status = item.ApprovalStatus;
                model.GetAllList.Add(resource);
            }
            return model;
        }

        public ActionResult EditUpliftData(int EmpId, int Year, int Month, int Day)
        {
            EmployeeProjectPlanner_UpliftViewModel model = new EmployeeProjectPlanner_UpliftViewModel();
            DateTime date = new DateTime(Year, Month, Day);
            model.Id = EmpId;
            model.yearId = Year;
            model.monthId = Month;
            model.day = Day;
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
            foreach (var item in _employeeMethod.GetAllCoustomerEmployeeList().Where(x => x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList())
            {
                model.CustomerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }
            if (EmpId > 0)
            {
                var data = _employeeProjectPlannerMethod.getUpliftByEmployeeId(EmpId, date);
                model.Date = String.Format("{0:dd-MM-yyy}", data.Date);
                model.Comment = data.Comments;
                model.UpliftPostionId = (int)data.UpliftPostionId;
                model.ProjectId = (int)data.ProjectId;
                model.CustomerId = data.CustomerId;

                if(EmpId!=0 && EmpId!=null)
                {
                    var EmployeeName = _db.AspNetUsers.Where(x => x.Id == EmpId).FirstOrDefault();
                    model.EmployeeName = EmployeeName.FirstName + " " + EmployeeName.LastName + " " + EmployeeName.SSOID;
                    model.EmployeeId = EmpId;
                }
                if(data!=null)
                {
                    int CmpId = Convert.ToInt32(data.CustomerId);
                    if (CmpId != 0 && data.CustomerId != null && data.CustomerId!="")
                    {
                        var Employee = _db.AspNetUsers.Where(x => x.Id == CmpId).FirstOrDefault();
                        if (Employee != null)
                        {
                            if (Employee.FirstName != "" && Employee.FirstName != null && Employee.LastName != "" && Employee.LastName != null)
                            {
                                model.Customer = Employee.FirstName + " " + Employee.LastName + " " + Employee.SSOID;
                                model.CustomerId = data.CustomerId;
                            }
                        }
                    }
                    int UpId = data.Id;
                    var UpliftDetail = _employeeProjectPlannerMethod.getAllUpliftDetail(UpId);
                    foreach (var Uplift in UpliftDetail)
                    {
                        EmployeeProjectPlanner_Uplift_DetailViewModel detailModel = new EmployeeProjectPlanner_Uplift_DetailViewModel();
                        detailModel.Id = Uplift.Id;
                        detailModel.InTimeHr = Uplift.InTimeHr;
                        detailModel.InTimeMin = Uplift.InTimeMin;
                        detailModel.EndTimeHr = Uplift.OutTimeHr;
                        detailModel.EndTimeMin = Uplift.OutTimeMin;
                        detailModel.FlagD = 1;
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
                        for (int i = 0; i < 60; i++)
                        {
                            detailModel.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                        }
                        for (int i = 0; i < 24; i++)
                        {
                            detailModel.HoursList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                        }
                        model.DetailList.Add(detailModel);
                    }

                    var UpliftDoument = _employeeProjectPlannerMethod.getAllUpliftDocument(UpId);
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
               
            }
            else
            {
                model.Date = String.Format("{0:dd-MM-yyy}", date);
                model.CustomerId = "0";
                model.ProjectId = 0;

            }
            return PartialView("_partialAdd_Uplift", model);
        }


        public ActionResult ExportExcelUplift(int Day, int month, int Year, int? BusiId, int? DiviId, int? PoolId, int? FunctId, int? ProjectId)
        {
            string ResourceList = "ResourceList";
            string isUplift = "1";
            ResourcesAsminProjectPlannerViewModel model = returnUpliftList(isUplift, Day, month, Year, BusiId, DiviId, PoolId, FunctId, ProjectId);
            DataTable dttable = new DataTable("Uplift");
            dttable.Columns.Add("Name", typeof(string));
            dttable.Columns.Add("Job Title", typeof(string));
            // dttable.Columns.Add("Days", typeof(string));
            dttable.Columns.Add("Business", typeof(string));
            dttable.Columns.Add("Division", typeof(string));
            dttable.Columns.Add("Pool", typeof(string));
            dttable.Columns.Add("Project", typeof(string));
            dttable.Columns.Add("Customer", typeof(string));
        //    dttable.Columns.Add("AssetName", typeof(string));
       //     dttable.Columns.Add("CostCode", typeof(string));
            dttable.Columns.Add("Status", typeof(string));
            foreach (var item in model.GetAllList)
            {
                List<string> lstStrRow = new List<string>();
                lstStrRow.Add(item.Resource_Name_SSO);
                lstStrRow.Add(item.jobtitle);
                //      lstStrRow.Add(item.Days);
                lstStrRow.Add(item.Business);
                lstStrRow.Add(item.Division);
                lstStrRow.Add(item.Pool);
                lstStrRow.Add(item.Project);
                lstStrRow.Add(item.Customer);
              //  lstStrRow.Add(item.AssetName);
             //   lstStrRow.Add(item.CostCode);
                lstStrRow.Add(item.Status);
                // lstStrRow.Add(null);
                string[] newArray = lstStrRow.ToArray();
                dttable.Rows.Add(newArray);
            }
            #region export file
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dttable);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + ResourceList + "_Skills.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            #endregion
            return View();
        }




        //Other
        public JsonResult GetDivisonByBusinessId(int BusinessID)
        {
            List<KeyValue> listOfKeyValue = new List<KeyValue>();
            try
            {
                listOfKeyValue = _db.Divisions.Where(x => x.BusinessID == BusinessID).Select(xx => new KeyValue
                {
                    Key = xx.Id,
                    Value = xx.Name
                }).ToList();

                return Json(listOfKeyValue, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPoolByDivisonID(int BusinessId, int DivisonID)
        {
            List<KeyValue> listOfKeyValue = new List<KeyValue>();
            try
            {
                listOfKeyValue = _db.Pools.Where(x => x.DivisionID == DivisonID && x.BusinessID == BusinessId).Select(xx => new KeyValue
                {
                    Key = xx.Id,
                    Value = xx.Name
                }).ToList();
                return Json(listOfKeyValue, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFunctionByPoolId(int BusinessId, int DivisonID)
        {
            List<KeyValue> listOfKeyValue = new List<KeyValue>();
            try
            {
                listOfKeyValue = _db.Functions.Where(x => x.DivisionID == DivisonID && x.BusinessID == BusinessId).Select(xx => new KeyValue
                {
                    Key = xx.Id,
                    Value = xx.Name
                }).ToList();
                return Json(listOfKeyValue, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetResourceByFunctionAndPoolID(int PoolId, int FunctionID)
        {
            int empId = SessionProxy.UserId;
            List<KeyValue> listOfKeyValue = new List<KeyValue>();
            try
            {
                var data = _db.AspNetUserRoles.Where(x => x.UserId == empId && x.RoleId == 1).ToList();
                if (data.Count > 0 && data != null)
                {
                    var EmpRelation = _db.EmployeeRelations.Where(x => x.FunctionID == FunctionID && x.PoolID == PoolId && x.IsActive == true).ToList();
                    foreach (var item in EmpRelation)
                    {
                        KeyValue KeyValue = new KeyValue();
                        var AspNetUser = _db.AspNetUsers.Where(x => x.Id == item.UserID).FirstOrDefault();
                        KeyValue.Key = AspNetUser.Id;
                        KeyValue.Value = AspNetUser.FirstName + ' ' + AspNetUser.LastName;
                        listOfKeyValue.Add(KeyValue);
                    }
                }
                else
                {
                    var EmpRelation = _db.EmployeeRelations.Where(x => x.FunctionID == FunctionID && x.PoolID == PoolId && x.IsActive == true && x.Reportsto==empId).ToList();
                    foreach (var item in EmpRelation)
                    {
                        KeyValue KeyValue = new KeyValue();
                        var AspNetUser = _db.AspNetUsers.Where(x => x.Id == item.UserID).FirstOrDefault();
                        KeyValue.Key = AspNetUser.Id;
                        KeyValue.Value = AspNetUser.FirstName + ' ' + AspNetUser.LastName;
                        listOfKeyValue.Add(KeyValue);
                    }
                }
                return Json(listOfKeyValue, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

    }
}