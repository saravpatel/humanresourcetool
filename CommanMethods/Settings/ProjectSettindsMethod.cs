using HRTool.DataModel;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.CommanMethods.Settings
{
    public class ProjectSettindsMethod
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();

        #endregion
        public IList<Cmp_Customer> getAllCustomer()
        {
            return _db.Cmp_Customer.ToList();
        }
        public IList<Project> getAllList()
        {
            return _db.Projects.ToList();
        }
        public int UpdateProJect(ProjectViewModel model)
        {
            Email_Setting ProjectUpdate = _db.Email_Setting.Where(x => x.Id == model.Id).FirstOrDefault();

            _db.SaveChanges();
            return ProjectUpdate.Id;
        }
        public int InsertProject(ProjectViewModel model)
        {
            Project ProjectAdd = new Project();

            _db.Projects.Add(ProjectAdd);
            _db.SaveChanges();
            return ProjectAdd.Id;
        }
        public List<SelectListItem> GeCountryList()
        {
            List<SelectListItem> model = new List<SelectListItem>();
            var CurrencyList = (from i in _db.Countries
                                select i).ToList();
            foreach (var item in CurrencyList)
            {
                model.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            return model;
        }
        public int GetBlockId(int Id)
        {
            var Block = _db.Projects.Where(x => x.Id == Id).FirstOrDefault();
            return (int)Block.Block;
        }
        public string LocationId(int Id)
        {
            var Location = _db.SystemListValues.Where(x => x.Id == Id).FirstOrDefault();
            return Location.Value;
        }
        public Project GetProjectListById(int Id)
        {
            return _db.Projects.Where(x => x.Id == Id).FirstOrDefault();
        }
        public void SaveProjectSet(ProjectViewModel model)
        {
            if (model.Id > 0)
            {
                Project _project = _db.Projects.Where(x => x.Id == model.Id).FirstOrDefault();
                _project.Name = model.Name;
                _project.Country = model.Country;
                _project.Location = model.Location;
                _project.Block = model.Block;
                _project.TaxZone = model.TaxZone;
                _project.AssetType = model.AssetType;
                _project.FromDate = DateTime.ParseExact(model.FromDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                _project.ToDate = DateTime.ParseExact(model.ToDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                _project.GeneralSkillsCSV = model.GeneralSkillsCSV;
                _project.TechnicalSkillsCSV = model.TechnicalSkillsCSV;
                _project.CustomersCSV = model.CustomersCSV;
                _project.OperatorCompany = model.OperatorCompany;
                _project.Description = model.Description;
                _project.ProjectOwner = model.ProjectOwner;
                _project.LastModified = DateTime.Now;
                _project.UserIDLastModifiedBy = model.CurrentUserId;
                _db.SaveChanges();

            }
            else
            {
                Project _project = new Project();
                _project.Name = model.Name;
                _project.Country = model.Country;
                _project.Location = model.Location;
                _project.Block = model.Block;
                _project.TaxZone = model.TaxZone;
                _project.AssetType = model.AssetType;
                _project.FromDate = DateTime.ParseExact(model.FromDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                _project.ToDate = DateTime.ParseExact(model.ToDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                _project.GeneralSkillsCSV = model.GeneralSkillsCSV;
                _project.TechnicalSkillsCSV = model.TechnicalSkillsCSV;
                _project.CustomersCSV = model.CustomersCSV;
                _project.OperatorCompany = model.OperatorCompany;
                _project.Description = model.Description;
                _project.ProjectOwner = model.ProjectOwner;
                _project.CreatedDate = DateTime.Now;
                _project.UserIDCreatedBy = model.CurrentUserId;
                _db.Projects.Add(_project);
                _db.SaveChanges();
            }
        }
        public Country GetCountryById(int Id)
        {
            var countryDetail = (from i in _db.Countries
                                 where i.Id == Id
                                 select i).FirstOrDefault();
            return countryDetail;
        }
        public void DeleteProject(int Id)
        {
            Project Project = _db.Projects.Where(x => x.Id == Id).FirstOrDefault();
            Project.Archived = true;
            Project.LastModified = DateTime.Now;
            Project.UserIDLastModifiedBy = SessionProxy.UserId;           
            //_db.Projects.Remove(Project);
            _db.SaveChanges();
        }
    }
}