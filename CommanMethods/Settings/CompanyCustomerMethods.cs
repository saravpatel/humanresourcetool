using HRTool.DataModel;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.CommanMethods.Settings
{
    public class CompanyCustomerMethods
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();

        #endregion

        #region Company-Customer Methods

        public Cmp_Customer getCompanyCustomerListById(int Id)
        {
            return _db.Cmp_Customer.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
        }
        public bool validateSSo(string sso)
        {
            var data = _db.AspNetUsers.Select(x => x.SSOID).ToList();
            int flag = 0;
            foreach (var item in data)
            {
                if (item == sso)
                {
                    flag = 1;
                    break;
                }
            }
            if (flag == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public List<Cmp_Customer> getAllCompanyCustomerList()
        {
            return _db.Cmp_Customer.Where(x => x.Archived == false).ToList();
        }

        public bool SaveCompanyCustomerData(string PhotoPath, int Title, string FirstName, string LastName, string Email, int Gender, DateTime DateOfBirth, string PostalCode, string Address, string WorkPhone, string Mobile, int Id, int UserId)
        {
            var data = _db.Cmp_Customer.Where(x => x.Email == Email && x.Id != Id).ToList();

            if (data.Count > 0)
            {
                return false;
            }
            else
            {
                if (Id == 0)
                {
                    Cmp_Customer save = new Cmp_Customer();
                    save.PhotoPath = PhotoPath;
                    save.Title = Title;
                    save.FirstName = FirstName;
                    save.LastName = LastName;
                    save.Email = Email;
                    save.Gender = Gender;
                    save.DateOfBirth = DateOfBirth;
                    save.PostalCode = PostalCode;
                    save.Address = Address;
                    save.WorkPhone = WorkPhone;
                    save.Mobile = Mobile;
                    save.CreatedDate = DateTime.Now;
                    save.Archived = false;
                    save.UserIDCreatedBy = UserId;
                    save.UserIDLastModifiedBy = UserId;
                    save.LastModified = DateTime.Now;
                    _db.Cmp_Customer.Add(save);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    var update = _db.Cmp_Customer.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
                    update.PhotoPath = PhotoPath;
                    update.Title = Title;
                    update.FirstName = FirstName;
                    update.LastName = LastName;
                    update.Email = Email;
                    update.Gender = Gender;
                    update.DateOfBirth = DateOfBirth;
                    update.PostalCode = PostalCode;
                    update.Address = Address;
                    update.WorkPhone = WorkPhone;
                    update.Mobile = Mobile;
                    update.UserIDLastModifiedBy = UserId;
                    update.LastModified = DateTime.Now;
                    _db.SaveChanges();
                    return true;
                }
            }
        }

        public bool deleteCompanyCustomer(int Id, int UserId)
        {
            var data = _db.Cmp_Customer.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
            data.Archived = true;
            data.UserIDLastModifiedBy = UserId;
            data.LastModified = DateTime.Now;
            _db.SaveChanges();
            return true;

        }

        #endregion

        #region Customer Company Method
        public Company_Customer GetCustomerCompanyDetailsById(int Id)
        {
            return _db.Company_Customer.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
        }

        public List<Company_Customer> GetAllCustomerCompanyList()
        {
            return _db.Company_Customer.Where(x => x.Archived == false).ToList();
        }

        public int GetAllCustomerByCustomerCompanyID(int Id) 
        {
            string ID = Convert.ToString(Id);
            var data = _db.AspNetUsers.Where(x => x.SelectCustomerCompanyId == ID && x.Archived == false).ToList();
            return data.Count;
        }

        public bool SaveCustomerCompanyData(CustomerCompanyViewModel model, int UserId)
        {
                if (model.Id == 0)
                {
                    Company_Customer save = new Company_Customer();
                    save.AccountName = model.AccountName;
                    save.AccountNumber = model.AccountNumber;
                    save.BankAddress=model.BankAddress;
                    save.BankName=model.BankName;
                    save.BankSortCode=model.BankSortCode;
                    save.BusinessAddress=model.BusinessAddress;
                    save.OriginalCompanyLogo = model.OriginalCompanyLogo;
                    save.CompanyLogo=model.CompanyLogo;
                    save.CompanyName=model.CompanyName;
                    save.CreditLimit=model.CreditLimit;
                    save.CreditStatus=model.CreditStatus;
                    save.Currency=model.CurrencyID;
                    save.Email=model.Email;
                    save.GeneralNotes=model.GeneralNotes;
                    save.IBAN=model.IBAN;
                    save.MailingAddress=model.MailingAddress;
                    save.ParentCompany=model.ParentCompany;
                    save.PaymentTerms=model.PaymentTerms;
                    save.PhoneNumber=model.PhoneNumber;
                    save.SalesRegions=model.SalesRegions;
                    save.SecondaryPhoneNumber=model.SecondaryPhoneNumber;
                    save.ShortName=model.ShortName;
                    save.TaxGroup=model.TaxGroup;
                    save.VAT_GST_Number=model.VAT_GST_Number;
                    save.Website=model.Website;
                    save.CreatedDate = DateTime.Now;
                    save.Archived = false;
                    save.UserIDCreatedBy = UserId;
                    save.UserIDLastModifiedBy = UserId;
                    save.LastModified = DateTime.Now;
                    _db.Company_Customer.Add(save);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    var update = _db.Company_Customer.Where(x => x.Id == model.Id).FirstOrDefault();
                    update.AccountName = model.AccountName;
                    update.AccountNumber = model.AccountNumber;
                    update.BankAddress=model.BankAddress;
                    update.BankName=model.BankName;
                    update.BankSortCode=model.BankSortCode;
                    update.BusinessAddress=model.BusinessAddress;
                    update.CompanyLogo=model.CompanyLogo;
                    update.OriginalCompanyLogo = model.OriginalCompanyLogo;
                    update.CompanyName=model.CompanyName;
                    update.CreditLimit=model.CreditLimit;
                    update.CreditStatus=model.CreditStatus;
                    update.Currency=model.CurrencyID;
                    update.Email=model.Email;
                    update.GeneralNotes=model.GeneralNotes;
                    update.IBAN=model.IBAN;
                    update.MailingAddress=model.MailingAddress;
                    update.ParentCompany=model.ParentCompany;
                    update.PaymentTerms=model.PaymentTerms;
                    update.PhoneNumber=model.PhoneNumber;
                    update.SalesRegions=model.SalesRegions;
                    update.SecondaryPhoneNumber=model.SecondaryPhoneNumber;
                    update.ShortName=model.ShortName;
                    update.TaxGroup=model.TaxGroup;
                    update.VAT_GST_Number=model.VAT_GST_Number;
                    update.Website=model.Website;
                    update.UserIDLastModifiedBy = UserId;
                    update.LastModified = DateTime.Now;
                    _db.SaveChanges();
                    return true;
                }
            }

        public bool DeleteCustomerCompany(int Id, int UserId)
        {
            var data = _db.Company_Customer.Where(x => x.Id == Id).FirstOrDefault();
            data.Archived = true;
            data.UserIDLastModifiedBy = UserId;
            data.LastModified = DateTime.Now;
            _db.SaveChanges();
            return true;

        }

        #endregion

    }
}