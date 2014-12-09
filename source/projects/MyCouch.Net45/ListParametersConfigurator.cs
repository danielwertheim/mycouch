using System.Linq;
using EnsureThat;
using System.Collections.Generic;

namespace MyCouch
{
    public class ListParametersConfigurator
    {
        protected readonly IListParameters Parameters;

        public ListParametersConfigurator(IListParameters parameters)
        {
            Parameters = parameters;
        }

        public virtual ListParametersConfigurator ViewName(string viewName)
        {
            Ensure.That(viewName, "viewName").IsNotNull();

            Parameters.ViewName = viewName;

            return this;
        }

        public virtual ListParametersConfigurator Key(object key)
        {
            Ensure.That(key, "key").IsNotNull();

            Parameters.Key = key;

            return this;
        }

        public virtual ListParametersConfigurator AdditionalQueryParameters(IDictionary<string, object> additionalQueryParameters)
        {
            Ensure.That(additionalQueryParameters, "additionalQueryParameters").HasItems();

            Parameters.AdditionalQueryParameters = additionalQueryParameters;

            return this;
        }
    }
}