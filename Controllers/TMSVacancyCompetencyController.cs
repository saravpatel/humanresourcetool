using HRTool.CommanMethods.Admin;
using HRTool.DataModel;
using HRTool.Models.Admin;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa.Options;
using Newtonsoft.Json;
using HRTool.CommanMethods;
using HRTool.Models.Settings;
namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class TMSVacancyCompetencyController : Controller
    {
        #region Constant
        EvolutionEntities _db = new EvolutionEntities();
        AdminTMSMethod _AdminTMSMethod = new AdminTMSMethod();
        #endregion
        // GET: /TMSVacancyCompetency/
        public ActionResult Index(ApplicantCompetencyPDfViewModel dataModel)
        {
            var ApplicantDetails = _AdminTMSMethod.getApplicantDetailsById(dataModel.ApplicantId);
            dataModel.FirstName = ApplicantDetails.FirstName;
            dataModel.LastName = ApplicantDetails.LastName;
            var vacancyDetails = _AdminTMSMethod.getVacancyDetailsById(dataModel.VacancyId);
            dataModel.VacancyName = vacancyDetails.Title;

            var competency = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(ApplicantDetails.CompatencyJSV);
            if (competency.Count > 0)
            {
                foreach (var c in competency)
                {
                    TMSSettingCompetencyDetails cc = new TMSSettingCompetencyDetails();
                    cc.Id = c.Id;
                    cc.CompetencyName = c.CompetencyName;
                    cc.Score = c.Score;
                    dataModel.CompatencyJSV.Add(cc);
                }
            }
            return View(dataModel);
        }

                
        public ActionResult genaratePDF(int ApplicantID, int VacancyID)
        {
            try
            {
                //int ApplicantID, int VacancyID
                ApplicantCompetencyPDfViewModel model = new ApplicantCompetencyPDfViewModel();
                var ApplicantDetails = _AdminTMSMethod.getApplicantDetailsById(ApplicantID);
                model.ApplicantId = ApplicantDetails.Id;
                model.VacancyId = VacancyID;
                model.VacancyName = _db.Vacancies.Where(x => x.Archived == false && x.Id == VacancyID).FirstOrDefault().Title;
                model.FirstName = ApplicantDetails.FirstName;
                model.LastName = ApplicantDetails.LastName;
                DateTime currentDate = DateTime.Now;
                if (ApplicantDetails.CompatencyJSV != null && ApplicantDetails.CompatencyJSV != "")                
                {
                    var com = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(ApplicantDetails.CompatencyJSV);
                    int total=0;
                    if (com.Count > 0)
                    {
                        foreach (var c in com)
                        {
                            TMSSettingCompetencyDetails cc = new TMSSettingCompetencyDetails();
                            cc.Id = c.Id;
                            cc.CompetencyName = c.CompetencyName;
                            cc.Score = c.Score;
                            total = total+Convert.ToInt32(cc.Score);
                            model.CompatencyJSV.Add(cc);
                        }
                        int totalCom= model.CompatencyJSV.Count();
                        decimal avg = total / totalCom;
                        model.totalAvg = Convert.ToString(avg);
                    }


                }
                string newfileName = string.Format("" + model.FirstName + "_" + model.LastName + "_Competencies.pdf", currentDate.Date);
                return new Rotativa.ViewAsPdf("Index", model)
                {
                    PageSize = Size.A4,
                    PageOrientation = Orientation.Landscape,
                    FileName = newfileName
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ActionResult genarateCommentPDF(int ApplicantID,int VacancyID)
        {
            try
            {
                ApplicantCommentPDfViewModel model = new ApplicantCommentPDfViewModel();
                var ApplicantDetails = _AdminTMSMethod.getApplicantDetailsById(ApplicantID);
                model.ApplicantId = ApplicantDetails.Id;
                model.VacancyName = _db.Vacancies.Where(x => x.Archived == false && x.Id == VacancyID).FirstOrDefault().Title;
                model.FirstName = ApplicantDetails.FirstName;
                model.LastName = ApplicantDetails.LastName;
                var commentData = _db.TMS_Applicant_Comments.Where(x => x.ApplicantID == ApplicantID && x.Archived==false).ToList();
                model.DateOfBirth = Convert.ToDateTime(ApplicantDetails.DateOfBirth);
                DateTime currentDate = DateTime.Now;             
                if (commentData.Count > 0 && commentData != null)
                    {
                        foreach (var data in commentData)
                        {
                            TMSCommentDetails cc = new TMSCommentDetails();
                            cc.Id = data.Id;
                            cc.CreatedName = data.CreatedName;
                            cc.Description = data.Description;
                            cc.CreatedDateTime = data.CreatedDateTime;
                            model.CommentJSV.Add(cc);
                        }
                    }                                    
                string newfileName = string.Format("" + model.FirstName + "_" + model.LastName + "_Comments.pdf", currentDate.Date);
                return new Rotativa.ViewAsPdf("TMSCommentPDf", model)
                {
                    PageSize = Size.A4,
                    PageOrientation = Orientation.Landscape,
                    FileName = newfileName
                };
            }
            catch (Exception e)
            {
                throw e;
            }
    }

    }
    }
