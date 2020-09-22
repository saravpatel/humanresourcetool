using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRTool.DataModel;
using HRTool.Models.Resources;
using HRTool.Models.Settings;
using System.Globalization;
using System.Web.Script.Serialization;

namespace HRTool.CommanMethods.Resources
{
    public class EmployeePerformanceMethod
    {
        EvolutionEntities _db = new EvolutionEntities();
        public bool SaveEmployeePerformace(EmployeePerformanceViewModel model)
        {
            var checkperfomance = _db.EmployeePerformances.Where(x => x.ReviewId == model.ReviewId && x.Archived == false && x.EmployeeId == model.EmployeeId).ToList();
            var checkPerformanceSetting = _db.PerformanceSettings.Where(x => x.Id == model.ReviewId && x.Archived == false).FirstOrDefault();

            if (checkPerformanceSetting.CompletionDate == null)
            {
                model.CompletionDateTime = model.CompletionDateTime.Replace("-", "/");
                model.CompletionDate = DateTime.ParseExact(model.CompletionDateTime, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                checkPerformanceSetting.CompletionDate = model.CompletionDate;
                _db.Entry(checkPerformanceSetting).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
            }
            if (checkperfomance.Count > 0)
            {
                return false;
            }
            else
            {
                EmployeePerformance EmpPer = new EmployeePerformance();
                EmpPer.ProjectId = model.ProjectId;
                EmpPer.EmployeeId = model.EmployeeId;
                EmpPer.ReviewId = model.ReviewId;
                EmpPer.ReviewDate = DateTime.Now;
                EmpPer.RevviewByEmpID = SessionProxy.UserId;
                EmpPer.ReviewStatus = "Open";
                EmpPer.Archived = false;
                EmpPer.UserIDCreatedBy = SessionProxy.UserId;
                EmpPer.UserIDLastModifiedBy = SessionProxy.UserId;
                EmpPer.CreatedDate = DateTime.Now;
                EmpPer.LastModifiedDate = DateTime.Now;
                _db.EmployeePerformances.Add(EmpPer);
                _db.SaveChanges();
                return true;
            }
        }

        public List<EmployeePerformance> getListofPerformance()
        {
            return _db.EmployeePerformances.Where(x => x.Archived == false && x.ReviewStatus == "Open").ToList();
        }
        public List<EmployeePerformance> getPastListPerformance()
        {
            return _db.EmployeePerformances.Where(x => x.Archived == false && x.ReviewDate < DateTime.Now).ToList();
        }
        public IList<AspNetUser> getAllUserList()
        {

            return (from Record1 in _db.AspNetUsers
                    join Record2 in _db.AspNetUserRoles on Record1.Id equals Record2.UserId
                    join Record3 in _db.AspNetRoles on Record2.RoleId equals Record3.Id
                    where Record1.Archived == false && Record3.Id == 2
                    select Record1).ToList();

        }
        public IList<AspNetUser> getAllCustomerList()
        {

            return (from Record1 in _db.AspNetUsers
                    join Record2 in _db.AspNetUserRoles on Record1.Id equals Record2.UserId
                    join Record3 in _db.AspNetRoles on Record2.RoleId equals Record3.Id
                    where Record1.Archived == false && Record3.Id == 4
                    select Record1).ToList();

        }
        public int SaveCoworkerInviteLink(int invitEmpID, int PerfId, int empID)
        {
            PerformanceCoWorkerDetail model = new PerformanceCoWorkerDetail();
            model.InviteEmployeeId = invitEmpID;
            model.EmployeeID = empID;
            model.PerformanceId = PerfId;
            model.Archived = false;
            model.Status = "Invited";
            model.InviteStatus = false;
            model.CreatedDate = DateTime.Now;
            model.LastModifiedDate = DateTime.Now;
            _db.PerformanceCoWorkerDetails.Add(model);
            _db.SaveChanges();
            return model.Id;
        }
        public int SaveCoworkerInviteLinkForMeEmployee(int invitEmpID, int PerfId, int empID, int reviewID, string fullName)
        {
            SendReviewToCoworker model = new SendReviewToCoworker();
            model.InviteeEmployee = invitEmpID;
            model.EmployeeID = empID;
            model.PerformanceID = PerfId;
            model.Archived = false;
            model.MailStatus = "Invited";
            model.ReviewID = reviewID;
            model.EmailSentDate = DateTime.Now;
            model.EmailSentBy = empID;
            model.FullName = fullName;
            _db.SendReviewToCoworkers.Add(model);
            _db.SaveChanges();
            return model.Id;
        }

        public int SaveCustomerInviteLinkForMeEmployee(int invitEmpID, int PerfId, int empID, int reviewID, string fullName)
        {
            SendReviewToCustomer model = new SendReviewToCustomer();
            model.InviteeEmployee = invitEmpID;
            model.EmployeeID = empID;
            model.PerformanceID = PerfId;
            model.Archived = false;
            model.MailStatus = "Invited";
            model.ReviewID = reviewID;
            model.EmailSentDate = DateTime.Now;
            model.EmailSentBy = empID;
            model.FullName = fullName;
            _db.SendReviewToCustomers.Add(model);
            _db.SaveChanges();
            return model.Id;
        }
        public bool UpdateCoWorkerDetails(int PerId, string ComCSV)
        {
            PerformanceCoWorkerDetail coDetails = _db.PerformanceCoWorkerDetails.Where(x => x.Id == PerId && x.Archived == false).FirstOrDefault();
            EditSegmentViewModel model = new EditSegmentViewModel();
            JavaScriptSerializer js = new JavaScriptSerializer();
            model.CoworkerSegment = js.Deserialize<List<CoworkerSegment>>(ComCSV);
            int totalScore = 0, totalQue = 0;
            foreach (var item in model.CoworkerSegment)
            {
                item.questionDataList = js.Deserialize<List<CoworkerSegmentQuetion>>(item.questionData);
                totalQue = totalQue + item.questionDataList.Count();
                foreach (var data in item.questionDataList)
                {

                    totalScore = totalScore + Convert.ToInt32(data.Score);
                }
            }
            int AvgCoreScore = totalScore / totalQue;
            coDetails.CoWorkerScoreJSC = ComCSV;
            coDetails.CoreScore = Convert.ToString(AvgCoreScore);
            coDetails.Status = "See Response";
            coDetails.InviteStatus = true;
            coDetails.LastModifiedDate = DateTime.Now;
            _db.SaveChanges();
            return true;
        }
        public bool UpdateCoWorkerDetailsForMeDecline(int perID)
        {
            SendReviewToCoworker coDetails = _db.SendReviewToCoworkers.Where(x => x.Id == perID && x.Archived == false).FirstOrDefault();
            coDetails.MailStatus = "Declined";
            _db.SaveChanges();
            return true;
        }

        public bool UpdateCustomerDetailsForMeDecline(int perID)
        {
            SendReviewToCustomer coDetails = _db.SendReviewToCustomers.Where(x => x.Id == perID && x.Archived == false).FirstOrDefault();
            coDetails.MailStatus = "Declined";
            _db.SaveChanges();
            return true;
        }
        public bool UpdateCoWorkerDetailsForMe(int PerId, string ComCSV)
        {
            SendReviewToCoworker coDetails = _db.SendReviewToCoworkers.Where(x => x.Id == PerId && x.Archived == false).FirstOrDefault();
            EditSegmentViewModel model = new EditSegmentViewModel();
            JavaScriptSerializer js = new JavaScriptSerializer();
            model.CoworkerSegment = js.Deserialize<List<CoworkerSegment>>(ComCSV);
            int totalScore = 0, totalQue = 0;
            foreach (var item in model.CoworkerSegment)
            {
                item.questionDataList = js.Deserialize<List<CoworkerSegmentQuetion>>(item.questionData);
                totalQue = totalQue + item.questionDataList.Count();
                foreach (var data in item.questionDataList)
                {

                    totalScore = totalScore + Convert.ToInt32(data.Score);
                }
            }
            int AvgCoreScore = totalScore / totalQue;
            coDetails.CoWorkerScoreJSC = ComCSV;
            coDetails.CoreScore = Convert.ToString(AvgCoreScore);
            coDetails.MailStatus = "See Response";
            _db.SaveChanges();
            return true;
        }
        public bool UpdateCustomerCoreScore(int PerId, string ComCSV)
        {
            SendReviewToCustomer coDetails = _db.SendReviewToCustomers.Where(x => x.Id == PerId && x.Archived == false).FirstOrDefault();
            EditSegmentViewModel model = new EditSegmentViewModel();
            JavaScriptSerializer js = new JavaScriptSerializer();
            model.CoworkerSegment = js.Deserialize<List<CoworkerSegment>>(ComCSV);
            int totalScore = 0, totalQue = 0;
            foreach (var item in model.CoworkerSegment)
            {
                item.questionDataList = js.Deserialize<List<CoworkerSegmentQuetion>>(item.questionData);
                totalQue = totalQue + item.questionDataList.Count();
                foreach (var data in item.questionDataList)
                {

                    totalScore = totalScore + Convert.ToInt32(data.Score);
                }
            }
            int AvgCoreScore = totalScore / totalQue;
            coDetails.CoWorkerScoreJSC = ComCSV;
            coDetails.CoreScore = Convert.ToString(AvgCoreScore);
            coDetails.MailStatus = "See Response";
            _db.SaveChanges();
            return true;
        }

        public bool deleteInviteCutomer(int CustomerId)
        {
            var inviteCustomerList = _db.PerformanceCustomerDetails.Where(x => x.InviteCustomer_Id == CustomerId).ToList();
            foreach (var item in inviteCustomerList)
            {
                PerformanceCustomerDetail delete = inviteCustomerList.Where(x => x.Id == item.Id).FirstOrDefault();
                _db.PerformanceCustomerDetails.Remove(delete);
                _db.SaveChanges();
                var deleteJSON = _db.EmployeePerformanceCustomerSegmentJSONDetails.Where(x => x.PerCustomerdetailId == item.Id).ToList();
                foreach (var JSONitem in deleteJSON)
                {
                    EmployeePerformanceCustomerSegmentJSONDetail custoJSON = deleteJSON.Where(x => x.Id == JSONitem.Id).FirstOrDefault();
                    _db.EmployeePerformanceCustomerSegmentJSONDetails.Remove(custoJSON);
                }
            }

            return true;
        }

        public bool deleteInviteEmployee(int EmployeID)
        {
            var EmployeeList = _db.EmployeePerformances.Where(x => x.EmployeeId == EmployeID).ToList();
            var EmployeePerformnceData = _db.PerformanceEmployeeDetails.Where(x => x.EmployeeId == EmployeID && x.IsArchived == false).FirstOrDefault();
            foreach (var item in EmployeeList)
            {
                EmployeePerformance delete = EmployeeList.Where(x => x.Id == item.Id).FirstOrDefault();
                _db.EmployeePerformances.Remove(delete);
                _db.SaveChanges();
                var deleteJSON = _db.EmployeePerformanceCoreJSONDetails.Where(x => x.PerEmployeedetailId == EmployeePerformnceData.Id).ToList();
                var delateJobRole = _db.EmployeePerformanceJobRoleSegmentJSONDetails.Where(x => x.PerEmployeedetailId == EmployeID && x.Archived == false).ToList();
                foreach (var JSONitem in deleteJSON)
                {
                    EmployeePerformanceCoreJSONDetail coreJSON = deleteJSON.Where(x => x.Id == JSONitem.Id).FirstOrDefault();
                    _db.EmployeePerformanceCoreJSONDetails.Remove(coreJSON);
                }
                foreach (var JSONitem in delateJobRole)
                {
                    EmployeePerformanceJobRoleSegmentJSONDetail jobroleJSON = delateJobRole.Where(x => x.PerEmployeedetailId == EmployeID && x.Archived == false).FirstOrDefault();
                }
            }
            return true;
        }
        public int saveInviteCustomer(string OverallScoreString, string Comments, int PerReviewId, int CustomerId, int EmpId, string JSONCustomerSegment, string JSONJobRoleSegment, string EmpPerfDetailId)
        {
            //var ExistPerformance = _db.PerformanceCustomerDetails.Where(x => x.Performance_Id == PerReviewId && x.InviteCustomer_Id == CustomerId).ToList();
            //if (ExistPerformance != null && ExistPerformance.Count > 0)
            //{
            //    bool delete = deleteInviteCutomer(CustomerId);

            //}
            PerformanceCustomerDetail custDetails = new PerformanceCustomerDetail();
            if (!string.IsNullOrEmpty(EmpPerfDetailId) && EmpPerfDetailId != "0")
            {
                custDetails.Id = Convert.ToInt32(EmpPerfDetailId);
                //Update OverAllScore Of Employee               
            }
            custDetails.Performance_Id = PerReviewId;
            custDetails.InviteCustomer_Id = CustomerId;
            // custDetails.OverallScoreJSON = OverallScoreString;
            custDetails.InviteStatus = "Invite";
            custDetails.Comments = Comments;
            custDetails.IsArchived = false;
            custDetails.EmployeeId = EmpId;
            custDetails.CusomerSegmentsJSON = JSONCustomerSegment;
            custDetails.LastModifiedBy = SessionProxy.UserId;
            custDetails.LastModifiedDate = DateTime.Now;
            _db.PerformanceCustomerDetails.Add(custDetails);
            _db.SaveChanges();
            JavaScriptSerializer js = new JavaScriptSerializer();
            EditSegmentViewModel model = new EditSegmentViewModel();
            if (!string.IsNullOrEmpty(JSONCustomerSegment))
            {
                model.CustomerSeg = js.Deserialize<List<CustomerSegment>>(JSONCustomerSegment);
                foreach (var item in model.CustomerSeg)
                {
                    item.CustomerQueListData = js.Deserialize<List<CustomerQuestionModel>>(item.QueationType);
                }
            }
            if (!string.IsNullOrEmpty(JSONJobRoleSegment))
            {
                model.JobRoleSeg = js.Deserialize<List<JobRoleSegment>>(JSONJobRoleSegment);
                foreach (var item in model.JobRoleSeg)
                {
                    item.JobRoleQueListData = js.Deserialize<List<JobRoleQuestionModel>>(item.QueationType);
                }
            }
            EmployeePerformanceCustomerSegmentJSONDetail custoJSON = new EmployeePerformanceCustomerSegmentJSONDetail();
            if (model.CustomerSeg != null && model.CustomerSeg.Count > 0)
            {
                foreach (var item in model.CustomerSeg)
                {
                    foreach (var data in item.CustomerQueListData)
                    {
                        custoJSON.CustomerSegId = item.CustoIds;
                        custoJSON.PerCustomerdetailId = custDetails.Id;
                        custoJSON.QueId = Convert.ToInt32(data.QueId);
                        custoJSON.Score = data.Score;
                        custoJSON.Comments = data.Comments;
                        custoJSON.Archived = false;
                        custoJSON.UserIdCreatedBy = SessionProxy.UserId;
                        custoJSON.UserIdCreatedDate = DateTime.Now;
                        custoJSON.LastModifiedBy = SessionProxy.UserId;
                        custoJSON.LastModifiedDate = DateTime.Now;
                        _db.EmployeePerformanceCustomerSegmentJSONDetails.Add(custoJSON);
                        _db.SaveChanges();
                    }
                }
            }
            EmployeePerformanceJobRoleSegmentJSONDetail jobroleJSON = new EmployeePerformanceJobRoleSegmentJSONDetail();
            if (model.JobRoleSeg != null && model.JobRoleSeg.Count > 0)
            {
                foreach (var item in model.JobRoleSeg)
                {
                    foreach (var data in item.JobRoleQueListData)
                    {
                        jobroleJSON.PerCustomerdetailId = custDetails.Id;
                        jobroleJSON.JobRoleSegId = item.JobRoleIds;
                        jobroleJSON.QueId = Convert.ToInt32(data.QueId);
                        jobroleJSON.Score = data.Score;
                        jobroleJSON.Comment = data.Comments;
                        jobroleJSON.Archived = false;
                        jobroleJSON.UserIdCreatedBy = SessionProxy.UserId;
                        jobroleJSON.UserIdCreatedDate = DateTime.Now;
                        jobroleJSON.LastModifiedBy = SessionProxy.UserId;
                        jobroleJSON.LastModifiedDate = DateTime.Now;
                        _db.EmployeePerformanceJobRoleSegmentJSONDetails.Add(jobroleJSON);
                        _db.SaveChanges();
                    }
                }
            }
            int EmpPerfDeId = Convert.ToInt32(EmpPerfDetailId);
            var CalculateCustomerScore = _db.EmployeePerformanceCustomerSegmentJSONDetails.Where(x => x.PerCustomerdetailId == EmpPerfDeId && x.Archived == false).ToList();
            if (CalculateCustomerScore.Count > 0)
            {
                int TotalCoreScore = 0;
                double AvgQueCoreScore = 0;
                foreach (var item in CalculateCustomerScore)
                {
                    TotalCoreScore = TotalCoreScore + Convert.ToInt32(item.Score);
                }
                AvgQueCoreScore = TotalCoreScore / CalculateCustomerScore.Count;
                var EmployeePerf = _db.PerformanceCustomerDetails.Where(x => x.Id == EmpPerfDeId).FirstOrDefault();
                EmployeePerf.CustomerScore = Convert.ToString(AvgQueCoreScore);
                EmployeePerf.LastModifiedDate = DateTime.Now;
                EmployeePerf.LastModifiedBy = SessionProxy.UserId;
                _db.SaveChanges();
            }
            return custDetails.Id;
        }

        public int saveEmployeePerformance(int PerformanceID, int Flag, string OverallScoreString, string Comments, int EmpId, string JSONCustomerSegment, string JSONJobRoleSegment, int ReviewID, int IsManagerEmployee, int EmpPerfDetailId, int EmployeePerformaceID)
        {
            var ExistPerformance = _db.PerformanceEmployeeDetails.Where(x => x.Performance_Id == ReviewID && x.EmployeeId == EmpId).ToList();
            JavaScriptSerializer js = new JavaScriptSerializer();
            EditSegmentViewModel model = new EditSegmentViewModel();
            EmployeePerformanceCoreJSONDetail custoJSON = new EmployeePerformanceCoreJSONDetail();
            EmployeePerformanceJobRoleSegmentJSONDetail jobroleJSON = new EmployeePerformanceJobRoleSegmentJSONDetail();
            int EmpPerfDeId = Convert.ToInt32(EmpPerfDetailId);
            int IsManagerID = Convert.ToInt32(IsManagerEmployee);
            //if (ExistPerformance != null && ExistPerformance.Count > 0)
            //{
            //    bool delete = deleteInviteCutomer(EmpId);

            //}
            int ID = 0;
            PerformanceEmployeeDetail perDetails = new PerformanceEmployeeDetail();
            if (Flag == 2)
            {
                var ExistPerformanceData = (from record in _db.PerformanceEmployeeDetails.Where(x => x.ReviewID == ReviewID && x.EmployeeId == EmpId && x.IsArchived == false && x.IsManager_Id == IsManagerEmployee) orderby record.UserIDCreatedDate descending select record).SingleOrDefault();
                if (ExistPerformanceData != null)
                {
                    ExistPerformanceData.OverAllScoreJSON = OverallScoreString;
                    ExistPerformanceData.Comments = Comments;
                    ExistPerformanceData.IsArchived = false;
                    ExistPerformanceData.EmployeeId = EmpId;
                    ExistPerformanceData.UserIDLastModifiedBy = SessionProxy.UserId;
                    ExistPerformanceData.UserIDLastModifiedDate = DateTime.Now;
                    _db.Entry(ExistPerformanceData).State = System.Data.Entity.EntityState.Modified;
                    ExistPerformanceData.IsManager_Id = IsManagerID;
                    _db.SaveChanges();
                    ID = (from record in _db.PerformanceEmployeeDetails where record.ReviewID == ReviewID && record.EmployeeId == EmpId && record.IsArchived == false && record.IsManager_Id == IsManagerEmployee orderby record.Id descending select record.Id).First();


                    //Update OverAllScore Of Employee               
                }
                else
                {
                    perDetails.Performance_Id = EmployeePerformaceID;
                    perDetails.OverAllScoreJSON = OverallScoreString;
                    perDetails.Comments = Comments;
                    if (IsManagerEmployee == 0)
                    {
                        perDetails.IsManager_Id = IsManagerID;
                        perDetails.EmployeeId = EmpId;
                    }
                    else if (IsManagerEmployee == 1)
                    {
                        perDetails.IsManager_Id = IsManagerID;
                        perDetails.Performance_ManagerId = SessionProxy.UserId;
                    }
                    perDetails.IsArchived = false;
                    perDetails.EmployeeId = EmpId;
                    perDetails.UserIDLastModifiedBy = SessionProxy.UserId;
                    perDetails.UserIDLastModifiedDate = DateTime.Now;
                    perDetails.ReviewID = ReviewID;
                    perDetails.IsFormCompleted = true;
                    _db.PerformanceEmployeeDetails.Add(perDetails);
                    _db.SaveChanges();
                    ID = (from record in _db.PerformanceEmployeeDetails where record.ReviewID == ReviewID && record.EmployeeId == EmpId && record.IsArchived == false && record.IsManager_Id == IsManagerEmployee orderby record.Id descending select record.Id).First();

                }
            }
            else if (Flag == 1)
            {

                if (!string.IsNullOrEmpty(JSONCustomerSegment))
                {
                    model.CoreSeg = js.Deserialize<List<CoreSegment>>(JSONCustomerSegment);
                    foreach (var item in model.CoreSeg)
                    {
                        item.CoreQueListData = js.Deserialize<List<QuestionModel>>(item.QueationType);
                    }
                }
                if (model.CoreSeg != null && model.CoreSeg.Count > 0)
                {
                    foreach (var item in model.CoreSeg)
                    {
                        foreach (var data in item.CoreQueListData)
                        {
                            int QueIDCoreSeg = Convert.ToInt32(data.QueId);
                            var ExistingCoreSegData = (from record in _db.EmployeePerformanceCoreJSONDetails.Where(x => x.ReviewID == ReviewID && x.EmployeeID == EmpId && x.QueId == QueIDCoreSeg && x.CoreId == item.CoreId && x.Archived == false && x.IsManager_Id == IsManagerEmployee) orderby record.UserIdCreatedBy descending select record).SingleOrDefault();
                            if (ExistingCoreSegData != null)
                            {
                                ExistingCoreSegData.Score = data.Score;
                                ExistingCoreSegData.Comments = data.Comments;
                                ExistingCoreSegData.UserIdLastModifiedBy = SessionProxy.UserId;
                                ExistingCoreSegData.UserIdLastModifiedDate = DateTime.Now;
                                ExistingCoreSegData.IsManager_Id = IsManagerID;
                                _db.Entry(ExistingCoreSegData).State = System.Data.Entity.EntityState.Modified;
                                _db.SaveChanges();
                                ID = (from record in _db.EmployeePerformanceCoreJSONDetails where record.ReviewID == ReviewID && record.EmployeeID == EmpId && record.Archived == false && record.IsManager_Id == IsManagerEmployee orderby record.Id descending select record.Id).First();
                            }
                            else
                            {
                                if (IsManagerEmployee == 1)
                                {
                                    custoJSON.IsManager_Id = IsManagerID;
                                    custoJSON.PerManagerDetailId = SessionProxy.UserId;
                                }
                                else
                                {
                                    custoJSON.IsManager_Id = IsManagerID;
                                    //custoJSON.PerManagerDetailId = SessionProxy.UserId;
                                }
                                custoJSON.CoreId = item.CoreId;
                                custoJSON.PerEmployeedetailId = perDetails.Id;
                                custoJSON.QueId = Convert.ToInt32(data.QueId);
                                custoJSON.Score = data.Score;
                                custoJSON.Comments = data.Comments;
                                custoJSON.Archived = false;
                                custoJSON.EmployeeID = EmpId;
                                custoJSON.UserIdCreatedBy = SessionProxy.UserId;
                                custoJSON.UserIdCreatedDate = DateTime.Now;
                                custoJSON.UserIdLastModifiedBy = SessionProxy.UserId;
                                custoJSON.UserIdLastModifiedDate = DateTime.Now;
                                custoJSON.ReviewID = ReviewID;
                                custoJSON.IsFormCompleted = true;
                                _db.EmployeePerformanceCoreJSONDetails.Add(custoJSON);
                                _db.SaveChanges();
                                ID = (from record in _db.EmployeePerformanceCoreJSONDetails where record.ReviewID == ReviewID && record.EmployeeID == EmpId && record.Archived == false && record.IsManager_Id == IsManagerEmployee orderby record.Id descending select record.Id).First();

                            }

                        }
                    }
                }

            }
            else if (Flag == 0)
            {

                if (!string.IsNullOrEmpty(JSONJobRoleSegment))
                {
                    model.JobRoleSeg = js.Deserialize<List<JobRoleSegment>>(JSONJobRoleSegment);
                    foreach (var item in model.JobRoleSeg)
                    {
                        item.JobRoleQueListData = js.Deserialize<List<JobRoleQuestionModel>>(item.QueationType);
                    }
                }
                if (model.JobRoleSeg != null && model.JobRoleSeg.Count > 0)
                {
                    foreach (var item in model.JobRoleSeg)
                    {
                        foreach (var data in item.JobRoleQueListData)
                        {
                            int QueIDJobRoleSeg = Convert.ToInt32(data.QueId);
                            var ExistingJObRoleSegData = (from record in _db.EmployeePerformanceJobRoleSegmentJSONDetails.Where(x => x.ReviewID == ReviewID && x.EmployeeID == EmpId && x.QueId == QueIDJobRoleSeg && x.JobRoleSegId == item.JobRoleIds && x.Archived == false && x.IsManager_Id == IsManagerEmployee) orderby record.UserIdCreatedBy descending select record).SingleOrDefault();
                            if (ExistingJObRoleSegData != null)
                            {
                                ExistingJObRoleSegData.Score = data.Score;
                                ExistingJObRoleSegData.Comment = data.Comments;
                                ExistingJObRoleSegData.LastModifiedBy = SessionProxy.UserId;
                                ExistingJObRoleSegData.LastModifiedDate = DateTime.Now;
                                _db.Entry(ExistingJObRoleSegData).State = System.Data.Entity.EntityState.Modified;
                                ExistingJObRoleSegData.IsManager_Id = IsManagerID;
                                _db.SaveChanges();
                                ID = (from record in _db.EmployeePerformanceJobRoleSegmentJSONDetails where record.ReviewID == ReviewID && record.EmployeeID == EmpId && record.Archived == false && record.IsManager_Id == IsManagerEmployee orderby record.Id descending select record.Id).First();
                            }
                            else
                            {
                                if (IsManagerEmployee == 1)
                                {
                                    jobroleJSON.IsManager_Id = IsManagerID;
                                    jobroleJSON.PerManagerdetailId = SessionProxy.UserId;
                                }
                                else
                                {
                                    jobroleJSON.IsManager_Id = IsManagerID;
                                    // jobroleJSON.PerManagerdetailId = null;
                                }
                                jobroleJSON.PerEmployeedetailId = perDetails.Id;
                                jobroleJSON.JobRoleSegId = item.JobRoleIds;
                                jobroleJSON.QueId = Convert.ToInt32(data.QueId);
                                jobroleJSON.Score = data.Score;
                                jobroleJSON.Comment = data.Comments;
                                jobroleJSON.Archived = false;
                                jobroleJSON.EmployeeID = EmpId;
                                jobroleJSON.UserIdCreatedBy = SessionProxy.UserId;
                                jobroleJSON.UserIdCreatedDate = DateTime.Now;
                                jobroleJSON.LastModifiedBy = SessionProxy.UserId;
                                jobroleJSON.LastModifiedDate = DateTime.Now;
                                jobroleJSON.ReviewID = ReviewID;
                                jobroleJSON.IsFormCompleted = true;
                                _db.EmployeePerformanceJobRoleSegmentJSONDetails.Add(jobroleJSON);
                                _db.SaveChanges();
                                ID = (from record in _db.EmployeePerformanceJobRoleSegmentJSONDetails where record.ReviewID == ReviewID && record.EmployeeID == EmpId && record.Archived == false && record.IsManager_Id == IsManagerEmployee orderby record.Id descending select record.Id).First();

                            }

                        }
                    }
                }

                // Update CoreScore

                var JobRoleScore = _db.EmployeePerformanceJobRoleSegmentJSONDetails.Where(x => x.ReviewID == ReviewID && x.EmployeeID == EmpId && x.Archived == false && x.IsManager_Id == IsManagerEmployee).ToList();
                if (JobRoleScore.Count > 0)
                {
                    int TotalJobRoleScore = 0;
                    double AvgQueJobRoleScore = 0;
                    foreach (var item in JobRoleScore)
                    {
                        int check;
                        int.TryParse(item.Score, out check);
                        TotalJobRoleScore = TotalJobRoleScore + check;
                    }
                    AvgQueJobRoleScore = TotalJobRoleScore / JobRoleScore.Count;
                    var EmployeePerf = (from record in _db.EmployeePerformances where record.ReviewId == ReviewID & record.Archived == false orderby record.Id descending select record).FirstOrDefault();
                    //var EmployeePerf = _db.EmployeePerformances.Where(x => x.Id == ReviewID && x.Archived == false).FirstOrDefault();
                    EmployeePerf.JobRoleScore = Convert.ToString(AvgQueJobRoleScore);
                    EmployeePerf.LastModifiedDate = DateTime.Now;
                    EmployeePerf.UserIDLastModifiedBy = SessionProxy.UserId;
                    _db.SaveChanges();
                }
            }
            var CalculateCoreScore = _db.EmployeePerformanceCoreJSONDetails.Where(x => x.ReviewID == ReviewID && x.EmployeeID == EmpId && x.Archived == false).ToList();
            if (CalculateCoreScore.Count > 0)
            {
                int TotalCoreScore = 0;
                double AvgQueCoreScore = 0;
                foreach (var item in CalculateCoreScore)
                {
                    int chkq;
                    int.TryParse(item.Score, out chkq);
                    TotalCoreScore = TotalCoreScore + chkq;
                }
                AvgQueCoreScore = TotalCoreScore / CalculateCoreScore.Count;
                var EmployeePerf = _db.PerformanceEmployeeDetails.Where(x => x.Id == EmpPerfDeId).FirstOrDefault();
                if (EmployeePerf != null)
                {
                    EmployeePerf.CoreScore = Convert.ToString(AvgQueCoreScore);
                    EmployeePerf.UserIDLastModifiedDate = DateTime.Now;
                    EmployeePerf.UserIDLastModifiedBy = SessionProxy.UserId;
                }
                _db.SaveChanges();
            }
            if (!string.IsNullOrEmpty(OverallScoreString))
            {
                string[] OverallScoreStringList = OverallScoreString.Split('^');
                int OverAllScoreValue = OverallScoreStringList.Length;
                var EmployeePerf = (from record in _db.EmployeePerformances where record.ReviewId == ReviewID & record.Archived == false orderby record.Id descending select record).FirstOrDefault();
                EmployeePerf.OverallScore = Convert.ToString(OverAllScoreValue);
                EmployeePerf.LastModifiedDate = DateTime.Now;
                EmployeePerf.UserIDLastModifiedBy = SessionProxy.UserId;
                _db.SaveChanges();
            }
            return ID;
        }

        public bool CloseEmployeePerformace(int PerReviewId, int EmpId)
        {
            var EmpPer = _db.EmployeePerformances.Where(x => x.ReviewId == PerReviewId && x.EmployeeId == EmpId && x.Archived == false).FirstOrDefault();
            EmpPer.ReviewStatus = "Close";
            EmpPer.UserIDLastModifiedBy = SessionProxy.UserId;
            EmpPer.LastModifiedDate = DateTime.Now;
            _db.SaveChanges();

            return true;
        }

        //Save Performance Objective
        public bool SaveEmployeePerformanceObjective(ObjectiveOfEmployeePerformance model)
        {
            string inputFormat = "dd-MM-yyyy";
            string outputFormat = "yyyy-MM-dd HH:mm:ss";

            if (model.Id > 0)
            {
                var GoalData = _db.EmployeePerformanceGoals.Where(x => x.Id == model.Id && x.Archived == false).FirstOrDefault();
                if (GoalData != null)
                {
                    GoalData.EmployeePerformanceId = model.EmpPerformanceId;
                    GoalData.EmployeePerformanceId = model.EmpPerformanceId;
                    GoalData.GoalName = model.GoalName;
                    GoalData.EmployeeId = model.EmployeeId;
                    GoalData.GoalDescription = model.GoalDescription;
                    if (model.DueDate != null)
                    {
                        var StartDateToString = DateTime.ParseExact(model.DueDate, inputFormat, CultureInfo.InvariantCulture);
                        GoalData.DueDate = Convert.ToDateTime(StartDateToString.ToString(outputFormat));
                    }
                    GoalData.Unit = model.UnitPercent;
                    GoalData.LastModifiedDate = DateTime.Now;
                    _db.SaveChanges();
                }
            }
            else
            {
                EmployeePerformanceGoal goalModel = new EmployeePerformanceGoal();
                goalModel.EmployeePerformanceId = model.EmpPerformanceId;
                goalModel.GoalName = model.GoalName;
                goalModel.EmployeeId = model.EmployeeId;
                goalModel.GoalDescription = model.GoalDescription;
                if (model.DueDate != null)
                {
                    var StartDateToString = DateTime.ParseExact(model.DueDate, inputFormat, CultureInfo.InvariantCulture);
                    goalModel.DueDate = Convert.ToDateTime(StartDateToString.ToString(outputFormat));
                }
                goalModel.Unit = model.UnitPercent;
                goalModel.CreatedDate = DateTime.Now;
                goalModel.GoalValueX = 280;
                goalModel.GoalValueY = 205;
                goalModel.GoalStatus = "Open";
                goalModel.Archived = false;
                _db.EmployeePerformanceGoals.Add(goalModel);
                _db.SaveChanges();
            }
            return true;
        }

        public List<EmployeePerformanceGoal> getListofPerformance(int EmpId)
        {
            return _db.EmployeePerformanceGoals.Where(x => x.EmployeeId == EmpId && x.Archived == false).ToList();
        }

        public bool SavePerformanceDocument(int GoalId, ObjectiveOfEmployeePerformance model, string Status, string UnitPercent)
        {
            var data = _db.EmployeePerformanceGoals.Where(x => x.Id == GoalId && x.Archived == false).FirstOrDefault();
            if (data != null)
            {
                data.GoalStatus = Status;
                if (UnitPercent != null)
                {
                    model.UnitPercent = UnitPercent;
                }
                data.LastModifiedDate = DateTime.Now;
                _db.SaveChanges();
            }
            foreach (var item in model.DocumentList)
            {
                EmployeePerformanceGoalDocument docModel = new EmployeePerformanceGoalDocument();
                docModel.PerformanceGoalId = GoalId;
                docModel.NewName = item.newName;
                docModel.OriginalName = item.originalName;
                docModel.UserIDCreatedBy = SessionProxy.UserId;
                docModel.Archived = false;
                docModel.CreatedDate = DateTime.Now;
                _db.EmployeePerformanceGoalDocuments.Add(docModel);
                _db.SaveChanges();
            }
            return true;
        }

        //Get GoalDocument
        public List<EmployeePerformanceGoalDocument> getAllGoalDocument(int goalId)
        {
            return _db.EmployeePerformanceGoalDocuments.Where(x => x.Archived == false && x.PerformanceGoalId == goalId).ToList();
        }

        //Save Goal Commnets

        public bool SaveGoalComment(int ComtId, int GoalId, int EmpPerformanceId, int EmployeeId, string Comment, string UnitPercent)
        {
            var GoalData = _db.EmployeePerformanceGoals.Where(x => x.Id == GoalId && x.Archived == false).FirstOrDefault();
            if (GoalData != null)
            {
                if (!string.IsNullOrEmpty(UnitPercent))
                {
                    GoalData.Unit = UnitPercent;
                    _db.SaveChanges();
                }
            }
            if (ComtId != 0)
            {
                var CommtData = _db.EmployeePerformanceGoalComments.Where(x => x.Id == ComtId && x.Archived == false).FirstOrDefault();
                CommtData.Description = Comment;
                CommtData.UserIDLastModifiedBy = SessionProxy.UserId;
                CommtData.LastModified = DateTime.Now;
                _db.SaveChanges();
            }
            else
            {
                EmployeePerformanceGoalComment model = new EmployeePerformanceGoalComment();
                model.Description = Comment;
                model.PerformanceGoalId = GoalId;
                model.UserIDCreatedBy = SessionProxy.UserId;
                model.CreatedDate = DateTime.Now;
                model.Archived = false;
                model.CreatedName = Convert.ToString(SessionProxy.UserId);
                _db.EmployeePerformanceGoalComments.Add(model);
                _db.SaveChanges();
            }
            return true;
        }
        public List<EmployeePerformanceGoalComment> getAllGoalCommnet(int goalId)
        {
            return _db.EmployeePerformanceGoalComments.Where(x => x.Archived == false && x.PerformanceGoalId == goalId).ToList();
        }

        public bool UpdateGraphDropValue(int Id, string value_x, string value_y, string UnitValue)
        {
            var goalData = _db.EmployeePerformanceGoals.Where(x => x.Id == Id && x.Archived == false).FirstOrDefault();
            goalData.GoalValueX = Convert.ToDecimal(value_x);
            goalData.GoalValueY = Convert.ToDecimal(value_y);
            goalData.Unit = UnitValue;
            goalData.LastModifiedDate = DateTime.Now;
            _db.SaveChanges();
            return true;
        }

        public bool DeleteEmployeePerformanceReview(int EmpPerfReviewID, int ReviewId, int EmpId)
        {
            //var DataEmployeePerformace = _db.PerformanceSettings.Where(x => x.Id == ReviewId && x.Archived == false).FirstOrDefault();
            //if (DataEmployeePerformace.CompletionDate != null && DataEmployeePerformace.AnnualReview == false)
            //{
            //    DataEmployeePerformace.CompletionDate = null;
            //    _db.Entry(DataEmployeePerformace).State = System.Data.Entity.EntityState.Modified;

            //}
            var EmployeePerData = _db.EmployeePerformances.Where(x => x.ReviewId == ReviewId && x.EmployeeId == EmpId && x.Archived == false).FirstOrDefault();
            EmployeePerData.Archived = true;
            EmployeePerData.LastModifiedDate = DateTime.Now;
            EmployeePerData.UserIDLastModifiedBy = SessionProxy.UserId;
            List<PerformanceEmployeeDetail> performanceEmployeeDetail = (from record in _db.PerformanceEmployeeDetails where record.ReviewID == ReviewId && record.EmployeeId == EmpId select record).ToList();
            if (performanceEmployeeDetail != null)
            {
                _db.PerformanceEmployeeDetails.RemoveRange(performanceEmployeeDetail);
            }
            List<EmployeePerformanceCoreJSONDetail> employeePerformanceCoreJSONDetail = (from record in _db.EmployeePerformanceCoreJSONDetails where record.ReviewID == ReviewId && record.EmployeeID == EmpId select record).ToList();

            if (employeePerformanceCoreJSONDetail != null)
            {
                foreach (var items in employeePerformanceCoreJSONDetail)
                {
                    items.Archived = true;
                    items.UserIdLastModifiedBy = SessionProxy.UserId;
                    items.UserIdLastModifiedDate = DateTime.Now;
                }

            }
            List<EmployeePerformanceJobRoleSegmentJSONDetail> employeePerformanceJobRoleSegmentJSONDetail = (from record in _db.EmployeePerformanceJobRoleSegmentJSONDetails where record.ReviewID == ReviewId && record.EmployeeID == EmpId select record).ToList();
            if (employeePerformanceJobRoleSegmentJSONDetail != null)
            {
                foreach (var items in employeePerformanceJobRoleSegmentJSONDetail)
                {
                    items.Archived = true;
                    items.LastModifiedBy = SessionProxy.UserId;
                    items.LastModifiedDate = DateTime.Now;
                }

            }
            List<SendReviewToCoworker> SendReviewToCoworker = (from record in _db.SendReviewToCoworkers where record.ReviewID == ReviewId && record.EmployeeID == EmpId select record).ToList();
            if (SendReviewToCoworker != null)
            {
                _db.SendReviewToCoworkers.RemoveRange(SendReviewToCoworker);
            }
            //for coworker delete for resource            
            List<PerformanceCoWorkerDetail> PerformanceCoWorkerDetails = (from record in _db.PerformanceCoWorkerDetails where record.EmployeeID == EmpId select record).ToList();
            if (PerformanceCoWorkerDetails != null)
            {
                _db.PerformanceCoWorkerDetails.RemoveRange(PerformanceCoWorkerDetails);
            }
            //for goal remove 
            List<EmployeePerformanceGoal> EmployeePerformanceGoals = (from record in _db.EmployeePerformanceGoals where record.EmployeeId == EmpId && record.Archived == false select record).ToList();
            if (EmployeePerformanceGoals != null)
            {
                _db.EmployeePerformanceGoals.RemoveRange(EmployeePerformanceGoals);
            }
            _db.SaveChanges();
            return true;
        }
    }

}