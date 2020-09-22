using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.Models.Approval
{
    public class TimeSheetApprovalViewModel
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public string Day { get; set; }
        public string Date { get; set; }
        public string Hours { get; set; }
        public string CostCode { get; set; }
        public string Project { get; set; }
        public string Customer { get; set; }
        public string Asset { get; set; }
        public string SupportingDocument { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string InTime { get; set; }
        public string EndTime { get; set; }
        public string FileName { get; set; }
    }
    public class ScheduleApproval
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal Duration { get; set; }
        public string Name { get; set; }
        public string InTime { get; set; }
        public string EndTime { get; set; }
        public string Hours { get; set; }
        public string Project { get; set; }
        public string Customer { get; set; }
        public string Asset { get; set; }
        public string SupportingDocument { get; set; }
        public string Status { get; set; }
    }

    public class AllApproveRequestList
    {
        public int id { get; set; }
        public int? EmployeeId { get; set; }
        public string Header { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public bool Gender { get; set; }
    }

    public class TravelApprove
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string FromCountry { get; set; }
        public string FromTown { get; set; }
        public string FromAirport { get; set; }
        public string ToCountry { get; set; }
        public string ToAirport { get; set; }
        public string ToTown { get; set; }
        public string Startdate { get; set; }
        public string Enddate { get; set; }
        public decimal Duration { get; set; }
        public string Hour { get; set; }
        public string Customer { get; set; }
        public string Project { get; set; }
        public string CostCode { get; set; }
        public string Status { get; set; }
        public string FileName { get; set; }
    }

    public class AnnualLeaveapprove
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal Duration { get; set; }
        public decimal TotalHolidays { get; set; }
        public Double HolidaysTaken { get; set; }
        public string Status { get; set; }
    }

    public class SickLeaveapprove
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Duration { get; set; }
        public bool SelfCertificationRequired { get; set; }
        public bool InterviewRequired { get; set; }
        public string Type { get; set; }
        public bool DoctorConsulted { get; set; }
        public string IssueAtWork { get; set; }
        public string Status { get; set; }
    }

    public class OtherLeaveapprove
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal Duration { get; set; }
        public string Reason { get; set; }
        public decimal TotalOtherLeaveTobeApproved { get; set; }
        public string FileName { get; set; }
        public double totalOtherLeave { get; set; }
    }
    public class OtherLeaveYear
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

    }
    public class TrainingRequest
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public string Name { get; set; }
        public string TrainingName { get; set; }
        public string Importance { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal Days { get; set; }
        public string Provider { get; set; }
        public int Cost { get; set; }
        public string Status { get; set; }
        public double TotalTrainingDaysApproved { get; set; }
        public string FileName { get; set; }

    }

    public class NewVacancy
    {
        public int Id { get; set; }
        public int? UserID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string VacancyName { get; set; }
        public string RecruitmentProcess { get; set; }
        public string ClosingDate { get; set; }
        public decimal Salary { get; set; }
        public string Location { get; set; }
        public string Pool { get; set; }
        public string Business { get; set; }
        public string Function { get; set; }
        public string Division { get; set; }
        public int totalVacancy { get; set; }
        public string FileName { get; set; }

    }

    public class Mat_PatLeave
    {
        public int Id { get; set; }
        public DateTime DueDate { get; set; }
        public string Link { get; set; }
    }

    public class Uplift
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public string Name { get; set; }
        public string Day { get; set; }
        public string Date { get; set; }
        public int UpliftPosition { get; set; }
        public string Hours { get; set; }
        public string EndTimeHr { get; set; }
        public string Project { get; set; }
        public string Customer { get; set; }
        public decimal ChangeInCustomerRate { get; set; }
        public decimal ChangeInWorkerRate { get; set; }
        public TimeSpan totalUpliftHr { get; set; }
        public string FileName { get; set; }

    }
}