using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace Haley.Models
{
    public class RestResponse
    {
        public string error_message { get; set; }
        public string server_url { get; set; }
        public Uri response_uri { get; set; }
        public Exception exception { get; set; }
        public byte[] contents_raw { get; set; }
        public bool is_success { get; set; }
        public HttpStatusCode status_code { get; set; }
        public string content { get; set; }
        public string content_encoding { get; set; }
        public long content_length { get; set; }
        public string content_type { get; set; }
        public string status_description { get; set; }
        public RestResponse() { }
    }
}
