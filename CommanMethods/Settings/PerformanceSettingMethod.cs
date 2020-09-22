using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRTool.DataModel;
using System.Web.Script.Serialization;
using HRTool.Models.Settings;

namespace HRTool.CommanMethods.Settings
{
    public class PerformanceSettingMethod
    {
        EvolutionEntities _db = new EvolutionEntities();
        public int getAllOpenReviewThisYear(int ReviewID)
        {
            int count = 0;
            count = _db.PerformanceSettings.Where(x => x.CompletionDate.Value.Year == DateTime.Now.Year).Count();
           return count;
        }
        public int getAllCompletedReviewThisYear(int ReviewID)
        {
            int count = 0;            
            count = _db.PerformanceSettings.Where(x => x.Archived == true && x.CompletionDate.Value.Year == DateTime.Now.Year).Count();            
            return count;
        }
        public int getAllOpenReview(int ReviewID)
        {
            int count = 0;
            if(ReviewID!=0)
            {
                var data = _db.PerformanceSettings.Where(x => x.Id == ReviewID && x.Archived == false).FirstOrDefault();
                if(data.AnnualReview==true)
                {
                    count = _db.PerformanceSettings.Where(x => x.Archived == false && x.Id == ReviewID && x.CompletionDate > DateTime.Now).Count();
                }
            }
            count = _db.PerformanceSettings.Where(x => x.Archived == false && x.CompletionDate > DateTime.Now).Count();
            //int countThisYearPerf = _db.PerformanceSettings.Where(x => x.CompletionDate.Value.Year == DateTime.Now.Year).Count();
            return count;
        }
        public double getTotalPerformanceIncrease()
        {
            
            int AllPerformance = _db.PerformanceSettings.Where(x => x.Archived == false).Count();            
            int countThisYearPerf = _db.PerformanceSettings.Where(x => x.CompletionDate.Value.Year == DateTime.Now.Year).Count();
            double Diff =Convert.ToDouble(countThisYearPerf) / Convert.ToDouble(AllPerformance) *100;                                    
            return Diff=Math.Ceiling(Diff);
        }
        public int getAllCloseReview()
        {
            var count = _db.PerformanceSettings.Where(x => x.CompletionDate < DateTime.Now).Count();
            return count;
        }
        public double getTotalReviewCompletedIncr()
        {
            var count = _db.PerformanceSettings.Where(x => x.CompletionDate < DateTime.Now).Count();
            var countThisYearPerfCompleted = _db.PerformanceSettings.Where(x => x.CompletionDate.Value.Year == DateTime.Now.Year).Count();
            double Diff = Convert.ToDouble(count) / Convert.ToDouble(countThisYearPerfCompleted) * 100;
            return Diff;
        }
        public int OustandingReviews(int ReviewId)
        {
            int count = 0;
            if(ReviewId!=0)
            {
                var data = _db.PerformanceSettings.Where(x => x.Id == ReviewId && x.Archived == false).FirstOrDefault();
                if(data.AnnualReview==true)
                {
                    count = _db.PerformanceSettings.Where(x => x.Archived == false && x.Id==ReviewId && x.CompletionDate > DateTime.Now).Count();
                }
                else
                {
                    count = _db.PerformanceSettings.Where(x => x.Archived == false && x.CompletionDate > DateTime.Now).Count();
                }                
            }
            else
            {
                count = _db.PerformanceSettings.Where(x => x.Archived == false && x.CompletionDate > DateTime.Now).Count();
            }            
            return count;
        }        
        public int GetAllCustomerOustanding(int ReviewId)
        {
            int count = 0;
            if(ReviewId!=0)
            {
                var data = _db.EmployeePerformances.Where(x => x.ReviewId == ReviewId).ToList();
                foreach (var item in data)
                {
                     count = count + _db.PerformanceCustomerDetails.Where(x => x.IsArchived == false && x.Performance_Id==item.Id).Count();
                }
            }
            else
            {
                count = _db.PerformanceCustomerDetails.Where(x => x.IsArchived == false).Count();
            }
            return count;
        }
        
        public int GetAllCoWorkerOutStanding(int ReviewId)
        {
            int count = 0;
            if (ReviewId != 0)
            {
                count = _db.EmployeePerformances.Where(x => x.ReviewId == ReviewId).Count();               
            }
            else
            {
                count = _db.EmployeePerformances.Where(x => x.ReviewStatus=="Open").Count();
            }
            return count;
        }

