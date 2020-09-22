using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Admin
{
    public class AdminCaseLogViewModel
    {
        public AdminCaseLogViewModel()
        {
            StatusList = new List<SelectListItem>();
            EmployeeList = new List<SelectListItem>();
            CategoryList = new List<SelectListItem>();
            CommentList = new List<AdminCaseLogCommentViewModel>();
            DocumentList = new List<AdminCaseLogDocumentViewModel>();
        }
        public int Id { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public IList<SelectListItem> StatusList { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public IList<SelectListItem> EmployeeList { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IList<SelectListItem> CategoryList { get; set; }
        public string Summary { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDate { get; set; }
        public IList<AdminCaseLogCommentViewModel> CommentList { get; set; }
        public IList<AdminCaseLogDocumentViewModel> DocumentList { get; set; }
        public string CommentListString { get; set; }
        public string DocumentListString { get; set; }
    }

    public class AdminCaseLogCommentViewModel 
    {
        public int Id { get; set; }
        public int CaseID { get; set; }
        public string comment { get; set; }
        public string commentBy { get; set; }
        public string commentTime { get; set; }
    }

    public class AdminCaseLogDocumentViewModel
    {
        public int Id { get; set; }
        public int CaseID { get; set; }
        public string originalName { get; set; }
        public string newName { get; set; }
        public string description { get; set; }
    }
}