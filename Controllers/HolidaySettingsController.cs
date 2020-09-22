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
    public class HolidaySettingsController : Controller
    {
        #region Constant

        private string inputFormat = "dd-MM-yyyy";
        private string outputFormat = "yyyy-MM-dd HH:mm:ss";
        EvolutionEntities _db = new EvolutionEntities();
        HolidayNAbsenceMethod _holidayNAbsenceMethod = new HolidayNAbsenceMethod();
        #endregion

        [Authorize]
        public ActionResult Index()
        {
            HolidayNAbsenceViewModel model = new HolidayNAbsenceViewModel();
            var HolidayNAbsenceId = _holidayNAbsenceMethod.getAllHolidaysNAbsenceSettingList();
            if (HolidayNAbsenceId != null)
            {
                model = _holidayNAbsenceMethod.BindHolidaysNAbsenceSettingRecords(HolidayNAbsenceId.Id);
            }
            else
            {
                var id = 0;
                model = _holidayNAbsenceMethod.BindHolidaysNAbsenceSettingRecords(id);
            }
            return View(model);
            
        }

        [HttpPost]
        public ActionResult Index(string Model)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            HolidayNAbsenceViewModel ValueArray = js.Deserialize<HolidayNAbsenceViewModel>(Model);
            if (ModelState.IsValid)
            {

                if (ValueArray.Id > 0)
                {
                    int UpdatedCompanySetting = _holidayNAbsenceMethod.InsertUpdateHolidaySetting(ValueArray, true);
                }
                else
                {
                    int InsertCompanySetting = _holidayNAbsenceMethod.InsertUpdateHolidaySetting(ValueArray, false);
                }


                return RedirectToAction("Index");
            }
            else 
            {
                return View(Model);
            }
        
        }

        #region Public holiday

        public ActionResult PublicHoliday(int Id)
        {
            publicHolidayCounty model = new publicHolidayCounty();
            if (Id > 0)
            {
                model = _holidayNAbsenceMethod.getCountryData(Id);
            }
            return PartialView("_partialPublicHolidayCountry", model);
        }

        public ActionResult SaveHolidayData(int CountryId, string CountryName, string HolidayData)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<publicHolidayCountyList> listValueArray = js.Deserialize<List<publicHolidayCountyList>>(HolidayData);
            _holidayNAbsenceMethod.SaveHolidayData(CountryId, CountryName, listValueArray, SessionProxy.UserId);
            return Json("sucess", JsonRequestBehavior.AllowGet);
        }

        public ActionResult updateHoliday(int Id, string Name, string Date)
        {
            _holidayNAbsenceMethod.updateHoliday(Id, Name, Date, SessionProxy.UserId);
            return Json("sucess", JsonRequestBehavior.AllowGet);
        }
        public ActionResult deleteHoliday(int Id)
        {
            _holidayNAbsenceMethod.dleteHoliday(Id, SessionProxy.UserId);
            return Json("sucess", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region BradfordFactor

        public ActionResult BradfordFactor()
        {
            BradfordFactorViewModel model = new BradfordFactorViewModel();
            var bradfordFactor = _holidayNAbsenceMethod.getBradfordFactor();
            if (bradfordFactor != null)
            {
                model.Id = bradfordFactor.Id;
                model.LowerValue1 = bradfordFactor.LowerValue1;
                model.UpperValue1 = bradfordFactor.UpperValue1;
                model.Alert1 = bradfordFactor.Alert1;
                model.LowerValue2 = bradfordFactor.LowerValue2;
                model.UpperValue2 = bradfordFactor.UpperValue2;
                model.Alert2 = bradfordFactor.Alert2;
                model.LowerValue3 = bradfordFactor.LowerValue3;
                model.UpperValue3 = bradfordFactor.UpperValue3;
                model.Alert3 = bradfordFactor.Alert3;
                model.LowerValue4 = bradfordFactor.LowerValue4;
                model.UpperValue4 = bradfordFactor.UpperValue4;
                model.Alert4 = bradfordFactor.Alert4;
            }
            else
            {
                model.Id = 0;
            }
            return PartialView("_partialBradfordFactor", model);
        }

        public ActionResult SaveBradfordFactor(BradfordFactorViewModel model)
        {
            _holidayNAbsenceMethod.SaveBradfordFactor(model);
            return Json("sucess", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Work patten

        public ActionResult workPatten(int Id)
        {
            WorkPatternViewModel model = new WorkPatternViewModel();
            if (Id > 0)
            {
                model = _holidayNAbsenceMethod.returnModel(Id);
            }
            else
            {
                model.IsRotating = false;
                model.Id = 0;
            }
            return PartialView("_partialWorkPattern", model);
        }

        

        public ActionResult TrueIsRotating(int workPatternId)
        {
            List<WorkPatternListViewModel> model = new List<WorkPatternListViewModel>();
            if (workPatternId > 0) { }
            else
            {
                WorkPatternListViewModel oneModel = new WorkPatternListViewModel();
                oneModel.WorkPatternID = workPatternId;
                oneModel.Id = 0;
                model.Add(oneModel);
            }
            return PartialView("_partialTrueIsRotating", model);
        }

        public ActionResult FalseIsRotating()
        {
            WorkPatternViewModel model = new WorkPatternViewModel();
            return PartialView("_partialFalseIsRotating", model);
        }

        public ActionResult SaveFalseRoatingData(string modelString)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
           // modelString = modelString.Replace(":", ".");
            WorkPatternViewModel ValueArray = js.Deserialize<WorkPatternViewModel>(modelString);
            _holidayNAbsenceMethod.SaveFalseRoatingData(ValueArray);
            return null;

        }


        public ActionResult SaveTrueRoatingData(int Id, string Name, bool IsRotating, string modelString)
        {
            WorkPatternViewModel model = new WorkPatternViewModel();
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<WorkPatternListViewModel> ValueArray = js.Deserialize<List<WorkPatternListViewModel>>(modelString);
            model.Id = Id;
            model.Name = Name;
            model.IsRotating = IsRotating;
            model.WorkPatternList = ValueArray;
            _holidayNAbsenceMethod.SaveTrueRoatingData(model);
            return null;

        }
        #endregion
    }
}