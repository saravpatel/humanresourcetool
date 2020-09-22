using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Resources
{
    public class EmployeeEmploymentViewModel
    {
        public EmployeeEmploymentViewModel()
        {
            NoticePeriodList = new List<SelectListItem>();
            ActivityTypeList = new List<SelectListItem>();
        }
        public int EmployeeId { get; set; }
        public string ProbationEndDate { get; set; }
        public string NextProbationReviewDate { get; set; }
        public int NoticePeriod { get; set; }
        public IList<SelectListItem> NoticePeriodList { get; set; }
        public string FixedTermEndDate { get; set; }
        public string MethodofRecruitmentSetup { get; set; }
        public string RecruitmentCost { get; set; }
        public int HolidayEnti { get; set; }
        public int ThisYearHolidays { get; set; }
        public int  NextYearHolidays { get; set; }
        public double includeThisYear { get; set; }
        public double notincludeThisYear { get; set; }
        public double includeNextYear { get; set; }
        public double notincludeNextYear { get; set; }
        public string CurruncyName { get; set; }
        public decimal? WorkerRate { get; set; }
        public string ActivityTypeName { get; set; }
        public int ActivityTypeId { get; set; }
        public double? rate { get; set; }
        public IList<SelectListItem> ActivityTypeList { get; set; }


    }
}