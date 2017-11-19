using System;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;

namespace MyCouch.HttpRequestFactories
{
    public class GetChangesHttpRequestFactory : GetChangesHttpRequestFactoryBase
    {
        public override HttpRequest Create(GetChangesRequest request)
        {
            Ensure.Any.IsNotNull(request, nameof(request));

            EnsureNonContinuousFeedIsRequested(request);

            return base.Create(request);
        }

        protected virtual void EnsureNonContinuousFeedIsRequested(GetChangesRequest request)
        {
            if (request.Feed.HasValue && request.Feed == ChangesFeed.Continuous)
                throw new ArgumentException(ExceptionStrings.GetChangesForNonContinuousFeedOnly, nameof(request));
        }
    }
}