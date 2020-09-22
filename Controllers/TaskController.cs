using HRTool.CommanMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class TaskController : Controller
    {

        public ActionResult Index()
        {
            return View();

        }

        public ActionResult List(string FilterValue)
        {
            return PartialView("_partialTaskIndex");
        }
    }
}