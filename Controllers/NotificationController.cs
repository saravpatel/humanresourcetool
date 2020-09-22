using HRTool.DataModel;
using HRTool.CommanMethods.Notification;
using HRTool.Models.Notification;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRTool.Models.Approval;
using HRTool.CommanMethods.Approval;
using System.Text;
using Microsoft.AspNet.Identity;
using HRTool.CommanMethods;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class NotificationController : Controller
    {
        EvolutionEntities _db = new EvolutionEntities();
        NotificationMethod _NotificationMethod = new NotificationMethod();
        TimeSheetApprovalMethod _TimeSheetApprovalMethod = new TimeSheetApprovalMethod();
        // GET: Notification

        public ActionResult Index()
        {
            List<AllNotificationList> ObjEmp = new List<AllNotificationList>();
            var LoginID = SessionProxy.UserId;
            var data = _NotificationMethod.geNotificationList(SessionProxy.UserId);
            //StringBuilder str = new StringBuilder();
            if (data.Count > 0)
            {
                foreach (var details in data)
                {
                    AllNotificationList datamodel = new AllNotificationList();
                    datamodel.id = Convert.ToInt64(details.Id);
                    datamodel.EmployeeId = details.EmployeeID;
                    datamodel.Title = details.Title;
                    datamodel.Header = details.Header;
                    if(details.Header== "New Vacancy Posted")
                    {
                        datamodel.vacancyLink = "/TMSVacancy/VacancyDetail/" + details.DetailId;                        
                    }
                    
                    datamodel.Name = details.Name;
                    datamodel.Detailid = Convert.ToInt16(details.DetailId);
                    datamodel.Initials = details.Initials;
                    datamodel.ApproveStatus = details.ApprovalStatus;
                    datamodel.StartDate = details.StartDate;
                    datamodel.EndDate = details.EndDate;
                    TempData["eid"] = details.EmployeeID;
                    TempData["DetailId"] = details.DetailId;
                    if (details.ImagePath != null)
                    {
                        datamodel.ImagePath = "~/Upload/Resources/" + details.ImagePath;
                    }
                    ObjEmp.Add(datamodel);

                }
            }
            return View(ObjEmp);
        }
        //public ActionResult Index()
        //{
        //    List<AllNotificationList> ObjEmp = new List<AllNotificationList>();
        //    var data = _NotificationMethod.geNotificationList();
        //    //StringBuilder str = new StringBuilder();
        //    if (data.Count > 0)
        //    {
        //        foreach (var details in data)
        //        {
        //            AllNotificationList datamodel = new AllNotificationList();
        //            datamodel.id = Convert.ToInt64(details.Id);
        //            datamodel.EmployeeId = details.EmployeeID;
        //            datamodel.Title = details.Title;
        //            datamodel.Header = details.Header;
        //            datamodel.Name = details.Name;
        //            datamodel.Detailid = Convert.ToInt16(details.DetailId);
        //            datamodel.Initials = details.Initials;
        //            TempData["eid"] = details.EmployeeID;
        //            TempData["DetailId"] = details.DetailId;
        //            ObjEmp.Add(datamodel);

                    
        //        }
        //    }
        //    return View(ObjEmp);
        //    /*  str.Append("<div class='panel panel-default'>");
        //          str.Append("<div class='panel-heading'>");
        //          str.Append("<h4 class='panel-title'>");
        //          str.Append("<a id = 'header' parent = '#accordion' href = #'" + datamodel.id + ">" + datamodel.Name + "</ a >");
        //          str.Append("</h4>");
        //          str.Append("</div>");
        //          str.Append("</div>");
        //          str.Append("<div id ='" + datamodel.id + "' class='panel-collapsecollapse>'");
        //          str.Append("<div id = 'displayData' ></ div >");
        //          str.Append("</ div >");
        //          str.Append("<input id='" + datamodel.EmployeeId + "_detail' type='hidden' value='" + datamodel.EmployeeId + "' />");
        //          //str.Append(< input id = "Lineid" name = "Lineid" type = "hidden" value = "@item.id" />);
        //          str.Append("<input id = '" + datamodel.Detailid + "_detail' type = 'hidden' value = '" + datamodel.Detailid + "' /> ");*/
        //    /* List<TravelApproveViewModel> model = new List<TravelApproveViewModel>();
        //   var data = _NotificationMethod.getAllTravelApproveList();
        //   if (data.Count > 0)
        //   {
        //       foreach (var details in data)
        //       {
        //           TravelApproveViewModel datamodel = new TravelApproveViewModel();
        //           datamodel.Id = details.Id;
        //           datamodel.Name = details.Name;
        //           model.Add(datamodel);
        //       }
        //   }
        //   return View(model);*/
        //   //List<TravelApproveViewModel> model = new List<TravelApproveViewModel>();
        //    //var data = _NotificationMethod.getAllTravelApproveList();
        //    //if (data.Count > 0)
        //    //{
        //    //    foreach (var details in data)
        //    //    {
        //    //        TravelApproveViewModel datamodel = new TravelApproveViewModel();
        //    //        datamodel.Id = details.Id;
        //    //        datamodel.Name = details.Name;
        //    //        model.Add(datamodel);
        //    //    }
        //    //}
        //    //return View(model);
        //}
        /* public StringBuilder returnHtmlData()
      {
          List<AllNotificationList> ObjEmp = new List<AllNotificationList>();
          var data = _NotificationMethod.geNotificationList();
          AllNotificationList datamodel = new AllNotificationList();
          StringBuilder str = new StringBuilder(10000);
          if (data.Count > 0)
          {
              foreach (var details in data)
              {
                  datamodel.id = Convert.ToInt64(details.Id);
                  datamodel.EmployeeId = details.EmployeeID;
                  datamodel.Header = details.Header;
                  datamodel.Name = details.Name;
                  datamodel.Detailid = Convert.ToInt16(details.DetailId);
                   ObjEmp.Add(datamodel);
                  str.Append("<div class='panel panel-default'>");
                  str.Append("<div class='panel-heading'>");
                  str.Append("<h4 class='panel-title'>");
                  str.Append("<a id ='header' data-toggle='collapse' parent ='#accordion' href='#" + datamodel.id + "'>" + datamodel.Name + "</a >");
                  str.Append("</h4>");
                  str.Append("</div>");
                  str.Append("<div id =" + datamodel.id + " class=panel-collapse collapse>");
                  str.Append("hiiii");
                  str.Append("</div>");
                  str.Append("</div>");
                  str.Append("<input id='" + datamodel.id + "EmpId" + "_detail' type='hidden' value='" + datamodel.EmployeeId + "' />");
                  str.Append("<input id ='" + datamodel.id + "DetailId" + "_detail' type = 'hidden' value ='" + datamodel.Detailid + "' /> ");
              }
              str.Append("<div></div>");
              StringBuilder str1 = new StringBuilder(10000);
              str1.Append("<script language='javascript'>");
              str1.Append("$('a').click(function () {");
              str1.Append("alert('1');");
              str1.Append("var hidden_fields = $(this).find('input:hidden').attr('id');");
              str1.Append("$.ajax({");
              str1.Append("type: 'POST',");
              str1.Append("url:'@Url.Content(~/Notification/GetNotificationDetails)',");
              str1.Append("data: {");
              str1.Append("eid:" + datamodel.EmployeeId + ",");
              str1.Append("tid:" + datamodel.Detailid);
              str1.Append("},");
              str1.Append("success: function (result) {");
              str1.Append("alert('sucess');");
              str1.Append("$('.panel-collapsecollapse').html(result);");
              str1.Append("$('.panel-collapsecollapse').show();");
              str1.Append("}");
              str1.Append("});");
              str1.Append("});");
              str1.Append("</script>");
              str.Append(str1.ToString());
          }
          return (str);
      }*/
        //public ActionResult GetNotificationDetails(string eid, int tid)
        //{
        //    EvolutionEntities _db = new EvolutionEntities();
        //    NotificationMethod _NotificationMethod = new NotificationMethod();
        //    List<AllNotificationDetail> ObjEmp = new List<AllNotificationDetail>();
        //    var data = _NotificationMethod.geNotificationDetail(eid, tid);
        //    if (data.Count > 0)
        //    {
        //        foreach (var details in data)
        //        {
        //            AllNotificationDetail datamodel = new AllNotificationDetail();
        //            datamodel.eid = details.EmployeeId;
        //            datamodel.name = details.Name;
        //            datamodel.tid = details.DetailId;
        //            datamodel.Customer = details.Customer;
        //            datamodel.Day = details.day;
        //            datamodel.Date = Convert.ToString(details.Date);
        //            datamodel.InTime = details.InTime;
        //            datamodel.EndTime = details.EndTime;
        //            datamodel.Hours = details.Hours;
        //            datamodel.CostCode = details.CostCode;
        //            datamodel.Project = details.Project;
        //            datamodel.AssetName = details.AssetName;
        //            datamodel.Status = details.Status;
        //            ObjEmp.Add(datamodel);
        //        }

        //    }
        //    return View(ObjEmp);
        //}

        [HttpPost]
        public PartialViewResult GetNotificationDetailsByKey(int EmployeeID, string Header, int? DetailsId)
        {
            NotificationMethod _NotificationMethod = new NotificationMethod();

            NotificationDetail _NotificationDetail = new NotificationDetail();

            if (Header == "TimeSheet Request")
            {
                var data = _NotificationMethod.GetTimeSheetNotificationByKey(EmployeeID, DetailsId).FirstOrDefault();
                if (data != null)
                {
                    _NotificationDetail.EmployeeId = data.EmployeeId;
                    _NotificationDetail.HeaderType = data.HeaderType;
                    _NotificationDetail.Hours = data.Hours;
                    _NotificationDetail.ProjectName = data.ProjectName;
                    _NotificationDetail.Day = data.Day;
                    _NotificationDetail.Date = data.Date;
                    _NotificationDetail.CustomerName = data.CustomerName;
                    _NotificationDetail.CostCode = data.CostCode;
                    _NotificationDetail.AssetName = data.AssetName;
                    var reportTodata = _db.EmployeeRelations.Where(x => x.IsActive == true && x.UserID == EmployeeID).FirstOrDefault();
                    var aspnetUserDetails = _db.AspNetUsers.Where(x => x.Archived == false && x.Id == EmployeeID).FirstOrDefault();
                    Employee_TimeSheet_Detail TimeSheetDetails = new Employee_TimeSheet_Detail();
                    TimeSheetDetails = _db.Employee_TimeSheet_Detail.Find(DetailsId);
                    if(TimeSheetDetails!=null && TimeSheetDetails.ApprovalStatus=="Pending")
                    {
                        _NotificationDetail.AppStatus = TimeSheetDetails.ApprovalStatus;
                    }
                    if (aspnetUserDetails != null && TimeSheetDetails != null)
                    {
                        if (aspnetUserDetails.HRResponsible == SessionProxy.UserId && aspnetUserDetails.HRResponsible != null)
                        {
                            TimeSheetDetails.IsReadHR = true;
                        }
                        if (aspnetUserDetails.AdditionalReportsto == SessionProxy.UserId)
                        {
                            TimeSheetDetails.IsReadAddRep = true;
                        }
                        if (TimeSheetDetails != null && reportTodata.Reportsto == SessionProxy.UserId)
                        {
                            TimeSheetDetails.IsRead = true;
                        }
                        _db.Entry(TimeSheetDetails).State = System.Data.Entity.EntityState.Modified;
                        _db.SaveChanges();
                    }
                }
            }
            if (Header == "Training Request Worker")
            {
                var data = _NotificationMethod.getEmployeeTrainingById(Convert.ToInt32(DetailsId)).FirstOrDefault();
                if (data != null)
                {
                    _NotificationDetail.EmployeeId = data.EmployeeId;
                    _NotificationDetail.HeaderType = "Training Request Worker";
                    _NotificationDetail.StartDate =Convert.ToString(data.StartDate);
                    _NotificationDetail.EndDate = Convert.ToString(data.EndDate);
                    _NotificationDetail.TrainingDescription = data.Description;

                    EmployeeTraining trainingDetails = new EmployeeTraining();
                    trainingDetails = _db.EmployeeTrainings.Find(DetailsId);
                    trainingDetails.IsReadWorker = true;
                    _db.Entry(trainingDetails).State = System.Data.Entity.EntityState.Modified;
                    _db.SaveChanges();

                }
            }
            else if(Header == "Skill Added")
            {
                var data = _NotificationMethod.getEmployeeSkillById(Convert.ToInt32(DetailsId)).FirstOrDefault();
                if(data!=null)
                {
                    _NotificationDetail.EmployeeId = data.EmployeeId;
                    _NotificationDetail.HeaderType = "Skill Added";
                    if(data.GeneralSkillsName!="" && data.GeneralSkillsName!=null)
                    {
                        int GenralSkillName = Convert.ToInt32(data.GeneralSkillsName);
                        _NotificationDetail.generalSkill = _db.SkillSets.Where(x => x.Id == GenralSkillName).FirstOrDefault().Name;
                    }
                    else
                    {
                        _NotificationDetail.generalSkill = null;
                    }
                    if (data.TechnicalSkillsName != "" && data.TechnicalSkillsName != null)
                    {
                        int TechnSkillName = Convert.ToInt32(data.TechnicalSkillsName);
                        _NotificationDetail.technicalskill = _db.SkillSets.Where(x => x.Id == TechnSkillName).FirstOrDefault().Name;
                    }
                    else
                    {
                        _NotificationDetail.technicalskill = null;
                    }                    
                    _NotificationDetail.StartDate =Convert.ToString(data.CreatedDate);

                    Employee_Skills EmpSkillDetails = new Employee_Skills();
                    EmpSkillDetails = _db.Employee_Skills.Find(DetailsId);
                    EmpSkillDetails.IsRead = true;
                    _db.Entry(EmpSkillDetails).State = System.Data.Entity.EntityState.Modified;
                    _db.SaveChanges();
                }

            }
            else if (Header == "Scheduling Request")
            {
                var data = _NotificationMethod.GetScheduleNotificationByKey(EmployeeID, DetailsId).FirstOrDefault();
                if (data != null)
                {
                    _NotificationDetail.EmployeeId = data.EmployeeId;
                    _NotificationDetail.HeaderType = data.HeaderType;
                    _NotificationDetail.Hours = data.Hours;
                    _NotificationDetail.ProjectName = data.Project;
                    _NotificationDetail.CustomerName = data.Customer;
                    _NotificationDetail.CostCode = data.AssetName;
                    _NotificationDetail.StartDate = data.StartDate;
                    _NotificationDetail.EndDate = data.EndDate;
                    _NotificationDetail.Duration = data.duration;
                    _NotificationDetail.AssetName = data.AssetName;
                    var reportTodata = _db.EmployeeRelations.Where(x => x.IsActive == true && x.UserID == EmployeeID).FirstOrDefault();
                    var aspnetUserDetails = _db.AspNetUsers.Where(x => x.Archived == false && x.Id == EmployeeID).FirstOrDefault();
                    Employee_ProjectPlanner_Scheduling SchedulingDetails = new Employee_ProjectPlanner_Scheduling();
                    SchedulingDetails = _db.Employee_ProjectPlanner_Scheduling.Find(DetailsId);
                    if (SchedulingDetails != null && SchedulingDetails.ApprovalStatus == "Pending")
                    {
                        _NotificationDetail.AppStatus = SchedulingDetails.ApprovalStatus;
                    }
                    if (aspnetUserDetails != null && SchedulingDetails != null)
                    {
                        if (aspnetUserDetails.HRResponsible == SessionProxy.UserId && aspnetUserDetails.HRResponsible != null)
                        {
                            SchedulingDetails.IsReadHR = true;
                        }
                        if (aspnetUserDetails.AdditionalReportsto == SessionProxy.UserId)
                        {
                            SchedulingDetails.IsReadAddRes = true;
                        }
                        if (SchedulingDetails != null && reportTodata.Reportsto == SessionProxy.UserId)
                        {
                            SchedulingDetails.IsRead = true;
                        }
                        _db.Entry(SchedulingDetails).State = System.Data.Entity.EntityState.Modified;
                        _db.SaveChanges();
                    }                    
                }
            }
            else if (Header == "Travel Request")
            {
                var data = _NotificationMethod.GetTravelNotificationByKey(EmployeeID, DetailsId).FirstOrDefault();
                if(data != null)
                {
                    _NotificationDetail.EmployeeId = data.EmployeeId;
                    _NotificationDetail.HeaderType = data.HeaderType;
                    _NotificationDetail.TravelType = data.Type;
                    _NotificationDetail.FromCountry = data.FromCountry;
                    _NotificationDetail.FromTown = data.FromCity;
                    _NotificationDetail.FromPlace = data.FromPlace;
                    _NotificationDetail.ToCountry = data.ToCountry;
                    _NotificationDetail.ToTown = data.ToCity;
                    _NotificationDetail.Toplace = data.ToPlace;
                    _NotificationDetail.StartDate = data.StartDate;
                    _NotificationDetail.EndDate = data.EndDate;
                    _NotificationDetail.Duration = data.Duration;
                    _NotificationDetail.Hours = data.Hour;
                    _NotificationDetail.CustomerName = data.CustomerName;
                    _NotificationDetail.ProjectName = data.ProjectName;
                    _NotificationDetail.CostCode = data.CostCode;
                    _NotificationDetail.Function = "";
                    _NotificationDetail.Link = "";
                    var reportTodata = _db.EmployeeRelations.Where(x => x.IsActive == true && x.UserID == EmployeeID).FirstOrDefault();
                    var aspnetUserDetails = _db.AspNetUsers.Where(x => x.Archived == false && x.Id == EmployeeID).FirstOrDefault();

                    Employee_TravelLeave TravelLeaveDetails = new Employee_TravelLeave();
                    TravelLeaveDetails = _db.Employee_TravelLeave.Find(DetailsId);
                    if (TravelLeaveDetails != null && TravelLeaveDetails.ApprovalStatus == "Pending")
                    {
                        _NotificationDetail.AppStatus = TravelLeaveDetails.ApprovalStatus;
                    }
                    if (aspnetUserDetails != null && TravelLeaveDetails != null)
                    {
                        if (aspnetUserDetails.HRResponsible == SessionProxy.UserId && aspnetUserDetails.HRResponsible != null)
                        {
                            TravelLeaveDetails.IsReadHR = true;
                        }
                        if (aspnetUserDetails.AdditionalReportsto == SessionProxy.UserId)
                        {
                            TravelLeaveDetails.IsReadAddReport = true;
                        }
                        if (TravelLeaveDetails != null && reportTodata.Reportsto == SessionProxy.UserId)
                        {
                            TravelLeaveDetails.IsRead = true;
                        }
                        _db.Entry(TravelLeaveDetails).State = System.Data.Entity.EntityState.Modified;
                        _db.SaveChanges();
                    }

                  
                }
            }
            else if (Header == "Annual Leave Request")
            {
                var data = _NotificationMethod.GetAnnualLeaveNotificationByKey(EmployeeID, DetailsId).FirstOrDefault();
                if(data != null)
                {
                    _NotificationDetail.EmployeeId = data.EmployeeId;
                    _NotificationDetail.StartDate = data.StartDate;
                    _NotificationDetail.EndDate = data.EndDate;
                    _NotificationDetail.HeaderType = "Annual Leave Request";
                    _NotificationDetail.Duration = data.Duration;
                    var reportTodata = _db.EmployeeRelations.Where(x => x.IsActive == true && x.UserID == EmployeeID).FirstOrDefault();
                    var aspnetUserDetails = _db.AspNetUsers.Where(x => x.Archived == false && x.Id == EmployeeID).FirstOrDefault();
                    Employee_AnualLeave AnualLeaveDetails = new Employee_AnualLeave();
                    AnualLeaveDetails = _db.Employee_AnualLeave.Find(DetailsId);
                    if (AnualLeaveDetails != null && AnualLeaveDetails.ApprovalStatus == "Pending")
                    {
                        _NotificationDetail.AppStatus = AnualLeaveDetails.ApprovalStatus;
                    }
                    if (aspnetUserDetails != null && AnualLeaveDetails != null)
                    {
                        if (aspnetUserDetails.HRResponsible == SessionProxy.UserId && aspnetUserDetails.HRResponsible != null)
                        {
                            AnualLeaveDetails.IsReadHR = true;
                        }
                        if (aspnetUserDetails.AdditionalReportsto == SessionProxy.UserId)
                        {
                            AnualLeaveDetails.IsReadAddRep = true;
                        }
                        if (AnualLeaveDetails != null && reportTodata.Reportsto == SessionProxy.UserId)
                        {
                            AnualLeaveDetails.IsRead = true;
                        }
                        _db.Entry(AnualLeaveDetails).State = System.Data.Entity.EntityState.Modified;
                        _db.SaveChanges();
                    }

                }
            }
            else if (Header == "Other Leave Request")
            {
                var data = _NotificationMethod.GetOtherLeaveNotificationByKey(EmployeeID, DetailsId).FirstOrDefault();
                if(data != null)
                {
                    _NotificationDetail.EmployeeId = data.EmployeeId;
                    _NotificationDetail.HeaderType = data.HeaderType;
                    _NotificationDetail.StartDate = data.StartDate;
                    _NotificationDetail.EndDate = data.EndDate;
                    _NotificationDetail.Duration = data.Duration;
                    _NotificationDetail.Reason = data.Reason;
                    var reportTodata = _db.EmployeeRelations.Where(x => x.IsActive == true && x.UserID == EmployeeID).FirstOrDefault();
                    var aspnetUserDetails = _db.AspNetUsers.Where(x => x.Archived == false && x.Id == EmployeeID).FirstOrDefault();
                    Employee_OtherLeave OtherLeaveDetails = new Employee_OtherLeave();
                    OtherLeaveDetails = _db.Employee_OtherLeave.Find(DetailsId);
                    if (OtherLeaveDetails != null && OtherLeaveDetails.ApprovalStatus == "Pending")
                    {
                        _NotificationDetail.AppStatus = OtherLeaveDetails.ApprovalStatus;
                    }
                    if (aspnetUserDetails != null && OtherLeaveDetails != null)
                    {
                        if (aspnetUserDetails.HRResponsible == SessionProxy.UserId && aspnetUserDetails.HRResponsible != null)
                        {
                            OtherLeaveDetails.IsReadHR = true;
                        }
                        if (aspnetUserDetails.AdditionalReportsto == SessionProxy.UserId)
                        {
                            OtherLeaveDetails.IsReadAddRep = true;
                        }
                        if (OtherLeaveDetails != null && reportTodata.Reportsto == SessionProxy.UserId)
                        {
                            OtherLeaveDetails.IsRead = true;
                        }
                        _db.Entry(OtherLeaveDetails).State = System.Data.Entity.EntityState.Modified;
                        _db.SaveChanges();
                    }

                   
                }
            }
            else if(Header== "Maternity/Paternity Leave")
            {
                var data = _NotificationMethod.GetMaternityPatLeaveByKey(Convert.ToInt32(DetailsId)).FirstOrDefault();
                if(data!=null)
                {
                    _NotificationDetail.EmployeeId = data.EmployeeID;
                    _NotificationDetail.HeaderType = "Maternity/Paternity Leave";
                    _NotificationDetail.StartDate =Convert.ToString(data.ActualStartDate);
                    _NotificationDetail.EndDate = Convert.ToString(data.ActualEndDate);
                    _NotificationDetail.dueDate = Convert.ToString(data.DueDate);
                    _NotificationDetail.Link = data.Link;
                    var reportTodata = _db.EmployeeRelations.Where(x => x.IsActive == true && x.UserID == EmployeeID).FirstOrDefault();
                    var aspnetUserDetails = _db.AspNetUsers.Where(x => x.Archived == false && x.Id == EmployeeID).FirstOrDefault();
                    Employee_MaternityOrPaternityLeaves MatPatLeaveDetails = new Employee_MaternityOrPaternityLeaves();
                    MatPatLeaveDetails = _db.Employee_MaternityOrPaternityLeaves.Find(DetailsId);                    
                    if (MatPatLeaveDetails != null && MatPatLeaveDetails.ApprovalStatus == "Pending")
                    {
                        _NotificationDetail.AppStatus = MatPatLeaveDetails.ApprovalStatus;
                    }
                    if (aspnetUserDetails != null && MatPatLeaveDetails != null)
                    {
                        if (aspnetUserDetails.HRResponsible == SessionProxy.UserId && aspnetUserDetails.HRResponsible != null)
                        {
                            MatPatLeaveDetails.IsReadHR = true;
                        }
                        if (aspnetUserDetails.AdditionalReportsto == SessionProxy.UserId)
                        {
                            MatPatLeaveDetails.IsReadAddRes = true;
                        }
                        if (MatPatLeaveDetails != null && reportTodata.Reportsto == SessionProxy.UserId)
                        {
                            MatPatLeaveDetails.IsRead = true;
                        }
                        _db.Entry(MatPatLeaveDetails).State = System.Data.Entity.EntityState.Modified;
                        _db.SaveChanges();
                    }

                }
            }
            else if(Header== "Sick Leave")
            {
                var data = _NotificationMethod.getSickLeaveByKey(Convert.ToInt32(DetailsId)).FirstOrDefault();
                if(data!=null)
                {
                    _NotificationDetail.EmployeeId = data.EmployeeId;
                    _NotificationDetail.HeaderType = "Sick Leave Request";
                    _NotificationDetail.StartDate = Convert.ToString(data.StartDate);
                    _NotificationDetail.EndDate = Convert.ToString(data.EndDate);
                    _NotificationDetail.Duration = Convert.ToDecimal(data.DurationDays);
                    _NotificationDetail.DoctConsulted = data.DoctorConsulted;
                    _NotificationDetail.Paid=data.IsPaid;
                    var reportTodata = _db.EmployeeRelations.Where(x => x.IsActive == true && x.UserID == EmployeeID).FirstOrDefault();
                    Employee_SickLeaves SickLeaveDetails = new Employee_SickLeaves();
                    SickLeaveDetails = _db.Employee_SickLeaves.Find(DetailsId);
                    if (SickLeaveDetails != null && SickLeaveDetails.ApprovalStatus == "Pending")
                    {
                        _NotificationDetail.AppStatus = SickLeaveDetails.ApprovalStatus;
                    }
                    var aspnetUserDetails = _db.AspNetUsers.Where(x => x.Archived == false && x.Id == EmployeeID).FirstOrDefault();
                    if (aspnetUserDetails!=null && SickLeaveDetails!=null)
                    {
                        if (aspnetUserDetails.HRResponsible == SessionProxy.UserId && aspnetUserDetails.HRResponsible != null)
                        {
                            SickLeaveDetails.IsReadHR = true;
                        }
                        if (aspnetUserDetails.AdditionalReportsto == SessionProxy.UserId)
                        {
                            SickLeaveDetails.IsReadAddRep = true;
                        }
                        if (SickLeaveDetails != null && reportTodata.Reportsto == SessionProxy.UserId)
                        {
                            SickLeaveDetails.IsRead = true;
                        }
                    
                        _db.Entry(SickLeaveDetails).State = System.Data.Entity.EntityState.Modified;
                        _db.SaveChanges();
                    }
                    
                }
            }           
            else if (Header == "Training Request")
            {
                var data = _NotificationMethod.GetTrainingNotificationByKey(EmployeeID, DetailsId).FirstOrDefault();
                if(data != null)
                {
                    _NotificationDetail.EmployeeId = data.EmployeeId;
                    _NotificationDetail.StartDate = data.StartDate;
                    _NotificationDetail.EndDate = data.EndDate;
                    _NotificationDetail.TrainingName = data.TrainingName;
                    _NotificationDetail.Provider = data.Provider;
                    if(data.Importance == 1)
                    {
                        _NotificationDetail.Importance = "Mandatory";
                    }
                    else
                    {
                        _NotificationDetail.Importance = "Optional";
                    }
                    
                    _NotificationDetail.Cost = data.Cost;
                    _NotificationDetail.Day = Convert.ToString(data.Days);
                    _NotificationDetail.HeaderType = "Training Request";

                    EmployeeTraining EmployeeTraining = new EmployeeTraining();
                    EmployeeTraining = _db.EmployeeTrainings.Find(DetailsId);
                    EmployeeTraining.IsRead = true;
                    _db.Entry(EmployeeTraining).State = System.Data.Entity.EntityState.Modified;
                    _db.SaveChanges();
                }
            }
            else if (Header == "New Vacancy")
            {
                var data = _NotificationMethod.GetNewVacancyNotificationByKey(EmployeeID, DetailsId).FirstOrDefault();
                if (data != null)
                {
                    _NotificationDetail.EmployeeId = null;
                    _NotificationDetail.HeaderType = "New Vacancy";
                    _NotificationDetail.Vacancy = data.Title;
                    _NotificationDetail.ClosingDate = data.ClosingDate;
                    _NotificationDetail.RecruitementProcess = data.RecruitmentProcesses;
                    _NotificationDetail.SalaryRange = data.Salary;
                    _NotificationDetail.Location = data.Location;
                    _NotificationDetail.Business = data.Business;
                    _NotificationDetail.Division = data.Division;
                    _NotificationDetail.Pool = data.Pool;
                    _NotificationDetail.Function = data.Functions;

                    Vacancy VacancyDetails = new Vacancy();
                    VacancyDetails = _db.Vacancies.Find(DetailsId);
                    VacancyDetails.IsRead = true;
                    _db.Entry(VacancyDetails).State = System.Data.Entity.EntityState.Modified;
                    _db.SaveChanges();

                }

            }
            else if (Header == "Uplift Submission")
            {
                var data = _NotificationMethod.GetUpLiftNotificationByKey(EmployeeID, DetailsId).FirstOrDefault();
                if (data != null)
                {
                    _NotificationDetail.EmployeeId = data.EmployeeId;
                    _NotificationDetail.HeaderType = data.HeaderType;
                    _NotificationDetail.Day = data.day;
                    _NotificationDetail.Date = data.Date;
                    _NotificationDetail.UpliftPosition = data.UpliftPostionId;
                    _NotificationDetail.Hours = data.Hours;
                    _NotificationDetail.ProjectName = data.Project;
                    _NotificationDetail.CustomerName = data.Customer;
                    _NotificationDetail.ChangeInWorkRate = data.WorkerRate;
                    _NotificationDetail.ChangeInCustomerRate = data.CustomerRate;

                    Employee_ProjectPlanner_Uplift_Detail UpliftDetails = new Employee_ProjectPlanner_Uplift_Detail();
                    var reportTodata = _db.EmployeeRelations.Where(x => x.IsActive == true && x.UserID == EmployeeID).FirstOrDefault();
                    UpliftDetails = _db.Employee_ProjectPlanner_Uplift_Detail.Find(DetailsId);
                    var aspnetUserDetails = _db.AspNetUsers.Where(x => x.Id == EmployeeID && x.Archived == false).FirstOrDefault();
                    if (UpliftDetails != null && UpliftDetails.ApprovalStatus == "Pending")
                    {
                        _NotificationDetail.AppStatus = UpliftDetails.ApprovalStatus;
                    }
                    if (aspnetUserDetails != null && UpliftDetails!=null)
                    {
                        if (aspnetUserDetails.HRResponsible == SessionProxy.UserId && aspnetUserDetails.HRResponsible != null)
                        {
                            UpliftDetails.IsReadHR = true;
                        }
                        if (aspnetUserDetails.AdditionalReportsto == SessionProxy.UserId)
                        {
                            UpliftDetails.IsReadAddRep = true;
                        }
                        if (UpliftDetails != null && reportTodata.Reportsto == SessionProxy.UserId)
                        {
                            UpliftDetails.IsRead = true;
                        }
                        _db.Entry(UpliftDetails).State = System.Data.Entity.EntityState.Modified;
                        _db.SaveChanges();
                    }
                }                
            }
            else if (Header == "Skill Endorsement")
            {
                Employee_AddEndrosementSkills EndrosementSkillseDetails = new Employee_AddEndrosementSkills();
                EndrosementSkillseDetails = _db.Employee_AddEndrosementSkills.Find(DetailsId);
                EndrosementSkillseDetails.IsRead = true;
                _db.Entry(EndrosementSkillseDetails).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
            }
            else if(Header== "New Resource")
            {
                var data = _NotificationMethod.getNewResourceDetails(EmployeeID);
                if (data != null)
                {
                    _NotificationDetail.EmployeeId = data.Id;
                    _NotificationDetail.HeaderType = "New Resource";
                    _NotificationDetail.StartDate = Convert.ToString(data.StartDate);
                    _NotificationDetail.email = Convert.ToString(data.UserName);

                    AspNetUser aspDetails = new AspNetUser();
                    var reportTodata = _db.EmployeeRelations.Where(x => x.IsActive == true && x.UserID == EmployeeID).FirstOrDefault();
                    aspDetails = _db.AspNetUsers.Find(EmployeeID);
                    if (aspDetails.HRResponsible == SessionProxy.UserId)
                    {
                        aspDetails.IsReadHRRespo = true;
                    }
                    if (aspDetails.AdditionalReportsto == SessionProxy.UserId)
                    {
                        aspDetails.IsReadAddReport = true;
                    }
                    if (reportTodata!=null && reportTodata.Reportsto == SessionProxy.UserId)
                    {
                        aspDetails.IsReadNewResource = true;
                    }
                    _db.Entry(aspDetails).State = System.Data.Entity.EntityState.Modified;
                    _db.SaveChanges();
                }
            }           
            else if(Header== "Delete employee")
            {
                var data = _NotificationMethod.getDeleteResourceDetails(EmployeeID);
                if(data!=null)
                {
                    _NotificationDetail.EmployeeId = data.Id;
                    _NotificationDetail.HeaderType = "Delete employee";
                    _NotificationDetail.EmployeeName = data.FirstName+" "+data.LastName;
                    _NotificationDetail.StartDate = Convert.ToString(data.StartDate);
                    _NotificationDetail.email = Convert.ToString(data.UserName);

                    AspNetUser aspDetails = new AspNetUser();
                    aspDetails = _db.AspNetUsers.Find(EmployeeID);
                    aspDetails.IsReadArchived = true;
                    _db.Entry(aspDetails).State = System.Data.Entity.EntityState.Modified;
                    _db.SaveChanges();
                }
            }
            else if(Header== "New Document")
            {
                var data = _NotificationMethod.getEmployeeDocumentByKey(Convert.ToInt32(DetailsId));
                _NotificationDetail.EmployeeId = data.Id;
                _NotificationDetail.HeaderType = "New Document";
                _NotificationDetail.DocumentName = data.DocumentPath;
                _NotificationDetail.Description = data.Description;
                _NotificationDetail.DocLink = data.LinkURL;
                Employee_Document aspDetails = new Employee_Document();
                aspDetails = _db.Employee_Document.Find(DetailsId);
                aspDetails.IsRead = true;
                _db.Entry(aspDetails).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
            }
            else if(Header== "Document Signature")
            {
                var data = _NotificationMethod.getEmployeeDocumentForSignatureByKey(Convert.ToInt32(DetailsId));
                _NotificationDetail.EmployeeId = data.Id;
                _NotificationDetail.HeaderType = "Document Signature";
                _NotificationDetail.DocumentName = data.DocumentPath;
                _NotificationDetail.Description = data.Description;
                _NotificationDetail.DocLink = data.LinkURL;
                Employee_Document aspDetails = new Employee_Document();
                aspDetails = _db.Employee_Document.Find(DetailsId);
                aspDetails.IsReadSignature = true;
                _db.Entry(aspDetails).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
            }
            else if(Header== "New Vacancy Posted")
            {
                var data = _NotificationMethod.getVacancyByKey(Convert.ToInt32(DetailsId));

                _NotificationDetail.EmployeeId = data.Id;
                _NotificationDetail.HeaderType = "New Vacancy Posted";
                Vacancy vacDetails = new Vacancy();
                vacDetails = _db.Vacancies.Find(DetailsId);
                vacDetails.IsReadVacancy = true;
                _db.Entry(vacDetails).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
            }
            else if(Header== "New Applicant")
            {
                var data = _NotificationMethod.getTMSApplicantDetailByKey(Convert.ToInt32(EmployeeID));

                _NotificationDetail.EmployeeId = data.Id;
                _NotificationDetail.HeaderType = "New Applicant";                
                TMS_Applicant aspDetails = new TMS_Applicant();
                aspDetails = _db.TMS_Applicant.Find(EmployeeID);
                aspDetails.IsReadHiringLead = true;
                _db.Entry(aspDetails).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
            }
            return PartialView("_GetNotificationDetails", _NotificationDetail);
        }

        public ActionResult loadstringdata()
        {
            //List<AnnualLeaveapprove> ObjEmp = new List<AnnualLeaveapprove>();
            StringBuilder str = new StringBuilder();
            str.Append("<div></div>");
            return Content(str.ToString());
        }

        // GET: Notification/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Notification/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Notification/Create
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

        // GET: Notification/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Notification/Edit/5
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

        // GET: Notification/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Notification/Delete/5
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
