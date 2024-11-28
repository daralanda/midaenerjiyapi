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
    public class ContactInfoApiController : ApiController
    {
        public string seoUrl = string.Empty;
        readonly DashboardMainCode mainCode = new DashboardMainCode("");
        [Route("api/GetAllContactInfo")]
        [HttpGet]
        public IHttpActionResult GetAllContactInfo()
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Select"))
                {
                    using (var db = new ConstructionContext())
                    {

                        var result = db.ContactInfos.OrderByDescending(p=>p.ContactInfoId).ToList();

                        return Ok(new
                        {
                            state = true,
                            data = result
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
        [Route("api/DeleteContactInfo")]
        [HttpDelete]
        public IHttpActionResult DeleteContactInfo(ContactInfo data)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Delete"))
                {
                    using (var db = new ConstructionContext())
                    {

                        var delete = db.ContactInfos.Where(p => p.ContactInfoId == data.ContactInfoId).FirstOrDefault();
                        db.ContactInfos.Remove(delete);
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
        [Route("api/AddContactInfo")]
        [HttpPost]
        public IHttpActionResult AddContactInfo(ContactInfo data)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Insert"))
                {
                    using (var db = new ConstructionContext())
                    {
                        db.ContactInfos.Add(data);
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
        [Route("api/UpdateContactInfo")]
        [HttpPut]
        public IHttpActionResult UpdateContactInfo(ContactInfo data)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Update"))
                {
                    using (var db = new ConstructionContext())
                    {
                        var result = db.ContactInfos.Where(p => p.ContactInfoId == data.ContactInfoId).FirstOrDefault();
                        if (result!=null)
                        {
                            result.Address = data.Address;
                            result.IsActive = data.IsActive;
                            result.Email = data.Email;
                            result.Fax = data.Fax;
                            result.IsDefault = data.IsDefault;
                            result.MapFrame = data.MapFrame;
                            result.Name = data.Name;
                            result.Phone = data.Phone;
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
        [Route("api/FindContactInfo")]
        [HttpPost]
        public IHttpActionResult FindContactInfo(ContactInfo data)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Select"))
                {
                    using (var db = new ConstructionContext())
                    {
                        var result = db.ContactInfos.Where(p => p.ContactInfoId == data.ContactInfoId).FirstOrDefault();
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
