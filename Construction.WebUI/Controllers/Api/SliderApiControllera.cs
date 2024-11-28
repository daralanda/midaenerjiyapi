using Construction.Dal.Context;
using Construction.Dal.Entity;
using Construction.WebUI.AppCode;
using System.Linq;
using System.Web.Http;

namespace Construction.WebUI.Controllers.Api
{
    public class SliderApiController : ApiController
    {
        public string seoUrl = string.Empty;
        readonly DashboardMainCode mainCode = new DashboardMainCode("");
        [Route("api/GetAllSlider")]
        [HttpGet]
        public IHttpActionResult GetAllSlider()
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
                        var list = db.Sliders.Select(p => new
                        {
                            p.SliderId,
                            p.ImageUrl,
                            p.Title,
                            p.Queno
                        }).ToList();
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
        [Route("api/AddSlider")]
        [HttpPost]
        public IHttpActionResult AddSlider(Slider data)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    db.Sliders.Add(data);
                    db.SaveChanges();
                }
                return Ok(new { state = true });
            }
            catch
            {
                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }

        [Route("api/FindSlider")]
        [HttpPost]
        public IHttpActionResult FindSlider(Slider data)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    var result = db.Sliders.Where(p => p.SliderId == data.SliderId).FirstOrDefault();
                    return Ok(new { state = true, data = result });
                }
            }
            catch
            {
                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }
        [Route("api/DeleteSlider")]
        [HttpDelete]
        public IHttpActionResult DeleteSlider(Slider data)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    var main = db.Sliders.Where(p => p.SliderId == data.SliderId).FirstOrDefault();
                    var sub = db.SliderConversions.Where(p => p.SliderId == data.SliderId).ToList();
                    db.SliderConversions.RemoveRange(sub);
                    db.Sliders.Remove(main);
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

        [Route("api/UpdateSlider")]
        [HttpPut]
        public IHttpActionResult UpdateSlider(Slider data)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    var resut = db.Sliders.Where(p => p.SliderId == data.SliderId).FirstOrDefault();
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
                        return Ok(new { state = false, message = "Aranılan slider bulunamadığı için güncelleme yapılamadı." });
                    }
                }

            }
            catch
            {
                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }

        [Route("api/FindSubSlider")]
        [HttpPost]
        public IHttpActionResult FindSubSlider(SliderConversion data)
        {
            try
            {
                using (var db = new ConstructionContext())
                {

                    var result = db.SliderConversions.Where(p => p.SliderId == data.SliderId && p.LanguageId == data.LanguageId).Select(p => new {
                        p.SliderId,
                        p.Title,
                        p.Description,
                        p.PostUrl,
                        p.LanguageId,
                        p.Language.LanguageName,
                        p.SliderConversionId
                    }).FirstOrDefault();                 
                    return Ok(new { state = true, data = result });
                }
            }
            catch
            {
                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }
        [Route("api/AddSubSlider")]
        [HttpPost]
        public IHttpActionResult AddSubSlider(SliderConversion data)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                   db.SliderConversions.Add(data);
                    db.SaveChanges();
                }
                return Ok(new { state = true });
            }
            catch
            {
                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }

        [Route("api/UpdateSubSlider")]
        [HttpPut]
        public IHttpActionResult UpdateSubSlider(SliderConversion data)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    var resut = db.SliderConversions.Where(p => p.SliderConversionId == data.SliderConversionId).FirstOrDefault();
                    if (resut != null)
                    {
                        resut.Title = data.Title;
                        resut.Description = data.Description;
                        resut.PostUrl = data.PostUrl;
                        db.SaveChanges();
                        return Ok(new { state = true });
                    }
                    else
                    {
                        return Ok(new { state = false, message = "Aranılan Slider bulunamadığı için güncelleme yapılamadı." });
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
