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
    public class LanguageApiController : ApiController
    {
        public string seoUrl = string.Empty;
        readonly DashboardMainCode mainCode = new DashboardMainCode("");
        [Route("api/GetAllLanguage")]
        [HttpGet]
        public IHttpActionResult GetAllLanguage()
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Select"))
                {
                    using (var db = new ConstructionContext())
                    {

                        var result = db.Languages.Select(p => new
                        {
                            p.LanguageId,
                            p.ShortCode,
                            p.LanguageName,
                            p.IsActive,
                            p.MetaKeywords,
                            p.Description,
                            p.Title,
                            p.UrlText,
                            p.Queno,
                            p.ContactUrl
                        }).OrderBy(p=>p.Queno).ToList();
                        var keys = db.ConversionKeys.Select(p=>new {
                            p.ConversionKeyId,
                            p.ConversionKeyName,
                            p.ConversionKeyDescription
                        }).ToList();

                        return Ok(new
                        {
                            state = true,
                            data = result,
                            keys
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
        [Route("api/DeleteLanguage")]
        [HttpDelete]
        public IHttpActionResult DeleteLanguage(Language data)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Delete"))
                {
                    using (var db = new ConstructionContext())
                    {

                        var deleteLang = db.Languages.Where(p => p.LanguageId == data.LanguageId).FirstOrDefault();
                        var deleteLanguageConversions = db.LanguageConversions.Where(p => p.LanguageId == data.LanguageId).ToList();
                        var delete = db.SliderConversions.Where(p => p.LanguageId == data.LanguageId).ToList();
                        var asd = db.UrlRecords.Where(p => p.LanguageId == data.LanguageId).ToList();
                        db.UrlRecords.RemoveRange(asd);
                        db.SliderConversions.RemoveRange(delete);
                        db.LanguageConversions.RemoveRange(deleteLanguageConversions);
                        db.Languages.Remove(deleteLang);
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
        [Route("api/AddLanguage")]
        [HttpPost]
        public IHttpActionResult AddLanguage(Language data)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Insert"))
                {
                    using (var db = new ConstructionContext())
                    {
                        db.Languages.Add(data);
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
        [Route("api/UpdateLanguage")]
        [HttpPut]
        public IHttpActionResult UpdateLanguage(Language data)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Update"))
                {
                    using (var db = new ConstructionContext())
                    {
                        var result = db.Languages.Where(p => p.LanguageId == data.LanguageId).FirstOrDefault();
                        if (result!=null)
                        {
                            result.IsActive = data.IsActive;
                            result.LanguageName = data.LanguageName;
                            result.Description = data.Description;
                            result.Title = data.Title;
                            result.MetaKeywords = data.MetaKeywords;
                            result.Queno = data.Queno;
                            result.MetaKeywords = data.MetaKeywords;
                            result.ContactUrl = data.ContactUrl;
                            result.ContactString = data.ContactString;
                            result.UrlString = data.UrlString;
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
        [Route("api/FindLanguage")]
        [HttpPost]
        public IHttpActionResult FindLanguage(Language data)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Select"))
                {
                    using (var db = new ConstructionContext())
                    {
                        var result = db.Languages.Where(p => p.LanguageId == data.LanguageId).FirstOrDefault();
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
        [Route("api/FindSubLanguage")]
        [HttpPost]
        public IHttpActionResult FindSubLanguage(Language data)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Select"))
                {
                    using (var db = new ConstructionContext())
                    {
                        var result = db.LanguageConversions.Where(p => p.LanguageId == data.LanguageId).
                            Select(p=>new { 
                            p.ConversionKeyId,
                            p.ConversionKey.ConversionKeyDescription,
                            p.ConversionKey.ConversionKeyName,
                            p.LanguageId,
                            p.LanguageConversionId,
                            p.Value
                            }).ToList();
                        return Ok(new { state = true, data = result });
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
        [Route("api/AddSubLanguage")]
        [HttpPost]
        public IHttpActionResult AddSubLanguage(List<LanguageConversion> data)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Select"))
                {
                    using (var db = new ConstructionContext())
                    {
                        int Id = data.FirstOrDefault().LanguageId;
                        var delete=   db.LanguageConversions.Where(p => p.LanguageId == Id).ToList();
                        db.LanguageConversions.RemoveRange(delete);
                        db.LanguageConversions.AddRange(data);
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
    }
}
