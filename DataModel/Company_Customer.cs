//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRTool.DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Company_Customer
    {
        public int Id { get; set; }
        public string OriginalCompanyLogo { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyName { get; set; }
        public string ShortName { get; set; }
        public string ParentCompany { get; set; }
        public string Website { get; set; }
        public Nullable<int> Currency { get; set; }
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
        public Nullable<int> UserIDCreatedBy { get; set; }
        public Nullable<int> UserIDLastModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
    }
}