using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRTool.DataModel;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HRTool.Models.BulkActions;

namespace HRTool.CommanMethods.BulkActions
{
    public class BulkActionsMethod
    {
        EvolutionEntities _db = new EvolutionEntities();
        private string inputFormat = "dd-MM-yyyy";
        private string outputFormat = "yyyy-MM-dd HH:mm:ss";

        public void saveActivityTypeData(int EmployeeId, int Year, string name, int currencyId, int workUnitId, decimal WorkerRate, decimal customerRate)
        {
            _db.SaveAllActivityTypeData(EmployeeId, Year, name, currencyId, workUnitId, WorkerRate, customerRate);
        }
        public List<Currency> GetCurrencyListRecord()
        {
            return _db.Currencies.ToList();
        }
        public List<GetCurrenciesIsFixData_Result> getIsFixedCurr()
        {

            return _db.GetCurrenciesIsFixData().ToList();
        }
        public List<GetCurrenciesFixedRateData_Result> getCurruncyFixedRate(int id)
        {
            return _db.GetCurrenciesFixedRateData(id).ToList();
        }
        public void saveSalaryDate(int EmployeeId, DateTime EffectiveFrom, int SalaryType, int PaymentFrequancy,
        string Amount, int Curruncy, string TotalSalary, int ReasonForChange, string comment)
        {
            _db.SaveAllSalaryData(EmployeeId, EffectiveFrom, SalaryType, PaymentFrequancy,
            Amount, Curruncy, TotalSalary, ReasonForChange, comment);
        }

        public void SaveBulkSalarySet(AddSalaryViewModel model, int userId,int EmpId)
        {
            Employee_Salary _salary = _db.Employee_Salary.Where(x => x.Id == model.Id).FirstOrDefault();
            if (_salary != null)
            {

                var ProbationEndDateToString = DateTime.ParseExact(model.EffectiveFrom, inputFormat, CultureInfo.InvariantCulture);
                _salary.EffectiveFrom = Convert.ToDateTime(ProbationEndDateToString.ToString(outputFormat));
                _salary.EmployeeID = EmpId;
                _salary.SalaryType = model.SalaryTypeID;
                _salary.PaymentFrequency = model.PaymentFrequencyID;
                _salary.Archived = false;
                _salary.Amount = Convert.ToString(model.Amount);
                _salary.Currency = model.CurrencyID;
                _salary.TotalSalary = model.TotalSalary;
                _salary.Comments = model.Comments;
                _salary.ReasonforChange = model.ReasonforChange;
                _salary.LastModificationDate = DateTime.Now;
                _db.SaveChanges();
            }
            else
            {
                Employee_Salary _salarys = new Employee_Salary();
                var ProbationEndDateToString = DateTime.ParseExact(model.EffectiveFrom, inputFormat, CultureInfo.InvariantCulture);
                _salarys.EffectiveFrom = Convert.ToDateTime(ProbationEndDateToString.ToString(outputFormat));
                _salarys.EmployeeID = EmpId;
                _salarys.SalaryType = model.SalaryTypeID;
                _salarys.PaymentFrequency = model.PaymentFrequencyID;
                _salarys.Archived = false;
                _salarys.Amount = Convert.ToString(model.Amount);
                _salarys.Currency = model.CurrencyID;
                _salarys.TotalSalary = model.TotalSalary;
                _salarys.ReasonforChange = model.ReasonforChange;
                _salarys.Comments = model.Comments;
                _salarys.CreateDate = DateTime.Now;
                _salarys.UserIDCreatedBy = SessionProxy.UserId;
                _db.Employee_Salary.Add(_salarys);
                _db.SaveChanges();

                var Deduction = _db.Employee_Salary_Deduction_Temp.Where(x => x.Archived == false).ToList();
                if (Deduction.Count > 0)
                {
                    foreach (var item in Deduction)
                    {
                        Employee_Salary_Deduction insert = new Employee_Salary_Deduction();
                        insert.Deduction = item.Deduction;
                        insert.EmployeeSalaryID = _salarys.Id;
                        insert.FixedAmount = item.FixedAmount;
                        insert.PercentOfSalary = item.PercentOfSalary;
                        insert.IncludeInSalary = item.IncludeInSalary;
                        insert.Comments = item.Comments;
                        insert.CreatedDate = DateTime.Now;
                        insert.LastModified = DateTime.Now;
                        insert.UserIDCreatedBy = userId;
                        insert.UserIDLastModifiedBy = userId;
                        _db.Employee_Salary_Deduction.Add(insert);
                        _db.SaveChanges();

                        // if (insert.IncludeInSalary == true)
                        //  {
                        //SaveTotalSalaryADDEdit(insert.EmployeeSalaryID, (-Convert.ToDecimal(insert.FixedAmount)));

                        // }

                    }

                }
                var Entitlement = _db.Employee_Salary_Entitlement_Temp.Where(x => x.Archived == false).ToList();
                if (Entitlement.Count > 0)
                {
                    foreach (var item in Entitlement)
                    {
                        Employee_Salary_Entitlements inserts = new Employee_Salary_Entitlements();
                        inserts.Entitlement = item.Entitlement;
                        inserts.EmployeeSalaryID = _salarys.Id;
                        inserts.FixedAmount = item.FixedAmount;
                        inserts.PercentOfSalary = item.PercentOfSalary;
                        inserts.IncludeInSalary = item.IncludeInSalary;
                        inserts.Comments = item.Comments;
                        inserts.CreatedDate = DateTime.Now;
                        inserts.LastModified = DateTime.Now;
                        inserts.UserIDCreatedBy = userId;
                        inserts.UserIDLastModifiedBy = userId;
                        _db.Employee_Salary_Entitlements.Add(inserts);
                        _db.SaveChanges();

                        //  if (inserts.IncludeInSalary == true)
                        //  {
                        //      SaveTotalSalaryADDEdit(inserts.EmployeeSalaryID, Convert.ToDecimal(inserts.FixedAmount));

                        //   }

                    }

                }

            }
        }

