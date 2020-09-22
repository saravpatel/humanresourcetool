using HRTool.CommanMethods;
using HRTool.Models.Admin;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRTool.Models.Resources;
using HRTool.DataModel;
using System.IO;
using HRTool.CommanMethods.Resources;
using System.Web.Script.Serialization;
using Rotativa;
using Rotativa.Options;
using HRTool.CommanMethods.Admin;
using System.Web.Routing;

namespace HRTool.Controllers
{

    public class MeEmployeePerformanceController : Controller
    {
        //
        // GET: /EmployeePerformance/
        EvolutionEntities _db = new EvolutionEntities();
        EmployeePerformanceMethod _employeePerformanceMethod = new EmployeePerformanceMethod();
        [CustomAuthorize]
        public ActionResult Index(string EmployeeId)
        {
            return View();
        }
        [CustomAuthorize]
        public ActionResult Review()
        {
            return View();
        }
        [CustomAuthorize]
        public ActionResult MeEmployeePerformance(string EmployeeId)
        {
            EmployeePerformanceViewModel model = new EmployeePerformanceViewModel();
            int EmpId = Convert.ToInt32(EmployeeId);
            model = getListOfPerformance(EmpId);
            model.EmployeeId = Convert.ToInt32(EmployeeId);
            return View(model);
        }
        [CustomAuthorize]
        public EmployeePerformanceViewModel getListOfPerformance(int EmpId)
        {
            List<EmployeePerformance> dataPerformance = _employeePerformanceMethod.getListofPerformance().Where(x => x.EmployeeId == EmpId).ToList();
            EmployeePerformanceViewModel reviewmodel = new EmployeePerformanceViewModel();
            foreach (var item in dataPerformance)
            {
                EmployeePerformanceViewModel model = new EmployeePerformanceViewModel();
                var CustomerShare = _db.PerformanceCustomerDetails.Where(x => x.Performance_Id == item.Id && x.IsArchived == false).FirstOrDefault();
                if (CustomerShare != null)
                {
                    model.CustomerShare = CustomerShare.InviteStatus;
                }
                model.Id = item.Id;
                var EmpReviewByName = _db.AspNetUsers.Where(x => x.Id == item.RevviewByEmpID && x.Archived == false).FirstOrDefault();
                if (EmpReviewByName != null)
                {
                    model.ReviewByName = EmpReviewByName.FirstName + " " + EmpReviewByName.LastName;
                }
                var ReviewName = _db.PerformanceSettings.Where(x => x.Id == item.ReviewId && x.Archived == false).FirstOrDefault();
                if (ReviewName != null)
                {
                    model.ReviewName = ReviewName.ReviewText;
                }
                model.ReviewId = item.ReviewId;
                model.EmployeeId = item.EmployeeId;
                model.ProjectId = item.ProjectId;
                if (!string.IsNullOrEmpty(item.OverallScore))
                {
                    model.OverAllScore = Convert.ToInt32(item.OverallScore);
                }
                var complatedata = _db.PerformanceSettings.Where(x => x.Id == model.ReviewId).Select(x => x.CompletionDate).Take(1).SingleOrDefault();
                model.CompletionDate = complatedata.GetValueOrDefault();
                model.CreatedDate = Convert.ToDateTime(item.CreatedDate).ToString("dd MMM yyyy");
                //model.CreatedDate = Convert.ToDateTime(item.CreatedDate);
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
                if (!string.IsNullOrEmpty(item.OverallScore))
                {
                    model.OverAllScore = Convert.ToInt32(item.OverallScore);
                }
                //model.CustomerScore
                //model.ManagerScore  
                model.CreatedDate = Convert.ToDateTime(item.CreatedDate).ToString("dd MMM yyyy");
                // model.CreatedDate = Convert.ToDateTime(item.CreatedDate);
                var complatedata = _db.PerformanceSettings.Where(x => x.Id == model.ReviewId).Select(x => x.CompletionDate).Take(1).SingleOrDefault();
                model.CompletionDate = complatedata.GetValueOrDefault();
                reviewmodel.ListOfPastPerformace.Add(model);
            }
            return reviewmodel;
        }
        [CustomAuthorize]
        public ActionResult ActiveReviews(string EmployeeId)
        {
            int EmpId = Convert.ToInt32(EmployeeId);
            EmployeePerformanceViewModel modelList = getListOfPerformance(EmpId);
            modelList.EmployeeId = EmpId;
            modelList.IsActivePastFlag = 1;
            return PartialView("_partialMeActivePerformanceReview", modelList);
        }
        [CustomAuthorize]
        public ActionResult PastReviews(string EmployeeId)
        {
            int EmpId = Convert.ToInt32(EmployeeId);
            EmployeePerformanceViewModel modelList = getListOfPerformance(EmpId);
            modelList.EmployeeId = EmpId;
            modelList.IsActivePastFlag = 0;
            return PartialView("_PartialMePastReview", modelList);
        }
        [CustomAuthorize]
        //Check For Is Review Exist

