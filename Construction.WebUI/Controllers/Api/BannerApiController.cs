using Construction.Dal.Context;
using Construction.Dal.Entity;
using Construction.WebUI.AppCode;
using System.Linq;
using System.Web.Http;

namespace Construction.WebUI.Controllers.Api
{
    public class BannerApiController : ApiController
    {
        public string seoUrl = string.Empty;
        readonly DashboardMainCode mainCode = new DashboardMainCode("");
        [Route("api/GetAllBanner")]
        [HttpGet]
        public IHttpActionResult GetAllBanner()
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Select"))
                {
                    using (var db = new ConstructionContext())
                    {

                        var lang = db.Languages.Where(p => p.IsActive).Select(p => new
                        {
                            p.LanguageId,
                            p.LanguageName,
                            p.ShortCode
                        }).ToList();
                        var list = db.Banners.Select(p => new
                        {
                            p.BannerId,
                            p.ImageUrl,
                            p.Title,
                            p.Queno
                        }).OrderBy(p=>p.Queno).ToList();
                        return Ok(new
                        {
                            state = true,
                            data = list,
                            lang
                        });
                    }
                }
                else
                {
                    return Ok(new { state = true, message = "Yetkiniz Bulunmamaktadır" });
                }
            }
            catch
            {
                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }
        [Route("api/AddBanner")]
        [HttpPost]
        public IHttpActionResult AddBanner(Banner data)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    db.Banners.Add(data);
                    db.SaveChanges();
                }
                return Ok(new { state = true });
            }
            catch
            {
                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }

        [Route("api/FindBanner")]
        [HttpPost]
        public IHttpActionResult FindBanner(Banner data)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    var result = db.Banners.Where(p => p.BannerId == data.BannerId).FirstOrDefault();
                    return Ok(new { state = true, data = result });
                }
            }
            catch
            {
                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }
        [Route("api/DeleteBanner")]
        [HttpDelete]
        public IHttpActionResult DeleteBanner(Banner data)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    var main = db.Banners.Where(p => p.BannerId == data.BannerId).FirstOrDefault();
                    db.Banners.Remove(main);
                    db.SaveChanges();
                    if (main!=null)
                    {
                        mainCode.FileDelete(main.ImageUrl);
                    }
                    return Ok(new { state = true });
                }
            }
            catch
            {
                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }

        [Route("api/UpdateBanner")]
        [HttpPut]
        public IHttpActionResult UpdateBanner(Banner data)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    var resut = db.Banners.Where(p => p.BannerId == data.BannerId).FirstOrDefault();
                    if (resut != null)
                    {
                        resut.Queno = data.Queno;
                        resut.Title = data.Title;
                        resut.ImageUrl = data.ImageUrl;
                        db.SaveChanges();
                        return Ok(new { state = true });
                    }
                    else
                    {
                        return Ok(new { state = false, message = "Aranılan Banner bulunamadığı için güncelleme yapılamadı." });
                    }
                }

            }
            catch
            {
                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }
    }
}
