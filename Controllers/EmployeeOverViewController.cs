using HRTool.CommanMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRTool.Models.Me;
using HRTool.CommanMethods.Me;
using HRTool.CommanMethods.Admin;
using HRTool.DataModel;
using HRTool.CommanMethods.Resources;
using HRTool.Models.Resources;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class EmployeeOverViewController : Controller
    {
        //
        // GET: /EmployeeOverView/
        EvolutionEntities _db = new EvolutionEntities();
        MeEmployeeInformation _EmployeeProfileMethod = new MeEmployeeInformation();
        AdminPearformanceMethod _AdminPearformanceMethod = new AdminPearformanceMethod();
        EmployeeEmploymentMethod _employeeEmploymentMethod = new EmployeeEmploymentMethod();
        public ActionResult Index(int EmployeeId)
        {

            DateTime datetimeDOB, datetimeDOH;
            int countAge;
            Double count=0;
            MeEmployeeDetail_Overview datamodal = new MeEmployeeDetail_Overview();
            var data = _EmployeeProfileMethod.getEmployeeData(EmployeeId);
            int toatalHolidayRemaining = 0;
            foreach (var item in data)
            {
                datamodal.id = EmployeeId;
                datamodal.CountryOfResidence = item.CName;
                datamodal.jobtitle = item.JobTitle;
                datamodal.business = item.BusinessName;
                datamodal.Division = item.DivisionName;
                datamodal.pool = item.PoolName;
                datamodal.FunctionName = item.FunctionName;
                datamodal.workPhone = item.WorkPhone;
                var GetReportto = _db.EmployeeRelations.Where(x => x.UserID == EmployeeId).Take(1).SingleOrDefault();
                if(GetReportto != null)
                {
                    int GetRepoertToId = Convert.ToInt32(GetReportto.Reportsto);
                    if(GetRepoertToId != 0)
                    {
                        var GetReportName = _db.AspNetUsers.Where(x => x.Id == GetRepoertToId).Take(1).SingleOrDefault();
                        var UsernameofReportto = GetReportName.FirstName + " " + GetReportName.LastName;
                        datamodal.ReportTo = UsernameofReportto;
                    }
                }
                datetimeDOB = Convert.ToDateTime(item.DOB);
                datamodal.DOB = datetimeDOB.ToString("ddd,dd MMM yyyy");
                datamodal.gender = item.Gender;
                datamodal.emailId = item.EmpEmail;
                datamodal.mobile = item.Mobile;
                datetimeDOH = Convert.ToDateTime(item.StartDate);
                datamodal.DateOfHire = datetimeDOH.ToString("ddd,dd MMM yyyy");
                datamodal.Location = item.Location;
                datamodal.workPhone = item.WorkPhone != null ? item.WorkPhone : "";
                countAge = CalculateAge(datetimeDOB);
                count = countAbsenceDay(EmployeeId);
                datamodal.AbsencesDay = countAbsenceDay(EmployeeId);
                datamodal.Age = countAge;
                datamodal.workedDay = countTotalWorkedDayInYear(EmployeeId);
                datamodal.DaysWorkedSinceEver = countTotalWorkDaySinceEver(EmployeeId);
                datamodal.employmentDay = _EmployeeProfileMethod.getTotalWorkingDayInfo(EmployeeId);
                datamodal.annualLeave = _EmployeeProfileMethod.getEmployeApproveAnnualLeave(EmployeeId);
                datamodal.totalLeave = _EmployeeProfileMethod.getTotalEmpoyeeHolidayThisYear(EmployeeId);

                toatalHolidayRemaining = datamodal.totalLeave - datamodal.annualLeave;
                datamodal.toatalHolidayRemain = toatalHolidayRemaining;
                datamodal.contractDays = countContractDays(EmployeeId);
                
            }
            return View(datamodal);
        }
        public double countContractDays(int EmployeeId)
        {
            EmployeeCalculationModel calculation = new EmployeeCalculationModel();
            Double publicHoliday = 0;
            Double contractDays = 0;
            Double IncludedHoliday = 0;
            double totalHoliday = 0;
            int year = _employeeEmploymentMethod.getEmployeeActiveYear();
            int endYear = _employeeEmploymentMethod.getEmployeeActiveEndYear();
            int startMonth = _employeeEmploymentMethod.getEmployeeActiveStartMonth();
            int endMonth = _employeeEmploymentMethod.getEmployeeActiveEndMonth();
            int dayInmonth = System.DateTime.DaysInMonth(endYear, endMonth);
            DateTime startdate = new DateTime(year, startMonth, 1);
            DateTime EndDate = new DateTime(endYear, endMonth, 31);
            double diff = (EndDate - startdate).TotalDays;
            int totalWeekend = counttotalWeekends(year, endYear, startMonth, endMonth);
            var data = _employeeEmploymentMethod.getHolidayEntiAndPublicHoliday();
            var publicHolidayCount = _employeeEmploymentMethod.getPublicHolidayByCountry();
            var employee = _db.AspNetUsers.Where(x => x.Id == EmployeeId).ToList();
            Double holidayEnti = Convert.ToDouble(employee.FirstOrDefault().HolidayEntitlement);
            foreach (var item in publicHolidayCount)
            {
                int Year = DateTime.Now.Year;
                DateTime holidayYear = Convert.ToDateTime(item.PublicHolidayDate);
                if (year == holidayYear.Year)
                {
                    publicHoliday++;
                }
            }
            if (employee.FirstOrDefault().IncludePublicHoliday==true)
            {
             
                foreach (var item in data)
                {
                    IncludedHoliday = holidayEnti - publicHoliday;
                    contractDays = diff - publicHoliday - Convert.ToDouble(totalWeekend) - IncludedHoliday;
                }
            }
            else
            {
                foreach (var item in data)
                {                   
                    contractDays = diff - publicHoliday - Convert.ToDouble(totalWeekend) - holidayEnti;
                }
            }
            
            return contractDays;
        }
        public int counttotalWeekends(int styear, int endyear, int startMonth, int endMonth)
        {
            int count = 0;
            for (int j = startMonth; j <= endMonth; j++)
            {
                DateTime EndOfMonth = new DateTime(styear, j, DateTime.DaysInMonth(styear, j));
                int day = EndOfMonth.Day;
                for (int i = 0; i <= day; i++)
                {
                    if (i >= 1)
                    {
                        DateTime d = new DateTime(styear, j, i);
                        if (d.DayOfWeek == DayOfWeek.Sunday || d.DayOfWeek == DayOfWeek.Saturday)
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        public int CalculateAge(DateTime DOB)
        {
            DateTime dateOfBirth = Convert.ToDateTime(DOB);
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
            {
                age = age - 1;
            }
            return age;
        }
        public ActionResult getResourceEmployeeInfo(int EmployeeId)
        {
            //EmployeeDetail_Overview modal = new MeEmployeeDetail_Overview();

            List<MeEmployeeResourceDetail_Overview> resourceModal = new List<MeEmployeeResourceDetail_Overview>();
            var RsouceEmployeeData = _EmployeeProfileMethod.getResourceEmployeeData(EmployeeId);
            foreach (var item in RsouceEmployeeData)
            {
                MeEmployeeResourceDetail_Overview modal = new MeEmployeeResourceDetail_Overview();
                string EmpName = item.FirstName + ' ' + item.LastName + '-' + item.SSOID;
                modal.Name = EmpName;
                modal.Location = item.Location;
                modal.business = item.Business;
                modal.Division = item.Division;
                modal.pool = item.PoolName;
                modal.FunctionName = item.FunctionName;
                resourceModal.Add(modal);
            }
            
            return PartialView("_PartialMeEmployeeResources", resourceModal);
        }
        public Double countAbsenceDay(int EmployeeId)
        {
            int count_SickLeave = _db.Employee_SickLeaves.Where(x=>x.EmployeeId==EmployeeId && x.StartDate.Value.Year==DateTime.Now.Year).Count();
            int count_otherLeave = _db.Employee_OtherLeave.Where(x => x.EmployeeId == EmployeeId && x.StartDate.Value.Year == DateTime.Now.Year).Count();
            double totalAbesnce = Convert.ToDouble(count_otherLeave) + Convert.ToDouble(count_SickLeave);
            return totalAbesnce;
        }
        //public Double countAbsenceDay(int EmployeeId)
        //{
        //    int count = 0;
        //    int workCount =0;
        //    var item = _EmployeeProfileMethod.getEmplyeeWorkDate(EmployeeId);
        //    var workDay = _EmployeeProfileMethod.getEmployeeWrokDay(EmployeeId);
        //    Double absenceDay = 0;
        //    CountAbsenceDay model = new CountAbsenceDay();
        //    DateTime startDate;
        //    int year = 0;
        //    int month = 0;
        //    int day = 0;
        //    Double diffOfDate=0;
        //    foreach (var data in item)
        //    {            
        //        startDate =Convert.ToDateTime(data.StartDate);
        //        year = data.StartDate.Value.Year;
        //        month = data.StartDate.Value.Month;
        //        day = data.StartDate.Value.Day;
        //        count = counttotalWeekends(startDate.Year,DateTime.Now.Year, startDate.Month,DateTime.Now.Month);
        //   //     count=countSunday(year,month,day);
        //        diffOfDate = DateTime.Today.Subtract(startDate).TotalDays + 1;
        //    }
        //    foreach(var data in workDay)
        //    {
        //        model.workDay = Convert.ToDateTime(data.Date);
        //        workCount = workCount+ countWorkDay(model.workDay.Day);
        //    }

        //    absenceDay = diffOfDate - Convert.ToDouble(count)-workCount;
        //    if(absenceDay>=0)
        //    {
        //        return absenceDay;
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}
        public int countWorkDay(int StartDay)
        {
            int count = 0;
            yearInfo yearModal = new yearInfo();
            var yearData = _EmployeeProfileMethod.getEmployeeYear(DateTime.Now.Year);
            foreach(var data in yearData)
            {
                yearModal.year = Convert.ToInt32(data.StartYear);
                yearModal.month = Convert.ToInt32(data.StartMonth);
            }
            if (yearModal.year != 0)
            {
                DateTime EndOfMonth = new DateTime(yearModal.year, yearModal.month, DateTime.DaysInMonth(yearModal.year, yearModal.month));
                int day = EndOfMonth.Day;
                for (int i = 0; i < day; i++)
                {
                    if (i == StartDay)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        public int countTotalWorkDaySinceEver(int EmployeeID)
        {
            int count = 0;
            var item = _EmployeeProfileMethod.getEmplyeeWorkDate(EmployeeID);
            var workDay = _EmployeeProfileMethod.getEmployeeWrokDay(EmployeeID);
            CountAbsenceDay model = new CountAbsenceDay();
            foreach (var data in workDay)
            {
                model.workDay = Convert.ToDateTime(data.Date);
                int year = data.Date.Value.Year;
                int month = data.Date.Value.Month;
                int StartDay = data.Date.Value.Day;
                DateTime EndOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                int day = EndOfMonth.Day;
                for(int i = 0; i < day; i++)
                {
                    if(i == StartDay)
                    {
                         count++;
                    }
                 }                
                }
            return count;
        }
        public int countTotalWorkedDayInYear(int EmployeeID) {
            int count = 0;
            var item = _EmployeeProfileMethod.getEmplyeeWorkDate(EmployeeID);
            var workDay = _EmployeeProfileMethod.getEmployeeWrokDay(EmployeeID);
            CountAbsenceDay model = new CountAbsenceDay();
            foreach (var data in workDay)
            {
                model.workDay = Convert.ToDateTime(data.Date);
                int year = data.Date.Value.Year;
                int month= data.Date.Value.Month;
                int StartDay = data.Date.Value.Day;
                DateTime EndOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                int day = EndOfMonth.Day;
                
                int currurntYear = DateTime.Now.Year;
                if (currurntYear == year)
                {
                    for (int i = 0; i < day; i++)
                    {
                        if (i == StartDay)
                        {
                            count++;
                        }
                    }
                } }
            return count;

        }
        public int countSunday(int year,int month,int StartDay)
        {
            DateTime EndOfMonth = new DateTime(year,month, DateTime.DaysInMonth(year,month));
            int day = EndOfMonth.Day;
            int count = 0;           
            for(int i=0;i<=day;i++)
            {
                if (i >= StartDay)
                {
                    DateTime d = new DateTime(year, month, i );
                    if (d.DayOfWeek == DayOfWeek.Sunday || d.DayOfWeek == DayOfWeek.Saturday)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    


    }
}