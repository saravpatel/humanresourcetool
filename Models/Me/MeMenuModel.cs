using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.Models.Me
{
    public class MeMenuModel
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeImage { get; set; }
        public string Jobtilte { get; set; }
        public bool Me_OverView { get; set; }
        public bool Me_Planner { get; set; }
        public bool Me_ProjectPlanner { get; set; }
        public bool Me_Performance { get; set; }
        public bool Me_SkillsEndorsement { get; set; }
        public bool Me_Skills { get; set; }
        public bool Me_Training { get; set; }
        public bool Me_Documents { get; set; }
        public bool Me_Resume_CV { get; set; }
        public bool Me_Profile { get; set; }
        public bool Me_Employment { get; set; }
        public bool Me_Contacts { get; set; }
        public bool Me_Benefits { get; set; }
        public bool Me_Caselog { get; set; }
    }
}