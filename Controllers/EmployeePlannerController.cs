using HRTool.CommanMethods.Resources;
using HRTool.DataModel;
using HRTool.Models.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HRTool.CommanMethods.Settings;
using System.Configuration;
using System.IO;
using System.Web.Script.Serialization;
using HRTool.CommanMethods.RolesManagement;
using HRTool.Models.Settings;
using HRTool.CommanMethods;
using Rotativa;
using Rotativa.Options;
using static HRTool.CommanMethods.Enums;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class EmployeePlannerController : Controller
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        EmployeePlannerMethod _employeePlannerMethod = new EmployeePlannerMethod();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        EmployeeMethod _employeeMethod = new EmployeeMethod();
        ProjectSettindsMethod _projectSettindsMethod = new ProjectSettindsMethod();
        HolidayNAbsenceMethod _holidayNAbsenceMethod = new HolidayNAbsenceMethod();
        RolesManagementMethod _RolesManagementMethod = new RolesManagementMethod();
        private string inputFormat = "dd-MM-yyyy";
        private string outputFormat = "yyyy-MM-dd HH:mm:ss";

        #endregion

        #region Index
        public ActionResult Index(int EmployeeId)
        {
            EmployeePlannerViewModel model = new EmployeePlannerViewModel();
            model.yearId = DateTime.Now.Year;
            model.currentMonth = DateTime.Now.Month;
            model.EmployeeId = EmployeeId;
            int userID = SessionProxy.UserId;
            var employee = _employeeMethod.getEmployeeById(userID);
            //model.RemainingDays = _employeePlannerMethod.GetAllRemainingDaysHolidys(EmployeeId);
            //model.bookDays = _employeePlannerMethod.GetAllBookHolidays(EmployeeId);
            model.bookDays = _employeePlannerMethod.GetAllBookedHolidayCount(EmployeeId);
            decimal reaminHoliday= _employeePlannerMethod.GetAllHolidayByEmployee(EmployeeId) - model.bookDays;
            if(reaminHoliday>0)
            {
                model.RemainingDays = reaminHoliday;
            }
            else
            {
                model.RemainingDays = 0;
            }
           
            //model.RemainingDays = _employeePlannerMethod.GetAllHolidayAbscence(EmployeeId) - model.bookDays;
            model.SickLeavesDays = _employeePlannerMethod.GetAllSickLeaveByEmployeeId(EmployeeId);
            model.WorkingDays = _employeePlannerMethod.GetAllWorkingDaysByEmployeeId(EmployeeId);
            var bradfordfactor = _employeeMethod.GetBradFordFactorDetails();
            model.LowerValue1 = (bradfordfactor.LowerValue1 == null ? 0 : (int)bradfordfactor.LowerValue1);
            model.LowerValue2 = (bradfordfactor.LowerValue2 == null ? 0 : (int)bradfordfactor.LowerValue2);
            model.LowerValue3 = (bradfordfactor.LowerValue3 == null ? 0 : (int)bradfordfactor.LowerValue3);
            model.LowerValue4 = (bradfordfactor.LowerValue4 == null ? 0 : (int)bradfordfactor.LowerValue4);
            model.UpperValue1 = (bradfordfactor.UpperValue1 == null ? 0 : (int)bradfordfactor.UpperValue1);
            model.UpperValue2 = (bradfordfactor.UpperValue2 == null ? 0 : (int)bradfordfactor.UpperValue2);
            model.UpperValue3 = (bradfordfactor.UpperValue3 == null ? 0 : (int)bradfordfactor.UpperValue3);
            model.UpperValue4 = (bradfordfactor.UpperValue4 == null ? 0 : (int)bradfordfactor.UpperValue4);

            model.BradfordFactor = _employeeMethod.GetBradFordFactorCount(EmployeeId);

            //model.minValue = 0;
            //model.maxValue = Convert.ToInt32(model.BradfordFactor) * 2;

            model.HolidayYear = _employeeMethod.GetHolidayYear();

            WorkpatternWeekend _WorkpatternWeekend = _employeeMethod.GetAllsickLeaveDayCount(EmployeeId);

            model.SundayDays = _WorkpatternWeekend.SundayDays != null ? _WorkpatternWeekend.SundayDays.Value : 0;
            model.MondayDays = _WorkpatternWeekend.MondayDays != null ? _WorkpatternWeekend.MondayDays.Value : 0;
            model.TuesdayDays = _WorkpatternWeekend.TuesdayDays != null ? _WorkpatternWeekend.TuesdayDays.Value : 0;
            model.WednessdayDays = _WorkpatternWeekend.WednessdayDays != null ? _WorkpatternWeekend.WednessdayDays.Value : 0;
            model.ThursdayDays = _WorkpatternWeekend.ThursdayDays != null ? _WorkpatternWeekend.ThursdayDays.Value : 0;
            model.FridayDays = _WorkpatternWeekend.FridayDays != null ? _WorkpatternWeekend.FridayDays.Value : 0;
            model.SaturdayDays = _WorkpatternWeekend.SaturdayDays != null ? _WorkpatternWeekend.SaturdayDays.Value : 0;

            model.Point = _employeePlannerMethod.GetPointByEmployeeId(EmployeeId);

            var HolidayDays = _employeePlannerMethod.GetAllWorkingDaysByEmployeeId(EmployeeId);

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
            var employeeData = _db.AspNetUsers.Where(x => x.Id == EmployeeId).ToList();         
            if(employeeData.FirstOrDefault().JobContryID!=null && employeeData.FirstOrDefault().JobContryID !=0)
            {
              var countryList = _holidayNAbsenceMethod.getHolidayAndAbsenceByEmployee(EmployeeId);
                foreach (var item in countryList)
                {
                    model.LastCountryId =Convert.ToInt32(item.JobContryID);
                    //model.CountryList.Add(new SelectListItem() { Text = item.Name, Value = item.publicHolidayId.ToString() });
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
            if (AssignCountry.Count > 0)
            {
                var Details = _employeePlannerMethod.publicHolidayListByEmployeeId(EmployeeId).OrderByDescending(x => x.EffecitveFrom).FirstOrDefault();                
                var Country = _holidayNAbsenceMethod.GetPublicHolidayCountryById((int)Details.PublicHolidayCountryId);
                if(Country != null)
                model.LastCountryId = Country.Id;
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
                else {
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
            return View(model);
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
            if(data.AuthorisorEmployeeID!=null && data.AuthorisorEmployeeID!="")
            {
                int AuthorisorEmpId = Convert.ToInt32(data.AuthorisorEmployeeID);
                var employeeData = _db.AspNetUsers.Where(x => x.Id == AuthorisorEmpId && x.Archived==false).FirstOrDefault();
                model.authorisationEmployeeName = employeeData.FirstName + employeeData.LastName + "-" + employeeData.SSOID;
            }
            if (data.AuthorisorEmployeeID != null && data.AuthorisorEmployeeID != "")
            {
                model.AuthoriseUserId = data.AuthorisorEmployeeID;
            }
            if (data.TOIL != null && data.TOIL!=0)
            {
                model.TOIL = (int)data.TOIL;
            }
            if (data.InculudedCarriedOver != null && data.InculudedCarriedOver!=0)
            {
                model.CarriedOver = (int)data.InculudedCarriedOver;
            }
            if (data.HolidayYear != null && data.HolidayYear!=0)
            {
                model.HolidayYear = (int)data.HolidayYear;
            }
            if (data.MeasuredIn != null && data.MeasuredIn!=0)
            {
                model.MeasuredIn = (int)data.MeasuredIn;
            }
            if (data.Thisyear != null && data.Thisyear!=0)
            {
                model.Thisyear = (int)data.Thisyear;
            }
            if (data.Nextyear != null && data.Nextyear!=0)
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
        public ActionResult getreRouteauthorisaton(int Id)
        {
            EmployeeAllowanceSettings model = new EmployeeAllowanceSettings();

            foreach (var item in _RolesManagementMethod.GetEmployeeManagerList())
            {
                if (item.Id != Id)
                {
                    model.authorisationsList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveAnnualSettings(AspNetUser data)
        {
            //AspNetUser model = _employeeMethod.getEmployeeById(data.Id); // _db.AspNetUsers.Where(x => x.Id == data.Id).FirstOrDefault();
            //model.AuthorisorEmployeeID = data.AuthorisorEmployeeID;
            //model.HolidayYear = data.HolidayYear;
            //model.MeasuredIn = data.MeasuredIn;
            //model.Thisyear = data.Thisyear;
            //model.Nextyear = data.Nextyear;
            //model.EntitlementIncludesPublicHoliday = data.EntitlementIncludesPublicHoliday;
            //model.AutoApproveHolidays = data.AutoApproveHolidays;
            //model.ExceedAllowance = data.ExceedAllowance;
            //_db.SaveChanges();
            if (data.Id != 0 && data.Id != null)
            {
                _employeePlannerMethod.UpdateAnnualSettings(data);
            }
            return Json("Record Update Successfully",JsonRequestBehavior.AllowGet);

        }

        public ActionResult AddEditTOIL(int Id)
        {

            EmployeeTOILModelView model = new EmployeeTOILModelView();
            var data = _employeeMethod.getEmployeeById(Id);//_db.AspNetUsers.Where(x => x.Id == Id).FirstOrDefault();
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


        #region Common Method
        public ActionResult ListOfMonth(int year, int EmployeeId, int HolidayCountryID)
        {
            EmployeePlannerYearViewModel yearModel = new EmployeePlannerYearViewModel();
            var annualLeave = _employeePlannerMethod.getAllAnnualLeave().Where(x => x.EmployeeId == EmployeeId).ToList();
            var lateLeave = _employeePlannerMethod.getAllLateLeave().Where(x => x.EmployeeId == EmployeeId).ToList();
            var otherLeave = _employeePlannerMethod.getAllOtherLeave().Where(x => x.EmployeeId == EmployeeId).ToList();
            var travelLeave = _employeePlannerMethod.getAllTravelLeave().Where(x => x.EmployeeId == EmployeeId).ToList();
            var timeSheet = _employeePlannerMethod.getAllTimeSheet().Where(x => x.EmployeeId == EmployeeId).ToList();
            var publicHolidayList = _employeePlannerMethod.getGetAllPublicHolidayByEmployee_JobCountry(EmployeeId).ToList();
            //var publicHolidayList = _employeePlannerMethod.publicHolidayList().Where(x => x.EmployeeId == EmployeeId).ToList();
            var empData = _db.AspNetUsers.Where(x => x.Id == EmployeeId && x.Archived == false).FirstOrDefault();
            int jobBountryId = Convert.ToInt32(empData.JobContryID);
            var publicHolidayListByCountry = _employeePlannerMethod.getPublicHolidayByCountryList(jobBountryId);
            if (publicHolidayList != null && publicHolidayList.Count > 0)
            {
                var PublicHolidayListByEmp = publicHolidayList.Where(x => x.EffectiveTo == null).FirstOrDefault();
                if (PublicHolidayListByEmp != null)
                {
                    publicHolidayListByCountry = publicHolidayListByCountry.Where(x => x.PublicHolidayDate <= PublicHolidayListByEmp.EffecitveFrom).ToList();
                    var NewpublicHolidayListByCountry = _employeePlannerMethod.getPublicHolidayByCountryList(PublicHolidayListByEmp.PublicHolidayCountryID);
                    publicHolidayListByCountry.AddRange(NewpublicHolidayListByCountry.Where(x => x.PublicHolidayDate >= PublicHolidayListByEmp.EffecitveFrom).ToList());
                }
            }

            var holidayList = _holidayNAbsenceMethod.getHolidayList().ToList();
            var sickLeaveList = _employeePlannerMethod.getAllSickLeave().Where(x => x.EmployeeId == EmployeeId).ToList();
            var employeeWorkPatternList = _employeePlannerMethod.getAllEmployeeWorkPattern().Where(x => x.EmployeeID == EmployeeId).ToList();
            var workPattenList = _holidayNAbsenceMethod.getAllWorkPattern();
            var maternityPaternityList = _employeePlannerMethod.getAllMaternityPaternityLeave().Where(x => x.EmployeeID == EmployeeId).ToList();
            var rotatingWorkPatternList = _holidayNAbsenceMethod.allWorkPatternDetail();

            yearModel.yearId = year;
            int TotalMonths = 12;
            int maxRotaingCount = 0;
            int currentRotatingWeekDays = 0;
            for (int i = 1; i <= TotalMonths; i++)
            {
                EmployeePlannerMonthViewModel monthModel = new EmployeePlannerMonthViewModel();
                monthModel.monthId = i;
                monthModel.yearId = year;
                monthModel.MonthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(i);
                var startDate = new DateTime(year, i, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                monthModel.SumAnnualLeave = _employeePlannerMethod.GetAllAnnualLeavesMonthWiseCount(EmployeeId, startDate, endDate);
                monthModel.sumLateLeave = _employeePlannerMethod.getAllLateLeavesMonthWiseCount(EmployeeId, startDate, endDate);
                monthModel.TimesheetCount = _employeePlannerMethod.GetTimeSheetTotalTimeInMonth(EmployeeId, startDate, endDate);
                monthModel.TravelCount = _employeePlannerMethod.GetAllTravelLeavesMonthWiseCount(EmployeeId, startDate, endDate);
                monthModel.OtherLeaveCount = _employeePlannerMethod.GetAllOtherLeavesMonthWiseCount(EmployeeId, startDate, endDate);
                monthModel.SickLeaveCount = _employeePlannerMethod.GetSickLeavesTotalTimeInMonth(EmployeeId, startDate, endDate);
                if(publicHolidayListByCountry!=null && publicHolidayListByCountry.Count>0)
                {
                    int totalHoliday = 0;
                    foreach(var item in publicHolidayListByCountry)
                    {
                        if(item.PublicHolidayDate.Value.Month==i)
                        {
                            totalHoliday = totalHoliday + 1;
                        }                        
                    }
                    monthModel.PublicHolidaysCount = Convert.ToString(totalHoliday);
                }
                //if(publicHolidayList.Count>0)
                //{
                //    int pHoliday = _employeePlannerMethod.GetAllPublicHolidayByMonth_JobCountryId(EmployeeId, i);
                //    monthModel.PublicHolidaysCount = Convert.ToString(pHoliday);
                //}
                //else
                //{                    
                //    int pHoliday = _employeePlannerMethod.GetAllPublicHolidayBy_AspnetUsersJobCountry(HolidayCountryID, i);
                //    monthModel.PublicHolidaysCount = Convert.ToString(pHoliday);
                //}
               // monthModel.PublicHolidaysCount = _employeePlannerMethod.GetAllPublicHolidaysMonthWise(EmployeeId, startDate, endDate);
                
               monthModel.MaternityorPaternity = _employeePlannerMethod.getAllMaternityPaternityLeaveMonthWise(EmployeeId, startDate, endDate);

                //monthModel.SumAnnualLeave = (decimal)annualLeave.Where(x=>x).Select(x => x.Duration).Sum();
                //monthModel.sumLateLeave = lateLeave.Count();

                int totalDays = DateTime.DaysInMonth(year, i);
                for (int j = 1; j <= totalDays; j++)
                {
                    EmployeePlannerDayViewModel dayModel = new EmployeePlannerDayViewModel();
                    dayModel.day = j;
                    var date = new DateTime(year, i, j);
                    dayModel.DayName = date.DayOfWeek.ToString();
                    dayModel.monthId = i;
                    dayModel.yearId = year;
                    dayModel.TimeSheetId = 0;
                    dayModel.TravelLeaveId = 0;
                    dayModel.AnnualLeaveId = 0;
                    dayModel.SickLeaveId = 0;
                    dayModel.LateLeaveId = 0;
                    dayModel.PublicholidayId = 0;
                    dayModel.MaternityLeaveId = 0;
                    dayModel.OtherLeaveId = 0;

                    #region Work Pattern

                    if (employeeWorkPatternList.Count == 1)
                    {
                        Employee_WorkPattern saveWorkPattern = employeeWorkPatternList.FirstOrDefault();
                        if (date.Date >= saveWorkPattern.EffectiveFrom.Date)
                        {
                            dayModel.Flag = 0;
                            WorkPattern workPatternDetail = workPattenList.Where(x => x.Id == saveWorkPattern.WorkPatternID).OrderByDescending(x => saveWorkPattern.EffectiveFrom).FirstOrDefault();
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

                                            #endregion
                                    }
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
                    else if (employeeWorkPatternList.Count > 1)
                    {
                        foreach (var item in employeeWorkPatternList)
                        {
                            if (date.Date >= item.EffectiveFrom.Date)
                            {
                                dayModel.Flag = 0;
                                currentRotatingWeekDays++;
                                WorkPattern workPatternDetail = workPattenList.Where(x => x.Id == item.WorkPatternID).OrderByDescending(x => item.EffectiveFrom).FirstOrDefault(); 
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

                                                #endregion
                                        }
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

                                                    #endregion
                                            }


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

                    var annualLeaveTaken = annualLeave.Where(x => date.Date >= x.StartDate.Value.Date && date.Date <= x.EndDate.Value.Date).FirstOrDefault();
                    if (annualLeaveTaken != null)
                    {
                        dayModel.AnnualLeaveId = annualLeaveTaken.Id;
                        dayModel.isAnnualLeaveTaken = true;
                    }

                    var lateLeaveTaken = lateLeave.Where(x => date.Date == x.LateDate.Value.Date).FirstOrDefault();
                    if (lateLeaveTaken != null)
                    {
                        dayModel.LateLeaveId = lateLeaveTaken.Id;
                        dayModel.isLateLeaveTaken = true;
                    }

                    var otherLeaveTaken = otherLeave.Where(x => date.Date >= x.StartDate.Value.Date && date.Date <= x.EndDate.Value.Date).FirstOrDefault();
                    if (otherLeaveTaken != null)
                    {
                        dayModel.OtherLeaveId = otherLeaveTaken.Id;
                        dayModel.isOtherLeaveTaken = true;
                    }

                    //var PublicHolidayListTaken = publicHolidayList.Where(x => x.PublicHolidayDate == date.Date).FirstOrDefault();
                    //if (PublicHolidayListTaken != null)
                    //{
                    //    dayModel.PublicholidayId = PublicHolidayListTaken.HolidayId;
                    //    dayModel.isPublicHoliday = true;
                    //}                    
                    // if (publicHolidayList.Count > 0)
                    //{
                        
                    //    var PublicHolidayListTaken = publicHolidayList.Where(x => x.HolidayDate == date.Date).FirstOrDefault();
                    //    if (PublicHolidayListTaken != null)
                    //    {
                    //        dayModel.PublicholidayId = PublicHolidayListTaken.PublicHolidayId;
                    //        dayModel.isPublicHoliday = true;                            
                    //    }
                    //}
                    //else
                    //{
                        var PublicHolidayListTaken = publicHolidayListByCountry.Where(x => x.PublicHolidayDate == date.Date).FirstOrDefault();
                        if (PublicHolidayListTaken != null)
                        {
                            dayModel.PublicholidayId = PublicHolidayListTaken.HolidayId;
                            dayModel.isPublicHoliday = true;
                        }
                       

                    //}
                    //#region public holiday

                    //if (publicHolidayList.Count == 1)
                    //{
                    //    if (date.Date >= publicHolidayList.FirstOrDefault().EffecitveFrom.Value.Date)
                    //    {
                    //        var newHolidayList = holidayList.Where(x => x.PublicHolidayCountryID == publicHolidayList.FirstOrDefault().PublicHolidayCountryId).Select(a => a.Date.Value.Date).ToList();

                    //        if (newHolidayList.Where(x => x.Date == date.Date).ToList().Count > 0)
                    //        {
                    //            dayModel.PublicholidayId = (int)publicHolidayList.FirstOrDefault().PublicHolidayCountryId;
                    //            dayModel.isPublicHoliday = true;
                    //        }
                    //    }
                    //}
                    //else if (publicHolidayList.Count > 1)
                    //{
                    //    var lastRecord = publicHolidayList.LastOrDefault();
                    //    foreach (var item in publicHolidayList)
                    //    {
                    //        if (item != lastRecord)
                    //        {
                    //            if (date.Date >= item.EffecitveFrom.Value.Date && date.Date <= item.EffectiveTo.Value.Date)
                    //            {
                    //                var newHolidayList = holidayList.Where(x => x.PublicHolidayCountryID == item.PublicHolidayCountryId).Select(a => a.Date.Value.Date).ToList();

                    //                if (newHolidayList.Where(x => x.Date == date.Date).ToList().Count > 0)
                    //                {
                    //                    dayModel.PublicholidayId = (int)item.PublicHolidayCountryId;
                    //                    dayModel.isPublicHoliday = true;
                    //                }
                    //            }
                    //        }
                    //        else
                    //        {
                    //            if (date.Date >= item.EffecitveFrom.Value.Date)
                    //            {
                    //                var newHolidayList = holidayList.Where(x => x.PublicHolidayCountryID == item.PublicHolidayCountryId).Select(a => a.Date.Value.Date).ToList();

                    //                if (newHolidayList.Where(x => x.Date == date.Date).ToList().Count > 0)
                    //                {
                    //                    dayModel.PublicholidayId = (int)item.PublicHolidayCountryId;
                    //                    dayModel.isPublicHoliday = true;
                    //                }
                    //            }
                    //        }

                    //    }
                    //}
                    //else { }

                    //#endregion

                    var sickLeaveTaken = sickLeaveList.Where(x => date.Date >= x.StartDate.Value.Date && date.Date <= (x.EndDate != null ? x.EndDate.Value.Date : x.StartDate.Value.Date)).FirstOrDefault();
                    if (sickLeaveTaken != null)
                    {
                        dayModel.SickLeaveId = sickLeaveTaken.Id;
                        dayModel.isSickLeaveTaken = true;
                    }
                    var MaternityLeaveTaken = maternityPaternityList.Where(x => date.Date >= x.ActualStartDate.Value.Date & date.Date <= x.ActualEndDate.Value.Date).FirstOrDefault();
                    if (MaternityLeaveTaken != null)
                    {
                        dayModel.MaternityLeaveId = MaternityLeaveTaken.Id;
                        dayModel.isMaternityPaternityLeaveTaken = true;
                    }

                    if (dayModel.isAnnualLeaveTaken || dayModel.isLateLeaveTaken || dayModel.isOtherLeaveTaken || dayModel.isPublicHoliday || dayModel.isSickLeaveTaken || dayModel.isMaternityPaternityLeaveTaken)
                    {
                        dayModel.isLeave = true;
                    }

                    monthModel.DayList.Add(dayModel);
                }
                yearModel.MonthList.Add(monthModel);
            }
            return PartialView("_partialListOfMonthYearWise", yearModel);
        }
        public List<EmployeePlannerDayViewModel> returnDayListTable(int MonthId, int YearId, int EmployeeId, int HolidayCountryID)
        {
            List<EmployeePlannerDayViewModel> model = new List<EmployeePlannerDayViewModel>();
            var annualLeave = _employeePlannerMethod.getAllAnnualLeave().Where(x => x.EmployeeId == EmployeeId).ToList();
            var lateLeave = _employeePlannerMethod.getAllLateLeave().Where(x => x.EmployeeId == EmployeeId).ToList();
            var otherLeave = _employeePlannerMethod.getAllOtherLeave().Where(x => x.EmployeeId == EmployeeId).ToList();
            var travelLeave = _employeePlannerMethod.getAllTravelLeave().Where(x => x.EmployeeId == EmployeeId).ToList();
            var timeSheet = _employeePlannerMethod.getAllTimeSheet().Where(x => x.EmployeeId == EmployeeId).ToList();
            //var publicHolidayList = _employeePlannerMethod.publicHolidayList().Where(x => x.EmployeeId == EmployeeId).ToList();
            //var publicHolidayList = _employeePlannerMethod.getGetAllPublicHolidayByEmployee_JobCountry(EmployeeId).ToList();
            var publicHolidayList = _employeePlannerMethod.getGetAllPublicHolidayByEmployee_JobCountry(EmployeeId).ToList();
            //var publicHolidayList = _employeePlannerMethod.publicHolidayList().Where(x => x.EmployeeId == EmployeeId).ToList();
            var empData = _db.AspNetUsers.Where(x => x.Id == EmployeeId && x.Archived == false).FirstOrDefault();
            int jobBountryId = Convert.ToInt32(empData.JobContryID);
            var publicHolidayListByCountry = _employeePlannerMethod.getPublicHolidayByCountryList(jobBountryId);
            if (publicHolidayList != null && publicHolidayList.Count > 0)
            {
                var PublicHolidayListByEmp = publicHolidayList.Where(x => x.EffectiveTo == null).FirstOrDefault();
                if (PublicHolidayListByEmp != null)
                {
                    publicHolidayListByCountry = publicHolidayListByCountry.Where(x => x.PublicHolidayDate <= PublicHolidayListByEmp.EffecitveFrom).ToList();
                    var NewpublicHolidayListByCountry = _employeePlannerMethod.getPublicHolidayByCountryList(PublicHolidayListByEmp.PublicHolidayCountryID);
                    publicHolidayListByCountry.AddRange(NewpublicHolidayListByCountry.Where(x => x.PublicHolidayDate >= PublicHolidayListByEmp.EffecitveFrom).ToList());
                }
            }
            //var publicHolidayList = _employeePlannerMethod.getPublicHolidayByCountryList(HolidayCountryID);
            var holidayList = _holidayNAbsenceMethod.getHolidayList().ToList();
            int totalDays = DateTime.DaysInMonth(YearId, MonthId);
            var sickLeaveList = _employeePlannerMethod.getAllSickLeave().Where(x => x.EmployeeId == EmployeeId).ToList();
            var workPatternList = _employeePlannerMethod.getAllEmployeeWorkPattern().Where(x => x.EmployeeID == EmployeeId).ToList();
            var maternityPaternityList = _employeePlannerMethod.getAllMaternityPaternityLeave().Where(x => x.EmployeeID == EmployeeId).ToList();

            for (int j = 1; j <= totalDays; j++)
            {
                EmployeePlannerDayViewModel dayModel = new EmployeePlannerDayViewModel();
                dayModel.day = j;
                var date = new DateTime(YearId, MonthId, j);
                dayModel.DayName = date.DayOfWeek.ToString();
                dayModel.monthId = MonthId;
                dayModel.yearId = YearId;
                dayModel.TimeSheetId = 0;
                dayModel.TravelLeaveId = 0;
                dayModel.AnnualLeaveId = 0;
                dayModel.SickLeaveId = 0;
                dayModel.LateLeaveId = 0;
                dayModel.PublicholidayId = 0;
                dayModel.MaternityLeaveId = 0;
                dayModel.OtherLeaveId = 0;

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

                var annualLeaveTaken = annualLeave.Where(x => date.Date >= x.StartDate.Value.Date && date.Date <= x.EndDate.Value.Date).FirstOrDefault();
                if (annualLeaveTaken != null)
                {
                    dayModel.AnnualLeaveId = annualLeaveTaken.Id;
                    dayModel.isAnnualLeaveTaken = true;
                }
                var lateLeaveTaken = lateLeave.Where(x => date.Date == x.LateDate.Value.Date).FirstOrDefault();
                if (lateLeaveTaken != null)
                {
                    dayModel.LateLeaveId = lateLeaveTaken.Id;
                    dayModel.isLateLeaveTaken = true;
                }

                var otherLeaveTaken = otherLeave.Where(x => date.Date >= x.StartDate.Value.Date && date.Date <= x.EndDate.Value.Date).FirstOrDefault();
                if (otherLeaveTaken != null)
                {
                    dayModel.OtherLeaveId = otherLeaveTaken.Id;
                    dayModel.isOtherLeaveTaken = true;
                }
                var sickLeaveTaken = sickLeaveList.Where(x => date.Date >= x.StartDate.Value.Date && date.Date <= (x.EndDate != null ? x.EndDate.Value.Date : x.StartDate.Value.Date)).FirstOrDefault();
                if (sickLeaveTaken != null)
                {
                    dayModel.SickLeaveId = sickLeaveTaken.Id;
                    dayModel.isSickLeaveTaken = true;
                }

                var maternitypaternityTaken = maternityPaternityList.Where(x => date.Date >= x.ActualStartDate.Value.Date && date.Date <= x.ActualEndDate.Value.Date).FirstOrDefault();
                if (maternitypaternityTaken != null)
                {
                    dayModel.MaternityLeaveId = maternitypaternityTaken.Id;
                    dayModel.isMaternityPaternityLeaveTaken = true;
                }

                var PublicHolidayListTaken = publicHolidayListByCountry.Where(x => x.PublicHolidayDate == date.Date).FirstOrDefault();
                if (PublicHolidayListTaken != null)
                {
                    dayModel.PublicholidayId = PublicHolidayListTaken.HolidayId;
                    dayModel.isPublicHoliday = true;
                }
                //#region public holiday
                //if (publicHolidayList.Count == 1)
                //{
                //    if (date.Date >= publicHolidayList.FirstOrDefault().EffecitveFrom.Value.Date)
                //    {
                //        var newHolidayList = holidayList.Where(x => x.PublicHolidayCountryID == publicHolidayList.FirstOrDefault().PublicHolidayCountryId).Select(a => a.Date.Value).ToList();

                //        if (newHolidayList.Where(x => x.Date == date.Date).ToList().Count > 0)
                //        {
                //            dayModel.isPublicHoliday = true;
                //        }
                //    }
                //}
                //else if (publicHolidayList.Count > 1)
                //{
                //    var lastRecord = publicHolidayList.LastOrDefault();
                //    foreach (var item in publicHolidayList)
                //    {
                //        if (item != lastRecord)
                //        {
                //            if (date.Date >= item.EffecitveFrom.Value.Date && date.Date <= item.EffectiveTo.Value.Date)
                //            {
                //                var newHolidayList = holidayList.Where(x => x.PublicHolidayCountryID == item.PublicHolidayCountryId).Select(a => a.Date.Value.Date).ToList();

                //                if (newHolidayList.Where(x => x.Date == date.Date).ToList().Count > 0)
                //                {
                //                    dayModel.isPublicHoliday = true;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            if (date.Date >= item.EffecitveFrom.Value.Date)
                //            {
                //                var newHolidayList = holidayList.Where(x => x.PublicHolidayCountryID == item.PublicHolidayCountryId).Select(a => a.Date.Value.Date).ToList();

                //                if (newHolidayList.Where(x => x.Date == date.Date).ToList().Count > 0)
                //                {
                //                    dayModel.isPublicHoliday = true;
                //                }
                //            }
                //        }

                //    }
                //}
                //else { }

                //#endregion

                if (dayModel.isAnnualLeaveTaken || dayModel.isLateLeaveTaken || dayModel.isOtherLeaveTaken || dayModel.isPublicHoliday || dayModel.isSickLeaveTaken)
                {
                    dayModel.isLeave = true;
                }

                model.Add(dayModel);
            }
            return model;
        }

        #endregion
        public JsonResult GetWorkPatternData(EmployeePlanner_TimeSheetViewModel model)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<EmployeePlanner_TimeSheet_DetailViewModel> listDetail = js.Deserialize<List<EmployeePlanner_TimeSheet_DetailViewModel>>(model.jsonDetailList);
            model.DetailList = listDetail;
            string inputFormat = "dd-MM-yyyy";
            var wData = _employeeMethod.getWorkPatternById(model.EmployeeId).OrderByDescending(x=>x.EffectiveFrom).FirstOrDefault();
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

        public ActionResult getEmployee(int Id)
        {
            int userId = SessionProxy.UserId;

            EmployeeProjectPlanner_TimeSheet_DetailViewModel detailModel = new EmployeeProjectPlanner_TimeSheet_DetailViewModel();
            detailModel.CustomerList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in _employeeMethod.GetAllCoustomerEmployeeList().Where(x => x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList())
            {
                detailModel.CustomerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }


            return Json(detailModel, JsonRequestBehavior.AllowGet);
        }
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
                    List<EmployeePlannerDayViewModel> returnModel = returnDayListTable(currentMonth, currentYear, model.EmployeeId, model.HolidayCountryID);
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
                List<EmployeePlannerDayViewModel> returnModel = returnDayListTable(currentMonth, currentYear, model.EmployeeId, model.HolidayCountryID);
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

        #region AnnualLeave

        public ActionResult AddEdit_AnnualLeave(int Id, int Date, int Month, int Year)
        {
            EmployeePlanner_AnnualLeaveViewModel model = new EmployeePlanner_AnnualLeaveViewModel();
            DateTime date = new DateTime(Year, Month, Date);
            model.Id = Id;
            model.yearId = Year;
            model.monthId = Month;
            model.day = Date;
            if (Id > 0)
            {
                var data = _employeePlannerMethod.getAnnualLeaveById(Id);
                model.StartDate = String.Format("{0:dd-MM-yyy}", data.StartDate);
                model.EndDate = String.Format("{0:dd-MM-yyy}", data.EndDate);
                model.Duration = data.Duration;
                model.IsLessThenADay = data.IsLessThenADay;
                model.TOIL = data.TOIL;
                model.Comment = data.Comment;
            }
            else
            {
                model.StartDate = String.Format("{0:dd-MM-yyy}", date);
                model.EndDate = String.Format("{0:dd-MM-yyy}", date);
                model.Duration = 1;
            }
            return PartialView("_partialAdd_AnnualLeave", model);
        }

        public ActionResult SaveData_AnnualLeave(EmployeePlanner_AnnualLeaveViewModel model)
        {
            _employeePlannerMethod.AnnualLeave_SaveData(model, SessionProxy.UserId);
            List<EmployeePlannerDayViewModel> returnModel = returnDayListTable(model.monthId, model.yearId, model.EmployeeId, model.HolidayCountry);
            return PartialView("_partialListOfDayMonthYearWise", returnModel);
        }



        #endregion

        #region Late Leave

        public ActionResult AddEdit_LateLeave(int Id, int Date, int Month, int Year)
        {
            EmployeePlanner_LateLeaveViewModel model = new EmployeePlanner_LateLeaveViewModel();
            DateTime date = new DateTime(Year, Month, Date);
            model.Id = Id;
            model.yearId = Year;
            model.monthId = Month;
            model.day = Date;
            if (Id > 0)
            {
                var data = _employeePlannerMethod.getLateLeaveById(Id);
                model.LateDate = String.Format("{0:dd-MM-yyy}", data.LateDate);
                model.LateHr = data.LateHr;
                model.LateMin = data.LateMin;
                model.Comment = data.Comments;
            }
            else
            {
                model.LateDate = String.Format("{0:dd-MM-yyy}", date);
            }
            for (int i = 0; i < 60; i++)
            {
                model.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            for (int i = 0; i < 24; i++)
            {
                model.HoursList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return PartialView("_partialAdd_LateLeave", model);
        }

        public ActionResult SaveData_LateLeave(EmployeePlanner_LateLeaveViewModel model)
        {
            _employeePlannerMethod.LateLeave_SaveData(model, SessionProxy.UserId);

            List<EmployeePlannerDayViewModel> returnModel = returnDayListTable(model.monthId, model.yearId, model.EmployeeId, model.HolidayCountryID);
            return PartialView("_partialListOfDayMonthYearWise", returnModel);
        }

        #endregion

        #region Other Leave

        public ActionResult AddEdit_OtherLeave(int Id, int Date, int Month, int Year)
        {
            EmployeePlanner_OtherLeaveViewModel model = new EmployeePlanner_OtherLeaveViewModel();
            DateTime date = new DateTime(Year, Month, Date);
            model.Id = Id;
            model.yearId = Year;
            model.monthId = Month;
            model.day = Date;
            model.Hour = 0;
            model.Min = 0;
            model.ReasonForLeaveList.Add(new SelectListItem() { Text = "-- Select Reason --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Other Leave Reason List"))
            {
                model.ReasonForLeaveList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            if (Id > 0)
            {
                var data = _employeePlannerMethod.getOtherLeaveById(Id);
                model.ReasonForLeaveId = data.ReasonForLeaveId;
                model.StartDate = String.Format("{0:dd-MM-yyy}", data.StartDate);
                model.EndDate = String.Format("{0:dd-MM-yyy}", data.EndDate);
                model.Duration = data.Duration;
                model.IsLessThenADay = data.IsLessThenADay;
                if (model.IsLessThenADay == true)
                {
                    model.Hour = (int)data.Hour;
                    model.Min = (int)data.Min;
                }
                model.Comment = data.Comment;


                var otherLeaveDoument = _employeePlannerMethod.getAllOtherLeaveDocument(Id);
                foreach (var item in otherLeaveDoument)
                {
                    EmployeePlanner_OtherLeave_DocumentsViewModel docModel = new EmployeePlanner_OtherLeave_DocumentsViewModel();
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
                model.Duration = 1;
            }
            for (int i = 0; i < 60; i++)
            {
                model.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            for (int i = 0; i < 12; i++)
            {
                model.HoursList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            return PartialView("_partialAdd_OtherLeave", model);
        }

        [HttpPost]
        public ActionResult OtherLeaveImageData()
        {
            string FilePath = string.Empty;
            string fileName = string.Empty;
            string originalFileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                FilePath = ConfigurationManager.AppSettings["Planner_OtherLeave"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
        }

        public ActionResult SaveData_OtherLeave(EmployeePlanner_OtherLeaveViewModel model)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<EmployeePlanner_OtherLeave_DocumentsViewModel> listDocument = js.Deserialize<List<EmployeePlanner_OtherLeave_DocumentsViewModel>>(model.jsonDocumentList);
            model.DocumentList = listDocument;

            _employeePlannerMethod.OtherLeave_SaveData(model, SessionProxy.UserId);

            List<EmployeePlannerDayViewModel> returnModel = returnDayListTable(model.monthId, model.yearId, model.EmployeeId, model.HolidayCountryID);
            return PartialView("_partialListOfDayMonthYearWise", returnModel);
        }

        #endregion

        #region Travel Leave

        public ActionResult AddEdit_Travel(int Id, int Date, int Month, int Year)
        {
            EmployeePlanner_TravelLeaveViewModel model = new EmployeePlanner_TravelLeaveViewModel();
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
            if (Id > 0)
            {
                var data = _employeePlannerMethod.getTravelLeaveById(Id);
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
                if (!string.IsNullOrEmpty(data.Customer) && data.Customer != null)
                {
                    int CmpId = Convert.ToInt32(data.Customer);
                    var Employee = _db.AspNetUsers.Where(x => x.Id == CmpId).FirstOrDefault();
                    model.Customer = Employee.FirstName + " " + Employee.LastName + " " + Employee.SSOID;
                    model.CustomerId = data.Customer;
                }
                //model.Customer = data.Customer;
                model.CostCode = data.CostCode;

                var otherLeaveDoument = _employeePlannerMethod.getAllTravelLeaveDocument(Id);
                foreach (var item in otherLeaveDoument)
                {
                    EmployeePlanner_TravelLeave_DocumentsViewModel docModel = new EmployeePlanner_TravelLeave_DocumentsViewModel();
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
            for (int i = 0; i < 60; i++)
            {
                model.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            for (int i = 0; i < 24; i++)
            {
                model.HoursList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
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
                FilePath = ConfigurationManager.AppSettings["Planner_TravelLeave"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
        }
        public ActionResult SaveData_TravelLeave(EmployeePlanner_TravelLeaveViewModel model)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<EmployeePlanner_TravelLeave_DocumentsViewModel> listDocument = js.Deserialize<List<EmployeePlanner_TravelLeave_DocumentsViewModel>>(model.jsonDocumentList);
            model.DocumentList = listDocument;

            _employeePlannerMethod.TravelLeave_SaveData(model, SessionProxy.UserId);

            List<EmployeePlannerDayViewModel> returnModel = returnDayListTable(model.monthId, model.yearId, model.EmployeeId, model.HolidayCountryID);
            return PartialView("_partialListOfDayMonthYearWise", returnModel);
        }
        #endregion

        #region TimeSheet

        public ActionResult AddEdit_TimeSheet(int Id, int Date, int Month, int Year)
        {
            EmployeePlanner_TimeSheetViewModel model = new EmployeePlanner_TimeSheetViewModel();
            DateTime date = new DateTime(Year, Month, Date);
            model.Id = Id;
            model.yearId = Year;
            model.monthId = Month;
            model.day = Date;

            if (Id > 0)
            {
                var data = _employeePlannerMethod.getTimeSheetById(Id);
                model.Date = String.Format("{0:dd-MM-yyy}", data.Date);
                model.Comment = data.Comments;
                var timeSheetDetail = _employeePlannerMethod.getAllTimeSheetDetail(Id);
                foreach (var timeSheet in timeSheetDetail)
                {

                    EmployeePlanner_TimeSheet_DetailViewModel detailModel = new EmployeePlanner_TimeSheet_DetailViewModel();
                    if (!string.IsNullOrEmpty(timeSheet.Customer) && timeSheet.Customer != null)
                    {
                        int CmpId = Convert.ToInt32(timeSheet.Customer);
                        var Employee = _db.AspNetUsers.Where(x => x.Id == CmpId).FirstOrDefault();
                        if (Employee != null)
                        {
                            detailModel.Customer = Employee.FirstName + " " + Employee.LastName + " " + Employee.SSOID;
                        }
                        detailModel.CustomerId = timeSheet.Customer;
                    }
                    detailModel.CostCodeList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
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
                    detailModel.Id = timeSheet.Id;
                    detailModel.InTimeHr = timeSheet.InTimeHr;
                    detailModel.InTimeMin = timeSheet.InTimeMin;
                    detailModel.EndTimeHr = timeSheet.EndTimeHr;
                    detailModel.EndTimeMin = timeSheet.EndTimeMin;
                    detailModel.Project = timeSheet.Project;
                    detailModel.CostCode = timeSheet.CostCode;
                    detailModel.Asset = timeSheet.Asset;
                    model.DetailList.Add(detailModel);
                    for (int i = 0; i < 60; i++)
                    {
                        detailModel.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                    }
                    for (int i = 0; i < 24; i++)
                    {
                        detailModel.HoursList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                    }
                }

                var timeSheetDoument = _employeePlannerMethod.getAllTimeSheetDocument(Id);
                foreach (var item in timeSheetDoument)
                {
                    EmployeePlanner_TimeSheet_DocumentsViewModel docModel = new EmployeePlanner_TimeSheet_DocumentsViewModel();
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
            EmployeePlanner_TimeSheet_DetailViewModel model = new EmployeePlanner_TimeSheet_DetailViewModel();

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

        public ActionResult SaveData_TimeSheet(EmployeePlanner_TimeSheetViewModel model)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<EmployeePlanner_TimeSheet_DocumentsViewModel> listDocument = js.Deserialize<List<EmployeePlanner_TimeSheet_DocumentsViewModel>>(model.jsonDocumentList);
            model.DocumentList = listDocument;
            List<EmployeePlanner_TimeSheet_DetailViewModel> listDetail = js.Deserialize<List<EmployeePlanner_TimeSheet_DetailViewModel>>(model.jsonDetailList);
            model.DetailList = listDetail;


            _employeePlannerMethod.TimeSheet_SaveData(model, SessionProxy.UserId);

            List<EmployeePlannerDayViewModel> returnModel = returnDayListTable(model.monthId, model.yearId, model.EmployeeId, model.HolidayCountryID);
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
                FilePath = ConfigurationManager.AppSettings["Planner_TimeSheet"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
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

        public ActionResult SavePublicHolidayTemplate(int CountryId, string EffactiveDate, int EmployeeId, int HolidayCountryID)
        {
            var EffactiveDateToString = DateTime.ParseExact(EffactiveDate, inputFormat, CultureInfo.InvariantCulture);
            DateTime effDate = Convert.ToDateTime(EffactiveDateToString.ToString(outputFormat));
            //_employeePlannerMethod.saveEmployeeJobTitleCountry(EmployeeId, CountryId);
            Employee_PublicHoliday employeeList = _employeePlannerMethod.publicHolidayList().Where(x => x.EmployeeId == EmployeeId).OrderBy(x => x.Id).LastOrDefault();
            if (employeeList != null)
            {
                if (employeeList.EffecitveFrom.Value.Date < effDate.Date)
                {
                    _employeePlannerMethod.SaveData_EffactivePublicHoliday(CountryId, EffactiveDate, EmployeeId, SessionProxy.UserId);

                    var currentYear = DateTime.Now.Year;
                    var currentMonth = DateTime.Now.Month;
                    List<EmployeePlannerDayViewModel> returnModel = returnDayListTable(currentMonth, currentYear, EmployeeId, HolidayCountryID);
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
                List<EmployeePlannerDayViewModel> returnModel = returnDayListTable(currentMonth, currentYear, EmployeeId, HolidayCountryID);
                return PartialView("_partialListOfDayMonthYearWise", returnModel);
            }

        }

        public ActionResult GetHolidayDetail(int Id, int Date, int Month, int Year)
        {
            var holiDay = _holidayNAbsenceMethod.GetAllHolidayListById(Id);
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

        #region Sick Leave

        [HttpPost]
        public ActionResult SickLeaveImageData()
        {
            string FilePath = string.Empty;
            string fileName = string.Empty;
            string originalFileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                FilePath = ConfigurationManager.AppSettings["Planner_SickLeave"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
        }

        public ActionResult AddEdit_SickLeave(int Id, int Date, int Month, int Year)
        {
            EmployeePlanner_SickLeaves model = new EmployeePlanner_SickLeaves();
            DateTime date = new DateTime(Year, Month, Date);
            model.Id = Id;
            model.yearId = Year;
            model.monthId = Month;
            model.day = Date;
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Sick Leave Reason List"))
            {
                model.ReasonSickLeaveList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            //for (int i = 0; i < 60; i++)
            //{
            //    model.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            //}
            //for (int i = 0; i < 24; i++)
            //{
            //    model.HoursList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            //}
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Time List"))
            {
                model.TimeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }

            if (Id > 0)
            {
                var data = _employeePlannerMethod.getSickLeaveById(Id);
                model.StartDate = String.Format("{0:dd-MM-yyy}", data.StartDate);
                model.IsADayOrMore = data.IsADayOrMore;
                model.IsHalfDay = data.IsHalfDay;
                model.IsHours = data.IsHours;

                if (data.IsADayOrMore == true)
                {
                    model.EndDate = String.Format("{0:dd-MM-yyy}", data.EndDate);
                    model.DurationDays = (decimal)data.DurationDays;
                }
                else
                {
                    //model.InTimeHrSD = data.InTimeHrSD;
                    //model.InTimeMinSD = data.InTimeMinSD;
                    //model.InTimeHrED = data.InTimeHrED;
                    //model.InTimeMinED = data.InTimeMinED;
                    model.PartOfDay = data.PartOfDay;
                    model.DurationHours = (decimal)data.DurationHours;
                }
                model.EmergencyLeave = data.EmergencyLeave;
                model.ConfirmedbyHR = data.ConfirmedbyHR;
                model.SelfCertificationFormRequired = data.SelfCertificationFormRequired;
                if (data.SelfCertificationFormRequired == true)
                {
                    model.SelfCertificateReceivedDate = String.Format("{0:dd-MM-yyy}", data.SelfCertificateReceivedDate);
                }
                model.BackToWorkInterviewRequired = data.BackToWorkInterviewRequired;
                if (data.BackToWorkInterviewRequired == true)
                {
                    model.InterviewDate = String.Format("{0:dd-MM-yyy}", data.InterviewDate);
                    model.InterviewConductedBy = data.InterviewConductedBy;
                }
                model.IsPaid = data.IsPaid;
                model.IsPaidatotherrate = data.IsPaidatotherrate;
                model.IsUnpaid = data.IsUnpaid;
                model.DoctorConsulted = data.DoctorConsulted;
                if (data.DoctorConsulted == true)
                {
                    model.DoctorName = data.DoctorName;
                    model.DoctorAdvice = data.DoctorAdvice;
                    model.MedicationPrescribed = data.MedicationPrescribed;
                    //model.TimeOfVisitStartHr = data.TimeOfVisitStartHr;
                    //model.TimeOfVisitStartMin = data.TimeOfVisitStartMin;
                    //model.TimeOfVisitEndHr = data.TimeOfVisitEndHr;
                    //model.TimeOfVisitEndMin = data.TimeOfVisitEndMin;
                    model.TimeOfVisit = data.TimeOfVisit;
                    model.DateOfVisit = String.Format("{0:dd-MM-yyy}", data.DateOfVisit);
                    model.MedicalCertificateIssuedDate = String.Format("{0:dd-MM-yyy}", data.MedicalCertificateIssuedDate);

                }
                else
                {
                    model.WhyDoctorNotConsulted = data.WhyDoctorNotConsulted;
                }
                var CommentDetails = _employeePlannerMethod.getAllSickLeaveComment(data.Id);
                foreach (var item in CommentDetails)
                {
                    SickLeavesCommentViewModel commentModel = new SickLeavesCommentViewModel();
                    commentModel.Id = item.Id;
                    commentModel.comment = item.Description;
                    commentModel.commentBy = item.CreatedName;
                    commentModel.commentTime = item.CreatedDateTime;
                    model.CommentList.Add(commentModel);
                }

                var DocumentDetails = _employeePlannerMethod.getAllSickLeaveDocument(data.Id);
                foreach (var item in DocumentDetails)
                {
                    SickLeavesDocumentViewModel docModel = new SickLeavesDocumentViewModel();
                    docModel.Id = item.Id;
                    docModel.originalName = item.OriginalName;
                    docModel.newName = item.NewName;
                    docModel.description = item.Description;
                    model.DocumentList.Add(docModel);
                }
            }
            else
            {
                model.IsPaid = true;
                model.IsADayOrMore = true;
                model.StartDate = String.Format("{0:dd-MM-yyy}", date);
                model.EndDate = String.Format("{0:dd-MM-yyy}", date);
                model.DurationDays = 1;
                model.DateOfVisit = String.Format("{0:dd-MM-yyy}", date);
                model.InterviewDate = String.Format("{0:dd-MM-yyy}", date);
                model.MedicalCertificateIssuedDate = String.Format("{0:dd-MM-yyy}", date);
                model.SelfCertificateReceivedDate = String.Format("{0:dd-MM-yyy}", date);
                model.PartOfDay = "";
                model.TimeOfVisit = "";
            }
            return PartialView("_PartialAdd_SickLeavesView", model);
        }

        public ActionResult SaveData_SickLeave(EmployeePlanner_SickLeaves model)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<SickLeavesCommentViewModel> listComment = js.Deserialize<List<SickLeavesCommentViewModel>>(model.CommentListString);
            List<SickLeavesDocumentViewModel> listDocument = js.Deserialize<List<SickLeavesDocumentViewModel>>(model.DocumentListString);

            _employeePlannerMethod.SickLeave_SaveData(model, listComment, listDocument, SessionProxy.UserId);
            List<EmployeePlannerDayViewModel> returnModel = returnDayListTable(model.monthId, model.yearId, model.EmployeeId, model.HolidayCountryID);
            return PartialView("_partialListOfDayMonthYearWise", returnModel);
        }

        #endregion

        #region Maternity Paternity Leave

        [HttpPost]
        public ActionResult MPLeaveImageData()
        {
            string FilePath = string.Empty;
            string fileName = string.Empty;
            string originalFileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                FilePath = ConfigurationManager.AppSettings["Planner_MPLeave"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
        }

        public ActionResult AddEdit_MPLeaves(int Id, int Date, int Month, int Year, int EmployeeID)
        {
            EmployeePlanner_MaternityPaternityViewModel model = new EmployeePlanner_MaternityPaternityViewModel();
            var details = _employeeMethod.getEmployeeById(EmployeeID);
            DateTime date = new DateTime(Year, Month, Date);
            model.Id = Id;
            model.yearId = Year;
            model.monthId = Month;
            model.day = Date;


            if (Id > 0)
            {
                var data = _employeePlannerMethod.getMaternityPaternityById(model.Id);

                model.ActualEndDate = String.Format("{0:dd-MM-yyyy}", data.ActualEndDate);
                model.ActualStartDate = String.Format("{0:dd-MM-yyyy}", data.ActualStartDate);
                model.AdditionalMaternityLeaveEndDate = String.Format("{0:dd-MM-yyyy}", data.AdditionalMaternityLeaveEndDate);
                model.AdditionalMaternityLeaveStartDate = String.Format("{0:dd-MM-yyyy}", data.AdditionalMaternityLeaveStartDate);
                model.DueDate = String.Format("{0:dd-MM-yyyy}", data.DueDate);
                model.EarliestBirthWeekStartDate = String.Format("{0:dd-MM-yyyy}", data.EarliestBirthWeekStartDate);
                model.ExptectedBirthWeekEndDate = String.Format("{0:dd-MM-yyyy}", data.ExptectedBirthWeekEndDate);
                model.ExptectedBirthWeekStartDate = String.Format("{0:dd-MM-yyyy}", data.ExptectedBirthWeekStartDate);
                model.OrdinaryMaternityLeaveEndDate = String.Format("{0:dd-MM-yyyy}", data.OrdinaryMaternityLeaveEndDate);
                model.OrdinaryMaternityLeaveStartDate = String.Format("{0:dd-MM-yyyy}", data.OrdinaryMaternityLeaveStartDate);

                var CommentDetails = _employeePlannerMethod.getAllMaternityPaternityLeaveComment(data.Id);
                foreach (var item in CommentDetails)
                {
                    MaternityPaternityCommentViewModel commentModel = new MaternityPaternityCommentViewModel();
                    commentModel.Id = item.Id;
                    commentModel.comment = item.Description;
                    commentModel.commentBy = item.CreatedName;
                    commentModel.commentTime = item.CreatedDateTime;
                    model.CommentList.Add(commentModel);
                }

                var DocumentDetails = _employeePlannerMethod.getAllMaternityPaternityLeaveDocument(data.Id);
                foreach (var item in DocumentDetails)
                {
                    MaternityPaternityDocumentViewModel docModel = new MaternityPaternityDocumentViewModel();
                    docModel.Id = item.Id;
                    docModel.originalName = item.OriginalName;
                    docModel.newName = item.NewName;
                    docModel.description = item.Description;
                    model.DocumentList.Add(docModel);
                }
            }
            else
            {

                var today = date;
                var yesterday = date.AddDays(-1);
                var thisWeekStart = date.AddDays(-(int)date.DayOfWeek);
                var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                var earliestDays = thisWeekStart.AddDays(-(11 * 7));
                var ordinaryenddate = thisWeekStart.AddDays(26 * 7);
                var additionStartdays = ordinaryenddate.AddDays(1);
                var additionEnddate = additionStartdays.AddDays(26 * 7);

                model.ActualEndDate = String.Format("{0:dd-MM-yyyy}", date);
                model.ActualStartDate = String.Format("{0:dd-MM-yyyy}", date);

                model.AdditionalMaternityLeaveEndDate = String.Format("{0:dd-MM-yyyy}", additionEnddate);
                model.AdditionalMaternityLeaveStartDate = String.Format("{0:dd-MM-yyyy}", additionStartdays);

                model.DueDate = String.Format("{0:dd-MM-yyyy}", date);

                model.EarliestBirthWeekStartDate = String.Format("{0:dd-MM-yyyy}", earliestDays);
                model.ExptectedBirthWeekEndDate = String.Format("{0:dd-MM-yyyy}", thisWeekEnd);
                model.ExptectedBirthWeekStartDate = String.Format("{0:dd-MM-yyyy}", thisWeekStart);
                model.OrdinaryMaternityLeaveEndDate = String.Format("{0:dd-MM-yyyy}", ordinaryenddate);
                model.OrdinaryMaternityLeaveStartDate = String.Format("{0:dd-MM-yyyy}", thisWeekStart);

            }

            #region Length of employment
            var diff = DateTimeSpan.CompareDates((DateTime)details.StartDate, DateTime.Now);

            if (diff.Years != 0)
            {
                model.Lengthofemployment = diff.Years + " " + "a year ago";
            }
            else
            {
                if (diff.Months != 0)
                {
                    model.Lengthofemployment = diff.Months + " " + "Months ago";
                }
                else
                {
                    if (diff.Days != 0)
                    {
                        model.Lengthofemployment = diff.Days + " " + "Days ago";
                    }
                    else
                    {
                        if (diff.Hours != 0)
                        {
                            model.Lengthofemployment = diff.Hours + " " + "Hours ago";
                        }
                        else
                        {
                            model.Lengthofemployment = diff.Minutes + " " + "Minutes ago";
                        }
                    }
                }

            }

            #endregion

            return PartialView("_partialAddMaternityPaternityLeavesView", model);
        }

        public ActionResult SaveData_MaternityPaternityLeave(EmployeePlanner_MaternityPaternityViewModel model)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<MaternityPaternityCommentViewModel> listComment = js.Deserialize<List<MaternityPaternityCommentViewModel>>(model.CommentListString);
            List<MaternityPaternityDocumentViewModel> listDocument = js.Deserialize<List<MaternityPaternityDocumentViewModel>>(model.DocumentListString);

            _employeePlannerMethod.MaternityPaternityLeave_SaveData(model, listComment, listDocument, SessionProxy.UserId);
            List<EmployeePlannerDayViewModel> returnModel = returnDayListTable(model.monthId, model.yearId, model.EmployeeID, model.HolidayCountryID);
            return PartialView("_partialListOfDayMonthYearWise", returnModel);
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

    }
}