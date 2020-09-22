using HRTool.CommanMethods;
using HRTool.Models.Resources;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRTool.DataModel;
using System.Web.Mvc;
using System.IO;
using HRTool.CommanMethods.Resources;
using System.Web.Script.Serialization;
using Rotativa;
using Rotativa.Options;
using System.Globalization;

namespace HRTool.Controllers
{
    public class EmployeePerformanceController : Controller
    {
        //
        // GET: /EmployeePerformance/
        // #region Constant
        EvolutionEntities _db = new EvolutionEntities();
        EmployeePerformanceMethod _employeePerformanceMethod = new EmployeePerformanceMethod();
        public ActionResult Index(string EmployeeId)
        {
            return View();
        }
        public ActionResult Review()
        {
            return View();
        }

        public ActionResult EmployeePerformance(string EmployeeId)
        {
            EmployeePerformanceViewModel model = new EmployeePerformanceViewModel();
            int EmpId = Convert.ToInt32(EmployeeId);
            model = getListOfPerformance(EmpId);
            model.EmployeeId = Convert.ToInt32(EmployeeId);
            return View(model);
        }

        public ActionResult ActiveReviews(string EmployeeId)
        {
            int EmpId = Convert.ToInt32(EmployeeId);
            EmployeePerformanceViewModel modelList = getListOfPerformance(EmpId);
            modelList.EmployeeId = EmpId;
            modelList.IsActivePastFlag = 1;
            return PartialView("_partialActivePerformanceReview", modelList);
        }
        public ActionResult DDlProject()
        {
            List<KeyValue> model = new List<KeyValue>();
            var List = _db.Projects.Where(x => x.Archived == false).ToList();
            model.Add(new KeyValue { Key = 0, Value = "-- Select Project --" });
            foreach (var item in List)
            {
                var value = item.Name;
                KeyValue _KeyValue = new KeyValue();
                _KeyValue.Key = item.Id;
                _KeyValue.Value = value;
                model.Add(_KeyValue);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckEmployeeReviewExist(int EmployeeId)
        {
            StartNewReviewViewModel model = new StartNewReviewViewModel();
            var EmployeePerformanceData = _db.EmployeePerformances.Where(x => x.EmployeeId == EmployeeId && x.ReviewStatus == "Open" && x.Archived == false).FirstOrDefault();
            if (EmployeePerformanceData != null)
            {
                model.IsEmployeeExistReview = "1";
            }
            else
            {
                model.IsEmployeeExistReview = "0";
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult StartNewReview(int EmployeeId)
        {
            StartNewReviewViewModel model = new StartNewReviewViewModel();
            //var ReviewList = _db.PerformanceSettings.Where(x => x.Archived == false && x.CompletionDate > DateTime.Now).ToList();
            var ReviewList = _db.PerformanceSettings.Where(x => x.Archived == false && x.CompletionDate > DateTime.Now || x.CompletionDate == null).ToList();

            foreach (var item in ReviewList)
            {
                model.ReviewList.Add(new SelectListItem() { Text = item.ReviewText, Value = item.Id.ToString() });
            }
            var ProjectList = _db.Projects.Where(x => x.Archived == false).ToList();
            foreach (var item in ProjectList)
            {
                model.ProjectList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            model.EmployeeId = EmployeeId;
            return PartialView("_partialStartNewReview", model);
            //return PartialView("_partialActivePerformanceReview"); 
        }


        public ActionResult PastReviews(string EmployeeId)
        {
            int EmpId = Convert.ToInt32(EmployeeId);
            EmployeePerformanceViewModel modelList = getListOfPerformance(EmpId);
            modelList.EmployeeId = EmpId;
            modelList.IsActivePastFlag = 0;
            return PartialView("_PartialPastReview", modelList);
        }
        public EmployeePerformanceViewModel getListOfPerformance(int EmpId)
        {
            List<EmployeePerformance> dataPerformance = _employeePerformanceMethod.getListofPerformance().Where(x => x.EmployeeId == EmpId && x.ReviewStatus == "Open").ToList();
            EmployeePerformanceViewModel reviewmodel = new EmployeePerformanceViewModel();
            foreach (var item in dataPerformance)
            {
                EmployeePerformanceViewModel model = new EmployeePerformanceViewModel();
                model.Id = item.Id;
                var CustomerStatus = _db.PerformanceCustomerDetails.Where(x => x.Performance_Id == item.Id && x.IsArchived == false).FirstOrDefault();
                if (CustomerStatus != null)
                {
                    model.CustomerShare = CustomerStatus.InviteStatus;
                }
                var EmpReviewByName = _db.AspNetUsers.Where(x => x.Id == item.RevviewByEmpID && x.Archived == false).FirstOrDefault();
                model.ReviewByName = EmpReviewByName.FirstName + " " + EmpReviewByName.LastName;
                var ReviewName = _db.PerformanceSettings.Where(x => x.Id == item.ReviewId && x.Archived == false).FirstOrDefault();
                model.CreatedDate = Convert.ToDateTime(item.CreatedDate).ToString("dd MMM yyyy");
                if (ReviewName.ReviewText != null)
                    model.ReviewName = ReviewName.ReviewText;
                model.OverAllScore = Convert.ToDouble(item.OverallScore);
                model.ReviewId = item.ReviewId;
                model.EmployeeId = item.EmployeeId;
                model.ProjectId = item.ProjectId;
                reviewmodel.ListofPerformance.Add(model);
            }
            List<EmployeePerformance> dataPastPerformance = _employeePerformanceMethod.getPastListPerformance().Where(x => x.EmployeeId == EmpId).ToList();
            foreach (var item in dataPastPerformance)
            {
                EmployeePerformanceViewModel model = new EmployeePerformanceViewModel();
                model.Id = item.Id;
                var EmpReviewByName = _db.AspNetUsers.Where(x => x.Id == item.RevviewByEmpID && x.Archived == false).FirstOrDefault();
                model.ReviewByName = EmpReviewByName.FirstName + " " + EmpReviewByName.LastName;
                var ReviewName = _db.PerformanceSettings.Where(x => x.Id == item.ReviewId && x.Archived == false).FirstOrDefault();
                model.ReviewName = ReviewName.ReviewText;
                model.ReviewId = item.ReviewId;
                model.EmployeeId = item.EmployeeId;
                model.ProjectId = item.ProjectId;
                model.CreatedDate = Convert.ToDateTime(item.CreatedDate).ToString("dd MMM yyyy");
                var complectiondata = _db.PerformanceSettings.Where(x => x.Id == item.ReviewId).Select(x => x.CompletionDate).FirstOrDefault();
                model.CompletionDate = Convert.ToDateTime(complectiondata);
                reviewmodel.ListOfPastPerformace.Add(model);
            }
            return reviewmodel;
        }

        public ActionResult SaveNewReview(EmployeePerformanceViewModel model)
        {
            bool saveData = _employeePerformanceMethod.SaveEmployeePerformace(model);
            if (saveData == false)
            {
                return Json(saveData, JsonRequestBehavior.AllowGet);
            }
            EmployeePerformanceViewModel modelList = getListOfPerformance(model.EmployeeId);
            return PartialView("_partialActivePerformanceReview", modelList);
        }

        public ActionResult getReviewDetails(int empPerReviewId, int ReviewId, int EmpID, int IsActivePastFlag)
        {
            ReviewDetails model = new ReviewDetails();
            model.ReviewId = ReviewId;
            model.EmpID = EmpID;
            model.Id = empPerReviewId;
            int CurrentUserId = SessionProxy.UserId;
            var ReportToData = _db.EmployeeRelations.Where(x => x.UserID == EmpID && x.Reportsto == CurrentUserId && x.IsActive == true).FirstOrDefault();
            if (ReportToData != null)
            {
                model.Flag = 1;
                model.ManagerId = Convert.ToInt32(ReportToData.Reportsto);
            }
            else
            {
                model.Flag = 0;
                model.ManagerId = CurrentUserId;
            }
            model.IsActivePastFlag = IsActivePastFlag;
            model.EmpPerfReviewId = Convert.ToString(empPerReviewId);
            return PartialView("_partialReviewDetails", model);
        }
        public Coworker getCoWorkerListData(int reviewID, int EmpId)
        {
            Coworker model = new Coworker();
            var EmployeeReviewData = _db.EmployeePerformances.Where(x => x.ReviewId == reviewID && x.EmployeeId == EmpId && x.ReviewStatus == "Open").ToList();
            foreach (var PerData in EmployeeReviewData)
            {
                var CoworkerData = _db.PerformanceCoWorkerDetails.Where(x => x.PerformanceId == PerData.Id && x.Archived == false).ToList();
                foreach (var item in CoworkerData)
                {
                    CoworkerInviteList coModel = new CoworkerInviteList();
                    coModel.Id = item.Id;
                    coModel.coworkerId = Convert.ToInt32(item.Id);
                    var EmpDetails = _db.AspNetUsers.Where(x => x.Id == item.InviteEmployeeId && x.Archived == false).FirstOrDefault();
                    coModel.EmpName = EmpDetails.FirstName + " " + EmpDetails.LastName;
                    coModel.Status = item.Status;
                    model.CoworkerInviteList.Add(coModel);
                }
                var CustomerData = _db.PerformanceCustomerDetails.Where(x => x.Performance_Id == PerData.Id && x.EmployeeId == EmpId && x.IsArchived == false).ToList();
                foreach (var item in CustomerData)
                {
                    CustomerInviteList customerModel = new CustomerInviteList();
                    customerModel.CustomerID = Convert.ToInt32(item.Id);
                    var CustomerDetails = _db.AspNetUsers.Where(x => x.Id == item.InviteCustomer_Id && x.Archived == false).FirstOrDefault();
                    customerModel.CustomerName = CustomerDetails.FirstName + " " + CustomerDetails.LastName;
                    //customerModel.Status=item.stat
                    model.CustomerInviteList.Add(customerModel);
                }
                model.TotalInvitedCustomer = CustomerData.Count() + " Employee";
                model.TotalInvitedCoworker = CoworkerData.Count() + " Employee";
            }
            return model;
        }

        public ActionResult GetCoworkerList(int ReviewId, int isManagerEmployee, int EmpId, int IsActivePastFlag)
        {
            Coworker model = new Coworker();
            model = getCoWorkerListData(ReviewId, EmpId);
            model.IsActivePastFlag = IsActivePastFlag;
            return PartialView("_PartialCoworkerDetails", model);
        }
        public JsonResult InviteEmployeeList()
        {
            CoworkerInviteList model = new CoworkerInviteList();
            var employeeData = _db.AspNetUsers.Where(x => x.Archived == false && x.SSOID.StartsWith("W")).ToList();
            foreach (var item in employeeData)
            {
                model.EmployeeList.Add(new SelectListItem() { Text = item.FirstName + " " + item.LastName + " - " + item.SSOID, Value = item.Id.ToString() });
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReviewDate(int ReviewId)
        {
            //StartNewReviewViewModel model = new StartNewReviewViewModel();
            var ReviewDate = _db.PerformanceSettings.Where(x => x.Id == ReviewId && x.Archived == false).FirstOrDefault().CompletionDate;
            var ReviewText = _db.PerformanceSettings.Where(x => x.Id == ReviewId && x.Archived == false).FirstOrDefault().AnnualReview;
            if (ReviewDate != null)
            {
                ReviewDate = Convert.ToDateTime(ReviewDate);
                return Json(new { data1 = ReviewDate.ToString().Substring(0, ReviewDate.ToString().IndexOf(" ")), data2 = ReviewText }, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(new { data1 = ReviewDate.ToString(), data2 = ReviewText }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult InviteCoworkerForFeedback()
        {
            CoworkerInviteList model = new CoworkerInviteList();
            var employeeData = _db.AspNetUsers.Where(x => x.Archived == false && x.SSOID.StartsWith("W")).ToList();
            foreach (var item in employeeData)
            {
                model.EmployeeList.Add(new SelectListItem() { Text = item.FirstName + " " + item.LastName + " - " + item.SSOID, Value = item.Id.ToString() });
            }
            return PartialView("_PartialInviteCoworker", model);
        }

        public ActionResult CoWorkerQueDetails(Guid Id, int PerCoworkerId)
        {
            string GUID = Convert.ToString(Id);
            //  SegmentViewModel model = new SegmentViewModel();
            var coworkerSegdata = _db.PerformanceSettings.Where(x => x.GuID == GUID && x.Archived == false).FirstOrDefault();
            var PerCoworkerDetials = _db.PerformanceCoWorkerDetails.Where(x => x.Id == PerCoworkerId && x.InviteStatus == false).FirstOrDefault();
            List<EditSegmentViewModel> model = new List<EditSegmentViewModel>();
            if (PerCoworkerDetials == null)
            {
                model.Add(new EditSegmentViewModel());
                model.FirstOrDefault().Flag = 0;
            }
            else
            {
                if (!string.IsNullOrEmpty(coworkerSegdata.CoWorkerSegmentJSON))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    model = js.Deserialize<List<EditSegmentViewModel>>(coworkerSegdata.CoWorkerSegmentJSON);
                    model.FirstOrDefault().Flag = 1;
                    foreach (var item in model)
                    {
                        item.CoreQueList = js.Deserialize<List<QuestionModel>>(item.QueationType);
                        item.PerCoworkerId = PerCoworkerId;
                    }
                }
            }
            return View("CoWorkerPerformanceRview", model);
        }

        public ActionResult getCoWorkerResponse(string Id, int PerCoworkerId)
        {
            List<EditSegmentViewModel> model = new List<EditSegmentViewModel>();
            Coworker modelWorker = new Coworker();
            var PerCoworkerDetials = _db.PerformanceCoWorkerDetails.Where(x => x.Id == PerCoworkerId).FirstOrDefault();
            int ReviewId = (from record in _db.EmployeePerformances where record.Id == PerCoworkerDetials.PerformanceId select record.ReviewId).SingleOrDefault();
            if (PerCoworkerDetials.InviteEmployeeId != 0)
            {
                if (SessionProxy.UserId == 0 || SessionProxy.UserId == null)
                {
                    return RedirectToAction("LoginRedirect", "Login");
                }
            }
            if (PerCoworkerDetials.InviteEmployeeId == 0 && Id != "true")
            {
                if (PerCoworkerDetials.Status == "Invited")
                {
                    modelWorker.cororkerId = PerCoworkerId.ToString();
                    return RedirectToAction("GuestReviewLogin", "MeEmployeePerformance", modelWorker);
                }
            }
            string GUID = Convert.ToString(Id);
            JavaScriptSerializer js = new JavaScriptSerializer();
            //  SegmentViewModel model = new SegmentViewModel();
            var coworkerSegdata = _db.PerformanceSettings.Where(x => x.Id == ReviewId && x.Archived == false).FirstOrDefault();
            if (Id != "true" && Id != string.Empty && Id != null)
            {
                coworkerSegdata = _db.PerformanceSettings.Where(x => x.GuID == GUID && x.Archived == false).FirstOrDefault();
            }
            List<Coworker> emodel = new List<Coworker>();
            if (PerCoworkerDetials.CoWorkerScoreJSC != null)
            {
                emodel = js.Deserialize<List<Coworker>>(PerCoworkerDetials.CoWorkerScoreJSC);
                foreach (var item in emodel)
                {
                    item.CoreQueListData = js.Deserialize<List<CoworkerQuestionModelForMe>>(item.questionData);

                }
            }

            if (PerCoworkerDetials.Status == "Invited" || PerCoworkerDetials.Status == "See Response")
            {
                if (!string.IsNullOrEmpty(coworkerSegdata.CoWorkerSegmentJSON))
                {
                    model = js.Deserialize<List<EditSegmentViewModel>>(coworkerSegdata.CoWorkerSegmentJSON);
                    if (model.Count() != 0)
                    {
                        model.FirstOrDefault().Flag = 1;

                    }
                    foreach (var item in model)
                    {
                        item.CoreQueList = js.Deserialize<List<QuestionModel>>(item.QueationType);
                        item.PerCoworkerId = PerCoworkerId;
                    }

                }
                if (emodel.Count() > 0)
                {
                    for (int i = 0; i < model.Count(); i++)
                    {
                        for (int j = 0; j < model[i].CoreQueList.Count(); j++)
                        {
                            if (model[i].CoreQueList[j].QueId == emodel[i].CoreQueListData[j].QueId)
                            {
                                model[i].CoreQueList[j].Score = emodel[i].CoreQueListData[j].score;
                            }
                        }
                    }
                }

            }
            else
            {
                model.Add(new EditSegmentViewModel());
                model.FirstOrDefault().Flag = 0;
            }
            model.FirstOrDefault().preWorkerID = PerCoworkerId;
            model.FirstOrDefault().HelpText = PerCoworkerDetials.Status;
            return View("CoWorkerPerformanceReviewEmployee", model);
        }
        public JsonResult UpdateCoWorkerCoreScore(int PerCoId, string questionListJSV)
        {
            List<EditSegmentViewModel> model = new List<EditSegmentViewModel>();
            var data = _employeePerformanceMethod.UpdateCoWorkerDetails(PerCoId, questionListJSV);
            model.Add(new EditSegmentViewModel());
            if (data == true)
            {
                model.FirstOrDefault().Flag = 0;
            }
            else
            {
                model.FirstOrDefault().Flag = 1;
            }
            return Json(model.FirstOrDefault().Flag, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SendMailToCoworker(int EmpId, string OtherempName, string OtherempEmail, int PerReviewId, int EmployeeId)
        {
            string message = "";
            CoworkerInviteList model = new CoworkerInviteList();
            Coworker datamodel = new Coworker();
            var EmpReviewData = _db.EmployeePerformances.Where(x => x.EmployeeId == EmployeeId && x.ReviewId == PerReviewId && x.Archived == false).FirstOrDefault();
            var ReviewData = _db.PerformanceSettings.Where(x => x.Id == PerReviewId && x.Archived == false).FirstOrDefault();
            if (EmpReviewData != null)
            {
                ReviewData = _db.PerformanceSettings.Where(x => x.Id == EmpReviewData.ReviewId && x.Archived == false).FirstOrDefault();
            }
            var emailData = _db.PerformanceCoWorkerDetails.Where(x => x.EmployeeID == EmployeeId && x.InviteEmployeeId == EmpId && x.Archived == false).FirstOrDefault();
            if (emailData == null)
            {
                if (EmpId > 0)
                {
                    int EmpID = Convert.ToInt32(EmpId);
                    int coDetailId = _employeePerformanceMethod.SaveCoworkerInviteLink(EmpID, EmpReviewData.Id, EmployeeId);
                    var FromData = _db.AspNetUsers.Where(x => x.Id == SessionProxy.UserId && x.Archived == false).FirstOrDefault();
                    HRTool.Models.MailModel mail = new HRTool.Models.MailModel();
                    using (StreamReader stramReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/SendMailCoworker.html")))
                    {
                        mail.Body = stramReader.ReadToEnd();
                    }
                    int ToId = Convert.ToInt32(EmpID);
                    var toData = _db.AspNetUsers.Where(x => x.Id == ToId && x.Archived == false).FirstOrDefault();
                    mail.From = FromData.UserName;
                    mail.To = toData.UserName;
                    mail.Subject = "Coworker mail";
                    mail.Body = mail.Body.Replace("{0}", toData.FirstName + " " + toData.LastName);
                    mail.Body = mail.Body.Replace("{3}", Convert.ToString(ReviewData.CompletionDate));
                    var baseUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["baseURL"].ToString();
                    var getGuId = ReviewData.GuID;
                    var link = baseUrl + "EmployeePerformance/CoWorkerQueDetails/" + coDetailId + "/" + getGuId;
                    mail.Body = mail.Body.Replace("{4}", link);
                    string mailToReceive = Common.sendMail(mail);
                }
                else
                {
                    message = "Invited not selected";
                }
            }
            else
            {
                message = "You have already invited this coworker";
            }

            if (!string.IsNullOrEmpty(OtherempName) && !string.IsNullOrEmpty(OtherempEmail))
            {
                var FromData = _db.AspNetUsers.Where(x => x.Id == SessionProxy.UserId && x.Archived == false).FirstOrDefault();
                HRTool.Models.MailModel mail = new HRTool.Models.MailModel();
                using (StreamReader stramReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/SendMailCoworker.html")))
                {
                    mail.Body = stramReader.ReadToEnd();
                }
                mail.From = FromData.UserName;
                mail.To = OtherempEmail;
                mail.Subject = "Coworker mail";
                mail.Body = mail.Body.Replace("{0}", OtherempName);
                string mailToReceive = Common.sendMail(mail);
            }
            // datamodel = getCoWorkerListData(PerReviewId, EmployeeId);
            // return PartialView("_PartialInviteCoworker", datamodel);
            return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
        }

        //Invite Customer For Feedback
        public JsonResult InviteCustomerList()
        {
            CustomerInviteList model = new CustomerInviteList();
            var customerData = _db.AspNetUsers.Where(x => x.Archived == false && x.SSOID.StartsWith("C")).ToList();
            foreach (var item in customerData)
            {
                model.CustomerList.Add(new SelectListItem() { Text = item.FirstName + " " + item.LastName + " - " + item.SSOID, Value = item.Id.ToString() });
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult InviteCustomerForFeedback()
        {
            CustomerInviteList model = new CustomerInviteList();
            var customerData = _db.AspNetUsers.Where(x => x.Archived == false && x.SSOID.StartsWith("C")).ToList();
            foreach (var item in customerData)
            {
                model.CustomerList.Add(new SelectListItem() { Text = item.FirstName + " " + item.LastName + " - " + item.SSOID, Value = item.Id.ToString() });
            }
            return PartialView("_partialInviteCustomer", model);
        }
        public ActionResult SendMailToCustomer(int CustId, int PerReviewId, int EmpId)
        {
            CustomerInviteList model = new CustomerInviteList();
            var EmpReviewData = _db.EmployeePerformances.Where(x => x.Id == PerReviewId && x.Archived == false).FirstOrDefault();
            var ReviewData = _db.PerformanceSettings.Where(x => x.Id == PerReviewId && x.Archived == false).FirstOrDefault();
            var EmployeeData = _db.AspNetUsers.Where(x => x.Id == EmpId && x.Archived == false).FirstOrDefault();
            if (CustId != 0 && CustId != null)
            {
                //int coDetailId = _employeePerformanceMethod.SaveCoworkerInviteLink(EmpId, PerReviewId);                
                var FromData = _db.AspNetUsers.Where(x => x.Id == SessionProxy.UserId && x.Archived == false).FirstOrDefault();
                HRTool.Models.MailModel mail = new HRTool.Models.MailModel();
                using (StreamReader stramReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/SendMailCustomer.html")))
                {
                    mail.Body = stramReader.ReadToEnd();
                }
                var toData = _db.AspNetUsers.Where(x => x.Id == CustId && x.Archived == false).FirstOrDefault();
                mail.From = FromData.UserName;
                mail.To = toData.UserName;
                mail.Subject = "Customer mail";
                mail.Body = mail.Body.Replace("{0}", toData.FirstName + " " + toData.LastName);
                mail.Body = mail.Body.Replace("{2}", EmployeeData.FirstName + " " + EmployeeData.LastName);
                var baseUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["baseURL"].ToString();
                var getGuId = ReviewData.GuID;
                var link = baseUrl + "EmployeePerformance/InviteCustomerDetails/" + EmpId + "/" + CustId + "/" + PerReviewId + "/" + getGuId;
                mail.Body = mail.Body.Replace("{3}", link);
                string mailToReceive = Common.sendMail(mail);
            }
            return PartialView("_partialInviteCustomer", model);
        }
        public ActionResult InviteCustomerDetails(int EmpId, int CustId, int PerReviewId, Guid Id)
        {
            string GUID = Convert.ToString(Id);
            //  SegmentViewModel model = new SegmentViewModel();
            var coworkerSegdata = _db.PerformanceSettings.Where(x => x.GuID == GUID && x.Archived == false).FirstOrDefault();
            EditSegmentViewModel model = new EditSegmentViewModel();
            if (!string.IsNullOrEmpty(coworkerSegdata.CoWorkerSegmentJSON))
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                var EmpPerformave = _db.PerformanceCustomerDetails.Where(x => x.Performance_Id == PerReviewId && x.InviteCustomer_Id == CustId && x.EmployeeId == EmpId && x.IsArchived == false).FirstOrDefault();
                model.CustomerSeg = js.Deserialize<List<CustomerSegment>>(coworkerSegdata.CustomerSegmentJSON);
                foreach (var item in model.CustomerSeg)
                {
                    item.CustomerQueListData = js.Deserialize<List<CustomerQuestionModel>>(item.QueationType);
                }
                model.JobRoleSeg = js.Deserialize<List<JobRoleSegment>>(coworkerSegdata.JobRoleSegmentJSON);
                foreach (var item in model.JobRoleSeg)
                {
                    item.JobRoleQueListData = js.Deserialize<List<JobRoleQuestionModel>>(item.QueationType);
                }
            }
            TempData["ReviewId"] = coworkerSegdata.Id;
            TempData["CustomerId"] = CustId;
            TempData["PerReviewId"] = PerReviewId;
            TempData["EmpId"] = EmpId;
            return View("CustomerPerformanceReview", model);
        }

        public ActionResult GetCustomerSegmentsDetailsById(int Id, string QueationType, int ReviewId, int Flag, int IsJobRoleCustomer, int CustomerId, int PerReviewId, int EmpId)
        {
            SegmentViewModel model = new SegmentViewModel();
            EditSegmentViewModel emodel = new EditSegmentViewModel();
            if (Flag == 2)
            {
                var overviewdata = _db.PerformanceSettings.Where(x => x.Id == ReviewId && x.Archived == false).FirstOrDefault();
                var EmployeePerOverviewDetails = _db.PerformanceCustomerDetails.Where(x => x.EmployeeId == EmpId && x.InviteCustomer_Id == CustomerId && x.Performance_Id == ReviewId && x.IsArchived == false).FirstOrDefault();
                if (overviewdata != null)
                {
                    if (!string.IsNullOrEmpty(overviewdata.OverallScoreJson))
                    {
                        string[] ScoreList = overviewdata.OverallScoreJson.Split('^');
                        List<KeyValue> listOfKeyValue = new List<KeyValue>();
                        for (int i = 0; i < ScoreList.Length; i++)
                        {
                            KeyValue _KeyValue = new KeyValue();
                            _KeyValue.Key = i;
                            _KeyValue.Value = ScoreList[i];
                            listOfKeyValue.Add(_KeyValue);
                        }
                        if (overviewdata.RatingOverAll == true)
                        {
                            model.OverallScoreList = listOfKeyValue;
                        }
                        if (overviewdata.RatingCore == true)
                        {
                            model.CoreScoreList = listOfKeyValue;
                        }
                        if (overviewdata.RatingJobRole == true)
                        {
                            model.JobRoleScoreList = listOfKeyValue;
                        }
                    }
                    if (EmployeePerOverviewDetails != null)
                    {
                        model.EmpPerDetailId = Convert.ToString(EmployeePerOverviewDetails.Id);
                        model.Comments = EmployeePerOverviewDetails.Comments;
                    }
                }
                model.Flag = 2;
            }
            else if (Flag == 1)
            {
                var overviewdata = _db.PerformanceSettings.Where(x => x.Id == ReviewId && x.Archived == false).FirstOrDefault();
                //if (IsJobRoleCustomer == 1)
                //{
                model.IsJobRoleCustomer = 1;
                var data = _db.PerformanceCustomerDetails.Where(x => x.InviteCustomer_Id == CustomerId && x.Performance_Id == PerReviewId && x.IsArchived == false).FirstOrDefault();
                JavaScriptSerializer js = new JavaScriptSerializer();
                model.CustomerQueList = js.Deserialize<List<CustomerQuestionModel>>(QueationType);
                model.JsonQuestionString = QueationType;
                if (data != null)
                {
                    //model.JSONCustomerSegmentString = js.Serialize(customerJSON.ToList());                        
                    foreach (var item in model.CustomerQueList)
                    {
                        int QueId = Convert.ToInt32(item.QueId);
                        var customerJSON = _db.EmployeePerformanceCustomerSegmentJSONDetails.Where(x => x.QueId == QueId && x.CustomerSegId == Id && x.Archived == false).ToList();
                        foreach (var customerJsonData in customerJSON)
                        {
                            int FiledId = Convert.ToInt32(item.FiledId);
                            item.QuFiledValue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).FirstOrDefault().Value;
                            item.Score = customerJsonData.Score;
                        }
                    }
                }
                else
                {
                    //model.JSONCustomerSegmentString = overviewdata.CustomerSegmentJSON;
                    foreach (var item in model.CustomerQueList)
                    {
                        int FiledId = Convert.ToInt32(item.FiledId);
                        item.QuFiledValue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).FirstOrDefault().Value;
                        item.Score = item.Score;
                    }
                }
                //}
                //else if(IsJobRoleCustomer==0)
                //{
                //    model.IsJobRoleCustomer = 0;
                //    model.JSONCustomerSegmentString = overviewdata.JobRoleSegmentJSON;
                //}

                model.Flag = 1;
            }
            else if (Flag == 0)
            {
                var overviewdata = _db.PerformanceSettings.Where(x => x.Id == ReviewId && x.Archived == false).FirstOrDefault();
                model.JSONCustomerSegmentString = overviewdata.CustomerSegmentJSON;
                if (!string.IsNullOrEmpty(QueationType))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    //if (IsJobRoleCustomer == 1)
                    //{
                    model.IsJobRoleCustomer = 1;
                    model.JsonQuestionString = QueationType;
                    var data = _db.PerformanceCustomerDetails.Where(x => x.InviteCustomer_Id == CustomerId && x.Performance_Id == PerReviewId && x.IsArchived == false).FirstOrDefault();
                    if (data != null)
                    {
                        //model.JSONCustomerSegmentString = js.Serialize(customerJSON.ToList());
                        model.JobroleQueList = js.Deserialize<List<JobRoleQuestionModel>>(QueationType);
                        foreach (var item in model.JobroleQueList)
                        {
                            int QueId = Convert.ToInt32(item.QueId);
                            var jboroleJSON = _db.EmployeePerformanceJobRoleSegmentJSONDetails.Where(x => x.PerCustomerdetailId == data.Id && x.Archived == false && x.QueId == QueId && x.JobRoleSegId == Id).ToList();
                            foreach (var jobroleJsonData in jboroleJSON)
                            {
                                int FiledId = Convert.ToInt32(item.FiledId);
                                item.QuFiledValue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).FirstOrDefault().Value;
                                item.Score = jobroleJsonData.Score;
                            }
                        }
                        model.IsJobRoleCustomer = 1;
                    }
                    else
                    {
                        model.JobroleQueList = js.Deserialize<List<JobRoleQuestionModel>>(QueationType);
                        foreach (var item in model.JobroleQueList)
                        {
                            int FiledId = Convert.ToInt32(item.FiledId);
                            item.QuFiledValue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).FirstOrDefault().Value;
                            item.Score = item.Score;
                        }
                    }
                    //else if (IsJobRoleCustomer == 0)
                    //{
                    //    model.IsJobRoleCustomer = 0;
                    //}
                    //model.CustomerQueList = js.Deserialize<List<CustomerQuestionModel>>(QueationType);
                    //foreach (var item in model.CustomerQueList)
                    //{
                    //    int FiledId = Convert.ToInt32(item.FiledId);
                    //    item.QuFiledValue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).FirstOrDefault().Value;
                    //}
                    //}
                }
                model.Flag = 0;
            }
            return PartialView("_PartialCustomerPerformanceReviewDetails", model);
        }
        //Customer Details Question Data
        public ActionResult getCustomerQuestionDataByID(int QuesId, int ReviewId, int EmpId, string QuestionData, int IsActivePastFlag, int Flag, int CustomerId, int PerReviewId)
        {

            SegmentViewModel model = new SegmentViewModel();
            int curruntUserId = SessionProxy.UserId;
            model.IsActivePastFlag = IsActivePastFlag;
            var data = _db.PerformanceCustomerDetails.Where(x => x.InviteCustomer_Id == CustomerId && x.Performance_Id == PerReviewId && x.IsArchived == false).FirstOrDefault();
            JavaScriptSerializer js = new JavaScriptSerializer();
            model.CustomerQueList = js.Deserialize<List<CustomerQuestionModel>>(QuestionData);

            if (Flag == 1)
            {
                var overviewdata = _db.PerformanceSettings.Where(x => x.Id == ReviewId && x.Archived == false).FirstOrDefault();
                model.IsJobRoleCustomer = 1;
                CustomerQuestionModel queModel = new CustomerQuestionModel();
                model.CustomerQueList = js.Deserialize<List<CustomerQuestionModel>>(QuestionData);
                string QId = Convert.ToString(QuesId);
                var qdata = model.CustomerQueList.Where(x => x.QueId == QId);
                foreach (var quesItem in qdata)
                {
                    queModel.FiledText = quesItem.FiledText;
                    queModel.FiledId = quesItem.FiledId;
                    queModel.QueId = quesItem.QueId;
                    queModel.questionData = quesItem.questionData.ToUpper();
                    //queModel.QuFiledValue = quesItem.QuFiledValue;
                    queModel.Score = quesItem.Score;
                    queModel.CValue = quesItem.CValue;
                }
                if (data != null)
                {
                    int QueId = Convert.ToInt32(QuesId);
                    var customerJSON = _db.EmployeePerformanceCustomerSegmentJSONDetails.Where(x => x.PerCustomerdetailId == data.Id && x.QueId == QueId && x.Archived == false).FirstOrDefault();
                    if (customerJSON != null)
                    {
                        int FiledId = Convert.ToInt32(queModel.FiledId);
                        queModel.QuFiledValue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).FirstOrDefault().Value;
                        queModel.Score = customerJSON.Score;
                        queModel.Comments = customerJSON.Comments;
                        model.EmpPerDetailId = Convert.ToString(data.Id);
                    }
                    else
                    {
                        var Qudata = model.CustomerQueList.Where(x => x.QueId == QId).FirstOrDefault();
                        int FiledId = Convert.ToInt32(Qudata.FiledId);
                        queModel.QuFiledValue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).FirstOrDefault().Value;
                        model.EmpPerDetailId = Convert.ToString(data.Id);
                    }
                }
                else
                {
                    var Qudata = model.CustomerQueList.Where(x => x.QueId == QId).FirstOrDefault();
                    int FiledId = Convert.ToInt32(Qudata.FiledId);
                    queModel.QuFiledValue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).FirstOrDefault().Value;
                }
                model.Flag = 1;
                model.IsJobRoleCustomer = 1;
                model.customerQuesModel = queModel;
            }
            else if (model.Flag == 0)
            {

                var overviewdata = _db.PerformanceSettings.Where(x => x.Id == ReviewId && x.Archived == false).FirstOrDefault();
                model.JSONCustomerSegmentString = overviewdata.CustomerSegmentJSON;
                JobRoleQuestionModel jobroleModel = new JobRoleQuestionModel();
                if (!string.IsNullOrEmpty(QuestionData))
                {
                    model.JobroleQueList = js.Deserialize<List<JobRoleQuestionModel>>(QuestionData);
                    model.TotalJobroleQuestionList = model.JobroleQueList.Count();
                    string QId = Convert.ToString(QuesId);
                    var qdata = model.JobroleQueList.Where(x => x.QueId == QId);
                    foreach (var quesItem in qdata)
                    {
                        jobroleModel.FiledText = quesItem.FiledText;
                        jobroleModel.FiledId = quesItem.FiledId;
                        jobroleModel.QueId = quesItem.QueId;
                        jobroleModel.questionData = quesItem.questionData;
                        jobroleModel.QuFiledValue = quesItem.QuFiledValue;
                        jobroleModel.Score = quesItem.Score;
                        jobroleModel.CValue = quesItem.CValue;
                    }
                    int QueId = Convert.ToInt32(QuesId);
                    if (data != null)
                    {
                        var jobroleJSON = _db.EmployeePerformanceJobRoleSegmentJSONDetails.Where(x => x.PerEmployeedetailId == data.Id && x.QueId == QueId && x.JobRoleSegId == QuesId && x.PerEmployeedetailId == EmpId && x.Archived == false).FirstOrDefault();
                        if (jobroleJSON != null)
                        {
                            int FiledId = Convert.ToInt32(jobroleModel.FiledId);
                            jobroleModel.QuFiledValue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).FirstOrDefault().Value;
                            jobroleModel.Score = jobroleModel.Score;
                            jobroleModel.Comments = jobroleJSON.Comment;
                            model.EmpPerDetailId = Convert.ToString(data.Id);
                        }
                        else
                        {
                            var JobRoleQudata = model.JobroleQueList.Where(x => x.QueId == QId).FirstOrDefault();
                            int FiledId = Convert.ToInt32(JobRoleQudata.FiledId);
                            jobroleModel.QuFiledValue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).FirstOrDefault().Value;
                            model.EmpPerDetailId = Convert.ToString(data.Id);
                        }
                    }
                    else
                    {
                        var JobRoleQudata = model.JobroleQueList.Where(x => x.QueId == QId).FirstOrDefault();
                        int FiledId = Convert.ToInt32(JobRoleQudata.FiledId);
                        jobroleModel.QuFiledValue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).FirstOrDefault().Value;
                        model.EmpPerDetailId = Convert.ToString(data.Id);
                    }
                }
                model.Flag = 0;
                model.IsJobRoleCustomer = 0;
                model.jobroleQueModel = jobroleModel;
            }
            return PartialView("_PartialCutomerPerformanceQuestionData", model);
        }

        public ActionResult saveCustomerInviteDetails(string OverallScoreString, string Comments, int PerReviewId, int CustomerId, int EmpId, string JSONCustomerSegment, string JSONJobRoleSegment, string EmpPerfDetailId)
        {
            SegmentViewModel emodel = new SegmentViewModel();
            int Id = _employeePerformanceMethod.saveInviteCustomer(OverallScoreString, Comments, PerReviewId, CustomerId, EmpId, JSONCustomerSegment, JSONJobRoleSegment, EmpPerfDetailId);
            emodel.Id = Id;
            emodel.ExistPerformanceId = Convert.ToString(Id);
            return Json(emodel, JsonRequestBehavior.AllowGet);
        }

        //Employee Details For Feedback
        public ActionResult GetEmployeeSegmentData(int EmpId, int ReviewId, int IsManagerEmployee, int IsActivePastFlag)
        {
            //  SegmentViewModel model = new SegmentViewModel();
            var coworkerSegdata = _db.PerformanceSettings.Where(x => x.Id == ReviewId && x.Archived == false).FirstOrDefault();
            EditSegmentViewModel model = new EditSegmentViewModel();
            if (IsManagerEmployee == 0)
            {
                model.IsManagerEmployee = 0;
            }
            else
            {
                model.IsManagerEmployee = 1;
            }
            if (IsManagerEmployee == 0)
            {
                model.IsManager_Id = 0;
            }
            else
            {
                model.IsManager_Id = 1;
            }
            if (!string.IsNullOrEmpty(coworkerSegdata.CoreSegmentJSON))
            {
                JavaScriptSerializer js = new JavaScriptSerializer();

                model.CoreSeg = js.Deserialize<List<CoreSegment>>(coworkerSegdata.CoreSegmentJSON);
                foreach (var item in model.CoreSeg)
                {
                    item.CoreQueListData = js.Deserialize<List<QuestionModel>>(item.QueationType);
                }
                model.JobRoleSeg = js.Deserialize<List<JobRoleSegment>>(coworkerSegdata.JobRoleSegmentJSON);
                foreach (var item in model.JobRoleSeg)
                {
                    item.JobRoleQueListData = js.Deserialize<List<JobRoleQuestionModel>>(item.QueationType);
                }
            }
            //TempData["ReviewId"] = coworkerSegdata.Id;
            //TempData["CustomerId"] = CustId;
            //TempData["PerReviewId"] = PerReviewId;
            //TempData["EmpId"] = EmpId;
            model.IsActivePastFlag = IsActivePastFlag;
            return PartialView("_PartialEmployeePerformanceReview", model);
        }
        public ActionResult GetEmployeeSegmentsDetailsById(int Id, string QueationType, int EmpPerfReviewId, int ReviewId, int Flag, int IsJobRoleCustomer, int EmpId, int IsManagerEmployee, int IsActivePastFlag, int IsManager)
        {

            if (IsManager == 1)
            {
                IsManager = 0;
            }
            else if (IsManager == 2)
            {
                IsManager = 1;
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            SegmentViewModel model = new SegmentViewModel();
            EditSegmentViewModel emodel = new EditSegmentViewModel();
            model.IsActivePastFlag = IsActivePastFlag;
            int curruntUserId = SessionProxy.UserId;
            if (Flag == 2)
            {
                var overviewdata = _db.PerformanceSettings.Where(x => x.Id == ReviewId && x.Archived == false).FirstOrDefault();
                var EmployeePerOverviewDetails = _db.PerformanceEmployeeDetails.Where(x => x.EmployeeId == EmpId && x.ReviewID == ReviewId && x.IsArchived == false && x.IsManager_Id == IsManager).OrderByDescending(y => y.UserIDLastModifiedDate).FirstOrDefault();
                if (overviewdata != null)
                {
                    if (!string.IsNullOrEmpty(overviewdata.OverallScoreJson))
                    {
                        string[] ScoreList = overviewdata.OverallScoreJson.Split('^');
                        List<KeyValue> listOfKeyValue = new List<KeyValue>();
                        for (int i = 0; i < ScoreList.Length; i++)
                        {
                            KeyValue _KeyValue = new KeyValue();
                            _KeyValue.Key = i;
                            _KeyValue.Value = ScoreList[i];
                            listOfKeyValue.Add(_KeyValue);
                        }
                        model.OverallScoreList = listOfKeyValue;
                        model.CoreScoreList = listOfKeyValue;
                        model.JobRoleScoreList = listOfKeyValue;
                        //if (overviewdata.RatingOverAll == true)
                        //{
                        //    model.OverallScoreList = listOfKeyValue;
                        //}
                        //if (overviewdata.RatingCore == true)
                        //{
                        //    model.CoreScoreList = listOfKeyValue;
                        //}
                        //if (overviewdata.RatingJobRole == true)
                        //{
                        //    model.JobRoleScoreList = listOfKeyValue;
                        //}
                    }
                    if (EmployeePerOverviewDetails != null)
                    {
                        model.EmpPerDetailId = Convert.ToString(EmployeePerOverviewDetails.Id);
                        model.Comments = EmployeePerOverviewDetails.Comments;
                        model.OverAllScoreJSON = EmployeePerOverviewDetails.OverAllScoreJSON;
                        string[] OverAllScoreJSONList = EmployeePerOverviewDetails.OverAllScoreJSON.Split('^');
                        for (int i = 0; i < OverAllScoreJSONList.Length; i++)
                        {
                            model.OverAllScore = OverAllScoreJSONList[0].ToString();
                            model.CoreScore = OverAllScoreJSONList[1].ToString();
                            model.JobRoleScore = OverAllScoreJSONList[2].ToString();

                        }
                    }
                }
                model.Flag = 2;
                model.IsJobRoleCustomer = 0;
            }
            else if (Flag == 1)
            {
                var overviewdata = _db.PerformanceSettings.Where(x => x.Id == ReviewId && x.Archived == false).FirstOrDefault();
                //if (IsJobRoleCustomer == 1)
                //{
                model.IsJobRoleCustomer = 1;
                var data = _db.PerformanceEmployeeDetails.Where(x => x.Performance_Id == EmpPerfReviewId && x.EmployeeId == EmpId && x.IsArchived == false && x.ReviewID == ReviewId && x.IsManager_Id == IsManager).FirstOrDefault();
                if (IsManagerEmployee == 1)
                {
                    data = _db.PerformanceEmployeeDetails.Where(x => x.Performance_Id == EmpPerfReviewId && x.Performance_ManagerId == curruntUserId && x.IsArchived == false && x.ReviewID == ReviewId && x.IsManager_Id == IsManager).FirstOrDefault();
                }
                else if (IsManagerEmployee == 0)
                {
                    data = _db.PerformanceEmployeeDetails.Where(x => x.Performance_Id == EmpPerfReviewId && x.EmployeeId == EmpId && x.IsArchived == false && x.ReviewID == ReviewId && x.IsManager_Id == IsManager).FirstOrDefault();
                }
                model.CoreQueList = js.Deserialize<List<QuestionModel>>(QueationType);
                model.TotalCoreQuestionList = model.CoreQueList.Count();
                model.JsonQuestionString = QueationType;
                foreach (var item in model.CoreQueList)
                {
                    int QueId = Convert.ToInt32(item.QueId);

                    var coreJSON = _db.EmployeePerformanceCoreJSONDetails.Where(x => x.QueId == QueId && x.IsManager_Id == IsManager && x.EmployeeID == EmpId && x.CoreId == Id && x.Archived == false && x.ReviewID == ReviewId).ToList();
                    if (coreJSON != null && coreJSON.Count > 0)
                    {
                        foreach (var customerJsonData in coreJSON)
                        {
                            int FiledId = Convert.ToInt32(item.FiledId);
                            item.QuFiledValue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).Select(x => x.Value).Take(1).SingleOrDefault();
                            item.Score = customerJsonData.Score;
                            item.Comments = customerJsonData.Comments;
                        }

                    }
                    else
                    {
                        foreach (var customerJsonData in model.CoreQueList)
                        {
                            int FiledId = Convert.ToInt32(item.FiledId);
                            var Qufilevalue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).Select(x => x.Value).Take(1).SingleOrDefault();
                            if (Qufilevalue != null)
                            {
                                item.QuFiledValue = Qufilevalue;
                            }
                            item.Score = customerJsonData.Score;
                        }
                    }


                }
                //}
                //else if(IsJobRoleCustomer==0)
                //{
                //    model.IsJobRoleCustomer = 0;
                //    model.JSONCustomerSegmentString = overviewdata.JobRoleSegmentJSON;
                //}

                model.Flag = 1;
                model.IsJobRoleCustomer = 1;
            }
            else if (Flag == 0)
            {
                var overviewdata = _db.PerformanceSettings.Where(x => x.Id == ReviewId && x.Archived == false).FirstOrDefault();
                model.JSONCustomerSegmentString = overviewdata.CustomerSegmentJSON;
                model.JsonQuestionString = QueationType;
                if (!string.IsNullOrEmpty(QueationType))
                {
                    //if (IsJobRoleCustomer == 1)
                    //{
                    var data = _db.PerformanceEmployeeDetails.Where(x => x.EmployeeId == EmpId && x.IsManager_Id == IsManager && x.Performance_Id == EmpPerfReviewId && x.IsArchived == false && x.ReviewID == ReviewId).FirstOrDefault();
                    //model.JSONCustomerSegmentString = js.Serialize(customerJSON.ToList());
                    model.JobroleQueList = js.Deserialize<List<JobRoleQuestionModel>>(QueationType);
                    model.TotalJobroleQuestionList = model.JobroleQueList.Count();
                    foreach (var item in model.JobroleQueList)
                    {
                        int QueId = Convert.ToInt32(item.QueId);
                        var jboroleJSON = _db.EmployeePerformanceJobRoleSegmentJSONDetails.Where(x => x.QueId == QueId && x.IsManager_Id == IsManager && x.EmployeeID == EmpId && x.JobRoleSegId == Id && x.Archived == false && x.ReviewID == ReviewId).ToList();
                        if (jboroleJSON != null && jboroleJSON.Count > 0)
                        {
                            foreach (var jobroleJsonData in jboroleJSON)
                            {
                                int FiledId = Convert.ToInt32(item.FiledId);
                                item.QuFiledValue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).FirstOrDefault().Value;
                                item.Score = jobroleJsonData.Score;
                            }
                        }
                        else
                        {
                            foreach (var customerJsonData in model.JobroleQueList)
                            {
                                int FiledId = Convert.ToInt32(item.FiledId);
                                var GetQuFileValue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).Take(1).SingleOrDefault();
                                if (GetQuFileValue != null)
                                {
                                    item.QuFiledValue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).FirstOrDefault().Value;
                                }
                                item.Score = customerJsonData.Score;
                            }
                        }
                    }
                }
                model.Flag = 0;
                model.IsJobRoleCustomer = 0;

            }
            var dataForCoreSeg = (from record in _db.PerformanceSettings where record.Id == ReviewId && record.Archived == false select record.CoreSegmentJSON).FirstOrDefault();
            var dataForJobRoleSeg = (from record in _db.PerformanceSettings where record.Id == ReviewId && record.Archived == false select record.JobRoleSegmentJSON).FirstOrDefault();
            EditSegmentViewModel segModel = new EditSegmentViewModel();
            String numberListCoreSeg = "";
            if (!string.IsNullOrEmpty(dataForCoreSeg))
            {

                segModel.CoreSeg = js.Deserialize<List<CoreSegment>>(dataForCoreSeg);
                foreach (var item in segModel.CoreSeg)
                {
                    item.CoreQueListData = js.Deserialize<List<QuestionModel>>(item.QueationType);
                    numberListCoreSeg += item.CoreQueListData.Count() + ",";
                }
            }
            String numberListjobrole = "";

            if (!string.IsNullOrEmpty(dataForJobRoleSeg))
            {
                segModel.JobRoleSeg = js.Deserialize<List<JobRoleSegment>>(dataForJobRoleSeg);
                foreach (var item in segModel.JobRoleSeg)
                {
                    item.JobRoleQueListData = js.Deserialize<List<JobRoleQuestionModel>>(item.QueationType);
                    numberListjobrole += item.JobRoleQueListData.Count() + ",";
                }
            }
            if (numberListCoreSeg != null && numberListCoreSeg != string.Empty)
            {
                model.coreSegOriginalData = numberListCoreSeg.Substring(0, numberListCoreSeg.Count() - 1);
            }
            if (numberListjobrole != null && numberListjobrole != string.Empty)
            {
                model.JobRoleOriginalData = numberListjobrole.Substring(0, numberListjobrole.Count() - 1);
            }


            var performanceEmployeeDetailslist = (from record in _db.PerformanceEmployeeDetails where record.ReviewID == ReviewId && record.IsManager_Id == IsManager && record.EmployeeId == EmpId && record.IsFormCompleted == true && record.IsArchived == false && record.IsManager_Id == 0 select record.Id).SingleOrDefault();
            int coreCount;
            string resultCore = "";
            segModel.CoreSeg = js.Deserialize<List<CoreSegment>>(dataForCoreSeg);
            foreach (var item in segModel.CoreSeg)
            {
                coreCount = (from record in _db.EmployeePerformanceCoreJSONDetails where record.ReviewID == ReviewId && record.IsManager_Id == IsManager && record.EmployeeID == EmpId && record.Archived == false && record.IsFormCompleted == true && record.CoreId == item.CoreId select record.QueId).Count();
                resultCore += coreCount + ",";
            }

            //var coreSegData = (from record in _db.EmployeePerformanceCoreJSONDetails where record.ReviewID == ReviewId && record.IsFormCompleted == true select record.CoreId).ToList();

            //foreach (var item in coreSegData)
            //{


            //}
            //var jobRoleSegData = (from record in _db.EmployeePerformanceJobRoleSegmentJSONDetails where record.ReviewID == ReviewId && record.IsFormCompleted == true select record.JobRoleSegId).ToList();
            int jobRoleCount;
            string resultJobRole = "";
            segModel.JobRoleSeg = js.Deserialize<List<JobRoleSegment>>(dataForJobRoleSeg);
            foreach (var item in segModel.JobRoleSeg)
            {
                jobRoleCount = (from record in _db.EmployeePerformanceJobRoleSegmentJSONDetails where record.ReviewID == ReviewId && record.IsManager_Id == IsManager && record.EmployeeID == EmpId && record.Archived == false && record.IsFormCompleted == true && record.JobRoleSegId == item.JobRoleIds select record.QueId).Count();
                resultJobRole += jobRoleCount + ",";
            }

            //foreach (var item in jobRoleSegData)
            //{


            //}
            if (performanceEmployeeDetailslist != 0 || performanceEmployeeDetailslist != null)
            {

                model.performanceEmployeeDetailslist = Convert.ToString(performanceEmployeeDetailslist);
            }
            if (resultCore != string.Empty)
            {
                model.coreSegData = resultCore.Substring(0, resultCore.Count() - 1);
            }
            if (resultJobRole != string.Empty)
            {
                model.jobRoleSegData = resultJobRole.Substring(0, resultJobRole.Count() - 1);
            }

            return PartialView("_PartialEmployeePerformanceSegmentDetails", model);
        }

        //Employee Details QuestionData
        public ActionResult getQuestionDataByID(int coreORjobroleID, int QuesId, int ReviewId, int EmpId, string QuestionData, int IsActivePastFlag, int Flag, int EmpPerReview, int IsManager)
        {


            if (IsManager == 1)
            {
                IsManager = 0;
            }
            else if (IsManager == 2)
            {
                IsManager = 1;
            }
            SegmentViewModel model = new SegmentViewModel();
            int curruntUserId = SessionProxy.UserId;
            model.IsActivePastFlag = IsActivePastFlag;
            var data = _db.PerformanceEmployeeDetails.Where(x => x.Performance_Id == EmpPerReview && x.IsManager_Id == IsManager && x.EmployeeId == EmpId && x.IsArchived == false && x.ReviewID == ReviewId).ToList();
            if (Flag == 1)
            {
                var overviewdata = _db.PerformanceSettings.Where(x => x.Id == ReviewId && x.Archived == false).FirstOrDefault();
                model.IsJobRoleCustomer = 1;
                JavaScriptSerializer js = new JavaScriptSerializer();
                QuestionModel queModel = new QuestionModel();
                model.CoreQueList = js.Deserialize<List<QuestionModel>>(QuestionData);
                model.TotalCoreQuestionList = model.CoreQueList.Count();
                string QId = Convert.ToString(QuesId);
                var qdata = model.CoreQueList.Where(x => x.QueId == QId);
                foreach (var quesItem in qdata)
                {
                    queModel.FiledText = quesItem.FiledText;
                    queModel.FiledId = quesItem.FiledId;
                    queModel.QueId = quesItem.QueId;
                    queModel.questionData = quesItem.questionData.ToUpper();
                    // queModel.QuFiledValue = quesItem.QuFiledValue;
                    queModel.Comments = quesItem.Comments;
                    queModel.Score = quesItem.Score;
                    queModel.CValue = quesItem.CValue;
                }
                int QueId = Convert.ToInt32(QuesId);


                var coreJSON = _db.EmployeePerformanceCoreJSONDetails.Where(x => x.QueId == QueId && x.IsManager_Id == IsManager && x.EmployeeID == EmpId && x.Archived == false && x.ReviewID == ReviewId && x.CoreId == coreORjobroleID).FirstOrDefault();
                if (coreJSON != null)
                {
                    int FiledId = Convert.ToInt32(queModel.FiledId);
                    var Qufilevalue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).Select(x => x.Value).Take(1).SingleOrDefault();
                    if (Qufilevalue != null)
                    {
                        queModel.QuFiledValue = Qufilevalue;
                    }
                    queModel.Score = coreJSON.Score;
                    queModel.Comments = coreJSON.Comments;
                    if (data.Count() != 0)
                    {

                        model.EmpPerDetailId = Convert.ToString(data.FirstOrDefault().Id);
                    }
                }
                else
                {
                    var CoreQuestionData = model.CoreQueList.Where(x => x.QueId == QId).FirstOrDefault();
                    int FiledId = Convert.ToInt32(CoreQuestionData.FiledId);
                    var Qufilevalue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).Select(x => x.Value).Take(1).SingleOrDefault();
                    if (Qufilevalue != null)
                    {
                        queModel.QuFiledValue = Qufilevalue;
                    }
                    if (data.Count() != 0)
                    {
                        model.EmpPerDetailId = Convert.ToString(data.FirstOrDefault().Id);
                    }
                }


                //else
                //{
                //    var CoreQuestionData = model.CoreQueList.Where(x => x.QueId == QId).FirstOrDefault();
                //    int FiledId = Convert.ToInt32(CoreQuestionData.FiledId);
                //    queModel.QuFiledValue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).FirstOrDefault().Value;
                //}
                model.Flag = 1;
                model.IsJobRoleCustomer = 1;
                model.questionsModel = queModel;
            }
            else if (model.Flag == 0)
            {

                var overviewdata = _db.PerformanceSettings.Where(x => x.Id == ReviewId && x.Archived == false).FirstOrDefault();
                model.JSONCustomerSegmentString = overviewdata.CustomerSegmentJSON;
                JobRoleQuestionModel jobroleModel = new JobRoleQuestionModel();
                if (!string.IsNullOrEmpty(QuestionData))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    model.JobroleQueList = js.Deserialize<List<JobRoleQuestionModel>>(QuestionData);
                    model.TotalJobroleQuestionList = model.JobroleQueList.Count();
                    string QId = Convert.ToString(QuesId);
                    var qdata = model.JobroleQueList.Where(x => x.QueId == QId);
                    foreach (var quesItem in qdata)
                    {
                        jobroleModel.FiledText = quesItem.FiledText;
                        jobroleModel.FiledId = quesItem.FiledId;
                        jobroleModel.QueId = quesItem.QueId;
                        jobroleModel.questionData = quesItem.questionData;
                        // jobroleModel.QuFiledValue = quesItem.QuFiledValue;
                        jobroleModel.Comments = quesItem.Comments;
                        jobroleModel.Score = quesItem.Score;
                        jobroleModel.CValue = quesItem.CValue;
                    }
                    int QueId = Convert.ToInt32(QuesId);

                    var jobroleJSON = _db.EmployeePerformanceJobRoleSegmentJSONDetails.Where(x => x.QueId == QueId && x.IsManager_Id == IsManager && x.EmployeeID == EmpId && x.Archived == false && x.ReviewID == ReviewId && x.JobRoleSegId == coreORjobroleID).FirstOrDefault();
                    if (jobroleJSON != null)
                    {
                        int FiledId = Convert.ToInt32(jobroleModel.FiledId);
                        jobroleModel.QuFiledValue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).FirstOrDefault().Value;
                        jobroleModel.Score = jobroleJSON.Score;
                        jobroleModel.Comments = jobroleJSON.Comment;
                    }
                    else
                    {
                        var jobroleQuestion = model.JobroleQueList.Where(x => x.QueId == QId).FirstOrDefault();
                        int FiledId = Convert.ToInt32(jobroleQuestion.FiledId);
                        var Qufilevalue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).Select(x => x.Value).Take(1).SingleOrDefault();
                        if (Qufilevalue != null)
                        {
                            jobroleModel.QuFiledValue = Qufilevalue.ToString();
                        }
                    }
                    if (data.Count() != 0)
                    {

                        model.EmpPerDetailId = Convert.ToString(data.FirstOrDefault().Id);
                    }


