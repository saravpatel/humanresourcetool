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
    public class TechnicalSkillsSetController : Controller
    {

        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        TechnicalSkillsSetMethod _technicalSkillsSetMethod = new TechnicalSkillsSetMethod();

        #endregion

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public List<TechnicalSkillsSetViewModel> returnList()
        {
            List<TechnicalSkillsSetViewModel> model = new List<TechnicalSkillsSetViewModel>();
            List<SkillSet> data = _technicalSkillsSetMethod.getAllList();
            string FilePath = ConfigurationManager.AppSettings["SkillSetFilePath"].ToString();

            foreach (var item in data)
            {
                TechnicalSkillsSetViewModel technicalSkillsSetViewModel = new TechnicalSkillsSetViewModel();
                technicalSkillsSetViewModel.Id = item.Id;
                technicalSkillsSetViewModel.Name = item.Name;
                technicalSkillsSetViewModel.Description = item.Description;
                if (!string.IsNullOrEmpty(item.Picture))
                {
                    technicalSkillsSetViewModel.Picture = item.Picture;
                }
                model.Add(technicalSkillsSetViewModel);
            }
            return model;
        }

        public ActionResult List()
        {
            List<TechnicalSkillsSetViewModel> model = returnList();
            return PartialView("_partialTechnicalSkillsSetList", model);
        }

        public ActionResult AddEditTechnicalSkillsSet(int Id)
        {
            string FilePath = ConfigurationManager.AppSettings["SkillSetFilePath"].ToString();
            TechnicalSkillsSetViewModel model = new TechnicalSkillsSetViewModel();
            model.Id = Id;
            var technicalSkillList = _otherSettingMethod.getAllSystemValueListByKeyName("Technical Skills");
            foreach (var item in technicalSkillList)
            {
                model.SkillValueList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            if (Id > 0)
            {
                var SkillSets = _technicalSkillsSetMethod.getSkillSetById(Id);
                model.Name = SkillSets.Name;
                model.Archived = (bool)SkillSets.Archived;
                model.Description = SkillSets.Description;
                model.TechnicalSkillsCSV = SkillSets.TechnicalSkillsCSV;
                if (SkillSets.TechnicalSkillsCSV.IndexOf(',') > 0)
                {
                    model.selectedValues = SkillSets.TechnicalSkillsCSV.Split(',').ToList();
                }
                else
                {
                    if (!string.IsNullOrEmpty(SkillSets.TechnicalSkillsCSV))
                    {
                        string record = SkillSets.TechnicalSkillsCSV;
                        model.selectedValues.Add(record);
                    }
                }
                if (!string.IsNullOrEmpty(SkillSets.Picture))
                {
                    model.Picture = SkillSets.Picture;
                }
            }
            return PartialView("_partialAddTechnicalSkillsSet", model);
        }

        public ActionResult SavData(int Id, string skillName, string Description, string SkillValueIds)
        {
            string FilePath = string.Empty;
            string fileName = string.Empty;
            if (Request.Files.Count > 0)
            {
                FilePath = ConfigurationManager.AppSettings["SkillSetFilePath"].ToString();
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                hpf.SaveAs(path);
            }
            _technicalSkillsSetMethod.SaveSkillsSet(Id, skillName, Description, SkillValueIds, fileName, SessionProxy.UserId);
            List<TechnicalSkillsSetViewModel> model = returnList();
            return PartialView("_partialTechnicalSkillsSetList", model);
        }
    }
}