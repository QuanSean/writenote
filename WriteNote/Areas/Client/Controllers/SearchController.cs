using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WriteNote.Cores;
using WriteNote.Models;

namespace WriteNote.Areas.Client.Controllers
{
    public class SearchController : Controller
    {
        private readonly WNDatabaseContext _dbContext;

        public SearchController()
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
                return View();
            }
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult Index(string value)
        {
            bool flag = false;
            int UserID = int.Parse(Session["UserID"].ToString());
            var dataSearch = _dbContext.Notes.Where(u => u.Title.Contains(value)).Where(u => u.UserID == UserID);
            flag = (dataSearch.Count() != 0) ? true : false;
            return Json(new { status = flag, data = dataSearch }, JsonRequestBehavior.AllowGet);
        }
    }
}