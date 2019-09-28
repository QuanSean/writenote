using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WriteNote.Cores
{
    public class RoleAccount : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string roleName = context.HttpContext.Session["RoleName"].ToString();
            if (roleName == "Admin")
            {
                context.Result =
                    new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new { controller = "Home", action = "Index", area = "Admin" }));
            }
            else if (roleName == "User")
            {
                context.Result =
                    new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new { controller = "Home", action = "Index", area = "Client" }));
            }
            else
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