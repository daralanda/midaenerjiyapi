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
    public class ContactSettingApiController : ApiController
    {
        readonly DashboardMainCode mainCode = new DashboardMainCode();
        string seoUrl = string.Empty;

        [Route("api/GetAllContactSetting")]
        [HttpGet]
        public IHttpActionResult getAllCustomer()
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Select"))
                {
                    using (var db = new ConstructionContext())
                    {
                        var result = db.ContactSettings.Where(p => p.ContactSettingId == 1).FirstOrDefault();
                        return Ok(new { state = true, data = result });
                    }
                }
                else
                {
                    return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
                }
            }
            catch
            {

                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }
        [Route("api/AddorUpdateContactSetting")]
        [HttpPost]
        public IHttpActionResult AddCustomer(ContactSetting contactSetting)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Insert"))
                {
                    using (var db = new ConstructionContext())
                    {
                        var result = db.ContactSettings.Where(p => p.ContactSettingId == 1).FirstOrDefault();
                        result.Host = contactSetting.Host;
                        result.Email = contactSetting.Email;
                        result.Port = contactSetting.Port;
                        result.EnableSsl = contactSetting.EnableSsl;
                        result.Password = contactSetting.Password;
                        db.SaveChanges();
                        return Ok(new { state = true });
                    }
                }
                else
                {
                    return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
                }
            }
            catch
            {
                return Ok(new { state = false, message = "Yetkiniz Bulunmamaktadır" });
            }
        }

    }
}
