using HRTool.CommanMethods.Admin;
using HRTool.CommanMethods.Resources;
using HRTool.DataModel;
using HRTool.Models.Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Text.RegularExpressions;
using HRTool.CommanMethods.Settings;
using HRTool.CommanMethods;
using System.IO;
using System.Globalization;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class AdminSkillEndrosmentController : Controller
    {
        //
        // GET: /AdminSkillEndrosment/
        #region
        EvolutionEntities _db = new EvolutionEntities();
        EmployeeSkillsEndorsementMethod _EmployeeSkillsEndorsementMethod = new EmployeeSkillsEndorsementMethod();
        AdminPearformanceMethod _AdminPearformanceMethod = new AdminPearformanceMethod();
        EmployeeMethod _EmployeeMethod = new EmployeeMethod();
        CompanyStructureMethod _CompanyStructureMethod = new CompanyStructureMethod();
                   
        #endregion
        public ActionResult Index(string EmployeeId)
        {
            EmployeeSkillsEndorsementViewModel model = new EmployeeSkillsEndorsementViewModel();
            model.EmployeeId = SessionProxy.UserId;
            var BusinessList = _CompanyStructureMethod.getAllBusinessList();
            model.BusinessList.Add(new SelectListItem() { Text = "All", Value = "0" });
            foreach (var itemBussiness in BusinessList)
            {
                model.BusinessList.Add(new SelectListItem() { Text = itemBussiness.Name, Value = itemBussiness.Id.ToString() });
            }
            var division = _CompanyStructureMethod.getAllDivisionList();
            model.DivisionList.Add(new SelectListItem() { Text = "All", Value = "0" });
            foreach (var itemdivision in division)
            {
                model.DivisionList.Add(new SelectListItem() { Text = itemdivision.Name, Value = itemdivision.Id.ToString() });
            }
            var Pool = _CompanyStructureMethod.getAllPoolsList();
            model.PoolList.Add(new SelectListItem() { Text = "All", Value = "0" });
            foreach (var itempool in Pool)
            {
                model.PoolList.Add(new SelectListItem() { Text = itempool.Name, Value = itempool.Id.ToString() });
            }
            var Funcation = _CompanyStructureMethod.getAllPoolsList();
            model.FunctionList.Add(new SelectListItem() { Text = "All", Value = "0" });
            foreach (var itemFuncation in Funcation)
            {
                model.FunctionList.Add(new SelectListItem() { Text = itemFuncation.Name, Value = itemFuncation.Id.ToString() });
            }
            var SkillsSet = _EmployeeSkillsEndorsementMethod.getSkillsAllList();
            model.AllSkillSetList.Add(new SelectListItem() { Text = "All", Value = "0" });
            foreach (var SkillsSetList in SkillsSet)
            {
                model.AllSkillSetList.Add(new SelectListItem() { Text = SkillsSetList.Name, Value = SkillsSetList.Id.ToString() });
            }
            List<AspNetUser> data = _AdminPearformanceMethod.getAllUserList().ToList();
            model.AllResourceList.Add(new SelectListItem() { Text = "All", Value = "0" });
            foreach (var item in data)
            {
                string Name = string.Format("{0} {1} - {2}", item.FirstName, item.LastName,item.SSOID);
                model.AllResourceList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
            }
            List<SkillSet> Technicalsdata = _EmployeeSkillsEndorsementMethod.getTechnicalSetAllList().ToList();
            List<SkillSet> Generalsdata = _EmployeeSkillsEndorsementMethod.getGeneralSetAllList().ToList();
            
            return View(model);
        }
        public ActionResult AskForEndorsement()
        {
            List<SkillSet> Technicalsdata = _EmployeeSkillsEndorsementMethod.getTechnicalSetAllList().ToList();
            List<SkillSet> Generalsdata = _EmployeeSkillsEndorsementMethod.getGeneralSetAllList().ToList();
            ViewSkillsViewModel model = new ViewSkillsViewModel();
            string FilePath = ConfigurationManager.AppSettings["SkillSetFilePath"].ToString();
            foreach (var item in Technicalsdata)
            {
                ViewSkillsViewModel Technical = new ViewSkillsViewModel();
                Technical.Name = item.Name;
                Technical.Id = item.Id;
                if (!string.IsNullOrEmpty(item.Picture))
                {
                    Technical.Picture = item.Picture;
                }
                model.TechnicalSkillSet.Add(Technical);
            }
            foreach (var item in Generalsdata)
            {
                ViewSkillsViewModel General = new ViewSkillsViewModel();
                General.Name = item.Name;
                General.Id = item.Id;
                if (!string.IsNullOrEmpty(item.Picture))
                {
                    General.Picture = item.Picture;
                }
                model.GeneralSkillSet.Add(General);
            }
            List<AspNetUser> data = _AdminPearformanceMethod.getAllUserList().ToList();
            model.EmployeesUserList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in data)
            {
                string Name = string.Format("{0} {1}", item.FirstName, item.LastName);
                model.EmployeesUserList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
            }
            return PartialView("_PartialAdminPartialAskForEndorsementView", model);
        }

        public ActionResult getEmployeeData()
        {
            ViewSkillsViewModel model = new ViewSkillsViewModel();
            int empId = SessionProxy.UserId;
            var admindata = _db.AspNetUserRoles.Where(x => x.UserId == empId && x.RoleId == 1).FirstOrDefault();
            if(admindata!=null)
            {
                List<AspNetUser> data = _AdminPearformanceMethod.getAllUserList().ToList();
                foreach (var item in data)
                {
                    string Name = string.Format("{0} {1} - {2}", item.FirstName, item.LastName, item.SSOID);
                    model.EmployeesUserList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
                }
            }
            else
            {
                foreach (var item in _EmployeeSkillsEndorsementMethod.getReportToEmployeeSkill(SessionProxy.UserId))
                {
                    model.EmployeesUserList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.ssoId, Value = item.EmployeeId.ToString() });
                }
            }          
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AskSomeBodyEndrosment()
        {
            List<SkillSet> Technicalsdata = _EmployeeSkillsEndorsementMethod.getTechnicalSetAllList().ToList();
            List<SkillSet> Generalsdata = _EmployeeSkillsEndorsementMethod.getGeneralSetAllList().ToList();
            ViewSkillsViewModel model = new ViewSkillsViewModel();
            string FilePath = ConfigurationManager.AppSettings["SkillSetFilePath"].ToString();
            foreach (var item in Technicalsdata)
            {
                ViewSkillsViewModel Technical = new ViewSkillsViewModel();
                Technical.Name = item.Name;
                Technical.Id = item.Id;
                if (!string.IsNullOrEmpty(item.Picture))
                {
                    Technical.Picture = item.Picture;
                }
                model.TechnicalSkillSet.Add(Technical);
            }
            foreach (var item in Generalsdata)
            {
                ViewSkillsViewModel General = new ViewSkillsViewModel();
                General.Name = item.Name;
                General.Id = item.Id;
                if (!string.IsNullOrEmpty(item.Picture))
                {
                    General.Picture = item.Picture;
                }
                model.GeneralSkillSet.Add(General);
            }
            List<AspNetUser> data = _AdminPearformanceMethod.getAllUserList().ToList();
            model.EmployeesUserList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in data)
            {
                string Name = string.Format("{0} {1}", item.FirstName, item.LastName);
                model.EmployeesUserList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
            }
            return PartialView("_PartialAdminEditAskForEndorsementSkillsSet", model);
        }
        public ActionResult getGkillData(int id)
        {
            ViewSkillsViewModel model = new ViewSkillsViewModel();
            string FilePath = ConfigurationManager.AppSettings["SkillSetFilePath"].ToString();
            List<SkillSet> Technicaldata = _EmployeeSkillsEndorsementMethod.getTechnicalSkillData().ToList();
            List<SkillSet> GenralData = _EmployeeSkillsEndorsementMethod.getGeneralSkillData().ToList();
            if (id == 0)
            {
                foreach (var item in Technicaldata)
                {
                    ViewSkillsViewModel Technical = new ViewSkillsViewModel();
                    Technical.Name = item.Name;
                    Technical.Id = item.Id;
                    if (!string.IsNullOrEmpty(item.Picture))
                    {
                        Technical.Picture = item.Picture;
                    }
                    //Technical.SelectedSkills = true;
                    model.GeneralSkillSet.Add(Technical);

                }
            }
             else if (id == 1)
            {
                foreach (var item in GenralData)
                {
                    ViewSkillsViewModel Genral = new ViewSkillsViewModel();
                    Genral.Name = item.Name;
                    Genral.Id = item.Id;
                    if (!string.IsNullOrEmpty(item.Picture))
                    {
                        Genral.Picture = item.Picture;
                    }
                    //Genral.SelectedSkills = true;
                    model.GeneralSkillSet.Add(Genral);

                }
            }
            return PartialView("_PartialSkillData",model);
        }
        public ActionResult TechnicalSetDetails(int TechanicalId)
        {
            ViewSkillsViewModel model = new ViewSkillsViewModel();
            var list = _EmployeeSkillsEndorsementMethod.GetViewSkillDetails(Convert.ToInt32(TechanicalId));
            string FilePath = ConfigurationManager.AppSettings["SkillSetFilePath"].ToString();
            if (!string.IsNullOrEmpty(list.Picture))
            {
                model.Picture = list.Picture;
            }
                if (list.TechnicalSkillsCSV.IndexOf(',') > 0)
                {
                    var skillName = list.TechnicalSkillsCSV.Split(',').ToList();
                    foreach (var item in skillName)
                    {
                        var record = _EmployeeSkillsEndorsementMethod.GetNameSkills(Convert.ToInt32(item));
                        model.SkillValueList.Add(record.Value);
                    }
                
            } 
            else
            {
                if (!string.IsNullOrEmpty(list.TechnicalSkillsCSV))
                {
                    var record = _EmployeeSkillsEndorsementMethod.GetNameSkills(Convert.ToInt32(list.TechnicalSkillsCSV));
                    model.SkillValueList.Add(record.Value);
                }

            }
            model.Description = list.Description;
            model.Name = list.Name;
            return PartialView("_PartialAdminPartialViewSkillsDetails", model);
        }
        public ActionResult GeneralSetDetails(int GeneralId,int? flag)
        {
            ViewSkillsViewModel model = new ViewSkillsViewModel();
            var list = _EmployeeSkillsEndorsementMethod.GetViewSkillDetails(Convert.ToInt32(GeneralId));
            string FilePath = ConfigurationManager.AppSettings["SkillSetFilePath"].ToString();
            if (!string.IsNullOrEmpty(list.Picture))
            {
                model.Picture = list.Picture;
            }
            if (list.GeneralSkillsCSV.IndexOf(',') > 0)
            {
                var skillName = list.GeneralSkillsCSV.Split(',').ToList();
                foreach (var item in skillName)
                {
                    var record = _EmployeeSkillsEndorsementMethod.GetNameSkills(Convert.ToInt32(item));
                    model.SkillValueList.Add(record.Value);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(list.GeneralSkillsCSV))
                {
                    var record = _EmployeeSkillsEndorsementMethod.GetNameSkills(Convert.ToInt32(list.GeneralSkillsCSV));
                    model.SkillValueList.Add(record.Value);
                }
            }
            model.Description = list.Description;
            model.Name = list.Name;
            return PartialView("_PartialAdminPartialViewSkillsDetails", model);
        }
        public List<ViewSkillsViewModel> returnEndrosementList()
        {
            List<Employee_AddEndrosementSkills> Endrosementdata = _EmployeeSkillsEndorsementMethod.getAllUserEndrosementList().ToList();
            List<ViewSkillsViewModel> model = new List<ViewSkillsViewModel>();
            foreach (var item in Endrosementdata)
            {
                ViewSkillsViewModel m = new ViewSkillsViewModel();
                m.Name = item.SkillsName;
                m.skillsId = (Convert.ToInt32(item.SkilsId));
                var emp_name = _EmployeeMethod.getEmployeeById(item.EmployeeId);
                if (item.EmployeeId != null)
                {
                    m.EmployeeUserId = item.EmployeeId.Value;
                }
                m.EmpolyeeName = string.Format("{0} {1}", emp_name.FirstName, emp_name.LastName);
                m.CreateDate = String.Format("{0:dd-MMM-yyy}", item.CreatedDate);
                m.EndrosementId = item.Id;
                m.CommentCount = _EmployeeSkillsEndorsementMethod.GetCommentCount(item.Id);
                string FilePath = ConfigurationManager.AppSettings["SkillSetFilePath"].ToString();
                var list = _EmployeeSkillsEndorsementMethod.GetViewSkillDetails(Convert.ToInt32(item.SkilsId));
                var listComment = _EmployeeSkillsEndorsementMethod.GetCommentList(item.Id);
                foreach (var itemComment in listComment)
                {
                    commenlistrecordviewModel modelComment = new commenlistrecordviewModel();
                    if (itemComment.Comments != null)
                    {
                        //string noHTML = Regex.Replace(itemComment.Comments, @"<[^>]+>| ", "").Trim();
                        modelComment.Comments = itemComment.Comments;
                    }
                    modelComment.Id = itemComment.Id;
                    //modelComment.ProfileImagePath = _EmployeeMethod.getEmployeeById(itemComment.UserIDCreatedBy).image;
                    modelComment.UserCreate = itemComment.UserIDCreatedBy;
                    m.commentList.Add(modelComment);
                }
                var aspList = _EmployeeMethod.GetAllEmployeeList();
                if (!string.IsNullOrEmpty(list.Picture))
                {
                    m.Picture = list.Picture;
                }
                foreach (var Aspitem in aspList)
                {

                    if (item.AssignSkillId != null && item.AssignSkillId != "")
                    {
                        if (item.AssignSkillId.IndexOf(",") > 0)

                        {
                            foreach (var Assignitem in item.AssignSkillId.Split(','))
                            {

                                if (Assignitem != "" && Assignitem != null)
                                {
                                    if (Aspitem.Id == Convert.ToInt32(Assignitem))
                                    {
                                        var Assign_name = _EmployeeMethod.getEmployeeById(Convert.ToInt32(Assignitem));
                                        m.AssignEmployeeName = string.Format("{0} {1}", Assign_name.FirstName, Assign_name.LastName);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(item.AssignSkillId))
                            {
                                if (Aspitem.Id == Convert.ToInt32(item.AssignSkillId))
                                {
                                    var Assign_name = _EmployeeMethod.getEmployeeById(Convert.ToInt32(item.AssignSkillId));
                                    m.AssignEmployeeName = string.Format("{0} {1}", Assign_name.FirstName, Assign_name.LastName);
                                }
                            }
                        }
                    }
                }
                model.Add(m);
            }
            return model;
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
        public ActionResult AssignEndrosementSkillsPreview(ViewSkillsViewModel model)
        {
            var EmployeeEndorseData = _db.AspNetUsers.Where(x => x.Id == model.EmployeeUserId && x.Archived == false).FirstOrDefault();
            PreviewEmployeeEndorseData EmpModel = new PreviewEmployeeEndorseData();
            if (EmployeeEndorseData!=null)
            {
                EmpModel.EmpId = EmployeeEndorseData.Id;
                EmpModel.EmpName = EmployeeEndorseData.FirstName + " " + EmployeeEndorseData.LastName + " - " + EmployeeEndorseData.SSOID;
                //var empskillId = _db.Employee_Skills.Where(x => x.EmployeeId == model.EmployeeUserId).FirstOrDefault();
                //if(empskillId!=null)
                //{                    
                //    int genralSkillId = Convert.ToInt32(empskillId.GeneralSkillsName);
                //    int technicalSkill = Convert.ToInt32(empskillId.TechnicalSkillsName);
                //    EmpModel.EmpGenrelSkill = _db.SkillSets.Where(x => x.Id == genralSkillId && x.Archived==false).Select(x => x.Name).FirstOrDefault();
                //    EmpModel.EmpTechnicalSkill= _db.SkillSets.Where(x => x.Id == technicalSkill && x.Archived == false).Select(x => x.Name).FirstOrDefault();                   
                //}
                if(model.Id!=null && model.Id!=0)
                {
                    var skill = _db.SkillSets.Where(x => x.Id == model.Id && x.Archived==false).FirstOrDefault();
                    EmpModel.EmpGenrelSkill = skill.Name;
                }
                EmpModel.comment = model.Comments;
                EmpModel.EmpImage = EmployeeEndorseData.image;
            }
            string[] recipientId = model.AssignUser.Split(',');
            for(int i=0;i<recipientId.Length;i++)
            {
                EndorsmentRecipientsList resiModel = new EndorsmentRecipientsList();
                if (recipientId[i] != null && recipientId[i] != "")
                {
                    int eid = Convert.ToInt32(recipientId[i]);
                    var recipientDetail = _EmployeeSkillsEndorsementMethod.getRecipientDetail(eid).FirstOrDefault();
                    if (recipientDetail != null)
                    {
                        resiModel.RepId = recipientDetail.RecipirntId;
                        resiModel.RepName = recipientDetail.ReciFirstName + " " + recipientDetail.ReciLastName;
                        resiModel.RepImage = recipientDetail.ReciImage;
                        resiModel.RepJobTitle = recipientDetail.ReciSSOID + " - " + recipientDetail.ReciJobTitle;
                        resiModel.RepBusiness = recipientDetail.ReciBusiness;
                        resiModel.RepDivision = recipientDetail.ReciDivision;
                        resiModel.RepPool = recipientDetail.ReciPool;
                        resiModel.RepFunction = recipientDetail.asReciFunction;
                    }
                }
                    EmpModel.recepientList.Add(resiModel);
                
            }
            //_EmployeeSkillsEndorsementMethod.SaveAssignEndrosementSet(model);
            //List<ViewSkillsViewModel> modesllist = returnEndrosementList();
            return PartialView("_PartialAdminSkillEndorsementRecipientPriview", EmpModel);
        }
        
        public ActionResult AssignEndrosementSkills(ViewSkillsViewModel model)
        {
            _EmployeeSkillsEndorsementMethod.SaveAssignEndrosementSet(model);
            List<ViewSkillsViewModel> modesllist = returnEndrosementList();
            var FromData = _db.AspNetUsers.Where(x => x.Id == model.EmployeeUserId && x.Archived == false).FirstOrDefault();
            int FromId = Convert.ToInt32(model.EmployeeUserId);
            HRTool.Models.MailModel mail = new HRTool.Models.MailModel();
            mail.From = FromData.UserName;
            mail.To = FromData.UserName;
            mail.Subject = "Skill Endorsement";
            mail.Header = FromData.FirstName + " " + FromData.LastName;
            string inputFormat = "ddd, dd MMM yyyy";
            string dateTimeEndorse = DateTime.Now.ToString("ddd, dd MMM yyyy");
            mail.EndorsementDate = dateTimeEndorse;
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/MailFromTemplate.html")))
            {
                mail.Body = reader.ReadToEnd();
            }
            mail.Body = mail.Body.Replace("{0}", mail.Header);
            string FromToName = string.Empty;
            if(model.AssignUser!=null && model.AssignUser!="")
            {
                string[] FromrecipientId = model.AssignUser.Split(',');                
                for (int i=0;i<FromrecipientId.Length;i++)
                {
                    if (FromrecipientId[i] != null && FromrecipientId[i] != "")
                    {
                        int ToId = Convert.ToInt32(FromrecipientId[i]);
                        var toData = _db.AspNetUsers.Where(x => x.Id == ToId && x.Archived == false).FirstOrDefault();
                        FromToName = FromToName + " " + toData.FirstName + " " + toData.LastName;
                    }
                }
            }
            mail.Body = mail.Body.Replace("{1}", FromToName);
            mail.Body = mail.Body.Replace("{2}", dateTimeEndorse);            
            string mailFromReceive = Common.sendMail(mail);
            string[] recipientId = model.AssignUser.Split(',');
            for (int i = 0; i < recipientId.Length; i++)
            {
                if (recipientId[i] != "" && recipientId[i] != null)
                {
                    using (StreamReader stramReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/MailToTemplate.html")))
                    {
                        mail.Body = stramReader.ReadToEnd();
                    }
                    int ToId = Convert.ToInt32(recipientId[i]);
                    var toData = _db.AspNetUsers.Where(x => x.Id == ToId && x.Archived == false).FirstOrDefault();
                    mail.From = FromData.UserName;
                    mail.To = toData.UserName;
                    mail.Subject = "Skill Endorsement";
                    mail.Body = mail.Body.Replace("{0}", toData.FirstName + " " + toData.LastName);
                    mail.Body = mail.Body.Replace("{1}", FromData.FirstName + " " + FromData.LastName);
                    string mailToReceive = Common.sendMail(mail);
                }
            }
            return PartialView("_PartialAdminPartialAddEndorsementSkillsList", modesllist);
        }
        public ActionResult GetEditAssignEndrosementSkills(int Id)
        {
            int Users = SessionProxy.UserId;
            ViewSkillsViewModel model = new ViewSkillsViewModel();
            List<SkillSet> Technicalsdata = _EmployeeSkillsEndorsementMethod.getTechnicalSetAllList().ToList();
            List<SkillSet> Generalsdata = _EmployeeSkillsEndorsementMethod.getGeneralSetAllList().ToList();
            var editlistrecord = _EmployeeSkillsEndorsementMethod.getEditAllEndrosementList(Id);
            string FilePath = ConfigurationManager.AppSettings["SkillSetFilePath"].ToString();
            foreach (var item in Technicalsdata)
            {
                if (item.Id == editlistrecord.SkilsId)
                {
                    ViewSkillsViewModel Technical = new ViewSkillsViewModel();
                    Technical.Name = item.Name;
                    Technical.Id = item.Id;
                    if (!string.IsNullOrEmpty(item.Picture))
                    {
                        Technical.Picture = item.Picture;
                    }
                    Technical.SelectedSkills = true;
                    model.TechnicalSkillSet.Add(Technical);

                }
                else
                {
                    ViewSkillsViewModel Technical = new ViewSkillsViewModel();
                    Technical.Name = item.Name;
                    Technical.Id = item.Id;
                    if (!string.IsNullOrEmpty(item.Picture))
                    {
                        Technical.Picture = item.Picture;
                    }
                    Technical.SelectedSkills = false;
                    model.TechnicalSkillSet.Add(Technical);

                }
            }
            foreach (var item in Generalsdata)
            {
                if (item.Id == editlistrecord.SkilsId)
                {
                    ViewSkillsViewModel General = new ViewSkillsViewModel();
                    General.Name = item.Name;
                    General.Id = item.Id;
                    if (!string.IsNullOrEmpty(item.Picture))
                    {
                        General.Picture = item.Picture;
                    }
                    General.SelectedSkills = true;
                    model.GeneralSkillSet.Add(General);
                }
                else
                {
                    ViewSkillsViewModel General = new ViewSkillsViewModel();
                    General.Name = item.Name;
                    General.Id = item.Id;
                    if (!string.IsNullOrEmpty(item.Picture))
                    {
                        General.Picture = item.Picture;
                    }
                    General.SelectedSkills = false;
                    model.GeneralSkillSet.Add(General);
                }

            }
            model.TechnicalSkillsCSV = editlistrecord.AssignSkillId;
            if(editlistrecord.AssignSkillId!=null)
            {
                string name = string.Empty;
                string SelectEmpId = string.Empty;
                string[] empName = editlistrecord.AssignSkillId.Split(',');
                for (int i=0;i<empName.Length;i++)
                {
                    if (empName[i] != null && empName[i] != "")
                    {
                        int eid = Convert.ToInt32(empName[i]);

                        var ename = _db.AspNetUsers.Where(x => x.Id == eid && x.Archived == false).FirstOrDefault();
                        name = ename.FirstName + " " + ename.LastName + " - " + ename.SSOID + "," + name;
                        SelectEmpId = ename.Id + "," + SelectEmpId;
                        model.selectEmpName = name;
                        model.selectEmpId = SelectEmpId;
                    }
                }
            }
            //if (editlistrecord.AssignSkillId.IndexOf(',') > 0)
            //{
            //    model.selectedemployee = editlistrecord.AssignSkillId.Split(',').ToList();
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(editlistrecord.AssignSkillId))
            //    {
            //        string record = editlistrecord.AssignSkillId;
            //        model.selectedemployee.Add(record);
            //    }
            //}
            List<AspNetUser> data = _AdminPearformanceMethod.getAllUserList().ToList();
            model.EmployeesUserList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in data)
            {
                string Name = string.Format("{0} {1}", item.FirstName, item.LastName);
                model.EmployeesUserList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
            }
            //model.Comments = editlistrecord.Commnets;
            model.EndrosementId = editlistrecord.Id;
            model.flag = 1;
            return PartialView("_PartialAdminEditAskForEndorsementSkillsSet", model);
        }
        public ActionResult EndorsementList()
        {
            List<ViewSkillsViewModel> modesllist = returnEndrosementList();
            return PartialView("_PartialAdminPartialAddEndorsementSkillsList", modesllist);
        }

        public ActionResult AddComments(int Id)
        {
            ViewSkillsViewModel model = new ViewSkillsViewModel();
            model.EndrosementId = Id;
            return PartialView("_PartialAdminPartialAddEndrosmentComment", model);
        }
        public ActionResult SaveCommentsRecvords(ViewSkillsViewModel model)
        {
            model.CurrentUserId = SessionProxy.UserId;
            _EmployeeSkillsEndorsementMethod.SaveCommentRecords(model);
            List<ViewSkillsViewModel> modesllist = returnEndrosementList();
            return PartialView("_PartialAdminPartialAddEndorsementSkillsList", modesllist);
        }
        public ActionResult EditCommentsRecord(int Id)
        {
            int Users = SessionProxy.UserId;
            ViewSkillsViewModel model = new ViewSkillsViewModel();
            var record = _EmployeeSkillsEndorsementMethod.Editcomments(Id);
            foreach (var item in record)
            {
                model.Id = item.Id;
                model.Comments = item.Comments;
                model.EndrosementId = (int)item.EndrosementId;
            }
            return PartialView("_PartialAdminPartialAddEndrosmentComment", model);
        }
        public ActionResult DeleteComment(int Id)
        {
            int Users = SessionProxy.UserId;
            _EmployeeSkillsEndorsementMethod.Deletecommentrecord(Id, Users);
            List<ViewSkillsViewModel> modesllist = returnEndrosementList();
            return PartialView("_PartialAdminPartialAddEndorsementSkillsList", modesllist);
        }
        public ActionResult DeleteEndrosment(int Id)
        {
            int Users = SessionProxy.UserId;
            _EmployeeSkillsEndorsementMethod.DeleteEndrosmentrecord(Id, Users);
            List<ViewSkillsViewModel> modesllist = returnEndrosementList();
            return PartialView("_PartialAdminPartialAddEndorsementSkillsList", modesllist);
        }
        public ActionResult FilterRecords(int SkillsSet, int Resourece, int Bussiness, int Pool, int Funcation)
        {
            List<ViewSkillsViewModel> modesllist = returnEndrosementList();
            if (SkillsSet != 0)
            {
                modesllist = modesllist.Where(x => x.skillsId == SkillsSet).ToList();
            }
            if (Resourece != 0)
            {
                modesllist = modesllist.Where(x => x.EmployeeUserId == Resourece).ToList();
            }
            return PartialView("_PartialAdminPartialAddEndorsementSkillsList", modesllist);
        }
    }
}