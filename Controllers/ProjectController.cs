using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HRTool.CommanMethods;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class ProjectController : Controller
    {
        #region Const
        ProjectSettindsMethod _ProjectMethod = new ProjectSettindsMethod();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        AddAssetsMethod AddAssets = new AddAssetsMethod();

        #endregion
        //
        // GET: /Project/
        public ActionResult Index()
        {
            return View();
        }
        public List<ProjectViewModel> returnList()
        {
            List<Project> data = _ProjectMethod.getAllList().Where(x=>x.Archived==false).ToList();
            List<ProjectViewModel> model = new List<ProjectViewModel>();
            foreach (var item in data)
            {
                ProjectViewModel Project = new ProjectViewModel();
                Project.Id = item.Id;
                Project.Name = item.Name;
                Project.CountryName = _ProjectMethod.GetCountryById(Convert.ToInt32(item.Country)).Name;
                Project.LocationName = _ProjectMethod.LocationId(Convert.ToInt32(item.Location));
                Project.ProjectOwner = item.ProjectOwner;
                model.Add(Project);
            }
            return model;
        }
        public ActionResult List()
        {
            List<ProjectViewModel> model = returnList();
            return PartialView("_PartialProjectList", model);
        }
        public ActionResult SavData(ProjectViewModel model)
        {
            int userId = SessionProxy.UserId;
            model.CurrentUserId = userId;
            _ProjectMethod.SaveProjectSet(model);
            List<ProjectViewModel> modelList = returnList();
            return PartialView("_PartialProjectList", modelList);
        }
        public ActionResult AddEditProjectSet(int Id)
        {
            string FilePath = ConfigurationManager.AppSettings["AssetsFilePath"].ToString();
            ProjectViewModel model = new ProjectViewModel();
            model.Id = Id;
            if (Id > 0)
            {
                var hr_project = _ProjectMethod.GetProjectListById(Id);
                model.TechnicalSkillsCSV = hr_project.TechnicalSkillsCSV;
                if (hr_project.TechnicalSkillsCSV.IndexOf(',') > 0)
                {
                    model.selectedValuesTechnical = hr_project.TechnicalSkillsCSV.Split(',').ToList();
                }
                else
                {
                    if (!string.IsNullOrEmpty(hr_project.TechnicalSkillsCSV))
                    {
                        string record = hr_project.TechnicalSkillsCSV;
                        model.selectedValuesTechnical.Add(record);
                    }

                }

                model.GeneralSkillsCSV = hr_project.GeneralSkillsCSV;
                if (hr_project.GeneralSkillsCSV.IndexOf(',') > 0)
                {
                    model.selectedValuesGeneral = hr_project.GeneralSkillsCSV.Split(',').ToList();
                }
                else
                {
                    if (!string.IsNullOrEmpty(hr_project.GeneralSkillsCSV))
                    {
                        string record = hr_project.GeneralSkillsCSV;
                        model.selectedValuesGeneral.Add(record);
                    }

                }
                model.CustomersCSV = hr_project.CustomersCSV;
                if (hr_project.CustomersCSV != null)
                {
                    if (hr_project.CustomersCSV.IndexOf(',') > 0)
                    {
                        model.selectedValuesCoustmer = hr_project.CustomersCSV.Split(',').ToList();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(hr_project.CustomersCSV))
                        {
                            string recordCustomer = hr_project.CustomersCSV;
                            model.selectedValuesCoustmer.Add(recordCustomer);
                        }
                    }
                }

                var AssetsType_1 = _otherSettingMethod.getAllSystemValueListByKeyName("Asset Type List");
                foreach (var item in AssetsType_1)
                {
                    if (item.Id == hr_project.AssetType)
                    {
                        model.AssetsTypeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        model.AssetsTypeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                    }
                }
                var Block = _otherSettingMethod.getAllSystemValueListByKeyName("Block List");
                foreach (var item in Block)
                {
                    if (item.Id == hr_project.Block)
                    {
                        model.BlockList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        model.BlockList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                    }
                }

                var Location = _otherSettingMethod.getAllSystemValueListByKeyName("Location List");
                foreach (var item in Location)
                {
                    if (item.Id == hr_project.Location)
                    {
                        model.LocationList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        model.LocationList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                    }
                }
                var TaxZone = _otherSettingMethod.getAllSystemValueListByKeyName("Tax Zone List");
                foreach (var item in TaxZone)
                {
                    if (item.Id == hr_project.TaxZone)
                    {
                        model.TaxZoneList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        model.TaxZoneList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                    }
                }
                var Country_List = _ProjectMethod.GeCountryList();
                foreach (var item in Country_List)
                {
                    if (Convert.ToInt32(item.Value) == hr_project.Country)
                    {
                        model.CountryList.Add(new SelectListItem() { Text = @item.Text, Value = @item.Value.ToString(), Selected = true });
                    }
                    else
                    {
                        model.CountryList.Add(new SelectListItem() { Text = @item.Text, Value = @item.Value.ToString() });
                    }
                }
                model.Name = hr_project.Name;
                model.FromDate = String.Format("{0:dd-MM-yyy}", hr_project.FromDate);
                model.ToDate = String.Format("{0:dd-MM-yyy}", hr_project.ToDate);
                model.OperatorCompany = hr_project.OperatorCompany;
                model.ProjectOwner = hr_project.ProjectOwner;
                model.Description = hr_project.Description;

            }

            var AssetsTypeList = _otherSettingMethod.getAllSystemValueListByKeyName("Asset Type List");
            foreach (var item in AssetsTypeList)
            {
                model.AssetsTypeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }

            var Blocklist = _otherSettingMethod.getAllSystemValueListByKeyName("Block List");
            foreach (var item in Blocklist)
            {
                model.BlockList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var LocationList = _otherSettingMethod.getAllSystemValueListByKeyName("Office Locations");
            foreach (var item in LocationList)
            {
                model.LocationList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var GeneralSkills = _otherSettingMethod.getAllSystemValueListByKeyName("General Skills");
            foreach (var item in GeneralSkills)
            {
                model.GeneralSkillsList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var TechanicalSkills = _otherSettingMethod.getAllSystemValueListByKeyName("Technical Skills");
            foreach (var item in TechanicalSkills)
            {
                model.TechnicalSkillsList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var TaxZoneList = _otherSettingMethod.getAllSystemValueListByKeyName("Tax Zone List");
            foreach (var item in TaxZoneList)
            {
                model.TaxZoneList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var AssetsOwnerList = AddAssets.BindAssetsOwnerList();
            foreach (var item in AssetsOwnerList)
            {
                model.CustomersList.Add(new SelectListItem() { Text = @item.Text, Value = @item.Value.ToString() });
            }
            var Country_ListRecord = _ProjectMethod.GeCountryList();
            foreach (var item in Country_ListRecord)
            {
                model.CountryList.Add(new SelectListItem() { Text = @item.Text, Value = @item.Value.ToString() });
            }
            return PartialView("_PartialAddProject", model);
        }
        public ActionResult DeleteProjectRecord(int Id)
        {
            _ProjectMethod.DeleteProject(Id);
            List<ProjectViewModel> modelList = returnList();
            return PartialView("_PartialProjectList", modelList);
        }
    }
}