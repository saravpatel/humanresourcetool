using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Settings
{
    public class HolidayNAbsenceViewModel
    {
        public HolidayNAbsenceViewModel()
        {
            countryList = new List<SelectListItem>();
            workPatternList = new List<SelectListItem>();
            calculationPeriodList = new List<SelectListItem>();
            holidayYearList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public string WorkingHours { get; set; }
        public string WorkPattern { get; set; }
        public string WorkPatternValue { get; set; }
        public string AnnualLeave { get; set; }
        public string CarryOverDays { get; set; }
        public string CarryOverHours { get; set; }
        public string HolidayYear { get; set; }
        public string HolidayYearValue { get; set; }
        public string PublicHolidayTemplate { get; set; }
        public string PublicHolidayTemplateValue { get; set; }
        public bool TotalHolidayEntitlement { get; set; }
        public string HolidayEntitlement { get; set; }
        public bool HolidayReturn { get; set; }

        public bool AuthoriseHolidays { get; set; }
        public string TOILPeriod { get; set; }

        public bool BradfordFactorAlerts { get; set; }
        public string CalculationPeriod { get; set; }
        public string CalculationPeriodValue { get; set; }


        public IList<SelectListItem> countryList { get; set; }
        public BradfordFactorViewModel BradfordFactor { get; set; }
        public IList<SelectListItem> workPatternList { get; set; }

        public IList<SelectListItem> calculationPeriodList { get; set; }

        public IList<SelectListItem> holidayYearList { get; set; }

    }


    public class publicHolidayCounty
    {
        public publicHolidayCounty()
        {
            holidayList = new List<publicHolidayCountyList>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<publicHolidayCountyList> holidayList { get; set; }
    }

    public class publicHolidayCountyList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public int countryId { get; set; }
    }

    public class BradfordFactorViewModel
    {
        public int Id { get; set; }
        public Nullable<int> LowerValue1 { get; set; }
        public Nullable<int> UpperValue1 { get; set; }
        public string Alert1 { get; set; }
        public Nullable<int> LowerValue2 { get; set; }
        public Nullable<int> UpperValue2 { get; set; }
        public string Alert2 { get; set; }
        public Nullable<int> LowerValue3 { get; set; }
        public Nullable<int> UpperValue3 { get; set; }
        public string Alert3 { get; set; }
        public Nullable<int> LowerValue4 { get; set; }
        public Nullable<int> UpperValue4 { get; set; }
        public string Alert4 { get; set; }
    }

    public class WorkPatternViewModel
    {
        public WorkPatternViewModel()
        {
            WorkPatternList = new List<WorkPatternListViewModel>();
        }
        public int Id { get; set; }
        public decimal TotalHours { get; set; }
        public string Effectivefromdate { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> MondayHours { get; set; }
        public Nullable<decimal> MondayDays { get; set; }
        public decimal MondayStart { get; set; }
        public decimal MondayEnd { get; set; }
        public Nullable<int> MondayBreakMins { get; set; }
        public Nullable<decimal> TuesdayHours { get; set; }
        public Nullable<decimal> TuesdayDays { get; set; }
        public decimal TuesdayStart { get; set; }
        public decimal TuesdayEnd { get; set; }
        public Nullable<int> TuesdayBreakMins { get; set; }
        public Nullable<decimal> WednessdayHours { get; set; }
        public Nullable<decimal> WednessdayDays { get; set; }
        public decimal WednessdayStart { get; set; }
        public decimal WednessdayEnd { get; set; }
        public Nullable<int> WednessdayBreakMins { get; set; }
        public Nullable<decimal> ThursdayHours { get; set; }
        public Nullable<decimal> ThursdayDays { get; set; }
        public decimal ThursdayStart { get; set; }
        public decimal ThursdayEnd { get; set; }
        public Nullable<int> ThursdayBreakMins { get; set; }
        public Nullable<decimal> FridayHours { get; set; }
        public Nullable<decimal> FridayDays { get; set; }
        public decimal FridayStart { get; set; }
        public decimal FridayEnd { get; set; }
        public Nullable<int> FridayBreakMins { get; set; }
        public Nullable<decimal> SaturdayHours { get; set; }
        public Nullable<decimal> SaturdayDays { get; set; }
        public decimal SaturdayStart { get; set; }
        public decimal SaturdayEnd { get; set; }
        public Nullable<int> SaturdayBreakMins { get; set; }
        public Nullable<decimal> SundayHours { get; set; }
        public Nullable<decimal> SundayDays { get; set; }
        public decimal SundayStart { get; set; }
        public decimal SundayEnd { get; set; }
        public Nullable<int> SundayBreakMins { get; set; }
        public bool IsRotating { get; set; }

        public IList<WorkPatternListViewModel> WorkPatternList { get; set; }

    }

    public class WorkPatternListViewModel
    {
        public int Id { get; set; }

        //public int CurrentWeekWorkPatternDetailID { get; set; }
        public int CurrentWeekWorkPatternDetailID { get; set; }
        public decimal TotalHours { get; set; }
        public int WorkPatternID { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> MondayHours { get; set; }
        public Nullable<decimal> MondayDays { get; set; }
        public  string MondayStart { get; set; }
        public  string MondayEnd { get; set; }
        public Nullable<int> MondayBreakMins { get; set; }
        public Nullable<decimal> TuesdayHours { get; set; }
        public Nullable<decimal> TuesdayDays { get; set; }
        public  string TuesdayStart { get; set; }
        public  string TuesdayEnd { get; set; }
        public Nullable<int> TuesdayBreakMins { get; set; }
        public Nullable<decimal> WednessdayHours { get; set; }
        public Nullable<decimal> WednessdayDays { get; set; }
        public  string WednessdayStart { get; set; }
        public  string WednessdayEnd { get; set; }
        public Nullable<int> WednessdayBreakMins { get; set; }
        public Nullable<decimal> ThursdayHours { get; set; }
        public Nullable<decimal> ThursdayDays { get; set; }
        public  string ThursdayStart { get; set; }
        public  string ThursdayEnd { get; set; }
        public Nullable<int> ThursdayBreakMins { get; set; }
        public Nullable<decimal> FridayHours { get; set; }
        public Nullable<decimal> FridayDays { get; set; }
        public  string FridayStart { get; set; }
        public  string FridayEnd { get; set; }
        public Nullable<int> FridayBreakMins { get; set; }
        public Nullable<decimal> SaturdayHours { get; set; }
        public Nullable<decimal> SaturdayDays { get; set; }
        public  string SaturdayStart { get; set; }
        public  string SaturdayEnd { get; set; }
        public Nullable<int> SaturdayBreakMins { get; set; }
        public Nullable<decimal> SundayHours { get; set; }
        public Nullable<decimal> SundayDays { get; set; }
        public  string SundayStart { get; set; }
        public  string SundayEnd { get; set; }
        public Nullable<int> SundayBreakMins { get; set; }
    }
}