using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Resources
{
    public class EmployeePerformanceViewModel
    {
        public EmployeePerformanceViewModel()
        {
            ListofPerformance = new List<EmployeePerformanceViewModel>();
            ListOfPastPerformace = new List<EmployeePerformanceViewModel>();
        }
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int ReviewId { get; set; }        
        public int ProjectId { get; set; }

        public string CompletionDateTime { get; set; }
        public DateTime CompletionDate { get; set; }
        public string CreatedDate { get; set; }
        public int IsActivePastFlag { get; set; }
        public string ReviewByName { get; set; }
        public string ReviewName { get; set; }
        public Nullable<Double> OverAllScore { get; set; }
        public string ManagerScore { get; set; }
        public string CustomerScore { get; set; }
        public string CustomerShare { get; set; }
        public IList<EmployeePerformanceViewModel> ListofPerformance { get; set; }
        public IList<EmployeePerformanceViewModel> ListOfPastPerformace { get; set; }
    }

    public class StartNewReviewViewModel
    {
        public StartNewReviewViewModel()
        {
            ProjectList = new List<SelectListItem>();
            ReviewList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public IList<SelectListItem> ProjectList { get; set; }
        public int ProjectId { get; set; }
        public IList<SelectListItem> ReviewList { get; set; }
        public int ReviewId { get; set; }
        public string IsEmployeeExistReview { get; set; }
        public string CompletionDate { get; set; }
    }

    public class ReviewDetails
    {
        public int Id { get; set; }
        public int ReviewId { get; set; }
        public bool Coworker { get; set; }
        public bool Manager { get; set; }
        public int EmpID { get; set; }
        public int Flag { get; set; }
        public int ManagerId { get; set; }
        public int IsActivePastFlag { get; set; }
        public string EmpPerfReviewId { get; set; }

        public List<Coworker> CoreSegmentsList { get; set; }
    }
    public class Coworker
    {
        public Coworker()
        {
            CoreQueList = new List<CoworkerList>();
            CoworkerInviteList = new List<CoworkerInviteList>();
            CustomerInviteList = new List<CustomerInviteList>();
        }
        public int CoreSegId { get; set; }
        public string TotalInvitedCustomer { get; set; }
        public string TotalInvitedCoworker { get; set; }
        public string Title { get; set; }
        public int IsActivePastFlag { get; set; }
        public string Description { get; set; }
        public List<CoworkerList> CoreQueList { get; set; }
        public List<CoworkerInviteList> CoworkerInviteList { get; set; }

        public List<CustomerInviteList> CustomerInviteList { get; set; }
        public string QueId { get; set; }
        public string Score { get; set; }

        public string questionData { get; set; }
        public string cororkerId { get; set; }
        public List<CoworkerQuestionModelForMe> CoreQueListData { get; set; }






    }
    public class CoworkerList
    {
        public int QueId { get; set; }
        public string questionData { get; set; }
        public int FiledId { get; set; }
        public string FiledText { get; set; }
        public int CValue { get; set; }
    }

    public class CoworkerInviteList
    {
        public CoworkerInviteList()
        {
            EmployeeList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public int EmpID { get; set; }
        public int coworkerId { get; set; }
        public string EmpName { get; set; }
        public string Status { get; set; }
        public string EmpEmail { get; set; }
        public IList<SelectListItem> EmployeeList { get; set; }
    }
    public class CustomerInviteList
    {
        public CustomerInviteList()
        {
            CustomerList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public int EmpID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
        public string EmpEmail { get; set; }
        public IList<SelectListItem> CustomerList { get; set; }
    }


}
