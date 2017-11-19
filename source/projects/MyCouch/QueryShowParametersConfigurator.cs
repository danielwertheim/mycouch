using System.Collections.Generic;
using EnsureThat;

namespace MyCouch
{
    public class QueryShowParametersConfigurator
    {
        protected readonly IShowParameters Parameters;

        public QueryShowParametersConfigurator(IShowParameters parameters)
        {
            Parameters = parameters;
        }

        public virtual QueryShowParametersConfigurator DocId(string value)
        {
            Ensure.String.IsNotNullOrWhiteSpace(value, nameof(value));

            Parameters.DocId = value;

            return this;
        }
        public virtual QueryShowParametersConfigurator Accepts(params string[] accepts)
        {
            EnsureArg.HasItems(accepts, nameof(accepts));

            Parameters.Accepts = accepts;

            return this;
        }

        public virtual QueryShowParametersConfigurator CustomQueryParameters(IDictionary<string, object> parameters)
        {
            EnsureArg.HasItems(parameters, nameof(parameters));

            Parameters.CustomQueryParameters = parameters;

            return this;
        }
    }
}