using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.CommanMethods
{
    public class SessionProxy
    {
        private const string USERID = "UserID";
        private const string USERNAME = "UserName";
        private const string IMAGEURL = "ImageUrl";
        private const string ISCUSTOMER = "IsCustomer";
        public static int UserId
        {
            get
            {
                return HttpContext.Current.Session[USERID] != null ? Convert.ToInt32(HttpContext.Current.Session[USERID]) : 0;
            }
            set
            {
                HttpContext.Current.Session[USERID] = value;
            }
        }
        public static string UserName
        {
            get
            {
                return HttpContext.Current.Session[USERNAME] != null ? HttpContext.Current.Session[USERNAME].ToString() : string.Empty;
            }
            set
            {
                HttpContext.Current.Session[USERNAME] = value;
            }
        }

        public static string ImageUrl
        {
            get
            {
                return HttpContext.Current.Session[IMAGEURL] != null ? HttpContext.Current.Session[IMAGEURL].ToString() : string.Empty;
            }
            set
            {
                HttpContext.Current.Session[IMAGEURL] = value;
            }
        }
        public static bool IsCustomer
        {
            get
            {
                return HttpContext.Current.Session[ISCUSTOMER] != null ? Convert.ToBoolean(HttpContext.Current.Session[ISCUSTOMER]) : false;
            }
            set
            {
                HttpContext.Current.Session[ISCUSTOMER] = value;
            }
        }
    }
}