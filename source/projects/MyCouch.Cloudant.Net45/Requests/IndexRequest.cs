using MyCouch.Cloudant.Querying;
using MyCouch.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCouch.Cloudant.Requests
{
#if !PCL
    [Serializable]
#endif
    public class IndexRequest : Request, IIndexParameters
    {
        protected IIndexParameters State { get; private set; }
        /// <summary>
        /// The design document to which an index belongs
        /// </summary>
        public string DesignDocument
        {
            get { return State.DesignDocument; }
            set { State.DesignDocument = value; }
        }
        /// <summary>
        /// The type of index
        /// </summary>
        public IndexType? Type
        {
            get { return State.Type; }
            set { State.Type = value; }
        }
        /// <summary>
        /// The name of the index
        /// </summary>
        public string Name
        {
            get { return State.Name; }
            set { State.Name = value; }
        }
        /// <summary>
        /// Index fields
        /// </summary>
        public IList<IndexField> Fields
        {
            get { return State.Fields; }
            set { State.Fields = value; }
        }

        public IndexRequest()
        {
            State = new IndexParameters();
        }

        public virtual IndexRequest Configure(Action<IndexParametersConfigurator> configurator)
        {
            configurator(new IndexParametersConfigurator(State));

            return this;
        }
    }
}
