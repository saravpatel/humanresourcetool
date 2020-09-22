using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRTool.Models.Settings
{
    public class EmailSettingViewModel
    {
        public int Id { get; set; }
        public string FromName { get; set; }

        [Required(ErrorMessage = "Email address required")]
        [EmailAddress(ErrorMessage = "Invalid Email address")]
        public string FromEmail { get; set; }

        [Required(ErrorMessage = "User Name required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email Password required")]
        public string EmailPassword { get; set; }

        [Required(ErrorMessage = "Server is required")]
        public string Server { get; set; }

        [Required(ErrorMessage = "Port is required")]
        public Nullable<int> Port { get; set; }
        public bool EnableSSL { get; set; }


    }
}