using System;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class GetContinuousChangesHttpRequestFactory : GetChangesHttpRequestFactoryBase
    {
        public override HttpRequest Create(GetChangesRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            EnsureContinuousFeedIsRequested(request);

            return base.Create(request);
        }

        protected virtual void EnsureContinuousFeedIsRequested(GetChangesRequest request)
        {
            if (request.Feed.HasValue && request.Feed != ChangesFeed.Continuous)
                throw new ArgumentException(ExceptionStrings.GetContinuousChangesInvalidFeed, "request");
        }
    }
}