using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using Construction.Dal.Context;
using Construction.WebUI.Models;

namespace Construction.WebUI.AppCode
{
    public class DashboardMainCode
    {
        public string Domain = string.Empty;
        public DashboardMainCode(string _domain=null)
        {
            Domain = _domain;
        }
        public string ProjectName = ConfigurationManager.AppSettings["ProjectName"];
        Security security = new Security();
        public UserSecurity GetSessionUser()
        {
            var data = security.SecurityRead();
            if (data!=null && data.UserSecurities.FirstOrDefault().Value!=null)
            {
                return data.UserSecurities.FirstOrDefault().Value;
            }
            else
            {
                UserSecurity user = new UserSecurity();
                user.RoleId = 0;
                return user;
            }
        }
        public string GetDashboarMainMenu()
        {
            string strResult = string.Empty;
            int RoleId = GetSessionUser().RoleId;
            using (var db=new ConstructionContext())
            {
                var mainData = db.RolePermissions.Where(p => p.RoleId == RoleId && p.IsSelect && p.SecurityObject.IsActive && p.Role.IsActive && !p.SecurityObject.IsFront)
                    .Select(p => new
                    {
                        p.SecurityObject.Icon,
                        p.SecurityObjectId,
                        p.SecurityObject.Queno,
                        p.SecurityObject.SecurityObjectName,
                        p.SecurityObject.SeoUrl,
                        p.SecurityObject.MainSecurityObjectId
                    }).OrderBy(p=>p.Queno).ToList();
                var mainItems = mainData.Where(p => p.MainSecurityObjectId == 0).ToList() ;
                foreach (var item in mainItems)
                {
                    var subItems = mainData.Where(p => p.MainSecurityObjectId == item.SecurityObjectId).ToList();
                    if (subItems.Count>0)
                    {
                        strResult+= "<li><a href='javascript: void(0);' class='has-arrow waves-effect'><i class='"+item.Icon+ "'></i><span>" + item.SecurityObjectName + "</span></a>" +
                                            "<ul class='sub-menu' aria-expanded='false'>";
                        foreach (var subItem in subItems)
                        {
                            strResult += "<li><a href='" + Domain + subItem.SeoUrl + "'>" + subItem.SecurityObjectName + "</a></li>";
                        }
                        strResult += "</ul></li>";

                    }
                    else
                    {
                        strResult += "<li><a href='"+ Domain+item.SeoUrl + "' class='waves-effect'><i class='"+item.Icon+"'></i><span>"+item.SecurityObjectName+"</span></a></li>";
                    }
                }

            }
            return strResult;
        }
        public string GetBreadcrumb(string SeoUrl)
        {
            string strResult = string.Empty;
            using (var db=new ConstructionContext())
            {              
                var data = db.SecurityObjects.Where(p => p.SeoUrl == SeoUrl).FirstOrDefault();
                if (data!=null)
                {
                    strResult = "<h4 class='mb-0 font-size-18'>" + data.SecurityObjectName + "</h4>" +
                                 "<div class='page-title-right'>" +
                                 "<ol class='breadcrumb m-0'>" +
                                      "<li class='breadcrumb-item'><a href='" + Domain + "/Dashboard" + "'>Anasayfa</a></li>";
                    if (data.MainSecurityObjectId != 0)
                    {
                        var mainData = db.SecurityObjects.Where(p => p.SecurityObjectId == data.MainSecurityObjectId).FirstOrDefault();
                        strResult += "<li class='breadcrumb-item'><a href='#'>" + mainData.SecurityObjectName + "</a></li>";
                    }
                    strResult += "<li class='breadcrumb-item active'>" + data.SecurityObjectName + "</li>";
                }
                else
                {
                    strResult = "<h4 class='mb-0 font-size-18'>Anasayfa</h4>" +
                               "<div class='page-title-right'>" +
                               "<ol class='breadcrumb m-0'>" +
                                    "<li class='breadcrumb-item'><a href='" + Domain + "/Dashboard" + "'>Anasayfa</a></li>";
                }
                strResult += "</ol></div>";

            }
            return strResult;
        }
        public bool IsPermission(string SeoUrl, string RunType)
        {
            bool state = false;
            int RoleId = GetSessionUser().RoleId;
            using (var db=new ConstructionContext())
            {
                var data = db.RolePermissions.Where(p => p.RoleId == RoleId && p.Role.IsActive && p.SecurityObject.IsActive && p.SecurityObject.SeoUrl == SeoUrl).FirstOrDefault();
                if (data!=null)
                {
                    switch (RunType)
                    {
                        case "Insert":
                            state = data.IsInsert;
                         break;
                        case "Select":
                            state = data.IsSelect;
                            break;
                        case "Update":
                            state = data.IsUpdate;
                            break;
                        case "Delete":
                            state = data.IsDelete;
                            break;
                    }
                   
                }
            }
            return state;
        }
        public string GetViewTitle(string url)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                    var data = db.SecurityObjects.Where(p => p.IsActive && p.SeoUrl == url).FirstOrDefault();
                    if (data!=null)
                    {
                        return data.SecurityObjectName;
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            catch
            {
                return "";
            }
        }
        public void FileDelete(string files)
        {
                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(files)))
                {
                    System.IO.File.Delete(HttpContext.Current.Server.MapPath(files));
                }
                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(files)))
                {
                    System.IO.File.Delete(HttpContext.Current.Server.MapPath(files));
                }
        }

        public bool MailPost(string Subject, string Body)
        {
            try
            {
                using (var db = new ConstructionContext())
                {
                   var  contact = db.ContactSettings.FirstOrDefault();
                    string PostEmail = ConfigurationManager.AppSettings["PostMail"];
                    var fromAddress = new MailAddress(contact.Email, ProjectName);
                    var toAddress = new MailAddress(PostEmail, ProjectName);
                    string fromPassword = contact.Password;
                    var smtp = new SmtpClient
                    {
                        Host = contact.Host,
                        Port = contact.Port,
                        EnableSsl = contact.EnableSsl,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = Subject,
                        Body = Body,
                        IsBodyHtml = true
                    })
                    {
                        smtp.Send(message);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
    }
}