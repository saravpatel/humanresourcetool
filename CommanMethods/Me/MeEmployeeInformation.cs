using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRTool.Models.Me;
using HRTool.DataModel;

namespace HRTool.CommanMethods.Me
{
    public class MeEmployeeInformation
    {
       EvolutionEntities _db = new EvolutionEntities();
        
        public int getTotalEmpoyeeHolidayThisYear(int EmployeeID)
        {
            int holidayThisYear = _db.AspNetUsers.Where(xx => xx.Id == EmployeeID).FirstOrDefault().Thisyear != null ? _db.AspNetUsers.Where(xx=>xx.Id==EmployeeID).FirstOrDefault().Thisyear.Value : 0;
            return holidayThisYear;
       }
        public int getTotalWorkingDayInfo(int EmployeeId)
        {
            int count = _db.GetLengthOfEmployment(EmployeeId).FirstOrDefault()!=null && _db.GetLengthOfEmployment(EmployeeId).FirstOrDefault() > 0 ? _db.GetLengthOfEmployment(EmployeeId).FirstOrDefault().Value : 0;
            return count;
        }
        public int getEmployeApproveAnnualLeave(int Employeeid)
        {
            int count = 0;
            count = _db.Employee_AnualLeave.Where(x => x.ApprovalStatus == "Approve" && x.EmployeeId == Employeeid).Count();
            return count;
        }

        public List<GetYearInformation_Result> getEmployeeYear(int year)
        {
            return _db.GetYearInformation(year).ToList();
        }

        public List<Employee_TimeSheet> getEmployeeWrokDay(int EmployeeId)
        {
            return _db.Employee_TimeSheet.Where(x => x.EmployeeId == EmployeeId).ToList();
        }
        public List<AspNetUser> getEmplyeeWorkDate(int EmployeeId)
        {
            return _db.AspNetUsers.Where(x => x.Id == EmployeeId).ToList();
        }
        public AddNewTaskListViewModel SaveTempTaskRecord(AddNewTaskListViewModel Model, int UserId)
        {
            Employee_Task_Temp save = new Employee_Task_Temp();
            save.Title = Model.Title;
            save.AlterBeforeDays = Model.AlertBeforeDays;
            save.Archived = false;
            save.AssignTo = Model.Assign;
            save.Created = DateTime.Now;
            save.CreatedBy = UserId;
            save.LastModified = DateTime.Now;
            save.LastModifiedBy = UserId;
            save.Status = Model.Status;
            save.Description = Model.Description;
            if (Model.DueDate != null)
            {
                var DyeDateToString = Model.DueDate;
            }            
            _db.Employee_Task_Temp.Add(save);
            _db.SaveChanges();
            Model.Id = save.Id;
            Model.IsTemp = 1;
            return Model;
        }
        public void UpdateTaskRecord(AddNewTaskListViewModel model, int UserId)
        {
               Task_List record = _db.Task_List.Where(x => x.Id == model.IdRecord).FirstOrDefault();

                record.Title = model.Title;
                record.Description = model.Description;
                record.AssignTo = model.Assign;
                record.LastModifiedBy = UserId;

                if (model.DueDate != null)
                {
                    var DyeDateToString =model.DueDate;
                 }
                record.AlterBeforeDays = model.AlertBeforeDays;
                record.Status = model.Status;
                record.Difualt = true;
                record.LastModified = DateTime.Now;
                _db.SaveChanges();
            

        }

        public List<Task_List> GetNewstaskrecord()
        {
            return _db.Task_List.Where(x => x.Difualt == true && x.Archived == false).ToList();
        }
        public List<GetEmployeeTaskList_Result> getEmployeeIncomplateTask(int EmployeeId)
        {
            return _db.GetEmployeeTaskList(EmployeeId).ToList();
        }
        public List<GetEmployeeAnualLeaveBenefitList_Result> getemployeeBenfit(int EmployeeId)
        {
            return _db.GetEmployeeAnualLeaveBenefitList(EmployeeId).ToList();
        }
        public List<SystemListValue> getAllSystemValueListByNameId(int Id)
        {
            return _db.SystemListValues.Where(x => x.Archived == false && x.SystemListID == Id).ToList();
        }
        public SystemList getSystemListByName(string Name)
        {
            return _db.SystemLists.Where(x => x.SystemListName == Name).FirstOrDefault();
        }
        
        public List<GetEmployeeProfileData_Result> getEmployeeData(int EmployeeId)
        {
            return _db.GetEmployeeProfileData(EmployeeId).ToList();
        }
        public List<GetResourceEmployeeData_Result> getResourceEmployeeData(int EmployeeId)
        {
            return _db.GetResourceEmployeeData(EmployeeId).Where(x=>x.SSOID.StartsWith("W")).ToList();
        }
        public List<SystemListValue> getAllSystemValueListByKeyName(string KeyName)
        {
            SystemList systemName = getSystemListByName(KeyName);
            return getAllSystemValueListByNameId(systemName.Id);
        }
        
    }
}