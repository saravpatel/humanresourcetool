using HRTool.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.CommanMethods.Settings
{
    public class AddSkillsMethod
    {

        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();

        #endregion

       

        public void SaveSkills(int Id, string Value, string Description, string SkillType, int UserId)
        {
            int skillTypeId = 0;
            if (SkillType == "Technical")
            {
                SystemList systemName = _otherSettingMethod.getSystemListByName("Technical Skills");
                skillTypeId = systemName.Id;
            }
            else
            {
                SystemList systemName = _otherSettingMethod.getSystemListByName("General Skills");
                skillTypeId = systemName.Id;
            }

            if (Id > 0)
            {
                SystemListValue SystemListValue = _db.SystemListValues.Where(x => x.Id == Id).FirstOrDefault();
                SystemListValue.SystemListID = skillTypeId;
                SystemListValue.Value = Value;
                SystemListValue.Archived = false;
                SystemListValue.UserIDCreatedBy = UserId;
                SystemListValue.CreatedDate = DateTime.Now;
                SystemListValue.UserIDLastModifiedBy = UserId;
                SystemListValue.LastModified = DateTime.Now;
                SystemListValue.Description = Description;

                _db.SaveChanges();

            }
            else
            {
                SystemListValue SystemListValue = new SystemListValue();
                SystemListValue.SystemListID = skillTypeId;
                SystemListValue.Value = Value;
                SystemListValue.Archived = false;
                SystemListValue.UserIDCreatedBy = UserId;
                SystemListValue.CreatedDate = DateTime.Now;
                SystemListValue.UserIDLastModifiedBy = UserId;
                SystemListValue.LastModified = DateTime.Now;
                SystemListValue.Description = Description;
                _db.SystemListValues.Add(SystemListValue);
                _db.SaveChanges();
            }
        }
    }
}