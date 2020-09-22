using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Settings
{
    public class GeneralSkillsSetViewModel
    {
        public GeneralSkillsSetViewModel()
        {
            SkillValueList = new List<SelectListItem>();
            selectedValues = new List<string>();
        }
        public int Id { get; set; }
        public string Picture { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public bool Archived { get; set; }       
        public string TechnicalSkillsCSV { get; set; }
        public string GeneralSkillsCSV { get; set; }
        public string SkillType { get; set; }
        public IList<SelectListItem> SkillValueList { get; set; }
        public List<string> selectedValues { get; set; }
    }
}