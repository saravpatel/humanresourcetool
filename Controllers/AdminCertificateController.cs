using HRTool.CommanMethods.Admin;
using HRTool.CommanMethods.Resources;
using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HRTool.Models.Admin;
using System.Configuration;
using System.IO;
using System.Web.Script.Serialization;
using HRTool.CommanMethods;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class AdminCertificateController : Controller
    {

        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        EmployeeMethod _employeeMethod = new EmployeeMethod();
        AdminCertificateMethod _adminCertificateMethod = new AdminCertificateMethod();
        #endregion
        //
        // GET: /AdminCertificate/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CertificateMenu()
        {
            var userId = SessionProxy.UserId;
            var todayDate = DateTime.Now;
            AdminCertificateMenuViewModel model = new AdminCertificateMenuViewModel();
            // var getAllActiveCertificate = _adminCertificateMethod.getAllCertificate();
            var data = _db.AspNetUserRoles.Where(x => x.UserId == userId && x.RoleId == 1).ToList();
            var getAllActiveCertificate = _adminCertificateMethod.getAllCertificateDetailList().Where(x => x.ReportToEmployeeId == SessionProxy.UserId || x.EmployeeRelationUSerId == SessionProxy.UserId);            
            var customerData = _db.AspNetUsers.Where(x => x.Archived == false && x.Id == userId).FirstOrDefault();
            if (data.Count>0 && data!=null)
            {
                getAllActiveCertificate = _adminCertificateMethod.getAllCertificateDetailList().ToList();
            }
            else if (customerData != null && customerData.SSOID.StartsWith("C"))
            {
                string[] EmpId = customerData.CustomerCareID.Split(',');
                for (int i = 0; i < EmpId.Length; i++)
                {
                    int EmpID = Convert.ToInt32(EmpId[i]);
                    getAllActiveCertificate = _adminCertificateMethod.getAllCertificateDetailList().Where(x => x.AssignTo == EmpID).ToList();
                }

            }
            else
            {
                getAllActiveCertificate = _adminCertificateMethod.getAllCertificateDetailList().Where(x => x.ReportToEmployeeId == SessionProxy.UserId || x.EmployeeRelationUSerId == SessionProxy.UserId);
            }
            var myCertificate = getAllActiveCertificate.Where(x => x.AssignTo == userId).ToList();
            var expiredCertificate = getAllActiveCertificate.Where(x => x.ExpiringDate.Value.Date < todayDate.Date).ToList();
            var newCertificate = getAllActiveCertificate.Where(x => x.ValidFrom.Value.Date == todayDate.Date).ToList();
            var expiringToday = getAllActiveCertificate.Where(x => x.ExpiringDate.Value.Date == todayDate.Date).ToList();
            var upcomingCertificate = getAllActiveCertificate.Where(x => x.ValidFrom.Value.Date > todayDate.Date).ToList();
            var validCertificate = getAllActiveCertificate.Where(x => x.certificateStatus == "Complete").ToList();
            model.MyCertificate = myCertificate.Count();
            model.Expired = expiredCertificate.Count();
            model.New = newCertificate.Count();
            model.ExpiringToday = expiringToday.Count();
            model.AllCertificate = getAllActiveCertificate.Count();
            model.Upcoming = upcomingCertificate.Count();
            model.Valid = validCertificate.Count();
            return PartialView("_partialAdminCertificateMenu", model);
        }

        public ActionResult List(string search)
        {
            List<AdminCertificateIdViewModel> model = modelLsit(search);
            return PartialView("_partialAdminCertificateList", model);
        }

        public List<AdminCertificateIdViewModel> modelLsit(string search)
        {
            
            List<AdminCertificateIdViewModel> model = new List<AdminCertificateIdViewModel>();
            int AdminuserId = SessionProxy.UserId;
            var CustomerData = _db.AspNetUsers.Where(x => x.Id == AdminuserId && x.Archived == false).FirstOrDefault();
            if (CustomerData != null)
            {
                if (CustomerData.SSOID.StartsWith("C") && CustomerData.SSOID.StartsWith("C"))
                {
                    string[] EmpId = CustomerData.CustomerCareID.Split(',');
                    for (int i = 0; i < EmpId.Length; i++)
                    {
                        int EmpID = Convert.ToInt32(EmpId[i]);
                        var listData = _adminCertificateMethod.getCerificateDetailsForCutomer().Where(x => x.AssignTo == EmpID).ToList();
                        var userId = SessionProxy.UserId;
                        var todayDate = DateTime.Now;
                       AdminCertificateIdViewModel m = new AdminCertificateIdViewModel();
                     
                        if (!string.IsNullOrEmpty(search))
                        {
                            if (search == "MyCertificate")
                            {
                                listData = listData.Where(x => x.AssignTo == userId).ToList();
                            }
                            if (search == "Expired")
                            {
                                listData = listData.Where(x => x.ExpiringDate.Value.Date < todayDate.Date).ToList();
                            }
                            if (search == "New")
                            {
                                listData = listData.Where(x => x.ValidFrom.Value.Date == todayDate.Date).ToList();
                            }
                            if (search == "ExpiringToday")
                            {
                                listData = listData.Where(x => x.ExpiringDate.Value.Date == todayDate.Date).ToList();
                            }
                            if (search == "Upcoming")
                            {
                                listData = listData.Where(x => x.ValidFrom.Value.Date > todayDate.Date).ToList();
                            }
                            if (search == "Valid")
                            {
                                listData = listData.Where(x => x.Status == 4089).ToList();
                            }
                        }
                        foreach (var item in listData)
                        {
                            
                            var aspData = _db.AspNetUsers.Where(x => x.Archived == false && x.Id == item.AssignTo).FirstOrDefault();
                            var employeeRelastionData = _db.EmployeeRelations.Where(x => x.UserID == item.AssignTo && x.IsActive==true).FirstOrDefault();
                            var PoolNname = _db.Pools.Where(x => x.Id == employeeRelastionData.PoolID && x.Archived==false).FirstOrDefault();
                            var functionName = _db.Functions.Where(x => x.Id == employeeRelastionData.FunctionID && x.Archived == false).FirstOrDefault();
                            var jobTitle = _db.SystemListValues.Where(x => x.Id == aspData.JobTitle).FirstOrDefault();
                            m.Id = item.Id;
                            var employeeDetail = _employeeMethod.getEmployeeById(item.AssignTo);
                            m.AssignTo = string.Format("{0} {1} - {2}", employeeDetail.FirstName, employeeDetail.LastName, employeeDetail.SSOID);
                            m.Name = item.Name;
                            var CertificateType = _otherSettingMethod.getSystemListValueById((int)item.Type);
                            m.Type = CertificateType.Value;
                            m.JobTitle=jobTitle.Value;
                            m.Pool = PoolNname.Name;
                            m.Function = functionName.Name;
                            m.ExpiryDate = String.Format("{0:dd-MMM-yyy}", item.ExpiringDate);
                            m.AlertBeforeDays = (int)item.AlertBeforeDays;
                            m.Body = item.Body;
                            m.Number = item.Number;
                            var relation = _employeeMethod.getEmployeeById(item.InRelationTo);
                            m.InRelationTo = string.Format("{0} {1} - {2}", relation.FirstName, relation.LastName, relation.SSOID);
                            if (item.Status > 0)
                            {
                                var status = _otherSettingMethod.getSystemListValueById((int)item.Status);
                                m.Status = status.Value;
                            }
                            model.Add(m);
                        }
                    }
                }
                else
                {
                    var data = _db.AspNetUserRoles.Where(x => x.UserId == AdminuserId && x.RoleId == 1).ToList();
                    var listData = _adminCertificateMethod.getAllCertificateDetailList().Where(x => x.ReportToEmployeeId == SessionProxy.UserId || x.EmployeeRelationUSerId == SessionProxy.UserId);

                    if (data.Count > 0 && data != null)
                    {
                        listData = _adminCertificateMethod.getAllCertificateDetailList().ToList();
                    }
                    else
                    {
                        listData = _adminCertificateMethod.getAllCertificateDetailList().Where(x => x.ReportToEmployeeId == SessionProxy.UserId || x.EmployeeRelationUSerId == SessionProxy.UserId);
                    }
                    var userId = SessionProxy.UserId;
                    var todayDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(search))
                    {
                        if (search == "MyCertificate")
                        {
                            listData = listData.Where(x => x.AssignTo == userId).ToList();
                        }
                        if (search == "Expired")
                        {
                            listData = listData.Where(x => x.ExpiringDate.Value.Date < todayDate.Date).ToList();
                        }
                        if (search == "New")
                        {
                            listData = listData.Where(x => x.ValidFrom.Value.Date == todayDate.Date).ToList();
                        }
                        if (search == "ExpiringToday")
                        {
                            listData = listData.Where(x => x.ExpiringDate.Value.Date == todayDate.Date).ToList();
                        }
                        if (search == "Upcoming")
                        {
                            listData = listData.Where(x => x.ValidFrom.Value.Date > todayDate.Date).ToList();
                        }
                        if (search == "Valid")
                        {
                            listData = listData.Where(x => x.certificateStatus == "Complete").ToList();
                        }
                    }

                    foreach (var item in listData)
                    {
                        AdminCertificateIdViewModel m = new AdminCertificateIdViewModel();
                        m.Id = item.Id;
                        var employeeDetail = _employeeMethod.getEmployeeById(item.AssignTo);
                        m.AssignTo = string.Format("{0} {1} - {2}", employeeDetail.FirstName, employeeDetail.LastName, employeeDetail.SSOID);
                        m.Name = item.Name;
                        var CertificateType = _otherSettingMethod.getSystemListValueById((int)item.Type);
                        m.Type = CertificateType.Value;
                        m.JobTitle = item.JobTitle;
                        m.Pool = item.PoolName;
                        m.Function = item.FunctionName;
                        m.ExpiryDate = String.Format("{0:dd-MMM-yyy}", item.ExpiringDate);
                        m.AlertBeforeDays = (int)item.AlertBeforeDays;
                        m.Body = item.Body;
                        m.Number = item.Number;
                        var relation = _employeeMethod.getEmployeeById(item.InRelationTo);
                        m.InRelationTo = string.Format("{0} {1} - {2}", relation.FirstName, relation.LastName, relation.SSOID);
                        if (item.Status > 0)
                        {
                            var status = _otherSettingMethod.getSystemListValueById((int)item.Status);
                            m.Status = status.Value;
                        }
                        model.Add(m);
                    }
                }
            }
            return model;
        }

        public ActionResult AddEditCertificate(int Id)
        {
            AdminCertificateIdViewModel model = new AdminCertificateIdViewModel();
            model.Id = Id;
            int currUserId = SessionProxy.UserId;
            var aspData = _db.AspNetUsers.Where(x => x.Id == currUserId && x.Archived == false).FirstOrDefault();
            if(aspData!=null)
            {
                if(aspData.SSOID.StartsWith("C"))
                {
                    model.Flag = 1;
                }
            }
            else
            {
                model.Flag = 0; 
            }
    
            model.TypeList.Add(new SelectListItem() { Text = "-- Select Certificate Types --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Certificate Types"))
            {
                model.TypeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            
            var employeeList = _employeeMethod.GetAllEmployeeList();

            model.AssignToList.Add(new SelectListItem() { Text = "-- Select Assign To --", Value = "0" });
            foreach (var item in employeeList)
            {
                model.AssignToList.Add(new SelectListItem() { Text = string.Format("{0} {1} - {2}", item.FirstName, item.LastName, item.SSOID), Value = item.Id.ToString() });
            }

            model.InRelationToList.Add(new SelectListItem() { Text = "-- Select In Relation To --", Value = "0" });
            foreach (var item in employeeList)
            {
                model.InRelationToList.Add(new SelectListItem() { Text = string.Format("{0} {1} - {2}", item.FirstName, item.LastName, item.SSOID), Value = item.Id.ToString() });
            }

            model.StatusList.Add(new SelectListItem() { Text = "-- Select Status --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Certificate Status"))
            {
                model.StatusList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }

            if (model.Id > 0)
            {
                var CertificateDetail = _adminCertificateMethod.getCertificateById(Id);
                model.Name = CertificateDetail.Name;
                model.TypeId= (int)CertificateDetail.Type;
                model.Body = CertificateDetail.Body;
                model.Number = CertificateDetail.Number;
               // model.AssignToId = CertificateDetail.AssignTo;
              //  model.InRelationToId = CertificateDetail.InRelationTo;
                model.ValidFrom = String.Format("{0:dd-MM-yyy}", CertificateDetail.ValidFrom);
                model.ExpiryDate = String.Format("{0:dd-MM-yyy}", CertificateDetail.ExpiringDate);
                model.StatusId = (int)CertificateDetail.Status;
                model.AlertBeforeDays = (int)CertificateDetail.AlertBeforeDays;
                model.Description = CertificateDetail.Description;
                model.Mandatory = (bool)CertificateDetail.Mandatory;
                model.Validate = (bool)CertificateDetail.Validate;
                var CertificateDoument = _adminCertificateMethod.getCertificateDocumentsByCertificateId(Id);
                foreach (var item in CertificateDoument)
                {
                    CertificateDocumentViewModel docModel = new CertificateDocumentViewModel();
                    docModel.Id = item.Id;
                    docModel.originalName = item.OriginalName;
                    docModel.newName = item.NewName;
                    docModel.description = item.Description;
                    model.DocumentList.Add(docModel);
                }
                var data = _db.AspNetUsers.Where(x => x.Id == CertificateDetail.AssignTo && x.Archived==false).FirstOrDefault();
                if(data!=null)
                {
                    if (data.FirstName != null && data.LastName != null && data.SSOID != null)
                    {
                        model.AssignToEmployeeName = data.FirstName + data.LastName + "-" + data.SSOID;
                        model.AssignToId = CertificateDetail.AssignTo;
                    }
                }
                var relationToId= _db.AspNetUsers.Where(x => x.Id == CertificateDetail.InRelationTo && x.Archived == false).FirstOrDefault();
                if (relationToId != null)
                {
                    if (relationToId.FirstName != null && relationToId.LastName != null && relationToId.SSOID != null)
                    {
                        model.AssignInRelationTo = relationToId.FirstName + relationToId.LastName + "-" + relationToId.SSOID;
                        model.InRelationToId = CertificateDetail.InRelationTo;
                    }
                }
                

            }

            return PartialView("_partialAddAdminCertificate", model);
        }

        [HttpPost]
        public ActionResult ImageData()
        {
            string FilePath = string.Empty;
            string fileName = string.Empty;
            string originalFileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                FilePath = ConfigurationManager.AppSettings["AdminCertificate"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
        }
        public ActionResult getEmployeeData()
        {
            AdminCertificateIdViewModel model = new AdminCertificateIdViewModel();
            int userId = SessionProxy.UserId;
            var data = _db.AspNetUserRoles.Where(x => x.UserId == userId && x.RoleId == 1).ToList();
            if (data.Count > 0 && data != null)
            {
                foreach (var item in _employeeMethod.GetAllResourceEmployeeList().Where(x => x.SSOID.StartsWith("W") && x.Archived == false).ToList())
                {
                    model.AssignToList.Add(new SelectListItem() { Text = string.Format("{0} {1} - {2}", item.FirstName, item.LastName, item.SSOID), Value = item.Id.ToString() });
                }
            }
            else
            {
                foreach (var item in _employeeMethod.getReportToEmployee(SessionProxy.UserId))
                {
                    model.AssignToList.Add(new SelectListItem() { Text = string.Format("{0} {1} - {2}", item.FirstName, item.LastName, item.ssoId), Value = item.EmployeeId.ToString() });
                }
            }
           var employeeList = _employeeMethod.GetAllEmployeeList();
           // foreach (var item in employeeList)
           // {
           //     model.AssignToList.Add(new SelectListItem() { Text = string.Format("{0} {1} - {2}", item.FirstName, item.LastName, item.SSOID), Value = item.Id.ToString() });
           // }
            foreach (var item in employeeList)
            {
                model.InRelationToList.Add(new SelectListItem() { Text = string.Format("{0} {1} - {2}", item.FirstName, item.LastName, item.SSOID), Value = item.Id.ToString() });
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveData(AdminCertificateIdViewModel model)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<CertificateDocumentViewModel> listDocument = js.Deserialize<List<CertificateDocumentViewModel>>(model.jsonDocumentListString);

            _adminCertificateMethod.SaveData(model, listDocument, SessionProxy.UserId);

            List<AdminCertificateIdViewModel> returnModel = modelLsit(model.FilterSearch);
            return PartialView("_partialAdminCertificateList", returnModel);
        }

        public ActionResult DeleteData(int Id, string search)
        {
            var CertificateData = _db.certificates.Where(x => x.Id == Id).FirstOrDefault();
            CertificateData.Archived = true;
            CertificateData.LastModified = DateTime.Now;
            CertificateData.LastModifiedBy = SessionProxy.UserId;
            _db.SaveChanges();

            var CertificateDocumentList = _db.certificate_document.Where(x => x.CertificateId == Id).ToList();
            foreach (var item in CertificateDocumentList)
            {
                item.Archived = true;
                item.LastModified = DateTime.Now;
                item.UserIDLastModifiedBy = SessionProxy.UserId;
                _db.SaveChanges();
            }

            List<AdminCertificateIdViewModel> returnModel = modelLsit(search);
            return PartialView("_partialAdminCertificateList", returnModel);
        }
	}
}