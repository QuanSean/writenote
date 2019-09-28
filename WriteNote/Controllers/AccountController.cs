using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WriteNote.Cores;
using WriteNote.Models;
using WriteNote.Models.ViewModels;

namespace WriteNote.Controllers
{
    public class AccountController : Controller
    {
        private static string mailPasswd = "";

        private readonly WNDatabaseContext _dbContext;

        public AccountController()
        {
            _dbContext = new WNDatabaseContext();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            mailPasswd = "";
            if (Request.Cookies["User"] != null)
            {
                Session["UserID"] = Request.Cookies["User"]["UserID"];
                Session["RoleName"] = Request.Cookies["User"]["RoleName"];
                Session["SubscriptionName"] = Request.Cookies["User"]["SubscriptionName"];
                Session["SubscriptionID"] = Request.Cookies["User"]["SubscriptionID"];
                return RedirectToAction("Index", "Home", new { area = "Client" });
            }
            else
            {
                if (Session == null || Session["UserID"] == null)
                {
                    ViewBag.Title = "Chào mừng trở lại";
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { area = "Client" });
                }
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Login(string Mail, string passwd, bool checkbox)
        {
            bool flag = false;
            string password = StringHash.crypto(passwd);

            var user = _dbContext.Users.Where(u => u.Email == Mail).Where(u => u.Password == password).FirstOrDefault();
            if (user != null)
            {
                var profile = from a in _dbContext.Users
                              join b in _dbContext.Roles
                              on a.RoleID equals b.RoleID
                              join c in _dbContext.Subscriptions
                              on a.SubscriptionID equals c.SubscriptionID
                              where a.UserID == user.UserID
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
                var UserInfo = profile.FirstOrDefault();

                Session["UserID"] = user.UserID.ToString();
                Session["RoleName"] = UserInfo.RoleName.ToString();
                Session["SubscriptionName"] = UserInfo.SubscriptionName.ToString();
                Session["SubscriptionID"] = UserInfo.SubscriptionID.ToString();

                if (checkbox)
                {
                    HttpCookie cookie = new HttpCookie("User");
                    cookie.Values["UserID"] = user.UserID.ToString();
                    cookie.Values["RoleName"] = UserInfo.RoleName.ToString();
                    cookie.Values["SubscriptionName"] = UserInfo.SubscriptionName.ToString();
                    cookie.Values["SubscriptionID"] = UserInfo.SubscriptionID.ToString();
                    cookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    HttpCookie cookie = new HttpCookie("User")
                    {
                        Expires = DateTime.Now.AddDays(-1d)
                    };
                    Response.Cookies.Add(cookie);
                }

                flag = true;
            }
            else
            {
                flag = false;
            }
            return Json(new { status = flag }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (Session == null || Session["UserID"] == null)
            {
                ViewBag.Title = "Tạo Tài khoản mới";
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "Client" });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Register(string Mail, string passwd)
        {
            bool flag = false;
            string password = StringHash.crypto(passwd);
            var CheckExistsEmail = _dbContext.Users.Where(u => u.Email == Mail).FirstOrDefault();
            if (CheckExistsEmail == null)
            {
                User user = new User
                {
                    Email = Mail,
                    Password = password,
                    DatetimeCreate = DateTime.Now,
                    SubscriptionID = 1,
                    RoleID = 2
                };
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                NoteBook notebook = new NoteBook()
                {
                    UserID = user.UserID,
                    Title = "Không có tiêu đề",
                    DatetimeCreate = DateTime.Now
                };
                _dbContext.NoteBooks.Add(notebook);
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
        public ActionResult Logout()
        {
            // Clear Sessions
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();

            //Clear Cookies
            if (Request.Cookies["User"] != null)
            {
                HttpCookie cookie = new HttpCookie("User")
                {
                    Expires = DateTime.Now.AddDays(-1d)
                };
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            if (Session == null || Session["UserID"] == null)
            {
                ViewBag.Title = "Đặt lại mật khẩu";
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "Client" });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult ForgotPassword(string Mail)
        {
            bool flag = false;
            var checkMailExists = _dbContext.Users.Where(u => u.Email == Mail).FirstOrDefault();
            if (checkMailExists != null)
            {
                try
                {
                    string code = GenerateString.RandomString(5);
                    var query = _dbContext.Users.SingleOrDefault(u => u.Email == Mail);
                    query.ForgotPassword = code;

                    string body = "<h1><font color = \"#00b894\">Thay đổi mật khẩu của bạn</font></h1><br /><p><i><font color=\"#dfe6e9\">Chúng tôi đã nhận được yêu cầu thay đổi mật khẩu của bạn. Nếu bạn đổi ý, hãy bỏ qua email này.<br />Mã xác nhận của bạn là</font></i></p><br /><h2>" + code + "</h2>";

                    WebMail.SmtpServer = "smtp.gmail.com";
                    WebMail.SmtpPort = 587;
                    WebMail.SmtpUseDefaultCredentials = true;
                    WebMail.EnableSsl = true;
                    WebMail.UserName = "yourEmail@gmail.com";
                    WebMail.Password = "yourPassword";
                    WebMail.From = "yourGmail@gmail.com";
                    WebMail.Send(to: Mail, subject: "Yêu cầu thay đổi mật khẩu", body: body, isBodyHtml: true);
                    _dbContext.SaveChanges();
                    mailPasswd = Mail;

                    flag = true;
                }
                catch (Exception)
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

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            if (mailPasswd == "" || mailPasswd == null)
            {
                return RedirectToAction("ForgotPassword", "Account", new { area = "" });
            }
            else
            {
                ViewBag.Title = "Đổi mật khẩu";
                ViewBag.mailPasswd = mailPasswd;
                return View();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult ChangePassword(string code, string passwd)
        {
            bool flag = false;
            string password = StringHash.crypto(passwd);

            string Upper_code = code.ToUpper();
            var update_password = _dbContext.Users
                                        .Where(u => u.Email == mailPasswd)
                                        .Where(u => u.ForgotPassword == Upper_code)
                                        .FirstOrDefault();

            if (update_password != null)
            {
                update_password.ForgotPassword = "";
                update_password.Password = password;
                _dbContext.SaveChanges();
                mailPasswd = "";

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
