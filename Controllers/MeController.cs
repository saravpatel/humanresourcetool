using HRTool.DataModel;
using HRTool.Models.Me;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HRTool.CommanMethods.RolesManagement;
using HRTool.CommanMethods.Resources;
using HRTool.CommanMethods.Settings;
using HRTool.CommanMethods;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class MeController : Controller
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        EmployeeMethod _employeeMethod = new EmployeeMethod();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();

        #endregion
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult BindMenuIndex()
        {
            MeMenuModel model = new MeMenuModel();
            int userId = SessionProxy.UserId;
            var employeeData = _employeeMethod.getEmployeeById(userId);
            model.Id = employeeData.Id;
            model.EmployeeName = employeeData.FirstName +" "+ employeeData.LastName;
            model.EmployeeImage = employeeData.image;
            if (employeeData.JobTitle != null)
            {
                var jobtitle = _otherSettingMethod.getSystemListValueById((int)employeeData.JobTitle);
                if(jobtitle != null)
                model.Jobtilte = jobtitle.Value;
            }
            //var account = new AccountController();
            //var userRoles = account.UserManager.GetRoles(User.Identity.GetUserId()).FirstOrDefault();
            int userRoles = _db.AspNetRoles.Where(x => x.AspNetUserRoles.Any(xx => xx.UserId == userId)).FirstOrDefault().Id;
            if (userRoles == 1)
            {
                model.Me_OverView = true;
                model.Me_Planner = true;
                model.Me_ProjectPlanner = true;
                model.Me_Performance = true;
                model.Me_SkillsEndorsement = true;
                //model.Me_Skills = true;
                model.Me_Training = true;
                model.Me_Documents = true;
                model.Me_Resume_CV = true;
                model.Me_Profile = true;
                model.Me_Employment = true;
                model.Me_Contacts = true;
               // model.Me_Benefits = true;
                model.Me_Caselog = true;
            }
            else
            {
                var userMenus = (from record in _db.Role_DefaultMenu where record.RoleId == userRoles select record).ToList();
                model.Me_OverView = false;
                model.Me_Planner = false;
                model.Me_ProjectPlanner = false;
                model.Me_Performance = false;
                model.Me_SkillsEndorsement = false;
               // model.Me_Skills = false;
                model.Me_Training = false;
                model.Me_Documents = false;
                model.Me_Resume_CV = false;
                model.Me_Profile = false;
                model.Me_Employment = false;
                model.Me_Contacts = false;
               // model.Me_Benefits = false;
                model.Me_Caselog = false;
                foreach (var item in userMenus)
                {
                    if (item.MenuKey == (int)Menukey.Me_OverView)
                    {
                        model.Me_OverView = true;
                    }
                    if (item.MenuKey == (int)Menukey.Me_Planner)
                    {
                        model.Me_Planner = true;
                    }
                    if (item.MenuKey == (int)Menukey.Me_ProjectPlanner)
                    {
                        model.Me_ProjectPlanner = true;
                    }
                    if (item.MenuKey == (int)Menukey.Me_Performance)
                    {
                        model.Me_Performance = true;
                    }
                    if (item.MenuKey == (int)Menukey.Me_SkillsEndorsement)
                    {
                        model.Me_SkillsEndorsement = true;
                    }
                    if (item.MenuKey == (int)Menukey.Me_Skills)
                    {
                        model.Me_Skills = true;
                    }
                    if (item.MenuKey == (int)Menukey.Me_Training)
                    {
                        model.Me_Training = true;
                    }
                    if (item.MenuKey == (int)Menukey.Me_Resume_CV)
                    {
                        model.Me_Resume_CV = true;
                    }
                    if (item.MenuKey == (int)Menukey.Me_Profile)
                    {
                        model.Me_Profile = true;
                    }
                    if (item.MenuKey == (int)Menukey.Me_Documents)
                    {
                        model.Me_Documents = true;
                    }
                    if (item.MenuKey == (int)Menukey.Me_Employment)
                    {
                        model.Me_Employment = true;
                    }
                    if (item.MenuKey == (int)Menukey.Me_Contacts)
                    {
                        model.Me_Contacts = true;
                    }
                    if (item.MenuKey == (int)Menukey.Me_Benefits)
                    {
                        model.Me_Benefits = true;
                    }
                    if (item.MenuKey == (int)Menukey.Me_Caselog)
                    {
                        model.Me_Caselog = true;
                    }
                }
            }
            return PartialView("_partialMeMenu", model);
        }
    }
}