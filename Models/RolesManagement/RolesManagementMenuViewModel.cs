using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.RolesManagement
{
    public class RolesManagementMenuViewModel
    {

        public RolesManagementMenuViewModel()
        {
            TableHeader = new List<string>();
            RoleListModelList = new List<RolesManagementMenuViewModel>();
            MenuList = new List<SelectListItem>();
            MenuKeyList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        [Required]
        public string MenuName { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string parenCategoryName { get; set; }
        public int SubmenuId { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string SuccessMessage { get; set; }
        public IList<string> TableHeader { get; set; }
        public IList<RolesManagementMenuViewModel> RoleListModelList { get; set; }
        public IList<SelectListItem> MenuList { get; set; }

        public IList<SelectListItem> MenuKeyList { get; set; }
        public bool isActive { get; set; }
        public List<string> BindSearchCharacter { get; set; }
        public string selectedSearchChar { get; set; }

        public int MenuKeyValues { get; set; }
    }

}