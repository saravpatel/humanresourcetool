using HRTool.CommanMethods.Resources;
using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.IO;
using HRTool.CommanMethods;
using HRTool.CommanMethods.Admin;
using HRTool.CommanMethods.Settings;
namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class EmployeeProfileController : Controller
    {
        //
        // GET: /EmployeeProfile/

        #region const
        EmployeeProfileMethod _EmployeeProfileMethod = new EmployeeProfileMethod();
        EvolutionEntities _db = new EvolutionEntities();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        CurrencyConverterMethod CurrencyMethod = new CurrencyConverterMethod();
        CompanyStructureMethod _CompanyStructureMethod = new CompanyStructureMethod();
        EmployeeMethod _EmployeeMethod = new EmployeeMethod();
        private string inputFormat = "dd-MM-yyyy";
        private string outputFormat = "yyyy-MM-dd HH:mm:ss";
        #endregion

        #region Index View
        public ActionResult Index(int EmployeeId)
        {
            MainResoureViewModel model = new MainResoureViewModel();
            model.Id = EmployeeId;
            return View(model);
        }

        public ActionResult ProfileRecord(int EmployeeId)
        {
            int userId = SessionProxy.UserId;
            MainResoureViewModel model = _EmployeeProfileMethod.EmployeeProfileGetByID(EmployeeId, userId);
            return PartialView("_PartialProfileUpdaterecord", model);
        }
        public ActionResult getEmployee(int Id)
        {
            int userId = SessionProxy.UserId;
            var Employee = _db.AspNetUsers.Where(x => x.Id == Id).FirstOrDefault();
            var EmployeeRelation = _db.EmployeeRelations.Where(x => x.UserID == Employee.Id && x.IsActive == true).FirstOrDefault();
            AdminPearformanceMethod _AdminPearformanceMethod = new AdminPearformanceMethod();
            MainResoureViewModel model = new MainResoureViewModel();
            List<AspNetUser> data = _AdminPearformanceMethod.getAllUserList().ToList();   
            //model.ReportstoList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            //model.AdditionalReportstoList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            //model.HRResponsibleList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });           
            foreach (var item in data)
            {
                string Name = string.Format("{0} {1}-{2}", item.FirstName, item.LastName,item.SSOID);
                if ((EmployeeRelation != null ? EmployeeRelation.Reportsto : 0) == item.Id)
                {
                    model.ReportstoList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString(), Selected = true });
                }
                else
                {
                    if (Employee.Id != item.Id)
                    {
                        model.ReportstoList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
                    }
                }
                if (Employee.AdditionalReportsto == item.Id)
                {
                    model.AdditionalReportstoList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString(), Selected = true });
                }
                else
                {
                    if (Employee.Id != item.Id)
                    {
                        model.AdditionalReportstoList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
                    }
                }

                if (Employee.HRResponsible == item.Id)
                {
                    model.HRResponsibleList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString(), Selected = true });
                }
                else
                {
                    if (Employee.Id != item.Id)
                    {
                        model.HRResponsibleList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
                    }
                }
            }
            var List = _db.AspNetUsers.Where(x => x.SSOID.Contains("W") && x.Archived == false).ToList();
            //model.Add("-- Select User --");
            //foreach (var item in List)
            //{
            //    var value = String.Format("{0} {1} - {2}", item.FirstName, item.LastName, item.SSOID);
            //    model.Add(value);
            //}
            foreach (var item in List)
            {
                model.EmployeeCustomerCare.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }
            return Json(model, JsonRequestBehavior.AllowGet);   
        }
        
        public ActionResult CustomerCare() 
        {
          //  var data = _EmployeeMethod.BindWorkerDropdown();
            MainResoureViewModel model = new MainResoureViewModel();
            
            var List = _db.AspNetUsers.Where(x => x.SSOID.Contains("W") && x.Archived == false).ToList();
            //model.Add("-- Select User --");
            //foreach (var item in List)
            //{
            //    var value = String.Format("{0} {1} - {2}", item.FirstName, item.LastName, item.SSOID);
            //    model.Add(value);
            //}
            foreach (var item in List)
            {
                model.EmployeeCustomerCare.Add(new SelectListItem() { Text = item.FirstName + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Customer Company 
        public ActionResult CustomerComapanyDetails(int Id) 
        {
            var data = _db.Company_Customer.Where(x => x.Id == Id).FirstOrDefault();
            if (data != null)
            {
                foreach (var item in  _otherSettingMethod.getAllSystemValueListByKeyName("Company Setting Currencies"))
                {
                    if (data.Currency == item.Id) 
                    {
                        data.IBAN = item.Value;
                    }
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else 
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Salary
        public List<AddSalaryViewModel> returnsalaryList(int? Id)
        {
            List<AddSalaryViewModel> model = new List<AddSalaryViewModel>();
            var data = _db.Employee_Salary.Where(x => x.EmployeeID == Id && x.Archived == false).ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    var value = _otherSettingMethod.getSystemListValueById(Convert.ToInt32(item.ReasonforChange));
                    AddSalaryViewModel dd = new AddSalaryViewModel();
                    dd.EmployeeId = item.EmployeeID;
                    dd.Id = item.Id;
                    dd.Amount = item.Amount;
                    dd.EffectiveFrom = String.Format("{0:dd-MMM-yyy}", item.EffectiveFrom);
                    if (value != null)
                    {
                        dd.ReasonforChangeName = value.Value;
                    }
                    model.Add(dd);
                }
            }
            return model;
        }
        public ActionResult AddSalaryDetailsList(int Id)
        {
            List<AddSalaryViewModel> Model = returnsalaryList(Id);
            return PartialView("_PartialAddSalaryList", Model);
        }
        public ActionResult SaveData(AddSalaryViewModel model)
        {
            int userId = SessionProxy.UserId;
            model.CurrentUserId = userId;
            _EmployeeProfileMethod.SavesalarySet(model,userId);
            DeleteTemp();
            List<AddSalaryViewModel> modelList = returnsalaryList(model.EmployeeId);
            return PartialView("_PartialAddSalaryList", modelList);
        }
        public ActionResult AddEditSalarytSet(int Id, int EmployeeID)
        {
            DeleteTemp();
            AddSalaryViewModel model = new AddSalaryViewModel();
            model.Id = Id;
            model.Tempmode = true;
            model.EmployeeId = EmployeeID;
            if (Id > 0)
            {

                var hr_salary = _EmployeeProfileMethod.GetsalaryListById(Id);
                model.Tempmode = false;
                model.EffectiveFrom = String.Format("{0:dd-MM-yyy}", hr_salary.EffectiveFrom);
                model.Amount = hr_salary.Amount;
                model.TotalSalary = hr_salary.TotalSalary;
                model.Comments = hr_salary.Comments;
                model.TableId = hr_salary.Id;
                var Salary_List = _otherSettingMethod.getAllSystemValueListByKeyName("SalaryType List");
                foreach (var item in Salary_List)
                {
                    if (hr_salary.SalaryType == item.Id)
                    {
                        model.SalaryTypeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        model.SalaryTypeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                    }

                }

                var PaymentFrequency_List = _otherSettingMethod.getAllSystemValueListByKeyName("PaymentFrequency List");
                foreach (var item in PaymentFrequency_List)
                {
                    if (hr_salary.PaymentFrequency == item.Id)
                    {
                        model.PaymentFrequencyList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        model.PaymentFrequencyList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                    }

                }
                var ReasonforChange_List = _otherSettingMethod.getAllSystemValueListByKeyName("ReasonforChange List");
                foreach (var item in ReasonforChange_List)
                {
                    if (hr_salary.ReasonforChange == item.Id)
                    {
                        model.ReasonforChangeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        model.ReasonforChangeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                    }
                }

                //model.CurrencyList = CurrencyMethod.BindCurrencyListRecord();
                var Currency_List = _otherSettingMethod.getAllSystemValueListByKeyName("Company Setting Currencies");
                foreach (var item in Currency_List)
                {
                    if (hr_salary.Currency == item.Id)
                    {
                        model.CurrencyList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        model.CurrencyList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                    }

                }
            }
            else
            {
                var Salary_List = _otherSettingMethod.getAllSystemValueListByKeyName("SalaryType List");

                foreach (var item in Salary_List)
                {
                    model.SalaryTypeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                }
                var PaymentFrequency_List = _otherSettingMethod.getAllSystemValueListByKeyName("PaymentFrequency List");
                foreach (var item in PaymentFrequency_List)
                {
                    model.PaymentFrequencyList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                }
                var ReasonforChange_List = _otherSettingMethod.getAllSystemValueListByKeyName("ReasonforChange List");
                foreach (var item in ReasonforChange_List)
                {
                    model.ReasonforChangeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                }
                //var Currency_List = CurrencyMethod.GetCurrencyListRecord();
                //foreach (var item in Currency_List)
                //{
                //    model.CurrencyList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString() });
                //}
                var Currency_Lists = _otherSettingMethod.getAllSystemValueListByKeyName("Company Setting Currencies");
                foreach (var item in Currency_Lists)
                {
                    model.CurrencyList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });

                }
                model.TotalSalary = "0";
                int count = (from row in _db.Employee_Salary select row).Count();
                model.TableId = count + 1;

            }

            return PartialView("_PartialAddSalary", model);

        }

        public ActionResult DeleteSalaryRecord(int Id, int EmployeeID)
        {
            _EmployeeProfileMethod.Deletesalary(Id);
            List<AddSalaryViewModel> modelList = returnsalaryList(EmployeeID);
            return PartialView("_PartialAddSalaryList", modelList);
        }

        #endregion

        #region Salary Temp

        public ActionResult SaveSalaryTemp(AddSalaryViewModel models)
        {
            int userId = SessionProxy.UserId;
            models.CurrentUserId = userId;
            _EmployeeProfileMethod.SavesalarySetTemp(models);
            AddSalaryDeductionViewModel model = new AddSalaryDeductionViewModel();
            model.SalaryTypeList.Add(new SelectListItem() { Text = "-- Select Deduction --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Deduction List"))
            {
                model.SalaryTypeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            return PartialView("_PartialAddSalaryDeductionTemp", model);
        }
        public ActionResult SaveSalaryEntitlementTemp(AddSalaryViewModel models)
        {
            int userId = SessionProxy.UserId;
            models.CurrentUserId = userId;
            _EmployeeProfileMethod.SavesalarySetTemp(models);
            AddSalaryEntitlementViewModel model = new AddSalaryEntitlementViewModel();
            model.SalaryTypeList.Add(new SelectListItem() { Text = "-- Select Entitlement --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Entitlement List"))
            {
                model.SalaryTypeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            return PartialView("_PartialAddSalaryEntitlementTemp", model);
        }

        public ActionResult DeleteTemp() 
        {
            var data = _db.Employee_Salary_Temp.Where(x => x.Archived == false).ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    _db.Employee_Salary_Temp.Remove(item);
                    _db.SaveChanges();
                }
            }
            DeleteSalaryEntitlementTempBySalaryId();
            DeleteSalaryDeductionTempBySalaryId();
            return Json("Sucesses", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AmountChangeTemp(AddSalaryViewModel Model) 
       {
            var details = _db.Employee_Salary.Where(x => x.Id == Model.OriginalId).FirstOrDefault();
            _EmployeeProfileMethod.SavesalarySetAllTemp(Model);
            AddSalaryViewModel modelss = new AddSalaryViewModel();
            if (details == null)
            {
                 modelss = GetAllListDetailsTemp(Model.EmployeeId, Model.Id);
                
            }
            else 
            {
                 modelss = GetAllListDetails(Model.EmployeeId, Model.OriginalId);
            }
            return PartialView("_PartialAddSalary", modelss);
        }
        
        #endregion

        #region Salary Deductions
        public List<AddSalaryDeductionViewModel> returnsalaryDeductionsList(int Id)
        {
            List<AddSalaryDeductionViewModel> model = new List<AddSalaryDeductionViewModel>();
            var data = _db.Employee_Salary_Deduction.Where(x => x.EmployeeSalaryID == Id && x.Archived == false).ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    var value = _otherSettingMethod.getSystemListValueById((int)item.Deduction);
                    AddSalaryDeductionViewModel dd = new AddSalaryDeductionViewModel();
                    dd.EmployeeSalaryID = item.EmployeeSalaryID;
                    dd.Id = item.Id;
                    dd.DeductionID = (int)item.Deduction;
                    dd.FixedAmount = item.FixedAmount;
                    dd.PercentOfSalary = item.PercentOfSalary;
                    dd.IncludeInSalary = item.IncludeInSalary;
                    dd.Comments = item.Comments;
                    if (value != null)
                    {
                        dd.Deduction = value.Value;
                    }
                    model.Add(dd);
                }
            }
            return model;
        }
        public ActionResult AddSalaryDeductionsList(int Id)
        {
            List<AddSalaryDeductionViewModel> Model = returnsalaryDeductionsList(Id);
            return PartialView("_PartialSalaryDeductionListView", Model);
        }

        public ActionResult SaveSalarytDeduction(AddSalaryDeductionViewModel model)
        {
            //var temp = DeleteSalaryDeductionTempBySalaryId(model.EmployeeSalaryID);
            //var tempE = DeleteSalaryEntitlementTempBySalaryId(model.EmployeeSalaryID);
            int userId = SessionProxy.UserId;
            _EmployeeProfileMethod.SavesalaryDeductionSet(model, userId);
            AddSalaryViewModel modelss = GetAllListDetails(model.EmployeeID, model.EmployeeSalaryID);
            return PartialView("_PartialAddSalary", modelss);
        }

        public ActionResult DeleteSalaryDeduction(AddSalaryDeductionViewModel model)
        {
            int userId = SessionProxy.UserId;
            _EmployeeProfileMethod.DeletesalaryDeduction(model.Id, userId);
            AddSalaryViewModel modelss = GetAllListDetails(model.EmployeeID, model.EmployeeSalaryID);
            return PartialView("_PartialAddSalary", modelss);

        }

        public ActionResult AddEditSalarytDeduction(int Id, int SalaryID)
        {
            AddSalaryDeductionViewModel model = new AddSalaryDeductionViewModel();
            model.SalaryTypeList.Add(new SelectListItem() { Text = "-- Select Deduction --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Deduction List"))
            {
                model.SalaryTypeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            var salarydetails = _db.Employee_Salary.Where(x => x.Id == SalaryID).FirstOrDefault();
            model.TotalSalary = salarydetails.Amount;
            var salaryType = _otherSettingMethod.getSystemListValueById((int)salarydetails.Currency);
            model.SalaryType = salaryType.Value;
            model.EmployeeSalaryID = salarydetails.Id;
            if (Id > 0)
            {
                var data = _db.Employee_Salary_Deduction.Where(x => x.Id == Id).FirstOrDefault();
                model.Id = data.Id;
                model.DeductionID = (int)data.Deduction;
                model.EmployeeSalaryID = data.EmployeeSalaryID;
                model.Comments = data.Comments;
                model.FixedAmount = data.FixedAmount;
                model.PercentOfSalary = data.PercentOfSalary;
                model.IncludeInSalary = data.IncludeInSalary;
            }
            return PartialView("_PartialAddSalaryDeduction", model);

        }

        #endregion

        #region Salary Deductions Temp

        public AddSalaryViewModel GetAllListDetailsTemp(int? EmployeeID, int SalaryID)
        {
            AddSalaryViewModel modelss = new AddSalaryViewModel();
            modelss.Id = SalaryID;
            modelss.Tempmode = true;
            modelss.EmployeeId = EmployeeID;
            modelss.TableId = SalaryID;
            var hr_salary = _EmployeeProfileMethod.GetsalaryTempListById(SalaryID);
            if (hr_salary != null)
            {
                if (hr_salary.EffectiveFrom != null)
                {
                    modelss.EffectiveFrom = String.Format("{0:dd-MM-yyy}", hr_salary.EffectiveFrom);
                }
                modelss.Amount = hr_salary.Amount;
                modelss.TotalSalary = hr_salary.TotalSalary;
                modelss.Comments = hr_salary.Comments;
                var Salary_List = _otherSettingMethod.getAllSystemValueListByKeyName("SalaryType List");
                foreach (var item in Salary_List)
                {
                    if (hr_salary.SalaryType == item.Id)
                    {
                        modelss.SalaryTypeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        modelss.SalaryTypeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                    }

                }

                var PaymentFrequency_List = _otherSettingMethod.getAllSystemValueListByKeyName("PaymentFrequency List");
                foreach (var item in PaymentFrequency_List)
                {
                    if (hr_salary.PaymentFrequency == item.Id)
                    {
                        modelss.PaymentFrequencyList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        modelss.PaymentFrequencyList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                    }

                }
                var ReasonforChange_List = _otherSettingMethod.getAllSystemValueListByKeyName("ReasonforChange List");
                foreach (var item in ReasonforChange_List)
                {
                    if (hr_salary.ReasonforChange == item.Id)
                    {
                        modelss.ReasonforChangeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        modelss.ReasonforChangeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                    }
                }

                //model.CurrencyList = CurrencyMethod.BindCurrencyListRecord();
                var Currency_List = _otherSettingMethod.getAllSystemValueListByKeyName("Company Setting Currencies");
                foreach (var item in Currency_List)
                {
                    if (hr_salary.Currency == item.Id)
                    {
                        modelss.CurrencyList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        modelss.CurrencyList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                    }

                }
            }
            else 
            {

            }
            return modelss;
        }

        public List<AddSalaryDeductionViewModel> returnsalaryDeductionsListTemp(int Id)
        {
            List<AddSalaryDeductionViewModel> model = new List<AddSalaryDeductionViewModel>();
            var data = _db.Employee_Salary_Deduction_Temp.Where(x => x.EmployeeSalaryID == Id && x.Archived == false).ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    var value = _otherSettingMethod.getSystemListValueById((int)item.Deduction);
                    AddSalaryDeductionViewModel dd = new AddSalaryDeductionViewModel();
                    dd.EmployeeSalaryID = item.EmployeeSalaryID;
                    dd.Id = item.Id;
                    dd.DeductionID = (int)item.Deduction;
                    dd.FixedAmount = item.FixedAmount;
                    dd.PercentOfSalary = item.PercentOfSalary;
                    dd.IncludeInSalary = item.IncludeInSalary;
                    dd.Comments = item.Comments;
                    if (value != null)
                    {
                        dd.Deduction = value.Value;
                    }
                    model.Add(dd);
                }
            }
            return model;
        }

        public ActionResult AddSalaryDeductionsListTemp(int Id)
        {
            List<AddSalaryDeductionViewModel> Model = returnsalaryDeductionsListTemp(Id);
            return PartialView("_PartialSalaryDeductionListViewTemp", Model);
        }

        public ActionResult SaveSalarytDeductionTemp(AddSalaryDeductionViewModel model)
        {
            int userId = SessionProxy.UserId;
            _EmployeeProfileMethod.SavesalaryDeductionSetTemp(model, userId);
            AddSalaryViewModel modelss = GetAllListDetailsTemp(model.EmployeeID, model.EmployeeSalaryID);
            return PartialView("_PartialAddSalary", modelss);
        }

        public ActionResult DeleteSalaryDeductionTemp(AddSalaryDeductionViewModel model)
        {
            int userId = SessionProxy.UserId;
            _EmployeeProfileMethod.DeletesalaryDeductionTemp(model.Id, userId);
            AddSalaryViewModel modelss = GetAllListDetailsTemp(model.EmployeeID, model.EmployeeSalaryID);
            return PartialView("_PartialAddSalary", modelss);

        }

        public ActionResult AddEditSalarytDeductionTemp(int Id, int SalaryID)
        {
            AddSalaryDeductionViewModel model = new AddSalaryDeductionViewModel();
            model.SalaryTypeList.Add(new SelectListItem() { Text = "-- Select Deduction --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Deduction List"))
            {
                model.SalaryTypeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            var salarydetails = _db.Employee_Salary_Temp.Where(x => x.Id == SalaryID).FirstOrDefault();
            model.TotalSalary = salarydetails.Amount;
            var salaryType = _otherSettingMethod.getSystemListValueById((int)salarydetails.Currency);
            model.SalaryType = salaryType.Value;
            model.EmployeeSalaryID = salarydetails.Id;
            if (Id > 0)
            {
                var data = _db.Employee_Salary_Deduction_Temp.Where(x => x.Id == Id).FirstOrDefault();
                model.Id = data.Id;
                model.DeductionID = (int)data.Deduction;
                model.EmployeeSalaryID = data.EmployeeSalaryID;
                model.Comments = data.Comments;
                model.FixedAmount = data.FixedAmount;
                model.PercentOfSalary = data.PercentOfSalary;
                model.IncludeInSalary = data.IncludeInSalary;
            }
            return PartialView("_PartialAddSalaryDeductionTemp", model);

        }

        public ActionResult DeleteSalaryDeductionTempBySalaryId()
        {
            var data = _db.Employee_Salary_Deduction_Temp.ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    _db.Employee_Salary_Deduction_Temp.Remove(item);
                    _db.SaveChanges();
                }
            }
            return Json("True", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Salary Entitlements
        public List<AddSalaryEntitlementViewModel> returnsalaryEntitlementsList(int Id)
        {
            List<AddSalaryEntitlementViewModel> model = new List<AddSalaryEntitlementViewModel>();
            var data = _db.Employee_Salary_Entitlements.Where(x => x.EmployeeSalaryID == Id && x.Archived == false).ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    var value = _otherSettingMethod.getSystemListValueById((int)item.Entitlement);
                    AddSalaryEntitlementViewModel dd = new AddSalaryEntitlementViewModel();
                    dd.EmployeeSalaryID = item.EmployeeSalaryID;
                    dd.Id = item.Id;
                    dd.FixedAmount = item.FixedAmount;
                    dd.PercentOfSalary = item.PercentOfSalary;
                    dd.IncludeInSalary = item.IncludeInSalary;
                    dd.EntitlementID = (int)item.Entitlement;
                    dd.Comments = item.Comments;
                    if (value != null)
                    {
                        dd.Entitlement = value.Value;
                    }
                    model.Add(dd);
                }
            }
            return model;
        }

        public ActionResult AddSalaryEntitlementsList(int Id)
        {
            List<AddSalaryEntitlementViewModel> Model = returnsalaryEntitlementsList(Id);
            return PartialView("_PartialSalaryEntitlementListView", Model);
        }

        public ActionResult SaveSalarytEntitlement(AddSalaryEntitlementViewModel model)
        {
            int userId = SessionProxy.UserId;
            _EmployeeProfileMethod.SavesalaryEntitlementSet(model, userId);
            AddSalaryViewModel modelss = GetAllListDetails(model.EmployeeID, model.EmployeeSalaryID);
            return PartialView("_PartialAddSalary", modelss);
        }

        public ActionResult DeleteSalaryEntitlement(AddSalaryEntitlementViewModel model)
        {
            int userId = SessionProxy.UserId;
            _EmployeeProfileMethod.DeletesalaryEntitlement(model.Id, userId);
            AddSalaryViewModel modelss = GetAllListDetails(model.EmployeeID, model.EmployeeSalaryID);
            return PartialView("_PartialAddSalary", modelss);

        }

        public ActionResult AddEditSalarytEntitlement(int Id, int SalaryID)
        {
            AddSalaryEntitlementViewModel model = new AddSalaryEntitlementViewModel();
            model.SalaryTypeList.Add(new SelectListItem() { Text = "-- Select Entitlement --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Entitlement List"))
            {
                model.SalaryTypeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            var salarydetails = _db.Employee_Salary.Where(x => x.Id == SalaryID).FirstOrDefault();
            model.TotalSalary = salarydetails.Amount;
            var salaryType = _otherSettingMethod.getSystemListValueById((int)salarydetails.Currency);
            model.SalaryType = salaryType.Value;
            model.EmployeeSalaryID = salarydetails.Id;
            if (Id > 0)
            {
                var data = _db.Employee_Salary_Entitlements.Where(x => x.Id == Id).FirstOrDefault();
                model.Id = data.Id;
                model.EntitlementID = (int)data.Entitlement;
                model.EmployeeSalaryID = data.EmployeeSalaryID;
                model.Comments = data.Comments;
                model.FixedAmount = data.FixedAmount;
                model.PercentOfSalary = data.PercentOfSalary;
                model.IncludeInSalary = data.IncludeInSalary;
            }
            return PartialView("_PartialAddSalaryEntitlement", model);

        }

        #endregion

        #region Salary Entitlements Temp
        public List<AddSalaryEntitlementViewModel> returnsalaryEntitlementsListTemp(int Id)
        {
            List<AddSalaryEntitlementViewModel> model = new List<AddSalaryEntitlementViewModel>();
            var data = _db.Employee_Salary_Entitlement_Temp.Where(x => x.EmployeeSalaryID == Id && x.Archived == false).ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    var value = _otherSettingMethod.getSystemListValueById((int)item.Entitlement);
                    AddSalaryEntitlementViewModel dd = new AddSalaryEntitlementViewModel();
                    dd.EmployeeSalaryID = item.EmployeeSalaryID;
                    dd.Id = item.Id;
                    dd.FixedAmount = item.FixedAmount;
                    dd.PercentOfSalary = item.PercentOfSalary;
                    dd.IncludeInSalary = item.IncludeInSalary;
                    dd.EntitlementID = (int)item.Entitlement;
                    dd.Comments = item.Comments;
                    if (value != null)
                    {
                        dd.Entitlement = value.Value;
                    }
                    model.Add(dd);
                }
            }
            return model;
        }

        public ActionResult AddSalaryEntitlementsListTemp(int Id)
        {
            List<AddSalaryEntitlementViewModel> Model = returnsalaryEntitlementsListTemp(Id);
            return PartialView("_PartialSalaryEntitlementListViewTemp", Model);
        }

        public ActionResult SaveSalarytEntitlementTemp(AddSalaryEntitlementViewModel model)
        {
            int userId = SessionProxy.UserId;
            _EmployeeProfileMethod.SavesalaryEntitlementSetTemp(model, userId);
            AddSalaryViewModel modelss = GetAllListDetailsTemp(model.EmployeeID, model.EmployeeSalaryID);
            return PartialView("_PartialAddSalary", modelss);
        }

        public ActionResult DeleteSalaryEntitlementTemp(AddSalaryEntitlementViewModel model)
        {
            int userId = SessionProxy.UserId;
            _EmployeeProfileMethod.DeletesalaryEntitlementTemp(model.Id, userId);
            AddSalaryViewModel modelss = GetAllListDetailsTemp(model.EmployeeID, model.EmployeeSalaryID);
            return PartialView("_PartialAddSalary", modelss);

        }

        public ActionResult AddEditSalarytEntitlementTemp(int Id, int SalaryID)
        {
            AddSalaryEntitlementViewModel model = new AddSalaryEntitlementViewModel();
            model.SalaryTypeList.Add(new SelectListItem() { Text = "-- Select Entitlement --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Entitlement List"))
            {
                model.SalaryTypeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            var salarydetails = _db.Employee_Salary_Temp.Where(x => x.Id == SalaryID).FirstOrDefault();
            model.TotalSalary = salarydetails.Amount;
            var salaryType = _otherSettingMethod.getSystemListValueById((int)salarydetails.Currency);
            model.SalaryType = salaryType.Value;
            model.EmployeeSalaryID = salarydetails.Id;
            if (Id > 0)
            {
                var data = _db.Employee_Salary_Entitlement_Temp.Where(x => x.Id == Id).FirstOrDefault();
                model.Id = data.Id;
                model.EntitlementID = (int)data.Entitlement;
                model.EmployeeSalaryID = data.EmployeeSalaryID;
                model.Comments = data.Comments;
                model.FixedAmount = data.FixedAmount;
                model.PercentOfSalary = data.PercentOfSalary;
                model.IncludeInSalary = data.IncludeInSalary;
            }
            return PartialView("_PartialAddSalaryEntitlementTemp", model);

        }

        public ActionResult DeleteSalaryEntitlementTempBySalaryId()
        {
            var data = _db.Employee_Salary_Entitlement_Temp.ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    _db.Employee_Salary_Entitlement_Temp.Remove(item);
                    _db.SaveChanges();
                }
            }
            return Json("True", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Comman Method
        public AddSalaryViewModel GetAllListDetails(int? EmployeeID, int SalaryID)
        {
            AddSalaryViewModel modelss = new AddSalaryViewModel();
            modelss.Id = SalaryID;
            modelss.EmployeeId = EmployeeID;
            var hr_salary = _EmployeeProfileMethod.GetsalaryListById(SalaryID);
            modelss.EffectiveFrom = String.Format("{0:dd-MM-yyy}", hr_salary.EffectiveFrom);
            modelss.Amount = hr_salary.Amount;
            modelss.TotalSalary = hr_salary.TotalSalary;
            modelss.Comments = hr_salary.Comments;
            var Salary_List = _otherSettingMethod.getAllSystemValueListByKeyName("SalaryType List");
            foreach (var item in Salary_List)
            {
                if (hr_salary.SalaryType == item.Id)
                {
                    modelss.SalaryTypeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                }
                else
                {
                    modelss.SalaryTypeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                }

            }

            var PaymentFrequency_List = _otherSettingMethod.getAllSystemValueListByKeyName("PaymentFrequency List");
            foreach (var item in PaymentFrequency_List)
            {
                if (hr_salary.PaymentFrequency == item.Id)
                {
                    modelss.PaymentFrequencyList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                }
                else
                {
                    modelss.PaymentFrequencyList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                }

            }
            var ReasonforChange_List = _otherSettingMethod.getAllSystemValueListByKeyName("ReasonforChange List");
            foreach (var item in ReasonforChange_List)
            {
                if (hr_salary.ReasonforChange == item.Id)
                {
                    modelss.ReasonforChangeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                }
                else
                {
                    modelss.ReasonforChangeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                }
            }

            //model.CurrencyList = CurrencyMethod.BindCurrencyListRecord();
            var Currency_List = _otherSettingMethod.getAllSystemValueListByKeyName("Company Setting Currencies");
            foreach (var item in Currency_List)
            {
                if (hr_salary.Currency == item.Id)
                {
                    modelss.CurrencyList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                }
                else
                {
                    modelss.CurrencyList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                }

            }

            return modelss;
        }
        public ActionResult UpdateProfile(MainResoureViewModel model)
        {
            int userId = SessionProxy.UserId;
            _EmployeeProfileMethod.SaveProfileSet(model);
            model = _EmployeeProfileMethod.EmployeeProfileGetByID(model.Id,userId);
            return PartialView("_PartialProfileUpdaterecord", model);
        }
        public ActionResult bindDivisionList(int businessId)
        {
            var data = _CompanyStructureMethod.GetDivisionListByBizId(businessId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public List<SelectListItem> bindDiv(int Id)
        {
            List<SelectListItem> model = new List<SelectListItem>();
            var data = _CompanyStructureMethod.GetDivisionListByBizId(Id);
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    SelectListItem mm = new SelectListItem();
                    mm.Value = item.Id.ToString();
                    mm.Text = item.Name;
                    model.Add(mm);
                }
            }

            return model;

        }
        public List<SelectListItem> poolDiv(int Id)
        {
            List<SelectListItem> model = new List<SelectListItem>();
            var data = _CompanyStructureMethod.GetPoolListByBizId(Id);
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    SelectListItem mm = new SelectListItem();
                    mm.Value = item.Id.ToString();
                    mm.Text = item.Name;
                    model.Add(mm);
                }
            }
            return model;

        }
        public List<SelectListItem> functionDiv(int Id)
        {
            List<SelectListItem> model = new List<SelectListItem>();
            var data = _CompanyStructureMethod.GetFuncationListByBizId(Id);
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    SelectListItem mm = new SelectListItem();
                    mm.Value = item.Id.ToString();
                    mm.Text = item.Name;
                    model.Add(mm);
                }
            }
            return model;
        }
        public ActionResult bindPoolList(int DivisionId)
        {
            var data = _CompanyStructureMethod.GetPoolListByBizId(DivisionId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult bindFuncationList(int DivisionId)
        {
            var data = _CompanyStructureMethod.GetFuncationListByBizId(DivisionId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ImageData()
        {
            string FilePath = string.Empty;
            string fileName = string.Empty;
            string originalFileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                FilePath = ConfigurationManager.AppSettings["ResourceFilePath"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
        }

        #endregion
    }
}