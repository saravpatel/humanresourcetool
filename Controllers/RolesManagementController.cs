using HRTool.CommanMethods.RolesManagement;
using HRTool.DataModel;
using HRTool.Models.RolesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.UI.WebControls;
using HRTool.CommanMethods;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class RolesManagementController : Controller
    {

        #region Const
        RolesManagementMethod _RolesManagementMethod = new RolesManagementMethod();
        EvolutionEntities _db = new EvolutionEntities();
        List<Menu_List> menuModel = new List<Menu_List>();
        RoleListViewModel result = new RoleListViewModel();
        List<SelectListItem> isMenuCheacked = new List<SelectListItem>();
        AssignRoleMethod _AssignRoleMethod = new AssignRoleMethod();
        #endregion
        //
        // GET: /RolesManagement/

        #region Private method

        /// <summary>
        /// For binding Role Table 0th level
        /// </summary>
        /// <returns>list of menu table grouping with it's parent menu</returns>
        public List<Menu_List> setOrderby()
        {
            List<Menu_List> list = (from i in _db.Menu_List
                                    select i).ToList();

            foreach (var item in list.Where(x => x.SubmenuID == 0))
            {
                Menu_List m = new Menu_List();
                m = item;
                menuModel.Add(m);
                var subMenuCount = list.Where(x => x.SubmenuID == item.ID).ToList();
                if (subMenuCount.Count > 0)
                {
                    ListOfnestedLoop(list, (int)item.ID);
                }

            }

            return menuModel;
        }

        /// <summary>
        /// For binding (Add in model) Role Table nth level
        /// </summary>
        /// <returns></returns>
        public void ListOfnestedLoop(List<Menu_List> dbData, int SubmenuId)
        {
            foreach (var item in dbData.Where(x => x.SubmenuID == SubmenuId))
            {
                Menu_List m = new Menu_List();
                m = item;
                menuModel.Add(m);
                var subMenuCount = dbData.Where(x => x.SubmenuID == item.ID).ToList();
                if (subMenuCount.Count > 0)
                {
                    ListOfnestedLoop(dbData, (int)item.ID);
                }
            }
        }

        /// <summary>
        /// for binding string nth level
        /// </summary>
        /// <param name="data">table Data</param>
        /// <param name="menuId"></param>
        /// <param name="breadCum">last breadcum</param>
        /// <returns>return breducum string </returns>
        public string createMenuBreadCum(List<Menu_List> data, int menuId, string breadCum)
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
                    breadCum = createMenuBreadCum(data, parentRecord.ID, breadCum);
                }
            }

            return breadCum;
        }

        /// <summary>
        /// for uncheck chiild node
        /// </summary>
        /// <param name="data">list of child node</param>
        public void nestedMethodForChild(List<Menu_List> data)
        {
            foreach (var item in data)
            {
                isMenuCheacked.Add(new SelectListItem { Text = "un", Value = item.ID.ToString() });
                var isSubChild = _db.Menu_List.Where(x => x.SubmenuID == item.ID).ToList();
                if (isSubChild.Count > 0)
                {
                    nestedMethodForChild(isSubChild);
                }
            }
        }

        /// <summary>
        /// for ckeck parent node
        /// </summary>
        /// <param name="parentId">pass parentId</param>
        public void nestedMethodForParent(int parentId)
        {
            isMenuCheacked.Add(new SelectListItem { Text = "ck", Value = parentId.ToString() });
            int? ParentId = _db.Menu_List.Where(x => x.ID == parentId).FirstOrDefault().SubmenuID;
            if (ParentId > 0)
            {
                nestedMethodForParent(Convert.ToInt32(ParentId));
            }
        }

        #endregion

        #region User Wise Menu

        public ActionResult Index()
        {
            //int userId = SessionProxy.UserId;
            //var account = new AccountController();
            //var currentUserRole = account.UserManager.GetRoles(User.Identity.GetUserId()).FirstOrDefault();
            int userId = SessionProxy.UserId;
            string currentUserRole = _db.AspNetRoles.Where(x => x.AspNetUserRoles.Any(xx => xx.UserId == userId)).FirstOrDefault().Name;
            var result = _db.AspNetRoles.ToList();
            RoleListViewModel model = new RoleListViewModel();
            foreach (var item in result)
            {
                if (item.Name != "SuperAdmin")
                {
                    RoleListViewModel m = new RoleListViewModel();
                    m.roleId = item.Id;
                    m.roleName = item.Name;
                    model.RoleList.Add(m);
                }
            }
            return View(model);
        }

        /// <summary>
        /// for binding dropdown Role Wise user (Ajax Call from Index.cshtml)
        /// </summary>
        /// <param name="id">Role id</param>
        /// <returns>List of user</returns>
        public ActionResult roleWiseUsers(int id)
        {
            //Commited code by yagnik
            result.Userlist = (from c in _db.AspNetUsers.Where(x => x.AspNetUserRoles.Select(y => y.RoleId).Contains(id))
                               select new RoleListViewModel
                               {
                                   userid = c.Id,
                                   username = c.UserName,
                                   FullName = c.FirstName + " " + c.LastName + "-" + c.SSOID
                               }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GerRoleByUserID(int UserID)
        {
            try
            {
                if (UserID > 0)
                {
                    int? roleId = _db.AspNetUserRoles.Where(x => x.UserId == UserID).FirstOrDefault().RoleId;
                    return Json(roleId, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult EditAssignRole(int UserID,int roleID)
        {
            try
            {
                if(UserID > 0)
                {
                    AspNetUserRole UserRole = _db.AspNetUserRoles.Where(x => x.UserId == UserID).FirstOrDefault();
                    UserRole.RoleId = roleID;
                    _db.SaveChanges();
                    return Json("Role upldate successfully", JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// Ajax Call from index.cshtml for binding list of menu
        /// </summary>
        /// <returns>partial view to bind List of table</returns>
        public ActionResult menuBreadcum(string userName)
        {
            List<MenuRoleViewModel> menuRoleList = new List<MenuRoleViewModel>();
            List<Menu_List> mainMenu = setOrderby();
            foreach (var item in mainMenu)
            {
                MenuRoleViewModel model = new MenuRoleViewModel();
                model.menuId = item.ID;
                model.submenuId = item.SubmenuID;
                model.menuName = item.SubmenuID == 0 ? item.MenuName : createMenuBreadCum(mainMenu, item.ID, "");
                var id = userName.Split('-')[1].Trim();
                var User = _db.AspNetUsers.Where(x => x.SSOID == id).FirstOrDefault();
                var isAssign = _db.UserMenus.Where(x => x.MenuID == item.ID && x.UserID == User.Id).FirstOrDefault();
                model.isAlreadyAssign = isAssign != null ? true : false;
                menuRoleList.Add(model);
            }
            return PartialView("_PartialBindMenuListTable", menuRoleList);
        }

        /// <summary>
        /// for save data in userMenu table
        /// </summary>
        /// <param name="SelectedMenu">Selected Menu Ids</param>
        /// <param name="UserId">UserId</param>
        /// <returns></returns>
        public ActionResult saveMenuUserWise(string SelectedMenu, string userName)
        {
            List<int> selectedList = new List<int>();
            foreach (var item in SelectedMenu.Split(','))
            {
                selectedList.Add(Convert.ToInt32(item));
            }
            var id = userName.Split('-')[1].Trim();
            var UserData = _db.AspNetUsers.Where(x => x.SSOID == id).FirstOrDefault();

            var userMenuList = _db.UserMenus.Where(x => x.UserID == UserData.Id).ToList();

            foreach (var item in selectedList)
            {
                var isExist = userMenuList.Where(x => x.MenuID == item).FirstOrDefault();
                if (isExist == null)
                {
                    UserMenu userMenu = new UserMenu();
                    userMenu.MenuID = item;
                    userMenu.UserID = UserData.Id;
                    userMenu.CreatedDate = DateTime.Now;
                    userMenu.CreatedBy = SessionProxy.UserId;
                    userMenu.IsActive = true;
                    var menuKey = _db.Menu_List.Where(x => x.ID == item).FirstOrDefault().MenuKey;
                    userMenu.MenuKey = menuKey;
                    _db.UserMenus.Add(userMenu);
                    _db.SaveChanges();
                }
            }

            //List<UserMenu> extraUserMenuList = _db.UserMenu.Where(x => !selectedList.Any(p => p == x.MenuID) && x.UserID == UserData.Id).ToList();
            List<UserMenu> extraUserMenuList = _db.UserMenus.Where(x => !selectedList.Contains((int)x.MenuID) && x.UserID == UserData.Id).ToList();

            if (extraUserMenuList.Count > 0)
            {
                foreach (var item in extraUserMenuList)
                {
                    _db.UserMenus.Remove(item);
                    _db.SaveChanges();
                }
            }

            return Json("sucess", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// for getting list of check or uncheck Id (Ajax Call from _BindMenuTable.cshtml)
        /// </summary>
        /// <param name="MenuId">clicked menu ID</param>
        /// <returns>return select list item</returns>
        public ActionResult returnselectedOrunselectedMenuId(int MenuId)
        {
            //For Child member
            var isChild = _db.Menu_List.Where(x => x.SubmenuID == MenuId).ToList();
            foreach (var item in isChild)
            {
                isMenuCheacked.Add(new SelectListItem { Text = "un", Value = item.ID.ToString() });
                var isSubChild = _db.Menu_List.Where(x => x.SubmenuID == item.ID).ToList();
                if (isSubChild.Count > 0)
                {
                    nestedMethodForChild(isSubChild);
                }
            }

            //For Parent Member
            int? ParentId = _db.Menu_List.Where(x => x.ID == MenuId).FirstOrDefault().SubmenuID;
            if (ParentId > 0)
            {
                nestedMethodForParent(Convert.ToInt32(ParentId));
            }
            return Json(isMenuCheacked, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Add / Edit / List Menu

        [Authorize]
        public ActionResult MenuIndex()
        {
            RolesManagementMenuViewModel model = new RolesManagementMenuViewModel();
            if (TempData["successMessage"] != null)
            {
                model.SuccessMessage = TempData["successMessage"].ToString();
            }
            return View(model);
        }
        public ActionResult RolesList()
        {
            RolesManagementMenuViewModel model = new RolesManagementMenuViewModel();
            model.RoleListModelList = GetMenuList();
            return PartialView("_PartialMenuList", model);
        }
        public List<RolesManagementMenuViewModel> GetMenuList()
        {
            List<RolesManagementMenuViewModel> model = new List<RolesManagementMenuViewModel>();
            List<Menu_List> mainMenu = setOrderby();
            foreach (var item in mainMenu)
            {
                RolesManagementMenuViewModel m = new RolesManagementMenuViewModel();
                m.Id = item.ID;
                m.MenuName = item.SubmenuID == 0 ? item.MenuName : _RolesManagementMethod.createBreadCum(mainMenu, item.ID, "");
                m.ActionName = item.ActionName;
                m.ControllerName = item.ControllerName;
                // m.DisplayOrder = (int)item.DisplayOrder;
                m.parenCategoryName = item.SubmenuID == 0 ? "" : _RolesManagementMethod.createParentBreadCum(mainMenu, (int)item.SubmenuID, "");
                model.Add(m);
            }
            return model;

        }


        [Authorize]
        public ActionResult CreateMenu(int Id = 0)
        {
            RolesManagementMenuViewModel model = new RolesManagementMenuViewModel();
            if (Id > 0)
            {
                model = _RolesManagementMethod.BindMenuListModelById(Id);
            }
            model.MenuKeyList = _RolesManagementMethod.BindMenuKey();
            model.MenuList = _RolesManagementMethod.bindMenuDropDown();
            return PartialView("_PartialAddMenu", model);
        }

        [HttpPost]
        public ActionResult CreateMenu(RolesManagementMenuViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Id > 0)
                    {
                        _RolesManagementMethod.UpdateRecord(model);
                        model.RoleListModelList = GetMenuList();
                        return PartialView("_PartialMenuList", model);
                    }
                    else
                    {
                        model.CreatedBy = SessionProxy.UserId;
                        int LastInsertedId = _RolesManagementMethod.InsertRecord(model);
                        model.RoleListModelList = GetMenuList();
                        return PartialView("_PartialMenuList", model);
                    }
                }
                else
                {
                    model.MenuList = _RolesManagementMethod.bindMenuDropDown();
                    return View(model);
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }


        #endregion

        #region AssignRole

        [Authorize]
        public ActionResult AssignRole()
        {
            AssignRoleViewModel model = new AssignRoleViewModel();
            model.UserNameList = _AssignRoleMethod.BindUserNamesDropdown();
            model.UserList = _AssignRoleMethod.BindUsersDropdown();
            model.RoleList = _AssignRoleMethod.BindRolesDropdown();
            return View(model);
        }

        public ActionResult DDlUserName()
        {
            List<KeyValue> data = _AssignRoleMethod.BindUserDropdown();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AssignRole(AssignRoleViewModel model)
        {
            if (model.userId == 0)
            {
                ModelState.AddModelError("userId", "Please select user");
            }
            if (model.RoleId == 0)
            {
                ModelState.AddModelError("RoleId", "Please select Role");
            }
            if (ModelState.IsValid)
            {
                var id = model.userId;

                var userData = _db.AspNetUsers.Where(x => x.Id == id).FirstOrDefault();
                _db.UpdateUserRole(userData.Id, model.RoleId);

                var RoleData = _db.AspNetRoles.Where(x => x.Id == model.RoleId).FirstOrDefault();
                var menuList = _db.Menu_List.ToList();

                List<UserMenu> UserMenuList = _db.UserMenus.Where(x => x.UserID == userData.Id).ToList();
                if (UserMenuList.Count > 0)
                {
                    foreach (var item in UserMenuList)
                    {
                        _db.UserMenus.Remove(item);
                        _db.SaveChanges();
                    }
                }
                var defaultMenus = _RolesManagementMethod.getDefaultMenuByRoleId(model.RoleId);
                foreach (var item in defaultMenus)
                {
                    int menuId = 0;
                    var test = menuList.Where(x => x.ID == item.MenuKey);
                    if(test != null) { 
                    menuId = test.FirstOrDefault().ID;
                    }
                    //int menuId = menuList.Where(x => x.ID == item.MenuKey).FirstOrDefault().ID;
                    UserMenu userMenu = new UserMenu();
                    userMenu.MenuID = menuId;
                    userMenu.UserID = userData.Id;
                    userMenu.CreatedDate = DateTime.Now;
                    userMenu.CreatedBy = SessionProxy.UserId;
                    userMenu.IsActive = true;
                    userMenu.MenuKey = item.MenuKey;
                    _db.UserMenus.Add(userMenu);
                    _db.SaveChanges();
                }
                return RedirectToAction("AssignRole");
            }
            else
            {
                model.UserList = _AssignRoleMethod.BindUsersDropdown();
                model.RoleList = _AssignRoleMethod.BindRolesDropdown();
                return View(model);
            }
        }

        #endregion

        #region Create Role

        [Authorize]
        public ActionResult CreateRole()
        {
            return Index();
        }

        public List<CreateRoleViewModel> roleModelList()
        {
            List<CreateRoleViewModel> model = new List<CreateRoleViewModel>();
            var roles = _RolesManagementMethod.GetAllRoles();
            foreach (var item in roles)
            {
                if (item.Name != "SuperAdmin")
                {
                    CreateRoleViewModel m = new CreateRoleViewModel();
                    m.Id = item.Id;
                    m.Name = item.Name;
                    m.Description = item.Description;
                    m.Active = (bool)item.Active;
                    model.Add(m);
                }
            }
            return model;
        }

        public ActionResult CreateRoleList()
        {
            List<CreateRoleViewModel> model = roleModelList();
            return PartialView("_partialCreateRoleList", model);
        }
        [HttpPost]
        public ActionResult AddEditRole(int Id)
        {
            CreateRoleViewModel model = new CreateRoleViewModel();
            List<Menu_List> mainMenu = setOrderby();


            if (Id > 0)
            {
                var data = _db.AspNetRoles.Where(x => x.Id == Id).FirstOrDefault();
                model.Id = data.Id;
                model.Name = data.Name;
                model.Description = data.Description;
                model.Active = (bool)data.Active;

                var defaultMenu = _db.Role_DefaultMenu.Where(x => x.RoleId == data.Id).ToList();
                //  foreach (var item in mainMenu)
                List<Menu_List> mainMenuList = (from x in _db.Menu_List select x).ToList();
                foreach (var item in mainMenuList)              
                {
                    MenuRoleViewModel menuModel = new MenuRoleViewModel();
                    //var Menudata = _db.Menu_List.Where(x => x.ID == item.MenuKey).FirstOrDefault();
                    //if (Menudata != null && Menudata.SubmenuID == 0)
                    //{
                    //    menuModel.menuId = Menudata.ID;
                    //}
                    //else
                    //{
                    //    menuModel.submenuId = Menudata.ID;
                    //}
                    menuModel.menuId = item.ID;
                    menuModel.submenuId = item.SubmenuID;
                    menuModel.menuName = item.SubmenuID == 0 ? item.MenuName : createMenuBreadCum(mainMenu, item.ID, "");
                    var isAssign = defaultMenu.Where(x => x.MenuKey == item.ID).FirstOrDefault();
                    menuModel.isAlreadyAssign = isAssign != null ? true : false;
                    model.menuList.Add(menuModel);

                }
            }
            else
            {
                foreach (var item in mainMenu)
                {
                    MenuRoleViewModel menuModel = new MenuRoleViewModel();
                    menuModel.menuId = item.ID;
                    menuModel.submenuId = item.SubmenuID;
                    menuModel.menuName = item.SubmenuID == 0 ? item.MenuName : createMenuBreadCum(mainMenu, item.ID, "");
                    model.menuList.Add(menuModel);
                }
            }
            return PartialView("_partialAddCreateRole", model);
        }

        public ActionResult SaveRoleData(CreateRoleViewModel model)
        {
            _RolesManagementMethod.SaveRoleData(model);
            List<CreateRoleViewModel> modelList = roleModelList();
            return PartialView("_partialCreateRoleList", modelList);
        }
        #endregion
        public ActionResult DeleteMenuRecord(int Id)
        {
            _RolesManagementMethod.Deletemenu(Id);
            RolesManagementMenuViewModel model = new RolesManagementMenuViewModel();
            model.RoleListModelList = GetMenuList();
            return PartialView("_PartialMenuList", model);
        }
    }

    public class MenuKeyWithMenuId
    {
        public int MenuId { get; set; }
        public Nullable<int> MenuKey { get; set; }
    }
}