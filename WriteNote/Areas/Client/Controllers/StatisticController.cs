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
    public class StatisticController : Controller
    {
        private readonly WNDatabaseContext _dbContext;

        public StatisticController()
        {
            _dbContext = new WNDatabaseContext();
        }

        [HttpGet]
        [AuthorizeAccount]
        public ActionResult Index()
        {
            if (int.Parse(Session["SubscriptionID"].ToString()) == 2 || int.Parse(Session["SubscriptionID"].ToString()) == 3)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Note", new { area = "Client" });
            }
        }

        [HttpPost]
        [AuthorizeAccount]
        public ActionResult Index(bool? flag)
        {
            int UserID = int.Parse(Session["UserID"].ToString());
            int noteCount = _dbContext.Notes.Where(u => u.UserID == UserID).Count();
            int notebookCount = _dbContext.NoteBooks.Where(u => u.UserID == UserID).Count();
            int deletedCount = _dbContext.NoteDeletes.Where(u => u.UserID == UserID).Count();
            int payCount = _dbContext.Buys.Where(u => u.UserID == UserID).Count();

            return Json(new { note = noteCount, notebook = notebookCount, deleted = deletedCount, pay = payCount, status = flag }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult HistoryBought(bool flag)
        {
            int UserID = int.Parse(Session["UserID"].ToString());
            var listBought = from a in _dbContext.Buys
                             join b in _dbContext.Users
                             on a.UserID equals b.UserID
                             join c in _dbContext.Subscriptions
                             on a.SubscriptionID equals c.SubscriptionID
                             orderby a.BuyID descending
                             select new BuyViewModel()
                             {
                                 UserID = b.UserID,
                                 SubscriptionID = a.SubscriptionID,
                                 Price = a.Price,
                                 CountMonth = a.CountMonth,
                                 DatetimeBought = a.DatetimeBought,
                                 SubscriptionName = c.SubscriptionName
                             };
            flag = (listBought.Count() != 0) ? true : false;
            return Json(new { status = flag, data = listBought}, JsonRequestBehavior.AllowGet);
        }
    }
}