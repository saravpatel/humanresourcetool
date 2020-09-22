using HRTool.DataModel;
using HRTool.Models.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRTool.CommanMethods.Settings;

namespace HRTool.CommanMethods.Resources
{
    public class EmployeePlannerMethod
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        private string inputFormat = "dd-MM-yyyy";
        private string outputFormat = "yyyy-MM-dd HH:mm:ss";
        HolidayNAbsenceMethod _holidayNAbsenceMethod = new HolidayNAbsenceMethod();
        #endregion

        #region LateLeave

        public decimal getAllLateLeavesMonthWiseCount(int EmployeeId, DateTime Startdate, DateTime Enddate)
        {
            var data = _db.Employee_LateLeave.Where(x => x.EmployeeId == EmployeeId && x.LateDate >= Startdate && x.LateDate <= Enddate).ToList();
            if (data.Count > 0)
            {
                return data.Count;
            }
            else
            {
                return 0;
            }

        }

        public List<Employee_LateLeave> getAllLateLeave()
        {
            return _db.Employee_LateLeave.Where(x => x.Archived == false).ToList();
        }

        public Employee_LateLeave getLateLeaveById(int Id)
        {
            return _db.Employee_LateLeave.Where(x => x.Id == Id).FirstOrDefault();
        }

        public void UpdateAnnualSettings(AspNetUser data)
        {
            AspNetUser model = _db.AspNetUsers.Where(x => x.Id == data.Id && x.Archived==false).FirstOrDefault(); // _db.AspNetUsers.Where(x => x.Id == data.Id).FirstOrDefault();
            if (data.AuthorisorEmployeeID != "" && data.AuthorisorEmployeeID != null)
            {
                model.AuthorisorEmployeeID = data.AuthorisorEmployeeID;
            }
            model.HolidayYear = data.HolidayYear;
            model.MeasuredIn = data.MeasuredIn;
            model.Thisyear = data.Thisyear;
            model.Nextyear = data.Nextyear;
            model.EntitlementIncludesPublicHoliday = data.EntitlementIncludesPublicHoliday;
            model.AutoApproveHolidays = data.AutoApproveHolidays;
            model.ExceedAllowance = data.ExceedAllowance;
            _db.SaveChanges();
        }

