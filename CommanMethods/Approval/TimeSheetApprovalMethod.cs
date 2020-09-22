using HRTool.DataModel;
using HRTool.Models.Approval;
using HRTool.Models.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.CommanMethods.Approval
{
    public class TimeSheetApprovalMethod
    {
        EvolutionEntities _db = new EvolutionEntities();

        public List<GetSick_Approval_Result> getAllSickLeaveApprovalList(int EmpID)
        {
            return _db.GetSick_Approval().Where(x => x.EmployeeId == EmpID).ToList();
        }

        public decimal countTottalSickDaysIntheYear(int EmpID)
        {
            var data = _db.Employee_SickLeaves.Where(x => x.Archived == false && x.EmployeeId == EmpID).ToList();
            decimal totalSickDays = 0;
            foreach(var item in data)
            {
                totalSickDays += Convert.ToDecimal(item.DurationDays);
            }
            return totalSickDays;
        }
        public List<GetTimeSheet_Approve_Result> getAllTimeSheetPendingApprovalList(int EmpID)
        {
            return _db.GetTimeSheet_Approve().Where(x=>x.EmployeeId== EmpID).ToList();
        }

        public List<GetProjectPlannerTimeSheet_Approve_Result> getProjectTimeSheetPendingApprovalList(int EmpID)
        {
            return _db.GetProjectPlannerTimeSheet_Approve().Where(x => x.EmployeeId == EmpID).ToList();
        }

        public List<GetScheduleApprovalList_Result> getScheduleApprovalList(int EmpID)
        {
            return _db.GetScheduleApprovalList().Where(x => x.EmployeeId == EmpID).ToList();
        }

        public List<GetTravelApproval_Result> getAllTravelList(int EmpID)
        {
            return _db.GetTravelApproval().Where(x => x.EmployeeId == EmpID).ToList();
        }
        public List<GetMatPatLeave_Approval_Result> getAllMat_PatList(int EmpId)
        {
            return _db.GetMatPatLeave_Approval().Where(x => x.EmployeeId == EmpId).ToList();
        }
        public int getTotalWorkingDaysOfEmployee(int EmployeeId)
        {
            var data = _db.AspNetUsers.Where(x => x.Id == EmployeeId && x.Archived == false).FirstOrDefault();
            int StartYear = data.StartDate.Value.Year;
            int currYear = DateTime.Now.Year;
            int totalEmployementLength = currYear - StartYear;
            return totalEmployementLength;
        }

        public List<GetAnnualLeaveRequest_Result> getAnualLeaveList(int EmpID)
        {
            return _db.GetAnnualLeaveRequest(EmpID).ToList();
        }

        public List<Employee_SickLeaves> getSickLeave()
        {
            return _db.Employee_SickLeaves.Where(x => x.Archived == false).ToList();
        }

        public List<GetOtherLeaveApprovelList_Result> getOtherLeave(int EmpID)
        {
            return _db.GetOtherLeaveApprovelList().Where(x => x.EmployeeId == EmpID).ToList();
        }
        public int getCurruentYear()
        {
            int currentYear = _db.HolidayYearAndMonthSettings.Where(x => x.IsActive == true).FirstOrDefault().StartYear!=null ? _db.HolidayYearAndMonthSettings.Where(x => x.IsActive == true).FirstOrDefault().StartYear.Value : 0;
            return currentYear;
        }
        public List<Employee_OtherLeave> getTotalLeave(int EmployeeId)
        {
            return _db.Employee_OtherLeave.Where(x => x.EmployeeId == EmployeeId).ToList();
        }
        public List<EmployeeTraining> getTotalEmployeeTraning(int EmployeeID)
        {
            return _db.EmployeeTrainings.Where(x => x.EmployeeId == EmployeeID).ToList();
        }
        public List<GetTrainingRequest_Result> getTrainingRequestlList(int EmpID)
        {
            return _db.GetTrainingRequest(EmpID).ToList();
        }

        public List<GetNewVacancyList_Result> getNewVacancy(int EmpID)
        {
            return _db.GetNewVacancyList().Where(x => x.UserIDLastModifiedBy == EmpID).ToList();
        }
        public int getTotalVacancy()
        {
            int totalVacancy = _db.Vacancies.Where(x => x.ApprovalStatus == "Pending").Count();
            return totalVacancy;
        }

        public List<Employee_MaternityOrPaternityLeaves> getMat_PatLeave()
        {
            return _db.Employee_MaternityOrPaternityLeaves.Where(x => x.Archived == false).ToList();
        }

        public List<GetUpliftApprove_Result> getUpliftApprove(int EmpID)
        {
            return _db.GetUpliftApprove().Where(x => x.EmployeeId == EmpID).ToList();
        }
        
        public List<GetAllApproveRequestList_Result> getApproveList(int empID)
        {
            return _db.GetAllApproveRequestList(empID).ToList();
            //return null;
        }

        public void UpdateTimeSheetApprovalStatus(string ID)
        {
            //var userData = _db.Inx_Employee_TimeSheet_Detail.Where(x => x.Id == ID).FirstOrDefault();
            _db.UpdateTimesheetApproval(ID);
        }

        public void UpdateTimeSheetRejectStatus(string ID)
        {
            //var userData = _db.Inx_Employee_TimeSheet_Detail.Where(x => x.Id == ID).FirstOrDefault();
            _db.UpdateTimesheetStatus_Reject(ID);
        }

        public void UpdateScheduleApprovalStatus(string ID)
        {
           // var userData = _db.Inx_Employee_ProjectPlanner_Scheduling.Where(x => x.Id == ID).FirstOrDefault();
            _db.UpdateScheduleStatusApprove(ID);
        }

        public void UpdateScheduleRejectStatus(string ID)
        {
           // var userData = _db.Inx_Employee_ProjectPlanner_Scheduling.Where(x => x.Id == ID).FirstOrDefault();
            _db.UpdateScheduleStatusReject(ID);
        }

        public void UpdateTravelApprovalStatus(string ID)
        {
            //var userData = _db.Inx_Employee_TravelLeave.Where(x => x.Id == ID).FirstOrDefault();
            _db.UpdateTravelApproveStatus(ID);
        }
        public void UpdateMat_PatApproveStatus(string ID)
        {
            _db.UpdateMat_PatApproveStatus(ID);
        }
        public void UpdateMat_PatRejectStatus(string ID)
        {
            _db.UpdateMat_PatRejectStatus(ID);
        }
        public void UpdateTravelRejectStatus(string ID)
        {
            //var userData = _db.Inx_Employee_TravelLeave.Where(x => x.Id == ID).FirstOrDefault();
            _db.UpdateTravelRejectStatus(ID);
        }

        public void UpdateAnualLeaveApprovalStatus(string ID)
        {
            //var userData = _db.Inx_Employee_AnualLeave.Where(x => x.Id == ID).FirstOrDefault();
            _db.UpdateAnnualLeaveApproveStatues(ID);
        }

        public void UpdateAnualLeaveRejectStatus(string ID)
        {
            //var userData = _db.Inx_Employee_AnualLeave.Where(x => x.Id == ID).FirstOrDefault();
            _db.UpdateAnnualLeaveRejectStatues(ID);
        }

        public void UpdateTrainingApprovalStatus(string ID)
        {
            //var userData = _db.Inx_EmployeeTraining.Where(x => x.Id == ID).FirstOrDefault();
            _db.UpdateTrainingApproval(ID);
        }

        public void UpdateTrainingRejectStatus(string ID)
        {
            //var userData = _db.Inx_EmployeeTraining.Where(x => x.Id == ID).FirstOrDefault();
            _db.UpdateTrainingReject(ID);
        }

        public void UpdateOtherLeaveApprovalStatus(string ID)
        {
            //var userData = _db.Inx_Employee_OtherLeave.Where(x => x.Id == ID).FirstOrDefault();
            _db.UpdateOtherLeaveApproval(ID);
        }
        public void UpdateSickLeaveApprovalStatus(string ID)
        {
            _db.UpdateSickLeaveApproval(ID);
        }

        public void UpdateOtherLeaveRejectStatus(string ID)
        {
            //var userData = _db.Inx_Employee_OtherLeave.Where(x => x.Id == ID).FirstOrDefault();
            _db.UpdateOtherLeaveReject(ID);
        }

        public void UpdateUpliftApprovalStatus(string ID)
        {
            //var userData = _db.Inx_Employee_ProjectPlanner_Uplift_Detail.Where(x => x.Id == ID).FirstOrDefault();
            _db.UpdateUpliftApproveStatus(ID);
        }

        public void UpdateUpliftRejectStatus(string ID)
        {
            //var userData = _db.Inx_Employee_ProjectPlanner_Uplift_Detail.Where(x => x.Id == ID).FirstOrDefault();
            _db.UpdateUpliftRejectStatus(ID);
        }

        public void UpdateSickRejectStatus(string ID)
        {
            _db.UpdateSickRejectStatus(ID);
        }
        public void UpdateNewVacancyApprovalStatus(string ID)
        {
            //var userData = _db.Inx_Vacancy.Where(x => x.Id == ID).FirstOrDefault();
            _db.UpdateNewVacancyApproval(ID);
        }

        public void UpdateNewVacancyRejectStatus(string ID)
        {
            //var userData = _db.Inx_Vacancy.Where(x => x.Id == ID).FirstOrDefault();
            _db.UpdateNewVacancyReject(ID);
        }


        public void UpdateProjectTimesheetApprovalStatus(string ID)
        {
            //var userData = _db.Inx_Employee_ProjectPlanner_TimeSheet_Detail.Where(x => x.Id == ID).FirstOrDefault();
            _db.UpdateProjectTimesheetApproval(ID);
        }

        public void UpdateProjectTimesheetRejectStatus(string ID)
        {
            //var userData = _db.Inx_Employee_ProjectPlanner_TimeSheet_Detail.Where(x => x.Id == ID).FirstOrDefault();
            _db.UpdateProjectTimesheetReject(ID);
        }
    }
}