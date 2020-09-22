using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.BulkActions
{
    public class BulkActionTraining
    {
        public BulkActionTraining()
        {
            BindTrainingStatusList = new List<SelectListItem>();
            BindTrainingList = new List<SelectListItem>();
            BindEmployeeList = new List<SelectListItem>();
            ListDocument = new List<BulkTraningDocumentViewModel>();
        }
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public Nullable<int> TrainingNameId { get; set; }
        public string Description { get; set; }
        public Nullable<int> Importance { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> Progress { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ExpiryDate { get; set; }
        public string Provider { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public string Notes { get; set; }
        public string CustomFieldsJSON { get; set; }
        public Nullable<bool> Archived { get; set; }
        public string UserIDCreatedBy { get; set; }
        public string UserIDLastModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public int CurrentUserId { get; set; }
        public string EmployeeName { get; set; }
        public IList<SelectListItem> BindTrainingList { get; set; }
        public IList<SelectListItem> BindTrainingStatusList { get; set; }
        public IList<SelectListItem> BindEmployeeList { get; set; }
        public string TrainingName { get; set; }
        public string StatusName { get; set; }
        public string ImportanceName { get; set; }
        public bool Mandatory { get; set; }
        public bool Optional { get; set; }
        public string TraingDocumentList { get; set; }

        public List<BulkTraningDocumentViewModel> ListDocument { get; set; }

    }
    public class BulkTraningDocumentViewModel
    {
        public int Id { get; set; }
        public int TrainingId { get; set; }
        public string originalName { get; set; }
        public string newName { get; set; }
        public string description { get; set; }
        //public List<string> ListDocument { get; set; }
    }
    public class FiledViewModel
    {
        public FiledViewModel()
        {
            BindFiledTypeList = new List<SelectListItem>();
        }
        public IList<SelectListItem> BindFiledTypeList { get; set; }
    }
}