using Construction.Dal.Context;
using Construction.WebUI.AppCode;
using System.Linq;
using System.Web.Http;

namespace Construction.WebUI.Controllers.Api
{
    public class HomeFrontController : ApiController
    {
        readonly MainCode mainCode = new MainCode();
        [Route("api/GetSliders")]
        public IHttpActionResult GetSliders(string url)
        {
            using (var db = new ConstructionContext())
            {
                var lang = mainCode.GetLangId(string.IsNullOrEmpty(url)?"/":url);
                var urlRecords = db.UrlRecords.Where(p => p.SecurityObjectId == 22 && p.LanguageId== lang).ToList();
                var result = from x in urlRecords
                             join y in db.Categories.Where(p => p.CategoryType == 5).ToList() on x.CategoryId equals y.CategoryId
                             select new
                             {
                                 type = "image",
                                 alt = x.Title,
                                 title = x.Title,
                                 description = string.Format("  {0} | {1} | {2}", y.Location, y.ProjectDate, y.Meters),
                                 image = db.Galleries.Where(p => p.CategoryId == x.CategoryId).First().Url,
                                 thmb = db.Galleries.Where(p => p.CategoryId == x.CategoryId).First().Url,
                                 link=x.SeoUrl
                             };
                return Ok(new { data = result.Where(p=>p.image!=null).ToList() });
            }
           
        }
        [Route("api/GetProject")]
        public IHttpActionResult GetProject(string url)
        {
            using (var db = new ConstructionContext())
            {
                var lang = mainCode.GetLangId(string.IsNullOrEmpty(url) ? "/" : url);
              
                var urlRecords = db.UrlRecords.Where(p => p.SecurityObjectId == 22 && p.LanguageId == lang  && p.SeoUrl==url).ToList();
                var result = from x in urlRecords
                             join y in db.Categories.Where(p => p.CategoryType == 5).ToList() on x.CategoryId equals y.CategoryId
                             join t in db.Galleries.ToList() on x.CategoryId equals t.CategoryId
                             select new
                             {
                                 type = "image",
                                 alt = x.Title,
                                 title = x.Title,
                                 description = string.Format(" {0}  | {1}  | {2}", y.Location, y.ProjectDate, y.Meters),
                                 image = t.Url,
                                 thmb = t.Url
                             };
                return Ok(new { data = result.Where(p => p.image != null).ToList() });
            }

        }


    }
}