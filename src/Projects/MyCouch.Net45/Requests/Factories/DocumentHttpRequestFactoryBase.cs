using System.Collections.Generic;
namespace MyCouch.Requests.Factories
{
    public abstract class DocumentHttpRequestFactoryBase : HttpRequestFactoryBase
    {
        protected DocumentHttpRequestFactoryBase(IConnection connection) : base(connection) {}

        protected virtual string GenerateRequestUrl(string id = null, string rev = null, bool batch = false)
        {
            var queryParameters = new List<string>();
            if (rev != null)
            {
                queryParameters.Add(string.Format("rev={0}", rev));
            }
            if (batch)
            {
                queryParameters.Add("batch=ok");
            }
            var queryParametersForURI = string.Join("&", queryParameters);

            return string.Format("{0}/{1}{2}",
                Connection.Address,
                id ?? string.Empty,
                string.IsNullOrEmpty(queryParametersForURI) ? "" : "?" + queryParametersForURI);
        }
    }
}