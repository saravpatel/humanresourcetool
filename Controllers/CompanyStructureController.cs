using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using HRTool.CommanMethods;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class CompanyStructureController : Controller
    {
        #region Constant
        EvolutionEntities _db = new EvolutionEntities();
        // OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        CompanyStructureMethod _CompanyStructureMethod = new CompanyStructureMethod();

        #endregion

        #region View

        // GET: /CompanyStructure/
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region Business Method
        public CompanyStructureViewModel returnList()
        {
            CompanyStructureViewModel model = new CompanyStructureViewModel();
            var listData = _CompanyStructureMethod.getAllBusinessList();
            foreach (var item in listData)
            {
                BusinessViewModel tableModel = new BusinessViewModel();
                tableModel.Id = item.Id;
                tableModel.Name = item.Name;
                tableModel.CreatedDate = item.CreatedDate;
                tableModel.Archived = item.Archived;
                model.businessLists.Add(tableModel);
            }
            return model;
        }

        public ActionResult businessList()
        {
            CompanyStructureViewModel model = returnList();
            return PartialView("_partialBusinessList", model);
        }

        public ActionResult addBusinessList(string Name, int Id)
        {

            var data = _CompanyStructureMethod.SaveBusinessData(Name, Id, SessionProxy.UserId);
            if (!data)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                CompanyStructureViewModel model = returnList();
                return PartialView("_partialBusinessList", model);
            }

        }

        public ActionResult editBusinessName(int Id)
        {

            var data = _CompanyStructureMethod.getBusinessListById(Id);

            if (data == null)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult deleteBusiness(int Id)
        {
            var data = _CompanyStructureMethod.deleteBusiness(Id, SessionProxy.UserId);

            if (!data)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                // return Json(data, JsonRequestBehavior.AllowGet);
                CompanyStructureViewModel model = returnList();
                return PartialView("_partialBusinessList", model);
            }
        }

        #endregion

        #region Division Method

        public CompanyStructureViewModel returndivList()
        {
            CompanyStructureViewModel model = new CompanyStructureViewModel();
            var listData = _CompanyStructureMethod.getAllDivisionList();
            var listbiz = _CompanyStructureMethod.getAllBusinessList();

            foreach (var item in listData)
            {
                var bizName = _CompanyStructureMethod.getBusinessListById(item.BusinessID);
                if (bizName != null)
                {
                    DivisionViewModel tableModel = new DivisionViewModel();
                    tableModel.Id = item.Id;
                    tableModel.Name = item.Name;
                    tableModel.BusinessName = bizName.Name;
                    tableModel.CreatedDate = item.CreatedDate;
                    tableModel.Archived = item.Archived;
                    model.divisionLists.Add(tableModel);
                }
            }
            foreach (var item in listbiz)
            {
                
                    BusinessViewModel tableModel = new BusinessViewModel();
                    tableModel.Id = item.Id;
                    tableModel.Name = item.Name;
                    tableModel.CreatedDate = item.CreatedDate;
                    tableModel.Archived = item.Archived;
                    model.businessLists.Add(tableModel);
            }

            return model;
        }
        public ActionResult divisionList()
        {
            CompanyStructureViewModel model = returndivList();
            return PartialView("_partialDivisionList", model);
        }

        public ActionResult addDivisionList(string Name, int Id, int businessId)
        {
            var data = _CompanyStructureMethod.SaveDivisionData(Name, Id, businessId, SessionProxy.UserId);
            if (!data)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                CompanyStructureViewModel model = returndivList();
                return PartialView("_partialDivisionList", model);
            }


        }

        public ActionResult editDivisionName(int Id)
        {
            DivisionViewModel model = new DivisionViewModel();
            var data = _CompanyStructureMethod.getDivisionById(Id);
            var bizName = _CompanyStructureMethod.getBusinessListById(data.BusinessID);
            model.Id = data.Id;
            model.Name = data.Name;
            model.BusinessID = data.BusinessID;
            model.BusinessName = bizName.Name;
            if (data == null)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult deleteDivision(int Id)
        {
            var data = _CompanyStructureMethod.deleteDivision(Id, SessionProxy.UserId);

            if (!data)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                CompanyStructureViewModel model = returndivList();
                return PartialView("_partialDivisionList", model);
            }
        }

        #endregion

        #region Pool Method

        public CompanyStructureViewModel returndPoolList()
        {
            CompanyStructureViewModel model = new CompanyStructureViewModel();
            var listpol = _CompanyStructureMethod.getAllPoolsList();
            var listData = _CompanyStructureMethod.getAllDivisionList();
            var listbiz = _CompanyStructureMethod.getAllBusinessList();

            foreach (var item in listpol)
            {
                var bizsName = _CompanyStructureMethod.getBusinessListById((int)item.BusinessID);

                if (bizsName != null)
                {
                    var divName = _CompanyStructureMethod.getDivisionById(item.DivisionID);
                    if (divName != null)
                    {
                        PoolViewModel tablepool = new PoolViewModel();
                        tablepool.Id = item.Id;
                        tablepool.Name = item.Name;
                        tablepool.DivisionName = divName.Name;
                        tablepool.BusinessName = bizsName.Name;
                        tablepool.CreatedDate = item.CreatedDate;
                        model.poolLists.Add(tablepool);
                    }
                    else
                    {
                        PoolViewModel tablepool = new PoolViewModel();
                        tablepool.Id = item.Id;
                        tablepool.Name = item.Name;
                        tablepool.DivisionName = "";
                        tablepool.BusinessName = bizsName.Name;
                        tablepool.CreatedDate = item.CreatedDate;
                        model.poolLists.Add(tablepool);


                    }
                }


            }

            foreach (var item in listData)
            {
                DivisionViewModel tableModel = new DivisionViewModel();
                tableModel.Id = item.Id;
                tableModel.Name = item.Name;
                tableModel.BusinessID = item.BusinessID;
                tableModel.CreatedDate = item.CreatedDate;
                tableModel.Archived = item.Archived;
                model.divisionLists.Add(tableModel);

            }
            foreach (var item in listbiz)
            {
                BusinessViewModel tableModel = new BusinessViewModel();
                tableModel.Id = item.Id;
                tableModel.Name = item.Name;
                tableModel.CreatedDate = item.CreatedDate;
                tableModel.Archived = item.Archived;
                model.businessLists.Add(tableModel);
            }

            return model;
        }

        public ActionResult poolList()
        {
            CompanyStructureViewModel model = returndPoolList();
            return PartialView("_partialPoolList", model);
        }

        public ActionResult bindDivisionList(int businessId)
        {
            var data = _CompanyStructureMethod.GetDivisionListByBizId(businessId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult addPoolList(string Name, int Id, int businessId, int divisionId)
        {
            var data = _CompanyStructureMethod.SavePoolData(Name, Id, businessId, divisionId, SessionProxy.UserId);
            if (!data)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                CompanyStructureViewModel model = returndPoolList();
                return PartialView("_partialPoolList", model);
            }

        }

        public ActionResult editPoolName(int Id)
        {
            PoolViewModel model = new PoolViewModel();
            var data = _CompanyStructureMethod.getPoolsListById(Id);
            var bizName = _CompanyStructureMethod.getBusinessListById((int)data.BusinessID);
            var divName = _CompanyStructureMethod.getDivisionById(data.DivisionID);

            model.Id = data.Id;
            model.Name = data.Name;
            model.BusinessID = (int)data.BusinessID;
            model.DivisionID = data.DivisionID;
            model.BusinessName = bizName.Name;
            if (divName == null)
            {
                model.DivisionName = "";

            }
            else
            {
                model.DivisionName = divName.Name;
            }

            if (data == null)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult deletePool(int Id)
        {
            var data = _CompanyStructureMethod.deletePool(Id, SessionProxy.UserId);

            if (!data)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                CompanyStructureViewModel model = returndPoolList();
                return PartialView("_partialPoolList", model);
            }
        }

        #endregion

        #region Function Method

        public CompanyStructureViewModel returndfunList()
        {
            CompanyStructureViewModel model = new CompanyStructureViewModel();
            var listfun = _CompanyStructureMethod.getAllFunctionsList();
            var listData = _CompanyStructureMethod.getAllDivisionList();
            var listbiz = _CompanyStructureMethod.getAllBusinessList();
            
            foreach (var item in listfun)
            {
                var bizsName = _CompanyStructureMethod.getBusinessListById((int)item.BusinessID);

                if (bizsName != null)
                {
                    var divName = _CompanyStructureMethod.getDivisionById(item.DivisionID);
                    if (divName != null)
                    {
                        FunctionViewModel tablepool = new FunctionViewModel();
                        tablepool.Id = item.Id;
                        tablepool.Name = item.Name;
                        tablepool.DivisionName = divName.Name;
                        tablepool.BusinessName = bizsName.Name;
                        tablepool.CreatedDate = item.CreatedDate;
                        model.functionLists.Add(tablepool);
                    }
                    else
                    {
                        FunctionViewModel tablepool = new FunctionViewModel();
                        tablepool.Id = item.Id;
                        tablepool.Name = item.Name;
                        tablepool.DivisionName = "";
                        tablepool.BusinessName = bizsName.Name;
                        tablepool.CreatedDate = item.CreatedDate;
                        model.functionLists.Add(tablepool);


                    }
                }
            }

            foreach (var item in listData)
            {
                DivisionViewModel tableModel = new DivisionViewModel();
                tableModel.Id = item.Id;
                tableModel.Name = item.Name;
                tableModel.BusinessID = item.BusinessID;
                tableModel.CreatedDate = item.CreatedDate;
                tableModel.Archived = item.Archived;
                model.divisionLists.Add(tableModel);

            }
            foreach (var item in listbiz)
            {
                BusinessViewModel tableModel = new BusinessViewModel();
                tableModel.Id = item.Id;
                tableModel.Name = item.Name;
                tableModel.CreatedDate = item.CreatedDate;
                tableModel.Archived = item.Archived;
                model.businessLists.Add(tableModel);
            }

            return model;
        }
        public ActionResult functionList()
        {
            CompanyStructureViewModel model = returndfunList();
            return PartialView("_partialFunctionList", model);
        }

        public ActionResult addFunctionList(string Name, int Id, int businessId, int divisionId)
        {
            var data = _CompanyStructureMethod.SaveFunctionData(Name, Id, businessId, divisionId, SessionProxy.UserId);
            if (!data)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                CompanyStructureViewModel model = returndfunList();
                return PartialView("_partialFunctionList", model);
            }

        }

        public ActionResult editFunctionName(int Id)
        {
            FunctionViewModel model = new FunctionViewModel();
            var data = _CompanyStructureMethod.getFunctionsListById(Id);
            var bizName = _CompanyStructureMethod.getBusinessListById((int)data.BusinessID);
            var divName = _CompanyStructureMethod.getDivisionById(data.DivisionID);

            model.Id = data.Id;
            model.Name = data.Name;
            model.BusinessID = (int)data.BusinessID;
            model.DivisionID = data.DivisionID;
            model.BusinessName = bizName.Name;
            if (divName == null)
            {
                model.DivisionName = "";

            }
            else
            {

                model.DivisionName = divName.Name;
            }

            if (data == null)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult deleteFunction(int Id)
        {
            var data = _CompanyStructureMethod.deleteFunction(Id, SessionProxy.UserId);

            if (!data)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                CompanyStructureViewModel model = returndfunList();
                return PartialView("_partialFunctionList", model);
            }
        }

        #endregion
    }
}