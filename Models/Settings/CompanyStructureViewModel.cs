using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.Models.Settings
{
    public class CompanyStructureViewModel
    {
        public CompanyStructureViewModel()
        {
            businessLists = new List<BusinessViewModel>();
            divisionLists = new List<DivisionViewModel>();
            poolLists = new List<PoolViewModel>();
            functionLists = new List<FunctionViewModel>();

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Archived { get; set; }
        public string UserIDCreatedBy { get; set; }
        public string UserIDLastModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
        public IList<BusinessViewModel> businessLists { get; set; }
        public IList<DivisionViewModel> divisionLists { get; set; }
        public IList<PoolViewModel> poolLists { get; set; }
        public IList<FunctionViewModel> functionLists { get; set; }

    }

    public class BusinessViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Archived { get; set; }
        public string UserIDCreatedBy { get; set; }
        public string UserIDLastModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
    }
    public class DivisionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BusinessID { get; set; }
        public string BusinessName { get; set; }
        public bool Archived { get; set; }
        public string UserIDCreatedBy { get; set; }
        public string UserIDLastModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
    }

    public class PoolViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BusinessID { get; set; }
        public string BusinessName { get; set; }
        public int DivisionID { get; set; }
        public string DivisionName { get; set; }
        public bool Archived { get; set; }
        public string UserIDCreatedBy { get; set; }
        public string UserIDLastModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }


    }

    public class FunctionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BusinessID { get; set; }
        public string BusinessName { get; set; }
        public int DivisionID { get; set; }
        public string DivisionName { get; set; }
        public bool Archived { get; set; }
        public string UserIDCreatedBy { get; set; }
        public string UserIDLastModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }

    }
}