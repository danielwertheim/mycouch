using System.IO;
using EnsureThat;
using Newtonsoft.Json;

namespace MyCouch.Serialization
{
    public class JsonResponseMapper
    {
        protected readonly SerializationConfiguration Configuration;

        public JsonResponseMapper(SerializationConfiguration configuration)
        {
            Ensure.That(configuration, "configuration").IsNotNull();

            Configuration = configuration;
        }

        public virtual void Map(Stream jsonData, JsonResponseMappings mappings)
        {
            Ensure.That(jsonData, "jsonData").IsNotNull();
            Ensure.That(mappings, "mappings").HasItems();

            var numOfHandlersProcessed = 0;

            using (var sr = new StreamReader(jsonData))
            {
                using (var jr = Configuration.ApplyConfigToReader(new JsonTextReader(sr)))
                {
                    while (jr.Read())
                    {
                        if (numOfHandlersProcessed == mappings.Count)
                            break;

                        if (jr.TokenType != JsonToken.PropertyName)
                            continue;

                        var propName = jr.Value.ToString();
                        if (!mappings.ContainsKey(propName))
                            continue;

                        if (!jr.Read())
                            break;

                        mappings[propName](jr);

                        numOfHandlersProcessed++;
                    }
                }
            }
        }
    }
}