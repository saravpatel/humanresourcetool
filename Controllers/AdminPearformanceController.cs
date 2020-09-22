using HRTool.CommanMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRTool.DataModel;
using HRTool.Models.Admin;
using HRTool.CommanMethods.Settings;
using ClosedXML.Excel;
using System.Data;
using System.IO;
using Newtonsoft.Json;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class AdminPearformanceController : Controller
    {
        //
        // GET: /AdminPearformance/
        //Constants
        EvolutionEntities _db = new EvolutionEntities();
        PerformanceSettingMethod _PerformanceSetting = new PerformanceSettingMethod();
        public ActionResult Index()
        {
            return View();
        }
        // Performance Index
        public ActionResult PerformanceIndex()
        {
            PerformanceIndexPageViewModel model = new PerformanceIndexPageViewModel();
            var PerformanceReviewList = _db.PerformanceSettings.Where(x => x.Archived == false).ToList();
            foreach (var item in PerformanceReviewList)
            {
                model.PerformanceReviewList.Add(new SelectListItem() { Text = item.ReviewText, Value = item.Id.ToString() });
            }
            model.NumberofReviewsOpenthisYear = _PerformanceSetting.getAllOpenReviewThisYear(0).ToString();
            model.NumberofCompletedReviewthisYear = _PerformanceSetting.getAllCompletedReviewThisYear(0).ToString();
            model.NumberofOpenReviewAllYear = _PerformanceSetting.getAllOpenReview(0).ToString();
            model.NumberofCompletedReviewAllYear = _PerformanceSetting.getAllCloseReview().ToString();
            model.OutstandingReview = _PerformanceSetting.OustandingReviews(0).ToString();
            model.CustomerOutstanding = _PerformanceSetting.GetAllCustomerOustanding(0);
            model.WorkerOutstanding = _PerformanceSetting.GetAllCoWorkerOutStanding(0);
            model.ManagerOutstanding = _PerformanceSetting.GetAllManagerOutStandin(0);
            model.PerformanceIncrease = _PerformanceSetting.getTotalPerformanceIncrease();
            model.PerformanceCompletedReviewInce = _PerformanceSetting.getTotalReviewCompletedIncr();
            return PartialView("_partialPerformanceIndexView", model);
        }

        public JsonResult GetDataByReview(int ReviewId)
        {
            PerformanceIndexPageViewModel model = new PerformanceIndexPageViewModel();
            var PerformanceReviewList = _db.PerformanceSettings.Where(x => x.Archived == false).ToList();
            foreach (var item in PerformanceReviewList)
            {
                model.PerformanceReviewList.Add(new SelectListItem() { Text = item.ReviewText, Value = item.Id.ToString() });
            }
            model.SelectedPerformanceId = ReviewId;
            model.NumberofReviewsOpenthisYear = _PerformanceSetting.getAllOpenReviewThisYear(ReviewId).ToString();
            model.NumberofCompletedReviewthisYear = _PerformanceSetting.getAllCompletedReviewThisYear(ReviewId).ToString();
            model.NumberofOpenReviewAllYear = _PerformanceSetting.getAllOpenReview(ReviewId).ToString();
            model.NumberofCompletedReviewAllYear = _PerformanceSetting.getAllCloseReview().ToString();
            model.OutstandingReview = _PerformanceSetting.OustandingReviews(ReviewId).ToString();
            model.CustomerOutstanding = _PerformanceSetting.GetAllCustomerOustanding(ReviewId);
            model.WorkerOutstanding = _PerformanceSetting.GetAllCoWorkerOutStanding(ReviewId);
            model.ManagerOutstanding = _PerformanceSetting.GetAllManagerOutStandin(0);
            if (ReviewId != 0)
            {
                model.CountTotalQuestionByReview = _PerformanceSetting.CountTotalQuestionByReview(ReviewId);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOutstandingGraph(int ReviewId)
        {
            PerformanceIndexPageViewModel model = new PerformanceIndexPageViewModel();
            model.CustomerOutstanding = _PerformanceSetting.GetAllCustomerOustanding(ReviewId);
            model.WorkerOutstanding = _PerformanceSetting.GetAllCoWorkerOutStanding(ReviewId);
            model.ManagerOutstanding = _PerformanceSetting.GetAllManagerOutStandin(0);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        // Performance OverallScore

        public ActionResult GetOverAllScore()
        {
            PerformanceIndexPageViewModel model = new PerformanceIndexPageViewModel();
            var data = _db.PerformanceSettings.Where(x => x.Archived == false).ToList();
            foreach (var item in data)
            {
                model.PerformanceReviewList.Add(new SelectListItem { Text = item.ReviewText, Value = item.Id.ToString() });
            }
            var jobTitle = _PerformanceSetting.getAllSystemValueListByKeyName("Job Title List");
            foreach (var item in jobTitle)
            {
                model.JobTitleList.Add(new SelectListItem { Text = item.Value, Value = item.Id.ToString() });
            }
            var PoolList = _db.Pools.Where(x => x.Archived == false).ToList();
            foreach (var item in PoolList)
            {
                model.PoolList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            var ResourceList = _db.AspNetUsers.Where(x => x.Archived == false && x.SSOID.StartsWith("W")).ToList();
            foreach (var item in ResourceList)
            {
                model.ManagerResourceList.Add(new SelectListItem { Text = item.FirstName + " " + item.LastName + " " + item.SSOID, Value = item.Id.ToString() });
            }
            List<string> FilterList = new List<string>();
            FilterList.Add("Not Started");
            FilterList.Add("In Progress by Employee");
            FilterList.Add("In Progress by Customer");
            //FilterList.Add("In Progress by Manager");
            FilterList.Add("Shared by Employee");
            FilterList.Add("Shared by Customer");
            //FilterList.Add("Shared by Manager");
            FilterList.Add("Close");
            foreach (var item in FilterList)
            {
                model.FilterList.Add(new SelectListItem() { Text = item, Value = item });
            }
            var EmployeeOverAllList = _PerformanceSetting.GetAllOverAllScoreList().ToList();
            foreach (var item in EmployeeOverAllList)
            {
                PerforamnceOverAllList pmodel = new PerforamnceOverAllList();
                pmodel.EmployeeId =Convert.ToString(item.EmployeeID);
                pmodel.PerfReviewId = Convert.ToString(item.PerfReviewId);
                pmodel.EmployeeName = item.EmployeeName;
                pmodel.EmployeeImage = item.EmployeeImage;
                pmodel.InviteEmployeeName = item.InviteCustomerName;
                pmodel.InviteEmployeeReviewStatus = item.CustomerInviteStatus;
                pmodel.InviteEmployeeImage = item.CustomerInviteStatus;
                if (!string.IsNullOrEmpty(item.OverAllScore))
                {
                    pmodel.OverAllScore = item.OverAllScore;
                }
                else
                {
                    pmodel.OverAllScore = "0";
                }
                model.GetOverAllEmployeeList.Add(pmodel);
            }
            return PartialView("_PartialPerformanceOverAllList", model);
        }
        public ActionResult GetOverAllScoreListByFilter(string ReviewId, string JobtitleId, string ManagerId, string PoolId, string FilterValue)
        {
            PerformanceIndexPageViewModel model = new PerformanceIndexPageViewModel();
            var data = _db.PerformanceSettings.Where(x => x.Archived == false).ToList();
            foreach (var item in data)
            {
                model.PerformanceReviewList.Add(new SelectListItem { Text = item.ReviewText, Value = item.Id.ToString() });
            }
            var jobTitle = _PerformanceSetting.getAllSystemValueListByKeyName("Job Title List");
            foreach (var item in jobTitle)
            {
                model.JobTitleList.Add(new SelectListItem { Text = item.Value, Value = item.Id.ToString() });
            }
            var PoolList = _db.Pools.Where(x => x.Archived == false).ToList();
            foreach (var item in PoolList)
            {
                model.PoolList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            var ResourceList = _db.AspNetUsers.Where(x => x.Archived == false && x.SSOID.StartsWith("W")).ToList();
            foreach (var item in ResourceList)
            {
                model.ManagerResourceList.Add(new SelectListItem { Text = item.FirstName + " " + item.LastName + " " + item.SSOID, Value = item.Id.ToString() });
            }
            List<string> FilterList = new List<string>();
            FilterList.Add("Not Started");
            FilterList.Add("In Progress by Employee");
            FilterList.Add("In Progress by Customer");
            //FilterList.Add("In Progress by Manager");
            FilterList.Add("Shared by Employee");
            FilterList.Add("Shared by Customer");
            //FilterList.Add("Shared by Manager");
            FilterList.Add("Close");
            foreach (var item in FilterList)
            {
                model.FilterList.Add(new SelectListItem() { Text = item, Value = item });
            }            
            var EmployeeOverAllList = _PerformanceSetting.GetAllOverAllScoreList().ToList();
            if (!string.IsNullOrEmpty(ReviewId) && ReviewId != "0")
            {
                int RId = Convert.ToInt32(ReviewId);
                EmployeeOverAllList = EmployeeOverAllList.Where(x => x.ReviewId == RId).ToList();
                model.SelectedReviewId = ReviewId;
            }
            if (!string.IsNullOrEmpty(JobtitleId) && JobtitleId != "0")
            {
                int JId = Convert.ToInt32(JobtitleId);
                EmployeeOverAllList = EmployeeOverAllList.Where(x => x.JobTitleId == JId).ToList();
                model.SelectedJobTitleId = JobtitleId;
            }
            if (!string.IsNullOrEmpty(PoolId) && PoolId != "0")
            {
                int PId = Convert.ToInt32(PoolId);
                EmployeeOverAllList = EmployeeOverAllList.Where(x => x.EmployeePoolId == PId).ToList();
                model.SelectedPoolId = PoolId;
            }
            if (!string.IsNullOrEmpty(ManagerId) && ManagerId != "0")
            {
                int MId = Convert.ToInt32(ManagerId);
                EmployeeOverAllList = EmployeeOverAllList.Where(x => x.EmployeeReportToId == MId).ToList();
                model.SelectedManagerId = ManagerId;
            }
            if(!string.IsNullOrEmpty(FilterValue) && FilterValue!="0")
            {
                if(FilterValue== "In Progress by Customer")
                {
                    EmployeeOverAllList = EmployeeOverAllList.Where(x => x.CustomerInviteStatus == "Invite").ToList();
                    model.SelectedFilterValue = "In Progress by Customer";
                }
                if(FilterValue== "In Progress by Employee")
                {
                    EmployeeOverAllList = EmployeeOverAllList.Where(x => x.CoWorkerInviteStatus == "Invited").ToList();
                    model.SelectedFilterValue = "In Progress by Employee";
                }
            }
            foreach (var item in EmployeeOverAllList)
            {
                PerforamnceOverAllList pmodel = new PerforamnceOverAllList();
                pmodel.EmployeeId =Convert.ToString(item.EmployeeID);
                pmodel.PerfReviewId = Convert.ToString(item.PerfReviewId);
                pmodel.EmployeeName = item.EmployeeName;
                pmodel.EmployeeImage = item.EmployeeImage;
                pmodel.InviteEmployeeName = item.InviteCustomerName;
                pmodel.InviteEmployeeReviewStatus = item.CustomerInviteStatus;
                pmodel.InviteEmployeeImage = item.CustomerInviteStatus;
                if (!string.IsNullOrEmpty(item.OverAllScore))
                {
                    pmodel.OverAllScore = "0";
                }
                else
                {
                    pmodel.OverAllScore = item.OverAllScore;
                }
                model.GetOverAllEmployeeList.Add(pmodel);
            }
            return PartialView("_PartialPerformanceOverAllList", model);
        }

        public ActionResult ExportToExcelOverallScore()
        {
            string ResourceList = "EmployeeList";
            var data = _PerformanceSetting.GetAllOverAllScoreList().ToList();
            DataTable dttable = new DataTable("EmployeeList");
            dttable.Columns.Add("Employee Name", typeof(string));
            dttable.Columns.Add("Invite Employee Name", typeof(string));
            dttable.Columns.Add("Invite Employee Review Status", typeof(string));
            dttable.Columns.Add("OverAllScore", typeof(string));
            dttable.Columns.Add("ReviewName", typeof(string));
            dttable.Columns.Add("Jobtitle", typeof(string));
            dttable.Columns.Add("ManagerName", typeof(string));
            dttable.Columns.Add("PoolName", typeof(string));
            foreach (var item in data)
            {
                List<string> lstStrRow = new List<string>();
                lstStrRow.Add(item.EmployeeName);
                lstStrRow.Add(item.InviteCustomerName);
                lstStrRow.Add(item.CustomerInviteStatus);
                lstStrRow.Add(item.OverAllScore);
                if (item.ReviewId != 0 && item.ReviewId != null)
                {
                    var ReviewName = _db.PerformanceSettings.Where(x => x.Id == item.ReviewId && x.Archived == false).FirstOrDefault();
                    lstStrRow.Add(ReviewName.ReviewText);
                }
                else
                {
                    lstStrRow.Add(null);
                }
                if (item.JobTitleId != 0 && item.JobTitleId != null)
                {
                    var JobTitle = _db.SystemListValues.Where(x => x.Id == item.JobTitleId && x.Archived == false).FirstOrDefault();
                    lstStrRow.Add(JobTitle.Value);
                }
                else
                { lstStrRow.Add(null); }
                if (item.EmployeeReportToId != 0 && item.EmployeeReportToId != null)
                {
                    var ManagerName = _db.AspNetUsers.Where(x => x.Id == item.EmployeeReportToId && x.Archived == false).FirstOrDefault();
                    lstStrRow.Add(ManagerName.FirstName + " " + ManagerName.LastName + "-" + ManagerName.SSOID);
                }
                if (item.EmployeePoolId != 0 && item.EmployeePoolId != null)
                {
                    var PoolName = _db.Pools.Where(x => x.Id == item.EmployeePoolId && x.Archived == false).FirstOrDefault();
                    lstStrRow.Add(PoolName.Name);
                }
                string[] newArray = lstStrRow.ToArray();
                dttable.Rows.Add(newArray);
            }
            #region export file
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dttable);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + ResourceList + "_Skills.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            #endregion
            return View();
        }


        // CoreScore List

        public ActionResult CoreScoreList()
        {
            PerformanceIndexPageViewModel model = new PerformanceIndexPageViewModel();
            var data = _PerformanceSetting.GetAllCoreScoreList();
            var ReviewList = _db.PerformanceSettings.Where(x => x.Archived == false).ToList();
            foreach (var item in ReviewList)
            {
                model.PerformanceReviewList.Add(new SelectListItem() { Text = item.ReviewText, Value = item.Id.ToString() });
            }
            var jobTitleList = _PerformanceSetting.getAllSystemValueListByKeyName("Job Title List");
            foreach (var item in jobTitleList)
            {
                model.JobTitleList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            var PoolList = _db.Pools.Where(x => x.Archived == false).ToList();
            foreach (var item in PoolList)
            {
                model.PoolList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            var ResourceList = _db.AspNetUsers.Where(x => x.SSOID.StartsWith("W") && x.Archived == false).ToList();
            foreach (var item in ResourceList)
            {
                model.ManagerResourceList.Add(new SelectListItem() { Text = item.FirstName + " " + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }
            foreach (var item in data)
            {
                PerforamnceOverAllList pmodel = new PerforamnceOverAllList();
                pmodel.PerfReviewId = Convert.ToString(item.PeformanceId);
                pmodel.EmployeeId =Convert.ToString(item.EmployeeId);
                pmodel.EmployeeName = item.EmployeeName;
                pmodel.EmployeeImage = item.EmloyeeImage;
                if (!string.IsNullOrEmpty(item.CoreScore))
                {
                    pmodel.CoreScore = item.CoreScore;
                }
                else
                {
                    pmodel.CoreScore = "0";
                }
                model.GetOverAllEmployeeList.Add(pmodel);
            }

            return PartialView("_partialPerformanceCoreScoreList", model);
        }

        //Filter by CoreScore

        public ActionResult CoreScoreByFilter(string ReviewId, string JobtitleId, string ManagerId, string PoolId, string FilterValue)
        {
            PerformanceIndexPageViewModel model = new PerformanceIndexPageViewModel();
            var ReviewList = _db.PerformanceSettings.Where(x => x.Archived == false).ToList();
            foreach (var item in ReviewList)
            {
                model.PerformanceReviewList.Add(new SelectListItem() { Text = item.ReviewText, Value = item.Id.ToString() });
            }
            var jobTitleList = _PerformanceSetting.getAllSystemValueListByKeyName("Job Title List");
            foreach (var item in jobTitleList)
            {
                model.JobTitleList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            var PoolList = _db.Pools.Where(x => x.Archived == false).ToList();
            foreach (var item in PoolList)
            {
                model.PoolList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            var ResourceList = _db.AspNetUsers.Where(x => x.SSOID.StartsWith("W") && x.Archived == false).ToList();
            foreach (var item in ResourceList)
            {
                model.ManagerResourceList.Add(new SelectListItem() { Text = item.FirstName + " " + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }
            var EmployeeScoreList = _PerformanceSetting.GetAllCoreScoreList();
            if (!string.IsNullOrEmpty(ReviewId) && ReviewId != "0")
            {
                int RId = Convert.ToInt32(ReviewId);
                EmployeeScoreList = EmployeeScoreList.Where(x => x.ReviewId == RId).ToList();
                model.SelectedReviewId = ReviewId;
            }
            if (!string.IsNullOrEmpty(JobtitleId) && JobtitleId != "0")
            {
                int JId = Convert.ToInt32(JobtitleId);
                EmployeeScoreList = EmployeeScoreList.Where(x => x.JobTitleId == JId).ToList();
                model.SelectedJobTitleId = JobtitleId;
            }
            if (!string.IsNullOrEmpty(ManagerId) && ManagerId != "0")
            {
                int MId = Convert.ToInt32(ManagerId);
                EmployeeScoreList = EmployeeScoreList.Where(x => x.ReportToId == MId).ToList();
                model.SelectedManagerId = ManagerId;
            }
            if (!string.IsNullOrEmpty(PoolId) && PoolId != "0")
            {
                int PId = Convert.ToInt32(PoolId);
                EmployeeScoreList = EmployeeScoreList.Where(x => x.PoolId == PId).ToList();
                model.SelectedPoolId = PoolId;
            }
            foreach (var item in EmployeeScoreList)
            {
                PerforamnceOverAllList pmodel = new PerforamnceOverAllList();
                pmodel.EmployeeId = Convert.ToString(item.EmployeeId);
                pmodel.PerfReviewId = Convert.ToString(item.PeformanceId);
                pmodel.EmployeeName = item.EmployeeName;
                pmodel.EmployeeImage = item.EmloyeeImage;
                if (!string.IsNullOrEmpty(item.CoreScore))
                {
                    pmodel.CoreScore = item.CoreScore;
                }
                else
                {
                    pmodel.CoreScore = "0";
                }

                model.GetOverAllEmployeeList.Add(pmodel);
            }
            return PartialView("_partialPerformanceCoreScoreList", model);
        }

        // ExportToExcelCoreScore

        public ActionResult ExportToExcelCoreScore()
        {
            string ResourceList = "EmployeeList";
            var data = _PerformanceSetting.GetAllCoreScoreList().ToList();
            DataTable dttable = new DataTable("EmployeeList");
            dttable.Columns.Add("Employee Name", typeof(string));
            dttable.Columns.Add("CoreScore", typeof(string));
            dttable.Columns.Add("ReviewName", typeof(string));
            dttable.Columns.Add("JobTitleName", typeof(string));
            dttable.Columns.Add("PoolName", typeof(string));
            dttable.Columns.Add("ManagerName", typeof(string));
            foreach (var item in data)
            {
                List<string> lstStrRow = new List<string>();
                lstStrRow.Add(item.EmployeeName);
                lstStrRow.Add(item.CoreScore);
                if (item.ReviewId != 0 && item.ReviewId != null)
                {
                    var ReviewName = _db.PerformanceSettings.Where(x => x.Archived == false).FirstOrDefault();
                    lstStrRow.Add(ReviewName.ReviewText);
                }
                else
                {
                    lstStrRow.Add(null);
                }
                if (item.JobTitleId != 0 && item.JobTitleId != null)
                {
                    var JobTitleName = _db.SystemListValues.Where(x => x.Id == item.JobTitleId && x.Archived == false).FirstOrDefault();
                    lstStrRow.Add(JobTitleName.Value);
                }
                else
                {
                    lstStrRow.Add(null);
                }
                if (item.PoolId != 0 && item.PoolId != null)
                {
                    var PoolName = _db.Pools.Where(x => x.Archived == false && x.Id == item.PoolId).FirstOrDefault();
                    lstStrRow.Add(PoolName.Name);
                }
                else
                {
                    lstStrRow.Add(null);
                }
                if (item.ReportToId != 0 && item.ReportToId != null)
                {
                    var ManagerName = _db.AspNetUsers.Where(x => x.Id == item.ReportToId && x.Archived == false).FirstOrDefault();
                    lstStrRow.Add(ManagerName.FirstName + " " + ManagerName.LastName + "-" + ManagerName.SSOID);
                }
                else
                {
                    lstStrRow.Add(null);
                }


                string[] newArray = lstStrRow.ToArray();
                dttable.Rows.Add(newArray);
            }
            #region export file
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dttable);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + ResourceList + "_Skills.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            #endregion
            return View();
        }

        public ActionResult GetEmployeePerformanceCompare()
        {
            PerformanceIndexPageViewModel model = new PerformanceIndexPageViewModel();
            var BussinessList = _db.Businesses.Where(x => x.Archived == false).ToList();
            foreach (var item in BussinessList)
            {
                model.BussinessList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            var JobTitleList = _PerformanceSetting.getAllSystemValueListByKeyName("Job Title List");
            foreach (var item in JobTitleList)
            {
                model.JobTitleList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            var ReviewList = _db.PerformanceSettings.Where(x => x.Archived == false).ToList();
            foreach (var item in ReviewList)
            {
                model.PerformanceReviewList.Add(new SelectListItem() { Text = item.ReviewText, Value = item.Id.ToString() });
            }
            var EmployeeList = _db.AspNetUsers.Where(x => x.SSOID.StartsWith("W") && x.Archived == false).ToList();
            foreach (var item in EmployeeList)
            {
                model.ManagerResourceList.Add(new SelectListItem() { Text = item.FirstName + " " + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }
            return PartialView("_PartialPearformanceCompare", model);
        }
        public JsonResult GetDivisonByBusinessId(int BusinessID)
        {
            List<KeyValue> listOfKeyValue = new List<KeyValue>();
            try
            {
                listOfKeyValue = _db.Divisions.Where(x => x.BusinessID == BusinessID).Select(xx => new KeyValue
                {
                    Key = xx.Id,
                    Value = xx.Name
                }).ToList();

                return Json(listOfKeyValue, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPoolByDivisonID(int BusinessId, int DivisonID)
        {
            List<KeyValue> listOfKeyValue = new List<KeyValue>();
            try
            {
                listOfKeyValue = _db.Pools.Where(x => x.DivisionID == DivisonID && x.BusinessID == BusinessId).Select(xx => new KeyValue
                {
                    Key = xx.Id,
                    Value = xx.Name
                }).ToList();
                return Json(listOfKeyValue, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFunctionByPoolId(int BusinessId, int DivisonID)
        {
            List<KeyValue> listOfKeyValue = new List<KeyValue>();
            try
            {
                listOfKeyValue = _db.Functions.Where(x => x.DivisionID == DivisonID && x.BusinessID == BusinessId).Select(xx => new KeyValue
                {
                    Key = xx.Id,
                    Value = xx.Name
                }).ToList();
                return Json(listOfKeyValue, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        //Performance Graph

        public JsonResult GetEmployeePerformanceGraph()
        {
            var data = _PerformanceSetting.GetEmployeePerformancDetails();
            List<PerformanceGraph> _ListPerformanceGraph = new List<PerformanceGraph>();
            var GroupByPearformance = data.GroupBy(x => x.EmployeeId, (key, g) => new
            {
                EmpId = key,
                EmpName = g.ToList(),
            });
            foreach (var item1 in GroupByPearformance)
            {
                PerformanceGraph _PerformanceGraph = new PerformanceGraph();
                _PerformanceGraph.EmpID = item1.EmpId;
                _PerformanceGraph.EmployeeName = item1.EmpName.FirstOrDefault().EmployeeName;
                _PerformanceGraph.EmployeeImage = item1.EmpName.FirstOrDefault().EmployeeImage;
                PerformanceReviewGraph model = new PerformanceReviewGraph();
                var _PerformanceReviewGraph = data.Where(x => x.EmployeeId == item1.EmpId).OrderByDescending(x => x.CreatedDate).Take(1).Select(s => new PerformanceReviewGraph
                {
                    LastReview = string.IsNullOrEmpty(s.OverAllScore) ? 0.00 : Convert.ToDouble(s.OverAllScore)
                    //PerviouseReview = s.OverAllScore != null ? 0.00 : Convert.ToDouble(s.OverAllScore)
                });
                var _PerformanceReviewGraph1 = data.Where(x => x.EmployeeId == item1.EmpId).OrderByDescending(x => x.CreatedDate).Skip(1).Take(1).Select(s => new PerformanceReviewGraph
                {
                    //LastReview = string.IsNullOrEmpty(s.OverAllScore) ? 0.00 : Convert.ToDouble(s.OverAllScore),
                    PerviouseReview = s.OverAllScore == null ? 0.00 : Convert.ToDouble(s.OverAllScore)
                });
                if (_PerformanceReviewGraph.FirstOrDefault() != null)
                {
                    model.LastReview = _PerformanceReviewGraph.FirstOrDefault().LastReview;
                }
                if (_PerformanceReviewGraph1.FirstOrDefault() != null)
                {
                    model.PerviouseReview = _PerformanceReviewGraph1.FirstOrDefault().PerviouseReview;
                }
                _PerformanceGraph.ListOfPerformanceReviewGraph.Add(model);
                _ListPerformanceGraph.Add(_PerformanceGraph);
            }
            return Json(_ListPerformanceGraph, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPerformanceGraphByFilter(string ReviewId, string ManagerId, string BussinessId, string JobTitle, string DivisionId, string PoolId, string FunctionId, string OverallScore, string CoreStrengths, string CustomerScore, string AverageScore)
        {
            var data = _PerformanceSetting.GetEmployeePerformancDetails();
            if (!string.IsNullOrEmpty(ReviewId) && ReviewId != "0")
            {
                int RId = Convert.ToInt32(ReviewId);
                data = data.Where(x => x.ReviewId == RId).ToList();
            }
            if (!string.IsNullOrEmpty(ManagerId) && ManagerId != "0")
            {
                int MId = Convert.ToInt32(ManagerId);
                data = data.Where(x => x.ReportToId == MId).ToList();
            }
            if (!string.IsNullOrEmpty(BussinessId) && BussinessId != "0")
            {
                int BId = Convert.ToInt32(BussinessId);
                data = data.Where(x => x.BussinessId == BId).ToList();
            }
            if (!string.IsNullOrEmpty(JobTitle) && JobTitle != "0")
            {
                int JId = Convert.ToInt32(JobTitle);
                data = data.Where(x => x.JobTitleId == JId).ToList();
            }
            if (!string.IsNullOrEmpty(DivisionId) && DivisionId != "0")
            {
                int DId = Convert.ToInt32(DivisionId);
                data = data.Where(x => x.DivisionId == DId).ToList();
            }
            if (!string.IsNullOrEmpty(PoolId) && PoolId != "0")
            {
                int PId = Convert.ToInt32(PoolId);
                data = data.Where(x => x.PoolId == PId).ToList();
            }
            if (!string.IsNullOrEmpty(FunctionId) && FunctionId != "0")
            {
                int FId = Convert.ToInt32(FunctionId);
                data = data.Where(x => x.FunctionId == FId).ToList();
            }
            if (!string.IsNullOrEmpty(OverallScore) && OverallScore != "0")
            {
                data = data.Where(x => x.OverAllScore == OverallScore).ToList();
            }
            List<PerformanceGraph> _ListPerformanceGraph = new List<PerformanceGraph>();
            var GroupByPearformance = data.GroupBy(x => x.EmployeeId, (key, g) => new
            {
                EmpId = key,
                EmpName = g.ToList()
            });
            foreach (var item1 in GroupByPearformance)
            {
                PerformanceGraph _PerformanceGraph = new PerformanceGraph();
                _PerformanceGraph.EmpID = item1.EmpId;
                _PerformanceGraph.EmployeeName = item1.EmpName.FirstOrDefault().EmployeeName;
                PerformanceReviewGraph model = new PerformanceReviewGraph();
                var _PerformanceReviewGraph = data.Where(x => x.EmployeeId == item1.EmpId).OrderByDescending(x => x.CreatedDate).Take(1).Select(s => new PerformanceReviewGraph
                {
                    LastReview = string.IsNullOrEmpty(s.OverAllScore) ? 0.00 : Convert.ToDouble(s.OverAllScore)
                    //PerviouseReview = s.OverAllScore != null ? 0.00 : Convert.ToDouble(s.OverAllScore)
                });
                var _PerformanceReviewGraph1 = data.Where(x => x.EmployeeId == item1.EmpId).OrderByDescending(x => x.CreatedDate).Skip(1).Take(1).Select(s => new PerformanceReviewGraph
                {
                    //LastReview = string.IsNullOrEmpty(s.OverAllScore) ? 0.00 : Convert.ToDouble(s.OverAllScore),
                    PerviouseReview = s.OverAllScore == null ? 0.00 : Convert.ToDouble(s.OverAllScore)
                });
                if (_PerformanceReviewGraph.FirstOrDefault() != null)
                {
                    model.LastReview = _PerformanceReviewGraph.FirstOrDefault().LastReview;
                }
                if (_PerformanceReviewGraph1.FirstOrDefault() != null)
                {
                    model.PerviouseReview = _PerformanceReviewGraph1.FirstOrDefault().PerviouseReview;
                }
                _PerformanceGraph.ListOfPerformanceReviewGraph.Add(model);
                _ListPerformanceGraph.Add(_PerformanceGraph);
            }
            return Json(_ListPerformanceGraph, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTopImproversePerGraphByFilter(string ReviewId, string ManagerId, string BussinessId, string JobTitle, string DivisionId, string PoolId, string FunctionId, string OverallScore, string CoreStrengths, string CustomerScore, string AverageScore)
        {
            var data = _PerformanceSetting.GetEmployeePerformancDetails();
            if (!string.IsNullOrEmpty(ReviewId) && ReviewId != "0")
            {
                int RId = Convert.ToInt32(ReviewId);
                data = data.Where(x => x.ReviewId == RId).ToList();
            }
            if (!string.IsNullOrEmpty(JobTitle) && JobTitle != "0")
            {
                int JId = Convert.ToInt32(JobTitle);
                data = data.Where(x => x.JobTitleId == JId).ToList();
            }
            if (!string.IsNullOrEmpty(ManagerId) && ManagerId != "0")
            {
                int MId = Convert.ToInt32(ManagerId);
                data = data.Where(x => x.ReportToId == MId).ToList();
            }
            if (!string.IsNullOrEmpty(BussinessId) && BussinessId != "0")
            {
                int BId = Convert.ToInt32(BussinessId);
                data = data.Where(x => x.BussinessId == BId).ToList();
            }
            if (!string.IsNullOrEmpty(DivisionId) && DivisionId != "0")
            {
                int DId = Convert.ToInt32(DivisionId);
                data = data.Where(x => x.DivisionId == DId).ToList();
            }
            if (!string.IsNullOrEmpty(PoolId) && PoolId != "0")
            {
                int PId = Convert.ToInt32(PoolId);
                data = data.Where(x => x.PoolId == PId).ToList();
            }
            if (!string.IsNullOrEmpty(FunctionId) && FunctionId != "0")
            {
                int FId = Convert.ToInt32(FunctionId);
                data = data.Where(x => x.FunctionId == FId).ToList();
            }
            List<PerformanceGraph> _ListPerformanceGraph = new List<PerformanceGraph>();
            var GroupByPearformance = data.GroupBy(x => x.EmployeeId, (key, g) => new
            {
                EmpId = key,
                EmpName = g.ToList()
            });
            foreach (var item1 in GroupByPearformance)
            {
                PerformanceGraph _PerformanceGraph = new PerformanceGraph();
                _PerformanceGraph.EmpID = item1.EmpId;
                _PerformanceGraph.EmployeeName = item1.EmpName.FirstOrDefault().EmployeeName;
                PerformanceReviewGraph model = new PerformanceReviewGraph();
                var _PerformanceReviewGraph = data.Where(x => x.EmployeeId == item1.EmpId).OrderByDescending(x => x.CreatedDate).Take(1).Select(s => new PerformanceReviewGraph
                {
                    LastReview = string.IsNullOrEmpty(s.OverAllScore) ? 0.00 : Convert.ToDouble(s.OverAllScore)
                });
                var _PerformanceReviewGraph1 = data.Where(x => x.EmployeeId == item1.EmpId).OrderByDescending(x => x.CreatedDate).Skip(1).Take(1).Select(s => new PerformanceReviewGraph
                {
                    //LastReview = string.IsNullOrEmpty(s.OverAllScore) ? 0.00 : Convert.ToDouble(s.OverAllScore),
                    PerviouseReview = s.OverAllScore == null ? 0.00 : Convert.ToDouble(s.OverAllScore)
                });
                if (_PerformanceReviewGraph.FirstOrDefault() != null)
                {
                    model.LastReview = _PerformanceReviewGraph.FirstOrDefault().LastReview;
                }
                if (_PerformanceReviewGraph1.FirstOrDefault() != null)
                {
                    model.PerviouseReview = _PerformanceReviewGraph1.FirstOrDefault().PerviouseReview;
                }
                _PerformanceGraph.DiffOfPerfReview = model.LastReview - model.PerviouseReview;
                _PerformanceGraph.ListOfPerformanceReviewGraph.Add(model);
                _ListPerformanceGraph.Add(_PerformanceGraph);
            }
            _ListPerformanceGraph = _ListPerformanceGraph.OrderByDescending(x => x.DiffOfPerfReview).ToList();
            var totalEmployee = _ListPerformanceGraph.Count();
            int employee = totalEmployee * 20 / 100;          
            if(employee==0)
            {
                employee = _ListPerformanceGraph.Where(x => x.DiffOfPerfReview > 1).Count();
            }
            _ListPerformanceGraph=_ListPerformanceGraph.Where(x => x.DiffOfPerfReview > 1).ToList();
            _ListPerformanceGraph = _ListPerformanceGraph.Take(employee).ToList();
            return Json(_ListPerformanceGraph, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBiggestDropPerGraphByFilter(string ReviewId, string ManagerId, string BussinessId, string JobTitle, string DivisionId, string PoolId, string FunctionId, string OverallScore, string CoreStrengths, string CustomerScore, string AverageScore)
        {
            var data = _PerformanceSetting.GetEmployeePerformancDetails();
            if (!string.IsNullOrEmpty(ReviewId) && ReviewId != "0")
            {
                int RId = Convert.ToInt32(ReviewId);
                data = data.Where(x => x.ReviewId == RId).ToList();
            }
            if (!string.IsNullOrEmpty(ManagerId) && ManagerId != "0")
            {
                int MId = Convert.ToInt32(ManagerId);
                data = data.Where(x => x.ReportToId == MId).ToList();
            }
            if (!string.IsNullOrEmpty(JobTitle) && JobTitle != "0")
            {
                int JId = Convert.ToInt32(JobTitle);
                data = data.Where(x => x.JobTitleId == JId).ToList();
            }
            if (!string.IsNullOrEmpty(BussinessId) && BussinessId != "0")
            {
                int BId = Convert.ToInt32(BussinessId);
                data = data.Where(x => x.BussinessId == BId).ToList();
            }
            if (!string.IsNullOrEmpty(DivisionId) && DivisionId != "0")
            {
                int DId = Convert.ToInt32(DivisionId);
                data = data.Where(x => x.DivisionId == DId).ToList();
            }
            if (!string.IsNullOrEmpty(PoolId) && PoolId != "0")
            {
                int PId = Convert.ToInt32(PoolId);
                data = data.Where(x => x.PoolId == PId).ToList();
            }
            if (!string.IsNullOrEmpty(FunctionId) && FunctionId != "0")
            {
                int FId = Convert.ToInt32(FunctionId);
                data = data.Where(x => x.FunctionId == FId).ToList();
            }
            List<PerformanceGraph> _ListPerformanceGraph = new List<PerformanceGraph>();
            var GroupByPearformance = data.GroupBy(x => x.EmployeeId, (key, g) => new
            {
                EmpId = key,
                EmpName = g.ToList()
            });
            foreach (var item1 in GroupByPearformance)
            {
                PerformanceGraph _PerformanceGraph = new PerformanceGraph();
                _PerformanceGraph.EmpID = item1.EmpId;
                _PerformanceGraph.EmployeeName = item1.EmpName.FirstOrDefault().EmployeeName;
                PerformanceReviewGraph model = new PerformanceReviewGraph();
                var _PerformanceReviewGraph = data.Where(x => x.EmployeeId == item1.EmpId).OrderByDescending(x => x.CreatedDate).Take(1).Select(s => new PerformanceReviewGraph
                {
                    LastReview = string.IsNullOrEmpty(s.OverAllScore) ? 0.00 : Convert.ToDouble(s.OverAllScore)
                });
                var _PerformanceReviewGraph1 = data.Where(x => x.EmployeeId == item1.EmpId).OrderByDescending(x => x.CreatedDate).Skip(1).Take(1).Select(s => new PerformanceReviewGraph
                {
                    //LastReview = string.IsNullOrEmpty(s.OverAllScore) ? 0.00 : Convert.ToDouble(s.OverAllScore),
                    PerviouseReview = s.OverAllScore == null ? 0.00 : Convert.ToDouble(s.OverAllScore)
                });
                if (_PerformanceReviewGraph.FirstOrDefault() != null)
                {
                    model.LastReview = _PerformanceReviewGraph.FirstOrDefault().LastReview;
                }
                if (_PerformanceReviewGraph1.FirstOrDefault() != null)
                {
                    model.PerviouseReview = _PerformanceReviewGraph1.FirstOrDefault().PerviouseReview;
                }
                _PerformanceGraph.DiffOfPerfReview = model.LastReview - model.PerviouseReview;
                _PerformanceGraph.ListOfPerformanceReviewGraph.Add(model);
                _ListPerformanceGraph.Add(_PerformanceGraph);
            }
            _ListPerformanceGraph = _ListPerformanceGraph.OrderBy(x => x.DiffOfPerfReview).ToList();
            var totalEmployee = _ListPerformanceGraph.Count();
            int employee = totalEmployee * 20 / 100;
            _ListPerformanceGraph = _ListPerformanceGraph.Take(employee).ToList();
            return Json(_ListPerformanceGraph, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMostConsistantEmployeeByFilter(string ReviewId, string ManagerId, string BussinessId, string JobTitle, string DivisionId, string PoolId, string FunctionId, string OverallScore, string CoreStrengths, string CustomerScore, string AverageScore)
        {
            var data = _PerformanceSetting.GetEmployeePerformancDetails();
            if(!string.IsNullOrEmpty(ReviewId)&& ReviewId!="0")
            {
                int RId = Convert.ToInt32(ReviewId);
                data = data.Where(x => x.ReviewId == RId).ToList();
            }           
            if(!string.IsNullOrEmpty(ManagerId) && ManagerId!="0")
            {
                int MId = Convert.ToInt32(ManagerId);
                data = data.Where(x => x.ReportToId == MId).ToList();
            }
            if(!string.IsNullOrEmpty(JobTitle) && JobTitle!="0")
            {
                int JId = Convert.ToInt32(JobTitle);
                data = data.Where(x => x.JobTitleId == JId).ToList();
            }
            if(!string.IsNullOrEmpty(BussinessId) && BussinessId!="0")
            {
                int BId = Convert.ToInt32(BussinessId);
                data = data.Where(x => x.BussinessId == BId).ToList();
            }
            if(!string.IsNullOrEmpty(DivisionId) && DivisionId!="0")
            {
                int DId = Convert.ToInt32(DivisionId);
                data = data.Where(x => x.DivisionId == DId).ToList();
            }
            if(!string.IsNullOrEmpty(PoolId) && PoolId!="0")
            {
                int PId = Convert.ToInt32(PoolId);
                data = data.Where(x => x.PoolId == PId).ToList();
            }
            if (!string.IsNullOrEmpty(FunctionId) && FunctionId!="0")
            {
                int FId = Convert.ToInt32(FunctionId);
                data = data.Where(x => x.FunctionId == FId).ToList();
            }
            List<PerformanceGraph> _ListPerformanceGraph = new List<PerformanceGraph>();
            var GroupByPearformance = data.GroupBy(x => x.EmployeeId, (key, g) => new
            {
                EmpId = key,
                EmpName = g.ToList()
            });
                    
            foreach (var item1 in GroupByPearformance)
            {
                PerformanceGraph _PerformanceGraph = new PerformanceGraph();
                _PerformanceGraph.EmpID = item1.EmpId;
                _PerformanceGraph.EmployeeName = item1.EmpName.FirstOrDefault().EmployeeName;
                PerformanceReviewGraph model = new PerformanceReviewGraph();
                var _PerformanceReviewGraph = data.Where(x => x.EmployeeId == item1.EmpId).OrderByDescending(x => x.CreatedDate).Take(1).Select(s => new PerformanceReviewGraph
                {
                    LastReview = string.IsNullOrEmpty(s.OverAllScore) ? 0.00 : Convert.ToDouble(s.OverAllScore)
                });
                var _PerformanceReviewGraph1 = data.Where(x => x.EmployeeId == item1.EmpId).OrderByDescending(x => x.CreatedDate).Skip(1).Take(1).Select(s => new PerformanceReviewGraph
                {
                    //LastReview = string.IsNullOrEmpty(s.OverAllScore) ? 0.00 : Convert.ToDouble(s.OverAllScore),
                    PerviouseReview = s.OverAllScore == null ? 0.00 : Convert.ToDouble(s.OverAllScore)
                });
                if (_PerformanceReviewGraph.FirstOrDefault() != null)
                {
                    model.LastReview = _PerformanceReviewGraph.FirstOrDefault().LastReview;
                }
                if (_PerformanceReviewGraph1.FirstOrDefault() != null)
                {
                    model.PerviouseReview = _PerformanceReviewGraph1.FirstOrDefault().PerviouseReview;
                }
                _PerformanceGraph.DiffOfPerfReview = model.LastReview - model.PerviouseReview;
                double MostConsistantValue = _PerformanceGraph.DiffOfPerfReview * 20 / 100;
                if (MostConsistantValue >= 0 && MostConsistantValue <= 1)
                {
                    _PerformanceGraph.MostConsistanceValue = _PerformanceGraph.DiffOfPerfReview;
                }
                else
                {
                    _PerformanceGraph.MostConsistanceValue = _PerformanceGraph.DiffOfPerfReview;
                }
                _PerformanceGraph.ListOfPerformanceReviewGraph.Add(model);
                _ListPerformanceGraph.Add(_PerformanceGraph);
            }
            _ListPerformanceGraph = _ListPerformanceGraph.OrderByDescending(x => x.MostConsistanceValue).ToList();            
            var totalEmployee = _ListPerformanceGraph.Count();
            int employee = totalEmployee * 20 / 100;
            _ListPerformanceGraph = _ListPerformanceGraph.Take(employee).ToList();
            return Json(_ListPerformanceGraph, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportToExcelChart()
        {
            string ResourceList = "Employee Performance";
            var data = _PerformanceSetting.GetEmployeePerformancDetails().ToList();
            DataTable dttable = new DataTable("EmployeeList");
            dttable.Columns.Add("Employee Name", typeof(string));
            dttable.Columns.Add("ReviewName", typeof(string));
            dttable.Columns.Add("JobTitleName", typeof(string));
            dttable.Columns.Add("PoolName", typeof(string));
            dttable.Columns.Add("ManagerName", typeof(string));
            dttable.Columns.Add("OverAllScore", typeof(string));
            dttable.Columns.Add("Co-WorkerScore", typeof(string));
            dttable.Columns.Add("JobRoleScore", typeof(string));
            foreach (var item in data)
            {
                List<string> lstStrRow = new List<string>();
                lstStrRow.Add(item.EmployeeName);                
                if (item.ReviewId != 0 && item.ReviewId != null)
                {
                    var ReviewName = _db.PerformanceSettings.Where(x => x.Archived == false).FirstOrDefault();
                    lstStrRow.Add(ReviewName.ReviewText);
                }
                else
                {
                    lstStrRow.Add(null);
                }
                if (item.JobTitleId != 0 && item.JobTitleId != null)
                {
                    var JobTitleName = _db.SystemListValues.Where(x => x.Id == item.JobTitleId && x.Archived == false).FirstOrDefault();
                    lstStrRow.Add(JobTitleName.Value);
                }
                else
                {
                    lstStrRow.Add(null);
                }
                if (item.PoolId != 0 && item.PoolId != null)
                {
                    var PoolName = _db.Pools.Where(x => x.Archived == false && x.Id == item.PoolId).FirstOrDefault();
                    lstStrRow.Add(PoolName.Name);
                }
                else
                {
                    lstStrRow.Add(null);
                }
                if (item.ReportToId != 0 && item.ReportToId != null)
                {
                    var ManagerName = _db.AspNetUsers.Where(x => x.Id == item.ReportToId && x.Archived == false).FirstOrDefault();
                    lstStrRow.Add(ManagerName.FirstName + " " + ManagerName.LastName + "-" + ManagerName.SSOID);
                }
                else
                {
                    lstStrRow.Add(null);
                }
                lstStrRow.Add(item.OverAllScore);
                lstStrRow.Add(Convert.ToString(item.CoWorkerScore));
                lstStrRow.Add(item.JobRoleScore);
                string[] newArray = lstStrRow.ToArray();
                dttable.Rows.Add(newArray);
            }
            #region export file
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dttable);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + ResourceList + "_Skills.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            #endregion
            return View();
        }



        public JsonResult GetEmployeeAllPerformance(string ssoId)
        {
            List<PerformanceGraph> _ListPerformanceGraph = new List<PerformanceGraph>();
            if (!string.IsNullOrEmpty(ssoId) && ssoId!="0")
            {
                int EmpId = _db.AspNetUsers.Where(x => x.SSOID == ssoId && x.Archived == false).FirstOrDefault().Id;
                var data = _db.EmployeePerformances.Where(x => x.EmployeeId == EmpId).ToList();
                foreach(var item in data)
                {
                    PerformanceGraph _PerformanceGraph = new PerformanceGraph();
                    var PerformanceReview = _db.PerformanceSettings.Where(x => x.Id == item.ReviewId).FirstOrDefault();
                    _PerformanceGraph.ReviewName = PerformanceReview.ReviewText;
                    if (!string.IsNullOrEmpty(item.OverallScore) && item.OverallScore!="0")
                    {
                        _PerformanceGraph.ReviewScore = Convert.ToDouble(item.OverallScore);
                    }
                    else
                    {
                        _PerformanceGraph.ReviewScore = 0;
                    }
                    _ListPerformanceGraph.Add(_PerformanceGraph);
                }
            }
            return Json(_ListPerformanceGraph,JsonRequestBehavior.AllowGet);
        }
    }
}