using HRTool.CommanMethods.Email;
using HRTool.DataModel;
using HRTool.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.CommanMethods.Admin
{
    public class AdminNewsMethod
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();

        #endregion
        public IList<News> getActiveList()
        {
            return _db.News.Where(x => x.Archived == false).ToList();
        }

      //  public IList<News> getActiveListbyUserId(string UserId) 
       // {
            
       // }

        public void saveData(AdminNewsViewModel model, int userId)
        {
            if (model.Id > 0)
            {
                News news = _db.News.Where(x => x.Id == model.Id).FirstOrDefault();
                news.Subject = model.Subject;
                news.Description = model.Description;
                news.EmployeeAccess = model.EmployeeAccess;
                news.ManagerAccess = model.ManagerAccess;
                news.CustomerAccess = model.CustomerAccess;
                news.CustomerID = model.CustomerID;
                news.ManagerID = model.ManagerID;
                news.WorkerID = model.WorkerID;
                news.SpecificWorker = model.SpecificWorker;
                news.SpecificManager = model.SpecificManager;
                news.SpecificCustomer = model.SpecificCustomer;
                news.NotifyEmployeeViaEmail = model.NotifyEmployeeViaEmail;
                news.AllowCustomer = model.AllowCustomer;
                news.LastModifiedBy = userId;
                news.LastModifiedDate = DateTime.Now;
                _db.SaveChanges();

            }
            else
            {
                News news = new News();
                news.Subject = model.Subject;
                news.Description = model.Description;
                news.EmployeeAccess = model.EmployeeAccess;
                news.ManagerAccess = model.ManagerAccess;
                news.CustomerAccess = model.CustomerAccess;
                news.NotifyEmployeeViaEmail = model.NotifyEmployeeViaEmail;
                news.AllowCustomer = model.AllowCustomer;
                news.CustomerID = model.CustomerID;
                news.ManagerID = model.ManagerID;
                news.WorkerID = model.WorkerID;
                news.SpecificWorker = model.SpecificWorker;
                news.SpecificManager = model.SpecificManager;
                news.SpecificCustomer = model.SpecificCustomer;
                news.Archived = false;
                news.CreatedBy = userId;
                news.CreatedDate = DateTime.Now;
                news.LastModifiedBy = userId;
                news.LastModifiedDate = DateTime.Now;
                _db.News.Add(news);
                _db.SaveChanges();

                if(news.NotifyEmployeeViaEmail == true)
                {
                    Mail.NewsNotificationSendEmailAsync(model);
                }
            }



        }
        public News GetNewsRecordById(int Id)
        {
            return _db.News.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
        }

        public News_Comments GetNewsCommentRecordById(int Id)
        {
            return _db.News_Comments.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
        }
        public void SaveCommentRecords(AdminNewsViewModel model)
        {
            if (model.Id > 0)
            {
                News_Comments _skills = _db.News_Comments.Where(x => x.Id == model.Id).FirstOrDefault();
                _skills.NewsId = model.NewsId;
                _skills.Archived = false;
                _skills.UserIDLastModifiedBy = model.CurrentUserId;
                _skills.Comments = model.Comments;
                _skills.LastModified = DateTime.Now;
                _db.SaveChanges();
            }
            else
            {
                News_Comments _skills = new News_Comments();
                _skills.NewsId = model.NewsId;
                _skills.Archived = false;
                _skills.UserIDCreatedBy = model.CurrentUserId;
                _skills.Comments = model.Comments;
                _skills.CreatedDate = DateTime.Now;
                _db.News_Comments.Add(_skills);
                _db.SaveChanges();

                if (model.NotifyEmployeeViaEmail == true)
                {
                    Mail.NewsCommentNotificationSendEmailAsync(model.NewsId,model.Comments);
                }
            }

        }
        public List<News_Comments> GetCommentList(int Id)
        {
            return _db.News_Comments.Where(x => x.NewsId == Id && x.Archived == false).ToList();
        }
        public int GetCommentCount(int Id)
        {
            return _db.News_Comments.Where(x => x.NewsId == Id && x.Archived == false).Count();
        }
        public List<News_Comments> Editcomments(int Id)
        {
            return _db.News_Comments.Where(x => x.Id == Id).ToList();
        }
        public void Deletecommentrecord(int Id, int Users)
        {
            AdminNewsViewModel model = new AdminNewsViewModel();
            News_Comments _skills = _db.News_Comments.Where(x => x.Id == Id).FirstOrDefault();
            _skills.Archived = true;
            _skills.LastModified = DateTime.Now;
            _skills.UserIDLastModifiedBy = Users;
            _db.SaveChanges();
        }
        public void DeleteNewsrecord(int Id, int Users)
        {
            AdminNewsViewModel model = new AdminNewsViewModel();
            News _skills = _db.News.Where(x => x.Id == Id).FirstOrDefault();
            _skills.Archived = true;
            _skills.LastModifiedDate = DateTime.Now;
            _skills.LastModifiedBy = Users;
            _db.SaveChanges();
        }
    }
}