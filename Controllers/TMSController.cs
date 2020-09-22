using HRTool.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HRTool.DataModel;
using HRTool.CommanMethods.Settings;
using HRTool.CommanMethods.Admin;
using System.Configuration;
using System.IO;
using HRTool.CommanMethods.RolesManagement;
using Newtonsoft.Json;
using HRTool.Models.Settings;
using HRTool.Models;
using HRTool.CommanMethods.Resources;
using System.Web.Script.Serialization;
using HRTool.CommanMethods;
using HRTool.Models.Resources;
using Rotativa.Options;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class TMSController : Controller
    {
        #region Constant
        EvolutionEntities _db = new EvolutionEntities();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        CompanyStructureMethod _CompanyStructureMethod = new CompanyStructureMethod();
        AdminTMSMethod _AdminTMSMethod = new AdminTMSMethod();
        TMSSettingsMethod _TMSSettingsMethod = new TMSSettingsMethod();
        EmployeeMethod _employeeMethod = new EmployeeMethod();
        RolesManagementMethod _RolesManagementMethod = new RolesManagementMethod();
        AdminPearformanceMethod _AdminPearformanceMethod = new AdminPearformanceMethod();
        
       // DateTimeSpan _date = new DateTimeSpan();
        #endregion

        #region View
        // GET: /TMS/
        [Authorize]
        public ActionResult Index()
        {
            commonViewModel model = new commonViewModel();
            var currentLoginEmployee = _employeeMethod.getEmployeeById(SessionProxy.UserId);
            model.UserId = currentLoginEmployee.Id;
            model.UserName = currentLoginEmployee.UserName;
            model.Name = string.Format("{0} {1} - {2}", currentLoginEmployee.FirstName, currentLoginEmployee.LastName, currentLoginEmployee.SSOID);
            return View(model);
        }

        #endregion

        #region TMS Index
        public ActionResult TMSIndex()
        {
            TMSIndexPageViewModel model = _AdminTMSMethod.GetAllCount();
            var BusinessList = _CompanyStructureMethod.getAllBusinessList();
            model.BusinessList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in BusinessList)
            {
                model.BusinessList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString() });
            }
            var vacaData = _db.Vacancies.Where(x => x.Archived == false).ToList();
            foreach (var item in vacaData)
            {
                model.VacancyList.Add(new SelectListItem() { Text = item.Title, Value = item.Id.ToString() });
            }
            var RecrutmentData = _db.TMS_Setting_RecruitmentProcesses.Where(x => x.Archived == false).ToList();
            foreach (var item in RecrutmentData)
            {
                model.RecruitmentProcessList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }

            return PartialView("_partialTMSIndexView",model);
        }
        public ActionResult getVacancyList()
        {
            TMSIndexPageViewModel model = _AdminTMSMethod.GetAllCount();
            var vacaData = _db.Vacancies.Where(x => x.Archived == false).ToList();
            foreach (var item in vacaData)
            {
                model.VacancyList.Add(new SelectListItem() { Text = item.Title, Value = item.Id.ToString() });
            }
            return Json(model,JsonRequestBehavior.AllowGet);
        }

        public ActionResult getDivision(int businessId)
        {

            var data = _db.Divisions.Where(x => x.Archived == false && x.BusinessID == businessId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getPool(int divisionId)
        {
            var data = _db.Pools.Where(c => c.DivisionID == divisionId && c.Archived == false);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getFunction(int divisionId)
        {
            var data = _db.Functions.Where(c => c.DivisionID == divisionId && c.Archived == false);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPiechartJSON(int? RecId, int? BusinessId,int? DivisionId,int? PoolId,int? FunctionId)
        {
            List<BlogPieChart> dataList = new List<BlogPieChart>();
            var vdata = _db.Vacancies.Where(x => x.Archived == false).ToList();
            var Applicant = _db.TMS_Applicant.Where(x => x.Archived == false).ToList();

            List<string> NAme = new List<string>();
            NAme.Add("Bussiness");
            NAme.Add("Division");
            NAme.Add("Pool");
            NAme.Add("Function");  
            if(RecId==null)
            {
                RecId = 0;
            }
            if(BusinessId==null)
            {
                BusinessId = 0;
            }          
            if(DivisionId==null)
            {
                DivisionId = 0;
            }
            if(PoolId==null)
            {
                PoolId = 0;
            }
            if(FunctionId==null)
            {
                FunctionId = 0;
            }
            var TMSataList = _AdminTMSMethod.getTMSRecList(RecId, BusinessId, DivisionId, PoolId, FunctionId).ToList();
            foreach (var item in NAme)
            {
                BlogPieChart details = new BlogPieChart();                                        
                if (item.ToString() == "Bussiness")
                {
                    details.Name = item.ToString();                    
                    if (BusinessId != 0 && RecId!=0)
                    {
                        details.BusinessNameCount = _db.Vacancies.Where(x => x.BusinessID == BusinessId && x.RecruitmentProcessID== RecId && x.Archived==false && x.StatusID==4092).Count();
                    }
                    else
                    {
                        details.BusinessNameCount = _db.Vacancies.Count();
                    }
                    //details.Name = item.ToString();
                    //details.BusinessNameCount = TMSataList.Select(x => x.BusinesCount).FirstOrDefault();
                }
               else if (item.ToString() == "Division")
                {
                    details.Name = item.ToString();
                    if (RecId != 0)
                    {
                        details.BusinessNameCount = _db.Vacancies.Where(x => x.DivisionID == DivisionId && x.RecruitmentProcessID == RecId && x.Archived == false && x.StatusID == 4092).Count();
                    }
                    else
                    {
                        details.BusinessNameCount = _db.Vacancies.Count();
                    }
                    // details.Name = item.ToString();
                    //details.BusinessNameCount = TMSataList.Select(x => x.DivisionCount).FirstOrDefault();
                }

                if (item.ToString() == "Pool")
                {
                    details.Name = item.ToString();
                    if (RecId != 0)
                    {
                        details.BusinessNameCount = _db.Vacancies.Where(x => x.PoolID == PoolId && x.RecruitmentProcessID == RecId && x.Archived == false && x.StatusID == 4092).Count();
                    }
                    else
                    {
                        details.BusinessNameCount = _db.Vacancies.Count();
                    }
                    //details.Name = item.ToString();
                    //details.BusinessNameCount = TMSataList.Select(x => x.Poolcount).FirstOrDefault();
                }
                if (item.ToString() == "Function")
                {
                    //details.Name = item.ToString();
                    //details.BusinessNameCount = TMSataList.Select(x => x.FunCount).FirstOrDefault();
                    details.Name = item.ToString();
                    if (RecId != 0)
                    {
                        details.BusinessNameCount = _db.Vacancies.Where(x => x.FunctionID == FunctionId && x.RecruitmentProcessID == RecId && x.Archived == false && x.StatusID == 4092).Count();
                    }
                    else
                    {
                        details.BusinessNameCount = _db.Vacancies.Count();
                    }

                }
                dataList.Add(details);
            }
            return Json(dataList, JsonRequestBehavior.AllowGet);

        }
        #endregion

        public JsonResult GetPiechartByStatusJSON(int? VacId)
        {
            List<BlogChartSource> dataList = new List<BlogChartSource>();
            var vdata = _db.TMS_Applicant.ToList();
            List<string> NAme = new List<string>();           
              foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Vacancy Source List"))
               {
                BlogChartSource model = new BlogChartSource();    
                model.Name = item.Value;
                if(VacId!=0 && VacId!=null)
                {
                    model.Count = vdata.Where(x => x.SourceID == item.Id && x.VacancyID==VacId).Count();
                }
                else
                {
                    model.Count = vdata.Where(x => x.SourceID == item.Id).Count();
                }
                 dataList.Add(model);
                }
                return Json(dataList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCountByVacancyId(int VacancyId)
        {
            TMSIndexPageViewModel model = _AdminTMSMethod.GetAllCountByVacancy(VacancyId);

            return Json(model, JsonRequestBehavior.AllowGet);
        }


        #region Vacancy

        public TMSVacancyDetailsViewModel returnVacancyList()
        {
            TMSVacancyDetailsViewModel model = new TMSVacancyDetailsViewModel();
            var data = _AdminTMSMethod.getAllVacancyList();
            if (data.Count > 0)
            {
                foreach (var s in data)
                {
                    TMSVacancyViewModel datamodel = new TMSVacancyViewModel();
                    datamodel.Id = s.Id;
                    datamodel.Title = s.Title;
                    datamodel.RecruitmentProcessID = s.RecruitmentProcessID;
                    if (datamodel.RecruitmentProcessID != 0)
                    {
                        var res = _TMSSettingsMethod.getTMSSettingListById(s.RecruitmentProcessID);
                        datamodel.RecruitmentProcess = res.Name;
                    }
                    datamodel.HiringLeadID = s.HiringLeadID;
                    if (datamodel.HiringLeadID != 0 && datamodel.HiringLeadID != null)
                    {
                        var info = _db.AspNetUsers.Where(x => x.Id == datamodel.HiringLeadID).FirstOrDefault();
                        datamodel.HiringLead = info.FirstName + info.LastName + "-" + info.SSOID;
                    }
                    datamodel.Location = s.Location;
                    datamodel.BusinessID = (int)s.BusinessID;
                    if (s.BusinessID != 0)
                    {
                        var buz = _CompanyStructureMethod.getBusinessListById((int)s.BusinessID);
                        datamodel.Business = buz.Name;
                    }
                    datamodel.DivisionID = (int)s.DivisionID;
                    if (s.DivisionID != 0)
                    {
                        var div = _CompanyStructureMethod.getDivisionById((int)s.DivisionID);
                        datamodel.Division = div.Name;
                    }
                    datamodel.PoolID = (int)s.PoolID;
                    if (s.PoolID != 0)
                    {
                        var pol = _CompanyStructureMethod.getPoolsListById((int)s.PoolID);
                        datamodel.Pool = pol.Name;
                    }
                    datamodel.FunctionID = (int)s.FunctionID;
                    if (s.FunctionID != 0)
                    {
                        var fun = _CompanyStructureMethod.getFunctionsListById((int)s.FunctionID);
                        datamodel.Function = fun.Name;
                    }

                    datamodel.ClosingDate = String.Format("{0:dd-MM-yyyy}", s.CreatedDate);
                    datamodel.Newapplicants = _AdminTMSMethod.getTotlaNewApplicanyByVac(s.Id);
                    datamodel.StatusID = s.StatusID;
                    if (s.StatusID != 0)
                    {
                        var st = _otherSettingMethod.getSystemListValueById((int)s.StatusID);
                        datamodel.Status = st.Value;
                    }
                    model.VacancyList.Add(datamodel);

                }

            }

            return model;
        }
        public ActionResult getLeadingHeadVacancy()
        {
            TMSVacancyViewModel ledModel = new TMSVacancyViewModel();
            foreach(var item in _RolesManagementMethod.GetEmployeeManagerList())
            {
                ledModel.HiringLeadList.Add(new SelectListItem() { Text = item.FirstName + " " + item.LastName + " - " + item.SSOID,Value=item.Id.ToString() });
            }
            return Json(ledModel, JsonRequestBehavior.AllowGet);
        }
        public TMSVacancyViewModel returnVacancyDetailsList(int Id)
        {
            TMSVacancyViewModel datamodel = new TMSVacancyViewModel();
            foreach (var item in _RolesManagementMethod.GetEmployeeManagerList())
            {
                datamodel.HiringLeadList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Vacancy Status List"))
            {
                datamodel.StatusList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            foreach (var item in _TMSSettingsMethod.getAllTMSSettingList())
            {
                datamodel.RecruitmentProcessList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Vacancy Source List"))
            {
                datamodel.SourceList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            foreach (var item in _CompanyStructureMethod.getAllBusinessList())
            {
                datamodel.BusinessList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });

            }

            if (Id == 0)
            {
                DateTime dt = DateTime.Now;
                datamodel.ClosingDate = String.Format("{0:dd-MM-yyyy}", dt);

            }
            else
            {
                var model = _AdminTMSMethod.getVacancyDetailsById(Id);
                datamodel.Id = model.Id;
                datamodel.Title = model.Title;
                datamodel.Summary = model.Summary;
                datamodel.RecruitmentProcessID = model.RecruitmentProcessID;
                if (datamodel.RecruitmentProcessID != 0)
                {
                    var res = _TMSSettingsMethod.getTMSSettingListById(datamodel.RecruitmentProcessID);
                    datamodel.RecruitmentProcess = res.Name;
                }
                if(model.HiringLeadID!=0 && model.HiringLeadID!=null)
                {
                    var hiringLeadName = _db.AspNetUsers.Where(x => x.Id == model.HiringLeadID && x.Archived == false).FirstOrDefault();
                    if(hiringLeadName!=null)
                    {
                        datamodel.HiringLeadID = model.HiringLeadID;
                        datamodel.HiringLead = hiringLeadName.FirstName + hiringLeadName.LastName + " - " + hiringLeadName.SSOID;
                    }
                }
              //  datamodel.HiringLeadID = model.HiringLeadID;
                datamodel.Location = model.Location;
                datamodel.BusinessID = (int)model.BusinessID;

                if (datamodel.BusinessID != 0)
                {
                    var buz = _CompanyStructureMethod.getBusinessListById((int)datamodel.BusinessID);
                    datamodel.Business = buz.Name;
                }
                datamodel.DivisionID = (int)model.DivisionID;
                if (datamodel.DivisionID != 0)
                {
                    var div = _CompanyStructureMethod.getDivisionById((int)datamodel.DivisionID);
                    datamodel.Division = div.Name;
                }
                datamodel.PoolID = (int)model.PoolID;
                if (datamodel.PoolID != 0)
                {
                    var pol = _CompanyStructureMethod.getPoolsListById((int)datamodel.PoolID);
                    datamodel.Pool = pol.Name;
                }
                datamodel.FunctionID = (int)model.FunctionID;
                if (datamodel.FunctionID != 0)
                {
                    var fun = _CompanyStructureMethod.getFunctionsListById((int)datamodel.FunctionID);
                    datamodel.Function = fun.Name;
                }

                datamodel.ClosingDate = String.Format("{0:dd-MM-yyyy}", model.ClosingDate);

                datamodel.StatusID = model.StatusID;
                if (datamodel.StatusID != 0)
                {
                    var st = _otherSettingMethod.getSystemListValueById((int)datamodel.StatusID);
                    datamodel.Status = st.Value;

                }
                datamodel.JobDescription = model.JobDescription;
                datamodel.Salary = model.Salary;
                datamodel.MustUploadCoverLetter = model.MustUploadCoverLetter;
                datamodel.MustUploadResumeCV = model.MustUploadResumeCV;
                datamodel.ApplicationFormPathOriginal = model.ApplicationFormPathOriginal;
                datamodel.ApplicationFormPath = model.ApplicationFormPath;
                datamodel.Question1On = model.Question1On;
                datamodel.Question1Text = model.Question1Text;
                datamodel.Question2On = model.Question2On;
                datamodel.Question2Text = model.Question2Text;
                datamodel.Question3On = model.Question3On;
                datamodel.Question3Text = model.Question3Text;
                datamodel.Question4On = model.Question4On;
                datamodel.Question4Text = model.Question4Text;
                datamodel.Question5On = model.Question5On;
                datamodel.Question5Text = model.Question5Text;
                datamodel.SourceID = (int)model.SourceID;
                
                if (datamodel.SourceID != 0)
                {
                    var source = _otherSettingMethod.getSystemListValueById((int)datamodel.SourceID);
                    datamodel.Source = source.Value;
                }
            }
            return datamodel;
        }

        public ActionResult bindDivisionList(int businessId)
        {
            var data = _CompanyStructureMethod.GetDivisionListByBizId(businessId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult bindPoolList(int DivisionId)
        {
            var data = _CompanyStructureMethod.GetPoolListByBizId(DivisionId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult bindFuncationList(int DivisionId)
        {
            var data = _CompanyStructureMethod.GetFuncationListByBizId(DivisionId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VacancyList()
        {
            TMSVacancyDetailsViewModel model = returnVacancyList();
            return PartialView("_partialVacancyListView", model);
        }

        public ActionResult AddEditVacancy(int Id)
        {
            TMSVacancyViewModel model = returnVacancyDetailsList(Id);
            return PartialView("_partialAddEditVacancyView", model);
        }

        [HttpPost]
        public ActionResult ImageData()
        {
            string FilePath = string.Empty;
            string fileName = string.Empty;
            string originalFileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                FilePath = ConfigurationManager.AppSettings["AdminVacancy"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
        }

        public ActionResult SaveVacancy(TMSVacancyViewModel model)
        {
            bool save = _AdminTMSMethod.SaveVacancyData(model, SessionProxy.UserId);

            if (save)
            {
                TMSVacancyDetailsViewModel models = returnVacancyList();
                return PartialView("_partialVacancyListView", models);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);

            }

        }

        public ActionResult DeleteVacancy(int Id)
        {
            bool delete = _AdminTMSMethod.DeleteVacancy(Id, SessionProxy.UserId);

            if (delete)
            {
                TMSVacancyDetailsViewModel models = returnVacancyList();
                return PartialView("_partialVacancyListView", models);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);

            }
        }


        #endregion

        #region AppLicant
        public ActionResult AppLicantList()
        {
            TMSVacancyDetailsViewModel model = returnVacancyList();
            return PartialView("_partialApplicantListView", model);
        }

        public ActionResult VacancyDetails(int Id)
        {
            TMSVacancyDetailsViewModel datamodel = new TMSVacancyDetailsViewModel();
            TMSVacancyDetailsViewModel modelss = returnVacancyList();
            datamodel.VacancyList = modelss.VacancyList;
            var model = _AdminTMSMethod.getVacancyDetailsById(Id);
            datamodel.Id = model.Id;
            datamodel.Title = model.Title;                        
            datamodel.ClosingDate = String.Format("{0:dd-MM-yyyy}", model.ClosingDate);
            DateTime TodaysDate = DateTime.Now;
            if(model.ClosingDate<TodaysDate)
            {
                if(model.StatusID!=0)
                {
                    int CloseId = _AdminTMSMethod.UpdateCloseStatus(model.StatusID, Id, TodaysDate);
                    datamodel.StatusID = CloseId;
                }
            }
            else
            {
                datamodel.StatusID = model.StatusID;
            }
            if (datamodel.StatusID != 0)
            {
                var st = _otherSettingMethod.getSystemListValueById((int)datamodel.StatusID);
                datamodel.Status = st.Value;
            }
            datamodel.RecruitmentProcessID = model.RecruitmentProcessID;
            if (datamodel.RecruitmentProcessID != 0)
            {
                var res = _TMSSettingsMethod.getTMSSettingListById(datamodel.RecruitmentProcessID);
                datamodel.RecruitmentProcess = res.Name;
                decimal totalComByVacancy = 0;
                var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(res.StepCSV);
                foreach (var s in steps)
                {
                    TMSSettingStepDetails ss = new TMSSettingStepDetails();
                    ss.Id = s.Id;
                    ss.StepName = s.StepName;
                    ss.ColorCode = s.ColorCode;
                    var applicant = _AdminTMSMethod.getApplicantListByStepIdVacancyID(ss.Id, model.Id);
                    if (applicant.Count > 0)
                    {
                        foreach (var item in applicant)
                        {
                            TMSApplicantDetails ap = new TMSApplicantDetails();
                            ap.Id = item.Id;
                            ap.FirstName = item.FirstName;
                            ap.LastName = item.LastName;
                           // ap.CreateDate = _date.();
                            var diff = DateTimeSpan.CompareDates((DateTime)item.CreatedDate,DateTime.Now);
                            if (datamodel.RecruitmentProcessID != 0)
                            {
                                if (item.CompatencyJSV != null && item.CompatencyJSV != "")
                                {
                                    //var listDatas = _TMSSettingsMethod.getTMSSettingListById(model.RecruitmentProcessID);
                                    //if (listDatas.StepCSV != null)
                                    //{
                                    //    var competency = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(listDatas.CompetencyCSV);
                                    //    totalComByVacancy = competency.Count();
                                    //}
                                    var com = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(item.CompatencyJSV);
                                    int t = 0;                                    
                                    if (com.Count > 0 && com != null)
                                    {
                                        foreach (var comData in com)
                                        {
                                            t = t + Convert.ToInt32(comData.Score);
                                        }
                                        decimal cometency = com.Count();
                                        decimal comscroeRatio = t/cometency;
                                        ap.comScore = comscroeRatio;
                                    }
                                }
                            }
                            ss.ApplicantList.Add(ap);
                            if (diff.Years != 0)
                            {
                                ap.CreateDate = diff.Years + " " +"a year ago";
                            }
                            else 
                            {
                                if (diff.Months != 0)
                                {
                                    ap.CreateDate = diff.Months +" "+ "Months ago";
                                }
                                else 
                                {
                                    if (diff.Days != 0)
                                    {
                                        ap.CreateDate = diff.Days + " " + "Days ago";
                                    }
                                    else 
                                    {
                                        if (diff.Hours != 0)
                                        {
                                            ap.CreateDate = diff.Hours + " " + "Hours ago";
                                        }
                                        else 
                                        {
                                            ap.CreateDate = diff.Minutes + " " + "Minutes ago";
                                        }
                                    }
                                }

                            }
                        }


                    }

                    datamodel.StepList.Add(ss);

                }
            }
            datamodel.Summary = model.Summary;
            
            //return Json(datamodel,JsonRequestBehavior.AllowGet);
            return PartialView("_partialApplicantListView", datamodel);
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
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("General Skills"))
            {
                datamodel.GeneralSkillsList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Technical Skills"))
            {
                datamodel.TechnicalSkillsList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            var data = _AdminTMSMethod.getVacancyDetailsById(VacancyId);

            TMSVacancyViewModel vacancyDetailsModel = new TMSVacancyViewModel();
            vacancyDetailsModel.Id = data.Id;
            vacancyDetailsModel.Title = data.Title;
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

            if (data.BusinessID != 0)
            {
                var buz = _CompanyStructureMethod.getBusinessListById((int)data.BusinessID);
                vacancyDetailsModel.Business = buz.Name;
            }
            if (data.DivisionID != 0)
            {
                var div = _CompanyStructureMethod.getDivisionById((int)data.DivisionID);
                vacancyDetailsModel.Division = div.Name;
            }
            if (data.PoolID != 0)
            {
                var pol = _CompanyStructureMethod.getPoolsListById((int)data.PoolID);
                vacancyDetailsModel.Pool = pol.Name;
            }
            if (data.FunctionID != 0)
            {
                var fun = _CompanyStructureMethod.getFunctionsListById((int)data.FunctionID);
                vacancyDetailsModel.Function = fun.Name;
            }

            if (data.RecruitmentProcessID != 0)
            {
                var res = _TMSSettingsMethod.getTMSSettingListById(data.RecruitmentProcessID);
                vacancyDetailsModel.RecruitmentProcess = res.Name;

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
                        var ApplicantList = _AdminTMSMethod.getApplicantListByStepId(ss.Id);
                        if (ApplicantList.Count > 0)
                        {
                            foreach (var item in ApplicantList)
                            {
                                TMSApplicantDetails ap = new TMSApplicantDetails();
                                ap.Id = item.Id;
                                ap.FirstName = item.FirstName;
                                ap.LastName = item.LastName;
                                ap.CreateDate = String.Format("{0:dd-MM-yyyy}", item.CreatedDate);
                                ss.ApplicantList.Add(ap);
                            }


                        }
                    }
                }
                var competency = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(res.CompetencyCSV);
                if (competency.Count > 0)
                {
                    foreach (var c in competency)
                    {
                        TMSSettingCompetencyDetails cc = new TMSSettingCompetencyDetails();
                        cc.Id = c.Id;
                        cc.CompetencyName = c.CompetencyName;
                        datamodel.CompatencyList.Add(cc);
                    }
                }

            }
            datamodel.VacancyDetails = vacancyDetailsModel;

            if (Id == 0)
            {

            }
            else
            {
                var ApplicantData = _AdminTMSMethod.getApplicantDetailsById(Id);
                datamodel.DateOfBirth = String.Format("{0:dd-MM-yyyy}", ApplicantData.DateOfBirth);
                var datas = _AdminTMSMethod.getVacancyDetailsById(ApplicantData.VacancyID);
                if (datas.RecruitmentProcessID != 0)
                {
                    var res = _TMSSettingsMethod.getTMSSettingListById(data.RecruitmentProcessID);
                    var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(res.StepCSV);
                    if (steps != null)
                    {
                        foreach (var s in steps)
                        {
                            TMSSettingStepDetails ss = new TMSSettingStepDetails();
                            //ss.Id = s.Id;
                            //ss.StepName = s.StepName;
                            //ss.ColorCode = s.ColorCode;
                            // datamodel.StatusList.Add(ss);
                            if (s.Id == datamodel.StatusID)
                            {
                                datamodel.Status = s.StepName;

                            }

                        }
                    }

                }
                var CommentList = _AdminTMSMethod.getApplicantCommentListById(ApplicantData.Id);
                var DocumentList = _AdminTMSMethod.getApplicantDocumentListById(ApplicantData.Id);
                if (CommentList.Count > 0)
                {
                    foreach (var item in CommentList)
                    {
                        TMSApplicantCommentViewModel mm = new TMSApplicantCommentViewModel();
                        mm.Id = item.Id;
                        mm.ApplicantID = item.ApplicantID;
                        mm.commentBy = item.CreatedName;
                        mm.commentTime = item.CreatedDateTime;
                        mm.comment = item.Description;
                        datamodel.CommentList.Add(mm);
                    }
                }
                if (DocumentList.Count > 0)
                {
                    foreach (var item in DocumentList)
                    {
                        TMSApplicantDocumentViewModel mm = new TMSApplicantDocumentViewModel();
                        mm.Id = item.Id;
                        mm.ApplicantID = item.ApplicantID;
                        mm.newName = item.NewName;
                        mm.originalName = item.OriginalName;
                        mm.description = item.Description;
                        datamodel.DocumentList.Add(mm);
                    }
                }
                datamodel.Id = ApplicantData.Id;
                datamodel.FirstName = ApplicantData.FirstName;
                datamodel.LastName = ApplicantData.LastName;
                datamodel.CreateDate = String.Format("{0:dd-MM-yyyy}", ApplicantData.CreatedDate);

            }
            return datamodel;
        }

        public ActionResult AddEditApplicant(int Id, int VacancyId)
        {
            TMSApplicantViewModel model = returnApplicantList(Id, VacancyId);
            return PartialView("_partialAddEditApplicantView", model);
        }

        public ActionResult SaveApplicant(TMSApplicantViewModel datamodel)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<TMSApplicantCommentViewModel> listComment = js.Deserialize<List<TMSApplicantCommentViewModel>>(datamodel.CommentJSV);
            List<TMSApplicantDocumentViewModel> listDocument = js.Deserialize<List<TMSApplicantDocumentViewModel>>(datamodel.DocumentJSV);
            bool save = _AdminTMSMethod.SaveApplicantData(datamodel, listComment, listDocument, SessionProxy.UserId);
            if (save)
            {
                if (datamodel.StatusID == 1)
                {
                    
                    HRTool.Models.MailModel mail = new HRTool.Models.MailModel();
                    mail.From = datamodel.Email;
                    mail.To = datamodel.Email;
                    if (datamodel.RejectReasonId != 0 && datamodel.RejectReasonId!=null) 
                    {
                        //var rejectResonName = _db.ApplicantRejectReasons.Where(x => x.Id == datamodel.RejectReasonId && x.Archived == false).FirstOrDefault();
                        //mail.Subject = rejectResonName.Name;
                    }
                    else
                    {
                        mail.Subject = "Application Reject";
                    }
                    string inputFormat = "ddd, dd MMM yyyy";
                    string dateTimeEndorse = DateTime.Now.ToString("ddd, dd MMM yyyy");
                    mail.Body = datamodel.RejectReasonComment;
                    string mailFromReceive = Common.sendMail(mail);
                }               
                TMSVacancyDetailsViewModel model = returnVacancyList();
                if (datamodel.flagEdit == 1)
                {
                    List<TMSApplicantListViewModel> emodel = returnApplicantList();
                    return PartialView("_partialPoolListView", emodel);

                }
                else
                {
                    return PartialView("_partialApplicantListView", model);
                }
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);

            }

        }

        public ActionResult Emailcheck(string Email, int Id, int VacancyID)
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

        public ActionResult DeleteApplicant(int Id) 
        {
            bool delete = _AdminTMSMethod.DeleteApplicant(Id, SessionProxy.UserId);

            if (delete)
            {
                TMSVacancyDetailsViewModel model = returnVacancyList();
                return PartialView("_partialApplicantListView", model);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult DeleteApplicantRecord(int Id)
        {
            bool delete = _AdminTMSMethod.DeleteApplicant(Id, SessionProxy.UserId);
            List<TMSApplicantListViewModel> model = returnApplicantList();
            return PartialView("_partialPoolListView", model);
        }


        public ActionResult MoveToStep(ApplicantStepMoveViewModel MODEL) 
        {
            var AccepteID=_AdminTMSMethod.GetAcceptedStepId(MODEL.ApplicantID);
            if (AccepteID == MODEL.StepID)
            {
                //MainResoureViewModel model = AccepteApplicantasEmployee(MODEL.ApplicantID);
               // return 
                //return PartialView("~/Views/Employee/_PartialAddResoureSet", model);
                return Json(new
                {
                    redirectUrl = RedirectToAction("AddApplicantAsResource", "Employee", new { Id = MODEL.ApplicantID, VacancyID = MODEL.VacancyID }),
                    isRedirect = true
                });
            }
            else
            {
            bool save = _AdminTMSMethod.UpdateStepMoveDetails(MODEL);
            if (save)
            {
                TMSVacancyDetailsViewModel datamodel = new TMSVacancyDetailsViewModel();
                TMSVacancyDetailsViewModel modelss = returnVacancyList();
                datamodel.VacancyList = modelss.VacancyList;
                var model = _AdminTMSMethod.getVacancyDetailsById(MODEL.VacancyID);
                datamodel.Id = model.Id;
                datamodel.Title = model.Title;
                datamodel.ClosingDate = String.Format("{0:dd-MM-yyyy}", model.ClosingDate);
                datamodel.RecruitmentProcessID = model.RecruitmentProcessID;
                    decimal totalComByVacancy = 0;
                    if (datamodel.RecruitmentProcessID != 0)
                {
                    var res = _TMSSettingsMethod.getTMSSettingListById(datamodel.RecruitmentProcessID);
                    datamodel.RecruitmentProcess = res.Name;
                    var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(res.StepCSV);
                    foreach (var s in steps)
                    {
                        TMSSettingStepDetails ss = new TMSSettingStepDetails();
                        ss.Id = s.Id;
                        ss.StepName = s.StepName;
                        ss.ColorCode = s.ColorCode;
                        var applicant = _AdminTMSMethod.getApplicantListByStepIdVacancyID(ss.Id, model.Id);
                        if (applicant.Count > 0)
                        {
                            foreach (var item in applicant)
                            {
                                TMSApplicantDetails ap = new TMSApplicantDetails();
                                ap.Id = item.Id;
                                ap.FirstName = item.FirstName;
                                ap.LastName = item.LastName;
                                    if (item.CompatencyJSV != null && item.CompatencyJSV != "")
                                    {
                                        var listDatas = _TMSSettingsMethod.getTMSSettingListById(model.RecruitmentProcessID);
                                        if (listDatas.StepCSV != null)
                                        {
                                            var competency = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(listDatas.CompetencyCSV);
                                            totalComByVacancy = competency.Count();
                                        }
                                        var com = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(item.CompatencyJSV);

                                        if (com.Count > 0 && com != null)
                                        {
                                            int t = 0;                                           
                                                foreach (var comData in com)
                                                {
                                                    t = t + Convert.ToInt32(comData.Score);
                                                }
                                                decimal cometency = com.Count();
                                                decimal comscroeRatio = t / cometency;
                                                ap.comScore = comscroeRatio;                                            
                                        }
                                    }
                                    var diff = DateTimeSpan.CompareDates((DateTime)item.CreatedDate, DateTime.Now);
                                if (diff.Years != 0)
                                {
                                    ap.CreateDate = diff.Years + " " + "a year ago";
                                }
                                else
                                {
                                    if (diff.Months != 0)
                                    {
                                        ap.CreateDate = diff.Months + " " + "Months ago";
                                    }
                                    else
                                    {
                                        if (diff.Days != 0)
                                        {
                                            ap.CreateDate = diff.Days + " " + "Days ago";
                                        }
                                        else
                                        {
                                            if (diff.Hours != 0)
                                            {
                                                ap.CreateDate = diff.Hours + " " + "Hours ago";
                                            }
                                            else
                                            {
                                                ap.CreateDate = diff.Minutes + " " + "Minutes ago";
                                            }
                                        }
                                    }

                                }
                                ss.ApplicantList.Add(ap);
                            }
                        }

                        datamodel.StepList.Add(ss);
                    }
                }
                datamodel.Summary = model.Summary;
                datamodel.StatusID = model.StatusID;
                if (datamodel.StatusID != 0)
                {
                    var st = _otherSettingMethod.getSystemListValueById((int)datamodel.StatusID);
                    datamodel.Status = st.Value;
                }
                //return Json(datamodel,JsonRequestBehavior.AllowGet);
                return PartialView("_partialApplicantListView", datamodel);

            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);

            }
            }
        }

        public ActionResult MoveAllApplicantToReject(ApplicantStepMoveViewModel MODEL) 
        {
            bool save = _AdminTMSMethod.AllStepMoveReject(MODEL.StepID);
            if (save)
            {
                TMSVacancyDetailsViewModel datamodel = new TMSVacancyDetailsViewModel();
                TMSVacancyDetailsViewModel modelss = returnVacancyList();
                datamodel.VacancyList = modelss.VacancyList;
                var model = _AdminTMSMethod.getVacancyDetailsById(MODEL.VacancyID);
                datamodel.Id = model.Id;
                datamodel.Title = model.Title;
                datamodel.ClosingDate = String.Format("{0:dd-MM-yyyy}", model.ClosingDate);
                datamodel.RecruitmentProcessID = model.RecruitmentProcessID;
                decimal totalComByVacancy = 0;

                if (datamodel.RecruitmentProcessID != 0)
                {
                    var res = _TMSSettingsMethod.getTMSSettingListById(datamodel.RecruitmentProcessID);
                    datamodel.RecruitmentProcess = res.Name;
                    var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(res.StepCSV);
                    foreach (var s in steps)
                    {
                        TMSSettingStepDetails ss = new TMSSettingStepDetails();
                        ss.Id = s.Id;
                        ss.StepName = s.StepName;
                        ss.ColorCode = s.ColorCode;
                        var applicant = _AdminTMSMethod.getApplicantListByStepIdVacancyID(ss.Id, model.Id);                        
                        if (applicant.Count > 0)
                        {
                            foreach (var item in applicant)
                            {
                                
                                TMSApplicantDetails ap = new TMSApplicantDetails();
                                ap.Id = item.Id;
                                ap.FirstName = item.FirstName;
                                ap.LastName = item.LastName;
                                if (item.CompatencyJSV != null && item.CompatencyJSV != "")
                                {
                                    var listDatas = _TMSSettingsMethod.getTMSSettingListById(model.RecruitmentProcessID);
                                    if (listDatas.StepCSV != null)
                                    {
                                        var competency = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(listDatas.CompetencyCSV);
                                        totalComByVacancy = competency.Count();
                                    }
                                    var com = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(item.CompatencyJSV);
                                    if (com.Count > 0 && com != null)
                                    {
                                        int t = 0;
                                        foreach (var comData in com)
                                        {
                                            t = t + Convert.ToInt32(comData.Score);
                                        }
                                        decimal cometency = com.Count();
                                        decimal comscroeRatio = t / cometency;
                                        ap.comScore = comscroeRatio;

                                    }
                                }
                                var diff = DateTimeSpan.CompareDates((DateTime)item.CreatedDate, DateTime.Now);

                                if (diff.Years != 0)
                                {
                                    ap.CreateDate = diff.Years + " " + "a year ago";
                                }
                                else
                                {
                                    if (diff.Months != 0)
                                    {
                                        ap.CreateDate = diff.Months + " " + "Months ago";
                                    }
                                    else
                                    {
                                        if (diff.Days != 0)
                                        {
                                            ap.CreateDate = diff.Days + " " + "Days ago";
                                        }
                                        else
                                        {
                                            if (diff.Hours != 0)
                                            {
                                                ap.CreateDate = diff.Hours + " " + "Hours ago";
                                            }
                                            else
                                            {
                                                ap.CreateDate = diff.Minutes + " " + "Minutes ago";
                                            }
                                        }
                                    }

                                }
                                ss.ApplicantList.Add(ap);
                            }
                        }

                        datamodel.StepList.Add(ss);

                    }
                }
                datamodel.Summary = model.Summary;
                datamodel.StatusID = model.StatusID;
                if (datamodel.StatusID != 0)
                {
                    var st = _otherSettingMethod.getSystemListValueById((int)datamodel.StatusID);
                    datamodel.Status = st.Value;
                }
                //return Json(datamodel,JsonRequestBehavior.AllowGet);
                return PartialView("_partialApplicantListView", datamodel);

            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);

            }

        }

        public ActionResult MoveAllApplicantToTelent(ApplicantStepMoveViewModel MODEL)
        {
            bool save = _AdminTMSMethod.AllStepMoveTalent(MODEL.StepID);
            if (save)
            {
                TMSVacancyDetailsViewModel datamodel = new TMSVacancyDetailsViewModel();
                TMSVacancyDetailsViewModel modelss = returnVacancyList();
                datamodel.VacancyList = modelss.VacancyList;
                var model = _AdminTMSMethod.getVacancyDetailsById(MODEL.VacancyID);
                datamodel.Id = model.Id;
                datamodel.Title = model.Title;
                datamodel.ClosingDate = String.Format("{0:dd-MM-yyyy}", model.ClosingDate);
                datamodel.RecruitmentProcessID = model.RecruitmentProcessID;
                decimal totalComByVacancy = 0;
                if (datamodel.RecruitmentProcessID != 0)
                {
                    var res = _TMSSettingsMethod.getTMSSettingListById(datamodel.RecruitmentProcessID);
                    datamodel.RecruitmentProcess = res.Name;
                    var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(res.StepCSV);
                    foreach (var s in steps)
                    {
                        TMSSettingStepDetails ss = new TMSSettingStepDetails();
                        ss.Id = s.Id;
                        ss.StepName = s.StepName;
                        ss.ColorCode = s.ColorCode;
                        var applicant = _AdminTMSMethod.getApplicantListByStepIdVacancyID(ss.Id, model.Id);
                        if (applicant.Count > 0)
                        {
                            foreach (var item in applicant)
                            {
                                TMSApplicantDetails ap = new TMSApplicantDetails();
                                ap.Id = item.Id;
                                ap.FirstName = item.FirstName;
                                ap.LastName = item.LastName;
                                if (item.CompatencyJSV != null && item.CompatencyJSV != "")
                                {
                                    var listDatas = _TMSSettingsMethod.getTMSSettingListById(model.RecruitmentProcessID);
                                    if (listDatas.StepCSV != null)
                                    {
                                        var competency = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(listDatas.CompetencyCSV);
                                        totalComByVacancy = competency.Count();
                                    }
                                    var com = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(item.CompatencyJSV);
                                    if (com.Count > 0 && com != null)
                                    {
                                        int t = 0;
                                        foreach (var comData in com)
                                        {
                                            t = t + Convert.ToInt32(comData.Score);
                                        }
                                        decimal cometency = com.Count();
                                        decimal comscroeRatio = t / cometency;
                                        ap.comScore = comscroeRatio;

                                    }
                                }
                                var diff = DateTimeSpan.CompareDates((DateTime)item.CreatedDate, DateTime.Now);
                                if (diff.Years != 0)
                                {
                                    ap.CreateDate = diff.Years + " " + "a year ago";
                                }
                                else
                                {
                                    if (diff.Months != 0)
                                    {
                                        ap.CreateDate = diff.Months + " " + "Months ago";
                                    }
                                    else
                                    {
                                        if (diff.Days != 0)
                                        {
                                            ap.CreateDate = diff.Days + " " + "Days ago";
                                        }
                                        else
                                        {
                                            if (diff.Hours != 0)
                                            {
                                                ap.CreateDate = diff.Hours + " " + "Hours ago";
                                            }
                                            else
                                            {
                                                ap.CreateDate = diff.Minutes + " " + "Minutes ago";
                                            }
                                        }
                                    }

                                }
                                ss.ApplicantList.Add(ap);
                            }


                        }

                        datamodel.StepList.Add(ss);

                    }
                }
                datamodel.Summary = model.Summary;
                datamodel.StatusID = model.StatusID;
                if (datamodel.StatusID != 0)
                {
                    var st = _otherSettingMethod.getSystemListValueById((int)datamodel.StatusID);
                    datamodel.Status = st.Value;
                }
                //return Json(datamodel,JsonRequestBehavior.AllowGet);
                return PartialView("_partialApplicantListView", datamodel);

            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);

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

       

        #endregion

        #region Pool

        public ActionResult getRejectionReasonList()
        {
            TMSApplicantViewModel datamodel = new TMSApplicantViewModel();

            foreach (var item in _otherSettingMethod.getApplicantRejectReason())
            {
                datamodel.RejectionReasonList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            return Json(datamodel.RejectionReasonList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddAppliacantRejectReason()
        {
            return PartialView("_partialAddNewApplicatRejectReason");
        }
        public ActionResult AddRejectReason(string Name)
        {
            bool success=_AdminTMSMethod.AddRejectReasonApplicant(Name);
            return PartialView("_partialAddNewApplicatRejectReason");
        }
        public List<TMSApplicantListViewModel> returnApplicantList()
        {
            List<TMSApplicantListViewModel> DataModel = new List<TMSApplicantListViewModel>();
            TMSApplicantListViewModel aataModel = new TMSApplicantListViewModel();
            //foreach (var item in _TMSSettingsMethod.getAllTMSSettingList())
            //{
            //    aataModel.RecruitmentProcessList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            //}
            //DataModel.Add(aataModel.RecruitmentProcessList);
            
            var AllList = _AdminTMSMethod.getAllApplicantList();
            decimal totalComByVacancy = 0;
            if (AllList.Count > 0)
            {
                foreach (var item in AllList)
                {
                    TMSApplicantListViewModel datamodel = new TMSApplicantListViewModel();
                    var data = _AdminTMSMethod.getApplicantDetailsById(item.Id);
                    datamodel.Id = data.Id;
                    datamodel.FirstName = data.FirstName;
                    datamodel.LastName = data.LastName;
                    datamodel.CreateDate = String.Format("{0:dd-MM-yyyy}", data.CreatedDate);
                    var vacancyDetails = _AdminTMSMethod.getVacancyDetailsById(data.VacancyID);
                    datamodel.VacancyName = vacancyDetails.Title;
                    if (vacancyDetails.RecruitmentProcessID != 0)
                    {
                        var res = _TMSSettingsMethod.getTMSSettingListById(vacancyDetails.RecruitmentProcessID);
                        datamodel.RecruitmentProcess = res.Name;
                        var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(res.StepCSV);
                        if (steps != null)
                        {
                            foreach (var s in steps)
                            {
                                TMSSettingStepDetails ss = new TMSSettingStepDetails();
                                if (data.StatusID == s.Id)
                                {
                                    datamodel.Status = s.StepName;

                                }

                            }
                        }
                        if (item.CompatencyJSV != null && item.CompatencyJSV != "")
                        {
                            var listDatas = _TMSSettingsMethod.getTMSSettingListById(vacancyDetails.RecruitmentProcessID);
                            if (listDatas.StepCSV != null)
                            {
                                var competency = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(listDatas.CompetencyCSV);
                                totalComByVacancy = competency.Count();
                            }
                            var com = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(item.CompatencyJSV);
                            if (com.Count > 0 && com != null)
                            {
                                int t = 0;
                                foreach (var comData in com)
                                {
                                    t = t + Convert.ToInt32(comData.Score);
                                }
                                decimal cometency = com.Count();
                                decimal comscroeRatio = t / cometency;
                                datamodel.comScore = comscroeRatio;
                            }
                        }
                    }
                    if (vacancyDetails.BusinessID != 0)
                    {
                        var buz = _CompanyStructureMethod.getBusinessListById((int)vacancyDetails.BusinessID);
                        datamodel.Business = buz.Name;
                    }
                    if (vacancyDetails.DivisionID != 0)
                    {
                        var div = _CompanyStructureMethod.getDivisionById((int)vacancyDetails.DivisionID);
                        datamodel.Division = div.Name;
                    }
                    if (vacancyDetails.PoolID != 0)
                    {
                        var pol = _CompanyStructureMethod.getPoolsListById((int)vacancyDetails.PoolID);
                        datamodel.Pool = pol.Name;
                    }
                    if (vacancyDetails.FunctionID != 0)
                    {
                        var fun = _CompanyStructureMethod.getFunctionsListById((int)vacancyDetails.FunctionID);
                        datamodel.Function = fun.Name;
                    }
                   
                    DataModel.Add(datamodel);
                    
                }

                foreach (var item in _TMSSettingsMethod.getAllTMSSettingList())
                {
                    DataModel.FirstOrDefault().RecruitmentProcessList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
                }

            }
            return DataModel;
        }
        public ActionResult PoolList()
        {
            List<TMSApplicantListViewModel> model = returnApplicantList();
            return PartialView("_partialPoolListView", model);
        }

        public TMSApplicantViewModel returnEditApplicantList(int Id)
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
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("General Skills"))
            {
                datamodel.GeneralSkillsList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Technical Skills"))
            {
                datamodel.TechnicalSkillsList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }

            var ApplicantData = _AdminTMSMethod.getApplicantDetailsById(Id);
            var data = _AdminTMSMethod.getVacancyDetailsById(ApplicantData.VacancyID);
            datamodel.GenderID = ApplicantData.GenderID;
            datamodel.DateOfBirth = String.Format("{0:dd-MM-yyyy}",ApplicantData.DateOfBirth);
            TMSVacancyViewModel vacancyDetailsModel = new TMSVacancyViewModel();
            vacancyDetailsModel.Id = data.Id;
            vacancyDetailsModel.Title = data.Title;
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

            if (data.BusinessID != 0)
            {
                var buz = _CompanyStructureMethod.getBusinessListById((int)data.BusinessID);
                vacancyDetailsModel.Business = buz.Name;
            }
            if (data.DivisionID != 0)
            {
                var div = _CompanyStructureMethod.getDivisionById((int)data.DivisionID);
                vacancyDetailsModel.Division = div.Name;
            }
            if (data.PoolID != 0)
            {
                var pol = _CompanyStructureMethod.getPoolsListById((int)data.PoolID);
                vacancyDetailsModel.Pool = pol.Name;
            }
            if (data.FunctionID != 0)
            {
                var fun = _CompanyStructureMethod.getFunctionsListById((int)data.FunctionID);
                vacancyDetailsModel.Function = fun.Name;
            }

            if (data.RecruitmentProcessID != 0)
            {
                var res = _TMSSettingsMethod.getTMSSettingListById(data.RecruitmentProcessID);
                vacancyDetailsModel.RecruitmentProcess = res.Name;

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
                        var ApplicantList = _AdminTMSMethod.getApplicantListByStepId(ss.Id);
                        if (ApplicantList.Count > 0)
                        {
                            foreach (var item in ApplicantList)
                            {
                                TMSApplicantDetails ap = new TMSApplicantDetails();
                                ap.Id = item.Id;
                                ap.FirstName = item.FirstName;
                                ap.LastName = item.LastName;
                                ap.CreateDate = String.Format("{0:dd-MM-yyyy}", item.CreatedDate);
                                ss.ApplicantList.Add(ap);
                            }


                        }
                    }
                }


            }
            var datas = _AdminTMSMethod.getVacancyDetailsById(ApplicantData.VacancyID);
            if (datas.RecruitmentProcessID != 0)
            {
                var res = _TMSSettingsMethod.getTMSSettingListById(data.RecruitmentProcessID);
                var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(res.StepCSV);
                if (steps != null)
                {
                    foreach (var s in steps)
                    {
                        TMSSettingStepDetails ss = new TMSSettingStepDetails();
                        ss.Id = s.Id;
                        ss.StepName = s.StepName;
                        ss.ColorCode = s.ColorCode;
                        //datamodel.StatusList.Add(ss);
                        if (s.Id == ApplicantData.StatusID)
                        {
                            datamodel.StatusID = s.Id;
                            datamodel.Status = s.StepName;

                        }
                    }
                }
                if (!string.IsNullOrEmpty(ApplicantData.CompatencyJSV) && ApplicantData.CompatencyJSV.Length != 0)
                {
                    var com = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(ApplicantData.CompatencyJSV);
                    if (com.Count > 0)
                    {
                        foreach (var c in com)
                        {
                            TMSSettingCompetencyDetails cc = new TMSSettingCompetencyDetails();
                            cc.Id = c.Id;
                            cc.CompetencyName = c.CompetencyName;
                            cc.Score = c.Score;
                            datamodel.CompatencyList.Add(cc);
                        }
                    }
                }
                if(datamodel.CompatencyList.Count==0)
                { 
                    var competency = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(res.CompetencyCSV);
                    if (competency.Count > 0)
                    {
                        foreach (var c in competency)
                        {
                            TMSSettingCompetencyDetails cc = new TMSSettingCompetencyDetails();
                            cc.Id = c.Id;
                            cc.CompetencyName = c.CompetencyName;
                            datamodel.CompatencyList.Add(cc);
                        }
                    }
                }

            }
            var CommentList = _AdminTMSMethod.getApplicantCommentListById(ApplicantData.Id);
            var DocumentList = _AdminTMSMethod.getApplicantDocumentListById(ApplicantData.Id);
            if (CommentList.Count > 0)
            {
                foreach (var item in CommentList)
                {
                    TMSApplicantCommentViewModel mm = new TMSApplicantCommentViewModel();
                    mm.Id = item.Id;
                    mm.ApplicantID = item.ApplicantID;
                    mm.commentBy = item.CreatedName;
                    mm.commentTime = item.CreatedDateTime;
                    mm.comment = item.Description;
                    datamodel.CommentList.Add(mm);
                }
            }
            if (DocumentList.Count > 0)
            {
                foreach (var item in DocumentList)
                {
                    TMSApplicantDocumentViewModel mm = new TMSApplicantDocumentViewModel();
                    mm.Id = item.Id;
                    mm.ApplicantID = item.ApplicantID;
                    mm.newName = item.NewName;
                    mm.originalName = item.OriginalName;
                    mm.description = item.Description;
                    datamodel.DocumentList.Add(mm);
                }
            }
            if (ApplicantData.GeneralSkillsJSV != null)
            {
                datamodel.SelectGeneralSkiils = ApplicantData.GeneralSkillsJSV.Split(',').ToList();
            }
            if (ApplicantData.TechnicalSkillsJSV != null)
            {
                datamodel.SelectTechnicalSkills = ApplicantData.TechnicalSkillsJSV.Split(',').ToList();
            }
            datamodel.Id = ApplicantData.Id;
            datamodel.FirstName = ApplicantData.FirstName;
            datamodel.LastName = ApplicantData.LastName;
            datamodel.CreateDate = String.Format("{0:dd-MM-yyyy}", ApplicantData.CreatedDate);
            datamodel.Email = ApplicantData.Email;
            datamodel.PostalCode = ApplicantData.PostalCode;
            datamodel.Address = ApplicantData.Address;
            datamodel.OtherContactDetails = ApplicantData.OtherContactDetails;
            datamodel.CoverLetterPath = ApplicantData.CoverLetterPath;
            datamodel.CoverLetterPathOriginal = ApplicantData.CoverLetterPathOriginal;
            datamodel.DownloadApplicationFormLink = ApplicantData.DownloadApplicationFormLink;
            datamodel.UploadApplicationFormPathOriginal = ApplicantData.UploadApplicationFormPathOriginal;
            datamodel.UploadApplicationFormPath = ApplicantData.UploadApplicationFormPath;
            datamodel.ResumePath = ApplicantData.ResumePath;
            datamodel.ResumePathOriginal = ApplicantData.ResumePathOriginal;
            datamodel.Question1Answer = ApplicantData.Question1Answer;
            datamodel.Question2Answer = ApplicantData.Question2Answer;
            datamodel.Question3Answer = ApplicantData.Question3Answer;
            datamodel.Question4Answer = ApplicantData.Question4Answer;
            datamodel.Question5Answer = ApplicantData.Question5Answer;
            
            

            datamodel.Cost = ApplicantData.Cost;

            datamodel.VacancyDetails = vacancyDetailsModel;

            return datamodel;
        }
        public ActionResult genaratePDF(int appId, int vacID, string CompJSV)
        {
            try
            {
                //int ApplicantID, int VacancyID
                ApplicantCompetencyPDfViewModel model = new ApplicantCompetencyPDfViewModel();
                var ApplicantDetails = _AdminTMSMethod.getApplicantDetailsById(appId);
                model.ApplicantId = ApplicantDetails.Id;
                model.VacancyId = vacID;
                model.VacancyName = _db.Vacancies.Where(x => x.Archived == false && x.Id == vacID).FirstOrDefault().Title;
                model.FirstName = ApplicantDetails.FirstName;
                model.LastName = ApplicantDetails.LastName;
                DateTime currentDate = DateTime.Now;
                //if (ApplicantDetails.CompatencyJSV != null && ApplicantDetails.CompatencyJSV != "")
                if (CompJSV != null && CompJSV != "")
                {
                    var com = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(CompJSV);
                    int total = 0;
                    if (com.Count > 0)
                    {
                        foreach (var c in com)
                        {
                            TMSSettingCompetencyDetails cc = new TMSSettingCompetencyDetails();
                            cc.Id = c.Id;
                            cc.CompetencyName = c.CompetencyName;
                            cc.Score = c.Score;
                            total = total + Convert.ToInt32(cc.Score);
                            model.CompatencyJSV.Add(cc);
                        }
                        int totalCom = model.CompatencyJSV.Count();
                        decimal avg = total / totalCom;
                        model.totalAvg = Convert.ToString(avg);
                    }


                }
                string newfileName = string.Format("" + model.FirstName + "_" + model.LastName + "_Competencies.pdf", currentDate.Date);
                return new Rotativa.ViewAsPdf("PrintCompetecenciesPDf", model)
                {
                    PageSize = Size.A4,
                    PageOrientation = Orientation.Landscape,
                    FileName = newfileName
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ActionResult EditApplicant(int Id)
        {
            TMSApplicantViewModel model = returnEditApplicantList(Id);
            return PartialView("_partialAddEditApplicantView", model);
        }
        public ActionResult QuickFilterByTalentPool(int seleProcessId, string seleId)
        {
            List<TMSApplicantListViewModel> DataModel = new List<TMSApplicantListViewModel>();
            decimal totalComByVacancy=0;
            //if (seleProcessId == 0 || seleId=="")
            //{
            //    DataModel = returnApplicantList();
            //}
           if (seleProcessId != 0 || seleId != "") 
            {
                //if (seleId == "" || seleId == null || seleId=="0")
                //{
                //    var AllList = _AdminTMSMethod.getAllApplicantList();
                //    if (AllList.Count > 0)
                //    {
                //        foreach (var item in AllList)
                //        {
                //            TMSApplicantListViewModel datamodel = new TMSApplicantListViewModel();
                //            var data = _AdminTMSMethod.getApplicantDetailsById(item.Id);
                //            datamodel.Id = data.Id;
                //            datamodel.FirstName = data.FirstName;
                //            datamodel.LastName = data.LastName;
                //            datamodel.CreateDate = String.Format("{0:dd-MM-yyyy}", data.CreatedDate);
                //            var vacancyDetails = _AdminTMSMethod.getVacancyDetailsById(data.VacancyID);
                //            datamodel.VacancyName = vacancyDetails.Title;
                //            if (vacancyDetails.RecruitmentProcessID != 0 && vacancyDetails.RecruitmentProcessID== seleProcessId)
                //            {
                //                var res = _TMSSettingsMethod.getTMSSettingListById(vacancyDetails.RecruitmentProcessID);
                //                datamodel.RecruitmentProcess = res.Name;
                //                var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(res.StepCSV);
                //                if (steps != null && steps.Count>0)
                //                {
                //                    foreach (var s in steps)
                //                    {
                //                        TMSSettingStepDetails ss = new TMSSettingStepDetails();
                //                        if (data.StatusID == s.Id)
                //                        {
                //                            datamodel.Status = s.StepName;
                //                        }
                //                    }
                //                }
                //            }
                //            if (item.CompatencyJSV != null && item.CompatencyJSV != "")
                //            {
                //                var listDatas = _TMSSettingsMethod.getTMSSettingListById(seleProcessId);
                //                if (listDatas.StepCSV != null)
                //                {
                //                    var competency = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(listDatas.CompetencyCSV);
                //                    totalComByVacancy = competency.Count();
                //                }                            
                //                var com = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(item.CompatencyJSV);
                //                if (com.Count > 0 && com!=null)
                //                {
                //                    decimal cometency = com.Count();
                //                    decimal comscroeRatio = Math.Round(totalComByVacancy) / cometency;

                //                    datamodel.comScore = comscroeRatio;

                //               }
                //            }
                //            if (vacancyDetails.BusinessID != 0)
                //            {
                //                var buz = _CompanyStructureMethod.getBusinessListById((int)vacancyDetails.BusinessID);
                //                datamodel.Business = buz.Name;
                //            }
                //            if (vacancyDetails.DivisionID != 0)
                //            {
                //                var div = _CompanyStructureMethod.getDivisionById((int)vacancyDetails.DivisionID);
                //                datamodel.Division = div.Name;
                //            }
                //            if (vacancyDetails.PoolID != 0)
                //            {
                //                var pol = _CompanyStructureMethod.getPoolsListById((int)vacancyDetails.PoolID);
                //                datamodel.Pool = pol.Name;
                //            }
                //            if (vacancyDetails.FunctionID != 0)
                //            {
                //                var fun = _CompanyStructureMethod.getFunctionsListById((int)vacancyDetails.FunctionID);
                //                datamodel.Function = fun.Name;
                //            }

                //            DataModel.Add(datamodel);
                //        }
                //    }
                //    if (DataModel.Count() == 0)
                //    {
                //        DataModel.Add(new TMSApplicantListViewModel());
                //    }
                //    foreach (var tadata in _TMSSettingsMethod.getAllTMSSettingList())
                //    {
                //        DataModel.FirstOrDefault().SelectedListId = seleProcessId;
                //        DataModel.FirstOrDefault().RecruitmentProcessList.Add(new SelectListItem() { Text = tadata.Name, Value = tadata.Id.ToString() });
                //    }
                //}
                //else
                //{
                if (seleId == "" || seleId == "0" && seleProcessId!=0)
                {

                }
                else
                {
                    string[] appId = seleId.Split(',');
                    if (appId != null)
                    {
                        for (int i = 0; i < appId.Length; i++)
                        {
                            int eid = Convert.ToInt32(appId[i]);
                            var AllList = _AdminTMSMethod.getAllApplicantList().Where(x => x.Id == eid && x.Archived == false).FirstOrDefault();
                            if (AllList != null)
                            {
                                var data = _AdminTMSMethod.getApplicantDetailsById(AllList.Id);
                                TMSApplicantListViewModel datamodel = new TMSApplicantListViewModel();
                                var vacancyDetails = _AdminTMSMethod.getVacancyDetailsById(data.VacancyID);
                                if (vacancyDetails.RecruitmentProcessID != 0 && vacancyDetails.RecruitmentProcessID == seleProcessId)
                                {
                                    datamodel.Id = data.Id;

                                    datamodel.FirstName = data.FirstName;
                                    datamodel.LastName = data.LastName;
                                    datamodel.CreateDate = String.Format("{0:dd-MM-yyyy}", data.CreatedDate);
                                    datamodel.VacancyName = vacancyDetails.Title;
                                    var res = _TMSSettingsMethod.getTMSSettingListById(vacancyDetails.RecruitmentProcessID);
                                    datamodel.RecruitmentProcess = res.Name;
                                    var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(res.StepCSV);
                                    if (steps != null)
                                    {
                                        foreach (var s in steps)
                                        {
                                            TMSSettingStepDetails ss = new TMSSettingStepDetails();
                                            if (data.StatusID == s.Id)
                                            {
                                                datamodel.Status = s.StepName;

                                            }

                                        }
                                    }
                                    if (data.CompatencyJSV != null && data.CompatencyJSV != "")
                                    {
                                        var listDatas = _TMSSettingsMethod.getTMSSettingListById(vacancyDetails.RecruitmentProcessID);
                                        if (listDatas.StepCSV != null)
                                        {
                                            var competency = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(listDatas.CompetencyCSV);
                                            totalComByVacancy = competency.Count();
                                        }
                                        var com = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(data.CompatencyJSV);
                                        if (com.Count > 0 && com != null)
                                        {
                                            int t = 0;
                                            foreach (var comData in com)
                                            {
                                                t = t + Convert.ToInt32(comData.Score);
                                            }
                                            decimal cometency = com.Count();
                                            decimal comscroeRatio = t / cometency;
                                            datamodel.comScore = comscroeRatio;
                                        }
                                    }
                                    //if (data.CompatencyJSV != null && data.CompatencyJSV != "")
                                    //{
                                    //    var listDatas = _TMSSettingsMethod.getTMSSettingListById(seleProcessId);
                                    //    if (listDatas.StepCSV != null)
                                    //    {
                                    //        var competency = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(listDatas.CompetencyCSV);
                                    //        totalComByVacancy = competency.Count();
                                    //    }
                                    //    var com = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(data.CompatencyJSV);
                                    //    if (com.Count > 0 && com != null)
                                    //    {
                                    //        int t = 0;
                                    //        foreach (var comData in com)
                                    //        {
                                    //            t = t + Convert.ToInt32(comData.Score);
                                    //        }
                                    //        decimal cometency = com.Count();
                                    //        decimal comscroeRatio = t / cometency;
                                    //    }
                                    //    else
                                    //    {
                                    //        datamodel.comScore = 0;
                                    //    }
                                    //}
                                    if (vacancyDetails.BusinessID != 0)
                                    {
                                        var buz = _CompanyStructureMethod.getBusinessListById((int)vacancyDetails.BusinessID);
                                        datamodel.Business = buz.Name;
                                    }
                                    if (vacancyDetails.DivisionID != 0)
                                    {
                                        var div = _CompanyStructureMethod.getDivisionById((int)vacancyDetails.DivisionID);
                                        datamodel.Division = div.Name;
                                    }
                                    if (vacancyDetails.PoolID != 0)
                                    {
                                        var pol = _CompanyStructureMethod.getPoolsListById((int)vacancyDetails.PoolID);
                                        datamodel.Pool = pol.Name;
                                    }
                                    if (vacancyDetails.FunctionID != 0)
                                    {
                                        var fun = _CompanyStructureMethod.getFunctionsListById((int)vacancyDetails.FunctionID);
                                        datamodel.Function = fun.Name;
                                    }
                                    DataModel.Add(datamodel);
                                }
                            }
                        }
                    }
                }                
                if (DataModel.Count() == 0)
                    {
                        DataModel.Add(new TMSApplicantListViewModel());
                    }
                    foreach (var tadata in _TMSSettingsMethod.getAllTMSSettingList())
                    {
                        DataModel.FirstOrDefault().SelectedListId = seleProcessId;
                        DataModel.FirstOrDefault().RecruitmentProcessList.Add(new SelectListItem() { Text = tadata.Name, Value = tadata.Id.ToString() });
                    }
                //}
            }
            else
            {
                DataModel = returnApplicantList();
            }
            return PartialView("_partialPoolListView", DataModel);
        }
    }
        #endregion
    }