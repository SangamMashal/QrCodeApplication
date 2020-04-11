using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace QR_Code_App.Controllers
{
    public class BaseApiController: ApiController
    {
        protected HttpResponseMessage OkResponse(object data)
        {
            var response = Request.CreateResponse(data);
            response.Headers.Add("CACHE-CONTROL", "NO-CACHE");
            return response;
        }
        protected HttpResponseMessage ImageResponse(byte[] data)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(data)
            };
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            response.Content.Headers.ContentDisposition.FileName = "qrCode.png";
            return response;
        }
        protected HttpResponseMessage BadRequestErrorResponse(string message)
        {
            var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            response.Headers.Add("CACHE-CONTROL", "NO-CACHE");
            return response;
        }
    }
}