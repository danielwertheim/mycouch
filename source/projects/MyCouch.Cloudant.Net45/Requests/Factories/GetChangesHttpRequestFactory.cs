using System;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class GetChangesHttpRequestFactory : GetChangesHttpRequestFactoryBase
    {
        public GetChangesHttpRequestFactory(IDbClientConnection connection) : base(connection) { }

        public override HttpRequest Create(GetChangesRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            EnsureNonContinuousFeedIsRequested(request);

            return base.Create(request);
        }

        protected virtual void EnsureNonContinuousFeedIsRequested(GetChangesRequest request)
        {
            if (request.Feed.HasValue && request.Feed == ChangesFeed.Continuous)
                throw new ArgumentException(ExceptionStrings.GetChangesForNonContinuousFeedOnly, "request");
        }
    }
}