using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Settings
{
    public class TimeSheetViewModel
    {
        public TimeSheetViewModel()
        {
            ProjectList = new List<SelectListItem>();
            FrequencyList = new List<SelectListItem>();
            DetailList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public Nullable<int> Frequency { get; set; }
        public Nullable<int> Detail { get; set; }
        public IList<SelectListItem> ProjectList { get; set; }
        public IList<SelectListItem> FrequencyList { get; set; }
        public IList<SelectListItem> DetailList { get; set; }
    }
}