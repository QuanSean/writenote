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
    public class NoteController : Controller
    {
        private readonly WNDatabaseContext _dbContext;

        public NoteController()
        {
            _dbContext = new WNDatabaseContext();
        }

        [HttpGet]
        [AuthorizeAccount]
        public ActionResult Index()
        {
            if (Session["RoleName"].ToString() == "Admin")
            {
                //var listNote = from a in _dbContext.Notes
                //               join b in _dbContext.NoteBooks
                //               on a.NotebookID equals b.NotebookID
                //               select new NoteViewModel()
                //               {
                //                   NoteID = a.NoteID,
                //                   UserID = a.UserID,
                //                   Title = a.Title,
                //                   Body = a.Body,
                //                   NotebookID = b.NotebookID,
                //                   NotebookName = b.Title,
                //                   IsDelete = a.IsDeleted,
                //                   DatetimeCreate = a.DatetimeCreate,
                //                   DatetimeUpdate = a.DatetimeUpdate
                //               };
                //ViewData["listNote"] = listNote;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "Client" });
            }
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult Index(bool? flag, int UserID)
        {
            if(UserID == 0)
            {
                var listNote = from a in _dbContext.Notes
                               join b in _dbContext.NoteBooks
                               on a.NotebookID equals b.NotebookID
                               select new NoteViewModel()
                               {
                                   NoteID = a.NoteID,
                                   UserID = a.UserID,
                                   Title = a.Title,
                                   Body = a.Body,
                                   NotebookID = b.NotebookID,
                                   NotebookName = b.Title,
                                   IsDelete = a.IsDeleted,
                                   DatetimeCreate = a.DatetimeCreate,
                                   DatetimeUpdate = a.DatetimeUpdate
                               };

                flag = (listNote.Count() != 0) ? true : false;
                return Json(new { data = listNote, status = flag }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var listNote = from a in _dbContext.Notes
                               join b in _dbContext.NoteBooks
                               on a.NotebookID equals b.NotebookID
                               where a.UserID == UserID
                               select new NoteViewModel()
                               {
                                   NoteID = a.NoteID,
                                   UserID = a.UserID,
                                   Title = a.Title,
                                   Body = a.Body,
                                   NotebookID = b.NotebookID,
                                   NotebookName = b.Title,
                                   IsDelete = a.IsDeleted,
                                   DatetimeCreate = a.DatetimeCreate,
                                   DatetimeUpdate = a.DatetimeUpdate
                               };

                flag = (listNote.Count() != 0) ? true : false;
                return Json(new { data = listNote, status = flag }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult ListUser(bool? flag)
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
            var listNote = from a in _dbContext.Notes
                           join b in _dbContext.NoteBooks
                           on a.NotebookID equals b.NotebookID
                           select new NoteViewModel()
                           {
                               NoteID = a.NoteID,
                               UserID = a.UserID,
                               Title = a.Title,
                               Body = a.Body,
                               NotebookID = b.NotebookID,
                               NotebookName = b.Title,
                               IsDelete = a.IsDeleted,
                               DatetimeCreate = a.DatetimeCreate,
                               DatetimeUpdate = a.DatetimeUpdate
                           };
            flag = (listUser.Count() != 0 || listNote.Count() != 0) ? true : false;
            return Json(new { dataus = listUser, datant = listNote, status = flag }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult DeleteNote(int? NoteID)
        {
            bool flag = false;

            var CheckExistsNote = _dbContext.Notes.SingleOrDefault(u => u.NoteID == NoteID);
            if (CheckExistsNote != null)
            {
                var NoteDelete = _dbContext.NoteDeletes.SingleOrDefault(u => u.NoteID == NoteID);

                if(NoteDelete != null)
                {
                    _dbContext.NoteDeletes.Remove(NoteDelete);
                    _dbContext.SaveChanges();
                }

                _dbContext.Notes.Remove(CheckExistsNote);
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
        public ActionResult CreateNote()
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
        public JsonResult GetNotebook(int? UserID)
        {
            bool flag = false;

            var listNotebook = from a in _dbContext.NoteBooks
                               where a.UserID == UserID
                               select new NoteBookViewModel()
                               {
                                   NotebookID = a.NotebookID,
                                   UserID = a.UserID,
                                   Title = a.Title,
                                   DatetimeCreate = a.DatetimeCreate
                               };

            flag = (listNotebook.Count() != 0) ? true : false;

            return Json(new { data = listNotebook, status = flag }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult CreateNote(int UserID, int NotebookID, string Title, string Body)
        {
            bool flag = false;

            var CheckNoteExists = _dbContext.Notes.SingleOrDefault(u => u.UserID == UserID && u.NotebookID == NotebookID);

            if (CheckNoteExists == null)
            {
                Note note = new Note()
                {
                    UserID = UserID,
                    NotebookID = NotebookID,
                    Title = Title,
                    Body = Body,
                    DatetimeCreate = DateTime.Now,
                    IsDeleted = 0,
                    DatetimeUpdate = DateTime.Now
                };
                _dbContext.Notes.Add(note);
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
        public ActionResult EditNote(int id)
        {
            if (Session["RoleName"].ToString() == "Admin")
            {
                //var dataNote = from a in _dbContext.Notes
                //               join b in _dbContext.NoteBooks
                //               on a.NotebookID equals b.NotebookID
                //               join c in _dbContext.Users
                //               on a.UserID equals c.UserID
                //               where a.NoteID == id
                //               select new NoteViewModel()
                //               {
                //                   UserID = a.UserID,
                //                   Email = c.Email,
                //                   Title = a.Title,
                //                   Body = a.Body,
                //                   NotebookName = b.Title
                //               };

                //var listNotebook = _dbContext.NoteBooks.Where(u => u.UserID == dataNote.FirstOrDefault().UserID);

                //ViewData["listNotebook"] = listNotebook;
                //ViewData["dataNote"] = dataNote;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "Client" });
            }
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult EditNote(bool? flag, int NoteID)
        {
            var dataNote = from a in _dbContext.Notes
                           join b in _dbContext.NoteBooks
                           on a.NotebookID equals b.NotebookID
                           join c in _dbContext.Users
                           on a.UserID equals c.UserID
                           where a.NoteID == NoteID
                           select new NoteViewModel()
                           {
                               UserID = a.UserID,
                               Email = c.Email,
                               Title = a.Title,
                               Body = a.Body,
                               NotebookName = b.Title,
                               NotebookID = b.NotebookID
                           };

            var listNotebook = _dbContext.NoteBooks.Where(u => u.UserID == dataNote.FirstOrDefault().UserID);

            return Json(new { status = flag, note = dataNote, notebook = listNotebook }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult UpdateNote(int UserID, int NoteID, int NotebookID, string Title, string Body)
        {
            bool flag = false;

            var CheckExistsNote = _dbContext.Notes.SingleOrDefault(u => u.UserID == UserID && u.NoteID == NoteID);

            if(CheckExistsNote != null)
            {
                CheckExistsNote.Title = Title;
                CheckExistsNote.Body = Body;
                CheckExistsNote.NotebookID = NotebookID;
                CheckExistsNote.DatetimeUpdate = DateTime.Now;

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