using System;
using System.Net;
using System.Net.Http;

namespace MyCouch
{
    public interface IResponse
    {
        HttpStatusCode StatusCode { get; set; }
        bool IsSuccess { get; }
        Uri RequestUri { get; set; }
        HttpMethod RequestMethod { get; set; }
        string Error { get; set; }
        string Reason { get; set; }
    }
}