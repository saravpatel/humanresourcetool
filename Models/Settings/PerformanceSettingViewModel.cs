using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using HRTool.CommanMethods;

namespace HRTool.Models.Settings
{
    public class PerformanceSettingViewModel
    {
        public PerformanceSettingViewModel()
        {
            CompanyList = new List<SelectListItem>();
            selectedCompany = new List<string>();
            LocationList = new List<SelectListItem>();
            selectedLocation = new List<string>();
            BusinessList = new List<SelectListItem>();
            selectedBusiness = new List<string>();
            DivisionList = new List<SelectListItem>();
            selectedDivision = new List<string>();
            PoolList = new List<SelectListItem>();
            selectedPoolList = new List<string>();
            FunctionList = new List<SelectListItem>();
            selectedFunction = new List<string>();
            JobTitleList = new List<SelectListItem>();
            selectedJobTitle = new List<string>();
            EmploymentList = new List<SelectListItem>();
            selectedEmployment = new List<string>();
            CopyFromList = new List<SelectListItem>();
            PerformanceTamalate = new List<SelectListItem>();
            OverallScoreJsonDetaillistList = new List<SelectListItem>();
        }

        public int Id { get; set; }
        public IList<SelectListItem> CompanyList { get; set; }
        public List<string> selectedCompany { get; set; }
        public IList<SelectListItem> LocationList { get; set; }
        public List<string> selectedLocation { get; set; }
        public IList<SelectListItem> BusinessList { get; set; }
        public List<string> selectedBusiness { get; set; }
        public IList<SelectListItem> DivisionList { get; set; }
        public List<string> selectedDivision { get; set; }
        public IList<SelectListItem> PoolList { get; set; }
        public List<string> selectedPoolList { get; set; }
        public IList<SelectListItem> FunctionList { get; set; }
        public List<string> selectedFunction { get; set; }
        public IList<SelectListItem> JobTitleList { get; set; }
        public List<string> selectedJobTitle { get; set; }
        public IList<SelectListItem> EmploymentList { get; set; }
        public List<string> selectedEmployment { get; set; }
        public IList<SelectListItem> CopyFromList { get; set; }
        public IList<SelectListItem> PerformanceTamalate { get; set; }
        public int Performanceid { get; set; }
        public string ReviewText { get; set; }
        public string CompletionDate { get; set; }
        public bool AnnualReview { get; set; }
        public string CompanyCSV { get; set; }
        public string LocationCSV { get; set; }
        public string JobRoleCSV { get; set; }
        public string EmploymentTypeCSV { get; set; }
        public string DivisionCSV { get; set; }
        public string BusinessCSV { get; set; }
        public string PoolCSV { get; set; }
        public string FunctionCSV { get; set; }
        public string OverallScoreJson { get; set; }
        public string CoreSegmentJSON { get; set; }
        public string JobRoleSegmentJSON { get; set; }
        public string CoWorkerSegmentJSON { get; set; }
        public string CustomerSegmentJSON { get; set; }
        public bool Archived { get; set; }
        public string UserIDCreatedBy { get; set; }
        public string UserIDLastModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
        public Nullable<bool> RatingOverAll { get; set; }
        public Nullable<bool> RatingCore { get; set; }
        public Nullable<bool> RatingJobRole { get; set; }
        public string CSUserLastModifiedBy_Id { get; set; }
        public int CurrentUserId { get; set; }
        public int CopyId { get; set; }

        public IList<string> OverallScoreJsonList { get; set; }
        public IList<SelectListItem> OverallScoreJsonDetaillistList { get; set; }
    }


