using System;
using HRTool.DataModel;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.CommanMethods.OrganizationChart
{
    public class OrganizationChartMethod
    {
        EvolutionEntities _db = new EvolutionEntities();
        public List<GetOrganizationDetails_Result> GetDetails()
        {
            return _db.GetOrganizationDetails().ToList();
        }

        public List<GetBusinessOrgChart_Result> GetBusinessDetails(int BusiID,int DivID,int PoolID, int FunID,int EmpTypeId)
        {
            return _db.GetBusinessOrgChart(BusiID, DivID, PoolID, FunID, EmpTypeId).ToList();
        }
        public int getTotalWorkingDayInfo(int EmployeeId)
        {
            int count = _db.GetLengthOfEmployment(EmployeeId).FirstOrDefault() != null && _db.GetLengthOfEmployment(EmployeeId).FirstOrDefault() > 0 ? _db.GetLengthOfEmployment(EmployeeId).FirstOrDefault().Value : 0;
            return count;
        }
    }
}