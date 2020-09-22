using HRTool.CommanMethods.Resources;
using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models.Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using HRTool.CommanMethods;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class EmployeeBenefitsController : Controller
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();

        EmployeeBenefitsMethod _employeeBenefitsMethod = new EmployeeBenefitsMethod();

        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();

        #endregion

        #region View
        // <add key="CaseLogDocuments" value="~/Upload/Documents/CaseLog/" />
        // GET: /EmpolyeeCases/
        public ActionResult Index(string EmployeeId)
        {
            BenefitsEmployeeViewModel model = new BenefitsEmployeeViewModel();
            model.EmployeeId = EmployeeId;
            return View(model);
        }


        #endregion

        #region Cases Method

        public List<BenefitsViewModel> returnBenefitsList(int EmployeeId)
        {
            List<BenefitsViewModel> model = new List<BenefitsViewModel>();
            var listData = _employeeBenefitsMethod.getBenifitByEmployeeId(EmployeeId);

            foreach (var item in listData)
            {
                BenefitsViewModel m = new BenefitsViewModel();
                m.Id = item.Id;
                m.BenefitValue = _otherSettingMethod.getSystemListValueById(item.BenefitID).Value;
                m.DateAwarded = String.Format("{0:dd-MMM-yyy}", item.DateAwarded);
                m.ExpiryDate = String.Format("{0:dd-MMM-yyy}", item.ExpiryDate);
                m.FixedAmount = (decimal)item.FixedAmount;
                model.Add(m);
            }

            return model;
        }
        public ActionResult List(int EmployeeId)
        {
            List<BenefitsViewModel> model = returnBenefitsList(EmployeeId);
            return PartialView("_partialEmployeeBenifitsList", model);
        }

        public ActionResult AddEditBenifits(int Id)
        {
            BenefitsViewModel model = new BenefitsViewModel();
            model.Id = Id;

            model.BenefitList.Add(new SelectListItem() { Text = "-- Select Benefit --", Value = "0" });
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Benefit List"))
            {
                model.BenefitList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Company Setting Currencies"))
            {
             model.CurrencyList.Add(new SelectListItem() { Text = item.Value, Value = item.Id.ToString() });
            }
            if (model.Id > 0)
            {
                var benifitDetail = _employeeBenefitsMethod.getBrnifitById(Id);
                model.BenefitID = benifitDetail.BenefitID;
                model.Currency = (int)benifitDetail.Currency;
                model.DateAwarded = String.Format("{0:dd-MM-yyy}", benifitDetail.DateAwarded);
                model.ExpiryDate = String.Format("{0:dd-MM-yyy}", benifitDetail.ExpiryDate);
                model.FixedAmount = (decimal)benifitDetail.FixedAmount;
                model.RecoverOnTermination = benifitDetail.RecoverOnTermination;
                model.Comments = benifitDetail.Comments;

                var caseDoument = _employeeBenefitsMethod.getBenifitDocumentByCaseId(Id);
                foreach (var item in caseDoument)
                {
                    BenefitsDocumentViewModel docModel = new BenefitsDocumentViewModel();
                    docModel.Id = item.Id;
                    docModel.originalName = item.OriginalName;
                    docModel.newName = item.NewName;
                    model.BenefitDocumentList.Add(docModel);
                }
            }

            return PartialView("_partialAddBenifits", model);
        }

        [HttpPost]
        public ActionResult ImageData()
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

        public ActionResult DeleteData(int Id,int EmployeeId)
        {
            _employeeBenefitsMethod.DeleteData(Id, SessionProxy.UserId);
            List<BenefitsViewModel> returnModel = returnBenefitsList(EmployeeId);
            return PartialView("_partialEmployeeBenifitsList", returnModel);
        }

        public ActionResult SaveData(BenefitsViewModel model)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<BenefitsDocumentViewModel> listDocument = js.Deserialize<List<BenefitsDocumentViewModel>>(model.DocumentListString);

            _employeeBenefitsMethod.SaveData(model, listDocument, SessionProxy.UserId);

            List<BenefitsViewModel> returnModel = returnBenefitsList(model.EmployeeID);
            return PartialView("_partialEmployeeBenifitsList", returnModel);
        }

        
        #endregion
    }
}