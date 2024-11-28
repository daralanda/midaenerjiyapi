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
    public class FooterPluginApiController : ApiController
    {
        public string seoUrl = string.Empty;
        readonly DashboardMainCode mainCode = new DashboardMainCode("");
        [Route("api/GetAllFooterPlugin")]
        [HttpGet]
        public IHttpActionResult GetAllFooterPlugin()
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Select"))
                {
                    using (var db = new ConstructionContext())
                    {

                        var result = db.FooterPlugins.Select(p=>new {
                            p.FooterPluginId,
                            p.Direction,
                            p.LanguageId,
                            p.MainFooterPluginId,
                            p.PostUrl,
                            p.Title,
                            p.Language.LanguageName,
                            p.Language.ShortCode
                        }).OrderByDescending(p=>p.FooterPluginId).ToList();
                        var lang = db.Languages.Select(p=>new { 
                        p.LanguageId,
                        p.LanguageName,
                        p.Queno,
                        p.IsActive,
                        }).Where(p => p.IsActive).OrderBy(p=>p.Queno).ToList();
                        var main= db.FooterPlugins.Select(p => new {
                            p.FooterPluginId,
                            p.Direction,
                            p.LanguageId,
                            p.MainFooterPluginId,
                            p.PostUrl,
                            p.Title
                        }).Where(p=>p.MainFooterPluginId==0).OrderByDescending(p => p.FooterPluginId).ToList();
                        return Ok(new
                        {
                            state = true,
                            data = result,
                            lang,
                            main
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
        [Route("api/DeleteFooterPlugin")]
        [HttpDelete]
        public IHttpActionResult DeleteFooterPlugin(FooterPlugin data)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Delete"))
                {
                    using (var db = new ConstructionContext())
                    {

                        var delete = db.FooterPlugins.Where(p => p.FooterPluginId == data.FooterPluginId).FirstOrDefault();
                        db.FooterPlugins.Remove(delete);
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
        [Route("api/AddFooterPlugin")]
        [HttpPost]
        public IHttpActionResult AddFooterPlugin(FooterPlugin data)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Insert"))
                {
                    using (var db = new ConstructionContext())
                    {
                        db.FooterPlugins.Add(data);
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
        [Route("api/UpdateFooterPlugin")]
        [HttpPut]
        public IHttpActionResult UpdateFooterPlugin(FooterPlugin data)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Update"))
                {
                    using (var db = new ConstructionContext())
                    {
                        var result = db.FooterPlugins.Where(p => p.FooterPluginId == data.FooterPluginId).FirstOrDefault();
                        if (result!=null)
                        {
                            result.LanguageId = data.LanguageId;
                            result.MainFooterPluginId = data.MainFooterPluginId;
                            result.Title = data.Title;
                            result.PostUrl = data.PostUrl;
                            result.Direction = data.Direction;

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
        [Route("api/FindFooterPlugin")]
        [HttpPost]
        public IHttpActionResult FindFooterPlugin(FooterPlugin data)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Select"))
                {
                    using (var db = new ConstructionContext())
                    {
                        var result = db.FooterPlugins.Where(p => p.FooterPluginId == data.FooterPluginId).Select(p=>new { 
                        p.FooterPluginId,
                        p.Direction,
                        p.LanguageId,
                        p.MainFooterPluginId,
                        p.PostUrl,
                        p.Title
                        }).FirstOrDefault();
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
