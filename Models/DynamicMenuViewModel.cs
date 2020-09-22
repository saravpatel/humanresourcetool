using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.Models
{
    public class DynamicMenuViewModel
    {
        public bool Dashboard { get; set; }
        public bool News { get; set; }
        public bool Resources { get; set; }
        public bool Me { get; set; }
        public bool Document { get; set; }
        public bool Planner { get; set; }
        public bool ProjectPlanner { get; set; }
        public bool Performance { get; set; }
        public bool SkillsEndorsement { get; set; }
        public bool Training { get; set; }
        public bool Certification { get; set; }
        public bool Visa { get; set; }
        public bool Tasks { get; set; }
        public bool Approve { get; set; }
        public bool Notifications { get; set; }
        public bool Queries { get; set; }
        public bool BulkActions { get; set; }
        public bool Reports { get; set; }
        public bool OrganizationChart { get; set; }
        public bool Cases { get; set; }
        public bool TMS { get; set; }
        public bool Settings { get; set; }
        public bool IsActive { get; set; }
    }
}