using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Admin
{

    public class TMSVacancyViewModel
    {
        public TMSVacancyViewModel()
        {
            StatusList = new List<SelectListItem>();
            RecruitmentProcessList = new List<SelectListItem>();
            BusinessList = new List<SelectListItem>();
            DivisionList = new List<SelectListItem>();
            PoolList = new List<SelectListItem>();
            FunctionList = new List<SelectListItem>();
            HiringLeadList = new List<SelectListItem>();
            SourceList = new List<SelectListItem>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public int StatusID { get; set; }
        public string Status { get; set; }
        public string ClosingDate { get; set; }
        public int RecruitmentProcessID { get; set; }
        public string RecruitmentProcess { get; set; }
        public string Salary { get; set; }
        public string Location { get; set; }
        public int BusinessID { get; set; }
        public string Business { get; set; }
        public int DivisionID { get; set; }
        public string Division { get; set; }
        public int PoolID { get; set; }
        public string Pool { get; set; } 
        public int FunctionID { get; set; }
        public string Function { get; set; }
        public string JobDescription { get; set; }
        public int HiringLeadID { get; set; }
        public string HiringLead { get; set; }
        public bool MustUploadCoverLetter { get; set; }
        public bool MustUploadResumeCV { get; set; }
        public string ApplicationFormPathOriginal { get; set; }
        public string ApplicationFormPath { get; set; }
        public bool Question1On { get; set; }
        public string Question1Text { get; set; }
        public bool Question2On { get; set; }
        public string Question2Text { get; set; }
        public bool Question3On { get; set; }
        public string Question3Text { get; set; }
        public bool Question4On { get; set; }
        public string Question4Text { get; set; }
        public bool Question5On { get; set; }
        public string Question5Text { get; set; }
        public int SourceID { get; set; }
        public string Source { get; set; }
        public int Newapplicants { get; set; }
        public IList<SelectListItem> StatusList { get; set; }
        public IList<SelectListItem> RecruitmentProcessList { get; set; }
        public IList<SelectListItem> BusinessList { get; set; }
        public IList<SelectListItem> DivisionList { get; set; }
        public IList<SelectListItem> PoolList { get; set; }
        public IList<SelectListItem> FunctionList { get; set; }
        public IList<SelectListItem> HiringLeadList { get; set; }
        public IList<SelectListItem> SourceList { get; set; }
    }

    public class TMSVacancyDetailsViewModel
    {
        public TMSVacancyDetailsViewModel()
        {
            VacancyList = new List<TMSVacancyViewModel>();
            StepList = new List<TMSSettingStepDetails>();
        }

        public IList<TMSVacancyViewModel> VacancyList { get; set; }

        public int Id { get; set; }
        public string Title { get; set; }
        public int RecruitmentProcessID { get; set; }
        public string RecruitmentProcess { get; set; }
        public int StatusID { get; set; }
        public string Status { get; set; }
        public string ClosingDate { get; set; }
        public string Summary { get; set; }
        public IList<TMSSettingStepDetails> StepList { get; set; }
    }

    public class TMSVacancyJobDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Business { get; set; }
        public string Division { get; set; }
        public string Pool { get; set; }
        public string Function { get; set; }
        public string JobDescription { get; set; }
        public string Salary { get; set; }
        public string Location { get; set; }

    }
}