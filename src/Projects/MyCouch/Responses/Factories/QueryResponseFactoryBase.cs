using System.IO;
using System.Net.Http;
using MyCouch.Extensions;
using MyCouch.Responses.Meta;
using MyCouch.Serialization;
using Newtonsoft.Json;

namespace MyCouch.Responses.Factories
{
    public abstract class QueryResponseFactoryBase : ResponseFactoryBase
    {
        protected readonly JsonSerializer Serializer;

        protected QueryResponseFactoryBase(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration)
        {
            Serializer = JsonSerializer.Create(SerializationConfiguration.Settings);
        }

        protected virtual void OnSuccessfulResponse<T>(QueryResponse<T> response, HttpResponseMessage httpResponse) where T : QueryResponseRow
        {
            using (var content = httpResponse.Content.ReadAsStream())
                PopulateResponse(response, content);
        }

        protected virtual void PopulateResponse<T>(QueryResponse<T> response, Stream data) where T : QueryResponseRow
        {
            var mappings = new JsonResponseMappings
            {
                {ResponseMeta.Scheme.Queries.TotalRows, jr => OnPopulateTotalRows(response, jr)},
                {ResponseMeta.Scheme.Queries.UpdateSeq, jr => OnPopulateUpdateSeq(response, jr)},
                {ResponseMeta.Scheme.Queries.Offset, jr => OnPopulateOffset(response, jr)},
                {ResponseMeta.Scheme.Queries.Rows, jr => OnPopulateRows(response, jr)}
            };
            JsonMapper.Map(data, mappings);
        }

        protected virtual void OnPopulateTotalRows<T>(QueryResponse<T> response, JsonReader jr) where T : QueryResponseRow
        {
            response.TotalRows = (long)jr.Value;
        }

        protected virtual void OnPopulateUpdateSeq<T>(QueryResponse<T> response, JsonReader jr) where T : QueryResponseRow
        {
            response.UpdateSeq = (long)jr.Value;
        }

        protected virtual void OnPopulateOffset<T>(QueryResponse<T> response, JsonReader jr) where T : QueryResponseRow
        {
            response.OffSet = (long)jr.Value;
        }

        protected abstract void OnPopulateRows<T>(QueryResponse<T> response, JsonReader jr) where T : QueryResponseRow;
    }
}