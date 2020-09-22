using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRTool.CommanMethods;
namespace HRTool.Models.Admin
{
    public class PerformanceIndexPageViewModel
    {
        public PerformanceIndexPageViewModel()
        {
            PerformanceReviewList = new List<SelectListItem>();
            JobTitleList = new List<SelectListItem>();
            BussinessList = new List<SelectListItem>();
            PoolList = new List<SelectListItem>();
            ManagerResourceList = new List<SelectListItem>();
            GetOverAllEmployeeList = new List<PerforamnceOverAllList>();
            FilterList = new List<SelectListItem>();
        }
        public string NumberofReviewsOpenthisYear { get; set; }
        public string NumberofCompletedReviewthisYear { get; set; }
        public string NumberofOpenReviewAllYear { get; set; }
        public string NumberofCompletedReviewAllYear { get; set; }
        public string OutstandingReview { get; set; }
        public int CustomerOutstanding { get; set; }
        public int ManagerOutstanding { get; set; }
        public int WorkerOutstanding { get; set; }
        public int CountTotalQuestionByReview { get; set; }
        public Double PerformanceIncrease { get; set; }
        public Double PerformanceCompletedReviewInce { get; set; }
        public int SelectedPerformanceId { get; set; }
        public int FlagForTotalQuestion { get; set; }
        public string SelectedReviewId { get; set; }
        public string SelectedBussinessId { get; set; }        
        public string SelectedPoolId { get; set; }
        public string SelectedJobTitleId { get; set; }

        public string SelectedManagerId { get; set; }
        public string SelectedFilterValue { get; set; }
       public IList<SelectListItem> PerformanceReviewList { get; set; }
        public IList<SelectListItem> JobTitleList { get; set; }
        public IList<SelectListItem> BussinessList { get; set; }
        public IList<SelectListItem> PoolList { get; set; }
        public List<KeyValue> listOfDivision { get; set; }
        public List<KeyValue> listOfPool { get; set; }
        public List<KeyValue> listOfFunction { get; set; }
        public IList<SelectListItem> ManagerResourceList { get; set; }
        public IList<PerforamnceOverAllList> GetOverAllEmployeeList { get; set; }
        public IList<SelectListItem> FilterList { get; set; }
    }

    public class PerforamnceOverAllList
    {
        
        public string EmployeeId { get; set; }
        public string PerfReviewId { get; set; }
        public string EmployeeName { get; set; }
        public string InviteEmployeeName { get; set; }
        public string EmployeeImage { get; set; }
        public string InviteEmployeeImage { get; set; }
        public string EmployeeReviewStatus { get; set; }
        public string InviteEmployeeReviewStatus { get; set; }
        public string OverAllScore { get; set; }
        public string CoreScore { get; set; }
    }

    public class PerformanceGraph
    {
        public PerformanceGraph()
        {
            ListOfPerformanceReviewGraph = new List<PerformanceReviewGraph>();
            
        }
        public int EmpID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeImage { get; set; }
        public double MostConsistanceValue { get; set; }
        public double DiffOfPerfReview { get; set; }
        public string ReviewName { get; set; }
        public double ReviewScore { get; set; }
        public IList<PerformanceReviewGraph> ListOfPerformanceReviewGraph { get; set; }
    }

    public class PerformanceReviewGraph
    {
        public double LastReview { get; set; }
        public double PerviouseReview { get; set; }
    }
}