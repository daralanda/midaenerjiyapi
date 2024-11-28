using Construction.Dal.Context;
using Construction.Dal.Entity;
using Construction.WebUI.AppCode;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Construction.WebUI.Controllers.Api
{
    public class ProductController : ApiController
    {
        public string seoUrl = string.Empty;
        readonly DashboardMainCode mainCode = new DashboardMainCode("");
        [Route("api/GetAllProducts")]
        [HttpGet]
        public IHttpActionResult GetAllProduct()
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Select"))
                {
                    using (var db = new ConstructionContext())
                    {

                        var mainCategory = db.Categories.Where(p => p.CategoryType== 2) .Select(p => new
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
                        var list = db.Categories.Where(p=>!p.IsBlog && p.CategoryType==5).Select(p => new
                        {
                            p.CategoryId,
							ImageUrl=p.Galleries.FirstOrDefault()!=null?p.Galleries.FirstOrDefault().Url:null,
                            p.Title,
                            p.MainCategoryId,
                            p.Queno,
                            p.CategoryType
                        }).OrderBy(p=>p.Queno).ToList();
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
        [Route("api/AddProduct")]
        [HttpPost]
        public IHttpActionResult AddProduct(Category category)
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

        [Route("api/FindProduct")]
        [HttpPost]
        public IHttpActionResult FindProduct(Category category)
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
        [Route("api/DeleteProduct")]
        [HttpDelete]
        public IHttpActionResult DeleteProduct(Category category)
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

        [Route("api/UpdateProduct")]
        [HttpPut]
        public IHttpActionResult UpdateProduct(Category category)
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

        [Route("api/FindSubProduct")]
        [HttpPost]
        public IHttpActionResult FindSubProduct(UrlRecord data)
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
        [Route("api/AddSubProduct")]
        [HttpPost]
        public IHttpActionResult AddSubProduct(UrlRecord data)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    int secId = 0;
                    var cat = db.Categories.Where(p => p.CategoryId == data.CategoryId).FirstOrDefault();
                    if (cat!=null)
                    {
                        secId = 20;
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

        [Route("api/UpdateSubProduct")]
        [HttpPut]
        public IHttpActionResult UpdateSubProduct(UrlRecord data)
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

        [Route("api/GetAllProductsJson")]
        [HttpGet]
        public IHttpActionResult GetAllProductsJson()
        {
            using (var db=new ConstructionContext())
            {
                var main = db.UrlRecords.Where(p => !p.Category.IsBlog && p.Category.CategoryType == 4).
                    Select(p => new
                    {
                        p.SeoUrl,
                        p.Title,
                        p.Category.Galleries.FirstOrDefault().Url
                    }).ToList();
                return Ok(main);
            }
            
        }
    }
}
