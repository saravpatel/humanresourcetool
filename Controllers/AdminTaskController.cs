using HRTool.CommanMethods.Admin;
using HRTool.CommanMethods.Resources;
using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HRTool.Models.Admin;
using System.Configuration;
using System.IO;
using System.Web.Script.Serialization;
using HRTool.CommanMethods;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class AdminTaskController : Controller
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        EmployeeMethod _employeeMethod = new EmployeeMethod();
        AdminTaskMethod _adminTaskMethod = new AdminTaskMethod();

        #endregion
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TaskMenu()
        {
            int userId = !string.IsNullOrEmpty(User.Identity.GetUserId()) ? Convert.ToInt32(SessionProxy.UserId) : 0;
            var todayDate = DateTime.Now;
            AdminTaskMenuViewModel model = new AdminTaskMenuViewModel();
            var getAllActiveTask = _adminTaskMethod.getAllTask();

            var myTask = getAllActiveTask.Where(x => x.AssignTo == userId).ToList();
            //var expiredVisa = getAllActiveVisa.Where(x => x.DueDate.Value.Date < todayDate.Date).ToList();
            //var newTask = getAllActiveTask.Where(x => x.Date.Value.Date == todayDate.Date).ToList();
            var DueTodayTask = getAllActiveTask.Where(x => x.DueDate.Value.Date == todayDate.Date).ToList();
            //var upcomingVisa = getAllActiveTask.Where(x => x.Date.Value.Date > todayDate.Date).ToList();

            model.MyTask = myTask.Count();
            //model.OverDue = expiredVisa.Count();
            //model.New = newVisa.Count();
            model.DueToday = DueTodayTask.Count();

            //model.Upcoming = upcomingVisa.Count();
            model.AllTask = getAllActiveTask.Count();
            return PartialView("_partialAdminTaskMenu", model);
        }

        public ActionResult List(string search)
        {
            List<AdminTaskViewModel> model = modelLsit(search);
            return PartialView("_partialAdminTaskList", model);
        }

        public List<AdminTaskViewModel> modelLsit(string search)
        {
            List<AdminTaskViewModel> model = new List<AdminTaskViewModel>();
            var listData = _adminTaskMethod.getAllTask().OrderByDescending(x => x.Id).ToList();
            var userId = !string.IsNullOrEmpty(User.Identity.GetUserId()) ? Convert.ToInt32(SessionProxy.UserId) : 0;
            var todayDate = DateTime.Now;
            if (!string.IsNullOrEmpty(search))
            {
                if (search == "MyVisa")
                {
                    listData = listData.Where(x => x.AssignTo == userId).ToList();
                }
                if (search == "Expired")
                {
                    listData = listData.Where(x => x.DueDate.Value.Date < todayDate.Date).ToList();
                }
                if (search == "New")
                {
                    listData = listData.Where(x => x.Created.Value.Date == todayDate.Date).ToList();
                }
                if (search == "ExpiringToday")
                {
                    listData = listData.Where(x => x.DueDate.Value.Date == todayDate.Date).ToList();
                }
                if (search == "Upcoming")
                {
                    listData = listData.Where(x => x.Created.Value.Date > todayDate.Date).ToList();
                }

            }

            foreach (var item in listData)
            {
                AdminTaskViewModel m = new AdminTaskViewModel();
                m.Id = item.Id;
                m.Title = item.Title;

                var employeeDetail = _employeeMethod.getEmployeeById(item.AssignTo);
                if (employeeDetail != null)
                {
                    m.AssignTo = string.Format("{0} {1} - {2}", employeeDetail.FirstName, employeeDetail.LastName, employeeDetail.SSOID);
                }

                var relation = _employeeMethod.getEmployeeById(item.InRelationTo);
                if (relation != null)
                {
                    m.InRelationTo = string.Format("{0} {1} - {2}", relation.FirstName, relation.LastName, relation.SSOID);
                }
                if (item.Category != null)
                {
                    var category = _otherSettingMethod.getSystemListValueById((int)item.Category);
                    m.CategoryName = category.Value;
                }
                m.DueDate = String.Format("{0:dd-MMM-yyy}", item.DueDate);
                m.AlertBeforeDays = (int)item.AlterBeforeDays;
                var status = _otherSettingMethod.getSystemListValueById((int)item.Status);
                if (status != null)
                {
                    m.Status = status.Value;
                }

                model.Add(m);
            }
            return model;
        }

        public ActionResult AddEditTask(int Id)
        {
            AdminTaskViewModel model = new AdminTaskViewModel();
            model.Id = Id;

            model.CategoryList.Add(new SelectListItem() { Text = "-- Select Category --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Task Categories"))
            {
                model.CategoryList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }

            var employeeList = _employeeMethod.GetAllEmployeeList();
            model.AssignToList.Add(new SelectListItem() { Text = "-- Select Assign To --", Value = "0" });
            foreach (var item in employeeList)
            {
                model.AssignToList.Add(new SelectListItem() { Text = string.Format("{0} {1} - {2}", item.FirstName, item.LastName, item.SSOID), Value = item.Id.ToString() });
            }

            model.InRelationToList.Add(new SelectListItem() { Text = "-- Select In Relation To --", Value = "0" });
            foreach (var item in employeeList)
            {
                model.InRelationToList.Add(new SelectListItem() { Text = string.Format("{0} {1} - {2}", item.FirstName, item.LastName, item.SSOID), Value = item.Id.ToString() });
            }

            model.StatusList.Add(new SelectListItem() { Text = "-- Select Status --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Task Status"))
            {
                model.StatusList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }

            if (model.Id > 0)
            {
                var taskDetail = _adminTaskMethod.getTaskById(Id);
                model.Title = taskDetail.Title;
                if (taskDetail.Category.HasValue==true)
                {
                    model.CategoryId = (int)taskDetail.Category;
                }
                model.AssignToId = taskDetail.AssignTo;
                model.InRelationToId = taskDetail.InRelationTo;
                model.DueDate = String.Format("{0:dd-MM-yyy}", taskDetail.DueDate);
                model.StatusId = (int)taskDetail.Status;
                model.AlertBeforeDays = (int)taskDetail.AlterBeforeDays;
                model.Description = taskDetail.Description;

                var taskDoument = _adminTaskMethod.getTaskDocumentsByVisaId(Id);
                foreach (var item in taskDoument)
                {
                    TaskDocumentViewModel docModel = new TaskDocumentViewModel();
                    docModel.Id = item.Id;
                    docModel.originalName = item.OriginalName;
                    docModel.newName = item.NewName;
                    docModel.description = item.Description;
                    model.DocumentList.Add(docModel);
                }
            }

            return PartialView("_partialAddAdminTask", model);
        }

        public ActionResult GetEmployeeName_Task()
        {
            var employeeList = _employeeMethod.GetAllEmployeeList();
            AdminTaskViewModel model = new AdminTaskViewModel();
            model.AssignToList.Add(new SelectListItem() { Text = "-- Select Assign To --", Value = "0" });
            foreach (var item in employeeList)
            {
                model.AssignToList.Add(new SelectListItem() { Text = string.Format("{0} {1} - {2}", item.FirstName, item.LastName, item.SSOID), Value = item.Id.ToString() });
            }

            model.InRelationToList.Add(new SelectListItem() { Text = "-- Select In Relation To --", Value = "0" });
            foreach (var item in employeeList)
            {
                model.InRelationToList.Add(new SelectListItem() { Text = string.Format("{0} {1} - {2}", item.FirstName, item.LastName, item.SSOID), Value = item.Id.ToString() });
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
                FilePath = ConfigurationManager.AppSettings["AdminTask"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
        }

        public ActionResult SaveData(AdminTaskViewModel model)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<TaskDocumentViewModel> listDocument = js.Deserialize<List<TaskDocumentViewModel>>(model.jsonDocumentListString);

            _adminTaskMethod.SaveData(model, listDocument, SessionProxy.UserId);

            List<AdminTaskViewModel> returnModel = modelLsit(model.FilterSearch);
            return PartialView("_partialAdminTaskList", returnModel);
        }

        public ActionResult DeleteData(int Id, string search)
        {
            var taskData = _db.Task_List.Where(x => x.Id == Id).FirstOrDefault();
            taskData.Archived = true;
            taskData.LastModified = DateTime.Now;
            taskData.LastModifiedBy = !string.IsNullOrEmpty(User.Identity.GetUserId()) ? Convert.ToInt32(SessionProxy.UserId) : 0;
            _db.SaveChanges();

            var taskDocumentList = _db.Task_Documents.Where(x => x.TaskId == Id).ToList();
            foreach (var item in taskDocumentList)
            {
                item.Archived = true;
                item.LastModified = DateTime.Now;
                item.UserIDLastModifiedBy = !string.IsNullOrEmpty(User.Identity.GetUserId()) ? Convert.ToInt32(SessionProxy.UserId) : 0;
                _db.SaveChanges();
            }

            List<AdminTaskViewModel> returnModel = modelLsit(search);
            return PartialView("_partialAdminTaskList", returnModel);
        }
    }
}