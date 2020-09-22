using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Admin
{
    public class AdminNewsViewModel
    {

        public AdminNewsViewModel()
        {
            WorkerList = new List<SelectListItem>();
            ManagerList = new List<SelectListItem>();
            CustomerList = new List<SelectListItem>();
            NewCommantsList = new List<NewCommentViewModel>();
        }
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public Nullable<bool> EmployeeAccess { get; set; }
        public Nullable<bool> ManagerAccess { get; set; }
        public Nullable<bool> CustomerAccess { get; set; }
        public Nullable<bool> NotifyEmployeeViaEmail { get; set; }
        public Nullable<bool> AllowCustomer { get; set; }
        public Nullable<bool> SpecificWorker { get; set; }
        public int? WorkerID { get; set; }
        public Nullable<bool> SpecificManager { get; set; }
        public int? ManagerID { get; set; }
        public Nullable<bool> SpecificCustomer { get; set; }
        public int? CustomerID { get; set; }
        public string CreatedBy { get; set; }
        public int? CreateUserId { get; set; }
        public string CreatedDate { get; set; }
        public IList<SelectListItem> WorkerList { get; set; }
        public IList<SelectListItem> ManagerList { get; set; }
        public IList<SelectListItem> CustomerList { get; set; }
        public IList<NewCommentViewModel> NewCommantsList { get; set; }
        public int CurrentUserId { get; set; }
        public string CurrentImage { get; set; }
        public string Comments { get; set; }
        public int NewsId { get; set; }
        public int CommentCount { get; set; }
        public bool Super { get; set; }
        public bool Emp { get; set; }
        public bool Cus { get; set; }
        public bool Man { get; set; }

        public string SpecificWorkerName { get; set; }
        public string SpecificManagerName { get; set; }
        public string SpecificCustomerName { get; set; }
    }

    public class NewsRole 
    {
        public int UserId { get; set; }
        public bool AddEdit { get; set; }
    }

    public class NewCommentViewModel
    {
        public int SuperAdmin { get; set; }
        public int Id { get; set; }
        public int NewsId { get; set; }
        public bool Archived { get; set; }
        public int? UserCreate { get; set; }
        public string Comments { get; set; }
        public string EmpolyeeName { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string LastModifyCreateBy { get; set; }
        public string LastModifyCreatedate { get; set; }
        public string Time { get; set; }
    }
}