using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.Models.RolesManagement
{
    public class AssignRoleViewModel
    {
        public AssignRoleViewModel()
        {
            UserList = new List<System.Web.Mvc.SelectListItem>();
            RoleList = new List<System.Web.Mvc.SelectListItem>();
            UserNameList = new List<string>();
        }
        public int userId { get; set; }
        public int RoleId { get; set; }
        public IList<System.Web.Mvc.SelectListItem> UserList { get; set; }
        public IList<string> UserNameList { get; set; }
        public IList<System.Web.Mvc.SelectListItem> RoleList { get; set; }
    }
}