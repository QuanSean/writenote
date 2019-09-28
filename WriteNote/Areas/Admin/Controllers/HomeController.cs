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
            if (Session["RoleName"].ToString() == "Admin")
            {
                int UserID = int.Parse(Session["UserID"].ToString());
                var dataUser = _dbContext.Users.Where(u => u.UserID == UserID).FirstOrDefault();
                decimal countUser = _dbContext.Users.Count();
                decimal countNote = _dbContext.Notes.Count();
                decimal countNotebook = _dbContext.NoteBooks.Count();
                decimal countSubscription = _dbContext.Buys.Count();

                ViewData["Fullname"] = dataUser.Fullname;
                ViewData["Username"] = dataUser.Username;
                ViewData["CountUser"] = countUser;
                ViewData["CountNote"] = countNote;
                ViewData["CountNotebook"] = countNotebook;
                ViewData["CountSubscription"] = countSubscription;

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "Client" });
            }
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult GetLog(bool? flag)
        {
            var listLog = from a in _dbContext.Logs
                          join b in _dbContext.Users
                          on a.UserID equals b.UserID
                          join c in _dbContext.Notes
                          on a.NoteID equals c.NoteID
                          join d in _dbContext.NoteBooks
                          on c.NotebookID equals d.NotebookID
                          select new LogViewModel()
                          {
                              LogID = a.LogID,
                              Email = b.Email,
                              UserID = a.UserID,
                              NoteID = a.NoteID,
                              NotebookID = d.NotebookID,
                              NoteTitle = c.Title,
                              NotebookTitle = d.Title,
                              DatetimeCreate = a.DatetimeCreate
                          };

            flag = (listLog.Count() != 0) ? true : false;

            return Json(new { data = listLog, status = flag }, JsonRequestBehavior.AllowGet);
        }
    }
}