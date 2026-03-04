using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Helper
{
    public class CommonResponse
    {
        public dynamic Data {  get; set; }

        public string? Message { get; set; } = "Something went wrong!";

        public bool? Status { get; set; } = false;

        public HttpStatusCode StatusCode { get; set; }
    }
}
