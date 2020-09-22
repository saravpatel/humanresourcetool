using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Admin
{
    public class AdminCertificateIdViewModel
    {
        public AdminCertificateIdViewModel()
        {
            TypeList = new List<SelectListItem>();
            AssignToList = new List<SelectListItem>();
            InRelationToList = new List<SelectListItem>();
            StatusList = new List<SelectListItem>();
            DocumentList = new List<CertificateDocumentViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public string Type { get; set; }
        public IList<SelectListItem> TypeList { get; set; }
        public string Body { get; set; }
        public string Number { get; set; }
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
        public bool Mandatory { get; set; }
        public bool Validate { get; set; }
        public IList<CertificateDocumentViewModel> DocumentList { get; set; }
        public string jsonDocumentListString { get; set; }
        public string JobTitle { get; set; }
        public string Pool { get; set; }
        public string Function { get; set; }
        public string FilterSearch { get; set; }
        public string AssignToEmployeeName { get; set; }
        public string AssignInRelationTo { get; set; }
        public Nullable<int> Flag { get; set; }
    }

    public class CertificateDocumentViewModel
    {
        public int Id { get; set; }
        public int CaseID { get; set; }
        public string originalName { get; set; }
        public string newName { get; set; }
        public string description { get; set; }
    }

    public class AdminCertificateMenuViewModel
    {
        public int MyCertificate { get; set; }
        public int Expired { get; set; }
        public int New { get; set; }
        public int ExpiringToday { get; set; }
        public int Upcoming { get; set; }
        public int Valid { get; set; }
        public int AllCertificate { get; set; }
    }
}