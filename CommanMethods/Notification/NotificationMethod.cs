using HRTool.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.CommanMethods.Notification
{
    public class NotificationMethod
    {
        EvolutionEntities _db = new EvolutionEntities();

        public List<GetTravelApproval_Result> getAllTravelApproveList()
        {
            return _db.GetTravelApproval().ToList();
        }

        public List<GetAllNotificationList_Result> geNotificationList(int EmployeeID)
        {
            return _db.GetAllNotificationList(EmployeeID).ToList();
        }
        public List<GetAllNotificationList_Result> geNotificationList()
        {
            //return _db.GetAllNotificationList().ToList();
            return null;

        }



        //public List<AllNotificationListDetails_Result> geNotificationDetail(string EmpId, int tID)
        //{
        //    return _db.AllNotificationListDetails(EmpId, tID).ToList();
        //}

        public List<GetTimeSheetNotificationDetails_Result> GetTimeSheetNotificationByKey(int EmployeeId, int? DetailsID)
        {
            return _db.GetTimeSheetNotificationDetails(EmployeeId, DetailsID).ToList();
        }

        public List<GetScheduleNotificationDetails_Result> GetScheduleNotificationByKey(int EmployeeId, int? DetailsID)
        {
            return _db.GetScheduleNotificationDetails(EmployeeId, DetailsID).ToList();
        }

        public List<GetTravelNotificationDetails_Result> GetTravelNotificationByKey(int EmployeeId, int? DetailsID)
        {
            return _db.GetTravelNotificationDetails(EmployeeId, DetailsID).ToList();
        }
        public List<GetAnnualLeaveNotificationDetails_Result> GetAnnualLeaveNotificationByKey(int EmployeeId, int? DetailsID)
        {
            return _db.GetAnnualLeaveNotificationDetails(EmployeeId, DetailsID).ToList();
        }

        public List<GetTrainingNotificationDetails_Result> GetTrainingNotificationByKey(int EmployeeId, int? DetailsID)
        {
            return _db.GetTrainingNotificationDetails(EmployeeId, DetailsID).ToList();
        }

        public List<GetNewVacancyNotificationDetails_Result> GetNewVacancyNotificationByKey(int EmployeeId, int? DetailsID)
        {
            return _db.GetNewVacancyNotificationDetails(EmployeeId, DetailsID).ToList();
        }
        public List<GetOtherLeaveNotificationDetails_Result> GetOtherLeaveNotificationByKey(int EmployeeId, int? DetailsID)
        {
            return _db.GetOtherLeaveNotificationDetails(EmployeeId, DetailsID).ToList();
        }
        public List<Employee_MaternityOrPaternityLeaves> GetMaternityPatLeaveByKey(int DetailsId)
        {
            return _db.Employee_MaternityOrPaternityLeaves.Where(x => x.Archived == false && x.Id==DetailsId && x.IsRead == false).ToList();
        }
        public List<Employee_SickLeaves> getSickLeaveByKey(int DetailsId)
        {
            return _db.Employee_SickLeaves.Where(x => x.Archived == false && x.Id == DetailsId && x.IsRead==false).ToList();
        }
        public List<GetUpliftNotificationDetails_Result> GetUpLiftNotificationByKey(int EmployeeId, int? DetailsID)
        {
            return _db.GetUpliftNotificationDetails(EmployeeId, DetailsID).ToList();
        }
        public List<Employee_Skills> getEmployeeSkillById(int DetailId)       
        {
            return _db.Employee_Skills.Where(x => x.Id == DetailId && x.IsRead == false).ToList();           
        }
        public List<EmployeeTraining> getEmployeeTrainingById(int DetailId)
        {
            return _db.EmployeeTrainings.Where(x => x.Id == DetailId && x.IsReadWorker == false).ToList();
        }
        public AspNetUser getNewResourceDetails(int empID)
        {
            return _db.AspNetUsers.Where(x => x.Archived == false && x.Id==empID).FirstOrDefault();
        }
        public AspNetUser getDeleteResourceDetails(int EmpID)
        {
            return _db.AspNetUsers.Where(x => x.Archived == true && x.Id==EmpID).FirstOrDefault();
        }
        public Employee_Document getEmployeeDocumentByKey(int DocId)
        {
            return _db.Employee_Document.Where(x => x.Id == DocId && x.Archived == false && x.IsRead==false).FirstOrDefault();
        }
        public Employee_Document getEmployeeDocumentForSignatureByKey(int DocId)
        {
            return _db.Employee_Document.Where(x => x.Id == DocId && x.Archived == false && x.IsReadSignature == false).FirstOrDefault();
        }
        public TMS_Applicant getTMSApplicantDetailByKey(int ApplicantId)
        {
            return _db.TMS_Applicant.Where(x => x.Id == ApplicantId && x.Archived == false).FirstOrDefault();
        }
        public Vacancy getVacancyByKey(int VacId)
        {
            return _db.Vacancies.Where(x => x.Id == VacId && x.Archived == false).FirstOrDefault();
        }
    }
}