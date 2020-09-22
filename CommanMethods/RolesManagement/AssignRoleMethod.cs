using HRTool.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.CommanMethods.RolesManagement
{
    public class AssignRoleMethod
    {
        #region Const
        EvolutionEntities _db = new EvolutionEntities();
        #endregion
        public List<string> BindUserNamesDropdown()
        {
            List<string> model = new List<string>();
            var List = _db.AspNetUsers.Where(x => x.Archived == false).ToList();
            model.Add("-- Select User --");
            foreach (var item in List)
            {
                var value = String.Format("{0} {1} - {2}", item.FirstName, item.LastName, item.SSOID);
                model.Add(value);
            }
            return model;
        }

        public List<KeyValue> BindUserDropdown()
        {
            List<KeyValue> model = new List<KeyValue>();
            var List = _db.AspNetUsers.Where(x => x.Archived == false).ToList();

            model.Add(new KeyValue { Key = 0, Value = "-- Select User --" });
            foreach (var item in List)
            {
                var value = String.Format("{0} {1} - {2}", item.FirstName, item.LastName, item.SSOID);
                KeyValue _KeyValue = new KeyValue();
                _KeyValue.Key = item.Id;
                _KeyValue.Value = value;
                model.Add(_KeyValue);
            }
            return model;
        }

        public List<SelectListItem> BindUsersDropdown()
        {
            List<SelectListItem> model = new List<SelectListItem>();
            var List = _db.AspNetUsers.Where(x=>x.Archived == false).ToList();
            model.Add(new SelectListItem { Text = "-- Select User --", Value = "0" });
            foreach (var item in List)
            {
                model.Add(new SelectListItem { Text = String.Format("{0} {1} - {2}",item.FirstName,item.LastName,item.SSOID), Value = Convert.ToString(item.Id) });
            }
            return model;
        }

        public List<SelectListItem> BindRolesDropdown()
        {
            List<SelectListItem> model = new List<SelectListItem>();
            var List = _db.AspNetRoles.ToList();
            model.Add(new SelectListItem { Text = "-- Select Roles --", Value = "0" });
            foreach (var item in List)
            {
                if (item.Name != "SuperAdmin")
                {
                    model.Add(new SelectListItem { Text = item.Name, Value = Convert.ToString(item.Id) });
                }
            }
            return model;
        }
    }
}