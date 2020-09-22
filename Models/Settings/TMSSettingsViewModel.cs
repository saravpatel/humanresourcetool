using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Settings
{
    public class TMSSettingsViewModel
    {
        public TMSSettingsViewModel()
        {

            TMSSettingSaveList = new List<TMSSettingAllDetailsViewModel>();
            StepList = new List<TMSSettingStepDetails>();
            CompentecyList = new List<TMSSettingCompetencyDetails>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int SelectCopyId { get; set; }
        public string StepCSV { get; set; }
        public int stepCount { get; set; }
        public int stepSortId { get; set; }
        public string CompetencyCSV { get; set; }

        public IList<TMSSettingAllDetailsViewModel> TMSSettingSaveList { get; set; }

        public IList<TMSSettingStepDetails> StepList { get; set; }

        public IList<TMSSettingCompetencyDetails> CompentecyList { get; set; }

    }

    public class TMSSettingAllDetailsViewModel
    {
        public TMSSettingAllDetailsViewModel()
        {
            
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string StepCSV { get; set; }
        public string CompetencyCSV { get; set; }

    }

    public class TMSSettingStepDetails 
    {

        public TMSSettingStepDetails()
        {
            ApplicantList = new List<TMSApplicantDetails>();  
        }

        public int Id { get; set; }
        public int SortId { get; set; }
        public string StepName { get; set; }
        public string ColorCode { get; set; }

        public IList<TMSApplicantDetails> ApplicantList { get; set; }
       
    }
    public class TMSSettingCompetencyDetails 
    {
        public int Id { get; set; }
        public int SortId { get; set; }
        public string CompetencyName { get; set; }
        public string Description { get; set; }
        public string Score { get; set; }
    }

    public class TMSCommentDetails
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDateTime { get; set; }
    }

    public class TMSApplicantDetails
    {
       
        public int Id { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }
       
        public string Star { get; set; }
       
        public string CreateDate { get;set; }
      
        public decimal comScore { get; set; }

    }

}