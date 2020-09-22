using HRTool.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Settings
{
    public class CompanyViewModel
    {
        public CompanyViewModel()
        {
            IndustryList = new List<SelectListItem>();
            EmployeeList = new List<SelectListItem>();
            BaseCurrencyList = new List<SelectListItem>();
            ManagerList = new List<SelectListItem>();
            DateFormatList = new List<SelectListItem>();
            TimeFormatList = new List<SelectListItem>();
        }

        public int Id { get; set; }

       // [Required(ErrorMessage = "Company Logo required")]
        public string Logo { get; set; }

      //  [Required(ErrorMessage = "Statement Is required")]
        public string Statement { get; set; }

     //    [Required(ErrorMessage = "Industry Name Is required")]
        public int IndustryID { get; set; }


        public string IndustryValue { get; set; }

      //  [Required(ErrorMessage = "DateFormat Is required")]
        public int DateFormat{get;set;}

        public string DateFormatValue { get; set; }

        // [Required(ErrorMessage = "Time Format Is required")]
        public int TimeFormat { get; set; }

        public string TimeFormatValue { get; set; }
   //     [Required(ErrorMessage = "BaseCurrency Is required")]
        public int BaseCurrency { get; set; }

        public string BaseCurrencyValue { get; set; }

        public bool DailyAdminEmail { get; set; }
        public bool WeeklyManagerEmail { get; set; }
        public bool WeeklyEmployeeEmail { get; set; }
        public bool ManagerSeeEmployeeSalary { get; set; }
        public bool EmployeeSeeSalary { get; set; }
        public bool OrganisationChart { get; set; }

     //   [Required(ErrorMessage = "ExternalLink Is required")]
        public string OrganisationChartExternalLink { get; set; }

        public bool AllowEmployeeUploadPhoto { get; set; }

        public bool ManagerSeeEmployeeContactDetail { get; set; }

        public bool CompanyReport { get; set; }
        public bool ManagerUploadDocument { get; set; }

        public string ProbationPeriod { get; set; }

        public string ProbationPeriodValue { get; set; }

        public int EmployeeAccess { get; set; }

        public string EmployeeAccessValue { get; set; }

        public int ManagerAccess { get; set; }

        public string ManagerAccessValue { get; set; }

        public bool OtherLeaveReasons { get; set; }
        public string UserIDCreatedBy { get; set; }
        public string UserIDLastModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }

        public IList<SelectListItem> IndustryList { get; set; }
        public IList<SelectListItem> EmployeeList { get; set; }
        public IList<SelectListItem> ManagerList { get; set; }
        public IList<SelectListItem> DateFormatList { get; set; }
        public IList<SelectListItem> TimeFormatList { get; set; }
        public IList<SelectListItem> BaseCurrencyList { get; set; }

    }
}