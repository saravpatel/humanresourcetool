using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HRTool.Models.RolesManagement;
using HRTool.DataModel;
using HRTool.Models;
using HRTool.CommanMethods.RolesManagement;
using HRTool.Models.Settings;
using HRTool.CommanMethods;

namespace HRTool.Controllers
{

    [CustomAuthorize]
    public class HomeController : Controller
    {
        #region const
        EvolutionEntities _db = new EvolutionEntities();

        #endregion

        [HttpGet]
        public ActionResult Index()
        {
            //int userId = SessionProxy.UserId;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        //[HttpGet]
        [ChildActionOnly]
        public PartialViewResult MenuList()
        {

            DynamicMenuViewModel model = new DynamicMenuViewModel();
            int userId = CommanMethods.SessionProxy.UserId;
            int userRoles = _db.AspNetRoles.Where(x => x.AspNetUserRoles.Any(xx => xx.UserId == userId)).FirstOrDefault().Id;
            //var AspRole = _db.AspNetUserRoles.Where(x => x.UserId == userId).FirstOrDefault();
            //var userRoles = _db.AspNetRoles.Where(xx => xx.Id == AspRole.RoleId).FirstOrDefault().Name;

            //var username = User.Identity.GetUserName();
            //var account = new AccountController();
            //var userRoles = account.UserManager.GetRoles(User.Identity.GetUserId()).FirstOrDefault();
            List<DynamicMenuViewModel> menuRoleList = new List<DynamicMenuViewModel>();
            List<Menu_List> mainMenu = new List<Menu_List>();
            if (userRoles == 1)
            {
                model.Dashboard = true;
                model.News = true;
                model.Resources = true;
                model.Me = true;
                model.Document = true;
                model.Planner = true;
                model.ProjectPlanner = true;
                model.Performance = true;
                model.SkillsEndorsement = true;
                model.Training = true;
                model.Certification = true;
                model.Visa = true;
                model.Tasks = true;
                model.Cases = true;
                model.TMS = true;
                model.Settings = true;
                model.OrganizationChart = true;
                model.Reports = true;
                model.BulkActions = true;
                model.Queries = true;
                model.Notifications = true;
                model.Approve = true;
            }
            else
            {
                var menuShow = (from record in _db.Role_DefaultMenu where record.RoleId == userRoles select record).ToList();
                //var userMenus = _db.UserMenus.Where(x => x.UserID == userId).ToList();
                model.Dashboard = false;
                model.News = false;
                model.Resources = false;
                model.Me = false;
                model.Document = false;
                model.Planner = false;
                model.ProjectPlanner = false;
                model.Performance = false;
                model.SkillsEndorsement = false;
                model.Training = false;
                model.Certification = false;
                model.Visa = false;
                model.Tasks = false;
                model.Cases = false;
                model.TMS = false;
                model.Settings = false;
                model.OrganizationChart = false;
                model.Reports = false;
                model.BulkActions = false;
                model.Queries = false;
                model.Notifications = false;
                model.Approve = false;
                foreach (var item in menuShow)
                {
                    if (item.MenuKey == (int)Menukey.Dashboard)
                    {
                        model.Dashboard = true;
                    }
                    if (item.MenuKey == (int)Menukey.News)
                    {
                        model.News = true;
                    }
                    if (item.MenuKey == (int)Menukey.Resources)
                    {
                        model.Resources = true;
                    }
                    if (item.MenuKey == (int)Menukey.Me)
                    {
                        model.Me = true;
                    }
                    if (item.MenuKey == (int)Menukey.Document)
                    {
                        model.Document = true;
                    }
                    if (item.MenuKey == (int)Menukey.Planner)
                    {
                        model.Planner = true;
                    }
                    if (item.MenuKey == (int)Menukey.ProjectPlanner)
                    {
                        model.ProjectPlanner = true;
                    }
                    if (item.MenuKey == (int)Menukey.Performance)
                    {
                        model.Performance = true;
                    }
                    if (item.MenuKey == (int)Menukey.SkillsEndorsement)
                    {
                        model.SkillsEndorsement = true;
                    }
                    if (item.MenuKey == (int)Menukey.Training)
                    {
                        model.Training = true;
                    }
                    if (item.MenuKey == (int)Menukey.Certification)
                    {
                        model.Certification = true;
                    }
                    if (item.MenuKey == (int)Menukey.Visa)
                    {
                        model.Visa = true;
                    }
                    if (item.MenuKey == (int)Menukey.Tasks)
                    {
                        model.Tasks = true;
                    }
                    if (item.MenuKey == (int)Menukey.Cases)
                    {
                        model.Cases = true;
                    }
                    if (item.MenuKey == (int)Menukey.Approve)
                    {
                        model.Approve = true;
                    }
                    if (item.MenuKey == (int)Menukey.TMS)
                    {
                        model.TMS = true;
                    }
                    if (item.MenuKey == (int)Menukey.Notifications)
                    {
                        model.Notifications = true;
                    }
                    if (item.MenuKey == (int)Menukey.Queries)
                    {
                        model.Queries = true;
                    }
                    if (item.MenuKey == (int)Menukey.BulkActions)
                    {
                        model.BulkActions = true;
                    }
                    if (item.MenuKey == (int)Menukey.Reports)
                    {
                        model.Reports = true;
                    }
                    if (item.MenuKey == (int)Menukey.OrganizationChart)
                    {
                        model.OrganizationChart = true;
                    }
                    if (item.MenuKey == (int)Menukey.Settings)
                    {
                        model.Settings = true;
                    }
                }
            }
            return PartialView("_PartialMenuList", model);
        }

        #region Setting menus

        public ActionResult SettingMenuList()
        {
            SettingMenuViewModel model = new SettingMenuViewModel();
            //int userId = SessionProxy.UserId;
            //var username = User.Identity.GetUserName();
            //var account = new AccountController();
            //var userRoles = account.UserManager.GetRoles(User.Identity.GetUserId()).FirstOrDefault();

            int userId = SessionProxy.UserId;
            int userRoles = _db.AspNetRoles.Where(x => x.AspNetUserRoles.Any(xx => xx.UserId == userId)).FirstOrDefault().Id;
            //string userRoles = _db.AspNetRoles.Where(x => x.AspNetUserRoles.Any(xx => xx.UserId == userId)).FirstOrDefault().Name;
            //_db.AspNetUserRoles.Where(xx=>xx.UserId == userId).FirstOrDefault().Id
            //_db.AspNetRoles.Where(x => x.AspNetUserRoles.Where(xx => xx.UserId == userId).Count() > 0).FirstOrDefault().Name;
            //var AspRole = _db.AspNetUserRoles.Where(x => x.UserId == userId).FirstOrDefault();
            //var userRoles = _db.AspNetRoles.Where(xx => xx.Id == AspRole.RoleId).FirstOrDefault().Name;
            if (userRoles == 1)
            {
                model.Settings_TMS = true;
                model.Settings_Projects = true;
                model.Settings_Performance = true;
                model.Settings_TechnicalSkillsSet = true;
                model.Settings_GeneralSkillsSet = true;
                model.Settings_AddSkills = true;
                model.Settings_ActivityType = true;
                model.Settings_AddCmpCustomer = true;
                model.Settings_AddAssets = true;
                model.Settings_Timesheet = true;
                model.Settings_HolidaysAbsence = true;
                model.Settings_Currency = true;
                model.Settings_Administrators = true;
                model.Settings_EmailSettings = true;
                model.Settings_API = true;
                model.Settings_Company = true;
                model.Settings_CompanyStructure = true;
                model.Settings_License = true;
                model.Settings_Mobile = true;
                model.Settings_CreateRole = true;
                model.Settings_AssignRole = true;
                model.Settings_RoleManagement = true;
                model.Settings_OtherSettings = true;
                model.Settings_SystemSettings = true;
            }
            else
            {
                var userMenus = (from record in _db.Role_DefaultMenu where record.RoleId == userRoles select record).ToList();
                model.Settings_TMS = false;
                model.Settings_Projects = false;
                model.Settings_Performance = false;
                model.Settings_TechnicalSkillsSet = false;
                model.Settings_GeneralSkillsSet = false;
                model.Settings_AddSkills = false;
                model.Settings_ActivityType = false;
                model.Settings_AddCmpCustomer = false;
                model.Settings_AddAssets = false;
                model.Settings_Timesheet = false;
                model.Settings_HolidaysAbsence = false;
                model.Settings_Currency = false;
                model.Settings_Administrators = false;
                model.Settings_EmailSettings = false;
                model.Settings_API = false;
                model.Settings_Company = false;
                model.Settings_CompanyStructure = false;
                model.Settings_License = false;
                model.Settings_Mobile = false;
                model.Settings_CreateRole = false;
                model.Settings_AssignRole = false;
                model.Settings_RoleManagement = false;
                model.Settings_OtherSettings = false;
                model.Settings_SystemSettings = false;
                foreach (var item in userMenus)
                {
                    if (item.MenuKey == (int)Menukey.Settings_TMS)
                        model.Settings_TMS = true;
                    if (item.MenuKey == (int)Menukey.Settings_Projects)
                        model.Settings_Projects = true;
                    if (item.MenuKey == (int)Menukey.Settings_Performance)
                        model.Settings_Performance = true;
                    if (item.MenuKey == (int)Menukey.Settings_TechnicalSkillsSet)
                        model.Settings_TechnicalSkillsSet = true;
                    if (item.MenuKey == (int)Menukey.Settings_GeneralSkillsSet)
                        model.Settings_GeneralSkillsSet = true;
                    if (item.MenuKey == (int)Menukey.Settings_AddSkills)
                        model.Settings_AddSkills = true;
                    if (item.MenuKey == (int)Menukey.Settings_ActivityType)
                        model.Settings_ActivityType = true;
                    if (item.MenuKey == (int)Menukey.Settings_AddCmpCustomer)
                        model.Settings_AddCmpCustomer = true;
                    if (item.MenuKey == (int)Menukey.Settings_AddAssets)
                        model.Settings_AddAssets = true;
                    if (item.MenuKey == (int)Menukey.Settings_Timesheet)
                        model.Settings_Timesheet = true;
                    if (item.MenuKey == (int)Menukey.Settings_HolidaysAbsence)
                        model.Settings_HolidaysAbsence = true;
                    if (item.MenuKey == (int)Menukey.Settings_Currency)
                        model.Settings_Currency = true;
                    if (item.MenuKey == (int)Menukey.Settings_Administrators)
                        model.Settings_Administrators = true;
                    if (item.MenuKey == (int)Menukey.Settings_EmailSettings)
                        model.Settings_EmailSettings = true;
                    if (item.MenuKey == (int)Menukey.Settings_API)
                        model.Settings_API = true;
                    if (item.MenuKey == (int)Menukey.Settings_Company)
                        model.Settings_Company = true;
                    if (item.MenuKey == (int)Menukey.Settings_CompanyStructure)
                        model.Settings_CompanyStructure = true;
                    if (item.MenuKey == (int)Menukey.Settings_License)
                        model.Settings_License = true;
                    if (item.MenuKey == (int)Menukey.Settings_Mobile)
                        model.Settings_Mobile = true;
                    if (item.MenuKey == (int)Menukey.Settings_CreateRole)
                        model.Settings_CreateRole = true;
                    if (item.MenuKey == (int)Menukey.Settings_AssignRole)
                        model.Settings_AssignRole = true;
                    if (item.MenuKey == (int)Menukey.Settings_RoleManagement)
                        model.Settings_RoleManagement = true;
                    if (item.MenuKey == (int)Menukey.Settings_OtherSettings)
                        model.Settings_OtherSettings = true;
                    if (item.MenuKey == (int)Menukey.Settings_SystemSettings)
                        model.Settings_SystemSettings = true;
                }
            }
            return PartialView("_PartialSettingMenuList", model);
        }
        #endregion
    }
}