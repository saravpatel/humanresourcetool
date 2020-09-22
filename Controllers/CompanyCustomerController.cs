using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HRTool.CommanMethods;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class CompanyCustomerController : Controller
    {

        #region Constant
        EvolutionEntities _db = new EvolutionEntities();
        // OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        CompanyCustomerMethods _CompanyCustomerMethod = new CompanyCustomerMethods();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();


        #endregion

        #region View
        // GET: /CompanyCustomer/
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region Customer Method
        public ActionResult validateSSO(string ID)
        {
            bool employeeData = _CompanyCustomerMethod.validateSSo(ID);
            return Json(employeeData, JsonRequestBehavior.AllowGet);
        }
        public CompanyCustomerViewModel returnCustomerList()
        {
            CompanyCustomerViewModel model = new CompanyCustomerViewModel();
            //string FilePath = ConfigurationManager.AppSettings["CmpCustomerFilePath"].ToString();
            var listData = _CompanyCustomerMethod.getAllCompanyCustomerList();
            var titleId = _otherSettingMethod.getSystemListByName("Title List");
            var genderId = _otherSettingMethod.getSystemListByName("Gender List");
            var titleLists = _otherSettingMethod.getAllSystemValueListByNameId(titleId.Id);
            var genderLists = _otherSettingMethod.getAllSystemValueListByNameId(genderId.Id);
            foreach (var item in listData)
            {
                CompanyCustomerListViewModel tableModel = new CompanyCustomerListViewModel();
                tableModel.Id = item.Id;
                tableModel.Name = item.FirstName + " " + item.LastName;
                tableModel.Phone = item.WorkPhone;
                tableModel.Email = item.Email;
                tableModel.Mobile = item.Mobile;
                model.customerList.Add(tableModel);

            }
            foreach (var item in titleLists)
            {
                OtherSettingValueViewModel tableModel = new OtherSettingValueViewModel();
                tableModel.Id = item.Id;
                tableModel.SystemListID = item.SystemListID;
                tableModel.Value = item.Value;
                model.titleList.Add(tableModel);
            }
            foreach (var item in genderLists)
            {
                OtherSettingValueViewModel tableModel = new OtherSettingValueViewModel();
                tableModel.Id = item.Id;
                tableModel.SystemListID = item.SystemListID;
                tableModel.Value = item.Value;
                model.genderList.Add(tableModel);
            }
            model.Dob = String.Format("{0:dd-MM-yyy}", DateTime.Now);
            return model;

        }
        public ActionResult customerList()
        {
            CompanyCustomerViewModel model = returnCustomerList();
            return PartialView("_partialCompanyCustomerList", model);
        }

        public ActionResult AddEditCustomer(int Id) 
        {
            string FilePath = ConfigurationManager.AppSettings["CmpCustomerFilePath"].ToString();
            CompanyCustomerViewModel model = new CompanyCustomerViewModel();
            model.Id = Id;
            var titleId = _otherSettingMethod.getSystemListByName("Title List");
            var genderId = _otherSettingMethod.getSystemListByName("Gender List");
            var titleLists = _otherSettingMethod.getAllSystemValueListByNameId(titleId.Id);
            var genderLists = _otherSettingMethod.getAllSystemValueListByNameId(genderId.Id);

            if (Id > 0)
            {
                var data = _CompanyCustomerMethod.getCompanyCustomerListById(Id);
                var title = _otherSettingMethod.getSystemListValueById(data.Title);
                var gender = _otherSettingMethod.getSystemListValueById(data.Gender);
                model.Id = data.Id;
                model.PhotoPath = data.PhotoPath;
                model.Title = data.Title;
                model.TitleValue = title.Value;
                model.FirstName = data.FirstName;
                model.LastName = data.LastName;
                model.Email = data.Email;
                model.Gender = data.Gender;
                model.GenderValue = gender.Value;
                model.DateOfBirth = data.DateOfBirth;
                model.Dob = String.Format("{0:dd-MM-yyy}", data.DateOfBirth);
                model.PostalCode = data.PostalCode;
                model.Address = data.Address;
                model.WorkPhone = data.WorkPhone;
                model.Mobile = data.Mobile;
            }
            else 
            {
                foreach (var item in titleLists)
                {
                    OtherSettingValueViewModel tableModel = new OtherSettingValueViewModel();
                    tableModel.Id = item.Id;
                    tableModel.SystemListID = item.SystemListID;
                    tableModel.Value = item.Value;
                    model.titleList.Add(tableModel);
                }
                foreach (var item in genderLists)
                {
                    OtherSettingValueViewModel tableModel = new OtherSettingValueViewModel();
                    tableModel.Id = item.Id;
                    tableModel.SystemListID = item.SystemListID;
                    tableModel.Value = item.Value;
                    model.genderList.Add(tableModel);
                }
                model.Dob = String.Format("{0:dd-MM-yyy}", DateTime.Now);
            
            }
            return PartialView("_partialAddCompayCustomer", model);
        
        }
        public ActionResult SaveCustomer(int title, string firstName, string lastname, string email, int gender, string dateofbirth, string postalcode, string address, string workphone, string mobile, int Id)
        {
            string FilePath = string.Empty;
            string fileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                FilePath = ConfigurationManager.AppSettings["CmpCustomerFilePath"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }
            DateTime date = Convert.ToDateTime(dateofbirth);
            var data = _CompanyCustomerMethod.SaveCompanyCustomerData(fileName, title, firstName, lastname, email, gender, date, postalcode, address, workphone, mobile, Id, SessionProxy.UserId);
            if (!data)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                CompanyCustomerViewModel model = returnCustomerList();
                return PartialView("_partialCompanyCustomerList", model);
            }
            
        }

        public ActionResult EditCustomer(int Id)
        {
            CompanyCustomerViewModel model = new CompanyCustomerViewModel();
            var titleId = _otherSettingMethod.getSystemListByName("Title List");
            var genderId = _otherSettingMethod.getSystemListByName("Gender List");
            var titleLists = _otherSettingMethod.getAllSystemValueListByNameId(titleId.Id);
            var genderLists = _otherSettingMethod.getAllSystemValueListByNameId(genderId.Id);

            var data = _CompanyCustomerMethod.getCompanyCustomerListById(Id);
            var title = _otherSettingMethod.getSystemListValueById(data.Title);
            var gender = _otherSettingMethod.getSystemListValueById(data.Gender);

            model.Id = data.Id;
            model.PhotoPath=data.PhotoPath;
            model.Title = data.Title;
            model.TitleValue = title.Value;
            model.FirstName = data.FirstName;
            model.LastName = data.LastName;
            model.Email = data.Email;
            model.Gender = data.Gender;
            model.GenderValue = gender.Value;
            model.DateOfBirth = data.DateOfBirth;
            model.Dob = String.Format("{0:dd-MM-yyy}", data.DateOfBirth);
            model.PostalCode = data.PostalCode;
            model.Address = data.Address;
            model.WorkPhone = data.WorkPhone;
            model.Mobile = data.Mobile;

            foreach (var item in titleLists)
            {
                OtherSettingValueViewModel tableModel = new OtherSettingValueViewModel();
                tableModel.Id = item.Id;
                tableModel.SystemListID = item.SystemListID;
                tableModel.Value = item.Value;
                model.titleList.Add(tableModel);
            }
            foreach (var item in genderLists)
            {
                OtherSettingValueViewModel tableModel = new OtherSettingValueViewModel();
                tableModel.Id = item.Id;
                tableModel.SystemListID = item.SystemListID;
                tableModel.Value = item.Value;
                model.genderList.Add(tableModel);
            }

            if (data == null)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return PartialView("_partialAddCompayCustomer", model);
            }
           
        }

        public ActionResult DeleteCustomer(int Id) 
        {
            var data = _CompanyCustomerMethod.deleteCompanyCustomer(Id, SessionProxy.UserId);

            if (!data)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                CompanyCustomerViewModel model = returnCustomerList();
                return PartialView("_partialCompanyCustomerList", model);
            }
        }
      

        #endregion

        #region New Customer Company Method


        public List<CustomerCompanyListViewModel> returnCustomerComapnayList() 
        {
            List<CustomerCompanyListViewModel> Model = new List<CustomerCompanyListViewModel>();
            var Data = _CompanyCustomerMethod.GetAllCustomerCompanyList();
            if (Data.Count > 0) 
            {
                foreach(var item in Data)
                {
                    CustomerCompanyListViewModel mm=new CustomerCompanyListViewModel();
                    var count = _CompanyCustomerMethod.GetAllCustomerByCustomerCompanyID(item.Id);
                    //var currency = _otherSettingMethod.getSystemListValueById((int)item.Currency);
                    int curruncyId = Convert.ToInt32(item.Currency);
                    var currency = _db.Currencies.Where(x=>x.Id== curruncyId && x.Archived==false).FirstOrDefault();
                    if (currency != null)
                    {
                        mm.Id = item.Id;
                        mm.CompanyName = item.CompanyName;
                        mm.ParentCompany = item.ParentCompany;
                        //mm.CurrencyName = currency.Value;
                        mm.CurrencyName = currency.Name;
                        mm.CustomerCount = count;
                    }
                    Model.Add(mm);
                }

            }
            return Model;
        }

        public ActionResult CustomerComapnyList()
        {
            List<CustomerCompanyListViewModel> model = returnCustomerComapnayList();
            return PartialView("_partialCustomerCompanyList", model);
        }

        [HttpPost]
        public ActionResult SaveCustomerCompany(CustomerCompanyViewModel Model)
        {
            var data = _CompanyCustomerMethod.SaveCustomerCompanyData(Model, SessionProxy.UserId);
            if (!data)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<CustomerCompanyListViewModel> model = returnCustomerComapnayList();
                return PartialView("_partialCustomerCompanyList", model);
            }

        }

        public ActionResult AddEditCustomerCompany(int Id)
        {
            CustomerCompanyViewModel model = new CustomerCompanyViewModel();
            model.Id = Id;
            var curr = _db.Currencies.ToList();
            //foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Company Setting Currencies"))
            //{
            //    model.CurrencyList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            //}
            foreach(var item in curr)
            {
                model.CurrencyList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            if (model.Id > 0)
            {
                var details = _CompanyCustomerMethod.GetCustomerCompanyDetailsById(Id);
                model.Id = details.Id;
                model.CustomerNumber = details.Id;
                model.AccountName = details.AccountName;
                model.AccountNumber = details.AccountNumber;
                model.BankAddress = details.BankAddress;
                model.BankName = details.BankName;
                model.BankSortCode = details.BankSortCode;
                model.BusinessAddress = details.BusinessAddress;
                model.OriginalCompanyLogo = details.OriginalCompanyLogo;
                model.CompanyLogo = details.CompanyLogo;
                model.CompanyName = details.CompanyName;
                model.CreditLimit = details.CreditLimit;
                model.CreditStatus = details.CreditStatus;
                model.CurrencyID = (int)details.Currency;
                model.Email = details.Email;
                model.GeneralNotes = details.GeneralNotes;
                model.IBAN = details.IBAN;
                model.MailingAddress = details.MailingAddress;
                model.ParentCompany = details.ParentCompany;
                model.PaymentTerms = details.PaymentTerms;
                model.PhoneNumber = details.PhoneNumber;
                model.SalesRegions = details.SalesRegions;
                model.SecondaryPhoneNumber = details.SecondaryPhoneNumber;
                model.ShortName = details.ShortName;
                model.TaxGroup = details.TaxGroup;
                model.VAT_GST_Number = details.VAT_GST_Number;
                model.Website = details.Website;
                return PartialView("_partialEditCustomerCompany", model);

            }
            else 
            {
                int count = (from row in _db.Company_Customer select row).Count();
                model.CustomerNumber = count + 1;
                return PartialView("_partialAddEditCustomerCompanyView", model);
            }
            
        }

        [HttpPost]
        public ActionResult ImageData()
        {
            string FilePath = string.Empty;
            string fileName = string.Empty;
            string originalFileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                FilePath = ConfigurationManager.AppSettings["CmpCustomerFilePath"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                originalFileName = hpf.FileName;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }

            return Json(new { originalFileName = originalFileName, NewFileName = fileName });
        }


        public ActionResult DeleteCustomerCompany(int Id)
        {
            var data = _CompanyCustomerMethod.DeleteCustomerCompany(Id, SessionProxy.UserId);

            if (!data)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<CustomerCompanyListViewModel> model = returnCustomerComapnayList();
                return PartialView("_partialCustomerCompanyList", model);
            }
        }

       

        #endregion
    }
}