using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Resources
{
    public class AddSalaryDeductionViewModel
    {

        public AddSalaryDeductionViewModel()
        {
            SalaryTypeList = new List<SelectListItem>();
            //SalaryDeductionList = new List<AddSalaryDeductionViewModel>();
        }
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public int EmployeeSalaryID { get; set; }
        public string TotalSalary { get; set; }
        public string SalaryType { get; set; }
        public int DeductionID { get; set; }
        public string Deduction { get; set; }
        public decimal FixedAmount { get; set; }
        public decimal PercentOfSalary { get; set; }
        public bool IncludeInSalary { get; set; }
        public string Comments { get; set; }

        //Salary Details
        public string EffectiveFrom { get; set; }
        public int SalaryTypeID { get; set; }
        public int PaymentFrequencyID { get; set; }
        public string Amount { get; set; }
        public int CurrencyID { get; set; }
        public int ReasonforChange { get; set; }
        public string SalaryComments { get; set; }

        public IList<SelectListItem> SalaryTypeList { get; set; }
        // public IList<AddSalaryDeductionViewModel> SalaryDeductionList { get; set; }
    }

    public class AddSalaryDeductionViewModelTemp
    {

        public AddSalaryDeductionViewModelTemp()
        {
            SalaryTypeList = new List<SelectListItem>();
            //SalaryDeductionList = new List<AddSalaryDeductionViewModel>();
        }
        public int Id { get; set; }
        public int DeductionID { get; set; }
        public string Deduction { get; set; }
        public decimal FixedAmount { get; set; }
        public decimal PercentOfSalary { get; set; }
        public bool IncludeInSalary { get; set; }
        public string Comments { get; set; }

        //Salary Details

        public string EmployeeID { get; set; }
        public int EmployeeSalaryID { get; set; }
        public string EffectiveFrom { get; set; }
        public int SalaryTypeID { get; set; }
        public string SalaryType { get; set; }
        public int PaymentFrequencyID { get; set; }
        public string Amount { get; set; }
        public int CurrencyID { get; set; }
        public int ReasonforChange { get; set; }
        public string TotalSalary { get; set; }
        public string SalaryComments { get; set; }
        public IList<SelectListItem> SalaryTypeList { get; set; }
        // public IList<AddSalaryDeductionViewModel> SalaryDeductionList { get; set; }
    }
}