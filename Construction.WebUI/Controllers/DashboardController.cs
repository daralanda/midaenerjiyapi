using Construction.Dal.Context;
using Construction.Dal.Entity;
using Construction.WebUI.AppCode;
using Construction.WebUI.Models;
using System.Linq;
using System.Web.Mvc;

namespace Construction.WebUI.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            Session.Clear();
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    var result = db.Users.Where(p => p.Password == user.Password && p.Email == user.Email && p.IsActive == true).ToList();
                    if (result.Count == 1)
                    {
                        Session["User"] = result.FirstOrDefault().FirstName + " " + result.FirstOrDefault().LastName;
                        Security security = new Security();
                        var drm = security.UserSecurityAdd(new UserSecurity
                        {
                            UserId = result.FirstOrDefault().UserId,
                            RoleId = result.FirstOrDefault().RoleId
                        });
                        if (result.FirstOrDefault().RoleId == 1)
                        {
                            return Redirect("/Dashboard/");
                        }
                        else
                        {
                            return Redirect("/");
                        }

                    }
                    else
                    {
                        Session["Message"] = "Giriş denemesi başarısızdır.";
                        return Redirect("/giris-yap.html");
                    }
                }
            }
            catch
            {
                Session["Message"] = "Giriş denemesi başarısızdır.";
                return Redirect("/giris-yap.html");
            }
        }
        public ActionResult RoleList() { return View(); }
        public ActionResult BannerList() { return View(); }
        public ActionResult ContatSetting() { return View(); }
        public ActionResult UserList() { return View(); }
        public ActionResult CategoryList() { return View(); }
        public ActionResult BlogList() { return View(); }
        public ActionResult SliderList() { return View(); }
        public ActionResult LanguageList() { return View(); }
        public ActionResult GalleryList() { return View(); }
        public ActionResult SocialMediaList() { return View(); }
        public ActionResult ContactInfo() { return View(); }
        public ActionResult MailList() { return View(); }
        public ActionResult VerificationList() { return View(); }
        public ActionResult FooterPluginList() { return View(); }
        public ActionResult ProductsList() { return View(); }

    }
}