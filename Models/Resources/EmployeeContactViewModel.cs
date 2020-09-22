using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Resources
{
    public class EmployeeContactViewModel
    {
        public EmployeeContactViewModel()
        {
            CountryList = new List<SelectListItem>();
            StateList = new List<SelectListItem>();
            TownList = new List<SelectListItem>();
            AirportList = new List<SelectListItem>();
            EmergencyContactsList=new List<EmergencyContactsViewModel>();

        }
        public int EmployeeId { get; set; }
        public int Id { get; set; }
        public int Country { get; set; }
        public string CountryValue { get; set; }
        public int State { get; set; }
        public string StateValue { get; set; }
        public int Town { get; set; }
        public string TownValue { get; set; }
        public int Airport { get; set; }
        public string AirportValue { get; set; }
        public string HouseNumber { get; set; }
        public string Postcode { get; set; }
        public string Address { get; set; }
        public string WorkPhone { get; set; }
        public string WorkMobile { get; set; }
        public string PersonalPhone { get; set; }
        public string PersonalMobile { get; set; }
        public string PersonalEmail { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string AccountNumber { get; set; }
        public string IBAN_Number { get; set; }
        public string SWIF_Code { get; set; }
        public string OtherAccountInformation { get; set; }
        public string AccountName { get; set; }
        public string BankAddress { get; set; }

        public IList<SelectListItem> CountryList { get; set; }
        public IList<SelectListItem> StateList { get; set; }
        public IList<SelectListItem> TownList { get; set; }
        public IList<SelectListItem> AirportList { get; set; }

        public IList<EmergencyContactsViewModel> EmergencyContactsList { get; set; }
    }


    public class EmergencyContactsViewModel 
    {
        public EmergencyContactsViewModel()
        {
            RelationshipList = new List<SelectListItem>();
        }

        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public int Relationship { get; set; }
        public string RelationshipValue { get; set; }
        public string HouseNumber { get; set; }
        public string Postcode { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string Comments { get; set; }
        public IList<SelectListItem> RelationshipList { get; set; }

    
    }

}