using System.IO;
using System.Linq;
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
        protected readonly QueryResponseRowsDeserializer RowsDeserializer;

        protected QueryResponseFactoryBase(SerializationConfiguration serializationConfiguration)
            : base(serializationConfiguration)
        {
            Serializer = JsonSerializer.Create(SerializationConfiguration.Settings);
            RowsDeserializer = new QueryResponseRowsDeserializer(SerializationConfiguration);
        }

        protected virtual void OnSuccessfulResponse<T>(QueryResponse<T> response, HttpResponseMessage httpResponse) where T : class
        {
            using (var content = httpResponse.Content.ReadAsStream())
                PopulateResponse(response, content);
        }

        protected virtual void PopulateResponse<T>(QueryResponse<T> response, Stream data) where T : class
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

        protected virtual void OnPopulateTotalRows<T>(QueryResponse<T> response, JsonReader jr) where T : class
        {
            response.TotalRows = (long)jr.Value;
        }

        protected virtual void OnPopulateUpdateSeq<T>(QueryResponse<T> response, JsonReader jr) where T : class
        {
            response.UpdateSeq = (long)jr.Value;
        }

        protected virtual void OnPopulateOffset<T>(QueryResponse<T> response, JsonReader jr) where T : class
        {
            response.OffSet = (long)jr.Value;
        }

        protected virtual void OnPopulateRows<T>(QueryResponse<T> response, JsonReader jr) where T : class
        {
            response.Rows = RowsDeserializer.Deserialize<T>(jr).ToArray();
        }
    }
}