        public void SaveTrainingdData(BulkActionTraining model, int userId)
        {
            string[] values = model.EmployeeId.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                EmployeeTraining Training = new EmployeeTraining();
                Training.EmployeeId = Convert.ToInt32(values[i]);
                Training.TrainingNameId = model.TrainingNameId;
                Training.Description = model.Description;
                Training.Importance = model.Importance;
                Training.Status = model.Status;
                Training.StartDate = DateTime.ParseExact(model.StartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture); ;
                Training.EndDate = DateTime.ParseExact(model.EndDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture); ;
                Training.ExpiryDate = DateTime.ParseExact(model.ExpiryDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                Training.Provider = model.Provider;
                Training.Progress = model.Progress;
                Training.Cost = model.Cost;
                Training.Notes = model.Notes;
                Training.Archived = false;
                Training.IsRead = false;
                Training.IsReadAddRep = false;
                Training.IsReadHR = false;
                Training.IsReadWorker = false;
                Training.UserIDCreatedBy = userId;
                Training.UserIDLastModifiedBy = userId;
                Training.CreatedDate = DateTime.Now;
                Training.CustomFieldsJSON = model.CustomFieldsJSON;
                _db.EmployeeTrainings.Add(Training);
                _db.SaveChanges();
                foreach (var item in model.ListDocument)
                {
                    Training_Document TrainingDocument = new Training_Document();
                    TrainingDocument.TrainingId = Training.Id;
                    TrainingDocument.NewName = item.newName;
                    TrainingDocument.OriginalName = item.originalName;
                    TrainingDocument.Description = item.description;
                    TrainingDocument.Archived = false;
                    TrainingDocument.UserIDCreatedBy = userId;
                    TrainingDocument.CreatedDate = DateTime.Now;
                    TrainingDocument.UserIDLastModifiedBy = userId;
                    TrainingDocument.LastModified = DateTime.Now;
                    _db.Training_Document.Add(TrainingDocument);
                    _db.SaveChanges();
                }
            }
        }

        //public void saveTrainingData(int EmployeeId,int TrainingNameId,string Description,int Importance ,int? status,DateTime startDate,DateTime emdDate ,DateTime expDate ,string provider,decimal? cost ,string note ,string CustomFieldsJSON,string attachment)
        //{
        //    _db.SaveAllTraningData(EmployeeId,TrainingNameId,Description,Importance,status,startDate,emdDate, expDate,provider, cost,  note, CustomFieldsJSON,attachment);
        //}
        public Benefit getBrnifitById(int Id)
        {
            return _db.Benefits.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
        }
        public List<Benefits_Documents> getBenifitDocumentByCaseId(int Id)
        {
            return _db.Benefits_Documents.Where(x => x.BenefitsID == Id).ToList();
        }
        public void SaveBenefitData(BenefitsViewModel model, List<BenefitsDocumentViewModel> documentList, int userId)
        {
            string[] values = model.EmployeeID.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                Benefit benfit = new Benefit();
                benfit.EmployeeID = Convert.ToInt32(values[i]);
                benfit.BenefitID = model.BenefitID;
                benfit.Currency = model.Currency;
                var DateAwardedToString = DateTime.ParseExact(model.DateAwarded, inputFormat, CultureInfo.InvariantCulture);
                benfit.DateAwarded = Convert.ToDateTime(DateAwardedToString.ToString(outputFormat));
                var ExpiryDateToString = DateTime.ParseExact(model.ExpiryDate, inputFormat, CultureInfo.InvariantCulture);
                benfit.ExpiryDate = Convert.ToDateTime(ExpiryDateToString.ToString(outputFormat));
                benfit.FixedAmount = model.FixedAmount;
                benfit.RecoverOnTermination = model.RecoverOnTermination;
                benfit.Comments = model.Comments;
                benfit.Currency = model.Currency;
                benfit.Archived = false;
                benfit.UserIDCreatedBy = userId;
                benfit.CreatedDate = DateTime.Now;
                benfit.UserIDLastModifiedBy = userId;
                benfit.LastModified = DateTime.Now;
                _db.Benefits.Add(benfit);
                _db.SaveChanges();

                foreach (var item in documentList)
                {
                    Benefits_Documents benfitDocument = new Benefits_Documents();
                    benfitDocument.BenefitsID = benfit.Id;
                    benfitDocument.NewName = item.newName;
                    benfitDocument.OriginalName = item.originalName;
                    benfitDocument.Description = "";
                    benfitDocument.Archived = false;
                    benfitDocument.UserIDCreatedBy = userId;
                    benfitDocument.CreatedDate = DateTime.Now;
                    benfitDocument.UserIDLastModifiedBy = userId;
                    benfitDocument.LastModified = DateTime.Now;
                    _db.Benefits_Documents.Add(benfitDocument);
                    _db.SaveChanges();
                }
            }
        }

