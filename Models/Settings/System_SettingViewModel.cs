using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.Models.Settings
{
    public class System_SettingViewModel
    {
        public System_SettingViewModel ()
        {
            SystemSettingList = new List<System_SettingViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<System_SettingViewModel> SystemSettingList { get; set; }
    }
}