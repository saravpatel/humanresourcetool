using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HRTool.CommanMethods
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            if (SessionProxy.UserId > 0 && SessionProxy.UserId != null)
            {

            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                    (new { controller = "Login", action = "LoginRedirect" }));
            }
            
            base.OnAuthorization(filterContext);
        }
    }
}