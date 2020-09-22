using HRTool.DataModel;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.CommanMethods.Settings
{
    public class HolidayNAbsenceMethod
    {

        #region Constant

        private string inputFormat = "dd-MM-yyyy";
        private string outputFormat = "yyyy-MM-dd HH:mm:ss";
        EvolutionEntities _db = new EvolutionEntities();
        OtherSettingMethod _OtherSettingMethod = new OtherSettingMethod();

        #endregion

        public PublicHolidayCountry getCountryById(int Id)
        {
            return _db.PublicHolidayCountries.Where(x => x.Id == Id).FirstOrDefault();
        }

        public List<PublicHolidayCountry> getAllCountryList()
        {
            return _db.PublicHolidayCountries.Where(x => x.Archived == false).ToList();
        }

        public List<GetHolidayAndAbsenecByEmployee_Result> getHolidayAndAbsenceByEmployee(int EmpId)
        {
            return _db.GetHolidayAndAbsenecByEmployee(EmpId).ToList();
        }
        public List<PublicHoliday> getHolidayListByCountryId(int Id)
        {
            return _db.PublicHolidays.Where(x => x.PublicHolidayCountryID == Id).ToList();
        }

        public List<PublicHoliday> getHolidayList()
        {
            return _db.PublicHolidays.Where(x => x.Archived == false).ToList();
        }

        public List<PublicHoliday> GetAllHolidayListById(int Id)
        {
            return _db.PublicHolidays.Where(x => x.Id == Id).ToList();
        }


        public publicHolidayCounty getCountryData(int Id)
        {

            publicHolidayCounty model = new publicHolidayCounty();
            var countryData = getCountryById(Id);
            model.Id = countryData.Id;
            model.Name = countryData.Name;

            foreach (var item in getHolidayListByCountryId(Id))
            {
                publicHolidayCountyList holidayModel = new publicHolidayCountyList();
                holidayModel.Id = item.Id;
                holidayModel.Name = item.Name;
                holidayModel.Date = String.Format("{0:dd-MM-yyy}", item.Date);
                model.holidayList.Add(holidayModel);
            }
            return model;
        }

        public void SaveHolidayData(int CountryId, string CountryName, List<publicHolidayCountyList> HolidayData, int UserId)
        {
            if (CountryId > 0)
            {
                PublicHolidayCountry country = _db.PublicHolidayCountries.Where(x => x.Id == CountryId).FirstOrDefault();
                country.Name = CountryName;
                country.UserIDLastModifiedBy = UserId;
                country.LastModified = DateTime.Now;
                _db.SaveChanges();

                foreach (var item in HolidayData)
                {
                    PublicHoliday holidayData = new PublicHoliday();
                    holidayData.Name = item.Name;
                    var holidayDate = DateTime.ParseExact(item.Date, inputFormat, CultureInfo.InvariantCulture);
                    holidayData.Date = Convert.ToDateTime(holidayDate.ToString(outputFormat));
                    holidayData.PublicHolidayCountryID = country.Id;
                    holidayData.Archived = false;
                    holidayData.UserIDCreatedBy = UserId;
                    holidayData.CreatedDate = DateTime.Now;
                    holidayData.UserIDLastModifiedBy = UserId;
                    holidayData.LastModified = DateTime.Now;
                    _db.PublicHolidays.Add(holidayData);
                    _db.SaveChanges();
                }
            }
            else
            {
                PublicHolidayCountry country = new PublicHolidayCountry();
                country.Name = CountryName;
                country.Archived = false;
                country.UserIDCreatedBy = UserId;
                country.CreatedDate = DateTime.Now;
                country.UserIDLastModifiedBy = UserId;
                country.LastModified = DateTime.Now;
                _db.PublicHolidayCountries.Add(country);
                _db.SaveChanges();

                foreach (var item in HolidayData)
                {
                    PublicHoliday holidayData = new PublicHoliday();
                    holidayData.Name = item.Name;
                    var holidayDate = DateTime.ParseExact(item.Date, inputFormat, CultureInfo.InvariantCulture);
                    holidayData.Date = Convert.ToDateTime(holidayDate.ToString(outputFormat));
                    holidayData.PublicHolidayCountryID = country.Id;
                    holidayData.Archived = false;
                    holidayData.UserIDCreatedBy = UserId;
                    holidayData.CreatedDate = DateTime.Now;
                    holidayData.UserIDLastModifiedBy = UserId;
                    holidayData.LastModified = DateTime.Now;
                    _db.PublicHolidays.Add(holidayData);
                    _db.SaveChanges();
                }


            }

        }

        public void updateHoliday(int Id, string Name, string Date, int UserId)
        {
            PublicHoliday holidayData = _db.PublicHolidays.Where(x => x.Id == Id).FirstOrDefault();
            holidayData.Name = Name;
            var holidayDate = DateTime.ParseExact(Date, inputFormat, CultureInfo.InvariantCulture);
            holidayData.Date = Convert.ToDateTime(holidayDate.ToString(outputFormat));
            holidayData.UserIDLastModifiedBy = UserId;
            holidayData.LastModified = DateTime.Now;
            _db.SaveChanges();
        }

        public void dleteHoliday(int Id, int UserId)
        {
            PublicHoliday holidayData = _db.PublicHolidays.Where(x => x.Id == Id).FirstOrDefault();
            holidayData.Archived = true;
            holidayData.UserIDLastModifiedBy = UserId;
            holidayData.LastModified = DateTime.Now;
            _db.SaveChanges();
        }

        #region BradfordFactor

        public BradfordFactor_HolidaySettings getBradfordFactor()
        {
            BradfordFactor_HolidaySettings bradfordFactor = new BradfordFactor_HolidaySettings();
            var firstData = _db.BradfordFactor_HolidaySettings.FirstOrDefault();
            if (firstData != null)
            {
                bradfordFactor = firstData;
            }
            return bradfordFactor;
        }

        public void SaveBradfordFactor(BradfordFactorViewModel model)
        {
            if (model.Id > 0)
            {
                BradfordFactor_HolidaySettings bradfordFactor = _db.BradfordFactor_HolidaySettings.FirstOrDefault();
                bradfordFactor.LowerValue1 = model.LowerValue1;
                bradfordFactor.UpperValue1 = model.UpperValue1;
                bradfordFactor.Alert1 = model.Alert1;
                bradfordFactor.LowerValue2 = model.LowerValue2;
                bradfordFactor.UpperValue2 = model.UpperValue2;
                bradfordFactor.Alert2 = model.Alert2;
                bradfordFactor.LowerValue3 = model.LowerValue3;
                bradfordFactor.UpperValue3 = model.UpperValue3;
                bradfordFactor.Alert3 = model.Alert3;
                bradfordFactor.LowerValue4 = model.LowerValue4;
                bradfordFactor.UpperValue4 = model.UpperValue4;
                bradfordFactor.Alert4 = model.Alert4;
                _db.SaveChanges();
            }
            else
            {
                BradfordFactor_HolidaySettings bradfordFactor = new BradfordFactor_HolidaySettings();
                bradfordFactor.LowerValue1 = model.LowerValue1;
                bradfordFactor.UpperValue1 = model.UpperValue1;
                bradfordFactor.Alert1 = model.Alert1;
                bradfordFactor.LowerValue2 = model.LowerValue2;
                bradfordFactor.UpperValue2 = model.UpperValue2;
                bradfordFactor.Alert2 = model.Alert2;
                bradfordFactor.LowerValue3 = model.LowerValue3;
                bradfordFactor.UpperValue3 = model.UpperValue3;
                bradfordFactor.Alert3 = model.Alert3;
                bradfordFactor.LowerValue4 = model.LowerValue4;
                bradfordFactor.UpperValue4 = model.UpperValue4;
                bradfordFactor.Alert4 = model.Alert4;
                _db.BradfordFactor_HolidaySettings.Add(bradfordFactor);
                _db.SaveChanges();
            }
        }

        #endregion

        public IList<WorkPatternDetail> allWorkPatternDetail()
        {
            return _db.WorkPatternDetails.Where(x=>x.Archived == false).ToList();
        }
        public IList<WorkPatternDetail> workPatternDetaiiById(int workPatternId)
        {
            return _db.WorkPatternDetails.Where(x => x.WorkPatternID == workPatternId).ToList();
        }

        public IList<WorkPattern> getAllWorkPattern()
        {
            return _db.WorkPatterns.Where(x=>x.Archived==false).ToList();
        }

        public WorkPattern getWorkPatternById(int Id)
        {
            return _db.WorkPatterns.Where(x => x.Id == Id && x.Archived ==false).FirstOrDefault();
        }

        public void SaveFalseRoatingData(WorkPatternViewModel model)
        {

            if (model.Id > 0)
            {
                foreach (var item in workPatternDetaiiById(model.Id))
                {
                    _db.WorkPatternDetails.Remove(item);
                    _db.SaveChanges();
                }

                WorkPattern data = _db.WorkPatterns.Where(x => x.Id == model.Id).FirstOrDefault();
                data.Name = model.Name;
                data.IsRotating = model.IsRotating;
                data.MondayHours = model.MondayHours;
                data.MondayDays = model.MondayDays;
                if (model.MondayStart!=0)
                    data.MondayStart = Convert.ToDecimal(model.MondayStart);
                if (model.MondayEnd != 0)
                    data.MondayEnd = Convert.ToDecimal(model.MondayEnd);
                data.MondayBreakMins = model.MondayBreakMins;
                data.TuesdayHours = model.TuesdayHours;
                data.TuesdayDays = model.TuesdayDays;
                if (model.TuesdayStart!=0)
                    data.TuesdayStart = Convert.ToDecimal(model.TuesdayStart);
                if (model.TuesdayEnd != 0)
                    data.TuesdayEnd = Convert.ToDecimal(model.TuesdayEnd);
                data.TuesdayBreakMins = model.TuesdayBreakMins;
                data.WednessdayHours = model.WednessdayHours;
                data.WednessdayDays = model.WednessdayDays;
                if (model.WednessdayStart!=0)
                    data.WednessdayStart = Convert.ToDecimal(model.WednessdayStart);
                if (model.WednessdayEnd!=0)
                    data.WednessdayEnd = Convert.ToDecimal(model.WednessdayEnd);
                data.WednessdayBreakMins = model.WednessdayBreakMins;
                data.ThursdayHours = model.ThursdayHours;
                data.ThursdayDays = model.ThursdayDays;
                if (model.ThursdayStart!=0)
                    data.ThursdayStart = Convert.ToDecimal(model.ThursdayStart);
                if (model.ThursdayEnd != 0)
                    data.ThursdayEnd = Convert.ToDecimal(model.ThursdayEnd);
                data.ThursdayBreakMins = model.ThursdayBreakMins;
                data.FridayHours = model.FridayHours;
                data.FridayDays = model.FridayDays;
                if (model.FridayStart != 0)
                    data.FridayStart = Convert.ToDecimal(model.FridayStart);
                if (model.FridayEnd != 0)
                    data.FridayEnd = Convert.ToDecimal(model.FridayEnd);
                data.FridayBreakMins = model.FridayBreakMins;
                data.SaturdayHours = model.SaturdayHours;
                data.SaturdayDays = model.SaturdayDays;
                if (model.SaturdayStart != 0)
                    data.SaturdayStart = Convert.ToDecimal(model.SaturdayStart);
                if (model.SaturdayEnd != 0)
                    data.SaturdayEnd = Convert.ToDecimal(model.SaturdayEnd);
                data.SaturdayBreakMins = model.SaturdayBreakMins;
                data.SundayHours = model.SundayHours;
                data.SundayDays = model.SundayDays;
                if (model.SundayStart != 0)
                    data.SundayStart = Convert.ToDecimal(model.SundayStart);
                if (model.SundayEnd != 0)
                    data.SundayEnd = Convert.ToDecimal(model.SundayEnd);
                data.SundayBreakMins = model.SundayBreakMins;
                _db.SaveChanges();
            }
            else
            {
                WorkPattern data = new WorkPattern();
                data.Name = model.Name;
                data.IsRotating = model.IsRotating;
                data.MondayHours = model.MondayHours;
                data.MondayDays = model.MondayDays;
                if (model.MondayStart != 0)
                {
                    data.MondayStart =Convert.ToDecimal(model.MondayStart);
                }
                if (model.MondayEnd != 0)
                {
                    data.MondayEnd = Convert.ToDecimal(model.MondayEnd);
                }
                data.MondayBreakMins = model.MondayBreakMins;
                data.TuesdayHours = model.TuesdayHours;
                data.TuesdayDays = model.TuesdayDays;
                if (model.TuesdayStart!=0)                    
                    data.TuesdayStart = Convert.ToDecimal(model.TuesdayStart);
                if (model.TuesdayEnd!=0)
                    data.TuesdayEnd = Convert.ToDecimal(model.TuesdayEnd);
                data.TuesdayBreakMins = model.TuesdayBreakMins;
                data.WednessdayHours = model.WednessdayHours;
                data.WednessdayDays = model.WednessdayDays;
                if (model.WednessdayStart!=0)
                    data.WednessdayStart = Convert.ToDecimal(model.WednessdayStart);
                if (model.WednessdayEnd!=0)
                    data.WednessdayEnd = Convert.ToDecimal(model.WednessdayEnd);
                data.WednessdayBreakMins = model.WednessdayBreakMins;
                data.ThursdayHours = model.ThursdayHours;
                data.ThursdayDays = model.ThursdayDays;
                if (model.ThursdayStart != 0)
                    data.ThursdayStart = Convert.ToDecimal(model.ThursdayStart);
                if (model.ThursdayEnd != 0)
                    data.ThursdayEnd = Convert.ToDecimal(model.ThursdayEnd);
                data.ThursdayBreakMins = model.ThursdayBreakMins;
                data.FridayHours = model.FridayHours;
                data.FridayDays = model.FridayDays;
                if (model.FridayStart != 0)
                    data.FridayStart = Convert.ToDecimal(model.FridayStart);
                if (model.FridayEnd != 0)
                    data.FridayEnd = Convert.ToDecimal(model.FridayEnd);
                data.FridayBreakMins = model.FridayBreakMins;
                data.SaturdayHours = model.SaturdayHours;
                data.SaturdayDays = model.SaturdayDays;
                if (model.SaturdayStart != 0)
                    data.SaturdayStart = Convert.ToDecimal(model.SaturdayStart);
                if (model.SaturdayEnd != 0)
                    data.SaturdayEnd = Convert.ToDecimal(model.SaturdayEnd);
                data.SaturdayBreakMins = model.SaturdayBreakMins;
                data.SundayHours = model.SundayHours;
                data.SundayDays = model.SundayDays;
                if (model.SundayStart != 0)
                    data.SundayStart = Convert.ToDecimal(model.SundayStart);
                if (model.SundayEnd != 0)
                    data.SundayEnd = Convert.ToDecimal(model.SundayEnd);
                data.SundayBreakMins = model.SundayBreakMins;
                _db.WorkPatterns.Add(data);
                _db.SaveChanges();
            }
        }

        public void SaveTrueRoatingData(WorkPatternViewModel model)
        {
            if (model.Id > 0)
            {
                foreach (var item in workPatternDetaiiById(model.Id))
                {
                    _db.WorkPatternDetails.Remove(item);
                    _db.SaveChanges();
                }

                WorkPattern data = _db.WorkPatterns.Where(x => x.Id == model.Id).FirstOrDefault();
                data.Name = model.Name;
                data.IsRotating = model.IsRotating;
                data.MondayHours = null;
                data.MondayDays = null;
                data.MondayStart = null;
                data.MondayEnd = null;
                data.MondayBreakMins = null;
                data.TuesdayHours = null;
                data.TuesdayDays = null;
                data.TuesdayStart = null;
                data.TuesdayEnd = null;
                data.TuesdayBreakMins = null;
                data.WednessdayHours = null;
                data.WednessdayDays = null;
                data.WednessdayStart = null;
                data.WednessdayEnd = null;
                data.WednessdayBreakMins = null;
                data.ThursdayHours = null;
                data.ThursdayDays = null;
                data.ThursdayStart = null;
                data.ThursdayEnd = null;
                data.ThursdayBreakMins = null;
                data.FridayHours = null;
                data.FridayDays = null;
                data.FridayStart = null;
                data.FridayEnd = null;
                data.FridayBreakMins = null;
                data.SaturdayHours = null;
                data.SaturdayDays = null;
                data.SaturdayStart = null;
                data.SaturdayEnd = null;
                data.SaturdayBreakMins = null;
                data.SundayHours = null;
                data.SundayDays = null;
                data.SundayStart = null;
                data.SundayEnd = null;
                data.SundayBreakMins = null;
                _db.SaveChanges();
                var n = 0;
                foreach (var itemData in model.WorkPatternList)
                {
                    n++;
                    WorkPatternDetail detailData = new WorkPatternDetail();
                    detailData.WorkPatternID = model.Id;
                    detailData.Name = n.ToString();
                    detailData.MondayHours = itemData.MondayHours;
                    detailData.MondayDays = itemData.MondayDays;
                    if (!string.IsNullOrEmpty(itemData.MondayStart))
                        detailData.MondayStart = Convert.ToDateTime(itemData.MondayStart);
                    if (!string.IsNullOrEmpty(itemData.MondayEnd))
                        detailData.MondayEnd = Convert.ToDateTime(itemData.MondayStart);
                    detailData.MondayBreakMins = itemData.MondayBreakMins;
                    detailData.TuesdayHours = itemData.TuesdayHours;
                    detailData.TuesdayDays = itemData.TuesdayDays;
                    if (!string.IsNullOrEmpty(itemData.TuesdayStart))
                        detailData.TuesdayStart = Convert.ToDateTime(itemData.TuesdayStart); 
                    if (!string.IsNullOrEmpty(itemData.TuesdayEnd))
                        detailData.TuesdayEnd = Convert.ToDateTime(itemData.TuesdayEnd); 
                    detailData.TuesdayBreakMins = itemData.TuesdayBreakMins;
                    detailData.WednessdayHours = itemData.WednessdayHours;
                    detailData.WednessdayDays = itemData.WednessdayDays;
                    if (!string.IsNullOrEmpty(itemData.WednessdayStart))
                        detailData.WednessdayStart = Convert.ToDateTime(itemData.WednessdayStart); 
                    if (!string.IsNullOrEmpty(itemData.WednessdayEnd))
                        detailData.WednessdayEnd = Convert.ToDateTime(itemData.WednessdayEnd); 
                    detailData.WednessdayBreakMins = itemData.WednessdayBreakMins;
                    detailData.ThursdayHours = itemData.ThursdayHours;
                    detailData.ThursdayDays = itemData.ThursdayDays;
                    if (!string.IsNullOrEmpty(itemData.ThursdayStart))
                        detailData.ThursdayStart = Convert.ToDateTime(itemData.ThursdayStart); 
                    if (!string.IsNullOrEmpty(itemData.ThursdayEnd))
                        detailData.ThursdayEnd = Convert.ToDateTime(itemData.ThursdayEnd); 
                    detailData.ThursdayBreakMins = itemData.ThursdayBreakMins;
                    detailData.FridayHours = itemData.FridayHours;
                    detailData.FridayDays = itemData.FridayDays;
                    if (!string.IsNullOrEmpty(itemData.FridayStart))
                        detailData.FridayStart = Convert.ToDateTime(itemData.FridayStart); 
                    if (!string.IsNullOrEmpty(itemData.FridayEnd))
                        detailData.FridayEnd = Convert.ToDateTime(itemData.FridayEnd); 
                    detailData.FridayBreakMins = itemData.FridayBreakMins;
                    detailData.SaturdayHours = itemData.SaturdayHours;
                    detailData.SaturdayDays = itemData.SaturdayDays;
                    if (!string.IsNullOrEmpty(itemData.SaturdayStart))
                        detailData.SaturdayStart = Convert.ToDateTime(itemData.SaturdayStart);
                    if (!string.IsNullOrEmpty(itemData.SaturdayEnd))
                        detailData.SaturdayEnd = Convert.ToDateTime(itemData.SaturdayEnd); 
                    detailData.SaturdayBreakMins = itemData.SaturdayBreakMins;
                    detailData.SundayHours = itemData.SundayHours;
                    detailData.SundayDays = itemData.SundayDays;
                    if (!string.IsNullOrEmpty(itemData.SundayStart))
                        detailData.SundayStart = Convert.ToDateTime(itemData.SundayStart); 
                    if (!string.IsNullOrEmpty(itemData.SundayEnd))
                        detailData.SundayEnd = Convert.ToDateTime(itemData.SundayEnd); 
                    detailData.SundayBreakMins = itemData.SundayBreakMins;
                    _db.WorkPatternDetails.Add(detailData);
                    _db.SaveChanges();
                }
            }
            else
            {
                WorkPattern item = new WorkPattern();
                item.Name = model.Name;
                item.IsRotating = model.IsRotating;
                _db.WorkPatterns.Add(item);
                _db.SaveChanges();
                var n = 0;
                foreach (var itemData in model.WorkPatternList)
                {
                    n++;
                    WorkPatternDetail data = new WorkPatternDetail();
                    data.WorkPatternID = item.Id;
                    data.Name = n.ToString();
                    data.MondayHours = itemData.MondayHours;
                    data.MondayDays = itemData.MondayDays;
                    if (!string.IsNullOrEmpty(itemData.MondayStart))
                        data.MondayStart = Convert.ToDateTime(itemData.MondayStart);
                    if (!string.IsNullOrEmpty(itemData.MondayEnd))
                        data.MondayEnd = Convert.ToDateTime(itemData.MondayStart);
                    data.MondayBreakMins = itemData.MondayBreakMins;
                    data.TuesdayHours = itemData.TuesdayHours;
                    data.TuesdayDays = itemData.TuesdayDays;
                    if (!string.IsNullOrEmpty(itemData.TuesdayStart))
                        data.TuesdayStart = Convert.ToDateTime(itemData.TuesdayStart);
                    if (!string.IsNullOrEmpty(itemData.TuesdayEnd))
                        data.TuesdayEnd = Convert.ToDateTime(itemData.TuesdayEnd);
                    data.TuesdayBreakMins = itemData.TuesdayBreakMins;
                    data.WednessdayHours = itemData.WednessdayHours;
                    data.WednessdayDays = itemData.WednessdayDays;
                    if (!string.IsNullOrEmpty(itemData.WednessdayStart))
                        data.WednessdayStart = Convert.ToDateTime(itemData.WednessdayStart);
                    if (!string.IsNullOrEmpty(itemData.WednessdayEnd))
                        data.WednessdayEnd = Convert.ToDateTime(itemData.WednessdayEnd);
                    data.WednessdayBreakMins = itemData.WednessdayBreakMins;
                    data.ThursdayHours = itemData.ThursdayHours;
                    data.ThursdayDays = itemData.ThursdayDays;
                    if (!string.IsNullOrEmpty(itemData.ThursdayStart))
                        data.ThursdayStart = Convert.ToDateTime(itemData.ThursdayStart);
                    if (!string.IsNullOrEmpty(itemData.ThursdayEnd))
                        data.ThursdayEnd = Convert.ToDateTime(itemData.ThursdayEnd);
                    data.ThursdayBreakMins = itemData.ThursdayBreakMins;
                    data.FridayHours = itemData.FridayHours;
                    data.FridayDays = itemData.FridayDays;
                    if (!string.IsNullOrEmpty(itemData.FridayStart))
                        data.FridayStart = Convert.ToDateTime(itemData.FridayStart);
                    if (!string.IsNullOrEmpty(itemData.FridayEnd))
                        data.FridayEnd = Convert.ToDateTime(itemData.FridayEnd);
                    data.FridayBreakMins = itemData.FridayBreakMins;
                    data.SaturdayHours = itemData.SaturdayHours;
                    data.SaturdayDays = itemData.SaturdayDays;
                    if (!string.IsNullOrEmpty(itemData.SaturdayStart))
                        data.SaturdayStart = Convert.ToDateTime(itemData.SaturdayStart);
                    if (!string.IsNullOrEmpty(itemData.SaturdayEnd))
                        data.SaturdayEnd = Convert.ToDateTime(itemData.SaturdayEnd);
                    data.SaturdayBreakMins = itemData.SaturdayBreakMins;
                    data.SundayHours = itemData.SundayHours;
                    data.SundayDays = itemData.SundayDays;
                    if (!string.IsNullOrEmpty(itemData.SundayStart))
                        data.SundayStart = Convert.ToDateTime(itemData.SundayStart);
                    if (!string.IsNullOrEmpty(itemData.SundayEnd))
                        data.SundayEnd = Convert.ToDateTime(itemData.SundayEnd);
                    data.SundayBreakMins = itemData.SundayBreakMins;
                    _db.WorkPatternDetails.Add(data);
                    _db.SaveChanges();
                }
            }
        }

        public WorkPatternViewModel returnModel(int Id)
        {
            WorkPatternViewModel returnModel = new WorkPatternViewModel();
            var data = getWorkPatternById(Id);
            returnModel.Id = Id;
            returnModel.Name = data.Name;
            returnModel.IsRotating = data.IsRotating;
            if (data.IsRotating)
            {
                foreach (var item in workPatternDetaiiById(Id))
                {
                    WorkPatternListViewModel detailData = new WorkPatternListViewModel();
                    detailData.Id = item.Id;
                    detailData.Name = item.Name;
                    detailData.WorkPatternID = item.WorkPatternID;
                    detailData.MondayHours = item.MondayHours;
                    detailData.MondayDays = item.MondayDays;
                    detailData.MondayStart = String.Format("{0:HH:mm}", item.MondayStart);
                    detailData.MondayEnd = String.Format("{0:HH:mm}", item.MondayEnd);
                    detailData.MondayBreakMins = item.MondayBreakMins;
                    detailData.TuesdayHours = item.TuesdayHours;
                    detailData.TuesdayDays = item.TuesdayDays;
                    detailData.TuesdayStart = String.Format("{0:HH:mm}", item.TuesdayStart);
                    detailData.TuesdayEnd = String.Format("{0:HH:mm}", item.TuesdayEnd);
                    detailData.TuesdayBreakMins = item.TuesdayBreakMins;
                    detailData.WednessdayHours = item.WednessdayHours;
                    detailData.WednessdayDays = item.WednessdayDays;
                    detailData.WednessdayStart = String.Format("{0:HH:mm}", item.WednessdayStart);
                    detailData.WednessdayEnd = String.Format("{0:HH:mm}", item.WednessdayEnd);
                    detailData.WednessdayBreakMins = item.WednessdayBreakMins;
                    detailData.ThursdayHours = item.ThursdayHours;
                    detailData.ThursdayDays = item.ThursdayDays;
                    detailData.ThursdayStart = String.Format("{0:HH:mm}", item.ThursdayStart);
                    detailData.ThursdayEnd = String.Format("{0:HH:mm}", item.ThursdayEnd);
                    detailData.ThursdayBreakMins = item.ThursdayBreakMins;
                    detailData.FridayHours = item.FridayHours;
                    detailData.FridayDays = item.FridayDays;
                    detailData.FridayStart = String.Format("{0:HH:mm}", item.FridayStart);
                    detailData.FridayEnd = String.Format("{0:HH:mm}", item.FridayEnd);
                    detailData.FridayBreakMins = item.FridayBreakMins;
                    detailData.SaturdayHours = item.SaturdayHours;
                    detailData.SaturdayDays = item.SaturdayDays;
                    detailData.SaturdayStart = String.Format("{0:HH:mm}", item.SaturdayStart);
                    detailData.SaturdayEnd = String.Format("{0:HH:mm}", item.SaturdayEnd);
                    detailData.SaturdayBreakMins = item.SaturdayBreakMins;
                    detailData.SundayHours = item.SundayHours;
                    detailData.SundayDays = item.SundayDays;
                    detailData.SundayStart = String.Format("{0:HH:mm}", item.SundayStart);
                    detailData.SundayEnd = String.Format("{0:HH:mm}", item.SundayEnd);
                    detailData.SundayBreakMins = item.SundayBreakMins;
                    detailData.TotalHours = Convert.ToDecimal((item.MondayHours == null ? 0 : item.MondayHours) + (item.TuesdayHours == null ? 0 : item.TuesdayHours) + (item.WednessdayHours == null ? 0 : item.WednessdayHours) + (item.ThursdayHours == null ? 0 : item.ThursdayHours) + (item.FridayHours == null ? 0 : item.FridayHours) + (item.SaturdayHours == null ? 0 : item.SaturdayHours) + (item.SundayHours == null ? 0 : item.SundayHours));

                    returnModel.WorkPatternList.Add(detailData);
                }
            }
            else
            {
                returnModel.MondayHours = data.MondayHours;
                returnModel.MondayDays = data.MondayDays;
                returnModel.MondayStart = Convert.ToDecimal( data.MondayStart);
                returnModel.MondayEnd = Convert.ToDecimal(data.MondayEnd);
                returnModel.MondayBreakMins = data.MondayBreakMins;
                returnModel.TuesdayHours = data.TuesdayHours;
                returnModel.TuesdayDays = data.TuesdayDays;
                returnModel.TuesdayStart = Convert.ToDecimal(data.TuesdayStart);
                returnModel.TuesdayEnd = Convert.ToDecimal(data.TuesdayEnd);
                returnModel.TuesdayBreakMins = data.TuesdayBreakMins;
                returnModel.WednessdayHours = data.WednessdayHours;
                returnModel.WednessdayDays = data.WednessdayDays;
                returnModel.WednessdayStart = Convert.ToDecimal(data.WednessdayStart);
                returnModel.WednessdayEnd = Convert.ToDecimal( data.WednessdayEnd);
                returnModel.WednessdayBreakMins = data.WednessdayBreakMins;
                returnModel.ThursdayHours = data.ThursdayHours;
                returnModel.ThursdayDays = data.ThursdayDays;
                returnModel.ThursdayStart = Convert.ToDecimal(data.ThursdayStart);
                returnModel.ThursdayEnd = Convert.ToDecimal(data.ThursdayEnd);
                returnModel.ThursdayBreakMins = data.ThursdayBreakMins;
                returnModel.FridayHours = data.FridayHours;
                returnModel.FridayDays = data.FridayDays;
                returnModel.FridayStart = Convert.ToDecimal(data.FridayStart);
                returnModel.FridayEnd = Convert.ToDecimal(data.FridayEnd);
                returnModel.FridayBreakMins = data.FridayBreakMins;
                returnModel.SaturdayHours = data.SaturdayHours;
                returnModel.SaturdayDays = data.SaturdayDays;
                returnModel.SaturdayStart = Convert.ToDecimal(data.SaturdayStart);
                returnModel.SaturdayEnd = Convert.ToDecimal(data.SaturdayEnd);
                returnModel.SaturdayBreakMins = data.SaturdayBreakMins;
                returnModel.SundayHours = data.SundayHours;
                returnModel.SundayDays = data.SundayDays;
                returnModel.SundayStart = Convert.ToDecimal(data.SundayStart);
                returnModel.SundayEnd = Convert.ToDecimal(data.SundayEnd);
                returnModel.SundayBreakMins = data.SundayBreakMins;
                returnModel.TotalHours = Convert.ToDecimal((data.MondayHours == null ? 0 : data.MondayHours) + (data.TuesdayHours == null ? 0 : data.TuesdayHours) + (data.WednessdayHours == null ? 0 : data.WednessdayHours) + (data.ThursdayHours == null ? 0 : data.ThursdayHours) + (data.FridayHours == null ? 0 : data.FridayHours) + (data.SaturdayHours == null ? 0 : data.SaturdayHours) + (data.SundayHours == null ? 0 : data.SundayHours));
            }

            return returnModel;
        }

        //Ajay 
        public HolidaysNAbsence_Setting getAllHolidaysNAbsenceSettingList()
        {
            return _db.HolidaysNAbsence_Setting.FirstOrDefault();
        }
        

        public WorkPattern workPatternDetailsById(int workPatternId)
        {
            return _db.WorkPatterns.Where(x => x.Id == workPatternId).FirstOrDefault();
        }

        public HolidayNAbsenceViewModel BindHolidaysNAbsenceSettingRecords(int Id)
        {
            HolidayNAbsenceViewModel Companysetting = new HolidayNAbsenceViewModel();

            var CId = _OtherSettingMethod.getSystemListByName("Calculation Period List");
            var HId = _OtherSettingMethod.getSystemListByName("Holiday Year List");

            Companysetting.countryList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in getAllCountryList())
            {
                Companysetting.countryList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }

            Companysetting.workPatternList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in getAllWorkPattern())
            {
                Companysetting.workPatternList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }

            Companysetting.holidayYearList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });

            foreach (var item in _OtherSettingMethod.getAllSystemValueListByNameId(HId.Id))
            {
                Companysetting.holidayYearList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }

            Companysetting.calculationPeriodList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in _OtherSettingMethod.getAllSystemValueListByNameId(CId.Id))
            {
                Companysetting.calculationPeriodList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }

            Companysetting.BradfordFactor = new BradfordFactorViewModel();
            var bradfordFactor = getBradfordFactor();
            if (bradfordFactor != null)
            {
                Companysetting.BradfordFactor.Id = bradfordFactor.Id;
                Companysetting.BradfordFactor.LowerValue1 = bradfordFactor.LowerValue1;
                Companysetting.BradfordFactor.UpperValue1 = bradfordFactor.UpperValue1;
                Companysetting.BradfordFactor.Alert1 = bradfordFactor.Alert1;
                Companysetting.BradfordFactor.LowerValue2 = bradfordFactor.LowerValue2;
                Companysetting.BradfordFactor.UpperValue2 = bradfordFactor.UpperValue2;
                Companysetting.BradfordFactor.Alert2 = bradfordFactor.Alert2;
                Companysetting.BradfordFactor.LowerValue3 = bradfordFactor.LowerValue3;
                Companysetting.BradfordFactor.UpperValue3 = bradfordFactor.UpperValue3;
                Companysetting.BradfordFactor.Alert3 = bradfordFactor.Alert3;
                Companysetting.BradfordFactor.LowerValue4 = bradfordFactor.LowerValue4;
                Companysetting.BradfordFactor.UpperValue4 = bradfordFactor.UpperValue4;
                Companysetting.BradfordFactor.Alert4 = bradfordFactor.Alert4;
            }

            if (Id != 0)
            {
                var model = _db.HolidaysNAbsence_Setting.Where(x => x.Id == Id).FirstOrDefault();
                var WorkPID = workPatternDetailsById(int.Parse(model.WorkPattern));
                var HoliID = _OtherSettingMethod.getSystemListValueById(int.Parse(model.HolidayYear));
                var countID = getCountryById(int.Parse(model.PublicHolidayTemplate));
                var CalId = _OtherSettingMethod.getSystemListValueById(int.Parse(model.CalculationPeriod));

                Companysetting.Id = model.Id;
                Companysetting.WorkingHours = model.WorkingHours;

                Companysetting.WorkPattern = model.WorkPattern;
                if(WorkPID != null)
                Companysetting.WorkPatternValue = WorkPID.Name;

                Companysetting.AnnualLeave = model.AnnualLeave;
                Companysetting.CarryOverDays = model.CarryOverDays;
                Companysetting.CarryOverHours = model.CarryOverHours;

                Companysetting.HolidayYear = model.HolidayYear;
                if(HoliID != null)
                Companysetting.HolidayYearValue = HoliID.Value;

                Companysetting.PublicHolidayTemplate = model.PublicHolidayTemplate;
                if(countID != null)
                Companysetting.PublicHolidayTemplateValue = countID.Name;

                Companysetting.TotalHolidayEntitlement = model.TotalHolidayEntitlement;
                Companysetting.HolidayEntitlement = model.HolidayEntitlement;
                Companysetting.HolidayReturn = model.HolidayReturn;
                Companysetting.AuthoriseHolidays = model.AuthoriseHolidays;
                Companysetting.TOILPeriod = model.TOILPeriod;
                Companysetting.BradfordFactorAlerts = model.BradfordFactorAlerts;

                Companysetting.CalculationPeriod = model.CalculationPeriod;
                Companysetting.CalculationPeriodValue = CalId.Value;
            }
            return Companysetting;

        }

        public int InsertUpdateHolidaySetting(HolidayNAbsenceViewModel Model, bool Insert)
        {
            HolidaysNAbsence_Setting model = new HolidaysNAbsence_Setting();
            if (Insert)
            {
                model = _db.HolidaysNAbsence_Setting.Where(x => x.Id == Model.Id).FirstOrDefault();
            }
            model.WorkingHours = Model.WorkingHours;
            model.WorkPattern = Model.WorkPattern;
            model.AnnualLeave = Model.AnnualLeave;
            model.CarryOverDays = Model.CarryOverDays;
            model.CarryOverHours = Model.CarryOverHours;
            model.HolidayYear = Model.HolidayYear;
            model.PublicHolidayTemplate = Model.PublicHolidayTemplate;
            model.TotalHolidayEntitlement = Model.TotalHolidayEntitlement;
            model.HolidayEntitlement = Model.HolidayEntitlement;
            model.HolidayReturn = Model.HolidayReturn;
            model.AuthoriseHolidays = Model.AuthoriseHolidays;
            model.TOILPeriod = Model.TOILPeriod;
            model.BradfordFactorAlerts = Model.BradfordFactorAlerts;
            model.CalculationPeriod = Model.CalculationPeriod;
            if (!Insert)
            {
                _db.HolidaysNAbsence_Setting.Add(model);

            }
            _db.SaveChanges();
            return model.Id;

        }

        public PublicHolidayCountry GetPublicHolidayCountryById(int Id)
        {
            return _db.PublicHolidayCountries.Where(x => x.Id == Id  && x.Archived ==false).FirstOrDefault();
        } 
    }
}