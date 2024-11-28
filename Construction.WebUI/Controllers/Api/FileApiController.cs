using Construction.WebUI.AppCode;
using Construction.WebUI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Construction.WebUI.Controllers.Api
{
    public class FileApiController : ApiController
    {
       
        Security security = new Security();
        [Route("upload/upload-file")]
        [HttpPost]
        public IHttpActionResult UploadFileImg()
        {
            var xSecurity = security.SecurityRead();

            if (xSecurity != null && xSecurity.UserSecurities.FirstOrDefault().Value != null)
            {
                bool state = false;
                var message = "";
                var httpRequest = HttpContext.Current.Request;
                var docfiles = new List<Files>();
                try
                {
                    if (httpRequest.Files.Count > 0)
                    {
                        var liste = httpRequest.Files.Keys;
                        for (int i = 0; i < liste.Count; i++)
                        {
                            Guid guid = Guid.NewGuid();
                            var postedFile = httpRequest.Files[i];
                            if (postedFile.ToString().Length > 3)
                            {
                                var FileNameOnly = Path.GetFileNameWithoutExtension(postedFile.FileName);
                                var fileExt = Path.GetExtension(postedFile.FileName);
                                var ModFileName = FileNameOnly + guid + fileExt;
                                var filePath = HttpContext.Current.Server.MapPath("~/Content/upload/" + ModFileName.Replace(' ', '-'));
                                var sanalpath = "~/Content/upload/" + ModFileName.Replace(' ', '-');
                                postedFile.SaveAs(filePath);
                                docfiles.Add(new Files
                                {
                                    FileName = httpRequest.Files[i].FileName,
                                    FileUrl = sanalpath,
                                    FileId = Guid.NewGuid()
                                });
                            }
                        }
                        state = true;
                    }
                    else
                    {
                        state = false;
                    }
                }
                catch (Exception ex)
                {
                    state = false;
                    message = ex.ToString();
                }
                return Ok(new { state = state, Img = docfiles.ToList(), Message = message });
            }
            else
            {
                return Ok(new { state = false, Message = "Yetkisiz giriş denemesi" });
            }
        }


        [Route("upload/delete-file")]
        [HttpPost]
        public IHttpActionResult DeleteFile(Files files)
        {
            try
            {
                var xSecurity = security.SecurityRead();
                if (xSecurity != null && xSecurity.UserSecurities.FirstOrDefault().Value != null)
                {

                    string value = files.FileUrl;
                    if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(value)))
                    {
                        System.IO.File.Delete(HttpContext.Current.Server.MapPath(value));
                    }
                    if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(value)))
                    {
                        System.IO.File.Delete(HttpContext.Current.Server.MapPath(value));
                    }
                    return Ok(new { state = true });
                }
                else
                {
                    return Ok(new { state = false });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { state = false, message=ex.ToString() });
            }
          
        }
    }
}
