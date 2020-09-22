using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRTool.Models.BulkActions;
using HRTool.CommanMethods.BulkActions;
using HRTool.DataModel;
using System.Web.Script.Serialization;
using HRTool.CommanMethods;
using System.Configuration;
using System.IO;
using System.Globalization;
using OfficeOpenXml;
using System.IO.Compression;
using Rotativa.Options;
using HRTool.CommanMethods.Admin;
using HRTool.CommanMethods.RolesManagement;
namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class BulkActionsController : Controller
    {
        // GET: BulkActions
        EvolutionEntities _db = new EvolutionEntities();
        BulkActionsMethod _bulkActionMethod = new BulkActionsMethod();
        AdminPearformanceMethod _AdminPearformanceMethod = new AdminPearformanceMethod();
        RolesManagementMethod _RolesManagementMethod = new RolesManagementMethod();

        public ActionResult Index()
        {

            ViewBag.BusinessList = _db.Businesses.ToList().Where(x => x.Archived == false);
            //ViewBag.DivisionList = _db.Divisions.ToList().Where(x => x.Archived == false);
            //ViewBag.PoolList = _db.Pools.ToList().Where(x => x.Archived == false);
            //ViewBag.FunctionList = _db.Functions.ToList().Where(x => x.Archived == false);
            AllDataList datamodel = new AllDataList();
            var BusinessList = _bulkActionMethod.getAllBusinessList();
            datamodel.BusinesList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in BusinessList)
            {
                datamodel.BusinesList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString() });
            }
            var jobTitleList = _bulkActionMethod.getAllSystemValueListByKeyName("Job Title List");
            foreach (var item in jobTitleList)
            {
                datamodel.JobTitleList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var CountryR = _db.Countries.Where(x => x.status == 1).ToList();
            foreach (var item in CountryR)
            {
                datamodel.GetCountryList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString() });
            }
            return View(datamodel);
        }
        public ActionResult ImageData()
        {
            string FilePath = string.Empty;
            string fileName = string.Empty;
            string originalFileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                FilePath = ConfigurationManager.AppSettings["AdminTraining"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
        }
        //Traning
        public ActionResult TraningImageData()
        {
            string FilePath = string.Empty;
            string fileName = string.Empty;
            string originalFileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                FilePath = ConfigurationManager.AppSettings["AdminTraining"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
        }
        public ActionResult AddEditTriningData(BulkActionTraining model)
        {
            int userId = SessionProxy.UserId;
            model.CurrentUserId = userId;
            JavaScriptSerializer js = new JavaScriptSerializer();
            model.ListDocument = js.Deserialize<List<BulkTraningDocumentViewModel>>(model.TraingDocumentList);
            _bulkActionMethod.SaveTrainingdData(model, userId);
            return Json("Data Inserted", JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllTrainingAddFiledDetail()
        {
            BulkActionsMethod _bsActionMethod = new BulkActionsMethod();
            FiledViewModel model = new FiledViewModel();
            model.BindFiledTypeList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in _bsActionMethod.getAllSystemValueListByKeyName("Field Type List"))
            {
                model.BindFiledTypeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            return PartialView("_partialAdd_Bulk_ListTrainingStep2", model);
        }
        public ActionResult GetAllTrainingDetail(string empId)
        {
            BulkActionsMethod _bsActionMethod = new BulkActionsMethod();
            BulkActionTraining model = new BulkActionTraining();
            model.BindTrainingList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in _bsActionMethod.getAllSystemValueListByKeyName("Training List"))
            {
                model.BindTrainingList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            model.BindTrainingStatusList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in _bsActionMethod.getAllSystemValueListByKeyName("Traning Status List"))
            {
                model.BindTrainingStatusList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            return PartialView("_partialAdd_Bulk_ListTraining", model);
        }
        // HolidayNextYear
        public ActionResult GetAllHolidayNextYear(string empId)
        {
            List<EmployeeHolidayEntitNextYear> sd = new List<EmployeeHolidayEntitNextYear>();
            sd = _bulkActionMethod.getEmpDataNextYear(empId);
            return PartialView("_partialAdd_Bulk_HolidayEntNextYear", sd);
        }
        public ActionResult AddEditEmpNextHoliday(List<employeeRequestThisYear> _employeeRequest)
        {
            EmployeeHolidayEntitNextYear modelDoc = new EmployeeHolidayEntitNextYear();
            BulkActionsMethod _bsActionMethod = new BulkActionsMethod();

            if (_employeeRequest.Count() > 0)
            {
                for (int i = 0; i < _employeeRequest.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(_employeeRequest[i].Year))
                        _bsActionMethod.updateHolidayNextYear(Convert.ToInt32(_employeeRequest[i].EmpId), Convert.ToInt32(_employeeRequest[i].Year));
                }
                return Json("Data Inserted", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        //Holiday This Year
        public ActionResult GetAllHolidayThisYear(string empId)
        {
            List<EmployeeHolidayEntitlementsThisYear> sd = new List<EmployeeHolidayEntitlementsThisYear>();
            sd = _bulkActionMethod.getEmpDataThisYear(empId);
            return PartialView("_partialAdd_Bulk_HolidayEntThisYear", sd);
        }
        public ActionResult AddEditEmpHoliday(List<employeeRequestThisYear> _employeeRequest)
        {
            EmployeeHolidayEntitlementsThisYear modelDoc = new EmployeeHolidayEntitlementsThisYear();
            BulkActionsMethod _bsActionMethod = new BulkActionsMethod();

            if (_employeeRequest.Count() > 0)
            {
                for (int i = 0; i < _employeeRequest.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(_employeeRequest[i].Year))
                        _bsActionMethod.updateHolidayThisYear(Convert.ToInt32(_employeeRequest[i].EmpId), Convert.ToInt32(_employeeRequest[i].Year));
                }
                return Json("Data Inserted", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        //Activity
        public ActionResult GetAllActivityTypeDetail(string empId)
        {
            BulkActionsMethod _bsActionMethod = new BulkActionsMethod();
            BulkActonActivityType model = new BulkActonActivityType();
            model.currencyList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in _bsActionMethod.getAllSystemValueListByKeyName("Company Setting Currencies"))
            {
                model.currencyList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            model.workUnitList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in _bsActionMethod.getAllSystemValueListByKeyName("Work Unit List"))
            {
                model.workUnitList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }

            return PartialView("_partialAdd_Bulk_ActivityType", model);
        }
        public ActionResult AddEditActivityTypeData(string EmpId, int Year, string name, int currencyId, int workUnitId, decimal WorkerRate, decimal customerRate)
        {
            ActivityTypeListViewModel modelDoc = new ActivityTypeListViewModel();
            BulkActionsMethod _bsActionMethod = new BulkActionsMethod();
            string[] values = EmpId.Split(',');

            for (int i = 0; i < values.Length; i++)
            {
                _bsActionMethod.saveActivityTypeData(Convert.ToInt32(values[i]), Year, name, currencyId, workUnitId, WorkerRate, customerRate);
            }
            return Json("Data Inserted", JsonRequestBehavior.AllowGet);
        }
        //Salary
        public ActionResult GetIdCurr(int id, double amount)
        {
            //AddSalaryViewModel model = new AddSalaryViewModel();
            //var data = _bulkActionMethod.getCurruncyFixedRate(id);
            //var isfixed = _bulkActionMethod.getIsFixedCurr().ToList();
            //Double fromVal = Convert.ToDouble(isfixed.FirstOrDefault().FixedRate);
            //if (id != 0)
            //{
            //    if (fromVal != 0)
            //    {
            //        if (data.FirstOrDefault().FixedRate != 0)
            //        {
            //            Double toVal = Convert.ToDouble(data.FirstOrDefault().FixedRate);
            //            Double baseResult = (1 / fromVal);
            //            Double currVal = baseResult * toVal * amount;
            //            model.ToAmount = currVal;
            //            model.ToAmount = Math.Round(model.ToAmount, 2);
            //        }
            //    }
            //}
            //return Json(model.ToAmount, JsonRequestBehavior.AllowGet);
            return Json(JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddEditSalaryData(string EmployeeId, DateTime EffectiveFrom, int SalaryType, int PaymentFrequancy, string Ammount, int Curruncy, string TotalSalary, int ReasonForChange, string comment)
        {
            AddSalaryViewModel model = new AddSalaryViewModel();
            BulkActionsMethod _bsActionMethod = new BulkActionsMethod();
            var incurrcodetoeuro = _bulkActionMethod.getCurruncyFixedRate(Curruncy);
            string[] values = EmployeeId.Split(',');
            var Currency_List = _bulkActionMethod.GetCurrencyListRecord();
            int curruntUser = SessionProxy.UserId;
            for (int i = 0; i < values.Length; i++)
            {
                model.EmployeeId = values[i];
                int EmpId = Convert.ToInt32(values[i]);                
                model.EffectiveFrom = String.Format("{0:dd-MM-yyy}", EffectiveFrom);
                model.SalaryTypeID = SalaryType;
                model.PaymentFrequencyID = PaymentFrequancy;
                model.Amount = Convert.ToDouble(Ammount);
                model.CurrencyID = Curruncy;
                model.TotalSalary = TotalSalary;
                model.ReasonforChange = ReasonForChange;
                model.Comments = comment;
                _bulkActionMethod.SaveBulkSalarySet(model, curruntUser, EmpId);
                _bsActionMethod.saveSalaryDate(Convert.ToInt32(values[i]), EffectiveFrom, SalaryType, PaymentFrequancy, Ammount, Curruncy, TotalSalary, ReasonForChange, comment);

            }

            return PartialView("_partialAdd_Bulk_Salary", model);
        }
        public ActionResult GetAllSalaryDetail(string empId, string raId)
        {
            AddSalaryViewModel model = new AddSalaryViewModel();
            model.EmployeeId = empId;
            model.SalaryTypeList.Add(new SelectListItem() { Text = "-- Select Salary Type --", Value = "0" });
            foreach (var item in _bulkActionMethod.getAllSystemValueListByKeyName("SalaryType List"))
            {
                model.SalaryTypeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            model.PaymentFrequencyList.Add(new SelectListItem() { Text = "-- Select Payment Frequncy Type--", Value = "0" });
            foreach (var item in _bulkActionMethod.getAllSystemValueListByKeyName("PaymentFrequency List"))
            {
                model.PaymentFrequencyList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            model.ReasonforChangeList.Add(new SelectListItem() { Text = "-- Select Reason For Change--", Value = "0" });
            foreach (var item in _bulkActionMethod.getAllSystemValueListByKeyName("ReasonforChange List"))
            {
                model.ReasonforChangeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            foreach (var item in _bulkActionMethod.getAllSystemValueListByKeyName("Company Setting Currencies"))
            {
                model.CurrencyList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            model.TotalSalary = "0";
            int count = (from row in _db.Employee_Salary select row).Count();
            model.TableId = count + 1;
            model.Id = 0;
            model.Tempmode = true;
            return PartialView("_partialAdd_Bulk_Salary", model);
        }
        public ActionResult AddSalaryDeductionsListTemp(string Id)
        {
            List<AddSalaryDeductionViewModel> Model = returnsalaryDeductionsListTemp(Id);
            return PartialView("_PartialSalaryDeductionListViewTemp", Model);
        }
        public List<AddSalaryDeductionViewModel> returnsalaryDeductionsListTemp(string Id)
        {
            string[] empId = Id.Split(',');
            List<AddSalaryDeductionViewModel> model = new List<AddSalaryDeductionViewModel>();
            for (int i = 0; i < empId.Length; i++)
            {
                int eId = Convert.ToInt32(empId[i]);
                var data = _db.Employee_Salary_Deduction_Temp.Where(x => x.EmployeeSalaryID == eId && x.Archived == false).ToList();
                if (data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        var value = _bulkActionMethod.getSystemListValueById((int)item.Deduction);
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
            }
            return model;
        }

        public ActionResult AddSalaryDeductionsList(string Id)
        {
            List<AddSalaryDeductionViewModel> Model = returnsalaryDeductionsList(Id);
            return PartialView("_PartialSalaryDeductionListView", Model);
        }
        public List<AddSalaryDeductionViewModel> returnsalaryDeductionsList(string Id)
        {
            string[] EmpId = Id.Split(',');            
            List<AddSalaryDeductionViewModel> model = new List<AddSalaryDeductionViewModel>();
           // var salarydetails = _db.Employee_Salary.Where(x => x.Id == SalaryID).FirstOrDefault();            
            //var salaryType = _bulkActionMethod.getSystemListValueById((int)salarydetails.Currency);
            //model.SalaryType = salaryType.Value;
            //model.EmployeeSalaryID = salarydetails.Id;
            //alert(1);
            for (int i = 0; i < EmpId.Length; i++)
            {
                int eid = Convert.ToInt32(EmpId[i]);
                var data = _db.Employee_Salary_Deduction.Where(x => x.EmployeeSalaryID == eid && x.Archived == false).ToList();
                if (data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        var value = _bulkActionMethod.getSystemListValueById((int)item.Deduction);
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
            }
            return model;
        }
        public ActionResult SaveSalaryTemp(AddSalaryViewModel models)
        {
            int userId = SessionProxy.UserId;
            int total = (from row in _db.Employee_Salary select row).Count();
            models.Id = total + 1;
            models.CurrentUserId = Convert.ToString(userId);
            _bulkActionMethod.SavesalarySetTemp(models);
            AddSalaryDeductionViewModel model = new AddSalaryDeductionViewModel();
            model.SalaryTypeList.Add(new SelectListItem() { Text = "-- Select Deduction --", Value = "0" });
            foreach (var item in _bulkActionMethod.getAllSystemValueListByKeyName("Deduction List"))
            {
                model.SalaryTypeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            return PartialView("_PartialAddSalaryDeductionTemp", model);
        }
        public ActionResult SaveSalarytDeduction(AddSalaryDeductionViewModel model)
        {
            //var temp = DeleteSalaryDeductionTempBySalaryId(model.EmployeeSalaryID);
            //var tempE = DeleteSalaryEntitlementTempBySalaryId(model.EmployeeSalaryID);
            int userId = SessionProxy.UserId;
            _bulkActionMethod.SavesalaryDeductionSet(model, userId);
            //AddSalaryViewModel modelss = new AddSalaryViewModel();
            AddSalaryViewModel modelss = GetAllListDetails(model.EmployeeID, model.EmployeeSalaryID);
            //return PartialView("_partialAdd_Bulk_Salary", modelss);
            return PartialView("_partialAdd_Bulk_Salary", modelss);

        }
        public ActionResult SaveSalarytDeductionTemp(AddSalaryDeductionViewModel model)
        {
            int userId = SessionProxy.UserId;
            _bulkActionMethod.SavesalaryDeductionSetTemp(model, userId);
            int total = (from row in _db.Employee_Salary select row).Count();
            model.Id = total + 1;
            AddSalaryViewModel modelss = GetAllListDetailsTemp(model.EmployeeID, model.Id);
            //  return PartialView("_PartialAddSalary", modelss);
            return PartialView("_partialAdd_Bulk_Salary", modelss);
        }
         public ActionResult SaveSalarytEntitTemp(AddSalaryEntitlementViewModel model)
        {
            int userId = SessionProxy.UserId;
            _bulkActionMethod.SavesalaryEntitlementSet(model, userId);
            int total = (from row in _db.Employee_Salary select row).Count();
            model.Id = total + 1;
            AddSalaryViewModel modelss = GetAllListDetailsTemp(model.EmployeeID, model.Id);
            //  return PartialView("_PartialAddSalary", modelss);
            return PartialView("_partialAdd_Bulk_Salary", modelss);
        }
        public AddSalaryViewModel GetAllListDetailsTemp(string EmployeeID, int SalaryID)
        {
            string[] EmpId = EmployeeID.Split(',');
            AddSalaryViewModel modelss = new AddSalaryViewModel();
            for (int i = 0; i < EmpId.Length; i++)
            {
                modelss.Id = SalaryID;
                modelss.Tempmode = true;
                modelss.EmployeeId = EmployeeID;
                modelss.TableId = SalaryID;
                var hr_salary = _bulkActionMethod.GetsalaryTempListById(SalaryID);
                if (hr_salary != null)
                {
                    if (hr_salary.EffectiveFrom != null)
                    {
                        modelss.EffectiveFrom = String.Format("{0:dd-MM-yyy}", hr_salary.EffectiveFrom);
                    }
                    modelss.Amount = Convert.ToDouble(hr_salary.Amount);
                    modelss.TotalSalary = hr_salary.TotalSalary;
                    modelss.Comments = hr_salary.Comments;
                    var Salary_List = _bulkActionMethod.getAllSystemValueListByKeyName("SalaryType List");
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

                    var PaymentFrequency_List = _bulkActionMethod.getAllSystemValueListByKeyName("PaymentFrequency List");
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
                    var ReasonforChange_List = _bulkActionMethod.getAllSystemValueListByKeyName("ReasonforChange List");
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
                    var Currency_List = _bulkActionMethod.getAllSystemValueListByKeyName("Company Setting Currencies");
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
            }
            return modelss;
        }

        public AddSalaryViewModel GetAllListDetails(string EmployeeID, int SalaryID)
        {
            AddSalaryViewModel modelss = new AddSalaryViewModel();
            string[] EmpId = EmployeeID.Split(',');
            for (int i = 0; i < EmpId.Length; i++)
            {
                modelss.Id = SalaryID;
                modelss.EmployeeId = EmpId[i];
                var hr_salary = _bulkActionMethod.GetsalaryListById(SalaryID);
                if (hr_salary != null)
                {
                    modelss.EffectiveFrom = String.Format("{0:dd-MM-yyy}", hr_salary.EffectiveFrom);
                    modelss.Amount = Convert.ToDouble(hr_salary.Amount);
                    modelss.TotalSalary = hr_salary.TotalSalary;
                    modelss.Comments = hr_salary.Comments;
                    var Salary_List = _bulkActionMethod.getAllSystemValueListByKeyName("SalaryType List");
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

                    var PaymentFrequency_List = _bulkActionMethod.getAllSystemValueListByKeyName("PaymentFrequency List");
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
                    var ReasonforChange_List = _bulkActionMethod.getAllSystemValueListByKeyName("ReasonforChange List");
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
                    var Currency_List = _bulkActionMethod.getAllSystemValueListByKeyName("Company Setting Currencies");
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
            }
            return modelss;

        }
        /// <summary>
        //Entitlement
        public ActionResult AddSalaryEntitlementsList(int Id)
        {
            List<AddSalaryEntitlementViewModel> Model = returnsalaryEntitlementsList(Id);
            return PartialView("_PartialSalaryEntitlementListView", Model);
        }
        public List<AddSalaryEntitlementViewModel> returnsalaryEntitlementsList(int Id)
        {
            List<AddSalaryEntitlementViewModel> model = new List<AddSalaryEntitlementViewModel>();
            var data = _db.Employee_Salary_Entitlements.Where(x => x.EmployeeSalaryID == Id && x.Archived == false).ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    var value = _bulkActionMethod.getSystemListValueById((int)item.Entitlement);
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
        public List<AddSalaryEntitlementViewModel> returnsalaryEntitlementsListTemp(int Id)
        {
            List<AddSalaryEntitlementViewModel> model = new List<AddSalaryEntitlementViewModel>();
            var data = _db.Employee_Salary_Entitlement_Temp.Where(x => x.EmployeeSalaryID == Id && x.Archived == false).ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    var value = _bulkActionMethod.getSystemListValueById((int)item.Entitlement);
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
        public ActionResult SaveSalaryEntitlementTemp(AddSalaryViewModel models)
        {
            int userId = SessionProxy.UserId;
            models.CurrentUserId = Convert.ToString(userId);

            AddSalaryEntitlementViewModel model = new AddSalaryEntitlementViewModel();
            model.SalaryTypeList.Add(new SelectListItem() { Text = "-- Select Entitlement --", Value = "0" });
            foreach (var item in _bulkActionMethod.getAllSystemValueListByKeyName("Entitlement List"))
            {
                model.SalaryTypeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            return PartialView("_PartialAddSalaryEntitlementTemp", model);
        }
        public ActionResult SaveSalarytEntitlement(AddSalaryEntitlementViewModel model)
        {
            int userId = SessionProxy.UserId;
            _bulkActionMethod.SavesalaryEntitlementSet(model, userId);
            AddSalaryViewModel modelss = GetAllListDetails(model.EmployeeID, model.EmployeeSalaryID);
            return PartialView("_partialAdd_Bulk_Salary", modelss);
        }
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="raId"></param>
        /// <returns></returns>
        /// 
        //Benefit
        public ActionResult GetAllBenifitsDetail(string empId, string raId)
        {
            BenefitsViewModel model = new BenefitsViewModel();
            model.BenefitList.Add(new SelectListItem() { Text = "-- Select Benefit --", Value = "0" });
            foreach (var item in _bulkActionMethod.getAllSystemValueListByKeyName("Benefit List"))
            {
                model.BenefitList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            foreach (var item in _bulkActionMethod.getAllSystemValueListByKeyName("Company Setting Currencies"))
            {
                model.CurrencyList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            return PartialView("_partialAdd_Bulk_Benifit", model);

        }
        public ActionResult BenefitImageData()
        {
            string FilePath = string.Empty;
            string fileName = string.Empty;
            string originalFileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                FilePath = ConfigurationManager.AppSettings["BenefitDocument"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
        }
        public ActionResult AddEditBenifitsData(BenefitsViewModel model)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<BenefitsDocumentViewModel> listDocument = js.Deserialize<List<BenefitsDocumentViewModel>>(model.DocumentListString);
            _bulkActionMethod.SaveBenefitData(model, listDocument, SessionProxy.UserId);
            return Json("Data Inserted", JsonRequestBehavior.AllowGet);
        }
        //Timesheet
        public ActionResult getTimesheetData(string empId)
        {
            EmployeePlanner_TimeSheetViewModel model = new EmployeePlanner_TimeSheetViewModel();
            model.Date = DateTime.Now.ToString("ddd,dd MMM yyyy");
            model.totoalHrInMonth = getTotalTimeMonth();
            model.totalHrOfWeek = getTottalTimeWeek();
            return PartialView("_partialAdd_Bulk_TimeSheet", model);
        }
        public TimeSpan getTotalTimeMonth()
        {
            int month = System.DateTime.Now.Month;
            var data = _bulkActionMethod.getTotalTimeSheetDuration();
            TimeSpan totalTimeOfMonth = new TimeSpan();
            foreach (var item in data)
            {
                DateTime dataMonth = Convert.ToDateTime(item.Date);
                if (month == dataMonth.Month)
                {
                    TimeSpan t = TimeSpan.Parse(item.Hours);
                    totalTimeOfMonth = totalTimeOfMonth.Add(t);
                }
            }
            return totalTimeOfMonth;
        }
        public TimeSpan getTottalTimeWeek()
        {
            int Curruentweek = GetWeekNumber(DateTime.Now);
            int month = System.DateTime.Now.Month;
            int year = System.DateTime.Now.Year;
            var data = _bulkActionMethod.getTotalTimeSheetDuration();
            TimeSpan totalTimeOfWeek = new TimeSpan();
            foreach (var item in data)
            {
                DateTime dataMonth = Convert.ToDateTime(item.Date);
                int dataWeek = GetWeekNumber(dataMonth);
                if (year == dataMonth.Year && month == dataMonth.Month && Curruentweek == dataWeek)
                {
                    TimeSpan t = TimeSpan.Parse(item.Hours);
                    totalTimeOfWeek = totalTimeOfWeek.Add(t);
                }
            }
            return totalTimeOfWeek;
        }
        public static int GetWeekNumber(DateTime now)
        {
            CultureInfo ci = CultureInfo.CurrentCulture;
            int weekNumber = ci.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNumber;
        }
        public ActionResult TimeSheetImageData()
        {
            string FilePath = string.Empty;
            string fileName = string.Empty;
            string originalFileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                FilePath = ConfigurationManager.AppSettings["Planner_TimeSheet"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
        }

        public ActionResult getCustomer()
        {
            EmployeeProjectPlanner_Scheduling_DocumnetsViewModel model = new EmployeeProjectPlanner_Scheduling_DocumnetsViewModel();
            var customerList = _bulkActionMethod.getAllCustomer().Where(x => x.Archived == false).ToList();
            foreach (var item in customerList)
            {
                model.CustomerList.Add(new SelectListItem() { Text = item.FirstName + " " + item.LastName + " " + item.SSOID, Value = item.Id.ToString() });
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllTimeSheetDetail(string empId)
        {
            EmployeePlanner_TimeSheet_DetailViewModel model = new EmployeePlanner_TimeSheet_DetailViewModel();
            model.CostCodeList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in _bulkActionMethod.getAllSystemValueListByKeyName("Cost Code List"))
            {
                model.CostCodeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }

            var projectList = _bulkActionMethod.getAllList().Where(x => x.Archived == false).ToList();
            model.ProjectList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in projectList)
            {
                model.ProjectList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }

            var customerList = _bulkActionMethod.getAllCustomer().Where(x => x.Archived == false).ToList();
            model.CustomerList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in customerList)
            {
                model.CustomerList.Add(new SelectListItem() { Text = item.FirstName + " " + item.LastName, Value = item.Id.ToString() });
            }

            model.AssetList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in _bulkActionMethod.getAllSystemValueListByKeyName("Asset Type List"))
            {
                model.AssetList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            for (int i = 1; i <= 60; i++)
            {
                model.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            for (int i = 1; i <= 24; i++)
            {
                model.HoursList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return PartialView("_partialBulkAdd_TimeSheet_Detail", model);
        }
        public ActionResult SaveData_TimeSheet(EmployeePlanner_TimeSheetViewModel model)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            BulkActionsMethod _bsActionMethod = new BulkActionsMethod();
            List<EmployeePlanner_TimeSheet_DocumentsViewModel> listDocument = js.Deserialize<List<EmployeePlanner_TimeSheet_DocumentsViewModel>>(model.jsonDocumentList);
            model.DocumentList = listDocument;
            List<EmployeePlanner_TimeSheet_DetailViewModel> listDetail = js.Deserialize<List<EmployeePlanner_TimeSheet_DetailViewModel>>(model.jsonDetailList);
            model.DetailList = listDetail;
            _bsActionMethod.TimeSheet_SaveData(model, SessionProxy.UserId);
            return PartialView("_partialAdd_Bulk_TimeSheet", model);
        }
        //public JsonResult GetWorkPatternData(EmployeePlanner_TimeSheetViewModel model)
        //{
        //    EmployeeMethod _employeeMethod = new EmployeeMethod();
        //    JavaScriptSerializer js = new JavaScriptSerializer();
        //    List<EmployeePlanner_TimeSheet_DetailViewModel> listDetail = js.Deserialize<List<EmployeePlanner_TimeSheet_DetailViewModel>>(model.jsonDetailList);
        //    model.DetailList = listDetail;
        //    string inputFormat = "dd-MM-yyyy";

        //    var wData = _employeeMethod.getWorkPatternById(model.EmployeeId).OrderByDescending(x => x.EffectiveFrom).FirstOrDefault();
        //    DateTime dt = DateTime.ParseExact(model.Date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //    bool flag = false;
        //    foreach (var data in model.DetailList)
        //    {
        //        string strSt = data.InTimeHr + "." + data.InTimeMin;
        //        decimal st_Time = Convert.ToDecimal(strSt);
        //        string strEt = data.EndTimeHr + "." + data.EndTimeMin;
        //        decimal et_Time = Convert.ToDecimal(strEt);

        //        if (dt.DayOfWeek == DayOfWeek.Sunday)
        //        {
        //            if ((st_Time >= wData.SundayStart && st_Time <= wData.SundayEnd) && (et_Time >= wData.SundayStart && et_Time <= wData.SundayEnd))
        //            {
        //                decimal TotalSundayTime = Convert.ToDecimal(wData.SundayStart) - Convert.ToDecimal(wData.SundayEnd);
        //                if (wData.sundayBreak != null && wData.sundayBreak != 0)
        //                {
        //                    decimal sundayBreakTime = Convert.ToDecimal(wData.sundayBreak);
        //                    decimal totalTime = TotalSundayTime - sundayBreakTime;
        //                }
        //                flag = true;
        //            }
        //            //if((st_Time>=wData.SundayStart&& st_Time>=wData.SundayEnd)&&(et_Time>=wData.SundayStart&& et_Time<=wData.SundayEnd))
        //            //{
        //            //    flag = false;
        //            //}
        //            //if((st_Time>=wData.SundayStart&&st_Time<=wData.SundayEnd)&&(et_Time>=wData.SundayEnd&&et_Time<=wData.SundayEnd))
        //            //{
        //            //    flag = false;
        //            //}
        //        }
        //        else if (dt.DayOfWeek == DayOfWeek.Monday)
        //        {
        //            if ((st_Time >= wData.MondayStart && st_Time <= wData.MondayEnd && (et_Time >= wData.MondayStart && et_Time <= wData.MondayEnd)))
        //            {
        //                decimal TotalMondayTime = Convert.ToDecimal(wData.MondayStart) - Convert.ToDecimal(wData.MondayEnd);
        //                if (wData.mondayBreak != null && wData.mondayBreak != 0)
        //                {
        //                    decimal mondayBreakTime = Convert.ToDecimal(wData.sundayBreak);
        //                    decimal totalTime = TotalMondayTime - mondayBreakTime;
        //                }
        //                flag = true;
        //            }
        //        }
        //        else if (dt.DayOfWeek == DayOfWeek.Tuesday)
        //        {
        //            if ((st_Time >= wData.TuesdayStart && st_Time <= wData.TuesdayEnd && (et_Time >= wData.TuesdayStart && et_Time <= wData.TuesdayEnd)))
        //            {
        //                decimal TotalTime = Convert.ToDecimal(wData.TuesdayStart) - Convert.ToDecimal(wData.TuesdayEnd);
        //                if (wData.mondayBreak != null && wData.mondayBreak != 0)
        //                {
        //                    decimal BreakTime = Convert.ToDecimal(wData.sundayBreak);
        //                    decimal totalTime = TotalTime - BreakTime;
        //                }
        //                flag = true;
        //            }
        //        }
        //        else if (dt.DayOfWeek == DayOfWeek.Wednesday)
        //        {
        //            if ((st_Time >= wData.WednessdayStart && st_Time <= wData.WednessdayEnd && (et_Time >= wData.WednessdayStart && et_Time <= wData.WednessdayEnd)))
        //            {
        //                decimal TotalTime = Convert.ToDecimal(wData.WednessdayStart) - Convert.ToDecimal(wData.WednessdayEnd);
        //                if (wData.wednessdayBreak != null && wData.wednessdayBreak != 0)
        //                {
        //                    decimal BreakTime = Convert.ToDecimal(wData.wednessdayBreak);
        //                    decimal totalTime = TotalTime - BreakTime;
        //                }
        //                flag = true;
        //            }
        //        }
        //        else if (dt.DayOfWeek == DayOfWeek.Thursday)
        //        {
        //            if ((st_Time >= wData.ThursdayStart && st_Time <= wData.ThursdayEnd && (et_Time >= wData.ThursdayStart && et_Time <= wData.ThursdayEnd)))
        //            {
        //                decimal TotalTime = Convert.ToDecimal(wData.ThursdayStart) - Convert.ToDecimal(wData.ThursdayEnd);
        //                if (wData.thursdayBreak != null && wData.thursdayBreak != 0)
        //                {
        //                    decimal BreakTime = Convert.ToDecimal(wData.thursdayBreak);
        //                    decimal totalTime = TotalTime - BreakTime;
        //                }
        //                flag = true;
        //            }
        //        }
        //        else if (dt.DayOfWeek == DayOfWeek.Friday)
        //        {
        //            if ((st_Time >= wData.FridayStart && st_Time <= wData.FridayEnd && (et_Time >= wData.FridayStart && et_Time <= wData.FridayEnd)))
        //            {
        //                decimal TotalTime = Convert.ToDecimal(wData.FridayStart) - Convert.ToDecimal(wData.FridayEnd);
        //                if (wData.fridayBreak != null && wData.fridayBreak != 0)
        //                {
        //                    decimal BreakTime = Convert.ToDecimal(wData.fridayBreak);
        //                    decimal totalTime = TotalTime - BreakTime;
        //                }
        //                flag = true;
        //            }
        //        }
        //        else if (dt.DayOfWeek == DayOfWeek.Saturday)
        //        {
        //            if ((st_Time >= wData.SaturdayStart && st_Time <= wData.SaturdayEnd && (et_Time >= wData.SaturdayStart && et_Time <= wData.SaturdayEnd)))
        //            {
        //                decimal TotalTime = Convert.ToDecimal(wData.SaturdayStart) - Convert.ToDecimal(wData.SaturdayEnd);
        //                if (wData.SaturdayStart != null && wData.saturdayBreak != 0)
        //                {
        //                    decimal BreakTime = Convert.ToDecimal(wData.saturdayBreak);
        //                    decimal totalTime = TotalTime - BreakTime;
        //                }
        //                flag = true;
        //            }
        //        }

        //    }
        //    return Json(flag, JsonRequestBehavior.AllowGet);

        //}

        //Scheduling
        public ActionResult AddEdit_Scheduling(string EmpID, bool? IsLessDay, bool? IsDayorMore, DateTime? startDate, DateTime? EndDate, decimal? duation, int? customer, int? project, int? asset, string comment,
             decimal? HrSD, decimal? MinSD, DateTime? CDate, decimal? HrED, decimal? MinED, decimal? durationHr)
        {
            BulkActionsMethod _bsActionMethod = new BulkActionsMethod();
            EmployeeProjectPlanner_Scheduling_DocumnetsViewModel model = new EmployeeProjectPlanner_Scheduling_DocumnetsViewModel();
            string[] values = EmpID.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                _bulkActionMethod.saveSchedulingData(Convert.ToInt32(values[i]), IsLessDay, IsDayorMore, startDate, EndDate, duation, customer, project, asset, comment,
                    HrSD, MinSD, CDate, HrED, MinED, durationHr);
            }
            return PartialView("_partialAdd_Bulk_Scheduling", model);
        }
        public ActionResult GetAllEmployeeResult(string empId)
        {
            BulkActionsMethod _bsActionMethod = new BulkActionsMethod();
            EmployeeProjectPlanner_Scheduling_DocumnetsViewModel model = new EmployeeProjectPlanner_Scheduling_DocumnetsViewModel();
            model.AssetList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in _bsActionMethod.getAllSystemValueListByKeyName("Asset Type List"))
            {
                model.AssetList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            var projectList = _bsActionMethod.getAllList().Where(x => x.Archived == false).ToList();
            model.ProjectList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in projectList)
            {
                model.ProjectList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            var customerList = _bsActionMethod.getAllCustomer().Where(x => x.Archived == false).ToList();
            model.CustomerList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in customerList)
            {
                model.CustomerList.Add(new SelectListItem() { Text = item.FirstName + " " + item.LastName, Value = item.Id.ToString() });
            }
            for (int i = 1; i <= 60; i++)
            {
                model.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            for (int i = 1; i <= 24; i++)
            {
                model.HoursList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            return PartialView("_partialAdd_Bulk_Scheduling", model);
        }
        //Travel
        public ActionResult TravelLeaveImageData()
        {
            string FilePath = string.Empty;
            string fileName = string.Empty;
            string originalFileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                FilePath = ConfigurationManager.AppSettings["ProjectPlanner_TravelLeave"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
        }
        public ActionResult AddEdit_TravelLeave(EmployeeProjectPlanner_TravelLeaveViewModel model)
        {
            BulkActionsMethod _bsActionMethod = new BulkActionsMethod();
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<EmployeeProjectPlanner_TravelLeave_DocumentsViewModel> listDocument = js.Deserialize<List<EmployeeProjectPlanner_TravelLeave_DocumentsViewModel>>(model.jsonDocumentList);
            model.DocumentList = listDocument;
            _bsActionMethod.TravelLeave_SaveData(model, SessionProxy.UserId);
            return PartialView("_partialAdd_Bulk_TravelLeave", model);

        }
        public ActionResult GetAllEmployeeTravelLeave(string empId)
        {
            EmployeeProjectPlanner_TravelLeaveViewModel model = new EmployeeProjectPlanner_TravelLeaveViewModel();
            BulkActionsMethod _bsActionMethod = new BulkActionsMethod();

            var projectList = _bsActionMethod.getAllList().Where(x => x.Archived == false).ToList();
            model.ProjectList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in projectList)
            {
                model.ProjectList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            var customerList = _bsActionMethod.getAllCustomer().Where(x => x.Archived == false).ToList();
            model.CustomerList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in customerList)
            {
                model.CustomerList.Add(new SelectListItem() { Text = item.FirstName + " " + item.LastName, Value = item.Id.ToString() });
            }
            model.ReasonForTravelList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in _bsActionMethod.getAllSystemValueListByKeyName("Travel Leave Reason List"))
            {
                model.ReasonForTravelList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }

            model.TravelTypeList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in _bsActionMethod.getAllSystemValueListByKeyName("Travel Type List"))
            {
                model.TravelTypeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }

            model.CostCodeList.Add(new SelectListItem() { Text = "-- Select --", Value = "0" });
            foreach (var item in _bsActionMethod.getAllSystemValueListByKeyName("Cost Code List"))
            {
                model.CostCodeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            model.FromCountryList = _bulkActionMethod.BindCountryDropdown();
            model.ToCountryList = _bulkActionMethod.BindCountryDropdown();
            for (int i = 1; i <= 60; i++)
            {
                model.MinutesList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            for (int i = 1; i <= 24; i++)
            {
                model.HoursList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            return PartialView("_partialAdd_Bulk_TravelLeave", model);
        }

        //Holiday
        public ActionResult getHoliday()
        {
            HolidayViewModel model = new HolidayViewModel();
            DateTime date = DateTime.Now;
            model.startDate = String.Format("{0:dd-MM-yyy}", date);
            model.endDate = String.Format("{0:dd-MM-yyy}", date);
            model.Duration = 1;
            foreach (var item in _bulkActionMethod.getAllSystemValueListByKeyName("Time List"))
            {
                model.TimeList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            return PartialView("_partialAdd_Bulk_Holiday", model);
        }
        public ActionResult SaveBookHoliday(HolidayViewModel model)
        {
            BulkActionsMethod _bsActionMethod = new BulkActionsMethod();
            _bsActionMethod.BookHoliday_saveData(model);
            return PartialView("_partialAdd_Bulk_Holiday", model);
        }



        //Index
        public ActionResult BindStateDropdown(int countryId)
        {
            try
            {
                var state = _bulkActionMethod.BindStateDropdown(countryId);
                return Json(state, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }
        public ActionResult BindCityDropdown(int stateId)
        {
            try
            {
                var city = _bulkActionMethod.BindCityDropdown(stateId);
                return Json(city, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }

        }
        public ActionResult BindAirportDropdown(int countryId)
        {
            try
            {
                var state = _bulkActionMethod.BindAirportDropdown(countryId);
                return Json(state, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }
        public ActionResult getEmployeeJobTitle()
        {
            var data = _db.GetAllEmployeeJobTitle().ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getEmployeeCountry()
        {
            //     ViewBag.countryOfR(_db.GetAllEmployeeCountry().ToList());
            var data = _db.GetAllEmployeeCountry().ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getDivision(int businessId)
        {

            var data = _db.Divisions.Where(x => x.Archived == false && x.BusinessID == businessId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getPool(int divisionId)
        {
            var data = _db.Pools.Where(c => c.DivisionID == divisionId && c.Archived == false);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getFunction(int divisionId)
        {
            var data = _db.Functions.Where(c => c.DivisionID == divisionId && c.Archived == false);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getSerchList(int busiId, int divId, int poolId, int funId, int jobId, int counId)
        {
            List<AllDataList> sd = new List<AllDataList>();
            var data = _bulkActionMethod.getSearchList(busiId, divId, poolId, funId, jobId, counId);
            if (data.Count > 0)
            {
                foreach (var details in data)
                {
                    AllDataList datamodel = new AllDataList();
                    //datamodel.EmployeeName = details.FirstName+" "+details.LastName;
                    datamodel.JobTitle = details.JobTitle;
                    datamodel.DivisionName = details.DivisionName;
                    datamodel.FunctionName = details.FunctionName;
                    datamodel.PoolName = details.PoolName;
                    datamodel.BusinessName = details.BusinessName;
                    datamodel.CountryOfR = details.CountryName;
                    datamodel.EmpId = details.EmpId;
                    sd.Add(datamodel);
                }
            }
            return PartialView("_PartialSearchData", data);
        }
        public ActionResult getSerchData()
        {
            List<AllDataList> sd = new List<AllDataList>();
            var data = _bulkActionMethod.getSearchData();
            if (data.Count > 0)
            {
                foreach (var details in data)
                {
                    AllDataList datamodel = new AllDataList();
                    datamodel.EmployeeName = details.Name;
                    datamodel.JobTitle = details.JobTitle;
                    datamodel.DivisionName = details.DivisionName;
                    datamodel.FunctionName = details.FunctionName;
                    datamodel.PoolName = details.PoolName;
                    datamodel.BusinessName = details.BusinessName;
                    datamodel.CountryOfR = details.CountryName;
                    sd.Add(datamodel);
                }
            }
            return PartialView("_PartialSearchData", data);
        }
        // GET: BulkActions/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: BulkActions/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: BulkActions/Create
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
        // GET: BulkActions/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        // POST: BulkActions/Edit/5
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
        // GET: BulkActions/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: BulkActions/Delete/5
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

        //BulkResource
        public ActionResult getBulkResourceFile()
        {
            int UserId = SessionProxy.UserId;
            AddResourceBulk model = new AddResourceBulk();
            model.CreateEmpId = UserId;
            model.ResourceSheetFormPath = "BulkAddResource.xlsx";
            return PartialView("_PartialAddBulkResource", model);
        }

        public ActionResult FileUploadData()
        {
            try
            {
                string FilePath = string.Empty;
                string fileName = string.Empty;
                string originalFileName = string.Empty;
                string extension = string.Empty;
                AddResourceBulk model = new AddResourceBulk();
                if (Request.Files.Count > 0)
                {
                    FilePath = ConfigurationManager.AppSettings["TMSApplicant"].ToString();
                    HttpPostedFileBase file = Request.Files[0] as HttpPostedFileBase;
                    originalFileName = file.FileName;
                    fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(file.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(file.FileName));
                    extension = Path.GetExtension(file.FileName);
                    if (extension == ".xls" || extension == ".xlsx")
                    {
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        string inputFormat = "dd-MM-yyyy";
                        string outputFormat = "yyyy-MM-dd HH:mm:ss";
                        model.FileOriginalPath = file.FileName;
                        model.FilePath = fileName;
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;
                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                AddResourceBulk user = new AddResourceBulk();
                                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 1].Value != "")
                                {
                                    var UserNameVal = workSheet.Cells[rowIterator, 1].Value;
                                    if (UserNameVal != null && UserNameVal != "")
                                    {
                                        user.UserName = UserNameVal.ToString();
                                    }
                                    else
                                    {
                                        user.UserName = "";
                                    }
                                }
                                if (workSheet.Cells[rowIterator, 2].Value != null && workSheet.Cells[rowIterator, 2].Value != "")
                                {
                                    var Title = _bulkActionMethod.getAllSystemValueListByKeyName("Title List");
                                    var NameTitle = workSheet.Cells[rowIterator, 2].Value;
                                    if (NameTitle != null)
                                    {
                                        NameTitle = Convert.ToString(NameTitle).ToUpper();
                                        if (NameTitle.ToString() == "MR" || NameTitle.ToString() == "MR.")
                                        {
                                            user.NameTitle = Title.Where(x => x.Value == "Mr.").FirstOrDefault().Id;
                                        }
                                        else if (NameTitle.ToString() == "MRS" || NameTitle.ToString() == "MRS.")
                                        {
                                            user.NameTitle = Title.Where(x => x.Value == "Mrs.").FirstOrDefault().Id;
                                        }
                                        else if (NameTitle.ToString() == "MISS" || NameTitle.ToString() == "MISS.")
                                        {
                                            user.NameTitle = Title.Where(x => x.Value == "Miss.").FirstOrDefault().Id;
                                        }
                                        else if (NameTitle.ToString() == "DR" || NameTitle.ToString() == "DR.")
                                        {
                                            user.NameTitle = Title.Where(x => x.Value == "Dr.").FirstOrDefault().Id;
                                        }
                                        else if (NameTitle.ToString() == "MS" || NameTitle.ToString() == "MS.")
                                        {
                                            user.NameTitle = Title.Where(x => x.Value == "Ms.").FirstOrDefault().Id;
                                        }
                                    }
                                    else
                                    {
                                        user.NameTitle = 0;
                                    }
                                }
                                if (workSheet.Cells[rowIterator, 3].Value != null && workSheet.Cells[rowIterator, 3].Value != "")
                                {
                                    var FirstNameVal = workSheet.Cells[rowIterator, 3].Value;
                                    if (FirstNameVal != null && FirstNameVal != "")
                                    {
                                        user.FirstName = Convert.ToString(FirstNameVal);
                                    }
                                    else
                                    {
                                        user.FirstName = "";
                                    }
                                }
                                if (workSheet.Cells[rowIterator, 4].Value != null && workSheet.Cells[rowIterator, 4].Value != "")
                                {
                                    var LastName = workSheet.Cells[rowIterator, 4].Value;
                                    if (LastName != null && LastName != "")
                                    {
                                        user.LastName = LastName.ToString();
                                    }
                                    else
                                    {
                                        user.LastName = "";
                                    }
                                }
                                if (workSheet.Cells[rowIterator, 5].Value != null && workSheet.Cells[rowIterator, 5].Value != "")
                                {
                                    var SSOID = workSheet.Cells[rowIterator, 5].Value;
                                    if (SSOID != null && SSOID != "")
                                    {
                                        bool s = _bulkActionMethod.validateSSOID(SSOID.ToString().ToUpper());
                                        if (s)
                                        {
                                            user.SSOID = "Already Exist SSO";
                                        }
                                        else
                                        {
                                            user.SSOID = "W" + SSOID.ToString().ToUpper();
                                        }

                                    }
                                    else
                                    {
                                        user.SSOID = "";
                                    }
                                }
                                if (workSheet.Cells[rowIterator, 6].Value != null && workSheet.Cells[rowIterator, 6].Value != "")
                                {
                                    var ganderName = workSheet.Cells[rowIterator, 6].Value;
                                    if (ganderName != null)
                                    {
                                        var GenderList = _bulkActionMethod.getAllSystemValueListByKeyName("Gender List");
                                        ganderName = ganderName.ToString().ToUpper();
                                        if (ganderName.ToString() == "MALE")
                                        {
                                            user.Gender = GenderList.Where(x => x.Value == "Male").FirstOrDefault().Id;
                                        }
                                        else if (ganderName.ToString() == "FEMALE")
                                        {
                                            user.Gender = GenderList.Where(x => x.Value == "Female").FirstOrDefault().Id;
                                        }
                                    }
                                    else
                                    {
                                        user.Gender = 0;
                                    }
                                }
                                if (workSheet.Cells[rowIterator, 7].Value != null && workSheet.Cells[rowIterator, 7].Value != "")
                                {
                                    var DOB = workSheet.Cells[rowIterator, 7].Value;
                                    if (DOB != null && DOB != "")
                                    {
                                        if (DOB.ToString().Contains("/"))
                                        {
                                            DOB = DOB.ToString().Replace('/', '-');
                                            var DateOfBirthToString = DateTime.ParseExact(DOB.ToString(), inputFormat, CultureInfo.InvariantCulture);
                                            int age = _bulkActionMethod.calculateAge(Convert.ToDateTime(DOB));
                                            if (age > 18)
                                            {
                                                user.DateOfBirth = Convert.ToDateTime(DateOfBirthToString.ToString(outputFormat));
                                            }
                                            else
                                            {
                                                user.DateOfBirth = null;
                                            }

                                        }
                                        else if (DOB.ToString().Contains("-"))
                                        {
                                            var DateOfBirthToString = DateTime.ParseExact(DOB.ToString(), inputFormat, CultureInfo.InvariantCulture);
                                            user.DateOfBirth = Convert.ToDateTime(DateOfBirthToString.ToString(outputFormat));
                                            int age = _bulkActionMethod.calculateAge(Convert.ToDateTime(DOB));
                                            if (age > 18)
                                            {
                                                user.DateOfBirth = Convert.ToDateTime(DateOfBirthToString.ToString(outputFormat));
                                            }
                                            else
                                            {
                                                user.DateOfBirth = null;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        user.DateOfBirth = null;
                                    }
                                }
                                if (workSheet.Cells[rowIterator, 8].Value != null && workSheet.Cells[rowIterator, 8].Value != "")
                                {
                                    var startDate = workSheet.Cells[rowIterator, 8].Value.ToString();
                                    if (startDate != null && startDate != "")
                                    {
                                        if (startDate.ToString().Contains("/"))
                                        {
                                            startDate = startDate.ToString().Replace('/', '-');
                                            var DateOfBirthToString = DateTime.ParseExact(startDate.ToString(), inputFormat, CultureInfo.InvariantCulture);
                                            user.StartDate = Convert.ToDateTime(DateOfBirthToString.ToString(outputFormat));
                                        }
                                        else if (startDate.ToString().Contains("-"))
                                        {
                                            var DateOfBirthToString = DateTime.ParseExact(startDate.ToString(), inputFormat, CultureInfo.InvariantCulture);
                                            user.StartDate = Convert.ToDateTime(DateOfBirthToString.ToString(outputFormat));
                                        }
                                    }

                                    else
                                    {
                                        user.StartDate = null;
                                    }
                                }
                                if (user.UserName != "" && user.UserName != null && user.NameTitle != 0 && user.NameTitle != null && user.LastName != null && user.LastName != "" &&
                                 user.SSOID != "" && user.SSOID != null && user.SSOID != "Already Exist SSO" && user.FirstName != "" && user.FirstName != null && user.StartDate != null)
                                {
                                    model.userList.Add(user);
                                }
                                //else if (user.SSOID == "Already Exist SSO")
                                //{
                                //    model.alreadyExistUserList.Add(user);
                                //}
                                else
                                {
                                    model.emptyuserList.Add(user);
                                }


                            }
                            if (model.emptyuserList == null || model.emptyuserList.Count == 0)
                            {
                                if (model.userList.Count > 0 && model.userList != null)
                                {
                                    _bulkActionMethod.saveBulkResource(model.userList);
                                }
                            }
                        }
                    }
                    string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                    file.SaveAs(path);
                }
                return PartialView("_PartialAddBulkResource", model);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ActionResult SendResumeCVToCustomer(string empId)
        {
            SendCVResumeToCustomer model = new SendCVResumeToCustomer();
            return PartialView("_PartialBulkSendCV_Resume", model);
        }
        public ActionResult getBulkResourceData()
        {
            SendCVResumeToCustomer model = new SendCVResumeToCustomer();
            var data = _db.AspNetUsers.Where(x => x.SSOID.StartsWith("C") && x.Archived == false).ToList();
            foreach (var item in data)
            {
                model.CustomerList.Add(new SelectListItem() { Text = item.FirstName + " " + item.LastName + "-" + item.SSOID, Value = item.Id.ToString() });
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult sendResumeCV(string Id, string mail)
        {
            string[] EmpId = Id.Split(',');
            genaratePDF(Id, mail);
            SendCVResumeToCustomer model = new SendCVResumeToCustomer();
            return PartialView("_PartialBulkSendCV_Resume", model);
        }

        private void CompressToZip(string fileName, Dictionary<string, byte[]> fileList, string mailString)
        {
            string FilePath = string.Empty;
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in fileList)
                    {
                        var demoFile = archive.CreateEntry(file.Key);
                        using (var entryStream = demoFile.Open())
                        using (var b = new BinaryWriter(entryStream))
                        {
                            b.Write(file.Value);
                        }
                    }
                }
                FilePath = ConfigurationManager.AppSettings["TMSApplicant"].ToString();
                string filePath = HttpContext.Server.MapPath(FilePath);
                using (var fileStream = new FileStream(@filePath + fileName, FileMode.Create))
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    memoryStream.CopyTo(fileStream);
                }
                string[] EmailArr = mailString.Split(',');
                for (int i = 0; i < EmailArr.Length; i++)
                {
                    string toMail = EmailArr[i];
                    if (!string.IsNullOrEmpty(toMail))
                    {
                        HRTool.Models.MailModel mail = new HRTool.Models.MailModel();
                        mail.From = "ami.gajjar@arhamtechnosoft.co.in";
                        mail.To = toMail;
                        mail.Subject = "Resume_CV";
                        string inputFormat = "ddd, dd MMM yyyy";
                        string dateTimeEndorse = DateTime.Now.ToString("ddd, dd MMM yyyy");
                        mail.Body = "Thank You";
                        mail.AttachmentPath = @filePath + fileName;
                        string mailFromReceive = Common.sendMail(mail);
                    }
                }
                //if (System.IO.File.Exists(@filePath + fileName))
                //{
                //    System.IO.File.Delete(@filePath + fileName);
                //}


            }
        }
        public ActionResult genaratePDF(string EmployeeId, string mailString)
        {
            try
            {
                Dictionary<string, byte[]> fileList = new Dictionary<string, byte[]>();
                SendCVResumeToCustomer modeldATA = new SendCVResumeToCustomer();
                DateTime currentDate = DateTime.Now;
                string[] EmpId = EmployeeId.Split(',');
                for (int i = 0; i < EmpId.Length; i++)
                {
                    int eid = Convert.ToInt32(EmpId[i]);
                    string FilePath = string.Empty;
                    HRTool.Models.Resources.EmployeeResumePDFViewModel model = new HRTool.Models.Resources.EmployeeResumePDFViewModel();
                    var EmployeeDetails = _bulkActionMethod.getBulkEmployeeById(eid);//_db.AspNetUsers.Where(x => x.Id == EmployeeId).FirstOrDefault();
                    model.EmployeeID = eid;
                    model.FirstName = EmployeeDetails.FirstName;
                    model.LastName = EmployeeDetails.LastName;
                    string newfileName = string.Format("" + model.FirstName + "_" + model.LastName + "_Resume.pdf", currentDate.Date);
                    FilePath = ConfigurationManager.AppSettings["TMSApplicant"].ToString();
                    string filePath = Path.Combine(HttpContext.Server.MapPath(FilePath), newfileName);
                    var actionPDF = new Rotativa.ActionAsPdf("ResumePDF", model)
                    {
                        PageSize = Size.A4,
                        PageOrientation = Orientation.Landscape,
                        FileName = newfileName,
                        //SaveOnServerPath= filePath
                    };
                    byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                    var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                    fileStream.Write(applicationPDFData, 0, applicationPDFData.Length);
                    fileStream.Close();
                    byte[] file1 = System.IO.File.ReadAllBytes(@filePath);
                    fileList.Add(newfileName, file1);
                }
                CompressToZip("ResourceResumeCV_1.zip", fileList, mailString);
                return PartialView("_PartialBulkSendCV_Resume", modeldATA);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult ResumePDF(HRTool.Models.Resources.EmployeeResumePDFViewModel dataModel)
        {
            var EmployeeDetails = _bulkActionMethod.getBulkEmployeeById(dataModel.EmployeeID);
            var EmployeeAddress = _db.EmployeeAddressInfoes.Where(x => x.UserId == dataModel.EmployeeID).FirstOrDefault();
            dataModel.FirstName = EmployeeDetails.FirstName;
            dataModel.LastName = EmployeeDetails.LastName;
            dataModel.Address = EmployeeAddress.ContactAddress;
            dataModel.Email = EmployeeAddress.PersonalEmail;
            dataModel.PersonalEmail = EmployeeAddress.PersonalEmail;
            dataModel.PersonalPhone = EmployeeAddress.PersonalPhone;
            //model.JobTitle
            //model.ResumeText
            //HRTool.Models.Resources.EmployeeResumeViewModel Alldetails = AllDetailsList(dataModel.EmployeeID);
            //dataModel.AllDetails = Alldetails;
            //dataModel.ResumeText = StripHTML(Alldetails.ResumeText);
            //if (Alldetails.WorkExperienceList.Count > 0)
            //{
            //    var list = Alldetails.WorkExperienceList.OrderBy(x => x.Id).LastOrDefault();
            //    dataModel.JobTitle = list.JobTitle;

            //}

            return View(dataModel);
        }

        public ActionResult getEmployeeEmail(int EmployeeId)
        {
            SendCVResumeToCustomer model = new SendCVResumeToCustomer();
            if (EmployeeId != 0)
            {
                var data = _db.AspNetUsers.Where(x => x.SSOID.StartsWith("C") && x.Archived == false && x.Id == EmployeeId).FirstOrDefault();
                //foreach (var item in data)
                //{
                //    model.CustomerEmailList.Add(new SelectListItem() { Text = item.UserName, Value = item.Id.ToString() });
                //}
                model.CustomerEmail = data.UserName;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //Bulk ResourceSetting
        public ActionResult getBulkEmployeeSetting(string EmpId)
        {
            BulkEmployeeSetting model = new BulkEmployeeSetting();

            var jobTitleList = _bulkActionMethod.getAllSystemValueListByKeyName("Job Title List");
            model.JobTitle.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in jobTitleList)
            {
                model.JobTitle.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var LocationList = _bulkActionMethod.getAllSystemValueListByKeyName("Location List");
            model.Location.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in LocationList)
            {
                model.Location.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var ResourceTypeList = _bulkActionMethod.getAllSystemValueListByKeyName("Job Role List");
            model.ResourceType.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in ResourceTypeList)
            {
                model.ResourceType.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var NoticePeriodList = _bulkActionMethod.getAllSystemValueListByKeyName("Notice Period List");
            model.NoticePeriod.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in NoticePeriodList)
            {
                model.NoticePeriod.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var Company = _bulkActionMethod.getAllSystemValueListByKeyName("Company List");
            model.CompanyList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in Company)
            {
                model.CompanyList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var BusinessList = _bulkActionMethod.getAllBusinessList();
            model.BusinessList.Add(new SelectListItem() { Text = "--Select--", Value = "0" });
            foreach (var item in BusinessList)
            {
                model.BusinessList.Add(new SelectListItem() { Text = @item.Name, Value = @item.Id.ToString() });
            }

            return PartialView("_PartialBulkEmployeeSetting", model);
        }
        public ActionResult getEmpCopyData()
        {
            BulkEmployeeSetting model = new BulkEmployeeSetting();
            List<AspNetUser> data = _AdminPearformanceMethod.getAllUserList().ToList();
            foreach (var item in data)
            {
                string Name = string.Format("{0} {1}-{2}", item.FirstName, item.LastName, item.SSOID);
                model.ReportstoList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
                model.AdditionalReportstoList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
                model.HRResponsibleList.Add(new SelectListItem() { Text = Name, Value = @item.Id.ToString() });
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult saveEmployeeSetting(BulkEmployeeSetting model)
        {
            _bulkActionMethod.SaveBulkEmployeeSetting(model);
            return PartialView("_PartialBulkEmployeeSetting", model);
        }

        public ActionResult getBulkAccesssetup(string empId)
        {
            BulkAccessSetup model = new BulkAccessSetup();
            return PartialView("_PartialAdd_Bulk_Accesssetup", model);
        }
        public ActionResult getAccesRoleData()
        {
            BulkAccessSetup model = new BulkAccessSetup();
            var aspnetroles = _db.AspNetRoles.Where(x => x.Active == true).ToList();
            foreach (var item in aspnetroles)
            {
                model.AspnetUsersRoleList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult saveBulkAspnetRole(BulkAccessSetup model)
        {
            string[] EmpId = model.EmployeeId.Split(',');
            if (EmpId != null)
            {
                for (int i = 0; i < EmpId.Length; i++)
                {
                    int eId = Convert.ToInt32(EmpId[i]);
                    var userData = _db.AspNetUsers.Where(x => x.Id == eId).FirstOrDefault();
                    _db.UpdateUserRole(userData.Id, model.AspnetRoleId);

                    var RoleData = _db.AspNetRoles.Where(x => x.Id == model.AspnetRoleId).FirstOrDefault();
                    var menuList = _db.Menu_List.ToList();
                    List<UserMenu> UserMenuList = _db.UserMenus.Where(x => x.UserID == userData.Id).ToList();
                    if (UserMenuList.Count > 0)
                    {
                        foreach (var item in UserMenuList)
                        {
                            _db.UserMenus.Remove(item);
                            _db.SaveChanges();
                        }
                    }

                    var defaultMenus = _RolesManagementMethod.getDefaultMenuByRoleId(model.AspnetRoleId);
                    foreach (var item in defaultMenus)
                    {
                        int menuId = menuList.Where(x => x.ID == item.MenuKey).FirstOrDefault().ID;
                        UserMenu userMenu = new UserMenu();
                        userMenu.MenuID = menuId;
                        userMenu.UserID = eId;
                        userMenu.CreatedDate = DateTime.Now;
                        userMenu.CreatedBy = SessionProxy.UserId;
                        userMenu.IsActive = true;
                        userMenu.MenuKey = item.MenuKey;
                        _db.UserMenus.Add(userMenu);
                        _db.SaveChanges();
                    }
                }
            }

            return PartialView("_PartialAdd_Bulk_Accesssetup", model);
        }
    }
}        
 
