using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WriteNote.Controllers;
using WriteNote.Cores;
using WriteNote.Models;
using WriteNote.Models.ViewModels;

namespace WriteNote.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly WNDatabaseContext _dbContext;

        public UserController()
        {
            _dbContext = new WNDatabaseContext();
        }

        [HttpGet]
        [AuthorizeAccount]
        public ActionResult Index()
        {
            if (Session["RoleName"].ToString() == "Admin")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "Client" });
            }
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult Index(bool flag)
        {
            var listUser = from a in _dbContext.Users
                           join b in _dbContext.Roles
                           on a.RoleID equals b.RoleID
                           join c in _dbContext.Subscriptions
                           on a.SubscriptionID equals c.SubscriptionID
                           select new UserViewModel()
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
            flag = (listUser.Count() != 0) ? true : false;
            return Json(new { data = listUser, status = flag }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult GetData(int? UserID)
        {
            bool flag = false;
            var dataUser = from a in _dbContext.Users
                           join b in _dbContext.Roles
                           on a.RoleID equals b.RoleID
                           join c in _dbContext.Subscriptions
                           on a.SubscriptionID equals c.SubscriptionID
                           where a.UserID == UserID
                           select new UserViewModel()
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

            flag = (dataUser.Count() != 0) ? true : false;
            return Json(new { data = dataUser, status = flag }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult DeleteUser(int? UserID)
        {
            int flag = 0;

            var CheckExistsUser = _dbContext.Users.Where(u => u.UserID == UserID).FirstOrDefault();
            if (CheckExistsUser != null)
            {
                if (CheckExistsUser.RoleID == 1)
                {
                    flag = 3;
                }
                else
                {
                    _dbContext.NoteBooks.RemoveRange(_dbContext.NoteBooks.Where(u => u.UserID == UserID));
                    _dbContext.SaveChanges();

                    _dbContext.Notes.RemoveRange(_dbContext.Notes.Where(u => u.UserID == UserID));
                    _dbContext.SaveChanges();

                    _dbContext.Buys.RemoveRange(_dbContext.Buys.Where(u => u.UserID == UserID));
                    _dbContext.SaveChanges();

                    _dbContext.Users.Remove(_dbContext.Users.SingleOrDefault(u => u.UserID == UserID));
                    _dbContext.SaveChanges();

                    flag = 1;
                }
            }
            else
            {
                flag = 2;
            }

            return Json(new { status = flag }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult UpdateUser(int? UserID, string Username, string Fullname, string Password, int RoleID, int SubscriptionID)
        {
            bool flag = false;
            var CheckExistsUser = _dbContext.Users.SingleOrDefault(u => u.UserID == UserID);
            string encode_password = StringHash.crypto(Password);

            if (CheckExistsUser != null)
            {
                CheckExistsUser.Username = Username;
                CheckExistsUser.Fullname = Fullname;
                if (Password != CheckExistsUser.Password)
                {
                    CheckExistsUser.Password = encode_password;
                }
                CheckExistsUser.RoleID = RoleID;
                CheckExistsUser.SubscriptionID = SubscriptionID;

                _dbContext.SaveChanges();

                flag = true;
            }
            else
            {
                flag = true;
            }

            return Json(new { status = flag }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeAccount]
        public ActionResult EditUser()
        {
            if (Session["RoleName"].ToString() == "Admin")
            {
                var listRole = _dbContext.Roles.ToList();
                ViewData["listRole"] = listRole;

                var listSubscription = _dbContext.Subscriptions.ToList();
                ViewData["listSubscription"] = listSubscription;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "Client" });
            }
        }

        [HttpGet]
        [AuthorizeAccount]
        public ActionResult CreateUser()
        {
            if (Session["RoleName"].ToString() == "Admin")
            {
                var listRole = _dbContext.Roles.ToList();
                ViewData["listRole"] = listRole;

                var listSubscription = _dbContext.Subscriptions.ToList();
                ViewData["listSubscription"] = listSubscription;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "Client" });
            }
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult CreateUser(string Username, string Fullname, string Email, string Password, int RoleID, int SubscriptionID)
        {
            bool flag = false;
            string encode_password = StringHash.crypto(Password);
            var CheckExistsEmail = _dbContext.Users.Where(u => u.Email == Email).FirstOrDefault();
            if (CheckExistsEmail == null)
            {
                User user = new User()
                {
                    Username = Username,
                    Password = encode_password,
                    Email = Email,
                    Fullname = Fullname,
                    DatetimeCreate = DateTime.Now,
                    RoleID = RoleID,
                    SubscriptionID = SubscriptionID
                };

                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                NoteBook notebook = new NoteBook()
                {
                    UserID = user.UserID,
                    Title = "Không có tiêu đề",
                    DatetimeCreate = DateTime.Now
                };

                _dbContext.NoteBooks.Add(notebook);
                _dbContext.SaveChanges();

                flag = true;
            }
            else
            {
                flag = false;
            }

            return Json(new { status = flag }, JsonRequestBehavior.AllowGet);
        }
    }
}