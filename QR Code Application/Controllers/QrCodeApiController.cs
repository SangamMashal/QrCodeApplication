using Newtonsoft.Json;
using QR_Code_App.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace QR_Code_App.Controllers
{
    public class QrCodeApiController : BaseApiController
    {
        const string QrCodeReadURL = "https://api.qrserver.com/v1/read-qr-code/";
        const string QrCodeCreateURL = "https://api.qrserver.com/v1/create-qr-code/?size={0}&data={1}";
        /// <summary>
        /// Generate the QR code.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="qrCodeText"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> Get(string size, string qrCodeText)
        {
            using (var client = new HttpClient())
            {
                var message = await client.GetAsync(string.Format(QrCodeCreateURL, size, qrCodeText));
                var result = message.Content.ReadAsByteArrayAsync().Result;
                return ImageResponse(result);
            }

        }
        /// <summary>
        /// Read the QR code.
        /// </summary>
        /// <param name="value"></param>
        // POST api/values
        [HttpPost]
        public async Task<HttpResponseMessage> Post()
        {
            List<QRCodeResponse> qRCodeResponses = null;
            var httpRequest = HttpContext.Current.Request;
            using (var client = new HttpClient())
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var requestContent = new MultipartFormDataContent();
                    var fileContent = new StreamContent(postedFile.InputStream);
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        Name = "\"file\"",
                        FileName = "\"" + postedFile.FileName + "\""
                    };
                    fileContent.Headers.ContentType =
                        MediaTypeHeaderValue.Parse(MimeMapping.GetMimeMapping(postedFile.FileName));

                    requestContent.Add(fileContent);
                    var result = await client.PostAsync(QrCodeReadURL, requestContent);
                    string resultContent = await result.Content.ReadAsStringAsync();
                    qRCodeResponses = JsonConvert.DeserializeObject<List<QRCodeResponse>>(resultContent);
                    if (qRCodeResponses != null && qRCodeResponses.All(x => x.symbol.All(y => !string.IsNullOrWhiteSpace(y.error))))
                    {
                        return BadRequestErrorResponse("Error occured while reading QR code.");
                    }
                }
            }
            return OkResponse(qRCodeResponses?.FirstOrDefault()?.symbol.FirstOrDefault()?.data);
        }

    }
}
