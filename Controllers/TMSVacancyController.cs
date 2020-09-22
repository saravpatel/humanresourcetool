using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HRTool.DataModel;
using HRTool.Models.Admin;
using HRTool.CommanMethods.Admin;
using HRTool.CommanMethods.Settings;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using HRTool.Models.Settings;
using System.Configuration;
using System.IO;
using HRTool.CommanMethods;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class TMSVacancyController : Controller
    {

        #region Constant
        EvolutionEntities _db = new EvolutionEntities();
        AdminTMSMethod _AdminTMSMethod = new AdminTMSMethod();
        TMSSettingsMethod _TMSSettingsMethod = new TMSSettingsMethod();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        CompanyStructureMethod _CompanyStructureMethod = new CompanyStructureMethod();

        #endregion

        // GET: /TMSVacancy/
        public ActionResult Index(int VacancyId)
        {
            TMSVacancyJobDetailsViewModel model = new TMSVacancyJobDetailsViewModel();
            var datamodel = _AdminTMSMethod.getVacancyDetailsById(VacancyId);
            model.Id = datamodel.Id;
            model.Title = datamodel.Title;
            model.Location = datamodel.Location;
            model.Salary = datamodel.Salary;
            model.JobDescription = StripHTML(datamodel.JobDescription);
            model.JobDescription = datamodel.JobDescription;
            if (datamodel.BusinessID != 0)
            {
                var buz = _CompanyStructureMethod.getBusinessListById((int)datamodel.BusinessID);
                model.Business = buz.Name;
            }
            if (datamodel.DivisionID != 0)
            {
                var div = _CompanyStructureMethod.getDivisionById((int)datamodel.DivisionID);
                model.Division = div.Name;
            }
            if (datamodel.PoolID != 0)
            {
                var pol = _CompanyStructureMethod.getPoolsListById((int)datamodel.PoolID);
                model.Pool = pol.Name;
            }
            if (datamodel.FunctionID != 0)
            {
                var fun = _CompanyStructureMethod.getFunctionsListById((int)datamodel.FunctionID);
                model.Function = fun.Name;
            }
            return View(model);
        }
        public ActionResult getDescription(int VacancyId)
        {
            TMSVacancyJobDetailsViewModel model = new TMSVacancyJobDetailsViewModel();
            var datamodel = _AdminTMSMethod.getVacancyDetailsById(VacancyId);
            model.JobDescription = datamodel.JobDescription;
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public TMSApplicantViewModel returnApplicantList(int Id, int VacancyId)
        {
            TMSApplicantViewModel datamodel = new TMSApplicantViewModel();

            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Gender List"))
            {
                datamodel.GenderList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Vacancy Source List"))
            {
                datamodel.SourceList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            var data = _AdminTMSMethod.getVacancyDetailsById(VacancyId);
            datamodel.GenderName = datamodel.GenderList[0].Value;
            datamodel.DownloadApplicationFormLink = data.ApplicationFormPath;
            TMSVacancyViewModel vacancyDetailsModel = new TMSVacancyViewModel();
            vacancyDetailsModel.Id = data.Id;
            vacancyDetailsModel.MustUploadCoverLetter = data.MustUploadCoverLetter;
            vacancyDetailsModel.MustUploadResumeCV = data.MustUploadResumeCV;
            vacancyDetailsModel.ApplicationFormPathOriginal = data.ApplicationFormPathOriginal;
            vacancyDetailsModel.ApplicationFormPath = data.ApplicationFormPath;
            vacancyDetailsModel.Question1On = data.Question1On;
            vacancyDetailsModel.Question1Text = data.Question1Text;
            vacancyDetailsModel.Question2On = data.Question2On;
            vacancyDetailsModel.Question2Text = data.Question2Text;
            vacancyDetailsModel.Question3On = data.Question3On;
            vacancyDetailsModel.Question3Text = data.Question3Text;
            vacancyDetailsModel.Question4On = data.Question4On;
            vacancyDetailsModel.Question4Text = data.Question4Text;
            vacancyDetailsModel.Question5On = data.Question5On;
            vacancyDetailsModel.Question5Text = data.Question5Text;
            datamodel.VacancyDetails = vacancyDetailsModel;
            if (data.RecruitmentProcessID != 0)
            {
                var res = _TMSSettingsMethod.getTMSSettingListById(data.RecruitmentProcessID);
                if (res.StepCSV != "1")
                {
                    var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(res.StepCSV);
                    if (steps != null)
                    {
                        foreach (var s in steps)
                        {
                            TMSSettingStepDetails ss = new TMSSettingStepDetails();
                            ss.Id = s.Id;
                            ss.StepName = s.StepName;
                            ss.ColorCode = s.ColorCode;
                            datamodel.StatusList.Add(ss);
                            if (s.StepName == "New Applicants")
                            {
                                datamodel.StatusID = s.Id;
                            }
                        }
                    }
                }
            }
            //DateTime today = DateTime.Now;
            //datamodel.DateOfBirth = String.Format("{0:dd-MM-yyyy}", today);
            return datamodel;
        }
        public ActionResult JobDetails(int Id, int VacancyId)
        {
            TMSApplicantViewModel model = returnApplicantList(Id, VacancyId);
            return PartialView("_partialAddJobApplyApplicant", model);
        }

        public ActionResult Emailcheck(string Email,int Id,int VacancyID) 
        {
            bool save = _AdminTMSMethod.Emailcheck(Email, Id, VacancyID);

            if (save)
            {
                return Json("True", JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult AddApplicant(TMSApplicantViewModel datamodel) 
        {
            
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<TMSApplicantCommentViewModel> listComment = new List<TMSApplicantCommentViewModel>();
            List<TMSApplicantDocumentViewModel> listDocument = new List<TMSApplicantDocumentViewModel>();

            bool save = _AdminTMSMethod.SaveApplicantData(datamodel, listComment, listDocument, SessionProxy.UserId);


            if (save)
            {
                HRTool.Models.MailModel mail = new HRTool.Models.MailModel();
                mail.From = datamodel.Email;
                mail.To = datamodel.Email;
                mail.Subject = "Application";
                string inputFormat = "ddd, dd MMM yyyy";
                string dateTimeEndorse = DateTime.Now.ToString("ddd, dd MMM yyyy");                
                mail.Body = "Thank You";
                string mailFromReceive = Common.sendMail(mail);
                return Json("True", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);

            }
        
        }
        public static string StripHTML(string input)
        {
            if (input != null)
            {
                return Regex.Replace(input, "<.*?>", String.Empty);
            }
            else
            {
                return String.Empty;
            }
        }

        [HttpPost]
        public ActionResult FileUploadData()
        {
            string FilePath = string.Empty;
            string fileName = string.Empty;
            string originalFileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                FilePath = ConfigurationManager.AppSettings["TMSApplicant"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
        }
    }
}