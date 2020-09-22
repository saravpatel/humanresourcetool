using HRTool.CommanMethods.Admin;
using HRTool.CommanMethods.Resources;
using HRTool.CommanMethods.RolesManagement;
using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.IO;
using HRTool.Models.Resources;
using HRTool.CommanMethods;
using static HRTool.CommanMethods.Enums;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class MeEmployeeDocumentController : Controller
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        EmployeeDocumentMethod _EmployeeDocument = new EmployeeDocumentMethod();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        EmployeeMethod _employeeMethod = new EmployeeMethod();
        CompanyStructureMethod _CompanyStructureMethod = new CompanyStructureMethod();
        AdminTMSMethod _AdminTMSMethod = new AdminTMSMethod();
        TMSSettingsMethod _TMSSettingsMethod = new TMSSettingsMethod();
        RolesManagementMethod _RolesManagementMethod = new RolesManagementMethod();

        #endregion

        // GET: /EmployeeDocument/
        [Authorize]
        public ActionResult Index(int EmployeeId)
        {
            EmployeeDocumentListModel model = new EmployeeDocumentListModel();
            //model.LoginUserID = SessionProxy.UserId;
            model.EmployeeID = EmployeeId;
            var data = _EmployeeDocument.getDocumentDetailsByEmployeeId(EmployeeId);
            if (data.Count > 0)
            {

                foreach (var details in data)
                {
                    if (!string.IsNullOrEmpty(details.DocumentPath) || !string.IsNullOrEmpty(details.Description))
                    {
                        EmployeeDocumentViewModel datamodel = new EmployeeDocumentViewModel();
                        datamodel.Id = details.Id;
                        datamodel.Type = "Document";
                        datamodel.EmployeeID = details.EmployeeID;
                        // datamodel.LoginUserID = SessionProxy.UserId;
                        datamodel.Description = details.Description;
                        datamodel.DocumentOriginalPath = details.DocumentOriginalPath;
                        datamodel.DocumentPath = details.DocumentPath;
                        datamodel.Category = (int)details.Category;
                        if (details.Category != 0)
                        {
                            var res = _otherSettingMethod.getSystemListValueById((int)details.Category);
                            datamodel.CategoryName = res.Value;
                        }
                        datamodel.BusinessID = (int)details.BusinessID;
                        if (details.BusinessID != 0)
                        {
                            var buz = _CompanyStructureMethod.getBusinessListById((int)details.BusinessID);
                            if (buz != null)
                            {
                                datamodel.Business = buz.Name;
                            }
                        }
                        datamodel.DivisionID = (int)details.DivisionID;
                        if (details.DivisionID != 0)
                        {
                            var div = _CompanyStructureMethod.getDivisionById((int)details.DivisionID);
                            if (div != null)
                            {
                                datamodel.Division = div.Name;
                            }
                        }
                        datamodel.PoolID = (int)details.PoolID;
                        if (details.PoolID != 0)
                        {
                            var pol = _CompanyStructureMethod.getPoolsListById((int)details.PoolID);
                            if (pol != null)
                            {
                                datamodel.Pool = pol.Name;
                            }
                        }
                        datamodel.FunctionID = (int)details.FunctionID;
                        if (details.FunctionID != 0)
                        {
                            var fun = _CompanyStructureMethod.getFunctionsListById((int)details.FunctionID);
                            if (fun != null)
                            {
                                datamodel.Function = fun.Name;
                            }
                        }
                        datamodel.CreateDate = String.Format("{0:dd-MM-yyyy}", details.CreatedDate);
                        datamodel.SignatureRequire = details.SignatureRequire;
                        if (details.SignatureRequire == true)
                        {
                            datamodel.Signature = "YES";
                        }
                        else
                        {
                            datamodel.Signature = "NO";
                        }
                        var SigCount = _EmployeeDocument.getDocumentSignatureListByDocumentId(details.Id);
                        if (SigCount != null)
                        {
                            datamodel.Signed = true;
                        }
                        else
                        {
                            datamodel.Signed = false;

                        }

                        model.EmployeeList.Add(datamodel);
                    }
                    else if(!string.IsNullOrEmpty(details.LinkDisplayText)|| !string.IsNullOrEmpty(details.LinkURL)){
                        EmployeeDocumentViewModel datamodel = new EmployeeDocumentViewModel();
                        datamodel.Type = "Link";
                        datamodel.Id = details.Id;
                        datamodel.EmployeeID = details.EmployeeID;
                        // datamodel.LoginUserID = SessionProxy.UserId;
                        datamodel.LinkDisplayText = details.LinkDisplayText;
                        datamodel.LinkURL = details.LinkURL;
                        datamodel.Category = (int)details.Category;
                        if (details.Category != 0)
                        {
                            var res = _otherSettingMethod.getSystemListValueById((int)details.Category);
                            datamodel.CategoryName = res.Value;
                        }
                        datamodel.BusinessID = (int)details.BusinessID;
                        if (details.BusinessID != 0)
                        {
                            var buz = _CompanyStructureMethod.getBusinessListById((int)details.BusinessID);
                            if (buz != null)
                            {
                                datamodel.Business = buz.Name;
                            }
                        }
                        datamodel.DivisionID = (int)details.DivisionID;
                        if (details.DivisionID != 0)
                        {
                            var div = _CompanyStructureMethod.getDivisionById((int)details.DivisionID);
                            if (div != null)
                            {
                                datamodel.Division = div.Name;
                            }
                        }
                        datamodel.PoolID = (int)details.PoolID;
                        if (details.PoolID != 0)
                        {
                            var pol = _CompanyStructureMethod.getPoolsListById((int)details.PoolID);
                            if (pol != null)
                            {
                                datamodel.Pool = pol.Name;
                            }
                        }
                        datamodel.FunctionID = (int)details.FunctionID;
                        if (details.FunctionID != 0)
                        {
                            var fun = _CompanyStructureMethod.getFunctionsListById((int)details.FunctionID);
                            if (fun != null)
                            {
                                datamodel.Function = fun.Name;
                            }
                        }
                        datamodel.CreateDate = String.Format("{0:dd-MM-yyyy}", details.CreatedDate);
                        datamodel.SignatureRequire = details.SignatureRequire;
                        if (details.SignatureRequire == true)
                        {
                            datamodel.Signature = "YES";
                        }
                        else
                        {
                            datamodel.Signature = "NO";
                        }
                        var SigCount = _EmployeeDocument.getDocumentSignatureListByDocumentId(details.Id);
                        if (SigCount != null)
                        {
                            datamodel.Signed = true;
                        }
                        else
                        {
                            datamodel.Signed = false;

                        }

                        model.EmployeeList.Add(datamodel);

                    }
                }

            }

            return View(model);
        }
        public ActionResult getCustomer_WorkertList()
        {
            EmployeeDocumentViewModel datamodel = new EmployeeDocumentViewModel();
            var WList = _db.AspNetUsers.Where(x => x.SSOID.Contains("W") && x.Archived == false).ToList();
            foreach (var item in WList)
            {
                datamodel.WorkerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }
            //foreach (var item in _employeeMethod.GetAllResourceEmployeeList().Where(x => x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList())
            //{
            //    datamodel.WorkerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            //}
            foreach (var item in _RolesManagementMethod.GetManagersList())
            {
                datamodel.ManagerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }
            //foreach (var item in _employeeMethod.GetAllCoustomerEmployeeList().Where(x => x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList())
            //{
            //    datamodel.CustomerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            //}
            var List = _db.AspNetUsers.Where(x => x.SSOID.Contains("C") && x.Archived == false).ToList();
            foreach (var item in List)
            {
                datamodel.CustomerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }
            return Json(datamodel, JsonRequestBehavior.AllowGet);
        }

        public EmployeeDocumentViewModel returnList(int Id,int EmployeeID)
        {
            EmployeeDocumentViewModel datamodel = new EmployeeDocumentViewModel();

            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Company Document Category"))
            {
                datamodel.CategoryList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            foreach (var item in _CompanyStructureMethod.getAllBusinessList())
            {
                datamodel.BusinessList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });

            }
            //foreach (var item in _RolesManagementMethod.GetEmployeesList())
            //{
            //    datamodel.WorkerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            //}
            //foreach (var item in _RolesManagementMethod.GetManagersList())
            //{
            //    datamodel.ManagerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            //}
            //foreach (var item in _RolesManagementMethod.GetCustomerList())
            //{
            //    datamodel.CustomerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            //}
            foreach (var item in _employeeMethod.GetAllResourceEmployeeList().Where(x => x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList())
            {
                datamodel.WorkerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }
            foreach (var item in _RolesManagementMethod.GetManagersList())
            {
                datamodel.ManagerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }
            foreach (var item in _employeeMethod.GetAllCoustomerEmployeeList().Where(x => x.AspNetUserRoles.Count() > 0 ? x.AspNetUserRoles.FirstOrDefault().RoleId != (int)Roles.SuperAdmin ? x.CreatedBy == SessionProxy.UserId : true : x.CreatedBy == SessionProxy.UserId).ToList())
            {
                datamodel.CustomerList.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }
            if (Id == 0)
            {
               datamodel.Id = 0;
                datamodel.EmployeeID = EmployeeID;
                // datamodel.LoginUserID = SessionProxy.UserId;
            }
            else
            {
                var data = _EmployeeDocument.getDocumentDetailsById(Id);
                datamodel.Id = data.Id;
                datamodel.EmployeeID = data.EmployeeID;
                //  datamodel.LoginUserID = SessionProxy.UserId;
                datamodel.DocumentOriginalPath = data.DocumentOriginalPath;
                datamodel.DocumentPath = data.DocumentPath;
                datamodel.LinkDisplayText = data.LinkDisplayText;
                datamodel.LinkURL = data.LinkURL;
                datamodel.Description = data.Description;
                datamodel.IpAddress = data.IpAddress;
                datamodel.BusinessID = (int)data.BusinessID;
                datamodel.DivisionID = (int)data.DivisionID;
                if (data.DivisionID != 0 && data.BusinessID != 0)
                {
                    datamodel.DivisionList = bindDiv((int)data.BusinessID);
                    datamodel.PoolList = poolDiv((int)data.DivisionID);
                    datamodel.FunctionList = functionDiv((int)data.DivisionID);
                }
                datamodel.PoolID = (int)data.PoolID;
                datamodel.FunctionID = (int)data.FunctionID;
                datamodel.Category = (int)data.Category;
                datamodel.EmployeeAccess = data.EmployeeAccess;
                datamodel.ManagerAccess = data.ManagerAccess;
                datamodel.CustomerAccess = data.CustomerAccess;
                datamodel.SpecificWorker = data.SpecificWorker;
              //  datamodel.WorkerID = data.WorkerID;
                datamodel.SpecificManager = data.SpecificManager;
              //  datamodel.ManagerID = data.ManagerID;
                datamodel.SpecificCustomer = data.SpecificCustomer;
             //   datamodel.CustomerID = data.CustomerID;
                datamodel.SignatureRequire = data.SignatureRequire;
                int CmpId = Convert.ToInt32(data.WorkerID);
                if (CmpId != 0 && !string.IsNullOrEmpty(data.WorkerID))
                {
                    var Employee = _db.AspNetUsers.Where(x => x.Id == CmpId).FirstOrDefault();
                    if (Employee != null)
                    {
                        datamodel.SpecificWorkerName = Employee.FirstName + " " + Employee.LastName + " " + Employee.SSOID;
                        datamodel.WorkerID = data.WorkerID;
                    }
                }
                int ManId = Convert.ToInt32(data.ManagerID);
                if (ManId != 0 && !string.IsNullOrEmpty(data.ManagerID))
                {
                    var Employee = _db.AspNetUsers.Where(x => x.Id == ManId).FirstOrDefault();
                    if (Employee != null)
                    {
                        datamodel.SpecificManagerName = Employee.FirstName + " " + Employee.LastName + " " + Employee.SSOID;
                        datamodel.ManagerID = data.ManagerID;
                    }
                }
                int CustId = Convert.ToInt32(data.CustomerID);
                if (CustId != 0 && !string.IsNullOrEmpty(data.CustomerID))
                {
                    var Employee = _db.AspNetUsers.Where(x => x.Id == CustId).FirstOrDefault();
                    if (Employee != null)
                    {
                        datamodel.SpecificCustomerName = Employee.FirstName + " " + Employee.LastName + " " + Employee.SSOID;
                        datamodel.CustomerID = data.CustomerID;
                    }
                }
            }
            return datamodel;

        }
        [HttpPost]
        public ActionResult AddDocument(int Id,int EmployeeID)
        {
        
            EmployeeDocumentViewModel Model = returnList(Id,EmployeeID);
            return PartialView("_partialAddEditEmployeeDocumentView", Model);

        }

        public ActionResult bindDivisionList(int businessId)
        {
            var data = _CompanyStructureMethod.GetDivisionListByBizId(businessId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public List<SelectListItem> bindDiv(int Id)
        {
            List<SelectListItem> model = new List<SelectListItem>();
            var data = _CompanyStructureMethod.GetDivisionListByBizId(Id);
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    SelectListItem mm = new SelectListItem();
                    mm.Value = item.Id.ToString();
                    mm.Text = item.Name;
                    model.Add(mm);
                }
            }

            return model;

        }

        public List<SelectListItem> poolDiv(int Id)
        {
            List<SelectListItem> model = new List<SelectListItem>();
            var data = _CompanyStructureMethod.GetPoolListByBizId(Id);
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    SelectListItem mm = new SelectListItem();
                    mm.Value = item.Id.ToString();
                    mm.Text = item.Name;
                    model.Add(mm);
                }
            }
            return model;

        }

        public List<SelectListItem> functionDiv(int Id)
        {
            List<SelectListItem> model = new List<SelectListItem>();
            var data = _CompanyStructureMethod.GetFuncationListByBizId(Id);
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    SelectListItem mm = new SelectListItem();
                    mm.Value = item.Id.ToString();
                    mm.Text = item.Name;
                    model.Add(mm);
                }
            }
            return model;
        }

        public ActionResult bindPoolList(int DivisionId)
        {
            var data = _CompanyStructureMethod.GetPoolListByBizId(DivisionId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult bindFuncationList(int DivisionId)
        {
            var data = _CompanyStructureMethod.GetFuncationListByBizId(DivisionId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult ImageData()
        {
            string FilePath = string.Empty;
            string fileName = string.Empty;
            string originalFileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                FilePath = ConfigurationManager.AppSettings["EmployeeDocument"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
        }

        public ActionResult SaveDocument(EmployeeDocumentViewModel model)
        {

            bool save = _EmployeeDocument.SaveDocumentData(model, SessionProxy.UserId);

            if (save)
            {
                return Json("True", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);

            }


        }

        public ActionResult DeleteDocument(int Id)
        {

            bool save = _EmployeeDocument.DeleteDocumentData(Id, SessionProxy.UserId);

            if (save)
            {
                return Json("True", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);

            }

        }


        #region Signature
        public ActionResult PendingSignature(int Id)
        {
            EmployeeSignatureViewModel model = new EmployeeSignatureViewModel();
            var UserRoleType = _RolesManagementMethod.GetLoginUserRoleType(SessionProxy.UserId);
            if (UserRoleType == "SuperAdmin")
            {
                model.Id = 0;
                return PartialView("_partialShowSendSignature", model);
            }
            else
            {
                var data = _EmployeeDocument.getDocumentDetailsById(Id);
                model.Id = data.Id;
                model.DocumentID = data.Id;
                model.EmployeeID = data.EmployeeID;
                if (data.Id != 0)
                {
                    var detail = _EmployeeDocument.getDocumentDetailsById(data.Id);
                    model.DocumentName = detail.DocumentOriginalPath;
                }
                model.CreateDate = String.Format("{0:ddd,dd MMM yyyy hh:mm tt}", data.CreatedDate);
                model.IpAddress = data.IpAddress;
                return PartialView("_partialAddEmployeeSignature", model);

            }

        }
        public ActionResult SignedSignature(int Id)
        {
            EmployeeSignatureViewModel model = new EmployeeSignatureViewModel();
            var data = _EmployeeDocument.getDocumentSignatureListByDocumentId(Id);
            model.Id = data.Id;
            model.DocumentID = data.DocumentID;
            if (data.DocumentID != 0)
            {
                var detail = _EmployeeDocument.getDocumentDetailsById(data.DocumentID);
                model.DocumentName = detail.DocumentOriginalPath;
            }
            model.CreateDate = String.Format("{0:ddd,dd MMM yyyy hh:mm tt}", data.CreatedDate);
            model.IpAddress = data.IpAddress;
            model.SignatureText = data.SignatureText;
            return PartialView("_partialShowEmployeeSignature", model);
        }
        public ActionResult SaveSignature(EmployeeSignatureViewModel datamodel) 
        {
            bool save = _EmployeeDocument.SaveSignature(datamodel, SessionProxy.UserId);

            if (save)
            {
                return Json("True", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);

            }

        }

        #endregion
    }
}