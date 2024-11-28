using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Construction.Dal.Context;
using Newtonsoft.Json;
using Construction.Dal.Entity;
using Construction.WebUI.AppCode;
using Construction.WebUI.Models;

namespace Construction.WebUI.Controllers.Api
{
    public class UserController : ApiController
    {
       readonly DashboardMainCode mainCode = new DashboardMainCode("");
        string seoUrl = string.Empty;

        [Route("api/getAllUsers")]
        [HttpGet]
        public IHttpActionResult GetAllUsers()
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Select"))
                {
                    using (var db = new ConstructionContext())
                    {
                        var result = db.Users.Select(p => new {
                            p.Email,
                            p.FirstName,
                            p.LastName,
                            p.Password,
                            p.RoleId,
                            p.UserId,
                            p.IsActive,
                            p.Role.RoleName,
                        }).ToList();
                        var roleList = db.Roles.Select(p => new {
                            p.IsActive,
                            p.RoleId,
                            p.RoleName
                        }).Where(p => p.IsActive == true).ToList();
                        return Ok(new
                        {
                            state = true,
                            data = result,
                            roleData = roleList,
                        });
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
        [Route("api/UsersAdd")]
        [HttpPost]
        public IHttpActionResult UsersAdd(User user)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Insert"))
                {
                    using (var db = new ConstructionContext())
                    {
                        user.CreateDate = DateTime.Now;
                        db.Users.Add(user);
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
        [Route("api/UserUpdate")]
        [HttpPut]
        public IHttpActionResult UserUpdate(User user)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Update"))
                {
                    using (var db = new ConstructionContext())
                    {
                        var result = db.Users.Where(p => p.UserId == user.UserId).FirstOrDefault();
                        result.IsActive = user.IsActive;
                        result.LastName = user.LastName;
                        result.Password = user.Password;
                        result.RoleId = user.RoleId;
                        result.FirstName = user.FirstName;
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
