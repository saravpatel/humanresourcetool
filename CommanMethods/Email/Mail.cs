using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using HRTool.DataModel;
using HRTool.CommanMethods.RolesManagement;
using HRTool.Models.Admin;
using System.Text.RegularExpressions;

namespace HRTool.CommanMethods.Email
{
    public class Mail
    {
        public static string AllEmployeeEmailString()
        {
            EvolutionEntities _db = new EvolutionEntities();
            var Users = _db.AspNetUsers.Where(x => x.SSOID.StartsWith("W") && x.Archived == false).ToList();
            string[] username = Users.Select(x => x.UserName).ToArray();
            string AllEmployeeEmailAddress = String.Join(",", username);
            return AllEmployeeEmailAddress;
        }
        public static string AllManagerEmailString()
        {
            EvolutionEntities _db = new EvolutionEntities();
            RolesManagementMethod _RolesManagementMethod = new RolesManagementMethod();
            var Users = _RolesManagementMethod.GetManagersList();
            string[] username = Users.Select(x => x.UserName).ToArray();
            string AllManagerEmailAddress = String.Join(",", username);
            return AllManagerEmailAddress;
        }
        public static string AllCustomerEmailString()
        {
            EvolutionEntities _db = new EvolutionEntities();
            var Users = _db.AspNetUsers.Where(x => x.SSOID.StartsWith("C") && x.Archived == false).ToList();
            string[] username = Users.Select(x => x.UserName).ToArray();
            string AllCustomerEmailAddress = String.Join(",", username);
            return AllCustomerEmailAddress;
        }

        private static Task NewsNotification(AdminNewsViewModel model)
        {
            try
            {
                EvolutionEntities _db = new EvolutionEntities();
                var emaildetails = _db.Email_Setting.ToList();
                if (emaildetails.Count > 0)
                {
                    MailMessage msg = new MailMessage();
                    msg.From = new MailAddress(emaildetails[0].From_Email);
                    msg.Subject = "There is news for you!";
                    string EmpEmail = AllEmployeeEmailString();
                    string MngEmail = AllManagerEmailString();
                    string CumEmail = AllCustomerEmailString();
                    if (model.EmployeeAccess == true)
                    {
                        foreach (var address in EmpEmail.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            msg.To.Add(address);
                        }
                    }
                    if (model.ManagerAccess == true)
                    {
                        foreach (var address in MngEmail.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            msg.To.Add(address);
                        }
                    }
                    if (model.CustomerAccess == true)
                    {
                        foreach (var address in CumEmail.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            msg.To.Add(address);
                        }
                    }
                    if (model.SpecificWorker == true)
                    {
                        var data = _db.AspNetUsers.Where(x => x.Id == model.WorkerID).FirstOrDefault();
                        if (data != null)
                        {
                            msg.To.Add(data.UserName);
                        }
                    }
                    if (model.SpecificManager == true)
                    {
                        var data = _db.AspNetUsers.Where(x => x.Id == model.ManagerID).FirstOrDefault();
                        if (data != null)
                        {
                        msg.To.Add(data.UserName);
                        }

                    }
                    if (model.SpecificCustomer == true)
                    {
                        var data = _db.AspNetUsers.Where(x => x.Id == model.CustomerID).FirstOrDefault();
                        if (data != null)
                        {
                            msg.To.Add(data.UserName);
                        }
                    }

                    msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(StripHTML(model.Description), null, MediaTypeNames.Text.Html));
                    SmtpClient smtpClient = new SmtpClient(emaildetails[0].Server, Convert.ToInt32(25));
                    //System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["mailAccount"], ConfigurationManager.AppSettings["secretPassword"]);
                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(emaildetails[0].From_Email, emaildetails[0].Email_Password);
                    smtpClient.Credentials = credentials;
                    msg.IsBodyHtml = true;
                    msg.Body = StripHTML(model.Description);
                    smtpClient.EnableSsl = (bool)emaildetails[0].Enable_SSL;
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    smtpClient.Send(msg);
                }

            }
            catch (Exception ex)
            {
                return Task.FromResult(0);
            }
            return Task.FromResult(0);
        }
        internal static Task NewsNotificationSendEmailAsync(AdminNewsViewModel model)
        {
            return NewsNotification(model);
        }

