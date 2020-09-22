using HRTool.DataModel;
using HRTool.Models;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.CommanMethods.Settings
{
    public class EmailSettingMethod
    {
        #region constant
        EmailSettingViewModel EmailSettingsModel = new EmailSettingViewModel();
        EvolutionEntities _db = new EvolutionEntities();
        #endregion
        public int UpdateEmailSetting(EmailSettingViewModel model)
        {
            Email_Setting Emailsetting = _db.Email_Setting.Where(x => x.Id == model.Id).FirstOrDefault();
            Emailsetting.From_Name = model.FromName;
            Emailsetting.From_Email = model.FromEmail;
            Emailsetting.User_Name = model.UserName;
            Emailsetting.Email_Password = model.EmailPassword;
            Emailsetting.Server = model.Server;
            Emailsetting.Port = model.Port;
            Emailsetting.Enable_SSL = model.EnableSSL;
            _db.SaveChanges();
            return Emailsetting.Id;
        }
        public int InsertEmailSetting(EmailSettingViewModel model)
        {
            Email_Setting Emailsetting = new Email_Setting();
            Emailsetting.From_Name = model.FromName;
            Emailsetting.From_Email = model.FromEmail;
            Emailsetting.User_Name = model.UserName;
            Emailsetting.Email_Password = model.EmailPassword;
            Emailsetting.Server = model.Server;
            Emailsetting.Port = model.Port;
            Emailsetting.Enable_SSL = model.EnableSSL;
            _db.Email_Setting.Add(Emailsetting);
            _db.SaveChanges();
            return Emailsetting.Id;
        }
        public EmailSettingViewModel BindEmailSettingRecords(int Id)
        {
            EmailSettingViewModel model = new EmailSettingViewModel();
            var Emailsetting = _db.Email_Setting.Where(x => x.Id == Id).FirstOrDefault();
            model.FromName = Emailsetting.From_Name;
            model.Id = Emailsetting.Id;
            model.FromEmail = Emailsetting.From_Email;
            model.UserName = Emailsetting.User_Name;
            model.EmailPassword = Emailsetting.Email_Password;
            model.Server = Emailsetting.Server;
            model.Port = Emailsetting.Port;
            model.EnableSSL = (bool)Emailsetting.Enable_SSL;
            return model;
        }
        public Email_Setting EmailsettingList()
        {
            return _db.Email_Setting.FirstOrDefault();
        }
    }
}