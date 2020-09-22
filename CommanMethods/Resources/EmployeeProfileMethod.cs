using HRTool.CommanMethods.Admin;
using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.CommanMethods.Resources
{
    public class EmployeeProfileMethod
    {
        #region
        EvolutionEntities _db = new EvolutionEntities();
        EmployeeMethod _employeeMethod = new EmployeeMethod();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        CompanyStructureMethod _CompanyStructureMethod = new CompanyStructureMethod();
        AdminPearformanceMethod _AdminPearformanceMethod = new AdminPearformanceMethod();
        private string inputFormat = "dd-MM-yyyy";
        private string outputFormat = "yyyy-MM-dd HH:mm:ss";
        #endregion
        public MainResoureViewModel EmployeeProfileGetByID(int Id,int UserId)
        {
           MainResoureViewModel model = new MainResoureViewModel();
            model.Id = Id;
            var Employee = _db.AspNetUsers.Where(x => x.Id == Id).FirstOrDefault();
            var EmployeeRelation = _db.EmployeeRelations.Where(x => x.UserID == Employee.Id && x.IsActive == true).FirstOrDefault();
            var EmployeeAddress = _db.EmployeeAddressInfoes.Where(x => x.UserId == Employee.Id).FirstOrDefault();
            model.Id = Employee.Id;
            model.FirstName = Employee.FirstName;
            model.LastName = Employee.LastName;
            model.OtherNames = Employee.OtherNames;
            model.KnownAs = Employee.KnownAs;
            model.UserNameEmail = Employee.UserName;
            model.IMAddress = Employee.IMAddress;
            model.Gender = Employee.Gender;
            model.SSO = Employee.SSOID;
            model.SelectCustomerCompanyId = Employee.SelectCustomerCompanyId;
            string[] customername;
            customername = new string[100];
            if (model.SSO.Contains('C'))
            {
                model.RoleType = true;
                if (Employee.CustomerCareID != null)
                {
                    string[] values = Employee.CustomerCareID.Split(',');
                    for (int i = 0; i < values.Length; i++)
                    {
                        model.CustomerCareId = values[i];
                        int customerCareNo = Convert.ToInt32(values[i]);
                        if (Employee.CustomerCareID != null)
                        {
                            var details = _db.AspNetUsers.Where(x => x.Id == customerCareNo).FirstOrDefault();
                            //model.CustomerCare = String.Format("{0} {1} - {2}", details.FirstName, details.LastName, details.SSOID);
                            customername[i] = String.Format("{0} {1} - {2}", details.FirstName, details.LastName, details.SSOID);

                        }
                    }
                }
                if(EmployeeAddress != null) { 
               
                    model.PostalCode = EmployeeAddress.PostCode;
                    model.Address = EmployeeAddress.ContactAddress;
                    model.WorkPhone = Employee.WorkPhone;
                    model.WorkMobile = Employee.WorkMobile;
                }
            }
            else
            {
                model.RoleType = false;
            }
            var CoustomerCompany = _db.Company_Customer.Where(x => x.Archived == false).ToList();
            model.CoustomerCompanyList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in CoustomerCompany)
            {
                // string Name = string.Format("{0}{1}", item.FirstName, item.LastName);
                model.CoustomerCompanyList.Add(new SelectListItem() { Text = item.CompanyName, Value = @item.Id.ToString() });
            }
            model.DateOfBirth = String.Format("{0:dd-MM-yyy}", Employee.DateOfBirth);
            var Nationality = _otherSettingMethod.getAllSystemValueListByKeyName("Nationality List");
            List<AspNetUser> datauser = _AdminPearformanceMethod.getAllUserList().ToList();
            model.CopyFromList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in datauser)
            {
                string Name = string.Format("{0} {1}", item.FirstName, item.LastName);
                model.CopyFromList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
            }

            model.NationalityList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in Nationality)
            {
                if (Employee.Nationality == item.Id)
                {
                    model.NationalityList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                }
                else
                {
                    model.NationalityList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                }
            }
            model.NIN_SSN = Employee.NINumberSSN;
            model.ImageURL = Employee.image;
            var ResourceType = _otherSettingMethod.getAllSystemValueListByKeyName("Job Role List");
            model.ResourceTypeList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in ResourceType)
            {
                if (Employee.ResourceType == item.Id)
                {
                    model.ResourceTypeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                }
                else
                {
                    model.ResourceTypeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                }
            }

            var Title = _otherSettingMethod.getAllSystemValueListByKeyName("Title List");
            model.TitleList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in Title)
            {
                if (Employee.NameTitle == item.Id)
                {
                    model.TitleList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                }
                else
                {
                    model.TitleList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                }
            }
            if (Employee.CustomerCareID != null)
            {
                string[] values = Employee.CustomerCareID.Split(',');
                string ccname = "";
                for (int i = 0; i < values.Length; i++)
                {
                    int custId = Convert.ToInt32(values[i]);
                    var cname = _db.AspNetUsers.Where(x => x.Id == custId && x.Archived == false).FirstOrDefault();
                    if(cname != null)
                    {
                        string nm = cname.FirstName + " " + cname.LastName + "-" + cname.SSOID;
                        ccname = ccname + nm + ",";
                    }
                }
                model.CustomerCareId = Employee.CustomerCareID;
                model.CustomerCareName = ccname;
            }
            model.StartDate = String.Format("{0:dd-MM-yyy}", Employee.StartDate);
            model.ContinuousServiceDate = String.Format("{0:dd-MM-yyy}", Employee.ContinuousServiceDate);
            List<AspNetUser> data = _AdminPearformanceMethod.getAllUserList().ToList();
            model.CustomerCareList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in data)
            {
                string Name = string.Format("{0} {1} - {2}", item.FirstName, item.LastName, item.SSOID);
                model.CustomerCareList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString(), Selected = true });
            }
            //var CopyName = datauser.Where(x => x.Id == Employee.Id).FirstOrDefault();
            //if(CopyName!=null)
            //{
            //    model.copyName = CopyName.FirstName + " " + CopyName.LastName + " " + CopyName.SSOID;
            //}
            
            if (EmployeeRelation != null)
            {
                if (EmployeeRelation.Reportsto != 0 && EmployeeRelation.Reportsto != null)
                {
                    var User = data.Where(x => x.Id == EmployeeRelation.Reportsto).FirstOrDefault();
                    model.RepoempName = User.FirstName + " " + User.LastName + "-" + User.SSOID;
                    model.RepoempId = Convert.ToInt32(EmployeeRelation.Reportsto);
                }
            }
            if (Employee != null)
            {
                if (Employee.AdditionalReportsto != 0 && Employee.AdditionalReportsto != null)
                {
                    var AddRepoName = data.Where(xx => xx.Id == Employee.AdditionalReportsto).FirstOrDefault();
                    model.AddReName = AddRepoName.FirstName + " " + AddRepoName.LastName + " " + AddRepoName.SSOID;
                    model.AddReId = Convert.ToInt32(Employee.AdditionalReportsto);
                }
                if (Employee.HRResponsible != 0 && Employee.HRResponsible != null)
                {
                    var HRRepoName = data.Where(xx => xx.Id == Employee.HRResponsible).FirstOrDefault();
                    model.HRResponsibleName = HRRepoName.FirstName + " " + HRRepoName.LastName + "-" + HRRepoName.SSOID;
                    model.HrId = Convert.ToInt32(Employee.HRResponsible);
                }
            }
            var currency = _db.Currencies.ToList();
            foreach (var item in currency)
            {
                model.CurruencyCodeList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString() });
            }
            var jobTitle = _otherSettingMethod.getAllSystemValueListByKeyName("Job Title List");
            model.JobTitleList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in jobTitle)
            {
                if (Employee.JobTitle == item.Id)
                {
                    model.JobTitleList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                }
                else
                {
                    model.JobTitleList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                }
            }
            var JobCountry = _employeeMethod.bindPublicHolidayCountryList();
            //model.JobCountryList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in JobCountry)
            {
                if (Employee.JobContryID == (Convert.ToInt32(item.Value)))
                {
                    model.JobCountryList.Add(new SelectListItem() { Text = @item.Text, Value = @item.Value.ToString(), Selected = true });
                }
                else
                {
                    model.JobCountryList.Add(new SelectListItem() { Text = @item.Text, Value = @item.Value.ToString() });
                }
            }
            var Company = _otherSettingMethod.getAllSystemValueListByKeyName("Company List");
            model.CompanyList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in Company)
            {
                if (Employee.Company == item.Id)
                {
                    model.CompanyList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                }
                else
                {
                    model.CompanyList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                }
            }
            var Location = _otherSettingMethod.getAllSystemValueListByKeyName("Office Locations");
            model.LocationList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in Location)
            {
                if (Employee.Location == item.Id)
                {
                    model.LocationList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                }
                else
                {
                    model.LocationList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                }
            }
            var BusinessList = _CompanyStructureMethod.getAllBusinessList();
            model.BusinessList.Add(new SelectListItem() { Text = "--Select Business--", Value = "0" });
            foreach (var item in BusinessList)
            {
                if ((EmployeeRelation != null ? EmployeeRelation.BusinessID : 0) == item.Id)
                {
                    model.BusinessList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString(), Selected = true });
                }
                else
                {
                    model.BusinessList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString() });
                }
            }
            var DivisionList = _CompanyStructureMethod.getAllDivisionList();
            model.DivisionList.Add(new SelectListItem() { Text = "--Select Division--", Value = "0" });
            foreach (var item in DivisionList)
            {
                if ((EmployeeRelation != null ? EmployeeRelation.DivisionID : 0) == item.Id)
                {
                    model.DivisionList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString(), Selected = true });
                }
                else
                {
                    model.DivisionList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString() });
                }
            }
            var PoolListrecord = _CompanyStructureMethod.getAllPoolsList();
            model.PoolList.Add(new SelectListItem() { Text = "--Select Pool--", Value = "0" });
            foreach (var item in PoolListrecord)
            {
                if ((EmployeeRelation != null ? EmployeeRelation.PoolID : 0) == item.Id)
                {
                    model.PoolList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString(), Selected = true });
                }
                else
                {
                    model.PoolList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString() });
                }
            }
            var FuncationRecord = _CompanyStructureMethod.getAllFunctionsList();
            model.FunctionList.Add(new SelectListItem() { Text = "--Select Function--", Value = "0" });
            foreach (var item in FuncationRecord)
            {
                if ((EmployeeRelation != null ? EmployeeRelation.FunctionID : 0) == item.Id)
                {
                    model.FunctionList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString(), Selected = true });
                }
                else
                {
                    model.FunctionList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString() });
                }
            }
            model.LastModified = DateTime.Now;
            model.Picture = Employee.image;
            int total = (from row in _db.Employee_Salary select row).Count();
            model.TempSalaryID = total + 1;
            return model;
        }
        public Employee_Salary GetsalaryListById(int Id)
        {
            return _db.Employee_Salary.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
        }

        public Employee_Salary_Temp GetsalaryTempListById(int Id)
        {
            return _db.Employee_Salary_Temp.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
        }
        public void SavesalarySet(AddSalaryViewModel model, int userId)
        {
            Employee_Salary _salary = _db.Employee_Salary.Where(x => x.Id == model.Id).FirstOrDefault();
            if (_salary != null)
            {

                var ProbationEndDateToString = DateTime.ParseExact(model.EffectiveFrom, inputFormat, CultureInfo.InvariantCulture);
                _salary.EffectiveFrom = Convert.ToDateTime(ProbationEndDateToString.ToString(outputFormat));
                _salary.EmployeeID = model.EmployeeId;
                _salary.SalaryType = model.SalaryTypeID;
                _salary.PaymentFrequency = model.PaymentFrequencyID;
                _salary.Archived = false;
                _salary.Amount = model.Amount;
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
                _salarys.EmployeeID = model.EmployeeId;
                _salarys.SalaryType = model.SalaryTypeID;
                _salarys.PaymentFrequency = model.PaymentFrequencyID;
                _salarys.Archived = false;
                _salarys.Amount = model.Amount;
                _salarys.Currency = model.CurrencyID;
                _salarys.TotalSalary = model.TotalSalary;
                _salarys.ReasonforChange = model.ReasonforChange;
                _salarys.Comments = model.Comments;
                _salarys.CreateDate = DateTime.Now;
                _salarys.UserIDCreatedBy = model.CurrentUserId;
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

        public void SavesalarySetTemp(AddSalaryViewModel model)
        {
            var data = _db.Employee_Salary_Temp.Where(x => x.Id == model.Id).ToList();
            if (data.Count > 0)
            {

                Employee_Salary_Temp _salary = _db.Employee_Salary_Temp.Where(x => x.Id == model.Id).FirstOrDefault();
                if (model.EffectiveFrom != null)
                {
                    var ProbationEndDateToString = DateTime.ParseExact(model.EffectiveFrom, inputFormat, CultureInfo.InvariantCulture);
                    _salary.EffectiveFrom = Convert.ToDateTime(ProbationEndDateToString.ToString(outputFormat));
                }
                _salary.EmployeeID = model.EmployeeId;
                _salary.SalaryType = model.SalaryTypeID;
                _salary.PaymentFrequency = model.PaymentFrequencyID;
                _salary.Archived = false;
                _salary.Amount = model.Amount;
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
                _salary.EmployeeID = model.EmployeeId;
                _salary.SalaryType = model.SalaryTypeID;
                _salary.PaymentFrequency = model.PaymentFrequencyID;
                _salary.Archived = false;
                _salary.Amount = model.Amount;
                _salary.Currency = model.CurrencyID;
                _salary.TotalSalary = model.TotalSalary;
                _salary.ReasonforChange = model.ReasonforChange;
                _salary.Comments = model.Comments;
                _salary.CreateDate = DateTime.Now;
                _salary.UserIDCreatedBy = model.CurrentUserId;
                _db.Employee_Salary_Temp.Add(_salary);
                _db.SaveChanges();
            }


        }

        public void SavesalarySetAllTemp(AddSalaryViewModel model)
        {
            var details = _db.Employee_Salary.Where(x => x.Id == model.OriginalId && x.Archived == false).FirstOrDefault();
            if (details == null)
            {
                var data = _db.Employee_Salary_Temp.Where(x => x.Id == model.Id && x.Archived == false).ToList();
                if (data.Count > 0)
                {

                    Employee_Salary_Temp _salary = _db.Employee_Salary_Temp.Where(x => x.Id == model.Id).FirstOrDefault();
                    if (model.EffectiveFrom != null)
                    {
                        var ProbationEndDateToString = DateTime.ParseExact(model.EffectiveFrom, inputFormat, CultureInfo.InvariantCulture);
                        _salary.EffectiveFrom = Convert.ToDateTime(ProbationEndDateToString.ToString(outputFormat));
                    }
                    _salary.EmployeeID = model.EmployeeId;
                    _salary.SalaryType = model.SalaryTypeID;
                    _salary.PaymentFrequency = model.PaymentFrequencyID;
                    _salary.Archived = false;
                    _salary.Amount = model.Amount;
                    _salary.Currency = model.CurrencyID;
                    _salary.TotalSalary = model.TotalSalary;
                    _salary.Comments = model.Comments;
                    _salary.ReasonforChange = model.ReasonforChange;
                    _salary.LastModificationDate = DateTime.Now;
                    _db.SaveChanges();

                    var Deduction = _db.Employee_Salary_Deduction_Temp.Where(x => x.EmployeeSalaryID == model.Id && x.Archived == false).ToList();
                    decimal totaldecuction = 0;
                    if (Deduction.Count > 0)
                    {
                        foreach (var item in Deduction)
                        {
                            if (item.IncludeInSalary == true)
                            {
                                totaldecuction += item.FixedAmount;
                            }
                            item.PercentOfSalary = (item.FixedAmount * 100) / Convert.ToDecimal(_salary.Amount);
                            _db.SaveChanges();
                        }
                    }
                    var Entitlement = _db.Employee_Salary_Entitlement_Temp.Where(x => x.EmployeeSalaryID == model.Id && x.Archived == false).ToList();
                    decimal totalEntitle = 0;
                    if (Entitlement.Count > 0)
                    {
                        foreach (var item in Entitlement)
                        {
                            if (item.IncludeInSalary == true)
                            {
                                totalEntitle += item.FixedAmount;
                            }
                            item.PercentOfSalary = (item.FixedAmount * 100) / Convert.ToDecimal(_salary.Amount);
                            _db.SaveChanges();
                        }
                    }

                    var diff = totaldecuction - totalEntitle;
                    Employee_Salary_Temp _salaryss = _db.Employee_Salary_Temp.Where(x => x.Id == model.Id).FirstOrDefault();

                    if (totaldecuction > totalEntitle)
                    {
                        decimal value = Convert.ToDecimal(_salaryss.TotalSalary) - diff;
                        _salaryss.TotalSalary = string.Format("{0:#,###0}", value.ToString());
                        _db.SaveChanges();
                    }
                    else
                    {
                        decimal value = Convert.ToDecimal(_salaryss.TotalSalary) - diff;
                        _salaryss.TotalSalary = string.Format("{0:#,###0}", value.ToString());
                        _db.SaveChanges();
                    }
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
                    _salary.EmployeeID = model.EmployeeId;
                    _salary.SalaryType = model.SalaryTypeID;
                    _salary.PaymentFrequency = model.PaymentFrequencyID;
                    _salary.Archived = false;
                    _salary.Amount = model.Amount;
                    _salary.Currency = model.CurrencyID;
                    _salary.TotalSalary = model.TotalSalary;
                    _salary.ReasonforChange = model.ReasonforChange;
                    _salary.Comments = model.Comments;
                    _salary.CreateDate = DateTime.Now;
                    _salary.UserIDCreatedBy = model.CurrentUserId;
                    _db.Employee_Salary_Temp.Add(_salary);
                    _db.SaveChanges();
                }

            }
            else
            {
                Employee_Salary update = _db.Employee_Salary.Where(x => x.Id == model.OriginalId && x.Archived == false).FirstOrDefault();
                if (model.EffectiveFrom != null)
                {
                    var ProbationEndDateToString = DateTime.ParseExact(model.EffectiveFrom, inputFormat, CultureInfo.InvariantCulture);
                    update.EffectiveFrom = Convert.ToDateTime(ProbationEndDateToString.ToString(outputFormat));
                }

                update.EmployeeID = model.EmployeeId;
                update.SalaryType = model.SalaryTypeID;
                update.PaymentFrequency = model.PaymentFrequencyID;
                update.Archived = false;
                update.Amount = model.Amount;
                update.Currency = model.CurrencyID;
                update.TotalSalary = model.TotalSalary;
                update.Comments = model.Comments;
                update.ReasonforChange = model.ReasonforChange;
                update.LastModificationDate = DateTime.Now;
                _db.SaveChanges();
                var Deduction = _db.Employee_Salary_Deduction.Where(x => x.EmployeeSalaryID == model.OriginalId && x.Archived == false).ToList();
                decimal totaldecuction = 0;
                if (Deduction.Count > 0)
                {
                    foreach (var item in Deduction)
                    {
                        if (item.IncludeInSalary == true)
                        {
                            totaldecuction += item.FixedAmount;
                        }
                        item.PercentOfSalary = (item.FixedAmount * 100) / Convert.ToDecimal(update.Amount);
                        _db.SaveChanges();
                    }
                }
                var Entitlement = _db.Employee_Salary_Entitlements.Where(x => x.EmployeeSalaryID == model.OriginalId && x.Archived == false).ToList();
                decimal totalEntitle = 0;
                if (Entitlement.Count > 0)
                {
                    foreach (var item in Entitlement)
                    {
                        if (item.IncludeInSalary == true)
                        {
                            totalEntitle += item.FixedAmount;
                        }
                        item.PercentOfSalary = (item.FixedAmount * 100) / Convert.ToDecimal(update.Amount);
                        _db.SaveChanges();
                    }
                }

                var diff = totaldecuction - totalEntitle;
                Employee_Salary _salaryss = _db.Employee_Salary.Where(x => x.Id == model.OriginalId && x.Archived == false).FirstOrDefault();

                if (totaldecuction > totalEntitle)
                {
                    decimal value = Convert.ToDecimal(_salaryss.TotalSalary) - diff;
                    _salaryss.TotalSalary = string.Format("{0:#,###0}", value.ToString());
                    _db.SaveChanges();
                }
                else
                {
                    decimal value = Convert.ToDecimal(_salaryss.TotalSalary) - diff;
                    _salaryss.TotalSalary = string.Format("{0:#,###0}", value.ToString());
                    _db.SaveChanges();
                }

            }

        }
        public void Deletesalary(int Id)
        {
            Employee_Salary Project = _db.Employee_Salary.Where(x => x.Id == Id).FirstOrDefault();
            Project.Archived = true;
            Project.LastModificationDate = DateTime.Now;
            _db.SaveChanges();
        }
        public void SaveProfileSet(MainResoureViewModel model)
        {
            AspNetUser AddUser = _db.AspNetUsers.Where(x => x.Id == model.Id).FirstOrDefault();
            EmployeeRelation _employeeRelation = _db.EmployeeRelations.Where(x => x.UserID == model.Id && x.IsActive == true).FirstOrDefault();
            EmployeeAddressInfo _employeeAddressInfo = _db.EmployeeAddressInfoes.Where(x => x.UserId == model.Id).FirstOrDefault();
            bool IsChanged = false;
            //step 1

            AddUser.NameTitle = model.Title;
            AddUser.FirstName = model.FirstName;
            AddUser.LastName = model.LastName;
            AddUser.OtherNames = model.OtherNames;
            AddUser.KnownAs = model.KnownAs;
            AddUser.UserName = model.UserNameEmail;
            AddUser.IMAddress = model.IMAddress;
            AddUser.Gender = model.Gender;
            AddUser.Company = model.Company;
            AddUser.image = model.Picture;
            AddUser.LastModifiedDate = DateTime.Now;
            AddUser.LastModifyBy = model.Id;
            if (model.ContinuousServiceDate != null)
            {
                var DateOfBirthToString = DateTime.ParseExact(model.ContinuousServiceDate, inputFormat, CultureInfo.InvariantCulture);
                AddUser.ContinuousServiceDate = Convert.ToDateTime(DateOfBirthToString.ToString(outputFormat));
            }
            if (model.DateOfBirth != null)
            {
                var DateOfBirthToString = DateTime.ParseExact(model.DateOfBirth, inputFormat, CultureInfo.InvariantCulture);
                AddUser.DateOfBirth = Convert.ToDateTime(DateOfBirthToString.ToString(outputFormat));
            }
            AddUser.Nationality = model.Nationality;
            AddUser.NINumberSSN = model.NIN_SSN;

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
            if (AddUser.SSOID.Contains('C'))
            {

                AddUser.SelectCustomerCompanyId = model.SelectCustomerCompanyId;
                //AddUser.ContactAddress = model.Address;
                AddUser.WorkPhone = model.WorkPhone;
                //AddUser.Postcode = model.PostalCode;
                AddUser.WorkMobile = model.WorkMobile;
                var id = model.CustomerCareId;
               // var userData = _db.AspNetUsers.Where(x => x.Id == id).FirstOrDefault();
                AddUser.CustomerCareID = model.CustomerCareId;
                if (_employeeAddressInfo != null)
                {
                    _employeeAddressInfo.ContactAddress = model.Address;
                    _employeeAddressInfo.PostCode = model.PostalCode;
                }
                

            }
            if (_employeeRelation != null)
            {
                if (_employeeRelation.Reportsto != model.Reportsto)
                {
                    IsChanged = true;
                }
                else if (_employeeRelation.DivisionID != model.DivisionID)
                {
                    IsChanged = true;
                }
                else if (_employeeRelation.BusinessID != model.BusinessID)
                {
                    IsChanged = true;
                }
                else if (_employeeRelation.PoolID != model.PoolID)
                {
                    IsChanged = true;
                }
                else if (_employeeRelation.FunctionID != model.FunctionID)
                {
                    IsChanged = true;
                }

                if (IsChanged)
                {

                    _employeeRelation.IsActive = false;
                    EmployeeRelation _relation = new EmployeeRelation();
                    _relation.UserID = model.Id;
                    _relation.Reportsto = model.Reportsto;
                    _relation.BusinessID = model.BusinessID;
                    _relation.DivisionID = model.DivisionID;
                    _relation.PoolID = model.PoolID;
                    _relation.FunctionID = model.FunctionID;
                    _relation.IsActive = true;
                    _relation.CreateBy = model.CurrentUserId;
                    _relation.CreatedDate = DateTime.Now;
                    _relation.UpdateBy = model.CurrentUserId;
                    _relation.UpdateDate = DateTime.Now;

                    _db.EmployeeRelations.Add(_relation);
                }
            }
            _db.SaveChanges();
        }

        //Salary Deduction
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
                    //var data = _db.Employee_Salary.Where(x => x.Id == _salary.EmployeeSalaryID).FirstOrDefault();
                    //var total = Convert.ToDecimal(data.TotalSalary.Replace(",", "")) - Convert.ToDecimal(_salary.FixedAmount);
                    //data.TotalSalary = string.Format("{0:#,###0}", total);
                    //// data.Amount = string.Format("{0:#,###0}", total);
                    //_db.SaveChanges();
                }
            }
        }
        public void DeletesalaryDeduction(int Id, int UserId)
        {
            Employee_Salary_Deduction _salary = _db.Employee_Salary_Deduction.Where(x => x.Id == Id).FirstOrDefault();
            _salary.Archived = true;
            _salary.LastModified = DateTime.Now;
            _salary.UserIDLastModifiedBy = UserId;
            _db.SaveChanges();
            var data = _db.Employee_Salary.Where(x => x.Id == _salary.EmployeeSalaryID).FirstOrDefault();
            var total = Convert.ToDecimal(data.TotalSalary.Replace(",", "")) + Convert.ToDecimal(_salary.FixedAmount);
            data.TotalSalary = string.Format("{0:#,###0}", total);
            //data.Amount = string.Format("{0:#,###0}", total);
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
        public void DeletesalaryDeductionTemp(int Id, int UserId)
        {
            Employee_Salary_Deduction_Temp _salary = _db.Employee_Salary_Deduction_Temp.Where(x => x.Id == Id).FirstOrDefault();
            _salary.Archived = true;
            _salary.LastModified = DateTime.Now;
            _salary.UserIDLastModifiedBy = UserId;
            _db.SaveChanges();
            var data = _db.Employee_Salary_Temp.Where(x => x.Id == _salary.EmployeeSalaryID).FirstOrDefault();
            var total = Convert.ToDecimal(data.TotalSalary.Replace(",", "")) + Convert.ToDecimal(_salary.FixedAmount);
            data.TotalSalary = string.Format("{0:#,###0}", total);
            //data.Amount = string.Format("{0:#,###0}", total);
            _db.SaveChanges();
        }

        //Salary Entitlement
        public void SavesalaryEntitlementSet(AddSalaryEntitlementViewModel model, int userId)
        {
            if (model.Id > 0)
            {
                Employee_Salary_Entitlements _salary = _db.Employee_Salary_Entitlements.Where(x => x.Id == model.Id).FirstOrDefault();
                //var diff = _salary.FixedAmount - model.FixedAmount;
                if (_salary.IncludeInSalary)
                {
                    decimal diff = _salary.FixedAmount - model.FixedAmount;
                    if (diff == 0)
                    {
                        diff = -model.FixedAmount;
                        SaveTotalSalaryADDEdit(_salary.EmployeeSalaryID, diff);
                    }
                    else
                    {
                        if (model.IncludeInSalary == true)
                        {
                            SaveTotalSalaryADDEdit(_salary.EmployeeSalaryID, -diff);
                        }
                        else
                        {
                            diff = -_salary.FixedAmount;
                            SaveTotalSalaryADDEdit(_salary.EmployeeSalaryID, diff);

                        }
                    }

                }
                else
                {
                    if (model.IncludeInSalary == true)
                    {
                        decimal diff = model.FixedAmount;
                        SaveTotalSalaryADDEdit(_salary.EmployeeSalaryID, diff);
                    }
                }

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
                //    decimal total = 0;
                //    var data = _db.Employee_Salary.Where(x => x.Id == _salary.EmployeeSalaryID).FirstOrDefault();
                //    total = Convert.ToDecimal(data.TotalSalary.Replace(",", "")) - diff;
                //    data.TotalSalary = string.Format("{0:#,###0}", total);
                //    // data.Amount = string.Format("{0:#,###0}", total);
                //    _db.SaveChanges();
                //}
            }
            else
            {
                Employee_Salary_Entitlements _salary = new Employee_Salary_Entitlements();
                _salary.Entitlement = model.EntitlementID;
                _salary.EmployeeSalaryID = model.EmployeeSalaryID;
                _salary.FixedAmount = model.FixedAmount;
                _salary.PercentOfSalary = model.PercentOfSalary;
                _salary.IncludeInSalary = model.IncludeInSalary;
                _salary.Comments = model.Comments;
                _salary.CreatedDate = DateTime.Now;
                _salary.LastModified = DateTime.Now;
                _salary.UserIDCreatedBy = userId;
                _salary.UserIDLastModifiedBy = userId;
                _db.Employee_Salary_Entitlements.Add(_salary);
                _db.SaveChanges();

                if (_salary.IncludeInSalary == true)
                {
                    SaveTotalSalaryADDEdit(_salary.EmployeeSalaryID, Convert.ToDecimal(_salary.FixedAmount));

                    //var data = _db.Employee_Salary.Where(x => x.Id == _salary.EmployeeSalaryID).FirstOrDefault();
                    //var total = Convert.ToDecimal(data.TotalSalary.Replace(",", "")) + Convert.ToDecimal(_salary.FixedAmount);
                    //data.TotalSalary = string.Format("{0:#,###0}", total);
                    //// data.Amount = string.Format("{0:#,###0}", total);
                    //_db.SaveChanges();
                }
            }
        }
        public void DeletesalaryEntitlement(int Id, int UserId)
        {
            Employee_Salary_Entitlements _salary = _db.Employee_Salary_Entitlements.Where(x => x.Id == Id).FirstOrDefault();
            _salary.Archived = true;
            _salary.LastModified = DateTime.Now;
            _salary.UserIDLastModifiedBy = UserId;
            _db.SaveChanges();
            var data = _db.Employee_Salary.Where(x => x.Id == _salary.EmployeeSalaryID).FirstOrDefault();
            var total = Convert.ToDecimal(data.TotalSalary.Replace(",", "")) - Convert.ToDecimal(_salary.FixedAmount);
            data.TotalSalary = string.Format("{0:#,###0}", total);
            // data.Amount = string.Format("{0:#,###0}", total);
            _db.SaveChanges();
        }
        public void SavesalaryEntitlementSetTemp(AddSalaryEntitlementViewModel model, int userId)
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
        public void DeletesalaryEntitlementTemp(int Id, int UserId)
        {
            Employee_Salary_Entitlement_Temp _salary = _db.Employee_Salary_Entitlement_Temp.Where(x => x.Id == Id).FirstOrDefault();
            _salary.Archived = true;
            _salary.LastModified = DateTime.Now;
            _salary.UserIDLastModifiedBy = UserId;
            _db.SaveChanges();
            var data = _db.Employee_Salary_Temp.Where(x => x.Id == _salary.EmployeeSalaryID).FirstOrDefault();
            var total = Convert.ToDecimal(data.TotalSalary.Replace(",", "")) - Convert.ToDecimal(_salary.FixedAmount);
            data.TotalSalary = string.Format("{0:#,###0}", total);
            // data.Amount = string.Format("{0:#,###0}", total);
            _db.SaveChanges();
        }

        //Common Method
        public void SaveTotalSalaryADDEdit(int SID, decimal diff) 
        {
            var data = _db.Employee_Salary.Where(x => x.Id == SID).FirstOrDefault();
            var total = Convert.ToDecimal(data.TotalSalary.Replace(",", "")) + diff;
            data.TotalSalary = string.Format("{0:#,###0}", total);
            _db.SaveChanges();
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
        //public void SaveTotalSalaryEntitlementTemp(int SID, decimal diff)
        //{
        //    var data = _db.Employee_Salary_Temp.Where(x => x.Id == SID).FirstOrDefault();
        //    if (data != null)
        //    {
        //        var total = Convert.ToDecimal(data.TotalSalary.Replace(",", "")) - diff;
        //        data.TotalSalary = string.Format("{0:#,###0}", total);
        //        _db.SaveChanges();
        //    }
        //}
    }
}