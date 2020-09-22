using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using HRTool.DataModel;
using HRTool.Models;
using System.IO;
using System.Web;
using System.Net;

namespace HRTool.CommanMethods
{
    public class Common
    {        
        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            System.Configuration.AppSettingsReader settingsReader =
                                                new AppSettingsReader();
            // Get the key from config file

            string key = (string)settingsReader.GetValue("SecurityKey",
                                                             typeof(String));
            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            System.Configuration.AppSettingsReader settingsReader =
                                                new AppSettingsReader();
            //Get your key from config file to open the lock!
            string key = (string)settingsReader.GetValue("SecurityKey",
                                                         typeof(String));

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static int DaysLeft(DateTime startDate, DateTime endDate, Boolean excludeWeekends, List<DateTime> excludeDates)
        {
            int count = 0;
            for (DateTime index = startDate; index < endDate; index = index.AddDays(1))
            {
                if (excludeWeekends && index.DayOfWeek != DayOfWeek.Sunday && index.DayOfWeek != DayOfWeek.Saturday)
                {
                    bool excluded = false; ;
                    for (int i = 0; i < excludeDates.Count; i++)
                    {
                        if (index.Date.CompareTo(excludeDates[i].Date) == 0)
                        {
                            excluded = true;
                            break;
                        }
                    }

                    if (!excluded)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        //public static string PopulateBody(MailModel _objModelMail)
        //{
        //    string body = string.Empty;
        //    using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("D:\Ami\Evalution\HRTool/App_Data/Template/MailTemplate.html")))
        //    {
        //        body = reader.ReadToEnd();
        //    }
        //    body = body.Replace("{1}", _objModelMail.Header);            
        //    return body;
        //}
        public static string sendMail(MailModel _objModelMail)
        {
            try
            {
                // string Body = PopulateBody(_objModelMail);
                EvolutionEntities _db = new EvolutionEntities();
                var mailData = _db.Email_Setting.FirstOrDefault();
                MailMessage mail = new MailMessage();
                mail.To.Add(_objModelMail.To);
                mail.From = new MailAddress(_objModelMail.From);
                mail.Subject = _objModelMail.Subject;
                mail.Body = _objModelMail.Body;
                mail.IsBodyHtml = true;
                if(_objModelMail.AttachmentPath!="" && _objModelMail.AttachmentPath!=null)
                {
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(_objModelMail.AttachmentPath);
                    mail.Attachments.Add(attachment);
                }
                SmtpClient smtp = new SmtpClient();
                smtp.Host = mailData.Server;
                smtp.Port = Convert.ToInt32(mailData.Port);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(mailData.From_Email, mailData.Email_Password); // Enter seders User name and password  
                smtp.EnableSsl = Convert.ToBoolean(mailData.Enable_SSL);
                smtp.Send(mail);
                string mailSend = "Mail Send Suucessfully";
                return mailSend;
            }
            catch (Exception e)
            {
                string mailCancle =e+ "Problem while sending email, Please check details.";
                return mailCancle;
            }
        }

        public static string sendMailWithoutAttachment(MailModel _objModelMail)
        {
            try
            {
                // string Body = PopulateBody(_objModelMail);
                EvolutionEntities _db = new EvolutionEntities();
                var mailData = _db.Email_Setting.FirstOrDefault();
                MailMessage mail = new MailMessage();
                mail.To.Add(_objModelMail.To);
                mail.From = new MailAddress(_objModelMail.From);
                mail.Subject = _objModelMail.Subject;
                mail.Body = _objModelMail.Body;
                mail.IsBodyHtml = true;              
                SmtpClient smtp = new SmtpClient();
                smtp.Host = mailData.Server;
                smtp.Port = Convert.ToInt32(mailData.Port);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(mailData.From_Email, mailData.Email_Password); // Enter seders User name and password  
                smtp.EnableSsl = Convert.ToBoolean(mailData.Enable_SSL);
                smtp.Send(mail);
                string mailSend = "Mail Send Suucessfully";
                return mailSend;
            }
            catch (Exception e)
            {
                string mailCancle = "Problem while sending email, Please check details." + e;
                return mailCancle;
            }
        }
    }
}