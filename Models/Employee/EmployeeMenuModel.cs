using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.Models.Employee
{
    public class EmployeeMenuModel
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeImage { get; set; }
        public string EmployeeRole { get; set; }
        public string Jobtilte { get; set; }
        public bool Resources_OverView { get; set; }
        public bool Resources_Planner { get; set; }
        public bool Resources_ProjectPlanner { get; set; }
        public bool Resources_Performance { get; set; }
        public bool Resources_SkillsEndorsement { get; set; }
        public bool Resources_Skills { get; set; }
        public bool Resources_Training { get; set; }
        public bool Resources_Documents { get; set; }
        public bool Resources_Resume_CV { get; set; }
        public bool Resources_Profile { get; set; }
        public bool Resources_Employment { get; set; }
        public bool Resources_Contacts { get; set; }
        public bool Resources_Benefits { get; set; }
        public bool Resources_Caselog { get; set; }

    }
}