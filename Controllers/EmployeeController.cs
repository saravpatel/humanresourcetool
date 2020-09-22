using HRTool.CommanMethods.Resources;
using HRTool.DataModel;
using HRTool.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HRTool.CommanMethods.RolesManagement;
using HRTool.Models.Resources;
using System.Configuration;
using HRTool.CommanMethods.Settings;
using HRTool.CommanMethods.Admin;
using HRTool.Models;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Owin.Security;
using System.Web.Configuration;
using ClosedXML.Excel;
using System.IO;
using System.Data;
using HRTool.CommanMethods;
using static HRTool.CommanMethods.Enums;
using System.Globalization;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class EmployeeController : Controller
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        EmployeeMethod _employeeMethod = new EmployeeMethod();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        CompanyStructureMethod _CompanyStructureMethod = new CompanyStructureMethod();
        AdminPearformanceMethod _AdminPearformanceMethod = new AdminPearformanceMethod();
        EmployeeProfileMethod _EmployeeProfileMethod = new EmployeeProfileMethod();
        AdminTMSMethod _AdminTMSMethod = new AdminTMSMethod();

        public string defaultPassword = WebConfigurationManager.AppSettings["defaultpassword"].ToString();

        #endregion

        public ActionResult validateSSO(string ID)
        {
            bool employeeData = _employeeMethod.validateSSo(ID);
            return Json(employeeData,JsonRequestBehavior.AllowGet);
        }

        public ActionResult validEmaiId(string ID)
        {
            bool employeeData = _employeeMethod.validEmailId(ID);
            return Json(employeeData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BindMenuIndex(int EmployeeId)
        {
            EmployeeMenuModel model = new EmployeeMenuModel();
            var employeeData = _employeeMethod.getEmployeeById(EmployeeId);
            model.Id = employeeData.Id;
            model.EmployeeName = employeeData.FirstName + " " + employeeData.LastName;
            model.EmployeeImage = employeeData.image;

            if (employeeData.JobTitle != null)
            {
                var jobtitle = _otherSettingMethod.getSystemListValueById((int)employeeData.JobTitle);
                model.Jobtilte = jobtitle.Value;
            }
            //model.EmployeeName = employeeData.FirstName + " " + employeeData.LastName;
            //model.EmployeeImage = employeeData.ImageURL;

            //string userId = User.Identity.GetUserId();
            //var account = new AccountController();
            //var userRoles = account.UserManager.GetRoles(userId).FirstOrDefault();
            int userId = SessionProxy.UserId;
            var userRoles = _db.AspNetRoles.Where(x => x.AspNetUserRoles.Any(xx => xx.UserId == userId)).FirstOrDefault();

            model.EmployeeRole = userRoles.Name;
            
            if (employeeData.SSOID.StartsWith("C"))
            {
                model.Resources_OverView = false;
                model.Resources_Planner = false;
                model.Resources_ProjectPlanner = false;
                model.Resources_Performance = true;
                model.Resources_SkillsEndorsement = true;
                model.Resources_Skills = false;
                model.Resources_Training = true;
                model.Resources_Documents = true;
                model.Resources_Resume_CV = false;
                model.Resources_Profile = true;
                model.Resources_Employment = false;
                model.Resources_Contacts = false;
                model.Resources_Benefits = false;
                model.Resources_Caselog = true;
            }
            else if (userRoles.Id == 1)
            {
                model.Resources_OverView = true;
                model.Resources_Planner = true;
                model.Resources_ProjectPlanner = true;
                model.Resources_Performance = true;
                model.Resources_SkillsEndorsement = true;
                model.Resources_Skills = true;
                model.Resources_Training = true;
                model.Resources_Documents = true;
                model.Resources_Resume_CV = true;
                model.Resources_Profile = true;
                model.Resources_Employment = true;
                model.Resources_Contacts = true;
                model.Resources_Benefits = true;
                model.Resources_Caselog = true;
            }
            else
            {
                var userMenus = (from record in _db.Role_DefaultMenu where record.RoleId == userRoles.Id select record).ToList();
                model.Resources_OverView = false;
                model.Resources_Planner = false;
                model.Resources_ProjectPlanner = false;
                model.Resources_Performance = false;
                model.Resources_SkillsEndorsement = false;
                model.Resources_Skills = false;
                model.Resources_Training = false;
                model.Resources_Documents = false;
                model.Resources_Resume_CV = false;
                model.Resources_Profile = false;
                model.Resources_Employment = false;
                model.Resources_Contacts = false;
                model.Resources_Benefits = false;
                model.Resources_Caselog = false;
                foreach (var item in userMenus)
                {
                    if (item.MenuKey == (int)Menukey.Resources_OverView)
                    {
                        model.Resources_OverView = true;
                    }
                    if (item.MenuKey == (int)Menukey.Resources_Planner)
                    {
                        model.Resources_Planner = true;
                    }
                    if (item.MenuKey == (int)Menukey.Resources_ProjectPlanner)
                    {
                        model.Resources_ProjectPlanner = true;
                    }
                    if (item.MenuKey == (int)Menukey.Resources_Performance)
                    {
                        model.Resources_Performance = true;
                    }
                    if (item.MenuKey == (int)Menukey.Resources_SkillsEndorsement)
                    {
                        model.Resources_SkillsEndorsement = true;
                    }
                    if (item.MenuKey == (int)Menukey.Resources_Skills)
                    {
                        model.Resources_Skills = true;
                    }
                    if (item.MenuKey == (int)Menukey.Resources_Training)
                    {
                        model.Resources_Training = true;
                    }
                    if (item.MenuKey == (int)Menukey.Resources_Documents)
                    {
                        model.Resources_Documents = true;
                    }
                    if (item.MenuKey == (int)Menukey.Resources_Resume_CV)
                    {
                        model.Resources_Resume_CV = true;
                    }
                    if (item.MenuKey == (int)Menukey.Resources_Profile)
                    {
                        model.Resources_Profile = true;
                    }
                    if (item.MenuKey == (int)Menukey.Resources_Employment)
                    {
                        model.Resources_Employment = true;
                    }
                    if (item.MenuKey == (int)Menukey.Resources_Contacts)
                    {
                        model.Resources_Contacts = true;
                    }
                    if (item.MenuKey == (int)Menukey.Resources_Benefits)
                    {
                        model.Resources_Benefits = true;
                    }
                    if (item.MenuKey == (int)Menukey.Resources_Caselog)
                    {
                        model.Resources_Caselog = true;
                    }
                }
            }

            return PartialView("_partialResourceMenu", model);
        }
        public ActionResult Index(string EmployeeId)
        {
            MainResoureViewModel model = new MainResoureViewModel();
            

            return View();
        }
        public MainResoureViewModel returnCustomerList(int EmpId)
        {            
            MainResoureViewModel model = new MainResoureViewModel();
            var data = _db.AspNetUsers.Where(x => x.Id == EmpId && x.Archived == false).FirstOrDefault();
            model.RoleType = true;
            if (data.CustomerCareID != null) { 
            string[] values = data.CustomerCareID.Split(',');
            model.cFlag = 1;
                for (int i = 0; i < values.Length; i++)
                {
                    model.CustomerCareId = values[i];
                    int customerCareNo = Convert.ToInt32(values[i]);
                    List<AspNetUser> dataResource = _db.AspNetUsers.Where(x => x.Id == customerCareNo && x.Archived == false).ToList();
                    foreach (var item in dataResource)
                    {
                        var EmployeeRelation = _db.EmployeeRelations.Where(x => x.UserID == item.Id && x.IsActive == true).FirstOrDefault();
                        MainResoureViewModel Resoure = new MainResoureViewModel();
                        var empAdd = _db.EmployeeAddressInfoes.Where(x => x.UserId == item.Id);
                        int techSkill = _db.Employee_Skills.Where(x => x.EmployeeId == item.Id).Select(x => x.TechnicalSkillsName).Count();
                        int genSkill = _db.Employee_Skills.Where(x => x.EmployeeId == item.Id).Select(x => x.GeneralSkillsName).Count();
                        Resoure.TotalSkill = Convert.ToString(techSkill + genSkill);
                        int dt = item.StartDate.Value.Year;
                        Resoure.LengthofService = Convert.ToString(DateTime.Now.Year - dt);
                        int noofSkill = _db.Employee_AddEndrosementSkills.Where(x => x.EmployeeId == item.Id && x.Archived == false).Select(x => x.UserIDCreatedBy).Count();
                        Resoure.NumberofSkillsEndorsed = Convert.ToString(noofSkill);
                        int noofEndReceive = _db.Employee_AddEndrosementSkills.Where(x => x.EmployeeId == item.Id && x.Archived == false).Select(x => x.EmployeeId).Count();
                        Resoure.NoOfEndorsmntReceive = Convert.ToString(noofEndReceive);
                        // model.TotalSkill= _AdminPearformanceMethod.getEmployeeSkill(item.Id);
                        Resoure.Id = item.Id;
                        Resoure.FirstName = string.Format("{0} {1}-{2}", item.FirstName, item.LastName, item.SSOID);
                        if (item.JobTitle != 0 && item.JobTitle != null)
                        { Resoure.JobTitleName = _employeeMethod.Jobtitel(Convert.ToInt32(item.JobTitle)); }
                        if (EmployeeRelation != null)
                        {
                            if (EmployeeRelation.BusinessID != 0 && EmployeeRelation.BusinessID != null)
                            { Resoure.BusinessName = _AdminPearformanceMethod.BusinessId(Convert.ToInt32(EmployeeRelation.BusinessID)); }
                            if (EmployeeRelation.DivisionID != 0 && EmployeeRelation.DivisionID != null)
                            { Resoure.DivisionName = _AdminPearformanceMethod.DivisionId(Convert.ToInt32(EmployeeRelation.DivisionID)); }
                            if (EmployeeRelation.PoolID != 0 && EmployeeRelation.PoolID != null)
                            { Resoure.PoolName = _AdminPearformanceMethod.PoolId(Convert.ToInt32(EmployeeRelation.PoolID)); }
                            if (EmployeeRelation.FunctionID != 0 && EmployeeRelation.FunctionID != null)
                            { Resoure.FunctionName = _AdminPearformanceMethod.FucantionId(Convert.ToInt32(EmployeeRelation.FunctionID)); }
                        }
                        if (empAdd != null)
                        {
                            foreach (var cdata in empAdd)
                            {
                                if (cdata.CountryId != 0 || cdata.CountryId != null)
                                {
                                    Resoure.ContryName = _employeeMethod.GetCountries(Convert.ToInt32(cdata.CountryId));
                                }
                            }
                        }
                        if (item.Location != 0 && item.Location != null)
                        { Resoure.LocationName = _AdminPearformanceMethod.LocationId(Convert.ToInt32(item.Location)); ; }
                        if (item.Company != 0 && item.Company != null)
                        { Resoure.CompanyName = _AdminPearformanceMethod.CompanyId(Convert.ToInt32(item.Company)); }
                        if (item.ResourceType != 0 && item.ResourceType != null)
                        { Resoure.ResourceTypeName = _employeeMethod.getResourceType(Convert.ToInt32(item.ResourceType)); }
                        if (item.Nationality != 0 && item.Nationality != null)
                        { Resoure.NationalityName = _employeeMethod.GetNationality(Convert.ToInt32(item.Nationality)); }
                        model.AllListData.Add(Resoure);
                    }
                }
            }
            return model;
        }
        public ActionResult ResoureList()
        {
            MainResoureViewModel model = returnList();
            return PartialView("_PartialAddResoureList", model);
        }
        //public ActionResult ResoureList(int EmpId)
        //{
        //    MainResoureViewModel model = new MainResoureViewModel();
        //    var data = _db.AspNetUsers.Where(x => x.Id == EmpId && x.Archived == false).FirstOrDefault();
        //    if (data != null)
        //    {
        //        if (data.SSOID.Contains('C'))
        //        {
        //            model = returnCustomerList(EmpId);
        //        }
        //        else
        //        {
        //            model = returnList();
        //        }
        //    }            
        //    else
        //    {
        //        model = returnList();
        //    }            
        //    return PartialView("_PartialAddResoureList", model);
        //}
        public MainResoureViewModel returnList()
        {

            //List<AspNetUser> dataResource = _employeeMethod.GetAllResourceEmployeeList().Where(x=> x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList();
            int userId = SessionProxy.UserId;
            List<AspNetUser> dataResource = _employeeMethod.GetAllResourceEmployeeList().ToList();
            var data = _db.AspNetUserRoles.Where(x => x.UserId == userId && x.RoleId == 1).ToList();
            if (data != null && data.Count > 0)
            {
                dataResource = _employeeMethod.GetAllResourceEmployeeList().Where(x => x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList();
            }
            else
            {
                dataResource = _employeeMethod.GetAllResourceEmployeeList().ToList();
            }
            MainResoureViewModel model = new MainResoureViewModel();


            //var employeeDetail=(from i in dataResource join er in _db.EmployeeRelations on i.Id equals er.Reportsto where er.IsActive==true select new {er.Reportsto,i.Id,i.FirstName,i.LastName,i.SSOID,i.JobTitle,i.Company
            //             ,i.ResourceType,i.Nationality,i.Location,i.StartDate
            //                }).ToList();
            //if (data.Count > 0 && data != null)
            //{

            //}
            //else
            //{
            //    employeeDetail = employeeDetail.Where(x => x.Reportsto == userId).ToList();
                foreach (var item in dataResource)
                {
                var EmployeeRelation = _db.EmployeeRelations.Where(x => x.UserID == item.Id && x.IsActive == true ).FirstOrDefault();
                if (data != null && data.Count > 0)
                {
                    EmployeeRelation = _db.EmployeeRelations.Where(x => x.UserID == item.Id && x.IsActive == true).FirstOrDefault();
                    MainResoureViewModel Resoure = new MainResoureViewModel();
                    var empAdd = _db.EmployeeAddressInfoes.Where(x => x.UserId == item.Id);
                    int techSkill = _db.Employee_Skills.Where(x => x.EmployeeId == item.Id).Select(x => x.TechnicalSkillsName).Count();
                    int genSkill = _db.Employee_Skills.Where(x => x.EmployeeId == item.Id).Select(x => x.GeneralSkillsName).Count();
                    Resoure.TotalSkill = Convert.ToString(techSkill + genSkill);
                    int dt = item.StartDate.Value.Year;
                    Resoure.LengthofService = Convert.ToString(DateTime.Now.Year - dt);
                    int noofSkill = _db.Employee_AddEndrosementSkills.Where(x => x.EmployeeId == item.Id && x.Archived == false).Select(x => x.UserIDCreatedBy).Count();
                    Resoure.NumberofSkillsEndorsed = Convert.ToString(noofSkill);
                    int noofEndReceive = _db.Employee_AddEndrosementSkills.Where(x => x.EmployeeId == item.Id && x.Archived == false).Select(x => x.EmployeeId).Count();
                    Resoure.NoOfEndorsmntReceive = Convert.ToString(noofEndReceive);
                    // model.TotalSkill= _AdminPearformanceMethod.getEmployeeSkill(item.Id);
                    Resoure.Id = item.Id;
                    Resoure.FirstName = string.Format("{0} {1}-{2}", item.FirstName, item.LastName, item.SSOID);
                    if (item.JobTitle != 0 && item.JobTitle != null)
                    { Resoure.JobTitleName = _employeeMethod.Jobtitel(Convert.ToInt32(item.JobTitle)); }
                    if (EmployeeRelation != null)
                    {
                        if (EmployeeRelation.BusinessID != 0 && EmployeeRelation.BusinessID != null)
                        { Resoure.BusinessName = _AdminPearformanceMethod.BusinessId(Convert.ToInt32(EmployeeRelation.BusinessID)); }
                        if (EmployeeRelation.DivisionID != 0 && EmployeeRelation.DivisionID != null)
                        { Resoure.DivisionName = _AdminPearformanceMethod.DivisionId(Convert.ToInt32(EmployeeRelation.DivisionID)); }
                        if (EmployeeRelation.PoolID != 0 && EmployeeRelation.PoolID != null)
                        { Resoure.PoolName = _AdminPearformanceMethod.PoolId(Convert.ToInt32(EmployeeRelation.PoolID)); }
                        if (EmployeeRelation.FunctionID != 0 && EmployeeRelation.FunctionID != null)
                        { Resoure.FunctionName = _AdminPearformanceMethod.FucantionId(Convert.ToInt32(EmployeeRelation.FunctionID)); }
                    }
                    if (empAdd != null)
                    {
                        foreach (var cdata in empAdd)
                        {
                            if (cdata.CountryId != 0 || cdata.CountryId != null)
                            {
                                Resoure.ContryName = _employeeMethod.GetCountries(Convert.ToInt32(cdata.CountryId));
                            }
                        }
                    }
                    if (item.Location != 0 && item.Location != null)
                    { Resoure.LocationName = _AdminPearformanceMethod.LocationId(Convert.ToInt32(item.Location)); ; }
                    if (item.Company != 0 && item.Company != null)
                    { Resoure.CompanyName = _AdminPearformanceMethod.CompanyId(Convert.ToInt32(item.Company)); }
                    if (item.ResourceType != 0 && item.ResourceType != null)
                    { Resoure.ResourceTypeName = _employeeMethod.getResourceType(Convert.ToInt32(item.ResourceType)); }
                    if (item.Nationality != 0 && item.Nationality != null)
                    { Resoure.NationalityName = _employeeMethod.GetNationality(Convert.ToInt32(item.Nationality)); }
                    model.AllListData.Add(Resoure);
                }
                else
                {
                    if (EmployeeRelation != null)
                    {
                        MainResoureViewModel Resoure = new MainResoureViewModel();
                        var empAdd = _db.EmployeeAddressInfoes.Where(x => x.UserId == item.Id);
                        int techSkill = _db.Employee_Skills.Where(x => x.EmployeeId == item.Id).Select(x => x.TechnicalSkillsName).Count();
                        int genSkill = _db.Employee_Skills.Where(x => x.EmployeeId == item.Id).Select(x => x.GeneralSkillsName).Count();
                        Resoure.TotalSkill = Convert.ToString(techSkill + genSkill);
                        int dt = item.StartDate.Value.Year;
                        Resoure.LengthofService = Convert.ToString(DateTime.Now.Year - dt);
                        int noofSkill = _db.Employee_AddEndrosementSkills.Where(x => x.EmployeeId == item.Id && x.Archived == false).Select(x => x.UserIDCreatedBy).Count();
                        Resoure.NumberofSkillsEndorsed = Convert.ToString(noofSkill);
                        int noofEndReceive = _db.Employee_AddEndrosementSkills.Where(x => x.EmployeeId == item.Id && x.Archived == false).Select(x => x.EmployeeId).Count();
                        Resoure.NoOfEndorsmntReceive = Convert.ToString(noofEndReceive);
                        // model.TotalSkill= _AdminPearformanceMethod.getEmployeeSkill(item.Id);
                        Resoure.Id = item.Id;
                        Resoure.FirstName = string.Format("{0} {1}-{2}", item.FirstName, item.LastName, item.SSOID);
                        if (item.JobTitle != 0 && item.JobTitle != null)
                        { Resoure.JobTitleName = _employeeMethod.Jobtitel(Convert.ToInt32(item.JobTitle)); }
                        if (EmployeeRelation != null)
                        {
                            if (EmployeeRelation.BusinessID != 0 && EmployeeRelation.BusinessID != null)
                            { Resoure.BusinessName = _AdminPearformanceMethod.BusinessId(Convert.ToInt32(EmployeeRelation.BusinessID)); }
                            if (EmployeeRelation.DivisionID != 0 && EmployeeRelation.DivisionID != null)
                            { Resoure.DivisionName = _AdminPearformanceMethod.DivisionId(Convert.ToInt32(EmployeeRelation.DivisionID)); }
                            if (EmployeeRelation.PoolID != 0 && EmployeeRelation.PoolID != null)
                            { Resoure.PoolName = _AdminPearformanceMethod.PoolId(Convert.ToInt32(EmployeeRelation.PoolID)); }
                            if (EmployeeRelation.FunctionID != 0 && EmployeeRelation.FunctionID != null)
                            { Resoure.FunctionName = _AdminPearformanceMethod.FucantionId(Convert.ToInt32(EmployeeRelation.FunctionID)); }
                        }
                        if (empAdd != null)
                        {
                            foreach (var cdata in empAdd)
                            {
                                if (cdata.CountryId != 0 || cdata.CountryId != null)
                                {
                                    Resoure.ContryName = _employeeMethod.GetCountries(Convert.ToInt32(cdata.CountryId));
                                }
                            }
                        }
                        if (item.Location != 0 && item.Location != null)
                        { Resoure.LocationName = _AdminPearformanceMethod.LocationId(Convert.ToInt32(item.Location)); ; }
                        if (item.Company != 0 && item.Company != null)
                        { Resoure.CompanyName = _AdminPearformanceMethod.CompanyId(Convert.ToInt32(item.Company)); }
                        if (item.ResourceType != 0 && item.ResourceType != null)
                        { Resoure.ResourceTypeName = _employeeMethod.getResourceType(Convert.ToInt32(item.ResourceType)); }
                        if (item.Nationality != 0 && item.Nationality != null)
                        { Resoure.NationalityName = _employeeMethod.GetNationality(Convert.ToInt32(item.Nationality)); }
                        model.AllListData.Add(Resoure);
                    }
                }
                }            
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Job Role List").ToList())
            {
                if (item.Value == "Full-Time")
                {
                    //model.FullTime = _db.AspNetUsers.Where(x => x.ResourceType == item.Id && x.SSOID.Contains("W") && x.Archived == false && x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList().Count();
                    model.FullTime = dataResource.Where(x=>x.ResourceType == item.Id).Count();

                }
                if (item.Value == "Part-Time")
                {
                    //model.PartTime = _db.AspNetUsers.Where(x => x.ResourceType == item.Id && x.SSOID.Contains("W") && x.Archived == false && x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList().Count();
                    model.PartTime = dataResource.Where(x => x.ResourceType == item.Id).Count();
                }
                if (item.Value == "Temporary")
                {
                    //model.TemporaryTime = _db.AspNetUsers.Where(x => x.ResourceType == item.Id && x.SSOID.Contains("W") && x.Archived == false && x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList().Count();
                    model.TemporaryTime = dataResource.Where(x => x.ResourceType == item.Id).Count();
                }
            }
            return model;
        }
        public ActionResult CustomerList()
        {
            MainResoureViewModel model = returnCustomerListList();
            return PartialView("_PartialAddCustomerResourceList", model);
        }
        public MainResoureViewModel returnCustomerListList()
        {
            //List<AspNetUser> data = _employeeMethod.GetAllEmployeeList();
            List<AspNetUser> dataCoustomer = _employeeMethod.GetAllCoustomerEmployeeList().Where(x => x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList();
            MainResoureViewModel model = new MainResoureViewModel();
            foreach (var item in dataCoustomer)
            {
                var EmployeeRelation = _db.EmployeeRelations.Where(x => x.UserID == item.Id && x.IsActive == true).FirstOrDefault();
                MainResoureViewModel Resoure = new MainResoureViewModel();
                Resoure.Id = item.Id;
                Resoure.FirstName = string.Format("{0} {1}-{2}", item.FirstName, item.LastName, item.SSOID);
                Resoure.StartDate = Convert.ToString(item.StartDate);
                if (item.JobTitle != 0 && item.JobTitle != null)
                { Resoure.JobTitleName = _employeeMethod.Jobtitel(Convert.ToInt32(item.JobTitle)); }
                if (EmployeeRelation != null)
                {
                    if (EmployeeRelation.BusinessID != 0 && EmployeeRelation.BusinessID != null)
                    { Resoure.BusinessName = _AdminPearformanceMethod.BusinessId(Convert.ToInt32(EmployeeRelation.BusinessID)); }
                    if (EmployeeRelation.DivisionID != 0 && EmployeeRelation.DivisionID != null)
                    { Resoure.DivisionName = _AdminPearformanceMethod.DivisionId(Convert.ToInt32(EmployeeRelation.DivisionID)); }
                    if (EmployeeRelation.PoolID != 0 && EmployeeRelation.PoolID != null)
                    { Resoure.PoolName = _AdminPearformanceMethod.PoolId(Convert.ToInt32(EmployeeRelation.PoolID)); }
                    if (EmployeeRelation.FunctionID != 0 && EmployeeRelation.FunctionID != null)
                    { Resoure.FunctionName = _AdminPearformanceMethod.FucantionId(Convert.ToInt32(EmployeeRelation.FunctionID)); }
                }
                int noofSkill = _db.Employee_AddEndrosementSkills.Where(x => x.EmployeeId == item.Id && x.Archived == false).Select(x => x.UserIDCreatedBy).Count();
                Resoure.NumberofSkillsEndorsed = Convert.ToString(noofSkill);
                DateTime futurDate = Convert.ToDateTime(item.StartDate);
                DateTime TodayDate = DateTime.Now;
                var numberOfDays = (TodayDate- futurDate).TotalDays;
                int totalDay = Convert.ToInt32(numberOfDays);
                Resoure.LengthofService =Convert.ToString(totalDay);
                if (item.Location != 0 && item.Location != null)
                { Resoure.LocationName = _AdminPearformanceMethod.LocationId(Convert.ToInt32(item.Location)); ; }
                if (item.Company != 0 && item.Company != null)
                { Resoure.CompanyName = _AdminPearformanceMethod.CompanyId(Convert.ToInt32(item.Company)); }
                if (item.ResourceType != 0 && item.ResourceType != null)
                { Resoure.ResourceTypeName = _employeeMethod.getResourceType(Convert.ToInt32(item.ResourceType)); }
                if (item.Nationality != 0 && item.Nationality != null)
                { Resoure.NationalityName = _employeeMethod.GetNationality(Convert.ToInt32(item.Nationality)); }
                model.AllListData.Add(Resoure);
            }
            var toatlcomp = _otherSettingMethod.getAllSystemValueListByKeyName("Company List");
            model.TotalCoumpany = toatlcomp.Where(x=>x.UserIDCreatedBy == SessionProxy.UserId).Count();
            model.TotalCustomer = dataCoustomer.Where(x=>x.CreatedBy == SessionProxy.UserId).Count();
            return model;
        }
        public ActionResult AddResoureRecord(MainResoureViewModel model)
        {
            _employeeMethod.DeleteTempTask();
            model.PasswordHash = Common.Encrypt(defaultPassword,true);
            //var account = new AccountController();
            //var user = new ApplicationUser() { UserName = model.UserNameEmail };
            //if (user != null)
            //{
            //    var result = account.UserManager.CreateAsync(user, model.PasswordHash);
            //}
            //var lastInsertesUser = _db.AspNetUsers.Where(x => x.Id == Convert.ToInt32(user.Id)).FirstOrDefault();
            //if (lastInsertesUser != null)
            //{
            //    model.Id = lastInsertesUser.Id;
            //}
            //var role = _db.AspNetRoles.Where(x => x.Name == "Employee").FirstOrDefault();
            //if (role != null)
            //{
            //    _db.AssignUserRole(lastInsertesUser.Id, role.Id);
            //}
            model.CurrentUserId = SessionProxy.UserId;
            
            int Id = _employeeMethod.SaveResourcetSet(model);
            if (Id > 0)
            {
                var role = _db.AspNetRoles.Where(x => x.Name == "Employee").FirstOrDefault();
                if (role != null)
                {
                    _db.AssignUserRole(Id, role.Id);
                }
            }
            if (model.ApplicantID == 0 || model.ApplicantID==null)
            {
                MainResoureViewModel modelListResource = returnList();
                return PartialView("_PartialAddResoureList", modelListResource);
            }
            else
            {
                return Json(new
                {
                    redirectUrl = RedirectToAction("VacancyDetails", "TMS", new { Id = model.VacancyId }),
                    isRedirect = true
                });
               
           }

        }
        public ActionResult getEmpCopyData()
        {
            MainResoureViewModel model = new MainResoureViewModel();
            List<AspNetUser> data = _AdminPearformanceMethod.getAllUserList().ToList();
            model.CopyFromList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in data)
            {
                string Name = string.Format("{0} {1} - {2}", item.FirstName, item.LastName,item.SSOID);
                model.CopyFromList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
            }
            //model.ReportstoList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            //model.AdditionalReportstoList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            //model.HRResponsibleList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in data)
            {
                string Name = string.Format("{0} {1} {2}", item.FirstName, item.LastName, item.SSOID);
                model.ReportstoList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
                model.AdditionalReportstoList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
                model.HRResponsibleList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddEditProjectSet(int Id)
        {
            _employeeMethod.DeleteTempTask();
            MainResoureViewModel model = new MainResoureViewModel();
            //var workData = _employeeMethod.workPattData();
            var Title = _otherSettingMethod.getAllSystemValueListByKeyName("Title List");
            model.TitleList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in Title)
            {
                model.TitleList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Gender List"))
            {
                model.GenderList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            var Nationality = _otherSettingMethod.getAllSystemValueListByKeyName("Nationality List");
            model.NationalityList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in Nationality)
            {
                model.NationalityList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var ResourceType = _otherSettingMethod.getAllSystemValueListByKeyName("Job Role List");
            model.ResourceTypeList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in ResourceType)
            {
                model.ResourceTypeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            List<AspNetUser> data = _AdminPearformanceMethod.getAllUserList().ToList();
            model.CopyFromList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            model.ReportstoList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            model.AdditionalReportstoList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            model.HRResponsibleList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            //model.AssignList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in data)
            {
                string Name = string.Format("{0} {1}", item.FirstName, item.LastName);
                model.CopyFromList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
                model.ReportstoList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
                model.AdditionalReportstoList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
                model.HRResponsibleList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
                //model.AssignList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
            }
            var jobTitle = _otherSettingMethod.getAllSystemValueListByKeyName("Job Title List");
            model.JobTitleList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in jobTitle)
            {
                model.JobTitleList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }

            var JobCountry = _employeeMethod.bindPublicHolidayCountryList();
            // model.JobCountryList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in JobCountry)
            {
                model.JobCountryList.Add(new SelectListItem() { Text = @item.Text, Value = @item.Value.ToString() });
            }
            var Location = _otherSettingMethod.getAllSystemValueListByKeyName("Office Locations");
            model.LocationList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in Location)
            {
                model.LocationList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var BusinessList = _CompanyStructureMethod.getAllBusinessList();
            model.BusinessList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in BusinessList)
            {
                model.BusinessList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString() });
            }

            var NoticePeriod = _otherSettingMethod.getAllSystemValueListByKeyName("Notice Period List");
            model.NoticePeriodList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in NoticePeriod)
            {
                model.NoticePeriodList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var currency = _db.Currencies.ToList();
            var currencyCodedata = _employeeMethod.getCurrCode();
            foreach (var item in currencyCodedata)
            {
                 model.curruencyCode = Convert.ToString(item.BaseCurrency);              
            }

            foreach (var item in currency)
            {
                model.CurruencyCodeList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }

            //foreach (var item in workData)
            //{
            //    model.WorkPatternList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            //}
            var Status = _otherSettingMethod.getAllSystemValueListByKeyName("Task Status");
            model.StatusList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in Status)
            {
                model.StatusList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }

            var listrecord = _employeeMethod.GetNewstaskrecord();
            foreach (var item in listrecord)
            {
                AddNewTaskListViewModel m = new AddNewTaskListViewModel();
                m.Id = item.Id;
                m.Title = item.Title;
                m.Description = item.Description;
                m.Assign = item.AssignTo;
                m.AlertBeforeDays = (int)item.AlterBeforeDays;
                m.DueDate = String.Format("{0:dd-MM-yyy}", item.DueDate);
                m.IsTemp = 0;
                model.NewsTasklistRecord.Add(m);
            }
            model.CountryDropdown = _employeeMethod.BindCountryDropdown().OrderBy(x => x.Text).ToList();
            int count = (from row in _db.AspNetUsers select row).Count();
            model.SSO = (count + 1).ToString();
            model.Gender = Convert.ToInt16(model.GenderList[0].Value);
            model.LoginUserID = SessionProxy.UserId;
            return PartialView("_PartialAddResoureSet", model);

        }

        public ActionResult AddApplicantAsResource(int Id, int VacancyID)
        {
            MainResoureViewModel model = new MainResoureViewModel();
            var Title = _otherSettingMethod.getAllSystemValueListByKeyName("Title List");
            model.TitleList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in Title)
            {
                model.TitleList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Gender List"))
            {
                model.GenderList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            var Nationality = _otherSettingMethod.getAllSystemValueListByKeyName("Nationality List");
            model.NationalityList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in Nationality)
            {
                model.NationalityList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var ResourceType = _otherSettingMethod.getAllSystemValueListByKeyName("Job Role List");
            model.ResourceTypeList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in ResourceType)
            {
                model.ResourceTypeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var currency = _db.Currencies.ToList();
            var currencyCodedata = _employeeMethod.getCurrCode();
            foreach (var item in currencyCodedata)
            {
                model.curruencyCode = Convert.ToString(item.BaseCurrency);
            }

            foreach (var item in currency)
            {
                model.CurruencyCodeList.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            List<AspNetUser> data = _AdminPearformanceMethod.getAllUserList().ToList();
            model.CopyFromList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            model.ReportstoList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            model.AdditionalReportstoList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            model.HRResponsibleList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            //model.AssignList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in data)
            {
                string Name = string.Format("{0} {1}", item.FirstName, item.LastName);
                model.CopyFromList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
                model.ReportstoList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
                model.AdditionalReportstoList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
                model.HRResponsibleList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
                //model.AssignList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
            }
            var jobTitle = _otherSettingMethod.getAllSystemValueListByKeyName("Job Title List");
            model.JobTitleList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in jobTitle)
            {
                model.JobTitleList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }

            var JobCountry = _employeeMethod.bindPublicHolidayCountryList();
            // model.JobCountryList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in JobCountry)
            {
                model.JobCountryList.Add(new SelectListItem() { Text = @item.Text, Value = @item.Value.ToString() });
            }
            var Location = _otherSettingMethod.getAllSystemValueListByKeyName("Office Locations");
            model.LocationList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in Location)
            {
                model.LocationList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var BusinessList = _CompanyStructureMethod.getAllBusinessList();
            model.BusinessList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in BusinessList)
            {
                model.BusinessList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString() });
            }

            var NoticePeriod = _otherSettingMethod.getAllSystemValueListByKeyName("Notice Period List");
            model.NoticePeriodList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in NoticePeriod)
            {
                model.NoticePeriodList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }

            var Status = _otherSettingMethod.getAllSystemValueListByKeyName("Task Status");
            model.StatusList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in Status)
            {
                model.StatusList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }

            var listrecord = _employeeMethod.GetNewstaskrecord();
            foreach (var item in listrecord)
            {
                AddNewTaskListViewModel m = new AddNewTaskListViewModel();
                m.Id = item.Id;
                m.Title = item.Title;
                m.Description = item.Description;
                m.Assign = item.AssignTo;
                m.AlertBeforeDays = (int)item.AlterBeforeDays;
                m.DueDate = String.Format("{0:dd-MM-yyy}", item.DueDate);
                model.NewsTasklistRecord.Add(m);
            }
            model.CountryDropdown = _employeeMethod.BindCountryDropdown();
            int count = (from row in _db.AspNetUsers select row).Count();
            model.SSO = (count + 1).ToString();
            if (Id != 0)
            {
                var ApplicantDetails = _AdminTMSMethod.getApplicantDetailsById(Id);
                model.VacancyId = VacancyID;
                model.ApplicantID = ApplicantDetails.Id;
                model.FirstName = ApplicantDetails.FirstName;
                model.LastName = ApplicantDetails.LastName;
                model.Gender = ApplicantDetails.GenderID;
                model.DateOfBirth = String.Format("{0:dd-MM-yyyy}", ApplicantDetails.DateOfBirth);
                model.PostalCode = ApplicantDetails.PostalCode;
                model.Address = ApplicantDetails.Address;
                model.PersonalEmail = ApplicantDetails.Email;
                model.PersonalMobile = ApplicantDetails.OtherContactDetails;
            }
            return PartialView("_PartialAddResoureSet", model);
        }

        public ActionResult getEmployeeCustomerCare()
        {
            MainResoureViewModel model = new MainResoureViewModel();
            foreach (var item in _employeeMethod.GetAllResourceEmployeeList().Where(x => x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList())
            {
                model.EmployeeCustomerCare.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }
            return Json(model, JsonRequestBehavior.AllowGet);

        }

        public ActionResult bindDivisionList(int businessId)
        {
            var data = _CompanyStructureMethod.GetDivisionListByBizId(businessId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult bindPoolList(int DivisionId)
        {
            var data = _CompanyStructureMethod.GetPoolListByBizId(DivisionId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult bindFuncationList(int DivisionId)
        {
            var data = _CompanyStructureMethod.GetFuncationListByBizId(DivisionId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BindStateDropdown(int countryId)
        {
            try
            {
                var state = _employeeMethod.BindStateDropdown(countryId);
                return Json(state, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }
        public ActionResult BindCityDropdown(int stateId)
        {
            try
            {
                var city = _employeeMethod.BindCityDropdown(stateId);
                return Json(city, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }

        }
        public ActionResult BindAirportDropdown(int countryId)
        {
            try
            {
                var state = _employeeMethod.BindAirportDropdown(countryId);
                return Json(state, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }
        public ActionResult DeleteRecord(int Id)
        {
            _employeeMethod.DeleteResource(Id);
            MainResoureViewModel modelList = returnList();
            return PartialView("_PartialAddResoureList", modelList);
        }
        public ActionResult CustomerResourceList(MainResoureViewModel model)
        {
            return PartialView("_PartialAddCustomerResourceList", model);
        }

        //Coustomer Record
        public ActionResult AddEditCusomerSet(int Id)
        {
            MainResoureViewModel model = new MainResoureViewModel();
            var Title = _otherSettingMethod.getAllSystemValueListByKeyName("Title List");
            model.TitleList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in Title)
            {
                model.TitleList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var jobTitle = _otherSettingMethod.getAllSystemValueListByKeyName("Job Title List");
            model.JobTitleList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in jobTitle)
            {
                model.JobTitleList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }

            //var CoustomerCompany = _otherSettingMethod.getAllSystemValueListByKeyName("Company List");
            var CoustomerCompany = _db.Company_Customer.Where(x => x.Archived == false).ToList();
            model.CoustomerCompanyList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in CoustomerCompany)
            {
                // string Name = string.Format("{0}{1}", item.FirstName, item.LastName);
                model.CoustomerCompanyList.Add(new SelectListItem() { Text = item.CompanyName, Value = @item.Id.ToString() });
            }
            int count = (from row in _db.AspNetUsers select row).Count();
            model.SSO = (count + 1).ToString();
            return PartialView("_PartialAddCustomerSet", model);
        }
        public ActionResult AddCoustomerRecord(MainResoureViewModel model)
        {
            //var account = new AccountController();
            //var user = new ApplicationUser() { UserName = model.UserNameEmail };
            //if (user != null)
            //{
            //    var result = account.UserManager.CreateAsync(user, model.PasswordHash);
            //}
            //var lastInsertesUser = _db.AspNetUsers.Where(x => x.Id == Convert.ToInt32(user.Id)).FirstOrDefault();
            //model.Id = lastInsertesUser.Id;
            //var role = _db.AspNetRoles.Where(x => x.Name == "Employee").FirstOrDefault();
            //if (role != null)
            //{
            //    _db.AssignUserRole(lastInsertesUser.Id, role.Id);
            //}
            model.PasswordHash = Common.Encrypt(defaultPassword,true);
            model.CurrentUserId = SessionProxy.UserId;
            model.StartDate = Convert.ToString(DateTime.Now);
            int Id = _employeeMethod.SaveCoustomertSet(model);
            if (Id > 0)
            {
                var role = _db.AspNetRoles.Where(x => x.Name == "Employee").FirstOrDefault();
                if (role != null)
                {
                    _db.AssignUserRole(Id, role.Id);
                }
                //MainResoureViewModel modelList = returnList();
                //return PartialView("_PartialAddResoureList", modelList);
            }
            MainResoureViewModel modelList = returnList();
            return PartialView("_PartialAddResoureList", modelList);
            //MainResoureViewModel modelList = returnCustomerListList();
            //return PartialView("_PartialAddCustomerResourceList", modelList);
        }
        public ActionResult DeleteWorkerRecord(int Id)
        {
            int Users = SessionProxy.UserId;
            _employeeMethod.DeleteWorkerRecord(Id, Users);
            MainResoureViewModel model = returnList();
            return PartialView("_PartialAddResoureList", model);
        }
        public ActionResult DeleteCoustomerRecord(int Id)
        {
            int Users = SessionProxy.UserId;
            _employeeMethod.DeleteCoustomerRecord(Id, Users);
            MainResoureViewModel model = returnCustomerListList();
            return PartialView("_PartialAddCustomerResourceList", model);

        }
        public ActionResult EditTask(int Id)
        {
            var Listrecortd = _db.Task_List.Where(x => x.Id == Id).FirstOrDefault();
            AddNewTaskListViewModel model = new AddNewTaskListViewModel();
            model.Id = Listrecortd.Id;
            model.Title = Listrecortd.Title;
            List<AspNetUser> data = _AdminPearformanceMethod.getAllUserList().ToList();
            foreach (var item in data)
            {
                string Name = string.Format("{0} {1}", item.FirstName, item.LastName);
                if (Listrecortd.AssignTo == item.Id)
                {
                    model.AssignList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString(), Selected = true });
                }
                else
                {
                    model.AssignList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
                }
            }
            var Status = _otherSettingMethod.getAllSystemValueListByKeyName("Task Status");
            model.StatusList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in Status)
            {
                if (Listrecortd.Status == item.Id)
                {
                    model.StatusList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                }
                else
                {
                    model.StatusList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                }
            }
            model.Description = Listrecortd.Description;
            model.DueDate = String.Format("{0:dd-MM-yyy}", Listrecortd.DueDate);
            model.AlertBeforeDays = (int)Listrecortd.AlterBeforeDays;
            return PartialView("_PartialAddTask", model);
        }
        public ActionResult CopyRecord(int EmployeeId)
        {
            MainResoureViewModel model = new MainResoureViewModel();
            if (EmployeeId != 0)
            {
                int userId = SessionProxy.UserId;
                model = _EmployeeProfileMethod.EmployeeProfileGetByID(EmployeeId, userId);
                return PartialView("_Partial_Step-2", model);
            }
            else
            {
                return View();
            }

        }
        public ActionResult AddNewTaskRecord(AddNewTaskListViewModel model)
        {
            int userid = SessionProxy.UserId;
            if (model.IdRecord > 0)
            {
                _employeeMethod.UpdateTaskRecord(model, userid);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = _employeeMethod.SaveTempTaskRecord(model, userid);

                return PartialView("_PartialAddNewsTaskSetList", data);
            }

        }
        public ActionResult ShowAddNewTask(AddNewTaskListViewModel model)
        {
            var Status = _otherSettingMethod.getAllSystemValueListByKeyName("Task Status");
            model.StatusList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in Status)
            {
                model.StatusList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            List<AspNetUser> data = _AdminPearformanceMethod.getAllUserList().ToList();
            model.AssignList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in data)
            {
                string Name = string.Format("{0} {1}", item.FirstName, item.LastName);
                model.AssignList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
            }
            return PartialView("_PartialAddTask", model);
        }
        public ActionResult DeteteTask(int Id)
        {
            Task_List AddUser = _db.Task_List.Where(x => x.Id == Id).FirstOrDefault();
            AddUser.Archived = true;
            AddUser.LastModified = DateTime.Now;
            _db.SaveChanges();
            var listrecord = _employeeMethod.GetNewstaskrecord();
            MainResoureViewModel model = new MainResoureViewModel();
            foreach (var item in listrecord)
            {
                AddNewTaskListViewModel m = new AddNewTaskListViewModel();
                m.Id = item.Id;
                m.Title = item.Title;
                m.Description = item.Description;
                m.Assign = item.AssignTo;
                m.AlertBeforeDays = (int)item.AlterBeforeDays;
                m.DueDate = String.Format("{0:dd-MM-yyy}", item.DueDate);
                model.NewsTasklistRecord.Add(m);
            }
            return PartialView("_Partial_Step-5", model);
        }

        public ActionResult UploadImage()
        {
            string FilePath = string.Empty;
            string fileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                FilePath = ConfigurationManager.AppSettings["ResourceFilePath"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }
            return Json(fileName, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExportExcel()
        {
            string ResourceList = "ResourceList";
            MainResoureViewModel model = returnList();
            DataTable dttable = new DataTable("Skill");
            dttable.Columns.Add("Name", typeof(string));
            dttable.Columns.Add("Job Title", typeof(string));
            dttable.Columns.Add("Company", typeof(string));
            dttable.Columns.Add("Business", typeof(string));
            dttable.Columns.Add("Division", typeof(string));
            dttable.Columns.Add("Pool", typeof(string));
            dttable.Columns.Add("Function", typeof(string));
            dttable.Columns.Add("Type", typeof(string));
            dttable.Columns.Add("Overall Score (Average Score)", typeof(string));
            dttable.Columns.Add("Core Strengths (Average Score)", typeof(string));
            dttable.Columns.Add("Number of Skills", typeof(int));
            dttable.Columns.Add("Number of Skills Endorsed", typeof(int));
            dttable.Columns.Add("Nationality", typeof(string));
            dttable.Columns.Add("Country of Residence", typeof(string));
            dttable.Columns.Add("Length of service", typeof(string));
            foreach (var item in model.AllListData)
            {
                List<string> lstStrRow = new List<string>();
                lstStrRow.Add(string.Format("{0}{1}", item.FirstName, item.LastName));
                lstStrRow.Add(item.JobTitleName);
                lstStrRow.Add(item.CompanyName);
                lstStrRow.Add(item.BusinessName);
                lstStrRow.Add(item.DivisionName);
                lstStrRow.Add(item.PoolName);
                lstStrRow.Add(item.FunctionName);
                lstStrRow.Add(item.ResourceTypeName);
                
                lstStrRow.Add(null);
                lstStrRow.Add(null);
                lstStrRow.Add(null);
                lstStrRow.Add(null);
                lstStrRow.Add(item.NationalityName);
                lstStrRow.Add(null);
                lstStrRow.Add(item.LengthofService);
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
        public ActionResult ExportExcelCoustomer()
        {
            string ResourceList = "CoustomerList";
            MainResoureViewModel model = returnCustomerListList();
            DataTable dttable = new DataTable("Skill");
            dttable.Columns.Add("Name", typeof(string));
            dttable.Columns.Add("Job Title", typeof(string));
            dttable.Columns.Add("Company", typeof(string));
            dttable.Columns.Add("Business", typeof(string));
            dttable.Columns.Add("Division", typeof(string));
            dttable.Columns.Add("Pool", typeof(string));
            dttable.Columns.Add("Function", typeof(string));
            dttable.Columns.Add("Type", typeof(string));
            dttable.Columns.Add("Overall Score (Average Score)", typeof(string));
            dttable.Columns.Add("Core Strengths (Average Score)", typeof(string));
            dttable.Columns.Add("Number of Skills", typeof(int));
            dttable.Columns.Add("Number of Skills Endorsed", typeof(int));
            dttable.Columns.Add("Nationality", typeof(string));
            dttable.Columns.Add("Country of Residence", typeof(string));
            foreach (var item in model.AllListData)
            {
                List<string> lstStrRow = new List<string>();
                lstStrRow.Add(string.Format("{0}{1}", item.FirstName, item.LastName));
                lstStrRow.Add(item.JobTitleName);
                lstStrRow.Add(item.CompanyName);
                lstStrRow.Add(item.BusinessName);
                lstStrRow.Add(item.DivisionName);
                lstStrRow.Add(item.PoolName);
                lstStrRow.Add(item.FunctionName);
                lstStrRow.Add(item.ResourceTypeName);
                lstStrRow.Add(null);
                lstStrRow.Add(null);
                lstStrRow.Add(null);
                lstStrRow.Add(null);
                lstStrRow.Add(item.NationalityName);
                lstStrRow.Add(null);
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

        public ActionResult HelpMeCalculate(HelpmecalculateviewModel model)
        {
            List<SelectListItem> data = new List<SelectListItem>();
            HelpmeCalculeteModel Details = new HelpmeCalculeteModel();
            int totalDays = 0;
            EmployeeEmploymentViewModel emodel = new EmployeeEmploymentViewModel();
            Details = _employeeMethod.GetPublicHolidayByContryId(model.StartDate, model.CountryId);
           
            int enti = Convert.ToInt32(model.FullTimeEntitlement);
            if (model.IncludePublicHolidays == "on")
            {
                
                //decimal contractdays = Details.totalWorkingDays - (Details.TotalHolidayYear + (Convert.ToInt16(model.FullTimeEntitlement) - Details.TotalHolidayYear));
                //decimal Accrualholidayrateperday = Math.Round((Convert.ToDecimal(model.FullTimeEntitlement) - Details.TotalHolidayYear) / contractdays, 2);
                //int Remainingholidays = Convert.ToInt16(Accrualholidayrateperday * Details.remainiingDays);
                //decimal RemainingHolidyasFromStatDate = Details.TotalRemainingHolidays;
                //totalDays = Convert.ToInt16(Remainingholidays + RemainingHolidyasFromStatDate);
                emodel.includeThisYear = HolidayIncludeContractDays(model.EmployeeID, enti, model.StartDate);
                if (emodel.includeThisYear > 0)
                {
                    data.Add(new SelectListItem { Text = emodel.includeThisYear.ToString(), Value = "holidaysThisYear" });
                }
                else
                {
                    data.Add(new SelectListItem { Text = totalDays.ToString(), Value = "holidaysThisYear" });
                }
                data.Add(new SelectListItem { Text = model.FullTimeEntitlement.ToString(), Value = "holidaysNextYear" });

            }
            else
            {
                //decimal contractdays = Details.totalWorkingDays - (Details.TotalHolidayYear + Convert.ToInt16(model.FullTimeEntitlement));
                //decimal Accrualholidayrateperday = Math.Round((Convert.ToDecimal(model.FullTimeEntitlement) - Details.TotalHolidayYear) / contractdays, 2);
                //decimal Remainingholidays = Accrualholidayrateperday * Details.remainiingDays;
                //decimal RemainingHolidyasFromStatDate = Details.TotalRemainingHolidays;
                //totalDays = Convert.ToInt16(Remainingholidays + RemainingHolidyasFromStatDate);
                //if (totalDays > 16)
                //{
                //    totalDays = totalDays + Convert.ToInt16(RemainingHolidyasFromStatDate);
                //}
                emodel.notincludeThisYear = HolidayNotIncludeContractDays(model.EmployeeID, enti);
                if (emodel.notincludeThisYear > 0)
                {
                    data.Add(new SelectListItem { Text = emodel.notincludeThisYear.ToString(), Value = "holidaysThisYear" });
                }
                else
                {
                    data.Add(new SelectListItem { Text = totalDays.ToString(), Value = "holidaysThisYear" });
                }


            }
            data.Add(new SelectListItem { Text = model.FullTimeEntitlement.ToString(), Value = "holidaysNextYear" });

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public Double HolidayNotIncludeContractDays(int ID, int holidayEn)
        {
            EmployeeEmploymentMethod _employeeEmploymentMethod = new EmployeeEmploymentMethod();
            var data = _employeeEmploymentMethod.getHolidayEntiAndPublicHoliday();
            Double publicHoliday = 0;
            Double holidayEnti = 0;
            Double contractDays = 0;
            double totalHoliday = 0;
            int year = _employeeEmploymentMethod.getEmployeeActiveYear();
            int endYear = _employeeEmploymentMethod.getEmployeeActiveEndYear();
            int startMonth = _employeeEmploymentMethod.getEmployeeActiveStartMonth();
            int endMonth = _employeeEmploymentMethod.getEmployeeActiveEndMonth();
            int dayInmonth = System.DateTime.DaysInMonth(endYear, endMonth);
            DateTime startdate = new DateTime(year, startMonth, 1);
            DateTime EndDate = new DateTime(endYear, endMonth, 31);
            double diff = (EndDate - startdate).TotalDays;
            int totalWeekend = counttotalWeekends(year, endYear, startMonth, endMonth);
            var publicHolidayCount = _employeeEmploymentMethod.getPublicHolidayByCountry();
            foreach (var item in publicHolidayCount)
            {
                int Year = DateTime.Now.Year;
                DateTime holidayYear = Convert.ToDateTime(item.PublicHolidayDate);
                if (year == holidayYear.Year)
                {
                    publicHoliday++;
                }
            }
            foreach (var item in data)
            {
                holidayEnti = holidayEn;
                contractDays = diff - publicHoliday - Convert.ToDouble(totalWeekend) - holidayEnti;
            }
            DateTime startDate = _employeeEmploymentMethod.getStartDateOfEmp(ID);
            int totalWeekendfromStart = counttotalWeekends(year, startDate.Year, startMonth, startDate.Month);
            double todayContractDay = (startDate - startdate).Days- Convert.ToDouble(totalWeekendfromStart);
            double remainingWorkDay = contractDays - todayContractDay;
            double holidayRateperDay = holidayEnti / contractDays;
            double remainingHoliday = holidayRateperDay * remainingWorkDay;
            double publicholiday = countHolidayfromStartDate(ID, EndDate, startdate);
            totalHoliday = Math.Round(remainingHoliday) + publicholiday;
            return totalHoliday;
        }

        public Double HolidayIncludeContractDays(int ID, int holidayEn,string StDate)
        {
             string inputFormat = "dd-MM-yyyy";
             EmployeeEmploymentMethod _employeeEmploymentMethod = new EmployeeEmploymentMethod();
            Double publicHoliday = 0;
            Double holidayEnti = 0;
            Double contractDays = 0;
            Double IncludedHoliday = 0;
            double totalHoliday = 0;
            int year = _employeeEmploymentMethod.getEmployeeActiveYear();
            int endYear = _employeeEmploymentMethod.getEmployeeActiveEndYear();
            int startMonth = _employeeEmploymentMethod.getEmployeeActiveStartMonth();
            int endMonth = _employeeEmploymentMethod.getEmployeeActiveEndMonth();
            int dayInmonth = System.DateTime.DaysInMonth(endYear, endMonth);
            DateTime startdate = new DateTime(year, startMonth, 1);
            DateTime EndDate = new DateTime(endYear, endMonth, 31);
            double diff = (EndDate - startdate).TotalDays;
            int totalWeekend = counttotalWeekends(year, endYear, startMonth, endMonth);
            var data = _employeeEmploymentMethod.getHolidayEntiAndPublicHoliday();
            var publicHolidayCount = _employeeEmploymentMethod.getPublicHolidayByCountry();
            foreach (var item in publicHolidayCount)
            {
                int Year = DateTime.Now.Year;
                DateTime holidayYear = Convert.ToDateTime(item.PublicHolidayDate);
                if (year == holidayYear.Year)
                {
                    publicHoliday++;
                }
            }
            foreach (var item in data)
            {
                //  holidayEnti = Convert.ToDouble(item.HolidayEntitlement);
                holidayEnti = holidayEn;
                IncludedHoliday = holidayEnti - publicHoliday;
                contractDays = diff - publicHoliday - Convert.ToDouble(totalWeekend) - IncludedHoliday;
            }
            if(!String.IsNullOrEmpty(StDate))
            {
                DateTime StartDate = DateTime.ParseExact(StDate, inputFormat, CultureInfo.InvariantCulture);
                int totalWeekendfromStart = counttotalWeekends(year, StartDate.Year, startMonth, StartDate.Month);
                double todayContractDay = (StartDate - startdate).Days - Convert.ToDouble(totalWeekendfromStart);
                double remainingWorkDay = contractDays - todayContractDay;
                double holidayRateperDay = IncludedHoliday / contractDays;
                double remainingHoliday = holidayRateperDay * remainingWorkDay;
                double publicholiday = countHolidayfromStartDate(ID, EndDate, StartDate);
                totalHoliday = Math.Round(remainingHoliday) + publicholiday;
            }
            return totalHoliday;
        }
        public int counttotalWeekends(int styear, int endyear, int startMonth, int endMonth)
        {
            int count = 0;
            for (int j = startMonth; j <= endMonth; j++)
            {
                DateTime EndOfMonth = new DateTime(styear, j, DateTime.DaysInMonth(styear, j));
                int day = EndOfMonth.Day;
                for (int i = 0; i <= day; i++)
                {
                    if (i >= 1)
                    {
                        DateTime d = new DateTime(styear, j, i);
                        if (d.DayOfWeek == DayOfWeek.Sunday || d.DayOfWeek == DayOfWeek.Saturday)
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }
        public double countHolidayfromStartDate(int ID, DateTime endDate,DateTime startDate)
        {
            EmployeeEmploymentMethod _employeeEmploymentMethod = new EmployeeEmploymentMethod();
            //DateTime startDate = _employeeEmploymentMethod.getStartDateOfEmp(ID);
            var publicHolidayCount = _employeeEmploymentMethod.getPublicHolidayByCountry();
            int count = 0;
            while (startDate.AddDays(1) <= endDate)
            {
                foreach (var data in publicHolidayCount)
                {
                    if (startDate == data.PublicHolidayDate.Value)
                    {
                        count++;
                    }
                }
                startDate = startDate.AddDays(1);
            }

            return count;
        }

        #region Task Temp
        public ActionResult EditTaskTemp(int Id)
        {
            var Listrecortd = _db.Employee_Task_Temp.Where(x => x.Id == Id).FirstOrDefault();
            AddNewTaskListViewModel model = new AddNewTaskListViewModel();
            model.Id = Listrecortd.Id;
            model.Title = Listrecortd.Title;
            List<AspNetUser> data = _AdminPearformanceMethod.getAllUserList().ToList();
            foreach (var item in data)
            {
                string Name = string.Format("{0} {1}", item.FirstName, item.LastName);
                if (Listrecortd.AssignTo == item.Id)
                {
                    model.AssignList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString(), Selected = true });
                }
                else
                {
                    model.AssignList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
                }
            }
            var Status = _otherSettingMethod.getAllSystemValueListByKeyName("Task Status");
            model.StatusList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in Status)
            {
                if (Listrecortd.Status == item.Id)
                {
                    model.StatusList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                }
                else
                {
                    model.StatusList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                }
            }
            model.Description = Listrecortd.Description;
            model.DueDate = String.Format("{0:dd-MM-yyy}", Listrecortd.DueDate);
            model.AlertBeforeDays = (int)Listrecortd.AlterBeforeDays;
            return PartialView("_PartialAddTask", model);
        }
        public ActionResult DeteteTaskTemp(int Id)
        {
            Employee_Task_Temp AddUser = _db.Employee_Task_Temp.Where(x => x.Id == Id).FirstOrDefault();
            AddUser.Archived = true;
            AddUser.LastModified = DateTime.Now;
            _db.SaveChanges();
            var listrecord = _employeeMethod.GetNewstaskrecord();
            var TempList = _employeeMethod.GetAllTaskRecordTemp();
            MainResoureViewModel model = new MainResoureViewModel();
            foreach (var item in listrecord)
            {
                AddNewTaskListViewModel m = new AddNewTaskListViewModel();
                m.Id = item.Id;
                m.Title = item.Title;
                m.Description = item.Description;
                m.Assign = item.AssignTo;
                m.AlertBeforeDays = (int)item.AlterBeforeDays;
                m.DueDate = String.Format("{0:dd-MM-yyy}", item.DueDate);
                model.NewsTasklistRecord.Add(m);
            }
            foreach (var item in TempList)
            {
                AddNewTaskListViewModel m = new AddNewTaskListViewModel();
                m.Id = item.Id;
                m.Title = item.Title;
                m.Description = item.Description;
                m.Assign = item.AssignTo;
                m.AlertBeforeDays = (int)item.AlterBeforeDays;
                m.DueDate = String.Format("{0:dd-MM-yyy}", item.DueDate);
                model.NewsTasklistRecord.Add(m);
            }

            return PartialView("_Partial_Step-5", model);
        }


        #endregion

    }
}