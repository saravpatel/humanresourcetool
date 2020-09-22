using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.Models.Settings
{
    public class AddSkillsViewModel
    {
        public int Id { get; set; }
        public int SystemListID { get; set; }
        public string Value { get; set; }
        public bool Archived { get; set; }
        public string Description { get; set; }
    }
}