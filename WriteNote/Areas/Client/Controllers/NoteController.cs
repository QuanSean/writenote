using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WriteNote.Cores;
using WriteNote.Models;
using WriteNote.Models.ViewModels;

namespace WriteNote.Areas.Client.Controllers
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
            return RedirectToAction("Index", "Home", new { area = "Client" });
        }

        [HttpGet]
        [AuthorizeAccount]
        public ActionResult Delete()
        {
            return RedirectToAction("Index", "Home", new { area = "Client" });
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult Create(string body, string title, int notebookID)
        {
            bool flag = false;
            title = (title == "") ? "Không có tiêu đề" : title;
            int UserID = int.Parse(Session["UserID"].ToString());
            try
            {
                Note note = new Note
                {
                    UserID = UserID,
                    Title = title,
                    Body = body,
                    NotebookID = notebookID,
                    IsDeleted = 0,
                    DatetimeCreate = DateTime.Now,
                    DatetimeUpdate = DateTime.Now
                };

                _dbContext.Notes.Add(note);
                _dbContext.SaveChanges();

                Log log = new Log
                {
                    UserID = UserID,
                    NoteID = note.NoteID,
                    DatetimeCreate = DateTime.Now
                };

                _dbContext.Logs.Add(log);
                _dbContext.SaveChanges();

                flag = true;
            }
            catch (Exception)
            {
                flag = false;
            }

            return Json(new { status = flag }, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //[AuthorizeAccount]
        //public ActionResult ViewNote(int? noteID)
        //{
        //    var email = Session["Email"].ToString();
        //    var userInfo = _dbContext.Users.Where(u => u.Email == email).FirstOrDefault<User>();
        //    var userID = userInfo.UserID;

        //    var listNote = _dbContext.Notes.Where(u => u.UserID == userID && u.NoteID == noteID).FirstOrDefault<Note>();


        //    if(listNote == null)
        //    {
        //        return RedirectToAction("Index", "Home", new { area = "Client" });
        //    }
        //    else
        //    {
        //        ViewData["dataNote"] = listNote;
        //        ViewBag.Title = listNote.Title;
        //        return View();
        //    }
        //}

        [HttpGet]
        [AuthorizeAccount]
        public ActionResult ViewNote()
        {
            int UserID = int.Parse(Session["UserID"].ToString());
            var noteBooks = _dbContext.NoteBooks.Where(u => u.UserID == UserID).ToList();
            ViewData["listNoteBook"] = noteBooks;
            return View();
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult ViewNote(int? noteID)
        {
            bool flag = false;
            int UserID = int.Parse(Session["UserID"].ToString());
            var note = from u in _dbContext.Notes
                       join b in _dbContext.NoteBooks
                       on u.NotebookID equals b.NotebookID
                       where u.UserID == UserID && u.IsDeleted == 0 && u.NoteID == noteID
                       select new NoteViewModel()
                       {
                           NotebookID = b.NotebookID,
                           NotebookName = b.Title,
                           NoteID = u.NoteID,
                           Body = u.Body,
                           Title = u.Title
                       };
            flag = (note != null) ? true : false;
            return Json(new { status = flag, data = note }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult UpdateNote(int? noteID, string body, int notebookID, string title)
        {
            bool flag = false;
            int UserID = int.Parse(Session["UserID"].ToString());
            title = (title == "") ? "Không có tiêu đề" : title;
            var note = _dbContext.Notes.SingleOrDefault(b => b.UserID == UserID && b.NoteID == noteID);
            if (note != null)
            {
                note.Title = title;
                note.Body = body;
                note.NotebookID = notebookID;
                note.DatetimeUpdate = DateTime.Now;
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
        public JsonResult DeleteNote(int? noteID)
        {
            int flag = 0;
            int UserID = int.Parse(Session["UserID"].ToString());

            var note = _dbContext.Notes.SingleOrDefault(u => u.UserID == UserID && u.NoteID == noteID);
            if (note != null)
            {
                note.IsDeleted = 1;
                NoteDelete noteDelete = new NoteDelete
                {
                    UserID = UserID,
                    NoteID = note.NoteID,
                    DateTimeDelete = DateTime.Now
                };
                _dbContext.NoteDeletes.Add(noteDelete);
                _dbContext.SaveChanges();
                flag = 1;
            }
            else
            {
                flag = 2;
            }

            return Json(new { status = flag }, JsonRequestBehavior.AllowGet);
        }
    }
}