using HRTool.CommanMethods.Admin;
using HRTool.CommanMethods.Resources;
using HRTool.CommanMethods.RolesManagement;
using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.IO;
using HRTool.CommanMethods;
using static HRTool.CommanMethods.Enums;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class AdminDocumentController : Controller
    {
        #region Constant
        EvolutionEntities _db = new EvolutionEntities();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        CompanyStructureMethod _CompanyStructureMethod = new CompanyStructureMethod();
        AdminTMSMethod _AdminTMSMethod = new AdminTMSMethod();
        TMSSettingsMethod _TMSSettingsMethod = new TMSSettingsMethod();
        EmployeeMethod _employeeMethod = new EmployeeMethod();
        RolesManagementMethod _RolesManagementMethod = new RolesManagementMethod();
        AdminDocumentMethod _AdminDocumentMethod = new AdminDocumentMethod();
        #endregion
        // GET: /AdminDocument/
        [Authorize]
        public ActionResult Index()
        {
            List<AdminDocumentViewModel> model = new List<AdminDocumentViewModel>();
            var data = _AdminDocumentMethod.getAllDocumentList();
            int EmpId = SessionProxy.UserId;
            var empData = _db.AspNetUsers.Where(x => x.Id == EmpId).FirstOrDefault();
            
            if (data.Count > 0)
            {
                foreach (var details in data)
                {
                    if (!string.IsNullOrEmpty(details.Description) || !string.IsNullOrEmpty(details.DocumentPath))
                    {
                        AdminDocumentViewModel datamodel = new AdminDocumentViewModel();
                        if (empData != null)
                        {
                            if (empData.SSOID.Contains('C'))
                            {
                                datamodel.flag = 1;  
                            }
                        }
                        datamodel.Type = "Document";
                        datamodel.Id = details.Id;
                        datamodel.Description = details.Description;
                        datamodel.DocumentOriginalPath = details.DocumentOriginalPath;
                        datamodel.DocumentPath = details.DocumentPath;
                        datamodel.Category = (int)details.Category;
                        if (details.Category != 0)
                        {
                            var res = _otherSettingMethod.getSystemListValueById((int)details.Category);
                            if(res != null)
                            datamodel.CategoryName = res.Value;
                        }
                        datamodel.BusinessID = (int)details.BusinessID;
                        if (details.BusinessID != 0)
                        {
                            var buz = _CompanyStructureMethod.getBusinessListById((int)details.BusinessID);
                            if(buz != null)
                            {
                                datamodel.Business = buz.Name;
                            }
                        }
                        datamodel.DivisionID = (int)details.DivisionID;
                        if (details.DivisionID != 0)
                        {
                            var div = _CompanyStructureMethod.getDivisionById((int)details.DivisionID);
                            if(div != null)
                            datamodel.Division = div.Name;
                        }
                        datamodel.PoolID = (int)details.PoolID;
                        if (details.PoolID != 0)
                        {
                            var pol = _CompanyStructureMethod.getPoolsListById((int)details.PoolID);
                            if(pol != null)
                            datamodel.Pool = pol.Name;
                        }
                        datamodel.FunctionID = (int)details.FunctionID;
                        if (details.FunctionID != 0)
                        {
                            var fun = _CompanyStructureMethod.getFunctionsListById((int)details.FunctionID);
                            if(fun != null)
                            datamodel.Function = fun.Name;
                        }
                        datamodel.CreateDate = String.Format("{0:dd-MM-yyyy}", details.CreatedDate);
                        model.Add(datamodel);
                    }
                    else if(!string.IsNullOrEmpty(details.LinkDisplayText) || !string.IsNullOrEmpty(details.LinkURL))
                    {
                        AdminDocumentViewModel datamodel = new AdminDocumentViewModel();
                        if (empData != null)
                        {
                            if (empData.SSOID.Contains('C'))
                            {
                                datamodel.flag = 1;
                            }
                        }
                        datamodel.Type = "Link";
                        datamodel.Id = details.Id;
                        datamodel.LinkDisplayText = details.LinkDisplayText;
                        datamodel.LinkURL = details.LinkURL;
                        //datamodel.DocumentPath = details.DocumentPath;
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
                            datamodel.Business = buz.Name;
                        }
                        datamodel.DivisionID = (int)details.DivisionID;
                        if (details.DivisionID != 0)
                        {
                            var div = _CompanyStructureMethod.getDivisionById((int)details.DivisionID);
                            datamodel.Division = div.Name;
                        }
                        datamodel.PoolID = (int)details.PoolID;
                        if (details.PoolID != 0)
                        {
                            var pol = _CompanyStructureMethod.getPoolsListById((int)details.PoolID);
                            datamodel.Pool = pol.Name;
                        }
                        datamodel.FunctionID = (int)details.FunctionID;
                        if (details.FunctionID != 0)
                        {
                            var fun = _CompanyStructureMethod.getFunctionsListById((int)details.FunctionID);
                            datamodel.Function = fun.Name;
                        }
                        datamodel.CreateDate = String.Format("{0:dd-MM-yyyy}", details.CreatedDate);
                        model.Add(datamodel);
                    }
                }

            }

            return View(model);
        }
        public AdminDocumentViewModel returnList(int Id)
        {
            AdminDocumentViewModel datamodel = new AdminDocumentViewModel();
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Document Type"))
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
            }
            else
            {
                var data = _AdminDocumentMethod.getDocumentDetailsById(Id);
                datamodel.Id = data.Id;
                datamodel.DocumentOriginalPath = data.DocumentOriginalPath;
                datamodel.DocumentPath = data.DocumentPath;
                datamodel.LinkDisplayText = data.LinkDisplayText;
                datamodel.LinkURL = data.LinkURL;
                datamodel.Description = data.Description;
                datamodel.IpAddress = data.IpAddress;
                datamodel.BusinessID =(int)data.BusinessID;
                datamodel.DivisionID = (int)data.DivisionID;
                if (data.DivisionID != 0 && data.BusinessID != 0) 
                {
                    datamodel.DivisionList = bindDiv((int)data.BusinessID);
                    datamodel.PoolList=poolDiv((int)data.DivisionID);
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
             //   datamodel.ManagerID = data.ManagerID;
                datamodel.SpecificCustomer = data.SpecificCustomer;
             //   datamodel.CustomerID = data.CustomerID;
                datamodel.SignatureRequire = data.SignatureRequire;
                int CmpId = Convert.ToInt32(data.WorkerID);
                if (CmpId != 0)
                {
                    var Employee = _db.AspNetUsers.Where(x => x.Id == CmpId && x.Archived==false).FirstOrDefault();
                    if (Employee != null)
                    {
                        datamodel.SpecificWorkerName = Employee.FirstName + " " + Employee.LastName + "-" + Employee.SSOID;
                        datamodel.WorkerID = data.WorkerID;
                    }
                }
                int ManId = Convert.ToInt32(data.ManagerID);
                if (ManId != 0)
                {
                    var Employee = _db.AspNetUsers.Where(x => x.Id == ManId && x.Archived == false).FirstOrDefault();
                    if (Employee != null)
                    {
                        datamodel.SpecificManagerName = Employee.FirstName + " " + Employee.LastName + " " + Employee.SSOID;
                        datamodel.ManagerID = data.ManagerID;
                    }
                }
                int CustId = Convert.ToInt32(data.CustomerID);
                if (CustId != 0 )
                {
                    var Employee = _db.AspNetUsers.Where(x => x.Id == CustId && x.Archived == false).FirstOrDefault();
                    if (Employee != null)
                    {
                        datamodel.SpecificCustomerName = Employee.FirstName + " " + Employee.LastName + " " + Employee.SSOID;
                        datamodel.CustomerID = data.CustomerID;
                    }
                }
            }
            return datamodel;
        }
        public ActionResult getCustomer_WorkertList()
        {
            AdminDocumentViewModel datamodel = new AdminDocumentViewModel();
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
            return Json(datamodel,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddDocument(int Id)
        {
            AdminDocumentViewModel Model = returnList(Id);
            return PartialView("_partialAddEditDocumentView", Model);
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
                FilePath = ConfigurationManager.AppSettings["AdminDocument"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
        }
        public ActionResult SaveDocument(AdminDocumentViewModel model) 
        {

            bool save = _AdminDocumentMethod.SaveDocumentData(model, SessionProxy.UserId);

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

            bool save = _AdminDocumentMethod.DeleteDocumentData(Id, SessionProxy.UserId);

            if (save)
            {
                return Json("True", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);

            }

        }

        //public ActionResult PendingSignatuer(int Id) 
        //{
        //    var save = _AdminDocumentMethod.SaveDocumentData(Id, User.Identity.GetUserId());

        //    if (save)
        //    {
        //        return Json("True", JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json("Error", JsonRequestBehavior.AllowGet);

        //    }
        //}
    }
}