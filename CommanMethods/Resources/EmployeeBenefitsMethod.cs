using HRTool.DataModel;
using HRTool.Models.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace HRTool.CommanMethods.Resources
{
    public class EmployeeBenefitsMethod
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        private string inputFormat = "dd-MM-yyyy";
        private string outputFormat = "yyyy-MM-dd HH:mm:ss";

        #endregion

        #region Cases Methods

        public Benefit getBrnifitById(int Id)
        {
            return _db.Benefits.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
        }

        public List<Benefit> getBenifitByEmployeeId(int Id)
        {
            return _db.Benefits.Where(x => x.EmployeeID == Id && x.Archived == false).ToList();
        }

        public List<Benefit> getActiveBenifits()
        {
            return _db.Benefits.Where(x => x.Archived == false).ToList();
        }

        public bool deleteCases(int Id, int UserId)
        {
            var data = _db.Benefits.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
            data.Archived = true;
            data.UserIDLastModifiedBy = UserId;
            data.LastModified = DateTime.Now;
            _db.SaveChanges();
            return true;

        }

        public void SaveData(BenefitsViewModel model, List<BenefitsDocumentViewModel> documentList, int userId)
        {
            if (model.Id > 0)
            {
                Benefit benfit = _db.Benefits.Where(x => x.Id == model.Id).FirstOrDefault();
                benfit.EmployeeID = model.EmployeeID;
                benfit.BenefitID = model.BenefitID;
                benfit.Currency = model.Currency;
                var DateAwardedToString = DateTime.ParseExact(model.DateAwarded, inputFormat, CultureInfo.InvariantCulture);
                benfit.DateAwarded = Convert.ToDateTime(DateAwardedToString.ToString(outputFormat));
                var ExpiryDateToString = DateTime.ParseExact(model.ExpiryDate, inputFormat, CultureInfo.InvariantCulture);
                benfit.ExpiryDate = Convert.ToDateTime(ExpiryDateToString.ToString(outputFormat));
                benfit.FixedAmount = model.FixedAmount;
                benfit.RecoverOnTermination = model.RecoverOnTermination;
                benfit.Comments = model.Comments;
                benfit.Archived = false;
                benfit.UserIDCreatedBy = userId;
                benfit.CreatedDate = DateTime.Now;
                benfit.UserIDLastModifiedBy = userId;
                benfit.LastModified = DateTime.Now;                
                _db.SaveChanges();

                foreach (var item in _db.Benefits_Documents.Where(x => x.BenefitsID== benfit.Id).ToList())
                {
                    _db.Benefits_Documents.Remove(item);
                    _db.SaveChanges();
                }
                foreach (var item in documentList)
                {
                    Benefits_Documents benfitDocument = new Benefits_Documents();
                    benfitDocument.BenefitsID = benfit.Id;
                    benfitDocument.NewName = item.newName;
                    benfitDocument.OriginalName = item.originalName;
                    benfitDocument.Description = "";
                    benfitDocument.Archived = false;
                    benfitDocument.UserIDCreatedBy = userId;
                    benfitDocument.CreatedDate = DateTime.Now;
                    benfitDocument.UserIDLastModifiedBy = userId;
                    benfitDocument.LastModified = DateTime.Now;
                    _db.Benefits_Documents.Add(benfitDocument);
                    _db.SaveChanges();
                }

            }
            else {
                Benefit benfit = new Benefit();
                benfit.EmployeeID = model.EmployeeID;
                benfit.BenefitID = model.BenefitID;
                benfit.Currency = model.Currency;
                var DateAwardedToString = DateTime.ParseExact(model.DateAwarded, inputFormat, CultureInfo.InvariantCulture);
                benfit.DateAwarded = Convert.ToDateTime(DateAwardedToString.ToString(outputFormat));
                var ExpiryDateToString = DateTime.ParseExact(model.ExpiryDate, inputFormat, CultureInfo.InvariantCulture);
                benfit.ExpiryDate = Convert.ToDateTime(ExpiryDateToString.ToString(outputFormat));
                benfit.FixedAmount = model.FixedAmount;
                benfit.RecoverOnTermination = model.RecoverOnTermination;
                benfit.Comments = model.Comments;
                benfit.Archived = false;
                benfit.UserIDCreatedBy = userId;
                benfit.CreatedDate = DateTime.Now;
                benfit.UserIDLastModifiedBy = userId;
                benfit.LastModified = DateTime.Now;
                _db.Benefits.Add(benfit);
                _db.SaveChanges();

                foreach (var item in documentList)
                {
                    Benefits_Documents benfitDocument = new Benefits_Documents();
                    benfitDocument.BenefitsID = benfit.Id;
                    benfitDocument.NewName = item.newName;
                    benfitDocument.OriginalName = item.originalName;
                    benfitDocument.Description = "";
                    benfitDocument.Archived = false;
                    benfitDocument.UserIDCreatedBy = userId;
                    benfitDocument.CreatedDate = DateTime.Now;
                    benfitDocument.UserIDLastModifiedBy = userId;
                    benfitDocument.LastModified = DateTime.Now;
                    _db.Benefits_Documents.Add(benfitDocument);
                    _db.SaveChanges();
                }
            }
        }

        public void DeleteData(int Id,int UserId) 
        {
            var benifitData = _db.Benefits.Where(x => x.Id == Id).FirstOrDefault();
            benifitData.Archived = true;
            benifitData.LastModified = DateTime.Now;
            benifitData.UserIDLastModifiedBy = UserId;
            _db.SaveChanges();

            var benifitsbeDocumentList = _db.Benefits_Documents.Where(x => x.BenefitsID == Id).ToList();
            foreach (var item in benifitsbeDocumentList)
            {
                item.Archived = true;
                item.LastModified = DateTime.Now;
                item.UserIDLastModifiedBy = UserId;
                _db.SaveChanges();
            }
        }

        public List<Benefits_Documents> getBenifitDocumentByCaseId(int Id)
        {
            return _db.Benefits_Documents.Where(x => x.BenefitsID == Id).ToList();
        }
        #endregion
    }
}
