using HRTool.DataModel;
using HRTool.Models.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.CommanMethods.Resources
{
    public class EmployeeSkillsEndorsementMethod
    {
        EvolutionEntities _db = new EvolutionEntities();
        public List<Employee_Skills> getTechnicalAllList()
        {
            return _db.Employee_Skills.Where(x => x.SkillType == "Technical Skills" && x.TechnicalSkillsArchived == false).ToList();
        }



        public List<Employee_AddEndrosementSkills> getAllEndrosementList(int EmployeeId)
        {
            string EmpId = Convert.ToString(EmployeeId);
            return _db.Employee_AddEndrosementSkills.Where(x => (x.AssignSkillId.Contains(EmpId) || x.EmployeeId == EmployeeId) && x.Archived == false).ToList();
        }
        public List<GetSkillEndFilterByAllEmployee_Result> getAllEndorsementListFilter(int EmpId)
        {
            return _db.GetSkillEndFilterByAllEmployee(EmpId).ToList();
        }



        public List<Employee_AddEndrosementSkills> getAllUserEndrosementList()
        {
            return _db.Employee_AddEndrosementSkills.Where(x => x.Archived == false).ToList();
        }
        public Employee_AddEndrosementSkills getEditAllEndrosementList(int Id)
        {
            return _db.Employee_AddEndrosementSkills.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
        }
        public List<Employee_Skills> getGeneralAllList()
        {
            return _db.Employee_Skills.Where(x => x.SkillType == "General Skills" && x.GeneralSkillsArchived == false).ToList();
        }
        public List<SkillSet> getSkillsAllList()
        {
            return _db.SkillSets.Where(x => x.Archived == false).ToList();
        }
        public List<SkillSet> getTechnicalSetAllList()
        {
            return _db.SkillSets.Where(x => x.SkillType == "Technical Skills" && x.Archived == false).ToList();
        }
        public List<SkillSet> getGeneralSetAllList()
        {
            return _db.SkillSets.Where(x => x.SkillType == "General Skills" && x.Archived == false).ToList();
        }
        public List<SelectListItem> BindTechnicalDropdown()
        {
            List<SelectListItem> model = new List<SelectListItem>();
            var countryList = (from i in _db.SkillSets
                               where i.SkillType == "Technical Skills"
                               select i).ToList();
            model.Add(new SelectListItem { Text = "-- Select --", Value = "0" });
            foreach (var item in countryList)
            {
                model.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            return model;
        }
        public List<SelectListItem> BindGeneralDropdown()
        {
            List<SelectListItem> model = new List<SelectListItem>();
            var countryList = (from i in _db.SkillSets
                               where i.SkillType == "General Skills"
                               select i).ToList();
            model.Add(new SelectListItem { Text = "-- Select --", Value = "0" });
            foreach (var item in countryList)
            {
                model.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            return model;
        }
        public void InsertTechnicalSkill(int Id, int EmployeeId, int TechID)
        {
            if (TechID == 0)
            {
                Employee_Skills skils = new Employee_Skills();
                skils.TechnicalSkillsName = Id.ToString();
                skils.GeneralSkillsName = null;
                skils.TechnicalSkillsArchived = false;
                skils.GeneralSkillsArchived = false;
                skils.SkillType = "Technical Skills";
                skils.CreatedDate = DateTime.Now;
                skils.EmployeeId = EmployeeId;
                _db.Employee_Skills.Add(skils);
            }
            else
            {
                string techID = TechID.ToString();
                Employee_Skills skills = (from record in _db.Employee_Skills where record.TechnicalSkillsName == techID select record).FirstOrDefault();
                skills.TechnicalSkillsName = Id.ToString();
                skills.LastModified = DateTime.Now;
                skills.UserIDLastModifiedBy = EmployeeId;
                _db.Entry(skills).State = System.Data.Entity.EntityState.Modified;
            }
            _db.SaveChanges();


        }
        public void InsertGeneralSkill(int Id, int EmployeeId, int GeneralID)
        {
            if (GeneralID == 0)
            {
                Employee_Skills skils = new Employee_Skills();
                skils.TechnicalSkillsName = null;
                skils.GeneralSkillsName = Id.ToString();
                skils.TechnicalSkillsArchived = false;
                skils.GeneralSkillsArchived = false;
                skils.SkillType = "General Skills";
                skils.CreatedDate = DateTime.Now;
                skils.EmployeeId = EmployeeId;
                _db.Employee_Skills.Add(skils);
            }
            else
            {
                string generalID = GeneralID.ToString();
                Employee_Skills skills = (from record in _db.Employee_Skills where record.GeneralSkillsName == generalID select record).FirstOrDefault();
                skills.GeneralSkillsName = Id.ToString();
                skills.LastModified = DateTime.Now;
                skills.UserIDLastModifiedBy = EmployeeId;
                _db.Entry(skills).State = System.Data.Entity.EntityState.Modified;
            }
            _db.SaveChanges();

        }



        public List<SkillSet> getGeneralSkillData()
        {
            return _db.SkillSets.Where(X => X.SkillType == "General Skills").ToList();
        }
        public List<SkillSet> getTechnicalSkillData()
        {
            return _db.SkillSets.Where(X => X.SkillType == "Technical Skills").ToList();
        }
        public string getTechnicalAllList(int Id)
        {
            return _db.SkillSets.Where(x => x.Id == Id).FirstOrDefault().Name;
        }
        public SkillSet GetViewSkillDetails(int Id)
        {
            return _db.SkillSets.Where(x => x.Id == Id).FirstOrDefault();
        }
        public Employee_Skills GetSkillSetRecord(int Id)
        {
            return _db.Employee_Skills.Where(x => x.Id == Id).FirstOrDefault();
        }
        public SystemListValue GetNameSkills(int Id)
        {
            return _db.SystemListValues.Where(x => x.Id == Id).FirstOrDefault();
        }
        public void SaveAssignEndrosementSet(ViewSkillsViewModel model)
        {
            if (model.EndrosementId > 0)
            {
                Employee_AddEndrosementSkills _skills = _db.Employee_AddEndrosementSkills.Where(x => x.Id == model.EndrosementId).FirstOrDefault();
                //List<Employee_Endrosement_Comments> _Comment = _db.Employee_Endrosement_Comments.Where(x => x.EndrosementId == _skills.Id).ToList();
                _skills.EmployeeId = model.EmployeeUserId;
                _skills.AssignSkillId = model.AssignUser;
                _skills.SkillsName = _db.SkillSets.Where(x => x.Id == model.Id).FirstOrDefault().Name;
                _skills.Archived = false;
                _skills.UserIDCreatedBy = model.EmployeeUserId;
                _skills.CreatedDate = DateTime.Now;
                _skills.SkilsId = model.Id;
                _skills.IsRead = false;
                //_skills.Commnets = model.Comments;
                _skills.SkillType = _db.SkillSets.Where(x => x.Id == model.Id).FirstOrDefault().SkillType;
                _db.SaveChanges();
            }
            else
            {
                Employee_AddEndrosementSkills _skills = new Employee_AddEndrosementSkills();
                _skills.EmployeeId = model.EmployeeUserId;
                _skills.AssignSkillId = model.AssignUser;
                if (model.Id > 0)
                {
                    _skills.SkillsName = _db.SkillSets.Where(x => x.Id == model.Id).FirstOrDefault().Name;
                }
                _skills.Archived = false;
                _skills.SkilsId = model.Id;
                _skills.UserIDCreatedBy = model.EmployeeUserId;
                _skills.CreatedDate = DateTime.Now;
                _skills.IsRead = false;
                if (model.Id > 0)
                {
                    _skills.SkillType = _db.SkillSets.Where(x => x.Id == model.Id).FirstOrDefault().SkillType;
                }
                //_skills.Commnets = model.Comments;               
                _db.Employee_AddEndrosementSkills.Add(_skills);
                _db.SaveChanges();
                if (model.Comments != null)
                {
                    Employee_Endrosement_Comments c_skills = new Employee_Endrosement_Comments();
                    c_skills.EndrosementId = _skills.Id;
                    c_skills.Archived = false;
                    c_skills.UserIDCreatedBy = model.CurrentUserId;
                    c_skills.Comments = model.Comments;
                    c_skills.LastModified = DateTime.Now;
                    _db.Employee_Endrosement_Comments.Add(c_skills);
                    _db.SaveChanges();
                }
            }
        }
        public void GetAssignEndrosementSkills(int Id, string Users)
        {
            Employee_AddEndrosementSkills _skills = _db.Employee_AddEndrosementSkills.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
            ViewSkillsViewModel model = new ViewSkillsViewModel();

        }
        public List<GetEndorseRecipientemployeeDetail_Result> getRecipientDetail(int EmpId)
        {
            return _db.GetEndorseRecipientemployeeDetail(EmpId).ToList();
        }
        public void SaveCommentRecords(ViewSkillsViewModel model)
        {
            if (model.Id > 0)
            {
                Employee_Endrosement_Comments _skills = _db.Employee_Endrosement_Comments.Where(x => x.Id == model.Id).FirstOrDefault();
                _skills.EndrosementId = model.EndrosementId;
                _skills.Archived = false;
                _skills.UserIDCreatedBy = model.CurrentUserId;
                _skills.Comments = model.Comments;
                _skills.LastModified = DateTime.Now;
                _db.SaveChanges();
            }
            else
            {
                Employee_Endrosement_Comments _skills = new Employee_Endrosement_Comments();
                _skills.EndrosementId = model.EndrosementId;
                _skills.Archived = false;
                _skills.UserIDCreatedBy = model.CurrentUserId;
                _skills.Comments = model.Comments;
                _skills.CreatedDate = DateTime.Now;
                _db.Employee_Endrosement_Comments.Add(_skills);
                _db.SaveChanges();
            }

        }
        public int GetCommentCount(int Id)
        {
            return _db.Employee_Endrosement_Comments.Where(x => x.EndrosementId == Id && x.Archived == false).Count();
        }
        public List<GetReportToResource_Result> getReportToEmployeeSkill(int EmpId)
        {
            return _db.GetReportToResource(EmpId).ToList();
        }
        public List<Employee_Endrosement_Comments> GetCommentList(int Id)
        {
            return _db.Employee_Endrosement_Comments.Where(x => x.EndrosementId == Id && x.Archived == false).ToList();
        }
        public List<Employee_Endrosement_Comments> Editcomments(int Id)
        {
            return _db.Employee_Endrosement_Comments.Where(x => x.Id == Id).ToList();
        }
        public List<Employee_Endrosement_Comments> EditSkillcomments(int Id)
        {
            return _db.Employee_Endrosement_Comments.Where(x => x.EndrosementId == Id).ToList();
        }
        public void Deletecommentrecord(int Id, int Users)
        {
            ViewSkillsViewModel model = new ViewSkillsViewModel();
            Employee_Endrosement_Comments _skills = _db.Employee_Endrosement_Comments.Where(x => x.Id == Id).FirstOrDefault();
            _skills.Archived = true;
            _skills.LastModified = DateTime.Now;
            _skills.UserIDLastModifiedBy = Users;
            _db.SaveChanges();
        }
        public void DeleteEndrosmentrecord(int Id, int Users)
        {
            ViewSkillsViewModel model = new ViewSkillsViewModel();
            Employee_AddEndrosementSkills _skills = _db.Employee_AddEndrosementSkills.Where(x => x.Id == Id).FirstOrDefault();
            var _skillscomment = _db.Employee_Endrosement_Comments.Where(x => x.EndrosementId == Id).ToList();
            foreach (var item in _skillscomment)
            {
                Employee_Endrosement_Comments _skill_commnet = _db.Employee_Endrosement_Comments.Where(x => x.Id == item.Id).FirstOrDefault();
                _skills.Archived = true;
                _skills.LastModified = DateTime.Now;
                _skills.UserIDLastModifiedBy = Users;
                _db.SaveChanges();
            }
            _skills.Archived = true;
            _skills.LastModified = DateTime.Now;
            _skills.UserIDLastModifiedBy = Users;
            _db.SaveChanges();
        }
        public void DeleteGeneralskills(int Id, int Users,int generalid)
        {
            Employee_Skills _skills = _db.Employee_Skills.Where(x => x.Id == generalid && x.EmployeeId == Users).FirstOrDefault();
            _db.Employee_Skills.Remove(_skills);
            _db.SaveChanges();
        }
        public void DeleteTechnicalskills(int Id, int Users, int techID)
        {
            Employee_Skills _skills = _db.Employee_Skills.Where(x => x.Id == techID && x.EmployeeId == Users).FirstOrDefault();
            _db.Employee_Skills.Remove(_skills);
            _db.SaveChanges();
        }

    }
}