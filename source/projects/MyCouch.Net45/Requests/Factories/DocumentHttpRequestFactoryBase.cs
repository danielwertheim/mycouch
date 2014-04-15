using System;
using System.Collections.Generic;
using System.Linq;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Net;

namespace MyCouch.Requests.Factories
{
    public abstract class DocumentHttpRequestFactoryBase : HttpRequestFactoryBase
    {
        protected ConstantRequestUrlGenerator RequestUrlGenerator { get; private set; }

        protected DocumentHttpRequestFactoryBase(IDbClientConnection connection)
        {
            Ensure.That(connection, "connection").IsNotNull();

            RequestUrlGenerator = new ConstantRequestUrlGenerator(connection.Address, connection.DbName);
        }

        protected virtual string GenerateRequestUrl(string id = null, string rev = null, params UrlParam[] parameters)
        {
            var queryParameters = new List<string>();

            if (rev != null)
                queryParameters.Add(string.Format("rev={0}", rev));

            if (parameters != null && parameters.Any())
            {
                queryParameters.AddRange(parameters.Where(p => p != null && p.HasValue).Select(p => string.Concat(p.Key, "=", p.Value)));
            }

            return string.Format("{0}/{1}{2}",
                RequestUrlGenerator.Generate(),
                id != null ? Uri.EscapeDataString(id) : string.Empty,
                queryParameters.Any() ? string.Join("&", queryParameters).PrependWith("?") : string.Empty);
        }
    }
}