using HRTool.CommanMethods.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Query
{
    public class QueryViewModel
    {
        public QueryViewModel()
        {
            TableNameList = new List<SelectListItem>();
            ColumnNameList = new List<SelectListItem>();
            QueryFilter = new List<QueryFilter>();
            MasterTable = new List<SelectListItem> ();
            BusinessList = new List<SelectListItem>();
            JobTitleList = new List<SelectListItem>();
            CustomerList = new List<SelectListItem>();
            ChildTableList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public int TableId { get; set; }
        public string DisplayName { get; set; }
        public string ColumnName { get; set; }
        public string QueryText { get; set; }
        public IList<SelectListItem> BusinessList { get; set; }
        public IList<SelectListItem> JobTitleList { get; set; }
        public IList<SelectListItem> CustomerList { get; set; }
        public IList<SelectListItem> TableNameList { get; set; }
        public IList<SelectListItem> ChildTableList { get; set; }
        public IList<SelectListItem> MasterTable { get; set; }
        public IList<SelectListItem> ColumnNameList { get; set; }
        public IList<QueryFilter> QueryFilter { get; set; }
        public Nullable<bool> IsDisplayAny { get; set; }
    }
    public class QueryFiledModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
   
        public Nullable<int> BussinessId  { get; set; }
        public Nullable<int> DivisionId { get; set; }
        public Nullable<int> PoolId { get; set; }
        public Nullable<int> FunctionId { get; set; }
        public Nullable<int> JobTitleId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string AllAnd { get; set; }
        public string AllOr { get; set; }
        public Nullable<int> selectTableId { get; set; }
        public string columnString { get; set; }
    }
    public class QueryDataSet
    {
        public QueryDataSet(){
            AllQueryData = new List<QueryDataSet>();
            }
        public int Id { get; set; }
        public string QueryName { get; set; }
        public string QueryDescription { get; set; }
        public string QueryText { get; set; }
        public IList<QueryDataSet> AllQueryData { get; set; }

    }
}
