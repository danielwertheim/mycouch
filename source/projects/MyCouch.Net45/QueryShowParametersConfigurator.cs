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
            Ensure.That(value, "value").IsNotNullOrWhiteSpace();

            Parameters.DocId = value;

            return this;
        }
        public virtual QueryShowParametersConfigurator Accepts(params string[] accepts)
        {
            Ensure.That(accepts, "accepts").HasItems();

            Parameters.Accepts = accepts;

            return this;
        }

        public virtual QueryShowParametersConfigurator CustomQueryParameters(IDictionary<string, object> parameters)
        {
            Ensure.That(parameters, "parameters").HasItems();

            Parameters.CustomQueryParameters = parameters;

            return this;
        }
    }
}