using Construction.Dal.Context;
using Construction.Dal.Entity;
using Construction.WebUI.AppCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Construction.WebUI.Controllers.Api
{
    public class SocialMediaApiController : ApiController
    {
        public string seoUrl = string.Empty;
        readonly DashboardMainCode mainCode = new DashboardMainCode("");
        [Route("api/GetAllSocialMedia")]
        [HttpGet]
        public IHttpActionResult GetAllSocialMedia()
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Select"))
                {
                    using (var db = new ConstructionContext())
                    {

                        var result = db.SocialMedias.OrderBy(p=>p.Queno).ToList();

                        return Ok(new
                        {
                            state = true,
                            data = result
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
        [Route("api/DeleteSocialMedia")]
        [HttpDelete]
        public IHttpActionResult DeleteSocialMedia(SocialMedia data)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Delete"))
                {
                    using (var db = new ConstructionContext())
                    {

                        var delete = db.SocialMedias.Where(p => p.SocialMediaId == data.SocialMediaId).FirstOrDefault();
                        db.SocialMedias.Remove(delete);
                        db.SaveChanges();
                        return Ok(new
                        {
                            state = true
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
        [Route("api/AddSocialMedia")]
        [HttpPost]
        public IHttpActionResult AddSocialMedia(SocialMedia data)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Insert"))
                {
                    using (var db = new ConstructionContext())
                    {
                        db.SocialMedias.Add(data);
                        db.SaveChanges();
                        return Ok(new { state = true });
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
        [Route("api/UpdateSocialMedia")]
        [HttpPut]
        public IHttpActionResult UpdateSocialMedia(SocialMedia data)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Update"))
                {
                    using (var db = new ConstructionContext())
                    {
                        var result = db.SocialMedias.Where(p => p.SocialMediaId == data.SocialMediaId).FirstOrDefault();
                        if (result!=null)
                        {
                            result.SocialMediaName = data.SocialMediaName;
                            result.IsActive = data.IsActive;
                            result.Icon = data.Icon;
                            result.PostUrl = data.PostUrl;
                            result.Queno = data.Queno;
                            db.SaveChanges();
                        }
                        return Ok(new { state = true });
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
        [Route("api/FindSocialMedia")]
        [HttpPost]
        public IHttpActionResult FindSocialMedia(SocialMedia data)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Select"))
                {
                    using (var db = new ConstructionContext())
                    {
                        var result = db.SocialMedias.Where(p => p.SocialMediaId == data.SocialMediaId).FirstOrDefault();
                        return Ok(new { state = true,data=result });
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
     }
}
