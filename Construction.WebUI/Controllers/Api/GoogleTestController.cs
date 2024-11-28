using Construction.WebUI.AppCode;
using Construction.WebUI.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web.Http;

namespace Construction.WebUI.Controllers.Api
{
    public class GoogleTestController : ApiController
    {
        DashboardMainCode xCode;

        [Route("api/GetVerify")]
        [HttpPost]
        public IHttpActionResult GetVerify(MailContent content)
        {
            xCode = new DashboardMainCode(Request.Headers.Referrer.AbsolutePath);
            string hostUrl = "https://www.google.com/recaptcha/api/siteverify?secret="+ ConfigurationManager.AppSettings["secretKey"] + "&response=" + content.Response;
            var request = (HttpWebRequest)WebRequest.Create(hostUrl);
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>("success");
                    if (isSuccess)
                    {
                        string mailBody= "<p><strong>Ad Soyad</strong> : <em>" + content.FullName + "</em></p>" +
                                                    "<p><strong>Telefon</strong> : <em>" + content.Phone + "</em></p>" +
                                                    "<p><strong>Email</strong> : <em>" + content.Email + "</em></p>" +
                                                    "<p><strong>Konu</strong> : <em>" + content.Subject + "</em></p>" +
                                                    "<p><em><strong>Tarih </strong>:" + DateTime.Now.ToShortDateString() + "</em></p>" +
                                                    "<p><strong>Mesaj</strong> : <em>" + content.Message + "</em></p>";
                        isSuccess = xCode.MailPost(content.Subject, mailBody);

                    }
                    return Ok(new
                    {
                        state = (isSuccess) ? true : false
                    });

                }
            }

            
        }

    }
}
