using HRTool.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HRTool.CommanMethods.Settings;
using HRTool.Models.Settings;
using HRTool.CommanMethods;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class ActivityTypeController : Controller
    {
        #region Constant
        EvolutionEntities _db = new EvolutionEntities();
        ActivityTypeMethod _ActivityTypeMethod = new ActivityTypeMethod();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();


        #endregion

        #region View
        // GET: /ActivityType/
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region ActivityType Method

        public ActivityTypeViewModel returnActivityTypeList()
        {
            ActivityTypeViewModel model = new ActivityTypeViewModel();
            //string FilePath = ConfigurationManager.AppSettings["CmpCustomerFilePath"].ToString();
            var listData = _ActivityTypeMethod.getAllActivityTypeList();
            var titleId = _otherSettingMethod.getSystemListByName("Work Unit List");
            var genderId = _otherSettingMethod.getSystemListByName("Company Setting Currencies");
            var titleLists = _otherSettingMethod.getAllSystemValueListByNameId(titleId.Id);
            var genderLists = _otherSettingMethod.getAllSystemValueListByNameId(genderId.Id);
            foreach (var item in listData)
            {
                var currencyList = _otherSettingMethod.getSystemListValueById((int)item.CurrencyID);
                var Worklist = _otherSettingMethod.getSystemListValueById((int)item.WorkUnitID);
                ActivityTypeListViewModel tableModel = new ActivityTypeListViewModel();
                tableModel.Id = item.Id;
                tableModel.Name = item.Name;
                if(currencyList != null)
                tableModel.Currency = currencyList.Value;
                if(Worklist != null)
                tableModel.WorkUnit = Worklist.Value;
                tableModel.Rate = (decimal)item.WorkerRate;
                model.activityTypeList.Add(tableModel);

            }
            foreach (var item in titleLists)
            {
                OtherSettingValueViewModel tableModel = new OtherSettingValueViewModel();
                tableModel.Id = item.Id;
                tableModel.SystemListID = item.SystemListID;
                tableModel.Value = item.Value;
                model.workUnitList.Add(tableModel);
            }
            foreach (var item in genderLists)
            {
                OtherSettingValueViewModel tableModel = new OtherSettingValueViewModel();
                tableModel.Id = item.Id;
                tableModel.SystemListID = item.SystemListID;
                tableModel.Value = item.Value;
                model.currencyList.Add(tableModel);
            }
            return model;

        }
        public ActionResult activityTypeList()
        {
            ActivityTypeViewModel model = returnActivityTypeList();
            return PartialView("_partialActivityTypeList", model);
        }

        public ActionResult AddEditActivityType(int Id)
        {
            ActivityTypeViewModel model = new ActivityTypeViewModel();
            var listData = _ActivityTypeMethod.getAllActivityTypeList();
            var titleId = _otherSettingMethod.getSystemListByName("Work Unit List");
            var genderId = _otherSettingMethod.getSystemListByName("Company Setting Currencies");
            var titleLists = _otherSettingMethod.getAllSystemValueListByNameId(titleId.Id);
            var genderLists = _otherSettingMethod.getAllSystemValueListByNameId(genderId.Id);


            if (Id > 0)
            {
                var data = _ActivityTypeMethod.getActivityTypeListById(Id);
                var currencyList = _otherSettingMethod.getSystemListValueById((int)data.CurrencyID);
                var Worklist = _otherSettingMethod.getSystemListValueById((int)data.WorkUnitID);
                model.Id = data.Id;
                model.Year = (int)data.Year;
                model.Name = data.Name;
                model.CurrencyID = (int)data.CurrencyID;
                model.WorkUnitID = (int)data.WorkUnitID;
                model.WorkerRate = (decimal)data.WorkerRate;
                model.CustomerRate = (decimal)data.CustomerRate;
                
            }
            else
            {
                foreach (var item in titleLists)
                {
                    OtherSettingValueViewModel tableModel = new OtherSettingValueViewModel();
                    tableModel.Id = item.Id;
                    tableModel.SystemListID = item.SystemListID;
                    tableModel.Value = item.Value;
                    model.workUnitList.Add(tableModel);
                }
                foreach (var item in genderLists)
                {
                    OtherSettingValueViewModel tableModel = new OtherSettingValueViewModel();
                    tableModel.Id = item.Id;
                    tableModel.SystemListID = item.SystemListID;
                    tableModel.Value = item.Value;
                    model.currencyList.Add(tableModel);
                }

            }
            return PartialView("_partailAddActivityType", model);

        }
        public ActionResult SaveActivityType(int Id, int year, string name, int curriencyId, int workunitId, decimal? workerRate, decimal? customerRate)
        {

            var data = _ActivityTypeMethod.SaveActivityTypeData(Id, SessionProxy.UserId, year,name,curriencyId,workunitId,workerRate,customerRate);
            if (!data)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                ActivityTypeViewModel model = returnActivityTypeList();
                return PartialView("_partialActivityTypeList", model);
            }

        }

        public ActionResult EditActivityType(int Id)
        {
            ActivityTypeViewModel model = new ActivityTypeViewModel();

            var data = _ActivityTypeMethod.getActivityTypeListById(Id);
            var currencyList = _otherSettingMethod.getSystemListValueById((int)data.CurrencyID);
            var Worklist = _otherSettingMethod.getSystemListValueById((int)data.WorkUnitID);
            var titleId = _otherSettingMethod.getSystemListByName("Work Unit List");
            var genderId = _otherSettingMethod.getSystemListByName("Company Setting Currencies");
            var titleLists = _otherSettingMethod.getAllSystemValueListByNameId(titleId.Id);
            var genderLists = _otherSettingMethod.getAllSystemValueListByNameId(genderId.Id);

            model.Id = data.Id;
            model.Name = data.Name;
            model.CurrencyID = (int)data.CurrencyID;
            model.CurrencyValue = currencyList.Value;
            model.WorkUnitID = (int)data.WorkUnitID;
            model.WorkUnitValue=Worklist.Value;
            model.Year = (int)data.Year;
            if (data.WorkerRate != 0 && data.WorkerRate!=null)
            {
                model.WorkerRate = (decimal)data.WorkerRate;
            }
            else
            {
                model.WorkerRate = 0;
            }
            if (data.CustomerRate != 0 && data.CustomerRate!=null)
            {
                 model.CustomerRate = (decimal)data.CustomerRate;
            }
            else
            {
                model.CustomerRate =0;
            }
            foreach (var item in titleLists)
            {
                OtherSettingValueViewModel tableModel = new OtherSettingValueViewModel();
                tableModel.Id = item.Id;
                tableModel.SystemListID = item.SystemListID;
                tableModel.Value = item.Value;
                model.workUnitList.Add(tableModel);
            }
            foreach (var item in genderLists)
            {
                OtherSettingValueViewModel tableModel = new OtherSettingValueViewModel();
                tableModel.Id = item.Id;
                tableModel.SystemListID = item.SystemListID;
                tableModel.Value = item.Value;
                model.currencyList.Add(tableModel);
            }

            if (data == null)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return PartialView("_partailAddActivityType", model);
            }

        }

        public ActionResult DeleteActivityType(int Id)
        {
            var data = _ActivityTypeMethod.deleteActivityType(Id, SessionProxy.UserId);

            if (!data)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                ActivityTypeViewModel model = returnActivityTypeList();
                return PartialView("_partialActivityTypeList", model);
            }
        }

        #endregion
    }
}