using HRTool.DataModel;
using HRTool.Models.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.CommanMethods.Resources
{
    public class EmployeeTrainingMethod
    {
        #region const
        EvolutionEntities _db = new EvolutionEntities();
        #endregion
        public IList<EmployeeTraining> getAllList()
        {
            return _db.EmployeeTrainings.Where(x=>x.Archived ==false).ToList();
        }
        public IList<EmployeeTraining> getAllListByEmployee(int EmpId)
        {
            return _db.EmployeeTrainings.Where(x=> x.EmployeeId== EmpId && x.Archived == false).ToList();
        }
        public void SaveTrainingdSet(EmployeeTrainingViewModel model,int userId)
        {
            if (model.Id > 0)
            {
                EmployeeTraining Training = _db.EmployeeTrainings.Where(x => x.Id == model.Id).FirstOrDefault();
                Training.EmployeeId = model.EmployeeId;
                Training.TrainingNameId = model.TrainingNameId;
                Training.Description = model.Description;
                Training.Importance = model.Importance;
                Training.Status = model.Status;
                Training.StartDate = DateTime.ParseExact(model.StartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture); ;
                Training.EndDate = DateTime.ParseExact(model.EndDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture); ;
                Training.ExpiryDate = DateTime.ParseExact(model.ExpiryDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                Training.Provider = model.Provider;
                Training.Progress = model.Progress;
                Training.Cost = model.Cost;
                Training.Notes = model.Notes;
                Training.UserIDCreatedBy = userId;
                Training.UserIDLastModifiedBy = userId;
                Training.LastModifiedDate = DateTime.Now;
                Training.Archived = false;
                Training.IsRead = false;
                Training.IsReadAddRep = false;
                Training.IsReadHR = false;
                Training.IsReadWorker = false;
                Training.CustomFieldsJSON = model.CustomFieldsJSON;
                _db.SaveChanges();

                foreach (var item in _db.Training_Document.Where(x => x.TrainingId == Training.Id).ToList())
                {
                    _db.Training_Document.Remove(item);
                    _db.SaveChanges();
                }
                foreach (var item in model.ListDocument)
                {
                    Training_Document TraningDocument = new Training_Document();
                    TraningDocument.TrainingId = Training.Id;
                    TraningDocument.NewName = item.newName;
                    TraningDocument.OriginalName = item.originalName;
                    TraningDocument.Description = item.description;
                    TraningDocument.Archived = false;
                    TraningDocument.UserIDCreatedBy = userId;
                    TraningDocument.CreatedDate = DateTime.Now;
                    TraningDocument.UserIDLastModifiedBy = userId;
                    TraningDocument.LastModified = DateTime.Now;
                    _db.Training_Document.Add(TraningDocument);
                    _db.SaveChanges();
                }

            }
            else
            {
                EmployeeTraining Training = new EmployeeTraining();
                Training.EmployeeId = model.EmployeeId;
                Training.TrainingNameId = model.TrainingNameId;
                Training.Description = model.Description;
                Training.Importance = model.Importance;
                Training.Status = model.Status;
                Training.StartDate = DateTime.ParseExact(model.StartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture); ;
                Training.EndDate = DateTime.ParseExact(model.EndDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture); ;
                Training.ExpiryDate = DateTime.ParseExact(model.ExpiryDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                Training.Provider = model.Provider;
                Training.Progress = model.Progress;
                Training.Cost = model.Cost;
                Training.Notes = model.Notes;
                Training.Archived = false;
                Training.UserIDCreatedBy = userId;
                Training.UserIDLastModifiedBy = userId;
                Training.CreatedDate = DateTime.Now;
                Training.CustomFieldsJSON = model.CustomFieldsJSON;
                _db.EmployeeTrainings.Add(Training);
                _db.SaveChanges();
                foreach (var item in model.ListDocument)
                {
                    Training_Document TrainingDocument = new Training_Document();
                    TrainingDocument.TrainingId = Training.Id;
                    TrainingDocument.NewName = item.newName;
                    TrainingDocument.OriginalName = item.originalName;
                    TrainingDocument.Description = item.description;
                    TrainingDocument.Archived = false;
                    TrainingDocument.UserIDCreatedBy = userId;
                    TrainingDocument.CreatedDate = DateTime.Now;
                    TrainingDocument.UserIDLastModifiedBy = userId;
                    TrainingDocument.LastModified = DateTime.Now;
                    _db.Training_Document.Add(TrainingDocument);
                    _db.SaveChanges();
                }
            }
        }
        public EmployeeTraining GetTrainingListById(int Id)
        {
            return _db.EmployeeTrainings.Where(x => x.Id == Id).FirstOrDefault();
        }

        public EmployeeTraining GetStatusNameById(int Id)
        {
            var status = _db.EmployeeTrainings.Where(x => x.EmployeeId == Id).FirstOrDefault();
            return status;
        }


        public bool DeleteTraining(int Id,int userId)
        {
            EmployeeTraining Training = _db.EmployeeTrainings.Where(x => x.Id == Id).FirstOrDefault();
            Training.Archived = true;
            Training.LastModifiedDate = DateTime.Now;
            Training.UserIDLastModifiedBy = userId;
            _db.SaveChanges();
            return true;

        }
        public AspNetUser GetEmployeeName(int Id)
        {
            return _db.AspNetUsers.Where(x => x.Id == Id).FirstOrDefault();
        }
        public List<SelectListItem> GetEmployeeList(int? EmployeeId)
        {
            List<SelectListItem> model = new List<SelectListItem>();
            var EmployeeList = (from i in _db.AspNetUsers
                                select i).ToList();
            foreach (var item in EmployeeList)
            {
                if (item.Id == EmployeeId)
                {
                    model.Add(new SelectListItem { Text = string.Format("{0} {1}", item.FirstName, item.LastName), Value = item.Id.ToString(), Selected = true });
                }
                //else
                //{
                //    model.Add(new SelectListItem { Text = string.Format("{0} {1}", item.FirstName, item.LastName), Value = item.Id.ToString() });
                //}
            }
            return model;
        }
        public List<SelectListItem> GetAllEmployeeList()
        {
            List<SelectListItem> model = new List<SelectListItem>();
            var EmployeeList = (from i in _db.AspNetUsers
                                select i).ToList();
            model.Add(new SelectListItem() { Text = "-- Select Employee --", Value = "0" });
            foreach (var item in EmployeeList)
            {
                model.Add(new SelectListItem { Text = string.Format("{0} {1} - {2}", item.FirstName, item.LastName,item.SSOID), Value = item.Id.ToString() });
            }
            return model;
        }

        //public List<SelectListItem> GetAllStatusList()
        //{
            
        //    List<SystemListValue> model = _db.SystemListValues.ToList();
        //    return model;
        //}
        public string getTrainingById(int TrainingId)
        {
            var TrainingName = _db.SystemListValues.Where(x => x.Id == TrainingId).FirstOrDefault();
            return TrainingName.Value;
        }
        public string getTrainingStatusById(int StatusId)
        {
            string StatusName="";
            if (StatusId != 0) {
                var TrainingStatusName = _db.SystemListValues.Where(x => x.Id == StatusId).FirstOrDefault();
                StatusName= TrainingStatusName.Value;
                return StatusName;
            }
            else
            {
                return StatusName;
            }
            
        }
        public String GetImportnceNameById(int Id)
        {
            string Imp_Name;
            var ImportnceName = _db.EmployeeTrainings.Where(x => x.Id == Id).FirstOrDefault();
            if (ImportnceName.Importance == 1)
            {
                Imp_Name = "Mandatory";
            }
            else
            {
                Imp_Name = "Optional";
            }
            return Imp_Name;
        }

        public List<Training_Document> getTrainigDocumentByCaseId(int Id)
        {
            return _db.Training_Document.Where(x => x.TrainingId == Id && x.Archived==false).ToList();
        }

    }
}