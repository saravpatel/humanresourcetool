using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.OrganizationChart
{
    public class OrganizationChartViewModel
    {
        public OrganizationChartViewModel()
        {
            BusinessList = new List<SelectListItem>();
            DivisionList = new List<SelectListItem>();
            PoolList = new List<SelectListItem>();
            FunctionList = new List<SelectListItem>();
            ResourceTypeList = new List<SelectListItem>();
        }
        public IList<SelectListItem> BusinessList { get; set; }
        public IList<SelectListItem> DivisionList { get; set; }
        public IList<SelectListItem> PoolList { get; set; }
        public IList<SelectListItem> FunctionList { get; set; }
        public IList<SelectListItem> ResourceTypeList { get; set; }

        public int? EmpId { get; set; }
        public string Name { get; set; }
        public Nullable<int> LengthOfEmployeement { get; set; }
        public int? ReportsTo { get; set; }
        public string Value { get; set; }
        public string ImageUrl { get; set; }
        public int BusinessID { get; set; }
        public int? ResourceTypeId { get; set; }

    }
}