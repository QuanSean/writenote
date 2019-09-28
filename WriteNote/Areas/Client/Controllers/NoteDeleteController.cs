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
    public class NoteDeleteController : Controller
    {
        private readonly WNDatabaseContext _dbContext;

        public NoteDeleteController()
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
            var listNote = from a in _dbContext.NoteDeletes
                           join b in _dbContext.Notes
                           on a.NoteID equals b.NoteID
                           where a.UserID == UserID
                           select new NoteDeleteViewModel()
                           {
                               NoteID = a.NoteID,
                               Title = b.Title
                           };
            flag = (listNote.Count() != 0) ? true : false;
            return Json(new { data = listNote, status = flag }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult Delete(int? noteID)
        {
            bool flag = false;
            int UserID = int.Parse(Session["UserID"].ToString());
            var noteDelete = _dbContext.NoteDeletes.SingleOrDefault(u => u.UserID == UserID && u.NoteID == noteID);
            var note = _dbContext.Notes.SingleOrDefault(u => u.UserID == UserID && u.NoteID == noteID && u.IsDeleted == 1);
            if (noteDelete != null)
            {
                _dbContext.NoteDeletes.Remove(noteDelete);
                _dbContext.SaveChanges();

                if (note != null)
                {
                    _dbContext.Notes.Remove(note);
                    _dbContext.SaveChanges();
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            else
            {
                flag = false;
            }

            return Json(new { status = flag }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult Revive(int? noteID)
        {
            bool flag = false;
            int UserID = int.Parse(Session["UserID"].ToString());
            var noteDelete = _dbContext.NoteDeletes.SingleOrDefault(u => u.UserID == UserID && u.NoteID == noteID);
            var note = _dbContext.Notes.SingleOrDefault(u => u.UserID == UserID && u.NoteID == noteID && u.IsDeleted == 1);
            if (noteDelete != null)
            {
                _dbContext.NoteDeletes.Remove(noteDelete);
                _dbContext.SaveChanges();

                if (note != null)
                {
                    note.IsDeleted = 0;
                    _dbContext.SaveChanges();
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            else
            {
                flag = false;
            }
            return Json(new { status = flag }, JsonRequestBehavior.AllowGet);
        }
    }
}