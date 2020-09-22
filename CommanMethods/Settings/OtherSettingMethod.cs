using HRTool.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HRTool.CommanMethods;
using HRTool.Models.Settings;

namespace HRTool.CommanMethods.Settings
{
    public class OtherSettingMethod
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();

        #endregion

        #region Methods

        public SystemList getSystemListById(int Id)
        {
            return _db.SystemLists.Where(x => x.Id == Id).FirstOrDefault();
        }

        public SystemList getSystemListByName(string Name)
        {
            return _db.SystemLists.Where(x => x.SystemListName == Name ).FirstOrDefault();
        }

        public List<SystemList> getAllSystemLise()
        {
            return _db.SystemLists.Where(x => x.Archived == false).ToList();
        }

        public List<SystemListValue> getAllSystemValueListByNameId(int Id)
        {
            return _db.SystemListValues.Where(x => x.Archived == false && x.SystemListID == Id).ToList();
        }

        public List<ApplicantRejectReason> getApplicantRejectReason()
        {
            return _db.ApplicantRejectReasons.Where(x => x.Archived == false).ToList();
        }


        public List<SystemListValue> getAllSystemValueListByKeyName(string KeyName)
        {
            SystemList systemName = getSystemListByName(KeyName);
            if (systemName != null)
            {
                return getAllSystemValueListByNameId(systemName.Id);
            }
            else
            {
                return null;
            }
        }
        public int getSystemListId(string KeyName)
        {
            SystemList systemName = getSystemListByName(KeyName);
            if (systemName != null)
            {
                return systemName.Id;
            }
            else
            {
                return 0;
            }
        }

        public SystemListValue getSystemListValueById(int Id)
        {
            return _db.SystemListValues.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
        }

        public void SaveData(int Id, string ListName, string[] ListValue, int UserId)
        {
            if (Id > 0)
            {
                foreach (var item in ListValue)
                {
                    SystemListValue SystemListValue = new SystemListValue();
                    SystemListValue.SystemListID = Id;
                    SystemListValue.Value = item;
                    SystemListValue.Archived = false;
                    SystemListValue.UserIDCreatedBy = UserId;
                    SystemListValue.CreatedDate = DateTime.Now;
                    SystemListValue.UserIDLastModifiedBy = UserId;
                    SystemListValue.LastModified = DateTime.Now;
                    SystemListValue.Description = "";
                    _db.SystemListValues.Add(SystemListValue);
                    _db.SaveChanges();
                }
            }
            else
            {
                SystemList SystemLists = new SystemList();
                SystemLists.SystemListName = ListName;
                SystemLists.Archived = false;
                SystemLists.UserIDCreatedBy = UserId;
                SystemLists.CreatedDate = DateTime.Now;
                SystemLists.UserIDLastModifiedBy = UserId;
                SystemLists.LastModified = DateTime.Now;
                _db.SystemLists.Add(SystemLists);
                _db.SaveChanges();

                foreach (var item in ListValue)
                {
                    SystemListValue SystemListValue = new SystemListValue();
                    SystemListValue.SystemListID = SystemLists.Id;
                    SystemListValue.Value = item;
                    SystemListValue.Archived = false;
                    SystemListValue.UserIDCreatedBy = UserId;
                    SystemListValue.CreatedDate = DateTime.Now;
                    SystemListValue.UserIDLastModifiedBy = UserId;
                    SystemListValue.LastModified = DateTime.Now;
                    SystemListValue.Description = "";
                    _db.SystemListValues.Add(SystemListValue);
                    _db.SaveChanges();
                }
            }
        }
        public void EditData(int Id, string ListName, List<systemListData> ListData, int UserId)
        {
            if (Id > 0)
            {

                foreach (var item in ListData)
                {
                    SystemListValue SystemListValue = _db.SystemListValues.Where(x => x.Id == item.Id).FirstOrDefault();
                    SystemListValue.Value = item.ListValue;
                    SystemListValue.UserIDLastModifiedBy = UserId;
                    SystemListValue.LastModified = DateTime.Now;
                    _db.SaveChanges();
                }
            }
        }
        #endregion

        #region CountryList

        public List<Country> countryList()
        {
            return _db.Countries.ToList();
        }

        public Country getCountryById(int Id)
        {
            return _db.Countries.Where(x => x.Id == Id).FirstOrDefault();
        }
        #endregion
    }
}