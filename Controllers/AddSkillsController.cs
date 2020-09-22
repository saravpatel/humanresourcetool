using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using HRTool.CommanMethods;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class AddSkillsController : Controller
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        AddSkillsMethod _addSkillsMethod = new AddSkillsMethod();

        #endregion

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public List<AddSkillsViewModel> returnList(string SkillType)
        {
            List<AddSkillsViewModel> model = new List<AddSkillsViewModel>();
            List<SystemListValue> data = new List<SystemListValue>();

            if (SkillType == "Technical")
            {
                data = _otherSettingMethod.getAllSystemValueListByKeyName("Technical Skills");
            }
            else
            {
                data = _otherSettingMethod.getAllSystemValueListByKeyName("General Skills");
            }

            foreach (var item in data)
            {
                AddSkillsViewModel addSkillsViewModel = new AddSkillsViewModel();
                addSkillsViewModel.Id = item.Id;
                addSkillsViewModel.Value = item.Value;
                addSkillsViewModel.Description = item.Description;
                addSkillsViewModel.SystemListID = item.SystemListID;
                model.Add(addSkillsViewModel);
            }
            return model;
        }

        public ActionResult List(string SkillType)
        {
            List<AddSkillsViewModel> model = returnList(SkillType);
            return PartialView("_partialAddSkillsList", model);
        }

        public ActionResult AddEditAddSkills(int Id)
        {
            AddSkillsViewModel model = new AddSkillsViewModel();
            model.Id = Id;
            if (Id > 0)
            {
                var SystemListValue = _otherSettingMethod.getSystemListValueById(Id);
                model.Value = SystemListValue.Value;
                model.Archived = (bool)SystemListValue.Archived;
                model.Description = SystemListValue.Description;
                model.SystemListID = SystemListValue.SystemListID;
            }
            return PartialView("_partialAddAddSkills", model);
        }


        public ActionResult SaveAddSkills(int Id, string Value, string Description, string SkillType)
        {
            _addSkillsMethod.SaveSkills(Id, Value, Description, SkillType, SessionProxy.UserId);
            List<AddSkillsViewModel> model = returnList(SkillType);
            return PartialView("_partialAddSkillsList", model);
        }

        public ActionResult ExportExcel(string SkillType)
        {
            List<AddSkillsViewModel> model = returnList(SkillType);
            DataTable dttable = new DataTable("Skill");
            dttable.Columns.Add("Name", typeof(string));
            dttable.Columns.Add("Description", typeof(string));

            foreach (var item in model)
            {
                List<string> lstStrRow = new List<string>();
                lstStrRow.Add(item.Value);
                lstStrRow.Add(item.Description);
                string[] newArray = lstStrRow.ToArray();
                dttable.Rows.Add(newArray);
            }




            #region Export file
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dttable);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= " + SkillType + "_Skills.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            #endregion
            return View();
        }

    }
}