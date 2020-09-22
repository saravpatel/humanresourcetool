using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Settings
{
    public class ProjectViewModel
    {
        public ProjectViewModel()
        {
            CountryList = new List<SelectListItem>();
            BlockList = new List<SelectListItem>();
            LocationList = new List<SelectListItem>();
            TaxZoneList = new List<SelectListItem>();
            AssetsTypeList = new List<SelectListItem>();
            CustomersList = new List<SelectListItem>();
            ProjectListRecord = new List<ProjectViewModel>();
            GeneralSkillsList = new List<SelectListItem>();
            TechnicalSkillsList = new List<SelectListItem>();
            selectedValuesGeneral = new List<string>();
            selectedValuesTechnical = new List<string>();
            selectedValuesCoustmer = new List<string>();

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> Country { get; set; }
        public Nullable<int> Location { get; set; }
        public Nullable<int> Block { get; set; }
        public Nullable<int> TaxZone { get; set; }
        public Nullable<int> AssetType { get; set; }
        public int OperatorEmployeeID { get; set; }
        public string ProjectOwner { get; set; }
        public string CountryName { get; set; }
        public string LocationName { get; set; }
        public string CustomersCSV { get; set; }
        public bool Archived { get; set; }
        public string UserIDCreatedBy { get; set; }
        public string UserIDLastModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GeneralSkillsCSV { get; set; }
        public string TechnicalSkillsCSV { get; set; }
        public string OperatorCompany { get; set; }
        public string AssetTypesCSV { get; set; }
        public string Description { get; set; }
        public IList<SelectListItem> CountryList { get; set; }
        public IList<SelectListItem> BlockList { get; set; }
        public IList<SelectListItem> LocationList { get; set; }
        public IList<SelectListItem> TaxZoneList { get; set; }
        public IList<SelectListItem> AssetsTypeList { get; set; }
        public IList<SelectListItem> CustomersList { get; set; }
        public IList<SelectListItem> GeneralSkillsList { get; set; }
        public IList<SelectListItem> TechnicalSkillsList { get; set; }
        public IList<ProjectViewModel> ProjectListRecord { get; set; }
        public List<string> selectedValuesGeneral { get; set; }
        public List<string> selectedValuesTechnical { get; set; }
        public List<string> selectedValuesCoustmer { get; set; }

        public int CurrentUserId { get; set; }

    }
}