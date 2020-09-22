using HRTool.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.CommanMethods.Settings
{
    public class CompanyStructureMethod
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();

        #endregion
        public SystemList getSystemListByName(string Name)
        {
            return _db.SystemLists.Where(x => x.SystemListName == Name).FirstOrDefault();
        }
        public List<SystemListValue> getAllSystemValueListByNameId(int Id)
        {
            return _db.SystemListValues.Where(x => x.Archived == false && x.SystemListID == Id).ToList();
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
        #region Business Methods
        public Business getBusinessListById(int Id)
        {
            return _db.Businesses.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
        }

        public List<Business> getAllBusinessList()
        {
            return _db.Businesses.Where(x => x.Archived == false).ToList();
        }

        public bool SaveBusinessData(string Name, int Id, int UserId)
        {
            var data = _db.Businesses.Where(x => x.Name == Name && x.Id != Id && x.Archived == false).ToList();
            if (data.Count > 0)
            {
                return false;
            }
            else
            {
                if (Id == 0)
                {
                    Business save = new Business();
                    save.Name = Name;
                    save.CreatedDate = DateTime.Now;
                    save.Archived = false;
                    save.UserIDCreatedBy = UserId;
                    save.UserIDLastModifiedBy = UserId;
                    save.LastModified = DateTime.Now;
                    _db.Businesses.Add(save);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    var update = _db.Businesses.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
                    update.Name = Name;
                    update.UserIDLastModifiedBy = UserId;
                    update.LastModified = DateTime.Now;
                    _db.SaveChanges();
                    return true;
                }
            }
        }

        public bool deleteBusiness(int Id, int UserId)
        {
            var data = _db.Businesses.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
            data.Archived = true;
            data.UserIDLastModifiedBy = UserId;
            data.LastModified = DateTime.Now;
            _db.SaveChanges();
            return true;

        }


        #endregion

        #region Division Methods
        public Division getDivisionById(int Id)
        {
            return _db.Divisions.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
        }

        public List<Division> GetDivisionListByBizId(int businessId)
        {
            return _db.Divisions.Where(x => x.Archived == false && x.BusinessID == businessId).ToList();
        }

        public List<Division> getAllDivisionList()
        {
            return _db.Divisions.Where(x => x.Archived == false).ToList();
        }

        public bool SaveDivisionData(string Name, int Id, int businessId, int UserId)
        {
            var Division = _db.Divisions.Where(x => x.Name == Name && x.Archived == false && x.BusinessID == businessId).ToList();
          
            if (Division.Count > 0)
            {
                return false;
            }
            else
            {
                if (Id == 0)
                {
                    Division save = new Division();
                    save.Name = Name;
                    save.CreatedDate = DateTime.Now;
                    save.Archived = false;
                    save.BusinessID = businessId;
                    save.UserIDCreatedBy = UserId;
                    save.UserIDLastModifiedBy = UserId;
                    save.LastModified = DateTime.Now;
                    _db.Divisions.Add(save);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    var update = _db.Divisions.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
                    update.Name = Name;
                    update.BusinessID = businessId;
                    update.UserIDLastModifiedBy = UserId;
                    update.LastModified = DateTime.Now;
                    _db.SaveChanges();
                    return true;

                }
            }

        }

        public bool deleteDivision(int Id, int UserId)
        {
            var data = _db.Divisions.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
            data.Archived = true;
            data.UserIDLastModifiedBy = UserId;
            data.LastModified = DateTime.Now;
            _db.SaveChanges();
            return true;

        }


        #endregion

        #region Pool Methods

        public Pool getPoolsListById(int Id)
        {
            return _db.Pools.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
        }

        public List<Pool> getAllPoolsList()
        {
            return _db.Pools.Where(x => x.Archived == false).ToList();
        }

        public bool SavePoolData(string Name, int Id, int BusinessId, int DivisionId, int UserId)
        {
            var data = _db.Pools.Where(x => x.Name == Name && x.Archived == false  && x.BusinessID == BusinessId && x.DivisionID == DivisionId).ToList();
            if (data.Count > 0)
            {
                return false;
            }
            else
            {
                if (Id == 0)
                {
                    Pool save = new Pool();
                    save.Name = Name;
                    save.BusinessID = BusinessId;
                    save.DivisionID = DivisionId;
                    save.CreatedDate = DateTime.Now;
                    save.UserIDCreatedBy = UserId;
                    save.UserIDLastModifiedBy = UserId;
                    save.LastModified = DateTime.Now;
                    _db.Pools.Add(save);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    var update = _db.Pools.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
                    update.Name = Name;
                    update.BusinessID = BusinessId;
                    update.DivisionID = DivisionId;
                    update.UserIDLastModifiedBy = UserId;
                    update.LastModified = DateTime.Now;
                    _db.SaveChanges();
                    return true;

                }
            }

        }

        public bool deletePool(int Id, int UserId)
        {
            var data = _db.Pools.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
            data.Archived = true;
            data.UserIDLastModifiedBy = UserId;
            data.LastModified = DateTime.Now;
            _db.SaveChanges();
            return true;

        }

        #endregion

        #region Function Methods
        public Function getFunctionsListById(int Id)
        {
            return _db.Functions.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
        }

        public List<Function> getAllFunctionsList()
        {
            return _db.Functions.Where(x => x.Archived == false).ToList();
        }

        public bool SaveFunctionData(string Name, int Id, int BusinessId, int DivisionId, int UserId)
        {
            var data = _db.Functions.Where(x => x.Name == Name  && x.Archived == false && x.BusinessID == BusinessId && x.DivisionID == DivisionId).ToList();
            if (data.Count > 0)
            {
                return false;
            }
            else
            {
                if (Id == 0)
                {
                    Function save = new Function();
                    save.Name = Name;
                    save.BusinessID = BusinessId;
                    save.DivisionID = DivisionId;
                    save.CreatedDate = DateTime.Now;
                    save.UserIDCreatedBy = UserId;
                    save.UserIDLastModifiedBy = UserId;
                    save.LastModified = DateTime.Now;
                    _db.Functions.Add(save);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    var update = _db.Functions.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
                    update.Name = Name;
                    update.BusinessID = BusinessId;
                    update.DivisionID = DivisionId;
                    update.UserIDLastModifiedBy = UserId;
                    update.LastModified = DateTime.Now;
                    _db.SaveChanges();
                    return true;
                }
            }
        }

        public bool deleteFunction(int Id, int UserId)
        {
            var data = _db.Functions.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
            data.Archived = true;
            data.UserIDLastModifiedBy = UserId;
            data.LastModified = DateTime.Now;
            _db.SaveChanges();
            return true;

        }

        #endregion

        public List<Pool> GetPoolListByBizId(int DivisionId)
        {
            return _db.Pools.Where(x => x.Archived == false && x.DivisionID == DivisionId).ToList();
        }
        public List<Business> GetBussiness()
        {
            return _db.Businesses.Where(x => x.Archived == false).ToList();
        }
        public List<Function> GetFuncationListByBizId(int DivisionId)
        {
            return _db.Functions.Where(x => x.Archived == false && x.DivisionID == DivisionId).ToList();
        }
    }
}