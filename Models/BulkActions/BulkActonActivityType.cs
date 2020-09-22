using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.BulkActions
{
    public class BulkActonActivityType
    {
        public BulkActonActivityType()
        {
            currencyList = new List<SelectListItem>();
            workUnitList = new List<SelectListItem>();
            activityTypeList = new List<ActivityTypeListViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrencyID { get; set; }

        public string CurrencyValue { get; set; }
        public int WorkUnitID { get; set; }

        public string WorkUnitValue { get; set; }
        public int Year { get; set; }

        public decimal WorkerRate { get; set; }
        public decimal CustomerRate { get; set; }

        public bool Archived { get; set; }
        public string UserIDCreatedBy { get; set; }
        public string UserIDLastModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }

        public IList<SelectListItem> currencyList { get; set; }
        public IList<SelectListItem> workUnitList { get; set; }

        public IList<ActivityTypeListViewModel> activityTypeList { get; set; }
    }
    public class ActivityTypeListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public string Currency { get; set; }
        public string WorkUnit { get; set; }


    }
}