        public int GetAllManagerOutStandin(int ReviewId)
        {
            int count = 0;
            var data = _db.PerformanceEmployeeDetails.ToList();
            if(data != null)
            {
                if (data.Count() > 0)
                {
                    foreach (var item in data)
                    {
                        if (item.Performance_ManagerId != null)
                        {
                            count = count + 1;
                        }
                    }
                }
            }
            return count;
        }
        public int countquestionsByID(int ReviewId)
        {
            var performanceData = _db.PerformanceSettings.Where(x => x.Id == ReviewId).FirstOrDefault();
            int TotalcoreQue = 0,TotalCustomer=0,TotalCoworker=0;
            int TotaljobRole = 0;
            int total=0;
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<EditSegmentViewModel> model = new List<EditSegmentViewModel>();
            if(!string.IsNullOrEmpty(performanceData.CoreSegmentJSON))
            {
                model = js.Deserialize<List<EditSegmentViewModel>>(performanceData.CoreSegmentJSON);
                foreach(var item in model)
                {
                    item.CoreQueList = js.Deserialize<List<QuestionModel>>(item.QueationType);
                    TotalcoreQue = TotalcoreQue + item.CoreQueList.Count();
                }
            }
            if(!string.IsNullOrEmpty(performanceData.JobRoleSegmentJSON))
            {
                model = js.Deserialize<List<EditSegmentViewModel>>(performanceData.JobRoleSegmentJSON);
                foreach(var item in model)
                {
                    item.JobroleQueList = js.Deserialize<List<JobRoleQuestionModel>>(item.QueationType);
                    TotaljobRole = TotaljobRole + item.JobroleQueList.Count();
                }
            }
            if(!string.IsNullOrEmpty(performanceData.CustomerSegmentJSON))
            {
                model = js.Deserialize<List<EditSegmentViewModel>>(performanceData.CustomerSegmentJSON);
                foreach(var item in model)
                {
                    item.CustomerQueList = js.Deserialize<List<CustomerQuestionModel>>(item.QueationType);
                    TotalCustomer = TotalCustomer + item.CustomerQueList.Count();
                }
            }
            if(!string.IsNullOrEmpty(performanceData.CoWorkerSegmentJSON))
            {
                model = js.Deserialize<List<EditSegmentViewModel>>(performanceData.CoWorkerSegmentJSON);
                foreach(var item in model)
                {
                    item.CoreQueList = js.Deserialize<List<QuestionModel>>(item.QueationType);
                    TotalCoworker = TotalCoworker + item.CoreQueList.Count();
                }
            }
            total = TotalCoworker + TotalCustomer + TotaljobRole;
            return total;
        }
        public int CountTotalQuestionByReview(int ReviewId)
        {
            var data = _db.PerformanceSettings.Where(x => x.Id == ReviewId && x.Archived == false).FirstOrDefault();
            int TotalQuestion = 0,TotalcoreQuestion = 0,TotaljobroleQuestion = 0,TotalcustomerQuestion= 0,TotalcoworkerQuestion = 0;
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<EditSegmentViewModel> model = new List<EditSegmentViewModel>();
            if (!string.IsNullOrEmpty(data.CoreSegmentJSON))
            {
                model = js.Deserialize<List<EditSegmentViewModel>>(data.CoreSegmentJSON);
                foreach (var item in model)
                {
                    item.CoreQueList = js.Deserialize<List<QuestionModel>>(item.QueationType);
                    TotalcoreQuestion = TotalcoreQuestion + item.CoreQueList.Count();
                }
            }
            if(!string.IsNullOrEmpty(data.JobRoleSegmentJSON))
            {
                model = js.Deserialize<List<EditSegmentViewModel>>(data.JobRoleSegmentJSON);
                foreach(var item in model)
                {
                    item.JobroleQueList = js.Deserialize<List<JobRoleQuestionModel>>(item.QueationType);
                    TotaljobroleQuestion = TotaljobroleQuestion + item.JobroleQueList.Count();
                }
            }
            if(!string.IsNullOrEmpty(data.CustomerSegmentJSON))
            {
                model = js.Deserialize<List<EditSegmentViewModel>>(data.CustomerSegmentJSON);
                foreach(var item in model)
                {
                    item.CustomerQueList = js.Deserialize<List<CustomerQuestionModel>>(item.QueationType);
                    TotalcustomerQuestion = TotalcustomerQuestion + item.CustomerQueList.Count();
                }
            }
            if(!string.IsNullOrEmpty(data.CoreSegmentJSON))
            {
                model = js.Deserialize<List<EditSegmentViewModel>>(data.CoWorkerSegmentJSON);
                foreach(var item in model)
                {
                    item.CoreQueList = js.Deserialize<List<QuestionModel>>(item.QueationType);
                    TotalcoworkerQuestion = TotalcoworkerQuestion + item.CoreQueList.Count();
                }
            }
            TotalQuestion = TotalcoreQuestion + TotaljobroleQuestion + TotalcustomerQuestion + TotalcoworkerQuestion;
            return TotalQuestion;
        }
        public List<SystemListValue> getAllSystemValueListByKeyName(string KeyName)
        {
            SystemList systemName = getSystemListByName(KeyName);
            if (systemName != null)
            {
                return getAllSystemValueListByNameId(systemName.Id);
            }
            else
            {
                return null;
            }
        }
        public List<SystemListValue> getAllSystemValueListByNameId(int Id)
        {
            return _db.SystemListValues.Where(x => x.Archived == false && x.SystemListID == Id).ToList();
        }
        public SystemList getSystemListByName(string Name)
        {
            return _db.SystemLists.Where(x => x.SystemListName == Name).FirstOrDefault();
        }

        public List<GetAllOverAllScorePerformance_Result> GetAllOverAllScoreList()
        {
            return _db.GetAllOverAllScorePerformance().ToList();
        }

        public List<GetAllCoreScorePerformance_Result> GetAllCoreScoreList()
        {
            return _db.GetAllCoreScorePerformance().ToList();
        }

        public List<GetAllEmployeePearformanceDetail_Result> GetEmployeePerformancDetails()
        {
            return _db.GetAllEmployeePearformanceDetail().ToList();
        }
    }
}