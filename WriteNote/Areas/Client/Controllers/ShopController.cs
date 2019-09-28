using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WriteNote.Cores;
using WriteNote.Models;

namespace WriteNote.Areas.Client.Controllers
{
    public class ShopController : Controller
    {
        private readonly WNDatabaseContext _dbContext;

        public ShopController()
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

        [HttpGet]
        [AuthorizeAccount]
        public ActionResult AddToCart(int id)
        {
            if (Session == null || Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                if (Session["BuySubscriptionID"] != null || Session["BuySubscriptionPrice"] != null || Session["BuySubscriptionCount"] != null)
                {
                    return RedirectToAction("Cart", "Shop", new { area = "Client" });
                }
                else
                {
                    return View();
                }
            }
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult AddToCart(bool flag, int subscriptionID)
        {
            int userID = int.Parse(Session["UserID"].ToString());
            var dataUser = _dbContext.Users.SingleOrDefault(u => u.UserID == userID);
            var dataSubsciption = _dbContext.Subscriptions.SingleOrDefault(u => u.SubscriptionID == subscriptionID);
            flag = (dataUser != null) ? true : false;
            return Json(new { status = flag, datauser = dataUser, datasub = dataSubsciption }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult Confirm(int subscriptionID, int price, int count)
        {
            bool flag = false;
            int userID = int.Parse(Session["UserID"].ToString());
            if(Session["BuySubscriptionID"] != null || Session["BuySubscriptionPrice"] != null || Session["BuySubscriptionCount"] != null)
            {
                flag = true;
            }
            else
            {
                Session["BuySubscriptionID"] = subscriptionID;
                Session["BuySubscriptionPrice"] = price;
                Session["BuySubscriptionCount"] = count;
                flag = true;
            }
            return Json(new { status = flag }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeAccount]
        public ActionResult Cart()
        {
            if (Session == null || Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                if (Session["BuySubscriptionID"] != null || Session["BuySubscriptionPrice"] != null || Session["BuySubscriptionCount"] != null)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Cart", "Shop", new { area = "Client" });
                }
            }
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult Cart(bool? flag)
        {
            int UserID = int.Parse(Session["UserID"].ToString());
            var user = _dbContext.Users.SingleOrDefault(u => u.UserID == UserID);
            var subcriptionID = int.Parse(Session["BuySubscriptionID"].ToString());
            var cart = _dbContext.Subscriptions.Where(u => u.SubscriptionID == subcriptionID).FirstOrDefault();

            flag = (cart != null) ? true : false;
            return Json(new { status = flag, dataCart = cart, dataUser = user }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeAccount]
        public JsonResult Pay(int total)
        {
            bool flag = false;
            if (Session["BuySubscriptionID"] != null || Session["BuySubscriptionPrice"] != null || Session["BuySubscriptionCount"] != null)
            {
                int subscriptionID = int.Parse(Session["BuySubscriptionID"].ToString());
                int count = int.Parse(Session["BuySubscriptionCount"].ToString());
                int UserID = int.Parse(Session["UserID"].ToString());

                Buy buy = new Buy
                {
                    UserID = UserID,
                    SubscriptionID = subscriptionID,
                    Price = total,
                    CountMonth = count,
                    DatetimeBought = DateTime.Now
                };

                _dbContext.Buys.Add(buy);
                _dbContext.SaveChanges();

                var dataUser = _dbContext.Users.SingleOrDefault(u => u.UserID == UserID);
                dataUser.SubscriptionID = subscriptionID;
                _dbContext.SaveChanges();

                Session["BuySubscriptionID"] = null;
                Session["BuySubscriptionPrice"] = null;
                Session["BuySubscriptionCount"] = null;

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
        public JsonResult Cancel(bool? flag)
        {
            Session["BuySubscriptionID"] = null;
            Session["BuySubscriptionPrice"] = null;
            Session["BuySubscriptionCount"] = null;

            flag = true;
            return Json(new { status = flag }, JsonRequestBehavior.AllowGet);
        }
    }
}