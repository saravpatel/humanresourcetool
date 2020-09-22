using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Resources
{
    public class EmployeeDocumentViewModel
    {
        public EmployeeDocumentViewModel()
        {
            BusinessList = new List<SelectListItem>();
            DivisionList = new List<SelectListItem>();
            PoolList = new List<SelectListItem>();
            FunctionList = new List<SelectListItem>();
            CategoryList = new List<SelectListItem>();
            WorkerList = new List<SelectListItem>();
            ManagerList = new List<SelectListItem>();
            CustomerList = new List<SelectListItem>();
        }
        public string Type { get; set; }
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentOriginalPath { get; set; }
        public string Description { get; set; }
        public string LinkDisplayText { get; set; }
        public string LinkURL { get; set; }
        public int BusinessID { get; set; }
        public string Business { get; set; }
        public int DivisionID { get; set; }
        public string Division { get; set; }
        public int PoolID { get; set; }
        public string Pool { get; set; }
        public int FunctionID { get; set; }
        public string Function { get; set; }
        public int Category { get; set; }
        public string CategoryName { get; set; }
        public bool EmployeeAccess { get; set; }
        public bool ManagerAccess { get; set; }
        public bool CustomerAccess { get; set; }
        public bool SpecificWorker { get; set; }
        public string SpecificWorkerName { get; set; }
        public string WorkerID { get; set; }
        public string SpecificManagerName { get; set; }
        public bool SpecificManager { get; set; }
        public string ManagerID { get; set; }
        public bool SpecificCustomer { get; set; }
        public string SpecificCustomerName { get; set; }
        public string CustomerID { get; set; }
        public bool SignatureRequire { get; set; }
        public string Signature { get; set; }
        public bool Signed { get; set; }
        public string IpAddress { get; set; }
        public string LoginUserID { get; set; }
        public string CreateDate { get; set; }
        public IList<SelectListItem> BusinessList { get; set; }
        public IList<SelectListItem> DivisionList { get; set; }
        public IList<SelectListItem> PoolList { get; set; }
        public IList<SelectListItem> FunctionList { get; set; }
        public IList<SelectListItem> CategoryList { get; set; }
        public IList<SelectListItem> WorkerList { get; set; }
        public IList<SelectListItem> ManagerList { get; set; }
        public IList<SelectListItem> CustomerList { get; set; }
    }

    public class EmployeeDocumentListModel 
    {
        public EmployeeDocumentListModel()
        {
            EmployeeList = new List<EmployeeDocumentViewModel>();
        }

        //public string LoginUserID { get; set; }
        public int EmployeeID { get; set; }
        public IList<EmployeeDocumentViewModel> EmployeeList { get; set; }
    }

   
}