using HRTool.CommanMethods;
using HRTool.CommanMethods.Resources;
using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class EmployeeEmploymentController : Controller
    {
        #region Constant
        EvolutionEntities _db = new EvolutionEntities();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        EmployeeEmploymentMethod _employeeEmploymentMethod = new EmployeeEmploymentMethod();
        EmployeeMethod _employeeMethod = new EmployeeMethod();
        #endregion
        public ActionResult Index(int EmployeeId)
        {
            EmployeeEmploymentViewModel model = new EmployeeEmploymentViewModel();
            model.EmployeeId = EmployeeId;
            model.NoticePeriodList.Add(new SelectListItem() { Text = "-- Select Notice Period --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Notice Period List"))
            {
                model.NoticePeriodList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            var employeeData = _employeeMethod.getEmployeeById(EmployeeId);
            model.ProbationEndDate = String.Format("{0:dd-MM-yyy}", employeeData.ProbationEndDate);
            model.NextProbationReviewDate = String.Format("{0:dd-MM-yyy}", employeeData.NextProbationReviewDate);
            if (employeeData.NoticePeriod != null)
                model.NoticePeriod = (int)employeeData.NoticePeriod;
            model.FixedTermEndDate = String.Format("{0:dd-MM-yyy}", employeeData.FixedTermEndDate);
            model.MethodofRecruitmentSetup = employeeData.MethodofRecruitmentSetup;
            if (employeeData.RecruitmentCost != null)
                model.RecruitmentCost = employeeData.RecruitmentCost;
            if (employeeData.Thisyear != null)
                model.ThisYearHolidays = (int)employeeData.Thisyear;
            if (employeeData.Nextyear != null)
                model.NextYearHolidays = (int)employeeData.Nextyear;
            if(employeeData.HolidayEntitlement!=null)
            {
                double totalHoliday=HolidayIncludeContractDays(model.EmployeeId,Convert.ToInt32(employeeData.HolidayEntitlement));
                var data = _db.Employee_Salary.Where(x => x.Archived == false && x.EmployeeID==model.EmployeeId).FirstOrDefault();
                if (data != null)
                {
                    model.rate = Convert.ToDouble(data.TotalSalary) / totalHoliday;
                }
            }
            if(employeeData.ActivityType!=null && employeeData.ActivityType!=0)
            {
                var activityInfo = _db.ActivityTypes.Where(x => x.Id == employeeData.ActivityType && x.Archived == false).FirstOrDefault();
                model.ActivityTypeId = activityInfo.Id;
                model.ActivityTypeName = activityInfo.Name;
                model.WorkerRate = activityInfo.WorkerRate;
                var currunciesInfo = _db.SystemListValues.Where(x => x.Id == activityInfo.CurrencyID && x.Archived == false).FirstOrDefault();
                model.CurruncyName = currunciesInfo.Value;
            }
            var ActivityData = _db.ActivityTypes.Where(x => x.Archived == false).ToList();
            foreach (var item in ActivityData)
            {
                model.ActivityTypeList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            //  model.includeThisYear = HolidayIncludeContractDays(EmployeeId);
            //  model.notincludeThisYear = HolidayNotIncludeContractDays(EmployeeId);
            return View(model);
        }
        //public ActionResult CalculateHoliday(HelpmecalculateviewModel hmodel)
        //{
        //    EmployeeEmploymentViewModel model = new EmployeeEmploymentViewModel();
        //    int enti = Convert.ToInt32(hmodel.FullTimeEntitlement);
        //    if (hmodel.IncludePublicHolidays == "on")
        //    {
        //        model.includeThisYear = HolidayIncludeContractDays(hmodel.EmployeeID, enti);
        //    }
        //    else if(hmodel.IncludePublicHolidays=="off")
        //    {
        //        model.notincludeThisYear = HolidayNotIncludeContractDays(hmodel.EmployeeID,enti);
        //    }

        //    return Json(model,JsonRequestBehavior.AllowGet);

        //}

        public ActionResult getAllActivityType()
        {
            var ActivityData = _db.ActivityTypes.Where(x => x.Archived == false).ToList();
            EmployeeEmploymentViewModel model = new EmployeeEmploymentViewModel();
            foreach (var item in ActivityData)
            {
                model.ActivityTypeList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            return Json(model.ActivityTypeList, JsonRequestBehavior.AllowGet);
        }
        public int counttotalWeekends(int styear, int endyear,int startMonth,int endMonth)
        {
            int count = 0;
            for (int j = startMonth; j <= endMonth; j++)
            {
                DateTime EndOfMonth = new DateTime(styear, j, DateTime.DaysInMonth(styear, j));
                int day = EndOfMonth.Day;
                for (int i = 0; i <= day; i++)
                {
                    if (i >= 1)
                    {
                        DateTime d = new DateTime(styear, j, i);
                        if (d.DayOfWeek == DayOfWeek.Sunday || d.DayOfWeek == DayOfWeek.Saturday)
                        {
                            count++;
                        }
                    }
                }
            }           
            return count;
        }
        public Double HolidayIncludeContractDays(int ID,int holidayEn)
        {
            Double publicHoliday = 0;
            Double holidayEnti = 0;
            Double contractDays = 0;
            Double IncludedHoliday = 0;
            double totalHoliday = 0;
            int year = _employeeEmploymentMethod.getEmployeeActiveYear();
            int endYear = _employeeEmploymentMethod.getEmployeeActiveEndYear();
            int startMonth = _employeeEmploymentMethod.getEmployeeActiveStartMonth();
            int endMonth = _employeeEmploymentMethod.getEmployeeActiveEndMonth();
            int dayInmonth = System.DateTime.DaysInMonth(endYear, endMonth);
            DateTime startdate = new DateTime(year, startMonth, 1);
            DateTime EndDate = new DateTime(endYear, endMonth, 31);
            double diff = (EndDate - startdate).TotalDays;
            int totalWeekend = counttotalWeekends(year, endYear, startMonth, endMonth);
            var data = _employeeEmploymentMethod.getHolidayEntiAndPublicHoliday();
            var publicHolidayCount = _employeeEmploymentMethod.getPublicHolidayByCountry();
            //  var publicHolidayCount = _employeeEmploymentMethod.getPublicHoliday(ID);
            foreach (var item in publicHolidayCount)
            {
                int Year = DateTime.Now.Year;
                DateTime holidayYear = Convert.ToDateTime(item.PublicHolidayDate);
                if(year==holidayYear.Year)
                {
                    publicHoliday++;
                }
            }
            foreach (var item in data)
            {
                //  holidayEnti = Convert.ToDouble(item.HolidayEntitlement);
                holidayEnti = holidayEn;
                IncludedHoliday =   holidayEnti- publicHoliday;
                contractDays = diff - publicHoliday - Convert.ToDouble(totalWeekend) - IncludedHoliday;
            }
            DateTime startDate = _employeeEmploymentMethod.getStartDateOfEmp(ID);
            int totalWeekendfromStart = counttotalWeekends(year, startDate.Year, startMonth, startDate.Month);
            double todayContractDay = (startDate - startdate).Days-Convert.ToDouble(totalWeekendfromStart);
            double remainingWorkDay = contractDays - todayContractDay;
            double holidayRateperDay =  IncludedHoliday/ contractDays;
            double remainingHoliday =holidayRateperDay * remainingWorkDay;
            double publicholiday=countHolidayfromStartDate( ID, EndDate);
            totalHoliday = Math.Round(remainingHoliday) + publicholiday;
            return totalHoliday;
        }
        public double countHolidayfromStartDate(int ID, DateTime endDate)
        {
            DateTime startDate = _employeeEmploymentMethod.getStartDateOfEmp(ID);
            var publicHolidayCount = _employeeEmploymentMethod.getPublicHolidayByCountry();
            int count = 0;          
            while (startDate.AddDays(1) <= endDate)
            {
                foreach (var data in publicHolidayCount)
                {
                    if (startDate == data.PublicHolidayDate.Value)
                    {
                        count++;
                    }                    
                }
                startDate = startDate.AddDays(1);
            }

            return count;
        }
        public Double HolidayNotIncludeContractDays(int ID, int holidayEn)
        {
            var data = _employeeEmploymentMethod.getHolidayEntiAndPublicHoliday();
            Double publicHoliday = 0;
            Double holidayEnti = 0;
            Double contractDays = 0;
            double totalHoliday = 0;
            int year = _employeeEmploymentMethod.getEmployeeActiveYear();
            int endYear = _employeeEmploymentMethod.getEmployeeActiveEndYear();
            int startMonth = _employeeEmploymentMethod.getEmployeeActiveStartMonth();
            int endMonth = _employeeEmploymentMethod.getEmployeeActiveEndMonth();
            int dayInmonth = System.DateTime.DaysInMonth(endYear, endMonth);
            DateTime startdate = new DateTime(year, startMonth, 1);
            DateTime EndDate = new DateTime(endYear, endMonth, 31);
            double diff = (EndDate - startdate).TotalDays;
            int totalWeekend = counttotalWeekends(year, endYear, startMonth, endMonth);
            var publicHolidayCount = _employeeEmploymentMethod.getPublicHolidayByCountry();
            //  var publicHolidayCount = _employeeEmploymentMethod.getPublicHoliday(ID);
            foreach (var item in publicHolidayCount)
            {
                int Year = DateTime.Now.Year;
                DateTime holidayYear = Convert.ToDateTime(item.PublicHolidayDate);
                if (year == holidayYear.Year)
                {
                    publicHoliday++;
                }
            }
            foreach (var item in data)
            {               
                holidayEnti =holidayEn;
                contractDays = diff - publicHoliday - Convert.ToDouble(totalWeekend) - holidayEnti;
            }
            DateTime startDate = _employeeEmploymentMethod.getStartDateOfEmp(ID);
            int totalWeekendfromStart = counttotalWeekends(year, startDate.Year, startMonth, startDate.Month);
            double todayContractDay = (startDate - startdate).Days - Convert.ToDouble(totalWeekendfromStart); ;
            double remainingWorkDay = contractDays - todayContractDay;
            double holidayRateperDay = holidayEnti / contractDays;
            double remainingHoliday = holidayRateperDay * remainingWorkDay;
            double publicholiday = countHolidayfromStartDate(ID, EndDate);
            totalHoliday = Math.Round(remainingHoliday) + publicholiday;
            return totalHoliday;
        }
        
        [HttpPost]
        public ActionResult UpdateEmployment(EmployeeEmploymentViewModel model)
        {
            _employeeEmploymentMethod.UpdateEmploymentDetail(model);
            return Json("success", JsonRequestBehavior.AllowGet);
        }
        public ActionResult getRateAndCurrencies(int activityId)
        {
            EmployeeEmploymentViewModel emodel = new EmployeeEmploymentViewModel();
            if (activityId != 0 && activityId != null)
            {
                var activityInfo = _db.ActivityTypes.Where(x => x.Id == activityId && x.Archived == false).FirstOrDefault();
                emodel.WorkerRate = activityInfo.WorkerRate;
                var currunciesInfo = _db.SystemListValues.Where(x => x.Id == activityInfo.CurrencyID && x.Archived == false).FirstOrDefault();
                emodel.CurruncyName = currunciesInfo.Value;
            }
            return Json(emodel,JsonRequestBehavior.AllowGet);
        }
        public ActionResult HelpMeCalculate(HelpmecalculateviewModel model)
        {
            EmployeeEmploymentViewModel emodel = new EmployeeEmploymentViewModel();
            var userinfo = _db.AspNetUsers.Where(x => x.Id == model.EmployeeID).FirstOrDefault();
            model.StartDate = String.Format("{0:dd-MM-yyyy}",userinfo.StartDate);
            if (userinfo.JobContryID ==null)
            {
                userinfo.JobContryID = 1;
            }
            model.CountryId = (int)userinfo.JobContryID;
            List<SelectListItem> data = new List<SelectListItem>();
            HelpmeCalculeteModel Details = new HelpmeCalculeteModel();
            int totalDays = 0;
            int enti = Convert.ToInt32(model.FullTimeEntitlement);
            Details = _employeeMethod.GetPublicHolidayByContryId(model.StartDate, model.CountryId);
            if (model.IncludePublicHolidays == "on")
            {
                emodel.includeThisYear = HolidayIncludeContractDays(model.EmployeeID, enti);
                data.Add(new SelectListItem { Text = emodel.includeThisYear.ToString(), Value = "holidaysThisYear" });
            }
            else
            {
                emodel.notincludeThisYear = HolidayNotIncludeContractDays(model.EmployeeID, enti);
                data.Add(new SelectListItem { Text = emodel.notincludeThisYear.ToString(), Value = "holidaysThisYear" });

            }
            if (enti != null && enti!=0)
            {
                double totalHoliday = HolidayIncludeContractDays(model.EmployeeID, enti);
                var edata = _db.Employee_Salary.Where(x => x.Archived == false && x.EmployeeID == model.EmployeeID).FirstOrDefault();
                if (edata != null)
                {
                    double rate = Convert.ToDouble(edata.TotalSalary) / totalHoliday;
                    data.Add(new SelectListItem { Text = emodel.rate.ToString(), Value = "recovryRate" });
                }
            }
            data.Add(new SelectListItem { Text = enti.ToString(), Value = "holidaysNextYear" });
            
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}