using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WriteNote.Cores
{
    public class AuthorizeAccount : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session == null || context.HttpContext.Session["UserID"] == null || context.HttpContext.Session["RoleName"] == null || context.HttpContext.Session["SubscriptionName"] == null)
            {
                context.Result = 
                    new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new { controller = "Home", action = "Index", area = "" }));
            }
            base.OnActionExecuting(context);
        }
    }
}