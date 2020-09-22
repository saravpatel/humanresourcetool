using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Settings
{
    public class CompanyCustomerViewModel
    {
        public CompanyCustomerViewModel()
        {
            titleList = new List<OtherSettingValueViewModel>();
            genderList = new List<OtherSettingValueViewModel>();
            customerList = new List<CompanyCustomerListViewModel>();
        }

        public int Id { get; set; }
        public string PhotoPath { get; set; }
        public int Title { get; set; }

        public string TitleValue { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Gender { get; set; }
        public string GenderValue { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }

        public string Dob { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public string WorkPhone { get; set; }
        public string Mobile { get; set; }
        public bool Archived { get; set; }
        public string UserIDCreatedBy { get; set; }
        public string UserIDLastModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
        public IList<OtherSettingValueViewModel> titleList { get; set; }
        public IList<OtherSettingValueViewModel> genderList { get; set; }

        public IList<CompanyCustomerListViewModel> customerList{get;set;}

    }
   public class  CompanyCustomerListViewModel
   {
      public int Id { get; set; }
      public string Name{get;set;}

      public string Phone { get; set; }

      public string Email { get; set; }

      public string Mobile { get; set; }
     }
   public class CompanyCustomeDetailsViewModel
   {
       public int Id { get; set; }
       public string PhotoPath { get; set; }
       public string Title { get; set; }
       public string FirstName { get; set; }
       public string LastName { get; set; }
       public string Email { get; set; }
       public string Gender { get; set; }
       public Nullable<System.DateTime> DateOfBirth { get; set; }

       public string Dob { get; set; }
       public string PostalCode { get; set; }
       public string Address { get; set; }
       public string WorkPhone { get; set; }
       public string Mobile { get; set; }
   
   }
   public class CustomerCompanyViewModel 
   {
       public CustomerCompanyViewModel()
       {
           CurrencyList =new List<SelectListItem>();
       }

       public int Id { get; set; }
       public int CustomerNumber { get; set; }
       public int CustomerCount { get; set; }
       public string OriginalCompanyLogo { get; set; }
       public string CompanyLogo { get; set; }
       public string CompanyName { get; set; }
       public string ShortName { get; set; }
       public string ParentCompany { get; set; }
       public string Website { get; set; }
       public int CurrencyID { get; set; }
       public string CurrencyName { get; set; }
       public string PhoneNumber { get; set; }
       public string Email { get; set; }
       public string SecondaryPhoneNumber { get; set; }
       public string BusinessAddress { get; set; }
       public string MailingAddress { get; set; }
       public string GeneralNotes { get; set; }
       public string VAT_GST_Number { get; set; }
       public string CreditLimit { get; set; }
       public string PaymentTerms { get; set; }
       public string CreditStatus { get; set; }
       public string SalesRegions { get; set; }
       public string TaxGroup { get; set; }
       public string BankName { get; set; }
       public string BankSortCode { get; set; }
       public string AccountNumber { get; set; }
       public string IBAN { get; set; }
       public string AccountName { get; set; }
       public string BankAddress { get; set; }
       public bool Archived { get; set; }
       public string UserIDCreatedBy { get; set; }
       public string UserIDLastModifiedBy { get; set; }
       public Nullable<System.DateTime> CreatedDate { get; set; }
       public Nullable<System.DateTime> LastModified { get; set; }
       public IList<SelectListItem> CurrencyList { get; set; }
   }
   public class CustomerCompanyListViewModel 
   {
     public CustomerCompanyListViewModel ()
	{
            
	}

     public int Id { get; set; }
     public int CustomerCount { get; set; }
     public string CompanyName { get; set; }
     public string ParentCompany { get; set; }
     public string CurrencyName { get; set; }
   } 
}