using System;
using System.Net;
using System.Net.Http;

namespace MyCouch
{
    public interface IResponse
    {
        HttpStatusCode StatusCode { get; }
        bool IsSuccess { get; }
        Uri RequestUri { get; }
        HttpMethod RequestMethod { get; }
        string Error { get; }
        string Reason { get; }
    }
}