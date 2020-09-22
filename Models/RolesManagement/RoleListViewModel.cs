using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.Models.RolesManagement
{
    public class RoleListViewModel
    {
        public RoleListViewModel()
        {
            RoleList = new List<RoleListViewModel>();

            Userlist = new List<RoleListViewModel>();
        }
        public int roleId { get; set; }
        public string roleName { get; set; }
        public string username { get; set; }
        public string FullName { get; set; }
        public int userid { get; set; }
        public List<RoleListViewModel> RoleList { get; set; }
        public List<RoleListViewModel> Userlist { get; set; }

    }
}