using Construction.Dal.Context;
using System;
using System.Linq;

namespace Construction.Infrastructure.Service
{
	public class MainService
	{
		private string _domain = string.Empty;
		private int _languageId = 0;
		private string _seoUrl = string.Empty;
		private string _homepage = string.Empty;
		public MainService(string domain, string SeoUrl)
		{
			_seoUrl = SeoUrl;
			_domain = domain;
			_languageId = GetLangId(SeoUrl);
		}	
		public void GetMainMenu()
		{
			string snc = string.Empty;
			using (var db = new ConstructionContext())
			{
				var result = db.UrlRecords.Where(p => !p.Category.IsBlog && p.LanguageId == _languageId && p.Category.CategoryType != 3).OrderBy(p => p.Category.Queno).ToList();
				var defaultData = db.Languages.Where(p => p.LanguageId == _languageId).FirstOrDefault();
				snc = (defaultData.UrlText != null || defaultData.UrlText != string.Empty) ? "<li><a href='" + _domain + defaultData.UrlText + "'>" + defaultData.UrlString + "</a></li>" : string.Empty;
				var main = result.Where(p => p.Category.MainCategoryId == 0).ToList();
				foreach (var item in main)
				{
					var sub = result.Where(p => p.Category.MainCategoryId == item.CategoryId && !p.Category.IsBlog).ToList();
					if (sub.Count > 0)
					{
						snc += "<li class='menu-item-has-children'><a href='#' >" + item.Title + "</a><ul class='axil-submenu'>";
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
			catch (Exception ex)
			{
			
			}	
			return Id;
		}
	}
}
