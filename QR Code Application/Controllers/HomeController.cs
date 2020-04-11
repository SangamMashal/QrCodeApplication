using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace QR_Code_Application.Controllers
{
    public class HomeController : Controller
    {
        const string requestUri = "http://localhost/QRCode/api/QrCodeApi";
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    byte[] Bytes = new byte[file.InputStream.Length + 1];
                    file.InputStream.Read(Bytes, 0, Bytes.Length);
                    var fileContent = new ByteArrayContent(Bytes);
                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = file.FileName };
                    content.Add(fileContent);
                   
                    var result = client.PostAsync(requestUri, content).Result;
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.Message = result.Content.ReadAsAsync<string>().Result;

                    }
                    else
                    {
                        ViewBag.Message = "Error!" + result.Content.ToString();
                    }
                }
            }
            return View();
        }
    }
}
