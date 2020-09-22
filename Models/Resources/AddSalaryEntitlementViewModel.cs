using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Resources
{
    public class AddSalaryEntitlementViewModel
    {
        public AddSalaryEntitlementViewModel()
        {
            SalaryTypeList = new List<SelectListItem>();
            // SalaryEntitlementList = new List<AddSalaryEntitlementViewModel>();
        }
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public int EmployeeSalaryID { get; set; }
        public int EntitlementID { get; set; }
        public string SalaryType { get; set; }
        public string TotalSalary { get; set; }
        public string Entitlement { get; set; }
        public decimal FixedAmount { get; set; }
        public decimal PercentOfSalary { get; set; }

        public bool IncludeInSalary { get; set; }

        public string Comments { get; set; }
        public IList<SelectListItem> SalaryTypeList { get; set; }
        // public IList<AddSalaryEntitlementViewModel> SalaryEntitlementList { get; set; }
    }
}