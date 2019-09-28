using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WriteNote.Cores
{
    public class VerifyAccount : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session == null || context.HttpContext.Session["Email"] == null)
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