using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QR_Code_App.Models
{
    public class QRCodeResponse
    {
        [JsonProperty("type")]
        public string type { get; set; }
        [JsonProperty("symbol")]
        public List<Symbol> symbol { get; set; }
    }
    public class Symbol
    {
        [JsonProperty("seq")]
        public int seq { get; set; }
        [JsonProperty("data")]
        public object data { get; set; }
        [JsonProperty("error")]
        public string error { get; set; }
    }
    //[{"type":"qrcode","symbol":[{"seq":0,"data":"HelloWorld auszulesen:","error":null}]}]
}