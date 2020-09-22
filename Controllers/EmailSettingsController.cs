using HRTool.CommanMethods;
using HRTool.CommanMethods.Settings;
using HRTool.Models;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class EmailSettingsController : Controller
    {

        #region Const
        EmailSettingMethod EmailSettingMethods = new EmailSettingMethod();
        #endregion
        // GET: EmailSettings
        /// <summary>
        /// Get Email Setting Record
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            EmailSettingViewModel model = new EmailSettingViewModel();
            var emailsettindId = EmailSettingMethods.EmailsettingList();
            if (emailsettindId != null)
            {
                model = EmailSettingMethods.BindEmailSettingRecords(emailsettindId.Id);
            }
            return View(model);
        }

        /// <summary>
        /// Insert Email Setting Record Using Email_Setting table
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(EmailSettingViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Id > 0)
                    {
                        int UpdatedEmailSetting = EmailSettingMethods.UpdateEmailSetting(model);
                    }
                    else
                    {
                        int InsertEmailSetting = EmailSettingMethods.InsertEmailSetting(model);
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}