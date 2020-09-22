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
    public class MobileSettingController : Controller
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        MobileSettingMethod _mobileSettingMethod = new MobileSettingMethod();

        #endregion

        public ActionResult Index()
        {
            MobileSettingViewModel model = new MobileSettingViewModel();
            var data = _mobileSettingMethod.getMobileSetting();
            if (data.Id > 0)
            {
                model.AllowMobileUse = data.AllowMobileUse;
                model.ShowPhoneNumber = data.ShowPhoneNumber;
            }
            else
            {
                model.AllowMobileUse = false;
                model.ShowPhoneNumber = false;
            }
            return View(model);
        }

        public ActionResult SaveData(string AllowMobileUse, string ShowPhoneNumber)
        {
            _mobileSettingMethod.saveData(Convert.ToBoolean(AllowMobileUse), Convert.ToBoolean(ShowPhoneNumber));
            return Json("success", JsonRequestBehavior.AllowGet);
        }
    }
}