        public JsonResult CheckIsPerformanceReviewExist(int EmployeeId)
        {
            StartNewReviewViewModel model = new StartNewReviewViewModel();
            var EmployeeReviewData = _db.EmployeePerformances.Where(x => x.EmployeeId == EmployeeId && x.ReviewStatus == "Open" && x.Archived == false).FirstOrDefault();
            if (EmployeeReviewData != null)
            {
                model.IsEmployeeExistReview = "1";
            }
            else
            {
                model.IsEmployeeExistReview = "0";
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorize]

        //Start New Review
        public ActionResult StartNewReview(int EmployeeId)
        {
            StartNewReviewViewModel model = new StartNewReviewViewModel();
            //var ReviewList = _db.PerformanceSettings.Where(x => x.Archived == false && x.CompletionDate>DateTime.Now).ToList();
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
            return PartialView("_partialMeStartNewReview", model);
        }
        [CustomAuthorize]
        //Save New Review
        public ActionResult SaveNewReview(EmployeePerformanceViewModel model)
        {

            bool saveData = _employeePerformanceMethod.SaveEmployeePerformace(model);
            if (saveData == false)
            {
                return Json(saveData, JsonRequestBehavior.AllowGet);
            }

            EmployeePerformanceViewModel modelList = getListOfPerformance(model.EmployeeId);
            return PartialView("_partialMeActivePerformanceReview", modelList);
        }
        [CustomAuthorize]
        public ActionResult GetCoworkerDetailForReview(int reviewID, int isManagerEmployee, int empID, int isActivePastFlag)
        {

            Coworker model = new Coworker();
            model = getCoWorkerListData(reviewID, empID);
            model.IsActivePastFlag = isActivePastFlag;
            return PartialView("_PartialMeEmployeePerformanceCoworkerListDetails", model);
        }
        [CustomAuthorize]
        public ActionResult GetCustomerDetailForReview(int reviewID, int isManagerEmployee, int empID, int isActivePastFlag)
        {

            Coworker model = new Coworker();
            model = getCustomerListData(reviewID, empID);
            model.IsActivePastFlag = isActivePastFlag;
            return PartialView("_PartialMeEmployeePerformanceCustomerListDetails", model);
        }
        [CustomAuthorize]
        private Coworker getCustomerListData(int reviewId, int empId)
        {
            Coworker model = new Coworker();
            var CustomerData = _db.SendReviewToCustomers.Where(x => x.ReviewID == reviewId && x.EmployeeID == empId && x.Archived == false).ToList();
            foreach (var item in CustomerData)
            {
                CustomerInviteList customerModel = new CustomerInviteList();
                customerModel.Id = Convert.ToInt32(item.Id);
                customerModel.CustomerID = Convert.ToInt32(item.Id);
                if (item.InviteeEmployee != 0)
                {
                    var EmpDetails = _db.AspNetUsers.Where(x => x.Id == item.InviteeEmployee && x.Archived == false).FirstOrDefault();
                    customerModel.CustomerName = EmpDetails.FirstName + " " + EmpDetails.LastName;
                }
                else
                {
                    customerModel.CustomerName = item.FullName;
                }
                customerModel.Status = item.MailStatus;
                //customerModel.Status=item.stat
                model.CustomerInviteList.Add(customerModel);
            }

            model.TotalInvitedCustomer = CustomerData.Count() + " Employee";
            return model;
        }
        [CustomAuthorize]
        private Coworker getCoWorkerListData(int reviewId, int empId)
        {
            Coworker model = new Coworker();
            var CoworkerData = _db.SendReviewToCoworkers.Where(x => x.ReviewID == reviewId && x.EmployeeID == empId && x.Archived == false).ToList();
            foreach (var item in CoworkerData)
            {
                CoworkerInviteList coModel = new CoworkerInviteList();
                coModel.Id = item.Id;
                coModel.coworkerId = Convert.ToInt32(item.Id);
                if (item.InviteeEmployee != 0)
                {
                    var EmpDetails = _db.AspNetUsers.Where(x => x.Id == item.InviteeEmployee && x.Archived == false).FirstOrDefault();
                    coModel.EmpName = EmpDetails.FirstName + " " + EmpDetails.LastName;
                }
                else
                {
                    coModel.EmpName = item.FullName;
                }
                coModel.Status = item.MailStatus;
                model.CoworkerInviteList.Add(coModel);
            }

            model.TotalInvitedCoworker = CoworkerData.Count() + " Employee";
            return model;
        }
        [CustomAuthorize]
        public ActionResult getEmplyeeUserList()
        {
            ViewSkillsViewModel model = new ViewSkillsViewModel();
            List<AspNetUser> data = _employeePerformanceMethod.getAllUserList().ToList();
            foreach (var item in data)
            {
                string Name = string.Format("{0} {1} - {2}", item.FirstName, item.LastName, item.SSOID);
                model.EmployeesUserList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorize]
        public ActionResult getCustomerUserList()
        {
            ViewSkillsViewModel model = new ViewSkillsViewModel();
            List<AspNetUser> data = _employeePerformanceMethod.getAllCustomerList().ToList();
            foreach (var item in data)
            {
                string Name = string.Format("{0} {1} - {2}", item.FirstName, item.LastName, item.SSOID);
                model.EmployeesUserList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorize]
        public ActionResult SendEmailToInvitees(string InviteesIDs, string EmpID, int ReviewId, string Username, string Email)
        {
            string message = "";
            Coworker datamodel = new Coworker();
            int EmployeeID = Convert.ToInt32(EmpID);
            var EmpReviewData = _db.EmployeePerformances.Where(x => x.EmployeeId == EmployeeID && x.ReviewId == ReviewId && x.Archived == false).FirstOrDefault();
            var ReviewData = _db.PerformanceSettings.Where(x => x.Id == ReviewId && x.Archived == false).FirstOrDefault();
            if (EmpReviewData != null)
            {
                ReviewData = _db.PerformanceSettings.Where(x => x.Id == EmpReviewData.ReviewId && x.Archived == false).FirstOrDefault();
            }
            if (!string.IsNullOrEmpty(InviteesIDs))
            {
                List<string> inviteesIDS = InviteesIDs.Split(',').ToList();
                foreach (var items in inviteesIDS)
                {
                    var intId = Convert.ToInt32(items);
                    var emailData = _db.SendReviewToCoworkers.Where(x => x.ReviewID == ReviewId && x.InviteeEmployee == intId).FirstOrDefault();
                    if (emailData == null)
                    {
                        if (!string.IsNullOrEmpty(EmpID))
                        {
                            //int coDetailId = _employeePerformanceMethod.SaveCoworkerInviteLink(EmpID, EmpReviewData.Id, EmployeeId);
                            var FromData = _db.AspNetUsers.Where(x => x.Id == SessionProxy.UserId && x.Archived == false).FirstOrDefault();
                            HRTool.Models.MailModel mail = new HRTool.Models.MailModel();
                            using (StreamReader stramReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/SendMailCoworker.html")))
                            {
                                mail.Body = stramReader.ReadToEnd();
                            }
                            var toData = _db.AspNetUsers.Where(x => x.Id == intId && x.Archived == false).FirstOrDefault();
                            var fullName = toData.FirstName + " " + toData.LastName;
                            int coDetailId = _employeePerformanceMethod.SaveCoworkerInviteLinkForMeEmployee(intId, EmpReviewData.Id, EmployeeID, ReviewId, fullName);
                            mail.From = "hrtool123@gmail.com";
                            mail.To = toData.UserName;
                            mail.Subject = "Coworker mail";
                            mail.Body = mail.Body.Replace("{0}", toData.FirstName + " " + toData.LastName);
                            mail.Body = mail.Body.Replace("{3}", Convert.ToString(ReviewData.CompletionDate));
                            var baseUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["baseURL"].ToString();
                            var getGuId = ReviewData.GuID;
                            var link = baseUrl + "MeEmployeePerformance/CoWorkerQueDetailsForMe/" + coDetailId + "/" + getGuId;
                            var Declinelink = baseUrl + "MeEmployeePerformance/DeclineInvitation/" + coDetailId + "/" + getGuId;
                            mail.Body = mail.Body.Replace("{4}", link);
                            mail.Body = mail.Body.Replace("{2}", Declinelink);
                            mail.Body = mail.Body.Replace("{5}", "");
                            string mailToReceive = Common.sendMail(mail);
                        }
                    }

                    else
                    {
                        message = "You have already invited this coworker";
                    }

                }
            }
            else if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Email))
            {
                var FromData = _db.AspNetUsers.Where(x => x.Id == SessionProxy.UserId && x.Archived == false).FirstOrDefault();
                HRTool.Models.MailModel mail = new HRTool.Models.MailModel();
                using (StreamReader stramReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/SendMailCoworker.html")))
                {
                    mail.Body = stramReader.ReadToEnd();
                }
                int coDetailId = _employeePerformanceMethod.SaveCoworkerInviteLinkForMeEmployee(0, EmpReviewData.Id, EmployeeID, ReviewId, Username);
                mail.From = FromData.UserName;
                mail.To = Email;
                mail.Subject = "Coworker mail";
                mail.Body = mail.Body.Replace("{0}", Username);
                mail.Body = mail.Body.Replace("{3}", Convert.ToString(ReviewData.CompletionDate));
                var getGuId = ReviewData.GuID;
                var baseUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["baseURL"].ToString();
                var link = baseUrl + "MeEmployeePerformance/CoWorkerQueDetailsForMe/" + coDetailId + "/" + getGuId;
                var Declinelink = baseUrl + "MeEmployeePerformance/DeclineInvitation/" + coDetailId + "/" + getGuId;
                mail.Body = mail.Body.Replace("{4}", link);
                mail.Body = mail.Body.Replace("{2}", Declinelink);
                mail.Body = mail.Body.Replace("{5}", "Your Temporary password to give review is: " + "<b>" + "As!234hj@Lo" + "</b>");
                string mailToReceive = Common.sendMail(mail);
            }

            return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);

        }

        [CustomAuthorize]
        public ActionResult SendEmailToInviteesCustomer(string InviteesIDs, string EmpID, int ReviewId, string Username, string Email)
        {
            string message = "";
            Coworker datamodel = new Coworker();
            int EmployeeID = Convert.ToInt32(EmpID);
            var EmpReviewData = _db.EmployeePerformances.Where(x => x.EmployeeId == EmployeeID && x.ReviewId == ReviewId && x.Archived == false).FirstOrDefault();
            var ReviewData = _db.PerformanceSettings.Where(x => x.Id == ReviewId && x.Archived == false).FirstOrDefault();
            if (EmpReviewData != null)
            {
                ReviewData = _db.PerformanceSettings.Where(x => x.Id == EmpReviewData.ReviewId && x.Archived == false).FirstOrDefault();
            }
            if (!string.IsNullOrEmpty(InviteesIDs))
            {
                List<string> inviteesIDS = InviteesIDs.Split(',').ToList();
                foreach (var items in inviteesIDS)
                {
                    var intId = Convert.ToInt32(items);
                    var emailData = _db.SendReviewToCustomers.Where(x => x.ReviewID == ReviewId && x.InviteeEmployee == intId).FirstOrDefault();
                    if (emailData == null)
                    {
                        if (!string.IsNullOrEmpty(EmpID))
                        {
                            //int coDetailId = _employeePerformanceMethod.SaveCoworkerInviteLink(EmpID, EmpReviewData.Id, EmployeeId);
                            var FromData = _db.AspNetUsers.Where(x => x.Id == SessionProxy.UserId && x.Archived == false).FirstOrDefault();
                            HRTool.Models.MailModel mail = new HRTool.Models.MailModel();
                            using (StreamReader stramReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/SendMailCoworker.html")))
                            {
                                mail.Body = stramReader.ReadToEnd();
                            }
                            var toData = _db.AspNetUsers.Where(x => x.Id == intId && x.Archived == false).FirstOrDefault();
                            var fullName = toData.FirstName + " " + toData.LastName;
                            int coDetailId = _employeePerformanceMethod.SaveCustomerInviteLinkForMeEmployee(intId, EmpReviewData.Id, EmployeeID, ReviewId, fullName);
                            mail.From = "hrtool123@gmail.com";
                            mail.To = toData.UserName;
                            mail.Subject = "Coworker mail";
                            mail.Body = mail.Body.Replace("{0}", toData.FirstName + " " + toData.LastName);
                            mail.Body = mail.Body.Replace("{3}", Convert.ToString(ReviewData.CompletionDate));
                            var baseUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["baseURL"].ToString();
                            var getGuId = ReviewData.GuID;
                            var link = baseUrl + "MeEmployeePerformance/CustomerQueDetailsForMe/" + coDetailId + "/" + getGuId;
                            var Declinelink = baseUrl + "MeEmployeePerformance/DeclineInvitationForCustomer/" + coDetailId + "/" + getGuId;
                            mail.Body = mail.Body.Replace("{4}", link);
                            mail.Body = mail.Body.Replace("{2}", Declinelink);
                            mail.Body = mail.Body.Replace("{5}", "");

                            string mailToReceive = Common.sendMail(mail);
                        }
                    }

                    else
                    {
                        message = "You have already invited this customer";
                    }

                }
            }
            else if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Email))
            {
                var FromData = _db.AspNetUsers.Where(x => x.Id == SessionProxy.UserId && x.Archived == false).FirstOrDefault();
                HRTool.Models.MailModel mail = new HRTool.Models.MailModel();
                using (StreamReader stramReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/SendMailCoworker.html")))
                {
                    mail.Body = stramReader.ReadToEnd();
                }
                int coDetailId = _employeePerformanceMethod.SaveCustomerInviteLinkForMeEmployee(0, EmpReviewData.Id, EmployeeID, ReviewId, Username);
                mail.From = FromData.UserName;
                mail.To = Email;
                mail.Subject = "Coworker mail";
                mail.Body = mail.Body.Replace("{0}", Username);
                mail.Body = mail.Body.Replace("{3}", Convert.ToString(ReviewData.CompletionDate));
                var getGuId = ReviewData.GuID;
                var baseUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["baseURL"].ToString();
                var link = baseUrl + "MeEmployeePerformance/CustomerQueDetailsForMe/" + coDetailId + "/" + getGuId;
                var Declinelink = baseUrl + "MeEmployeePerformance/DeclineInvitationForCustomer/" + coDetailId + "/" + getGuId;
                mail.Body = mail.Body.Replace("{4}", link);
                mail.Body = mail.Body.Replace("{2}", Declinelink);
                mail.Body = mail.Body.Replace("{5}", "Your Temporary password to give review is :" + "<b>" + "As!234hj@Lo" + "</b>");

                string mailToReceive = Common.sendMail(mail);
            }

            return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult CoWorkerQueDetailsForMe(string Id, int PerCoworkerId)
        {
            List<EditSegmentViewModel> model = new List<EditSegmentViewModel>();
            Coworker modelWorker = new Coworker();
            var PerCoworkerDetials = _db.SendReviewToCoworkers.Where(x => x.Id == PerCoworkerId).FirstOrDefault();
            if (PerCoworkerDetials.InviteeEmployee != 0)
            {
                if (SessionProxy.UserId == 0 || SessionProxy.UserId == null)
                {
                    return RedirectToAction("LoginRedirect", "Login");
                }
            }
            if (PerCoworkerDetials.InviteeEmployee == 0 && Id != "true")
            {
                if (PerCoworkerDetials.MailStatus == "Invited")
                {
                    modelWorker.cororkerId = PerCoworkerId.ToString();
                    return RedirectToAction("GuestReviewLogin", "MeEmployeePerformance", modelWorker);
                }
            }
            string GUID = Convert.ToString(Id);
            JavaScriptSerializer js = new JavaScriptSerializer();
            //  SegmentViewModel model = new SegmentViewModel();
            var coworkerSegdata = _db.PerformanceSettings.Where(x => x.Id == PerCoworkerDetials.ReviewID && x.Archived == false).FirstOrDefault();
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

            if (PerCoworkerDetials.MailStatus == "Invited" || PerCoworkerDetials.MailStatus == "See Response")
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
            model.FirstOrDefault().HelpText = PerCoworkerDetials.MailStatus;
            return View("CoWorkerPerformanceReviewMeEmployee", model);
        }
        public ActionResult GuestReviewLogin(Coworker model)
        {
            return View(model);
        }
        public ActionResult GuestReviewLoginForCustomer(Coworker model)
        {
            return View(model);
        }

        public ActionResult verifyPasswordOfGuest(int id, string password)
        {
            if (password == "As!234hj@Lo")
            {
                return Json(new { message = true }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { message = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult verifyPasswordOfGuestCustomer(int id, string password)
        {
            if (password == "As!234hj@Lo")
            {
                return Json(new { message = true }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { message = false }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult CustomerQueDetailsForMe(string Id, int PerCoworkerId)
        {
            string GUID = Convert.ToString(Id);
            JavaScriptSerializer js = new JavaScriptSerializer();
            Coworker modelWorker = new Coworker();
            //  SegmentViewModel model = new SegmentViewModel();
            var PerCoworkerDetials = _db.SendReviewToCustomers.Where(x => x.Id == PerCoworkerId).FirstOrDefault();
            if (PerCoworkerDetials.InviteeEmployee != 0)
            {
                if (SessionProxy.UserId == 0 || SessionProxy.UserId == null)
                {
                    return RedirectToAction("LoginRedirect", "Login");
                }
            }
            if (PerCoworkerDetials.InviteeEmployee == 0 && Id != "true")
            {
                modelWorker.cororkerId = PerCoworkerId.ToString();
                return RedirectToAction("GuestReviewLoginForCustomer", "MeEmployeePerformance", modelWorker);
            }
            var coworkerSegdata = _db.PerformanceSettings.Where(x => x.Id == PerCoworkerDetials.ReviewID && x.Archived == false).FirstOrDefault();
            if (!string.IsNullOrEmpty(GUID))
            {
                coworkerSegdata = _db.PerformanceSettings.Where(x => x.GuID == GUID && x.Archived == false).FirstOrDefault();
            }
            List<EditSegmentViewModel> model = new List<EditSegmentViewModel>();
            List<Coworker> emodel = new List<Coworker>();
            if (PerCoworkerDetials.CoWorkerScoreJSC != null)
            {
                emodel = js.Deserialize<List<Coworker>>(PerCoworkerDetials.CoWorkerScoreJSC);
                foreach (var item in emodel)
                {
                    item.CoreQueListData = js.Deserialize<List<CoworkerQuestionModelForMe>>(item.questionData);

                }
            }

            if (PerCoworkerDetials.MailStatus == "Invited" || PerCoworkerDetials.MailStatus == "See Response")
            {
                if (!string.IsNullOrEmpty(coworkerSegdata.CustomerSegmentJSON))
                {
                    model = js.Deserialize<List<EditSegmentViewModel>>(coworkerSegdata.CustomerSegmentJSON);
                    model.FirstOrDefault().Flag = 1;
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
            model.FirstOrDefault().HelpText = PerCoworkerDetials.MailStatus;
            return View("CustomerPerformanceReviewMeEmployee", model);
        }
        public ActionResult DeclineInvitation(Guid Id, int PerCoworkerId)
        {
            List<EditSegmentViewModel> model = new List<EditSegmentViewModel>();
            var data = _employeePerformanceMethod.UpdateCoWorkerDetailsForMeDecline(PerCoworkerId);
            model.Add(new EditSegmentViewModel());
            if (data == true)
            {
                model.FirstOrDefault().Flag = 0;
            }
            else
            {
                model.FirstOrDefault().Flag = 1;
            }
            model.FirstOrDefault().Title = "Declined";
            return View("CoWorkerPerformanceReviewMeEmployee", model);
        }

        public ActionResult DeclineInvitationForCustomer(Guid Id, int PerCoworkerId)
        {
            List<EditSegmentViewModel> model = new List<EditSegmentViewModel>();
            var data = _employeePerformanceMethod.UpdateCustomerDetailsForMeDecline(PerCoworkerId);
            model.Add(new EditSegmentViewModel());
            if (data == true)
            {
                model.FirstOrDefault().Flag = 0;
            }
            else
            {
                model.FirstOrDefault().Flag = 1;
            }
            model.FirstOrDefault().Title = "Declined";
            return View("CustomerPerformanceReviewMeEmployee", model);
        }
        public JsonResult UpdateCoWorkerCoreScore(int PerCoId, string questionListJSV)
        {
            List<EditSegmentViewModel> model = new List<EditSegmentViewModel>();
            var data = _employeePerformanceMethod.UpdateCoWorkerDetailsForMe(PerCoId, questionListJSV);
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

        public JsonResult UpdateCustomerCoreScore(int PerCoId, string questionListJSV)
        {
            List<EditSegmentViewModel> model = new List<EditSegmentViewModel>();
            var data = _employeePerformanceMethod.UpdateCoWorkerDetailsForMe(PerCoId, questionListJSV);
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

        [CustomAuthorize]
        public ActionResult ShareEmployeeReview(int ReviewId, int EmpId, int ManagerId)
        {
            ShareEmployeeReview model = new ShareEmployeeReview();
            var EmpRelation = _db.EmployeeRelations.Where(x => x.UserID == EmpId && x.IsActive == true).FirstOrDefault();
            if (EmpRelation != null)
            {
                var ManagerName = _db.AspNetUsers.Where(x => x.Id == EmpRelation.Reportsto && x.Archived == false).FirstOrDefault();
                model.ManagerName = ManagerName.FirstName + " " + ManagerName.LastName;
                model.MaanegrId = ManagerName.Id;
            }
            model.EmployeeId = EmpId;
            return PartialView("_PartialShareMeEmployeePerformance", model);
        }
        [CustomAuthorize]
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
                    mail.From = "hrtool123@gmail.com";
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
                    Managermail.From = "hrtool123@gmail.com";
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
                    mail.From = "hrtool123@gmail.com";
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
                        custmail.From = "hrtool123@gmail.com";
                        custmail.To = EmpShareCustomerData.UserName;
                        custmail.Subject = EmployeeData.FirstName + " " + EmployeeData.LastName + "Performance Review is Ready";
                        custmail.Body = custmail.Body.Replace("{1}", FromData.FirstName + " " + FromData.LastName);
                        custmail.Body = custmail.Body.Replace("{2}", EmployeeData.FirstName + " " + EmployeeData.LastName);
                        custmail.Body = custmail.Body.Replace("{0}", EmpShareCustomerData.FirstName + " " + EmpShareCustomerData.LastName);
                        string custmailToReceive = Common.sendMail(custmail);
                    }
                }
            }
            return PartialView("_PartialShareMeEmployeePerformance", model);

        }

        [CustomAuthorize]
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
            return PartialView("_PartialUnShareMeEmployeePerformance", model);
        }
        [CustomAuthorize]
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
            return PartialView("_PartialUnShareMeEmployeePerformance", model);

        }

        [CustomAuthorize]

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
            return PartialView("_partialMeReviewDetails", model);
        }
        [CustomAuthorize]

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
            if (coworkerSegdata != null)
            {
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
            }

            //TempData["ReviewId"] = coworkerSegdata.Id;
            //TempData["CustomerId"] = CustId;
            //TempData["PerReviewId"] = PerReviewId;
            //TempData["EmpId"] = EmpId;
            model.IsActivePastFlag = IsActivePastFlag;
            return PartialView("_PartialMeEmployeePerformanceReview", model);
        }
        [CustomAuthorize]

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

            return PartialView("_PartialMeEmployeePerformanceSegmentDetails", model);
        }
        [CustomAuthorize]

        //Get Question Details
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
            return PartialView("_PartialMeEmployeePerformanceQuestionData", model);
        }
        [CustomAuthorize]


        // Save Employee Segment Data
        public JsonResult saveEmployeePerformanceDetails(int PerformanceID, string OverallScoreString, string Comments, int EmpId, string JSONCustomerSegment, string JSONJobRoleSegment, int ReviewID, int IsManagerEmployee, int Flag, int IsActivePastFlag, int EmpPerfDetailId, int EmployeePerformaceID)
        {
            SegmentViewModel model = new SegmentViewModel();
            int Id = _employeePerformanceMethod.saveEmployeePerformance(PerformanceID, Flag, OverallScoreString, Comments, EmpId, JSONCustomerSegment, JSONJobRoleSegment, ReviewID, IsManagerEmployee, EmpPerfDetailId, EmployeePerformaceID);
            model.ExistPerformanceId = Convert.ToString(Id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorize]

        //Performance Goal
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
            return PartialView("_PartialMeEmployeePerformanceObjectiveList", reviewmodel);
        }
        [CustomAuthorize]

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
        [CustomAuthorize]

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
            return PartialView("_PartialMeEmployeePerformanceObjective", model);
        }
        [CustomAuthorize]


        public ActionResult saveEmployeePerformanceGoal(ObjectiveOfEmployeePerformance model)
        {
            bool addupdateData = _employeePerformanceMethod.SaveEmployeePerformanceObjective(model);
            ObjectiveOfEmployeePerformance goalmodel = EmployeePerformnceGoalList(model.EmployeeId);
            return RedirectToAction("GetEmployeePerformanceGoal", "MeEmployeePerformance", new { EmpPerReviewId = model.EmpPerformanceId, EmpId = model.EmployeeId });
            //return PartialView("_PartialEmployeePerformanceObjective", goalmodel);
        }
        [CustomAuthorize]

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
            return PartialView("_PartialMeEmployeePerfromanceGoalProgress", model);
        }
        [CustomAuthorize]

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
        [CustomAuthorize]

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
        [CustomAuthorize]

        public ActionResult SavePerfromanceGoalDocument(string jsonDocumentList, int PerGoalId, int EmpPerGoalId, string Status, string UnitPercent, int EmployeeId, int EmpPerReview)
        {
            ObjectiveOfEmployeePerformance model = new ObjectiveOfEmployeePerformance();
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<EmployeePerformanceGoalDocumentsViewModel> listDocument = js.Deserialize<List<EmployeePerformanceGoalDocumentsViewModel>>(jsonDocumentList);
            model.DocumentList = listDocument;
            bool updateData = _employeePerformanceMethod.SavePerformanceDocument(PerGoalId, model, Status, UnitPercent);
            // ObjectiveOfEmployeePerformance goalmodel = EmployeePerformnceGoalList(EmployeeId);
            // return PartialView("_PartialMeEmployeePerfromanceGoalProgress", model);
            return RedirectToAction("GetEmployeePerformanceGoal", "MeEmployeePerformance", new { EmpPerReviewId = EmpPerReview, EmpId = EmployeeId });
        }
        [CustomAuthorize]

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
            return PartialView("_PartialMeEmployeePerfromanceGoalComment", model);
        }
        [CustomAuthorize]


        [ValidateInput(false)]
        public ActionResult SaveGoalComment(int ComtId, int GoalId, int EmpPerformanceId, int EmployeeId, string Comment, string UnitPercent)
        {
            ObjectiveOfEmployeePerformance model = new ObjectiveOfEmployeePerformance();
            bool updateData = _employeePerformanceMethod.SaveGoalComment(ComtId, GoalId, EmpPerformanceId, EmployeeId, Comment, UnitPercent);
            return RedirectToAction("ViewPerformanceObjectiveProgress", "MeEmployeePerformance", new { Id = GoalId, EmployeeId = EmployeeId, EmpPerformanceId = EmpPerformanceId });
        }
        [CustomAuthorize]



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
            return PartialView("_PartialMeEmployeePerformanceObjectiveList", reviewmodel);
        }
        [CustomAuthorize]

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
        [CustomAuthorize]