        public void LateLeave_SaveData(EmployeePlanner_LateLeaveViewModel model, int UserId)
        {
            if (model.Id > 0)
            {
                Employee_LateLeave Leave = _db.Employee_LateLeave.Where(x => x.Id == model.Id).FirstOrDefault();
                Leave.EmployeeId = model.EmployeeId;
                var LateDateToString = DateTime.ParseExact(model.LateDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.LateDate = Convert.ToDateTime(LateDateToString.ToString(outputFormat));
                Leave.Comments = model.Comment;
                Leave.LateHr = model.LateHr;
                Leave.LateMin = model.LateMin;
                Leave.LastModifiedBy = UserId;
                Leave.lastModifiedDate = DateTime.Now;
                _db.SaveChanges();
            }
            else
            {
                Employee_LateLeave Leave = new Employee_LateLeave();
                Leave.EmployeeId = model.EmployeeId;
                var LateDateToString = DateTime.ParseExact(model.LateDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.LateDate = Convert.ToDateTime(LateDateToString.ToString(outputFormat));
                Leave.Comments = model.Comment;
                Leave.LateHr = model.LateHr;
                Leave.LateMin = model.LateMin;
                Leave.Archived = false;
                Leave.CreatedBy = UserId;
                Leave.CreatedDate = DateTime.Now;
                Leave.LastModifiedBy = UserId;
                Leave.lastModifiedDate = DateTime.Now;
                _db.Employee_LateLeave.Add(Leave);
                _db.SaveChanges();
            }
        }

        #endregion

        #region AnnualLeave

        public decimal GetAllAnnualLeavesMonthWiseCount(int EmployeeId, DateTime Startdate, DateTime Enddate)
        {
            var data = _db.Employee_AnualLeave.Where(x => x.EmployeeId == EmployeeId && x.StartDate >= Startdate && x.StartDate <= Enddate).ToList();
            if (data.Count > 0)
            {
                return data.Count;
            }
            else
            {
                return 0;
            }

        }
        public List<Employee_AnualLeave> getAllAnnualLeave()
        {
            return _db.Employee_AnualLeave.Where(x => x.Archived == false).ToList();
        }

        public Employee_AnualLeave getAnnualLeaveById(int Id)
        {
            return _db.Employee_AnualLeave.Where(x => x.Id == Id).FirstOrDefault();
        }

        public void AnnualLeave_SaveData(EmployeePlanner_AnnualLeaveViewModel model, int UserId)
        {
            if (model.Id > 0)
            {
                Employee_AnualLeave annualLeave = _db.Employee_AnualLeave.Where(x => x.Id == model.Id).FirstOrDefault();
                annualLeave.EmployeeId = model.EmployeeId;
                annualLeave.IsLessThenADay = model.IsLessThenADay;
                annualLeave.Duration = model.Duration;
                annualLeave.TOIL = model.TOIL;
                var StartDateToString = DateTime.ParseExact(model.StartDate, inputFormat, CultureInfo.InvariantCulture);
                annualLeave.StartDate = Convert.ToDateTime(StartDateToString.ToString(outputFormat));
                if (model.IsLessThenADay == false)
                {
                    var endDateToString = DateTime.ParseExact(model.EndDate, inputFormat, CultureInfo.InvariantCulture);
                    annualLeave.EndDate = Convert.ToDateTime(endDateToString.ToString(outputFormat));
                    annualLeave.Comment = model.Comment;
                }
                else
                {
                    var endDateToString = DateTime.ParseExact(model.StartDate, inputFormat, CultureInfo.InvariantCulture);
                    annualLeave.EndDate = Convert.ToDateTime(endDateToString.ToString(outputFormat));
                    annualLeave.Comment = "";
                }
                annualLeave.LastModifiedBy = UserId;
                annualLeave.LastModifiedDate = DateTime.Now;
                _db.SaveChanges();
            }
            else
            {
                Employee_AnualLeave annualLeave = new Employee_AnualLeave();
                annualLeave.EmployeeId = model.EmployeeId;
                annualLeave.IsLessThenADay = model.IsLessThenADay;
                annualLeave.Duration = model.Duration;
                annualLeave.TOIL = model.TOIL;
                var StartDateToString = DateTime.ParseExact(model.StartDate, inputFormat, CultureInfo.InvariantCulture);
                annualLeave.StartDate = Convert.ToDateTime(StartDateToString.ToString(outputFormat));
                if (model.IsLessThenADay == false)
                {
                    var endDateToString = DateTime.ParseExact(model.EndDate, inputFormat, CultureInfo.InvariantCulture);
                    annualLeave.EndDate = Convert.ToDateTime(endDateToString.ToString(outputFormat));
                    annualLeave.Comment = model.Comment;
                }
                else
                {
                    var endDateToString = DateTime.ParseExact(model.StartDate, inputFormat, CultureInfo.InvariantCulture);
                    annualLeave.EndDate = Convert.ToDateTime(endDateToString.ToString(outputFormat));
                }
                annualLeave.Archived = false;
                annualLeave.ApprovalStatus = "Pending";
                annualLeave.IsRead = false;
                annualLeave.IsReadAddRep = false;
                annualLeave.IsReadHR = false;
                annualLeave.CreatedBy = UserId;
                annualLeave.CreatedDate = DateTime.Now;
                annualLeave.LastModifiedBy = UserId;
                annualLeave.LastModifiedDate = DateTime.Now;
                _db.Employee_AnualLeave.Add(annualLeave);
                _db.SaveChanges();
            }
        }

        #endregion

        #region OtherLeave
        public decimal GetAllOtherLeavesMonthWiseCount(int EmployeeId, DateTime Startdate, DateTime Enddate)
        {
            var data = _db.Employee_OtherLeave.Where(x => x.EmployeeId == EmployeeId && x.StartDate >= Startdate && x.StartDate <= Enddate).ToList();
            if (data.Count > 0)
            {
                return data.Count;
            }
            else
            {
                return 0;
            }

        }

        public List<Employee_OtherLeave> getAllOtherLeave()
        {
            return _db.Employee_OtherLeave.Where(x => x.Archived == false).ToList();
        }

        public Employee_OtherLeave getOtherLeaveById(int Id)
        {
            return _db.Employee_OtherLeave.Where(x => x.Id == Id).FirstOrDefault();
        }

        public List<Employee_OtherLeave> getOtherLeaveByEmployeeId(int Id, string st, string ed)
        {
            var stDateToString = DateTime.ParseExact(st, inputFormat, CultureInfo.InvariantCulture);

            DateTime Startdate = Convert.ToDateTime(stDateToString.ToString(outputFormat));

            var edDateToString = DateTime.ParseExact(ed, inputFormat, CultureInfo.InvariantCulture);

            DateTime Enddate = Convert.ToDateTime(edDateToString.ToString(outputFormat));


            return _db.Employee_OtherLeave.Where(x => x.EmployeeId == Id && x.StartDate >= Startdate && x.EndDate <= Enddate && x.Archived == false).ToList();
        }
        public void OtherLeave_SaveData(EmployeePlanner_OtherLeaveViewModel model, int UserId)
        {
            string duration = "";

            if (model.Id > 0)
            {
                Employee_OtherLeave Leave = _db.Employee_OtherLeave.Where(x => x.Id == model.Id).FirstOrDefault();
                Leave.EmployeeId = model.EmployeeId;
                Leave.ReasonForLeaveId = model.ReasonForLeaveId;
                Leave.IsLessThenADay = model.IsLessThenADay;
                var StartDateToString = DateTime.ParseExact(model.StartDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.StartDate = Convert.ToDateTime(StartDateToString.ToString(outputFormat));
                if (model.IsLessThenADay == false)
                {
                    var endDateToString = DateTime.ParseExact(model.EndDate, inputFormat, CultureInfo.InvariantCulture);
                    Leave.EndDate = Convert.ToDateTime(endDateToString.ToString(outputFormat));
                    Leave.Comment = model.Comment;
                    Leave.Duration = model.Duration;
                    Leave.Hour = 0;
                    Leave.Min = 0;
                }
                else
                {
                    var endDateToString = DateTime.ParseExact(model.StartDate, inputFormat, CultureInfo.InvariantCulture);
                    Leave.EndDate = Convert.ToDateTime(endDateToString.ToString(outputFormat));
                    Leave.Comment = model.Comment;
                    Leave.Hour = model.Hour;
                    Leave.Min = model.Min;
                    duration = Convert.ToString(model.Hour) + "." + Convert.ToString(model.Min);
                    Leave.Duration = Convert.ToDecimal(duration);

                }
                Leave.LastModifiedBy = UserId;
                Leave.LastModifiedDate = DateTime.Now;
                _db.SaveChanges();

                foreach (var item in _db.Employee_OtherLeave_Documents.Where(x => x.OtherLeaveId == Leave.Id).ToList())
                {
                    _db.Employee_OtherLeave_Documents.Remove(item);
                    _db.SaveChanges();
                }

                foreach (var item in model.DocumentList)
                {
                    Employee_OtherLeave_Documents Document = new Employee_OtherLeave_Documents();
                    Document.OtherLeaveId = Leave.Id;
                    Document.NewName = item.newName;
                    Document.OriginalName = item.originalName;
                    Document.Description = item.description;
                    Document.Archived = false;
                    Document.CreatedBy = UserId;
                    Document.CreatedDate = DateTime.Now;
                    Document.LastModifiedBy = UserId;
                    Document.LastModifiedDate = DateTime.Now;
                    _db.Employee_OtherLeave_Documents.Add(Document);
                    _db.SaveChanges();
                }
            }
            else
            {
                Employee_OtherLeave Leave = new Employee_OtherLeave();
                Leave.EmployeeId = model.EmployeeId;
                Leave.ReasonForLeaveId = model.ReasonForLeaveId;
                Leave.IsLessThenADay = model.IsLessThenADay;

                var StartDateToString = DateTime.ParseExact(model.StartDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.StartDate = Convert.ToDateTime(StartDateToString.ToString(outputFormat));
                if (model.IsLessThenADay == false)
                {
                    var endDateToString = DateTime.ParseExact(model.EndDate, inputFormat, CultureInfo.InvariantCulture);
                    Leave.EndDate = Convert.ToDateTime(endDateToString.ToString(outputFormat));
                    Leave.Comment = model.Comment;
                    Leave.Duration = model.Duration;
                }
                else
                {
                    var endDateToString = DateTime.ParseExact(model.StartDate, inputFormat, CultureInfo.InvariantCulture);
                    Leave.EndDate = Convert.ToDateTime(endDateToString.ToString(outputFormat));
                    Leave.Hour = model.Hour;
                    Leave.Min = model.Min;
                    duration = Convert.ToString(model.Hour) + "." + Convert.ToString(model.Min);
                    Leave.Duration = Convert.ToDecimal(duration);
                    Leave.Comment = model.Comment;

                }
                Leave.Archived = false;
                Leave.ApprovalStatus = "Pending";
                Leave.IsRead = false;
                Leave.IsReadAddRep = false;
                Leave.IsReadHR = false;
                Leave.CreatedBy = UserId;
                Leave.CreatedDate = DateTime.Now;
                Leave.LastModifiedBy = UserId;
                Leave.LastModifiedDate = DateTime.Now;
                _db.Employee_OtherLeave.Add(Leave);
                _db.SaveChanges();

                foreach (var item in model.DocumentList)
                {
                    Employee_OtherLeave_Documents Document = new Employee_OtherLeave_Documents();
                    Document.OtherLeaveId = Leave.Id;
                    Document.NewName = item.newName;
                    Document.OriginalName = item.originalName;
                    Document.Description = item.description;
                    Document.Archived = false;
                    Document.CreatedBy = UserId;
                    Document.CreatedDate = DateTime.Now;
                    Document.LastModifiedBy = UserId;
                    Document.LastModifiedDate = DateTime.Now;
                    _db.Employee_OtherLeave_Documents.Add(Document);
                    _db.SaveChanges();
                }
            }
        }

        public List<Employee_OtherLeave_Documents> getAllOtherLeaveDocument(int otherLeaveId)
        {
            return _db.Employee_OtherLeave_Documents.Where(x => x.Archived == false && x.OtherLeaveId == otherLeaveId).ToList();
        }

        #endregion

        #region TravelLeave

        public decimal GetAllTravelLeavesMonthWiseCount(int EmployeeId, DateTime Startdate, DateTime Enddate)
        {
            var data = _db.Employee_TravelLeave.Where(x => x.EmployeeId == EmployeeId && x.StartDate >= Startdate && x.StartDate <= Enddate).ToList();
            if (data.Count > 0)
            {
                return data.Count;
            }
            else
            {
                return 0;
            }

        }
        public decimal GetAllProjrctPlannerTravelLeavesMonthWiseCount(int EmployeeId, DateTime Startdate, DateTime Enddate)
        {
            var data = _db.Employee_ProjectPlanner_TravelLeave.Where(x => x.EmployeeId == EmployeeId && x.StartDate >= Startdate && x.StartDate <= Enddate).ToList();
            if (data.Count > 0)
            {
                return data.Count;
            }
            else
            {
                return 0;
            }

        }

        public List<Employee_TravelLeave> getAllTravelLeave()
        {
            return _db.Employee_TravelLeave.Where(x => x.Archived == false).ToList();
        }

        public Employee_TravelLeave getTravelLeaveById(int Id)
        {
            return _db.Employee_TravelLeave.Where(x => x.Id == Id).FirstOrDefault();
        }

        public List<Employee_TravelLeave_Documents> getAllTravelLeaveDocument(int travelLeaveId)
        {
            return _db.Employee_TravelLeave_Documents.Where(x => x.Archived == false && x.TravelLeaveId == travelLeaveId).ToList();
        }

        public void TravelLeave_SaveData(EmployeePlanner_TravelLeaveViewModel model, int UserId)
        {
            if (model.Id > 0)
            {
                Employee_TravelLeave Leave = _db.Employee_TravelLeave.Where(x => x.Id == model.Id).FirstOrDefault();
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

                foreach (var item in _db.Employee_TravelLeave_Documents.Where(x => x.TravelLeaveId == Leave.Id).ToList())
                {
                    _db.Employee_TravelLeave_Documents.Remove(item);
                    _db.SaveChanges();
                }

                foreach (var item in model.DocumentList)
                {
                    Employee_TravelLeave_Documents Document = new Employee_TravelLeave_Documents();
                    Document.TravelLeaveId = Leave.Id;
                    Document.NewName = item.newName;
                    Document.OriginalName = item.originalName;
                    Document.Description = item.description;
                    Document.Archived = false;
                    Document.CreatedBy = UserId;
                    Document.CreatedDate = DateTime.Now;
                    Document.LastModifiedBy = UserId;
                    Document.LastModifiedDate = DateTime.Now;
                    _db.Employee_TravelLeave_Documents.Add(Document);
                    _db.SaveChanges();
                }
            }
            else
            {
                Employee_TravelLeave Leave = new Employee_TravelLeave();
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
                Leave.IsReadAddReport = false;
                Leave.IsReadHR = false;
                Leave.ApprovalStatus = "Pending";                
                Leave.CreatedBy = UserId;
                Leave.CreatedDate = DateTime.Now;
                Leave.LastModifiedBy = UserId;
                Leave.LastModifiedDate = DateTime.Now;
                _db.Employee_TravelLeave.Add(Leave);
                _db.SaveChanges();

                foreach (var item in model.DocumentList)
                {
                    Employee_TravelLeave_Documents Document = new Employee_TravelLeave_Documents();
                    Document.TravelLeaveId = Leave.Id;
                    Document.NewName = item.newName;
                    Document.OriginalName = item.originalName;
                    Document.Description = item.description;
                    Document.Archived = false;
                    Document.CreatedBy = UserId;
                    Document.CreatedDate = DateTime.Now;
                    Document.LastModifiedBy = UserId;
                    Document.LastModifiedDate = DateTime.Now;
                    _db.Employee_TravelLeave_Documents.Add(Document);
                    _db.SaveChanges();
                }
            }
        }

        #endregion

        #region TimeSheet

        public List<Employee_TimeSheet> getAllTimeSheet()
        {
            return _db.Employee_TimeSheet.Where(x => x.Archived == false).ToList();
        }

        public Employee_TimeSheet getTimeSheetById(int Id)
        {
            return _db.Employee_TimeSheet.Where(x => x.Id == Id).FirstOrDefault();
        }

        public List<Employee_TimeSheet_Documents> getAllTimeSheetDocument(int timeSheetId)
        {
            return _db.Employee_TimeSheet_Documents.Where(x => x.Archived == false && x.TimeSheetId == timeSheetId).ToList();
        }

        public List<Employee_TimeSheet_Detail> getAllTimeSheetDetail(int timeSheetId)
        {
            return _db.Employee_TimeSheet_Detail.Where(x => x.Archived == false && x.TimeSheetId == timeSheetId).ToList();
        }

        public Employee_TimeSheet getTimeSheetPlannerByEmployeeId(int Id,DateTime dt)
        {
            return _db.Employee_TimeSheet.Where(x => x.EmployeeId == Id && x.Date==dt && x.Archived==false).FirstOrDefault();
        }

        public List<Employee_TimeSheet_Detail>getAlltimehseetPlannerDetail(int timesheetId)
        {
            return _db.Employee_TimeSheet_Detail.Where(x => x.Archived == false && x.TimeSheetId == timesheetId).ToList();
        }
    

    public void TimeSheet_SaveData(EmployeePlanner_TimeSheetViewModel model, int UserId)
        {
            if (model.Id > 0)
            {
                Employee_TimeSheet timesheet = _db.Employee_TimeSheet.Where(x => x.Id == model.Id).FirstOrDefault();
                timesheet.EmployeeId = model.EmployeeId;
                var DateToString = DateTime.ParseExact(model.Date, inputFormat, CultureInfo.InvariantCulture);
                timesheet.Date = Convert.ToDateTime(DateToString.ToString(outputFormat));
                timesheet.Comments = model.Comment;
                timesheet.LastModifiedBy = UserId;
                timesheet.LastModifiedDate = DateTime.Now;
                _db.SaveChanges();

                foreach (var item in _db.Employee_TimeSheet_Detail.Where(x => x.TimeSheetId == timesheet.Id).ToList())
                {
                    _db.Employee_TimeSheet_Detail.Remove(item);
                    _db.SaveChanges();
                }

                foreach (var item in model.DetailList)
                {
                    Employee_TimeSheet_Detail Detail = new Employee_TimeSheet_Detail();
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
                    _db.Employee_TimeSheet_Detail.Add(Detail);
                    _db.SaveChanges();
                }

                foreach (var item in _db.Employee_TimeSheet_Documents.Where(x => x.TimeSheetId == timesheet.Id).ToList())
                {
                    _db.Employee_TimeSheet_Documents.Remove(item);
                    _db.SaveChanges();
                }

                foreach (var item in model.DocumentList)
                {
                    Employee_TimeSheet_Documents Document = new Employee_TimeSheet_Documents();
                    Document.TimeSheetId = timesheet.Id;
                    Document.NewName = item.newName;
                    Document.OriginalName = item.originalName;
                    Document.Description = item.description;
                    Document.Archived = false;
                    Document.CreatedBy = UserId;
                    Document.CreatedDate = DateTime.Now;
                    Document.LastModifiedBy = UserId;
                    Document.LastModifiedDate = DateTime.Now;
                    _db.Employee_TimeSheet_Documents.Add(Document);
                    _db.SaveChanges();
                }
            }
            else
            {
                Employee_TimeSheet timesheet = new Employee_TimeSheet();
                timesheet.EmployeeId = model.EmployeeId;
                var DateToString = DateTime.ParseExact(model.Date, inputFormat, CultureInfo.InvariantCulture);
                timesheet.Date = Convert.ToDateTime(DateToString.ToString(outputFormat));
                timesheet.Comments = model.Comment;
                timesheet.Archived = false;
                
                timesheet.CreatedBy = UserId;
                timesheet.CreatedDate = DateTime.Now;
                timesheet.LastModifiedBy = UserId;
                timesheet.LastModifiedDate = DateTime.Now;
                _db.Employee_TimeSheet.Add(timesheet);
                _db.SaveChanges();

                foreach (var item in model.DetailList)
                {
                    Employee_TimeSheet_Detail Detail = new Employee_TimeSheet_Detail();
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
                    Detail.IsRead = false;
                    Detail.IsReadAddRep = false;
                    Detail.IsReadHR = false;
                    Detail.ApprovalStatus = "Pending";
                    Detail.CreatedBy = UserId;
                    Detail.CreatedDate = DateTime.Now;
                    Detail.LastModifiedBy = UserId;
                    Detail.LastModifiedDate = DateTime.Now;
                    _db.Employee_TimeSheet_Detail.Add(Detail);
                    _db.SaveChanges();
                }

                foreach (var item in model.DocumentList)
                {
                    Employee_TimeSheet_Documents Document = new Employee_TimeSheet_Documents();
                    Document.TimeSheetId = timesheet.Id;
                    Document.NewName = item.newName;
                    Document.OriginalName = item.originalName;
                    Document.Description = item.description;
                    Document.Archived = false;
                    Document.CreatedBy = UserId;
                    Document.CreatedDate = DateTime.Now;
                    Document.LastModifiedBy = UserId;
                    Document.LastModifiedDate = DateTime.Now;
                    _db.Employee_TimeSheet_Documents.Add(Document);
                    _db.SaveChanges();
                }
            }
        }

        public string GetTimeSheetTotalTimeInMonth(int Employeeid, DateTime StartDate, DateTime EndDate)
        {
            //string details = "0";
            var data = _db.Employee_TimeSheet.Where(x => x.EmployeeId == Employeeid && x.Date >= StartDate && x.Date <= EndDate && x.Archived == false).ToList();
            if (data.Count > 0)
            {
                List<int> timeSheetIds = data.Select(x => x.Id).ToList();
                var timeSheetHoursDetails = _db.Employee_TimeSheet_Detail.Where(x => timeSheetIds.Contains((int)x.TimeSheetId)).ToList();
                List<double> totalSecound = new List<double>();
                if (timeSheetHoursDetails.Count > 0)
                {
                    foreach (var item in timeSheetHoursDetails)
                    {
                        TimeSpan inTimeSpan = TimeSpan.Parse(string.Format("{0}:{1}", item.InTimeHr, item.InTimeMin));
                        TimeSpan OutTimeSpan = TimeSpan.Parse(string.Format("{0}:{1}", item.EndTimeHr, item.EndTimeMin));
                        TimeSpan diffance = OutTimeSpan - inTimeSpan;
                        totalSecound.Add(diffance.TotalSeconds);
                    }
                }

                if (totalSecound.Count > 0)
                {
                    return ConvertToTime(totalSecound.Sum());
                }
                else
                {
                    return "00:00";
                }
            }
            else
            {
                return "00:00";
            }
        }

        public string GetProjectPlannerTimeSheetTotalTimeInMonth(int Employeeid, DateTime StartDate, DateTime EndDate)
        {
            //string details = "0";
            var data = _db.Employee_ProjectPlanner_TimeSheet.Where(x => x.EmployeeId == Employeeid && x.Date >= StartDate && x.Date <= EndDate && x.Archived == false).ToList();
            if (data.Count > 0)
            {
                List<int> timeSheetIds = data.Select(x => x.Id).ToList();
                var timeSheetHoursDetails = _db.Employee_ProjectPlanner_TimeSheet_Detail.Where(x => timeSheetIds.Contains((int)x.TimeSheetId)).ToList();
                List<double> totalSecound = new List<double>();
                if (timeSheetHoursDetails.Count > 0)
                {
                    foreach (var item in timeSheetHoursDetails)
                    {
                        TimeSpan inTimeSpan = TimeSpan.Parse(string.Format("{0}:{1}", item.InTimeHr, item.InTimeMin));
                        TimeSpan OutTimeSpan = TimeSpan.Parse(string.Format("{0}:{1}", item.EndTimeHr, item.EndTimeMin));
                        TimeSpan diffance = OutTimeSpan - inTimeSpan;
                        totalSecound.Add(diffance.TotalSeconds);
                    }
                }

                if (totalSecound.Count > 0)
                {
                    return ConvertToTime(totalSecound.Sum());
                }
                else
                {
                    return "00:00";
                }
            }
            else
            {
                return "00:00";
            }
        }

        public string ConvertToTime(double timeSeconds)
        {

            int mySeconds = System.Convert.ToInt32(timeSeconds);
            int myHours = mySeconds / 3600;
            mySeconds %= 3600;
            int myMinutes = mySeconds / 60; //60 Seconds in a minute
            mySeconds %= 60;
            string mySec = mySeconds.ToString(),
                myMin = myMinutes.ToString(),
            myHou = myHours.ToString();
            if (myHours < 10) { myHou = myHou.Insert(0, "0"); }
            if (myMinutes < 10) { myMin = myMin.Insert(0, "0"); }
            if (mySeconds < 10) { mySec = mySec.Insert(0, "0"); }
            return myHou + ":" + myMin;
        }
        #endregion

        #region Public Holiday

        public int GetAllPublicHolidayByMonth_JobCountryId(int EmpId,int day)
        {
            var data = _db.GetAllPublicHolidayByEmployee_JobCountry(EmpId).ToList();
            int PublicHolidayMonth = 0;
            foreach (var item in data)
            {
                if (item.HolidayDate.Value.Month == day)
                {
                    PublicHolidayMonth = PublicHolidayMonth + 1;
                }
            }


            return PublicHolidayMonth;
        }
        public int GetAllPublicHolidayBy_AspnetUsersJobCountry(int CountryId,int day)
        {
            var data = _db.GetPublicHolidayByCountryID(CountryId).ToList();
            int PublicHolidayMonth = 0;
            foreach (var item in data)
            {
                if (item.PublicHolidayDate.Value.Month == day)
                {
                    PublicHolidayMonth = PublicHolidayMonth + 1;
                }
            }
            return PublicHolidayMonth;
        }
        public string GetAllPublicHolidaysMonthWise(int EmployeeId, DateTime Startdate, DateTime Enddate)
        {
            var data = _db.Employee_PublicHoliday.Where(x => x.EmployeeId == EmployeeId && x.EffecitveFrom >= Startdate && x.EffecitveFrom <= Enddate).ToList();

            if (data.Count > 0)
            {
                List<int> timeSheetIds = data.Select(x => (int)x.PublicHolidayCountryId).ToList();
                var timeSheetHoursDetails = _db.PublicHolidays.Where(x => timeSheetIds.Contains((int)x.PublicHolidayCountryID)).ToList();
                //List<double> totalSecound = new List<double>();
                if (timeSheetHoursDetails.Count > 0)
                {
                    return Convert.ToString(timeSheetHoursDetails.Count);
                }
                else
                {
                    return "0";
                }
            }
            else
            {
                return "0";
            }
        }
        public IList<Employee_PublicHoliday> publicHolidayList()
        {
            return _db.Employee_PublicHoliday.ToList();
        }
        public void saveEmployeeJobTitleCountry(int EmpId,int JobCountryId)
        {
            AspNetUser model = new AspNetUser();
            if(EmpId>0)
            {
                var data = _db.AspNetUsers.Where(x => x.Id == EmpId && x.Archived==false).FirstOrDefault();
                data.JobContryID = JobCountryId;                
                _db.SaveChanges();
            }

        }
        public IList<GetAllPublicHolidayByEmployee_JobCountry_Result> getGetAllPublicHolidayByEmployee_JobCountry(int EmpId)
        {
            return _db.GetAllPublicHolidayByEmployee_JobCountry(EmpId).ToList();
        }
        public List<GetPublicHoliday_Result> getPublicHolidayList(int Id)
        {
            return _db.GetPublicHoliday(Id).ToList();
        }

        public List<GetPublicHolidayByCountryID_Result> getPublicHolidayByCountryList(int CountryID)
        {
            return _db.GetPublicHolidayByCountryID(CountryID).Where(x=>x.PublicHolidayDate.Value.Year==DateTime.Now.Year).ToList();
        }

        public IList<Employee_PublicHoliday> publicHolidayListByEmployeeId(int EmployeeId)
        {
            return _db.Employee_PublicHoliday.Where(x => x.EmployeeId == EmployeeId).ToList();
        }
        public decimal GetTravelInYearCount(List<Employee_ProjectPlanner_TravelLeave> TravelProjectPlanner, HolidayYearAndMonthSetting HolidayYearAndMonth)
        {
            int TravelYearCount = 0;

            if (HolidayYearAndMonth != null)
            {
                DateTime StartDate = new DateTime(HolidayYearAndMonth.StartYear.Value, HolidayYearAndMonth.StartMonth.Value, 1);
                DateTime EndDate = new DateTime(HolidayYearAndMonth.EndYear.Value, HolidayYearAndMonth.EndMonth.Value, 1);

                if (TravelProjectPlanner != null)
                {
                    var Travel = TravelProjectPlanner.Where(x => x.StartDate >= StartDate && x.EndDate <= EndDate).ToList();
                    //List<DateTime> ListOfDate = new List<DateTime>();
                    if (Travel != null)
                    {
                        foreach (var item in Travel)
                        {
                            for (DateTime i = item.StartDate.Value; i <= item.EndDate; i = i.AddDays(1))
                            {
                                TravelYearCount  = TravelYearCount + 1;
                                //ListOfDate.Add(i);
                            }
                        }
                    }
                    //TravelYearCount = Convert.ToDecimal(Common.DaysLeft(StartDate, EndDate, true, ListOfDate));
                }
            }
            return Convert.ToDecimal(TravelYearCount);
        }

        public decimal GetTravelsinceEverCount(List<Employee_ProjectPlanner_TravelLeave> TravelProjectPlanner)
        {
            int TravelDaySinceYear = 0;
            if (TravelProjectPlanner != null && TravelProjectPlanner.Count > 0)
            {
                foreach (var item in TravelProjectPlanner)
                {
                    for (DateTime i = item.StartDate.Value; i <= item.EndDate; i = i.AddDays(1))
                    {
                        TravelDaySinceYear = TravelDaySinceYear + 1;
                    }
                }
            }
            return Convert.ToDecimal(TravelDaySinceYear);
        }

        public decimal GetUpliftDayInYearCount(List<Employee_ProjectPlanner_Uplift> UpliftProjectPlanner, HolidayYearAndMonthSetting HolidayYearAndMonth)
        {
            int UpliftDayCount = 0;
            if (HolidayYearAndMonth != null)
            {
                DateTime StartDate = new DateTime(HolidayYearAndMonth.StartYear.Value, HolidayYearAndMonth.StartMonth.Value, 1);
                DateTime EndDate = new DateTime(HolidayYearAndMonth.EndYear.Value, HolidayYearAndMonth.EndMonth.Value, 1);

                if (UpliftProjectPlanner != null)
                {
                    var Uplift = UpliftProjectPlanner.Where(x => x.Date >= StartDate && x.Date <= EndDate).ToList();
                    List<DateTime> ListOfDate = new List<DateTime>();
                    if (Uplift != null)
                    {
                        foreach (var item in Uplift)
                        {
                            UpliftDayCount = UpliftDayCount + 1;
                            //    ListOfDate.Add(item.Date);
                        }
                    }
                    //UpliftDayCount = Convert.ToDecimal(Common.DaysLeft(StartDate, EndDate, true, ListOfDate));
                }
            }
            return Convert.ToDecimal(UpliftDayCount);
        }
        
        public decimal GetTimeSheetDayInYearCount(List<Employee_ProjectPlanner_TimeSheet> TimeSheetProjectPlanner, HolidayYearAndMonthSetting HolidayYearAndMonth)
        {
            int TimeSheetDayCount = 0;
            if (HolidayYearAndMonth != null)
            {
                DateTime StartDate = new DateTime(HolidayYearAndMonth.StartYear.Value, HolidayYearAndMonth.StartMonth.Value, 1);
                DateTime EndDate = new DateTime(HolidayYearAndMonth.EndYear.Value, HolidayYearAndMonth.EndMonth.Value, 1);

                if (TimeSheetProjectPlanner != null)
                {
                    var TimeSheet = TimeSheetProjectPlanner.Where(x => x.Date >= StartDate && x.Date <= EndDate).ToList();
                    List<DateTime> ListOfDate = new List<DateTime>();
                    if (TimeSheet != null)
                    {
                        foreach (var item in TimeSheet)
                        {
                            if (item.Date != null)
                            {
                                TimeSheetDayCount = TimeSheetDayCount + 1;
                            }
                        }
                    }
                    //TimeSheetDayCount = Convert.ToDecimal(Common.DaysLeft(StartDate, EndDate, true, ListOfDate));
                }
            }
            return Convert.ToDecimal(TimeSheetDayCount);
        }

        public void SaveData_EffactivePublicHoliday(int CountryId, string EffactiveDate, int EmployeeId, int userId)
        {
            var EffactiveDateToString = DateTime.ParseExact(EffactiveDate, inputFormat, CultureInfo.InvariantCulture);
            DateTime effDate = Convert.ToDateTime(EffactiveDateToString.ToString(outputFormat));
            Employee_PublicHoliday employeeList = publicHolidayList().Where(x => x.EmployeeId == EmployeeId).OrderBy(x => x.Id).LastOrDefault();
            if (employeeList != null)
            {
                employeeList.EffectiveTo = effDate.AddDays(-1);
                _db.SaveChanges();
            }

            Employee_PublicHoliday model = new Employee_PublicHoliday();
            model.PublicHolidayCountryId = CountryId;
            model.EffecitveFrom = effDate;
            model.EmployeeId = EmployeeId;
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = userId;
            _db.Employee_PublicHoliday.Add(model);
            _db.SaveChanges();
        }

        #endregion

        #region Settings

        public bool SaveEmployeeAddEditTOIL(EmployeeTOILModelView Model, int UserId)
        {
            if (Model.Id > 0)
            {
                var data = _db.Employee_TOIL.Where(x => x.Id == Model.Id).FirstOrDefault();
                data.DurationDays = Model.DurationDays;
                var LateDateToString = DateTime.ParseExact(Model.ExpiryDate, inputFormat, CultureInfo.InvariantCulture);
                data.ExpiryDate = Convert.ToDateTime(LateDateToString.ToString(outputFormat));
                data.SupportingComments = Model.SupportingComments;
                data.LastModified = DateTime.Now;
                data.UserIDLastModifiedBy = UserId;
                _db.SaveChanges();
                var Details = _db.AspNetUsers.Where(x => x.Id == Model.EmployeeId).FirstOrDefault();
                if (Model.AddEdit)
                {
                    Details.TOIL = Details.TOIL + Model.DurationDays;
                }
                else
                {
                    Details.TOIL = Details.TOIL - Model.DurationDays;
                }
                _db.SaveChanges();
            }
            else
            {
                Employee_TOIL Save = new Employee_TOIL();
                Save.EmployeeId = Model.EmployeeId;
                var LateDateToString = DateTime.ParseExact(Model.ExpiryDate, inputFormat, CultureInfo.InvariantCulture);
                Save.ExpiryDate = Convert.ToDateTime(LateDateToString.ToString(outputFormat));
                Save.DurationDays = Model.DurationDays;
                Save.SupportingComments = Model.SupportingComments;
                Save.Archived = false;
                Save.CreatedDate = DateTime.Now;
                Save.LastModified = DateTime.Now;
                Save.UserIDCreatedBy = UserId;
                Save.UserIDLastModifiedBy = UserId;
                _db.Employee_TOIL.Add(Save);
                _db.SaveChanges();
                var Details = _db.AspNetUsers.Where(x => x.Id == Model.EmployeeId).FirstOrDefault();
                if (Details.TOIL == null)
                {
                    Details.TOIL = 0;
                }
                if (Model.AddEdit)
                {

                    Details.TOIL = (Details.TOIL) + (Model.DurationDays);
                }
                else
                {
                    Details.TOIL = Details.TOIL - Model.DurationDays;
                }
                _db.SaveChanges();
            }

            return true;
        }

        #endregion

        #region WorkPattern

        public List<Employee_WorkPattern> getAllEmployeeWorkPattern()
        {
            return _db.Employee_WorkPattern.Where(x => x.Archived == false).ToList();
        }

        public List<Employee_WorkPattern> GetNoorderEmployeeWorkPatternListByEmployeeId(int Id)
        {
            return _db.Employee_WorkPattern.Where(x => x.EmployeeID == Id).ToList();
        }

        public List<Employee_WorkPattern> GetEmployeeWorkPatternListByEmployeeId(int Id)
        {
            return _db.Employee_WorkPattern.Where(x => x.EmployeeID == Id).OrderByDescending(x => x.Id).ToList();
        }

        public bool SaveDataEmployeeWorkPattern(EmployeeWorkPatternViewModel Model, int UserId)
        {
            //var data = _db.Employee_WorkPattern.Where(x => x.WorkPatternID == Model.WorkPatternId && x.EmployeeID == Model.EmployeeId).FirstOrDefault();
            //if (data != null)
            //{
            //    //var data = _db.Employee_WorkPattern.Where(x => x.Id == Model.Id).FirstOrDefault();
            //    var EffectiveFromDateToString = DateTime.ParseExact(Model.EffectiveFrom, inputFormat, CultureInfo.InvariantCulture);
            //    data.EffectiveFrom = Convert.ToDateTime(EffectiveFromDateToString.ToString(outputFormat));
            //    data.EmployeeID = Model.EmployeeId;
            //    data.CurrentWeekWorkPatternDetailID = Model.CurrentWeekWorkPatternDetailID;
            //    data.WorkPatternID = Model.WorkPatternId;
            //    data.UserIDLastModifiedBy = UserId;
            //    data.LastModified = DateTime.Now;
            //    _db.SaveChanges();

            //}
            //else
            //{
            var EffactiveDateToString = DateTime.ParseExact(Model.EffectiveFrom, inputFormat, CultureInfo.InvariantCulture);
            DateTime effDate = Convert.ToDateTime(EffactiveDateToString.ToString(outputFormat));
            Employee_WorkPattern employeeList = getAllEmployeeWorkPattern().Where(x => x.EmployeeID == Model.EmployeeId).OrderBy(x => x.Id).LastOrDefault();
            if (employeeList != null)
            {
                employeeList.EffectiveTo = effDate.AddDays(-1);
                employeeList.NewWorkPatternID = Model.WorkPatternId;
                _db.SaveChanges();
            }
            Employee_WorkPattern work = new Employee_WorkPattern();
            work.EmployeeID = Model.EmployeeId;
            work.WorkPatternID = Model.WorkPatternId;
            var EffectiveFromDateToString = DateTime.ParseExact(Model.EffectiveFrom, inputFormat, CultureInfo.InvariantCulture);
            work.EffectiveFrom = Convert.ToDateTime(EffectiveFromDateToString.ToString(outputFormat));
            work.CurrentWeekWorkPatternDetailID = Model.CurrentWeekWorkPatternDetailID;
            work.Archived = false;
            work.UserIDCreatedBy = UserId;
            work.UserIDLastModifiedBy = UserId;
            work.CreatedDate = DateTime.Now;
            work.LastModified = DateTime.Now;
            _db.Employee_WorkPattern.Add(work);
            _db.SaveChanges();

            // }
            return true;
        }



        #endregion

        #region Sick Leave

        //GetSickLeavesTotalTimeInMonth
        public Decimal GetSickLeavesTotalTimeInMonth(int Employeeid, DateTime StartDate, DateTime EndDate)
        {
            //string details = "0";
            var data = _db.Employee_SickLeaves.Where(x => x.EmployeeId == Employeeid && x.StartDate >= StartDate && x.StartDate <= EndDate && x.Archived == false).ToList();
            if (data.Count > 0)
            {
                List<double> totalSecound = new List<double>();

                foreach (var item in data)
                {
                    if (item.StartDate != null && item.EndDate != null)
                    {
                        DateTime t1 = (DateTime)item.StartDate;
                        DateTime t2 = (DateTime)item.EndDate;
                        var diffance = (t2 - t1).TotalDays;
                        totalSecound.Add(diffance + 1);
                    }

                }


                if (totalSecound.Count > 0)
                {
                    return Convert.ToDecimal(totalSecound.Sum());
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public List<Employee_SickLeaves> getAllSickLeave()
        {
            return _db.Employee_SickLeaves.Where(x => x.Archived == false).ToList();
        }

        public Employee_SickLeaves getSickLeaveById(int Id)
        {
            return _db.Employee_SickLeaves.Where(x => x.Id == Id).FirstOrDefault();
        }

        public List<Employee_SickLeaves> getSickLeaveByEmployeeId(int Id, string st, string ed)
        {
            var stDateToString = DateTime.ParseExact(st, inputFormat, CultureInfo.InvariantCulture);
            DateTime Startdate = Convert.ToDateTime(stDateToString.ToString(outputFormat));
            var edDateToString = DateTime.ParseExact(ed, inputFormat, CultureInfo.InvariantCulture);
            DateTime Enddate = Convert.ToDateTime(edDateToString.ToString(outputFormat));

            return _db.Employee_SickLeaves.Where(x => x.EmployeeId == Id && x.StartDate >= Startdate && x.EndDate <= Enddate && x.Archived == false).ToList();
        }

        public List<Employee_SickLeaves_Documents> getAllSickLeaveDocument(int Id)
        {
            return _db.Employee_SickLeaves_Documents.Where(x => x.Archived == false && x.SickLeaveID == Id).ToList();
        }

        public List<Employee_SickLeaves_Comments> getAllSickLeaveComment(int Id)
        {
            return _db.Employee_SickLeaves_Comments.Where(x => x.Archived == false && x.SickLeaveID == Id).ToList();
        }

        public void SickLeave_SaveData(EmployeePlanner_SickLeaves model, List<SickLeavesCommentViewModel> CommentList, List<SickLeavesDocumentViewModel> DocumentList, int UserId)
        {
            if (model.Id > 0)
            {
                Employee_SickLeaves Leave = _db.Employee_SickLeaves.Where(x => x.Id == model.Id).FirstOrDefault();
                Leave.EmployeeId = model.EmployeeId;
                //Step 1
                Leave.Reason = model.Reason;
                Leave.IsADayOrMore = model.IsADayOrMore;
                Leave.IsHalfDay = model.IsHalfDay;
                Leave.IsHours = model.IsHours;
                var StartDateToString = DateTime.ParseExact(model.StartDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.StartDate = Convert.ToDateTime(StartDateToString.ToString(outputFormat));
                if (Leave.IsADayOrMore == true)
                {
                    var endDateToString = DateTime.ParseExact(model.EndDate, inputFormat, CultureInfo.InvariantCulture);
                    Leave.EndDate = Convert.ToDateTime(endDateToString.ToString(outputFormat));
                    Leave.DurationDays = model.DurationDays;
                }
                else
                {
                    //Leave.InTimeHrSD = model.InTimeHrSD;
                    //Leave.InTimeMinSD = model.InTimeMinSD;
                    //Leave.InTimeHrED = model.InTimeHrED;
                    //Leave.InTimeMinED = model.InTimeMinED;
                    Leave.PartOfDay = model.PartOfDay;
                    Leave.DurationHours = model.DurationHours;
                }
                Leave.EmergencyLeave = model.EmergencyLeave;

                //Step 2
                Leave.ConfirmedbyHR = model.ConfirmedbyHR;
                Leave.SelfCertificationFormRequired = model.SelfCertificationFormRequired;
                if (Leave.SelfCertificationFormRequired == true)
                {
                    var ReceivedDateToString = DateTime.ParseExact(model.SelfCertificateReceivedDate, inputFormat, CultureInfo.InvariantCulture);
                    Leave.SelfCertificateReceivedDate = Convert.ToDateTime(ReceivedDateToString.ToString(outputFormat));

                }
                Leave.BackToWorkInterviewRequired = model.BackToWorkInterviewRequired;
                if (Leave.BackToWorkInterviewRequired == true)
                {
                    var InterviewDateToString = DateTime.ParseExact(model.InterviewDate, inputFormat, CultureInfo.InvariantCulture);
                    Leave.InterviewDate = Convert.ToDateTime(InterviewDateToString.ToString(outputFormat));
                    Leave.InterviewConductedBy = model.InterviewConductedBy;
                }
                Leave.IsPaid = model.IsPaid;
                Leave.IsPaidatotherrate = model.IsPaidatotherrate;
                Leave.IsUnpaid = model.IsUnpaid;

                //Step 3
                Leave.DoctorConsulted = model.DoctorConsulted;
                if (Leave.DoctorConsulted == true)
                {
                    Leave.DoctorName = model.DoctorName;
                    var DateOfVisitToString = DateTime.ParseExact(model.DateOfVisit, inputFormat, CultureInfo.InvariantCulture);
                    Leave.DateOfVisit = Convert.ToDateTime(DateOfVisitToString.ToString(outputFormat));
                    //Leave.TimeOfVisitStartHr = model.TimeOfVisitStartHr;
                    //Leave.TimeOfVisitStartMin = model.TimeOfVisitStartMin;
                    //Leave.TimeOfVisitEndHr = model.TimeOfVisitEndHr;
                    //Leave.TimeOfVisitEndMin = model.TimeOfVisitEndMin;

                    var IssuedDateToString = DateTime.ParseExact(model.MedicalCertificateIssuedDate, inputFormat, CultureInfo.InvariantCulture);
                    Leave.MedicalCertificateIssuedDate = Convert.ToDateTime(IssuedDateToString.ToString(outputFormat));
                    Leave.DoctorAdvice = model.DoctorAdvice;
                    Leave.MedicationPrescribed = model.MedicationPrescribed;

                }
                else
                {
                    Leave.WhyDoctorNotConsulted = model.WhyDoctorNotConsulted;
                }

                Leave.IsDuaToAccident = model.IsDuaToAccident;
                Leave.UserIDLastModifiedBy = UserId;
                Leave.LastModified = DateTime.Now;
                _db.SaveChanges();

                foreach (var item in _db.Employee_SickLeaves_Comments.Where(x => x.SickLeaveID == Leave.Id).ToList())
                {
                    _db.Employee_SickLeaves_Comments.Remove(item);
                    _db.SaveChanges();
                }
                foreach (var item in CommentList)
                {
                    Employee_SickLeaves_Comments Comment = new Employee_SickLeaves_Comments();
                    Comment.SickLeaveID = Leave.Id;
                    Comment.Description = item.comment;
                    Comment.CreatedName = item.commentBy;
                    Comment.CreatedDateTime = item.commentTime;
                    Comment.Archived = false;
                    Comment.UserIDCreatedBy = UserId;
                    Comment.CreatedDate = DateTime.Now;
                    Comment.UserIDLastModifiedBy = UserId;
                    Comment.LastModified = DateTime.Now;
                    _db.Employee_SickLeaves_Comments.Add(Comment);
                    _db.SaveChanges();
                }


                foreach (var item in _db.Employee_SickLeaves_Documents.Where(x => x.SickLeaveID == Leave.Id).ToList())
                {
                    _db.Employee_SickLeaves_Documents.Remove(item);
                    _db.SaveChanges();
                }

                foreach (var item in DocumentList)
                {
                    Employee_SickLeaves_Documents Document = new Employee_SickLeaves_Documents();
                    Document.SickLeaveID = Leave.Id;
                    Document.NewName = item.newName;
                    Document.OriginalName = item.originalName;
                    Document.Description = item.description;
                    Document.Archived = false;
                    Document.UserIDCreatedBy = UserId;
                    Document.CreatedDate = DateTime.Now;
                    Document.UserIDLastModifiedBy = UserId;
                    Document.LastModified = DateTime.Now;
                    _db.Employee_SickLeaves_Documents.Add(Document);
                    _db.SaveChanges();
                }
            }
            else
            {
                Employee_SickLeaves Leave = new Employee_SickLeaves();
                Leave.EmployeeId = model.EmployeeId;
                //Step 1
                Leave.Reason = model.Reason;
                Leave.IsADayOrMore = model.IsADayOrMore;
                var StartDateToString = DateTime.ParseExact(model.StartDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.StartDate = Convert.ToDateTime(StartDateToString.ToString(outputFormat));
                if (Leave.IsADayOrMore == true)
                {
                    var endDateToString = DateTime.ParseExact(model.EndDate, inputFormat, CultureInfo.InvariantCulture);
                    Leave.EndDate = Convert.ToDateTime(endDateToString.ToString(outputFormat));
                    Leave.DurationDays = model.DurationDays;
                }
                else
                {
                    //Leave.InTimeHrSD = model.InTimeHrSD;
                    //Leave.InTimeMinSD = model.InTimeMinSD;
                    //Leave.InTimeHrED = model.InTimeHrED;
                    //Leave.InTimeMinED = model.InTimeMinED;
                    Leave.PartOfDay = model.PartOfDay;
                    Leave.DurationHours = model.DurationHours;
                }
                Leave.EmergencyLeave = model.EmergencyLeave;

                //Step 2
                Leave.ConfirmedbyHR = model.ConfirmedbyHR;
                Leave.SelfCertificationFormRequired = model.SelfCertificationFormRequired;
                if (Leave.SelfCertificationFormRequired == true)
                {
                    var ReceivedDateToString = DateTime.ParseExact(model.SelfCertificateReceivedDate, inputFormat, CultureInfo.InvariantCulture);
                    Leave.SelfCertificateReceivedDate = Convert.ToDateTime(ReceivedDateToString.ToString(outputFormat));

                }
                Leave.BackToWorkInterviewRequired = model.BackToWorkInterviewRequired;
                if (Leave.BackToWorkInterviewRequired == true)
                {
                    var InterviewDateToString = DateTime.ParseExact(model.InterviewDate, inputFormat, CultureInfo.InvariantCulture);
                    Leave.InterviewDate = Convert.ToDateTime(InterviewDateToString.ToString(outputFormat));
                    Leave.InterviewConductedBy = model.InterviewConductedBy;
                }
                Leave.IsPaid = model.IsPaid;
                Leave.IsPaidatotherrate = model.IsPaidatotherrate;
                Leave.IsUnpaid = model.IsUnpaid;

                //Step 3
                Leave.DoctorConsulted = model.DoctorConsulted;
                if (Leave.DoctorConsulted == true)
                {
                    Leave.DoctorName = model.DoctorName;
                    var DateOfVisitToString = DateTime.ParseExact(model.DateOfVisit, inputFormat, CultureInfo.InvariantCulture);
                    Leave.DateOfVisit = Convert.ToDateTime(DateOfVisitToString.ToString(outputFormat));
                    //Leave.TimeOfVisitStartHr = model.TimeOfVisitStartHr;
                    //Leave.TimeOfVisitStartMin = model.TimeOfVisitStartMin;
                    //Leave.TimeOfVisitEndHr = model.TimeOfVisitEndHr;
                    //Leave.TimeOfVisitEndMin = model.TimeOfVisitEndMin;
                    Leave.TimeOfVisit = model.TimeOfVisit;
                    var IssuedDateToString = DateTime.ParseExact(model.MedicalCertificateIssuedDate, inputFormat, CultureInfo.InvariantCulture);
                    Leave.MedicalCertificateIssuedDate = Convert.ToDateTime(IssuedDateToString.ToString(outputFormat));
                    Leave.DoctorAdvice = model.DoctorAdvice;
                    Leave.MedicationPrescribed = model.MedicationPrescribed;

                }
                else
                {
                    Leave.WhyDoctorNotConsulted = model.WhyDoctorNotConsulted;
                }

                Leave.IsDuaToAccident = model.IsDuaToAccident;
                Leave.Archived = false;
                Leave.UserIDCreatedBy = UserId;
                Leave.CreatedDate = DateTime.Now;
                Leave.UserIDLastModifiedBy = UserId;
                Leave.LastModified = DateTime.Now;
                Leave.ApprovalStatus = "Pending";
                Leave.IsRead = false;
                Leave.IsReadAddRep = false;
                Leave.IsReadHR = false;
                _db.Employee_SickLeaves.Add(Leave);
                _db.SaveChanges();

                foreach (var item in CommentList)
                {
                    Employee_SickLeaves_Comments leaveComment = new Employee_SickLeaves_Comments();
                    leaveComment.SickLeaveID = Leave.Id;
                    leaveComment.Description = item.comment;
                    leaveComment.CreatedName = item.commentBy;
                    leaveComment.CreatedDateTime = item.commentTime;
                    leaveComment.Archived = false;
                    leaveComment.UserIDCreatedBy = UserId;
                    leaveComment.CreatedDate = DateTime.Now;
                    leaveComment.UserIDLastModifiedBy = UserId;
                    leaveComment.LastModified = DateTime.Now;
                    _db.Employee_SickLeaves_Comments.Add(leaveComment);
                    _db.SaveChanges();
                }

                foreach (var item in DocumentList)
                {
                    Employee_SickLeaves_Documents Document = new Employee_SickLeaves_Documents();
                    Document.SickLeaveID = Leave.Id;
                    Document.NewName = item.newName;
                    Document.OriginalName = item.originalName;
                    Document.Description = item.description;
                    Document.Archived = false;
                    Document.UserIDCreatedBy = UserId;
                    Document.CreatedDate = DateTime.Now;
                    Document.UserIDLastModifiedBy = UserId;
                    Document.LastModified = DateTime.Now;
                    _db.Employee_SickLeaves_Documents.Add(Document);
                    _db.SaveChanges();
                }
            }
        }

        #endregion

        #region Maternity Paternity Leave


        //getAllMaternityPaternityLeaveMonthWise

        public Decimal getAllMaternityPaternityLeaveMonthWise(int EmployeeId, DateTime Startdate, DateTime Enddate)
        {
            var data = _db.Employee_MaternityOrPaternityLeaves.Where(x => x.EmployeeID == EmployeeId && x.ActualStartDate >= Startdate && x.ActualStartDate <= Enddate).ToList();

            if (data.Count > 0)
            {
                List<double> totalSecound = new List<double>();

                foreach (var item in data)
                {
                    DateTime t1 = (DateTime)item.ActualStartDate;
                    DateTime t2 = (DateTime)item.ActualEndDate;
                    var diffance = (t2 - t1).TotalDays;
                    totalSecound.Add(diffance + 1);

                }


                if (totalSecound.Count > 0)
                {
                    return Convert.ToDecimal(totalSecound.Sum());
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public List<Employee_MaternityOrPaternityLeaves> getAllMaternityPaternityLeave()
        {
            return _db.Employee_MaternityOrPaternityLeaves.Where(x => x.Archived == false).ToList();
        }

        public Employee_MaternityOrPaternityLeaves getMaternityPaternityById(int Id)
        {
            return _db.Employee_MaternityOrPaternityLeaves.Where(x => x.Id == Id).FirstOrDefault();
        }

        public List<Employee_MaternityOrPaternityLeaves_Document> getAllMaternityPaternityLeaveDocument(int Id)
        {
            return _db.Employee_MaternityOrPaternityLeaves_Document.Where(x => x.Archived == false && x.MaternityOrPaternityID == Id).ToList();
        }

        public List<Employee_MeternityOrPaternityLeaves_Comment> getAllMaternityPaternityLeaveComment(int Id)
        {
            return _db.Employee_MeternityOrPaternityLeaves_Comment.Where(x => x.Archived == false && x.MaternityOrPaternityID == Id).ToList();
        }


        public void MaternityPaternityLeave_SaveData(EmployeePlanner_MaternityPaternityViewModel model, List<MaternityPaternityCommentViewModel> CommentList, List<MaternityPaternityDocumentViewModel> DocumentList, int UserId)
        {
            if (model.Id > 0)
            {
                Employee_MaternityOrPaternityLeaves Leave = _db.Employee_MaternityOrPaternityLeaves.Where(x => x.Id == model.Id).FirstOrDefault();
                Leave.EmployeeID = model.EmployeeID;
                //Step 1
                var StartDateToString = DateTime.ParseExact(model.ActualStartDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.ActualStartDate = Convert.ToDateTime(StartDateToString.ToString(outputFormat));
                var EndDateToString = DateTime.ParseExact(model.ActualEndDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.ActualEndDate = Convert.ToDateTime(EndDateToString.ToString(outputFormat));
                var DuaDateToString = DateTime.ParseExact(model.DueDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.DueDate = Convert.ToDateTime(DuaDateToString.ToString(outputFormat));
                var AddisionStartToString = DateTime.ParseExact(model.AdditionalMaternityLeaveStartDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.AdditionalMaternityLeaveStartDate = Convert.ToDateTime(AddisionStartToString.ToString(outputFormat));
                var AddisionEndToString = DateTime.ParseExact(model.AdditionalMaternityLeaveEndDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.AdditionalMaternityLeaveEndDate = Convert.ToDateTime(AddisionEndToString.ToString(outputFormat));
                var EarliestToString = DateTime.ParseExact(model.EarliestBirthWeekStartDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.EarliestBirthWeekStartDate = Convert.ToDateTime(EarliestToString.ToString(outputFormat));
                var ExptedStartToString = DateTime.ParseExact(model.ExptectedBirthWeekStartDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.ExptectedBirthWeekStartDate = Convert.ToDateTime(ExptedStartToString.ToString(outputFormat));
                var ExptedEndToString = DateTime.ParseExact(model.ExptectedBirthWeekEndDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.ExptectedBirthWeekEndDate = Convert.ToDateTime(ExptedEndToString.ToString(outputFormat));
                var OrdinaryStartToString = DateTime.ParseExact(model.OrdinaryMaternityLeaveStartDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.OrdinaryMaternityLeaveStartDate = Convert.ToDateTime(OrdinaryStartToString.ToString(outputFormat));
                var OrdinaryEndToString = DateTime.ParseExact(model.OrdinaryMaternityLeaveEndDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.OrdinaryMaternityLeaveEndDate = Convert.ToDateTime(OrdinaryEndToString.ToString(outputFormat));
                Leave.UserIDLastModifiedBy = UserId;
                Leave.LastModified = DateTime.Now;
                _db.SaveChanges();

                foreach (var item in _db.Employee_MeternityOrPaternityLeaves_Comment.Where(x => x.MaternityOrPaternityID == Leave.Id).ToList())
                {
                    _db.Employee_MeternityOrPaternityLeaves_Comment.Remove(item);
                    _db.SaveChanges();
                }
                foreach (var item in CommentList)
                {
                    Employee_MeternityOrPaternityLeaves_Comment Comment = new Employee_MeternityOrPaternityLeaves_Comment();
                    Comment.MaternityOrPaternityID = Leave.Id;
                    Comment.Description = item.comment;
                    Comment.CreatedName = item.commentBy;
                    Comment.CreatedDateTime = item.commentTime;
                    Comment.Archived = false;
                    Comment.UserIDCreatedBy = UserId;
                    Comment.CreatedDate = DateTime.Now;
                    Comment.UserIDLastModifiedBy = UserId;
                    Comment.LastModified = DateTime.Now;
                    _db.Employee_MeternityOrPaternityLeaves_Comment.Add(Comment);
                    _db.SaveChanges();
                }


                foreach (var item in _db.Employee_MaternityOrPaternityLeaves_Document.Where(x => x.MaternityOrPaternityID == Leave.Id).ToList())
                {
                    _db.Employee_MaternityOrPaternityLeaves_Document.Remove(item);
                    _db.SaveChanges();
                }

                foreach (var item in DocumentList)
                {
                    Employee_MaternityOrPaternityLeaves_Document Document = new Employee_MaternityOrPaternityLeaves_Document();
                    Document.MaternityOrPaternityID = Leave.Id;
                    Document.NewName = item.newName;
                    Document.OriginalName = item.originalName;
                    Document.Description = item.description;
                    Document.Archived = false;
                    Document.UserIDCreatedBy = UserId;
                    Document.CreatedDate = DateTime.Now;
                    Document.UserIDLastModifiedBy = UserId;
                    Document.LastModified = DateTime.Now;
                    _db.Employee_MaternityOrPaternityLeaves_Document.Add(Document);
                    _db.SaveChanges();
                }
            }
            else
            {
                Employee_MaternityOrPaternityLeaves Leave = new Employee_MaternityOrPaternityLeaves();
                Leave.EmployeeID = model.EmployeeID;
                var StartDateToString = DateTime.ParseExact(model.ActualStartDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.ActualStartDate = Convert.ToDateTime(StartDateToString.ToString(outputFormat));
                var EndDateToString = DateTime.ParseExact(model.ActualEndDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.ActualEndDate = Convert.ToDateTime(EndDateToString.ToString(outputFormat));
                var DuaDateToString = DateTime.ParseExact(model.DueDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.DueDate = Convert.ToDateTime(DuaDateToString.ToString(outputFormat));
                var AddisionStartToString = DateTime.ParseExact(model.AdditionalMaternityLeaveStartDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.AdditionalMaternityLeaveStartDate = Convert.ToDateTime(AddisionStartToString.ToString(outputFormat));
                var AddisionEndToString = DateTime.ParseExact(model.AdditionalMaternityLeaveEndDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.AdditionalMaternityLeaveEndDate = Convert.ToDateTime(AddisionEndToString.ToString(outputFormat));
                var EarliestToString = DateTime.ParseExact(model.EarliestBirthWeekStartDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.EarliestBirthWeekStartDate = Convert.ToDateTime(EarliestToString.ToString(outputFormat));
                var ExptedStartToString = DateTime.ParseExact(model.ExptectedBirthWeekStartDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.ExptectedBirthWeekStartDate = Convert.ToDateTime(ExptedStartToString.ToString(outputFormat));
                var ExptedEndToString = DateTime.ParseExact(model.ExptectedBirthWeekEndDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.ExptectedBirthWeekEndDate = Convert.ToDateTime(ExptedEndToString.ToString(outputFormat));
                var OrdinaryStartToString = DateTime.ParseExact(model.OrdinaryMaternityLeaveStartDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.OrdinaryMaternityLeaveStartDate = Convert.ToDateTime(OrdinaryStartToString.ToString(outputFormat));
                var OrdinaryEndToString = DateTime.ParseExact(model.OrdinaryMaternityLeaveEndDate, inputFormat, CultureInfo.InvariantCulture);
                Leave.OrdinaryMaternityLeaveEndDate = Convert.ToDateTime(OrdinaryEndToString.ToString(outputFormat));
                Leave.Archived = false;
                Leave.UserIDCreatedBy = UserId;
                Leave.CreatedDate = DateTime.Now;
                Leave.UserIDLastModifiedBy = UserId;
                Leave.LastModified = DateTime.Now;
                Leave.ApprovalStatus = "Pending";
                Leave.IsRead = false;
                Leave.IsReadAddRes = false;
                Leave.IsReadHR = false;
                _db.Employee_MaternityOrPaternityLeaves.Add(Leave);
                _db.SaveChanges();

                foreach (var item in CommentList)
                {
                    Employee_MeternityOrPaternityLeaves_Comment leaveComment = new Employee_MeternityOrPaternityLeaves_Comment();
                    leaveComment.MaternityOrPaternityID = Leave.Id;
                    leaveComment.Description = item.comment;
                    leaveComment.CreatedName = item.commentBy;
                    leaveComment.CreatedDateTime = item.commentTime;
                    leaveComment.Archived = false;
                    leaveComment.UserIDCreatedBy = UserId;
                    leaveComment.CreatedDate = DateTime.Now;
                    leaveComment.UserIDLastModifiedBy = UserId;
                    leaveComment.LastModified = DateTime.Now;
                    _db.Employee_MeternityOrPaternityLeaves_Comment.Add(leaveComment);
                    _db.SaveChanges();
                }

                foreach (var item in DocumentList)
                {
                    Employee_MaternityOrPaternityLeaves_Document Document = new Employee_MaternityOrPaternityLeaves_Document();
                    Document.MaternityOrPaternityID = Leave.Id;
                    Document.NewName = item.newName;
                    Document.OriginalName = item.originalName;
                    Document.Description = item.description;
                    Document.Archived = false;
                    Document.UserIDCreatedBy = UserId;
                    Document.CreatedDate = DateTime.Now;
                    Document.UserIDLastModifiedBy = UserId;
                    Document.LastModified = DateTime.Now;
                    _db.Employee_MaternityOrPaternityLeaves_Document.Add(Document);
                    _db.SaveChanges();
                }
            }
        }

        #endregion

        #region Graph Details

        public decimal GetAllRemainingDaysHolidys(int EmployeeId)
        {
            var details = _db.AspNetUsers.Where(x => x.Id == EmployeeId).FirstOrDefault();
            var yearId = DateTime.Now.Year;
            DateTime St = new DateTime(yearId, 1, 1);
            DateTime ed = new DateTime(yearId, 12, 31);
            decimal Count = (decimal)(details.StartDate >= St && details.StartDate <= ed == true ? (details.Thisyear == null ? 0 : details.Thisyear) : (details.Nextyear == null ? 0 : details.Nextyear));

            return Count;

        }
        public decimal GetAllWorkingDaysByEmployeeId(int EmployeeId)
        {
            var details = _db.AspNetUsers.Where(x => x.Id == EmployeeId).FirstOrDefault();
            WorkpatternWeekend ww = new WorkpatternWeekend();
            WorkPattern workdetails = new WorkPattern();
            WorkPatternDetail weekday = new WorkPatternDetail();
            DateTime stdate = new DateTime();
            decimal totalWorkingDay=0;
            if (details.StartDate != null && !details.StartDate.HasValue)
            {
                stdate = (DateTime)details.StartDate;
            }
            else
            {
                stdate = DateTime.Now;
            }
            var workpattern = _db.Employee_WorkPattern.Where(x => x.EmployeeID == EmployeeId).FirstOrDefault();
            if (workpattern != null)
            {

                if (workpattern.NewWorkPatternID == null && workpattern.NewWorkPatternID==0)
                {
                    workdetails = _db.WorkPatterns.Where(x => x.Id == workpattern.WorkPatternID).FirstOrDefault();
                }
                else
                {
                    workdetails = _db.WorkPatterns.Where(x => x.Id == workpattern.NewWorkPatternID).FirstOrDefault();
                }

            }
            else
            {
                var Defaults = _holidayNAbsenceMethod.getAllHolidaysNAbsenceSettingList();
                if (Defaults != null)
                {
                    int wID = Convert.ToInt16(Defaults.WorkPattern);
                    workdetails = _db.WorkPatterns.Where(x => x.Id == wID).FirstOrDefault();
                }

            }
           if(workdetails!=null)
            {  if (workdetails.IsRotating == true)
                {
                    weekday = _db.WorkPatternDetails.Where(x => x.WorkPatternID == workdetails.Id).FirstOrDefault();
                    ww.Id = weekday.WorkPatternID;
                    ww.MondayDays = weekday.MondayDays;
                    ww.TuesdayDays = weekday.TuesdayDays;
                    ww.WednessdayDays = weekday.WednessdayDays;
                    ww.ThursdayDays = weekday.ThursdayDays;
                    ww.FridayDays = weekday.FridayDays;
                    ww.SaturdayDays = weekday.SaturdayDays;
                    ww.SundayDays = weekday.SundayDays;
                }
                else
                {
                    ww.Id = workdetails.Id;
                    ww.MondayDays = workdetails.MondayDays;
                    ww.TuesdayDays = workdetails.TuesdayDays;
                    ww.WednessdayDays = workdetails.WednessdayDays;
                    ww.ThursdayDays = workdetails.ThursdayDays;
                    ww.FridayDays = workdetails.FridayDays;
                    ww.SaturdayDays = workdetails.SaturdayDays;
                    ww.SundayDays = workdetails.SundayDays;
                }
                List<int> OffWeekday = new List<int>();

                if (ww.SundayDays == null)
                {
                    OffWeekday.Add(6);
                }
                if (ww.MondayDays == null)
                {
                    OffWeekday.Add(0);
                }
                if (ww.TuesdayDays == null)
                {
                    OffWeekday.Add(1);
                }
                if (ww.WednessdayDays == null)
                {
                    OffWeekday.Add(2);
                }
                if (ww.ThursdayDays == null)
                {
                    OffWeekday.Add(3);
                }
                if (ww.FridayDays == null)
                {
                    OffWeekday.Add(4);
                }
                if (ww.SaturdayDays == null)
                {
                    OffWeekday.Add(5);
                }


                var dd = DateTime.Now.DayOfWeek;
                DateTime yearStart = new DateTime(stdate.Year, 1, 1);
                DateTime yeatend = new DateTime(stdate.Year, 12, 31);
                var totalWorkingDays = Weekdays(yearStart, yeatend, OffWeekday[0], OffWeekday[1]);
                return totalWorkingDays;
                // var remainiingDays = Weekdays(stdate, yeatend);
            }
                return totalWorkingDay;
           
        }
        public decimal GetAllSickLeaveByEmployeeId(int EmployeeId)
        {
            var details = _db.Employee_SickLeaves.Where(x => x.EmployeeId == EmployeeId && x.Archived == false).ToList();

            return details.Count;
        }
        public decimal GetAllBookHolidays(int EmployeeId)
        {
            var details = _db.Employee_PublicHoliday.Where(x => x.EmployeeId == EmployeeId).ToList();
            return details.Count;
        }
        public decimal GetAllBookedHolidayCount(int EmployeeID)
        {
            var HolidayYearAndMonthSetting = _db.HolidayYearAndMonthSettings.Where(x => x.IsActive == true).ToList();
            int StartYear = HolidayYearAndMonthSetting.FirstOrDefault().StartYear.Value;
            int EndYear = HolidayYearAndMonthSetting.FirstOrDefault().EndYear.Value;
            int StartMonth = HolidayYearAndMonthSetting.FirstOrDefault().StartMonth.Value;
            int EndMonth = HolidayYearAndMonthSetting.FirstOrDefault().EndMonth.Value;
            DateTime StartDate = new DateTime(StartYear, StartMonth, 1);
            DateTime EndDate = new DateTime(EndYear, EndMonth, 1);

            decimal BookedHoliday = 0;
            var Employee_AnualLeave = _db.Employee_AnualLeave.Where(x => x.EmployeeId == EmployeeID).ToList();
            foreach (var item in Employee_AnualLeave)
            {
                if (item.StartDate != null)
                {
                    if (StartDate <= item.StartDate && EndDate >= item.EndDate)
                    {
                        if(item.Duration != null)
                        {
                            BookedHoliday += item.Duration.Value;
                        }
                        
                    }
                    //if (item.StartDate.Value.Year >= StartYear && item.EndDate.Value.Year <= EndYear)
                    //{
                    //        BookedHoliday = item.Duration.Value;
                    //}
                }
            }
            return BookedHoliday;
        }
        public decimal GetAllHolidayByEmployee(int EmployeeID)
        {
            Decimal HolidayEntitlement = _db.GetAllAllPublicHolidayByEmployee(EmployeeID).FirstOrDefault().Value;
            if (HolidayEntitlement != null && HolidayEntitlement != 0)
            {
                return HolidayEntitlement;
            }
            else
            {
                return 0;
            }
        }
        public decimal GetAllHolidayAbscence(int EmployeeID)
        {

            var HolidaysNAbsence_Setting = _db.HolidaysNAbsence_Setting.ToList();
            Decimal HolidayEntitlement = 0;
            if (HolidaysNAbsence_Setting != null)
            {
                HolidayEntitlement = Convert.ToDecimal(HolidaysNAbsence_Setting.FirstOrDefault().HolidayEntitlement);
            }
            //int HolidayEntitlement = _db.HolidaysNAbsence_Setting != null ? Convert.ToInt32(_db.HolidaysNAbsence_Setting.FirstOrDefault().HolidayEntitlement) : 0;
            return HolidayEntitlement;
        }
        public decimal GetPointByEmployeeId(int EmployeeId)
        {
            return 0;
        }

        public int Weekdays(DateTime dtmStart, DateTime dtmEnd, int offday1, int offday2)
        {
            var off1 = offday1;
            var off2 = offday2;
            if (offday1 > offday2)
            {
                offday1 = off2;
                offday2 = off1;
            }


            // This function includes the start and end date in the count if they fall on a weekday
            int dowStart = ((int)dtmStart.DayOfWeek == 0 ? 7 : (int)dtmStart.DayOfWeek);
            int dowEnd = ((int)dtmEnd.DayOfWeek == 0 ? 7 : (int)dtmEnd.DayOfWeek);
            TimeSpan tSpan = dtmEnd - dtmStart;
            if (dowStart <= dowEnd)
            {
                return (((tSpan.Days / 7) * offday1) + Math.Max((Math.Min((dowEnd + 1), offday2) - dowStart), 0));
            }
            return (((tSpan.Days / 7) * offday1) + Math.Min((dowEnd + offday2) - Math.Min(dowStart, offday2), offday1));
        }

        #endregion

    }
}