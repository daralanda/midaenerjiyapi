using Construction.Dal.Context;
using Construction.Dal.Entity;
using Construction.WebUI.AppCode;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Construction.WebUI.Controllers.Api
{
    public class BlogsApiController : ApiController
    {
        public string seoUrl = string.Empty;
        readonly DashboardMainCode mainCode = new DashboardMainCode("");
        [Route("api/GetAllBlogs")]
        [HttpGet]
        public IHttpActionResult GetAllBlogs()
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Select"))
                {
                    using (var db = new ConstructionContext())
                    {

                        var mainCategory = db.Categories.Where(p => p.MainCategoryId == 0 && p.CategoryType == 4).Select(p => new
                        {
                            p.CategoryId,
                            p.Title,
                        }).ToList();
                        var lang = db.Languages.Where(p => p.IsActive).Select(p => new
                        {
                            p.LanguageId,
                            p.LanguageName,
                            p.ShortCode
                        }).ToList();
                        var list = db.Categories.Select(p => new
                        {
                            p.CategoryId,
                            p.ImageUrl,
                            p.Title,
                            p.MainCategoryId,
                            p.Queno,
                            p.CategoryType,
                            p.IsBlog
                        }).Where(p => p.IsBlog).ToList();
                        return Ok(new
                        {
                            state = true,
                            data = list,
                            main = mainCategory,
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
        [Route("api/AddBlogs")]
        [HttpPost]
        public IHttpActionResult AddBlogs(Category category)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    db.Categories.Add(category);
                    db.SaveChanges();
                }
                return Ok(new { state = true });
            }
            catch
            {
                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }

        [Route("api/FindBlogs")]
        [HttpPost]
        public IHttpActionResult FindBlogs(Category category)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    var result = db.Categories.Where(p => p.CategoryId == category.CategoryId).FirstOrDefault();
                    return Ok(new { state = true, data = result });
                }
            }
            catch
            {
                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }
        [Route("api/DeleteBlogs")]
        [HttpDelete]
        public IHttpActionResult DeleteBlogs(Category category)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    var main = db.Categories.Where(p => p.CategoryId == category.CategoryId || p.MainCategoryId == category.CategoryId).ToList();
                    var lists = new List<UrlRecord>();
                    foreach (var item in main.Where(p=>p.IsBlog).ToList())
                    {
                        var sub = db.UrlRecords.Where(p => p.CategoryId == item.CategoryId).ToList();
                        lists.AddRange(sub);
                    }
                    db.UrlRecords.RemoveRange(lists);
                    db.Categories.RemoveRange(main);
                    db.SaveChanges();
                    foreach (var item in main)
                    {
                        mainCode.FileDelete(item.ImageUrl);
                    }
                    return Ok(new { state = true });
                }
            }
            catch
            {
                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }

        [Route("api/UpdateBlogs")]
        [HttpPut]
        public IHttpActionResult UpdateBlogs(Category category)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    var resut = db.Categories.Where(p => p.CategoryId == category.CategoryId).FirstOrDefault();
                    if (resut != null)
                    {
                        resut.CategoryType = category.CategoryType;
                        resut.ImageUrl = category.ImageUrl;
                        resut.MainCategoryId = category.MainCategoryId;
                        resut.Queno = category.Queno;
                        resut.Title = category.Title;
                        db.SaveChanges();
                        return Ok(new { state = true });
                    }
                    else
                    {
                        return Ok(new { state = false, message = "Aranılan kategori bulunamadığı için güncelleme yapılamadı." });
                    }
                }

            }
            catch
            {
                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }

        [Route("api/FindSubBlogs")]
        [HttpPost]
        public IHttpActionResult FindSubBlogs(UrlRecord data)
        {
            try
            {
                using (var db = new ConstructionContext())
                {

                    var result = db.UrlRecords.Where(p => p.CategoryId == data.CategoryId && p.LanguageId == data.LanguageId).Select(p => new {
                        p.CategoryId,
                        p.Content,
                        p.Description,
                        p.Keywords,
                        p.Title,
                        p.Category.MainCategoryId,
                        p.LanguageId,
                        p.Language.LanguageName,
                        p.SeoUrl,
                        p.UrlRecordId
                    }).FirstOrDefault();
                    string mainCategoryName = string.Empty;
                    if (result != null)
                    {
                        var mainCat = db.UrlRecords.Where(p => p.CategoryId == result.MainCategoryId && p.LanguageId == result.LanguageId).FirstOrDefault();
                        if (mainCat != null)
                        {
                            mainCategoryName = mainCat.Title;
                        }
                      
                    }
                    else
                    {
                        var mainCat = db.Categories.Where(p => p.CategoryId == data.CategoryId).FirstOrDefault();
                        if (mainCat != null && mainCat.MainCategoryId != 0)
                        {
                            mainCategoryName = db.UrlRecords.Where(p => p.CategoryId == mainCat.MainCategoryId && p.LanguageId == data.LanguageId).FirstOrDefault().Title;
                        }
                    }
                    return Ok(new { state = true, data = result, mainCategoryName });
                }
            }
            catch
            {
                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }
        [Route("api/AddSubBlogs")]
        [HttpPost]
        public IHttpActionResult AddSubBlogs(UrlRecord data)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    int secId = 0;
                    var cat = db.Categories.Where(p => p.CategoryId == data.CategoryId).FirstOrDefault();
                    if (cat != null)
                    {
                        if (cat.CategoryType == 1)
                        {
                            secId = 14;
                        }
                        else if (cat.CategoryType == 2)
                        {
                            secId = 16;
                        }
                        else
                        {
                            secId = 15;
                        }
                    }
                    data.SecurityObjectId = secId;
                    db.UrlRecords.Add(data);
                    db.SaveChanges();
                }
                return Ok(new { state = true });
            }
            catch
            {
                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }

        [Route("api/UpdateSubBlogs")]
        [HttpPut]
        public IHttpActionResult UpdateSubBlogs(UrlRecord data)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    var resut = db.UrlRecords.Where(p => p.UrlRecordId == data.UrlRecordId).FirstOrDefault();
                    if (resut != null)
                    {
                        resut.Title = data.Title;
                        resut.Keywords = data.Keywords;
                        resut.Description = data.Description;
                        resut.Content = data.Content;
                        resut.SeoUrl = data.SeoUrl;
                        resut.LanguageId = data.LanguageId;
                        db.SaveChanges();
                        return Ok(new { state = true });
                    }
                    else
                    {
                        return Ok(new { state = false, message = "Aranılan kategori bulunamadığı için güncelleme yapılamadı." });
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
