using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WriteNote.Cores;
using WriteNote.Models;

namespace WriteNote.Areas.Client.Controllers
{
    public class NoteBookController : Controller
    {
        private readonly WNDatabaseContext _dbContext;

        public NoteBookController()
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
            var listNoteBook = _dbContext.NoteBooks.Where(i => i.UserID == UserID).OrderByDescending(i => i.NotebookID).ToList();
            flag = (listNoteBook.Count != 0) ? true : false;
            return Json(new { data = listNoteBook, status = flag }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult Create(string title)
        {
            bool flag = false;
            int UserID = int.Parse(Session["UserID"].ToString());
            try
            {
                NoteBook notebook = new NoteBook
                {
                    UserID = UserID,
                    Title = title,
                    DatetimeCreate = DateTime.Now
                };
                _dbContext.NoteBooks.Add(notebook);
                _dbContext.SaveChanges();

                flag = true;
            }
            catch (Exception)
            {
                flag = false;
            }
            return Json(new { status = flag }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeAccount]
        public ActionResult ViewNoteBook()
        {
            return View();
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult ViewNoteBook(int? notebookID)
        {
            int UserID = int.Parse(Session["UserID"].ToString());
            var notebook = _dbContext.NoteBooks
                        .Where(u => u.UserID == UserID)
                        .Where(u => u.NotebookID == notebookID)
                        .FirstOrDefault();
            return Json(new { data = notebook }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult UpdateNoteBook(int? notebookID, string title)
        {
            int flag = 0;
            int UserID = int.Parse(Session["UserID"].ToString());
            var notebook = _dbContext.NoteBooks.SingleOrDefault(b => b.UserID == UserID && b.NotebookID == notebookID);
            if (notebook != null)
            {
                notebook.Title = title;
                _dbContext.SaveChanges();
                flag = 1;
            }
            else
            {
                flag = 2;
            }

            return Json(new { status = flag }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult DeleteNoteBook(int? notebookID)
        {
            int flag = 0;
            int UserID = int.Parse(Session["UserID"].ToString());

            var CheckNoteExists = _dbContext.Notes.Where(u => u.UserID == UserID)
                .Where(u => u.NotebookID == notebookID).FirstOrDefault();

            if (CheckNoteExists == null)
            {
                var notebook = _dbContext.NoteBooks.SingleOrDefault(u => u.UserID == UserID && u.NotebookID == notebookID);
                if (notebook != null)
                {
                    _dbContext.NoteBooks.Remove(notebook);
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