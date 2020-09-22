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
using HRTool.CommanMethods.Settings;
using HRTool.CommanMethods;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class MeEmployeeSkillsEndorsementController : Controller
    {
        #region
        EmployeeSkillsEndorsementMethod _EmployeeSkillsEndorsementMethod = new EmployeeSkillsEndorsementMethod();
        AdminPearformanceMethod _AdminPearformanceMethod = new AdminPearformanceMethod();
        EmployeeMethod _EmployeeMethod = new EmployeeMethod();
        EvolutionEntities _db = new EvolutionEntities();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        #endregion
        // GET: /EmployeeSkillsEndorsement/
        public ActionResult Index(int EmployeeId)
        {
            EmployeeSkillsEndorsementViewModel model = new EmployeeSkillsEndorsementViewModel();
            model.EmployeeId = EmployeeId;
            var jobtitle = _otherSettingMethod.getAllSystemValueListByKeyName("Job Title List");
            model.JobTitleList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in jobtitle)
            {
                model.JobTitleList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            var skillSet = _db.SkillSets.ToList();
            model.SkillsetList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            model.PoolList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var skills in skillSet)
            {
                model.SkillsetList.Add(new SelectListItem() { Text = skills.Name, Value = skills.Id.ToString() });
            }
            var rData = _db.AspNetUsers.Where(x => x.Id == EmployeeId && x.Archived == false).ToList();
            if (rData.Count > 0)
            {
                foreach (var item in rData)
                {
                    if (item.CustomerCareID != null)
                    {
                        string[] custId = item.CustomerCareID.Split(',');
                        for (int i = 0; i < custId.Length; i++)
                        {
                            int cstId = Convert.ToInt32(custId[i]);
                            if (rData.FirstOrDefault().CustomerCareID != null)
                            {
                                var Resuorcedata = _db.AspNetUsers.Where(x => x.Id == cstId).ToList();
                                if (Resuorcedata.Count > 0)
                                {
                                    string Name = string.Format("{0} {1} - {2}", Resuorcedata.FirstOrDefault().FirstName, Resuorcedata.FirstOrDefault().LastName, Resuorcedata.FirstOrDefault().SSOID);
                                    model.AllResourceList.Add(new SelectListItem() { Text = Name, Value = cstId.ToString() });
                                }
                                var poollist = _AdminPearformanceMethod.getResourcePool(cstId);
                                if (poollist.Count > 0)
                                {
                                    model.PoolList.Add(new SelectListItem() { Text = poollist.FirstOrDefault().Name, Value = poollist.FirstOrDefault().Id.ToString() });
                                }
                            }
                        }
                        //model.PoolListDis = model.PoolList.DistinctBy(p => new { p.Id, p.Name }).ToList();
                        // model.PoolListDis = model.PoolList.Select(s => s.Value).Distinct().ToList();

                        model.PoolListDis = model.PoolList.GroupBy(p => p.Text).Select(g => g.FirstOrDefault()).ToList();
                    }
                }
            }
            return View(model);
        }
        public EmployeeSkillsEndorsementViewModel returnList()
        {
            List<Employee_Skills> Technicalsdata = _EmployeeSkillsEndorsementMethod.getTechnicalAllList().Where(x => x.EmployeeId == SessionProxy.UserId).ToList();
            List<Employee_Skills> Generalsdata = _EmployeeSkillsEndorsementMethod.getGeneralAllList().Where(x => x.EmployeeId == SessionProxy.UserId).ToList();
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
        public ActionResult SkilListRecord()
        {
            EmployeeSkillsEndorsementViewModel model = returnList();
            return PartialView("_PartialSkillsEndorsmentList", model);
        }
        public ActionResult SaveTechnicalSkill(int Id, int EmployeeId,int TechID)
        {

            _EmployeeSkillsEndorsementMethod.InsertTechnicalSkill(Id, EmployeeId, TechID);
            EmployeeSkillsEndorsementViewModel model = returnList();
            return PartialView("_PartialSkillsEndorsmentList", model);
        }
        public ActionResult SaveGeneralSkill(int Id, int EmployeeId, int GeneralID)
        {
            _EmployeeSkillsEndorsementMethod.InsertGeneralSkill(Id, EmployeeId, GeneralID);
            EmployeeSkillsEndorsementViewModel model = returnList();
            return PartialView("_PartialSkillsEndorsmentList", model);
        }
        public ActionResult GetDetailOfGeneralDetail(int id, int EmployeeId)
        {
            EmployeeSkillsEndorsementViewModel model = returnListForgeneralSkill(id, EmployeeId);
            return PartialView("_PartialAddGeneralSkillEndorsement", model);

        }
        public EmployeeSkillsEndorsementViewModel returnListForgeneralSkill(int Id, int EmployeeId)
        {
            List<Employee_Skills> Generalsdata = new List<Employee_Skills>();
            EmployeeSkillsEndorsementViewModel model = new EmployeeSkillsEndorsementViewModel();
     
            if (Id == 0)
            {
                Generalsdata= _EmployeeSkillsEndorsementMethod.getGeneralAllList().Where(x => x.EmployeeId == SessionProxy.UserId && x.Id == Id).ToList();
            }
            else
            {
                Generalsdata= _EmployeeSkillsEndorsementMethod.getGeneralAllList().Where(x => x.EmployeeId == SessionProxy.UserId && x.Id == Id).ToList();

                model.Id = Convert.ToInt32(Generalsdata.FirstOrDefault().GeneralSkillsName);
            }
            foreach (var item in Generalsdata)
            {
                EmployeeSkillsEndorsementViewModel General = new EmployeeSkillsEndorsementViewModel();
                General.GeneralSkillsName = _EmployeeSkillsEndorsementMethod.getTechnicalAllList(Convert.ToInt32(item.GeneralSkillsName));
                General.Id = item.Id;
                model.GeneralSkillsList.Add(General);
            }
            model.GeneralList = _EmployeeSkillsEndorsementMethod.BindGeneralDropdown();

            return model;
        }
        public ActionResult GetDetailOfTechnicalDetail(int Id, int EmployeeId)
        {
            EmployeeSkillsEndorsementViewModel model = returnListForTechnicalSkill(Id, EmployeeId);
            return PartialView("_PartialAddTechnicalSkillEndorsement", model);
        }
        public EmployeeSkillsEndorsementViewModel returnListForTechnicalSkill(int Id, int EmployeeId)
        {
            EmployeeSkillsEndorsementViewModel model = new EmployeeSkillsEndorsementViewModel();
            List<Employee_Skills> Technicalsdata = new List<Employee_Skills>();
            if (Id == 0)
            {
                Technicalsdata = _EmployeeSkillsEndorsementMethod.getTechnicalAllList().Where(x => x.EmployeeId == SessionProxy.UserId && x.Id == Id).ToList();
            }
            else
            {
                Technicalsdata = _EmployeeSkillsEndorsementMethod.getTechnicalAllList().Where(x => x.EmployeeId == SessionProxy.UserId && x.Id == Id).ToList();
                model.Id = Convert.ToInt32(Technicalsdata.FirstOrDefault().TechnicalSkillsName);
            }
            foreach (var item in Technicalsdata)
            {
                EmployeeSkillsEndorsementViewModel Technical = new EmployeeSkillsEndorsementViewModel();
                Technical.TechnicalSkillsName = _EmployeeSkillsEndorsementMethod.getTechnicalAllList(Convert.ToInt32(item.TechnicalSkillsName));
                Technical.Id = item.Id;

                model.TechnicalSkillsList.Add(Technical);
            }
            model.TechnicalList = _EmployeeSkillsEndorsementMethod.BindTechnicalDropdown();
            return model;
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
            //List<AspNetUser> data = _AdminPearformanceMethod.getAllUserList().ToList();
            //model.EmployeesUserList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            //foreach (var item in data)
            //{
            //    string Name = string.Format("{0} {1}", item.FirstName, item.LastName);
            //    model.EmployeesUserList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
            //}
            return PartialView("_PartialAskForEndorsementView", model);
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
            if (Endrosementdata.Count > 0)
            {
                foreach (var item in Endrosementdata)
                {
                    ViewSkillsViewModel m = new ViewSkillsViewModel();
                    m.Name = item.SkillsName;
                    var emp_name = _EmployeeMethod.getEmployeeById(item.EmployeeId);
                    if (emp_name != null)
                    { m.EmpolyeeName = string.Format("{0} {1}", emp_name.FirstName, emp_name.LastName); }
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
                    foreach (var Aspitem in aspList)
                    {
                        if (!string.IsNullOrEmpty(item.AssignSkillId))
                        {
                            if (item.AssignSkillId.IndexOf(",") > 0)
                            {
                                foreach (var Assignitem in item.AssignSkillId.Split(','))
                                {
                                    if (Assignitem != "" && Assignitem != null)
                                    {
                                        if (SessionProxy.UserId == Convert.ToInt32(Assignitem))
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
                                    if (SessionProxy.UserId == Convert.ToInt32(item.AssignSkillId))
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
            if (editlistrecord.AssignSkillId != "")
            {
                if (editlistrecord.AssignSkillId.IndexOf(',') > 0)
                {
                    model.selectedemployee = editlistrecord.AssignSkillId.Split(',').ToList();
                    string[] seleEmp = editlistrecord.AssignSkillId.Split(',');
                    string name = "";
                    for (int i = 0; i < seleEmp.Length; i++)
                    {
                        int seleId = Convert.ToInt32(seleEmp[i]);
                        var edata = _db.AspNetUsers.Where(x => x.Id == seleId).ToList();

                        if (edata.Count > 0)
                        {
                            foreach (var item in edata)
                            {
                                name = item.FirstName + " " + item.LastName + "-" + item.SSOID + ", " + name;
                            }
                        }
                    }
                    model.selectEmpName = name;
                    model.selectEmpId = editlistrecord.AssignSkillId;
                }
                else
                {
                    if (!string.IsNullOrEmpty(editlistrecord.AssignSkillId))
                    {
                        string record = editlistrecord.AssignSkillId;
                        model.selectedemployee.Add(record);
                    }
                }
                ViewSkillsViewModel skmodel = new ViewSkillsViewModel();
                var coment_record = _EmployeeSkillsEndorsementMethod.EditSkillcomments(Id);
                foreach (var item in coment_record)
                {
                    skmodel.Id = item.Id;
                    skmodel.Comments = item.Comments;
                    skmodel.EndrosementId = (int)item.EndrosementId;

                }
                model.commenet = skmodel.Comments;
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
        public ActionResult DeleteGeneralSkills(int Id, int EmployeeID,int generalid)
        {
            int Users = SessionProxy.UserId;
            _EmployeeSkillsEndorsementMethod.DeleteGeneralskills(Id, Users, generalid);
            EmployeeSkillsEndorsementViewModel model = returnList();
            return PartialView("_PartialSkillsEndorsmentList", model);
        }
        public ActionResult DeleteTechnicalSkills(int Id, int EmployeeID, int techID)
        {
            int Users = SessionProxy.UserId;
            _EmployeeSkillsEndorsementMethod.DeleteTechnicalskills(Id, Users, techID);
            EmployeeSkillsEndorsementViewModel model = returnList();
            return PartialView("_PartialSkillsEndorsmentList", model);
        }

        //customer Filter
        public ActionResult skillFilterByJobTitle(int EmpID, int jobId, int skiId, int poolId, int selEmpId)
        {
            List<Employee_AddEndrosementSkills> Endrosementdata = _EmployeeSkillsEndorsementMethod.getAllEndrosementList(EmpID).ToList();
            List<ViewSkillsViewModel> model = new List<ViewSkillsViewModel>();
            if (Endrosementdata.Count > 0)
            {
                if (Endrosementdata.FirstOrDefault().AssignSkillId != null)
                {
                    string[] EmpId = Endrosementdata.FirstOrDefault().AssignSkillId.Split(',');
                    for (int i = 0; i < EmpId.Length; i++)
                    {
                        int eid = Convert.ToInt32(EmpId[i]);
                        var data = _db.AspNetUsers.Where(x => x.Id == eid).ToList();
                        foreach (var jdata in data)
                        {
                            if (jdata.JobTitle == jobId)
                            {
                                foreach (var sitem in Endrosementdata)
                                {
                                    var aspdata = _db.Employee_AddEndrosementSkills.Where(x => x.AssignSkillId.Contains(sitem.AssignSkillId) && x.EmployeeId == EmpID).ToList();
                                    foreach (var item in aspdata)
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
                                        foreach (var Aspitem in aspList)
                                        {
                                            if (item.AssignSkillId.IndexOf(",") > 0)
                                            {
                                                foreach (var Assignitem in item.AssignSkillId.Split(','))
                                                {
                                                    if (SessionProxy.UserId == Convert.ToInt32(Assignitem))
                                                    {
                                                        var Assign_name = _EmployeeMethod.getEmployeeById(Convert.ToInt32(Assignitem));
                                                        m.AssignEmployeeName = string.Format("{0} {1}", Assign_name.FirstName, Assign_name.LastName);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(item.AssignSkillId))
                                                {
                                                    if (SessionProxy.UserId == Convert.ToInt32(item.AssignSkillId))
                                                    {
                                                        var Assign_name = _EmployeeMethod.getEmployeeById(Convert.ToInt32(item.AssignSkillId));
                                                        m.AssignEmployeeName = string.Format("{0} {1}", Assign_name.FirstName, Assign_name.LastName);
                                                    }
                                                }
                                            }
                                        }
                                        model.Add(m);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return PartialView("_PartialAddEndorsementSkillsList", model);

        }
        public ActionResult SkillsBySkillId(int EmplId, int ESkillId)
        {
            List<Employee_AddEndrosementSkills> Endrosementdata = _EmployeeSkillsEndorsementMethod.getAllEndrosementList(EmplId).ToList();
            List<ViewSkillsViewModel> model = new List<ViewSkillsViewModel>();
            if (Endrosementdata.Count > 0)
            {
                if (Endrosementdata.FirstOrDefault().AssignSkillId != null)
                {
                    foreach (var sitem in Endrosementdata)
                    {
                        var aspdata = _db.Employee_AddEndrosementSkills.Where(x => x.AssignSkillId.Contains(sitem.AssignSkillId) && x.SkilsId == ESkillId && x.EmployeeId == EmplId).ToList();
                        foreach (var item in aspdata)
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
                            foreach (var Aspitem in aspList)
                            {
                                if (item.AssignSkillId.IndexOf(",") > 0)
                                {
                                    foreach (var Assignitem in item.AssignSkillId.Split(','))
                                    {
                                        if (SessionProxy.UserId == Convert.ToInt32(Assignitem))
                                        {
                                            var Assign_name = _EmployeeMethod.getEmployeeById(Convert.ToInt32(Assignitem));
                                            m.AssignEmployeeName = string.Format("{0} {1}", Assign_name.FirstName, Assign_name.LastName);
                                        }
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(item.AssignSkillId))
                                    {
                                        if (SessionProxy.UserId == Convert.ToInt32(item.AssignSkillId))
                                        {
                                            var Assign_name = _EmployeeMethod.getEmployeeById(Convert.ToInt32(item.AssignSkillId));
                                            m.AssignEmployeeName = string.Format("{0} {1}", Assign_name.FirstName, Assign_name.LastName);
                                        }
                                    }
                                }
                            }
                            model.Add(m);
                        }
                    }
                }
            }
            return PartialView("_PartialAddEndorsementSkillsList", model);
        }
        public ActionResult skillFilterByName(int Id, string EmpId)
        {
            List<Employee_AddEndrosementSkills> Endrosementdata = _EmployeeSkillsEndorsementMethod.getAllEndrosementList(Id).ToList();
            List<ViewSkillsViewModel> model = new List<ViewSkillsViewModel>();
            if (Endrosementdata.Count > 0)
            {
                if (Endrosementdata.FirstOrDefault().AssignSkillId != null)
                {
                    string[] EmplId = Endrosementdata.FirstOrDefault().AssignSkillId.Split(',');
                    for (int i = 0; i < EmplId.Length; i++)
                    {
                        int eid = Convert.ToInt32(EmplId[i]);
                        //var data = _db.Employee_AddEndrosementSkills.Where(x => x.AssignSkillId.Contains(EmpId)).ToList();
                        foreach (var sitem in Endrosementdata)
                        {
                            var aspdata = _db.Employee_AddEndrosementSkills.Where(x => x.AssignSkillId.Contains(EmpId) && x.EmployeeId == Id).ToList();
                            foreach (var item in aspdata)
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
                                foreach (var Aspitem in aspList)
                                {
                                    if (item.AssignSkillId.IndexOf(",") > 0)
                                    {
                                        foreach (var Assignitem in item.AssignSkillId.Split(','))
                                        {
                                            if (SessionProxy.UserId == Convert.ToInt32(Assignitem))
                                            {
                                                var Assign_name = _EmployeeMethod.getEmployeeById(Convert.ToInt32(Assignitem));
                                                m.AssignEmployeeName = string.Format("{0} {1}", Assign_name.FirstName, Assign_name.LastName);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(item.AssignSkillId))
                                        {
                                            if (SessionProxy.UserId == Convert.ToInt32(item.AssignSkillId))
                                            {
                                                var Assign_name = _EmployeeMethod.getEmployeeById(Convert.ToInt32(item.AssignSkillId));
                                                m.AssignEmployeeName = string.Format("{0} {1}", Assign_name.FirstName, Assign_name.LastName);
                                            }
                                        }
                                    }
                                }
                                model.Add(m);
                            }
                        }
                    }
                }
            }
            return PartialView("_PartialAddEndorsementSkillsList", model);
        }
        public ActionResult SkillsEndorsedbyYou(int EmpId)
        {
            List<ViewSkillsViewModel> modesllist = returnEndrosementList(EmpId);
            return PartialView("_PartialAddEndorsementSkillsList", modesllist);
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

            //return PartialView("_PartialEditAskForEndorsementSkillsSet", model);
            return PartialView("_PartialEditAskForEndorsementSkillsSet", model);
        }
        public ActionResult getResource(int CustomerId)
        {
            EmployeeSkillsEndorsementViewModel model = new EmployeeSkillsEndorsementViewModel();
            List<AspNetUser> data = _AdminPearformanceMethod.getAllUserList().ToList();
            //model.AllResourceList.Add(new SelectListItem() { Text = "All", Value = "0" });
            //foreach (var item in data)
            //{
            //    string Name = string.Format("{0} {1} - {2}", item.FirstName, item.LastName,item.SSOID);
            //    model.AllResourceList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
            //}
            var rData = _db.AspNetUsers.Where(x => x.Id == CustomerId && x.Archived == false).ToList();
            foreach (var item in rData)
            {
                if (item.CustomerCareID != null)
                {
                    string[] custId = item.CustomerCareID.Split(',');
                    for (int i = 0; i < custId.Length; i++)
                    {
                        int cstId = Convert.ToInt32(custId[i]);
                        if (rData.FirstOrDefault().CustomerCareID != null)
                        {
                            var Resuorcedata = _db.AspNetUsers.Where(x => x.Id == cstId).ToList();
                            if (Resuorcedata.Count != 0)
                            {
                                string Name = string.Format("{0} {1} - {2}", Resuorcedata.FirstOrDefault().FirstName, Resuorcedata.FirstOrDefault().LastName, Resuorcedata.FirstOrDefault().SSOID);
                                model.AllResourceList.Add(new SelectListItem() { Text = Name, Value = cstId.ToString() });
                            }
                            //var jobtitilelist = _AdminPearformanceMethod.getJobtitle(cstId);
                            //model.JobTitleList.Add(new SelectListItem() { Text = jobtitilelist.FirstOrDefault().Value, Value = jobtitilelist.FirstOrDefault().Id.ToString() });
                        }
                    }
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult skillFilterByPool(int Id, int PoolId)
        {
            List<Employee_AddEndrosementSkills> Endrosementdata = _EmployeeSkillsEndorsementMethod.getAllEndrosementList(Id).ToList();
            List<ViewSkillsViewModel> model = new List<ViewSkillsViewModel>();
            if (Endrosementdata != null)
            {
                if (Endrosementdata.FirstOrDefault().AssignSkillId != null)
                {

                    string[] EmpId = Endrosementdata.FirstOrDefault().AssignSkillId.Split(',');
                    for (int i = 0; i < EmpId.Length; i++)
                    {
                        int eid = Convert.ToInt32(EmpId[i]);
                        var data = _db.EmployeeRelations.Where(x => x.UserID == eid).ToList();
                        if (data.FirstOrDefault().PoolID == PoolId)
                        {
                            foreach (var sitem in Endrosementdata)
                            {
                                var aspdata = _db.Employee_AddEndrosementSkills.Where(x => x.AssignSkillId.Contains(sitem.AssignSkillId) && x.EmployeeId == Id).ToList();
                                foreach (var item in aspdata)
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
                                    foreach (var Aspitem in aspList)
                                    {
                                        if (item.AssignSkillId.IndexOf(",") > 0)
                                        {
                                            foreach (var Assignitem in item.AssignSkillId.Split(','))
                                            {
                                                if (SessionProxy.UserId == Convert.ToInt32(Assignitem))
                                                {
                                                    var Assign_name = _EmployeeMethod.getEmployeeById(Convert.ToInt32(Assignitem));
                                                    m.AssignEmployeeName = string.Format("{0} {1}", Assign_name.FirstName, Assign_name.LastName);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(item.AssignSkillId))
                                            {
                                                if (SessionProxy.UserId == Convert.ToInt32(item.AssignSkillId))
                                                {
                                                    var Assign_name = _EmployeeMethod.getEmployeeById(Convert.ToInt32(item.AssignSkillId));
                                                    m.AssignEmployeeName = string.Format("{0} {1}", Assign_name.FirstName, Assign_name.LastName);
                                                }
                                            }
                                        }
                                    }
                                    model.Add(m);

                                }
                            }
                        }
                    }
                }
            }
            return PartialView("_PartialAddEndorsementSkillsList", model);
        }

        public ActionResult FilterByAll(int EmpID, int? jobId, int? skiId, int? poolId, int? selEmpId)
        {
            List<Employee_AddEndrosementSkills> Endrosementdata = _EmployeeSkillsEndorsementMethod.getAllEndrosementList(EmpID).ToList();
            List<ViewSkillsViewModel> model = new List<ViewSkillsViewModel>();
            //string[] EmpId = Endrosementdata.FirstOrDefault().AssignSkillId.Split(',');
            var aspdata = _EmployeeSkillsEndorsementMethod.getAllEndrosementList(EmpID).ToList();
            var skillEndorse = _EmployeeSkillsEndorsementMethod.getAllEndorsementListFilter(EmpID).ToList();

            if (skillEndorse != null)
            {

                if (jobId != 0)
                {
                    skillEndorse = skillEndorse.Where(x => x.JobTitle == jobId && x.EmployeeId == EmpID).ToList();
                }
                if (skiId != 0)
                {
                    skillEndorse = skillEndorse.Where(x => x.JobTitle == jobId && x.SkilsId == skiId && x.EmployeeId == EmpID).ToList();
                }
                if (poolId != 0)
                {
                    skillEndorse = skillEndorse.Where(x => x.JobTitle == jobId && x.SkilsId == skiId && x.EmployeeId == EmpID && x.PoolID == poolId).ToList();
                }
                if (selEmpId != null)
                {
                    skillEndorse = skillEndorse.Where(x => x.JobTitle == jobId && x.SkilsId == skiId && x.EmployeeId == EmpID && x.PoolID == poolId && x.UserID == selEmpId).ToList();
                }
                foreach (var item in skillEndorse)
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
                    foreach (var Aspitem in aspList)
                    {
                        if (item.AssignSkillId.IndexOf(",") > 0)
                        {
                            foreach (var Assignitem in item.AssignSkillId.Split(','))
                            {
                                if (SessionProxy.UserId == Convert.ToInt32(Assignitem))
                                {
                                    var Assign_name = _EmployeeMethod.getEmployeeById(Convert.ToInt32(Assignitem));
                                    m.AssignEmployeeName = string.Format("{0} {1}", Assign_name.FirstName, Assign_name.LastName);
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(item.AssignSkillId))
                            {
                                if (SessionProxy.UserId == Convert.ToInt32(item.AssignSkillId))
                                {
                                    var Assign_name = _EmployeeMethod.getEmployeeById(Convert.ToInt32(item.AssignSkillId));
                                    m.AssignEmployeeName = string.Format("{0} {1}", Assign_name.FirstName, Assign_name.LastName);
                                }
                            }
                        }
                    }
                    model.Add(m);
                }
            }
            return PartialView("_PartialAddEndorsementSkillsList", model);
        }
    }
}





