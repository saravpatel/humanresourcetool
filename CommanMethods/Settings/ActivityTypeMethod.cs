using HRTool.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.CommanMethods.Settings
{
    public class ActivityTypeMethod
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();

        #endregion

        #region ActivityType Methods

        public ActivityType getActivityTypeListById(int Id)
        {
            return _db.ActivityTypes.Where(x => x.Id == Id).FirstOrDefault();
        }

        public List<ActivityType> getAllActivityTypeList()
        {
            return _db.ActivityTypes.Where(x => x.Archived == false).ToList();
        }

        public bool SaveActivityTypeData(int Id, int UserId, int year, string Name, int CurriencyId, int WorkUnitId, decimal? workerRate, decimal? customerRate)
        {
            var data = _db.ActivityTypes.Where(x => x.Name == Name && x.Id != Id && x.Archived == false).ToList();

            if (data.Count > 0)
            {
                return false;
            }
            else
            {
                if (Id == 0)
                {
                    ActivityType save = new ActivityType();
                    save.Year = year;
                    save.Name = Name;
                    save.CurrencyID = CurriencyId;
                    save.WorkUnitID = WorkUnitId;
                    save.WorkerRate = workerRate;
                    save.CustomerRate = customerRate;
                    save.CreatedDate = DateTime.Now;
                    save.Archived = false;
                    save.UserIDCreatedBy = UserId;
                    save.UserIDLastModifiedBy = UserId;
                    save.LastModified = DateTime.Now;
                    _db.ActivityTypes.Add(save);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    var update = _db.ActivityTypes.Where(x => x.Id == Id).FirstOrDefault();
                    update.Year = year;
                    update.Name = Name;
                    update.CurrencyID = CurriencyId;
                    update.WorkUnitID = WorkUnitId;
                    update.WorkerRate = workerRate;
                    update.CustomerRate = customerRate;
                    update.UserIDLastModifiedBy = UserId;
                    update.LastModified = DateTime.Now;
                    _db.SaveChanges();
                    return true;
                }
            }
        }

        public bool deleteActivityType(int Id, int UserId)
        {
            var data = _db.ActivityTypes.Where(x => x.Id == Id).FirstOrDefault();
            data.Archived = true;
            data.UserIDLastModifiedBy = UserId;
            data.LastModified = DateTime.Now;
            _db.SaveChanges();
            return true;

        }

        #endregion
    }
}