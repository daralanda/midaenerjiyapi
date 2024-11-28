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
    public class VerificationApiController : ApiController
    {
        public string seoUrl = string.Empty;
        readonly DashboardMainCode mainCode = new DashboardMainCode("");
        [Route("api/GetAllVerification")]
        [HttpGet]
        public IHttpActionResult GetAllVerification()
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Select"))
                {
                    using (var db = new ConstructionContext())
                    {

                        var result = db.Verifications.OrderByDescending(p=>p.VerificationId).ToList();

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
        [Route("api/DeleteVerification")]
        [HttpDelete]
        public IHttpActionResult DeleteVerification(Verification data)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Delete"))
                {
                    using (var db = new ConstructionContext())
                    {

                        var delete = db.Verifications.Where(p => p.VerificationId == data.VerificationId).FirstOrDefault();
                        db.Verifications.Remove(delete);
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
        [Route("api/AddVerification")]
        [HttpPost]
        public IHttpActionResult AddVerification(Verification data)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Insert"))
                {
                    using (var db = new ConstructionContext())
                    {
                        db.Verifications.Add(data);
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
        [Route("api/UpdateVerification")]
        [HttpPut]
        public IHttpActionResult UpdateVerification(Verification data)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Update"))
                {
                    using (var db = new ConstructionContext())
                    {
                        var result = db.Verifications.Where(p => p.VerificationId == data.VerificationId).FirstOrDefault();
                        if (result!=null)
                        {
                            result.VerificationKey = data.VerificationKey;
                            result.VerificationValue = data.VerificationValue;
             
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
        [Route("api/FindVerification")]
        [HttpPost]
        public IHttpActionResult FindVerification(Verification data)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Select"))
                {
                    using (var db = new ConstructionContext())
                    {
                        var result = db.Verifications.Where(p => p.VerificationId == data.VerificationId).FirstOrDefault();
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
