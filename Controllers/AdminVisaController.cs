using HRTool.CommanMethods.Resources;
using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models.Admin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using HRTool.CommanMethods.Admin;
using HRTool.CommanMethods;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class AdminVisaController : Controller
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        EmployeeMethod _employeeMethod = new EmployeeMethod();
        AdminVisaMethod _adminVisaMethod = new AdminVisaMethod();
        #endregion
        //
        // GET: /AdminVisa/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult VisaMenu()
        {
            var userId = SessionProxy.UserId;
            var todayDate = DateTime.Now;
            AdminVisaMenuViewModel model = new AdminVisaMenuViewModel();
            //var getAllActiveVisa = _adminVisaMethod.getAllVisa();
            var getAllActiveVisa = _adminVisaMethod.getVisaDetail().Where(x => x.ReportToEmployeeId == userId || x.EmployeeRelationUSerId == userId).ToList();
            var data = _db.AspNetUserRoles.Where(x => x.UserId == userId && x.RoleId == 1).ToList();
            var customerData = _db.AspNetUsers.Where(x => x.Archived == false && x.Id == userId).FirstOrDefault();
            if (data.Count>0 && data!=null)
            {
                getAllActiveVisa = _adminVisaMethod.getVisaDetail();
            }
            else if(customerData!=null && customerData.SSOID.StartsWith("C"))
            {
             
                    string[] EmpId = customerData.CustomerCareID.Split(',');
                    for (int i = 0; i < EmpId.Length; i++)
                    {
                        int EmpID = Convert.ToInt32(EmpId[i]);
                        var vdata = _adminVisaMethod.getVisaDetail().Where(x => x.AssignedToEmployeeId == EmpID).ToList();
                        foreach (var item in vdata)
                        {
                            getAllActiveVisa.Add(item);
                        }

                    }
               
            }
            else
            {
                getAllActiveVisa = _adminVisaMethod.getVisaDetail().Where(x=>x.ReportToEmployeeId ==userId || x.EmployeeRelationUSerId==userId).ToList();
            }
            var myVisa = getAllActiveVisa.Where(x => x.AssignedToEmployeeId == userId).ToList();
            var expiredVisa = getAllActiveVisa.Where(x => x.DueDate.Value.Date < todayDate.Date).ToList();
            var newVisa = getAllActiveVisa.Where(x => x.Date.Value.Date == todayDate.Date).ToList();
            var expiringToday = getAllActiveVisa.Where(x => x.DueDate.Value.Date == todayDate.Date).ToList();
            var upcomingVisa = getAllActiveVisa.Where(x => x.Date.Value.Date > todayDate.Date).ToList();
            var validVisa = getAllActiveVisa.Where(x => x.VisaStatus == "Complete").ToList();
            model.MyVisa = myVisa.Count();
            model.Expired = expiredVisa.Count();
            model.New = newVisa.Count();
            model.ExpiringToday = expiringToday.Count();
            model.AllVisa = getAllActiveVisa.Count();
            model.Upcoming = upcomingVisa.Count();
            model.Valid = validVisa.Count();
            return PartialView("_partialAdminVisaMenu", model);
        }

        public ActionResult List(string search)
        {
            List<AdminVisaViewModel> model = modelLsit(search);
            return PartialView("_partialAdminVisaList", model);
        }

        public List<AdminVisaViewModel> modelLsit(string search)
        {
            List<AdminVisaViewModel> model = new List<AdminVisaViewModel>();
            //var listData = _adminVisaMethod.getAllVisa();
            int AdminuserId = SessionProxy.UserId;
            var customerData = _db.AspNetUsers.Where(x => x.Archived == false && x.Id == AdminuserId).FirstOrDefault();
            if (customerData != null && customerData.SSOID.StartsWith("C"))
            {
                var userId = SessionProxy.UserId;
                var todayDate = DateTime.Now;
                
                    if (customerData.CustomerCareID != null && customerData.CustomerCareID != "")
                    {
                        string[] EmpId = customerData.CustomerCareID.Split(',');
                        for (int i = 0; i < EmpId.Length; i++)
                        {
                            int EmpID = Convert.ToInt32(EmpId[i]);
                            var listData = _adminVisaMethod.getVisaDetail().Where(x => x.AssignedToEmployeeId == EmpID).ToList();
                            if (!string.IsNullOrEmpty(search))
                            {
                                if (search == "MyVisa")
                                {
                                    listData = listData.Where(x => x.AssignedToEmployeeId == userId).ToList();
                                }
                                if (search == "Expired")
                                {
                                    listData = listData.Where(x => x.DueDate.Value.Date < todayDate.Date).ToList();
                                }
                                if (search == "New")
                                {
                                    listData = listData.Where(x => x.Date.Value.Date == todayDate.Date).ToList();
                                }
                                if (search == "ExpiringToday")
                                {
                                    listData = listData.Where(x => x.DueDate.Value.Date == todayDate.Date).ToList();
                                }
                                if (search == "Upcoming")
                                {
                                    listData = listData.Where(x => x.Date.Value.Date > todayDate.Date).ToList();
                                }
                                if (search == "Valid")
                                {
                                    listData = listData.Where(x => x.VisaStatus == "Complete").ToList();
                                }
                            }
                            foreach (var item in listData)
                            {
                                AdminVisaViewModel m = new AdminVisaViewModel();
                                m.Id = item.Id;
                                var employeeDetail = _employeeMethod.getEmployeeById(item.AssignedToEmployeeId);
                                m.AssignTo = string.Format("{0} {1} - {2}", employeeDetail.FirstName, employeeDetail.LastName, employeeDetail.SSOID);
                                var country = _otherSettingMethod.getCountryById((int)item.Country);
                                m.CountryName = country.Name;
                                var visaType = _otherSettingMethod.getSystemListValueById((int)item.VisaType);
                                m.VisaType = visaType.Value;
                                m.ExpiryDate = String.Format("{0:dd-MMM-yyy}", item.DueDate);
                                m.AlertBeforeDays = (int)item.AlertBeforeDays;
                                m.VisaNumber = item.Number;
                                m.Pool = item.PoolName;
                                m.Function = item.FunctionName;
                                var agency = _otherSettingMethod.getSystemListValueById((int)item.ServiceAgency);
                                m.ServiceAgency = agency.Value;
                                var relation = _employeeMethod.getEmployeeById(item.RelationToCSEmployeeID);
                                m.InRelationTo = string.Format("{0} {1} - {2}", relation.FirstName, relation.LastName, relation.SSOID);
                                var status = _otherSettingMethod.getSystemListValueById((int)item.Status);
                                m.Status = status.Value;
                                model.Add(m);
                            }
                        }
                    }
               
            }
            else
            {
                var data = _db.AspNetUserRoles.Where(x => x.UserId == AdminuserId && x.RoleId == 1).ToList();
                var listData = _adminVisaMethod.getVisaDetail().Where(x => x.ReportToEmployeeId == SessionProxy.UserId || x.EmployeeRelationUSerId == SessionProxy.UserId).ToList();
                if (data.Count > 0 && data != null)
                {
                    listData = _adminVisaMethod.getVisaDetail().ToList();
                }
                else
                {
                    listData = _adminVisaMethod.getVisaDetail().Where(x => x.ReportToEmployeeId == SessionProxy.UserId || x.EmployeeRelationUSerId == SessionProxy.UserId).ToList();
                }
                var userId = SessionProxy.UserId;
                var todayDate = DateTime.Now;
                if (!string.IsNullOrEmpty(search))
                {
                    if (search == "MyVisa")
                    {
                        listData = listData.Where(x => x.AssignedToEmployeeId == userId).ToList();
                    }
                    if (search == "Expired")
                    {
                        listData = listData.Where(x => x.DueDate.Value.Date < todayDate.Date).ToList();
                    }
                    if (search == "New")
                    {
                        listData = listData.Where(x => x.Date.Value.Date == todayDate.Date).ToList();
                    }
                    if (search == "ExpiringToday")
                    {
                        listData = listData.Where(x => x.DueDate.Value.Date == todayDate.Date).ToList();
                    }
                    if (search == "Upcoming")
                    {
                        listData = listData.Where(x => x.Date.Value.Date > todayDate.Date).ToList();
                    }
                    if (search == "Valid")
                    {
                        listData = listData.Where(x => x.VisaStatus == "Complete").ToList();
                    }
                }
                foreach (var item in listData)
                {
                    AdminVisaViewModel m = new AdminVisaViewModel();
                    m.Id = item.Id;
                    var employeeDetail = _employeeMethod.getEmployeeById(item.AssignedToEmployeeId);
                    m.AssignTo = string.Format("{0} {1} - {2}", employeeDetail.FirstName, employeeDetail.LastName, employeeDetail.SSOID);
                    var country = _otherSettingMethod.getCountryById((int)item.Country);
                    m.CountryName = country.Name;
                    var visaType = _otherSettingMethod.getSystemListValueById((int)item.VisaType);
                    m.VisaType = visaType.Value;
                    m.ExpiryDate = String.Format("{0:dd-MMM-yyy}", item.DueDate);
                    m.AlertBeforeDays = (int)item.AlertBeforeDays;
                    m.VisaNumber = item.Number;
                    m.Pool = item.PoolName;
                    m.Function = item.FunctionName;
                    var agency = _otherSettingMethod.getSystemListValueById((int)item.ServiceAgency);
                    m.ServiceAgency = agency.Value;
                    var relation = _employeeMethod.getEmployeeById(item.RelationToCSEmployeeID);
                    if(relation != null)
                    {
                        m.InRelationTo = string.Format("{0} {1} - {2}", relation.FirstName, relation.LastName, relation.SSOID);
                    }
                    var status = _otherSettingMethod.getSystemListValueById((int)item.Status);
                    m.Status = status.Value;
                    model.Add(m);
                }
            }
            return model;
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

        public ActionResult AddEditVisa(int Id)
        {
            AdminVisaViewModel model = new AdminVisaViewModel();
            model.Id = Id;
            int currUserId = SessionProxy.UserId;
            var aspData = _db.AspNetUsers.Where(x => x.Id == currUserId && x.Archived == false).FirstOrDefault();
            if (aspData != null)
            {
                if (aspData.SSOID.StartsWith("C"))
                {
                    model.Flag = 1;
                }
            }
            else
            {
                model.Flag = 0;
            }
            model.CountryList.Add(new SelectListItem() { Text = "-- Select Country --", Value = "0" });
            foreach (var item in _otherSettingMethod.countryList())
            {
                model.CountryList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            model.VisaTypeList.Add(new SelectListItem() { Text = "-- Select Visa Type --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Visa Types"))
            {
                model.VisaTypeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            model.ServiceAgencyList.Add(new SelectListItem() { Text = "-- Select Service Agency --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Visa Service Agencies"))
            {
                model.ServiceAgencyList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
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
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Visa Status"))
            {
                model.StatusList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }

            if (model.Id > 0)
            {
                var visaDetail = _adminVisaMethod.getVisaById(Id);
                model.CountryId = (int)visaDetail.Country;
                model.VisaTypeId = (int)visaDetail.VisaType;
                model.ServiceAgencyId = (int)visaDetail.ServiceAgency;
                model.VisaNumber = visaDetail.Number;
              //  model.AssignToId = visaDetail.AssignedToEmployeeId;
                //model.InRelationToId = visaDetail.RelationToCSEmployeeID;
                model.ValidFrom = String.Format("{0:dd-MM-yyy}", visaDetail.Date);
                model.ExpiryDate = String.Format("{0:dd-MM-yyy}", visaDetail.DueDate);
                model.StatusId = (int)visaDetail.Status;
                model.AlertBeforeDays = (int)visaDetail.AlertBeforeDays;
                model.Description = visaDetail.Description;
                var visaDoument = _adminVisaMethod.getVisaDocumentsByVisaId(Id);
                foreach (var item in visaDoument)
                {
                    VisaDocumentViewModel docModel = new VisaDocumentViewModel();
                    docModel.Id = item.Id;
                    docModel.originalName = item.OriginalName;
                    docModel.newName = item.NewName;
                    docModel.description = item.Description;
                    model.DocumentList.Add(docModel);
                }
                var data = _db.AspNetUsers.Where(x => x.Id == visaDetail.AssignedToEmployeeId && x.Archived == false).FirstOrDefault();
                if (data != null)
                {
                    if (data.FirstName != null && data.LastName != null && data.SSOID != null)
                    {
                        model.AssignTo = data.FirstName + data.LastName + "-" + data.SSOID;
                        model.AssignToId = visaDetail.AssignedToEmployeeId;
                    }
                }
                var relationToId = _db.AspNetUsers.Where(x => x.Id == visaDetail.RelationToCSEmployeeID && x.Archived == false).FirstOrDefault();
                if (relationToId != null)
                {
                    if (relationToId.FirstName != null && relationToId.LastName != null && relationToId.SSOID != null)
                    {
                        model.InRelationTo = relationToId.FirstName + relationToId.LastName + "-" + relationToId.SSOID;
                        model.InRelationToId = visaDetail.RelationToCSEmployeeID;
                    }
                }
            }

            return PartialView("_partialAddAdminVisa", model);
        }

        [HttpPost]
        public ActionResult ImageData()
        {
            string FilePath = string.Empty;
            string fileName = string.Empty;
            string originalFileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                FilePath = ConfigurationManager.AppSettings["AdminVisa"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
        }

        public ActionResult SaveData(AdminVisaViewModel model)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<VisaDocumentViewModel> listDocument = js.Deserialize<List<VisaDocumentViewModel>>(model.jsonDocumentListString);

            _adminVisaMethod.SaveData(model, listDocument, SessionProxy.UserId);

            List<AdminVisaViewModel> returnModel = modelLsit(model.FilterSearch);
            return PartialView("_partialAdminVisaList", returnModel);
        }

        public ActionResult DeleteData(int Id, string search)
        {
            var visaData = _db.Visas.Where(x => x.Id == Id).FirstOrDefault();
            visaData.Archived = true;
            visaData.LastModified = DateTime.Now;
            visaData.CSUserIDLastModifiedBy = SessionProxy.UserId;
            _db.SaveChanges();

            var VisaDocumentList = _db.Visa_Document.Where(x => x.VisaId == Id).ToList();
            foreach (var item in VisaDocumentList)
            {
                item.Archived = true;
                item.LastModified = DateTime.Now;
                item.UserIDLastModifiedBy = SessionProxy.UserId;
                _db.SaveChanges();
            }

            List<AdminVisaViewModel> returnModel = modelLsit(search);
            return PartialView("_partialAdminVisaList", returnModel);
        }
    }
}