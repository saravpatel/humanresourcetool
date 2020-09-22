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
    public class ApproveHeaderController : Controller
    {
        EvolutionEntities _db = new EvolutionEntities();
        TimeSheetApprovalMethod _TimeSheetApprovalMethod = new TimeSheetApprovalMethod();

        // GET: ApproveHeader
        //public ActionResult Index(IList<string> Timesheet)
        //{
        //    List<TimeSheetApprovalViewModel> model = new List<TimeSheetApprovalViewModel>();
        //    var data = _TimeSheetApprovalMethod.getAllTimeSheetPendingApprovalList();
        //    if (data.Count > 0)
        //    {
        //        foreach (var details in data)
        //        {
        //            TimeSheetApprovalViewModel datamodel = new TimeSheetApprovalViewModel();
        //            datamodel.Id = details.Id;
        //            datamodel.Day = details.day;
        //            datamodel.Date = Convert.ToDateTime(details.Date); 
        //            datamodel.Hours =Convert.ToInt32(details.hour);
        //            datamodel.CostCode =Convert.ToInt32(details.CostCode);
        //            datamodel.Project = Convert.ToString(details.Project);
        //            datamodel.Customer = details.Customer;
        //            datamodel.Asset = details.AssetName;
        //            datamodel.Status = details.Status;
        //            model.Add(datamodel);
        //        }
        //    }
        //            return View(model);
        //}

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
                    datamodel.Header = details.Header;
                    datamodel.Name = details.Name;
                    
                    ObjEmp.Add(datamodel);
                }
            }
            //return list as Json    
            return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public JsonResult ScheduleApprove()
        //{
        //    //Creating List    
        //    List<TimeSheetApprovalViewModel> ObjEmp = new List<TimeSheetApprovalViewModel>();
        //    var data = _TimeSheetApprovalMethod.getAllTimeSheetPendingApprovalList();
        //    if (data.Count > 0)
        //    {
        //        foreach (var details in data)
        //        {
        //            TimeSheetApprovalViewModel datamodel = new TimeSheetApprovalViewModel();
        //            datamodel.Id = details.Id;
        //            datamodel.Day = details.day;
        //            datamodel.Date = Convert.ToDateTime(details.Date);
        //            datamodel.Hours = Convert.ToInt32(details.hour);
        //            datamodel.CostCode = Convert.ToInt32(details.CostCode);
        //            datamodel.Project = Convert.ToString(details.Project);
        //            datamodel.Customer = details.Customer;
        //            datamodel.Asset = details.AssetName;
        //            datamodel.Status = details.Status;

        //            ObjEmp.Add(datamodel);
        //        }
        //    }
        //    //return list as Json    
        //    return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public JsonResult TravelApprove()
        //{
        //    //Creating List    
        //    List<TravelApprove> ObjEmp = new List<TravelApprove>();
        //    var data = _TimeSheetApprovalMethod.getAllTravelList();
        //    if (data.Count > 0)
        //    {
        //        foreach (var details in data)
        //        {
        //            TravelApprove datamodel = new TravelApprove();
        //            datamodel.Id = details.Id;
        //            datamodel.Type =details.Type;
        //            datamodel.FromAirport = details.FromPlace;
        //            datamodel.FromCountry = details.FromCountry;
        //            datamodel.FromTown =details.FromCity;
        //            datamodel.ToAirport = details.ToPlace;
        //            datamodel.ToCountry = details.ToCountry;
        //            datamodel.ToTown = details.ToCity;
        //            datamodel.Startdate = Convert.ToDateTime(details.StartDate);
        //            datamodel.Enddate = Convert.ToDateTime(details.EndDate);
        //            datamodel.Duration =Convert.ToDecimal(details.Duration);
        //            datamodel.Customer = details.FirstName;
        //            datamodel.Project = details.Name;
        //            datamodel.CostCode =details.CostCode;

        //            ObjEmp.Add(datamodel);
        //        }
        //    }
        //    //return list as Json    
        //    return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public JsonResult AnualLeaveApprove()
        //{
        //    //Creating List    
        //    List<AnnualLeaveapprove> ObjEmp = new List<AnnualLeaveapprove>();
        //    var data = _TimeSheetApprovalMethod.getAnualLeaveList();
        //    if (data.Count > 0)
        //    {
        //        foreach (var details in data)
        //        {
        //            AnnualLeaveapprove datamodel = new AnnualLeaveapprove();
        //            datamodel.Id = details.Id;
                    
        //            datamodel.StartDate = Convert.ToDateTime(details.StartDate);
        //            datamodel.EndDate = Convert.ToDateTime(details.EndDate);
        //            datamodel.Duration = Convert.ToDecimal(details.Duration);
                    
        //            ObjEmp.Add(datamodel);
        //        }
        //    }
        //    //return list as Json    
        //    return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public JsonResult SickLeaveApprove()
        {
            //Creating List    
            List<SickLeaveapprove> ObjEmp = new List<SickLeaveapprove>();
            var data = _TimeSheetApprovalMethod.getSickLeave();
            if (data.Count > 0)
            {
                foreach (var details in data)
                {
                    SickLeaveapprove datamodel = new SickLeaveapprove();
                    datamodel.Id = details.Id;
                    datamodel.StartDate = Convert.ToDateTime(details.StartDate);
                    datamodel.EndDate = Convert.ToDateTime(details.EndDate);
                    datamodel.Duration = Convert.ToDecimal(details.DurationDays);
                    //Commented for yagnik
                    datamodel.SelfCertificationRequired = details.SelfCertificationFormRequired;
                    datamodel.InterviewRequired = details.BackToWorkInterviewRequired;
                    datamodel.Type = details.Type;
                    datamodel.DoctorConsulted = details.DoctorConsulted;
                    datamodel.IssueAtWork = details.IssueAtWork;
                    ObjEmp.Add(datamodel);
                }
            }
            //return list as Json    
            return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public JsonResult OtherLeaveApprove()
        //{
        //    //Creating List    
        //    List<OtherLeaveapprove> ObjEmp = new List<OtherLeaveapprove>();
        //    var data = _TimeSheetApprovalMethod.getOtherLeave();
        //    if (data.Count > 0)
        //    {
        //        foreach (var details in data)
        //        {
        //            OtherLeaveapprove datamodel = new OtherLeaveapprove();
        //            datamodel.Id = details.Id;
        //            datamodel.StartDate = Convert.ToDateTime(details.StartDate);
        //            datamodel.EndDate = Convert.ToDateTime(details.EndDate);
        //            datamodel.Duration = Convert.ToDecimal(details.Duration);
        //            datamodel.Reason =Convert.ToInt32(details.ReasonForLeaveId);
                    
        //            ObjEmp.Add(datamodel);
        //        }
        //    }
        //    //return list as Json    
        //    return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public JsonResult TrainingRequestApprove()
        //{
        //    //Creating List    
        //    List<TrainingRequest> ObjEmp = new List<TrainingRequest>();
        //    var data = _TimeSheetApprovalMethod.getTrainingRequestlList();
        //    if (data.Count > 0)
        //    {
        //        foreach (var details in data)
        //        {
        //            TrainingRequest datamodel = new TrainingRequest();
        //            //datamodel.Id = details.id;
        //           // datamodel.Days =Convert.ToDecimal(details.);
        //            datamodel.StartDate = Convert.ToDateTime(details.StartDate);
        //            datamodel.EndDate = Convert.ToDateTime(details.EndDate);
        //            var Importance = Convert.ToInt32(details.Importance);
        //            if (Importance == 1)
        //            {
        //                datamodel.Importance = "Mandatory";
        //            }
        //            else
        //            {
        //                datamodel.Importance = "Optional";
        //            }
        //            datamodel.TrainingName =details.Value;
        //            datamodel.Provider =Convert.ToInt32(details.Provider);
        //            datamodel.Cost = Convert.ToInt32(details.Cost);
        //            //datamodel.Status = details.Status;

        //            ObjEmp.Add(datamodel);
        //        }
        //    }
        //    //return list as Json    
        //    return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public JsonResult NewVacancyApprove(string )
        //{
        //    //Creating List    
        //    List<NewVacancy> ObjEmp = new List<NewVacancy>();
        //    var data = _TimeSheetApprovalMethod.getNewVacancy();
        //    if (data.Count > 0)
        //    {
        //        foreach (var details in data)
        //        {
        //            NewVacancy datamodel = new NewVacancy();
        //            //datamodel.Id = details.id;
        //            // datamodel.Days =Convert.ToDecimal(details.);
        //            datamodel.VacancyName = details.Title;
        //            datamodel.ClosingDate = Convert.ToDateTime(details.ClosingDate);
        //            datamodel.RecruitmentProcess = details.RecruitmentProcesses;
        //            datamodel.Salary =Convert.ToDecimal(details.Salary);
        //            datamodel.Location = details.Location;
        //            datamodel.Pool = details.Pool;
        //            datamodel.Business = details.Business;
        //            datamodel.Division = details.Division;
        //            datamodel.Function = details.Functions;
                  
        //            ObjEmp.Add(datamodel);
        //        }
        //    }
        //    //return list as Json    
        //    return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public JsonResult Mat_PatLeaveRequest()
        {
            //Creating List    
            List<Mat_PatLeave> ObjEmp = new List<Mat_PatLeave>();
            var data = _TimeSheetApprovalMethod.getMat_PatLeave();
            if (data.Count > 0)
            {
                foreach (var details in data)
                {
                    Mat_PatLeave datamodel = new Mat_PatLeave();
                    //datamodel.Id = details.id;
                    // datamodel.Days =Convert.ToDecimal(details.);
                    
                    datamodel.DueDate = Convert.ToDateTime(details.DueDate);
                    //Commented for yagnik
                    datamodel.Link = details.Link;

                    ObjEmp.Add(datamodel);
                }
            }
            //return list as Json    
            return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public JsonResult UpliftApprove()
        //{
        //    //Creating List    
        //    List<Uplift> ObjEmp = new List<Uplift>();
        //    var data = _TimeSheetApprovalMethod.getUpliftApprove();
        //    if (data.Count > 0)
        //    {
        //        foreach (var details in data)
        //        {
        //            Uplift datamodel = new Uplift();
        //            datamodel.Id = details.Id;
        //            datamodel.Day =details.day;
        //            datamodel.Date = Convert.ToDateTime(details.Date);
        //            datamodel.UpliftPosition = details.UpliftPosition;
        //            datamodel.Hours = Convert.ToInt32(details.Hour);
        //            datamodel.Project = details.Project;
        //            datamodel.Customer = details.Customer;
        //            datamodel.ChangeInCustomerRate = details.ChangeInCustomerRate;
        //            datamodel.ChangeInWorkerRate = details.ChangeInWorkerRate;

        //            ObjEmp.Add(datamodel);
        //        }
        //    }
        //    //return list as Json    
        //    return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        //}

        // GET: ApproveHeader/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ApproveHeader/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApproveHeader/Create
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

        // GET: ApproveHeader/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ApproveHeader/Edit/5
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

        // GET: ApproveHeader/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ApproveHeader/Delete/5
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
