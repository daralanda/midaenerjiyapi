using Construction.Dal.Context;
using Construction.Dal.Entity;
using Construction.WebUI.AppCode;
using System;
using System.Linq;
using System.Web.Http;

namespace Construction.WebUI.Controllers.Api
{
    public class GalleryApiController : ApiController
    {

        public string seoUrl = string.Empty;
        readonly DashboardMainCode mainCode = new DashboardMainCode("");
        [Route("api/GetAllGallery")]
        [HttpGet]
        public IHttpActionResult GetAllGallery()
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
                        var list = db.Galleries.Select(p => new
                        {
                            p.GalleryId,
                            p.Url,
                            p.Categories.Title
                        }).ToList();
                        var cat = db.Categories.Where(p => p.IsBlog == false && p.CategoryType == 4).OrderBy(p => p.Queno).Select(p => new
                        {
                            id=p.CategoryId,
                            text=p.Title
                        }).ToList();
                        return Ok(new
                        {
                            state = true,
                            data = list,
                            lang,
                            cat
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
        [Route("api/AddGallery")]
        [HttpPost]
        public IHttpActionResult AddGallery(Gallery data)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    db.Galleries.Add(data);
                    db.SaveChanges();
                }
                return Ok(new { state = true });
            }
            catch
            {
                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }

        [Route("api/FindGallery")]
        [HttpPost]
        public IHttpActionResult FindGallery(Gallery data)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    var result = db.Galleries.Where(p => p.GalleryId == data.GalleryId).Select(p=>new { 
                    p.CategoryId,
                    p.GalleryId,
                    p.Url
                    }).FirstOrDefault();
                    return Ok(new { state = true, data = result });
                }
            }
            catch
            {
                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }
        [Route("api/DeleteGallery")]
        [HttpDelete]
        public IHttpActionResult DeleteGallery(Gallery data)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    var main = db.Galleries.Where(p => p.GalleryId == data.GalleryId).FirstOrDefault();
                    db.Galleries.Remove(main);
                    db.SaveChanges();
                    if (main != null)
                    {
                        mainCode.FileDelete(main.Url);
                    }
                    return Ok(new { state = true });
                }
            }
            catch
            {
                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }

        [Route("api/UpdateGallery")]
        [HttpPut]
        public IHttpActionResult UpdateGallery(Gallery data)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    var resut = db.Galleries.Where(p => p.GalleryId == data.GalleryId).FirstOrDefault();
                    if (resut != null)
                    {
                        resut.Url = data.Url;
                        db.SaveChanges();
                        return Ok(new { state = true });
                    }
                    else
                    {
                        return Ok(new { state = false, message = "Aranılan Gallery bulunamadığı için güncelleme yapılamadı." });
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
