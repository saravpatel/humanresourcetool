using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using HRTool.CommanMethods;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class OtherSettingController : Controller
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();

        #endregion

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public List<OtherSettingViewModel> returnList()
        {
            List<OtherSettingViewModel> model = new List<OtherSettingViewModel>();
            var listData = _otherSettingMethod.getAllSystemLise();
            foreach (var item in listData)
            {
                OtherSettingViewModel tableModel = new OtherSettingViewModel();
                tableModel.Id = item.Id;
                tableModel.SystemListName = item.SystemListName;
                tableModel.Archived = (bool)item.Archived;
                model.Add(tableModel);
            }
            return model;
        }

        public ActionResult List()
        {
            List<OtherSettingViewModel> model = returnList();
            return PartialView("_PartialOtherSettingList", model);
        }

        public ActionResult AddEditOtherSetting(int Id)
        {
            OtherSettingViewModel model = new OtherSettingViewModel();
            model.Id = Id;
            if (Id > 0)
            {
                var SystemLists = _otherSettingMethod.getSystemListById(Id);
                var SystemListValueList = _otherSettingMethod.getAllSystemValueListByNameId(Id);
                model.SystemListName = SystemLists.SystemListName;
                model.Archived = (bool)SystemLists.Archived;
                foreach (var item in SystemListValueList)
                {
                    OtherSettingValueViewModel otherSettingValueViewModel = new OtherSettingValueViewModel();
                    otherSettingValueViewModel.Id = item.Id;
                    otherSettingValueViewModel.SystemListID = item.SystemListID;
                    otherSettingValueViewModel.Value = item.Value;
                    otherSettingValueViewModel.Archived = (bool)item.Archived;
                    model.valueList.Add(otherSettingValueViewModel);
                }
            }
            return PartialView("_partialAddOtherSetting", model);
        }
        public ActionResult DeleteOtherSetting(int Id)
        {
            OtherSettingViewModel model = new OtherSettingViewModel();
            model.Id = Id;
            if (Id > 0)
            {                
                SystemListValue SystemListValue = _db.SystemListValues.Where(x => x.Id == Id).FirstOrDefault();
                SystemListValue.UserIDLastModifiedBy = SessionProxy.UserId;
                SystemListValue.LastModified = DateTime.Now;
                SystemListValue.Archived = true;
                _db.SaveChanges();                                
            }
            return PartialView("_PartialOtherSettingList",returnList());
            
        }
        public ActionResult saveOtherSetting(int Id, string ListName, string ListValue, bool Flag)
        {
            List<OtherSettingViewModel> model = null;
            if (Flag == false)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                if (!string.IsNullOrEmpty(ListValue))
                {
                    string[] listValueArray = js.Deserialize<string[]>(ListValue);
                    _otherSettingMethod.SaveData(Id, ListName, listValueArray, SessionProxy.UserId);
                }
                model = returnList();

            }
            else if (Flag == true)
            {
                //List<systemListData> ListData = new List<systemListData>();
                JavaScriptSerializer js = new JavaScriptSerializer();
                List<systemListData> ListData = js.Deserialize<List<systemListData>>(ListValue);
                _otherSettingMethod.EditData(Id, ListName, ListData, SessionProxy.UserId);
                model = returnList();

            }
            return PartialView("_PartialOtherSettingList", model);
        }
    }
}