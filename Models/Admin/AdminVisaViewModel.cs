using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Admin
{
    public class AdminVisaViewModel
    {
        public AdminVisaViewModel()
        {
            CountryList = new List<SelectListItem>();
            VisaTypeList = new List<SelectListItem>();
            ServiceAgencyList = new List<SelectListItem>();
            AssignToList = new List<SelectListItem>();
            InRelationToList = new List<SelectListItem>();
            StatusList = new List<SelectListItem>();
            DocumentList = new List<VisaDocumentViewModel>();
        }
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public IList<SelectListItem> CountryList { get; set; }
        public int VisaTypeId { get; set; }
        public string VisaType { get; set; }
        public IList<SelectListItem> VisaTypeList { get; set; }
        public int ServiceAgencyId { get; set; }
        public string ServiceAgency { get; set; }
        public IList<SelectListItem> ServiceAgencyList { get; set; }
        public string VisaNumber { get; set; }
        public int? AssignToId { get; set; }
        public string AssignTo { get; set; }
        public IList<SelectListItem> AssignToList { get; set; }
        public int? InRelationToId { get; set; }
        public string InRelationTo { get; set; }
        public IList<SelectListItem> InRelationToList { get; set; }
        public string ValidFrom { get; set; }
        public string ExpiryDate { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public IList<SelectListItem> StatusList { get; set; }
        public int AlertBeforeDays { get; set; }
        public string Description { get; set; }
        public IList<VisaDocumentViewModel> DocumentList { get; set; }
        public string jsonDocumentListString { get; set; }
        public string JobTitle { get; set; }
        public string Pool { get; set; }
        public string Function { get; set; }
        public string FilterSearch { get; set; }

        public Nullable<int> Flag { get; set; }
    }

    public class VisaDocumentViewModel
    {
        public int Id { get; set; }
        public int CaseID { get; set; }
        public string originalName { get; set; }
        public string newName { get; set; }
        public string description { get; set; }
    }

    public class AdminVisaMenuViewModel
    {
        public int MyVisa { get; set; }
        public int Expired { get; set; }
        public int New { get; set; }
        public int ExpiringToday { get; set; }
        public int Upcoming { get; set; }
        public int Valid { get; set; }
        public int AllVisa { get; set; }
    }

}