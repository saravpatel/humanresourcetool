using HRTool.CommanMethods.Admin;
using HRTool.CommanMethods.Resources;
using HRTool.DataModel;
using HRTool.Models.Resources;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Text.RegularExpressions;
using HRTool.CommanMethods;
using System.IO;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class EmployeeSkillsEndorsementController : Controller
    {
        #region
        EmployeeSkillsEndorsementMethod _EmployeeSkillsEndorsementMethod = new EmployeeSkillsEndorsementMethod();
        AdminPearformanceMethod _AdminPearformanceMethod = new AdminPearformanceMethod();
        EmployeeMethod _EmployeeMethod = new EmployeeMethod();
        #endregion
        EvolutionEntities _db = new EvolutionEntities();
        // GET: /EmployeeSkillsEndorsement/
        public ActionResult Index(int EmployeeId)
        {
            EmployeeSkillsEndorsementViewModel model = new EmployeeSkillsEndorsementViewModel();
            model.EmployeeId = EmployeeId;
            return View(model);
        }
        public EmployeeSkillsEndorsementViewModel returnList(int employeeId)
        {
            List<Employee_Skills> Technicalsdata = _EmployeeSkillsEndorsementMethod.getTechnicalAllList().Where(x => x.EmployeeId == employeeId).ToList();
            List<Employee_Skills> Generalsdata = _EmployeeSkillsEndorsementMethod.getGeneralAllList().Where(x=>x.EmployeeId == employeeId).ToList();
            EmployeeSkillsEndorsementViewModel model = new EmployeeSkillsEndorsementViewModel();
            foreach (var item in Technicalsdata)
            {
                EmployeeSkillsEndorsementViewModel Technical = new EmployeeSkillsEndorsementViewModel();
                Technical.TechnicalSkillsName = _EmployeeSkillsEndorsementMethod.getTechnicalAllList(Convert.ToInt32(item.TechnicalSkillsName));
                Technical.Id = item.Id;

                model.TechnicalSkillsList.Add(Technical);
            }
            foreach (var item in Generalsdata)
            {
                EmployeeSkillsEndorsementViewModel General = new EmployeeSkillsEndorsementViewModel();
                General.GeneralSkillsName = _EmployeeSkillsEndorsementMethod.getTechnicalAllList(Convert.ToInt32(item.GeneralSkillsName));
                General.Id = item.Id;
                model.GeneralSkillsList.Add(General);
            }
            model.TechnicalList = _EmployeeSkillsEndorsementMethod.BindTechnicalDropdown();
            model.GeneralList = _EmployeeSkillsEndorsementMethod.BindGeneralDropdown();
            return model;
        }
        public ActionResult SkilListRecord(int EmployeeId)
        {
            EmployeeSkillsEndorsementViewModel model = returnList(EmployeeId);
            return PartialView("_PartialSkillsEndorsmentList", model);
        }
        public ActionResult SaveTechnicalSkill(int Id, int EmployeeId)
        {

            _EmployeeSkillsEndorsementMethod.InsertTechnicalSkill(Id, EmployeeId,0);
            EmployeeSkillsEndorsementViewModel model = returnList(EmployeeId);
            return PartialView("_PartialSkillsEndorsmentList", model);
        }
        public ActionResult SaveGeneralSkill(int Id, int EmployeeId)
        {
            _EmployeeSkillsEndorsementMethod.InsertGeneralSkill(Id, EmployeeId,0);
            EmployeeSkillsEndorsementViewModel model = returnList(EmployeeId);
            return PartialView("_PartialSkillsEndorsmentList", model);
        }
        public ActionResult ViewSkillSTechnicalDetails(int TechanicalId)
        {
            ViewSkillsViewModel model = new ViewSkillsViewModel();
            var skilset = _EmployeeSkillsEndorsementMethod.GetSkillSetRecord(TechanicalId);
            var list = _EmployeeSkillsEndorsementMethod.GetViewSkillDetails(Convert.ToInt32(skilset.TechnicalSkillsName));
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
            return PartialView("_PartialViewSkillsDetails", model);
        }
        public ActionResult ViewSkillSGeneralDetails(int GeneralId)
        {
            ViewSkillsViewModel model = new ViewSkillsViewModel();
            var skilset = _EmployeeSkillsEndorsementMethod.GetSkillSetRecord(GeneralId);
            var list = _EmployeeSkillsEndorsementMethod.GetViewSkillDetails(Convert.ToInt32(skilset.GeneralSkillsName));
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
            return PartialView("_PartialViewSkillsDetails", model);
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
            return PartialView("_PartialAskForEndorsementView", model);
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
            return PartialView("_PartialViewSkillsDetails", model);
        }
        public ActionResult getEmplyeeUserList()
        {
            ViewSkillsViewModel model = new ViewSkillsViewModel();
            List<AspNetUser> data = _AdminPearformanceMethod.getAllUserList().ToList();
            foreach (var item in data)
            {
                string Name = string.Format("{0} {1} - {2}", item.FirstName, item.LastName, item.SSOID);
                model.EmployeesUserList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GeneralSetDetails(int GeneralId)
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
            return PartialView("_PartialViewSkillsDetails", model);
        }
        public List<ViewSkillsViewModel> returnEndrosementList(int EmployeeId)
        {
            List<Employee_AddEndrosementSkills> Endrosementdata = _EmployeeSkillsEndorsementMethod.getAllEndrosementList(EmployeeId).ToList();
            List<ViewSkillsViewModel> model = new List<ViewSkillsViewModel>();
            foreach (var item in Endrosementdata)
            {
                ViewSkillsViewModel m = new ViewSkillsViewModel();
                m.Name = item.SkillsName;
                var emp_name = _EmployeeMethod.getEmployeeById(item.EmployeeId);
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
                    modelComment.UserCreate = itemComment.UserIDCreatedBy;
                    m.commentList.Add(modelComment);
                }

                var aspList = _EmployeeMethod.GetAllEmployeeList();
                if (!string.IsNullOrEmpty(list.Picture))
                {
                    m.Picture = list.Picture;
                }
                if(aspList!=null)
                {
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
                                        if ((Aspitem.Id == Convert.ToInt32(Assignitem)) || (item.EmployeeId == Convert.ToInt32(Assignitem)))
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
                                    if ((Aspitem.Id == Convert.ToInt32(item.AssignSkillId)) || (item.EmployeeId == Convert.ToInt32(item.AssignSkillId)))
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
            }
            return model;
        }
        public ActionResult AssignEndrosementSkills(ViewSkillsViewModel model)
        {
            _EmployeeSkillsEndorsementMethod.SaveAssignEndrosementSet(model);
            List<ViewSkillsViewModel> modesllist = returnEndrosementList(model.EmployeeUserId);
            string[] userId = model.AssignUser.Split(',');
            var FromData = _db.AspNetUsers.Where(x => x.Id == SessionProxy.UserId).FirstOrDefault();
            var toUser = _db.AspNetUsers.Where(x => x.Id == model.EmployeeUserId).FirstOrDefault();
            if (userId.Length > 0)
            {
                for (int i = 0; i < userId.Length; i++)
                {
                    int UserId = Convert.ToInt32(userId[i]);
                    var cusruntUser = _db.AspNetUsers.Where(x => x.Id == UserId && x.Archived==false).FirstOrDefault();
               
                        HRTool.Models.MailModel mail = new HRTool.Models.MailModel();
                        mail.From = FromData.UserName;
                        mail.To = cusruntUser.UserName;
                        mail.Subject = "Resource Skill Endorsement";
                        mail.Header = "Resource Skill Endorsement";
                        string dateTimeEndorse = DateTime.Now.ToString("ddd, dd MMM yyyy");
                        //string dateTimeEndorse = date.ToString("ddd, dd MMM yyyy");
                        mail.EndorsementDate = dateTimeEndorse;
                        using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Template/ResourceSkillEndorcementMail.html")))
                        {
                            mail.Body = reader.ReadToEnd();
                        }
                        mail.Body = mail.Body.Replace("{0}", cusruntUser.FirstName);
                        string FromToName = string.Empty;
                        mail.Body = mail.Body.Replace("{1}", toUser.FirstName+" "+toUser.LastName);
                        mail.Body = mail.Body.Replace("{2}", model.MailURL);
                        string mailFromReceive = Common.sendMailWithoutAttachment(mail);
                  
                }
            }
            return PartialView("_PartialAddEndorsementSkillsList", modesllist);
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
            if (editlistrecord.AssignSkillId != null && editlistrecord.AssignSkillId != "")
            {
                if (editlistrecord.AssignSkillId.IndexOf(',') > 0)
                {
                    model.selectedemployee = editlistrecord.AssignSkillId.Split(',').ToList();
                }
                else
                {
                    if (!string.IsNullOrEmpty(editlistrecord.AssignSkillId))
                    {
                        string record = editlistrecord.AssignSkillId;
                        model.selectedemployee.Add(record);
                    }
                }
            }
            List<AspNetUser> data = _AdminPearformanceMethod.getAllUserList().ToList();
            model.EmployeesUserList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in data)
            {
                string Name = string.Format("{0} {1}", item.FirstName, item.LastName);
                model.EmployeesUserList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
            }
           // model.Comments = editlistrecord.Commnets;
            model.EndrosementId = editlistrecord.Id;
            return PartialView("_PartialEditAskForEndorsementSkillsSet", model);
        }
        public ActionResult EndorsementList(int EmployeeId)
        {
            List<ViewSkillsViewModel> modesllist = returnEndrosementList(EmployeeId);
            return PartialView("_PartialAddEndorsementSkillsList", modesllist);
        }
        public ActionResult AddComments(int Id)
        {
            ViewSkillsViewModel model = new ViewSkillsViewModel();
            model.EndrosementId = Id;
            return PartialView("_PartialAddEndrosmentComment", model);
        }
        public ActionResult SaveCommentsRecvords(ViewSkillsViewModel model)
        {
            model.CurrentUserId = SessionProxy.UserId;
            _EmployeeSkillsEndorsementMethod.SaveCommentRecords(model);
            List<ViewSkillsViewModel> modesllist = returnEndrosementList(model.EmployeeUserId);
            return PartialView("_PartialAddEndorsementSkillsList", modesllist);
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
            return PartialView("_PartialAddEndrosmentComment", model);
        }
        public ActionResult DeleteComment(int Id, int Employee)
        {
            int Users = SessionProxy.UserId;
            _EmployeeSkillsEndorsementMethod.Deletecommentrecord(Id, Users);
            List<ViewSkillsViewModel> modesllist = returnEndrosementList(Employee);
            return PartialView("_PartialAddEndorsementSkillsList", modesllist);
        }
        public ActionResult DeleteEndrosment(int Id, int Employee)
        {
            int Users = SessionProxy.UserId;
            _EmployeeSkillsEndorsementMethod.DeleteEndrosmentrecord(Id, Users);
            List<ViewSkillsViewModel> modesllist = returnEndrosementList(Employee);
            return PartialView("_PartialAddEndorsementSkillsList", modesllist);
        }
        public ActionResult DeleteGeneralSkills(int Id,int EmployeeID)
        {
            int Users = SessionProxy.UserId;
            _EmployeeSkillsEndorsementMethod.DeleteGeneralskills(0, EmployeeID, Id);
            EmployeeSkillsEndorsementViewModel model = returnList(EmployeeID);
            return PartialView("_PartialSkillsEndorsmentList", model);
        }
        public ActionResult DeleteTechnicalSkills(int Id,int EmployeeID)
        {
            int Users = SessionProxy.UserId;
            _EmployeeSkillsEndorsementMethod.DeleteTechnicalskills(0, EmployeeID, Id);
            EmployeeSkillsEndorsementViewModel model = returnList(EmployeeID);
            return PartialView("_PartialSkillsEndorsmentList", model);
        }
    }
}