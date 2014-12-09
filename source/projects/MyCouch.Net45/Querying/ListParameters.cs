using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyCouch.Querying
{
#if !PCL
    [Serializable]
#endif
    public class ListParameters : IListParameters
    {
        public ListIdentity ListIdentity { get; private set; }

        public ListParameters(ListIdentity listIdentity, string viewName)
        {
            Ensure.That(listIdentity, "listIdentity").IsNotNull();
            Ensure.That(viewName, "viewName").IsNotNullOrWhiteSpace();

            ListIdentity = listIdentity;
            ViewName = viewName;
        }

        public string ViewName { get; set; }
        public object Key { get; set; }
        public IDictionary<string, object> AdditionalQueryParameters { get; set; }
        public bool HasAdditionalQueryParameters { get { return AdditionalQueryParameters != null && AdditionalQueryParameters.Any(); } }
    }
}