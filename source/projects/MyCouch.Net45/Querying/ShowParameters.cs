using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCouch.Querying
{
#if !PCL
    [Serializable]
#endif
    public class ShowParameters : IShowParameters
    {
        public ShowIdentity ShowIdentity { get; private set; }
        public string Id { get; set; }
        public IDictionary<string, object> CustomQueryParameters { get; set; }
        public bool HasCustomQueryParameters
        {
            get { return CustomQueryParameters != null && CustomQueryParameters.Any(); }
        }

        public ShowParameters(ShowIdentity showIdentity)
        {
            Ensure.That(showIdentity, "showIdentity").IsNotNull();

            ShowIdentity = showIdentity;
        }
    }
}
