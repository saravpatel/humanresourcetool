using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Me
{
    public class MeEmployeeDetail_Overview
    {
        public int id { get; set; }
        public int EmployeeID { get; set; }
        public string DateOfHire { get; set; }
        public string CountryOfResidence { get; set; }
        public string Location { get; set; }
        //public string NearestAirport { get; set; }
        public string business { get; set; }
        public string Division { get; set; }
        public string pool { get; set; }
        public string jobtitle { get; set; }
        public string workPhone { get; set; }
        public string DOB { get; set; }
        public string gender { get; set; }
        public string emailId { get; set; }
        public string mobile { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string FunctionName { get; set; }
        public Double AbsencesDay { get; set; }
        public int? employmentDay { get; set; }
        public int workedDay { get; set; }
        public int DaysWorkedSinceEver { get; set; }
        public IList<MeEmployeeDetail_Overview> AllEmployeeData { get; set; }
        public int annualLeave { get; set; }
        public int totalLeave { get; set; }
        public int toatalHolidayRemain { get; set; }
        public double contractDays { get; set; }

        public string ReportTo { get; set; }
    }
    public class MeEmployeeResourceDetail_Overview
    {
        public int id { get; set; }
        public int EmployeeID { get; set; }
        public string Location { get; set; }
        public string business { get; set; }
        public string Division { get; set; }
        public string pool { get; set; }
        public string Name { get; set; }
        public string FunctionName { get; set; }
    }
    public class MeEmployeeLeaverWizard
    {
        public List<BenefitList> BenefitList { get; set; }
        public List<TaskList> TaskList { get; set; }
        public MeEmployeeLeaverWizard()
        {
            LeaveReasonList = new List<SelectListItem>();
            EmployeeList = new List<SelectListItem>();
            BenefitList = new List<BenefitList>();
            TaskList = new List<TaskList>();
        }
        public IList<SelectListItem> LeaveReasonList { get; set; }
        public IList<SelectListItem> EmployeeList { get; set; }
        public int reEmployeeable { get; set; }
    }
    public class BenefitList
    {
        public int id { get; set; }
        public string benifitName { get; set; }
        public DateTime dateAwarded { get; set; }
    }
    public class TaskList
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Value { get; set; }
        public string status { get; set; }
        public int EmployeeId { get; set; }
    }
    public class AddNewTaskListViewModel
    {
        public AddNewTaskListViewModel()
        {
            AssignList = new List<SelectListItem>();
            StatusList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Assign { get; set; }
        public string DueDate { get; set; }
        public int Status { get; set; }
        public int AlertBeforeDays { get; set; }
        public IList<SelectListItem> StatusList { get; set; }
        public IList<SelectListItem> AssignList { get; set; }
        public int IdRecord { get; set; }
        public int IsTemp { get; set; }
    }
    public class CountAbsenceDay
    {
        public DateTime workDay { get; set; }
        public DateTime AbsenceDay { get; set; }
    }
    public class yearInfo
    {
        public int year { get; set; }
        public int month { get; set; }
    }
}