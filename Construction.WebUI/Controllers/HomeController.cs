using Construction.Dal.Context;
using System.Linq;
using System.Web.Mvc;
using PagedList;

namespace Construction.WebUI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            using (var db=new ConstructionContext())
            {
             var result= db.UrlRecords.ToList();
            }
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult PageList(int? page)
        {
            try
            {
                int id = int.Parse(ControllerContext.RouteData.Values["id"].ToString());
                using (var db = new ConstructionContext())
                {
                    var result = db.Categories.Where(p => p.MainCategoryId == id ).OrderByDescending(p => p.Queno).ToList();
                    return PartialView(result.ToPagedList(page ?? 1, 2));
                }
            }
            catch
            {
                return View();
            }
        }
        public ActionResult PageDetails()
        {
            return View();
        }
        public ActionResult ErrorPage()
        {
            return View();
        }
        public ActionResult SiteMap()
        {
            return View();
        }
        public ActionResult RobotsTxt()
        {
            return View();
        }
		public ActionResult ServiceDetails()
		{
			return View();
		}
		public ActionResult Projects()
		{
			return View();
		}
		public ActionResult ProjectDetails()
		{
			return View();
		}
	}
}