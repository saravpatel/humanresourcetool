using HRTool.DataModel;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace HRTool.CommanMethods.Settings
{
    public class TMSSettingsMethod
    {
        #region Constant

        EvolutionEntities _db = new EvolutionEntities();

        #endregion

        #region TMS Settings Methods
        public TMS_Setting_RecruitmentProcesses getTMSSettingListById(int Id)
        {
            return _db.TMS_Setting_RecruitmentProcesses.Where(x => x.Id == Id).FirstOrDefault();

        }
        public List<TMS_Setting_RecruitmentProcesses> getAllTMSSettingList()
        {
            return _db.TMS_Setting_RecruitmentProcesses.Where(x => x.Archived == false).ToList();
        }

        public List<TMS_Setting_RecruitmentProcesses> GetTMSList()
        {
            return _db.TMS_Setting_RecruitmentProcesses.Where(x => x.Archived == false).ToList();
        }
        public bool SaveTMSSettingData(TMSSettingsViewModel Model, int UserId)
        {
            var data = _db.TMS_Setting_RecruitmentProcesses.Where(x => x.Name == Model.Name && x.Id != Model.Id && x.Archived == false).ToList();
            if (data.Count > 0)
            {
                return false;
            }
            else
            {
                if (Model.Id == 0)
                {
                    TMS_Setting_RecruitmentProcesses save = new TMS_Setting_RecruitmentProcesses();
                    save.Name = Model.Name;
                    save.StepCSV = Model.StepCSV;
                    save.CompetencyCSV = Model.CompetencyCSV;
                    save.CreatedDate = DateTime.Now;
                    save.Archived = false;
                    save.UserIDCreatedBy = UserId;
                    save.UserIDLastModifiedBy = UserId;
                    save.LastModified = DateTime.Now;
                    _db.TMS_Setting_RecruitmentProcesses.Add(save);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    var update = _db.TMS_Setting_RecruitmentProcesses.Where(x => x.Id == Model.Id).FirstOrDefault();
                    update.Name = Model.Name;
                    update.StepCSV = Model.StepCSV;
                    update.CompetencyCSV = Model.CompetencyCSV;
                    update.UserIDLastModifiedBy = UserId;
                    update.LastModified = DateTime.Now;
                    _db.SaveChanges();
                    return true;
                }
            }
        }

        public bool deleteTMSSetting(int Id, int UserId)
        {
            var data = _db.TMS_Setting_RecruitmentProcesses.Where(x => x.Id == Id).FirstOrDefault();
            data.Archived = true;
            data.UserIDLastModifiedBy = UserId;
            data.LastModified = DateTime.Now;
            _db.SaveChanges();
            return true;

        }
        public bool UpdateStepMoveOfCometencies(string AllCompetencyStep, int SortId, int RecId, int flagUpDown)
        {
            if (flagUpDown == 0)
            {
                if (RecId != 0)
                {
                    var res = _db.TMS_Setting_RecruitmentProcesses.Where(x => x.Id == RecId && x.Archived == false).FirstOrDefault();
                    var steps = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(AllCompetencyStep);
                    //var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(res.StepCSV);
                    List<TMSSettingCompetencyDetails> StepsAdd = new List<TMSSettingCompetencyDetails>();
                    foreach (var s in steps)
                    {
                        TMSSettingCompetencyDetails ss = new TMSSettingCompetencyDetails();
                        ss.Id = s.Id;
                        int Totalstep = steps.Count();
                        int srt = SortId + 1;
                        if (s.SortId == SortId + 1 && srt <= Totalstep)
                        {
                            ss.SortId = SortId;
                        }
                        else if (s.SortId == SortId && srt <= Totalstep)
                        {
                            ss.SortId = SortId + 1;
                        }
                        else
                        {
                            ss.SortId = s.SortId;
                        }
                        ss.CompetencyName = s.CompetencyName;
                        ss.Description = s.Description;
                        StepsAdd.Add(ss);
                    }
                    var ConvertJsonStepCSV = JsonConvert.SerializeObject(StepsAdd);
                    res.CompetencyCSV = ConvertJsonStepCSV;
                    res.LastModified = DateTime.Now;
                    _db.SaveChanges();
                }
            }
            else
            {
                if (RecId != 0)
                {
                    var res = _db.TMS_Setting_RecruitmentProcesses.Where(x => x.Id == RecId && x.Archived == false).FirstOrDefault();
                    var steps = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(AllCompetencyStep);
                    //var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(res.StepCSV);
                    List<TMSSettingCompetencyDetails> StepsAdd = new List<TMSSettingCompetencyDetails>();
                    foreach (var s in steps)
                    {
                        TMSSettingCompetencyDetails ss = new TMSSettingCompetencyDetails();
                        ss.Id = s.Id;
                        int srt = SortId - 1;
                        if (s.SortId == SortId - 1 && srt != 0)
                        {
                            ss.SortId = SortId;
                        }
                        else if (s.SortId == SortId && srt != 0)
                        {
                            ss.SortId = SortId - 1;
                        }
                        else
                        {
                            ss.SortId = s.SortId;
                        }
                        ss.CompetencyName = s.CompetencyName;
                        ss.Description = s.Description;
                        StepsAdd.Add(ss);
                    }
                    var ConvertJsonStepCSV = JsonConvert.SerializeObject(StepsAdd);
                    res.CompetencyCSV = ConvertJsonStepCSV;
                    res.LastModified = DateTime.Now;
                    _db.SaveChanges();
                }
            }
            return true;
        }
        public bool UpdateStepMoveOfRecProcess(string AllStepSegmentJsonm, int SortId, int RecId, int flagUpDown)
        {
            //0=Down
            if (flagUpDown == 0)
            {
                if (RecId != 0)
                {
                    var res = _db.TMS_Setting_RecruitmentProcesses.Where(x => x.Id == RecId && x.Archived == false).FirstOrDefault();
                    var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(AllStepSegmentJsonm);
                    //var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(res.StepCSV);
                    List<TMSSettingStepDetails> StepsAdd = new List<TMSSettingStepDetails>();
                    foreach (var s in steps)
                    {
                        TMSSettingStepDetails ss = new TMSSettingStepDetails();
                        ss.Id = s.Id;
                        int Totalstep = steps.Count();
                        int srt = SortId + 1;
                        if (s.SortId == SortId + 1 && srt != Totalstep)
                        {
                            ss.SortId = SortId;
                        }
                        else if (s.SortId == SortId && srt != Totalstep)
                        {
                            ss.SortId = SortId + 1;
                        }
                        else
                        {
                            ss.SortId = s.SortId;
                        }
                        ss.StepName = s.StepName;
                        ss.ColorCode = s.ColorCode;
                        StepsAdd.Add(ss);
                    }
                    var ConvertJsonStepCSV = JsonConvert.SerializeObject(StepsAdd);
                    res.StepCSV = ConvertJsonStepCSV;
                    res.LastModified = DateTime.Now;
                    _db.SaveChanges();
                }
            }
            //1=Up
            else if (flagUpDown == 1)
            {
                if (RecId != 0)
                {
                    var res = _db.TMS_Setting_RecruitmentProcesses.Where(x => x.Id == RecId && x.Archived == false).FirstOrDefault();
                    //var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(res.StepCSV);
                    var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(AllStepSegmentJsonm);
                    List<TMSSettingStepDetails> StepsAdd = new List<TMSSettingStepDetails>();
                    foreach (var s in steps)
                    {
                        TMSSettingStepDetails ss = new TMSSettingStepDetails();
                        ss.Id = s.Id;
                        int srt = SortId - 1;
                        if (s.SortId == srt && srt != 0)
                        {
                            ss.SortId = SortId;
                        }
                        else if (s.SortId == SortId && srt != 0)
                        {
                            ss.SortId = SortId - 1;
                        }
                        else
                        {
                            ss.SortId = s.SortId;
                        }
                        ss.StepName = s.StepName;
                        ss.ColorCode = s.ColorCode;
                        StepsAdd.Add(ss);
                    }
                    var ConvertJsonStepCSV = JsonConvert.SerializeObject(StepsAdd);
                    res.StepCSV = ConvertJsonStepCSV;
                    res.LastModified = DateTime.Now;
                    _db.SaveChanges();
                }
            }
            return true;
        }


        public List<TMSSettingStepDetails> UpdateStepMove(string AllStepSegmentJsonm, int SortId, int RecId, int flagUpDown)
        {
            List<TMSSettingStepDetails> StepsAdd = new List<TMSSettingStepDetails>();
            if (flagUpDown == 0)
            {
                var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(AllStepSegmentJsonm);
                foreach (var s in steps)
                {
                    TMSSettingStepDetails ss = new TMSSettingStepDetails();
                    ss.Id = s.Id;
                    int Totalstep = steps.Count();
                    int srt = SortId + 1;
                    if (s.SortId == SortId + 1 && srt != Totalstep)
                    {
                        ss.SortId = SortId;
                    }
                    else if (s.SortId == SortId && srt != Totalstep)
                    {
                        ss.SortId = SortId + 1;
                    }
                    else
                    {
                        ss.SortId = s.SortId;
                    }
                    ss.StepName = s.StepName;
                    ss.ColorCode = s.ColorCode;
                    StepsAdd.Add(ss);
                }
                return StepsAdd.ToList();
            }
            //1=Up
            else if (flagUpDown == 1)
            {
                var steps = JsonConvert.DeserializeObject<List<TMSSettingStepDetails>>(AllStepSegmentJsonm);
                foreach (var s in steps)
                {
                    TMSSettingStepDetails ss = new TMSSettingStepDetails();
                    ss.Id = s.Id;
                    int srt = SortId - 1;
                    if (s.SortId == srt && srt != 0)
                    {
                        ss.SortId = SortId;
                    }
                    else if (s.SortId == SortId && srt != 0)
                    {
                        ss.SortId = SortId - 1;
                    }
                    else
                    {
                        ss.SortId = s.SortId;
                    }
                    ss.StepName = s.StepName;
                    ss.ColorCode = s.ColorCode;
                    StepsAdd.Add(ss);
                }
                return StepsAdd.ToList();
            }

            return StepsAdd.ToList();
        }


        #endregion

        public List<TMSSettingCompetencyDetails> UpdateStepCometencies(string AllCompetencyStep, int SortId, int RecId, int flagUpDown)
        {
            List<TMSSettingCompetencyDetails> StepsAdd = new List<TMSSettingCompetencyDetails>();
            if (flagUpDown == 0)
            {
                var steps = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(AllCompetencyStep);
                foreach (var s in steps)
                {
                    TMSSettingCompetencyDetails ss = new TMSSettingCompetencyDetails();
                    ss.Id = s.Id;
                    int Totalstep = steps.Count();
                    int srt = SortId + 1;
                    if (s.SortId == SortId + 1 && srt != Totalstep)
                    {
                        ss.SortId = SortId;
                    }
                    else if (s.SortId == SortId && srt != Totalstep)
                    {
                        ss.SortId = SortId + 1;
                    }
                    else
                    {
                        ss.SortId = s.SortId;
                    }
                    ss.CompetencyName = s.CompetencyName;
                    ss.Description = s.Description;
                    StepsAdd.Add(ss);
                }
                return StepsAdd.ToList();
            }
            //1=Up
            else if (flagUpDown == 1)
            {
                var steps = JsonConvert.DeserializeObject<List<TMSSettingCompetencyDetails>>(AllCompetencyStep);
                foreach (var s in steps)
                {
                    TMSSettingCompetencyDetails ss = new TMSSettingCompetencyDetails();
                    ss.Id = s.Id;
                    int srt = SortId - 1;
                    if (s.SortId == srt && srt != 0)
                    {
                        ss.SortId = SortId;
                    }
                    else if (s.SortId == SortId && srt != 0)
                    {
                        ss.SortId = SortId - 1;
                    }
                    else
                    {
                        ss.SortId = s.SortId;
                    }
                    ss.CompetencyName = s.CompetencyName;
                    ss.Description = s.Description;
                    StepsAdd.Add(ss);
                }
                return StepsAdd.ToList();
            }

            return StepsAdd.ToList();
        }

    }

}