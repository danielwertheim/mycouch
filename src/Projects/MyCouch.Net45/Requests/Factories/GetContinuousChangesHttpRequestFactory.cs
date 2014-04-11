using System;
using MyCouch.EnsureThat;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public class GetContinuousChangesHttpRequestFactory : GetChangesHttpRequestFactoryBase
    {
        public GetContinuousChangesHttpRequestFactory(IDbClientConnection connection) : base(connection) { }

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