    public class SegmentViewModel
    {
        public SegmentViewModel()
        {
            QuestionList = new List<string>();
            CoreQueList = new List<QuestionModel>();
            JobroleQueList = new List<JobRoleQuestionModel>();
            CustomerQueList = new List<CustomerQuestionModel>();
            CoworkerQueList = new List<CoworkerQuestionModel>();
            SegmentViewModelList = new List<SegmentViewModel>();
        }
        public bool isAddMode { get; set; }
        public int SegmentId { get; set; }
        public int Id { get; set; }
        public int Flag { get; set; }
        public string Title { get; set; }
        public int IsActivePastFlag { get; set; }
        public int IsJobRoleCustomer { get; set; }
        public int TotalCoreQuestionList { get; set; }
        public int TotalJobroleQuestionList { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public string ExistPerformanceId { get; set; }
        public string JsonQuestionString { get; set; }
        public string EmpPerDetailId { get; set; }
        public Nullable<int>TotalQueastion { get; set; }
        //public IList<string> QuestionList { get; set; }
        public IList<string> QuestionList { get; set; }
        
        public IList<JobRoleQuestionModel> JobroleQueList { get; set; }
        public List<KeyValue> OverallScoreList { get; set; }
        public List<KeyValue> CoreScoreList { get; set; }
        public List<KeyValue> JobRoleScoreList { get; set; }
        public IList<QuestionModel> CoreQueList { get; set; }
        public IList<CustomerQuestionModel> CustomerQueList { get; set; }
        public IList<CoworkerQuestionModel> CoworkerQueList { get; set; }
        public IList<SegmentViewModel> SegmentViewModelList { get; set; }
        public QuestionModel questionsModel { get; set; }

        public JobRoleQuestionModel jobroleQueModel { get; set; }
        public CustomerQuestionModel customerQuesModel { get; set; }
        public string JSONCustomerSegmentString { get; set; }
        public string OverAllScoreJSON { get; set; }
        public string OverAllScore { get; set; }
        public string CoreScore { get; set; }
        public string JobRoleScore { get; set; }
        public string performanceEmployeeDetailslist { get; set; }
        public string coreSegData { get; set; }
        public string jobRoleSegData { get; set; }

        public string coreSegOriginalData { get; set; }
        public string JobRoleOriginalData { get; set; }



    }
    public class QuestionModel
    {
        public string QueId { get; set; }
        public string questionData { get; set; }
        public string FiledId { get; set; }
        public string FiledText { get; set; }
        public string CValue { get; set; }
        public string Score { get; set; }
        public string QuFiledValue { get; set; }
        public string Comments { get; set; }

    }
    public class JobRoleQuestionModel
    {
        public string QueId { get; set; }
        public string questionData { get; set; }
        public string FiledId { get; set; }
        public string FiledText { get; set; }
        public string CValue { get; set; }
        public string Score { get; set; }
        public string QuFiledValue { get; set; }
        public string Comments { get; set; }

    }
    public class CustomerQuestionModel
    {
        public string QueId { get; set; }
        public string Score { get; set; }
        public string questionData { get; set; }
        public string FiledId { get; set; }
        public string FiledText { get; set; }
        public string CValue { get; set; }
        public string QuFiledValue { get; set; }
        public string Comments { get; set; }

    }
    public class CoworkerQuestionModel
    {
        public string QueId { get; set; }
        public string questionData { get; set; }
        public string FiledId { get; set; }
        public string FiledText { get; set; }
        public string CValue { get; set; }
        public string Comments { get; set; }
    }
    public class CoworkerQuestionModelForMe
    {
        public string QueId { get; set; }
        public int cororkerId { get; set; }
        public string questionData { get; set; }

        public string score { get; set; }
        public string FiledId { get; set; }
        public string FiledText { get; set; }
        public string CValue { get; set; }
        public string Comments { get; set; }
    }

    public class EditSegmentViewModel
    {
        public EditSegmentViewModel()
        {
            CoreFiledTypeList = new List<SelectListItem>();
            CoreValueList = new List<SelectListItem>();
            JobRoleFiledTypeList = new List<SelectListItem>();
            JobRoleValueList = new List<SelectListItem>();
            CoreQueList = new List<QuestionModel>();
            CustomerQueList = new List<CustomerQuestionModel>();
            JobRoleSeg = new List<JobRoleSegment>();
            CoreSeg = new List<CoreSegment>();
            CoworkerSegment = new List<CoworkerSegment>();
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public string JsonQuestionString { get; set; }
        public string QueationType { get; set; }
        public string TotalQueastion { get; set; }
        public string HelpText { get; set; }        
        public int PerCoworkerId { get; set; }
        public int QueId { get; set; }
        public int Flag { get; set; }
        public int IsManager_Id { get; set; }
        public int IsManagerEmployee { get; set; }
        public int CowoIds { get; set; }
        public Nullable<int> ReviewId { get; set; }
        public List<SelectListItem> CoreFiledTypeList { get; set; }
        public List<SelectListItem> CoreValueList { get; set; }
        public List<SelectListItem> JobRoleFiledTypeList { get; set; }
        public List<SelectListItem> JobRoleValueList { get; set; }
        public List<QuestionModel> CoreQueList { get;set;}
        public List<CustomerQuestionModel> CustomerQueList { get; set; }
        public List<JobRoleQuestionModel> JobroleQueList { get; set; }
        public List<CustomerSegment> CustomerSeg { get; set; }
        public List<CoworkerSegment> CoworkerSegment { get; set; }
        //public CoworkerSegment CoworkerSegmentData { get; set; }
        public List<CoreSegment> CoreSeg { get; set; }
        public List<JobRoleSegment> JobRoleSeg { get; set; }
        public int IsActivePastFlag { get; set; }
        public int IsCompleteSegment { get; set; }

        public int preWorkerID { get; set; }

    }
    public class CoworkerSegment
    {
        public CoworkerSegment()
        {
            questionDataList = new List<CoworkerSegmentQuetion>();
        }
        public int CororkerId { get; set; }
        public string questionData { get; set; }
        public string QueId { get; set; }
        public string FiledId { get; set; }
        public string FiledText { get; set; }
        public string CValue { get; set; }
        public string Score { get; set; }
        public string QuFiledValue { get; set; }

