using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WriteNote.Controllers;
using WriteNote.Cores;
using WriteNote.Models;
using WriteNote.Models.ViewModels;

namespace WriteNote.Areas.Client.Controllers
{
    public class ProfileController : Controller
    {
        private readonly WNDatabaseContext _dbContext;

        public ProfileController()
        {
            _dbContext = new WNDatabaseContext();
        }

        [HttpGet]
        [AuthorizeAccount]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult Index(bool? flag)
        {
            int UserID = int.Parse(Session["UserID"].ToString());

            var profile = from a in _dbContext.Users
                          join b in _dbContext.Roles
                          on a.RoleID equals b.RoleID
                          join c in _dbContext.Subscriptions
                          on a.SubscriptionID equals c.SubscriptionID
                          where a.UserID == UserID
                          select new ProfileViewModel()
                          {
                              UserID = a.UserID,
                              Username = a.Username,
                              Password = a.Password,
                              Fullname = a.Fullname,
                              Email = a.Email,
                              DatetimeCreate = a.DatetimeCreate,
                              RoleName = b.RoleName,
                              RoleID = b.RoleID,
                              SubscriptionID = c.SubscriptionID,
                              SubscriptionName = c.SubscriptionName
                          };
            flag = (profile.Count() != 0) ? true : false;
            return Json(new { data = profile, status = flag }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult ChangePassword(string oldpassword, string newpassword)
        {
            bool flag = false;
            int UserID = int.Parse(Session["UserID"].ToString());
            var userInfo = _dbContext.Users
                .Where(u => u.UserID == UserID)
                .FirstOrDefault<User>();

            string encode_oldpassword = StringHash.crypto(oldpassword);

            if (userInfo.Password == encode_oldpassword)
            {
                string encode_newpassword = StringHash.crypto(newpassword);
                userInfo.Password = encode_newpassword;
                _dbContext.SaveChanges();
                flag = true;
            } 
            else
            {
                flag = false;
            }

            return Json(new { status = flag }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult ChangeFullname(string username, string fullname)
        {
            bool flag = false;
            int UserID = int.Parse(Session["UserID"].ToString());
            var userInfo = _dbContext.Users
                            .Where(u => u.UserID == UserID)
                            .FirstOrDefault();
            try
            {
                userInfo.Fullname = fullname;
                userInfo.Username = username;
                _dbContext.SaveChanges();
                flag = true;
            }
            catch(Exception)
            {
                flag = false;
            }
            return Json(new { status = flag }, JsonRequestBehavior.AllowGet);
        }
    }
}