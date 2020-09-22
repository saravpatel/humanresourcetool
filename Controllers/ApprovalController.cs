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

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class ApprovalController : Controller
    {
        EvolutionEntities _db = new EvolutionEntities();
        TimeSheetApprovalMethod _TimeSheetApprovalMethod = new TimeSheetApprovalMethod();
                                                                                                                                           
        // GET: Approval

        public ActionResult Index(IList<string> Approval)
        {
            List<AllApproveRequestList> ObjEmp = new List<AllApproveRequestList>();
            var data = _TimeSheetApprovalMethod.getApproveList(SessionProxy.UserId);
            if (data.Count > 0)
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

        [HttpGet]
        public JsonResult ApproveRequestList()
        {
            //Creating List    
            List<AllApproveRequestList> ObjEmp = new List<AllApproveRequestList>();
            var data = _TimeSheetApprovalMethod.getApproveList(SessionProxy.UserId);
            if (data.Count > 0)
            {
                foreach (var details in data)
                {
                    AllApproveRequestList datamodel = new AllApproveRequestList();
                    // datamodel.id = details.ID;
                    datamodel.EmployeeId = details.EmployeeID;
                    datamodel.Header = details.Header;
                    datamodel.Name = details.Name;
                    datamodel.Title = details.Title;
                    datamodel.Gender =Convert.ToBoolean(details.Gender);
                    ObjEmp.Add(datamodel);
                }
            }
            //return list as Json    
            return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult TimeSheetApprove(int EmpID)
        {
            //Creating List    
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
                    datamodel.Date =details.Date;
                    // datamodel.Hours = Convert.ToInt32(details.hour);
                    datamodel.CostCode =details.CostCode;
                    datamodel.Project = details.Project;
                    datamodel.Customer = details.Customer;
                    datamodel.Asset = details.AssetName;
                    datamodel.Status = details.Status;
                    datamodel.Name = details.Name;
                    datamodel.InTime = details.InTime;
                    datamodel.EndTime = details.EndTime;
                    ObjEmp.Add(datamodel);
                }
            }
            //return list as Json    
            return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult TimeSheetApprove1(int EmpID)
        {
            //Creating List    
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
                    datamodel.Date = Convert.ToString(details.Date);                    // datamodel.Hours = Convert.ToInt32(details.hour);
                    datamodel.CostCode = details.CostCode;
                    datamodel.Project = Convert.ToString(details.Project);
                    datamodel.Customer = details.Customer;
                    datamodel.Asset = details.AssetName;
                    datamodel.Status = details.Status;
                    datamodel.Name = details.Name;
                    datamodel.InTime = details.InTime;
                    datamodel.EndTime = details.EndTime;
                    ObjEmp.Add(datamodel);
                }
            }
            //return list as Json    
            return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ProjectTimeSheetApprove(int EmpID)
        {
            //Creating List    
            List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            var data = _TimeSheetApprovalMethod.getProjectTimeSheetPendingApprovalList(EmpID);
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
                    ObjEmp.Add(datamodel);
                }
            }
            //return list as Json    
            return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ScheduleApproval(int EmpID)
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
                    datamodel.StartDate = details.StartDate;
                    datamodel.EndDate = details.EndDate;
                    datamodel.Duration = Convert.ToDecimal(details.duration);
                    datamodel.Name = details.Name;
                    datamodel.Hours = details.Hours;
                    
                    datamodel.Project = Convert.ToString(details.Project);
                    datamodel.Customer = details.Customer;
                    datamodel.Asset = details.AssetName;
                    datamodel.Status = details.Status;

                    ObjEmp.Add(datamodel);
                }
            }
            //return list as Json    
            return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult TravelApprove(int EmpID)
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

                    ObjEmp.Add(datamodel);
                }
            }
            //return list as Json    
            return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AnualLeaveApprove(int EmpID)
        {
            //Creating List    
            List<AnnualLeaveapprove> ObjEmp = new List<AnnualLeaveapprove>();
            var data = _TimeSheetApprovalMethod.getAnualLeaveList(EmpID);
            if (data.Count > 0)
            {
                foreach (var details in data)
                {
                    AnnualLeaveapprove datamodel = new AnnualLeaveapprove();
                    datamodel.Id = details.Id;
                    datamodel.EmployeeId = details.EmployeeId;
                    datamodel.Name = details.Name;
                    datamodel.StartDate = Convert.ToString( details.StartDate);
                    datamodel.EndDate =Convert.ToString(details.EndDate);
                    datamodel.Duration = Convert.ToDecimal(details.Duration);
                    datamodel.TotalHolidays = Convert.ToDecimal(details.TotalHoliday);
                    datamodel.HolidaysTaken = Convert.ToDouble(details.HolidayTaken);

                    ObjEmp.Add(datamodel);
                }
            }
            //return list as Json    
            return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult OtherLeaveApprove(int EmpID)
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
                    datamodel.EndDate =details.EndDate;
                    datamodel.Duration = Convert.ToDecimal(details.Duration);
                    datamodel.Reason =details.Reason;
                    datamodel.Name = details.Name;
                    ObjEmp.Add(datamodel);
                }
            }
            //return list as Json    
            return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult TrainingRequestApprove(int EmpID)
        {
            //Creating List    
            List<TrainingRequest> ObjEmp = new List<TrainingRequest>();
            var data = _TimeSheetApprovalMethod.getTrainingRequestlList(EmpID);
            if (data.Count > 0)
            {
                foreach (var details in data)
                {
                    TrainingRequest datamodel = new TrainingRequest();
                    datamodel.Id = details.Id;
                    datamodel.EmployeeId = details.EmployeeId;
                    datamodel.Days =Convert.ToDecimal(details.Days);
                    datamodel.StartDate = Convert.ToString( details.StartDate);
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
                    datamodel.TotalTrainingDaysApproved = Convert.ToDouble(details.TotalDaysApproved);
                    datamodel.Name = details.Name;

                    ObjEmp.Add(datamodel);
                }
            }
            //return list as Json    
            return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult UpliftApprove(int EmpID)
        {
            //Creating List    
            List<Uplift> ObjEmp = new List<Uplift>();
            var data = _TimeSheetApprovalMethod.getUpliftApprove(EmpID);
            if (data.Count > 0)
            {
                foreach (var details in data)
                {
                    Uplift datamodel = new Uplift();
                    datamodel.Id = details.Id;
                    datamodel.EmployeeId = details.EmployeeId;
                    datamodel.Name = details.Name;
                    datamodel.Day = details.day;
                    datamodel.Date = details.Date;
                    datamodel.UpliftPosition =Convert.ToInt32(details.UpliftPostionId);
                    datamodel.Hours = details.Hours;
                   // datamodel.EndTimeHr = details.EndTime;
                    datamodel.Project = details.Project;
                    datamodel.Customer = details.Customer;
                    datamodel.ChangeInCustomerRate =Convert.ToDecimal(details.CustomerRate);
                    datamodel.ChangeInWorkerRate =Convert.ToDecimal(details.WorkerRate);

                    ObjEmp.Add(datamodel);
                }
            }
            //return list as Json    
            return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult NewVacancyApprove(int EmpID)
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

                    ObjEmp.Add(datamodel);
                }
            }
            //return list as Json    
            return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string UpdateTimeSheetApproval(string ID)
        {
            //List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            _TimeSheetApprovalMethod.UpdateTimeSheetApprovalStatus(ID);
            return "success";
        }

        [HttpPost]
        public string UpdateTimeSheetReject(string ID)
        {
            //List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            _TimeSheetApprovalMethod.UpdateTimeSheetRejectStatus(ID);
            //return Json("success", JsonRequestBehavior.AllowGet);
            return "success";
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

        [HttpPost]
        public JsonResult UpdateOtherLeaveApprova(string ID)
        {
            //List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            _TimeSheetApprovalMethod.UpdateOtherLeaveApprovalStatus(ID);
            return Json("sucess", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateOtherLeaveReject(string ID)
        {
            //List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            _TimeSheetApprovalMethod.UpdateOtherLeaveRejectStatus(ID);
            return Json("sucess", JsonRequestBehavior.AllowGet);
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
        public JsonResult UpdateProjectTimesheetApprove(string ID)
        {
            //List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            _TimeSheetApprovalMethod.UpdateProjectTimesheetApprovalStatus(ID);
            return Json("sucess", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateProjectTimesheetReject(string ID)
        {
            //List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
            _TimeSheetApprovalMethod.UpdateProjectTimesheetRejectStatus(ID);
            return Json("sucess", JsonRequestBehavior.AllowGet);
        }

        // GET: Approval/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Approval/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Approval/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Approval/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Approval/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Approval/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Approval/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
