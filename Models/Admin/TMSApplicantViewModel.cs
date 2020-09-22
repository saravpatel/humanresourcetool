using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Admin
{
    public class TMSApplicantViewModel
    {
        public TMSApplicantViewModel()
        {
            GenderList = new List<SelectListItem>();
            StatusList = new List<TMSSettingStepDetails>();
            CompatencyList = new List<TMSSettingCompetencyDetails>();
            CommentList = new List<TMSApplicantCommentViewModel>();
            DocumentList = new List<TMSApplicantDocumentViewModel>();
            SourceList = new List<SelectListItem>();
            GeneralSkillsList = new List<SelectListItem>();
            TechnicalSkillsList = new List<SelectListItem>();
            SelectGeneralSkiils = new List<string>();
            SelectTechnicalSkills = new List<string>();
            RejectionReasonList = new List<SelectListItem>();
        }

        public int Id { get; set; }
        public int VacancyID { get; set; }
        public Nullable<int> flagEdit { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int GenderID { get; set; }
        public string GenderName { get; set; }
        public string DateOfBirth { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public string OtherContactDetails { get; set; }
        public int StatusID { get; set; }
        public string Status { get; set; }
        public string CoverLetterPath { get; set; }
        public string CoverLetterPathOriginal { get; set; }
        public string DownloadApplicationFormLink { get; set; }
        public string UploadApplicationFormPathOriginal { get; set; }
        public string UploadApplicationFormPath { get; set; }
        public string ResumePathOriginal { get; set; }
        public string ResumePath { get; set; }
        public string Question1 { get; set; }
        public string Question1Answer { get; set; }
        public string Question2 { get; set; }
        public string Question2Answer { get; set; }
        public string Question3 { get; set; }
        public string Question3Answer { get; set; }
        public string Question4 { get; set; }
        public string Question4Answer { get; set; }
        public string Question5 { get; set; }
        public string Question5Answer { get; set; }
        public int SourceID { get; set; }
        public string CompatencyJSV { get; set; }
        public string CommentJSV { get; set; }
        public string DocumentJSV { get; set; }
        public string GeneralSkillsJSV { get; set; }
        public List<string> SelectGeneralSkiils { get; set; }
        public string TechnicalSkillsJSV { get; set; }
        public List<string> SelectTechnicalSkills { get; set; }
        public string Cost { get; set; }
        public Nullable<int> RejectReasonId { get; set; }
        public string RejectReasonComment { get; set; }
        public string CreateDate { get; set; }
        public IList<SelectListItem> RejectionReasonList { get; set; }
        public IList<SelectListItem> GenderList { get; set; }
        public IList<TMSSettingStepDetails> StatusList { get; set; }
        public IList<TMSSettingCompetencyDetails> CompatencyList { get; set; }
        public IList<TMSApplicantCommentViewModel> CommentList { get; set; }
        public IList<TMSApplicantDocumentViewModel> DocumentList { get; set; }
        public IList<SelectListItem> SourceList { get; set; }
        public IList<SelectListItem> GeneralSkillsList { get; set; }
        public IList<SelectListItem> TechnicalSkillsList { get; set; }
        public TMSVacancyViewModel VacancyDetails { get; set; }

    }

    public class TMSApplicantCommentViewModel
    {
        public int Id { get; set; }
        public int ApplicantID { get; set; }
        public string comment { get; set; }
        public string commentBy { get; set; }
        public string commentTime { get; set; }
    }

    public class TMSApplicantDocumentViewModel
    {
        public int Id { get; set; }
        public int ApplicantID { get; set; }
        public string originalName { get; set; }
        public string newName { get; set; }
        public string description { get; set; }
    }

    public class ApplicantCompetencyPDfViewModel
    {
        public ApplicantCompetencyPDfViewModel()
        {
            CompatencyJSV = new List<TMSSettingCompetencyDetails>();
            CommentJSV = new List<TMSCommentDetails>();
        }

        public int ApplicantId { get; set; }
        public int VacancyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string VacancyName { get; set; }

        public string totalAvg { get; set; }

        public List<TMSSettingCompetencyDetails> CompatencyJSV { get; set; }
        public List<TMSCommentDetails> CommentJSV { get; set; }
    }
    public class ApplicantCommentPDfViewModel
    {
        public ApplicantCommentPDfViewModel()
        {            
            CommentJSV = new List<TMSCommentDetails>();
        }
        public DateTime DateOfBirth { get; set; }

        public int ApplicantId { get; set; }
        public int VacancyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string VacancyName { get; set; }
        public List<TMSCommentDetails> CommentJSV { get; set; }
    }

    public class CommentViewModel
    {
        public int AppId { get; set; }
        public string AppName { get; set; }
        public string AppEmail { get; set; }
        public string AppGender { get; set; }
        public string VacancyName { get; set; }
    }
    public class CompetecyPDfViewModel
    {
        public int Id { get; set; }
        public string CompetencyName { get; set; }
        public int Score { get; set; }
    }

    public class GeneralSkillsJson
    {
        public int GeneralSkiilID { get; set; }
    }
    public class TechnicalSkillsJson
    {
        public int TechnicalSkiilID { get; set; }
    }

    public class TMSApplicantListViewModel
    {

        public TMSApplicantListViewModel()
         {
            RecruitmentProcessList=new List<SelectListItem>();
            }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string VacancyName { get; set; }
        public string RecruitmentProcess { get; set; }
        public string Business { get; set; }
        public string Division { get; set; }
        public string Pool { get; set; }
        public string Function { get; set; }
        public string Status { get; set; }
        public decimal comScore { get; set; }
        public string CreateDate { get; set; }
        public IList<SelectListItem> RecruitmentProcessList { get; set; }
        public Nullable<int> SelectedListId { get; set; }
    }

    public class TMSIndexPageViewModel
    {
        public TMSIndexPageViewModel()
        {
            count = new List<TMSSettingStepDetails>();
            VacancyList = new List<SelectListItem>();
            BusinessList = new List<SelectListItem>();
            DivisionList = new List<SelectListItem>();
            PoolList = new List<SelectListItem>();
            FunctionList = new List<SelectListItem>();
            RecruitmentProcessList = new List<SelectListItem>();
        }

        public int ActiveVacancies { get; set; }

        public int TalentStep { get; set; }

        public int RejectStep { get; set; }
        public int NewApplicent { get; set; }

        public int Accepted { get; set; }

        public List<TMSSettingStepDetails> count { get; set; }
        public IList<SelectListItem> VacancyList { get; set; }
        public IList<SelectListItem> RecruitmentProcessList { get; set; }
        public IList<SelectListItem> BusinessList { get; set; }
        public IList<SelectListItem> DivisionList { get; set; }
        public IList<SelectListItem> PoolList { get; set; }
        public IList<SelectListItem> FunctionList { get; set; }
    }
    public class BlogPieChart
    {
        public BlogPieChart()
        {
            GetSourceList = new List<SelectListItem>();
        }
        public string Name { get; set; }
        public int? BusinessNameCount { get; set; }
        
        public int? DivisionNameCount { get; set; }
        public IList<SelectListItem> GetSourceList { get; set; }

    }

    public class BlogChartSource
    {
        
        public string Name { get; set; }
        
        public int Count { get; set; }
        

    }

    public class ApplicantStepMoveViewModel 
    {
        public int StepID { get; set; }
        public int ApplicantID { get; set; }
        public int VacancyID { get; set; }
        public string StepName{get;set;}

    }

    public class ApplicantRejectReasonViewModel
    {
        int Id { get; set; }
        public string RejectReasonName { get; set; }
    }

}