                    else
                    {
                        var jobroleQuestion = model.JobroleQueList.Where(x => x.QueId == QId).FirstOrDefault();
                        int FiledId = Convert.ToInt32(jobroleQuestion.FiledId);
                        jobroleModel.QuFiledValue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).FirstOrDefault().Value;
                    }
                }
                model.Flag = 0;
                model.IsJobRoleCustomer = 0;
                model.jobroleQueModel = jobroleModel;
            }
            return PartialView("_PartialEmployeePerformanceQuestionData", model);
        }
        public JsonResult saveEmployeePerformanceDetails(int PerformanceID, string OverallScoreString, string Comments, int EmpId, string JSONCustomerSegment, string JSONJobRoleSegment, int ReviewID, int IsManagerEmployee, int Flag, int IsActivePastFlag, int EmpPerfDetailId, int EmployeePerformaceID)
        {
            string reportToId = _db.EmployeeRelations.Where(x => x.UserID == EmpId && x.IsActive == true).FirstOrDefault().Reportsto.ToString();
            int reportTo = Convert.ToInt32(reportToId);
            SegmentViewModel model = new SegmentViewModel();
            int Id = _employeePerformanceMethod.saveEmployeePerformance(PerformanceID, Flag, OverallScoreString, Comments, EmpId, JSONCustomerSegment, JSONJobRoleSegment, ReviewID, IsManagerEmployee, EmpPerfDetailId, EmployeePerformaceID);
            model.ExistPerformanceId = Convert.ToString(Id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        //Print Employee Performance

        public ActionResult PrintEmployeePerformance(int EmpId, int ReviewId, string EmpPerfId)
        {
            PrintPerformancePDF model = new PrintPerformancePDF();
            int EmpPerID = Convert.ToInt32(EmpPerfId);
            var ReviewData = _db.PerformanceSettings.Where(x => x.Id == ReviewId && x.Archived == false).FirstOrDefault();
            var IsReviewEmployee = _db.PerformanceEmployeeDetails.Where(x => x.EmployeeId == EmpId && x.Performance_Id == EmpPerID && x.IsArchived == false).FirstOrDefault();
            var EmployeeName = _db.AspNetUsers.Where(x => x.Id == EmpId && x.Archived == false).FirstOrDefault();
            DateTime curruntDate = DateTime.Now;

            if (IsReviewEmployee != null)
            {
                var ManagerName = _db.AspNetUsers.Where(x => x.Id == IsReviewEmployee.Performance_ManagerId && x.Archived == false).FirstOrDefault();
                var forCustomerName = _db.PerformanceCustomerDetails.Where(x => x.EmployeeId == EmpId && x.IsArchived == false).FirstOrDefault();
                if (forCustomerName != null)
                {
                    var CustomerName = _db.AspNetUsers.Where(x => x.Id == forCustomerName.InviteCustomer_Id && x.Archived == false).FirstOrDefault();
                    if (CustomerName != null)
                    {
                        model.CustomerName = CustomerName.FirstName + " " + CustomerName.LastName;
                    }
                }
                if (EmployeeName != null)
                {
                    model.EmployeeName = EmployeeName.FirstName + " " + EmployeeName.LastName;
                }
                if (ManagerName != null)
                {
                    model.ManagerName = ManagerName.FirstName + " " + ManagerName.LastName;
                }
            }
            if (IsReviewEmployee != null)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                model.CoreSeg = js.Deserialize<List<CoreSegment>>(ReviewData.CoreSegmentJSON);
                foreach (var item in model.CoreSeg)
                {
                    item.CoreQueListData = js.Deserialize<List<QuestionModel>>(item.QueationType);
                    foreach (var eitem in item.CoreQueListData)
                    {
                        int QueId = Convert.ToInt32(eitem.QueId);
                        if (IsReviewEmployee != null)
                        {
                            var coreJSON = _db.EmployeePerformanceCoreJSONDetails.Where(x => x.QueId == QueId && x.PerEmployeedetailId == IsReviewEmployee.Id && x.Archived == false).ToList();
                            if (coreJSON != null && coreJSON.Count > 0)
                            {
                                foreach (var customerJsonData in coreJSON)
                                {
                                    int FiledId = Convert.ToInt32(eitem.FiledId);
                                    eitem.QuFiledValue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).FirstOrDefault().Value;
                                    eitem.Comments = customerJsonData.Comments;
                                    eitem.Score = customerJsonData.Score;
                                }
                            }
                        }
                    }
                }
                model.JobRoleSeg = js.Deserialize<List<JobRoleSegment>>(ReviewData.JobRoleSegmentJSON);
                foreach (var jobRole in model.JobRoleSeg)
                {
                    jobRole.JobRoleQueListData = js.Deserialize<List<JobRoleQuestionModel>>(jobRole.QueationType);
                    foreach (var item in jobRole.JobRoleQueListData)
                    {
                        int QueId = Convert.ToInt32(item.QueId);
                        var jboroleJSON = _db.EmployeePerformanceJobRoleSegmentJSONDetails.Where(x => x.QueId == QueId && x.PerEmployeedetailId == IsReviewEmployee.Id && x.PerEmployeedetailId == EmpId && x.Archived == false).ToList();
                        if (jboroleJSON != null && jboroleJSON.Count > 0)
                        {
                            foreach (var jobroleJsonData in jboroleJSON)
                            {
                                int FiledId = Convert.ToInt32(item.FiledId);
                                item.QuFiledValue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).FirstOrDefault().Value;
                                item.Score = jobroleJsonData.Score;
                            }
                        }
                        else
                        {
                            foreach (var customerJsonData in jobRole.JobRoleQueListData)
                            {
                                int FiledId = Convert.ToInt32(item.FiledId);
                                item.QuFiledValue = _db.SystemListValues.Where(x => x.Id == FiledId && x.Archived == false).FirstOrDefault().Value;
                                item.Score = customerJsonData.Score;
                            }
                        }
                    }
                }
            }
            string newfileName = string.Format("" + EmployeeName.FirstName + "_" + EmployeeName.LastName + "_Comments.pdf", curruntDate.Date);
            return new Rotativa.ViewAsPdf("EmployeePerformanceReviewPDF", model)
            {
                PageSize = Size.A4,
                PageOrientation = Orientation.Landscape,
                FileName = newfileName
            };
        }


        //Share Employee Review
        public ActionResult ShareEmployeeReview(int ReviewId, int EmpId, int ManagerId)
        {
            ShareEmployeeReview model = new ShareEmployeeReview();
            var EmpRelation = _db.EmployeeRelations.Where(x => x.UserID == EmpId && x.IsActive == true).FirstOrDefault();
            if (EmpRelation != null)
            {
                var ManagerName = _db.AspNetUsers.Where(x => x.Id == EmpRelation.Reportsto && x.Archived == false).FirstOrDefault();
                model.ManagerName = ManagerName.FirstName + " " + ManagerName.LastName;
                model.MaanegrId = ManagerName.Id;
                model.EmployeeId = EmpId;
            }
            return PartialView("_PartialSharEmployeePerformance", model);
        }

        public ActionResult sendMailtoshareperformnce(int ManagerId, int EmployeeId)
        {
            ShareEmployeeReview model = new ShareEmployeeReview();
            int CurruntUser = SessionProxy.UserId;
            string EmpID = Convert.ToString(EmployeeId);
            var CustomerData = _db.AspNetUsers.Where(x => x.Id == CurruntUser && x.SSOID.StartsWith("C") && x.Archived == false).FirstOrDefault();
            var EmployeeData = _db.AspNetUsers.Where(x => x.Id == EmployeeId && x.Archived == false).FirstOrDefault();
            var ManagerData = _db.AspNetUsers.Where(x => x.Id == ManagerId && x.Archived == false).FirstOrDefault();
            if (ManagerId != 0 && ManagerId != null)
            {
                if (EmployeeId == SessionProxy.UserId)
                {
                    var FromData = _db.AspNetUsers.Where(x => x.Id == SessionProxy.UserId && x.Archived == false).FirstOrDefault();
                    var SendMailCustomerData = _db.AspNetUsers.Where(x => x.CustomerCareID.Contains(EmpID) && x.SSOID.StartsWith("C") && x.Archived == false).ToList();
                    foreach (var item in SendMailCustomerData)
                    {
                        var EmpShareCustomerData = _db.AspNetUsers.Where(x => x.Id == item.Id && x.Archived == false).FirstOrDefault();
                        HRTool.Models.MailModel custmail = new HRTool.Models.MailModel();
                        using (StreamReader stramReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/SendMailToManagerPerformanceShare.html")))
                        {
                            custmail.Body = stramReader.ReadToEnd();
                        }
                        custmail.From = FromData.UserName;
                        custmail.To = EmpShareCustomerData.UserName;
                        custmail.Subject = EmployeeData.FirstName + " " + EmployeeData.LastName + "Self-Assessment is Ready";
                        custmail.Body = custmail.Body.Replace("{1}", EmployeeData.FirstName + " " + EmployeeData.LastName);
                        custmail.Body = custmail.Body.Replace("{0}", EmpShareCustomerData.FirstName + " " + EmpShareCustomerData.LastName);
                        string custmailToReceive = Common.sendMail(custmail);
                    }
                    HRTool.Models.MailModel mail = new HRTool.Models.MailModel();
                    using (StreamReader stramReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/SendMailToManagerPerformanceShare.html")))
                    {
                        mail.Body = stramReader.ReadToEnd();
                    }
                    mail.From = FromData.UserName;
                    mail.To = ManagerData.UserName;
                    mail.Subject = FromData.FirstName + " " + FromData.LastName + "Self-Assessment is Ready";
                    mail.Body = mail.Body.Replace("{1}", EmployeeData.FirstName + " " + EmployeeData.LastName);
                    mail.Body = mail.Body.Replace("{0}", ManagerData.FirstName + " " + ManagerData.LastName);
                    string mailToReceive = Common.sendMail(mail);
                }
                else if (CustomerData != null)
                {
                    var FromData = _db.AspNetUsers.Where(x => x.Id == CustomerData.Id && x.Archived == false).FirstOrDefault();
                    HRTool.Models.MailModel mail = new HRTool.Models.MailModel();
                    using (StreamReader stramReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/SendMailFromCustomerShare.html")))
                    {
                        mail.Body = stramReader.ReadToEnd();
                    }
                    mail.From = FromData.UserName;
                    mail.To = EmployeeData.UserName;
                    mail.Subject = FromData.FirstName + " " + FromData.LastName + " Has Shared Your Performance Review";
                    mail.Body = mail.Body.Replace("{1}", CustomerData.FirstName + " " + CustomerData.LastName);
                    mail.Body = mail.Body.Replace("{0}", EmployeeData.FirstName + " " + EmployeeData.LastName);
                    string mailToReceiveEmployee = Common.sendMail(mail);
                    var FromCutomerToManager = _db.AspNetUsers.Where(x => x.Id == ManagerId && x.Archived == false).FirstOrDefault();
                    HRTool.Models.MailModel Managermail = new HRTool.Models.MailModel();
                    using (StreamReader stramReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/SendMailFromCustomerToManagerPerformanceShare.html")))
                    {
                        Managermail.Body = stramReader.ReadToEnd();
                    }
                    Managermail.From = FromData.UserName;
                    Managermail.To = FromCutomerToManager.UserName;
                    Managermail.Subject = FromData.FirstName + " " + FromData.LastName + " Has Shared Performance Review";
                    Managermail.Body = Managermail.Body.Replace("{1}", CustomerData.FirstName + " " + CustomerData.LastName);
                    Managermail.Body = Managermail.Body.Replace("{2}", FromData.FirstName + " " + FromData.LastName);
                    Managermail.Body = Managermail.Body.Replace("{0}", FromCutomerToManager.FirstName + " " + FromCutomerToManager.LastName);
                    string mailToReceive = Common.sendMail(Managermail);
                }
                else
                {
                    var FromData = _db.AspNetUsers.Where(x => x.Id == ManagerId && x.Archived == false).FirstOrDefault();
                    HRTool.Models.MailModel mail = new HRTool.Models.MailModel();
                    using (StreamReader stramReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/SendMailFromManagerShare.html")))
                    {
                        mail.Body = stramReader.ReadToEnd();
                    }
                    mail.From = FromData.UserName;
                    mail.To = EmployeeData.UserName;
                    mail.Subject = "Your Performance Review is Ready";
                    mail.Body = mail.Body.Replace("{1}", ManagerData.FirstName + " " + ManagerData.LastName);
                    mail.Body = mail.Body.Replace("{0}", EmployeeData.FirstName + " " + EmployeeData.LastName);
                    string mailToReceive = Common.sendMail(mail);
                    //SendMailFromManagerToCustomerPerformanceShare                                        
                    var ManagerToCustomerMailData = _db.AspNetUsers.Where(x => x.CustomerCareID.Contains(EmpID) && x.SSOID.StartsWith("C") && x.Archived == false).ToList();
                    foreach (var item in ManagerToCustomerMailData)
                    {
                        var EmpShareCustomerData = _db.AspNetUsers.Where(x => x.Id == item.Id && x.Archived == false).FirstOrDefault();
                        HRTool.Models.MailModel custmail = new HRTool.Models.MailModel();
                        using (StreamReader stramReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/SendMailFromManagerToCustomerPerformanceShare.html")))
                        {
                            custmail.Body = stramReader.ReadToEnd();
                        }
                        custmail.From = FromData.UserName;
                        custmail.To = EmpShareCustomerData.UserName;
                        custmail.Subject = EmployeeData.FirstName + " " + EmployeeData.LastName + "Performance Review is Ready";
                        custmail.Body = custmail.Body.Replace("{1}", FromData.FirstName + " " + FromData.LastName);
                        custmail.Body = custmail.Body.Replace("{2}", EmployeeData.FirstName + " " + EmployeeData.LastName);
                        custmail.Body = custmail.Body.Replace("{0}", EmpShareCustomerData.FirstName + " " + EmpShareCustomerData.LastName);
                        string custmailToReceive = Common.sendMail(custmail);
                    }
                }
            }
            return PartialView("_PartialSharEmployeePerformance", model);

        }
        //UnShare Employee Review
        public ActionResult UnShareEmployeeReview(int ReviewId, int EmpId)
        {
            ShareEmployeeReview model = new ShareEmployeeReview();
            var EmpRelation = _db.EmployeeRelations.Where(x => x.UserID == EmpId && x.IsActive == true).FirstOrDefault();
            if (EmpRelation != null)
            {
                var ManagerName = _db.AspNetUsers.Where(x => x.Id == EmpRelation.Reportsto && x.Archived == false).FirstOrDefault();
                model.ManagerName = ManagerName.FirstName + " " + ManagerName.LastName;
                model.MaanegrId = ManagerName.Id;
                model.EmployeeId = EmpId;
            }
            return PartialView("_PartialUnShareEmployeePerformance", model);
        }
        public ActionResult sendMailtoUnshareperformnce(int ManagerId, int EmployeeId)
        {
            ShareEmployeeReview model = new ShareEmployeeReview();
            string EmpID = Convert.ToString(EmployeeId);
            int CurrentUser = SessionProxy.UserId;
            var EmployeeData = _db.AspNetUsers.Where(x => x.Id == EmployeeId && x.Archived == false).FirstOrDefault();
            var ManagerData = _db.AspNetUsers.Where(x => x.Id == ManagerId && x.Archived == false).FirstOrDefault();
            var CustomerData = _db.AspNetUsers.Where(x => x.Id == CurrentUser && x.SSOID.StartsWith("C") && x.Archived == false).FirstOrDefault();
            if (ManagerId != 0 && ManagerId != null)
            {
                if (EmployeeId == SessionProxy.UserId)
                {
                    //Employee Un-share To Manager
                    var FromData = _db.AspNetUsers.Where(x => x.Id == SessionProxy.UserId && x.Archived == false).FirstOrDefault();
                    HRTool.Models.MailModel mail = new HRTool.Models.MailModel();
                    using (StreamReader stramReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/SendMailToManagerPerformanceUnShare.html")))
                    {
                        mail.Body = stramReader.ReadToEnd();
                    }
                    mail.From = FromData.UserName;
                    mail.To = ManagerData.UserName;
                    mail.Subject = FromData.FirstName + " " + FromData.LastName + "Self-Assessment Needs More Time";
                    mail.Body = mail.Body.Replace("{1}", EmployeeData.FirstName + " " + EmployeeData.LastName);
                    mail.Body = mail.Body.Replace("{0}", ManagerData.FirstName + " " + ManagerData.LastName);
                    string mailToReceive = Common.sendMail(mail);
                    //Employee Un-Share to customer
                    var EmployeeToCustomer = _db.AspNetUsers.Where(x => x.CustomerCareID.Contains(EmpID) && x.SSOID.StartsWith("C")).ToList();
                    foreach (var item in EmployeeToCustomer)
                    {
                        var FromEmployeeToCustomer = _db.AspNetUsers.Where(x => x.Id == item.Id && x.Archived == false).FirstOrDefault();
                        HRTool.Models.MailModel customerMail = new HRTool.Models.MailModel();
                        using (StreamReader stramReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/SendMailToManagerPerformanceUnShare.html")))
                        {
                            customerMail.Body = stramReader.ReadToEnd();
                        }
                        customerMail.From = FromData.UserName;
                        customerMail.To = FromEmployeeToCustomer.UserName;
                        customerMail.Subject = FromData.FirstName + " " + FromData.LastName + "Self-Assessment Needs More Time";
                        customerMail.Body = mail.Body.Replace("{1}", EmployeeData.FirstName + " " + EmployeeData.LastName);
                        customerMail.Body = mail.Body.Replace("{0}", FromEmployeeToCustomer.FirstName + " " + FromEmployeeToCustomer.LastName);
                        string mailToCustomerReceive = Common.sendMail(customerMail);
                    }
                }
                else if (CustomerData != null)
                {
                    //Un-share To Employee
                    var FromData = _db.AspNetUsers.Where(x => x.Id == CustomerData.Id && x.Archived == false).FirstOrDefault();
                    HRTool.Models.MailModel mail = new HRTool.Models.MailModel();
                    using (StreamReader stramReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/SendMailToManagerPerformanceUnShare.html")))
                    {
                        mail.Body = stramReader.ReadToEnd();
                    }
                    mail.From = FromData.UserName;
                    mail.To = EmployeeData.UserName;
                    mail.Subject = FromData.FirstName + " " + FromData.LastName + " is Editing Your Performance Review";
                    mail.Body = mail.Body.Replace("{1}", CustomerData.FirstName + " " + CustomerData.LastName);
                    mail.Body = mail.Body.Replace("{0}", EmployeeData.FirstName + " " + EmployeeData.LastName);
                    string mailToReceive = Common.sendMail(mail);
                    //Un-share To Manager
                    var FromCutomerToManager = _db.AspNetUsers.Where(x => x.Id == ManagerId && x.Archived == false).FirstOrDefault();
                    HRTool.Models.MailModel Managermail = new HRTool.Models.MailModel();
                    using (StreamReader stramReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/SendMailFromCustomerToManagerUnShare.html")))
                    {
                        Managermail.Body = stramReader.ReadToEnd();
                    }
                    Managermail.From = FromData.UserName;
                    Managermail.To = FromCutomerToManager.UserName;
                    Managermail.Subject = FromData.FirstName + " " + FromData.LastName + " is Editing" + EmployeeData.FirstName + " " + EmployeeData.LastName + "Performance Review ";
                    Managermail.Body = Managermail.Body.Replace("{1}", CustomerData.FirstName + " " + CustomerData.LastName);
                    Managermail.Body = Managermail.Body.Replace("{2}", EmployeeData.FirstName + " " + EmployeeData.LastName);
                    Managermail.Body = Managermail.Body.Replace("{0}", FromCutomerToManager.FirstName + " " + FromCutomerToManager.LastName);
                    string mailToManagerReceive = Common.sendMail(Managermail);
                }
                else
                {
                    //Manager Un-share to Employee
                    var FromData = _db.AspNetUsers.Where(x => x.Id == ManagerId && x.Archived == false).FirstOrDefault();
                    HRTool.Models.MailModel mail = new HRTool.Models.MailModel();
                    using (StreamReader stramReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/SendMailFromManagerUnShare.html")))
                    {
                        mail.Body = stramReader.ReadToEnd();
                    }
                    mail.From = FromData.UserName;
                    mail.To = EmployeeData.UserName;
                    mail.Subject = "Manager UnShare Performance mail";
                    mail.Body = mail.Body.Replace("{1}", ManagerData.FirstName + " " + ManagerData.LastName);
                    mail.Body = mail.Body.Replace("{0}", EmployeeData.FirstName + " " + EmployeeData.LastName);
                    string mailToReceive = Common.sendMail(mail);

                    //Manager Un-share to Customer
                    var EmployeeToCustomer = _db.AspNetUsers.Where(x => x.CustomerCareID.Contains(EmpID) && x.SSOID.StartsWith("C")).ToList();
                    foreach (var item in EmployeeToCustomer)
                    {
                        var FromEmployeeToCustomer = _db.AspNetUsers.Where(x => x.Id == item.Id && x.Archived == false).FirstOrDefault();
                        HRTool.Models.MailModel customerMail = new HRTool.Models.MailModel();
                        using (StreamReader stramReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/SendMailFromCustomerToManagerUnShare.html")))
                        {
                            customerMail.Body = stramReader.ReadToEnd();
                        }
                        customerMail.From = FromData.UserName;
                        customerMail.To = FromEmployeeToCustomer.UserName;
                        customerMail.Subject = FromData.FirstName + " " + FromData.LastName + " is Editing Your Performance Review";
                        customerMail.Body = mail.Body.Replace("{1}", ManagerData.FirstName + " " + ManagerData.LastName);
                        customerMail.Body = mail.Body.Replace("{2}", EmployeeData.FirstName + " " + EmployeeData.LastName);
                        customerMail.Body = mail.Body.Replace("{0}", FromEmployeeToCustomer.FirstName + " " + FromEmployeeToCustomer.LastName);
                        string mailToCustomerReceive = Common.sendMail(customerMail);
                    }

                }
            }
            return PartialView("_PartialUnShareEmployeePerformance", model);

        }

        public ActionResult ClosePerformanceReview(int ReviewId, int EmpId)
        {
            EmployeePerformanceViewModel model = new EmployeePerformanceViewModel();
            model.ReviewId = ReviewId;
            model.EmployeeId = EmpId;
            return PartialView("_PartialClosePerformanceReview", model);
        }

        public ActionResult SavecloseEmployeePerformanceReview(int PerReviewId, int EmplyeeId, int ManagerID)
        {
            EmployeePerformanceViewModel model = new EmployeePerformanceViewModel();
            string EmployeeId = Convert.ToString(EmplyeeId);
            bool updateData = _employeePerformanceMethod.CloseEmployeePerformace(PerReviewId, EmplyeeId);
            var FromData = _db.AspNetUsers.Where(x => x.Id == ManagerID && x.Archived == false).FirstOrDefault();
            if (updateData == true)
            {
                var toData = _db.AspNetUsers.Where(x => x.Id == EmplyeeId && x.Archived == false).FirstOrDefault();
                HRTool.Models.MailModel mail = new HRTool.Models.MailModel();
                using (StreamReader stramReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/SendMailFromMangerCloseReview.html")))
                {
                    mail.Body = stramReader.ReadToEnd();
                }
                mail.From = FromData.UserName;
                mail.To = toData.UserName;
                mail.Subject = "Close Performance";
                mail.Body = mail.Body.Replace("{0}", toData.FirstName + " " + toData.LastName);
                mail.Body = mail.Body.Replace("{1}", FromData.FirstName + " " + FromData.LastName);
                string mailToReceive = Common.sendMail(mail);
                var SendMailCustomerData = _db.AspNetUsers.Where(x => x.CustomerCareID.Contains(EmployeeId) && x.SSOID.StartsWith("C") && x.Archived == false).ToList();
                foreach (var item in SendMailCustomerData)
                {
                    var EmpShareCustomerData = _db.AspNetUsers.Where(x => x.Id == item.Id && x.Archived == false).FirstOrDefault();
                    HRTool.Models.MailModel custmail = new HRTool.Models.MailModel();
                    using (StreamReader stramReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/SendMailFromMangerTocustomerCloseReview.html")))
                    {
                        custmail.Body = stramReader.ReadToEnd();
                    }
                    custmail.From = FromData.UserName;
                    custmail.To = EmpShareCustomerData.UserName;
                    custmail.Subject = toData.FirstName + " " + toData.LastName + " Performance Review is Closed";
                    custmail.Body = custmail.Body.Replace("{1}", FromData.FirstName + " " + FromData.LastName);
                    custmail.Body = custmail.Body.Replace("{2}", toData.FirstName + " " + toData.LastName);
                    custmail.Body = custmail.Body.Replace("{0}", EmpShareCustomerData.FirstName + " " + EmpShareCustomerData.LastName);
                    string custmailToReceive = Common.sendMail(custmail);
                }
            }
            return PartialView("_PartialClosePerformanceReview", model);
        }

        // Objective Of Performance

        public ActionResult EmployeeObjectiveOfPerformance(int Id, int EmpPerReviewId, int EmpId)
        {
            ObjectiveOfEmployeePerformance model = new ObjectiveOfEmployeePerformance();
            if (Id > 0)
            {
                var ObjectiveData = _db.EmployeePerformanceGoals.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
                model.Id = ObjectiveData.Id;
                model.GoalName = ObjectiveData.GoalName;
                model.GoalDescription = ObjectiveData.GoalDescription;
                model.DueDate = String.Format("{0:dd-MM-yyy}", ObjectiveData.DueDate);
                model.UnitPercent = ObjectiveData.Unit;
            }
            model.EmpPerformanceId = EmpPerReviewId;
            return PartialView("_PartialEmployeePerformanceObjective", model);
        }

        public ActionResult GetEmployeePerformanceGoal(int EmpPerReviewId, int EmpId)
        {
            ObjectiveOfEmployeePerformance reviewmodel = new ObjectiveOfEmployeePerformance();
            reviewmodel = EmployeePerformnceGoalList(EmpId);
            reviewmodel.EmpPerformanceId = EmpPerReviewId;
            reviewmodel.EmployeeId = EmpId;
            List<string> StatusList = new List<string>();
            StatusList.Add("Open");
            StatusList.Add("Parked");
            StatusList.Add("Closed");
            foreach (var item in StatusList)
            {
                reviewmodel.Status.Add(new SelectListItem() { Text = item, Value = item });
            }
            return PartialView("_PartialEmployeePerformanceObjectiveList", reviewmodel);
        }
        public ObjectiveOfEmployeePerformance EmployeePerformnceGoalList(int EmpId)
        {
            List<EmployeePerformanceGoal> dataPerformance = _employeePerformanceMethod.getListofPerformance(EmpId).Where(x => x.GoalStatus == "Open").ToList();
            ObjectiveOfEmployeePerformance reviewmodel = new ObjectiveOfEmployeePerformance();
            foreach (var item in dataPerformance)
            {
                ObjectiveOfEmployeePerformance model = new ObjectiveOfEmployeePerformance();
                model.Id = item.Id;
                model.EmployeeId = Convert.ToInt32(item.EmployeeId);
                model.EmpPerformanceId = Convert.ToInt32(item.EmployeePerformanceId);
                model.GoalName = item.GoalName;
                model.GoalDescription = item.GoalDescription;
                model.DueDate = String.Format("{0:dd-MM-yyy}", item.DueDate);
                model.StartDate = String.Format("{0:dd-MM-yyy}", item.CreatedDate);
                model.GoalStatus = item.GoalStatus;
                model.UnitPercent = item.Unit;
                reviewmodel.EmployeePerformanceGoalList.Add(model);
            }
            return reviewmodel;
        }
        public ActionResult saveEmployeePerformanceGoal(ObjectiveOfEmployeePerformance model)
        {
            bool addupdateData = _employeePerformanceMethod.SaveEmployeePerformanceObjective(model);
            ObjectiveOfEmployeePerformance goalmodel = EmployeePerformnceGoalList(model.EmployeeId);
            return RedirectToAction("GetEmployeePerformanceGoal", "EmployeePerformance", new { EmpPerReviewId = model.EmpPerformanceId, EmpId = model.EmployeeId });
            //return PartialView("_PartialEmployeePerformanceObjective", goalmodel);
        }
        // View Performance Progress
        public static string StripHTML(string input)
        {
            if (input != null)
            {
                return System.Text.RegularExpressions.Regex.Replace(input, "<.*?>", String.Empty);
            }
            else
            {
                return String.Empty;
            }
        }
        public ActionResult ViewPerformanceObjectiveProgress(int Id, int EmployeeId, int EmpPerformanceId)
        {
            ObjectiveOfEmployeePerformance model = new ObjectiveOfEmployeePerformance();
            var GoalProgressData = _db.EmployeePerformanceGoals.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
            List<string> StatusList = new List<string>();
            StatusList.Add("Open");
            StatusList.Add("Parked");
            StatusList.Add("Closed");
            foreach (var item in StatusList)
            {
                model.Status.Add(new SelectListItem() { Text = item, Value = item });
            }
            model.Id = GoalProgressData.Id;
            model.EmployeeId = Convert.ToInt32(GoalProgressData.EmployeeId);
            model.EmpPerformanceId = Convert.ToInt32(GoalProgressData.EmployeePerformanceId);
            model.DueDate = String.Format("{0:dd-MM-yyy}", GoalProgressData.DueDate);
            model.UnitPercent = GoalProgressData.Unit;
            model.GoalName = GoalProgressData.GoalName;
            model.GoalStatus = GoalProgressData.GoalStatus;

            var goalDoument = _employeePerformanceMethod.getAllGoalDocument(Id);
            foreach (var item in goalDoument)
            {
                EmployeePerformanceGoalDocumentsViewModel docModel = new EmployeePerformanceGoalDocumentsViewModel();
                docModel.Id = item.Id;
                docModel.originalName = item.OriginalName;
                docModel.newName = item.NewName;
                model.DocumentList.Add(docModel);
            }
            var goalComments = _employeePerformanceMethod.getAllGoalCommnet(Id);
            foreach (var item in goalComments)
            {
                EmployeePerformanceGoalCommentViewModel docModel = new EmployeePerformanceGoalCommentViewModel();
                docModel.Id = item.Id;
                docModel.comment = StripHTML(item.Description);
                if (!string.IsNullOrEmpty(item.CreatedName))
                {
                    int UserId = Convert.ToInt32(item.CreatedName);
                    var EmpData = _db.AspNetUsers.Where(x => x.Id == UserId && x.Archived == false).FirstOrDefault();
                    docModel.commentBy = EmpData.FirstName + " " + EmpData.LastName;
                }
                model.CommentList.Add(docModel);
            }
            return PartialView("_PartialEmployeePerfromanceGoalProgress", model);
        }

        //Performance Goal Image Upload
        public ActionResult GoalImageData()
        {
            string FilePath = string.Empty;
            string fileName = string.Empty;
            string originalFileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                FilePath = System.Configuration.ConfigurationManager.AppSettings["Performance_Goal"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
        }

        //Save Perfromance Goal Image
        public ActionResult SavePerfromanceGoalDocument(string jsonDocumentList, int PerGoalId, int EmpPerGoalId, string Status, string UnitPercent, int EmployeeId, int EmpPerReview)
        {
            ObjectiveOfEmployeePerformance model = new ObjectiveOfEmployeePerformance();
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<EmployeePerformanceGoalDocumentsViewModel> listDocument = js.Deserialize<List<EmployeePerformanceGoalDocumentsViewModel>>(jsonDocumentList);
            model.DocumentList = listDocument;
            bool updateData = _employeePerformanceMethod.SavePerformanceDocument(PerGoalId, model, Status, UnitPercent);
            //ObjectiveOfEmployeePerformance goalmodel = EmployeePerformnceGoalList(model.EmployeeId);
            // return PartialView("_PartialEmployeePerfromanceGoalProgress", model);
            return RedirectToAction("GetEmployeePerformanceGoal", "EmployeePerformance", new { EmpPerReviewId = EmpPerReview, EmpId = EmployeeId });
        }
        public ActionResult GetGoalComment(int CmtId, int GoalId, int EmpPerformanceId, int EmployeeId)
        {
            ObjectiveOfEmployeePerformance model = new ObjectiveOfEmployeePerformance();
            model.Id = GoalId;
            model.EmpPerformanceId = EmpPerformanceId;
            model.EmployeeId = EmployeeId;
            if (CmtId != 0)
            {
                var CommntData = _db.EmployeePerformanceGoalComments.Where(x => x.Id == CmtId && x.Archived == false).FirstOrDefault();
                model.CommnetId = Convert.ToString(CommntData.Id);
                model.Description = StripHTML(CommntData.Description);
            }
            return PartialView("_PartialEmployeePerfromanceGoalComment", model);
        }

        [ValidateInput(false)]
        public ActionResult SaveGoalComment(int ComtId, int GoalId, int EmpPerformanceId, int EmployeeId, string Comment, string UnitPercent)
        {
            ObjectiveOfEmployeePerformance model = new ObjectiveOfEmployeePerformance();
            bool updateData = _employeePerformanceMethod.SaveGoalComment(ComtId, GoalId, EmpPerformanceId, EmployeeId, Comment, UnitPercent);
            return RedirectToAction("ViewPerformanceObjectiveProgress", "EmployeePerformance", new { Id = GoalId, EmployeeId = EmployeeId, EmpPerformanceId = EmpPerformanceId });
        }

        public ActionResult FilterByGoalStatus(string StatusOfGoal, int EmployeeId, int EmpPerId)
        {
            ObjectiveOfEmployeePerformance reviewmodel = new ObjectiveOfEmployeePerformance();
            if (!string.IsNullOrEmpty(StatusOfGoal))
            {
                List<EmployeePerformanceGoal> dataPerformance = _employeePerformanceMethod.getListofPerformance(EmployeeId).Where(x => x.GoalStatus == StatusOfGoal).ToList();
                foreach (var item in dataPerformance)
                {
                    ObjectiveOfEmployeePerformance model = new ObjectiveOfEmployeePerformance();
                    model.Id = item.Id;
                    model.EmployeeId = Convert.ToInt32(item.EmployeeId);
                    model.EmpPerformanceId = Convert.ToInt32(item.EmployeePerformanceId);
                    model.GoalName = item.GoalName;
                    model.GoalDescription = item.GoalDescription;
                    model.DueDate = String.Format("{0:dd-MM-yyy}", item.DueDate);
                    model.StartDate = String.Format("{0:dd-MM-yyy}", item.CreatedDate);
                    model.GoalStatus = item.GoalStatus;
                    model.UnitPercent = item.Unit;
                    reviewmodel.EmployeePerformanceGoalList.Add(model);
                }
            }
            else
            {
                reviewmodel = EmployeePerformnceGoalList(EmployeeId);
            }
            reviewmodel.EmpPerformanceId = EmpPerId;
            reviewmodel.EmployeeId = EmployeeId;
            List<string> StatusList = new List<string>();
            StatusList.Add("Open");
            StatusList.Add("Parked");
            StatusList.Add("Closed");
            foreach (var item in StatusList)
            {
                reviewmodel.Status.Add(new SelectListItem() { Text = item, Value = item });
            }
            reviewmodel.SelectedStatus = StatusOfGoal;
            return PartialView("_PartialEmployeePerformanceObjectiveList", reviewmodel);
        }

        public ActionResult GetGoalChart(int EmpId, string StatusOfGoal)
        {
            ObjectiveOfEmployeePerformance goalmodel = new ObjectiveOfEmployeePerformance();
            var GoalData = _db.EmployeePerformanceGoals.Where(x => x.EmployeeId == EmpId && x.Archived == false).ToList();
            if (!string.IsNullOrEmpty(StatusOfGoal))
            {
                GoalData = _db.EmployeePerformanceGoals.Where(x => x.EmployeeId == EmpId && x.GoalStatus == StatusOfGoal && x.Archived == false).ToList();
            }

            goalmodel.CountTotalGoal = Convert.ToString(GoalData.Count());
            foreach (var item in GoalData)
            {
                ObjectiveOfEmployeePerformance model = new ObjectiveOfEmployeePerformance();
                model.Id = item.Id;
                model.EmployeeId = Convert.ToInt32(item.EmployeeId);
                model.EmpPerformanceId = Convert.ToInt32(item.EmployeePerformanceId);
                model.GoalName = item.GoalName;
                model.GoalXValue = Convert.ToDecimal(item.GoalValueX);
                model.GoalYValue = Convert.ToDecimal(item.GoalValueY);
                goalmodel.EmployeePerformanceGoalList.Add(model);
            }
            return Json(goalmodel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateGoalChart(int Id, string value_x, string value_y, string Unitvalue)
        {
            bool updateData = _employeePerformanceMethod.UpdateGraphDropValue(Id, value_x, value_y, Unitvalue);

            ObjectiveOfEmployeePerformance goalmodel = new ObjectiveOfEmployeePerformance();
            return Json(goalmodel, JsonRequestBehavior.AllowGet);
        }

        //Delete Performance Review
        public JsonResult DeletePerformanceReview(int EmpPerfReviewID, int ReviewId, int EmpId)
        {
            ReviewDetails model = new ReviewDetails();
            bool saveData = _employeePerformanceMethod.DeleteEmployeePerformanceReview(EmpPerfReviewID, ReviewId, EmpId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}