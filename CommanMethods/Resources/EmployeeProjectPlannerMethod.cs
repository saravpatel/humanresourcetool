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
    public class EmployeeProjectPlannerMethod
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        private string inputFormat = "dd-MM-yyyy";
        private string outputFormat = "yyyy-MM-dd HH:mm:ss";
        HolidayNAbsenceMethod _holidayNAbsenceMethod = new HolidayNAbsenceMethod();

        #endregion

        #region TimeSheet

        public List<Employee_ProjectPlanner_TimeSheet> getAllTimeSheet()
        {
            return _db.Employee_ProjectPlanner_TimeSheet.Where(x => x.Archived == false).ToList();
        }

        public Employee_ProjectPlanner_TimeSheet getTimeSheetById(int Id)
        {
            return _db.Employee_ProjectPlanner_TimeSheet.Where(x => x.Id == Id).FirstOrDefault();
        }
        public Employee_ProjectPlanner_TimeSheet getTimeSheetByEmployeeId(int Id,DateTime stdate)
        {
            return _db.Employee_ProjectPlanner_TimeSheet.Where(x => x.EmployeeId == Id && x.Date==stdate && x.Archived==false).FirstOrDefault();
        }

        public List<Employee_ProjectPlanner_TimeSheet_Documents> getAllTimeSheetDocument(int timeSheetId)
        {
            return _db.Employee_ProjectPlanner_TimeSheet_Documents.Where(x => x.Archived == false && x.TimeSheetId == timeSheetId).ToList();
        }

        public List<Employee_ProjectPlanner_TimeSheet_Detail> getAllTimeSheetDetail(int timeSheetId)
        {
            return _db.Employee_ProjectPlanner_TimeSheet_Detail.Where(x => x.Archived == false && x.TimeSheetId == timeSheetId).ToList();
        }
        
        
        public void TimeSheet_SaveData(EmployeeProjectPlanner_TimeSheetViewModel model, int UserId)
        {
            if (model.Id > 0)
            {
                Employee_ProjectPlanner_TimeSheet timesheet = _db.Employee_ProjectPlanner_TimeSheet.Where(x => x.Id == model.Id).FirstOrDefault();
                timesheet.EmployeeId = model.EmployeeId;
                var DateToString = DateTime.ParseExact(model.Date, inputFormat, CultureInfo.InvariantCulture);
                timesheet.Date = Convert.ToDateTime(DateToString.ToString(outputFormat));
                timesheet.Comments = model.Comment;
                timesheet.LastModifiedBy = UserId;
                timesheet.LastModifiedDate = DateTime.Now;
                _db.SaveChanges();

                foreach (var item in _db.Employee_ProjectPlanner_TimeSheet_Detail.Where(x => x.TimeSheetId == timesheet.Id).ToList())
                {
                    _db.Employee_ProjectPlanner_TimeSheet_Detail.Remove(item);
                    _db.SaveChanges();
                }

                foreach (var item in model.DetailList)
                {
                    Employee_ProjectPlanner_TimeSheet_Detail Detail = new Employee_ProjectPlanner_TimeSheet_Detail();
                    Detail.TimeSheetId = timesheet.Id;
                    Detail.InTimeHr = (int)item.InTimeHr;
                    Detail.InTimeMin = item.InTimeMin;
                    Detail.EndTimeHr = item.EndTimeHr;
                    Detail.EndTimeMin = item.EndTimeMin;
                    Detail.Project = item.Project;
                    Detail.CostCode = item.CostCode;
                    Detail.Customer = item.Customer;
                    Detail.Asset = item.Asset;
                    Detail.Archived = false;
                    Detail.CreatedBy = UserId;
                    Detail.CreatedDate = DateTime.Now;
                    Detail.LastModifiedBy = UserId;
                    Detail.LastModifiedDate = DateTime.Now;
                    _db.Employee_ProjectPlanner_TimeSheet_Detail.Add(Detail);
                    _db.SaveChanges();
                }

                foreach (var item in _db.Employee_ProjectPlanner_TimeSheet_Documents.Where(x => x.TimeSheetId == timesheet.Id).ToList())
                {
                    _db.Employee_ProjectPlanner_TimeSheet_Documents.Remove(item);
                    _db.SaveChanges();
                }

                foreach (var item in model.DocumentList)
                {
                    Employee_ProjectPlanner_TimeSheet_Documents Document = new Employee_ProjectPlanner_TimeSheet_Documents();
                    Document.TimeSheetId = timesheet.Id;
                    Document.NewName = item.newName;
                    Document.OriginalName = item.originalName;
                    Document.Description = item.description;
                    Document.Archived = false;
                    Document.CreatedBy = UserId;
                    Document.CreatedDate = DateTime.Now;
                    Document.LastModifiedBy = UserId;
                    Document.LastModifiedDate = DateTime.Now;
                    _db.Employee_ProjectPlanner_TimeSheet_Documents.Add(Document);
                    _db.SaveChanges();
                }
            }
            else
            {
                Employee_ProjectPlanner_TimeSheet timesheet = new Employee_ProjectPlanner_TimeSheet();
                timesheet.EmployeeId = model.EmployeeId;
                var DateToString = DateTime.ParseExact(model.Date, inputFormat, CultureInfo.InvariantCulture);
                timesheet.Date = Convert.ToDateTime(DateToString.ToString(outputFormat));
                timesheet.Comments = model.Comment;
                timesheet.Archived = false;
                timesheet.CreatedBy = UserId;
                timesheet.CreatedDate = DateTime.Now;
                timesheet.LastModifiedBy = UserId;
                timesheet.LastModifiedDate = DateTime.Now;
                _db.Employee_ProjectPlanner_TimeSheet.Add(timesheet);
                _db.SaveChanges();

                foreach (var item in model.DetailList)
                {
                    Employee_ProjectPlanner_TimeSheet_Detail Detail = new Employee_ProjectPlanner_TimeSheet_Detail();
                    Detail.TimeSheetId = timesheet.Id;
                    Detail.InTimeHr = item.InTimeHr;
                    Detail.InTimeMin = item.InTimeMin;
                    Detail.EndTimeHr = item.EndTimeHr;
                    Detail.EndTimeMin = item.EndTimeMin;
                    Detail.Project = item.Project;
                    Detail.CostCode = item.CostCode;
                    Detail.Customer = item.Customer;
                    Detail.Asset = item.Asset;
                    Detail.Archived = false;
                    Detail.CreatedBy = UserId;
                    Detail.CreatedDate = DateTime.Now;
                    Detail.LastModifiedBy = UserId;
                    Detail.LastModifiedDate = DateTime.Now;
                    Detail.ApprovalStatus = "Pending";
                    _db.Employee_ProjectPlanner_TimeSheet_Detail.Add(Detail);
                    _db.SaveChanges();
                }

                foreach (var item in model.DocumentList)
                {
                    Employee_ProjectPlanner_TimeSheet_Documents Document = new Employee_ProjectPlanner_TimeSheet_Documents();
                    Document.TimeSheetId = timesheet.Id;
                    Document.NewName = item.newName;
                    Document.OriginalName = item.originalName;
                    Document.Description = item.description;
                    Document.Archived = false;
                    Document.CreatedBy = UserId;
                    Document.CreatedDate = DateTime.Now;
                    Document.LastModifiedBy = UserId;
                    Document.LastModifiedDate = DateTime.Now;
                    _db.Employee_ProjectPlanner_TimeSheet_Documents.Add(Document);
                    _db.SaveChanges();
                }
            }
        }

        #endregion

        #region TravelLeave

        public List<Employee_ProjectPlanner_TravelLeave> getAllTravelLeave()
        {
            return _db.Employee_ProjectPlanner_TravelLeave.Where(x => x.Archived == false).ToList();
        }

        public Employee_ProjectPlanner_TravelLeave getTravelLeaveById(int Id)
        {
            return _db.Employee_ProjectPlanner_TravelLeave.Where(x => x.Id == Id).FirstOrDefault();
        }

        public List<Employee_ProjectPlanner_TravelLeave_Documents> getAllTravelLeaveDocument(int travelLeaveId)
        {
            return _db.Employee_ProjectPlanner_TravelLeave_Documents.Where(x => x.Archived == false && x.TravelLeaveId == travelLeaveId).ToList();
        }

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
                    Leave.StartTimeMin= 0;
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

        #endregion

        #region Scheduling Leaves

        public List<Employee_ProjectPlanner_Scheduling> getAllSchedulingLeves()
        {
            return _db.Employee_ProjectPlanner_Scheduling.Where(x => x.Archived == false).ToList();
        }

        public Employee_ProjectPlanner_Scheduling getSchedulingLeaveById(int Id)
        {
            return _db.Employee_ProjectPlanner_Scheduling.Where(x => x.Id == Id).FirstOrDefault();
        }

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
                    Leave.DurationHr =0;
                    Leave.InTimeHr = 0;
                    Leave.InTimeMin =0;
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
                Leave.IsRead = false;
                Leave.IsReadAddRes = false;
                Leave.IsReadHR = false;
                Leave.LastModifiedDate = DateTime.Now;
                _db.Employee_ProjectPlanner_Scheduling.Add(Leave);
                _db.SaveChanges();

            }
            return true;
        }

        #endregion

        #region Uplift

        public List<Employee_ProjectPlanner_Uplift> getAllUplift()
        {
            return _db.Employee_ProjectPlanner_Uplift.Where(x => x.Archived == false).ToList();
        }

        public Employee_ProjectPlanner_Uplift getUpliftById(int Id)
        {
            return _db.Employee_ProjectPlanner_Uplift.Where(x => x.Id == Id).FirstOrDefault();
        }
        public Employee_ProjectPlanner_Uplift getUpliftByEmployeeId(int Id,DateTime stDate)
        {
            return _db.Employee_ProjectPlanner_Uplift.Where(x => x.EmployeeId == Id && x.Date==stDate && x.Archived==false).FirstOrDefault();
        }
        public List<Employee_ProjectPlanner_Uplift_Documents> getAllUpliftDocument(int UpliftId)
        {
            //Commented for yagnik
            return _db.Employee_ProjectPlanner_Uplift_Documents.Where(x => x.Archived == false && x.UpliftId == UpliftId).ToList();
        }

        public List<Employee_ProjectPlanner_Uplift_Detail> getAllUpliftDetail(int UpliftId)
        {
            return _db.Employee_ProjectPlanner_Uplift_Detail.Where(x => x.Archived == false && x.UpliftId == UpliftId).ToList();
        }

        public void Uplift_SaveData(EmployeeProjectPlanner_UpliftViewModel model, int UserId)
        {
            if (model.Id > 0)
            {
                Employee_ProjectPlanner_Uplift uplift = getUpliftById(model.Id); //_db.Employee_ProjectPlanner_Uplift.Where(x => x.Id == model.Id).FirstOrDefault();
                uplift.EmployeeId = model.EmployeeId;
                var DateToString = DateTime.ParseExact(model.Date, inputFormat, CultureInfo.InvariantCulture);
                uplift.Date = Convert.ToDateTime(DateToString.ToString(outputFormat));

                uplift.UpliftPostionId = model.UpliftPostionId;
               
                uplift.ProjectId = model.ProjectId;
                uplift.CustomerId = model.CustomerId;
                

                uplift.Comments = model.Comment;

                uplift.LastModifiedBy = UserId;
                uplift.LastModifiedDate = DateTime.Now;
                _db.SaveChanges();

                foreach (var item in _db.Employee_ProjectPlanner_Uplift_Detail.Where(x => x.UpliftId == uplift.Id).ToList())
                {
                    _db.Employee_ProjectPlanner_Uplift_Detail.Remove(item);
                    _db.SaveChanges();
                }

                foreach (var item in model.DetailList)
                {
                    Employee_ProjectPlanner_Uplift_Detail Detail = new Employee_ProjectPlanner_Uplift_Detail();
                    Detail.UpliftId = uplift.Id;
                    Detail.InTimeHr = (int)item.InTimeHr;
                    Detail.InTimeMin = item.InTimeMin;
                    Detail.OutTimeHr = item.EndTimeHr;
                    Detail.OutTimeMin = item.EndTimeMin;
                    Detail.WorkerRate = item.WorkerRate;
                    Detail.WorkerRatePer = item.WorkerRatePer;
                    Detail.CustomerRate = item.CustomerRate;
                    Detail.CustomerRatePer = item.CustomerRatePer;
                    Detail.Archived = false;
                    Detail.CreatedDate = DateTime.Now;
                    _db.Employee_ProjectPlanner_Uplift_Detail.Add(Detail);
                    _db.SaveChanges();
                }

                foreach (var item in _db.Employee_ProjectPlanner_Uplift_Documents.Where(x => x.UpliftId == uplift.Id).ToList())
                {
                    _db.Employee_ProjectPlanner_Uplift_Documents.Remove(item);
                    _db.SaveChanges();
                }

                foreach (var item in model.DocumentList)
                {
                    Employee_ProjectPlanner_Uplift_Documents Document = new Employee_ProjectPlanner_Uplift_Documents();
                    Document.UpliftId = uplift.Id;
                    Document.NewName = item.newName;
                    Document.OriginalName = item.originalName;
                    Document.Description = item.description;
                    Document.Archived = false;
                    Document.CreatedBy = UserId;
                    Document.CreatedDate = DateTime.Now;
                    Document.LastModifiedBy = UserId;
                    Document.LastModifiedDate = DateTime.Now;
                    _db.Employee_ProjectPlanner_Uplift_Documents.Add(Document);
                    _db.SaveChanges();
                }
            }
            else
            {
                Employee_ProjectPlanner_Uplift uplift = new Employee_ProjectPlanner_Uplift();
                uplift.EmployeeId = model.EmployeeId;
                var DateToString = DateTime.ParseExact(model.Date, inputFormat, CultureInfo.InvariantCulture);
                uplift.Date = Convert.ToDateTime(DateToString.ToString(outputFormat));
                uplift.Comments = model.Comment;
                uplift.UpliftPostionId = model.UpliftPostionId;
               
                uplift.ProjectId = model.ProjectId;
                uplift.CustomerId = model.CustomerId;
              
                uplift.Archived = false;
                uplift.CreatedBy = UserId;
                uplift.CreatedDate = DateTime.Now;
                uplift.LastModifiedBy = UserId;
                uplift.LastModifiedDate = DateTime.Now;
                _db.Employee_ProjectPlanner_Uplift.Add(uplift);
                _db.SaveChanges();

                foreach (var item in model.DetailList)
                {
                    Employee_ProjectPlanner_Uplift_Detail Detail = new Employee_ProjectPlanner_Uplift_Detail();
                    Detail.UpliftId = uplift.Id;
                    Detail.InTimeHr = item.InTimeHr;
                    Detail.InTimeMin = item.InTimeMin;
                    Detail.OutTimeHr = item.EndTimeHr;
                    Detail.OutTimeMin = item.EndTimeMin;
                    Detail.WorkerRate = item.WorkerRate;
                    Detail.WorkerRatePer = item.WorkerRatePer;
                    Detail.CustomerRate = item.CustomerRate;
                    Detail.CustomerRatePer = item.CustomerRatePer;
                    Detail.Archived = false;
                    Detail.ApprovalStatus = "Pending";
                    Detail.IsRead = false;
                    Detail.IsReadAddRep = false;
                    Detail.IsReadHR = false;
                    Detail.CreatedDate = DateTime.Now;
                    _db.Employee_ProjectPlanner_Uplift_Detail.Add(Detail);
                    _db.SaveChanges();
                }

                foreach (var item in model.DocumentList)
                {
                    Employee_ProjectPlanner_Uplift_Documents Document = new Employee_ProjectPlanner_Uplift_Documents();
                    Document.UpliftId = uplift.Id;
                    Document.NewName = item.newName;
                    Document.OriginalName = item.originalName;
                    Document.Description = item.description;
                    Document.Archived = false;
                    Document.CreatedBy = UserId;
                    Document.CreatedDate = DateTime.Now;
                    Document.LastModifiedBy = UserId;
                    Document.LastModifiedDate = DateTime.Now;
                    _db.Employee_ProjectPlanner_Uplift_Documents.Add(Document);
                    _db.SaveChanges();
                }
            }
        }

        #endregion

        #region Graph Details

        public decimal GetProjectbookDaysByEmployeeId(int EmployeeId)
        {
           // var details = _db.Eplo.Where(x => x.Id == EmployeeId).FirstOrDefault();
            var yearId = DateTime.Now.Year;
            DateTime St = new DateTime(yearId, 1, 1);
            DateTime ed = new DateTime(yearId, 12, 31);
           // decimal Count = (decimal)(details.StartDate >= St && details.StartDate <= ed == true ? (details.Thisyear == null ? 0 : details.Thisyear) : (details.Nextyear == null ? 0 : details.Nextyear));

            return 0;

        }
        
        public decimal GetProjectRemainingDaysByEmployeeId(int EmployeeId)
        {
            //var details = _db.Employee_SickLeaves.Where(x => x.EmployeeId == EmployeeId && x.Archived == false).ToList();
            var yearId = DateTime.Now.Year;
            DateTime St = new DateTime(yearId, 1, 1);
            DateTime ed = new DateTime(yearId, 12, 31);
            TimeSpan span = (ed -St);
            return span.Days;//details.Count;
        }
        public decimal GetTimeSheetbookDaysByEmployeeId(int EmployeeId)
        {
            var yearId = DateTime.Now.Year;
            DateTime St = new DateTime(yearId, 1, 1);
            DateTime ed = new DateTime(yearId, 12, 31);
            var details = _db.Employee_ProjectPlanner_TimeSheet.Where(x => x.EmployeeId == EmployeeId && x.Date >= St && x.Date <= ed).ToList();

            return (decimal)details.Count;//details.Count;
        }
        public decimal GetTimeSheetRemainingDaysByEmployeeId(int EmployeeId)
        {
            var yearId = DateTime.Now.Year;
            DateTime St = new DateTime(yearId, 1, 1);
            DateTime ed = new DateTime(yearId, 12, 31);
            TimeSpan span = (ed - St);
            return span.Days;
        }

        public decimal GetTravelDaysByEmployeeId(int EmployeeId)
        {
            var yearId = DateTime.Now.Year;
            DateTime St = new DateTime(yearId, 1, 1);
            DateTime ed = new DateTime(yearId, 12, 31);
            var details = _db.Employee_ProjectPlanner_TravelLeave.Where(x => x.EmployeeId == EmployeeId && x.StartDate >= St && x.StartDate <= ed ).ToList();
            List<int> add = new List<int>();
            foreach (var item in details)
            {
                add.Add((int)(item.Duration == null ? 0 : item.Duration));
            }
            return (decimal)add.Sum();
        }
        public decimal GetTravelWorkingDaysByEmployeeId(int EmployeeId)
        {
            var yearId = DateTime.Now.Year;
            DateTime St = new DateTime(yearId, 1, 1);
            DateTime ed = new DateTime(yearId, 12, 31);
            TimeSpan span = (ed - St);
            return span.Days;
        }
        public decimal GetUpliftbookDaysByEmployeeId(int EmployeeId)
        {
            var yearId = DateTime.Now.Year;
            DateTime St = new DateTime(yearId, 1, 1);
            DateTime ed = new DateTime(yearId, 12, 31);
            var details = _db.Employee_ProjectPlanner_Uplift.Where(x => x.EmployeeId == EmployeeId && x.Date >= St && x.Date <= ed).ToList();
            return (decimal)details.Count;
        }
        

        #endregion
    }
}