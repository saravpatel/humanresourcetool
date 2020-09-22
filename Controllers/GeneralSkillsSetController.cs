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
    public class GeneralSkillsSetController : Controller
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        GeneralSkillsSetMethod _generalSkillsSetMethod = new GeneralSkillsSetMethod();

        #endregion

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public List<GeneralSkillsSetViewModel> returnList()
        {
            List<GeneralSkillsSetViewModel> model = new List<GeneralSkillsSetViewModel>();
            List<SkillSet> data = _generalSkillsSetMethod.getAllList();

            foreach (var item in data)
            {
                GeneralSkillsSetViewModel generalSkillsSetViewModel = new GeneralSkillsSetViewModel();
                generalSkillsSetViewModel.Id = item.Id;
                generalSkillsSetViewModel.Name = item.Name;
                generalSkillsSetViewModel.Description = item.Description;
                if (!string.IsNullOrEmpty(item.Picture))
                {
                    generalSkillsSetViewModel.Picture = item.Picture;
                }
                model.Add(generalSkillsSetViewModel);
            }
            return model;
        }

        public ActionResult List()
        {
            List<GeneralSkillsSetViewModel> model = returnList();
            return PartialView("_partialGeneralSkillsSetList", model);
        }

        public ActionResult AddEditGeneralSkillsSet(int Id)
        {
            string FilePath = ConfigurationManager.AppSettings["SkillSetFilePath"].ToString();
            GeneralSkillsSetViewModel model = new GeneralSkillsSetViewModel();
            model.Id = Id;
            var technicalSkillList = _otherSettingMethod.getAllSystemValueListByKeyName("General Skills");
            foreach (var item in technicalSkillList)
            {
                model.SkillValueList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            if (Id > 0)
            {
                var SkillSets = _generalSkillsSetMethod.getSkillSetById(Id);
                model.Name = SkillSets.Name;
                model.Archived = (bool)SkillSets.Archived;
                model.Description = SkillSets.Description;
                model.GeneralSkillsCSV = SkillSets.GeneralSkillsCSV;
                if (SkillSets.GeneralSkillsCSV.IndexOf(',') > 0)
                {
                    model.selectedValues = SkillSets.GeneralSkillsCSV.Split(',').ToList();
                }
                else
                {
                    if (!string.IsNullOrEmpty(SkillSets.GeneralSkillsCSV))
                    {
                        string record = SkillSets.GeneralSkillsCSV;
                        model.selectedValues.Add(record);
                    }
                }
                if (!string.IsNullOrEmpty(SkillSets.Picture))
                {
                    model.Picture = SkillSets.Picture;
                }
            }
            return PartialView("_partialAddGeneralSkillsSet", model);
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
            _generalSkillsSetMethod.SaveSkillsSet(Id, skillName, Description, SkillValueIds, fileName, SessionProxy.UserId);
            List<GeneralSkillsSetViewModel> model = returnList();
            return PartialView("_partialGeneralSkillsSetList", model);
        }
    }
}