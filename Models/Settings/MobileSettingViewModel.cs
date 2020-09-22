using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.Models.Settings
{
    public class MobileSettingViewModel
    {
        public int Id { get; set; }
        public Nullable<bool> AllowMobileUse { get; set; }
        public Nullable<bool> ShowPhoneNumber { get; set; }
    }
}