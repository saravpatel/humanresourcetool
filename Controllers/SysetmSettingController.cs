using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRTool.Models.Settings;
using HRTool.CommanMethods.Settings;
namespace HRTool.Controllers
{
    public class SysetmSettingController : Controller
    {
        // GET: SysetmSetting
        Sysetm_Setting_Method _systemSettingMethod = new Sysetm_Setting_Method();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SystemSetting_List()
        {
            List<System_SettingViewModel> model = returnList();
            return PartialView("_PartialSysetm_SettingList", model);
        }
        public List<System_SettingViewModel> returnList()
        {
            List<System_SettingViewModel> model = new List<System_SettingViewModel>();
            var listData = _systemSettingMethod.getAllSystemList();
            foreach (var item in listData)
            {
                System_SettingViewModel sysModel = new System_SettingViewModel();
                sysModel.Id = item.Id;
                
                //tableModel.SystemListName = item.SystemListName;
                //tableModel.Archived = (bool)item.Archived;
                //model.Add(tableModel);
            }
            return model;
        }
    }
}