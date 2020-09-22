using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Resources
{

    public class EmployeeSkillsEndorsementViewModel
    {
        public EmployeeSkillsEndorsementViewModel()
        {
            TechnicalSkillsList = new List<EmployeeSkillsEndorsementViewModel>();
            GeneralSkillsList = new List<EmployeeSkillsEndorsementViewModel>();
            TechnicalList = new List<SelectListItem>();
            GeneralList = new List<SelectListItem>();
            BusinessList = new List<SelectListItem>();
            DivisionList = new List<SelectListItem>();
            PoolList = new List<SelectListItem>();
            FunctionList = new List<SelectListItem>();
            AllSkillSetList = new List<SelectListItem>();
            AllResourceList = new List<SelectListItem>();
            JobTitleList = new List<SelectListItem>();
            SkillsetList = new List<SelectListItem>();
            PoolListDis = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int EndrosementId { get; set; }
        public string TechnicalSkillsName { get; set; }
        public string GeneralSkillsName { get; set; }
        public Nullable<bool> TechnicalSkillsArchived { get; set; }
        public Nullable<bool> GeneralSkillsArchived { get; set; }
        public string UserIDCreatedBy { get; set; }
        public string UserIDLastModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
        public string SkillType { get; set; }
        public IList<EmployeeSkillsEndorsementViewModel> TechnicalSkillsList { get; set; }
        public IList<EmployeeSkillsEndorsementViewModel> GeneralSkillsList { get; set; }
        public IList<SelectListItem> TechnicalList { get; set; }
        public IList<SelectListItem> GeneralList { get; set; }
        public IList<SelectListItem> BusinessList { get; set; }
        public IList<SelectListItem> DivisionList { get; set; }
        public IList<SelectListItem> PoolList { get; set; }
        public IList<SelectListItem> PoolListDis { get; set; }
        public IList<SelectListItem> FunctionList { get; set; }
        public IList<SelectListItem> AllSkillSetList { get; set; }
        public IList<SelectListItem> AllResourceList { get; set; }
        public IList<SelectListItem> JobTitleList { get; set; }
        public IList<SelectListItem> SkillsetList { get; set; }
    }


    public class ViewSkillsViewModel
    {
        public ViewSkillsViewModel()
        {
            SkillValueList = new List<string>();
            GeneralSkillSet = new List<ViewSkillsViewModel>();
            TechnicalSkillSet = new List<ViewSkillsViewModel>();
            EmployeesUserList = new List<SelectListItem>();
            selectedemployee = new List<string>();
            commentList = new List<commenlistrecordviewModel>();
        }
        public int Id { get; set; }
        public int EndrosementId { get; set; }
        public string Picture { get; set; }
        public string Name { get; set; }
        public Nullable<int> flag { get; set; }
        public string Description { get; set; }
        public bool Archived { get; set; }
        public string TechnicalSkillsCSV { get; set; }
        public string GeneralSkillsCSV { get; set; }
        public IList<string> SkillValueList { get; set; }
        public IList<ViewSkillsViewModel> TechnicalSkillSet { get; set; }
        public IList<ViewSkillsViewModel> GeneralSkillSet { get; set; }
        public IList<SelectListItem> EmployeesUserList { get; set; }
        public List<string> selectedemployee { get; set; }
        public int EmployeeUserId { get; set; }
        public string AssignUser { get; set; }
        public string Comments { get; set; }
        public string EmpolyeeName { get; set; }
        public string CreateDate { get; set; }
        public string AssignEmployeeName { get; set; }
        public int CommentCount { get; set; }
        public int CurrentUserId { get; set; }
        public int skillsId { get; set; }
        public string selectEmpName { get; set; }
        public string selectEmpId { get; set; }
        public List<commenlistrecordviewModel> commentList { get; set; }
        public string SendMailToOtherUser { get; set; }
        public string MailURL { get; set; }
        public string commenet { get; set; }
        public int AssignSkillsId { get; set; }

        public bool SelectedSkills { get; set; }
    }

    public class commenlistrecordviewModel
    {
        public int Id { get; set; }
        public int EndrosementId { get; set; }
        public bool Archived { get; set; }
        public string EmployeeUserId { get; set; }
        public int? UserCreate { get; set; }
        public string Comments { get; set; }
        public string ProfileImagePath { get; set; }
    }

    public class PreviewEmployeeEndorseData
    {
        public PreviewEmployeeEndorseData()
        {
            recepientList = new List<EndorsmentRecipientsList>();
        }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpGenrelSkill { get; set; }        
        public string EmpTechnicalSkill { get; set;}
        public string comment { get; set; }
        public string EmpImage { get; set; }
        public List<EndorsmentRecipientsList> recepientList { get; set; }

    }
    public class EndorsmentRecipientsList
    {
        public int RepId { get; set; }
        public string RepName { get; set; }        
        public string RepImage { get; set; }
        public string RepJobTitle { get; set; }
        public string RepBusiness { get; set; }
        public string RepDivision { get; set; }
        public string RepPool { get; set; }
        public string RepFunction { get; set; }

    }
}