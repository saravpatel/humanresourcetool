using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Admin
{
    public class AdminTaskViewModel
    {
        public AdminTaskViewModel()
        {
            CategoryList = new List<SelectListItem>();
            AssignToList = new List<SelectListItem>();
            InRelationToList = new List<SelectListItem>();
            StatusList = new List<SelectListItem>();
            DocumentList = new List<TaskDocumentViewModel>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IList<SelectListItem> CategoryList { get; set; }
        public int? AssignToId { get; set; }
        public string AssignTo { get; set; }
        public IList<SelectListItem> AssignToList { get; set; }
        public int? InRelationToId { get; set; }
        public string InRelationTo { get; set; }
        public IList<SelectListItem> InRelationToList { get; set; }
        public string DueDate { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public IList<SelectListItem> StatusList { get; set; }
        public int AlertBeforeDays { get; set; }
        public string Description { get; set; }
        public IList<TaskDocumentViewModel> DocumentList { get; set; }
        public string jsonDocumentListString { get; set; }
        public string FilterSearch { get; set; }

    }

    public class TaskDocumentViewModel
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string originalName { get; set; }
        public string newName { get; set; }
        public string description { get; set; }
    }

    public class AdminTaskMenuViewModel
    {
        public int MyTask { get; set; }
        public int OverDue { get; set; }
        public int New { get; set; }
        public int DueToday { get; set; }
        public int Upcoming { get; set; }
        public int Complete { get; set; }
        public int AllTask { get; set; }
    }
}