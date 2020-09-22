using HRTool.CommanMethods.Resources;
using HRTool.DataModel;
using Microsoft.AspNet.Identity;
using HRTool.Models.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRTool.CommanMethods.Settings;
using System.Web.Script.Serialization;
using System.Configuration;
using System.IO;
using HRTool.CommanMethods;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class EmployeeTrainingController : Controller
    {

        #region const
        EmployeeTrainingMethod _EmployeeTrainingMethod = new EmployeeTrainingMethod();
        EmployeeMethod _employeeMethod = new EmployeeMethod();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        #endregion

        //
        // GET: /EmployeeTraining/
        public ActionResult Index(int EmployeeId)
        {
            EmployeeTrainingViewModel model = new EmployeeTrainingViewModel();
            model.CurrentUserId = EmployeeId;
            return View(model);
        }
        public List<EmployeeTrainingViewModel> returnList(int Id)
        {
            EmployeeTrainingViewModel models = new EmployeeTrainingViewModel();
             //List<EmployeeTraining> datass = _EmployeeTrainingMethod.getAllList().ToList();
            List<EmployeeTraining> data = _EmployeeTrainingMethod.getAllListByEmployee(Id).ToList();
            List<EmployeeTrainingViewModel> model = new List<EmployeeTrainingViewModel>();
            foreach (var item in data)
            {
                var employeeName = _employeeMethod.getEmployeeById(item.EmployeeId);
                string Name = string.Format("{0} {1}", employeeName.FirstName, employeeName.LastName);
                EmployeeTrainingViewModel training = new EmployeeTrainingViewModel();
                training.Id = item.Id;
                training.EmployeeName = Name;
                if (item.TrainingNameId != null)
                {
                    training.TrainingName = _EmployeeTrainingMethod.getTrainingById(Convert.ToInt32(item.TrainingNameId));
                }
                if (item.Id != null)
                {
                    training.ImportanceName = _EmployeeTrainingMethod.GetImportnceNameById(Convert.ToInt32(item.Id));
                }
                if (item.Status != 0)
                {
                    training.StatusName = _EmployeeTrainingMethod.getTrainingStatusById(Convert.ToInt32(item.Status));
                }

                var TrainingStatus = _otherSettingMethod.getAllSystemValueListByKeyName("Traning Status List");

                //foreach (var items in TrainingStatus)
                //{

                //    training.StatusName += items.Value;
                //}
                training.Progress = item.Progress;
                training.StartDate = String.Format("{0:dd-MMM-yyy}", item.StartDate);
                training.EndDate = String.Format("{0:dd-MMM-yyy}", item.EndDate);
                training.ExpiryDate = String.Format("{0:dd-MMM-yyy}", item.ExpiryDate);
                model.Add(training);
            }
            return model;
        }
        public ActionResult List(int Id)
        {
            List<EmployeeTrainingViewModel> model = returnList(Id);
            return PartialView("_PartialTrainingList", model);
        }
        public ActionResult SavData(EmployeeTrainingViewModel model)
        {
            int userId = SessionProxy.UserId;
            model.CurrentUserId = userId;
            JavaScriptSerializer js = new JavaScriptSerializer();
            model.ListDocument = js.Deserialize<List<AdminTraningDocumentViewModel>>(model.TraingDocumentList);
            _EmployeeTrainingMethod.SaveTrainingdSet(model, userId);
            List<EmployeeTrainingViewModel> modelList = returnList(model.EmployeeId);
            return PartialView("_PartialTrainingList", modelList);
        }
        public ActionResult AddEditTrainingSet(int Id, int EmployeeId)
        {
            EmployeeTrainingViewModel model = new EmployeeTrainingViewModel();
            model.Id = Id;
            if (Id > 0)
            {
                var hr_Training = _EmployeeTrainingMethod.GetTrainingListById(Id);
                model.BindEmployeeList = _EmployeeTrainingMethod.GetEmployeeList(hr_Training.EmployeeId);
                var Training = _otherSettingMethod.getAllSystemValueListByKeyName("Training List");
                foreach (var item in Training)
                {
                    if (item.Id == hr_Training.TrainingNameId)
                    {
                        model.BindTrainingList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        model.BindTrainingList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                    }
                }
                var TrainingStatus = _otherSettingMethod.getAllSystemValueListByKeyName("Traning Status List");
                foreach (var item in TrainingStatus)
                {
                    if (item.Id == hr_Training.Status)
                    {
                        model.BindTrainingStatusList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        model.BindTrainingStatusList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                    }
                }

                model.Description = hr_Training.Description;
                model.Importance = hr_Training.Importance;
                model.Progress = hr_Training.Progress;
                model.StartDate = String.Format("{0:dd-MM-yyy}", hr_Training.StartDate);
                model.EndDate = String.Format("{0:dd-MM-yyy}", hr_Training.EndDate);
                model.ExpiryDate = String.Format("{0:dd-MM-yyy}", hr_Training.ExpiryDate);
                model.Provider = hr_Training.Provider;
                model.Cost = hr_Training.Cost;
                model.Notes = hr_Training.Notes;
                var TraingDoument = _EmployeeTrainingMethod.getTrainigDocumentByCaseId(Id);
                foreach (var item in TraingDoument)
                {
                    AdminTraningDocumentViewModel docModel = new AdminTraningDocumentViewModel();
                    docModel.Id = item.Id;
                    docModel.originalName = item.OriginalName;
                    docModel.newName = item.NewName;
                    docModel.description = item.Description;
                    model.ListDocument.Add(docModel);
                }
            }
            else
            {
                model.BindEmployeeList = _EmployeeTrainingMethod.GetEmployeeList(EmployeeId);
                var hr_Training = _EmployeeTrainingMethod.GetStatusNameById(EmployeeId);
                var Training = _otherSettingMethod.getAllSystemValueListByKeyName("Training List");
                model.BindTrainingList.Add(new SelectListItem() { Text = "-- Select Training --", Value = "0" });

                foreach (var item in Training)
                {
                    model.BindTrainingList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                }
                var TrainingStatus = _otherSettingMethod.getAllSystemValueListByKeyName("Traning Status List");
                foreach (var item in TrainingStatus)
                {
                    //if (item.Id == Convert.ToInt32(hr_Training.Status))
                    //{
                        model.BindTrainingStatusList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                    //}
                }
            }
            return PartialView("_PartialTrainingAddSet", model);

        }
        public ActionResult DeleteTrainingRecord(int Id,int EmpId)
        {
            int userId = SessionProxy.UserId;
            _EmployeeTrainingMethod.DeleteTraining(Id, userId);
            List<EmployeeTrainingViewModel> modelList = returnList(EmpId);
            return PartialView("_PartialTrainingList", modelList);
    
        }
        public ActionResult FiledTypeList()
        {
            FiledViewModel model = new FiledViewModel();
            var AssetsTypeList = _otherSettingMethod.getAllSystemValueListByKeyName("Field Type List");
            foreach (var item in AssetsTypeList)
            {
                model.BindFiledTypeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            return PartialView("_PartialFiledType", model);
        }

        [HttpPost]
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
    }
}