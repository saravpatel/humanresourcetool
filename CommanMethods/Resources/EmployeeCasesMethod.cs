using HRTool.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.CommanMethods.Resources
{
    public class EmployeeCasesMethod
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();

        #endregion

        #region Cases Methods

        public Case getCasesById(int Id)
        {
            return _db.Cases.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
        }

        public Case getCasesByEmployeeId(int Id)
        {
            return _db.Cases.Where(x => x.EmployeeID == Id && x.Archived == false).FirstOrDefault();
        }

        public List<Case> getAllCasesList()
        {
            return _db.Cases.Where(x => x.Archived == false).ToList();
        }

        public List<Cases_Comments> getAllCasesCommentsListById(int Id) 
        {
            return _db.Cases_Comments.Where(x => x.Archived == false && x.CaseID == Id).ToList();
        }

        public List<Cases_Documents> getAllCasesDocumentListById(int Id)
        {
            return _db.Cases_Documents.Where(x => x.Archived == false && x.CaseID == Id).ToList();
        }

        //public bool SaveCompanyCustomerData(string PhotoPath, int Title, string FirstName, string LastName, string Email, int Gender, DateTime DateOfBirth, string PostalCode, string Address, string WorkPhone, string Mobile, int Id, string UserId)
        //{
        //    var data = _db.Cmp_Customer.Where(x => x.Email == Email && x.Id != Id).ToList();

        //    if (data.Count > 0)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        if (Id == 0)
        //        {
        //            Cmp_Customer save = new Cmp_Customer();
        //            save.PhotoPath = PhotoPath;
        //            save.Title = Title;
        //            save.FirstName = FirstName;
        //            save.LastName = LastName;
        //            save.Email = Email;
        //            save.Gender = Gender;
        //            save.DateOfBirth = DateOfBirth;
        //            save.PostalCode = PostalCode;
        //            save.Address = Address;
        //            save.WorkPhone = WorkPhone;
        //            save.Mobile = Mobile;
        //            save.CreatedDate = DateTime.Now;
        //            save.Archived = false;
        //            save.UserIDCreatedBy = UserId;
        //            save.UserIDLastModifiedBy = UserId;
        //            save.LastModified = DateTime.Now;
        //            _db.Cmp_Customer.Add(save);
        //            _db.SaveChanges();
        //            return true;
        //        }
        //        else
        //        {
        //            var update = _db.Cmp_Customer.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
        //            update.PhotoPath = PhotoPath;
        //            update.Title = Title;
        //            update.FirstName = FirstName;
        //            update.LastName = LastName;
        //            update.Email = Email;
        //            update.Gender = Gender;
        //            update.DateOfBirth = DateOfBirth;
        //            update.PostalCode = PostalCode;
        //            update.Address = Address;
        //            update.WorkPhone = WorkPhone;
        //            update.Mobile = Mobile;
        //            update.UserIDLastModifiedBy = UserId;
        //            update.LastModified = DateTime.Now;
        //            _db.SaveChanges();
        //            return true;
        //        }
        //    }
        //}

        public bool deleteCases(int Id, int UserId)
        {
            var data = _db.Cases.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
            data.Archived = true;
            data.UserIDLastModifiedBy = UserId;
            data.LastModified = DateTime.Now;
            _db.SaveChanges();
            return true;

        }

        #endregion
    }
}