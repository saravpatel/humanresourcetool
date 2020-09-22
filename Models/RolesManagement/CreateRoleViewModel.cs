using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.Models.RolesManagement
{
    public class CreateRoleViewModel
    {
        public CreateRoleViewModel()
        {
            menuList = new List<MenuRoleViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public IList<MenuRoleViewModel> menuList { get; set; }
        public string SelectedList { get; set; }
    }
}