        private static Task NewsCommentNotification(AdminNewsViewModel model)
        {
            try
            {
                EvolutionEntities _db = new EvolutionEntities();
                var emaildetails = _db.Email_Setting.ToList();
                if (emaildetails.Count > 0)
                {
                    MailMessage msg = new MailMessage();
                    msg.From = new MailAddress(emaildetails[0].From_Email);
                    msg.Subject = "There is news for you!";
                    string EmpEmail = AllEmployeeEmailString();
                    string MngEmail = AllManagerEmailString();
                    string CumEmail = AllCustomerEmailString();
                    if (model.EmployeeAccess == true)
                    {
                        foreach (var address in EmpEmail.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            msg.To.Add(address);
                        }
                    }
                    if (model.ManagerAccess == true)
                    {
                        foreach (var address in MngEmail.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            msg.To.Add(address);
                        }
                    }
                    if (model.CustomerAccess == true)
                    {
                        foreach (var address in CumEmail.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            msg.To.Add(address);
                        }
                    }
                    if (model.SpecificWorker == true)
                    {
                        var data = _db.AspNetUsers.Where(x => x.Id == model.WorkerID).FirstOrDefault();
                        if (data != null)
                        {
                            msg.To.Add(data.UserName);
                        }
                    }
                    if (model.SpecificManager == true)
                    {
                        var data = _db.AspNetUsers.Where(x => x.Id == model.ManagerID).FirstOrDefault();
                        if (data != null)
                        {
                            msg.To.Add(data.UserName);
                        }

                    }
                    if (model.SpecificCustomer == true)
                    {
                        var data = _db.AspNetUsers.Where(x => x.Id == model.CustomerID).FirstOrDefault();
                        if (data != null)
                        {
                            msg.To.Add(data.UserName);
                        }
                    }

                    msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(StripHTML(model.Description), null, MediaTypeNames.Text.Html));
                    SmtpClient smtpClient = new SmtpClient(emaildetails[0].Server, Convert.ToInt32(25));
                    //System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["mailAccount"], ConfigurationManager.AppSettings["secretPassword"]);
                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(emaildetails[0].From_Email, emaildetails[0].Email_Password);
                    smtpClient.Credentials = credentials;
                    msg.IsBodyHtml = true;
                    msg.Body = StripHTML(model.Description);
                    smtpClient.EnableSsl = (bool)emaildetails[0].Enable_SSL;
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    smtpClient.Send(msg);
                }

            }
            catch (Exception ex)
            {
                return Task.FromResult(0);
            }
            return Task.FromResult(0);
        }
        internal static Task NewsCommentNotificationSendEmailAsync(int Id,string Comment)
        {
            EvolutionEntities _db = new EvolutionEntities();
            var data = _db.News.Where(x => x.Id == Id).FirstOrDefault();
            AdminNewsViewModel model = new AdminNewsViewModel();
            model.EmployeeAccess = data.EmployeeAccess;
            model.ManagerAccess = data.ManagerAccess;
            model.CustomerAccess = data.CustomerAccess;
            model.SpecificWorker = data.SpecificWorker;
            model.SpecificManager = data.SpecificManager;
            model.SpecificCustomer = data.SpecificCustomer;
            model.WorkerID = data.WorkerID;
            model.ManagerID = data.ManagerID;
            model.CustomerID = data.CustomerID;
            model.Description = Comment;
            return NewsCommentNotification(model);
        }

        public static string StripHTML(string input)
        {
            if (input != null)
            {
                return Regex.Replace(input, "<.*?>", String.Empty);
            }
            else
            {
                return String.Empty;
            }
        }
    }
}