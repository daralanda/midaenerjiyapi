using Construction.Dal.Context;
using Construction.Dal.Entity;
using Construction.WebUI.AppCode;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Construction.WebUI.Controllers.Api
{
    public class CategoryApiController : ApiController
    {
        public string seoUrl = string.Empty;
        readonly DashboardMainCode mainCode = new DashboardMainCode("");
        [Route("api/GetAllCategories")]
        [HttpGet]
        public IHttpActionResult GetAllCategories()
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Select"))
                {
                    using (var db = new ConstructionContext())
                    {

                        var mainCategory = db.Categories.Where(p => p.MainCategoryId == 0 ) .Select(p => new
                        {
                            p.CategoryId,
                            p.Title,
                        }).ToList();
                        var lang = db.Languages.Where(p=>p.IsActive).Select(p => new
                        {
                            p.LanguageId,
                            p.LanguageName,
                            p.ShortCode
                        }).ToList();
                        var list = db.Categories.Where(p=>!p.IsBlog && p.CategoryType!=5 && p.CategoryType!=6).Select(p => new
                        {
                            p.CategoryId,
                            p.ImageUrl,
                            p.Title,
                            p.MainCategoryId,
                            p.Queno,
                            p.CategoryType
                        }).ToList();
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
        [Route("api/AddCategory")]
        [HttpPost]
        public IHttpActionResult AddCategory(Category category)
        {
            try
            {
                using (var db=new ConstructionContext())
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

        [Route("api/FindCategory")]
        [HttpPost]
        public IHttpActionResult FindCategory(Category category)
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
        [Route("api/DeleteCategory")]
        [HttpDelete]
        public IHttpActionResult DeleteCategory(Category category)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    var main = db.Categories.Where(p => p.CategoryId == category.CategoryId || p.MainCategoryId==category.CategoryId).ToList();
                    var lists = new List<UrlRecord>();
                    foreach (var item in main.Where(p=>!p.IsBlog).ToList())
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
                    return Ok(new { state = true});
                }
            }
            catch
            {
                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }

        [Route("api/UpdateCategory")]
        [HttpPut]
        public IHttpActionResult UpdateCategory(Category category)
        {
            try
            {
                using (var db=new ConstructionContext())
                {
                    var resut = db.Categories.Where(p => p.CategoryId == category.CategoryId).FirstOrDefault();
                    if (resut!=null)
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
                        return Ok(new { state = false, message="Aranılan kategori bulunamadığı için güncelleme yapılamadı." });
                    }
                }
              
            }
            catch
            {
                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }

        [Route("api/FindSubCategory")]
        [HttpPost]
        public IHttpActionResult FindSubCategory(UrlRecord data)
        {
            try
            {
                using (var db = new ConstructionContext())
                {

                    var result = db.UrlRecords.Where(p => p.CategoryId == data.CategoryId && p.LanguageId==data.LanguageId).Select(p=>new { 
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

                    if (result!=null)
                    {
                        var mainCat = db.UrlRecords.Where(p => p.CategoryId == result.MainCategoryId && p.LanguageId == result.LanguageId).FirstOrDefault();
                        if (mainCat != null )
                        {
                            mainCategoryName = mainCat.Title;
                        }
                    }
                    else
                    {
                        var mainCat = db.Categories.Where(p => p.CategoryId == data.CategoryId).FirstOrDefault();
                        if (mainCat != null && mainCat.MainCategoryId!=0)
                        {
                            mainCategoryName = db.UrlRecords.Where(p => p.CategoryId == mainCat.MainCategoryId && p.LanguageId == data.LanguageId).FirstOrDefault().Title;
                        }
                    }
                    return Ok(new { state = true, data = result, mainCategoryName  });
                }
            }
            catch
            {
                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }
        [Route("api/AddSubCategory")]
        [HttpPost]
        public IHttpActionResult AddSubCategory(UrlRecord data)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    int secId = 0;
                    var cat = db.Categories.Where(p => p.CategoryId == data.CategoryId).FirstOrDefault();
                    if (cat!=null)
                    {
                        if (cat.CategoryType==1)
                        {
                            secId = 14;
                        }
                        else if (cat.CategoryType==2)
                        {
                            secId = 16;
                        }
                        else if (cat.CategoryType==3)
                        {
                            secId = 19;
                        }
                        else if (cat.CategoryType == 4)
                        {
                            secId = 20;
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

        [Route("api/UpdateSubCategory")]
        [HttpPut]
        public IHttpActionResult UpdateSubCategory(UrlRecord data)
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
