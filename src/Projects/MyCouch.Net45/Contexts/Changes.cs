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
    public class Changes : ApiContextBase<IDbClientConnection>, IChanges
    {
        protected GetChangesHttpRequestFactory HttpRequestFactory { get; set; }
        protected GetContinuousChangesHttpRequestFactory ContinuousHttpRequestFactory { get; set; }
        protected ChangesResponseFactory ChangesResponseFactory { get; set; }
        protected ContinuousChangesResponseFactory ContinuousChangesResponseFactory { get; set; }

        public Func<IScheduler> ObservableSubscribeOnScheduler { protected get; set; }

        public Changes(IDbClientConnection connection, ISerializer serializer)
            : base(connection)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            HttpRequestFactory = new GetChangesHttpRequestFactory(Connection);
            ContinuousHttpRequestFactory = new GetContinuousChangesHttpRequestFactory(Connection);
            ChangesResponseFactory = new ChangesResponseFactory(serializer);
            ContinuousChangesResponseFactory = new ContinuousChangesResponseFactory(serializer);
            ObservableSubscribeOnScheduler = () => TaskPoolScheduler.Default;
        }

        public virtual async Task<ChangesResponse> GetAsync(GetChangesRequest request)
        {
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
            using (var httpRequest = HttpRequestFactory.Create(request))
            {
                using (var httpResponse = await SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead).ForAwait())
                {
                    return ChangesResponseFactory.Create<TIncludedDoc>(httpResponse);
                }
            }
        }

        public virtual async Task<ContinuousChangesResponse> GetAsync(GetChangesRequest request, Action<string> onRead, CancellationToken cancellationToken)
        {
            using (var httpRequest = ContinuousHttpRequestFactory.Create(request))
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
                                    onRead(reader.ReadLine());
                                }
                            }
                        }
                    }
                    return response;
                }
            }
        }

        public virtual IObservable<string> ObserveContinuous(GetChangesRequest request, CancellationToken cancellationToken)
        {
            EnsureContinuousFeedIsRequested(request);

            return Observable.Create<string>(async o =>
            {
                using (var httpRequest = ContinuousHttpRequestFactory.Create(request))
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

        protected virtual void EnsureContinuousFeedIsRequested(GetChangesRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            if (request.Feed.HasValue && request.Feed != ChangesFeed.Continuous)
                throw new ArgumentException(ExceptionStrings.GetContinuousChangesInvalidFeed, "request");
        }
    }
}