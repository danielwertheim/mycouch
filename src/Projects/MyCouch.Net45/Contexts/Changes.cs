using System;
using System.IO;
using System.Net.Http;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Requests;
using MyCouch.Requests.Factories;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Contexts
{
    public class Changes : ApiContextBase, IChanges
    {
        protected GetChangesHttpRequestFactory HttpRequestFactory { get; set; }
        protected ChangesResponseFactory ChangesResponseFactory { get; set; }
        protected ContinuousChangesResponseFactory ContinuousChangesResponseFactory { get; set; }

        public Func<IScheduler> ObservableSubscribeOnScheduler { protected get; set; }

        public Changes(IConnection connection, ISerializer serializer)
            : base(connection)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            HttpRequestFactory = new GetChangesHttpRequestFactory(Connection);
            ChangesResponseFactory = new ChangesResponseFactory(serializer);
            ContinuousChangesResponseFactory = new ContinuousChangesResponseFactory(serializer);
            ObservableSubscribeOnScheduler = () => TaskPoolScheduler.Default;
        }

        public virtual async Task<ChangesResponse> GetAsync(GetChangesRequest request)
        {
            Ensure.That(request, "request").IsNotNull();
            EnsureNonContinuousFeed(request);

            using (var httpRequest = HttpRequestFactory.Create(request))
            {
                using (var httpResponse = await SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead).ForAwait())
                {
                    return ChangesResponseFactory.Create(httpResponse);
                }
            }
        }

        public virtual async Task<ChangesResponse<TIncludedDoc>> GetAsync<TIncludedDoc>(GetChangesRequest request)
        {
            Ensure.That(request, "request").IsNotNull();
            EnsureNonContinuousFeed(request);

            using (var httpRequest = HttpRequestFactory.Create(request))
            {
                using (var httpResponse = await SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead).ForAwait())
                {
                    return ChangesResponseFactory.Create<TIncludedDoc>(httpResponse);
                }
            }
        }

        public virtual IObservable<string> GetAsync(GetChangesRequest request, CancellationToken cancellationToken)
        {
            Ensure.That(request, "request").IsNotNull();

            if (!request.Feed.HasValue || request.Feed != ChangesFeed.Continuous)
                throw new ArgumentException(ExceptionStrings.GetContinuousChangesInvalidFeed, "request");


            return Observable.Create<string>(async o =>
            {
                using (var httpRequest = HttpRequestFactory.Create(request))
                {
                    using (var httpResponse = await SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ForAwait())
                    {
                        var response = ContinuousChangesResponseFactory.Create(httpResponse);
                        if (response.IsSuccess)
                        {
                            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
                            {
                                using (var reader = new StreamReader(content, MyCouchRuntime.DefaultEncoding))
                                {
                                    while (!cancellationToken.IsCancellationRequested && !reader.EndOfStream)
                                    {
                                        cancellationToken.ThrowIfCancellationRequested();
                                        o.OnNext(reader.ReadLine());
                                    }
                                }
                            }
                        }
                        o.OnCompleted();

                        return Disposable.Empty;
                    }
                }
            }).SubscribeOn(ObservableSubscribeOnScheduler());
        }

        protected virtual void EnsureNonContinuousFeed(GetChangesRequest request)
        {
            if(request.Feed.HasValue && request.Feed == ChangesFeed.Continuous)
                throw new ArgumentException(ExceptionStrings.GetChangesForNonContinuousFeedOnly, "request");
        }
    }
}