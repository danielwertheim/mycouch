using MyCouch.Cloudant.Querying;
using MyCouch.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyCouch.Cloudant.Requests
{
#if !PCL && !vNext
    [Serializable]
#endif
    public class FindRequest : Request, IFindParameters
    {
        protected IFindParameters State { get; private set; }

        /// <summary>
        /// JSON object describing criteria used to select documents.
        /// </summary>
        public string SelectorExpression
        {
            get { return State.SelectorExpression; }
            set { State.SelectorExpression = value; }
        }
        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        public int? Limit 
        {
            get { return State.Limit; }
            set { State.Limit = value; }
        }
        /// <summary>
        /// Skip the first n results, where n is the value specified.
        /// </summary>
        public int? Skip
        {
            get { return State.Skip; }
            set { State.Skip = value; }
        }
        /// <summary>
        /// List of fields with sort directions to specify sorting of results.
        /// </summary>
        public IList<SortableField> Sort
        {
            get { return State.Sort; }
            set { State.Sort = value; }
        }
        /// <summary>
        /// The list of fields of the documents to be returned.
        /// </summary>
        public IList<string> Fields
        {
            get { return State.Fields; }
            set { State.Fields = value; }
        }
        /// <summary>
        /// Read quorum needed for the result.
        /// </summary>
        public int? ReadQuorum
        {
            get { return State.ReadQuorum; }
            set { State.ReadQuorum = value; }
        }

        public FindRequest()
        {
            State = new FindParameters();
        }

        public virtual FindRequest Configure(Action<FindParametersConfigurator> configurator)
        {
            configurator(new FindParametersConfigurator(State));

            return this;
        }
        
        public virtual bool HasSortings()
        {
            return Sort != null && Sort.Any();
        }

        public virtual bool HasFields()
        {
            return Fields != null && Fields.Any();
        }
    }
}
