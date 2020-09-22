using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.Models.Notification
{
    public class TravelApproveViewModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string FromCountry { get; set; }
        public string ToCountry { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public string FromPlace { get; set; }
        public string ToPlace { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<decimal> Duration { get; set; }
        public string FirstName { get; set; }
        public string Name { get; set; }
        public string CostCode { get; set; }
        public string status { get; set; }
    }
    public class AllNotificationList
    {
        public long id { get; set; }
        public int? EmployeeId { get; set; }
        public string Header { get; set; }
        public string Name { get; set; }
        public int Detailid { get; set; }
        public string Title { get; set; }
        public string Initials { get; set; }
        public string ImagePath { get; set; }
        public string ApproveStatus { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string vacancyLink { get; set; }

        public string applicantName { get; set; }
    }
    public class AllNotificationDetail
    {
        public string eid { get; set; }
        public string name { get; set; }
        public int tid { get; set; }
        public string Customer { get; set; }
        public string Day { get; set; }
        public string Date { get; set; }
        public string InTime { get; set; }
        public string EndTime { get; set; }
        public string Hours { get; set; }
        public string CostCode { get; set; }
        public string Project { get; set; }
        public string AssetName { get; set; }
        public string Status { get; set; }
        public int DetailId { get; set; }
    }


}