        public List<CoworkerSegmentQuetion> questionDataList { get; set; }
    }
    public class CoworkerSegmentQuetion
    {
        public string QueId { get; set; }
        public string Score { get; set; }
    }
    public class CoreSegment
    {
        public CoreSegment()
        {
            CoreQueListData = new List<QuestionModel>();
        }
        public int CoreId { get; set; }
        public string QueationType { get; set; }
        public string Title { get; set; }
        public List<QuestionModel> CoreQueListData { get; set; }
    }
    public class CustomerSegment
    {
        public CustomerSegment()
        {
            CustomerQueListData = new List<CustomerQuestionModel>();
        }
        public int CustoIds { get; set; }
        public string QueationType { get; set; }
        public string Title { get; set; }
        public List<CustomerQuestionModel> CustomerQueListData { get; set; }


    }
    public class JobRoleSegment
    {
        public JobRoleSegment()
        {
            JobRoleQueListData = new List<JobRoleQuestionModel>();
        }
        public int JobRoleIds { get; set; }
        public string QueationType { get; set; }
        public string Title { get; set; }
        public List<JobRoleQuestionModel> JobRoleQueListData { get; set; }

    }

    public class PrintPerformancePDF
    {
        public PrintPerformancePDF()
        {
            CoreQueList = new List<QuestionModel>();
            CoreSeg = new List<CoreSegment>();
            JobRoleSeg = new List<JobRoleSegment>();
        }
        public string ReviewName { get; set; }
        public string EmployeeName { get; set; }
        public string ManagerName { get; set; }
        public string CustomerName { get; set; }
        public DateTime ReviewComplitionDate { get; set; }
        public IList<QuestionModel> CoreQueList { get; set; }
        public IList<CoreSegment> CoreSeg { get; set; }
        public IList<JobRoleSegment> JobRoleSeg { get; set; }
    }
    public class ShareEmployeeReview
    {
       public int EmployeeId { get; set; }
       public int MaanegrId { get; set; }
       public string ManagerName { get; set; }
    }

    public class ObjectiveOfEmployeePerformance
    {
        public ObjectiveOfEmployeePerformance()
        {
            EmployeePerformanceGoalList = new List<ObjectiveOfEmployeePerformance>();
            Status = new List<SelectListItem>();
            DocumentList = new List<EmployeePerformanceGoalDocumentsViewModel>();
            CommentList = new List<EmployeePerformanceGoalCommentViewModel>();
        }
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int EmpPerformanceId { get; set; }
       public string GoalName { get; set; } 
       public string GoalDescription { get; set; } 
        public string DueDate { get; set; }
        public string StartDate { get; set; }
        public string GoalStatus { get; set; }
        public string CommnetId { get; set; }
        public string Description { get; set; }
        public string UnitPercent { get; set; }
        public string SelectedStatus { get; set; }
        public decimal GoalXValue { get; set; }
        public decimal GoalYValue { get; set; }

        public string CountTotalGoal { get; set; }
        public IList<SelectListItem> Status { get; set; }
        public IList<ObjectiveOfEmployeePerformance> EmployeePerformanceGoalList { get; set; }
        public IList<EmployeePerformanceGoalDocumentsViewModel> DocumentList { get; set; }
        public IList<EmployeePerformanceGoalCommentViewModel> CommentList { get; set; }

    }
    public class EmployeePerformanceGoalDocumentsViewModel
    {
        public int Id { get; set; }
        public int PerformanceGoalId { get; set; }
        public string originalName { get; set; }
        public string newName { get; set; }
        public string description { get; set; }
    }
    public class EmployeePerformanceGoalCommentViewModel
    {
        public int Id { get; set; }
        public int PerformanceGoalId { get; set; }
        public string comment { get; set; }
        public string commentBy { get; set; }
        public string commentTime { get; set; }
    }
}
