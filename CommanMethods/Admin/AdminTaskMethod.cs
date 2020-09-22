using HRTool.DataModel;
using HRTool.Models.Admin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace HRTool.CommanMethods.Admin
{
    public class AdminTaskMethod
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        private string inputFormat = "dd-MM-yyyy";
        private string outputFormat = "yyyy-MM-dd HH:mm:ss";

        #endregion

        public List<Task_List> getAllTask()
        {
            return _db.Task_List.Where(x => x.Archived == false).ToList();
        }

        public Task_List getTaskById(int Id)
        {
            return _db.Task_List.Where(x => x.Id == Id).FirstOrDefault();
        }

        public IList<Task_Documents> getTaskDocumentsByVisaId(int TaskId)
        {
            return _db.Task_Documents.Where(x => x.TaskId == TaskId).ToList();
        }

        public void SaveData(AdminTaskViewModel model, List<TaskDocumentViewModel> documentList, int userId)
        {

            if (model.Id > 0)
            {
                Task_List task = _db.Task_List.Where(x => x.Id == model.Id).FirstOrDefault();
                task.Title = model.Title;
                task.Category = (int)model.CategoryId;                
                task.AssignTo = model.AssignToId;
                task.InRelationTo = model.InRelationToId;
                var DyeDateToString = DateTime.ParseExact(model.DueDate, inputFormat, CultureInfo.InvariantCulture);
                task.DueDate = Convert.ToDateTime(DyeDateToString.ToString(outputFormat));
                task.Status = (int)model.StatusId;
                task.AlterBeforeDays = model.AlertBeforeDays;
                task.Description = model.Description;
                task.LastModifiedBy = userId;
                task.LastModified = DateTime.Now;
                _db.SaveChanges();


                foreach (var item in _db.Task_Documents.Where(x => x.TaskId == task.Id).ToList())
                {
                    _db.Task_Documents.Remove(item);
                    _db.SaveChanges();
                }
                foreach (var item in documentList)
                {
                    Task_Documents taskDocument = new Task_Documents();
                    taskDocument.TaskId = task.Id;
                    taskDocument.NewName = item.newName;
                    taskDocument.OriginalName = item.originalName;
                    taskDocument.Description = item.description;
                    taskDocument.Archived = false;
                    taskDocument.UserIDCreatedBy = userId;
                    taskDocument.CreatedDate = DateTime.Now;
                    taskDocument.UserIDLastModifiedBy = userId;
                    taskDocument.LastModified = DateTime.Now;
                    _db.Task_Documents.Add(taskDocument);
                    _db.SaveChanges();
                }
            }
            else
            {
                Task_List task = new Task_List();
                task.Title = model.Title;
                task.Category = (int)model.CategoryId;
                task.AssignTo = model.AssignToId;
                task.InRelationTo = model.InRelationToId;
                var DyeDateToString = DateTime.ParseExact(model.DueDate, inputFormat, CultureInfo.InvariantCulture);
                task.DueDate = Convert.ToDateTime(DyeDateToString.ToString(outputFormat));
                task.Status = (int)model.StatusId;
                task.AlterBeforeDays = model.AlertBeforeDays;
                task.Description = model.Description;
                task.Archived = false;
                task.CreatedBy = userId;
                task.Created = DateTime.Now;
                task.LastModifiedBy = userId;
                task.LastModified = DateTime.Now;
                _db.Task_List.Add(task);
                _db.SaveChanges();


                foreach (var item in documentList)
                {
                    Task_Documents taskDocument = new Task_Documents();
                    taskDocument.TaskId = task.Id;
                    taskDocument.NewName = item.newName;
                    taskDocument.OriginalName = item.originalName;
                    taskDocument.Description = item.description;
                    taskDocument.Archived = false;
                    taskDocument.UserIDCreatedBy = userId;
                    taskDocument.CreatedDate = DateTime.Now;
                    taskDocument.UserIDLastModifiedBy = userId;
                    taskDocument.LastModified = DateTime.Now;
                    _db.Task_Documents.Add(taskDocument);
                    _db.SaveChanges();
                }
            }

        }
    }
}