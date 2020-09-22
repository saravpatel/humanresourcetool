using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRTool.Models.Resources
{
    public class EmployeeSignatureViewModel
    {
        public EmployeeSignatureViewModel()
        {
                
        }
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public int DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string SignatureText { get; set; }
        public string IpAddress { get; set; }
        public string CreateDate { get; set; }
    }

}

