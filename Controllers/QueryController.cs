using HRTool.CommanMethods;
using HRTool.CommanMethods.Query;
using HRTool.DataModel;
using HRTool.Models.Query;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HRTool.CommanMethods.Settings;
using System.Globalization;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class QueryController : Controller
    {
        EvolutionEntities _db = new EvolutionEntities();
        List<QueryFilter> _QueryFilter = new List<QueryFilter>();
        CompanyStructureMethod _CompanyStructureMethod = new CompanyStructureMethod();
        QueryMethod _QueryMethod = new QueryMethod();
        // GET: Query

        public ActionResult Index()
        {
            QueryDataSet model = returnQueryList();
            return View(model);
        }
        public QueryDataSet returnQueryList()
        {
            List<QueryData> dataQuery = _db.QueryDatas.ToList();
            QueryDataSet modelSet = new QueryDataSet();
            foreach (var item in dataQuery)
            {
                QueryDataSet model = new QueryDataSet();
                model.QueryDescription = item.Description;
                model.QueryName = item.Name;
                modelSet.AllQueryData.Add(model);
            }
            return modelSet;
        }

        public ActionResult CreateQuery()
        {
            QueryViewModel _QueryViewModel = BindData();
            return PartialView("_partialAddQuery",_QueryViewModel);
        }


        #region Bind data of query view
        public QueryViewModel BindData()
        {
            QueryViewModel _QueryViewModel = new QueryViewModel();
            _QueryFilter.Add(new QueryFilter());

            foreach (TableList item in Enum.GetValues(typeof(TableList)))
            {
                _QueryViewModel.TableNameList.Add(new SelectListItem() { Text = GetTableValueText(item.ToString()), Value = Convert.ToString((int)item) });
            }
            foreach(var item in Enum.GetValues(typeof(ChildTable)))
            {
                _QueryViewModel.ChildTableList.Add(new SelectListItem() { Text = GetTableValueText(item.ToString()), Value = Convert.ToString((int)item) });
            }
            foreach(var data in _db.Businesses)
            {
                _QueryViewModel.BusinessList.Add(new SelectListItem() { Text = data.Name, Value = data.Id.ToString() });
            }
            var ResourceType = _CompanyStructureMethod.getAllSystemValueListByKeyName("Job Title List");
            foreach (var item in ResourceType)
            {
                _QueryViewModel.JobTitleList.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            }
            var CustomerList = _db.AspNetUsers.Where(x => x.SSOID.StartsWith("C") && x.Archived == false).ToList();
            foreach(var data in CustomerList)
            {
                _QueryViewModel.CustomerList.Add(new SelectListItem() { Text = data.FirstName + ' ' + data.LastName + '-' + data.SSOID, Value = data.Id.ToString() });
            }
            //foreach (TableList item in Enum.GetValues(typeof(MasterTableList)))
            //{
            //    _QueryViewModel.MasterTable.Add(new SelectListItem() { Text = GetMasterTableTextValue(item.ToString()), Value = Convert.ToString((int)item) });
            //}
            //  _QueryViewModel.IsDisplayAny = false;
            return _QueryViewModel;
        }
        #endregion
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
        public ActionResult bindBussiness()
        {
            var data = _CompanyStructureMethod.GetBussiness();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCustomer()
        {
            var data = _db.AspNetUsers.Where(x => x.SSOID.StartsWith("C") && x.Archived == false).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult bindFuncationList(int DivisionId)
        {
            var data = _CompanyStructureMethod.GetFuncationListByBizId(DivisionId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #region Get Column List by TableId
        public JsonResult GetColumnList(string TableID)
        {
            List<SelectListItem> _ColumnList = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(TableID))
            {
                int Id = Convert.ToInt32(TableID);
                foreach (var item in _db.QueryColunNames.Where(x=>x.TableId == Id).ToList()) 
                {
                    _ColumnList.Add(new SelectListItem() { Text = item.DisplayName, Value = item.ColumnName });
                }
                return Json(_ColumnList, JsonRequestBehavior.AllowGet);
            }
         

            return Json(null, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Save Query on database
        public ActionResult SaveAllQuery(QueryFiledModel model)
        {
            string inputFormat = "dd-MM-yyyy";
            string ReplaceString = model.columnString.Replace(" ", "");
            string[] ArrayofColumn = ReplaceString.Split(',');
            List<string> tableName = new List<string>();
            foreach (var item in ArrayofColumn)
            {
                tableName.Add(item.Split('.')[0]);
            }
            StringBuilder SqlQuery = new StringBuilder();
            if (model.selectTableId == 17)
            {
                SqlQuery.Append("select distinct ");
                SqlQuery.Append(model.columnString);
                SqlQuery.Append(" from Company_Customer cmpCustomer");
                SqlQuery.Append(" Left Join Currencies cmpCun on cmpCustomer.Currency=cmpCun.Id ");
            }
            if (model.selectTableId == 13)
            {
                SqlQuery.Append("select distinct ");
                SqlQuery.Append(model.columnString);
                SqlQuery.Append(" from Projects Proj");
                SqlQuery.Append(" Left Join SystemListValue SLVProjecrCountry on SLVProjecrCountry.Id=Proj.Country ");
                SqlQuery.Append(" Left Join SystemListValue SLVProjecrLocation on SLVProjecrLocation.Id=Proj.Location ");
                SqlQuery.Append(" Left Join SystemListValue SLVProjecrBlock on SLVProjecrBlock.Id=Proj.Block ");
                SqlQuery.Append(" Left Join SystemListValue SLVProjecrTaxZone on SLVProjecrTaxZone.Id=Proj.TaxZone ");
            }
            else if (model.selectTableId == 14)
            {
                SqlQuery.Append("select distinct ");
                SqlQuery.Append(model.columnString);
                SqlQuery.Append(" from Assets Asset");
                SqlQuery.Append(" Left Join SystemListValue SLVAsset on SLVAsset.Id=Asset.AssetType ");
                SqlQuery.Append(" Left Join SystemListValue SLVAsset2 on SLVAsset2.Id=Asset.AssetType2 ");
            }
            else if (model.selectTableId == 16)
            {
                SqlQuery.Append("select distinct ");
                SqlQuery.Append(model.columnString);
                SqlQuery.Append(" from AspNetUsers ASPCust");
                SqlQuery.Append(" Left Join SystemListValue SLVCustGen on SLVCustGen.Id=ASPCust.Gender ");
                SqlQuery.Append(" Left Join SystemListValue SLVCustJob on SLVCustJob.Id=ASPCust.JobTitle ");
                SqlQuery.Append(" Left Join SystemListValue SLVCustLoc on SLVCustLoc.Id=ASPCust.Location ");
            }
            else if (model.selectTableId == 32)
            {
                SqlQuery.Append("select distinct ");
                SqlQuery.Append(model.columnString);
                SqlQuery.Append(" from SystemListValue SLVMasterCostCode");
            }
            else if (model.selectTableId == 26)
            {
                SqlQuery.Append("select distinct ");
                SqlQuery.Append(model.columnString);
                SqlQuery.Append(" from TMS_Applicant TMSApp ");
                SqlQuery.Append(" Left Join Vacancy TMSVac on TMSVac.Id=TMSApp.VacancyID ");
                SqlQuery.Append(" Left Join TMS_Setting_RecruitmentProcesses TMSRecProcess on TMSRecProcess.Id=TMSVac.RecruitmentProcessID ");
                SqlQuery.Append(" Left Join AspNetUsers ANUAppHiringLead on ANUAppHiringLead.Id=TMSVac.HiringLeadID ");
                SqlQuery.Append(" Left Join SystemListValue SLVAppVacSource on SLVAppVacSource.Id=TMSVac.SourceID ");
                SqlQuery.Append(" Left Join ApplicantRejectReason AppRejReason on AppRejReason.Id=TMSApp.RejectReasonId ");
                SqlQuery.Append(" Left Join SystemListValue SLVAppGender on SLVAppGender.Id=TMSApp.GenderID ");
            }
            else
            {
                SqlQuery.Append("select distinct ");
                SqlQuery.Append(model.columnString);
                SqlQuery.Append(" from AspNetUsers ANU");
                SqlQuery.Append(" Left Join SystemListValue SLVcom on ANU.Company = SLVcom.Id ");
                SqlQuery.Append(" Left Join SystemListValue SLVTitle on SLVTitle.Id = ANU.NameTitle ");
                SqlQuery.Append(" left join SystemListValue SLVJob on SLVJob.Id=ANU.JobTitle ");
                SqlQuery.Append(" left join SystemListValue SLVNan on SLVNan.Id=ANU.Nationality ");
                SqlQuery.Append(" left join SystemListValue SLVRes on SLVRes.Id=ANU.ResourceType ");
                SqlQuery.Append(" left join SystemListValue SLVLoc on SLVLoc.Id=ANU.Location ");
                SqlQuery.Append(" left join SystemListValue SLVNotice on SLVNotice.Id=ANU.NoticePeriod ");
                SqlQuery.Append(" left join Countries COJob on COJob.Id=ANU.JobContryID ");
                SqlQuery.Append(" left join EmployeeAddressInfo ERD on ERD.UserID=ANU.Id");
                SqlQuery.Append(" left join EmployeeRelation ER on ER.Reportsto=ANU.Id");
                SqlQuery.Append(" left join Countries COUI on COUI.Id=ERD.CountryId ");
                SqlQuery.Append(" left join States ST on ST.Id = ERD.StateId ");
                SqlQuery.Append(" left join Cities CT on CT.Id = ERD.TownId ");
                //EmployeeRelation
                SqlQuery.Append(" left join Business BNU on BNU.Id=ER.BusinessID ");
                SqlQuery.Append(" left join Divisions DNU on DNU.Id=ER.DivisionID ");
                SqlQuery.Append(" left join Pools PNU on PNU.Id=ER.PoolID ");
                SqlQuery.Append(" left join Functions FNU on FNU.Id=ER.FunctionID ");
                //Salary Type
                SqlQuery.Append(" left join Employee_Salary EMSAL on EMSAL.EmployeeID = ANU.ID ");
                SqlQuery.Append(" left join SystemListValue SLVSAL on SLVSAL.Id = EMSAL.SalaryType ");
                SqlQuery.Append(" left join SystemListValue SLVPayFreq on SLVPayFreq.Id = EMSAL.PaymentFrequency ");
                SqlQuery.Append(" left join SystemListValue SLVReasonChange on SLVReasonChange.Id = EMSAL.ReasonforChange ");
                SqlQuery.Append(" left join SystemListValue SLVCurruncies on SLVCurruncies.Id = EMSAL.Currency ");
                //EmployeemeneDetails
                SqlQuery.Append(" left join SystemListValue SLVNoticePeriod on SLVNoticePeriod.Id = ANU.NoticePeriod ");
                //Contact Details
                SqlQuery.Append(" left join EmployeeAddressInfo EmpAddInfo on EmpAddInfo.UserId = ANU.Id ");
                SqlQuery.Append(" left join EmployeeBankInfo EmpBankInfo on EmpBankInfo.UserId = ANU.Id ");
                //EmployeeAnnualLeave
                SqlQuery.Append(" left join Employee_AnualLeave EmpAnuLev on EmpAnuLev.EmployeeId = ANU.Id ");
                //EmployeeOtherLeave
                SqlQuery.Append(" left join Employee_OtherLeave EmpOthElv on EmpOthElv.EmployeeId = ANU.Id ");
                SqlQuery.Append(" left join SystemListValue SLVOtherLeave on SLVOtherLeave.Id = EmpOthElv.ReasonForLeaveId ");
                //EmployeeSickLeave
                SqlQuery.Append(" left join Employee_SickLeaves EmpSickLev on EmpSickLev.EmployeeId = ANU.Id ");
                SqlQuery.Append(" left join Employee_SickLeaves_Comments EmpSickLevCmt on EmpSickLevCmt.SickLeaveID = EmpSickLev.Id ");
                //Maternity/PaternityLeave
                SqlQuery.Append(" left join Employee_MaternityOrPaternityLeaves EmpMatPatLev on EmpMatPatLev.EmployeeId = ANU.Id ");
                //Late Leave
                SqlQuery.Append(" left join Employee_LateLeave EmpLateLev on EmpLateLev.EmployeeId = ANU.Id ");
                //Scheduling
                SqlQuery.Append(" left join Employee_ProjectPlanner_Scheduling EmpProjPlnSch on EmpProjPlnSch.EmployeeId = ANU.Id ");
                SqlQuery.Append(" left join SystemListValue SLVSchAsset on SLVSchAsset.Id=EmpProjPlnSch.AssetId ");
                SqlQuery.Append(" left join AspNetUsers ASPSChCust on ASPSChCust.Id=EmpProjPlnSch.CustomerId ");

                SqlQuery.Append(" left join Projects SchedulingProject on EmpProjPlnSch.ProjectId=SchedulingProject.Id ");
                SqlQuery.Append(" Left Join SystemListValue SLVSchedulingCountry on SLVSchedulingCountry.Id=SchedulingProject.Country  ");
                SqlQuery.Append(" Left Join SystemListValue SLVSchedulingLocation on SLVSchedulingLocation.Id=SchedulingProject.Location ");
                SqlQuery.Append(" Left Join SystemListValue SLVSchedulingBlock on SLVSchedulingBlock.Id=SchedulingProject.Block ");
                SqlQuery.Append(" Left Join SystemListValue SLVSchedulingTaxZone on SLVSchedulingTaxZone.Id=SchedulingProject.TaxZone ");
                //Benefit
                SqlQuery.Append(" left join Benefit EmpBenefit on EmpBenefit.EmployeeId = ANU.Id ");
                SqlQuery.Append(" left join SystemListValue SLVEmpBenefit on SLVEmpBenefit.Id = EmpBenefit.BenefitID ");
                //Certification
                SqlQuery.Append(" left join certificate EmpCertificate on EmpCertificate.AssignTo = ANU.Id ");
                SqlQuery.Append(" left join SystemListValue SLVEmpCertificate on SLVEmpCertificate.Id = EmpCertificate.Type ");
                SqlQuery.Append(" left join AspNetUsers ASPCertificateInRelationTo on SLVEmpCertificate.Id = EmpCertificate.Type ");
                //visa
                SqlQuery.Append(" left join Visa EmpVisa on EmpVisa.AssignedToEmployeeId = ANU.Id ");
                SqlQuery.Append(" left join AspNetUsers aspRelationTo on aspRelationTo.Id = EmpVisa.RelationToCSEmployeeID ");
                SqlQuery.Append(" left join SystemListValue SLVEmpVisa on SLVEmpVisa.Id = EmpVisa.VisaType ");
                SqlQuery.Append(" left join SystemListValue SLVEmpVisaAggencies on SLVEmpVisaAggencies.Id = EmpVisa.ServiceAgency ");
                SqlQuery.Append(" left join Countries VisaCoun on VisaCoun.Id = EmpVisa.Country ");
                //Task
                SqlQuery.Append(" left join Task_List EmpTask on EmpTask.AssignTo = ANU.Id ");
                SqlQuery.Append(" left join AspNetUsers aspTaskRelationTo on aspTaskRelationTo.Id = EmpTask.InRelationTo ");
                SqlQuery.Append(" left join SystemListValue SLVEmpTaskCat on SLVEmpTaskCat.Id=EmpTask.Category ");
                SqlQuery.Append(" left join SystemListValue SLVEmpTaskStatus on SLVEmpTaskStatus.Id=EmpTask.Status ");
                //Cases
                SqlQuery.Append(" left join Cases EmpCases on EmpCases.EmployeeID = ANU.Id ");
                SqlQuery.Append(" left join SystemListValue SLVCaseCat on SLVCaseCat.Id=EmpCases.Category ");
                SqlQuery.Append(" left join SystemListValue SLVEmpCaseStatus on SLVEmpCaseStatus.Id=EmpCases.Status ");
                //Training
                SqlQuery.Append(" left join EmployeeTraining EmpTrain on EmpTrain.EmployeeId = ANU.Id ");
                SqlQuery.Append(" left join SystemListValue SLVTrainStatus on SLVTrainStatus.Id=EmpTrain.Status ");
                //Document
                SqlQuery.Append(" left join Employee_Document EmpDoc on EmpDoc.EmployeeId = ANU.Id ");
                SqlQuery.Append(" left join SystemListValue SLVDocCat on SLVDocCat.Id=EmpDoc.Category ");
                //WorkPattern
                SqlQuery.Append(" left join Employee_WorkPattern EmpWP on EmpWP.EmployeeID = ANU.Id ");
                SqlQuery.Append(" left join WorkPatterns worPattern on worPattern.Id=EmpWP.WorkPatternID ");
                //Skill 
                SqlQuery.Append(" left join Employee_Skills EmpSkill on EmpSkill.EmployeeId = ANU.Id ");
                SqlQuery.Append(" left join SkillSets GenSkillSet on GenSkillSet.Id=EmpSkill.GeneralSkillsName ");
                SqlQuery.Append(" left join SkillSets TechSkillSet on TechSkillSet.Id=EmpSkill.TechnicalSkillsName ");
                //Uplift
                SqlQuery.Append(" left join Employee_ProjectPlanner_Uplift EmpUplift on EmpUplift.EmployeeId = ANU.Id ");
                SqlQuery.Append(" left join Employee_ProjectPlanner_Uplift_Detail EmpUpliftDetail on EmpUpliftDetail.UpliftId=EmpUplift.Id ");
                SqlQuery.Append(" left join SystemListValue SLVUpliftPos on SLVUpliftPos.Id=EmpUplift.UpliftPostionId ");
                //Activity Type
                SqlQuery.Append(" left join ActivityTypes ActType on ActType.Id=ANU.ActivityType ");
                SqlQuery.Append(" left join SystemListValue SLVActWorkCurr on SLVActWorkCurr.Id=ActType.CurrencyID ");
                SqlQuery.Append(" left join SystemListValue SLVActWorkrate on SLVActWorkrate.Id=ActType.WorkUnitID ");
                //Timesheet
                SqlQuery.Append(" left join Employee_TimeSheet EmpTimesheet on EmpTimesheet.EmployeeId=ANU.Id ");
                SqlQuery.Append(" left join Employee_TimeSheet_Detail EmpTimeshetDetail on EmpTimeshetDetail.TimeSheetId=EmpTimesheet.Id ");
                SqlQuery.Append(" left join SystemListValue EmptimeAssetType on EmptimeAssetType.Id=EmpTimeshetDetail.Asset ");
                SqlQuery.Append(" left join SystemListValue EmptimeCostCode on EmptimeCostCode.Id=EmpTimeshetDetail.CostCode ");
                SqlQuery.Append(" left join AspNetUsers EmptimeCustomer on EmptimeCustomer.Id=EmpTimeshetDetail.Customer ");

                SqlQuery.Append(" left join Projects TimeSheetProject on EmpTimeshetDetail.Project=TimeSheetProject.Id ");
                SqlQuery.Append(" Left Join SystemListValue SLVTimeSheetProjectCountry on SLVTimeSheetProjectCountry.Id=TimeSheetProject.Country ");
                SqlQuery.Append(" Left Join SystemListValue SLVTimeSheetProjectLocation on SLVTimeSheetProjectLocation.Id=TimeSheetProject.Location ");
                SqlQuery.Append(" Left Join SystemListValue SLVTimeSheetProjectBlock on SLVTimeSheetProjectBlock.Id=TimeSheetProject.Block ");
                SqlQuery.Append(" Left Join SystemListValue SLVTimeSheetProjectTaxZone on SLVTimeSheetProjectTaxZone.Id=TimeSheetProject.TaxZone ");
                //Project Planner Timesheet
                SqlQuery.Append(" left join Employee_ProjectPlanner_TimeSheet EmpProjTimesheet on EmpProjTimesheet.EmployeeId=ANU.Id ");
                SqlQuery.Append(" left join Employee_ProjectPlanner_TimeSheet_Detail EmpProjTimeshetDetail on EmpProjTimeshetDetail.TimeSheetId=EmpProjTimesheet.Id ");
                SqlQuery.Append(" left join SystemListValue EmpProjtimeAssetType on EmpProjtimeAssetType.Id=EmpProjTimeshetDetail.Asset ");
                SqlQuery.Append(" left join SystemListValue EmpProjtimeCostCode on EmpProjtimeCostCode.Id=EmpProjTimeshetDetail.CostCode ");
                SqlQuery.Append(" left join AspNetUsers EmpProjtimeCustomer on EmpProjtimeCustomer.Id=EmpProjTimeshetDetail.Customer ");

                SqlQuery.Append(" left join Projects EmpProjTimeshetProject on EmpProjTimeshetDetail.Project=EmpProjTimeshetProject.Id ");
                SqlQuery.Append(" Left Join SystemListValue SLVEmpProjTimeshetCountry on SLVEmpProjTimeshetCountry.Id=EmpProjTimeshetProject.Country ");
                SqlQuery.Append(" Left Join SystemListValue SLVEmpProjTimeshetLocation on SLVEmpProjTimeshetLocation.Id=EmpProjTimeshetProject.Location ");
                SqlQuery.Append(" Left Join SystemListValue SLVEmpProjTimeshetBlock on SLVEmpProjTimeshetBlock.Id=EmpProjTimeshetProject.Block ");
                SqlQuery.Append(" Left Join SystemListValue SLVEmpProjTimeshetTaxZone on SLVEmpProjTimeshetTaxZone.Id=EmpProjTimeshetProject.TaxZone ");
                //Travel
                SqlQuery.Append(" left join  Employee_TravelLeave EmpTravel on ANU.Id=EmpTravel.EmployeeId ");
                SqlQuery.Append(" left join Countries EmpTraFromCoun on EmpTraFromCoun.Id=EmpTravel.FromCountryId ");
                SqlQuery.Append(" left join Countries EmpTraToCoun on EmpTraToCoun.Id=EmpTravel.ToCountryId ");
                SqlQuery.Append(" left join States EmpTraFromState on EmpTraFromState.Id=EmpTravel.FromStateId ");
                SqlQuery.Append(" left join States EmpTraToState on EmpTraToState.Id=EmpTravel.ToStateId ");
                SqlQuery.Append(" left join Cities EmpTraFromCity on EmpTraFromCity.Id=EmpTravel.FromTownId ");
                SqlQuery.Append(" left join Cities EmpTraToCity on EmpTraToCity.Id=EmpTravel.ToTownId ");
                SqlQuery.Append(" left join SystemListValue SLVTravReason on SLVTravReason.Id=EmpTravel.ReasonForTravelId ");
                SqlQuery.Append(" left join SystemListValue SLVTravCostCode on SLVTravCostCode.Id=EmpTravel.CostCode ");
                SqlQuery.Append(" left join SystemListValue SLVTravType on SLVTravType.Id=EmpTravel.Type ");

                SqlQuery.Append(" left join Projects TravelProject on EmpTravel.Project=TravelProject.Id ");
                SqlQuery.Append(" Left Join SystemListValue SLVTravelProjecrCountry on SLVTravelProjecrCountry.Id=TravelProject.Country ");
                SqlQuery.Append(" Left Join SystemListValue SLVTravelProjecrLocation on SLVTravelProjecrLocation.Id=TravelProject.Location ");
                SqlQuery.Append(" Left Join SystemListValue SLVTravelProjecrBlock on SLVTravelProjecrBlock.Id=TravelProject.Block ");
                SqlQuery.Append(" Left Join SystemListValue SLVTravelProjecrTaxZone on SLVTravelProjecrTaxZone.Id=TravelProject.TaxZone ");
                //SqlQuery.Append("  ");
                //Project Planner Travel
                SqlQuery.Append(" left join  Employee_ProjectPlanner_TravelLeave EmpProjTravel on ANU.Id=EmpProjTravel.EmployeeId ");
                SqlQuery.Append(" left join Countries EmpProjTraFromCoun on EmpProjTraFromCoun.Id=EmpProjTravel.FromCountryId ");
                SqlQuery.Append(" left join Countries EmpProjTraToCoun on EmpProjTraToCoun.Id=EmpProjTravel.ToCountryId ");
                SqlQuery.Append(" left join States EmpProjTraFromState on EmpProjTraFromState.Id=EmpProjTravel.FromStateId ");
                SqlQuery.Append(" left join States EmpProjTraToState on EmpProjTraToState.Id=EmpProjTravel.ToStateId ");
                SqlQuery.Append(" left join Cities EmpProjTraFromCity on EmpProjTraFromCity.Id=EmpProjTravel.FromTownId ");
                SqlQuery.Append(" left join Cities EmpProjTraToCity on EmpProjTraToCity.Id=EmpProjTravel.ToTownId ");
                SqlQuery.Append(" left join SystemListValue SLVProjTravReason on SLVProjTravReason.Id=EmpProjTravel.ReasonForTravelId ");
                SqlQuery.Append(" left join SystemListValue SLVProjTravCostCode on SLVProjTravCostCode.Id=EmpProjTravel.CostCode ");
                SqlQuery.Append(" left join SystemListValue SLVProjTravType on SLVProjTravType.Id=EmpProjTravel.Type ");

                SqlQuery.Append(" left join Projects EmpProjTravelProject on EmpProjTravel.Project=EmpProjTravelProject.Id ");
                SqlQuery.Append(" Left Join SystemListValue SLVEmpProjTravelCountry on SLVEmpProjTravelCountry.Id=EmpProjTravelProject.Country ");
                SqlQuery.Append(" Left Join SystemListValue SLVEmpProjTravelLocation on SLVEmpProjTravelLocation.Id=EmpProjTravelProject.Location ");
                SqlQuery.Append(" Left Join SystemListValue SLVEmpProjTravelBlock on SLVEmpProjTravelBlock.Id=EmpProjTravelProject.Block ");
                SqlQuery.Append(" Left Join SystemListValue SLVEmpProjTravelTaxZone on SLVEmpProjTravelTaxZone.Id=EmpProjTravelProject.TaxZone ");
            }
            bool IsWhereCluse = true;
            if (model.selectTableId == 16)
            {
                SqlQuery.Append(" where ");
                IsWhereCluse = false;
                SqlQuery.Append("ASPCust.SSOID like 'C%'");
            }
            else if (model.selectTableId == 31)
            {
                SqlQuery.Append(" where ");
                IsWhereCluse = false;
                SqlQuery.Append("SystemListID=50");
            }
            if (model.CustomerId != null && model.CustomerId != 0)
            {
                SqlQuery.Append("left join AspNetUsers asp on cast(ANU.Id as nvarchar) in (select Items from dbo.Split(asp.CustomerCareID, ',')) where asp.Id='" + model.CustomerId + "'");
                IsWhereCluse = false;
            }
            if (model.FirstName != null)
            {
                if (model.selectTableId != 17 && model.selectTableId != 13 && model.selectTableId != 14 && model.selectTableId != 16 && model.selectTableId != 32 && model.selectTableId != 26)
                {
                    if (IsWhereCluse)
                    {
                        SqlQuery.Append(" where ");
                    }
                    else
                    {
                        if (model.AllAnd == "on")
                        {
                            SqlQuery.Append(" And ");
                        }
                        else
                        {
                            SqlQuery.Append(" Or ");
                        }
                    }
                IsWhereCluse = false;
                SqlQuery.Append("ANU.FirstName like '%" + model.FirstName + "%'");
                }
            }        
            if(model.LastName!=null)
            {
                if (model.selectTableId != 17 && model.selectTableId != 13 && model.selectTableId != 14 && model.selectTableId != 16 && model.selectTableId != 32 && model.selectTableId != 26)
                {
                    if (IsWhereCluse)
                    {
                        SqlQuery.Append(" where ");
                    }
                    else
                    {
                        if (model.AllAnd == "on")
                        {
                            SqlQuery.Append(" And ");
                        }
                        else
                        {
                            SqlQuery.Append(" Or ");
                        }
                    }
                    IsWhereCluse = false;
                    SqlQuery.Append("ANU.LastName like '%" + model.LastName + "%'");
                }
            }
            if (model.startDate != null)
            {
                //string[] Alias = model.columnString.Split('.');
                if (model.selectTableId != 17 && model.selectTableId != 13 && model.selectTableId != 14 && model.selectTableId != 16 && model.selectTableId != 32 && model.selectTableId != 26)
                {
                    string Alias = FindAliasFromString(model.columnString);
                    if (IsWhereCluse)
                    {
                        SqlQuery.Append(" where ");
                    }
                    else
                    {
                        if (model.AllAnd == "on")
                        {
                            SqlQuery.Append(" And ");
                        }
                        else
                        {
                            SqlQuery.Append(" Or ");
                        }
                    }
                    IsWhereCluse = false;
                    DateTime FromSeDate = DateTime.ParseExact(model.startDate, inputFormat, CultureInfo.InvariantCulture);
                    SqlQuery.Append(Alias + ".StartDate >= '" + FromSeDate + "'");
                }
            }
            if (model.endDate != null)
            {
                if (model.selectTableId != 17 && model.selectTableId != 13 && model.selectTableId != 14 && model.selectTableId != 16 && model.selectTableId != 32 && model.selectTableId != 26)
                {
                    string Alias = FindAliasFromString(model.columnString);
                    if (IsWhereCluse)
                    {
                        SqlQuery.Append(" where ");
                    }
                    else
                    {
                        if (model.AllAnd == "on")
                        {
                            SqlQuery.Append(" And ");
                        }
                        else
                        {
                            SqlQuery.Append(" Or ");
                        }
                    }
                    IsWhereCluse = false;
                    DateTime FromSeDate = DateTime.ParseExact(model.endDate, inputFormat, CultureInfo.InvariantCulture);
                    SqlQuery.Append(Alias + ".EndDate <= '" + FromSeDate + "'");
                }
            }
            if (model.BussinessId != null && model.BussinessId != 0)
            {
                if (model.selectTableId != 17 && model.selectTableId != 13 && model.selectTableId != 14 && model.selectTableId != 16 && model.selectTableId != 32 && model.selectTableId != 26)
                {
                    if (IsWhereCluse)
                    {
                        SqlQuery.Append(" where ");
                    }
                    else
                    {
                        if (model.AllAnd == "on")
                        {
                            SqlQuery.Append(" And ");
                        }
                        else
                        {
                            SqlQuery.Append(" Or ");
                        }
                    }
                    IsWhereCluse = false;
                    SqlQuery.Append("ER.BusinessID ='" + model.BussinessId + "'");
                }
            }
            if (model.DivisionId != null && model.DivisionId != 0)
            {
                if (model.selectTableId != 17 && model.selectTableId != 13 && model.selectTableId != 14 && model.selectTableId != 16 && model.selectTableId != 32 && model.selectTableId != 26)
                {
                    if (IsWhereCluse)
                    {
                        SqlQuery.Append(" where ");
                    }
                    else
                    {
                        if (model.AllAnd == "on")
                        {
                            SqlQuery.Append(" And ");
                        }
                        else
                        {
                            SqlQuery.Append(" Or ");
                        }
                    }
                    IsWhereCluse = false;
                    SqlQuery.Append("ER.DivisionID ='" + model.DivisionId + "'");
                }
            }
            if (model.PoolId != null && model.PoolId != 0)
            {
                if (model.selectTableId != 17 && model.selectTableId != 13 && model.selectTableId != 14 && model.selectTableId != 16 && model.selectTableId != 32 && model.selectTableId != 26)
                {
                    if (IsWhereCluse)
                    {
                        SqlQuery.Append(" where ");
                    }
                    else
                    {
                        if (model.AllAnd == "on")
                        {
                            SqlQuery.Append(" And ");
                        }
                        else
                        {
                            SqlQuery.Append(" Or ");
                        }
                    }
                    IsWhereCluse = false;
                    SqlQuery.Append("ER.PoolID ='" + model.PoolId + "'");
                }
            }
            if (model.FunctionId != null && model.FunctionId != 0)
            {
                if (model.selectTableId != 17 && model.selectTableId != 13 && model.selectTableId != 14 && model.selectTableId != 16 && model.selectTableId != 32 && model.selectTableId != 26)
                {
                    if (IsWhereCluse)
                    {
                        SqlQuery.Append(" where ");
                    }
                    else
                    {
                        if (model.AllAnd == "on")
                        {
                            SqlQuery.Append(" And ");
                        }
                        else
                        {
                            SqlQuery.Append(" Or ");
                        }
                    }
                    IsWhereCluse = false;
                    SqlQuery.Append("ER.FunctionID ='" + model.FunctionId + "'");
                }
            }
            if (model.JobTitleId != null && model.JobTitleId != 0)
            {
                if (model.selectTableId != 17 && model.selectTableId != 13 && model.selectTableId != 14 && model.selectTableId != 16 && model.selectTableId != 32 && model.selectTableId != 26)
                {
                    if (IsWhereCluse)
                    {
                        SqlQuery.Append(" where ");
                    }
                    else
                    {
                        if (model.AllAnd == "on")
                        {
                            SqlQuery.Append(" And ");
                        }
                        else
                        {
                            SqlQuery.Append(" Or ");
                        }
                    }
                    IsWhereCluse = false;
                    SqlQuery.Append("ANU.JobTitle ='" + model.JobTitleId + "'");
                }
            }

            if (!string.IsNullOrEmpty(model.columnString))
            {
                using (var ctx = new EvolutionEntities())
                using (var cmd = ctx.Database.Connection.CreateCommand())
                {
                    ctx.Database.Connection.Open();
                    cmd.CommandText = Convert.ToString(SqlQuery);
                    using (var reader = cmd.ExecuteReader())
                    {
                        var modell = Read(reader).ToList();
                        return PartialView("_partialQueryResult", modell);
                    }
                }
            }
            return PartialView(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]  
        public ActionResult SaveQuery(string ColumnString, List<string> p_ColumnNameArray,List<string> p_ColumnValueArray,int selectTableId,string DivAll,string DivAnyOf,string DivAllAnd,string DivAllOr,List<string> p_columnConditionArray)
        {            
            string ReplaceString = ColumnString.Replace(" ","");
            string[] ArrayofColumn = ReplaceString.Split(',');

            List<string> tableName = new List<string>();
            foreach (var item in ArrayofColumn)
            {
                tableName.Add(item.Split('.')[0]);
            }                        
            StringBuilder SqlQuery = new StringBuilder();
            if(selectTableId==17)
            {
                SqlQuery.Append("select distinct ");
                SqlQuery.Append(ColumnString);
                SqlQuery.Append(" from Company_Customer cmpCustomer");
                SqlQuery.Append(" Left Join Currencies cmpCun on cmpCustomer.Currency=cmpCun.Id ");
            }
            if (selectTableId == 13)
            {
                SqlQuery.Append("select distinct ");
                SqlQuery.Append(ColumnString);
                SqlQuery.Append(" from Projects Proj");
                SqlQuery.Append(" Left Join SystemListValue SLVProjecrCountry on SLVProjecrCountry.Id=Proj.Country ");
                SqlQuery.Append(" Left Join SystemListValue SLVProjecrLocation on SLVProjecrLocation.Id=Proj.Location ");
                SqlQuery.Append(" Left Join SystemListValue SLVProjecrBlock on SLVProjecrBlock.Id=Proj.Block ");
                SqlQuery.Append(" Left Join SystemListValue SLVProjecrTaxZone on SLVProjecrTaxZone.Id=Proj.TaxZone ");
            }
            else if(selectTableId==14)
            {
                SqlQuery.Append("select distinct ");
                SqlQuery.Append(ColumnString);
                SqlQuery.Append(" from Assets Asset");
                SqlQuery.Append(" Left Join SystemListValue SLVAsset on SLVAsset.Id=Asset.AssetType ");
                SqlQuery.Append(" Left Join SystemListValue SLVAsset2 on SLVAsset2.Id=Asset.AssetType2 ");
            }
            else if (selectTableId == 16)
            {
                SqlQuery.Append("select distinct ");
                SqlQuery.Append(ColumnString);
                SqlQuery.Append(" from AspNetUsers ASPCust");
                SqlQuery.Append(" Left Join SystemListValue SLVCustGen on SLVCustGen.Id=ASPCust.Gender ");
                SqlQuery.Append(" Left Join SystemListValue SLVCustJob on SLVCustJob.Id=ASPCust.JobTitle ");
                SqlQuery.Append(" Left Join SystemListValue SLVCustLoc on SLVCustLoc.Id=ASPCust.Location ");
            }
            else if(selectTableId==32)
            {
                SqlQuery.Append("select distinct ");
                SqlQuery.Append(ColumnString);
                SqlQuery.Append(" from SystemListValue SLVMasterCostCode");
            }
            else if(selectTableId==26)
            {
                SqlQuery.Append("select distinct ");
                SqlQuery.Append(ColumnString);
                SqlQuery.Append(" from TMS_Applicant TMSApp ");
                SqlQuery.Append(" Left Join Vacancy TMSVac on TMSVac.Id=TMSApp.VacancyID ");
                SqlQuery.Append(" Left Join TMS_Setting_RecruitmentProcesses TMSRecProcess on TMSRecProcess.Id=TMSVac.RecruitmentProcessID ");
                SqlQuery.Append(" Left Join AspNetUsers ANUAppHiringLead on ANUAppHiringLead.Id=TMSVac.HiringLeadID ");
                SqlQuery.Append(" Left Join SystemListValue SLVAppVacSource on SLVAppVacSource.Id=TMSVac.SourceID ");
                SqlQuery.Append(" Left Join ApplicantRejectReason AppRejReason on AppRejReason.Id=TMSApp.RejectReasonId ");
                SqlQuery.Append(" Left Join SystemListValue SLVAppGender on SLVAppGender.Id=TMSApp.GenderID ");
            }
            else
            {
                SqlQuery.Append("select distinct ");
                SqlQuery.Append(ColumnString);
                SqlQuery.Append(" from AspNetUsers ANU");
                SqlQuery.Append(" Left Join SystemListValue SLVcom on ANU.Company = SLVcom.Id ");
                SqlQuery.Append(" Left Join SystemListValue SLVTitle on SLVTitle.Id = ANU.NameTitle ");
                SqlQuery.Append(" left join SystemListValue SLVJob on SLVJob.Id=ANU.JobTitle ");
                SqlQuery.Append(" left join SystemListValue SLVNan on SLVNan.Id=ANU.Nationality ");
                SqlQuery.Append(" left join SystemListValue SLVRes on SLVRes.Id=ANU.ResourceType ");
                SqlQuery.Append(" left join SystemListValue SLVLoc on SLVLoc.Id=ANU.Location ");
                SqlQuery.Append(" left join SystemListValue SLVNotice on SLVNotice.Id=ANU.NoticePeriod ");
                SqlQuery.Append(" left join Countries COJob on COJob.Id=ANU.JobContryID ");
                SqlQuery.Append(" left join EmployeeAddressInfo ERD on ERD.UserID=ANU.Id");
                SqlQuery.Append(" left join EmployeeRelation ER on ER.Reportsto=ANU.Id");
                SqlQuery.Append(" left join Countries COUI on COUI.Id=ERD.CountryId ");
                SqlQuery.Append(" left join States ST on ST.Id = ERD.StateId ");
                SqlQuery.Append(" left join Cities CT on CT.Id = ERD.TownId ");
                //EmployeeRelation
                SqlQuery.Append(" left join Business BNU on BNU.Id=ER.BusinessID ");
                SqlQuery.Append(" left join Divisions DNU on DNU.Id=ER.DivisionID ");
                SqlQuery.Append(" left join Pools PNU on PNU.Id=ER.PoolID ");
                SqlQuery.Append(" left join Functions FNU on FNU.Id=ER.FunctionID ");
                //Salary Type
                SqlQuery.Append(" left join Employee_Salary EMSAL on EMSAL.EmployeeID = ANU.ID ");
                SqlQuery.Append(" left join SystemListValue SLVSAL on SLVSAL.Id = EMSAL.SalaryType ");
                SqlQuery.Append(" left join SystemListValue SLVPayFreq on SLVPayFreq.Id = EMSAL.PaymentFrequency ");
                SqlQuery.Append(" left join SystemListValue SLVReasonChange on SLVReasonChange.Id = EMSAL.ReasonforChange ");
                SqlQuery.Append(" left join SystemListValue SLVCurruncies on SLVCurruncies.Id = EMSAL.Currency ");
                //EmployeemeneDetails
                SqlQuery.Append(" left join SystemListValue SLVNoticePeriod on SLVNoticePeriod.Id = ANU.NoticePeriod ");
                //Contact Details
                SqlQuery.Append(" left join EmployeeAddressInfo EmpAddInfo on EmpAddInfo.UserId = ANU.Id ");
                SqlQuery.Append(" left join EmployeeBankInfo EmpBankInfo on EmpBankInfo.UserId = ANU.Id ");
                //EmployeeAnnualLeave
                SqlQuery.Append(" left join Employee_AnualLeave EmpAnuLev on EmpAnuLev.EmployeeId = ANU.Id ");
                //EmployeeOtherLeave
                SqlQuery.Append(" left join Employee_OtherLeave EmpOthElv on EmpOthElv.EmployeeId = ANU.Id ");
                SqlQuery.Append(" left join SystemListValue SLVOtherLeave on SLVOtherLeave.Id = EmpOthElv.ReasonForLeaveId ");
                //EmployeeSickLeave
                SqlQuery.Append(" left join Employee_SickLeaves EmpSickLev on EmpSickLev.EmployeeId = ANU.Id ");
                SqlQuery.Append(" left join Employee_SickLeaves_Comments EmpSickLevCmt on EmpSickLevCmt.SickLeaveID = EmpSickLev.Id ");
                //Maternity/PaternityLeave
                SqlQuery.Append(" left join Employee_MaternityOrPaternityLeaves EmpMatPatLev on EmpMatPatLev.EmployeeId = ANU.Id ");
                //Late Leave
                SqlQuery.Append(" left join Employee_LateLeave EmpLateLev on EmpLateLev.EmployeeId = ANU.Id ");
                //Scheduling
                SqlQuery.Append(" left join Employee_ProjectPlanner_Scheduling EmpProjPlnSch on EmpProjPlnSch.EmployeeId = ANU.Id ");
                SqlQuery.Append(" left join SystemListValue SLVSchAsset on SLVSchAsset.Id=EmpProjPlnSch.AssetId ");
                SqlQuery.Append(" left join AspNetUsers ASPSChCust on ASPSChCust.Id=EmpProjPlnSch.CustomerId ");

                SqlQuery.Append(" left join Projects SchedulingProject on EmpProjPlnSch.ProjectId=SchedulingProject.Id ");
                SqlQuery.Append(" Left Join SystemListValue SLVSchedulingCountry on SLVSchedulingCountry.Id=SchedulingProject.Country  ");
                SqlQuery.Append(" Left Join SystemListValue SLVSchedulingLocation on SLVSchedulingLocation.Id=SchedulingProject.Location ");
                SqlQuery.Append(" Left Join SystemListValue SLVSchedulingBlock on SLVSchedulingBlock.Id=SchedulingProject.Block ");
                SqlQuery.Append(" Left Join SystemListValue SLVSchedulingTaxZone on SLVSchedulingTaxZone.Id=SchedulingProject.TaxZone ");
                //Benefit
                SqlQuery.Append(" left join Benefit EmpBenefit on EmpBenefit.EmployeeId = ANU.Id ");
                SqlQuery.Append(" left join SystemListValue SLVEmpBenefit on SLVEmpBenefit.Id = EmpBenefit.BenefitID ");
                //Certification
                SqlQuery.Append(" left join certificate EmpCertificate on EmpCertificate.AssignTo = ANU.Id ");
                SqlQuery.Append(" left join SystemListValue SLVEmpCertificate on SLVEmpCertificate.Id = EmpCertificate.Type ");
                SqlQuery.Append(" left join AspNetUsers ASPCertificateInRelationTo on SLVEmpCertificate.Id = EmpCertificate.Type ");
                //visa
                SqlQuery.Append(" left join Visa EmpVisa on EmpVisa.AssignedToEmployeeId = ANU.Id ");
                SqlQuery.Append(" left join AspNetUsers aspRelationTo on aspRelationTo.Id = EmpVisa.RelationToCSEmployeeID ");
                SqlQuery.Append(" left join SystemListValue SLVEmpVisa on SLVEmpVisa.Id = EmpVisa.VisaType ");
                SqlQuery.Append(" left join SystemListValue SLVEmpVisaAggencies on SLVEmpVisaAggencies.Id = EmpVisa.ServiceAgency ");
                SqlQuery.Append(" left join Countries VisaCoun on VisaCoun.Id = EmpVisa.Country ");
                //Task
                SqlQuery.Append(" left join Task_List EmpTask on EmpTask.AssignTo = ANU.Id ");
                SqlQuery.Append(" left join AspNetUsers aspTaskRelationTo on aspTaskRelationTo.Id = EmpTask.InRelationTo ");
                SqlQuery.Append(" left join SystemListValue SLVEmpTaskCat on SLVEmpTaskCat.Id=EmpTask.Category ");
                SqlQuery.Append(" left join SystemListValue SLVEmpTaskStatus on SLVEmpTaskStatus.Id=EmpTask.Status ");
                //Cases
                SqlQuery.Append(" left join Cases EmpCases on EmpCases.EmployeeID = ANU.Id ");
                SqlQuery.Append(" left join SystemListValue SLVCaseCat on SLVCaseCat.Id=EmpCases.Category ");
                SqlQuery.Append(" left join SystemListValue SLVEmpCaseStatus on SLVEmpCaseStatus.Id=EmpCases.Status ");
                //Training
                SqlQuery.Append(" left join EmployeeTraining EmpTrain on EmpTrain.EmployeeId = ANU.Id ");
                SqlQuery.Append(" left join SystemListValue SLVTrainStatus on SLVTrainStatus.Id=EmpTrain.Status ");
                //Document
                SqlQuery.Append(" left join Employee_Document EmpDoc on EmpDoc.EmployeeId = ANU.Id ");
                SqlQuery.Append(" left join SystemListValue SLVDocCat on SLVDocCat.Id=EmpDoc.Category ");
                //WorkPattern
                SqlQuery.Append(" left join Employee_WorkPattern EmpWP on EmpWP.EmployeeID = ANU.Id ");
                SqlQuery.Append(" left join WorkPatterns worPattern on worPattern.Id=EmpWP.WorkPatternID ");
                //Skill 
                SqlQuery.Append(" left join Employee_Skills EmpSkill on EmpSkill.EmployeeId = ANU.Id ");
                SqlQuery.Append(" left join SkillSets GenSkillSet on GenSkillSet.Id=EmpSkill.GeneralSkillsName ");
                SqlQuery.Append(" left join SkillSets TechSkillSet on TechSkillSet.Id=EmpSkill.TechnicalSkillsName ");
                //Uplift
                SqlQuery.Append(" left join Employee_ProjectPlanner_Uplift EmpUplift on EmpUplift.EmployeeId = ANU.Id ");
                SqlQuery.Append(" left join Employee_ProjectPlanner_Uplift_Detail EmpUpliftDetail on EmpUpliftDetail.UpliftId=EmpUplift.Id ");
                SqlQuery.Append(" left join SystemListValue SLVUpliftPos on SLVUpliftPos.Id=EmpUplift.UpliftPostionId ");
                //Activity Type
                SqlQuery.Append(" left join ActivityTypes ActType on ActType.Id=ANU.ActivityType ");
                SqlQuery.Append(" left join SystemListValue SLVActWorkCurr on SLVActWorkCurr.Id=ActType.CurrencyID ");
                SqlQuery.Append(" left join SystemListValue SLVActWorkrate on SLVActWorkrate.Id=ActType.WorkUnitID ");
                //Timesheet
                SqlQuery.Append(" left join Employee_TimeSheet EmpTimesheet on EmpTimesheet.EmployeeId=ANU.Id ");
                SqlQuery.Append(" left join Employee_TimeSheet_Detail EmpTimeshetDetail on EmpTimeshetDetail.TimeSheetId=EmpTimesheet.Id ");
                SqlQuery.Append(" left join SystemListValue EmptimeAssetType on EmptimeAssetType.Id=EmpTimeshetDetail.Asset ");
                SqlQuery.Append(" left join SystemListValue EmptimeCostCode on EmptimeCostCode.Id=EmpTimeshetDetail.CostCode ");
                SqlQuery.Append(" left join AspNetUsers EmptimeCustomer on EmptimeCustomer.Id=EmpTimeshetDetail.Customer ");

                SqlQuery.Append(" left join Projects TimeSheetProject on EmpTimeshetDetail.Project=TimeSheetProject.Id ");
                SqlQuery.Append(" Left Join SystemListValue SLVTimeSheetProjectCountry on SLVTimeSheetProjectCountry.Id=TimeSheetProject.Country ");
                SqlQuery.Append(" Left Join SystemListValue SLVTimeSheetProjectLocation on SLVTimeSheetProjectLocation.Id=TimeSheetProject.Location ");
                SqlQuery.Append(" Left Join SystemListValue SLVTimeSheetProjectBlock on SLVTimeSheetProjectBlock.Id=TimeSheetProject.Block ");
                SqlQuery.Append(" Left Join SystemListValue SLVTimeSheetProjectTaxZone on SLVTimeSheetProjectTaxZone.Id=TimeSheetProject.TaxZone ");

                //Project Planner Timesheet
                SqlQuery.Append(" left join Employee_ProjectPlanner_TimeSheet EmpProjTimesheet on EmpProjTimesheet.EmployeeId=ANU.Id ");
                SqlQuery.Append(" left join Employee_ProjectPlanner_TimeSheet_Detail EmpProjTimeshetDetail on EmpProjTimeshetDetail.TimeSheetId=EmpProjTimesheet.Id ");
                SqlQuery.Append(" left join SystemListValue EmpProjtimeAssetType on EmpProjtimeAssetType.Id=EmpProjTimeshetDetail.Asset ");
                SqlQuery.Append(" left join SystemListValue EmpProjtimeCostCode on EmpProjtimeCostCode.Id=EmpProjTimeshetDetail.CostCode ");
                SqlQuery.Append(" left join AspNetUsers EmpProjtimeCustomer on EmpProjtimeCustomer.Id=EmpProjTimeshetDetail.Customer ");

                SqlQuery.Append(" left join Projects EmpProjTimeshetProject on EmpProjTimeshetDetail.Project=EmpProjTimeshetProject.Id ");
                SqlQuery.Append(" Left Join SystemListValue SLVEmpProjTimeshetCountry on SLVEmpProjTimeshetCountry.Id=EmpProjTimeshetProject.Country ");
                SqlQuery.Append(" Left Join SystemListValue SLVEmpProjTimeshetLocation on SLVEmpProjTimeshetLocation.Id=EmpProjTimeshetProject.Location ");
                SqlQuery.Append(" Left Join SystemListValue SLVEmpProjTimeshetBlock on SLVEmpProjTimeshetBlock.Id=EmpProjTimeshetProject.Block ");
                SqlQuery.Append(" Left Join SystemListValue SLVEmpProjTimeshetTaxZone on SLVEmpProjTimeshetTaxZone.Id=EmpProjTimeshetProject.TaxZone ");
                //Travel
                SqlQuery.Append(" left join  Employee_TravelLeave EmpTravel on ANU.Id=EmpTravel.EmployeeId ");
                SqlQuery.Append(" left join Countries EmpTraFromCoun on EmpTraFromCoun.Id=EmpTravel.FromCountryId ");
                SqlQuery.Append(" left join Countries EmpTraToCoun on EmpTraToCoun.Id=EmpTravel.ToCountryId ");
                SqlQuery.Append(" left join States EmpTraFromState on EmpTraFromState.Id=EmpTravel.FromStateId ");
                SqlQuery.Append(" left join States EmpTraToState on EmpTraToState.Id=EmpTravel.ToStateId ");
                SqlQuery.Append(" left join Cities EmpTraFromCity on EmpTraFromCity.Id=EmpTravel.FromTownId ");
                SqlQuery.Append(" left join Cities EmpTraToCity on EmpTraToCity.Id=EmpTravel.ToTownId ");
                SqlQuery.Append(" left join SystemListValue SLVTravReason on SLVTravReason.Id=EmpTravel.ReasonForTravelId ");
                SqlQuery.Append(" left join SystemListValue SLVTravCostCode on SLVTravCostCode.Id=EmpTravel.CostCode ");
                SqlQuery.Append(" left join SystemListValue SLVTravType on SLVTravType.Id=EmpTravel.Type ");

                SqlQuery.Append(" left join Projects TravelProject on EmpTravel.Project=TravelProject.Id ");
                SqlQuery.Append(" Left Join SystemListValue SLVTravelProjecrCountry on SLVTravelProjecrCountry.Id=TravelProject.Country ");
                SqlQuery.Append(" Left Join SystemListValue SLVTravelProjecrLocation on SLVTravelProjecrLocation.Id=TravelProject.Location ");
                SqlQuery.Append(" Left Join SystemListValue SLVTravelProjecrBlock on SLVTravelProjecrBlock.Id=TravelProject.Block ");
                SqlQuery.Append(" Left Join SystemListValue SLVTravelProjecrTaxZone on SLVTravelProjecrTaxZone.Id=TravelProject.TaxZone ");
                //Project Planner Travel
                SqlQuery.Append(" left join  Employee_ProjectPlanner_TravelLeave EmpProjTravel on ANU.Id=EmpProjTravel.EmployeeId ");
                SqlQuery.Append(" left join Countries EmpProjTraFromCoun on EmpProjTraFromCoun.Id=EmpProjTravel.FromCountryId ");
                SqlQuery.Append(" left join Countries EmpProjTraToCoun on EmpProjTraToCoun.Id=EmpProjTravel.ToCountryId ");
                SqlQuery.Append(" left join States EmpProjTraFromState on EmpProjTraFromState.Id=EmpProjTravel.FromStateId ");
                SqlQuery.Append(" left join States EmpProjTraToState on EmpProjTraToState.Id=EmpProjTravel.ToStateId ");
                SqlQuery.Append(" left join Cities EmpProjTraFromCity on EmpProjTraFromCity.Id=EmpProjTravel.FromTownId ");
                SqlQuery.Append(" left join Cities EmpProjTraToCity on EmpProjTraToCity.Id=EmpProjTravel.ToTownId ");
                SqlQuery.Append(" left join SystemListValue SLVProjTravReason on SLVProjTravReason.Id=EmpProjTravel.ReasonForTravelId ");
                SqlQuery.Append(" left join SystemListValue SLVProjTravCostCode on SLVProjTravCostCode.Id=EmpProjTravel.CostCode ");
                SqlQuery.Append(" left join SystemListValue SLVProjTravType on SLVProjTravType.Id=EmpProjTravel.Type ");

                SqlQuery.Append(" left join Projects EmpProjTravelProject on EmpProjTravel.Project=EmpProjTravelProject.Id ");
                SqlQuery.Append(" Left Join SystemListValue SLVEmpProjTravelCountry on SLVEmpProjTravelCountry.Id=EmpProjTravelProject.Country ");
                SqlQuery.Append(" Left Join SystemListValue SLVEmpProjTravelLocation on SLVEmpProjTravelLocation.Id=EmpProjTravelProject.Location ");
                SqlQuery.Append(" Left Join SystemListValue SLVEmpProjTravelBlock on SLVEmpProjTravelBlock.Id=EmpProjTravelProject.Block ");
                SqlQuery.Append(" Left Join SystemListValue SLVEmpProjTravelTaxZone on SLVEmpProjTravelTaxZone.Id=EmpProjTravelProject.TaxZone ");
            }
            bool IsWhereCluse = true;
            if(selectTableId==16)
            {
                SqlQuery.Append(" where ");
                IsWhereCluse = false;
                SqlQuery.Append("ASPCust.SSOID like 'C%'");
            }
            else if(selectTableId==31)
            {
                SqlQuery.Append(" where ");
                IsWhereCluse = false;
                SqlQuery.Append("SystemListID=50");
            }
            if (p_ColumnNameArray != null && p_ColumnValueArray != null && p_columnConditionArray != null)
            {
                for (int i = 0; i < p_ColumnNameArray.Count(); i++ )
                {
                    for (int j = 0; j < p_columnConditionArray.Count(); j++)
                    {
                        {
                            if (p_ColumnNameArray[i] == Convert.ToString((int)Filter.FirstName))
                            {
                                if (!string.IsNullOrEmpty(p_ColumnValueArray[i]))
                                {
                                    if (IsWhereCluse)
                                    {
                                        SqlQuery.Append(" where ");
                                    }
                                    else
                                    {
                                        if (p_columnConditionArray[j] == "And")
                                        {
                                            SqlQuery.Append(" And ");
                                        }
                                        else if (p_columnConditionArray[j] == "Or")
                                        {
                                            SqlQuery.Append(" Or ");
                                        }
                                    }
                                    IsWhereCluse = false;
                                    SqlQuery.Append("ANU.FirstName like '%" + p_ColumnValueArray[i] + "%'");
                                }
                            }
                            if (p_ColumnNameArray[i] == Convert.ToString((int)Filter.LastName))
                            {
                                if (!string.IsNullOrEmpty(p_ColumnValueArray[i]))
                                {
                                    if (IsWhereCluse)
                                    {
                                        SqlQuery.Append(" where ");
                                    }
                                    else
                                    {
                                        if (p_columnConditionArray[j] == "And")
                                        {
                                            SqlQuery.Append(" And ");
                                        }
                                        else if (p_columnConditionArray[j] == "Or")
                                        {
                                            SqlQuery.Append(" Or ");
                                        }
                                    }
                                    IsWhereCluse = false;
                                    SqlQuery.Append("ANU.LastName like '%" + p_ColumnValueArray[i] + "%'");
                                }
                            }
                            if (p_ColumnNameArray[i] == Convert.ToString((int)Filter.FromDate))
                            {
                                string inputFormat = "dd-MM-yyyy";
                                //string[] Alias = ColumnString.Split('.');
                                string Alias = FindAliasFromString(ColumnString);
                                if (!string.IsNullOrEmpty(p_ColumnValueArray[i]))
                                {
                                    if (IsWhereCluse)
                                    {
                                        SqlQuery.Append(" where ");
                                    }
                                    else
                                    {
                                        if (p_columnConditionArray[j] == "And")
                                        {
                                            SqlQuery.Append(" And ");
                                        }
                                        if (p_columnConditionArray[j] == "Or")
                                        {
                                            SqlQuery.Append(" Or ");
                                        }
                                    }

                                    IsWhereCluse = false;
                                    IsWhereCluse = false;
                                    DateTime FromSeDate = DateTime.ParseExact(p_ColumnValueArray[i], inputFormat, CultureInfo.InvariantCulture);
                                    SqlQuery.Append(Alias + ".StartDate >= '" + FromSeDate + "'");
                                }
                            }
                            if (p_ColumnNameArray[i] == Convert.ToString((int)Filter.ToDate))
                            {
                                string inputFormat = "dd-MM-yyyy";
                                string Alias = FindAliasFromString(ColumnString);
                                if (!string.IsNullOrEmpty(p_ColumnValueArray[i]))
                                {
                                    if (IsWhereCluse)
                                    {
                                        SqlQuery.Append(" where ");
                                    }
                                    else
                                    {
                                        if (p_columnConditionArray[j] == "And")
                                        {
                                            SqlQuery.Append(" And ");
                                        }
                                        if (p_columnConditionArray[j] == "Or")
                                        {
                                            SqlQuery.Append(" Or ");
                                        }
                                    }
                                    IsWhereCluse = false;
                                    IsWhereCluse = false;
                                    DateTime FromSeDate = DateTime.ParseExact(p_ColumnValueArray[i], inputFormat, CultureInfo.InvariantCulture);
                                    SqlQuery.Append(Alias + ".EndDate <= '" + FromSeDate + "'");
                                }
                            }
                            if (p_ColumnNameArray[i] == Convert.ToString((int)Filter.Business))
                            {
                                string[] Alias = ColumnString.Split('.');
                                if (!string.IsNullOrEmpty(p_ColumnValueArray[i]))
                                {
                                    if (IsWhereCluse)
                                    {
                                        SqlQuery.Append(" where ");
                                    }
                                    else
                                    {
                                        if (p_columnConditionArray[j] == "And")
                                        {
                                            SqlQuery.Append(" And ");
                                        }
                                        if (p_columnConditionArray[j] == "Or")
                                        {
                                            SqlQuery.Append(" Or ");
                                        }
                                    }
                                    IsWhereCluse = false;
                                    SqlQuery.Append("ER.BusinessID ='" + p_ColumnValueArray[i] + "'");
                                }
                            }
                            if (p_ColumnNameArray[i] == Convert.ToString((int)Filter.Division))
                            {
                                string[] Alias = ColumnString.Split('.');
                                if (!string.IsNullOrEmpty(p_ColumnValueArray[i]))
                                {
                                    if (IsWhereCluse)
                                    {
                                        SqlQuery.Append(" where ");
                                    }
                                    else
                                    {
                                        if (p_columnConditionArray[j] == "And")
                                        {
                                            SqlQuery.Append(" And ");
                                        }
                                        if (p_columnConditionArray[j] == "Or")
                                        {
                                            SqlQuery.Append(" Or ");
                                        }
                                    }
                                    IsWhereCluse = false;
                                    SqlQuery.Append("ER.DivisionID ='" + p_ColumnValueArray[i] + "'");
                                }
                            }
                            if (p_ColumnNameArray[i] == Convert.ToString((int)Filter.Pool))
                            {
                                if (!string.IsNullOrEmpty(p_ColumnValueArray[i]))
                                {
                                    if (IsWhereCluse)
                                    {
                                        SqlQuery.Append(" where ");
                                    }
                                    else
                                    {
                                        if (p_columnConditionArray[j] == "And")
                                        {
                                            SqlQuery.Append(" And ");
                                        }
                                        if (p_columnConditionArray[j] == "Or")
                                        {
                                            SqlQuery.Append(" Or ");
                                        }
                                    }
                                    IsWhereCluse = false;
                                    SqlQuery.Append("ER.PoolID ='" + p_ColumnValueArray[i] + "'");
                                }
                            }
                            if (p_ColumnNameArray[i] == Convert.ToString((int)Filter.Function))
                            {
                                string[] Alias = ColumnString.Split('.');
                                if (!string.IsNullOrEmpty(p_ColumnValueArray[i]))
                                {
                                    if (IsWhereCluse)
                                    {
                                        SqlQuery.Append(" where ");
                                    }
                                    else
                                    {
                                        if (p_columnConditionArray[j] == "And")
                                        {
                                            SqlQuery.Append(" And ");
                                        }
                                        if (p_columnConditionArray[j] == "Or")
                                        {
                                            SqlQuery.Append(" Or ");
                                        }
                                    }
                                    IsWhereCluse = false;
                                    SqlQuery.Append("ER.FunctionID ='" + p_ColumnValueArray[i] + "'");
                                }
                            }
                            //if (p_ColumnNameArray[i] == Convert.ToString((int)Filter.Function))
                            //{
                            //    string[] Alias = ColumnString.Split('.');
                            //    if (!string.IsNullOrEmpty(p_ColumnValueArray[i]))
                            //    {
                            //        if (IsWhereCluse)
                            //        {
                            //            SqlQuery.Append(" where ");
                            //        }
                            //        else
                            //        {
                            //            SqlQuery.Append(" And ");
                            //        }
                            //        IsWhereCluse = false;
                            //        SqlQuery.Append("left join AspNetUsers asp on cast(ANU.Id as nvarchar) in (select Items from dbo.Split(asp.CustomerCareID, ',')) where asp.Id='" + p_ColumnNameArray[i] + "'");
                            //    }
                            //}


                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(ColumnString))
            {
                using (var ctx = new EvolutionEntities())
                using (var cmd = ctx.Database.Connection.CreateCommand())
                {
                    ctx.Database.Connection.Open();
                    cmd.CommandText = Convert.ToString(SqlQuery);
                    using (var reader = cmd.ExecuteReader())
                    {
                        var model = Read(reader).ToList();
                        TempData["QueryText"] = cmd.CommandText;
                        return PartialView("_partialQueryResult", model);
                    }
                }
            }
            return PartialView(null, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult SaveQueryData(QueryDataSet model)
        {                                   
            int Id = _QueryMethod.saveQueryData(model);
            //QueryDataSet modelSet = returnQueryList();
            QueryViewModel modelQuery = new QueryViewModel();
            return PartialView("_partialAddQuery", modelQuery);
        }




        #region Add filter
        public ActionResult AddFilter(List<QueryFilter> p_QueryFilter)
        {
            _QueryFilter.Add(new QueryFilter());
            return PartialView("_partialAddFilter", _QueryFilter);
        }
        #endregion
        //Find Alias
        public string FindAliasFromString(string ColumnString)
        {
            string[] coumnstr = ColumnString.Split(',');
            string Alias = string.Empty;
            for (int i=0;i<coumnstr.Length;i++)
            {
                //for (int j = 0; j < coumnstr[i].Length; j++)
                //{
                    //string[] data = coumnstr[j].Split('.');
                    if(coumnstr[i].Contains(".StartDate"))
                    {
                        Alias = coumnstr[i].Split('.')[0];
                    }
               // }

            }            
            return Alias;
        }
        #region Set enum text value of correct formate
      
        public string GetTableValueText(string value)
        {
            string ValueText = string.Empty;
            if(value == "EmployeeDetails")
            {
                ValueText = "Employee Details";
            }
            if(value == "ContactDetails")
            {
                ValueText = "Contact Details";
            }
            if(value== "SalaryDetails")
            {
                ValueText = "Salary Details";
            }
            if (value == "EmploymentDetails")
            {
                ValueText = "Employment Details";
            }
            if (value == "AnnualLeaveEntitlement")
            {
                ValueText = "AnnualLeave Entitlement";
            }
            //if (value == "Holidays")
            //{
            //    ValueText = "Holidays";
            //}
            if (value == "OtherLeave")
            {
                ValueText = "Other Leave";
            }
            if (value == "Absences")
            {
                ValueText = "Absences (Sick Leave)";
            }
            if (value == "Maternity_Paternity")
            {
                ValueText = "Maternity/Paternity";
            }
            if (value == "Late")
            {
                ValueText = "Late";
            }
            if (value == "Scheduling")
            {
                ValueText = "Scheduling";
            }
            if (value == "Timesheet")
            {
                ValueText = "Timesheet";
            }
        
            if (value == "Assets")
            {
                ValueText = "Assets";
            }
            if (value == "ActivityTypes")
            {
                ValueText = "Activity Types";
            }
            if (value == "CustomerCompany")
            {
                ValueText = "Customer Company";
            }
            if (value == "CustomerUser")
            {
                ValueText = "Customer User";
            }
            if (value == "Travel")
            {
                ValueText = "Travel";
            }
            if (value == "Uplift")
            {
                ValueText = "Uplift";
            }
            if (value == "Benefit")
            {
                ValueText = "Benefit";
            }
            if (value == "Certification")
            {
                ValueText = "Certification";
            }
            if (value == "Visa")
            {
                ValueText = "Visa";
            }
            if (value == "Tasks")
            {
                ValueText = "Tasks";
            }
            if (value == "Cases")
            {
                ValueText = "Cases";
            }
            if (value == "Training")
            {
                ValueText = "Training";
            }
            if (value == "TMSApplicant")
            {
                ValueText = "Applicant";
            }
            if (value == "Document")
            {
                ValueText = "Document";
            }
            if (value == "WorkPattern")
            {
                ValueText = "WorkPattern";
            }
            if (value == "Skills")
            {
                ValueText = "Skills";
            }
            if (value == "Project")
            {
                ValueText = "Project";
            }
            if(value== "ProjectPlannerTimesheet")
            {
                ValueText = "Project Planner Timesheet";
            }
            if (value == "ProjectPlannerTravel")
            {
                ValueText = "Project Planner Travel";
            }
            if (value == "CostCode")
            {
                ValueText = "Cost Code";
            }
            return ValueText;
        }
        #endregion
       
        public enum TableList
        {
            EmployeeDetails = 1,          
            CustomerUser=16,
            CustomerCompany=17,            
            TMSApplicant=26,            
            Project = 13,
            Assets = 14,            
            CostCode=32,
            //Employee_AddEndrosementSkills=29,
            //SkillSets=29,
            //Employee_Skills=29
        }
        public enum ChildTable
        {
            ProjectPlannerTimesheet = 30,
            ProjectPlannerTravel = 31,
            Document = 27,
            WorkPattern = 28,
            Skills = 29,
            Travel = 18,
            Uplift = 19,
            Benefit = 20,
            Certification = 21,
            Visa = 22,
            Tasks = 23,
            Cases = 24,
            Training = 25,
            AnnualLeaveEntitlement = 5,
           // Holidays = 6,
            OtherLeave = 7,
            Absences = 8,
            Maternity_Paternity = 9,
            Late = 10,
            Scheduling = 11,
            Timesheet = 12,
            ContactDetails = 2,
            SalaryDetails = 3,
            EmploymentDetails = 4,
            ActivityTypes = 15,
        }
        public enum Filter
        {
            FirstName = 1,
            LastName = 2,
            FromDate = 3,
            ToDate = 4,
            Business=5,
            Division=6,
            Pool=7,
            Function=8,
            Customer=9
        }
       public enum FilterCondition
        {
            And=1,
            Or=2
        }

        public List<Dictionary<string, object>> Read(DbDataReader reader)
        {
            List<Dictionary<string, object>> expandolist = new List<Dictionary<string, object>>();
            foreach (var item in reader)
            {
                IDictionary<string, object> expando = new System.Dynamic.ExpandoObject();
                foreach (System.ComponentModel.PropertyDescriptor propertyDescriptor in System.ComponentModel.TypeDescriptor.GetProperties(item))
                {
                    var obj = propertyDescriptor.GetValue(item);
                    expando.Add(propertyDescriptor.Name, obj);
                }
                expandolist.Add(new Dictionary<string, object>(expando));
            }
            return expandolist;
        }

    }
}