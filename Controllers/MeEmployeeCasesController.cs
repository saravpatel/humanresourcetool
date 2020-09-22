using HRTool.CommanMethods.Admin;
using HRTool.CommanMethods.Resources;
using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models.Resources;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using HRTool.CommanMethods;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class MeEmployeeCasesController : Controller
    {

        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        EmployeeCasesMethod _EmployeeCasesMethod = new EmployeeCasesMethod();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        AdminCaseLogMethod _adminCaseLogMethod = new AdminCaseLogMethod();
        EmployeeMethod _employeeMethod = new EmployeeMethod();

        #endregion

        #region Methods
       
        public ActionResult Index(int EmployeeId)
        {
            CaseLogEmployeeViewModel model = new CaseLogEmployeeViewModel();
            var currentLoginEmployee = _employeeMethod.getEmployeeById(EmployeeId);
            model.EmployeeId = EmployeeId;
            model.UserName = currentLoginEmployee.UserName;
            model.Name = string.Format("{0} {1} - {2}", currentLoginEmployee.FirstName, currentLoginEmployee.LastName, currentLoginEmployee.SSOID);
            return View(model);
        }

        public ActionResult List(int EmployeeId)
        {
            List<CaseLogViewModel> model = modelList(EmployeeId);
            return PartialView("_partialCasesList", model);
        }

        public List<CaseLogViewModel> modelList(int EmployeeId)
        {
            List<CaseLogViewModel> model = new List<CaseLogViewModel>();
            var listData = _adminCaseLogMethod.getActiveList().Where(x => x.EmployeeID == EmployeeId).ToList();
            foreach (var item in listData)
            {
                CaseLogViewModel m = new CaseLogViewModel();
                m.Id = item.Id;
                var employeeDetail = _employeeMethod.getEmployeeById(item.EmployeeID);
                m.EmployeeName = string.Format("{0} {1} - {2}", employeeDetail.FirstName, employeeDetail.LastName, employeeDetail.SSOID);
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

        public ActionResult AddEditEmployeeCaseLog(int Id)
        {
            CaseLogViewModel model = new CaseLogViewModel();
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
            
            if (model.Id > 0)
            {
                var caseDetail = _adminCaseLogMethod.getCaseById(Id);
                model.CategoryId = caseDetail.Category;
                model.StatusId = caseDetail.Status;
                model.EmployeeId = caseDetail.EmployeeID;
                model.Summary = caseDetail.Summary;
                model.CreatedDate = String.Format("{0:dd-MM-yyy}", caseDetail.CreatedDate);
                var createdDetail = _employeeMethod.getEmployeeById(caseDetail.UserIDCreatedBy);
                model.CreatedName = string.Format("{0} {1}", createdDetail.FirstName, createdDetail.LastName);
                var caseComment = _adminCaseLogMethod.getCaseCommentByCaseId(Id);
                foreach (var item in caseComment)
                {
                    CaseLogCommentViewModel commentModel = new CaseLogCommentViewModel();
                    commentModel.Id = item.Id;
                    commentModel.comment = item.Description;
                    commentModel.commentBy = item.CreatedName;
                    commentModel.commentTime = item.CreatedDateTime;
                    model.CommentList.Add(commentModel);
                }

                var caseDoument = _adminCaseLogMethod.getCaseDocumentByCaseId(Id);
                foreach (var item in caseDoument)
                {
                    CaseLogDocumentViewModel docModel = new CaseLogDocumentViewModel();
                    docModel.Id = item.Id;
                    docModel.originalName = item.OriginalName;
                    docModel.newName = item.NewName;
                    docModel.description = item.Description;
                    model.DocumentList.Add(docModel);
                }
            }
            return PartialView("_partialAddEditCases", model);
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
        public ActionResult SaveData(CaseLogViewModel model)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<CaseLogCommentViewModel> listComment = js.Deserialize<List<CaseLogCommentViewModel>>(model.CommentListString);
            List<CaseLogDocumentViewModel> listDocument = js.Deserialize<List<CaseLogDocumentViewModel>>(model.DocumentListString);

            _adminCaseLogMethod.SaveEmployeeCaseData(model.Id, model.StatusId, model.EmployeeId, model.CategoryId, model.Summary, listComment, listDocument, SessionProxy.UserId);

            List<CaseLogViewModel> newModel = modelList(model.EmployeeId);
            return PartialView("_partialCasesList", newModel);
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

            List<CaseLogViewModel> newModel = modelList(caseData.EmployeeID);
            return PartialView("_partialCasesList", newModel);
        }
        #endregion
             
    }
}