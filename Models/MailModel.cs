using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.Models
{
    public class MailModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Header { get; set; }
        public string EndorsementDate { get; set; }

        public string AttachmentPath { get; set; }
    }
}