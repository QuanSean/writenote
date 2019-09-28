using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WriteNote.Cores;
using WriteNote.Models;
using WriteNote.Models.ViewModels;

namespace WriteNote.Areas.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly WNDatabaseContext _dbContext;

        public HomeController()
        {
            _dbContext = new WNDatabaseContext();
        }

        [HttpGet]
        [AuthorizeAccount]
        public ActionResult Index()
        {
            if (Session == null || Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                int userID = int.Parse(Session["UserID"].ToString());
                var noteBooks = _dbContext.NoteBooks.Where(u => u.UserID == userID).ToList();
                ViewData["listNoteBook"] = noteBooks;
                return View();
            }
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult Index(bool? flag)
        {
            int userID = int.Parse(Session["UserID"].ToString());
            var listNote = from u in _dbContext.Notes
                           join b in _dbContext.NoteBooks
                           on u.NotebookID equals b.NotebookID
                           where u.UserID == userID && u.IsDeleted == 0
                           select new NoteViewModel()
                           {
                               NotebookName = b.Title,
                               NoteID = u.NoteID,
                               Body = u.Body,
                               Title = u.Title,
                               DatetimeCreate = u.DatetimeCreate,
                               DatetimeUpdate = u.DatetimeUpdate
                           };

            flag = (listNote.Count() != 0) ? true : false;
            return Json(new { data = listNote , status = flag}, JsonRequestBehavior.AllowGet);
        }
    }
}