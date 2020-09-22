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
    public class MeEmployeeEmploymentController : Controller
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
            return View(model);
        }

        [HttpPost]
     
        public ActionResult UpdateEmployment(EmployeeEmploymentViewModel model)
        {
            _employeeEmploymentMethod.UpdateEmploymentDetail(model);
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult HelpMeCalculate(HelpmecalculateviewModel model)
        {
            var userinfo = _db.AspNetUsers.Where(x => x.Id == model.EmployeeID).FirstOrDefault();
            model.StartDate = String.Format("",userinfo.StartDate);
            if (userinfo.JobContryID ==null)
            {
                userinfo.JobContryID = 1;
            }
            model.CountryId = (int)userinfo.JobContryID;
            List<SelectListItem> data = new List<SelectListItem>();
            HelpmeCalculeteModel Details = new HelpmeCalculeteModel();
            int totalDays = 0;
            Details = _employeeMethod.GetPublicHolidayByContryId(model.StartDate, model.CountryId);
            if (model.IncludePublicHolidays == "on")
            {
                decimal contractdays = Details.totalWorkingDays - (Details.TotalHolidayYear + (Convert.ToInt16(model.FullTimeEntitlement) - Details.TotalHolidayYear));
                decimal Accrualholidayrateperday = Math.Round((Convert.ToDecimal(model.FullTimeEntitlement) - Details.TotalHolidayYear) / contractdays, 2);
                int Remainingholidays = Convert.ToInt16(Accrualholidayrateperday * Details.remainiingDays);
                decimal RemainingHolidyasFromStatDate = Details.TotalRemainingHolidays;
                totalDays = Convert.ToInt16(Remainingholidays + RemainingHolidyasFromStatDate);
            }
            else
            {
                decimal contractdays = Details.totalWorkingDays - (Details.TotalHolidayYear + Convert.ToInt16(model.FullTimeEntitlement));
                decimal Accrualholidayrateperday = Math.Round((Convert.ToDecimal(model.FullTimeEntitlement) - Details.TotalHolidayYear) / contractdays, 2);
                decimal Remainingholidays = Accrualholidayrateperday * Details.remainiingDays;
                decimal RemainingHolidyasFromStatDate = Details.TotalRemainingHolidays;
                totalDays = Convert.ToInt16(Remainingholidays + RemainingHolidyasFromStatDate);
                if (totalDays > 16)
                {
                    totalDays = totalDays + Convert.ToInt16(RemainingHolidyasFromStatDate);
                }

            }
            data.Add(new SelectListItem { Text = totalDays.ToString(), Value = "holidaysThisYear" });
            data.Add(new SelectListItem { Text = model.FullTimeEntitlement.ToString(), Value = "holidaysNextYear" });

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}