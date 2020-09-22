using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.Models.Notification
{
    public class NotificationDetail
    {
        public string HeaderType { get; set; }
        public int? EmployeeId { get; set; }
        public string Hours { get; set; }
        public string Day { get; set; }
        public string Date { get; set; }
        public string CostCode { get; set; }
        public string ProjectName { get; set; }
        public string CustomerName { get; set; }
        public string AssetName { get; set; }
        public string StartDate{ get; set; }
        public string EndDate{ get; set; }
        public Nullable<decimal> Duration { get; set; }
        public string TravelType { get; set; }
        public string FromCountry { get; set; }
        public string FromTown { get; set; }
        public string FromPlace { get; set; }
        public string ToCountry { get; set; }
        public string ToTown { get; set; }
        public string Toplace{ get; set; }
        public int TotalCostInFunctionalCurrency { get; set; }
        public string Link { get; set; }

        public string Vacancy { get; set; }
        public string ClosingDate { get; set; }
        public string RecruitementProcess { get; set; }
        public string SalaryRange { get; set; }
        public string Location { get; set; }
        public string Business { get; set; }
        public string Division { get; set; }
        public string Pool { get; set; }
        public string Function { get; set; }

        public int? UpliftPosition { get; set; }
        public decimal? ChangeInWorkRate { get; set; }
        public decimal? ChangeInCustomerRate { get; set; }


        public string Reason { get; set; }

        public string TrainingName { get; set; }
        public string Importance { get; set; }
        public string Provider { get; set; }
        public decimal? Cost { get; set; }

        public bool? DoctConsulted { get; set; }
        public bool? Paid { get; set; }
        public string dueDate { get; set; }
        public string technicalskill { get; set; }
        public string generalSkill { get; set; }             
        public string email { get; set; }
        public string TrainingDescription { get; set; }  
        public string EmployeeName { get; set; }
        public string DocumentName { get; set; }

        public string Description { get; set; }
        public string DocLink { get; set; }

        public string AppStatus { get; set; }

    }
}