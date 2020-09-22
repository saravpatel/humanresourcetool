using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Resources
{
    public class EmployeeResumeViewModel
    {
        public EmployeeResumeViewModel()
        {
            WorkExperienceList = new List<EmployeeExperienceViewModel>();
            EductionList = new List<EmployeeEducationsViewModel>();
            QualificationList = new List<EmployeeQualificationsViewModel>();
            LanguageDetailsList = new List<EmployeeLanguagesViewModel>();
            ResumeTexts = new EmployeeResumeTextViewModel();
        }

        public int EmployeeID { get; set; }
      
        public IList<EmployeeExperienceViewModel> WorkExperienceList { get; set; }

        public IList<EmployeeEducationsViewModel> EductionList { get; set; }

        public IList<EmployeeQualificationsViewModel> QualificationList { get; set; }

        public IList<EmployeeLanguagesViewModel> LanguageDetailsList { get; set; }

        public EmployeeResumeTextViewModel ResumeTexts { get; set; }
        public int ResumeID { get; set; }
        public string ResumeText { get; set; }

    }

    public class EmployeeExperienceViewModel
    {

        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string JobStartDate { get; set; }
        public string JobEndDate { get; set; }

        public string OtherInformation { get; set; }

    }

    public class EmployeeResumeTextViewModel 
    {
        public int EmployeeID { get; set; }
        public int ResumeID { get; set; }
        public string ResumeText { get; set; }
    }

    public class EmployeeEducationsViewModel
    {
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public string CourseName { get; set; }
        public string InstitutionName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string OtherInformation { get; set; }
    }

    public class EmployeeQualificationsViewModel
    {
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public string Detail { get; set; }

    }

    public class EmployeeLanguagesViewModel
    {
        public EmployeeLanguagesViewModel()
        {
            LanguageList = new List<SelectListItem>();
            SpeakingList = new List<SelectListItem>();
            ListeningList = new List<SelectListItem>();
            WritingList = new List<SelectListItem>();
            ReadingList = new List<SelectListItem>();

        }

        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public int LanguageID { get; set; }

        public string LanguageName { get; set; }
        public int SpeakingID { get; set; }
        // public string SpeakingName { get; set; }
        public int ListeningID { get; set; }
        //  public string ListeningName { get; set; }
        public int WritingID { get; set; }
        //  public string WritingName { get; set; }
        public int ReadingID { get; set; }
        //   public string ReadingName { get; set; }
        public string TotalPercentage { get; set; }

        public IList<SelectListItem> LanguageList { get; set; }
        public IList<SelectListItem> SpeakingList { get; set; }
        public IList<SelectListItem> ListeningList { get; set; }
        public IList<SelectListItem> WritingList { get; set; }
        public IList<SelectListItem> ReadingList { get; set; }

    }


    public class EmployeeResumePDFViewModel
    {
        public EmployeeResumePDFViewModel()
        {
            AllDetails = new EmployeeResumeViewModel();
        }
        public int EmployeeID { get; set; }
        public string PhotoPath { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string Address { get; set; }
        public string PersonalPhone { get; set; }
        public string PersonalEmail { get; set; }
        public string Email { get; set; }
        public string ResumeText { get; set; }
        public EmployeeResumeViewModel AllDetails { get; set; }

    }



}