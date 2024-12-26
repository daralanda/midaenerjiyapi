using Construction.Dal.Entity;
using System.Collections.Generic;
using System.Linq;
using Construction.Dal.Context;
using System.Configuration;
using Construction.WebUI.Models;
using System;
using System.Web;

namespace Construction.WebUI.AppCode
{
    public class MainCode
    {
       
        public string _domain = string.Empty;
        public int _languageId = 0;
        private string _seoUrl = string.Empty;
        public string _homepage = string.Empty;

        public string ProjectName = ConfigurationManager.AppSettings["ProjectName"];
        public MainCode(string domain,string SeoUrl)
        {
            _seoUrl = SeoUrl;
            _domain = domain;
            _languageId = GetLangId(SeoUrl);
        }
        public MainCode()
        {

        }
        public int GetLangId(string Url)
        {
            int Id = 0;
            try
            {
                using (var db = new ConstructionContext())
                {
                    var data = db.UrlRecords.Where(p => p.SeoUrl == Url).FirstOrDefault();
                    if (data != null)
                    {
                        Id = data.LanguageId;
                    }
                    else
                    {
                        Id = db.Languages.Where(p => p.IsActive && (p.UrlText == Url || p.ContactUrl == Url)).FirstOrDefault().LanguageId;
                    }

                    _homepage = _domain + db.Languages.Where(p => p.LanguageId == Id).FirstOrDefault().UrlText;
                }
            }
            catch
            {
                Id = 5;
            }
            return Id;
        }
        public string GetLang()
        {
            string result = string.Empty;
            using (var db = new ConstructionContext())
            {
                var data = db.Languages.Where(p => p.IsActive).OrderBy(p => p.Queno).ToList();
                if (data.Count > 0)
                {
                    result = "<button class='menu-item-has-children' type='button' data-bs-toggle='dropdown' aria-expanded='false'>" + data.Where(p => p.LanguageId == _languageId).FirstOrDefault().LanguageName + "</button><ul class='sub-menu'>";
                }
                foreach (var item in data)
                {
                    result += "<li ><a class='dropdown-item' href='" + item.UrlText + "' >" + item.LanguageName + "</a></li>";
                }
                result += "</ul>";
            }
            return result;
        }
        public string GetMainMenu()
        {
            string snc = string.Empty;
            using (var db=new ConstructionContext())
            {
                var result = db.UrlRecords.Where(p => !p.Category.IsBlog && p.LanguageId==_languageId && p.Category.CategoryType!=6 && p.Category.CategoryType != 5).OrderBy(p => p.Category.Queno).ToList();
                var defaultData = db.Languages.Where(p => p.LanguageId == _languageId).FirstOrDefault();
                //Anasayfa Link
                snc= (defaultData.UrlText!= null || defaultData.UrlText!=string.Empty) ? "<li><a href='"+_domain+defaultData.UrlText+"'>"+ defaultData.UrlString + "</a></li>" : string.Empty;
                var main = result.Where(p => p.Category.MainCategoryId == 0).ToList() ;
                foreach (var item in main)
                {
                    var sub = result.Where(p => p.Category.MainCategoryId == item.CategoryId && !p.Category.IsBlog && p.Category.CategoryType != 5).ToList();
                    if (sub.Count>0)
                    {
                        snc += "<li class='menu-item-has-children'><a href='#' >" + item.Title + "</a><ul class='sub-menu'>";
                        foreach (var subitem in sub)
                        {
                            snc += "<li><a href='" + _domain + subitem.SeoUrl + "'  class='dropdown-item'>" + subitem.Title + "</a></li>";
                        }
                        snc += "</ul></li>";
                    }
                    else
                    {
                        snc += "<li><a href='" + _domain + item.SeoUrl + "' class='nav-link'>" + item.Title + "</a></li>";
                    }
                }
                //İletişim Link
                snc += (defaultData.ContactUrl != null || defaultData.ContactUrl != string.Empty) ? "<li><a href='" + _domain + defaultData.ContactUrl + "' >" + defaultData.ContactString + "</a></li>" : string.Empty;

            }
            return snc;
        }
        public string GetProductMenu()
        {
            string snc = string.Empty;
            using (var db = new ConstructionContext())
            {
                var result = db.UrlRecords.Where(p => !p.Category.IsBlog && p.LanguageId == _languageId && p.Category.CategoryType == 3).OrderBy(p => p.Category.Queno).ToList();
                //Anasayfa Link
                if (result!=null && result.Count>0)
                {
                    snc += "<button id='prooduct-menu' class='header-department-text department-title'>" +
                    "<span class='icon'><i class='fal fa-bars'></i></span>" +
                    "<span class='text'>" + result.Where(p => p.Category.MainCategoryId == 0).FirstOrDefault().Title + "</span>" +
                    "</button>" +
                    "<nav class='department-nav-menu'>" +
                    "<button class='sidebar-close'><i class='fas fa-times'></i></button>" +
                    "<ul class='nav-menu-list'>";
                    var main = result.Where(p => p.Category.MainCategoryId != 0).ToList();
                    foreach (var item in main)
                    {
                        snc += "<li><a href='" + _domain + item.SeoUrl + "' class='nav-link'><span class='menu-text'>" + item.Title + "</span></a></li>";
                    }
                    snc += "</ul></nav>";
                }
            }
            return snc;
        }
        public string GetMobileMenu()
        {
            string snc = string.Empty;
            using (var db = new ConstructionContext())
            {
                var result = db.UrlRecords.Where(p => !p.Category.IsBlog && p.LanguageId == _languageId).OrderBy(p => p.Category.Queno).ToList();
                var defaultData = db.Languages.Where(p => p.LanguageId == _languageId).FirstOrDefault();
                //Anasayfa Link
                snc = (defaultData.UrlText != null || defaultData.UrlText != string.Empty) ? "<li><a href='" + _domain + defaultData.UrlText + "'>" + defaultData.UrlString + "</a></li>" : string.Empty;
                var main = result.Where(p => p.Category.MainCategoryId == 0).ToList();
                foreach (var item in main)
                {
                    var sub = result.Where(p => p.Category.MainCategoryId == item.CategoryId && !p.Category.IsBlog).ToList();
                    if (sub.Count > 0)
                    {
                        snc += "<li><a href='" + _domain + item.SeoUrl + "' >" + item.Title + "</a><ul>";
                        foreach (var subitem in sub)
                        {
                            snc += "<li><a href='" + _domain + subitem.SeoUrl + "'>" + subitem.Title + "</a></li>";
                        }
                        snc += "</ul></li>";
                    }
                    else
                    {
                        snc += "<li><a href='" + _domain + item.SeoUrl + "'>" + item.Title + "</a></li>";
                    }
                }
                //İletişim Link
                snc += (defaultData.ContactUrl != null || defaultData.ContactUrl != string.Empty) ? "<li><a href='" + _domain + defaultData.ContactUrl + "' >" + defaultData.ContactString + "</a></li>" : string.Empty;

            }
            return snc;
        }
        public string GetSocialMedia()
        {
            using (var db=new ConstructionContext())
            {
                var result=db.SocialMedias.Where(p=>p.IsActive).OrderBy(p=>p.Queno).ToList();
                string resultString = string.Empty;
                foreach (var item in result)
                {
                    resultString += "<li><a href='"+item.PostUrl+"' class='"+item.Icon+ "' title='"+item.SocialMediaName+"' target='_blank'></a></li>";
                }
                return string.Format(@"<ul class='socials_list'>{0}</ul>",resultString);
            }
        }
        public SecurityObject FindUrl(string url)
        {
            SecurityObject data = new SecurityObject();
            using (var db = new ConstructionContext())
            {
                var result = db.UrlRecords.Where(p => p.SeoUrl == url && p.SecurityObject.IsActive).FirstOrDefault();
                if (result != null)
                {
                    data.SecurityObjectId = result.UrlRecordId;
                    data.ControllerName = result.SecurityObject.ControllerName;
                    data.ActionName = result.SecurityObject.ActionName;
                }
                else
                {
                    var sec = db.SecurityObjects.Where(p => p.SeoUrl == url).FirstOrDefault();
                    if (sec != null)
                    {
                        data.ControllerName = sec.ControllerName;
                        data.ActionName = sec.ActionName;
                    }
                    else
                    {
                        var mainPage = db.Languages.Where(p => p.UrlText == url).FirstOrDefault();
                        if (mainPage!=null)
                        {
                            data.ControllerName = "Home";
                            data.ActionName = "Index";
                            _languageId = mainPage.LanguageId;

                        }
                        else
                        {
                            var contact = db.Languages.Where(p => p.ContactUrl == url).FirstOrDefault();
                            if (contact!=null)
                            {
                                data.ControllerName = "Home";
                                data.ActionName = "Contact";
                                _languageId = contact.LanguageId;
                            }
                        }
                    }
                }
            }
            return data;
        }
        public string Conversion(string Key)
        {
            string snc = string.Empty;
            try
            {
                using (var db = new ConstructionContext())
                {
                    var data = db.LanguageConversions.Where(p => p.ConversionKey.ConversionKeyName == Key && p.LanguageId == _languageId).FirstOrDefault();
                    if (data != null && data.Value != string.Empty && data.Value != null)
                    {
                        snc = data.Value;
                    }
                    else
                    {
                        snc = Key;
                    }
                }
            }
            catch 
            {
                snc = Key;
            }
            return snc;
        }

