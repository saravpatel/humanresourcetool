using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models.Resources;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.CommanMethods.Resources
{

    public class EmployeeContactMethod
    {

        #region Constant
        EvolutionEntities _db = new EvolutionEntities();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        #endregion

        public EmployeeContactViewModel employeeDetailsById(int Id)
        {
            EmployeeContactViewModel Companysetting = new EmployeeContactViewModel();
            Companysetting.Id = Id;
            var model = _db.AspNetUsers.Where(x => x.Id == Id).FirstOrDefault();

            EmployeeAddressInfo AddressInfo = _db.EmployeeAddressInfoes.Where(x => x.UserId == model.Id).FirstOrDefault();
            EmployeeBankInfo BankInfo = _db.EmployeeBankInfoes.Where(x => x.UserId == model.Id).FirstOrDefault();
            
            Companysetting.Id = model.Id;
            if(AddressInfo != null)
            {
                if (AddressInfo.TownId != 0)
                {
                    var city = _db.Cities.Where(x => x.Id == AddressInfo.TownId).FirstOrDefault();
                    var state = _db.States.Where(x => x.Id == city.StateId).FirstOrDefault();
                    var country = _db.Countries.Where(x => x.Id == state.CountryId).FirstOrDefault();
                    Companysetting.Country = country.Id;
                    Companysetting.CountryValue = country.Name;
                    Companysetting.State = state.Id;
                    Companysetting.StateValue = state.Name;
                    Companysetting.Town = (int)AddressInfo.TownId;
                    Companysetting.TownValue = city.Name;

                }
                if (AddressInfo.AirportId != null && AddressInfo.AirportId > 0)
                {
                    var airport = _db.Airports.Where(x => x.Id == AddressInfo.AirportId).FirstOrDefault();
                    Companysetting.Airport = (int)AddressInfo.AirportId;
                    Companysetting.AirportValue = airport.Name;
                }
            }
             
            if(BankInfo != null)
            {
                Companysetting.Address = BankInfo.BankAddress;
                Companysetting.WorkPhone = model.WorkPhone;
                Companysetting.WorkMobile = model.WorkMobile;
                Companysetting.HouseNumber = AddressInfo.HousNumber;
                Companysetting.Postcode = AddressInfo.PostCode;
                Companysetting.PersonalPhone = AddressInfo.PersonalPhone;
                Companysetting.PersonalMobile = AddressInfo.PersonalMobile;
                Companysetting.PersonalEmail = AddressInfo.PersonalEmail;
                Companysetting.BankName = BankInfo.BankName;
                Companysetting.BankCode = BankInfo.BankCode;
                Companysetting.IBAN_Number = BankInfo.IBAN_No;
                Companysetting.SWIF_Code = BankInfo.SWIFT_Code;
                Companysetting.AccountNumber = BankInfo.AccountNumber;
                Companysetting.OtherAccountInformation = BankInfo.OtherAccountInformation;
                Companysetting.AccountName = BankInfo.AccountName;
                Companysetting.BankAddress = BankInfo.BankAddress;
                Companysetting.EmployeeId = Convert.ToInt32(AddressInfo.UserId);
            }  
            foreach (var item in _db.Countries.ToList())
            {
                if (AddressInfo != null)
                {

                    if (AddressInfo.CountryId == item.Id)
                    {
                        Companysetting.CountryList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        Companysetting.CountryList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
                    }
                }
            }
            foreach (var item in _db.States.ToList())
            {
                if (AddressInfo != null)
                {
                    if (AddressInfo.StateId == item.Id)
                    {
                        Companysetting.StateList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        Companysetting.StateList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
                    }
                }
            }
            foreach (var item in _db.Cities.ToList())
            {
                if (AddressInfo != null)
                {
                    if (AddressInfo.TownId == item.Id)
                    {
                        Companysetting.TownList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        Companysetting.TownList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
                    }
                }
            }
            foreach (var item in _db.Airports.ToList())
            {
                if (AddressInfo != null)
                {
                    if (AddressInfo.AirportId == item.Id)
                    {
                        Companysetting.AirportList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        Companysetting.AirportList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
                    }
                }

            }
            return Companysetting;
        }

        public Employe_EmergencyContacts getEmergencyContactById(int Id)
        {
            return _db.Employe_EmergencyContacts.Where(x => x.Id == Id && x.Archived==false).FirstOrDefault();
        }

        public void saveEmergencyContact(EmergencyContactsViewModel model, int userId)
        {
            if (model.Id > 0)
            {
                Employe_EmergencyContacts data = _db.Employe_EmergencyContacts.Where(x => x.Id == model.Id).FirstOrDefault();
                data.EmployeeID = model.EmployeeId;
                data.Name = model.Name;
                data.Relationship = model.Relationship;
                data.PostCode = model.Postcode;
                data.Address = model.Address;
                data.Telephone = model.Telephone;
                data.Mobile = model.Mobile;
                data.Comments = model.Comments;
                data.UserIDLastModifiedBy = userId;
                data.LastModified = DateTime.Now;
                _db.SaveChanges();
            }
            else
            {
                Employe_EmergencyContacts data = new Employe_EmergencyContacts();
                data.EmployeeID = model.EmployeeId;
                data.Name = model.Name;
                data.Relationship = model.Relationship;
                data.PostCode = model.Postcode;
                data.Address = model.Address;
                data.Telephone = model.Telephone;
                data.Mobile = model.Mobile;
                data.Comments = model.Comments;
                data.Archived = false;
                data.UserIDCreatedBy = userId;
                data.CreatedDate = DateTime.Now;
                data.UserIDLastModifiedBy = userId;
                data.LastModified = DateTime.Now;
                _db.Employe_EmergencyContacts.Add(data);
                _db.SaveChanges();
            }

        }

        public void SaveContactSet(EmployeeContactViewModel model)
        {
            AspNetUser AddUser = _db.AspNetUsers.Where(x => x.Id == model.Id).FirstOrDefault();
            EmployeeAddressInfo AddressInfo = _db.EmployeeAddressInfoes.Where(x => x.UserId == model.Id).FirstOrDefault();
            EmployeeBankInfo BankInfo = _db.EmployeeBankInfoes.Where(x => x.UserId == model.Id).FirstOrDefault();
            //step 4 

            AddUser.WorkPhone = model.WorkPhone;
            AddUser.WorkMobile = model.WorkMobile;
            //Address Details
            AddressInfo.CountryId = model.Country;
            AddressInfo.StateId = model.State;
            AddressInfo.TownId = model.Town;
            AddressInfo.AirportId = model.Airport;
            AddressInfo.HousNumber = model.HouseNumber;
            AddressInfo.PostCode = model.Postcode;
            AddressInfo.PersonalPhone = model.PersonalPhone;
            AddressInfo.PersonalMobile = model.PersonalMobile;
            AddressInfo.PersonalEmail = model.PersonalEmail;
            
            //Bank details
            BankInfo.BankAddress = model.Address;
            BankInfo.BankName = model.BankName;
            BankInfo.BankCode = model.BankCode;
            BankInfo.AccountNumber = model.AccountNumber;
            BankInfo.OtherAccountInformation = model.OtherAccountInformation;
            BankInfo.AccountName = model.AccountName;
            BankInfo.BankAddress = model.BankAddress;
            BankInfo.IBAN_No = model.IBAN_Number;
            BankInfo.SWIFT_Code = model.SWIF_Code;
            _db.SaveChanges();
        }
    }
}