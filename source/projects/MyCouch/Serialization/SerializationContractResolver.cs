using MyCouch.Extensions;
using Newtonsoft.Json.Serialization;

namespace MyCouch.Serialization
{
    public class SerializationContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return base.ResolvePropertyName(propertyName.ToCamelCase());
        }
    }
}