        public string GetHeaderTopContact()
        {
            string snc = string.Empty;
            using (var db=new ConstructionContext())
            {
                var result = db.ContactInfos.Where(p => p.IsDefault).FirstOrDefault();
                if (result!=null)
                {
                    snc= "<li><a href='tel: "+result.Phone+ "' class='ff'><i class='icon-phone'></i> " + result.Phone+"</a></li>" +
                            "<li><a href='mailto: " + result.Email+ "' class='ff'><i class='icon-envelope'></i>  " + result.Email+"</a></li>";
                }
            }
            return snc;
        }
        public string GetSlider()
        {
            string snc = string.Empty;
            using (var db=new ConstructionContext())
            {
                var sliderList = db.Sliders.OrderBy(p=>p.Queno).ToList();
                var sliderConversion = db.SliderConversions.Where(p => p.LanguageId == _languageId).ToList();
                foreach (var item in sliderList)
                {
                    var sub = sliderConversion.Where(p => p.SliderId == item.SliderId).FirstOrDefault();
                    if (sub!=null)
                    {
						//snc += "<div class='intro-content intro-content-right'>" +
						//                "<h3 class='intro-title font-weight-bold text-white mb-0'>" + sub.Title + "</h3>" +
						//                "<h6 class='font-weight-normal text-white my-2 mt-0'>" + sub.Description + "</h6>" +
						//                "<a href='" + sub.PostUrl + "' targett='_blank' class='btn btn-primary text-uppercase'> Detaylı İncele</a>" +
						////            "</div>";
						//snc += String.Format(@"<div class='single-slide slick-slide'>
						//                 <div class='main-slider-content'>
						//                  <span class='subtitle'>{0}</span>
						//                  <h2 class='title'> {1}</h2>
						//                  <div class='shop-btn'><a href='{3}' class='axil-btn'>{4}<i class='fal fa-long-arrow-right'></i></a></div>
						//                 </div>
						//                 <div class='main-slider-thumb'><img src='{2}' alt='{3}'></div>
						//                </div>", sub.Title,sub.Description,_domain+item.ImageUrl,sub.PostUrl, Conversion("home_slider_button"));
						snc += String.Format(@"  <div class='th-hero-slide'>
                                                                      <div class='th-hero-bg' data-bg-src='{1}' data-overlay='overlay2' data-opacity='9'></div>
                                                                      <div class='container'>
                                                                          <div class='hero-style8'>
                                                                              <h1 class='hero-title' data-ani='slideinleft' data-ani-delay='0.3s'>{0}</h1>
                                                                              <div class='btn-group' data-ani='slideinleft' data-ani-delay='0.7s'>
                                                                                  <a href='{2}' class='th-btn'>
                                                                                     Detay<i class='fas fa-long-arrow-right ms-2'></i>
                                                                                  </a>
       
                                                                              </div>
                                                                          </div>
                                                                      </div>
                                                                      <div class='hero-img'>
                                                                          <img src='{1}' alt='Hero Image'>
                                                                          <button data-slick-next='.hero-slider-8' class='icon-btn'>
                                                                              <i class='far fa-long-arrow-right'></i>
                                                                          </button>
                                                                      </div>
                                                                  </div>", sub.Title, _domain + item.ImageUrl,sub.PostUrl);
					}
                    
                }
            }
            return snc;
        }
        public string GetServicesList()
        {
            string result=string.Empty;
            try
            {
                using (var db=new ConstructionContext())
                {
                    var data = db.Categories.Where(p => p.CategoryType == 7).Select(p => new
                    {
                        p.CategoryId,
                        p.Queno,
                        p.ImageUrl
                    }).OrderBy(p=>p.Queno).Take(5).ToList();
                    string snc=string.Empty;    
                    foreach (var item in data)
                    {
                        var subData = db.UrlRecords.Where(p => p.CategoryId == item.CategoryId && p.LanguageId == _languageId).FirstOrDefault();
                        if (subData!=null)
                        {
                            string viewTxt = Conversion("view");
                            string content = (subData.Content.Length > 110 ? subData.Content.Substring(0, 110) : subData.Content);
                            snc += string.Format(@"        <div class='col-md-6 col-lg-4 col-xl-3'>
                                                                                    <div class='service-card2'>
                                                                                        <div class='service-card2_img'>
                                                                                            <img src='{3}' alt='{0}'>
                                                                                        </div>
                                                                                        <div class='service-card2_content' data-bg-src='{5}/Content/assets/img/bg/bg_pattern_5.png'>
                                                                                            <h4 class='service-card2_title'>
                                                                                                <a href='{1}'>{0}</a>
                                                                                            </h4>
                                                                                            <p class='service-card2_text'>{2}'</p>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>", subData.Title, subData.SeoUrl, content, _domain + item.ImageUrl, viewTxt,_domain);
                        }
                    }
                    result = snc;
                }
            }
            catch (Exception)
            {

            }
            return result;
        }
        public string GetBlogList()
        {
            string snc = string.Empty;
            using (var db=new ConstructionContext())
            {
                var result = db.UrlRecords.Where(p => p.Category.IsBlog && p.Category.MainCategoryId != 0 && p.LanguageId == _languageId).Take(3).OrderBy(p=>p.Category.Queno).ToList();
                foreach (var item in result)
                {
                    string cont= (item.Content .Length> 145) ? item.Content.Substring(0, 145) :item.Content;
                    //snc += "<article class='entry'><figure class='entry-media'>" +
                    //            "<a href='"+_domain+item.SeoUrl+ "'><img src='" + _domain + item.Category.ImageUrl + "' alt='" +  item.Title + "'></a></figure>" +
                    //            "<div class='entry-body text-center'>" +
                    //            "<h3 class='entry-title'><a href='" + _domain + item.SeoUrl + "'>" +  item.Title + "</a></h3>" +
                    //            "<div class='entry-content'><p>" + cont + "</p></div></div></article>";

                    snc += string.Format(@"<div class='col-lg-4'>
	                                        <div class='content-blog blog-grid'>
		                                        <div class='inner'>
			                                        <div class='thumbnail'>
				                                        <a href='{0}'><img src='{1}' alt='{2}'></a>
			                                        </div>
			                                        <div class='content'>
				                                        <h5 class='title'><a href='{0}'>{2}</a></h5>
			                                        </div>
		                                        </div>
	                                        </div>
                                        </div>", _domain + item.SeoUrl, _domain + item.Category.ImageUrl,item.Title);
                }
            }
            return snc;
        }
        public string GetBreadcrumb()
        {
            string result = string.Empty;
            using (var db=new ConstructionContext())
            {
                var main = db.UrlRecords.Where(p => p.SeoUrl == _seoUrl && p.LanguageId==_languageId).FirstOrDefault();
                var lang = db.Languages.Where(p => p.LanguageId == _languageId).FirstOrDefault();
                if (main!=null)
                {
                    result = "<nav aria-label='breadcrumb' class='breadcrumb-nav mb-3'>" +
                                "<div class='container'><ol class='breadcrumb'>" +
                                "<li class='breadcrumb-item'><a href='" +_domain+ lang.UrlText + "'>" + lang .UrlString+ "</a></li>";
                    int CatId= (main.Category.MainCategoryId != 0) ? main.Category.MainCategoryId:0;
                    if (CatId!=0)
                    {
                        var category = db.UrlRecords.Where(p => p.CategoryId == CatId && p.LanguageId==_languageId).FirstOrDefault();
                        result += "<li class='breadcrumb-item'><a href='#'>" + category.Title + "</a></li>";
                    }
                    result += "<li class='breadcrumb-item active'><a href='" + _domain + main.SeoUrl + "'>" + main.Title + "</a></li></ol></div></nav>";
                }
                else if(lang.ContactUrl== _seoUrl)
                {
                    result = "<nav aria-label='breadcrumb' class='breadcrumb-nav mb-3'>" +
                            "<div class='container'><ol class='breadcrumb'>" +
                            "<li class='breadcrumb-item'><a href='" + _domain + lang.UrlText + "'>" + lang.UrlString + "</a></li>"+
                             "<li class='breadcrumb-item'><a href='" + _domain + lang.ContactUrl + "'>" + lang.ContactString + "</a></li>"+
                             "</ol></div></nav>";
                }
            }
            return result;
        }
        public ResponseUrlRecord GetPageDetails()
        {
            using (var db=new ConstructionContext())
            {
                var data = new ResponseUrlRecord();
                var main = db.UrlRecords.Where(p => p.LanguageId == _languageId && p.SeoUrl == _seoUrl).FirstOrDefault();
                data.ImageUrl = _domain+main.Category.ImageUrl;
                data.Content = main.Content;
                data.Keywords = main.Keywords;
                data.Title = main.Title;
                data.Description = main.Description;
                data.Id = main.CategoryId;
                data.Location = main.Category.Location;
                data.Meters = main.Category.Meters;
                data.ProjectDate = main.Category.ProjectDate;
                return data;
            }
        }
        public List<ResponseUrlRecord> GetPageList()
        {
            using (var db = new ConstructionContext())
            {
                var data = new List<ResponseUrlRecord>();
                var mainId = db.UrlRecords.Where(p => p.LanguageId == _languageId && p.SeoUrl == _seoUrl).FirstOrDefault().CategoryId;
                var cat = db.Categories.Where(p => p.MainCategoryId == mainId).ToList();
                foreach (var item in cat)
                {
                    var subData =new  ResponseUrlRecord();
                    var sub = db.UrlRecords.Where(p => p.LanguageId == _languageId && p.CategoryId == item.CategoryId).FirstOrDefault();
                    if (sub!=null)
                    {
                        subData.ImageUrl = _domain + item.ImageUrl;
                        subData.Title = sub.Title;
                        subData.Content = (sub.Content.Length>115)?sub.Content.Substring(0,115):sub.Content;
                        subData.SeoUrl = _domain + sub.SeoUrl;
                        data.Add(subData);
                    }
                }
                return data;
            }
        }
        public ResponseUrlRecord GetSpecialPage()
        {
            var data = new ResponseUrlRecord();
            using (var db=new ConstructionContext())
            {
                var result = db.Languages.Where(p => p.LanguageId == _languageId).FirstOrDefault();
                if (result!=null)
                {
                    data.Title = (result.ContactUrl==_seoUrl)?result.ContactString:result.UrlString;
                    data.Keywords = result.MetaKeywords;
                    data.Description = result.Description;
                }
            }
            return data;
        }

        public List<Verification> GetVerifications()
        {
            using (var db=new ConstructionContext())
            {
                return db.Verifications.ToList();
            }
        }

        public List<ContactInfo> GetContactInfos()
        {
            using (var db=new ConstructionContext())
            {
                return db.ContactInfos.OrderBy(p=>p.ContactInfoId).ToList();
            }
        }
        public string GetCategoryProductList()
        {
            string result = string.Empty;
            string urun = Conversion("Product");
            try
            {
                using (var db=new ConstructionContext())
                {
                    var datas= db.UrlRecords.Where(p => p.Category.CategoryType == 2 && p.LanguageId==_languageId ).Select(p=>new { 
                    p.Category.Queno,
                    p.Category.ImageUrl,
                    p.SeoUrl,
                    p.Title,
                    Count=p.Category.Galleries.Count()
                    }).OrderBy(p=>p.Queno).ToList();
                    if (datas!=null)
                    {
                        foreach (var item in datas)
                        {
                            result += "<div class='col-6 col-sm-4'>" +
                                            "   <div class='cat bg-white pt-1 mb-2'>" +
                                            "        <div class='cat-image d-flex justify-content-center align-items-center'>" +
                                            "            <a href='" + _domain + item.SeoUrl + "'><img src='" + _domain + item.ImageUrl + "' width='100%' height='auto'></a>" +
                                            "        </div>" +
                                            "        <div class='cat-content text-center'>" +
                                            "            <a href='" + _domain + item.SeoUrl + "' class='cat-title'>" + item.Title + "</a>" +
                                            "            <h4 class='cat-count letter-spacing-normal d-block font-weight-light'>" +  item.Count +" "+ urun + " </h4>" +
                                            "        </div>" +
                                            "    </div>" +
                                            "</div>";
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        public string GetHomeProductList()
        {
            string snc = string.Empty;
            using (var db = new ConstructionContext())
            {
                var result = db.UrlRecords.Where(p => !p.Category.IsBlog && p.LanguageId == _languageId && p.Category.CategoryType == 4 && p.Category.MainCategoryId!=0).OrderBy(p => p.Category.Queno).Take(8).ToList();
                foreach (var item in result)
                {
                    var img = db.Galleries.Where(p => p.CategoryId == item.CategoryId).FirstOrDefault();
                    string imgUrl = string.Empty;
                    if (img != null)
                    {
                        imgUrl= _domain + img.Url;
                    }
                    else
                    {
                        imgUrl = _domain + "/content/upload/default.jpg";
                    }
                    string mainCatName = db.Categories.Where(p => p.CategoryId == item.Category.MainCategoryId).Select(p => p.UrlRecords.FirstOrDefault().Title).FirstOrDefault();
                    snc += string.Format(@"<div class='col-xl-3 col-lg-4 col-sm-6 col-12 mb--30'>
	                                            <div class='axil-product product-style-one'>
		                                            <div class='thumbnail'>
			                                            <a href='{0}'>
				                                            <img data-sal='fade' data-sal-delay='100' data-sal-duration='1500' src='{3}' alt='{1}'>
			                                            </a>
			                                            <div class='label-block label-right'>
				                                            <div class='product-badget'>{2}</div>
			                                            </div>
		                                            </div>
		                                            <div class='product-content'>
			                                            <div class='inner'>
				                                            <h5 class='title'><a href='{0}'>{1}</a></h5>
			                                            </div>
		                                            </div>
	                                            </div>
                                            </div>", item.SeoUrl,item.Title, mainCatName,imgUrl);
                }

            }
            return snc;
        }
        public string GetProductList(int id)
        {
            string snc = string.Empty;
            using (var db = new ConstructionContext())
            {
                var mainId = db.UrlRecords.Where(p => p.SeoUrl == _seoUrl).FirstOrDefault().CategoryId;
                var result = db.UrlRecords.Where(p => !p.Category.IsBlog && p.LanguageId == _languageId && p.Category.CategoryType == 4 &&p.Category.MainCategoryId== mainId).OrderBy(p => p.Category.Queno).ToList();
                foreach (var item in result)
                {
                    var img = db.Galleries.Where(p => p.CategoryId == item.CategoryId).FirstOrDefault();
                    string imgUrl = string.Empty;
                    if (img != null)
                    {
                        imgUrl = _domain + img.Url;
                    }
                    else
                    {
                        imgUrl = _domain + "/content/upload/default.jpg";
                    }
                    string mainCatName = db.Categories.Where(p => p.CategoryId == item.Category.MainCategoryId).Select(p => p.UrlRecords.FirstOrDefault().Title).FirstOrDefault();
                    snc += string.Format(@"<div class='col-xl-3 col-lg-4 col-sm-6 col-12 mb--30'>
	                                            <div class='axil-product product-style-one'>
		                                            <div class='thumbnail'>
			                                            <a href='{0}'>
				                                            <img data-sal='fade' data-sal-delay='100' data-sal-duration='1500' src='{3}' alt='{1}'>
			                                            </a>
			                                            <div class='label-block label-right'>
				                                            <div class='product-badget'>{2}</div>
			                                            </div>
		                                            </div>
		                                            <div class='product-content'>
			                                            <div class='inner'>
				                                            <h5 class='title'><a href='{0}'>{1}</a></h5>
			                                            </div>
		                                            </div>
	                                            </div>
                                            </div>", item.SeoUrl, item.Title, mainCatName, imgUrl);
                }

            }
            return snc;
        }

        public List<Gallery> GetProductGallery(int id)
        {
            using (var db = new ConstructionContext())
            {
                var main = db.UrlRecords.Where(p => p.LanguageId == _languageId && p.UrlRecordId==id).FirstOrDefault().CategoryId;
                return db.Galleries.Where(p=>p.CategoryId==main).OrderBy(p => p.GalleryId).ToList();
            }
        }
        public string GetHomeProject()
        {
            string result = string.Empty;
            try
            {
                using (var db=new ConstructionContext())
                {
                    var projects = db.UrlRecords.Where(p => p.Category.CategoryType == 5 && p.LanguageId == _languageId).Select(p => new
                    {
                        p.Title,
                        p.Category.Galleries.FirstOrDefault().Url,
                        p.SeoUrl,
                        p.CategoryId,
                        p.Category.Queno
                    }).OrderBy(p => p.Queno).Take(4).ToList();
                    string snc = string.Empty;
                    foreach (var item in projects)
                    {
                        snc += string.Format(@" <div class='col-md-6 col-lg-4'>
                                                                     <div class='project-card2'>
                                                                         <div class='project-img'>
                                                                             <img src='{1}' alt='{0}'>
                                                                         </div>
                                                                         <div class='project-content'>
                                                                             <h3 class='project-title'>
                                                                                 <a href='{2}'>{0}</a>
                                                                             </h3>
                                                                             <a href='{2}' class='project-icon'>
                                                                                 <i class='far fa-plus'></i>
                                                                             </a>
                                                                         </div>
                                                                     </div>
                                                                 </div>", item.Title,_domain+item.Url, item.SeoUrl);
                    }
                    result = snc;
					if (projects.Count == 0)
					{
						result = string.Empty;
					}
				}
            }
            catch (Exception)
            {

				result=null;
            }
            return result;
        }

        public string GetHomeBanners()
        {
			string result = string.Empty;
			try
			{
				using (var db = new ConstructionContext())
				{
					var projects = db.Banners.OrderBy(p => p.Queno).ToList();
					string snc = string.Empty;
					foreach (var item in projects)
					{
						snc += string.Format(@"<div class='col-auto'>
                                                                    <div class='brand-box'>
                                                                        <img src='{1}' alt='{0}'>
                                                                    </div>
                                                                </div>", item.Title, _domain + item.ImageUrl, item.BannerUrl);
					}
                    result = snc;

					if (projects.Count == 0)
					{
						result = string.Empty;
					}
				}
			}
			catch (Exception)
			{

				result = null;
			}
			return result;
		}

		public string GetHomeBlogs()
		{
			string result = string.Empty;
			try
			{
				using (var db = new ConstructionContext())
				{
					string con = Conversion("home-blogs");
					var projects = db.UrlRecords.Where(p => p.Category.CategoryType ==6 && p.LanguageId == _languageId).Select(p => new
					{
						p.Title,
						p.SeoUrl,
						p.CategoryId,
                        p.Content,
                        p.Category.ImageUrl
					}).Take(3).OrderByDescending(p => p.CategoryId).ToList();
					string snc = string.Empty;
					foreach (var item in projects)
					{
                        string content = (item.Content.Length > 110 ? item.Content.Substring(0, 110) : item.Content);
						snc += string.Format(@" <div class='col-md-6 col-xl-4'>
                                                                     <div class='blog-card'>
                                                                         <div class='blog-img'>
                                                                             <img src='{1}' alt='{0}'>
                                                                         </div>
                                                                         <div class='blog-content'>
                                                                             <h3 class='box-title'>
                                                                                 <a href='{2}'>{0}</a>
                                                                             </h3>
                                                                             <p class='blog-text'>{3}</p>
                                                                         </div>
                                                                     </div>
                                                                 </div>", item.Title, _domain + item.ImageUrl, item.SeoUrl, content);
					}
                    result = snc;
                    if (projects.Count==0)
                    {
                        result = string.Empty;
                    }
				}
			}
			catch (Exception)
			{

				result = null;
			}
			return result;
		}

        public string GetServiceList()
        {
            string result= string.Empty;
			string snc = string.Empty;
			try
            {
                using (var db=new ConstructionContext())
                {
					var projects = db.UrlRecords.Where(p => p.Category.CategoryType == 7 && p.LanguageId == _languageId).Select(p => new
					{
						p.Title,
						p.SeoUrl,
						p.CategoryId,
                        p.Category.Queno
					}).OrderBy(p => p.Queno).ToList();
					
                    foreach (var item in projects)
                    {
                        snc += string.Format(@"<li><a href='{1}'>{0}</a></li>", item.Title,item.SeoUrl);
                    }
				}
                result = snc;
            }
            catch (Exception)
            {
                result=string.Empty;
            }
            return result;
        }

        public string GetProjectList()
        {
            string result= string.Empty;
            try
            {
                using (var db=new ConstructionContext())
                {
					var projects = db.UrlRecords.Where(p => p.Category.CategoryType == 5 && p.LanguageId == _languageId).Select(p => new
					{
						p.Title,
						p.SeoUrl,
						p.CategoryId,
						p.Category.Queno,
                        p.Category.Galleries.FirstOrDefault().Url,
                        p.Category.Meters,
                        p.Category.Location,
                        p.Category.ProjectDate
					}).OrderBy(p => p.Queno).ToList();

                    foreach (var item in projects)
                    {

                        result += string.Format(@"  <div class='blogpost_preview_fw element'>
                                                                          <div class='fw-portPreview'>
                                                                              <div class='img_block wrapped_img fs_port_item'>
                                                                                  <a class='featured_ico_link' href='{1}'>
                                                                                      <img alt='{0}' src='{2}' />
                                                                                  </a>
                                                                                  <div class='bottom_box'>
                                                                                      <div class='bc_content'>
                                                                                          <h5 class='bc_title'><a href='{1}'>{0}</a></h5>
                                                                                          <div class='featured_items_meta'>
                                                                                              <span>{5}</span>
                                                                                              <span>{4}</span>
                                                                                              <span>{3}</span>
                                                                                          </div>
                                                                                      </div>
                                                                                  </div>
                                                                              </div>
                                                                          </div>
                                                                      </div>", item.Title, item.SeoUrl, item.Url,item.Meters,item.ProjectDate,item.Location);
                    }
				}
            }
            catch (Exception)
            {
                result = string.Empty;
            }
            return result;
        }
        public List<Gallery> GetProjectImages(int ProjectId)
        {
            try
            {
                using (var db=new ConstructionContext())
                {
                    var data = db.Galleries.Where(p => p.CategoryId == ProjectId).ToList();
                    return data;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetFooterProject()
        {
            string result = string.Empty;   
            try
            {
                using (var db=new ConstructionContext())
                {
					var projects = db.UrlRecords.Where(p => p.Category.CategoryType == 5 && p.LanguageId == _languageId).Select(p => new
					{
						p.Title,
						p.Category.Galleries.FirstOrDefault().Url,
						p.SeoUrl,
						p.CategoryId
					}).Take(6).OrderByDescending(p => p.CategoryId).ToList();
                    foreach (var item in projects)
                    {
                        result += string.Format(@"<li><a href='{1}' class='text-color-hover-primary'>{0}</a></li>", item.Title, item.SeoUrl);
                    }
				}
            }
            catch (Exception)
            {
                result=string.Empty;
            }
            return result;
        }
		public string GetFooterService()
		{
			string result = string.Empty;
			try
			{
				using (var db = new ConstructionContext())
				{
					var projects = db.UrlRecords.Where(p => p.Category.CategoryType == 7 && p.LanguageId == _languageId).Select(p => new
					{
						p.Title,
						p.Category.Galleries.FirstOrDefault().Url,
						p.SeoUrl,
						p.CategoryId
					}).Take(6).OrderByDescending(p => p.CategoryId).ToList();
					foreach (var item in projects)
					{
						result += string.Format(@"<li><a href='{1}' class='text-color-hover-primary'>{0}</a></li>", item.Title, item.SeoUrl);
					}
				}
			}
			catch (Exception)
			{
				result = string.Empty;
			}
			return result;
		}

        public static implicit operator MainCode(DashboardMainCode v)
        {
            throw new NotImplementedException();
        }
    }
}