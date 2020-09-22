using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.Models.Settings
{
    public class SettingMenuViewModel
    {
        public bool Settings_TMS { get; set; }
        public bool Settings_Projects { get; set; }
        public bool Settings_Performance { get; set; }
        public bool Settings_TechnicalSkillsSet { get; set; }
        public bool Settings_GeneralSkillsSet { get; set; }
        public bool Settings_AddSkills { get; set; }
        public bool Settings_ActivityType { get; set; }
        public bool Settings_AddCmpCustomer { get; set; }
        public bool Settings_AddAssets { get; set; }
        public bool Settings_Timesheet { get; set; }
        public bool Settings_HolidaysAbsence { get; set; }
        public bool Settings_Currency { get; set; }
        public bool Settings_Administrators { get; set; }
        public bool Settings_EmailSettings { get; set; }
        public bool Settings_API { get; set; }
        public bool Settings_Company { get; set; }
        public bool Settings_CompanyStructure { get; set; }
        public bool Settings_License { get; set; }
        public bool Settings_Mobile { get; set; }
        public bool Settings_CreateRole { get; set; }
        public bool Settings_AssignRole { get; set; }
        public bool Settings_RoleManagement { get; set; }
        public bool Settings_OtherSettings { get; set; }
        public bool Settings_SystemSettings { get; set; }
    }
}