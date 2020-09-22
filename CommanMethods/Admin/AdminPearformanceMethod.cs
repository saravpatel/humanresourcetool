using HRTool.DataModel;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.CommanMethods.Admin
{
    public class AdminPearformanceMethod
    {
        #region const
        EvolutionEntities _db = new EvolutionEntities();
        #endregion
        public IList<PerformanceSetting> getAllList()
        {
            return _db.PerformanceSettings.Where(x=>x.Archived==false).ToList();
        }
        public List<GetResourcePool_Result> getResourcePool(int CustId)
        {
            return _db.GetResourcePool(CustId).ToList();
               
        }
 
        public IList<AspNetUser> getAllUserList()
        {
            return _db.AspNetUsers.Where(x => x.SSOID.StartsWith("W") && x.Archived == false).ToList();

        }
        public void SaveProjectSet(PerformanceSettingViewModel model)
        {
            Guid obj = Guid.NewGuid();
            if (model.Id > 0)
            {
                PerformanceSetting _Perfromance = _db.PerformanceSettings.Where(x => x.Id == model.Id).FirstOrDefault();
                _Perfromance.ReviewText = model.ReviewText;
                _Perfromance.AnnualReview = model.AnnualReview;
                if (!string.IsNullOrEmpty(model.CompletionDate))
                    _Perfromance.CompletionDate = DateTime.ParseExact(model.CompletionDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                _Perfromance.CompanyCSV = model.CompanyCSV;
                _Perfromance.LocationCSV = model.LocationCSV;
                _Perfromance.BusinessCSV = model.BusinessCSV;
                _Perfromance.DivisionCSV = model.DivisionCSV;
                _Perfromance.PoolCSV = model.PoolCSV;
                _Perfromance.FunctionCSV = model.FunctionCSV;
                _Perfromance.JobRoleCSV = model.JobRoleCSV;
                _Perfromance.EmploymentTypeCSV = model.EmploymentTypeCSV;
                _Perfromance.RatingOverAll = model.RatingOverAll;
                _Perfromance.RatingCore = model.RatingCore;
                _Perfromance.RatingJobRole = model.RatingJobRole;
                _Perfromance.OverallScoreJson = model.OverallScoreJson;
                _Perfromance.CoreSegmentJSON = model.CoreSegmentJSON;
                _Perfromance.JobRoleSegmentJSON = model.JobRoleSegmentJSON;
                _Perfromance.CoWorkerSegmentJSON = model.CoWorkerSegmentJSON;
                _Perfromance.CustomerSegmentJSON = model.CustomerSegmentJSON;
                _Perfromance.LastModified = DateTime.Now;
                _Perfromance.UserIDLastModifiedBy = model.CurrentUserId;
                _db.SaveChanges();

            }
            else
            {
                PerformanceSetting _Perfromance = new PerformanceSetting();
                _Perfromance.GuID = obj.ToString();
                _Perfromance.ReviewText = model.ReviewText;
                _Perfromance.AnnualReview = model.AnnualReview;
                if (!string.IsNullOrEmpty(model.CompletionDate))
                    _Perfromance.CompletionDate = DateTime.ParseExact(model.CompletionDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                _Perfromance.CompanyCSV = model.CompanyCSV;
                _Perfromance.LocationCSV = model.LocationCSV;
                _Perfromance.BusinessCSV = model.BusinessCSV;
                _Perfromance.DivisionCSV = model.DivisionCSV;
                _Perfromance.PoolCSV = model.PoolCSV;
                _Perfromance.FunctionCSV = model.FunctionCSV;
                _Perfromance.JobRoleCSV = model.JobRoleCSV;
                _Perfromance.EmploymentTypeCSV = model.EmploymentTypeCSV;
                _Perfromance.RatingOverAll = model.RatingOverAll;
                _Perfromance.RatingCore = model.RatingCore;
                _Perfromance.RatingJobRole = model.RatingJobRole;
                _Perfromance.OverallScoreJson = model.OverallScoreJson;
                _Perfromance.CoreSegmentJSON = model.CoreSegmentJSON;
                _Perfromance.JobRoleSegmentJSON = model.JobRoleSegmentJSON;
                _Perfromance.CoWorkerSegmentJSON = model.CoWorkerSegmentJSON;
                _Perfromance.CustomerSegmentJSON = model.CustomerSegmentJSON;
                _Perfromance.CreatedDate = DateTime.Now;
                _Perfromance.UserIDCreatedBy = model.CurrentUserId;
                _db.PerformanceSettings.Add(_Perfromance);
                _db.SaveChanges();
            }
        }
        public string CompanyId(int Id)
        {
            var Company = _db.SystemListValues.Where(x => x.Id == Id).FirstOrDefault();
            return Company.Value;
        }
        //public int getEmployeeSkill(int Id)
        //{
        //    var data = _db.GetAllEmployeeSkill(Id);
            
        //    return data;
        //}
        public string LocationId(int Id)
        {
            var Location = _db.SystemListValues.Where(x => x.Id == Id).FirstOrDefault();
            return Location.Value;
        }
        public string BusinessId(int Id)
        {
            var Business = _db.Businesses.Where(x => x.Id == Id).FirstOrDefault();
            return Business.Name;
        }
        public string DivisionId(int Id)
        {
            var Business = _db.Divisions.Where(x => x.Id == Id).FirstOrDefault();
            return Business.Name;
        }
        public string PoolId(int Id)
        {
            var Business = _db.Pools.Where(x => x.Id == Id).FirstOrDefault();
            return Business.Name;
        }
        public string FucantionId(int Id)
        {
            var Business = _db.Functions.Where(x => x.Id == Id).FirstOrDefault();
            return Business.Name;
        }
        public string JobtitleId(int Id)
        {
            var JobTital = _db.SystemListValues.Where(x => x.Id == Id).FirstOrDefault();
            return JobTital.Value;
        }
        public string EmployeementId(int Id)
        {
            var Employeement = _db.SystemListValues.Where(x => x.Id == Id).FirstOrDefault();
            return Employeement.Value;
        }



        public PerformanceSetting getPerformanceSetById(int Id)
        {
            return _db.PerformanceSettings.Where(x => x.Id == Id).FirstOrDefault();
        }


        public bool deletePerformance(int Id,int userId)
        {
            var model = _db.PerformanceSettings.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
            model.Archived = true;
            model.UserIDLastModifiedBy = userId;
            model.LastModified = DateTime.Now;
            _db.SaveChanges();
            return true;            
        }
    }
}