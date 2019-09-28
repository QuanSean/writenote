using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WriteNote.Cores;
using WriteNote.Models;
using WriteNote.Models.ViewModels;

namespace WriteNote.Areas.Admin.Controllers
{
    public class NotebookController : Controller
    {
        private readonly WNDatabaseContext _dbContext;

        public NotebookController()
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
        public JsonResult Index(bool? flag)
        {
            var listNotebook = from a in _dbContext.NoteBooks
                               join b in _dbContext.Users
                               on a.UserID equals b.UserID
                               select new NoteBookViewModel()
                               {
                                   NotebookID = a.NotebookID,
                                   UserID = a.UserID,
                                   Title = a.Title,
                                   DatetimeCreate = a.DatetimeCreate,
                                   Email = b.Email
                               };
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
            flag = (listNotebook.Count() != 0 || listUser.Count() != 0) ? true : false;
            return Json(new { datanb = listNotebook, dataus = listUser, status = flag }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult GetNotebook(bool? flag, int UserID)
        {
            if (UserID == 0)
            {
                var listNotebook = from a in _dbContext.NoteBooks
                                   join b in _dbContext.Users
                                   on a.UserID equals b.UserID
                                   select new NoteBookViewModel()
                                   {
                                       NotebookID = a.NotebookID,
                                       UserID = a.UserID,
                                       Title = a.Title,
                                       DatetimeCreate = a.DatetimeCreate,
                                       Email = b.Email
                                   };

                flag = (listNotebook.Count() != 0) ? true : false;
                return Json(new { data = listNotebook, status = flag }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var listNotebook = from a in _dbContext.NoteBooks
                                   join b in _dbContext.Users
                                   on a.UserID equals b.UserID
                                   where a.UserID == UserID
                                   select new NoteBookViewModel()
                                   {
                                       NotebookID = a.NotebookID,
                                       UserID = a.UserID,
                                       Title = a.Title,
                                       DatetimeCreate = a.DatetimeCreate,
                                       Email = b.Email
                                   };

                flag = (listNotebook.Count() != 0) ? true : false;
                return Json(new { data = listNotebook, status = flag }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeAccount]
        public ActionResult CreateNotebook()
        {
            if (Session["RoleName"].ToString() == "Admin")
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
                ViewData["listUser"] = listUser;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "Client" });
            }
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult CreateNotebook(int UserID, string Title)
        {
            bool flag = false;
            var CheckExistsUser = _dbContext.Users.SingleOrDefault(u => u.UserID == UserID);
            if(CheckExistsUser != null)
            {
                NoteBook notebook = new NoteBook()
                {
                    UserID = UserID,
                    Title = Title,
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

        [HttpGet]
        [AuthorizeAccount]
        public ActionResult EditNotebook(int? id)
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
        public JsonResult EditNotebook(bool flag, int NotebookID)
        {
            var dataNotebook = from a in _dbContext.NoteBooks
                               join b in _dbContext.Users
                               on a.UserID equals b.UserID
                               where a.NotebookID == NotebookID
                               select new NoteBookViewModel()
                               {
                                   NotebookID = a.NotebookID,
                                   UserID = a.UserID,
                                   Title = a.Title,
                                   DatetimeCreate = a.DatetimeCreate,
                                   Email = b.Email
                               };

            return Json(new { status = flag, data = dataNotebook }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult UpdateNotebook(int UserID, int NotebookID, string Title)
        {
            bool flag = false;

            var CheckExistsNotebook = _dbContext.NoteBooks.SingleOrDefault(u => u.UserID == UserID && u.NotebookID == NotebookID);

            if (CheckExistsNotebook != null)
            {
                CheckExistsNotebook.Title = Title;
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
        public JsonResult DeleteNotebook(int? NotebookID)
        {
            int flag = 0;
            var CheckExistsNotebook = _dbContext.NoteBooks.SingleOrDefault(u => u.NotebookID == NotebookID);

            if (CheckExistsNotebook != null)
            {
                var CheckExistsNote = _dbContext.Notes.SingleOrDefault(u => u.NotebookID == NotebookID);

                if (CheckExistsNote == null)
                {
                    _dbContext.NoteBooks.Remove(CheckExistsNotebook);
                    _dbContext.SaveChanges();
                    flag = 1;
                }
                else
                {
                    flag = 2;
                }
            }
            else
            {
                flag = 3;
            }
            return Json(new { status = flag }, JsonRequestBehavior.AllowGet);
        }
    }
}