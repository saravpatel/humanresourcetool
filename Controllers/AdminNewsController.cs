using HRTool.CommanMethods.Admin;
using HRTool.DataModel;
using HRTool.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HRTool.CommanMethods.Resources;
using HRTool.CommanMethods.RolesManagement;
using System.Text.RegularExpressions;
using HRTool.CommanMethods;
using static HRTool.CommanMethods.Enums;
using System.Globalization;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class AdminNewsController : Controller
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        AdminNewsMethod _adminNewsMethod = new AdminNewsMethod();
        EmployeeMethod _employeeMethod = new EmployeeMethod();
        RolesManagementMethod _RolesManagementMethod = new RolesManagementMethod();

        #endregion
        //
        // GET: /AdminNews/
        public ActionResult Index()
        {
            return View();
        }
        public List<AdminNewsViewModel> modelList()
        {
            List<AdminNewsViewModel> model = new List<AdminNewsViewModel>();
            int CurrentUser = SessionProxy.UserId;
            int SuperAdmin = _RolesManagementMethod.GetSuperAdminId();
            var details = _RolesManagementMethod.GetLoginUserRoleType(CurrentUser);
            bool Super = false;
            if (Convert.ToInt32(CurrentUser) == SuperAdmin)
            {
                Super = true;
            }
            var data = _adminNewsMethod.getActiveList();
            foreach (var item in data)
            {
                AdminNewsViewModel m = new AdminNewsViewModel();
                m.Super = Super;
                m.Id = item.Id;
                var detail = _db.AspNetUsers.Where(x => x.Id == item.CreatedBy).FirstOrDefault();
                if (detail.SSOID.Contains('W'))
                {
                    
                        m.Emp = true;
                    
                }
                if (detail.SSOID.Contains('C'))
                {
                    
                        m.Cus = true;
                    
                }
                if (_RolesManagementMethod.GetLoginUserRoleType(detail.Id) == "Manager")
                {
                    m.Man = true;
                }
                m.CurrentImage = detail.image;
                m.Subject = item.Subject;
                m.Description = item.Description;
                m.EmployeeAccess = item.EmployeeAccess;
                m.ManagerAccess = item.ManagerAccess;
                m.CustomerAccess = item.CustomerAccess;
                m.SpecificWorker = item.SpecificWorker;
                m.WorkerID = item.WorkerID;
                m.SpecificCustomer = item.SpecificCustomer;
                m.CustomerID = item.CustomerID;
                m.SpecificManager = item.SpecificManager;
                m.ManagerID = item.ManagerID;
                m.NotifyEmployeeViaEmail = item.NotifyEmployeeViaEmail;
                m.AllowCustomer = item.AllowCustomer;
                var employee = _employeeMethod.getEmployeeById(item.CreatedBy);
                m.CreateUserId = item.CreatedBy;
                m.CreatedBy = string.Format("{0} {1} - {2}", employee.FirstName, employee.LastName, employee.SSOID);
                m.CreatedDate = String.Format("{0:dd-MMM-yyy}", item.CreatedDate);
                m.CommentCount = _adminNewsMethod.GetCommentCount(item.Id);
                var listComment = _adminNewsMethod.GetCommentList(item.Id);
                foreach (var itemComment in listComment)
                {
                    NewCommentViewModel modelComment = new NewCommentViewModel();
                    if (itemComment.Comments != null)
                    {
                        //string noHTML = Regex.Replace(itemComment.Comments, @"<[^>]+>| ", "").Trim();
                        modelComment.Comments = itemComment.Comments;
                    }
                    modelComment.Id = itemComment.Id;
                    modelComment.UserCreate = itemComment.UserIDCreatedBy;
                    modelComment.SuperAdmin = SuperAdmin;
                    var diff = DateTimeSpan.CompareDates((DateTime)itemComment.CreatedDate, DateTime.Now);
                    if (diff.Years != 0)
                    {
                        modelComment.Time = diff.Years + " " + "a year ago";
                    }
                    else
                    {
                        if (diff.Months != 0)
                        {
                            modelComment.Time = diff.Months + " " + "Months ago";
                        }
                        else
                        {
                            if (diff.Days != 0)
                            {
                                modelComment.Time = diff.Days + " " + "Days ago";
                            }
                            else
                            {
                                if (diff.Hours != 0)
                                {
                                    modelComment.Time = diff.Hours + " " + "Hours ago";
                                }
                                else
                                {
                                    if (diff.Minutes > 1)
                                    {
                                        modelComment.Time = diff.Minutes + " " + "Minutes ago";
                                    }
                                    else
                                    {
                                        modelComment.Time = diff.Minutes + " " + "Few Minutes ago";

                                    }
                                }
                            }
                        }

                    }
                    m.NewCommantsList.Add(modelComment);
                }
                model.Add(m);
            }

            return model.OrderByDescending(x => x.CreatedDate).ToList();
        }
        public ActionResult List()
        {
            List<AdminNewsViewModel> model = modelList();
            NewsRole newsModel = new NewsRole();
            bool AddEdit = false;
            newsModel.UserId = SessionProxy.UserId;
            var SuperAdmin = _RolesManagementMethod.GetSuperAdminId();
            if (newsModel.UserId == SuperAdmin)
            {
                AddEdit = true;
            }
            else
            {
                var data = _db.UserMenus.Where(x => x.UserID == newsModel.UserId).ToList();
                if (data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        if (item.MenuKey == 74)
                        {
                            AddEdit = true;
                        }

                    }
                }
            }
            newsModel.AddEdit = AddEdit;
            return PartialView("_partialAdminNewsList", new Tuple<NewsRole, List<AdminNewsViewModel>>(newsModel, model));
        }

        public ActionResult searchByFromToDate(string fromDate,string Todate)
        {
            string inputFormat = "dd-MM-yyyy";
            List<AdminNewsViewModel> model = modelList().ToList();
            if (fromDate!="" && Todate!="")
            {
                DateTime FromSeDate = DateTime.ParseExact(fromDate, inputFormat, CultureInfo.InvariantCulture);
                DateTime ToSerdate = DateTime.ParseExact(Todate, inputFormat, CultureInfo.InvariantCulture);
                model = modelList().Where(x => Convert.ToDateTime(x.CreatedDate) >= FromSeDate && Convert.ToDateTime(x.CreatedDate) <= ToSerdate).ToList();
            }
            NewsRole newsModel = new NewsRole();
            bool AddEdit = false;
            newsModel.UserId = SessionProxy.UserId;
            var SuperAdmin = _RolesManagementMethod.GetSuperAdminId();
            if (newsModel.UserId == SuperAdmin)
            {
                AddEdit = true;
            }
            else
            {
                var data = _db.UserMenus.Where(x => x.UserID == newsModel.UserId).ToList();
                if (data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        if (item.MenuKey == 74)
                        {
                            AddEdit = true;
                        }

                    }
                }
            }
            newsModel.AddEdit = AddEdit;
            return PartialView("_partialAdminNewsList", new Tuple<NewsRole, List<AdminNewsViewModel>>(newsModel, model));
        }
        public ActionResult AddEditAdminNews(int Id)
        {
            AdminNewsViewModel model = new AdminNewsViewModel();
            int CurrentUser = SessionProxy.UserId;
            var SuperAdmin = _RolesManagementMethod.GetSuperAdminId();
            foreach (var item in _employeeMethod.GetAllResourceEmployeeList().Where(x => x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList())
            {
                if (_RolesManagementMethod.GetLoginUserRoleType(item.Id) != "Manager")
                {

                    model.WorkerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
                }
            }
            foreach (var item in _RolesManagementMethod.GetManagersList())
            {
                model.ManagerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }
            foreach (var item in _employeeMethod.GetAllCoustomerEmployeeList().Where(x => x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList())
            {
                if (_RolesManagementMethod.GetLoginUserRoleType(item.Id) != "Manager")
                {
                    model.CustomerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
                }
            }
            if (Id > 0)
            {
                var record = _adminNewsMethod.GetNewsRecordById(Id);
                if (record.CreatedBy == CurrentUser || CurrentUser == SuperAdmin)
                {
                    model.Id = record.Id;
                    model.Subject = record.Subject;
                    model.Description = record.Description;
                    model.EmployeeAccess = record.EmployeeAccess;
                    model.ManagerAccess = record.ManagerAccess;
                    model.CustomerAccess = record.CustomerAccess;
                    model.CustomerID = record.CustomerID;
                    model.ManagerID = record.ManagerID;
                    model.WorkerID = record.WorkerID;
                    model.SpecificCustomer = record.SpecificCustomer;
                    model.SpecificManager = record.SpecificManager;
                    model.SpecificWorker = record.SpecificWorker;
                    model.NotifyEmployeeViaEmail = record.NotifyEmployeeViaEmail;
                    model.AllowCustomer = record.AllowCustomer;
                    if (record.WorkerID != 0 && record.WorkerID != null)
                    {
                        int CmpId = Convert.ToInt32(record.WorkerID);
                        if (CmpId != 0)
                        {
                            var Employee = _db.AspNetUsers.Where(x => x.Id == CmpId && x.Archived == false).FirstOrDefault();
                            if (Employee != null)
                            {
                                model.SpecificWorkerName = Employee.FirstName + " " + Employee.LastName + "-" + Employee.SSOID;
                                model.WorkerID = record.WorkerID;
                            }
                        }
                    }
                    if (record.ManagerID != 0 && record.ManagerID != null)
                    {
                        int ManId = Convert.ToInt32(record.ManagerID);
                        if (ManId != 0)
                        {
                            var Employee = _db.AspNetUsers.Where(x => x.Id == ManId && x.Archived == false).FirstOrDefault();
                            if (Employee != null)
                            {
                                model.SpecificManagerName = Employee.FirstName + " " + Employee.LastName + " " + Employee.SSOID;
                                model.ManagerID = record.ManagerID;
                            }
                        }
                    }
                    if (record.CustomerID != 0 && record.CustomerID != null)
                    {
                        int CustId = Convert.ToInt32(record.CustomerID);
                        if (CustId != 0)
                        {
                            var Employee = _db.AspNetUsers.Where(x => x.Id == CustId && x.Archived == false).FirstOrDefault();
                            if (Employee != null)
                            {
                                model.SpecificCustomerName = Employee.FirstName + " " + Employee.LastName + " " + Employee.SSOID;
                                model.CustomerID = record.CustomerID;
                            }
                        }
                    }



                    return PartialView("_partialAddAdminNews", model);
                    
                }
                else
                {
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return PartialView("_partialAddAdminNews", model);
            }

            //foreach (var item in _RolesManagementMethod.GetEmployeesList())
            //{
            //    model.WorkerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            //}
            //foreach (var item in _RolesManagementMethod.GetManagersList())
            //{
            //    model.ManagerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            //}
            //foreach (var item in _RolesManagementMethod.GetCustomerList())
            //{
            //    model.CustomerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            //}


        }

        public ActionResult getWorker_CustomerList()
        {
            AdminNewsViewModel model = new AdminNewsViewModel();
            foreach (var item in _employeeMethod.GetAllResourceEmployeeList().Where(x => x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList())
            {
                if (_RolesManagementMethod.GetLoginUserRoleType(item.Id) != "Manager")
                {

                    model.WorkerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
                }
            }
            foreach (var item in _RolesManagementMethod.GetManagersList())
            {
                model.ManagerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }
            foreach (var item in _employeeMethod.GetAllCoustomerEmployeeList().Where(x => x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList())
            {
                if (_RolesManagementMethod.GetLoginUserRoleType(item.Id) != "Manager")
                {
                    model.CustomerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveData(AdminNewsViewModel model)
        {
            _adminNewsMethod.saveData(model, SessionProxy.UserId);
            //List<AdminNewsViewModel> returnModel = modelList();
            return List(); //PartialView("_partialAdminNewsList", returnModel);
        }
        public ActionResult AddComments(int Id)
        {
            AdminNewsViewModel model = new AdminNewsViewModel();
            model.NewsId = Id;
            return PartialView("_PartialAddNewsCommentViewSet", model);
        }
        public ActionResult SaveCommentsRecvords(AdminNewsViewModel model)
        {
            model.CurrentUserId = SessionProxy.UserId;
            _adminNewsMethod.SaveCommentRecords(model);
            return List();
            //List<AdminNewsViewModel> modellist = modelList();
            //return PartialView("_partialAdminNewsList", modellist);
        }
        public ActionResult EditCommentsRecord(int Id)
        {
            int Users = SessionProxy.UserId;
            AdminNewsViewModel model = new AdminNewsViewModel();
            model.Id = Id;
            var record = _adminNewsMethod.Editcomments(Id);
            foreach (var item in record)
            {
                model.Id = item.Id;
                model.Comments = item.Comments;
                model.NewsId = (int)item.NewsId;
            }
            return PartialView("_PartialAddNewsCommentViewSet", model);
        }
        public ActionResult DeleteComment(int Id)
        {
            int Users = SessionProxy.UserId;
            var SuperAdmin = _RolesManagementMethod.GetSuperAdminId();
            var data = _adminNewsMethod.GetNewsCommentRecordById(Id);
            if (data.UserIDCreatedBy == Users || SuperAdmin == Users)
            {
                _adminNewsMethod.Deletecommentrecord(Id, Users);
                //List<AdminNewsViewModel> modellist = modelList();
                //return PartialView("_partialAdminNewsList", modellist);
                return List();
            }
            else 
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeleteData(int Id, int LoginUserId)
        {
            var SuperAdmin = _RolesManagementMethod.GetSuperAdminId();
            var data = _adminNewsMethod.GetNewsRecordById(Id);
            if (data.CreatedBy == LoginUserId || SuperAdmin == LoginUserId)
            {
                _adminNewsMethod.DeleteNewsrecord(Id, LoginUserId);
                //List<AdminNewsViewModel> modellist = modelList();
                //return PartialView("_partialAdminNewsList", modellist);
                return List();
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

    }
}