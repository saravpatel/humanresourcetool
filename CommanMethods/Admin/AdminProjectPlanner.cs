using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRTool.Models.Resources;
using HRTool.DataModel;
using System.Globalization;

namespace HRTool.CommanMethods.Admin
{
    public class AdminProjectPlanner
    {
        EvolutionEntities _db = new EvolutionEntities();
        private string inputFormat = "dd-MM-yyyy";
        private string outputFormat = "yyyy-MM-dd HH:mm:ss";
        public bool SaveData_SchedulingLeave(EmployeeProjectPlanner_Scheduling_DocumentsViewModel model, int UserId)
        {
            if (model.Id > 0)
            {
                var Leave = _db.Employee_ProjectPlanner_Scheduling.Where(x => x.Id == model.Id).FirstOrDefault();
                Leave.EmployeeId = model.EmployeeId;
                Leave.IsDayOrMore = model.IsDayOrMore;
                Leave.IsLessThenADay = model.IsLessThenADay;
                if (model.IsDayOrMore)
                {
                    var StartDateToString = DateTime.ParseExact(model.StartDate, inputFormat, CultureInfo.InvariantCulture);
                    Leave.StartDate = Convert.ToDateTime(StartDateToString.ToString(outputFormat));
                    var EndDateToString = DateTime.ParseExact(model.EndDate, inputFormat, CultureInfo.InvariantCulture);
                    Leave.EndDate = Convert.ToDateTime(EndDateToString.ToString(outputFormat));
                    Leave.DurationDays = model.DurationDays;
                    Leave.DurationHr = 0;
                    Leave.InTimeHr = 0;
                    Leave.InTimeMin = 0;
                    Leave.EndTimeHr = 0;
                    Leave.EndTimeMin = 0;
                }
                if (model.IsLessThenADay)
                {
                    var StartDateToString = DateTime.ParseExact(model.StartDate, inputFormat, CultureInfo.InvariantCulture);
                    Leave.StartDate = Convert.ToDateTime(StartDateToString.ToString(outputFormat));
                    Leave.InTimeHr = model.InTimeHr;
                    Leave.InTimeMin = model.InTimeMin;
                    Leave.EndTimeHr = model.EndTimeHr;
                    Leave.EndTimeMin = model.EndTimeMin;
                    Leave.DurationHr = model.DurationHr;
                    Leave.DurationDays = null;
                    Leave.EndDate = Convert.ToDateTime(StartDateToString.ToString(outputFormat));
                }
                Leave.CustomerId = model.Customer;
                Leave.ProjectId = model.Project;
                Leave.AssetId = model.Asset;
                Leave.Comment = model.Comments;
                Leave.LastModifiedBy = UserId;
                Leave.LastModifiedDate = DateTime.Now;
                _db.SaveChanges();
            }
            else
            {
                Employee_ProjectPlanner_Scheduling Leave = new Employee_ProjectPlanner_Scheduling();
                Leave.EmployeeId = model.EmployeeId;
                Leave.IsDayOrMore = model.IsDayOrMore;
                Leave.IsLessThenADay = model.IsLessThenADay;
                if (model.IsDayOrMore)
                {
                    var StartDateToString = DateTime.ParseExact(model.StartDate, inputFormat, CultureInfo.InvariantCulture);
                    Leave.StartDate = Convert.ToDateTime(StartDateToString.ToString(outputFormat));
                    var EndDateToString = DateTime.ParseExact(model.EndDate, inputFormat, CultureInfo.InvariantCulture);
                    Leave.EndDate = Convert.ToDateTime(EndDateToString.ToString(outputFormat));
                    Leave.DurationDays = model.DurationDays;
                }
                if (model.IsLessThenADay)
                {
                    var StartDateToString = DateTime.ParseExact(model.StartDate, inputFormat, CultureInfo.InvariantCulture);
                    Leave.StartDate = Convert.ToDateTime(StartDateToString.ToString(outputFormat));
                    Leave.InTimeHr = model.InTimeHr;
                    Leave.InTimeMin = model.InTimeMin;
                    Leave.EndTimeHr = model.EndTimeHr;
                    Leave.EndTimeMin = model.EndTimeMin;
                    Leave.DurationHr = model.DurationHr;
                    Leave.EndDate = Convert.ToDateTime(StartDateToString.ToString(outputFormat));
                }
                Leave.CustomerId = model.Customer;
                Leave.ProjectId = model.Project;
                Leave.AssetId = model.Asset;
                Leave.Comment = model.Comments;
                Leave.Archived = false;
                Leave.CreatedBy = UserId;
                Leave.LastModifiedBy = UserId;
                Leave.CreatedDate = DateTime.Now;
                Leave.ApprovalStatus = "Pending";
                Leave.LastModifiedDate = DateTime.Now;
                _db.Employee_ProjectPlanner_Scheduling.Add(Leave);
                _db.SaveChanges();

            }
            return true;
        }
        public List<GetResourceDatabyFilter_Result> getResourceDataByFilter(DateTime date)
        {
            return _db.GetResourceDatabyFilter(date).ToList();
        }
        public List<Employee_ProjectPlanner_Scheduling> getSchedulingDataByDate(DateTime date,int EmpID)
        {
            return _db.Employee_ProjectPlanner_Scheduling.Where(x => x.StartDate <= date && x.EndDate>=date &&x.EmployeeId== EmpID && x.Archived == false).ToList();
        }
        public bool validateResource_StartDate(DateTime startDate,int EmpId)
        {
            var data = _db.Employee_ProjectPlanner_Scheduling.ToList();
            int flag = 0;
            foreach (var item in data)
            {
                if (item.EmployeeId == EmpId && item.StartDate== startDate)
                {
                    flag = 1;
                    break;
                }
            }
            if (flag == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        //Travel
        public void TravelLeave_SaveData(EmployeeProjectPlanner_TravelLeaveViewModel model, int UserId)
        {
            if (model.Id > 0)
            {
                Employee_ProjectPlanner_TravelLeave Leave = _db.Employee_ProjectPlanner_TravelLeave.Where(x => x.Id == model.Id).FirstOrDefault();
                Leave.EmployeeId = model.EmployeeId;
                Leave.FromCountryId = model.FromCountryId;
                Leave.FromStateId = model.FromStateId;
                Leave.FromTownId = model.FromTownId;
                Leave.FromAirportId = model.FromAirportId;
                Leave.ToCountryId = model.ToCountryId;
                Leave.ToStateId = model.ToStateId;
                Leave.ToTownId = model.ToTownId;
                Leave.ToAirportId = model.ToAirportId;
                Leave.ReasonForTravelId = model.ReasonForTravelId;
                Leave.IsLessThenADay = model.IsLessThenADay;
                var StartDateToString = DateTime.ParseExact(model.StartDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.StartDate = Convert.ToDateTime(StartDateToString.ToString(outputFormat));
                if (model.IsLessThenADay == false)
                {
                    var endDateToString = DateTime.ParseExact(model.EndDate, inputFormat, CultureInfo.InvariantCulture);
                    Leave.EndDate = Convert.ToDateTime(endDateToString.ToString(outputFormat));
                    Leave.Duration = model.Duration;
                    Leave.StartTimeHour = 0;
                    Leave.StartTimeMin = 0;
                    Leave.EndTimeHour = 0;
                    Leave.EndTimeMin = 0;
                    Leave.DurationHr = 0;
                }
                else
                {
                    var endDateToString = DateTime.ParseExact(model.StartDate, inputFormat, CultureInfo.InvariantCulture);
                    Leave.EndDate = Convert.ToDateTime(endDateToString.ToString(outputFormat));
                    Leave.Duration = 0;
                    Leave.StartTimeHour = model.InTimeHr;
                    Leave.StartTimeMin = model.InTimeMin;
                    Leave.EndTimeHour = model.EndTimeHr;
                    Leave.EndTimeMin = model.EndTimeMin;
                    Leave.DurationHr = model.DurationHr;
                }
                Leave.Comment = model.Comment;
                Leave.Type = model.Type;
                Leave.Project = model.Project;
                Leave.Customer = model.Customer;
                Leave.CostCode = model.CostCode;
                Leave.LastModifiedBy = UserId;
                Leave.LastModifiedDate = DateTime.Now;
                _db.SaveChanges();

                foreach (var item in _db.Employee_ProjectPlanner_TravelLeave_Documents.Where(x => x.TravelLeaveId == Leave.Id).ToList())
                {
                    _db.Employee_ProjectPlanner_TravelLeave_Documents.Remove(item);
                    _db.SaveChanges();
                }

                foreach (var item in model.DocumentList)
                {
                    Employee_ProjectPlanner_TravelLeave_Documents Document = new Employee_ProjectPlanner_TravelLeave_Documents();
                    Document.TravelLeaveId = Leave.Id;
                    Document.NewName = item.newName;
                    Document.OriginalName = item.originalName;
                    Document.Description = item.description;
                    Document.Archived = false;
                    Document.CreatedBy = UserId;
                    Document.CreatedDate = DateTime.Now;
                    Document.LastModifiedBy = UserId;
                    Document.LastModifiedDate = DateTime.Now;
                    _db.Employee_ProjectPlanner_TravelLeave_Documents.Add(Document);
                    _db.SaveChanges();
                }
            }
            else
            {
                Employee_ProjectPlanner_TravelLeave Leave = new Employee_ProjectPlanner_TravelLeave();
                Leave.EmployeeId = model.EmployeeId;
                Leave.FromCountryId = model.FromCountryId;
                Leave.FromStateId = model.FromStateId;
                Leave.FromTownId = model.FromTownId;
                Leave.FromAirportId = model.FromAirportId;
                Leave.ToCountryId = model.ToCountryId;
                Leave.ToStateId = model.ToStateId;
                Leave.ToTownId = model.ToTownId;
                Leave.ToAirportId = model.ToAirportId;
                Leave.ReasonForTravelId = model.ReasonForTravelId;
                Leave.IsLessThenADay = model.IsLessThenADay;

                var StartDateToString = DateTime.ParseExact(model.StartDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.StartDate = Convert.ToDateTime(StartDateToString.ToString(outputFormat));
                if (model.IsLessThenADay == false)
                {
                    var endDateToString = DateTime.ParseExact(model.EndDate, inputFormat, CultureInfo.InvariantCulture);
                    Leave.EndDate = Convert.ToDateTime(endDateToString.ToString(outputFormat));
                    Leave.Duration = model.Duration;
                }
                else
                {
                    var endDateToString = DateTime.ParseExact(model.StartDate, inputFormat, CultureInfo.InvariantCulture);
                    Leave.EndDate = Convert.ToDateTime(endDateToString.ToString(outputFormat));
                    Leave.StartTimeHour = model.InTimeHr;
                    Leave.StartTimeMin = model.InTimeMin;
                    Leave.EndTimeHour = model.EndTimeHr;
                    Leave.EndTimeMin = model.EndTimeMin;
                    Leave.DurationHr = model.DurationHr;
                }
                Leave.Comment = model.Comment;
                Leave.Type = model.Type;
                Leave.Project = model.Project;
                Leave.Customer = model.Customer;
                Leave.CostCode = model.CostCode;
                Leave.Archived = false;
                Leave.CreatedBy = UserId;
                Leave.CreatedDate = DateTime.Now;
                Leave.LastModifiedBy = UserId;
                Leave.LastModifiedDate = DateTime.Now;
                _db.Employee_ProjectPlanner_TravelLeave.Add(Leave);
                _db.SaveChanges();

                foreach (var item in model.DocumentList)
                {
                    Employee_ProjectPlanner_TravelLeave_Documents Document = new Employee_ProjectPlanner_TravelLeave_Documents();
                    Document.TravelLeaveId = Leave.Id;
                    Document.NewName = item.newName;
                    Document.OriginalName = item.originalName;
                    Document.Description = item.description;
                    Document.Archived = false;
                    Document.CreatedBy = UserId;
                    Document.CreatedDate = DateTime.Now;
                    Document.LastModifiedBy = UserId;
                    Document.LastModifiedDate = DateTime.Now;
                    _db.Employee_ProjectPlanner_TravelLeave_Documents.Add(Document);
                    _db.SaveChanges();
                }
            }
        }
        public List<GetResourceDatabyFilterTravel_Result> getResourceDataByFilterTravel(DateTime date)
        {
            return _db.GetResourceDatabyFilterTravel(date).ToList();
        }
        public List<GetResourcePlannerDatabyFilterTravel_Result> getResourcePlannerDataByFilterTravel(DateTime date)
        {
            return _db.GetResourcePlannerDatabyFilterTravel(date).ToList();
        }
        public Employee_ProjectPlanner_TravelLeave getTravelLeaveById(int Id)
        {
            return _db.Employee_ProjectPlanner_TravelLeave.Where(x => x.Id == Id).FirstOrDefault();
        }
        public Employee_ProjectPlanner_TravelLeave getTravelLeaveByEmployeeId(int Id,DateTime stDate)
        {
            return _db.Employee_ProjectPlanner_TravelLeave.Where(x => x.EmployeeId == Id && x.StartDate<= stDate && x.EndDate>=stDate && x.Archived==false).FirstOrDefault();
        }
        public Employee_TravelLeave getPlannerTravelLeaveByEmployee(int Id,DateTime dt)
        {
            return _db.Employee_TravelLeave.Where(x => x.EmployeeId == Id && x.StartDate<=dt && x.EndDate>=dt && x.Archived==false).FirstOrDefault();
        }
        public List<Employee_ProjectPlanner_TravelLeave_Documents> getAllTravelLeaveDocument(int travelLeaveId)
        {
            return _db.Employee_ProjectPlanner_TravelLeave_Documents.Where(x => x.Archived == false && x.TravelLeaveId == travelLeaveId).ToList();
        }
        public List<Employee_TravelLeave_Documents> getPlannerTravelLeaveDocument(int travelLeaveID)
        {
            return _db.Employee_TravelLeave_Documents.Where(x => x.Archived == false && x.TravelLeaveId == travelLeaveID).ToList();
        }
        //Timesheet
        public List<getTotalProjectPlannerTimesheetDuration_Result> getTotalTimeSheetDuration()
        {
            return _db.getTotalProjectPlannerTimesheetDuration().ToList();
        }
        public List<getTotalProjectPlannerUpliftDuration_Result> getTotalUpliftDuration()
        {
            return _db.getTotalProjectPlannerUpliftDuration().ToList();
        }
        public List<getTotalTimesheetDuration_Result> getTotalPlannerTimesheetDuration()
        {
            return _db.getTotalTimesheetDuration().ToList();
        }

        public List<GetResourceDatabyFilterTimesheet_Result> getTimesheetDrillDownData(DateTime date)
        {
            return _db.GetResourceDatabyFilterTimesheet(date).ToList();
        }

        public List<GetResourcePlannerDatabyFilterTimesheet_Result> getTimesheetPlannerDrillDownData(DateTime date)
        {
            return _db.GetResourcePlannerDatabyFilterTimesheet(date).ToList();
        }

        public List<GetResourceDatabyFilterUplift_Result> getUpliftDrillDownData(DateTime date)
        {
            return _db.GetResourceDatabyFilterUplift(date).ToList();
        }
        public WorkpatternWeekend GetAllsickLeaveDayCount()
        {
            WorkpatternWeekend _WorkpatternWeekend = new WorkpatternWeekend();
            _WorkpatternWeekend.SundayDays = 0;
            _WorkpatternWeekend.MondayDays = 0;
            _WorkpatternWeekend.TuesdayDays = 0;
            _WorkpatternWeekend.WednessdayDays = 0;
            _WorkpatternWeekend.ThursdayDays = 0;
            _WorkpatternWeekend.FridayDays = 0;
            _WorkpatternWeekend.SaturdayDays = 0;

               List<Employee_SickLeaves> SickLeave = _db.Employee_SickLeaves.Where(x => x.Archived==false).ToList();
                foreach (var item in SickLeave)
                {
                    string DayofWeek = string.Empty;
                    int count = 0;
                    //item.da
                    if (item.EndDate != null)
                    {
                        for (int i = 1; i <= item.DurationDays; i++)
                        {
                            if (item.StartDate != null)
                            {
                                DateTime startDate;
                                //if (count == 0)
                                //{
                                startDate = item.StartDate.Value.Date.AddDays(i - 1);
                                //}
                                //else
                                //{
                                //    startDate = item.StartDate.Value.Date.AddDays(i);
                                //}
                                count++;
                                DayofWeek = startDate.DayOfWeek.ToString();

                                switch (DayofWeek)
                                {
                                    case "Monday":
                                        _WorkpatternWeekend.MondayDays += Convert.ToDecimal(1);
                                        break;
                                    case "Tuesday":
                                        _WorkpatternWeekend.TuesdayDays += Convert.ToDecimal(1);
                                        break;
                                    case "Wednesday":
                                        _WorkpatternWeekend.WednessdayDays += Convert.ToDecimal(1);
                                        break;
                                    case "Thursday":
                                        _WorkpatternWeekend.ThursdayDays += Convert.ToDecimal(1);
                                        break;
                                    case "Friday":
                                        _WorkpatternWeekend.FridayDays += Convert.ToDecimal(1);
                                        break;
                                    case "Saturday":
                                        _WorkpatternWeekend.SaturdayDays += Convert.ToDecimal(1);
                                        break;
                                    case "Sunday":
                                        _WorkpatternWeekend.SundayDays += Convert.ToDecimal(1);
                                        break;
                                }

                            }
                        }
                    }
                    else
                    {
                        DayofWeek = item.StartDate.Value.DayOfWeek.ToString();
                        switch (DayofWeek)
                        {
                            case "Monday":
                                _WorkpatternWeekend.MondayDays += Convert.ToDecimal(item.DurationHours);
                                break;
                            case "Tuesday":
                                _WorkpatternWeekend.TuesdayDays += Convert.ToDecimal(item.DurationHours);
                                break;
                            case "Wednesday":
                                _WorkpatternWeekend.WednessdayDays += Convert.ToDecimal(item.DurationHours);
                                break;
                            case "Thursday":
                                _WorkpatternWeekend.ThursdayDays += Convert.ToDecimal(item.DurationHours);
                                break;
                            case "Friday":
                                _WorkpatternWeekend.FridayDays += Convert.ToDecimal(item.DurationHours);
                                break;
                            case "Saturday":
                                _WorkpatternWeekend.SaturdayDays += Convert.ToDecimal(item.DurationHours);
                                break;
                            case "Sunday":
                                _WorkpatternWeekend.SundayDays += Convert.ToDecimal(item.DurationHours);
                                break;
                        }
                    }              
            }
            return _WorkpatternWeekend;
        }
        public decimal GetAllSickLeave()
        {
            var details = _db.Employee_SickLeaves.Where(x=>x.Archived == false).ToList();
            return details.Count;
        }
        public List<PublicHoliday> GetAllAdminPlannerHolidayListById(DateTime date)
        {
            return _db.PublicHolidays.Where(x => x.Date == date).ToList();
        }
    }
}