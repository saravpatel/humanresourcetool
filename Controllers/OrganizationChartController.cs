using HRTool.Models.OrganizationChart;
using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRTool.CommanMethods.OrganizationChart;
using HRTool.CommanMethods;
using Rotativa;
using Rotativa.Options;
namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class OrganizationChartController : Controller
    {
        EvolutionEntities _db = new EvolutionEntities();
        CompanyStructureMethod _CompanyStructureMethod = new CompanyStructureMethod();
        OrganizationChartMethod _OrgChartMethod = new OrganizationChartMethod();
        // GET: OrganizationChart
        public ActionResult Index()
        {
            OrganizationChartViewModel datamodel = new OrganizationChartViewModel();
            var BUsinessList = _CompanyStructureMethod.getAllBusinessList();
            //datamodel.BusinessList.Add(new SelectListItem() { Text = "-- Select Project --", Value = "0" });
            foreach (var item in BUsinessList)
            {
                datamodel.BusinessList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            var ResourceType = _CompanyStructureMethod.getAllSystemValueListByKeyName("Job Role List");
            foreach (var item in ResourceType)
            {
                datamodel.ResourceTypeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            return View(datamodel);
        }

        public OrganizationChartViewModel returnList()
        {
            OrganizationChartViewModel datamodel = new OrganizationChartViewModel();
            foreach (var item in _CompanyStructureMethod.getAllBusinessList())
            {
                datamodel.BusinessList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            var ResourceType = _CompanyStructureMethod.getAllSystemValueListByKeyName("Job Role List");
            foreach (var item in ResourceType)
            {
                datamodel.ResourceTypeList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            //datamodel.BusinessID = (int)data.BusinessID;
            return datamodel;
        }

        public ActionResult bindDivisionList(int businessId)
        {
            var data = _CompanyStructureMethod.GetDivisionListByBizId(businessId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult bindPoolList(int DivisionId)
        {
            var data = _CompanyStructureMethod.GetPoolListByBizId(DivisionId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult bindFuncationList(int DivisionId)
        {
            var data = _CompanyStructureMethod.GetFuncationListByBizId(DivisionId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrgChartDetails()
        {
            List<OrganizationChartViewModel> ObjEmp = new List<OrganizationChartViewModel>();
            var data = _OrgChartMethod.GetDetails();
            if (data.Count > 0)
            {
                foreach (var details in data)
                {
                    OrganizationChartViewModel datamodal = new OrganizationChartViewModel();
                    datamodal.EmpId = details.Id;
                    datamodal.Name = details.FirstName;
                    datamodal.LengthOfEmployeement = _OrgChartMethod.getTotalWorkingDayInfo(Convert.ToInt32(details.Id));
                    datamodal.ReportsTo = details.Reportsto;
                    datamodal.Value = details.Value;
                    ObjEmp.Add(datamodal);
                }
            }
            return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BusiOrgChartDetails(int BusiID, int DivID, int PoolID, int FunID,int EmpTypeId,string futureStater)
        {
            List<OrganizationChartViewModel> ObjEmp = new List<OrganizationChartViewModel>();
            var data = _OrgChartMethod.GetBusinessDetails(BusiID, DivID, PoolID, FunID, EmpTypeId);
            futureStater=futureStater.ToUpper();
            if (futureStater=="ON")
            {
                data = _OrgChartMethod.GetBusinessDetails(BusiID, DivID, PoolID, FunID, EmpTypeId);
            }
            else if(futureStater=="OFF")
            {
                data = _OrgChartMethod.GetBusinessDetails(BusiID, DivID, PoolID, FunID, EmpTypeId).Where(x => x.StartDate >= DateTime.Now).ToList(); ;
            }
            
            if (data.Count > 0)
            {
                foreach (var details in data)
                {
                    OrganizationChartViewModel datamodal = new OrganizationChartViewModel();
                    datamodal.EmpId = details.Id;
                    datamodal.Name = details.FirstName;
                    if (data.Any(x => x.Id == details.Reportsto))
                    {
                        datamodal.ReportsTo = details.Reportsto;
                    }
                    else
                    {
                        datamodal.ReportsTo = null;
                    }
                    datamodal.LengthOfEmployeement = _OrgChartMethod.getTotalWorkingDayInfo(Convert.ToInt32(details.Id));
                    //datamodal.ReportsTo = details.Reportsto==null ?  (details.Reportsto):"Vacant";
                    datamodal.Value = details.Value;
                    datamodal.ImageUrl = details.ImageUrl;
                    datamodal.BusinessID = Convert.ToInt32(details.BusinessID);
                    ObjEmp.Add(datamodal);
                }
            }
            return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintChartData(String chartData)
        {
            OrganizationChartViewModel datamodel = new OrganizationChartViewModel();
            try
            {
                string newfileName = string.Format("donloadPDF");
                return new Rotativa.ViewAsPdf("GenerateOrganizationChartPDF", datamodel)
                {
                    PageSize = Size.A4,
                    PageOrientation = Orientation.Landscape,
                    FileName = newfileName
                };
            }
            catch (Exception ex)
            {
                throw ex;            
                }
        }
        // GET: OrganizationChart/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrganizationChart/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrganizationChart/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: OrganizationChart/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrganizationChart/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: OrganizationChart/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrganizationChart/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
