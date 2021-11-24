using System;
using System.Net.Http;
using EnsureThat;
using MyCouch.EntitySchemes;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Serialization;

namespace MyCouch.HttpRequestFactories
{
    public class PurgeEntityHttpRequestFactory
    {
        protected IEntityReflector Reflector { get; private set; }
        protected ISerializer Serializer { get; private set; }

        public PurgeEntityHttpRequestFactory(IEntityReflector reflector, ISerializer serializer)
        {
            Ensure.Any.IsNotNull(reflector, nameof(reflector));
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            Reflector = reflector;
            Serializer = serializer;
        }

        public virtual HttpRequest Create<T>(PurgeEntityRequest<T> request) where T : class
        {
            Ensure.Any.IsNotNull(request, nameof(request));

            var entityId = GetEntityId(request);
            var entityRev = GetEntityRev(request);
            var data = new PurgeData(entityId, entityRev);

            return new HttpRequest(HttpMethod.Post, GenerateRelativeUrl(entityId, entityRev))
                .SetRequestTypeHeader(request.GetType())
                .SetJsonContent(Serializer.ToJson(data));
        }

        protected virtual string GetEntityId<T>(PurgeEntityRequest<T> request) where T : class
        {
            var entityId = Reflector.IdMember.GetValueFrom(request.Entity);

            if (string.IsNullOrWhiteSpace(entityId))
                throw new ArgumentException("Could not extract entity Id from entity being deleted. Ensure member exists.", nameof(request));

            return entityId;
        }

        protected virtual string GetEntityRev<T>(PurgeEntityRequest<T> request) where T : class
        {
            var entityRev = Reflector.RevMember.GetValueFrom(request.Entity);
            if (string.IsNullOrWhiteSpace(entityRev))
                throw new ArgumentException("Could not extract entity rev from entity being deleted. Ensure member exists.", nameof(request));

            return entityRev;
        }

        protected virtual string GenerateRelativeUrl(string entityId, string entityRev)
        {
            return "/_purge";
        }
    }
}