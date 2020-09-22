using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.CommanMethods.Admin
{
    public class AdminDocumentMethod
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        private string inputFormat = "dd-MM-yyyy";
        private string outputFormat = "yyyy-MM-dd HH:mm:ss";

        #endregion

        #region Method
        public Admin_Document getDocumentDetailsById(int Id)
        {
            return _db.Admin_Document.Where(x => x.Id == Id).FirstOrDefault();
        }

        public List<Admin_Document> getAllDocumentList()
        {
            return _db.Admin_Document.Where(x => x.Archived == false).ToList();

        }

        public bool SaveDocumentData(AdminDocumentViewModel DataModel, int UserId)
        {
            //DateTime Create = new DateTime();
            if (DataModel.Id == 0)
            {
                Admin_Document model = new Admin_Document();
                model.DocumentOriginalPath = DataModel.DocumentOriginalPath;
                model.DocumentPath = DataModel.DocumentPath;
                model.LinkDisplayText = DataModel.LinkDisplayText;
                model.LinkURL = DataModel.LinkURL;
                model.Description = DataModel.Description;
                model.BusinessID = DataModel.BusinessID;
                model.DivisionID = DataModel.DivisionID;
                model.PoolID = DataModel.PoolID;
                model.FunctionID = DataModel.FunctionID;
                model.Category = DataModel.Category;
                model.EmployeeAccess = DataModel.EmployeeAccess;
                model.ManagerAccess = DataModel.ManagerAccess;
                model.CustomerAccess = DataModel.CustomerAccess;
                model.SpecificWorker = DataModel.SpecificWorker;
                model.WorkerID = DataModel.WorkerID;
                model.SpecificManager = DataModel.SpecificManager;
                model.ManagerID = DataModel.ManagerID;
                model.SpecificCustomer = DataModel.SpecificCustomer;
                model.CustomerID = DataModel.CustomerID;
                model.SignatureRequire = DataModel.SignatureRequire;
                model.IpAddress = DataModel.IpAddress;
                model.Archived = false;
                model.UserIDCreatedBy = UserId;
                model.UserIDLastModifiedBy = UserId;
                model.CreatedDate =DateTime.Now;
                model.LastModified =DateTime.Now;
                _db.Admin_Document.Add(model);
                _db.SaveChanges();
            }
            else
            {
                var model = _db.Admin_Document.Where(x => x.Id == DataModel.Id).FirstOrDefault();
                model.DocumentOriginalPath = DataModel.DocumentOriginalPath;
                model.DocumentPath = DataModel.DocumentPath;
                model.LinkDisplayText = DataModel.LinkDisplayText;
                model.LinkURL = DataModel.LinkURL;
                model.Description = DataModel.Description;
                model.BusinessID = DataModel.BusinessID;
                model.DivisionID = DataModel.DivisionID;
                model.PoolID = DataModel.PoolID;
                model.FunctionID = DataModel.FunctionID;
                model.Category = DataModel.Category;
                model.EmployeeAccess = DataModel.EmployeeAccess;
                model.ManagerAccess = DataModel.ManagerAccess;
                model.CustomerAccess = DataModel.CustomerAccess;
                model.SpecificWorker = DataModel.SpecificWorker;
                model.WorkerID = DataModel.WorkerID;
                model.SpecificManager = DataModel.SpecificManager;
                model.ManagerID = DataModel.ManagerID;
                model.SpecificCustomer = DataModel.SpecificCustomer;
                model.CustomerID = DataModel.CustomerID;
                model.SignatureRequire = DataModel.SignatureRequire;
                model.IpAddress = DataModel.IpAddress;
                model.UserIDLastModifiedBy = UserId;
                model.LastModified = DateTime.Now;
                _db.SaveChanges();

            }

            return true;
        }

        public bool DeleteDocumentData (int Id,int UserId)
        {
            var model = _db.Admin_Document.Where(x => x.Id == Id).FirstOrDefault();
            model.Archived = true;
            model.UserIDLastModifiedBy = UserId;
            model.LastModified = DateTime.Now;
            _db.SaveChanges();
            return true;
        }

       // public 
        #endregion
    }
}