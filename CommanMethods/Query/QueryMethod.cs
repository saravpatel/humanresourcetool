using HRTool.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRTool.Models.Query;
using HRTool.CommanMethods.Admin;

namespace HRTool.CommanMethods.Query
{
    public class QueryMethod
    {
        EvolutionEntities _db = new EvolutionEntities();

        public int saveQueryData(QueryDataSet model)
        {
            QueryData query = new QueryData();
            query.Name = model.QueryName;
            query.Description = model.QueryDescription;
            query.QueryText = model.QueryText;
            query.Archived = false;
            query.UserIDCreatedBy = SessionProxy.UserId;
            query.UserIDLastModifiedBy = SessionProxy.UserId;
            query.CreatedDate = DateTime.Now;
            query.LastModified = DateTime.Now;
            _db.QueryDatas.Add(query);
            _db.SaveChanges();
            return 0;
        }
       
    }
}