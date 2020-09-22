using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.Models.RolesManagement
{
    public class MenuRoleViewModel
    {
        public MenuRoleViewModel()
        {
            menulist = new List<MenuRoleViewModel>();
        }
        public int menuId { get; set; }
        public string menuName { get; set; }
        public string actionName { get; set; }
        public string controllerName { get; set; }
        public int DisplayOrder { get; set; }
        public Nullable<int> submenuId { get; set; }
        public List<MenuRoleViewModel> menulist { get; set; }
        public bool isAlreadyAssign { get; set; }
        public bool IsActive { get; set; }
        public int PendingLeave { get; set; }
    }
}