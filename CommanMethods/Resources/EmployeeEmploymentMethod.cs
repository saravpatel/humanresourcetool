using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace HRTool.CommanMethods.Resources
{
    public class EmployeeEmploymentMethod
    {
        #region Constant
        EvolutionEntities _db = new EvolutionEntities();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        private string inputFormat = "dd-MM-yyyy";
        private string outputFormat = "yyyy-MM-dd HH:mm:ss";
        #endregion      
       
        public List<GetPublicHolidayByCountryID_Result>getPublicHolidayByCountry()
        {
            string countyId = _db.HolidaysNAbsence_Setting.Select(x => x.PublicHolidayTemplate).FirstOrDefault();
            int cid = Convert.ToInt16(countyId);
            return _db.GetPublicHolidayByCountryID(cid).ToList();
        }

        //public List< GetPublicHoliday_Result> getPublicHoliday(int Id)
        //{
        //    return _db.GetPublicHoliday(Id).ToList();
        //}
        public DateTime getStartDateOfEmp(int Id)
        {
            DateTime startDate = _db.AspNetUsers.Where(x => x.Id == Id).FirstOrDefault().StartDate.Value;
            return startDate;
        }
        public List<GetTotalHolidayEntiAndPublicHoliday_Result> getHolidayEntiAndPublicHoliday()
        {
            return _db.GetTotalHolidayEntiAndPublicHoliday().ToList();
        }
        public int getEmployeeActiveYear()
        {
            int year = _db.HolidayYearAndMonthSettings.Where(x => x.IsActive == true).FirstOrDefault().StartYear.Value;
            return year;           
        }
        public int getEmployeeActiveEndYear()
        {
            int EndYear = _db.HolidayYearAndMonthSettings.Where(x => x.IsActive == true).FirstOrDefault().EndYear.Value;
            return EndYear;
        }
        public int getEmployeeActiveStartMonth()
        {
            int startMonth = _db.HolidayYearAndMonthSettings.Where(x => x.IsActive == true).FirstOrDefault().StartMonth.Value;
            return startMonth;
        }
        public int getEmployeeActiveEndMonth()
        {
            int EndMonth = _db.HolidayYearAndMonthSettings.Where(x => x.IsActive == true).FirstOrDefault().EndMonth.Value;
            return EndMonth;
        }
        public void UpdateEmploymentDetail(EmployeeEmploymentViewModel model)
        {
            AspNetUser employeeData = _db.AspNetUsers.Where(x => x.Id == model.EmployeeId).FirstOrDefault();
            if (!string.IsNullOrEmpty(model.ProbationEndDate))
            {
                var ProbationEndDateToString = DateTime.ParseExact(model.ProbationEndDate, inputFormat, CultureInfo.InvariantCulture);
                employeeData.ProbationEndDate = Convert.ToDateTime(ProbationEndDateToString.ToString(outputFormat));
            }
            if (!string.IsNullOrEmpty(model.NextProbationReviewDate))
            {
                var NextProbationReviewDateToString = DateTime.ParseExact(model.NextProbationReviewDate, inputFormat, CultureInfo.InvariantCulture);
                employeeData.NextProbationReviewDate = Convert.ToDateTime(NextProbationReviewDateToString.ToString(outputFormat));
            }
            employeeData.NoticePeriod = model.NoticePeriod;
            if (!string.IsNullOrEmpty(model.FixedTermEndDate))
            {
                var FixedTermEndDateToString = DateTime.ParseExact(model.FixedTermEndDate, inputFormat, CultureInfo.InvariantCulture);
                employeeData.FixedTermEndDate = Convert.ToDateTime(FixedTermEndDateToString.ToString(outputFormat));
            }
            employeeData.MethodofRecruitmentSetup = model.MethodofRecruitmentSetup;
            employeeData.RecruitmentCost = model.RecruitmentCost;
            if (model.HolidayEnti != 0 && model.HolidayEnti != null)
            {
                employeeData.HolidayEntitlement = model.HolidayEnti;
            }
            if(model.ActivityTypeId!=null && model.ActivityTypeId!=0)
            {
                employeeData.ActivityType = model.ActivityTypeId;
            }
            employeeData.Thisyear = model.ThisYearHolidays;
            employeeData.Nextyear = model.NextYearHolidays;
            if (model.rate != 0 && model.rate != null)
            {
                employeeData.RecovryRate = Convert.ToDecimal(model.rate);
            }
            _db.SaveChanges();
        }
    }
}