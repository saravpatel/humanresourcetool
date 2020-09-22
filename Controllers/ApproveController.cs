using HRTool.CommanMethods;
using HRTool.CommanMethods.Approval;
using HRTool.DataModel;
using HRTool.Models.Approval;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRTool.CommanMethods.Me;
using HRTool.Models.Me;
using HRTool.CommanMethods.Resources;
namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class ApproveController : Controller
    {
        EvolutionEntities _db = new EvolutionEntities();
        TimeSheetApprovalMethod _TimeSheetApprovalMethod = new TimeSheetApprovalMethod();
        MeEmployeeInformation _EmployeeProfileMethod = new MeEmployeeInformation();
        EmployeeMethod _employeeMethod = new EmployeeMethod();

        // GET: TimeSheetApproval
        public ActionResult Index(IList<string> Approval)
        {
            List<AllApproveRequestList> ObjEmp = new List<AllApproveRequestList>();
            var data = _TimeSheetApprovalMethod.getApproveList(SessionProxy.UserId);
            if (data.Count > 0 && data != null)
            {
                foreach (var details in data)
                {
                    AllApproveRequestList datamodel = new AllApproveRequestList();
                    //datamodel.id = details.ID;
                    datamodel.EmployeeId = details.EmployeeID;
                    datamodel.Header = details.Header;
                    datamodel.Name = details.Name;
                    datamodel.Title = details.Title;
                    datamodel.Gender = Convert.ToBoolean(details.Gender);
                    ObjEmp.Add(datamodel);

                }
            }
            return View(ObjEmp);
        }

        public ActionResult TimeSheetApprove(int EmpID)
        {
            List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            var data = _TimeSheetApprovalMethod.getAllTimeSheetPendingApprovalList(EmpID);

            if (data.Count > 0)
            {
                foreach (var details in data)
                {
                    TimeSheetApprovalViewModel datamodel = new TimeSheetApprovalViewModel();
                    datamodel.Id = details.Id;
                    datamodel.EmployeeId = details.EmployeeId;
                    datamodel.Day = details.day;
                    datamodel.Date = Convert.ToString(details.Date);
                    // datamodel.Hours = Convert.ToInt32(details.hour);
                    datamodel.CostCode = details.CostCode;
                    datamodel.Project = Convert.ToString(details.Project);
                    datamodel.Customer = details.Customer;
                    datamodel.Asset = details.AssetName;
                    datamodel.Status = details.Status;
                    datamodel.Name = details.Name;
                    datamodel.InTime = details.InTime;
                    datamodel.EndTime = details.EndTime;
                    datamodel.Hours = details.Hours;
                    TempData["Name"] = details.Name;
                    TempData["Id"] = details.EmployeeId;
                    datamodel.FileName = details.FileName;
                    ObjEmp.Add(datamodel);
                }
            }
            return PartialView("_partialTimesheetApproval", ObjEmp);
           // return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SickLeaveApprove(int EmpID)
        {
            
            List<SickLeaveapprove> ObjEmp = new List<SickLeaveapprove>();
            var data = _TimeSheetApprovalMethod.getAllSickLeaveApprovalList(EmpID);
            var tottalSick = _TimeSheetApprovalMethod.countTottalSickDaysIntheYear(EmpID);
            var brandFactorPoint = _employeeMethod.GetBradFordFactorCount(EmpID);
            if (data.Count > 0)
            {
                foreach (var details in data)
                {
                    SickLeaveapprove datamodal = new SickLeaveapprove();
                    datamodal.Id = details.Id;
                    datamodal.StartDate = Convert.ToDateTime(details.StartDate);
                    datamodal.EndDate = Convert.ToDateTime(details.EndDate);
                    datamodal.Duration = Convert.ToDecimal(details.DurationDays);
                    datamodal.Type = details.Type;
                    datamodal.Status = details.ApprovalStatus;
                    datamodal.IssueAtWork = details.IssueAtWork;
                    datamodal.DoctorConsulted = details.DoctorConsulted;
                    datamodal.InterviewRequired = details.BackToWorkInterviewRequired;
                    datamodal.SelfCertificationRequired = details.SelfCertificationFormRequired;
                    datamodal.FileName = details.FileName;
                    TempData["SickEmpName"] = details.Name;
                    TempData["SickEmpId"] = details.EmployeeId;
                    TempData["TotalSick"] = tottalSick;
                    TempData["Brandforspoint"] = brandFactorPoint;
                    ObjEmp.Add(datamodal);
                }
            }
            return PartialView("_partialSickLeaveApproval", ObjEmp);
        }


        [HttpPost]
        public JsonResult UpdateTimeSheetApproval(string ID)
        {
            //List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            _TimeSheetApprovalMethod.UpdateTimeSheetApprovalStatus(ID);
            return Json("success", JsonRequestBehavior.AllowGet);
            //return "success";
        }

        [HttpPost]
        public JsonResult UpdateTimeSheetReject(string ID)
        {
            //List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            _TimeSheetApprovalMethod.UpdateTimeSheetRejectStatus(ID);
            return Json("success", JsonRequestBehavior.AllowGet);
            //return "success";
        }

        public ActionResult ScheduleApproval(int EmpID)
        {
            //Creating List    
            List<ScheduleApproval> ObjEmp = new List<ScheduleApproval>();
            var data = _TimeSheetApprovalMethod.getScheduleApprovalList(EmpID);
            if (data.Count > 0)
            {
                foreach (var details in data)
                {
                    ScheduleApproval datamodel = new ScheduleApproval();
                    datamodel.Id = details.Id;
                    datamodel.EmployeeId = details.EmployeeId;
                    datamodel.StartDate =details.StartDate;
                    datamodel.EndDate = details.EndDate;
                    datamodel.Duration = Convert.ToDecimal(details.duration);
                    datamodel.Name = details.Name;
                    datamodel.Hours = details.Hours;

                    datamodel.Project = Convert.ToString(details.Project);
                    datamodel.Customer = details.Customer;
                    datamodel.Asset = details.AssetName;
                    datamodel.Status = details.Status;
                    TempData["ScheWName"] = details.Name;
                    TempData["ScheWId"] = details.EmployeeId;
                    ObjEmp.Add(datamodel);
                }
            }
            //return list as Json    
            return PartialView("_partialSchedulingApproval",ObjEmp);
        }

        [HttpPost]
        public JsonResult UpdateScheduleApprova(string ID)
        {
            //List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            _TimeSheetApprovalMethod.UpdateScheduleApprovalStatus(ID);
            return Json("sucess", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateScheduleReject(string ID)
        {
            //List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            _TimeSheetApprovalMethod.UpdateScheduleRejectStatus(ID);
            return Json("sucess", JsonRequestBehavior.AllowGet);
        }

        public ActionResult TravelApprove(int EmpID)
        {
            //Creating List    
            List<TravelApprove> ObjEmp = new List<TravelApprove>();
            var data = _TimeSheetApprovalMethod.getAllTravelList(EmpID);
            if (data.Count > 0)
            {
                foreach (var details in data)
                {
                    TravelApprove datamodel = new TravelApprove();
                    datamodel.Id = details.Id;
                    datamodel.EmployeeId = details.EmployeeId;
                    datamodel.Name = details.Name;
                    datamodel.Type = details.Type;
                    datamodel.FromAirport = details.FromPlace;
                    datamodel.FromCountry = details.FromCountry;
                    datamodel.FromTown = details.FromCity;
                    datamodel.ToAirport = details.ToPlace;
                    datamodel.ToCountry = details.ToCountry;
                    datamodel.ToTown = details.ToCity;
                    datamodel.Startdate = details.StartDate;
                    datamodel.Enddate = details.EndDate;
                    datamodel.Duration = Convert.ToDecimal(details.Duration);
                    datamodel.Hour = details.Hour;
                    datamodel.Customer = details.CustomerName;
                    datamodel.Project = details.ProjectName;
                    datamodel.CostCode = details.CostCode;
                    TempData["TravWName"] = details.Name;
                    TempData["TraveWId"] = details.EmployeeId;
                    datamodel.FileName = details.FileName;
                    ObjEmp.Add(datamodel);
                }
            }
            //return list as Json    
            return PartialView("_partialTravelApproval", ObjEmp);
        }

        public ActionResult Mat_PatApprove(int EmpID)
        {
            //Creating List    
            List<Mat_PatLeave> ObjEmp = new List<Mat_PatLeave>();
            var data = _TimeSheetApprovalMethod.getAllMat_PatList(EmpID);
            int employmentDay = _TimeSheetApprovalMethod.getTotalWorkingDaysOfEmployee(EmpID);
            if (data.Count > 0)
            {
                foreach (var details in data)
                {
                    Mat_PatLeave datamodel = new Mat_PatLeave();
                    datamodel.Id = details.Id;
                    datamodel.EmployeeId = details.EmployeeId;
                    datamodel.Name = details.Name;                                                                                    
                    TempData["MatPatWName"] = details.Name;
                    TempData["MatPatWId"] = details.EmployeeId;
                    TempData["MatLengthOfEmployement"] = employmentDay;
                    datamodel.FileName = details.FileName;
                    ObjEmp.Add(datamodel);
                }
            }
            return PartialView("_partialMatPatApproval", ObjEmp);
        }

        public JsonResult UpdateMatPatApprova(string ID)
        {
            _TimeSheetApprovalMethod.UpdateMat_PatApproveStatus(ID);
            return Json("sucess", JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateMatPatReject(string ID)
        {
            _TimeSheetApprovalMethod.UpdateMat_PatRejectStatus(ID);
            return Json("sucess", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateTravelApprova(string ID)
        {
            //List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            _TimeSheetApprovalMethod.UpdateTravelApprovalStatus(ID);
            return Json("sucess", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateTravelReject(string ID)
        {
            //List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            _TimeSheetApprovalMethod.UpdateTravelRejectStatus(ID);
            return Json("sucess", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AnualLeaveApprove(int EmpID)
        {
            //Creating List    
            List<AnnualLeaveapprove> ObjEmp = new List<AnnualLeaveapprove>();
            var data = _TimeSheetApprovalMethod.getAnualLeaveList(EmpID);
            int anuualLeave = _EmployeeProfileMethod.getEmployeApproveAnnualLeave(EmpID);
            int holidayInthisYear = _EmployeeProfileMethod.getTotalEmpoyeeHolidayThisYear(EmpID);
            int totalHolidayRemain = holidayInthisYear- anuualLeave;
            if(totalHolidayRemain>0)
            {
                totalHolidayRemain = totalHolidayRemain;
            }
            else
            {
                totalHolidayRemain = 0;
            }
            if (data.Count > 0)
            {
                foreach (var details in data)
                {
                    AnnualLeaveapprove datamodel = new AnnualLeaveapprove();
                    datamodel.Id = details.Id;
                    datamodel.EmployeeId = details.EmployeeId;
                    datamodel.Name = details.Name;
                    datamodel.StartDate = Convert.ToString(details.StartDate);
                    datamodel.EndDate = Convert.ToString(details.EndDate);
                    datamodel.Duration = Convert.ToDecimal(details.Duration);
                    datamodel.TotalHolidays = _EmployeeProfileMethod.getTotalEmpoyeeHolidayThisYear(EmpID); ;
                    //datamodel.HolidaysTaken = Convert.ToDecimal(details.HolidayTaken);
                    datamodel.HolidaysTaken = countAbsenceDay(EmpID);
                    TempData["AnulWName"] = details.Name;
                    TempData["AnulWId"] = details.EmployeeId;
                    TempData["AnulTtlholiday"] = Convert.ToDecimal(details.TotalHoliday);
                    TempData["Anulholitaken"] = Convert.ToDecimal(details.HolidayTaken);
                    TempData["HolidayRemain"] = totalHolidayRemain;
                    TempData["TotalHolidays"] = datamodel.TotalHolidays;
                    TempData["HolidaysTaken"] = datamodel.HolidaysTaken;
                    ObjEmp.Add(datamodel);
                }
            }
            //return list as Json    
            return PartialView("_partialAnualleaveapproval", ObjEmp);
        }
        public Double countAbsenceDay(int EmployeeId)
        {
            int count = 0;
            int workCount = 0;
            var item = _EmployeeProfileMethod.getEmplyeeWorkDate(EmployeeId);
            var workDay = _EmployeeProfileMethod.getEmployeeWrokDay(EmployeeId);
            Double absenceDay = 0;
            CountAbsenceDay model = new CountAbsenceDay();
            DateTime startDate;
            int year = 0;
            int month = 0;
            int day = 0;
            Double diffOfDate = 0;
            if (item.Count > 0)
            {                      
                foreach (var data in item)
                {
                    startDate = Convert.ToDateTime(data.StartDate);
                    year = data.StartDate.Value.Year;
                    month = data.StartDate.Value.Month;
                    day = data.StartDate.Value.Day;
                    count = countSunday(year, month, day);
                    diffOfDate = DateTime.Today.Subtract(startDate).TotalDays + 1;
                }
               }
            foreach (var data in workDay)
            {
                model.workDay = Convert.ToDateTime(data.Date);
                workCount = workCount + countWorkDay(model.workDay.Day);
            }
            absenceDay = diffOfDate - Convert.ToDouble(count + workCount);
            return absenceDay;
        }
        public int countWorkDay(int StartDay)
        {
            int count = 0;
            yearInfo yearModal = new yearInfo();
            var yearData = _EmployeeProfileMethod.getEmployeeYear(DateTime.Now.Year);
            foreach (var data in yearData)
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

        public int countSunday(int year, int month, int StartDay)
        {
            DateTime EndOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            int day = EndOfMonth.Day;
            int count = 0;
            for (int i = 0; i <= day; i++)
            {
                if (i >= StartDay)
                {
                    DateTime d = new DateTime(year, month, i);
                    if (d.DayOfWeek == DayOfWeek.Sunday || d.DayOfWeek == DayOfWeek.Saturday)
                    {
                        count++;
                    }
                }
            }
            return count;
        }



        [HttpPost]
        public JsonResult UpdateAnnualLeaveApprova(string ID)
        {
            //List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            _TimeSheetApprovalMethod.UpdateAnualLeaveApprovalStatus(ID);
            return Json("sucess", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateAnnualLeaveReject(string ID)
        {
            //List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            _TimeSheetApprovalMethod.UpdateAnualLeaveRejectStatus(ID);
            return Json("sucess", JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpliftApprove(int EmpID)
        {
            //Creating List    
            List<Uplift> ObjEmp = new List<Uplift>();
            var data = _TimeSheetApprovalMethod.getUpliftApprove(EmpID);
            if (data.Count > 0)
            {
                foreach (var details in data)
                {
                    Uplift datamodel = new Uplift();
                    datamodel.Id = details.DetailId;
                    datamodel.EmployeeId = details.EmployeeId;
                    datamodel.Name = details.Name;
                    datamodel.Day = details.day;
                    datamodel.Date = details.Date;
                    datamodel.UpliftPosition = Convert.ToInt32(details.UpliftPostionId);
                    datamodel.Hours = details.Hours;
                    // datamodel.EndTimeHr = details.EndTime;
                    datamodel.Project = details.Project;
                    datamodel.Customer = details.Customer;
                    datamodel.FileName = details.FileName;
                    datamodel.ChangeInCustomerRate = Convert.ToDecimal(details.CustomerRate);
                    datamodel.ChangeInWorkerRate = Convert.ToDecimal(details.WorkerRate);
                    datamodel.totalUpliftHr = CountTotalUpliftHrInYear(EmpID);
                    TempData["UpliftWName"] = details.Name;
                    TempData["UpliftWId"] = details.EmployeeId;
                    ObjEmp.Add(datamodel);
                }
            }
            return PartialView("_partialUpliftApproval", ObjEmp);
        }
        public TimeSpan CountTotalUpliftHrInYear(int EmpID)
        {
            TimeSpan totalHr=new TimeSpan(0, 0, 0);
            double total = 0;
            string currentYear = Convert.ToString(_TimeSheetApprovalMethod.getCurruentYear());
            Uplift ObjEmp = new Uplift();
            var data = _TimeSheetApprovalMethod.getUpliftApprove(EmpID);
            foreach (var item in data)
            {
                string str = item.Date.Split('-')[0];
                TimeSpan t = TimeSpan.Parse(item.Hours);
                //int hr = Convert.ToInt16(item.Hours.Split(':')[0]);
                //int min = Convert.ToInt16(item.Hours.Split(':')[1]);
                //int sumHours = 0;
                //int sumMinutes = 0;
                if (currentYear == str)
                {
                    //totalHr = totalHr + Convert.ToDouble(item.Hours);
                    //hr = sumHours + (sumMinutes / 60);
                    //min =sumMinutes;
                    totalHr = totalHr.Add(t);
                }
            }
            
            return totalHr;
        }

        public ActionResult NewVacancyApprove(int EmpID)
        {
            //Creating List    
            List<NewVacancy> ObjEmp = new List<NewVacancy>();
            var data = _TimeSheetApprovalMethod.getNewVacancy(EmpID);
            if (data.Count > 0)
            {
                foreach (var details in data)
                {
                    NewVacancy datamodel = new NewVacancy();
                    datamodel.Id = details.Id;
                    datamodel.UserID = details.UserIDLastModifiedBy;
                    datamodel.Name = details.Name;
                    datamodel.Title = details.Title;
                    datamodel.ClosingDate = details.ClosingDate;
                    datamodel.RecruitmentProcess = details.RecruitmentProcesses;
                    datamodel.Salary = Convert.ToDecimal(details.Salary);
                    datamodel.Location = details.Location;
                    datamodel.Business = details.Business;
                    datamodel.Division = details.Division;
                    datamodel.Pool = details.Pool;
                    datamodel.Function = details.Functions;
                    datamodel.FileName = details.FileName;
                    datamodel.totalVacancy = _TimeSheetApprovalMethod.getTotalVacancy();
                    TempData["VacnyWName"] = details.Name;
                    TempData["VacnyWId"] = details.UserIDLastModifiedBy;
                    ObjEmp.Add(datamodel);
                }
            }
            //return list as Json    
            return PartialView("_partialNewVacancyApproval", ObjEmp);
        }
        public ActionResult TrainingRequestApprove(int EmpID)
        {
            List<TrainingRequest> ObjEmp = new List<TrainingRequest>();
            var data = _TimeSheetApprovalMethod.getTrainingRequestlList(EmpID);
            if (data.Count > 0)
            {
                foreach (var details in data)
                {
                    TrainingRequest datamodel = new TrainingRequest();
                    datamodel.Id = details.Id;
                    datamodel.EmployeeId = details.EmployeeId;
                    datamodel.Days = Convert.ToDecimal(details.Days);
                    datamodel.StartDate = Convert.ToString(details.StartDate);
                    datamodel.EndDate = Convert.ToString(details.EndDate);
                    var Importance = Convert.ToInt32(details.Importance);
                    if (Importance == 1)
                    {
                        datamodel.Importance = "Mandatory";
                    }
                    else
                    {
                        datamodel.Importance = "Optional";
                    }
                    datamodel.TrainingName = details.Value;
                    datamodel.Provider = details.Provider;
                    datamodel.Cost = Convert.ToInt32(details.Cost);
                    datamodel.TotalTrainingDaysApproved = calculateTotalTraining(EmpID);
                    datamodel.Name = details.Name;
                    datamodel.FileName = details.FileName;
                    TempData["TraingWName"] = details.Name;
                    TempData["TraingWId"] = details.EmployeeId;
                    ObjEmp.Add(datamodel);
                }
            }
            return PartialView("_partialTrainingRequestApproval", ObjEmp);
        }
        public double calculateTotalTraining(int EmployeeID)
        {
            int currentYear = _TimeSheetApprovalMethod.getCurruentYear();
            OtherLeaveYear model = new OtherLeaveYear();
            var data = _TimeSheetApprovalMethod.getTotalEmployeeTraning(EmployeeID);
            int count = 0;
            double DateDiff = 0;
            double TotalDiff = 0;
            foreach (var item in data)
            {
                model.startDate = Convert.ToDateTime(item.StartDate);
                model.endDate = Convert.ToDateTime(item.EndDate);
                if (currentYear == item.StartDate.Value.Year)
                {
                    if (model.startDate == model.endDate)
                    {
                        count = 1;
                        DateDiff = (model.endDate - model.startDate).TotalDays;
                        TotalDiff = TotalDiff + DateDiff + count;
                    }
                    else
                    {
                        DateDiff = (model.endDate - model.startDate).TotalDays;
                        TotalDiff = TotalDiff + DateDiff;
                    }
                }
            }
            return TotalDiff;
        }
        public ActionResult OtherLeaveApprove(int EmpID)
        {
            //Creating List    
            List<OtherLeaveapprove> ObjEmp = new List<OtherLeaveapprove>();
            var data = _TimeSheetApprovalMethod.getOtherLeave(EmpID);
            if (data.Count > 0)
            {
                foreach (var details in data)
                {
                    OtherLeaveapprove datamodel = new OtherLeaveapprove();
                    datamodel.Id = details.Id;
                    datamodel.EmployeeId = details.EmployeeId;
                    datamodel.StartDate = details.StartDate;
                    datamodel.EndDate = details.EndDate;
                    datamodel.Duration = Convert.ToDecimal(details.Duration);
                    datamodel.Reason = details.Reason;
                    datamodel.Name = details.Name;
                    TempData["OthWName"] = details.Name;
                    TempData["OthWId"] = details.EmployeeId;
                    datamodel.FileName = details.FileName;
                    datamodel.totalOtherLeave = calculateOtherLeave(EmpID);
                    ObjEmp.Add(datamodel);
                }
            }
            //return list as Json    
            return PartialView("_partialOtherLeaveApproval", ObjEmp);
        }
        public double calculateOtherLeave(int EmployeeID)
        {
            int currentYear = _TimeSheetApprovalMethod.getCurruentYear();
            OtherLeaveYear model = new OtherLeaveYear();
            var data = _TimeSheetApprovalMethod.getTotalLeave(EmployeeID);
            int count=0;
            double DateDiff=0;
            double TotalDiff = 0;
            foreach(var item in data)
            {
                model.startDate = Convert.ToDateTime(item.StartDate);
                model.endDate = Convert.ToDateTime(item.EndDate);
                if(currentYear== item.StartDate.Value.Year)
                {
                    if (model.startDate == model.endDate)
                    {
                        count = 1;
                        DateDiff = (model.endDate - model.startDate).TotalDays;
                        TotalDiff = TotalDiff + DateDiff + count;
                    }
                    else {
                        DateDiff = (model.endDate - model.startDate).TotalDays;
                        TotalDiff = TotalDiff + DateDiff;
                    }
                }
            }
            return TotalDiff;
        }
        [HttpPost]
        public JsonResult UpdateOtherLeaveApprova(string ID)
        {
            //List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            _TimeSheetApprovalMethod.UpdateOtherLeaveApprovalStatus(ID);
            return Json("sucess", JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateSickLeaveApproval(string ID)
        {
            _TimeSheetApprovalMethod.UpdateSickLeaveApprovalStatus(ID);
            return Json("sucess", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateSickLeaveReject(string ID)
        {
            //List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            _TimeSheetApprovalMethod.UpdateSickRejectStatus(ID);
            return Json("sucess", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateOtherLeaveReject(string ID,int EmpId)
        {
            //List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            _TimeSheetApprovalMethod.UpdateOtherLeaveRejectStatus(ID);
            // return Json("sucess", JsonRequestBehavior.AllowGet);
            List<OtherLeaveapprove> ObjEmp = new List<OtherLeaveapprove>();
            var data = _TimeSheetApprovalMethod.getOtherLeave(EmpId);
            if (data.Count > 0)
            {
                foreach (var details in data)
                {
                    OtherLeaveapprove datamodel = new OtherLeaveapprove();
                    datamodel.Id = details.Id;
                    datamodel.EmployeeId = details.EmployeeId;
                    datamodel.StartDate = details.StartDate;
                    datamodel.EndDate = details.EndDate;
                    datamodel.Duration = Convert.ToDecimal(details.Duration);
                    datamodel.Reason = details.Reason;
                    datamodel.Name = details.Name;
                    TempData["OthWName"] = details.Name;
                    TempData["OthWId"] = details.EmployeeId;
                    datamodel.FileName = details.FileName;
                    datamodel.totalOtherLeave = calculateOtherLeave(EmpId);
                    ObjEmp.Add(datamodel);
                }
            }
            //return list as Json    
            return PartialView("_partialOtherLeaveApproval", ObjEmp);
        }

        [HttpPost]
        public JsonResult UpdateUpliftApprova(string ID)
        {
            //List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            _TimeSheetApprovalMethod.UpdateUpliftApprovalStatus(ID);
            return Json("sucess", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateUpliftReject(string ID)
        {
            //List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            _TimeSheetApprovalMethod.UpdateUpliftRejectStatus(ID);
            return Json("sucess", JsonRequestBehavior.AllowGet);          
        }

        [HttpPost]
        public JsonResult UpdateNewVacancyApprova(string ID)
        {
            //List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            _TimeSheetApprovalMethod.UpdateNewVacancyApprovalStatus(ID);
            return Json("sucess", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateNewVacancyReject(string ID)
        {
            //List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            _TimeSheetApprovalMethod.UpdateNewVacancyRejectStatus(ID);
            return Json("sucess", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateTrainingApprova(string ID)
        {
            //List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            _TimeSheetApprovalMethod.UpdateTrainingApprovalStatus(ID);
            return Json("sucess", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateTrainingReject(string ID)
        {
            //List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            _TimeSheetApprovalMethod.UpdateTrainingRejectStatus(ID);
            return Json("sucess", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Download(string FilePath, string Text)
        {
            try
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Upload/Resources/Planner/" + Text + "/" + FilePath));
                string fileName = FilePath;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception e)
            {
                return View("Index");
             }
        }
    }
}