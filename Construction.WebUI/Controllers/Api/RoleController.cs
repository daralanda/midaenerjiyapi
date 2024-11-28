using Construction.Dal.Context;
using Construction.Dal.Entity;
using Construction.WebUI.AppCode;
using Construction.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Construction.WebUI.Controllers.Api
{
    public class RoleController : ApiController
    {
        public string seoUrl = "/dashboard/kullanici-yonetimi/yetki-gruplari";
        readonly DashboardMainCode mainCode = new DashboardMainCode("");
        [Route("api/GetAllRoles")]
        [HttpGet]
        public IHttpActionResult GetAllRoles()
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Select"))
                {
                    using (var db = new ConstructionContext())
                    {
                        var result = db.Roles.ToList();
                        var plugins = db.SecurityObjects.Where(p => p.IsActive == true && !p.IsFront).Select(p => new
                        {
                            p.SecurityObjectId,
                            p.SecurityObjectName,
                            p.MainSecurityObjectId,
                            Select = false,
                            IsDelete = false,
                            Update = false,
                            Insert = false,
                            RoleId = "",
                            IsActive = false,
                            RoleName = "",
                            p.Queno,
                        }).ToList();
                        var asd = plugins.Where(p => p.MainSecurityObjectId == 0).OrderBy(p => p.Queno).ToList();
                        var roleCategories = asd.ToList();
                        return Ok(new
                        {
                            state = true,
                            data = result,
                            main = roleCategories,
                            plugins
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
                return Ok(new { state = true, message = "Yetkiniz Bulunmamaktadır" });
            }
        }
        [Route("api/RoleAdd")]
        [HttpPost]
        public IHttpActionResult RoleAdd(Role role)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission("/dashboard/kullanici-yonetimi/yetki-gruplari", "Insert"))
                {
                    using (var db = new ConstructionContext())
                    {
                        db.Roles.Add(role);
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
                return Ok(new { state = true, message = "Yetkiniz Bulunmamaktadır" });
            }
        }     
        [Route("api/RoleUpdate")]
        [HttpPut]
        public IHttpActionResult RoleUpdate(Role role)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Update"))
                {
                    using (var db = new ConstructionContext())
                    {
                        int roleID = role.RoleId;
                        var result = db.Roles.Where(p => p.RoleId == roleID).FirstOrDefault();
                        //db.SaveChanges();
                        result.IsActive = role.IsActive;
                        db.SaveChanges();
                        var IsDelete = db.RolePermissions.Where(p => p.RoleId == roleID).ToList();
                        db.RolePermissions.RemoveRange(IsDelete);
                        foreach (var item in role.RolePermissions)
                        {
                            item.RoleId = role.RoleId;
                        }
                        db.RolePermissions.AddRange(role.RolePermissions);
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
                return Ok(new { state = true, message = "Yetkiniz Bulunmamaktadır" });
            }
        }
        [Route("api/FindRolePermission")]
        [HttpPost]
        public IHttpActionResult FindRolePermission(Role role)
        {
            try
            {
                seoUrl = Request.Headers.Referrer.AbsolutePath;
                if (mainCode.IsPermission(seoUrl, "Select"))
                {
                    using (var db = new ConstructionContext())
                    {
                        var result = db.RolePermissions.Where(p => p.RoleId == role.RoleId).Select(p => new {
                            p.SecurityObjectId,
                            p.IsDelete,
                            p.IsSelect,
                            p.IsUpdate,
                            p.IsInsert,
                            p.SecurityObject.SecurityObjectName,
                            p.SecurityObject.MainSecurityObjectId,
                            p.RoleId,
                            p.SecurityObject.Queno
                        }).ToList();
                        var dataRole = db.Roles.Where(p => p.RoleId == role.RoleId).FirstOrDefault();
                        db.SaveChanges();
                        return Ok(new { state = true, data = result, dataRoles = dataRole });
                    }
                }
                else
                {
                    return Ok(new { state = true, message = "Yetkiniz Bulunmamaktadır." });
                }
            }
            catch 
            {
                return Ok(new { state = true, message = "Yetkiniz Bulunmamaktadır." });
            }
        }
    }
}

