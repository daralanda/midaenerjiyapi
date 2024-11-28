using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using Construction.WebUI.AppCode;

namespace Construction.WebUI
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);            
        }
		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			var mainCode = new MainCode();
			if (!Request.RawUrl.Contains("/api/"))
			{
				string SeoUrl = Request.RawUrl;
				int baslangic = Request.RawUrl.ToString().IndexOf('?');
				if (baslangic > 0)
				{
					SeoUrl = Request.RawUrl.ToString().Substring(0, baslangic);
				}
				var result = mainCode.FindUrl(SeoUrl);
				if (result.SecurityObjectId == 0 && result.ControllerName != null && result.ControllerName != "")
				{
					Context.RewritePath("~/" + result.ControllerName + "/" + result.ActionName);
				}
				else if (SeoUrl == "/giris-yap.html")
				{
					Context.RewritePath("~/Dashboard/Login");
				}
				else if (SeoUrl == "/sitemap.xml")
				{
					Context.RewritePath("~/Home/SiteMap");
				}
				else if (SeoUrl == "/robots.txt")
				{
					Context.RewritePath("~/Home/RobotsTxt");
				}
				else if (SeoUrl.Contains("/hatali-sayfa"))
				{
					Context.RewritePath("~/Home/ErrorPage");
				}
				else if (SeoUrl == "/Dashboard/profile")
				{
					Context.RewritePath("~/Dashboard/profile");
				}
				else if (result != null && result.SecurityObjectId != 0)
				{
					Context.RewritePath("~/" + result.ControllerName + "/" + result.ActionName + "/" + result.SecurityObjectId);
				}
			}

		}

		protected void Application_PostAuthorizeRequest()
		{
			HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
		}
	}
}