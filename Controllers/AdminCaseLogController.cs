
using HRTool.CommanMethods.Resources;
using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models;
using HRTool.Models.Admin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Script.Serialization;
using HRTool.CommanMethods.Admin;
using HRTool.CommanMethods;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class AdminCaseLogController : Controller
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        EmployeeMethod _employeeMethod = new EmployeeMethod();
        AdminCaseLogMethod _adminCaseLogMethod = new AdminCaseLogMethod();

        #endregion

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

        public List<AdminCaseLogViewModel> modelList()
        {
            List<AdminCaseLogViewModel> model = new List<AdminCaseLogViewModel>();
            //var listData = _adminCaseLogMethod.getActiveList();
            int EmpID = SessionProxy.UserId;
            var data = _db.AspNetUserRoles.Where(x => x.UserId == EmpID && x.RoleId == 1).ToList();
            var listData = _adminCaseLogMethod.getActiveCaseDetail().Where(x=>x.Reportsto==EmpID).ToList();
            var customerData = _db.AspNetUsers.Where(x => x.Archived == false && x.Id == EmpID).FirstOrDefault();
            if (data!=null && data.Count>0)
            {
                listData = _adminCaseLogMethod.getActiveCaseDetail().ToList();
            }
            else if(customerData!=null && customerData.SSOID.StartsWith("C"))
            {
                
                string[] EmpId = customerData.CustomerCareID.Split(',');
                for (int i = 0; i < EmpId.Length; i++)
                {
                    int empid = Convert.ToInt32(EmpId[i]);
                    var caseData = _adminCaseLogMethod.getActiveCaseDetail().Where(x => x.EmployeeID == empid).ToList();
                    foreach (var item in caseData)
                    {
                        listData.Add(item);
                    }
                }
            }
            else
            {
                listData = _adminCaseLogMethod.getActiveCaseDetail().Where(x => x.Reportsto == EmpID).ToList();
            }
            foreach (var item in listData)
            {
                AdminCaseLogViewModel m = new AdminCaseLogViewModel();
                m.Id = item.CaseId;
                var employeeDetail = _employeeMethod.getEmployeeById(item.EmployeeID);
                if (employeeDetail != null)
                {
                    m.EmployeeName = string.Format("{0} {1} - {2}", employeeDetail.FirstName, employeeDetail.LastName, employeeDetail.SSOID);
                }
                m.Summary = item.Summary;
                var categoryDetail = _otherSettingMethod.getSystemListValueById(item.Category);
                m.CategoryName = categoryDetail.Value;
                var statusDetail = _otherSettingMethod.getSystemListValueById(item.Status);
                m.Status = statusDetail.Value;
                var createdDetail = _employeeMethod.getEmployeeById(item.UserIDCreatedBy);
                m.CreatedName = string.Format("{0} {1}", createdDetail.FirstName, createdDetail.LastName);
                m.CreatedDate = String.Format("{0:dd-MMM-yyy}", item.CreatedDate);
                model.Add(m);
            }
            return model;
        }

        public ActionResult List()
        {
            List<AdminCaseLogViewModel> model = modelList();
            return PartialView("_partialAdminCaseLogList", model);
        }

        public ActionResult AddEditAdminCaseLog(int Id)
        {
            AdminCaseLogViewModel model = new AdminCaseLogViewModel();
            model.Id = Id;

            model.StatusList.Add(new SelectListItem() { Text = "-- Select Status --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Case Status List"))
            {
                model.StatusList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }

            model.CategoryList.Add(new SelectListItem() { Text = "-- Select Category --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Case Category"))
            {
                model.CategoryList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }

            model.EmployeeList.Add(new SelectListItem() { Text = "-- Select Employee --", Value = "0" });
            foreach (var item in _employeeMethod.GetAllEmployeeList())
            {
                model.EmployeeList.Add(new SelectListItem() { Text = string.Format("{0} {1} - {2}", item.FirstName, item.LastName, item.SSOID), Value = item.Id.ToString() });
            }

            if (model.Id > 0)
            {
                var caseDetail = _adminCaseLogMethod.getCaseById(Id);
                model.CategoryId = caseDetail.Category;
                model.StatusId = caseDetail.Status;
               // model.EmployeeId = caseDetail.EmployeeID;
                model.Summary = caseDetail.Summary;
                model.CreatedDate = String.Format("{0:dd-MM-yyy}", caseDetail.CreatedDate);
                var createdDetail = _employeeMethod.getEmployeeById(caseDetail.UserIDCreatedBy);
                model.CreatedName = string.Format("{0} {1}", createdDetail.FirstName, createdDetail.LastName);
                var caseComment = _adminCaseLogMethod.getCaseCommentByCaseId(Id);
                foreach (var item in caseComment)
                {
                    AdminCaseLogCommentViewModel commentModel = new AdminCaseLogCommentViewModel();
                    commentModel.Id = item.Id;
                    commentModel.comment = item.Description;
                    commentModel.commentBy = item.CreatedName;
                    commentModel.commentTime = item.CreatedDateTime;
                    model.CommentList.Add(commentModel);
                }
                var data = _db.AspNetUsers.Where(x => x.Id == caseDetail.EmployeeID).FirstOrDefault();
                if(data!=null)
                {
                    model.EmployeeName = data.FirstName + " " + data.LastName + "-" + data.SSOID;
                    model.EmployeeId = caseDetail.EmployeeID;
                }
                var caseDoument = _adminCaseLogMethod.getCaseDocumentByCaseId(Id);
                foreach (var item in caseDoument)
                {
                    AdminCaseLogDocumentViewModel docModel = new AdminCaseLogDocumentViewModel();
                    docModel.Id = item.Id;
                    docModel.originalName = item.OriginalName;
                    docModel.newName = item.NewName;
                    docModel.description = item.Description;
                    model.DocumentList.Add(docModel);
                }
            }
            return PartialView("_partialAddAdminCaseLog", model);
        }
        public ActionResult getEmployeeData()
        {
            int userId = SessionProxy.UserId;
            AdminCaseLogViewModel model = new AdminCaseLogViewModel();
            var data = _db.AspNetUserRoles.Where(x => x.UserId == userId && x.RoleId == 1).ToList();
            if (data.Count > 0 && data != null)
            {
                foreach (var item in _employeeMethod.GetAllResourceEmployeeList().Where(x => x.SSOID.StartsWith("W") && x.Archived == false).ToList())
                {
                    model.EmployeeList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
                }
            }
            else
            {
                foreach (var item in _employeeMethod.getReportToEmployee(SessionProxy.UserId).Where(x=>x.ReportToId==userId))
                {
                    model.EmployeeList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.ssoId, Value = item.EmployeeId.ToString() });
                }
            }            
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ImageData()
        {
            string FilePath = string.Empty;
            string fileName = string.Empty;
            string originalFileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                FilePath = ConfigurationManager.AppSettings["AdminCaseLog"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveData(AdminCaseLogViewModel model)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<AdminCaseLogCommentViewModel> listComment = js.Deserialize<List<AdminCaseLogCommentViewModel>>(model.CommentListString);
            List<AdminCaseLogDocumentViewModel> listDocument = js.Deserialize<List<AdminCaseLogDocumentViewModel>>(model.DocumentListString);

            _adminCaseLogMethod.SaveData(model.Id, model.StatusId, model.EmployeeId, model.CategoryId, model.Summary, listComment, listDocument, SessionProxy.UserId);

            List<AdminCaseLogViewModel> newModel = modelList();
            return PartialView("_partialAdminCaseLogList", newModel);
        }

        public ActionResult DeleteData(int Id)
        {
            var caseData = _db.Cases.Where(x => x.Id == Id).FirstOrDefault();
            caseData.Archived = true;
            caseData.LastModified = DateTime.Now;
            caseData.UserIDLastModifiedBy = SessionProxy.UserId;
            _db.SaveChanges();

            var CaseCommentList = _db.Cases_Comments.Where(x => x.CaseID == Id).ToList();
            foreach (var item in CaseCommentList)
            {
                item.Archived = true;
                item.LastModified = DateTime.Now;
                item.UserIDLastModifiedBy = SessionProxy.UserId;
                _db.SaveChanges();
            }

            var CaseDocumentList = _db.Cases_Documents.Where(x => x.CaseID == Id).ToList();
            foreach (var item in CaseDocumentList)
            {
                item.Archived = true;
                item.LastModified = DateTime.Now;
                item.UserIDLastModifiedBy = SessionProxy.UserId;
                _db.SaveChanges();
            }

            List<AdminCaseLogViewModel> newModel = modelList();
            return PartialView("_partialAdminCaseLogList", newModel);
        }
    }
}