using HRTool.CommanMethods.Admin;
using HRTool.DataModel;
using HRTool.Models.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using static HRTool.CommanMethods.Enums;

namespace HRTool.CommanMethods.Resources
{
    public class EmployeeMethod
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        AdminTMSMethod _AdminTMSMethod = new AdminTMSMethod();
        private string inputFormat = "dd-MM-yyyy";
        private string outputFormat = "yyyy-MM-dd HH:mm:ss";
        #endregion
      
        public List<GetDefaultCurrenciesCode_Result> getCurrCode()
        {
            return _db.GetDefaultCurrenciesCode().ToList();
        }
        public List<GetWorkPatternDataById_Result> getWorkPatternById(int EmpId)
        {
            return _db.GetWorkPatternDataById(EmpId).ToList();
        }
        public bool validateSSo(string sso)
        {           
            var data = _db.AspNetUsers.Select(x => x.SSOID).ToList();
            int flag=0;
            sso = sso.ToUpper();
            foreach(var item in data)
            {

                if(item==sso)
                {
                    flag=1;
                    break;
                }              
            }
            if(flag==1)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public bool validEmailId(string emaiId)
        {
            var data = _db.AspNetUsers.Select(x => x.UserName).ToList();
            int flag = 0;
          
            foreach (var item in data)
            {

                if (item == emaiId)
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
        public List<string> BindWorkerDropdown()
        {
            List<string> model = new List<string>();
            var List = _db.AspNetUsers.Where(x => x.SSOID.Contains("W") && x.Archived == false).ToList();
            model.Add("-- Select User --");
            foreach (var item in List)
            {
                var value = String.Format("{0} {1} - {2}", item.FirstName, item.LastName, item.SSOID);
                model.Add(value);
            }
            return model;
        }
        public AspNetUser getEmployeeById(int? EmployeeId)
        {
            return _db.AspNetUsers.Where(x => x.Id == EmployeeId).FirstOrDefault();
        }

        public List<AspNetUser> GetAllEmployeeList()
        {
            return _db.AspNetUsers.Where(x=>x.Archived==false).ToList();
        }
        public List<AspNetUser> GetAllResourceEmployeeList()
        {
            return _db.AspNetUsers.Include("AspNetUserRoles").Where(x => x.SSOID.StartsWith("W")).ToList();
        }

      public List<AspNetUser> getCustomerResponsibleWorker(int EmpID)
        {
            var EmpId = _db.AspNetUsers.Where(x => x.Archived == false && x.Id == EmpID).FirstOrDefault();
            List<AspNetUser> asp = new List<AspNetUser>();
            if (EmpId != null)
            {
                if (EmpId.CustomerCareID != null && EmpId.CustomerCareID!="")
                {
                    string[] EId = EmpId.CustomerCareID.Split(',');

                    for (int i = 0; i < EId.Length; i++)
                    {
                        int eid = Convert.ToInt32(EId[i]);
                        asp.Add(_db.AspNetUsers.Where(x => x.Archived == false && x.Id == eid).FirstOrDefault());
                    }
                }
            }
            return asp;
        }
        public List<GetReportToResource_Result> getReportToEmployee(int EmpId)
        {
            return _db.GetReportToResource(EmpId).ToList();
        }
        //public string getEmpName(int Id)
        //{
        //    var data = _db.getEmpNameById(Id).ToList();
        //    string name = "";
        //    foreach(var item in data)
        //    {
        //        name = item.FirstName + " " + item.LastName + " " + item.SSOID;
        //    }
        //    return name;
        //}
        public List<AspNetUser> GetAllCoustomerEmployeeList()
        {
            return _db.AspNetUsers.Include("AspNetUserRoles").Where(x => x.SSOID.StartsWith("C") && x.Archived == false).ToList();
        }
        public int SaveResourcetSet(MainResoureViewModel model)
        {
            //AspNetUser AddUser = _db.AspNetUsers.Where(x => x.Id == model.Id).FirstOrDefault();
            AspNetUser AddUser = new AspNetUser();
            //step 1
            AddUser.NameTitle = model.Title;
            AddUser.FirstName = model.FirstName;
            AddUser.LastName = model.LastName;
            AddUser.OtherNames = model.OtherNames;
            AddUser.KnownAs = model.KnownAs;
            AddUser.SSOID =model.SSO.ToUpper();
            AddUser.UserName = model.UserNameEmail;
            AddUser.IMAddress = model.IMAddress;
            AddUser.Gender = model.Gender;
            AddUser.Archived = false;
            AddUser.PasswordHash = model.PasswordHash;
            AddUser.CreatedDate = DateTime.Now;
            AddUser.CreatedBy = SessionProxy.UserId;
            if (model.DateOfBirth != null)
            {
                var DateOfBirthToString = DateTime.ParseExact(model.DateOfBirth, inputFormat, CultureInfo.InvariantCulture);
                AddUser.DateOfBirth = Convert.ToDateTime(DateOfBirthToString.ToString(outputFormat));
            }
            AddUser.Nationality = model.Nationality;
            AddUser.NINumberSSN = model.NIN_SSN;
            AddUser.image = model.Picture;
            AddUser.IsReadAddReport = false;
            AddUser.IsReadArchived = false;
            AddUser.IsReadHRRespo = false;
           
            //Step 2
            if (model.StartDate != null)
            {
                var StartDateToString = DateTime.ParseExact(model.StartDate, inputFormat, CultureInfo.InvariantCulture);
                AddUser.StartDate = Convert.ToDateTime(StartDateToString.ToString(outputFormat));
            }
            AddUser.ResourceType = model.ResourceType;
            //AddUser.Reportsto = model.Reportsto;
            AddUser.AdditionalReportsto = model.AdditionalReportsto;
            AddUser.HRResponsible = model.HRResponsible;
            AddUser.JobTitle = model.JobTitle;
            AddUser.JobContryID = model.JobCountrID;
            AddUser.Location = model.Location;
            //AddUser.BusinessID = model.BusinessID;
            //AddUser.DivisionID = model.DivisionID;
            //AddUser.PoolID = model.PoolID;
            //AddUser.FunctionID = model.FunctionID;

            //step 3
            if (model.ProbationEndDate != null)
            {
                var ProbationEndDateToString = DateTime.ParseExact(model.ProbationEndDate, inputFormat, CultureInfo.InvariantCulture);
                AddUser.ProbationEndDate = Convert.ToDateTime(ProbationEndDateToString.ToString(outputFormat));
            }
            if (model.NextProbationReviewDate != null)
            {
                var NextProbationReviewDateToString = DateTime.ParseExact(model.NextProbationReviewDate, inputFormat, CultureInfo.InvariantCulture);
                AddUser.ProbationEndDate = Convert.ToDateTime(NextProbationReviewDateToString.ToString(outputFormat));
            }
            if (model.FixedTermEndDate != null)
            {
                var FixedTermEndDateToString = DateTime.ParseExact(model.FixedTermEndDate, inputFormat, CultureInfo.InvariantCulture);
                AddUser.FixedTermEndDate = Convert.ToDateTime(FixedTermEndDateToString.ToString(outputFormat));
            }
            AddUser.NoticePeriod = model.NoticePeriodID;
            AddUser.MethodofRecruitmentSetup = model.MethodofRecruitmentSetup;
            AddUser.RecruitmentCost = model.RecruitmentCost.ToString();
           // AddUser.CurrenciesId = model.curruncyID;
            if (model.HolidaysThisYear != null)
                AddUser.Thisyear = (int)model.HolidaysThisYear;
            if (model.HolidaysNextYear != null)
                AddUser.Nextyear = (int)model.HolidaysNextYear;
            if(model.IncludePublicHoliday!=null)
            {
                if (model.IncludePublicHoliday == "on")
                { AddUser.IncludePublicHoliday = true; }
                else
                { AddUser.IncludePublicHoliday = false; }
            }
            if(model.HolidayEntit!=0 && model.HolidayEntit!=null )
            {
                AddUser.HolidayEntitlement = model.HolidayEntit;
            }
            //step 4 
            //AddUser.Country = model.CountryId;
            //AddUser.State = model.StateId;
            //AddUser.Town = model.CityyId;
            //AddUser.Airport = model.AirportId;
            //AddUser.Postcode = model.PostalCode;
            //AddUser.BankAddress = model.Address;
            AddUser.WorkPhone = model.WorkPhone;
            AddUser.WorkMobile = model.WorkMobile;
            //AddUser.PersonalPhone = model.PersonalPhone;
            //AddUser.PersonalMobile = model.PersonalMobile;
            //AddUser.PersonalEmail = model.PersonalEmail;
            //AddUser.BankName = model.BankName;
            //AddUser.BankCode = model.BankCode;
            //AddUser.AccountNumber = model.AccountNumber;
            //AddUser.OtherAccountInformation = model.OtherAccountInformation;
            //AddUser.AccountName = model.AccountName;
            //AddUser.BankAddress = model.BankAddress;
            AddUser.CreatedBy = model.CurrentUserId;

            //Step 5
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<AddnewtaskRecordmodel> listValueArray = js.Deserialize<List<AddnewtaskRecordmodel>>(model.JsonNewtaskList);
            foreach (var item in listValueArray)
            {
                Task_List ModelNew = new Task_List();
                ModelNew.InRelationTo = model.Id;
                ModelNew.Title = item.Title;
                ModelNew.Description = item.Description;
                ModelNew.AssignTo = item.Assign;
                ModelNew.Archived = false;
                ModelNew.AlterBeforeDays = item.AlertBeforeDays;
                if (item.DueDate != "")
                {
                    var DueDateToString = DateTime.ParseExact(item.DueDate, inputFormat, CultureInfo.InvariantCulture);
                    ModelNew.DueDate = Convert.ToDateTime(DueDateToString.ToString(outputFormat));
                }
                ModelNew.Status = item.Status;
                ModelNew.LastModified = DateTime.Now;
                ModelNew.Created = DateTime.Now;
                _db.Task_List.Add(ModelNew);
            }
            _db.AspNetUsers.Add(AddUser);
            _db.SaveChanges();


            if (AddUser.Id > 0)
            {
                //Employee Relation
                EmployeeRelation _employeeRelation = new EmployeeRelation();
                _employeeRelation.UserID = AddUser.Id;
                if (model.Reportsto != null)
                {
                    _employeeRelation.Reportsto = model.Reportsto.Value;
                }
                _employeeRelation.BusinessID = model.BusinessID;
                _employeeRelation.DivisionID = model.DivisionID;
                _employeeRelation.PoolID = model.PoolID;
                _employeeRelation.FunctionID = model.FunctionID;
                _employeeRelation.CreateBy = model.CurrentUserId;
                _employeeRelation.CreatedDate = DateTime.Now;
                _employeeRelation.IsActive = true;
                _db.EmployeeRelations.Add(_employeeRelation);

                //Employee Address Info
                EmployeeAddressInfo _EmployeeAddressInfo = new EmployeeAddressInfo();
                _EmployeeAddressInfo.UserId = AddUser.Id;
                _EmployeeAddressInfo.CountryId = model.CountryId;
                _EmployeeAddressInfo.StateId = model.StateId;
                _EmployeeAddressInfo.TownId = model.CityyId;
                _EmployeeAddressInfo.AirportId = model.AirportId;
                _EmployeeAddressInfo.PostCode = model.PostalCode;
                _EmployeeAddressInfo.ContactAddress = model.Address;
                _EmployeeAddressInfo.PersonalPhone = model.PersonalPhone;
                _EmployeeAddressInfo.PersonalMobile = model.PersonalMobile;
                _EmployeeAddressInfo.PersonalEmail = model.PersonalEmail;
                _db.EmployeeAddressInfoes.Add(_EmployeeAddressInfo);

                EmployeeBankInfo _EmployeeBankInfo = new EmployeeBankInfo();
                _EmployeeBankInfo.UserId = AddUser.Id;
                _EmployeeBankInfo.BankName = model.BankName;
                _EmployeeBankInfo.BankCode = model.BankCode;
                _EmployeeBankInfo.AccountName = model.AccountName;
                _EmployeeBankInfo.AccountNumber = model.AccountNumber;
                _EmployeeBankInfo.OtherAccountInformation = model.OtherAccountInformation;
                _EmployeeBankInfo.BankAddress = model.BankAddress;
                _EmployeeBankInfo.IBAN_No = model.IBAN_Number;
                _EmployeeBankInfo.SWIFT_Code = model.SWIFT_Code;
                _db.EmployeeBankInfoes.Add(_EmployeeBankInfo);
            }

            if (model.ApplicantID != 0 && model.ApplicantID != null)
            {
                var AccepteID = _AdminTMSMethod.GetAcceptedStepId((int)model.ApplicantID);
                var data = _db.TMS_Applicant.Where(x => x.Id == model.ApplicantID).FirstOrDefault();
                data.Archived = true;
                data.StatusID = AccepteID;
                data.LastModified = DateTime.Now;
            }
            _db.SaveChanges();
            return AddUser.Id;
        }
        public List<SelectListItem> BindCountryDropdown()
        {
            List<SelectListItem> model = new List<SelectListItem>();
            var countryList = (from i in _db.Countries
                               select i).ToList();
            model.Add(new SelectListItem { Text = "-- Select --", Value = "0" });
            foreach (var item in countryList)
            {
                model.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            return model;
        }
        
        public List<SelectListItem> bindPublicHolidayCountryList()
        {
            List<SelectListItem> model = new List<SelectListItem>();
            var countryList = (from i in _db.PublicHolidayCountries
                               select i).ToList();
            model.Add(new SelectListItem { Text = "-- Select --", Value = "0" });
            foreach (var item in countryList)
            {
                model.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            return model;

        }
        public List<SelectListItem> BindStateDropdown(int countryId)
        {
            List<SelectListItem> model = new List<SelectListItem>();
            var countryList = (from i in _db.States
                               where i.CountryId == countryId
                               select i).ToList();
            foreach (var item in countryList)
            {
                model.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            return model;
        }
        public List<SelectListItem> BindCityDropdown(int stateId)
        {
            List<SelectListItem> model = new List<SelectListItem>();
            var countryList = (from i in _db.Cities
                               where i.StateId == stateId
                               select i).ToList();
            foreach (var item in countryList)
            {
                model.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            return model;
        }
        public List<SelectListItem> BindAirportDropdown(int countryId)
        {
            List<SelectListItem> model = new List<SelectListItem>();
            var countryList = (from i in _db.Airports
                               select i).ToList();
            foreach (var item in countryList)
            {
                model.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            return model;
        }
        public string Jobtitel(int Id)
        {
            var Company = _db.SystemListValues.Where(x => x.Id == Id).FirstOrDefault();
            return Company.Value;
        }
        public string getResourceType(int Id)
        {
            var Company = _db.SystemListValues.Where(x => x.Id == Id).FirstOrDefault();
            return Company.Value;
        }
        public string GetNationality(int Id)
        {
            var Company = _db.SystemListValues.Where(x => x.Id == Id).FirstOrDefault();
            return Company.Value;
        }
        public string GetCountries(int Id)
        {
            var Country = _db.Countries.Where(x => x.Id == Id).FirstOrDefault();
            return Country.Name;
        }

        public void DeleteResource(int Id)
        {
            AspNetUser Project = _db.AspNetUsers.Where(x => x.Id == Id).FirstOrDefault();
            _db.AspNetUsers.Remove(Project);
            _db.SaveChanges();
        }
        public List<Cmp_Customer> CoustomerCompanyListrecord()
        {
            return _db.Cmp_Customer.ToList();
        }
        public int SaveCoustomertSet(MainResoureViewModel model)
        {
            //AspNetUser AddUser = _db.AspNetUsers.Where(x => x.Id == model.Id).FirstOrDefault();
            AspNetUser AddUser = new AspNetUser();
            //step 1
            AddUser.NameTitle = model.Title;
            AddUser.FirstName = model.FirstName;
            AddUser.LastName = model.LastName;
            AddUser.OtherNames = model.OtherNames;
            AddUser.KnownAs = model.KnownAs;
            AddUser.SSOID = model.SSO;
            AddUser.UserName = model.UserNameEmail;
            AddUser.IMAddress = model.IMAddress;
            AddUser.Gender = model.Gender;
            AddUser.image = model.Picture;
            AddUser.PasswordHash = model.PasswordHash;
            AddUser.CreatedDate = DateTime.Now;
            if (model.DateOfBirth != null)
            {
                var DateOfBirthToString = DateTime.ParseExact(model.DateOfBirth, inputFormat, CultureInfo.InvariantCulture);
                AddUser.DateOfBirth = Convert.ToDateTime(DateOfBirthToString.ToString(outputFormat));
            }
            AddUser.JobTitle = model.JobTitle;
            AddUser.WorkPhone = model.WorkPhone;
            AddUser.WorkMobile = model.WorkMobile;
            AddUser.Archived = false;
            AddUser.CreatedBy = SessionProxy.UserId;
            AddUser.CreatedDate = DateTime.Now;
            AddUser.CustomerCareID = model.CustomerCareId;
            AddUser.SelectCustomerCompanyId = model.SelectCustomerCompanyId;
            _db.AspNetUsers.Add(AddUser);
            _db.SaveChanges();

            return AddUser.Id;
        }
        public void DeleteCoustomerRecord(int Id, int Users)
        {
            MainResoureViewModel model = new MainResoureViewModel();
            AspNetUser _skills = _db.AspNetUsers.Where(x => x.Id == Id).FirstOrDefault();
            _skills.Archived = true;
            _skills.LastModifiedDate = DateTime.Now;
            _skills.CreatedBy = Users;
            _db.SaveChanges();
        }
        public void DeleteWorkerRecord(int Id, int Users)
        {
            MainResoureViewModel model = new MainResoureViewModel();
            AspNetUser _skills = _db.AspNetUsers.Where(x => x.Id == Id).FirstOrDefault();
            _skills.Archived = true;
            _skills.LastModifiedDate = DateTime.Now;
            _skills.CreatedBy = Users;
            _db.SaveChanges();
        }

        public List<Employee_Task_Temp> GetAllTaskRecordTemp()
        {
            return _db.Employee_Task_Temp.Where(x => x.Archived == false).ToList();
        }
        public List<Task_List> GetNewstaskrecord()
        {
            return _db.Task_List.Where(x => x.Difualt == true && x.Archived == false).ToList();
        }
        public void UpdateTaskRecord(AddNewTaskListViewModel model, int UserId)
        {
            if (model.IsTemp==0)
            {
                Task_List record = _db.Task_List.Where(x => x.Id == model.IdRecord).FirstOrDefault();

                record.Title = model.Title;
                record.Description = model.Description;
                record.AssignTo = model.Assign;
                record.LastModifiedBy = UserId;

                if (model.DueDate != null)
                {
                    var DyeDateToString = DateTime.ParseExact(model.DueDate, inputFormat, CultureInfo.InvariantCulture);
                    record.DueDate = Convert.ToDateTime(DyeDateToString.ToString(outputFormat));
                }
                record.AlterBeforeDays = model.AlertBeforeDays;
                record.Status = model.Status;
                record.Difualt = true;
                record.LastModified = DateTime.Now;
                _db.SaveChanges();
            }
            else
            {
                Employee_Task_Temp save = _db.Employee_Task_Temp.Where(x => x.Id == model.IdRecord).FirstOrDefault();
                save.Title = model.Title;
                save.Description = model.Description;
                save.AssignTo = model.Assign;
                if (model.DueDate != null)
                {
                    var DyeDateToString = DateTime.ParseExact(model.DueDate, inputFormat, CultureInfo.InvariantCulture);
                    save.DueDate = Convert.ToDateTime(DyeDateToString.ToString(outputFormat));
                }
                save.AlterBeforeDays = model.AlertBeforeDays;
                save.Status = model.Status;
                save.LastModified = DateTime.Now;
                _db.SaveChanges();
            }

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
                var DyeDateToString = DateTime.ParseExact(Model.DueDate, inputFormat, CultureInfo.InvariantCulture);
                save.DueDate = Convert.ToDateTime(DyeDateToString.ToString(outputFormat));
            }
            _db.Employee_Task_Temp.Add(save);
            _db.SaveChanges();

            Model.Id = save.Id;
            Model.IsTemp = 1;
            return Model;
        }


        public HelpmeCalculeteModel GetPublicHolidayByContryId(string Date, int Id)
        {
            HelpmeCalculeteModel model = new HelpmeCalculeteModel();

            DateTime stdate = new DateTime();
            if (Date != null && Date != "")
            {
                var st = DateTime.ParseExact(Date, inputFormat, CultureInfo.InvariantCulture);
                stdate = Convert.ToDateTime(st.ToString(outputFormat));
                DateTime yearStart = new DateTime(stdate.Year, 1, 1);
                DateTime yeatend = new DateTime(stdate.Year, 12, 31);
                var totalWorkingDays = Weekdays(yearStart, yeatend);
                var remainiingDays = Weekdays(stdate, yeatend);

                var TotalHolidayYear = _db.PublicHolidays.Where(x => x.PublicHolidayCountryID == Id && x.Date >= yearStart && x.Date <= yeatend).ToList();

                var TotalRemainingHolidays = _db.PublicHolidays.Where(x => x.PublicHolidayCountryID == Id && x.Date >= stdate && x.Date <= yeatend).ToList();

                var ExpiredHolidays = _db.PublicHolidays.Where(x => x.PublicHolidayCountryID == Id && x.Date >= yearStart && x.Date <= stdate).ToList();

                model.TotalHolidayYear = TotalHolidayYear.Count;
                model.TotalRemainingHolidays = TotalRemainingHolidays.Count;
                model.ExpiredHolidays = ExpiredHolidays.Count;
                model.TotalHolidayYear = TotalHolidayYear.Count;
                model.totalWorkingDays = totalWorkingDays;
                model.remainiingDays = remainiingDays;
            }
            return model;
        }

        public int Weekdays(DateTime dtmStart, DateTime dtmEnd)
        {
            // This function includes the start and end date in the count if they fall on a weekday
            int dowStart = ((int)dtmStart.DayOfWeek == 0 ? 7 : (int)dtmStart.DayOfWeek);
            int dowEnd = ((int)dtmEnd.DayOfWeek == 0 ? 7 : (int)dtmEnd.DayOfWeek);
            TimeSpan tSpan = dtmEnd - dtmStart;
            if (dowStart <= dowEnd)
            {
                return (((tSpan.Days / 7) * 5) + Math.Max((Math.Min((dowEnd + 1), 6) - dowStart), 0));
            }
            return (((tSpan.Days / 7) * 5) + Math.Min((dowEnd + 6) - Math.Min(dowStart, 6), 5));
        }

        public void DeleteTempTask()
        {
            var data = _db.Employee_Task_Temp.ToList();
            foreach (var item in data)
            {
                _db.Employee_Task_Temp.Remove(item);
                _db.SaveChanges();
            }
        }

        public BradfordFactor_HolidaySettings GetBradFordFactorDetails()
        {
            return _db.BradfordFactor_HolidaySettings.FirstOrDefault();
        }

        public string GetHolidayYear()
        {
            string HolidayYear = string.Empty;
            var HolidayYearSetting = _db.HolidayYearAndMonthSettings.Where(x => x.IsActive == true).FirstOrDefault();
            if(HolidayYearSetting != null)
            {
                if(HolidayYearSetting.StartYear < HolidayYearSetting.EndYear)
                {
                    HolidayYear = Convert.ToString(HolidayYearSetting.StartYear) + " - " + Convert.ToString(HolidayYearSetting.EndYear);
                }
                else
                {
                    HolidayYear = Convert.ToString(HolidayYearSetting.StartYear);
                }
            }
            return HolidayYear;
        }

        public WorkpatternWeekend GetAllsickLeaveDayCount(int EmployeeId)
        {
            WorkpatternWeekend _WorkpatternWeekend = new WorkpatternWeekend();
            _WorkpatternWeekend.SundayDays = 0;
            _WorkpatternWeekend.MondayDays = 0;
            _WorkpatternWeekend.TuesdayDays = 0;
            _WorkpatternWeekend.WednessdayDays = 0;
            _WorkpatternWeekend.ThursdayDays = 0;
            _WorkpatternWeekend.FridayDays = 0;
            _WorkpatternWeekend.SaturdayDays = 0;

            if (EmployeeId != null && EmployeeId > 0)
            {
                List<Employee_SickLeaves> SickLeave = _db.Employee_SickLeaves.Where(x => x.EmployeeId == EmployeeId).ToList();
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

            }
            return _WorkpatternWeekend;
        }

        public decimal GetBradFordFactorCount(int EmployeeId)
        {
            decimal BradFactorCount = 0;
            if (EmployeeId != null && EmployeeId > 0)
            {
                List<Employee_SickLeaves> SickLeave = _db.Employee_SickLeaves.Where(x => x.EmployeeId == EmployeeId).ToList();
                int AbsenceCount = SickLeave.Count();
                decimal DayAbsentCount = 0;
                foreach (var item in SickLeave)
                {
                    if (item.EndDate != null)
                    {
                        DayAbsentCount += item.DurationDays != null ? item.DurationDays.Value : 0;
                    }
                    else
                    {
                        DayAbsentCount += item.DurationHours != null ? item.DurationHours.Value : 0;
                    }
                }
                BradFactorCount = AbsenceCount * DayAbsentCount * DayAbsentCount;
            }

            return BradFactorCount;
        }

        
    }
}