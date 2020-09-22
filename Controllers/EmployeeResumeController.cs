using HRTool.DataModel;
using HRTool.Models.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Globalization;
using HRTool.CommanMethods.Settings;
using System.Text.RegularExpressions;
using Rotativa;
using Rotativa.Options;
using HRTool.CommanMethods.Resources;
using HRTool.CommanMethods;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class EmployeeResumeController : Controller
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        private string inputFormat = "dd-MM-yyyy";
        private string outputFormat = "yyyy-MM-dd HH:mm:ss";
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        EmployeeMethod _employeeMethod = new EmployeeMethod();


        #endregion

        #region View
        // GET: /EmployeeResume/
        public ActionResult Index(int EmployeeId)
        {
            EmployeeResumeViewModel model = new EmployeeResumeViewModel();
            model.EmployeeID = EmployeeId;
            return View(model);
        }

        #endregion

        public EmployeeResumeViewModel AllDetailsList(int EmployeeId)
        {
            EmployeeResumeViewModel modelList = new EmployeeResumeViewModel();
            modelList.EmployeeID = EmployeeId;
            var workdetails = _db.Employee_Experiences.Where(x => x.EmployeeID == EmployeeId && x.Archived == false).ToList();
            var educationDetails = _db.Employee_Educations.Where(x => x.EmployeeID == EmployeeId && x.Archived == false).ToList();
            var qualificationlist = _db.Employee_Qualifications.Where(x => x.EmployeeID == EmployeeId && x.Archived == false).ToList();
            var laguageDetailsList = _db.Employee_Languages.Where(x => x.EmployeeID == EmployeeId && x.Archived == false).ToList();
            var resumeText = _db.Employee_Resume.Where(x => x.EmployeeID == EmployeeId).FirstOrDefault();
            if (resumeText != null)
            {
                modelList.ResumeID = resumeText.Id;
                //modelList.ResumeText = resumeText.ResumeText;
                string s = Regex.Replace(resumeText.ResumeText, "<.*?>", String.Empty);
                modelList.ResumeText = s;
            }
            if (workdetails.Count > 0)
            {
                foreach (var item in workdetails)
                {
                    EmployeeExperienceViewModel model = new EmployeeExperienceViewModel();
                    model.Id = item.Id;
                    model.EmployeeID = item.EmployeeID;
                    model.CompanyName = item.CompanyName;
                    model.JobTitle = item.JobTitle;
                    model.JobStartDate = String.Format("{0:MMM-yyyy}", item.JobStartDate);
                    model.JobEndDate = String.Format("{0:MMM-yyyy}", item.JobEndDate);
                    model.OtherInformation = item.OtherInformation;
                    modelList.WorkExperienceList.Add(model);
                }


            }
            if (educationDetails.Count > 0)
            {
                foreach (var item in educationDetails)
                {
                    EmployeeEducationsViewModel model = new EmployeeEducationsViewModel();
                    model.Id = item.Id;
                    model.EmployeeID = item.EmployeeID;
                    model.CourseName = item.CourseName;
                    model.InstitutionName = item.InstitutionName;
                    model.StartDate = String.Format("{0:MMM-yyyy}", item.StartDate);
                    model.EndDate = String.Format("{0:MMM-yyyy}", item.EndDate);
                    model.OtherInformation = item.OtherInformation;
                    modelList.EductionList.Add(model);
                }
            }
            if (qualificationlist.Count > 0)
            {
                foreach (var item in qualificationlist)
                {
                    EmployeeQualificationsViewModel model = new EmployeeQualificationsViewModel();
                    model.Id = item.Id;
                    model.EmployeeID = item.EmployeeID;
                    model.Detail = item.Detail;
                    modelList.QualificationList.Add(model);
                }
            }
            if (laguageDetailsList.Count > 0)
            {
                foreach (var item in laguageDetailsList)
                {
                    var languageName = _otherSettingMethod.getSystemListValueById((int)item.LanguageID);
                    //var SpeakingName = _otherSettingMethod.getSystemListValueById((int)item.SpeakingID);
                    //var ListeningName = _otherSettingMethod.getSystemListValueById((int)item.ListeningID);
                    //var WritingName = _otherSettingMethod.getSystemListValueById((int)item.WritingID);
                    //var ReadingName = _otherSettingMethod.getSystemListValueById((int)item.ReadingID);

                    EmployeeLanguagesViewModel model = new EmployeeLanguagesViewModel();
                    model.Id = item.Id;
                    model.EmployeeID = item.EmployeeID;
                    model.LanguageID = (int)item.LanguageID;
                    model.LanguageName = languageName.Value;
                    modelList.LanguageDetailsList.Add(model);
                    //model.SpeakingID = (int)item.SpeakingID;
                    //model.SpeakingName = SpeakingName.Value;
                    //model.ListeningID = (int)item.ListeningID;
                    //model.ListeningName = ListeningName.Value;
                    //model.WritingID = (int)item.WritingID;
                    //model.WritingName = WritingName.Value;
                    //model.ReadingID = (int)item.ReadingID;
                    //model.ReadingName = ReadingName.Value;

                }
            }
            return modelList;


        }

        public ActionResult ResumeDetailsList(int EmployeeId)
        {
            EmployeeResumeViewModel model = AllDetailsList(EmployeeId);
            return PartialView("_partialResumeDetailsList", model);
        }

        #region Resume Text

        public ActionResult SaveResumeText(EmployeeResumeTextViewModel datamodel)
        {
            var model = _db.Employee_Resume.Where(x => x.EmployeeID == datamodel.EmployeeID).FirstOrDefault();
            if (model == null)
            {
                Employee_Resume mm = new Employee_Resume();
                mm.EmployeeID = datamodel.EmployeeID;
                mm.ResumeText = datamodel.ResumeText;
                _db.Employee_Resume.Add(mm);
                _db.SaveChanges();
            }
            else
            {
                model.ResumeText = datamodel.ResumeText;
                _db.SaveChanges();

            }

            EmployeeResumeViewModel models = AllDetailsList(datamodel.EmployeeID);
            return PartialView("_partialResumeDetailsList", models);

        }

        #endregion

        #region Work Exxperience

        public ActionResult AddEditWorkExperience(int Id, int employeeId)
        {
            EmployeeExperienceViewModel model = new EmployeeExperienceViewModel();
            if (Id == 0)
            {
                model.Id = 0;
                model.EmployeeID = employeeId;
                model.JobStartDate = String.Format("{0:dd-MM-yyyy}", DateTime.Now);
                model.JobEndDate = String.Format("{0:dd-MM-yyyy}", DateTime.Now);
            }
            else
            {
                var data = _db.Employee_Experiences.Where(x => x.Id == Id).FirstOrDefault();
                model.Id = data.Id;
                model.EmployeeID = data.EmployeeID;
                model.JobTitle = data.JobTitle;
                model.CompanyName = data.CompanyName;
                model.JobStartDate = String.Format("{0:dd-MM-yyyy}", data.JobStartDate);
                model.JobEndDate = String.Format("{0:dd-MM-yyyy}", data.JobEndDate);
                model.OtherInformation = data.OtherInformation;

            }
            return PartialView("_partialAddEditWorkExperience", model);
        }

        public ActionResult SaveWorkExperirnce(EmployeeExperienceViewModel model)
        {
            DateTime StDate = new DateTime();
            DateTime EdDate = new DateTime();
            var startDateToString = DateTime.ParseExact(model.JobStartDate, inputFormat, CultureInfo.InvariantCulture);
            StDate = Convert.ToDateTime(startDateToString.ToString(outputFormat));
            var EndDateToString = DateTime.ParseExact(model.JobEndDate, inputFormat, CultureInfo.InvariantCulture);
            EdDate = Convert.ToDateTime(EndDateToString.ToString(outputFormat));

            if (model.Id == 0)
            {
                Employee_Experiences mm = new Employee_Experiences();
                mm.EmployeeID = model.EmployeeID;
                mm.JobTitle = model.JobTitle;
                mm.CompanyName = model.CompanyName;
                mm.JobStartDate = StDate;
                mm.JobEndDate = EdDate;
                mm.OtherInformation = model.OtherInformation;
                mm.Archived = false;
                mm.CreatedDate = DateTime.Now;
                mm.LastModified = DateTime.Now;
                mm.UserIDCreatedBy = SessionProxy.UserId;
                mm.UserIDLastModifiedBy = SessionProxy.UserId;
                _db.Employee_Experiences.Add(mm);
                _db.SaveChanges();
            }
            else
            {
                var data = _db.Employee_Experiences.Where(x => x.Id == model.Id).FirstOrDefault();
                data.JobTitle = model.JobTitle;
                data.CompanyName = model.CompanyName;
                data.JobStartDate = StDate;
                data.JobEndDate = EdDate;
                data.OtherInformation = model.OtherInformation;
                data.Archived = false;
                data.LastModified = DateTime.Now;
                data.UserIDLastModifiedBy = SessionProxy.UserId;
                _db.SaveChanges();

            }

            EmployeeResumeViewModel models = AllDetailsList(model.EmployeeID);
            return PartialView("_partialResumeDetailsList", models);

        }

        public ActionResult DeleteWorkExperience(int Id, int EmployeeID)
        {
            var data = _db.Employee_Experiences.Where(x => x.Id == Id).FirstOrDefault();
            data.Archived = true;
            data.LastModified = DateTime.Now;
            data.UserIDLastModifiedBy = SessionProxy.UserId;
            _db.SaveChanges();

            EmployeeResumeViewModel model = AllDetailsList(EmployeeID);
            return PartialView("_partialResumeDetailsList", model);

        }


        #endregion

        #region Education

        public ActionResult AddEditEducation(int Id, int employeeId)
        {
            EmployeeEducationsViewModel model = new EmployeeEducationsViewModel();
            if (Id == 0)
            {
                model.Id = 0;
                model.EmployeeID = employeeId;
                model.StartDate = String.Format("{0:dd-MM-yyyy}", DateTime.Now);
                model.EndDate = String.Format("{0:dd-MM-yyyy}", DateTime.Now);
            }
            else
            {
                var data = _db.Employee_Educations.Where(x => x.Id == Id).FirstOrDefault();
                model.Id = data.Id;
                model.EmployeeID = data.EmployeeID;
                model.CourseName = data.CourseName;
                model.InstitutionName = data.InstitutionName;
                model.StartDate = String.Format("{0:dd-MM-yyyy}", data.StartDate);
                model.EndDate = String.Format("{0:dd-MM-yyyy}", data.EndDate);
                model.OtherInformation = data.OtherInformation;

            }
            return PartialView("_partialAddEditEducation", model);
        }

        public ActionResult SaveEducation(EmployeeEducationsViewModel model)
        {
            DateTime StDate = new DateTime();
            DateTime EdDate = new DateTime();
            var startDateToString = DateTime.ParseExact(model.StartDate, inputFormat, CultureInfo.InvariantCulture);
            StDate = Convert.ToDateTime(startDateToString.ToString(outputFormat));
            var EndDateToString = DateTime.ParseExact(model.EndDate, inputFormat, CultureInfo.InvariantCulture);
            EdDate = Convert.ToDateTime(EndDateToString.ToString(outputFormat));

            if (model.Id == 0)
            {
                Employee_Educations mm = new Employee_Educations();
                mm.EmployeeID = model.EmployeeID;
                mm.CourseName = model.CourseName;
                mm.InstitutionName = model.InstitutionName;
                mm.StartDate = StDate;
                mm.EndDate = EdDate;
                mm.OtherInformation = model.OtherInformation;
                mm.Archived = false;
                mm.CreatedDate = DateTime.Now;
                mm.LastModified = DateTime.Now;
                mm.UserIDCreatedBy = SessionProxy.UserId;
                mm.UserIDLastModifiedBy = SessionProxy.UserId;
                _db.Employee_Educations.Add(mm);
                _db.SaveChanges();
            }
            else
            {
                var data = _db.Employee_Educations.Where(x => x.Id == model.Id).FirstOrDefault();
                data.CourseName = model.CourseName;
                data.InstitutionName = model.InstitutionName;
                data.StartDate = StDate;
                data.EndDate = EdDate;
                data.OtherInformation = model.OtherInformation;
                data.Archived = false;
                data.LastModified = DateTime.Now;
                data.UserIDLastModifiedBy = SessionProxy.UserId;
                _db.SaveChanges();

            }

            EmployeeResumeViewModel models = AllDetailsList(model.EmployeeID);
            return PartialView("_partialResumeDetailsList", models);

        }

        public ActionResult DeleteEducation(int Id, int EmployeeID)
        {
            var data = _db.Employee_Educations.Where(x => x.Id == Id).FirstOrDefault();
            data.Archived = true;
            data.LastModified = DateTime.Now;
            data.UserIDLastModifiedBy = SessionProxy.UserId;
            _db.SaveChanges();

            EmployeeResumeViewModel model = AllDetailsList(EmployeeID);

            return PartialView("_partialResumeDetailsList", model);

        }


        #endregion

        #region Qualifacation
        public ActionResult AddEditQualification(int Id, int employeeId)
        {
            EmployeeQualificationsViewModel model = new EmployeeQualificationsViewModel();
            if (Id == 0)
            {
                model.Id = 0;
                model.EmployeeID = employeeId;
            }
            else
            {
                var data = _db.Employee_Qualifications.Where(x => x.Id == Id).FirstOrDefault();
                model.Id = data.Id;
                model.EmployeeID = data.EmployeeID;
                model.Detail = data.Detail;

            }
            return PartialView("_partialAddEditQualification", model);
        }

        public ActionResult SaveQualification(EmployeeQualificationsViewModel model)
        {
            if (model.Id == 0)
            {
                Employee_Qualifications mm = new Employee_Qualifications();
                mm.EmployeeID = model.EmployeeID;
                mm.Detail = model.Detail;
                mm.Archived = false;
                mm.CreatedDate = DateTime.Now;
                mm.LastModified = DateTime.Now;
                mm.UserIDCreatedBy = SessionProxy.UserId;
                mm.UserIDLastModifiedBy = SessionProxy.UserId;
                _db.Employee_Qualifications.Add(mm);
                _db.SaveChanges();
            }
            else
            {
                var data = _db.Employee_Qualifications.Where(x => x.Id == model.Id).FirstOrDefault();
                data.Detail = model.Detail;
                data.Archived = false;
                data.LastModified = DateTime.Now;
                data.UserIDLastModifiedBy = SessionProxy.UserId;
                _db.SaveChanges();

            }

            EmployeeResumeViewModel models = AllDetailsList(model.EmployeeID);
            return PartialView("_partialResumeDetailsList", models);

        }

        public ActionResult DeleteQualification(int Id, int EmployeeID)
        {
            var data = _db.Employee_Qualifications.Where(x => x.Id == Id).FirstOrDefault();
            data.Archived = true;
            data.LastModified = DateTime.Now;
            data.UserIDLastModifiedBy = SessionProxy.UserId;
            _db.SaveChanges();

            EmployeeResumeViewModel model = AllDetailsList(EmployeeID);

            return PartialView("_partialResumeDetailsList", model);

        }

        #endregion

        #region Language

        public ActionResult AddEditLanguage(int Id, int employeeId)
        {
            EmployeeLanguagesViewModel model = new EmployeeLanguagesViewModel();
            var Languagelist = _otherSettingMethod.getAllSystemValueListByKeyName("Language List");
            var ProficiencyList = _otherSettingMethod.getAllSystemValueListByKeyName("Language Proficiency List");

            if (Id == 0)
            {
                model.Id = 0;
                model.EmployeeID = employeeId;
                foreach (var item in Languagelist)
                {
                    model.LanguageList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
                }
                foreach (var item in ProficiencyList)
                {
                    model.SpeakingList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
                }
                foreach (var item in ProficiencyList)
                {
                    model.ListeningList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
                }
                foreach (var item in ProficiencyList)
                {
                    model.WritingList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
                }
                foreach (var item in ProficiencyList)
                {
                    model.ReadingList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
                }



            }
            else
            {
                var data = _db.Employee_Languages.Where(x => x.Id == Id).FirstOrDefault();
                var languageName = _otherSettingMethod.getSystemListValueById((int)data.LanguageID);
                var SpeakingName = _otherSettingMethod.getSystemListValueById((int)data.SpeakingID);
                var ListeningName = _otherSettingMethod.getSystemListValueById((int)data.ListeningID);
                var WritingName = _otherSettingMethod.getSystemListValueById((int)data.WritingID);
                var ReadingName = _otherSettingMethod.getSystemListValueById((int)data.ReadingID);
                model.Id = data.Id;
                model.EmployeeID = data.EmployeeID;
                model.LanguageID = (int)data.LanguageID;
                model.LanguageName = languageName.Value;
                model.SpeakingID = (int)data.SpeakingID;
                // model.SpeakingName = SpeakingName.Value;
                model.ListeningID = (int)data.ListeningID;
                //  model.ListeningName = ListeningName.Value;
                model.WritingID = (int)data.WritingID;
                //   model.WritingName = WritingName.Value;
                model.ReadingID = (int)data.ReadingID;
                //  model.ReadingName = ReadingName.Value;


            }
            return PartialView("_partialAddEditLanguageDetails", model);
        }

        public ActionResult SaveLanguage(EmployeeLanguagesViewModel model)
        {

            var dataDetails = _db.Employee_Languages.Where(x => x.LanguageID == model.LanguageID && x.EmployeeID != model.EmployeeID).ToList();
            if (dataDetails.Count > 0)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (model.Id == 0)
                {
                    Employee_Languages mm = new Employee_Languages();
                    mm.EmployeeID = model.EmployeeID;
                    mm.LanguageID = model.LanguageID;
                    mm.SpeakingID = model.SpeakingID;
                    mm.ListeningID = model.ListeningID;
                    mm.WritingID = model.WritingID;
                    mm.ReadingID = model.ReadingID;
                    mm.Archived = false;
                    mm.Created = DateTime.Now;
                    mm.LastModified = DateTime.Now;
                    mm.UserIDCreatedBy = SessionProxy.UserId;
                    mm.UserIDLastModifiedBy = SessionProxy.UserId;
                    _db.Employee_Languages.Add(mm);
                    _db.SaveChanges();
                }
                else
                {
                    var data = _db.Employee_Languages.Where(x => x.Id == model.Id).FirstOrDefault();
                    data.LanguageID = model.LanguageID;
                    data.SpeakingID = model.SpeakingID;
                    data.ListeningID = model.ListeningID;
                    data.WritingID = model.WritingID;
                    data.ReadingID = model.ReadingID;
                    data.Archived = false;
                    data.LastModified = DateTime.Now;
                    data.UserIDLastModifiedBy = SessionProxy.UserId;
                    _db.SaveChanges();

                }
                EmployeeResumeViewModel models = AllDetailsList(model.EmployeeID);
                return PartialView("_partialResumeDetailsList", models);
            }
        }

        public ActionResult DeleteLanguage(int Id, int EmployeeID)
        {
            var data = _db.Employee_Languages.Where(x => x.Id == Id).FirstOrDefault();
            data.Archived = true;
            data.LastModified = DateTime.Now;
            data.UserIDLastModifiedBy = SessionProxy.UserId;
            _db.SaveChanges();

            EmployeeResumeViewModel model = AllDetailsList(EmployeeID);

            return PartialView("_partialResumeDetailsList", model);

        }

        #endregion

        #region Export Pdf Resume

        public ActionResult genaratePDF(int EmployeeId)
        {
            try
            {
                EmployeeResumePDFViewModel model = new EmployeeResumePDFViewModel();
                var EmployeeDetails = _employeeMethod.getEmployeeById(EmployeeId);//_db.AspNetUsers.Where(x => x.Id == EmployeeId).FirstOrDefault();
                model.EmployeeID = EmployeeId;
                model.FirstName = EmployeeDetails.FirstName;
                model.LastName = EmployeeDetails.LastName;
                DateTime currentDate = DateTime.Now;
                string newfileName = string.Format("" + model.FirstName + "_" + model.LastName + "_Resume.pdf", currentDate.Date);
                return new Rotativa.ViewAsPdf("ResumePDF", model)
                {
                    PageSize = Size.A4,
                    PageOrientation = Orientation.Landscape,
                    FileName = newfileName
                };
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ResumePDF(EmployeeResumePDFViewModel dataModel)
        {
            var EmployeeDetails = _employeeMethod.getEmployeeById(dataModel.EmployeeID);
            var EmployeeAddress = _db.EmployeeAddressInfoes.Where(x => x.UserId == dataModel.EmployeeID).FirstOrDefault();
            dataModel.FirstName = EmployeeDetails.FirstName;
            dataModel.LastName = EmployeeDetails.LastName;
            dataModel.Address = EmployeeAddress.ContactAddress;
            dataModel.Email = EmployeeAddress.PersonalEmail;
            dataModel.PersonalEmail = EmployeeAddress.PersonalEmail;
            dataModel.PersonalPhone = EmployeeAddress.PersonalPhone;
            //model.JobTitle
            //model.ResumeText
            EmployeeResumeViewModel Alldetails = AllDetailsList(dataModel.EmployeeID);
            dataModel.AllDetails = Alldetails;
            dataModel.ResumeText = StripHTML(Alldetails.ResumeText);
            if (Alldetails.WorkExperienceList.Count > 0)
            {
                var list = Alldetails.WorkExperienceList.OrderBy(x => x.Id).LastOrDefault();
                dataModel.JobTitle = list.JobTitle;

            }

            return View(dataModel);
        }

        #endregion

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

    }
}