using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HRTool.CommanMethods.Admin;
using System.Web.Script.Serialization;
using HRTool.CommanMethods;
using System.IO;
using System.Globalization;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class PerformanceSettingController : Controller
    {

        #region
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        CompanyStructureMethod _CompanyStructureMethod = new CompanyStructureMethod();
        AdminPearformanceMethod _AdminPearformanceMethod = new AdminPearformanceMethod();

        #endregion
        //
        EvolutionEntities _db = new EvolutionEntities();
        // GET: /PerformanceSetting/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            List<PerformanceSettingViewModel> modelList = returnList();
            return PartialView("_partialPerformanceReviewList", modelList);
        }

        public List<PerformanceSettingViewModel> returnList()
        {
            List<PerformanceSetting> data = _AdminPearformanceMethod.getAllList().ToList();
            List<PerformanceSettingViewModel> model = new List<PerformanceSettingViewModel>();
            foreach (var item in data)
            {
                PerformanceSettingViewModel Performance = new PerformanceSettingViewModel();
                Performance.Id = item.Id;
                Performance.ReviewText = item.ReviewText;
                List<string> Company = new List<string>();
                if (item.CompanyCSV == "All" || string.IsNullOrEmpty(item.CompanyCSV))
                {
                    Performance.CompanyCSV = "All";
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.CompanyCSV))
                    {
                        string[] companysplit = item.CompanyCSV.Split(',');
                        foreach (var Companyitem in companysplit)
                        {
                            var list = _AdminPearformanceMethod.CompanyId(Convert.ToInt32(Companyitem));
                            Company.Add(list);
                        }
                        Performance.CompanyCSV = string.Join(",", Company);
                    }
                }
                List<string> Location = new List<string>();
                if (item.LocationCSV == "All" || string.IsNullOrEmpty(item.LocationCSV))
                {
                    Performance.LocationCSV = "All";
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.LocationCSV))
                    {
                        string[] locationsplit = item.LocationCSV.Split(',');
                        foreach (var Locationitem in locationsplit)
                        {
                            var list = _AdminPearformanceMethod.LocationId(Convert.ToInt32(Locationitem));
                            Location.Add(list);
                        }
                        Performance.LocationCSV = string.Join(",", Location);
                    }
                }
                List<string> Business = new List<string>();
                if (item.BusinessCSV == "All" || string.IsNullOrEmpty(item.BusinessCSV))
                {
                    Performance.BusinessCSV = "All";
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.BusinessCSV))
                    {
                        string[] Businesssplit = item.BusinessCSV.Split(',');
                        foreach (var Businessitem in Businesssplit)
                        {
                            var list = _AdminPearformanceMethod.BusinessId(Convert.ToInt32(Businessitem));
                            Business.Add(list);
                        }
                        Performance.BusinessCSV = string.Join(",", Business);
                    }
                }

                List<string> Division = new List<string>();
                if (item.DivisionCSV == "All" || string.IsNullOrEmpty(item.DivisionCSV))
                {
                    Performance.DivisionCSV = "All";
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.DivisionCSV))
                    {
                        string[] Divisionsplit = item.DivisionCSV.Split(',');
                        foreach (var Divisionitem in Divisionsplit)
                        {
                            var list = _AdminPearformanceMethod.DivisionId(Convert.ToInt32(Divisionitem));
                            Division.Add(list);
                        }
                        Performance.DivisionCSV = string.Join(",", Division);
                    }
                }

                List<string> Pool = new List<string>();
                if (item.PoolCSV == "All" || string.IsNullOrEmpty(item.PoolCSV))
                {
                    Performance.PoolCSV = "All";
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.PoolCSV))
                    {
                        string[] Poolsplit = item.PoolCSV.Split(',');
                        foreach (var Poolitem in Poolsplit)
                        {
                            var list = _AdminPearformanceMethod.PoolId(Convert.ToInt32(Poolitem));
                            Pool.Add(list);
                        }
                        Performance.PoolCSV = string.Join(",", Pool);
                    }
                }

                List<string> Function = new List<string>();
                if (item.FunctionCSV == "All" || string.IsNullOrEmpty(item.FunctionCSV))
                {
                    Performance.FunctionCSV = "All";
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.FunctionCSV))
                    {
                        string[] Functionsplit = item.FunctionCSV.Split(',');
                        foreach (var Functionitem in Functionsplit)
                        {
                            var list = _AdminPearformanceMethod.FucantionId(Convert.ToInt32(Functionitem));
                            Function.Add(list);
                        }
                        Performance.FunctionCSV = string.Join(",", Function);
                    }
                }
                List<string> JobTitle = new List<string>();
                if (item.JobRoleCSV == "All" || string.IsNullOrEmpty(item.JobRoleCSV))
                {
                    Performance.JobRoleCSV = "All";
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.JobRoleCSV))
                    {
                        string[] JobTitlesplit = item.JobRoleCSV.Split(',');
                        foreach (var JobTitleitem in JobTitlesplit)
                        {
                            var list = _AdminPearformanceMethod.JobtitleId(Convert.ToInt32(JobTitleitem));
                            JobTitle.Add(list);
                        }
                        Performance.JobRoleCSV = string.Join(",", JobTitle);
                    }
                }
                List<string> EmploymentType = new List<string>();
                if (item.EmploymentTypeCSV == "All" || string.IsNullOrEmpty(item.EmploymentTypeCSV))
                {
                    Performance.EmploymentTypeCSV = "All";
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.EmploymentTypeCSV))
                    {
                        string[] Employmentsplit = item.EmploymentTypeCSV.Split(',');
                        foreach (var Employmentitem in Employmentsplit)
                        {
                            var list = _AdminPearformanceMethod.EmployeementId(Convert.ToInt32(Employmentitem));
                            EmploymentType.Add(list);
                        }
                        Performance.EmploymentTypeCSV = string.Join(",", EmploymentType);
                    }
                }

                model.Add(Performance);
            }
            return model;
        }
        public ActionResult AddEditPerformancetSet(PerformanceSettingViewModel model)
        {
            var CompanyList = _otherSettingMethod.getAllSystemValueListByKeyName("Company List");
            model.CompanyList.Add(new SelectListItem() { Text = "All", Value = "All" });
            foreach (var item in CompanyList)
            {
                model.CompanyList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var LocationList = _otherSettingMethod.getAllSystemValueListByKeyName("Office Locations");
            model.LocationList.Add(new SelectListItem() { Text = "All", Value = "All" });
            foreach (var item in LocationList)
            {
                model.LocationList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var BusinessList = _CompanyStructureMethod.getAllBusinessList();
            model.BusinessList.Add(new SelectListItem() { Text = "All", Value = "All" });
            foreach (var item in BusinessList)
            {
                model.BusinessList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString() });
            }
            var DivisionList = _CompanyStructureMethod.getAllDivisionList();
            model.DivisionList.Add(new SelectListItem() { Text = "All", Value = "All" });
            foreach (var item in DivisionList)
            {
                model.DivisionList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString() });
            }
            var PoolList = _CompanyStructureMethod.getAllPoolsList();
            model.PoolList.Add(new SelectListItem() { Text = "All", Value = "All" });
            foreach (var item in PoolList)
            {
                model.PoolList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString() });
            }
            var FunctionListrecord = _CompanyStructureMethod.getAllFunctionsList();
            model.FunctionList.Add(new SelectListItem() { Text = "All", Value = "All" });
            foreach (var item in FunctionListrecord)
            {
                model.FunctionList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString() });
            }
            var jobList = _otherSettingMethod.getAllSystemValueListByKeyName("Job Title List");
            model.JobTitleList.Add(new SelectListItem() { Text = "All", Value = "All" });
            foreach (var item in jobList)
            {
                model.JobTitleList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var EmploymentListrecord = _otherSettingMethod.getAllSystemValueListByKeyName("Employment Type List");
            model.EmploymentList.Add(new SelectListItem() { Text = "All", Value = "All" });
            foreach (var item in EmploymentListrecord)
            {
                model.EmploymentList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            List<PerformanceSetting> data = _AdminPearformanceMethod.getAllList().ToList();
            model.CopyFromList.Add(new SelectListItem() { Text = "--Select Performance Template--", Value = "0" });
            foreach (var item in data)
            {
                model.CopyFromList.Add(new SelectListItem() { Text = @item.ReviewText, Value = @item.Id.ToString() });
            }
            if (model.Id > 0)
            {
                var performance = _AdminPearformanceMethod.getPerformanceSetById(model.Id);
                model.ReviewText = performance.ReviewText;
                model.AnnualReview = (bool)performance.AnnualReview;
                model.CompletionDate = String.Format("{0:dd-MM-yyy}", performance.CompletionDate);
                model.RatingOverAll = performance.RatingOverAll;
                model.RatingCore = performance.RatingCore;
                model.RatingJobRole = performance.RatingJobRole;
                model.CompanyCSV = performance.CompanyCSV;
                if (!string.IsNullOrEmpty(performance.CompanyCSV))
                {
                    if (performance.CompanyCSV.IndexOf(',') > 0)
                    {
                        model.selectedCompany = performance.CompanyCSV.Split(',').ToList();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(performance.CompanyCSV))
                    {
                        string record = performance.CompanyCSV;
                        model.selectedCompany.Add(record);
                    }
                }

                model.LocationCSV = performance.LocationCSV;
                if (performance.LocationCSV.IndexOf(',') > 0)
                {
                    model.selectedLocation = performance.LocationCSV.Split(',').ToList();
                }
                else
                {
                    if (!string.IsNullOrEmpty(performance.LocationCSV))
                    {
                        string record = performance.LocationCSV;
                        model.selectedLocation.Add(record);
                    }
                }
                model.BusinessCSV = performance.BusinessCSV;
                if (!string.IsNullOrEmpty(performance.BusinessCSV))
                {
                    if (performance.BusinessCSV.IndexOf(',') > 0)
                    {
                        model.selectedBusiness = performance.BusinessCSV.Split(',').ToList();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(performance.BusinessCSV))
                    {
                        string record = performance.BusinessCSV;
                        model.selectedBusiness.Add(record);
                    }
                }
                model.DivisionCSV = performance.DivisionCSV;
                if (performance.DivisionCSV.IndexOf(',') > 0)
                {
                    model.selectedDivision = performance.DivisionCSV.Split(',').ToList();
                }
                else
                {
                    if (!string.IsNullOrEmpty(performance.DivisionCSV))
                    {
                        string record = performance.DivisionCSV;
                        model.selectedDivision.Add(record);
                    }
                }

                model.PoolCSV = performance.PoolCSV;
                if (performance.PoolCSV.IndexOf(',') > 0)
                {
                    model.selectedPoolList = performance.PoolCSV.Split(',').ToList();
                }
                else
                {
                    if (!string.IsNullOrEmpty(performance.PoolCSV))
                    {
                        string record = performance.PoolCSV;
                        model.selectedPoolList.Add(record);
                    }
                }
                model.FunctionCSV = performance.FunctionCSV;
                if (!string.IsNullOrEmpty(performance.FunctionCSV))
                {
                    if (performance.FunctionCSV.IndexOf(',') > 0)
                    {
                        model.selectedFunction = performance.FunctionCSV.Split(',').ToList();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(performance.FunctionCSV))
                    {
                        string record = performance.FunctionCSV;
                        model.selectedFunction.Add(record);
                    }
                }
                model.JobRoleCSV = performance.JobRoleCSV;
                if (!string.IsNullOrEmpty(performance.JobRoleCSV))
                {
                    if (performance.JobRoleCSV.IndexOf(',') > 0)
                    {
                        model.selectedJobTitle = performance.JobRoleCSV.Split(',').ToList();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(performance.JobRoleCSV))
                    {
                        string record = performance.JobRoleCSV;
                        model.selectedJobTitle.Add(record);
                    }
                }

                model.EmploymentTypeCSV = performance.EmploymentTypeCSV;
                if (!string.IsNullOrEmpty(performance.EmploymentTypeCSV))
                {
                    if (performance.EmploymentTypeCSV.IndexOf(',') > 0)
                    {
                        model.selectedEmployment = performance.EmploymentTypeCSV.Split(',').ToList();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(performance.EmploymentTypeCSV))
                    {
                        string record = performance.EmploymentTypeCSV;
                        model.selectedEmployment.Add(record);
                    }
                }

                model.OverallScoreJsonList = performance.OverallScoreJson.Split('^').ToList();
                foreach (var item in model.OverallScoreJsonList)
                {
                    model.OverallScoreJsonDetaillistList.Add(new SelectListItem() { Text = item, Value = item });
                }
                model.CoreSegmentJSON = performance.CoreSegmentJSON;
                model.CoWorkerSegmentJSON = performance.CoWorkerSegmentJSON;
                model.JobRoleSegmentJSON = performance.JobRoleSegmentJSON;
                model.CustomerSegmentJSON = performance.CustomerSegmentJSON;
            }
            else
            {
                model.selectedCompany.Add("All");
                model.selectedLocation.Add("All");
                model.selectedBusiness.Add("All");
                model.selectedDivision.Add("All");
                model.selectedPoolList.Add("All");
                model.selectedFunction.Add("All");
                model.selectedJobTitle.Add("All");
                model.selectedEmployment.Add("All");
            }

            return PartialView("_PartialPearformanceCreate", model);
        }
        public ActionResult CreateMultipleOverallScore(int actionid)
        {
            try
            {
                actionid = actionid + 1;
                return PartialView("_PartialOverallScoreAdd", actionid);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }

        }
                
    
      
        public ActionResult SavePerformance(PerformanceSettingViewModel model)
        {
            int userId = SessionProxy.UserId;
            model.CurrentUserId = userId;
            _AdminPearformanceMethod.SaveProjectSet(model);
            List<PerformanceSettingViewModel> modelList = returnList();
            return PartialView("_partialPerformanceReviewList", modelList);
        }
        public ActionResult CopyFrom(PerformanceSettingViewModel model)
        {
            model.ReviewText = model.ReviewText;
            model.AnnualReview = (bool)model.AnnualReview;
            model.CompletionDate = String.Format("{0:dd-MM-yyy}", model.CompletionDate);
            var CompanyList = _otherSettingMethod.getAllSystemValueListByKeyName("Company List");
            model.CompanyList.Add(new SelectListItem() { Text = "All", Value = "All" });
            foreach (var item in CompanyList)
            {
                model.CompanyList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var LocationList = _otherSettingMethod.getAllSystemValueListByKeyName("Office Locations");
            model.LocationList.Add(new SelectListItem() { Text = "All", Value = "All" });
            foreach (var item in LocationList)
            {
                model.LocationList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var BusinessList = _CompanyStructureMethod.getAllBusinessList();
            model.BusinessList.Add(new SelectListItem() { Text = "All", Value = "All" });
            foreach (var item in BusinessList)
            {
                model.BusinessList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString() });
            }
            var DivisionList = _CompanyStructureMethod.getAllDivisionList();
            model.DivisionList.Add(new SelectListItem() { Text = "All", Value = "All" });
            foreach (var item in DivisionList)
            {
                model.DivisionList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString() });
            }
            var PoolList = _CompanyStructureMethod.getAllPoolsList();
            model.PoolList.Add(new SelectListItem() { Text = "All", Value = "All" });
            foreach (var item in PoolList)
            {
                model.PoolList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString() });
            }
            var FunctionListrecord = _CompanyStructureMethod.getAllFunctionsList();
            model.FunctionList.Add(new SelectListItem() { Text = "All", Value = "All" });
            foreach (var item in FunctionListrecord)
            {
                model.FunctionList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString() });
            }
            var jobList = _otherSettingMethod.getAllSystemValueListByKeyName("Job Title List");
            model.JobTitleList.Add(new SelectListItem() { Text = "All", Value = "All" });
            foreach (var item in jobList)
            {
                model.JobTitleList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var EmploymentListrecord = _otherSettingMethod.getAllSystemValueListByKeyName("Employment Type List");
            model.EmploymentList.Add(new SelectListItem() { Text = "All", Value = "All" });
            foreach (var item in EmploymentListrecord)
            {
                model.EmploymentList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }

            List<PerformanceSetting> data = _AdminPearformanceMethod.getAllList().ToList();
            model.CopyFromList.Add(new SelectListItem() { Text = "--Select Performance Template--", Value = "0" });
            foreach (var item in data)
            {
                model.CopyFromList.Add(new SelectListItem() { Text = @item.ReviewText, Value = @item.Id.ToString() });
            }
            var performance = _AdminPearformanceMethod.getPerformanceSetById(model.CopyId);
            if (model.CompanyCSV.IndexOf(',') > 0)
            {
                model.selectedCompany = model.CompanyCSV.Split(',').ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(model.CompanyCSV))
                {
                    string record = model.CompanyCSV;
                    model.selectedCompany.Add(record);
                }
            }

            model.LocationCSV = model.LocationCSV;
            if (model.LocationCSV.IndexOf(',') > 0)
            {
                model.selectedLocation = model.LocationCSV.Split(',').ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(model.LocationCSV))
                {
                    string record = model.LocationCSV;
                    model.selectedLocation.Add(record);
                }
            }
            model.BusinessCSV = model.BusinessCSV;
            if (model.BusinessCSV.IndexOf(',') > 0)
            {
                model.selectedBusiness = model.BusinessCSV.Split(',').ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(model.BusinessCSV))
                {
                    string record = model.BusinessCSV;
                    model.selectedBusiness.Add(record);
                }
            }
            model.DivisionCSV = model.DivisionCSV;
            if (model.DivisionCSV.IndexOf(',') > 0)
            {
                model.selectedDivision = model.DivisionCSV.Split(',').ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(model.DivisionCSV))
                {
                    string record = model.DivisionCSV;
                    model.selectedDivision.Add(record);
                }
            }

            model.PoolCSV = model.PoolCSV;
            if (model.PoolCSV.IndexOf(',') > 0)
            {
                model.selectedPoolList = model.PoolCSV.Split(',').ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(model.PoolCSV))
                {
                    string record = model.PoolCSV;
                    model.selectedPoolList.Add(record);
                }
            }
            model.FunctionCSV = model.FunctionCSV;
            if (model.FunctionCSV.IndexOf(',') > 0)
            {
                model.selectedFunction = model.FunctionCSV.Split(',').ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(model.FunctionCSV))
                {
                    string record = model.FunctionCSV;
                    model.selectedFunction.Add(record);
                }
            }
            model.JobRoleCSV = model.JobRoleCSV;
            if (model.JobRoleCSV.IndexOf(',') > 0)
            {
                model.selectedJobTitle = model.JobRoleCSV.Split(',').ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(model.JobRoleCSV))
                {
                    string record = model.JobRoleCSV;
                    model.selectedJobTitle.Add(record);
                }
            }

            model.EmploymentTypeCSV = model.EmploymentTypeCSV;
            if (model.EmploymentTypeCSV.IndexOf(',') > 0)
            {
                model.selectedEmployment = model.EmploymentTypeCSV.Split(',').ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(model.EmploymentTypeCSV))
                {
                    string record = model.EmploymentTypeCSV;
                    model.selectedEmployment.Add(record);
                }
            }
            if(performance != null)
            {
            model.CoreSegmentJSON = performance.CoreSegmentJSON;
            model.CoWorkerSegmentJSON = performance.CoWorkerSegmentJSON;
            model.JobRoleSegmentJSON = performance.JobRoleSegmentJSON;
            model.CustomerSegmentJSON = performance.CustomerSegmentJSON;
            model.RatingOverAll = performance.RatingOverAll;
            model.RatingCore = performance.RatingCore;
            model.RatingJobRole = performance.RatingJobRole;
            model.OverallScoreJsonList = performance.OverallScoreJson.Split('^').ToList();
            }
            if(model.OverallScoreJsonList != null)
            {
                foreach (var item in model.OverallScoreJsonList)
                {
                    model.OverallScoreJsonDetaillistList.Add(new SelectListItem() { Text = item, Value = item });
                }
            }
           
            return PartialView("_PartialPearformanceCreate", model);
        }


        #region Core Segment
        public ActionResult SegmentCoreSectionCreate(string coreSegmentJSON)
        {
            List<EditSegmentViewModel> model = new List<EditSegmentViewModel>();
            //EditSegmentViewModel EditModel = new EditSegmentViewModel();
            List<KeyValue> ListOfCoreFiled = new List<KeyValue>();
            List<KeyValue> CoreValueList = new List<KeyValue>();
            if (!string.IsNullOrEmpty(coreSegmentJSON))
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                model = js.Deserialize<List<EditSegmentViewModel>>(coreSegmentJSON);
            }         
            var CoreFiledTypeList = _otherSettingMethod.getAllSystemValueListByKeyName("Field Type List");
            ListOfCoreFiled = CoreFiledTypeList.Select(xx => new KeyValue()
            {
                Key = xx.Id,
                Value = xx.Value
            }).ToList();
            ViewBag.CoreFiledList = ListOfCoreFiled;  
            var CoreValList= _otherSettingMethod.getAllSystemValueListByKeyName("Core Values");
            CoreValueList = CoreValList.Select(xx => new KeyValue()
            {
                Key = xx.Id,
                Value = xx.Value
            }).ToList();
            ViewBag.Core_ValueList = CoreValueList;
            //var CoreTypeList = _otherSettingMethod.getAllSystemValueListByKeyName(" Core Values");
            //foreach (var item in CoreTypeList)
            //{
            //    EditModel.JobRoleValueList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            //}
            //model.Add(EditModel);
            return PartialView("_PartialCoreSegmentSection", model);
        }
        public ActionResult AddEditCoreSegment(SegmentViewModel model)
        {
            if (model.isAddMode == false)
            {
              JavaScriptSerializer js = new JavaScriptSerializer();
              QuestionModel queModel = new QuestionModel();
              if (!string.IsNullOrEmpty(model.JsonQuestionString))
                {
                    model.CoreQueList = js.Deserialize<List<QuestionModel>>(model.JsonQuestionString);                                       
                   }                
           }
            else
            {
                model.SegmentId = 0;
            }
            return PartialView("_partialAddEditCoreSegment", model);
        }        
        #endregion

        #region Job Roles Segment

        public ActionResult SegmentJobRolesSectionCreate(string jobRoleSegmentJSON)
        {
            List<EditSegmentViewModel> model = new List<EditSegmentViewModel>();
            List<KeyValue> ListOfJobRoleFiled = new List<KeyValue>();
            List<KeyValue> JobRoleValueList = new List<KeyValue>();
            if (!string.IsNullOrEmpty(jobRoleSegmentJSON))
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                model = js.Deserialize<List<EditSegmentViewModel>>(jobRoleSegmentJSON);
            }
            var JobRoleFiledTypeList = _otherSettingMethod.getAllSystemValueListByKeyName("Field Type List");
            ListOfJobRoleFiled = JobRoleFiledTypeList.Select(xx => new KeyValue()
            {
                Key = xx.Id,
                Value = xx.Value
            }).ToList();
            ViewBag.JobRoleFiledList = ListOfJobRoleFiled;
            var CoreValList = _otherSettingMethod.getAllSystemValueListByKeyName("Core Values");
            JobRoleValueList = CoreValList.Select(xx => new KeyValue()
            {
                Key = xx.Id,
                Value = xx.Value
            }).ToList();
            ViewBag.JobRole_ValueList = JobRoleValueList;
            return PartialView("_PartialJobRolesSegmentSection", model);
        }

        public ActionResult AddEditJobRoleSegment(SegmentViewModel model)
        {
            if (model.isAddMode == false)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                JobRoleQuestionModel queModel = new JobRoleQuestionModel();
                if (!string.IsNullOrEmpty(model.JsonQuestionString))
                {
                    model.JobroleQueList = js.Deserialize<List<JobRoleQuestionModel>>(model.JsonQuestionString);
                //    foreach (var item in model.JobroleQueList)
                //    {
                //        if (item.FiledId != null)
                //        {
                //            int CoreFiledid = Convert.ToInt32(item.FiledId);
                //            var coreData = _db.SystemListValues.Where(x => x.Id == CoreFiledid).FirstOrDefault();
                //            item.FiledId = Convert.ToString(coreData.Id);
                //        }
                //        if (item.CValue != null)
                //        {
                //            int CoreValueid = Convert.ToInt32(item.CValue);
                //            var coreValueData = _db.SystemListValues.Where(x => x.Id == CoreValueid).FirstOrDefault();
                //            item.CValue = Convert.ToString(coreValueData.Id);
                //        }
                //    }
                }
            }
            else
            {
                model.SegmentId = 0;
            }
            return PartialView("_partialAddEditJobSegment", model);
        }

        #endregion

        #region Customer Segment
        public ActionResult SegmentCustomerSectionCreate(string customerSegmentJSON)
        {
            List<EditSegmentViewModel> model = new List<EditSegmentViewModel>();
            List<KeyValue> ListOfCustomerFiled = new List<KeyValue>();
            List<KeyValue> CustomerValueList = new List<KeyValue>();
            var CustomerFiledTypeList = _otherSettingMethod.getAllSystemValueListByKeyName("Field Type List");
            ListOfCustomerFiled = CustomerFiledTypeList.Select(xx => new KeyValue()
            {
                Key = xx.Id,
                Value = xx.Value
            }).ToList();
            ViewBag.CustomerFiledList = ListOfCustomerFiled;
            var CoreValList = _otherSettingMethod.getAllSystemValueListByKeyName("Core Values");
            CustomerValueList = CoreValList.Select(xx => new KeyValue()
            {
                Key = xx.Id,
                Value = xx.Value
            }).ToList();
            ViewBag.Customer_ValueList = CustomerValueList;
            if (!string.IsNullOrEmpty(customerSegmentJSON))
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                model = js.Deserialize<List<EditSegmentViewModel>>(customerSegmentJSON);
            }            
           
            return PartialView("_PartialCustomerSegmentSection", model);
        }
        public ActionResult AddEditCustomerSegment(SegmentViewModel model)
        {
            if (model.isAddMode == false)
            {
                JobRoleQuestionModel queModel = new JobRoleQuestionModel();
                if (!string.IsNullOrEmpty(model.JsonQuestionString))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    model.CustomerQueList = js.Deserialize<List<CustomerQuestionModel>>(model.JsonQuestionString);                
                }
            }
            else
            {
                model.SegmentId = 0;
            }
            return PartialView("_partialAddEditCustomerSegment", model);
        }

        #endregion

        #region  Co-Worker Segment

        public ActionResult SegmentCoWorkerSectionCreate(string coWorkerSegmentJSON)
        {
            List<EditSegmentViewModel> model = new List<EditSegmentViewModel>();
            List<KeyValue> ListOfCoWorkerFiled = new List<KeyValue>();
            List<KeyValue> CoWorkerValueList = new List<KeyValue>();
            if (!string.IsNullOrEmpty(coWorkerSegmentJSON))
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                model = js.Deserialize<List<EditSegmentViewModel>>(coWorkerSegmentJSON);
            }
            var CoWorkerFiledTypeList = _otherSettingMethod.getAllSystemValueListByKeyName("Field Type List");
            ListOfCoWorkerFiled = CoWorkerFiledTypeList.Select(xx => new KeyValue()
            {
                Key = xx.Id,
                Value = xx.Value
            }).ToList();
            ViewBag.CoWorkerFiledList = ListOfCoWorkerFiled;
            var CoWorkerValList = _otherSettingMethod.getAllSystemValueListByKeyName("Core Values");
            CoWorkerValueList = CoWorkerValList.Select(xx => new KeyValue()
            {
                Key = xx.Id,
                Value = xx.Value
            }).ToList();
            ViewBag.CoWorker_ValueList = CoWorkerValueList;
            return PartialView("_PartialCoWorkerSegmentSection", model);
        }
        public ActionResult AddEditCoWorkerSegment(SegmentViewModel model)
        {
            if (model.isAddMode == false)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                model.CoworkerQueList = js.Deserialize<List<CoworkerQuestionModel>>(model.JsonQuestionString);
                //foreach (var item in model.CoworkerQueList)
                //{
                //    if (item.FiledId != null)
                //    {
                //        int CoreFiledid = Convert.ToInt32(item.FiledId);
                //        var coreData = _db.SystemListValues.Where(x => x.Id == CoreFiledid).FirstOrDefault();
                //        item.FiledId = Convert.ToString(coreData.Id);
                //    }
                //    if (item.CValue != null)
                //    {
                //        int CoreValueid = Convert.ToInt32(item.CValue);
                //        var coreValueData = _db.SystemListValues.Where(x => x.Id == CoreValueid).FirstOrDefault();
                //        item.CValue = Convert.ToString(coreValueData.Id);
                //    }
               // }
            }
            else
            {
                model.SegmentId = 0;
            }
            return PartialView("_partialAddEditCoWorkerSegment", model);
        }

        #endregion

        //SendMail
        public ActionResult SendMailToAll(string Compltiondate)
        {
            PerformanceSettingViewModel model = new PerformanceSettingViewModel();
            var ToData = _db.AspNetUsers.Where(x => x.SSOID.StartsWith("W") && x.Archived == false).ToList();
            var FromData = _db.AspNetUsers.Where(x=>x.Id==SessionProxy.UserId).FirstOrDefault();
            if (!string.IsNullOrEmpty(Compltiondate))
            {
                foreach (var item in ToData)
                {
                    HRTool.Models.MailModel mail = new HRTool.Models.MailModel();
                    mail.From = FromData.UserName;
                    mail.To = item.UserName;
                    mail.Subject = "Performance Review";
                    mail.Header = "Performance Review";
                    string inputFormat = "dd-MM-yyyy";
                    string outputFormat = "ddd, dd MMM yyyy";
                    var VarCompetiondate = DateTime.ParseExact(Compltiondate, inputFormat, CultureInfo.InvariantCulture);
                    DateTime date = Convert.ToDateTime(VarCompetiondate);
                    //string dateTimeEndorse = DateTime.Now.ToString("ddd, dd MMM yyyy");
                    string dateTimeEndorse = date.ToString("ddd, dd MMM yyyy");
                    mail.EndorsementDate = dateTimeEndorse;
                    using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/MailToAllEmployeePerformanceSetting.html")))
                    {
                        mail.Body = reader.ReadToEnd();
                    }
                    mail.Body = mail.Body.Replace("{0}", item.FirstName);
                    string FromToName = string.Empty;
                    mail.Body = mail.Body.Replace("{1}", dateTimeEndorse);
                    string mailFromReceive = Common.sendMailWithoutAttachment(mail);
                }
            }
            return PartialView("_PartialPearformanceCreate", model);

        }

        //Delete Review

        public ActionResult DeleteDocument(int Id)
        {

            bool save = _AdminPearformanceMethod.deletePerformance(Id, SessionProxy.UserId);

            if (save)
            {
                List<PerformanceSettingViewModel> modelList = returnList();
                return PartialView("_partialPerformanceReviewList", modelList);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);

            }

        }
    }
}