using HRTool.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.CommanMethods.Settings
{
    public class GeneralSkillsSetMethod
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();

        #endregion

        public SkillSet getSkillSetById(int Id)
        {
            return _db.SkillSets.Where(x => x.Id == Id).FirstOrDefault();
        }

        public List<SkillSet> getAllList()
        {
            return _db.SkillSets.Where(x => x.SkillType == "General Skills").ToList();
        }

        public void SaveSkillsSet(int Id, string Value, string Description, string SkillValueIds, string ImahePath, int UserId)
        {

            if (Id > 0)
            {
                SkillSet SkillSets = _db.SkillSets.Where(x => x.Id == Id).FirstOrDefault();
                SkillSets.Name = Value;
                SkillSets.Description = Description;
                if (!string.IsNullOrEmpty(ImahePath))
                {
                    SkillSets.Picture = ImahePath;
                }
                SkillSets.Date = DateTime.Now;
                SkillSets.GeneralSkillsCSV = SkillValueIds;
                SkillSets.UserIDLastModifiedBy = UserId;
                SkillSets.LastModified = DateTime.Now;
                _db.SaveChanges();

            }
            else
            {
                SkillSet SkillSets = new SkillSet();
                SkillSets.Name = Value;
                SkillSets.Description = Description;
                SkillSets.Picture = ImahePath;
                SkillSets.Date = DateTime.Now;
                SkillSets.Archived = false;
                SkillSets.UserIDCreatedBy = UserId;
                SkillSets.CreatedDate = DateTime.Now;
                SkillSets.UserIDLastModifiedBy = UserId;
                SkillSets.LastModified = DateTime.Now;
                SkillSets.GeneralSkillsCSV = SkillValueIds;
                SkillSets.SkillType = "General Skills";
                _db.SkillSets.Add(SkillSets);
                _db.SaveChanges();
            }
        }
    }

}