using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Microsoft.AspNet.Identity;
using HRTool.Models.Settings;
using System.Configuration;
using HRTool.CommanMethods;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class CompanyController : Controller
    {
        #region Constant

        CompanyMethod _CompanyMethod = new CompanyMethod();

        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();

        #endregion


        #region View

        // GET:  CompanySettings
        /// <summary>
        /// Get  Company Setting Record
        /// </summary>
        /// <returns></returns>

        [Authorize]
        public ActionResult Index()
        {
            CompanyViewModel model = new CompanyViewModel();
            var companysettindId = _CompanyMethod.CompanysettingList();
            if (companysettindId != null)
            {
                model = _CompanyMethod.BindCompanySettingRecords(companysettindId.Id);
            }
            else
            {
                model = _CompanyMethod.defaultCompanyDatabind();
            }
            return View(model);
        }


        /// <summary>
        /// Insert Company Setting Record Using Company_Settings table
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(CompanyViewModel model, HttpPostedFileBase fileToUpload)
        {
            try
            {
                string FilePath = string.Empty;
                //string fileName = string.Empty;


                if (ModelState.IsValid)
                {
                    FilePath = ConfigurationManager.AppSettings["CompanyLogoFilePath"].ToString();

                    if (fileToUpload != null && fileToUpload.ContentLength > 0)
                    {
                        string path = Path.Combine(Server.MapPath(FilePath),
                                                   Path.GetFileName(fileToUpload.FileName));
                        fileToUpload.SaveAs(path);
                        model.Logo = fileToUpload.FileName;
                    }

                    else
                    {
                        ViewBag.Message = "You have not specified a file.";
                    }
                    if (model.Id > 0)
                    {
                        int UpdatedCompanySetting = _CompanyMethod.InsertUpdateCompanySetting(model, SessionProxy.UserId, true);
                    }
                    else
                    {
                        int InsertCompanySetting = _CompanyMethod.InsertUpdateCompanySetting(model, SessionProxy.UserId, false);
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
        #endregion


    }
}