using HRTool.DataModel;
using HRTool.Models.RolesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace HRTool.CommanMethods.RolesManagement
{
    public class RolesManagementMethod
    {
        #region constant
        EvolutionEntities _db = new EvolutionEntities();
        #endregion
        public string createBreadCum(List<Menu_List> data, int menuId, string breadCum)
        {
            var currentRecord = data.Where(x => x.ID == menuId).FirstOrDefault();
            Menu_List parentRecord = null;
            if (currentRecord.SubmenuID > 0)
            {
                parentRecord = data.Where(x => x.ID == currentRecord.SubmenuID).FirstOrDefault();
            }
            if (string.IsNullOrEmpty(breadCum))
            {
                if (parentRecord != null)
                {
                    breadCum = string.Format("{0} >> {1}", parentRecord.MenuName, currentRecord.MenuName);
                }
            }
            else
            {
                breadCum = string.Format("{0} >> {1}", parentRecord.MenuName, breadCum);
            }

            if (parentRecord != null)
            {
                if (parentRecord.SubmenuID > 0)
                {
                    breadCum = createBreadCum(data, parentRecord.ID, breadCum);
                }
            }

            return breadCum;
        }
        public string createParentBreadCum(List<Menu_List> data, int menuId, string breadCum)
        {
            var currentRecord = data.Where(x => x.ID == menuId).FirstOrDefault();
            Menu_List parentRecord = null;
            if (currentRecord.SubmenuID > 0)
            {
                parentRecord = data.Where(x => x.ID == currentRecord.SubmenuID).FirstOrDefault();
            }
            if (string.IsNullOrEmpty(breadCum))
            {
                if (parentRecord != null)
                {
                    breadCum = string.Format("{0} >> {1}", parentRecord.MenuName, currentRecord.MenuName);
                }
                else
                {
                    breadCum = currentRecord.MenuName;
                }
            }
            else
            {
                breadCum = string.Format("{0} >> {1}", parentRecord.MenuName, breadCum);
            }

            if (parentRecord != null)
            {
                if (parentRecord.SubmenuID > 0)
                {
                    breadCum = createParentBreadCum(data, parentRecord.ID, breadCum);
                }
            }
            return breadCum;
        }
        public List<SelectListItem> bindMenuDropDown()
        {
            List<SelectListItem> MenuList = new List<SelectListItem>();
            var menus = (from i in _db.Menu_List
                         select i).ToList();
            MenuList.Add(new SelectListItem { Text = "-- Select --", Value = "0" });
            foreach (var item in menus.Where(x => x.SubmenuID == 0))
            {
                MenuList.Add(new SelectListItem { Text = createParentBreadCum(menus, item.ID, ""), Value = item.ID.ToString() });
            }
            return MenuList;
        }
        public List<SelectListItem> BindMenuKey()
        {
            List<SelectListItem> MenuKeyList = new List<SelectListItem>();
            string[] enumNames = System.Enum.GetNames(typeof(Menukey));
            foreach (string item in enumNames)
            {
                //get the enum item value
                int value = (int)Enum.Parse(typeof(Menukey), item);
                MenuKeyList.Add(new SelectListItem { Text = item, Value = value.ToString() });
            }
            return MenuKeyList;
        }

        public RolesManagementMenuViewModel BindMenuListModelById(int Id)
        {
            RolesManagementMenuViewModel model = new RolesManagementMenuViewModel();
            var menu = _db.Menu_List.Where(x => x.ID == Id).FirstOrDefault();
            model.Id = menu.ID;
            model.MenuName = menu.MenuName;
            model.ActionName = menu.ActionName;
            model.ControllerName = menu.ControllerName;
            model.SubmenuId = Convert.ToInt32(menu.SubmenuID);
            return model;
        }
        public void UpdateRecord(RolesManagementMenuViewModel model)
        {
            Menu_List menu = _db.Menu_List.Where(x => x.ID == model.Id).FirstOrDefault();
            menu.MenuName = model.MenuName;
            menu.ActionName = model.ActionName;
            menu.ControllerName = model.ControllerName;
            menu.SubmenuID = model.SubmenuId;
            //menu.CreatedBy = model.CreatedBy;
            menu.Createdate = DateTime.Now;
            _db.SaveChanges();
        }

        /// <summary>
        /// insert new record in table
        /// </summary>
        /// <param name="model">pass all menu model</param>
        /// <returns>last inserted recod ID</returns>
        public int InsertRecord(RolesManagementMenuViewModel model)
        {
            Menu_List menu = new Menu_List();
            menu.MenuName = model.MenuName;
            menu.ActionName = model.ActionName;
            menu.ControllerName = model.ControllerName;
            menu.SubmenuID = model.SubmenuId;
            menu.DisplayOrder = lastDisplayOrder(model.SubmenuId);
            menu.Createdate = DateTime.Now;
            menu.CreatedBy = model.CreatedBy;
            menu.MenuKey = model.MenuKeyValues;
            _db.Menu_List.Add(menu);
            _db.SaveChanges();
            return menu.ID;
        }
        public int lastDisplayOrder(int parentMenuId)
        {
            var menuId = _db.Menu_List.Where(x => x.SubmenuID == parentMenuId).OrderByDescending(x => x.DisplayOrder).FirstOrDefault();
            if (menuId != null)
            {
                return Convert.ToInt32(menuId.DisplayOrder + 1);
            }
            else
            {
                return Convert.ToInt32(1);
            }

        }
        public UserMenu getUserMenu(string Action, string controller, int userId)
        {
            UserMenu umenu = null;
            Menu_List menu = _db.Menu_List.Where(x => x.ActionName.ToLower() == Action.ToLower() && x.ControllerName.ToLower() == controller.ToLower()).FirstOrDefault();
            if (menu != null)
            {
                umenu = _db.UserMenus.Where(x => x.MenuID == menu.ID && x.UserID == userId).FirstOrDefault();
            }
            return umenu;
        }

        public void Deletemenu(int Id)
        {
            Menu_List Menu = _db.Menu_List.Where(x => x.ID == Id).FirstOrDefault();
            _db.Menu_List.Remove(Menu);
            _db.SaveChanges();
        }

        #region CreateRole

        public IList<AspNetRole> GetAllRoles()
        {
            return _db.AspNetRoles.ToList();
        }

        public void SaveRoleData(CreateRoleViewModel model)
        {
            List<int> selectedList = new List<int>();
            if (!string.IsNullOrEmpty(model.SelectedList))
            {
                foreach (var item in model.SelectedList.Split(','))
                {
                    selectedList.Add(Convert.ToInt32(item));
                }
            }
            if (model.Id > 0)
            {
                AspNetRole role = _db.AspNetRoles.Where(x => x.Id == model.Id).FirstOrDefault();
                role.Name = model.Name;
                role.Description = model.Description;
                role.Active = model.Active;
                _db.SaveChanges();
                var roleMenu = _db.Role_DefaultMenu.Where(x => x.RoleId == model.Id).ToList();
                foreach (var item in roleMenu)
                {
                    Role_DefaultMenu delete = roleMenu.Where(x => x.Id == item.Id).FirstOrDefault();
                    _db.Role_DefaultMenu.Remove(delete);
                    _db.SaveChanges();
                }

                foreach (var item in selectedList)
                {
                    Role_DefaultMenu defaultMenu = new Role_DefaultMenu();
                    var menuKey = _db.Menu_List.Where(x => x.ID == item).FirstOrDefault().ID;
                    defaultMenu.RoleId = role.Id;
                    defaultMenu.MenuKey = menuKey;
                    _db.Role_DefaultMenu.Add(defaultMenu);
                    _db.SaveChanges();
                }
            }
            else
            {
                AspNetRole role = new AspNetRole();
                role.Name = model.Name;
                role.Description = model.Description;
                role.Active = model.Active;
                _db.AspNetRoles.Add(role);
                _db.SaveChanges();

                foreach (var item in selectedList)
                {
                    Role_DefaultMenu defaultMenu = new Role_DefaultMenu();
                    var menuKey = _db.Menu_List.Where(x => x.ID == item).FirstOrDefault().ID;
                    defaultMenu.RoleId = role.Id;
                    defaultMenu.MenuKey = menuKey;
                    _db.Role_DefaultMenu.Add(defaultMenu);
                    _db.SaveChanges();
                }
            }
        }

        public List<Role_DefaultMenu> getDefaultMenuByRoleId(int roleId)
        {
            return _db.Role_DefaultMenu.Where(x => x.RoleId == roleId).ToList();
        }

        #endregion

        #region Employee ,Manager, Worker  List 
        public List<AspNetUser> GetEmployeeManagerList()
        {
            var userroleId = _db.AspNetRoles.Where(x => x.Name == "Employee" && x.Active == true).FirstOrDefault();
            var managerId = _db.AspNetRoles.Where(x => x.Name == "Manager" && x.Active == true).FirstOrDefault();
            var employeelist = _db.GetUserRoleList(userroleId.Id).ToList();
            var managerList = _db.GetUserRoleList(managerId.Id).ToList();
            List<AspNetUser> model = new List<AspNetUser>();
            foreach(var item in employeelist)
            {
                AspNetUser mm = new AspNetUser();
                var data = _db.AspNetUsers.Where(x => x.Id == item.UserId && x.SSOID.StartsWith("W") && x.Archived==false).FirstOrDefault();
                if (data != null)
                {
                    mm.Id = data.Id;
                    mm.FirstName = data.FirstName;
                    mm.LastName = data.LastName;
                    mm.SSOID = data.SSOID;
                    model.Add(mm);
                }

            }
            foreach (var item in managerList)
            {
                AspNetUser mm = new AspNetUser();
                var data = _db.AspNetUsers.Where(x => x.Id == item.UserId && x.SSOID.StartsWith("W") && x.Archived==false).FirstOrDefault();
                if (data != null)
                {
                    mm.Id = data.Id;
                    mm.FirstName = data.FirstName;
                    mm.LastName = data.LastName;
                    mm.SSOID = data.SSOID;
                    model.Add(mm);
                }
            }
            return model;
        }

        public List<AspNetUser> GetEmployeesList()
        {
            var userroleId = _db.AspNetRoles.Where(x => x.Name == "Employee" && x.Active == true).FirstOrDefault();
            var employeelist = _db.GetUserRoleList(userroleId.Id).ToList();
            List<AspNetUser> model = new List<AspNetUser>();
            foreach (var item in employeelist)
            {
                var data = _db.AspNetUsers.Where(x => x.Id == item.UserId && x.Archived == false).FirstOrDefault();
                if (data.SSOID.StartsWith("W"))
                {
                    AspNetUser mm = new AspNetUser();
                    mm.Id = data.Id;
                    mm.FirstName = data.FirstName;
                    mm.LastName = data.LastName;
                    mm.SSOID = data.SSOID;
                    model.Add(mm);
                }
            }
            return model;
        }
        public List<AspNetUser> GetManagersList()
        {
            List<AspNetUser> model = new List<AspNetUser>();
            var manager = _db.AspNetRoles.Where(x => x.Name == "Manager" && x.Active == true).FirstOrDefault();
            if (manager != null)
            {
                var managerList = _db.GetUserRoleList(manager.Id).ToList();
                foreach (var item in managerList)
                {
                    
                    var data = _db.AspNetUsers.Where(x => x.Id == item.UserId && x.Archived == false).FirstOrDefault();
                    if(data != null)
                    {
                        if (data.SSOID.StartsWith("W"))
                        {
                            AspNetUser mm = new AspNetUser();
                            mm.Id = data.Id;
                            mm.FirstName = data.FirstName;
                            mm.LastName = data.LastName;
                            mm.SSOID = data.SSOID;
                            model.Add(mm);
                        }
                    }
                }
            }
            return model;
        }

        public List<AspNetUser> GetCustomerList()
        {
            var userroleId = _db.AspNetRoles.Where(x => x.Name == "Employee" && x.Active == true).FirstOrDefault();
            var employeelist = _db.GetUserRoleList(userroleId.Id).ToList();
            List<AspNetUser> model = new List<AspNetUser>();
            foreach (var item in employeelist)
            {
                var data = _db.AspNetUsers.Where(x => x.Id == item.UserId && x.Archived ==false).FirstOrDefault();
                if (data.SSOID.Contains("C"))
                {
                    AspNetUser mm = new AspNetUser();
                    mm.Id = data.Id;
                    mm.FirstName = data.FirstName;
                    mm.LastName = data.LastName;
                    mm.SSOID = data.SSOID;
                    model.Add(mm);
                }

            }
            return model;
        }

        public string GetLoginUserRoleType(int UserId) 
        {
            var data=_db.GetLoginUserRoleType(UserId).FirstOrDefault();
            if (data != null)
            {
                return data.Name;
            }
            return null;
        }

        public int GetSuperAdminId()
        {
            
            var userroleId = _db.AspNetRoles.Where(x => x.Name == "SuperAdmin" && x.Active == true).FirstOrDefault();
            var employeelist = _db.GetUserRoleList(userroleId.Id).ToList();
            if (employeelist.Count > 0)
            {
                return employeelist[0].UserId.Value;
            }
            return 0;
        }

        #endregion
    }
}