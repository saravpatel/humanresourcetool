using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models.Admin;
using HRTool.Models.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace HRTool.CommanMethods.Admin
{
    public class AdminTMSMethod
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        TMSSettingsMethod _TMSSettingsMethod = new TMSSettingsMethod();
        private string inputFormat = "dd-MM-yyyy";
        private string outputFormat = "yyyy-MM-dd HH:mm:ss";

        #endregion

        #region Index
        public TMSIndexPageViewModel GetAllCountByVacancy(int VacId)
        {
            TMSIndexPageViewModel model = new TMSIndexPageViewModel();
            var activevacancy = getAllActiveVacancyList();
            if (VacId != 0 && VacId != null)
            {
                int totalApp = totalAllapplicant().Where(x => x.VacancyID == VacId).Count();
                int totalRej = getRejectedByVacId(VacId); 
                int totalTalent = getTotalTalaentPoolByVacId(VacId);
                int totalAccepted = getCountByVacancyTotalAccepted(VacId);
                model.NewApplicent = totalApp;
                model.RejectStep = totalRej;
                model.TalentStep = totalTalent;
                model.Accepted = totalAccepted;
            }
            else
            {
                int totalApp = totalAllapplicant().Count();                                
                int totalRej = getTotalRejected();                
                int totalTalent = getTotalTalaentPool();
                int totalAccepted = getTotalAccepted();
                model.NewApplicent = totalApp;
                model.RejectStep = totalRej;
                model.TalentStep = totalTalent;
                model.Accepted = totalAccepted;
            }
            model.ActiveVacancies = activevacancy.Count;
            return model;
        }

        public TMSIndexPageViewModel GetAllCount()
        {
          
            TMSIndexPageViewModel model = new TMSIndexPageViewModel();
            var activevacancy = getAllActiveVacancyList();
            int totalApp = totalAllapplicant().Count();
            //var newapplicant = getAllApplicantList();
            //var rejectApplicant = getAllRejectdApplicantList();
            int totalRej = getTotalRejected();
            //var talentApplicant = getAllTalentApplicantList();
            int totalTalent = getTotalTalaentPool();
            int totalAccepted = getTotalAccepted();
            //var AcceptedList = GetAcceptedList();
            //if (AcceptedList.Count > 0)
            //{
            //    foreach (var item in AcceptedList)
            //    {
            //        var ApplicantData = getApplicantDetailsById(item.Id);
            //        var datas = getVacancyDetailsById(ApplicantData.VacancyID);
            //        if (datas.RecruitmentProcessID != 0)
            //        {
            //            var res = _TMSSettingsMethod.getTMSSettingListById(datas.RecruitmentProcessID);
            //            var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(res.StepCSV);
            //            if (steps != null)
            //            {
            //                foreach (var s in steps)
            //                {

            //                    TMSSettingStepDetails ss = new TMSSettingStepDetails();
            //                    if (s.Id == ApplicantData.StatusID)
            //                    {
            //                        if (s.StepName == "Accepted") 
            //                        {
            //                            ss.Id = s.Id;
            //                            model.count.Add(ss);
            //                        }

            //                    }

            //                }
            //            }

            //        }

            //    }

            //}

            model.ActiveVacancies = activevacancy.Count;
            model.NewApplicent = totalApp;
            //model.RejectStep = rejectApplicant.Count;
            model.RejectStep = totalRej;
            //model.TalentStep = talentApplicant.Count;
            model.TalentStep = totalTalent;
            //model.Accepted = model.count.Count;
            model.Accepted = totalAccepted;
            return model;
        }

        

        #endregion

        #region TMS Vacancy Methods
        public Vacancy getVacancyDetailsById(int Id)
        {
            return _db.Vacancies.Where(x => x.Id == Id).FirstOrDefault();
        }
        public int UpdateCloseStatus(int StatusId,int VacId,DateTime CloseDate)
        {
            int CloseId = 0;
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Vacancy Status List"))
            {
                if (item.Value == "Close")
                {
                    CloseId = item.Id;
                }
            }
            var update = _db.Vacancies.Where(x => x.Id == VacId).FirstOrDefault();           
            update.StatusID = CloseId;
            update.ClosingDate = CloseDate;
            update.IsReadVacancy = false;
            update.IsRead = false;
            update.UserIDLastModifiedBy = SessionProxy.UserId;
            update.LastModified = DateTime.Now;
            _db.SaveChanges();
            return CloseId;

        }
        public List<Vacancy> getAllVacancyList()
        {
            return _db.Vacancies.Where(x => x.Archived == false).ToList();
        }

        public List<Vacancy> getAllActiveVacancyList()
        {
            int active = 0;
            foreach (var item in _otherSettingMethod.getAllSystemValueListByKeyName("Vacancy Status List"))
            {
                if (item.Value == "Active")
                {
                    active = item.Id;
                }
            }
            return _db.Vacancies.Where(x => x.StatusID == active && x.Archived == false).ToList();
        }



        public bool SaveVacancyData(TMSVacancyViewModel Model, int UserId)
        {
            var data = _db.Vacancies.Where(x => x.Title == Model.Title && x.Id != Model.Id && x.Archived == false).ToList();
            
            if (data.Count < 0)
            {
                return false;
            }
            else
            {
                DateTime StDate = new DateTime();
                var startDateToString = DateTime.ParseExact(Model.ClosingDate, inputFormat, CultureInfo.InvariantCulture);
                StDate = Convert.ToDateTime(startDateToString.ToString(outputFormat));
                if (Model.Id == 0)
                {
                    Vacancy save = new Vacancy();
                    save.Title = Model.Title;
                    save.Summary = Model.Summary;
                    save.StatusID = Model.StatusID;
                    save.ClosingDate = StDate;
                    save.RecruitmentProcessID = Model.RecruitmentProcessID;
                    save.Salary = Model.Salary;
                    save.Location = Model.Location;
                    save.BusinessID = Model.BusinessID;
                    save.DivisionID = Model.DivisionID;
                    save.PoolID = Model.PoolID;
                    save.FunctionID = Model.FunctionID;
                    save.JobDescription = Model.JobDescription;
                    save.HiringLeadID = Model.HiringLeadID;
                    save.MustUploadCoverLetter = Model.MustUploadCoverLetter;
                    save.MustUploadResumeCV = Model.MustUploadResumeCV;
                    save.ApplicationFormPathOriginal = Model.ApplicationFormPathOriginal;
                    save.ApplicationFormPath = Model.ApplicationFormPath;
                    save.Question1On = Model.Question1On;
                    save.Question1Text = Model.Question1Text;
                    save.Question2On = Model.Question2On;
                    save.Question2Text = Model.Question2Text;
                    save.Question3On = Model.Question3On;
                    save.Question3Text = Model.Question3Text;
                    save.Question4On = Model.Question4On;
                    save.Question4Text = Model.Question4Text;
                    save.Question5On = Model.Question5On;
                    save.Question5Text = Model.Question5Text;
                    save.SourceID = Model.SourceID;
                    save.CreatedDate = DateTime.Now;
                    save.Archived = false;
                    save.IsReadVacancy = false;
                    save.UserIDCreatedBy = UserId;
                    save.UserIDLastModifiedBy = UserId;
                    save.LastModified = DateTime.Now;
                    _db.Vacancies.Add(save);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    var update = _db.Vacancies.Where(x => x.Id == Model.Id).FirstOrDefault();
                    update.Title = Model.Title;
                    update.Summary = Model.Summary;
                    update.StatusID = Model.StatusID;
                    update.ClosingDate = StDate;
                    update.RecruitmentProcessID = Model.RecruitmentProcessID;
                    update.Salary = Model.Salary;
                    update.Location = Model.Location;
                    update.BusinessID = Model.BusinessID;
                    update.DivisionID = Model.DivisionID;
                    update.PoolID = Model.PoolID;
                    update.FunctionID = Model.FunctionID;
                    update.JobDescription = Model.JobDescription;
                    update.HiringLeadID = Model.HiringLeadID;
                    update.MustUploadCoverLetter = Model.MustUploadCoverLetter;
                    update.MustUploadResumeCV = Model.MustUploadResumeCV;
                    update.ApplicationFormPathOriginal = Model.ApplicationFormPathOriginal;
                    update.ApplicationFormPath = Model.ApplicationFormPath;
                    update.Question1On = Model.Question1On;
                    update.Question1Text = Model.Question1Text;
                    update.Question2On = Model.Question2On;
                    update.Question2Text = Model.Question2Text;
                    update.Question3On = Model.Question3On;
                    update.Question3Text = Model.Question3Text;
                    update.Question4On = Model.Question4On;
                    update.Question4Text = Model.Question4Text;
                    update.Question5On = Model.Question5On;
                    update.Question5Text = Model.Question5Text;
                    update.SourceID = Model.SourceID;
                    update.IsReadVacancy = false;
                    update.IsRead = false;
                    update.UserIDLastModifiedBy = UserId;
                    update.LastModified = DateTime.Now;
                    _db.SaveChanges();
                    return true;
                }
            }
        }

        public bool DeleteVacancy(int Id, int UserId)
        {
            var data = _db.Vacancies.Where(x => x.Id == Id).FirstOrDefault();
            data.Archived = true;
            data.UserIDLastModifiedBy = UserId;
            data.LastModified = DateTime.Now;
            _db.SaveChanges();
            return true;

        }


        #endregion

        #region TMS Applicant Method
        public bool AddRejectReasonApplicant(string Name)
        {
            ApplicantRejectReason appRejectReason = new ApplicantRejectReason();
            appRejectReason.Name = Name;
            appRejectReason.Archived = false;
            appRejectReason.CreatedDate = DateTime.Now;
            appRejectReason.UserIdCreatedNy = SessionProxy.UserId;
            appRejectReason.UserIdLastModifiedBy = SessionProxy.UserId;
            _db.ApplicantRejectReasons.Add(appRejectReason);
            _db.SaveChanges();
            return true;
        }
        public TMS_Applicant getApplicantDetailsById(int Id)
        {
            return _db.TMS_Applicant.Where(x => x.Id == Id).FirstOrDefault();
        }
        public List<TMS_Applicant> getAllApplicantList()
        {
            return _db.TMS_Applicant.Where(x => x.Archived == false).ToList();
        }
        public List<TMS_Applicant> totalAllapplicant()
        {           
            var t = (from tms in _db.TMS_Applicant
                     join vac in _db.Vacancies on tms.VacancyID equals vac.Id                     
                     join sys in _db.SystemListValues on vac.StatusID equals sys.Id
                     where sys.Value == "Active" && sys.Archived==false
                     select tms).ToList();           
            int total = t.Count();
            return t;
        }
        public List<TMS_Applicant> getAllNewApplicantList()
        {          
            return _db.TMS_Applicant.Where(x => x.StatusID == 3 && x.Archived == false).ToList();
        }
        public List<TMS_Applicant> getAllTalentApplicantList()
        {
            return _db.TMS_Applicant.Where(x => x.StatusID == 2 && x.Archived == false).ToList();
        }
        public List<TMS_Applicant> getAllRejectdApplicantList()
        {
            return _db.TMS_Applicant.Where(x => x.StatusID == 1 && x.Archived == false).ToList();
        }

        public int getTotlaNewApplicanyByVac(int vacId)
        {
            List<TMS_Applicant> model = new List<TMS_Applicant>();
            int cnt = 0;
            var t = _db.TMS_Applicant.Where(x=>x.VacancyID==vacId).ToList();
            foreach (var data in t)
            {
                var vacData = _db.Vacancies.Where(x => x.Id == data.VacancyID).FirstOrDefault();
                var RecData = _db.TMS_Setting_RecruitmentProcesses.Where(x => x.Id == vacData.RecruitmentProcessID).FirstOrDefault();
                if (RecData.StepCSV != null)
                {
                    var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(RecData.StepCSV);
                    if (steps != null)
                    {
                        foreach (var s in steps)
                        {

                            TMSSettingStepDetails ss = new TMSSettingStepDetails();
                            if (s.Id == data.StatusID)
                            {
                                if (s.StepName == "New Applicants")
                                {
                                    ss.Id = s.Id;
                                    cnt = cnt + 1;
                                }
                            }
                        }
                    }
                }
            }
            return cnt;
        }

        public int getTotalTalaentPool()
        {
            List<TMS_Applicant> model = new List<TMS_Applicant>();
            int cnt = 0;
            var t = _db.TMS_Applicant.ToList();
            foreach (var data in t)
            {
                var vacData = _db.Vacancies.Where(x => x.Id == data.VacancyID).FirstOrDefault();
                var RecData = _db.TMS_Setting_RecruitmentProcesses.Where(x => x.Id == vacData.RecruitmentProcessID).FirstOrDefault();
                if (RecData.StepCSV != null)
                {
                    var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(RecData.StepCSV);
                    if (steps != null)
                    {
                        foreach (var s in steps)
                        {

                            TMSSettingStepDetails ss = new TMSSettingStepDetails();
                            if (s.Id == data.StatusID)
                            {
                                if (s.StepName == "Talent Pool")
                                {
                                    ss.Id = s.Id;
                                    cnt = cnt + 1;
                                }
                            }
                        }
                    }
                }
            }
            return cnt;
        }
        public int getTotalTalaentPoolByVacId(int vacId)
        {
            List<TMS_Applicant> model = new List<TMS_Applicant>();
            int cnt = 0;
            var t = _db.TMS_Applicant.Where(x=>x.VacancyID==vacId).ToList();
            foreach (var data in t)
            {
                var vacData = _db.Vacancies.Where(x => x.Id == data.VacancyID).FirstOrDefault();
                var RecData = _db.TMS_Setting_RecruitmentProcesses.Where(x => x.Id == vacData.RecruitmentProcessID).FirstOrDefault();
                if (RecData.StepCSV != null)
                {
                    var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(RecData.StepCSV);
                    if (steps != null)
                    {
                        foreach (var s in steps)
                        {

                            TMSSettingStepDetails ss = new TMSSettingStepDetails();
                            if (s.Id == data.StatusID)
                            {
                                if (s.StepName == "Talent Pool")
                                {
                                    ss.Id = s.Id;
                                    cnt = cnt + 1;
                                }
                            }
                        }
                    }
                }
            }
            return cnt;
        }

        public int getTotalRejected()
        {
            int cnt = 0;
            var t = _db.TMS_Applicant.ToList();
            foreach (var data in t)
            {
                var vacData = _db.Vacancies.Where(x => x.Id == data.VacancyID).FirstOrDefault();
                var RecData = _db.TMS_Setting_RecruitmentProcesses.Where(x => x.Id == vacData.RecruitmentProcessID).FirstOrDefault();
                if (RecData.StepCSV != null)
                {
                    var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(RecData.StepCSV);
                    if (steps != null)
                    {
                        foreach (var s in steps)
                        {

                            TMSSettingStepDetails ss = new TMSSettingStepDetails();
                            if (s.Id == data.StatusID)
                            {
                                if (s.StepName == "Rejected")
                                {
                                    ss.Id = s.Id;
                                    cnt = cnt + 1;
                                }
                            }
                        }
                    }
                }
            }
            return cnt;           
        }
        public int getRejectedByVacId(int vacId)
        {
            int cnt = 0;
            var t = _db.TMS_Applicant.Where(x=>x.VacancyID==vacId).ToList();
            foreach (var data in t)
            {
                var vacData = _db.Vacancies.Where(x => x.Id == data.VacancyID).FirstOrDefault();
                var RecData = _db.TMS_Setting_RecruitmentProcesses.Where(x => x.Id == vacData.RecruitmentProcessID).FirstOrDefault();
                if (RecData.StepCSV != null)
                {
                    var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(RecData.StepCSV);
                    if (steps != null)
                    {
                        foreach (var s in steps)
                        {

                            TMSSettingStepDetails ss = new TMSSettingStepDetails();
                            if (s.Id == data.StatusID)
                            {
                                if (s.StepName == "Rejected")
                                {
                                    ss.Id = s.Id;
                                    cnt = cnt + 1;
                                }
                            }
                        }
                    }
                }
            }
            return cnt;
        }
        public int getTotalAccepted()
        {
            int cnt = 0;
            var t = _db.TMS_Applicant.ToList();
            foreach (var data in t)
            {
                var vacData = _db.Vacancies.Where(x => x.Id == data.VacancyID).FirstOrDefault();
                var RecData = _db.TMS_Setting_RecruitmentProcesses.Where(x => x.Id == vacData.RecruitmentProcessID).FirstOrDefault();
                if (RecData.StepCSV != null)
                {
                    var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(RecData.StepCSV);
                    if (steps != null)
                    {
                        foreach (var s in steps)
                        {

                            TMSSettingStepDetails ss = new TMSSettingStepDetails();
                            if (s.Id == data.StatusID)
                            {
                                if (s.StepName == "Accepted")
                                {
                                    ss.Id = s.Id;
                                    cnt = cnt + 1;
                                }
                            }
                        }
                    }
                }
            }
            return cnt;
        }

        public int getCountByVacancyTotalAccepted(int vacId)
        {
            int cnt = 0;
            var t = _db.TMS_Applicant.Where(x=>x.VacancyID==vacId).ToList();
            foreach (var data in t)
            {
                var vacData = _db.Vacancies.Where(x => x.Id == data.VacancyID).FirstOrDefault();
                var RecData = _db.TMS_Setting_RecruitmentProcesses.Where(x => x.Id == vacData.RecruitmentProcessID).FirstOrDefault();
                if (RecData.StepCSV != null)
                {
                    var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(RecData.StepCSV);
                    if (steps != null)
                    {
                        foreach (var s in steps)
                        {

                            TMSSettingStepDetails ss = new TMSSettingStepDetails();
                            if (s.Id == data.StatusID)
                            {
                                if (s.StepName == "Accepted")
                                {
                                    ss.Id = s.Id;
                                    cnt = cnt + 1;
                                }
                            }
                        }
                    }
                }
            }
            return cnt;
        }
        public List<TMS_Applicant> GetAcceptedList()
        {
            return _db.TMS_Applicant.Where(x=>x.Archived == true).ToList();
        }
       
        

        public int GetAcceptedStepId (int Id)
        {
            var ApplicantData = getApplicantDetailsById(Id);
            var VacancyDetails = getVacancyDetailsById(ApplicantData.VacancyID);
            var rec = _TMSSettingsMethod.getTMSSettingListById(VacancyDetails.RecruitmentProcessID);
            var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(rec.StepCSV);
            return steps.Count;
        }

        public List<TMS_Applicant> getApplicantListByStepId(int Id)
        {
            return _db.TMS_Applicant.Where(x => x.StatusID == Id && x.Archived == false).ToList();
        }

        public List<TMS_Applicant> getApplicantListByStepIdVacancyID(int Id, int VacancyID)
        {
            return _db.TMS_Applicant.Where(x => x.StatusID == Id && x.VacancyID == VacancyID && x.Archived == false).ToList();
        }

        public List<TMS_Applicant_Comments> getApplicantCommentListById(int Id)
        {
            return _db.TMS_Applicant_Comments.Where(x => x.ApplicantID == Id && x.Archived==false).ToList();
        }

        public List<TMS_Applicant_Documents> getApplicantDocumentListById(int Id)
        {
            return _db.TMS_Applicant_Documents.Where(x => x.ApplicantID == Id && x.Archived == false).ToList();
        }
        public bool Emailcheck(string Email, int Id, int VacancyID)
        {
            var data = _db.TMS_Applicant.Where(x => x.Email.ToLower() == Email.ToLower() && x.Id != Id && x.VacancyID != VacancyID && x.Archived == false).ToList();
            if (data.Count < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool SaveApplicantData(TMSApplicantViewModel datamodel, List<TMSApplicantCommentViewModel> listComment, List<TMSApplicantDocumentViewModel> listDocument, int UserId)
        {
            var data = _db.TMS_Applicant.Where(x => x.Email == datamodel.Email && x.Id != datamodel.Id && x.VacancyID != datamodel.VacancyID && x.Archived == false).ToList();
            List<TMS_Applicant_Documents> tmsDoc = new List<TMS_Applicant_Documents> ();
            if (data.Count < 0)
            {
                return false;
            }
            else
            {
                DateTime StDate = new DateTime();
                if (datamodel.DateOfBirth != "" && datamodel.DateOfBirth != null)
                {
                    var startDateToString = DateTime.ParseExact(datamodel.DateOfBirth, inputFormat, CultureInfo.InvariantCulture);
                    StDate = Convert.ToDateTime(startDateToString.ToString(outputFormat));
                }                
                if (datamodel.Id == 0)
                {
                    TMS_Applicant save = new TMS_Applicant();
                    save.VacancyID = datamodel.VacancyID;
                    save.FirstName = datamodel.FirstName;
                    save.LastName = datamodel.LastName;
                    save.Email = datamodel.Email;
                    save.GenderID = datamodel.GenderID;
                    save.DateOfBirth = StDate;
                    save.PostalCode = datamodel.PostalCode;
                    save.Address = datamodel.Address;
                    save.OtherContactDetails = datamodel.OtherContactDetails;
                    save.StatusID = datamodel.StatusID;
                    save.CoverLetterPath = datamodel.CoverLetterPath;
                    save.CoverLetterPathOriginal = datamodel.CoverLetterPathOriginal;
                    save.DownloadApplicationFormLink = datamodel.DownloadApplicationFormLink;
                    save.UploadApplicationFormPathOriginal = datamodel.UploadApplicationFormPathOriginal;
                    save.UploadApplicationFormPath = datamodel.UploadApplicationFormPath;
                    save.ResumePathOriginal = datamodel.ResumePathOriginal;
                    save.ResumePath = datamodel.ResumePath;
                    save.Question1Answer = datamodel.Question1Answer;
                    save.Question2Answer = datamodel.Question2Answer;
                    save.Question3Answer = datamodel.Question3Answer;
                    save.Question4Answer = datamodel.Question4Answer;
                    save.Question5Answer = datamodel.Question5Answer;
                    save.SourceID = datamodel.SourceID;
                    save.CompatencyJSV = datamodel.CompatencyJSV;
                    save.CommentJSV = datamodel.CommentJSV;
                    save.DocumentJSV = datamodel.DocumentJSV;
                    save.GeneralSkillsJSV = datamodel.GeneralSkillsJSV;
                    save.TechnicalSkillsJSV = datamodel.TechnicalSkillsJSV;
                    save.Cost = datamodel.Cost;
                    save.CreatedDate = DateTime.Now;
                    save.Archived = false;
                    save.UserIDCreatedBy = UserId;
                    save.UserIDLastModifiedBy = UserId;
                    save.LastModified = DateTime.Now;
                    save.IsReadHiringLead = false;
                    if (datamodel.RejectReasonId != 0 && datamodel.RejectReasonId != null)
                    {
                        save.RejectReasonId = datamodel.RejectReasonId;
                        save.RejectReasonComment = datamodel.RejectReasonComment;
                     }
                    _db.TMS_Applicant.Add(save);
                    _db.SaveChanges();

                    foreach (var item in _db.TMS_Applicant_Comments.Where(x => x.ApplicantID == save.Id).ToList())
                    {
                        _db.TMS_Applicant_Comments.Remove(item);
                        _db.SaveChanges();
                    }
                    foreach (var item in listComment)
                    {
                        TMS_Applicant_Comments mm = new TMS_Applicant_Comments();
                        mm.ApplicantID = save.Id;
                        mm.Description = item.comment;
                        mm.CreatedName = item.commentBy;
                        mm.CreatedDateTime = item.commentTime;
                        mm.Archived = false;
                        mm.UserIDCreatedBy = UserId;
                        mm.CreatedDate = DateTime.Now;
                        mm.UserIDLastModifiedBy = UserId;
                        mm.LastModified = DateTime.Now;
                        _db.TMS_Applicant_Comments.Add(mm);
                        _db.SaveChanges();
                    }
                    foreach (var item in _db.TMS_Applicant_Documents.Where(x => x.ApplicantID == save.Id).ToList())
                    {
                        _db.TMS_Applicant_Documents.Remove(item);
                        _db.SaveChanges();
                    }
                    foreach (var item in listDocument)
                    {
                        TMS_Applicant_Documents Document = new TMS_Applicant_Documents();
                        Document.ApplicantID = save.Id;
                        Document.NewName = item.newName;
                        Document.OriginalName = item.originalName;
                        Document.Description = item.description;
                        Document.Archived = false;
                        Document.UserIDCreatedBy = UserId;
                        Document.CreatedDate = DateTime.Now;
                        Document.UserIDLastModifiedBy = UserId;
                        Document.LastModified = DateTime.Now;
                        _db.TMS_Applicant_Documents.Add(Document);
                        _db.SaveChanges();
                    }
                    return true;
                }
                else
                {
                    TMS_Applicant update = new TMS_Applicant();
                    update = _db.TMS_Applicant.Where(x => x.Id == datamodel.Id).FirstOrDefault();
                    update.VacancyID = datamodel.VacancyID;
                    update.FirstName = datamodel.FirstName;
                    update.LastName = datamodel.LastName;
                    update.Email = datamodel.Email;
                    update.GenderID = datamodel.GenderID;
                    update.DateOfBirth = StDate;
                    update.PostalCode = datamodel.PostalCode;
                    update.Address = datamodel.Address;
                    update.OtherContactDetails = datamodel.OtherContactDetails;
                    update.StatusID = datamodel.StatusID;
                    update.CoverLetterPath = datamodel.CoverLetterPath;
                    update.DownloadApplicationFormLink = datamodel.DownloadApplicationFormLink;
                    update.UploadApplicationFormPath = datamodel.UploadApplicationFormPath;
                    update.ResumePath = datamodel.ResumePath;
                    update.Question1Answer = datamodel.Question1Answer;
                    update.Question2Answer = datamodel.Question2Answer;
                    update.Question3Answer = datamodel.Question3Answer;
                    update.Question4Answer = datamodel.Question4Answer;
                    update.Question5Answer = datamodel.Question5Answer;
                    update.SourceID = datamodel.SourceID;
                    update.CompatencyJSV = datamodel.CompatencyJSV;
                    update.CommentJSV = datamodel.CommentJSV;
                    update.DocumentJSV = datamodel.DocumentJSV;
                    update.GeneralSkillsJSV = datamodel.GeneralSkillsJSV;
                    update.TechnicalSkillsJSV = datamodel.TechnicalSkillsJSV;
                    update.Cost = datamodel.Cost;
                    update.UserIDLastModifiedBy = UserId;
                    update.IsReadHiringLead = false;
                    update.LastModified = DateTime.Now;
                    _db.SaveChanges();

                    foreach (var item in listComment)
                    {
                        var tmsCommnts = _db.TMS_Applicant_Comments.Where(x => x.ApplicantID == update.Id).ToList();

                        TMS_Applicant_Comments mm = new TMS_Applicant_Comments();
                        mm.ApplicantID = update.Id;
                        mm.Description = item.comment;
                        mm.CreatedName = item.commentBy;
                        mm.CreatedDateTime = item.commentTime;
                        mm.Archived = false;
                        mm.UserIDCreatedBy = UserId;
                        mm.CreatedDate = DateTime.Now;
                        mm.UserIDLastModifiedBy = UserId;
                        mm.LastModified = DateTime.Now;
                        _db.TMS_Applicant_Comments.Add(mm);
                        _db.SaveChanges();
                    }
                    if (listDocument.Count == 0)
                    {
                        var ddata = _db.TMS_Applicant_Documents.Where(x => x.ApplicantID == update.Id).ToList();
                        if (ddata != null)
                        {
                            foreach (var item in ddata)
                            {
                                TMS_Applicant_Documents Document = new TMS_Applicant_Documents();
                                Document.ApplicantID = update.Id;
                                Document.Archived = true;
                                Document.UserIDCreatedBy = UserId;
                                Document.CreatedDate = DateTime.Now;
                                Document.UserIDLastModifiedBy = UserId;
                                Document.LastModified = DateTime.Now;
                                tmsDoc.Add(Document);                                
                            }
                            _db.SaveChanges();
                        }
                    }
                    else
                    {
                        foreach (var item in listDocument)
                        {
                            var ddata = _db.TMS_Applicant_Documents.Where(x => x.ApplicantID == update.Id).ToList();
                            //foreach (var docitem in _db.TMS_Applicant_Documents.Where(x => x.ApplicantID == update.Id).ToList())
                            //{
                            //    _db.TMS_Applicant_Documents.Remove(docitem);
                            //    _db.SaveChanges();
                            //}
                            //foreach (var dd in ddata)
                            //{
                            //    TMS_Applicant_Documents Document = new TMS_Applicant_Documents();
                            //    if (item.Id==dd.Id)
                            //    {
                            //        Document.ApplicantID = update.Id;
                            //        Document.NewName = item.newName;
                            //        Document.OriginalName = item.originalName;
                            //        Document.Description = item.description;
                            //        Document.Archived = false;
                            //        Document.UserIDCreatedBy = UserId;
                            //        Document.CreatedDate = DateTime.Now;
                            //        Document.UserIDLastModifiedBy = UserId;
                            //        Document.LastModified = DateTime.Now;
                            //        tmsDoc.Add(Document);
                            //        //_db.TMS_Applicant_Documents.Add(Document);
                            //    }
                            //    else
                            //    {
                            //        Document.ApplicantID = update.Id;
                            //        Document.NewName = item.newName;
                            //        Document.OriginalName = item.originalName;
                            //        Document.Description = item.description;
                            //        Document.Archived = true;
                            //        Document.UserIDCreatedBy = UserId;
                            //        Document.CreatedDate = DateTime.Now;
                            //        Document.UserIDLastModifiedBy = UserId;
                            //        Document.LastModified = DateTime.Now;
                            //        tmsDoc.Add(Document);                                    
                            //    }
                            //    _db.SaveChanges();
                            //}
                        }
                        
                    }
                    return true;
                }
            }
        }

        public List<GetTMSRecruitmentProcessChart_Result> getTMSRecList(int? RecId,int? BusiId,int? DivId,int? PoolId,int? FunId)
        {
            return _db.GetTMSRecruitmentProcessChart(RecId, BusiId, DivId, PoolId, FunId).ToList();
        }

        public bool UpdateStepMoveDetails(ApplicantStepMoveViewModel model)
        {
            var data = _db.TMS_Applicant.Where(x => x.Id == model.ApplicantID).FirstOrDefault();
            data.StatusID = model.StepID;
            _db.SaveChanges();
            return true;

        }

        public bool AllStepMoveReject(int Id) 
        {
            var data = _db.TMS_Applicant.Where(x => x.StatusID == Id).ToList();
            if(data.Count>0){
                foreach (var model in data)
                {
                    model.StatusID = 1;
                    _db.SaveChanges();
                }
            }
            return true;

        }
        public bool AllStepMoveTalent(int Id)
        {
            var data = _db.TMS_Applicant.Where(x => x.StatusID == Id).ToList();
            if (data.Count > 0)
            {
                foreach (var model in data)
                {
                    model.StatusID = 2;
                    _db.SaveChanges();
                }
            }
            return true;

        }

        public bool DeleteApplicant(int Id, int UserId)
        {
            var data = _db.TMS_Applicant.Where(x => x.Id == Id).FirstOrDefault();
            data.Archived = true;
            data.UserIDLastModifiedBy = UserId;
            data.LastModified = DateTime.Now;
            _db.SaveChanges();
            return true;

        }

        


        #endregion
    }
}