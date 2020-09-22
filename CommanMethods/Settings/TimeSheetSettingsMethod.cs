using HRTool.DataModel;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace HRTool.CommanMethods.Settings
{
    public class TimeSheetSettingsMethod
    {
        #region Constant

        private string inputFormat = "dd-MM-yyyy";
        private string outputFormat = "yyyy-MM-dd HH:mm:ss";
        EvolutionEntities _db = new EvolutionEntities();

        #endregion

        public TimeSheet_Setting getTimeSheetSetting()
        {
            TimeSheet_Setting data = new TimeSheet_Setting();
            var tableData = _db.TimeSheet_Setting.FirstOrDefault();
            if (tableData != null)
            {
                data = tableData;
            }
            return data;
        }

        public void saveData(int projectId, int FrequencyId, int DetailId)
        {
            var tableDataOLd = getTimeSheetSetting();
            if (tableDataOLd.Id > 0)
            {
                TimeSheet_Setting tableData = new TimeSheet_Setting();
                tableData.ProjectId = projectId;
                tableData.Frequency = FrequencyId;
                tableData.Detail = DetailId;
                _db.TimeSheet_Setting.Add(tableData);
                _db.SaveChanges();
            }
            else
            {
                var tableData = _db.TimeSheet_Setting.FirstOrDefault();
                tableData.ProjectId = projectId;
                tableData.Frequency = FrequencyId;
                tableData.Detail = DetailId;
                _db.SaveChanges();
            }

        }


    }
}