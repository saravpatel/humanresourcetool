using HRTool.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.CommanMethods.Settings
{
    public class MobileSettingMethod
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();

        #endregion

        public Mobile_setting getMobileSetting()
        {
            Mobile_setting data = new Mobile_setting();
            var tableData = _db.Mobile_setting.FirstOrDefault();
            if (tableData != null)
            {
                data = tableData;
            }
            return data;
        }

        public void saveData(bool AllowMobile, bool ShowPhoneNumber)
        {
            var tableDataOLd = getMobileSetting();
            if (tableDataOLd.Id == 0)
            {
                Mobile_setting tableData = new Mobile_setting();
                tableData.AllowMobileUse = AllowMobile;
                tableData.ShowPhoneNumber = ShowPhoneNumber;
                _db.Mobile_setting.Add(tableData);
                _db.SaveChanges();
            }
            else
            {
                var tableData = _db.Mobile_setting.FirstOrDefault();
                tableData.AllowMobileUse = AllowMobile;
                tableData.ShowPhoneNumber = ShowPhoneNumber;
                _db.SaveChanges();
            }

        }
    }
}