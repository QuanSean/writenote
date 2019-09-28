using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace WriteNote.Controllers
{
    public class ContactController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult SendEmail(string name, string email, string body)
        {
            bool flag = false;
            try
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.SmtpPort = 587;
                WebMail.SmtpUseDefaultCredentials = true;
                WebMail.EnableSsl = true;
                WebMail.UserName = "phuxxxtruong@gmail.com";
                WebMail.Password = "Cuty9997";
                WebMail.From = email;
                WebMail.Send(to: email, subject: "Phản hồi tới WriteNote", body: body, isBodyHtml: true);
                flag = true;
            }
            catch (Exception)
            {
                flag = false;
            }
            return Json(new { status = flag }, JsonRequestBehavior.AllowGet);
        }
    }
}