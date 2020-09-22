using HRTool.DataModel;
using HRTool.Models.Admin;
using HRTool.Models.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.CommanMethods.Admin
{
    public class AdminCaseLogMethod
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();

        #endregion

        public IList<Case> getActiveList() 
        {
            return _db.Cases.Where(x=>x.Archived == false).ToList();
        }
        public IList<GetAllEmployeeCaseDetail_Result> getActiveCaseDetail()
        {
            return _db.GetAllEmployeeCaseDetail().ToList();
        }

        public Case getCaseById(int Id)
        {
            return _db.Cases.Where(x => x.Id == Id).FirstOrDefault();
        }

        public List<Cases_Comments> getCaseCommentByCaseId(int Id)
        {
            return _db.Cases_Comments.Where(x => x.CaseID == Id).ToList();
        }

        public List<Cases_Documents> getCaseDocumentByCaseId(int Id)
        {
            return _db.Cases_Documents.Where(x => x.CaseID == Id).ToList();
        }

        public void SaveData(int Id, int Status, int EmployeeId, int CategoryId, string Summary, List<AdminCaseLogCommentViewModel> CommentList, List<AdminCaseLogDocumentViewModel> DocumentList, int UserId)
        {

            if (Id > 0)
            {
                Case cases = _db.Cases.Where(x => x.Id == Id).FirstOrDefault();
                cases.EmployeeID = EmployeeId;
                cases.Summary = Summary;
                cases.Category = CategoryId;
                cases.Status = Status;
                cases.Archived = false;
                cases.UserIDCreatedBy = UserId;
                cases.CreatedDate = DateTime.Now;
                cases.UserIDLastModifiedBy = UserId;
                cases.LastModified = DateTime.Now;
                _db.SaveChanges();

                foreach (var item in _db.Cases_Comments.Where(x=>x.CaseID == cases.Id).ToList())
                {
                    _db.Cases_Comments.Remove(item);
                    _db.SaveChanges();
                }
                foreach (var item in CommentList)
                {
                    Cases_Comments caseComment = new Cases_Comments();
                    caseComment.CaseID = cases.Id;
                    caseComment.Description = item.comment;
                    caseComment.CreatedName = item.commentBy;
                    caseComment.CreatedDateTime = item.commentTime;
                    caseComment.Archived = false;
                    caseComment.UserIDCreatedBy = UserId;
                    caseComment.CreatedDate = DateTime.Now;
                    caseComment.UserIDLastModifiedBy = UserId;
                    caseComment.LastModified = DateTime.Now;
                    _db.Cases_Comments.Add(caseComment);
                    _db.SaveChanges();
                }

                foreach (var item in _db.Cases_Documents.Where(x => x.CaseID == cases.Id).ToList())
                {
                    _db.Cases_Documents.Remove(item);
                    _db.SaveChanges();
                }
                foreach (var item in DocumentList)
                {
                    Cases_Documents caseDocument = new Cases_Documents();
                    caseDocument.CaseID = cases.Id;
                    caseDocument.NewName = item.newName;
                    caseDocument.OriginalName = item.originalName;
                    caseDocument.Description = item.description;
                    caseDocument.Archived = false;
                    caseDocument.UserIDCreatedBy = UserId;
                    caseDocument.CreatedDate = DateTime.Now;
                    caseDocument.UserIDLastModifiedBy = UserId;
                    caseDocument.LastModified = DateTime.Now;
                    _db.Cases_Documents.Add(caseDocument);
                    _db.SaveChanges();
                }
            }
            else {

                Case cases = new Case();
                cases.EmployeeID = EmployeeId;
                cases.Summary = Summary;
                cases.Category = CategoryId;
                cases.Status = Status;
                cases.Archived = false;
                cases.UserIDCreatedBy = UserId;
                cases.CreatedDate = DateTime.Now;
                cases.UserIDLastModifiedBy = UserId;
                cases.LastModified = DateTime.Now;
                _db.Cases.Add(cases);
                _db.SaveChanges();

                foreach (var item in CommentList)
                {
                    Cases_Comments caseComment = new Cases_Comments();
                    caseComment.CaseID = cases.Id;
                    caseComment.Description = item.comment;
                    caseComment.CreatedName = item.commentBy;
                    caseComment.CreatedDateTime = item.commentTime;
                    caseComment.Archived = false;
                    caseComment.UserIDCreatedBy = UserId;
                    caseComment.CreatedDate = DateTime.Now;
                    caseComment.UserIDLastModifiedBy = UserId;
                    caseComment.LastModified = DateTime.Now;
                    _db.Cases_Comments.Add(caseComment);
                    _db.SaveChanges();
                }

                foreach (var item in DocumentList)
                {
                    Cases_Documents caseDocument = new Cases_Documents();
                    caseDocument.CaseID = cases.Id;
                    caseDocument.NewName = item.newName;
                    caseDocument.OriginalName = item.originalName;
                    caseDocument.Description = item.description;
                    caseDocument.Archived = false;
                    caseDocument.UserIDCreatedBy = UserId;
                    caseDocument.CreatedDate = DateTime.Now;
                    caseDocument.UserIDLastModifiedBy = UserId;
                    caseDocument.LastModified = DateTime.Now;
                    _db.Cases_Documents.Add(caseDocument);
                    _db.SaveChanges();
                }
            }
        
        }

        public void SaveEmployeeCaseData(int Id, int Status, int EmployeeId, int CategoryId, string Summary, List<CaseLogCommentViewModel> CommentList, List<CaseLogDocumentViewModel> DocumentList, int UserId)
        {

            if (Id > 0)
            {
                Case cases = _db.Cases.Where(x => x.Id == Id).FirstOrDefault();
                cases.EmployeeID = EmployeeId;
                cases.Summary = Summary;
                cases.Category = CategoryId;
                cases.Status = Status;
                cases.Archived = false;
                cases.UserIDCreatedBy = UserId;
                cases.CreatedDate = DateTime.Now;
                cases.UserIDLastModifiedBy = UserId;
                cases.LastModified = DateTime.Now;
                _db.SaveChanges();

                foreach (var item in _db.Cases_Comments.Where(x => x.CaseID == cases.Id).ToList())
                {
                    _db.Cases_Comments.Remove(item);
                    _db.SaveChanges();
                }
                foreach (var item in CommentList)
                {
                    Cases_Comments caseComment = new Cases_Comments();
                    caseComment.CaseID = cases.Id;
                    caseComment.Description = item.comment;
                    caseComment.CreatedName = item.commentBy;
                    caseComment.CreatedDateTime = item.commentTime;
                    caseComment.Archived = false;
                    caseComment.UserIDCreatedBy = UserId;
                    caseComment.CreatedDate = DateTime.Now;
                    caseComment.UserIDLastModifiedBy = UserId;
                    caseComment.LastModified = DateTime.Now;
                    _db.Cases_Comments.Add(caseComment);
                    _db.SaveChanges();
                }

                foreach (var item in _db.Cases_Documents.Where(x => x.CaseID == cases.Id).ToList())
                {
                    _db.Cases_Documents.Remove(item);
                    _db.SaveChanges();
                }
                foreach (var item in DocumentList)
                {
                    Cases_Documents caseDocument = new Cases_Documents();
                    caseDocument.CaseID = cases.Id;
                    caseDocument.NewName = item.newName;
                    caseDocument.OriginalName = item.originalName;
                    caseDocument.Description = item.description;
                    caseDocument.Archived = false;
                    caseDocument.UserIDCreatedBy = UserId;
                    caseDocument.CreatedDate = DateTime.Now;
                    caseDocument.UserIDLastModifiedBy = UserId;
                    caseDocument.LastModified = DateTime.Now;
                    _db.Cases_Documents.Add(caseDocument);
                    _db.SaveChanges();
                }
            }
            else
            {

                Case cases = new Case();
                cases.EmployeeID = EmployeeId;
                cases.Summary = Summary;
                cases.Category = CategoryId;
                cases.Status = Status;
                cases.Archived = false;
                cases.UserIDCreatedBy = UserId;
                cases.CreatedDate = DateTime.Now;
                cases.UserIDLastModifiedBy = UserId;
                cases.LastModified = DateTime.Now;
                _db.Cases.Add(cases);
                _db.SaveChanges();

                foreach (var item in CommentList)
                {
                    Cases_Comments caseComment = new Cases_Comments();
                    caseComment.CaseID = cases.Id;
                    caseComment.Description = item.comment;
                    caseComment.CreatedName = item.commentBy;
                    caseComment.CreatedDateTime = item.commentTime;
                    caseComment.Archived = false;
                    caseComment.UserIDCreatedBy = UserId;
                    caseComment.CreatedDate = DateTime.Now;
                    caseComment.UserIDLastModifiedBy = UserId;
                    caseComment.LastModified = DateTime.Now;
                    _db.Cases_Comments.Add(caseComment);
                    _db.SaveChanges();
                }

                foreach (var item in DocumentList)
                {
                    Cases_Documents caseDocument = new Cases_Documents();
                    caseDocument.CaseID = cases.Id;
                    caseDocument.NewName = item.newName;
                    caseDocument.OriginalName = item.originalName;
                    caseDocument.Description = item.description;
                    caseDocument.Archived = false;
                    caseDocument.UserIDCreatedBy = UserId;
                    caseDocument.CreatedDate = DateTime.Now;
                    caseDocument.UserIDLastModifiedBy = UserId;
                    caseDocument.LastModified = DateTime.Now;
                    _db.Cases_Documents.Add(caseDocument);
                    _db.SaveChanges();
                }
            }

        }
    }
}