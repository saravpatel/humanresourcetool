using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HRTool.CommanMethods;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class TMSSettingsController : Controller
    {
        #region Constant
        EvolutionEntities _db = new EvolutionEntities();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        TMSSettingsMethod _TMSSettingsMethod = new TMSSettingsMethod();

        #endregion

        #region View
        // GET: /TMSSettings/

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region TMS Setting Method
        public ActionResult StepMoveTMSSetting(string AllStepSegmentJsonm, int SortId, int RecId, int flagUpDown)
        {
            bool save = _TMSSettingsMethod.UpdateStepMoveOfRecProcess(AllStepSegmentJsonm, SortId, RecId, flagUpDown);
            List<TMSSettingStepDetails> listdata = _TMSSettingsMethod.UpdateStepMove(AllStepSegmentJsonm, SortId, RecId, flagUpDown);
            List<TMSSettingCompetencyDetails> listcompet = new List<TMSSettingCompetencyDetails>();
            TMSSettingsViewModel model = returnList(RecId, listdata, listcompet);
            return PartialView("_partialAddEditTMSSetting", model);

        }

        public ActionResult Cancel()
        {
            return RedirectToAction("Index", "TMSSettings");
        }
        public ActionResult StepMoveCompetencyTMSSetting(string AllStepSegmentJsonm, int SortId, int RecId, int flagUpDown)
        {
            bool save = _TMSSettingsMethod.UpdateStepMoveOfCometencies(AllStepSegmentJsonm, SortId, RecId, flagUpDown);
            List<TMSSettingStepDetails> listdata = new List<TMSSettingStepDetails>();
            List<TMSSettingCompetencyDetails> listcompet = _TMSSettingsMethod.UpdateStepCometencies(AllStepSegmentJsonm, SortId, RecId, flagUpDown);
            TMSSettingsViewModel model = returnList(RecId, listdata, listcompet);
            return PartialView("_partialAddEditTMSSetting", model);
        }
        public TMSSettingsViewModel returnList(int Id, List<TMSSettingStepDetails> listdata, List<TMSSettingCompetencyDetails> listcompet)
        {
            TMSSettingsViewModel model = new TMSSettingsViewModel();
            var listData = _TMSSettingsMethod.getAllTMSSettingList();
            if (listData.Count > 0)
            {
                foreach (var item in listData)
                {
                    TMSSettingAllDetailsViewModel datamodel = new TMSSettingAllDetailsViewModel();
                    datamodel.Id = item.Id;
                    datamodel.Name = item.Name;
                    model.stepCount = 4;
                    model.TMSSettingSaveList.Add(datamodel);
                }
            }
            else
            {
                model.stepCount = 4;

            }
            if (Id == 0)
            {
                model.Id = Id;
                if (listdata.Count > 0)
                {
                    foreach (var s in listdata)
                    {
                        TMSSettingStepDetails ss = new TMSSettingStepDetails();
                        ss.Id = s.Id;
                        ss.SortId = s.SortId;
                        ss.StepName = s.StepName;
                        ss.ColorCode = s.ColorCode;
                        model.StepList.Add(ss);
                    }
                }
                else
                {
                    var JsonStepCsv = "[{'Id':1,'SortId':1,'StepName':'Rejected','ColorCode':'#CC4400'},{ 'Id':2,'SortId':2,'StepName':'Talent Pool','ColorCode':'#FF9900'},{ 'Id':3,'SortId':3,'StepName':'New Applicants','ColorCode':'#00CCFF'},{ 'Id':4,'SortId':4,'StepName':'Accepted','ColorCode':'#00CC00'}]";
                    var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(JsonStepCsv);
                    foreach (var s in steps)
                    {

                        TMSSettingStepDetails ss = new TMSSettingStepDetails();
                        ss.Id = s.Id;
                        ss.SortId = s.SortId;
                        ss.StepName = s.StepName;
                        ss.ColorCode = s.ColorCode;
                        model.StepList.Add(ss);
                    }
                }
                if (listcompet.Count > 0)
                {
                    foreach (var s in listcompet)
                    {
                        TMSSettingCompetencyDetails ss = new TMSSettingCompetencyDetails();
                        ss.Id = s.Id;
                        ss.SortId = s.SortId;
                        ss.CompetencyName = s.CompetencyName;
                        model.CompentecyList.Add(ss);
                    }
                }
            }
            else
            {
                var listDatas = _TMSSettingsMethod.getTMSSettingListById(Id);
                model.Id = listDatas.Id;
                model.Name = listDatas.Name;
                if (listDatas.StepCSV != null)
                {
                    var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(listDatas.StepCSV).OrderBy(x => x.SortId);
                    if (steps.Count() == 0)
                    {
                        model.stepCount = 4;
                    }
                    else
                    {
                        model.stepCount = steps.Count();
                        foreach (var s in steps)
                        {
                            TMSSettingStepDetails ss = new TMSSettingStepDetails();
                            ss.Id = s.Id;
                            ss.SortId = s.SortId;
                            ss.StepName = s.StepName;
                            ss.ColorCode = s.ColorCode;
                            model.StepList.Add(ss);
                        }
                    }
                }
                if (listDatas.StepCSV != null)
                {
                    var competency = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(listDatas.CompetencyCSV);
                    if (competency.Count > 0)
                    {
                        foreach (var c in competency)
                        {
                            TMSSettingCompetencyDetails com = new TMSSettingCompetencyDetails();
                            com.Id = c.Id;
                            com.SortId = c.SortId;
                            com.CompetencyName = c.CompetencyName;
                            com.Description = c.Description;
                            model.CompentecyList.Add(com);
                        }
                    }
                }
                else
                {
                    var competency = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(listDatas.CompetencyCSV);
                    if (competency.Count > 0)
                    {
                        foreach (var c in competency)
                        {
                            TMSSettingCompetencyDetails com = new TMSSettingCompetencyDetails();
                            com.Id = c.Id;
                            com.SortId = c.SortId;
                            com.CompetencyName = c.CompetencyName;
                            com.Description = c.Description;
                            model.CompentecyList.Add(com);
                        }
                    }
                }
            }
            return model;
        }

        public ActionResult TMSSettingsList()
        {
            var Id = 0;
            List<TMSSettingStepDetails> listdata = new List<TMSSettingStepDetails>();
            List<TMSSettingCompetencyDetails> listcompet = new List<TMSSettingCompetencyDetails>();
            TMSSettingsViewModel model = returnList(Id, listdata, listcompet);
            return PartialView("_partialTMSSettingList", model);
        }

        public ActionResult AddEditTMSSetting(int Id)
        {
            List<TMSSettingStepDetails> listdata = new List<TMSSettingStepDetails>();
            List<TMSSettingCompetencyDetails> listcompet = new List<TMSSettingCompetencyDetails>();
            TMSSettingsViewModel model = returnList(Id, listdata, listcompet);
            return PartialView("_partialAddEditTMSSetting", model);

        }

        public ActionResult AddStep(TMSSettingsViewModel model)
        {
            var id = 0;
            List<TMSSettingStepDetails> listdata = new List<TMSSettingStepDetails>();
            List<TMSSettingCompetencyDetails> listcompet = new List<TMSSettingCompetencyDetails>();
            TMSSettingsViewModel datamodel = returnList(id, listdata, listcompet);
            var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(model.StepCSV).OrderBy(x => x.SortId);
            if (steps.Count() == 0)
            {
                model.stepCount = 4;
            }
            else
            {
                model.stepCount = steps.Count();

                foreach (var s in steps)
                {
                    TMSSettingStepDetails ss = new TMSSettingStepDetails();
                    ss.Id = s.Id;
                    ss.SortId = s.SortId;
                    ss.StepName = s.StepName;
                    ss.ColorCode = s.ColorCode;
                    model.StepList.Add(ss);
                }
            }
            return PartialView("_partialAddEditTMSSetting", model);

        }
        public ActionResult CopyTMSSetting(int Id, int RPID, string Name)
        {
            List<TMSSettingStepDetails> listdata = new List<TMSSettingStepDetails>();
            List<TMSSettingCompetencyDetails> listcompet = new List<TMSSettingCompetencyDetails>();
            TMSSettingsViewModel model = returnList(Id, listdata, listcompet);
            model.Id = RPID;
            model.Name = Name;
            model.SelectCopyId = Id;
            return PartialView("_partialAddEditTMSSetting", model);
        }

        public ActionResult SegmentStepSectionCreate(TMSSettingsViewModel model)
        {
            model.stepCount = model.StepList.Count();
            return PartialView("_PartialStepSegmentSection", model);
        }

        public ActionResult SegmentCompetencieSectionCreate(TMSSettingsViewModel model)
        {
            return PartialView("_PartialCompetencieSegmentSection", model);
        }

        [HttpPost]
        public ActionResult SaveTMSSetting(TMSSettingsViewModel model)
        {
            bool save = _TMSSettingsMethod.SaveTMSSettingData(model, SessionProxy.UserId);

            if (save)
            {
                model.Id = 0;
                List<TMSSettingCompetencyDetails> listcompet = new List<TMSSettingCompetencyDetails>();
                List<TMSSettingStepDetails> listdata = new List<TMSSettingStepDetails>();
                model = returnList(model.Id, listdata, listcompet);
                return PartialView("_partialTMSSettingList", model);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult DeleteTMSSetting(int Id)
        {
            bool delete = _TMSSettingsMethod.deleteTMSSetting(Id, SessionProxy.UserId);
            if (delete)
            {
                Id = 0;
                List<TMSSettingStepDetails> listdata = new List<TMSSettingStepDetails>();
                List<TMSSettingCompetencyDetails> listcompet = new List<TMSSettingCompetencyDetails>();
                TMSSettingsViewModel model = returnList(Id, listdata, listcompet);
                return PartialView("_partialTMSSettingList", model);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]
        public ActionResult TMSSettings()
        {
            return View();
        }
        #endregion


    }
}