using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Resources
{
    public class BenefitsViewModel
    {
        public BenefitsViewModel()
        {
            BenefitList = new List<SelectListItem>();
            BenefitDocumentList = new List<BenefitsDocumentViewModel>();
            CurrencyList = new List<SelectListItem>();
        }

        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public int BenefitID { get; set; }

        public int Currency { get; set; }
        public string BenefitValue { get; set; }
        public string DateAwarded { get; set; }
        public string ExpiryDate { get; set; }
        public decimal FixedAmount { get; set; }
        public bool RecoverOnTermination { get; set; }
        public string Comments { get; set; }
        public string StatusValue { get; set; }       
        public IList<SelectListItem> BenefitList { get; set; }
        public IList<BenefitsDocumentViewModel> BenefitDocumentList { get; set; }
        public IList<SelectListItem> CurrencyList { get; set; }
        public string DocumentListString { get; set; }
    }


    public class BenefitsDocumentViewModel
    {
        public int Id { get; set; }
        public int BenifitId { get; set; }
        public string originalName { get; set; }
        public string newName { get; set; }
        public string description { get; set; }
    }

    public class BenefitsEmployeeViewModel
    {
               public string EmployeeId { get; set; }
       
    }
}