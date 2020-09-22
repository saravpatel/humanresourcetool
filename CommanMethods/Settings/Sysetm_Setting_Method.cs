using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRTool.DataModel;
namespace HRTool.CommanMethods.Settings
{
    public class Sysetm_Setting_Method
    {
        EvolutionEntities _db = new EvolutionEntities();
        public List<System_settings> getAllSystemList()
        {
            return _db.System_settings.ToList();
        }
    }
}