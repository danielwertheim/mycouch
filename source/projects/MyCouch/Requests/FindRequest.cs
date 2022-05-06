using System;
using System.Collections.Generic;
using System.Linq;
using MyCouch.Querying;

namespace MyCouch.Requests
{
    public class FindRequest : Request, IFindParameters
    {
        protected IFindParameters State { get; }

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

        /// <summary>
        ///  Include conflicted documents if true. Intended use is to easily find conflicted documents, without an index or view.
        /// </summary>
        public bool? Conflicts
        {
            get { return this.State.Conflicts; }
            set { State.Conflicts = value; }
        }

        /// <summary>
        /// Whether or not the view results should be returned from a “stable” set of shards.
        /// </summary>
        public bool? Stable
        {
            get { return this.State.Stable; }
            set { State.Stable = value; }
        }

        /// <summary>
        /// Whether to update the index prior to returning the result.
        /// </summary>
        public bool? Update
        {
            get { return this.State.Update; }
            set { State.Update = value; }
        }

        /// <summary>
        /// Instruct a query to use a specific index. Specified either as "design_document"
        /// </summary>
        public string UseIndex
        {
            get { return this.State.UseIndex; }
            set { State.UseIndex = value; }
        }

        /// <summary>
        /// A string that enables you to specify which page of results you require. Used for paging through result sets. Every query returns an opaque string under the bookmark key that can then be passed back in a query to get the next page of results. If any part of the selector query changes between requests, the results are undefined.
        /// </summary>
        public string Bookmark
        {
            get { return this.State.Bookmark; }
            set { State.Bookmark = value; }
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

        public virtual bool HasUseIndex()
        {
            return !string.IsNullOrWhiteSpace(this.UseIndex);
        }

        public virtual bool HasBookmark()
        {
            return !string.IsNullOrWhiteSpace(this.Bookmark);
        }
    }
}