        public ActionResult UpdateGoalChart(int Id, string value_x, string value_y, string UnitValue)
        {
            bool updateData = _employeePerformanceMethod.UpdateGraphDropValue(Id, value_x, value_y, UnitValue);

            ObjectiveOfEmployeePerformance goalmodel = new ObjectiveOfEmployeePerformance();
            return Json(goalmodel, JsonRequestBehavior.AllowGet);
        }

        // Print 
        [CustomAuthorize]

        public ActionResult PrintEmployeePerformance(int EmpId, int ReviewId, string EmpPerfId)
        {
            PrintPerformancePDF model = new PrintPerformancePDF();
            int EmpPerID = Convert.ToInt32(EmpPerfId);
            var ReviewData = _db.PerformanceSettings.Where(x => x.Id == ReviewId && x.Archived == false).FirstOrDefault();
            var IsReviewEmployee = _db.PerformanceEmployeeDetails.Where(x => x.EmployeeId == EmpId && x.ReviewID == ReviewId && x.IsArchived == false).FirstOrDefault();
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
                            var coreJSON = _db.EmployeePerformanceCoreJSONDetails.Where(x => x.QueId == QueId && x.ReviewID == ReviewId && x.Archived == false).ToList();
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
                        var jboroleJSON = _db.EmployeePerformanceJobRoleSegmentJSONDetails.Where(x => x.QueId == QueId && x.ReviewID == ReviewId && x.PerEmployeedetailId == EmpId && x.Archived == false).ToList();
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
            return new Rotativa.ViewAsPdf("MeEmployeePerformanceReviewPDF", model)
            {
                PageSize = Size.A4,
                PageOrientation = Orientation.Landscape,
                FileName = newfileName
            };
        }

        //Delete Employee Performance
        [CustomAuthorize]

        public JsonResult DeletePerformanceReview(int EmpPerfReviewID, int ReviewId, int EmpId)
        {
            ReviewDetails model = new ReviewDetails();
            bool saveData = _employeePerformanceMethod.DeleteEmployeePerformanceReview(EmpPerfReviewID, ReviewId, EmpId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


    }
}