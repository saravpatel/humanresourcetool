using HRTool.CommanMethods;
using HRTool.CommanMethods.Resources;
using HRTool.CommanMethods.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class EmpolyeeCasesController : Controller
    {

        #region Constant

        EmpolyeeCasesMethod _EmpolyeeCasesMethod = new EmpolyeeCasesMethod();

        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();

        #endregion

        // <add key="CaseLogDocuments" value="~/Upload/Documents/CaseLog/" />
        // GET: /EmpolyeeCases/
        public ActionResult Index(string EmployeeId)
        {
            return View();
        }
	}
}