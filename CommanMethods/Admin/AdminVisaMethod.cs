using HRTool.DataModel;
using HRTool.Models.Admin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace HRTool.CommanMethods.Admin
{
    public class AdminVisaMethod
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        private string inputFormat = "dd-MM-yyyy";
        private string outputFormat = "yyyy-MM-dd HH:mm:ss";

        #endregion

        public List<Visa> getAllVisa() {
            return _db.Visas.Where(x => x.Archived == false).ToList();
        }
        public List<GetVisaDetail_Result> getVisaDetail()
        {
            return _db.GetVisaDetail().ToList();
        }
        public Visa getVisaById(int Id) {
            return _db.Visas.Where(x => x.Id == Id).FirstOrDefault();
        }

        public IList<Visa_Document> getVisaDocumentsByVisaId(int VisaId) {
            return _db.Visa_Document.Where(x => x.VisaId == VisaId).ToList();
        }

        public void SaveData(AdminVisaViewModel model,List<VisaDocumentViewModel> documentList,int userId)
        {

            if (model.Id > 0)
            {
                Visa visa = _db.Visas.Where(x => x.Id == model.Id).FirstOrDefault();
                visa.Country = (int)model.CountryId;
                visa.VisaType = (int)model.VisaTypeId;
                visa.ServiceAgency = (int)model.ServiceAgencyId;
                visa.Number = model.VisaNumber;
                visa.AssignedToEmployeeId = model.AssignToId;
                visa.RelationToCSEmployeeID = model.InRelationToId;
                var validFromToString = DateTime.ParseExact(model.ValidFrom, inputFormat, CultureInfo.InvariantCulture);
                visa.Date = Convert.ToDateTime(validFromToString.ToString(outputFormat));
                var ExpiryDateToString = DateTime.ParseExact(model.ExpiryDate, inputFormat, CultureInfo.InvariantCulture);
                visa.DueDate = Convert.ToDateTime(ExpiryDateToString.ToString(outputFormat)); ;
                visa.Status = (int)model.StatusId;
                visa.AlertBeforeDays = model.AlertBeforeDays;
                visa.Description = model.Description;
                visa.CSUserIDLastModifiedBy = userId;
                visa.LastModified = DateTime.Now;
                _db.SaveChanges();

                
                foreach (var item in _db.Visa_Document.Where(x => x.VisaId == visa.Id).ToList())
                {
                    _db.Visa_Document.Remove(item);
                    _db.SaveChanges();
                }
                foreach (var item in documentList)
                {
                    Visa_Document visaDocument = new Visa_Document();
                    visaDocument.VisaId = visa.Id;
                    visaDocument.NewName = item.newName;
                    visaDocument.OriginalName = item.originalName;
                    visaDocument.Description = item.description;
                    visaDocument.Archived = false;
                    visaDocument.UserIDCreatedBy = userId;
                    visaDocument.CreatedDate = DateTime.Now;
                    visaDocument.UserIDLastModifiedBy = userId;
                    visaDocument.LastModified = DateTime.Now;
                    _db.Visa_Document.Add(visaDocument);
                    _db.SaveChanges();
                }
            }
            else
            {
                Visa visa = new Visa();
                visa.Country = (int)model.CountryId;
                visa.VisaType = (int)model.VisaTypeId;
                visa.ServiceAgency = (int)model.ServiceAgencyId;
                visa.Number = model.VisaNumber;
                visa.AssignedToEmployeeId = model.AssignToId;
                visa.RelationToCSEmployeeID = model.InRelationToId;
                var validFromToString = DateTime.ParseExact(model.ValidFrom, inputFormat, CultureInfo.InvariantCulture);
                visa.Date = Convert.ToDateTime(validFromToString.ToString(outputFormat));
                var ExpiryDateToString = DateTime.ParseExact(model.ExpiryDate, inputFormat, CultureInfo.InvariantCulture);
                visa.DueDate = Convert.ToDateTime(ExpiryDateToString.ToString(outputFormat)); ;
                visa.Status = (int)model.StatusId;
                visa.AlertBeforeDays = model.AlertBeforeDays;
                visa.Description = model.Description;
                visa.Archived = false;
                visa.CSUserIDCreatedBy = userId;
                visa.Created = DateTime.Now;
                visa.CSUserIDLastModifiedBy = userId;
                visa.LastModified = DateTime.Now;
                _db.Visas.Add(visa);
                _db.SaveChanges();

                
                foreach (var item in documentList)
                {
                    Visa_Document visaDocument = new Visa_Document();
                    visaDocument.VisaId = visa.Id;
                    visaDocument.NewName = item.newName;
                    visaDocument.OriginalName = item.originalName;
                    visaDocument.Description = item.description;
                    visaDocument.Archived = false;
                    visaDocument.UserIDCreatedBy = userId;
                    visaDocument.CreatedDate = DateTime.Now;
                    visaDocument.UserIDLastModifiedBy = userId;
                    visaDocument.LastModified = DateTime.Now;
                    _db.Visa_Document.Add(visaDocument);
                    _db.SaveChanges();
                }
            }

        }
    }
}