        public List<getTotalTimesheetDuration_Result> getTotalTimeSheetDuration()
        {
            return _db.getTotalTimesheetDuration().ToList();
        }
        public void TimeSheet_SaveData(EmployeePlanner_TimeSheetViewModel model, int UserId)
        {
            string[] values = model.EmployeeId.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                Employee_TimeSheet timesheet = new Employee_TimeSheet();
                timesheet.EmployeeId = Convert.ToInt32(values[i]);
                //var DateToString = DateTime.ParseExact(model.Date, inputFormat, CultureInfo.InvariantCulture);
                //     timesheet.Date = Convert.ToDateTime(DateToString.ToString(outputFormat));
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

        public void saveSchedulingData(int EmpId, bool? isLessDay, bool? IsDayorMore, DateTime? startDate, DateTime? endDate,
        decimal? duration, int? customer, int? project, int? asset, string comment, decimal? HrSD, decimal? MinSD, DateTime? CDate, decimal? HrED, decimal? MinED, decimal? durationHr)
        {
            _db.SaveAllSchadulingData(EmpId, isLessDay, IsDayorMore, startDate, endDate, duration, customer, project, asset, comment, HrSD, MinSD, CDate, HrED, MinED, durationHr);
        }
        public void BookHoliday_saveData(HolidayViewModel model)
        {
            string[] values = model.EmployeeId.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                Employee_AnualLeave annualLeave = new Employee_AnualLeave();
                annualLeave.EmployeeId = Convert.ToInt32(values[i]);
                annualLeave.IsLessThenADay = model.IsLessThenADay;
                annualLeave.Duration = model.Duration;
                annualLeave.TOIL = false;//as per document
                var StartDateToString = DateTime.ParseExact(model.startDate, inputFormat, CultureInfo.InvariantCulture);
                annualLeave.StartDate = Convert.ToDateTime(StartDateToString.ToString(outputFormat));
                if (model.IsLessThenADay == false)
                {
                    var endDateToString = DateTime.ParseExact(model.endDate, inputFormat, CultureInfo.InvariantCulture);
                    annualLeave.EndDate = Convert.ToDateTime(endDateToString.ToString(outputFormat));
                }
                else
                {
                    var endDateToString = DateTime.ParseExact(model.startDate, inputFormat, CultureInfo.InvariantCulture);
                    annualLeave.EndDate = Convert.ToDateTime(endDateToString.ToString(outputFormat));
                    annualLeave.PartOfTheDay = Convert.ToInt32(model.PartOfDay);
                }
                annualLeave.Comment = model.comment;
                annualLeave.Archived = false;
                annualLeave.ApprovalStatus = "Pending";
                annualLeave.IsRead = false;
                annualLeave.IsReadAddRep = false;
                annualLeave.IsReadHR = false;
                annualLeave.CreatedBy = SessionProxy.UserId;
                annualLeave.CreatedDate = DateTime.Now;
                annualLeave.LastModifiedBy = SessionProxy.UserId;
                annualLeave.LastModifiedDate = DateTime.Now;
                _db.Employee_AnualLeave.Add(annualLeave);
                _db.SaveChanges();

            }
        }
        public void TravelLeave_SaveData(EmployeeProjectPlanner_TravelLeaveViewModel model, int UserId)
        {
            string[] values = model.EmployeeId.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                Employee_TravelLeave Leave = new Employee_TravelLeave();
                Leave.EmployeeId = Convert.ToInt32(values[i]);
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
                Leave.IsRead = false;
                Leave.IsReadAddReport = false;
                Leave.IsReadHR = false;
                Leave.Archived = false;
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

        public List<Business> getBussiness()
        {
            return _db.Businesses.ToList();
        }
        public void updateHolidayThisYear(int empId, int thisYear)
        {
            _db.UpHolidayEntitlThisYearData(empId, thisYear);
        }
        public void updateHolidayNextYear(int empId, int thisYear)
        {
            _db.UpHolidayEntitlNextYearData(empId, thisYear);
        }
        public List<EmployeeHolidayEntitlementsThisYear> getEmpDataThisYear(string empId)
        {

            //EmployeeHolidayEntitlementsThisYear model = new EmployeeHolidayEntitlementsThisYear();
            var empIdArray = empId.Split(',').Select(Int32.Parse).ToList();
            List<EmployeeHolidayEntitlementsThisYear> empIDList = _db.AspNetUsers.Where(x => empIdArray.Contains(x.Id)).Select(xx => new EmployeeHolidayEntitlementsThisYear {
                EmpId = xx.Id,
                EmpName = xx.FirstName + " " + xx.LastName,
                current = xx.Thisyear != null ? xx.Thisyear.Value : 0,
            }).ToList();
            return empIDList;
        }
        public List<EmployeeHolidayEntitNextYear> getEmpDataNextYear(string empId)
        {

            //EmployeeHolidayEntitlementsThisYear model = new EmployeeHolidayEntitlementsThisYear();
            var empIdArray = empId.Split(',').Select(Int32.Parse).ToList();
            List<EmployeeHolidayEntitNextYear> empIDList = _db.AspNetUsers.Where(x => empIdArray.Contains(x.Id)).Select(xx => new EmployeeHolidayEntitNextYear
            {
                EmpId = xx.Id,
                EmpName = xx.FirstName + " " + xx.LastName,
                current = xx.Nextyear != null ? xx.Nextyear.Value : 0,
            }).ToList();
            return empIDList;
        }
        public List<Division> getDivision(int busId)
        {
            return _db.Divisions.Where(x => x.BusinessID == busId).ToList();
        }
        public List<GetAllEmployeeCountry_Result> getEmployeeCountry()
        {
            return _db.GetAllEmployeeCountry().ToList();
        }
        public List<GetAllEmployeeJobTitle_Result> getEmployeeJobTitle()
        {
            return _db.GetAllEmployeeJobTitle().ToList();
        }
        public List<GetAllEmployeeSearchData_Result> getSearchData()
        {
            return _db.GetAllEmployeeSearchData().ToList();
        }
        //public List<GetEmployeeSearchList_Result> getSearchList(int bid,int divId)
        //{
        //    return _db.GetEmployeeSearchList(bid, divId).ToList();
        //}
        public List<GetEmployeeSearchList_Result> getSearchList(int bid, int divId, int poolId, int funId, int jobId, int counId)
        {
            return _db.GetEmployeeSearchList(bid, divId, poolId, funId, jobId, counId).Where(x=>x.ASPSSOID.StartsWith("W")).ToList();
        }
        public IList<Project> getAllList()
        {
            return _db.Projects.ToList();
        }
        public SystemList getSystemListByName(string Name)
        {
            return _db.SystemLists.Where(x => x.SystemListName == Name).FirstOrDefault();
        }
        public List<SystemListValue> getAllSystemValueListByNameId(int Id)
        {
            return _db.SystemListValues.Where(x => x.Archived == false && x.SystemListID == Id).ToList();
        }
        public List<SystemListValue> getAllSystemValueListByKeyName(string KeyName)
        {
            SystemList systemName = getSystemListByName(KeyName);
            return getAllSystemValueListByNameId(systemName.Id);
        }
        public IList<AspNetUser> getAllCustomer()
        {
            return _db.AspNetUsers.Where(x => x.Archived == false && x.SSOID.StartsWith("C")).ToList();
            //return _db.Cmp_Customer.ToList();
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
        public List<SelectListItem> BindStateDropdown(int countryId)
        {
            List<SelectListItem> model = new List<SelectListItem>();
            var countryList = (from i in _db.States
                               where i.CountryId == countryId
                               select i).ToList();
            model.Add(new SelectListItem { Text = "-- Select --", Value = "0" });
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
            model.Add(new SelectListItem { Text = "-- Select --", Value = "0" });
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
            model.Add(new SelectListItem { Text = "-- Select --", Value = "0" });
            foreach (var item in countryList)
            {
                model.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            return model;
        }
        public List<Employee_ProjectPlanner_TravelLeave> getAllTravelLeave()
        {
            return _db.Employee_ProjectPlanner_TravelLeave.Where(x => x.Archived == false).ToList();
        }
        public Employee_ProjectPlanner_TravelLeave getTravelLeaveById(int Id)
        {
            return _db.Employee_ProjectPlanner_TravelLeave.Where(x => x.Id == Id).FirstOrDefault();
        }

        public SystemListValue getSystemListValueById(int Id)
        {
            return _db.SystemListValues.Where(x => x.Id == Id).FirstOrDefault();
        }
        public void SavesalarySetTemp(AddSalaryViewModel model)
        {
            if (model.EmployeeId != "" && model.EmployeeId != null)
            {
                string[] str = model.EmployeeId.Split(',');
                for (int i = 0; i < str.Length; i++)
                {
                    int eid = Convert.ToInt32(str[i]);
                    var data = _db.Employee_Salary_Temp.Where(x => x.Id == model.Id).ToList();
                    if (data.Count > 0)
                    {

                        Employee_Salary_Temp _salary = _db.Employee_Salary_Temp.Where(x => x.Id == model.Id).FirstOrDefault();
                        if (model.EffectiveFrom != null)
                        {
                            var ProbationEndDateToString = DateTime.ParseExact(model.EffectiveFrom, inputFormat, CultureInfo.InvariantCulture);
                            _salary.EffectiveFrom = Convert.ToDateTime(ProbationEndDateToString.ToString(outputFormat));
                        }
                        _salary.EmployeeID = eid;
                        _salary.SalaryType = model.SalaryTypeID;
                        _salary.PaymentFrequency = model.PaymentFrequencyID;
                        _salary.Archived = false;
                        _salary.Amount = Convert.ToString(model.Amount);
                        _salary.Currency = model.CurrencyID;
                        _salary.TotalSalary = model.TotalSalary;
                        _salary.Comments = model.Comments;
                        _salary.ReasonforChange = model.ReasonforChange;
                        _salary.LastModificationDate = DateTime.Now;
                        _db.SaveChanges();
                    }
                    else
                    {
                        Employee_Salary_Temp _salary = new Employee_Salary_Temp();
                        if (model.EffectiveFrom != null)
                        {
                            var ProbationEndDateToString = DateTime.ParseExact(model.EffectiveFrom, inputFormat, CultureInfo.InvariantCulture);
                            _salary.EffectiveFrom = Convert.ToDateTime(ProbationEndDateToString.ToString(outputFormat));
                        }
                        _salary.Id = model.Id;
                        _salary.EmployeeID = eid;
                        _salary.SalaryType = model.SalaryTypeID;
                        _salary.PaymentFrequency = model.PaymentFrequencyID;
                        _salary.Archived = false;
                        _salary.Amount = Convert.ToString(model.Amount);
                        _salary.Currency = model.CurrencyID;
                        _salary.TotalSalary = model.TotalSalary;
                        _salary.ReasonforChange = model.ReasonforChange;
                        _salary.Comments = model.Comments;
                        _salary.CreateDate = DateTime.Now;
                        _salary.UserIDCreatedBy = Convert.ToInt32(model.CurrentUserId);
                        _db.Employee_Salary_Temp.Add(_salary);
                        _db.SaveChanges();
                    }

                }
            }
        }
        public void SavesalaryDeductionSet(AddSalaryDeductionViewModel model, int userId)
        {
            if (model.Id > 0)
            {
                Employee_Salary_Deduction _salary = _db.Employee_Salary_Deduction.Where(x => x.Id == model.Id).FirstOrDefault();
                _salary.Deduction = model.DeductionID;
                if (_salary.IncludeInSalary)
                {
                    decimal diff = _salary.FixedAmount - model.FixedAmount;
                    if (diff == 0)
                    {
                        diff = model.FixedAmount;
                        SaveTotalSalaryADDEdit(_salary.EmployeeSalaryID, diff);
                    }
                    else
                    {
                        if (model.IncludeInSalary == true)
                        {
                            SaveTotalSalaryADDEdit(_salary.EmployeeSalaryID, diff);
                        }
                        else
                        {
                            diff = _salary.FixedAmount;
                            SaveTotalSalaryADDEdit(_salary.EmployeeSalaryID, diff);

                        }
                    }

                }
                else
                {
                    if (model.IncludeInSalary == true)
                    {
                        decimal diff = -model.FixedAmount;
                        SaveTotalSalaryADDEdit(_salary.EmployeeSalaryID, diff);
                    }
                }
                //var diff = _salary.FixedAmount - model.FixedAmount;
                _salary.EmployeeSalaryID = model.EmployeeSalaryID;
                _salary.FixedAmount = model.FixedAmount;
                _salary.PercentOfSalary = model.PercentOfSalary;
                _salary.IncludeInSalary = model.IncludeInSalary;
                _salary.Comments = model.Comments;
                _salary.LastModified = DateTime.Now;
                _salary.UserIDLastModifiedBy = userId;
                _db.SaveChanges();
                //if (_salary.IncludeInSalary == true)
                //{

                //    var data = _db.Employee_Salary.Where(x => x.Id == _salary.EmployeeSalaryID).FirstOrDefault();
                //    var total = Convert.ToDecimal(data.TotalSalary.Replace(",", "")) + diff;
                //    data.TotalSalary = string.Format("{0:#,###0}", total);
                //    //data.Amount = string.Format("{0:#,###0}", total);
                //    _db.SaveChanges();
                //}

            }
            else
            {
                Employee_Salary_Deduction _salary = new Employee_Salary_Deduction();
                _salary.Deduction = model.DeductionID;
                _salary.EmployeeSalaryID = model.EmployeeSalaryID;
                _salary.FixedAmount = model.FixedAmount;
                _salary.PercentOfSalary = model.PercentOfSalary;
                _salary.IncludeInSalary = model.IncludeInSalary;
                _salary.Comments = model.Comments;
                _salary.CreatedDate = DateTime.Now;
                _salary.LastModified = DateTime.Now;
                _salary.UserIDCreatedBy = userId;
                _salary.UserIDLastModifiedBy = userId;
                _db.Employee_Salary_Deduction.Add(_salary);
                _db.SaveChanges();

                if (_salary.IncludeInSalary == true)
                {
                    SaveTotalSalaryADDEdit(_salary.EmployeeSalaryID, (-Convert.ToDecimal(_salary.FixedAmount)));

                }
            }
        }

        public void SaveTotalSalaryADDEdit(int SID, decimal diff)
        {
            var data = _db.Employee_Salary.Where(x => x.Id == SID).FirstOrDefault();
            var total = Convert.ToDecimal(data.TotalSalary.Replace(",", "")) + diff;
            data.TotalSalary = string.Format("{0:#,###0}", total);
            _db.SaveChanges();
        }

        public void SavesalaryDeductionSetTemp(AddSalaryDeductionViewModel model, int userId)
        {
            if (model.Id > 0)
            {
                Employee_Salary_Deduction_Temp _salary = _db.Employee_Salary_Deduction_Temp.Where(x => x.Id == model.Id).FirstOrDefault();
                _salary.Deduction = model.DeductionID;

                if (_salary.IncludeInSalary)
                {
                    decimal diff = _salary.FixedAmount - model.FixedAmount;
                    if (diff == 0)
                    {
                        diff = model.FixedAmount;
                        SaveTotalSalaryDeductionTemp(_salary.EmployeeSalaryID, diff);
                    }
                    else
                    {
                        if (model.IncludeInSalary == true)
                        {
                            SaveTotalSalaryDeductionTemp(_salary.EmployeeSalaryID, diff);
                        }
                        else
                        {
                            diff = _salary.FixedAmount;
                            SaveTotalSalaryDeductionTemp(_salary.EmployeeSalaryID, diff);

                        }
                    }

                }
                else
                {
                    if (model.IncludeInSalary == true)
                    {
                        decimal diff = -model.FixedAmount;
                        SaveTotalSalaryDeductionTemp(_salary.EmployeeSalaryID, diff);
                    }
                }
                _salary.EmployeeSalaryID = model.EmployeeSalaryID;
                _salary.Archived = false;
                _salary.FixedAmount = model.FixedAmount;
                _salary.PercentOfSalary = model.PercentOfSalary;
                _salary.IncludeInSalary = model.IncludeInSalary;
                _salary.Comments = model.Comments;
                _salary.LastModified = DateTime.Now;
                _salary.UserIDLastModifiedBy = userId;
                _db.SaveChanges();

            }
            else
            {
                Employee_Salary_Deduction_Temp _salary = new Employee_Salary_Deduction_Temp();
                _salary.Id = model.Id;
                _salary.Deduction = model.DeductionID;
                _salary.EmployeeSalaryID = model.EmployeeSalaryID;
                _salary.FixedAmount = model.FixedAmount;
                _salary.PercentOfSalary = model.PercentOfSalary;
                _salary.IncludeInSalary = model.IncludeInSalary;
                _salary.Comments = model.Comments;
                _salary.Archived = false;
                _salary.CreatedDate = DateTime.Now;
                _salary.LastModified = DateTime.Now;
                _salary.UserIDCreatedBy = userId;
                _salary.UserIDLastModifiedBy = userId;
                _db.Employee_Salary_Deduction_Temp.Add(_salary);
                _db.SaveChanges();

                if (_salary.IncludeInSalary == true)
                {
                    //var data = _db.Employee_Salary_Temp.Where(x => x.Id == _salary.EmployeeSalaryID).FirstOrDefault();
                    //if (data != null)
                    //{
                    //    var total = Convert.ToDecimal(data.TotalSalary.Replace(",", "")) - Convert.ToDecimal(_salary.FixedAmount);
                    //    data.TotalSalary = string.Format("{0:#,###0}", total);
                    //    _db.SaveChanges();
                    //}
                    SaveTotalSalaryDeductionTemp(_salary.EmployeeSalaryID, (-Convert.ToDecimal(_salary.FixedAmount)));
                    // data.Amount = string.Format("{0:#,###0}", total);

                }
            }
        }
        public void SaveTotalSalaryDeductionTemp(int SID, decimal diff)
        {

            var data = _db.Employee_Salary_Temp.Where(x => x.Id == SID).FirstOrDefault();
            if (data != null)
            {
                var total = Convert.ToDecimal(data.TotalSalary.Replace(",", "")) + diff;
                data.TotalSalary = string.Format("{0:#,###0}", total);
                _db.SaveChanges();
            }
        }
        public Employee_Salary_Temp GetsalaryTempListById(int Id)
        {
            return _db.Employee_Salary_Temp.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
        }
        public Employee_Salary GetsalaryListById(int Id)
        {
            return _db.Employee_Salary.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
        }
        public void SavesalaryEntitlementSet(AddSalaryEntitlementViewModel model, int userId)
        {
            if (model.EmployeeID != "" && model.EmployeeID != null)
            {
                string[] EmpId = model.EmployeeID.Split(',');
                for (int i = 0; i < EmpId.Length; i++)
                {
                    if (model.Id > 0)
                    {
                        Employee_Salary_Entitlement_Temp _salary = _db.Employee_Salary_Entitlement_Temp.Where(x => x.Id == model.Id).FirstOrDefault();
                        //var diff = _salary.FixedAmount - model.FixedAmount;
                        if (_salary.IncludeInSalary)
                        {
                            decimal diff = _salary.FixedAmount - model.FixedAmount;
                            if (diff == 0)
                            {
                                diff = -model.FixedAmount;
                                SaveTotalSalaryDeductionTemp(_salary.EmployeeSalaryID, diff);
                            }
                            else
                            {
                                if (model.IncludeInSalary == true)
                                {
                                    SaveTotalSalaryDeductionTemp(_salary.EmployeeSalaryID, -diff);
                                }
                                else
                                {
                                    diff = -_salary.FixedAmount;
                                    SaveTotalSalaryDeductionTemp(_salary.EmployeeSalaryID, diff);

                                }
                            }

                        }
                        else
                        {
                            if (model.IncludeInSalary == true)
                            {
                                decimal diff = model.FixedAmount;
                                SaveTotalSalaryDeductionTemp(_salary.EmployeeSalaryID, diff);
                            }
                        }


                        _salary.Archived = false;
                        _salary.Entitlement = model.EntitlementID;
                        _salary.EmployeeSalaryID = model.EmployeeSalaryID;
                        _salary.FixedAmount = model.FixedAmount;
                        _salary.PercentOfSalary = model.PercentOfSalary;
                        _salary.IncludeInSalary = model.IncludeInSalary;
                        _salary.Comments = model.Comments;
                        _salary.LastModified = DateTime.Now;
                        _salary.UserIDLastModifiedBy = userId;
                        _db.SaveChanges();

                        //if (_salary.IncludeInSalary == true)
                        //{
                        //    var data = _db.Employee_Salary_Temp.Where(x => x.Id == _salary.EmployeeSalaryID).FirstOrDefault();
                        //    if (data != null)
                        //    {
                        //        var total = Convert.ToDecimal(data.TotalSalary.Replace(",", "")) - diff;
                        //        data.TotalSalary = string.Format("{0:#,###0}", total);
                        //        _db.SaveChanges();
                        //    }
                        //}
                    }
                    else
                    {
                        Employee_Salary_Entitlement_Temp _salary = new Employee_Salary_Entitlement_Temp();
                        _salary.Entitlement = model.EntitlementID;
                        _salary.EmployeeSalaryID = model.EmployeeSalaryID;
                        _salary.FixedAmount = model.FixedAmount;
                        _salary.Archived = false;
                        _salary.PercentOfSalary = model.PercentOfSalary;
                        _salary.IncludeInSalary = model.IncludeInSalary;
                        _salary.Comments = model.Comments;
                        _salary.CreatedDate = DateTime.Now;
                        _salary.LastModified = DateTime.Now;
                        _salary.UserIDCreatedBy = userId;
                        _salary.UserIDLastModifiedBy = userId;
                        _db.Employee_Salary_Entitlement_Temp.Add(_salary);
                        _db.SaveChanges();

                        if (_salary.IncludeInSalary == true)
                        {
                            SaveTotalSalaryDeductionTemp(_salary.EmployeeSalaryID, Convert.ToDecimal(_salary.FixedAmount));

                            //var data = _db.Employee_Salary_Temp.Where(x => x.Id == _salary.EmployeeSalaryID).FirstOrDefault();
                            //if (data != null)
                            //{
                            //    var total = Convert.ToDecimal(data.TotalSalary.Replace(",", "")) + Convert.ToDecimal(_salary.FixedAmount);
                            //    data.TotalSalary = string.Format("{0:#,###0}", total);
                            //    _db.SaveChanges();
                            //}
                        }
                    }
                }
            }
        }

        public void saveBulkResource(IList<AddResourceBulk> data)
        {
            AspNetUser AddUser = new AspNetUser();
            foreach (var model in data)
            {
                //step 1
                AddUser.NameTitle = model.NameTitle;
                AddUser.FirstName = model.FirstName;
                AddUser.LastName = model.LastName;
                //AddUser.OtherNames = model.OtherNames;
                //AddUser.KnownAs = model.KnownAs;
                AddUser.SSOID = model.SSOID.ToUpper();
                AddUser.UserName = model.UserName;
                //AddUser.IMAddress = model.IMAddress;
                AddUser.Gender = model.Gender;
                AddUser.Archived = false;
                //AddUser.PasswordHash = model.PasswordHash;
                if (model.DateOfBirth != null)
                {
                    //var DateOfBirthToString = DateTime.ParseExact(model.DateOfBirth, inputFormat, CultureInfo.InvariantCulture);
                    //AddUser.DateOfBirth = Convert.ToDateTime(DateOfBirthToString.ToString(outputFormat));
                    AddUser.DateOfBirth = model.DateOfBirth;
                }
                //AddUser.Nationality = model.Nationality;
                //AddUser.NINumberSSN = model.NINumberSSN;
                //AddUser.image = model.image;
                AddUser.IsReadAddReport = false;
                AddUser.IsReadArchived = false;
                AddUser.IsReadHRRespo = false;
                //Step 2
                //if (model.StartDate != null)
                //{
                //    var StartDateToString = DateTime.ParseExact(model.StartDate, inputFormat, CultureInfo.InvariantCulture);
                //    AddUser.StartDate = Convert.ToDateTime(StartDateToString.ToString(outputFormat));
                //}
                //AddUser.ResourceType = model.ResourceType;
                ////AddUser.Reportsto = model.Reportsto;
                //AddUser.AdditionalReportsto = model.AdditionalReportsto;
                //AddUser.HRResponsible = model.HRResponsible;
                //AddUser.JobTitle = model.JobTitle;
                //AddUser.JobContryID = model.JobContryID;
                //AddUser.Location = model.Location;
                //AddUser.BusinessID = model.BusinessID;
                //AddUser.DivisionID = model.DivisionID;
                //AddUser.PoolID = model.PoolID;
                //AddUser.FunctionID = model.FunctionID;

                //step 3
                //if (model.ProbationEndDate != null)
                //{
                //    var ProbationEndDateToString = DateTime.ParseExact(model.ProbationEndDate, inputFormat, CultureInfo.InvariantCulture);
                //    AddUser.ProbationEndDate = Convert.ToDateTime(ProbationEndDateToString.ToString(outputFormat));
                //}
                //if (model.NextProbationReviewDate != null)
                //{
                //    var NextProbationReviewDateToString = DateTime.ParseExact(model.NextProbationReviewDate, inputFormat, CultureInfo.InvariantCulture);
                //    AddUser.ProbationEndDate = Convert.ToDateTime(NextProbationReviewDateToString.ToString(outputFormat));
                //}
                //if (model.FixedTermEndDate != null)
                //{
                //    var FixedTermEndDateToString = DateTime.ParseExact(model.FixedTermEndDate, inputFormat, CultureInfo.InvariantCulture);
                //    AddUser.FixedTermEndDate = Convert.ToDateTime(FixedTermEndDateToString.ToString(outputFormat));
                //}
                //AddUser.NoticePeriod = model.NoticePeriod;
                //AddUser.MethodofRecruitmentSetup = model.MethodofRecruitmentSetup;
                //AddUser.RecruitmentCost = model.RecruitmentCost.ToString();
                // AddUser.CurrenciesId = model.curruncyID;
                //if (model.HolidaysThisYear != null)
                //    AddUser.Thisyear = (int)model.HolidaysThisYear;
                //if (model.HolidaysNextYear != null)
                //    AddUser.Nextyear = (int)model.HolidaysNextYear;
                //if (model.IncludePublicHoliday != null)
                //{
                //    if (model.IncludePublicHoliday == "on")
                //    { AddUser.IncludePublicHoliday = true; }
                //    else
                //    { AddUser.IncludePublicHoliday = false; }
                //}
                //if (model.HolidayEntit != 0 && model.HolidayEntit != null)
                //{
                //    AddUser.HolidayEntitlement = model.HolidayEntit;
                //}
                //step 4 
                //AddUser.Country = model.CountryId;
                //AddUser.State = model.StateId;
                //AddUser.Town = model.CityyId;
                //AddUser.Airport = model.AirportId;
                //AddUser.Postcode = model.PostalCode;
                //AddUser.BankAddress = model.Address;
                //  AddUser.WorkPhone = model.WorkPhone;
                // AddUser.WorkMobile = model.WorkMobile;
                //AddUser.PersonalPhone = model.PersonalPhone;
                //AddUser.PersonalMobile = model.PersonalMobile;
                //AddUser.PersonalEmail = model.PersonalEmail;
                //AddUser.BankName = model.BankName;
                //AddUser.BankCode = model.BankCode;
                //AddUser.AccountNumber = model.AccountNumber;
                //AddUser.OtherAccountInformation = model.OtherAccountInformation;
                //AddUser.AccountName = model.AccountName;
                //AddUser.BankAddress = model.BankAddress;
                AddUser.CreatedBy = SessionProxy.UserId;
                //Step 5
                //JavaScriptSerializer js = new JavaScriptSerializer();
                //List<AddbulknewtaskRecordmodel> listValueArray = js.Deserialize<List<AddbulknewtaskRecordmodel>>(model.JsonNewtaskList);
                //foreach (var item in listValueArray)
                //{
                // Task_List ModelNew = new Task_List();
                //ModelNew.InRelationTo = model.Id;
                //ModelNew.Title = item.Title;
                //ModelNew.Description = item.Description;
                //ModelNew.AssignTo = item.Assign;
                //ModelNew.Archived = false;
                //ModelNew.AlterBeforeDays = item.AlertBeforeDays;
                //if (item.DueDate != "")
                //{
                //    var DueDateToString = DateTime.ParseExact(item.DueDate, inputFormat, CultureInfo.InvariantCulture);
                //    ModelNew.DueDate = Convert.ToDateTime(DueDateToString.ToString(outputFormat));
                //}
                //ModelNew.Status = item.Status;
                //ModelNew.LastModified = DateTime.Now;
                //ModelNew.Created = DateTime.Now;
                //_db.Task_List.Add(ModelNew);
                _db.AspNetUsers.Add(AddUser);
                _db.SaveChanges();



                if (AddUser.Id > 0)
                {
                    //Employee Relation
                    EmployeeRelation _employeeRelation = new EmployeeRelation();
                    _employeeRelation.UserID = AddUser.Id;
                    //if (model.Reportsto != null)
                    //{
                    //    _employeeRelation.Reportsto = model.Reportsto.Value;
                    //}
                    //_employeeRelation.BusinessID = model.BusinessID;
                    //_employeeRelation.DivisionID = model.DivisionID;
                    //_employeeRelation.PoolID = model.PoolID;
                    //_employeeRelation.FunctionID = model.FunctionID;
                    //_employeeRelation.CreateBy = model.CurrentUserId;
                    _employeeRelation.CreatedDate = DateTime.Now;
                    _employeeRelation.IsActive = true;
                    _db.EmployeeRelations.Add(_employeeRelation);

                    //Employee Address Info
                    EmployeeAddressInfo _EmployeeAddressInfo = new EmployeeAddressInfo();
                    _EmployeeAddressInfo.UserId = AddUser.Id;
                    //_EmployeeAddressInfo.CountryId = model.CountryId;
                    //_EmployeeAddressInfo.StateId = model.StateId;
                    //_EmployeeAddressInfo.TownId = model.CityyId;
                    //_EmployeeAddressInfo.AirportId = model.AirportId;
                    //_EmployeeAddressInfo.PostCode = model.PostalCode;
                    //_EmployeeAddressInfo.ContactAddress = model.Address;
                    //_EmployeeAddressInfo.PersonalPhone = model.PersonalPhone;
                    //_EmployeeAddressInfo.PersonalMobile = model.PersonalMobile;
                    //_EmployeeAddressInfo.PersonalEmail = model.PersonalEmail;
                    _db.EmployeeAddressInfoes.Add(_EmployeeAddressInfo);

                    EmployeeBankInfo _EmployeeBankInfo = new EmployeeBankInfo();
                    _EmployeeBankInfo.UserId = AddUser.Id;
                    //_EmployeeBankInfo.BankName = model.BankName;
                    //_EmployeeBankInfo.BankCode = model.BankCode;
                    //_EmployeeBankInfo.AccountName = model.AccountName;
                    //_EmployeeBankInfo.AccountNumber = model.AccountNumber;
                    //_EmployeeBankInfo.OtherAccountInformation = model.OtherAccountInformation;
                    //_EmployeeBankInfo.BankAddress = model.BankAddress;
                    //_EmployeeBankInfo.IBAN_No = model.IBAN_Number;
                    //_EmployeeBankInfo.SWIFT_Code = model.SWIFT_Code;
                    _db.EmployeeBankInfoes.Add(_EmployeeBankInfo);
                }

                //if (model.ApplicantID != 0 && model.ApplicantID != null)
                //{
                //    var AccepteID = _AdminTMSMethod.GetAcceptedStepId((int)model.ApplicantID);
                //    var data = _db.TMS_Applicant.Where(x => x.Id == model.ApplicantID).FirstOrDefault();
                //    data.Archived = true;
                //    data.StatusID = AccepteID;
                //    data.LastModified = DateTime.Now;
                //}
                _db.SaveChanges();
            }
        }
        public bool validateSSOID(string sso)
        {
            var data = _db.AspNetUsers.Select(x => x.SSOID).ToList();
            int flag = 0;
            foreach (var item in data)
            {
                if (item == sso)
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

        public int calculateAge(DateTime dt)
        {
            DateTime dob = dt;
            DateTime PresentYear = DateTime.Now;
            TimeSpan ts = PresentYear - dob;
            int Age = ts.Days / 365;
            return Age;
        }
        public AspNetUser getBulkEmployeeById(int? EmployeeId)
        {
            return _db.AspNetUsers.Where(x => x.Id == EmployeeId).FirstOrDefault();
        }
        public IList<Business> getAllBusinessList()
        {
            return _db.Businesses.Where(x => x.Archived == false).ToList();
        }


        public void SaveBulkEmployeeSetting(BulkEmployeeSetting model)
        {
            if (!string.IsNullOrEmpty(model.EmpId))
            {
                string[] EmployeeId = model.EmpId.Split(',');

                for (int i = 0; i < EmployeeId.Length; i++)
                {
                    if (EmployeeId[i] != null && EmployeeId[i] != "")
                    {
                        int empId = Convert.ToInt32(EmployeeId[i]);
                        AspNetUser AddUser = _db.AspNetUsers.Where(x => x.Id == empId).FirstOrDefault();
                        EmployeeRelation _employeeRelation = _db.EmployeeRelations.Where(x => x.UserID == empId && x.IsActive == true).FirstOrDefault();
                        EmployeeAddressInfo _employeeAddressInfo = _db.EmployeeAddressInfoes.Where(x => x.UserId == empId).FirstOrDefault();
                        bool IsChanged = false;
                        //step 1                
                        if (model.CompanyId != 0 && model.CompanyId != null)
                        {
                            AddUser.Company = model.CompanyId;
                        }
                        if (model.ResourceTypeId != 0 && model.ResourceTypeId != null)
                        {
                            AddUser.ResourceType = model.ResourceTypeId;
                        }
                        if (model.AdditinalReportId != 0 && model.AdditinalReportId != null)
                        {
                            AddUser.AdditionalReportsto = model.AdditinalReportId;
                        }
                        if (model.HRRepoId != 0 && model.HRRepoId != null)
                        {
                            AddUser.HRResponsible = model.HRRepoId;
                        }
                        if (model.JobTitleId != 0 && model.JobTitleId != null)
                        {
                            AddUser.JobTitle = model.JobTitleId;
                        }
                        if (model.LocationId != null && model.LocationId != 0)
                        {
                            AddUser.Location = model.LocationId;
                        }
                        if (_employeeRelation != null)
                        {
                            if (_employeeRelation.Reportsto != model.ReportId)
                            {
                                IsChanged = true;
                            }
                            else if (_employeeRelation.DivisionID != model.DivisonId)
                            {
                                IsChanged = true;
                            }
                            else if (_employeeRelation.BusinessID != model.BusinessId)
                            {
                                IsChanged = true;
                            }
                            else if (_employeeRelation.PoolID != model.PoolId)
                            {
                                IsChanged = true;
                            }
                            else if (_employeeRelation.FunctionID != model.FunctionId)
                            {
                                IsChanged = true;
                            }

                            if (IsChanged)
                            {

                                _employeeRelation.IsActive = false;
                                EmployeeRelation _relation = new EmployeeRelation();
                                if (empId != 0 && empId != null)
                                {
                                    _relation.UserID = empId;
                                }
                                if (model.ReportId != 0 && model.ReportId != null)
                                {
                                    _relation.Reportsto = model.ReportId;
                                }
                                if (model.BusinessId != 0 && model.BusinessId != null)
                                {
                                    _relation.BusinessID = model.BusinessId;
                                }
                                if (model.DivisonId != 0 && model.DivisonId != null)
                                {
                                    _relation.DivisionID = model.DivisonId;
                                }
                                if (model.PoolId != null && model.PoolId != 0)
                                {
                                    _relation.PoolID = model.PoolId;
                                }
                                if (model.FunctionId != 0 && model.FunctionId != null)
                                {
                                    _relation.FunctionID = model.FunctionId;
                                }
                                _relation.IsActive = true;
                                _relation.UpdateBy = SessionProxy.UserId;
                                _relation.UpdateDate = DateTime.Now;
                                _db.EmployeeRelations.Add(_relation);
                            }
                        }
                        _db.SaveChanges();
                    }
                }
            }
        }

    }
}