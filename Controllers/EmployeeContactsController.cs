using HRTool.CommanMethods.Resources;
using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models.Resources;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HRTool.CommanMethods;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class EmployeeContactsController : Controller
    {


        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        EmployeeContactMethod _EmployeeContactMethod = new EmployeeContactMethod();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        EmployeeMethod _employeeMethod = new EmployeeMethod();


        #endregion

        [Authorize]
        public ActionResult Index(int EmployeeId)
        {
            EmployeeContactViewModel model = new EmployeeContactViewModel();
            model.Id = EmployeeId;
            return View(model);
        }
        public ActionResult ContectRecordRecord(int EmployeeId)
        {
            EmployeeContactViewModel model = _EmployeeContactMethod.employeeDetailsById(EmployeeId);
            return PartialView("_PartialEmployeeContactsRecord", model);
        }
        public EmployeeContactViewModel returnEmergencyContactList(int Id)
        {
            EmployeeContactViewModel model = new EmployeeContactViewModel();
            model.Id = Id;
            var data = _db.Employe_EmergencyContacts.Where(x => x.EmployeeID == Id && x.Archived ==false).ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    var value = _otherSettingMethod.getSystemListValueById(item.Relationship);
                    EmergencyContactsViewModel dd = new EmergencyContactsViewModel();
                    dd.Id = item.Id;
                    dd.Name = item.Name;
                    dd.EmployeeId = item.EmployeeID;
                    if (value != null)
                    {
                        dd.RelationshipValue = value.Value;
                    }
                    dd.Telephone = item.Telephone;
                    model.EmergencyContactsList.Add(dd);

                }
            }
            return model;

        }
        public ActionResult EmergencyContactsList(int Id)
        {
            EmployeeContactViewModel Model = returnEmergencyContactList(Id);
            return PartialView("_partialEmergencyContactList", Model);
        }
        public ActionResult AddEditEmergencyContacts(int Id,int EmployeeID)
        {
            EmergencyContactsViewModel model = new EmergencyContactsViewModel();
            model.Id = Id;
            model.EmployeeId = EmployeeID;
            model.RelationshipList.Add(new SelectListItem() { Text = "-- Select Relationship --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Relationship List"))
            {
                model.RelationshipList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }

            if (Id > 0)
            {
                var data = _EmployeeContactMethod.getEmergencyContactById(Id);

                model.Id = data.Id;
                model.EmployeeId = data.EmployeeID;
                model.Name = data.Name;
                model.Relationship = data.Relationship;
                if (data.Relationship > 0)
                {
                    var relationship = _otherSettingMethod.getSystemListValueById(data.Relationship);
                    if (relationship != null)
                        model.RelationshipValue = relationship.Value;
                }
                model.Postcode = data.PostCode;
                model.Address = data.Address;
                model.Mobile = data.Mobile;
                model.Telephone = data.Telephone;
                model.Mobile = data.Mobile;
                model.Comments = data.Comments;

            }

            return PartialView("_partialAddEditEmergencyContact", model);

        }
        public ActionResult DeleteEmergencyContact(int Id, int EmployeeId)
        {
            var data = _db.Employe_EmergencyContacts.Where(x => x.Id == Id).FirstOrDefault();
            data.Archived = true;
            data.LastModified = DateTime.Now;
            data.UserIDLastModifiedBy = SessionProxy.UserId;
            _db.SaveChanges();

            EmployeeContactViewModel Model = returnEmergencyContactList(EmployeeId);
            return PartialView("_partialEmergencyContactList", Model);
        }
        public ActionResult SaveEmergencyContacts(EmergencyContactsViewModel model)
        {

            _EmployeeContactMethod.saveEmergencyContact(model, SessionProxy.UserId);

          //  EmployeeContactViewModel Model = returnEmergencyContactList(model.EmployeeId);
           // return PartialView("_partialEmergencyContactList", Model);
            EmployeeContactViewModel models =_EmployeeContactMethod.employeeDetailsById(model.EmployeeId);
            return PartialView("_PartialEmployeeContactsRecord", models);

        }
        public ActionResult BindStateDropdown(int countryId)
        {
            try
            {
                var state = _employeeMethod.BindStateDropdown(countryId);
                return Json(state, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }
        public ActionResult BindCityDropdown(int stateId)
        {
            try
            {
                var city = _employeeMethod.BindCityDropdown(stateId);
                return Json(city, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }

        }
        public ActionResult BindAirportDropdown(int countryId)
        {
            try
            {
                var state = _employeeMethod.BindAirportDropdown(countryId);
                return Json(state, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }
        public ActionResult UpdateContactsRecord(EmployeeContactViewModel model)
        {
            _EmployeeContactMethod.SaveContactSet(model);
            model = _EmployeeContactMethod.employeeDetailsById(model.Id);
            return PartialView("_PartialEmployeeContactsRecord", model);
        }
    }
}