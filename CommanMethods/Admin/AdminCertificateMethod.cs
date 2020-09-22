using HRTool.DataModel;
using HRTool.Models.Admin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace HRTool.CommanMethods.Admin
{
    public class AdminCertificateMethod
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        private string inputFormat = "dd-MM-yyyy";
        private string outputFormat = "yyyy-MM-dd HH:mm:ss";

        #endregion

        public List<certificate> getAllCertificate()
        {
            return _db.certificates.Where(x => x.Archived == false).ToList();
        }
        public List<certificate> getCerificateDetailsForCutomer()
        {

            return _db.certificates.Where(x => x.Archived == false).ToList();
        }
        public List<GetCertificateDetail_Result> getAllCertificateDetailList()
        {
            return _db.GetCertificateDetail().ToList();
        }
        public certificate getCertificateById(int Id)
        {
            return _db.certificates.Where(x => x.Id == Id).FirstOrDefault();
        }

        public IList<certificate_document> getCertificateDocumentsByCertificateId(int CertificateId)
        {
            return _db.certificate_document.Where(x => x.CertificateId == CertificateId).ToList();
        }

        public void SaveData(AdminCertificateIdViewModel model, List<CertificateDocumentViewModel> documentList, int userId)
        {

            if (model.Id > 0)
            {
                certificate certificate = _db.certificates.Where(x => x.Id == model.Id).FirstOrDefault();
                certificate.Name = model.Name;
                certificate.Type = (int)model.TypeId;
                certificate.Body = model.Body;
                certificate.Number = model.Number;                
                certificate.AssignTo = model.AssignToId;
                certificate.InRelationTo = model.InRelationToId;
                var validFromToString = DateTime.ParseExact(model.ValidFrom, inputFormat, CultureInfo.InvariantCulture);
                certificate.ValidFrom = Convert.ToDateTime(validFromToString.ToString(outputFormat));
                var ExpiryDateToString = DateTime.ParseExact(model.ExpiryDate, inputFormat, CultureInfo.InvariantCulture);
                certificate.ExpiringDate = Convert.ToDateTime(ExpiryDateToString.ToString(outputFormat)); ;
                certificate.Status = (int)model.StatusId;
                certificate.AlertBeforeDays = model.AlertBeforeDays;
                certificate.Description = model.Description;
                certificate.Mandatory = model.Mandatory;
                certificate.Validate = model.Validate;
                certificate.LastModifiedBy = userId;
                certificate.LastModified = DateTime.Now;
                _db.SaveChanges();


                foreach (var item in _db.certificate_document.Where(x => x.CertificateId == certificate.Id).ToList())
                {
                    _db.certificate_document.Remove(item);
                    _db.SaveChanges();
                }
                foreach (var item in documentList)
                {
                    certificate_document certificateDocument = new certificate_document();
                    certificateDocument.CertificateId = certificate.Id;
                    certificateDocument.NewName = item.newName;
                    certificateDocument.OriginalName = item.originalName;
                    certificateDocument.Description = item.description;
                    certificateDocument.Archived = false;
                    certificateDocument.UserIDCreatedBy = userId;
                    certificateDocument.CreatedDate = DateTime.Now;
                    certificateDocument.UserIDLastModifiedBy = userId;
                    certificateDocument.LastModified = DateTime.Now;
                    _db.certificate_document.Add(certificateDocument);
                    _db.SaveChanges();
                }
            }
            else
            {
                certificate certificate = new certificate();
                certificate.Name = model.Name;
                certificate.Type = (int)model.TypeId;
                certificate.Body = model.Body;
                certificate.Number = model.Number;
                certificate.AssignTo = model.AssignToId;
                certificate.InRelationTo = model.InRelationToId;
                var validFromToString = DateTime.ParseExact(model.ValidFrom, inputFormat, CultureInfo.InvariantCulture);
                certificate.ValidFrom = Convert.ToDateTime(validFromToString.ToString(outputFormat));
                var ExpiryDateToString = DateTime.ParseExact(model.ExpiryDate, inputFormat, CultureInfo.InvariantCulture);
                certificate.ExpiringDate = Convert.ToDateTime(ExpiryDateToString.ToString(outputFormat)); ;
                certificate.Status = (int)model.StatusId;
                certificate.AlertBeforeDays = model.AlertBeforeDays;
                certificate.Description = model.Description;
                certificate.Mandatory = model.Mandatory;
                certificate.Validate = model.Validate;
                certificate.Archived = false;
                certificate.CreateBy = userId;
                certificate.CreatedDate = DateTime.Now;
                certificate.LastModifiedBy = userId;
                certificate.LastModified = DateTime.Now;
                _db.certificates.Add(certificate);
                _db.SaveChanges();


                foreach (var item in documentList)
                {
                    certificate_document certificateDocument = new certificate_document();
                    certificateDocument.CertificateId = certificate.Id;
                    certificateDocument.NewName = item.newName;
                    certificateDocument.OriginalName = item.originalName;
                    certificateDocument.Description = item.description;
                    certificateDocument.Archived = false;
                    certificateDocument.UserIDCreatedBy = userId;
                    certificateDocument.CreatedDate = DateTime.Now;
                    certificateDocument.UserIDLastModifiedBy = userId;
                    certificateDocument.LastModified = DateTime.Now;
                    _db.certificate_document.Add(certificateDocument);
                    _db.SaveChanges();
                }
            }

        }
    }
}