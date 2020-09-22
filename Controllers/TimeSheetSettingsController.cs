using HRTool.CommanMethods;
using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class TimeSheetSettingsController : Controller
    {

        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        TimeSheetSettingsMethod _timeSheetSettingsMethod = new TimeSheetSettingsMethod();
        ProjectSettindsMethod _projectSettindsMethod = new ProjectSettindsMethod();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();

        #endregion
        // GET: TimeSheetSettings
        public ActionResult Index()
        {
            TimeSheetViewModel model = new TimeSheetViewModel();
            var projectList = _projectSettindsMethod.getAllList().Where(x=>x.Archived==false);
            var frequencyList = _otherSettingMethod.getAllSystemValueListByKeyName("Time Sheet Tasks").Where(x=>x.Archived==false);
            var detailList = _otherSettingMethod.getAllSystemValueListByKeyName("Time Sheet Details").Where(x=>x.Archived==false);

            model.ProjectList.Add(new SelectListItem() { Text = "-- Select Project --", Value = "0" });
            foreach (var item in projectList)
            {
                model.ProjectList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }

            model.FrequencyList.Add(new SelectListItem() { Text = "-- Select Frequency --", Value = "0" });
            foreach (var item in frequencyList)
            {
                model.FrequencyList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }

            model.DetailList.Add(new SelectListItem() { Text = "-- Select Detail --", Value = "0" });
            foreach (var item in detailList)
            {
                model.DetailList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }

            return View(model);
        }
        public ActionResult SaveData(int Project, int Frequency, int Detail)
        {
            _timeSheetSettingsMethod.saveData(Project, Frequency, Detail);
            return RedirectToAction("Index");
        }
    }
}