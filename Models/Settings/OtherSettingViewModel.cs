using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.Models.Settings
{
    public class OtherSettingViewModel
    {
        public OtherSettingViewModel()
        {
            valueList = new List<OtherSettingValueViewModel>();
        }
        public int Id { get; set; }
        public string SystemListName { get; set; }
        public bool Archived { get; set; }       
        public IList<OtherSettingValueViewModel> valueList { get; set; }
    }

    public class OtherSettingValueViewModel
    {
        public int Id { get; set; }
        public int SystemListID { get; set; }
        public string Value { get; set; }
        public bool Archived { get; set; }      
    }
    public class systemListData {
        public int Id { get; set; }
        public string ListValue { get; set; }

    }
}