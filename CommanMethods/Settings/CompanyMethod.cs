using HRTool.DataModel;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.CommanMethods.Settings
{
    public class CompanyMethod
    {
        #region Constant
        EvolutionEntities _db = new EvolutionEntities();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        CurrencyConverterMethod _CurrencyConverterMethod = new CurrencyConverterMethod();
        #endregion

        #region Company Methods

        public int InsertUpdateCompanySetting(CompanyViewModel model, int UserID,bool Insert)
        {
             Company_Settings Companysetting = new Company_Settings();
            if (Insert) {
                Companysetting = _db.Company_Settings.Where(x => x.Id == model.Id).FirstOrDefault();
            }
            Companysetting.Logo = model.Logo;
            Companysetting.Statement = model.Statement;
            Companysetting.IndustryID = model.IndustryID;
            Companysetting.DateFormat = model.DateFormat;
            Companysetting.TimeFormat = model.TimeFormat;
            Companysetting.BaseCurrency = model.BaseCurrency;
            Companysetting.DailyAdminEmail = model.DailyAdminEmail;
            Companysetting.WeeklyManagerEmail = model.WeeklyManagerEmail;
            Companysetting.WeeklyEmployeeEmail = model.WeeklyEmployeeEmail;
            Companysetting.ManagerSeeEmployeeSalary = model.ManagerSeeEmployeeSalary;
            Companysetting.EmployeeSeeSalary = model.EmployeeSeeSalary;
            Companysetting.OrganisationChart = model.OrganisationChart;
            Companysetting.OrganisationChartExternalLink = model.OrganisationChartExternalLink;
            Companysetting.AllowEmployeeUploadPhoto = model.AllowEmployeeUploadPhoto;
            Companysetting.ManagerSeeEmployeeContactDetail = model.ManagerSeeEmployeeContactDetail;
            Companysetting.ManagerUploadDocument = model.ManagerUploadDocument;
            Companysetting.CompanyReport = model.CompanyReport;
            Companysetting.ProbationPeriod = model.ProbationPeriod +" "+model.ProbationPeriodValue;
            Companysetting.EmployeeAccess = model.EmployeeAccess;
            Companysetting.ManagerAccess = model.ManagerAccess;
            Companysetting.OtherLeaveReasons = model.OtherLeaveReasons;
            if (Insert)
            {
                Companysetting.UserIDLastModifiedBy = UserID;
                Companysetting.LastModified = DateTime.Now;
            }
            else
            {
                Companysetting.UserIDCreatedBy = UserID;
                Companysetting.UserIDLastModifiedBy = UserID;
                Companysetting.CreatedDate = DateTime.Now;
                Companysetting.LastModified = DateTime.Now;
                _db.Company_Settings.Add(Companysetting);
            
            }
           
            _db.SaveChanges();
            return Companysetting.Id;
        }
        public CompanyViewModel BindCompanySettingRecords(int Id)
        {
            CompanyViewModel Companysetting = new CompanyViewModel();
            Companysetting=defaultCompanyDatabind();
            var model = _db.Company_Settings.Where(x => x.Id == Id).FirstOrDefault();
            Companysetting.Id = model.Id;
            Companysetting.Logo = model.Logo;
            Companysetting.Statement = model.Statement;
            Companysetting.IndustryID = (int)model.IndustryID;
            Companysetting.DateFormat = (int)model.DateFormat;
            Companysetting.TimeFormat = (int)model.TimeFormat;
            Companysetting.BaseCurrency = (int)model.BaseCurrency;
            Companysetting.DailyAdminEmail = model.DailyAdminEmail;
            Companysetting.WeeklyManagerEmail = model.WeeklyManagerEmail;
            Companysetting.WeeklyEmployeeEmail = model.WeeklyEmployeeEmail;
            Companysetting.ManagerSeeEmployeeSalary = model.ManagerSeeEmployeeSalary;
            Companysetting.EmployeeSeeSalary = model.EmployeeSeeSalary;
            Companysetting.OrganisationChart = model.OrganisationChart;
            Companysetting.OrganisationChartExternalLink = model.OrganisationChartExternalLink;
            Companysetting.AllowEmployeeUploadPhoto = model.AllowEmployeeUploadPhoto;
            Companysetting.ManagerSeeEmployeeContactDetail = model.ManagerSeeEmployeeContactDetail;
            Companysetting.ManagerUploadDocument = model.ManagerUploadDocument;
            Companysetting.CompanyReport = model.CompanyReport;
            Companysetting.ProbationPeriod = model.ProbationPeriod.Split(' ')[0].Trim();
            Companysetting.ProbationPeriodValue = model.ProbationPeriod.Split(' ')[1].Trim();
            Companysetting.EmployeeAccess = model.EmployeeAccess;
            Companysetting.ManagerAccess = model.ManagerAccess;
            Companysetting.OtherLeaveReasons = model.OtherLeaveReasons;
            return Companysetting;
        }
        public Company_Settings CompanysettingList()
        {
            return _db.Company_Settings.FirstOrDefault();
        }

        public CompanyViewModel defaultCompanyDatabind() 
        {
            CompanyViewModel Companysetting = new CompanyViewModel();
            Companysetting.Id = 0;
            var Industrie = _otherSettingMethod.getSystemListByName("Document Type");
            var dateformat = _otherSettingMethod.getSystemListByName("Date Format");
            var timeformat = _otherSettingMethod.getSystemListByName("Time Format");
            var Company = _otherSettingMethod.getSystemListByName("Company Access");
            var curriencyList = _CurrencyConverterMethod.GetCurrencyListRecord();
            var IndustrieList = _otherSettingMethod.getAllSystemValueListByNameId(Industrie.Id);
            var dateformatList = _otherSettingMethod.getAllSystemValueListByNameId(dateformat.Id);
            var timeformatList = _otherSettingMethod.getAllSystemValueListByNameId(timeformat.Id);
            var CompanyList = _otherSettingMethod.getAllSystemValueListByNameId(Company.Id);

          
            foreach(var item in IndustrieList)
            {
                Companysetting.IndustryList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            foreach (var item in curriencyList)
            {
                Companysetting.BaseCurrencyList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            foreach (var item in dateformatList)
            {
                Companysetting.DateFormatList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            foreach (var item in timeformatList)
            {
                Companysetting.TimeFormatList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            foreach (var item in CompanyList)
            {
                Companysetting.EmployeeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            foreach (var item in CompanyList)
            {
                Companysetting.ManagerList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }

            return Companysetting;

        }

        #endregion
    }
}