﻿using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.HttpRequestFactories;
using MyCouch.Requests;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using MyCouch.Serialization;

namespace MyCouch.Contexts
{
    public class Changes : ApiContextBase<IDbConnection>, IChanges
    {
        protected GetChangesHttpRequestFactory HttpRequestFactory { get; set; }
        protected GetContinuousChangesHttpRequestFactory ContinuousHttpRequestFactory { get; set; }
        protected ChangesResponseFactory ChangesResponseFactory { get; set; }
        protected ContinuousChangesResponseFactory ContinuousChangesResponseFactory { get; set; }

        public Func<TaskFactory> ObservableWorkTaskFactoryResolver { protected get; set; }

        public Changes(IDbConnection connection, ISerializer serializer)
            : base(connection)
        {
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            HttpRequestFactory = new GetChangesHttpRequestFactory(serializer);
            ContinuousHttpRequestFactory = new GetContinuousChangesHttpRequestFactory(serializer);
            ChangesResponseFactory = new ChangesResponseFactory(serializer);
            ContinuousChangesResponseFactory = new ContinuousChangesResponseFactory(serializer);
            ObservableWorkTaskFactoryResolver = () => Task.Factory;
        }

        public virtual async Task<ChangesResponse> GetAsync(GetChangesRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = HttpRequestFactory.Create(request);

            using (var httpResponse = await SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ForAwait())
            {
                return await ChangesResponseFactory.CreateAsync(httpResponse).ForAwait();
            }
        }

        public virtual async Task<ChangesResponse<TIncludedDoc>> GetAsync<TIncludedDoc>(GetChangesRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = HttpRequestFactory.Create(request);

            using (var httpResponse = await SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ForAwait())
            {
                return await ChangesResponseFactory.CreateAsync<TIncludedDoc>(httpResponse).ForAwait();
            }
        }

        public virtual Task<ContinuousChangesResponse> GetAsync(GetChangesRequest request, Action<string> onRead, CancellationToken cancellationToken = default)
        {
            return GetAsync(request, data =>
            {
                onRead(data);
                return Task.FromResult(0);
            }, cancellationToken);
        }

        public virtual async Task<ContinuousChangesResponse> GetAsync(GetChangesRequest request, Func<string,Task> onRead, CancellationToken cancellationToken = default)
        {
            var httpRequest = ContinuousHttpRequestFactory.Create(request);

            using (var httpResponse = await SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ForAwait())
            {
                var response = await ContinuousChangesResponseFactory.CreateAsync(httpResponse).ForAwait();
                if (response.IsSuccess)
                {
                    using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
                    {
                        using (var reader = new StreamReader(content, MyCouchRuntime.DefaultEncoding))
                        {
                            while (!cancellationToken.IsCancellationRequested && !reader.EndOfStream)
                            {
                                //cancellationToken.ThrowIfCancellationRequested();
                                if(!cancellationToken.IsCancellationRequested)
                                    await onRead(reader.ReadLine()).ForAwait();
                            }
                        }
                    }
                }
                return response;
            }
        }

        public virtual IObservable<string> ObserveContinuous(GetChangesRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = ContinuousHttpRequestFactory.Create(request);

            var ob = new MyObservable<string>();

            Task.Factory.StartNew(async () =>
            {
                using (var httpResponse = await SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ForAwait())
                {
                    var response = await ContinuousChangesResponseFactory.CreateAsync(httpResponse).ForAwait();
                    if (response.IsSuccess)
                    {
                        using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
                        {
                            using (var reader = new StreamReader(content, MyCouchRuntime.DefaultEncoding))
                            {
                                while (!cancellationToken.IsCancellationRequested && !reader.EndOfStream)
                                {
                                    //cancellationToken.ThrowIfCancellationRequested();
                                    if (!cancellationToken.IsCancellationRequested)
                                        ob.Notify(reader.ReadLine());
                                }
                                ob.Complete();
                            }
                        }
                    }
                }
            }, cancellationToken).ForAwait();

            return ob;
        }
    }
}