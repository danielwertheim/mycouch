using System;
using System.Net.Http;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class PutAttachmentHttpRequestFactory
    {
        public virtual HttpRequest Create(PutAttachmentRequestBase request)
        {
            Ensure.Any.IsNotNull(request, nameof(request));

            var httpRequest = new HttpRequest(HttpMethod.Put, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType())
                .SetIfMatchHeader(request.DocRev);

            SetContent(request, httpRequest);

            return httpRequest;
        }

        protected virtual string GenerateRelativeUrl(PutAttachmentRequestBase request)
        {
            var urlParams = new UrlParams();

            urlParams.AddIfNotNullOrWhiteSpace("rev", request.DocRev);

            return string.Format("/{0}/{1}{2}",
                new UrlSegment(request.DocId),
                new UrlSegment(request.Name),
                new QueryString(urlParams));
        }

        protected virtual void SetContent(PutAttachmentRequestBase request, HttpRequest httpRequest)
        {
            var bufferRequest = request as PutAttachmentRequest;
            if (bufferRequest != null)
            {
                httpRequest.SetContent(bufferRequest.Content, request.ContentType);
                return;
            }

            var streamRequest = request as PutAttachmentStreamRequest;
            if (streamRequest == null)
            {
                throw new ArgumentException(
                    string.Format("Unsupported {0} implementation.", nameof(PutAttachmentRequestBase)),
                    nameof(request));
            }

            httpRequest.SetContent(streamRequest.Content, request.ContentType);
        }
    }
}