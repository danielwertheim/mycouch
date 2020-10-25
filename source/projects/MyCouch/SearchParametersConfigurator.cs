using System.Linq;
using EnsureThat;
using System.Collections.Generic;

namespace MyCouch
{
   public class SearchParametersConfigurator
   {
       protected readonly ISearchParameters Parameters;

       public SearchParametersConfigurator(ISearchParameters parameters)
       {
           Parameters = parameters;
       }

       /// <summary>
       /// Lucene expression that will be used to query the index.
       /// </summary>
       /// <param name="value"></param>
       /// <returns></returns>
       public virtual SearchParametersConfigurator Expression(string value)
       {
           Ensure.String.IsNotNullOrWhiteSpace(value, nameof(value));

           Parameters.Expression = value;

           return this;
       }

       /// <summary>
       /// Allow the results from a stale search index to be used.
       /// </summary>
       /// <param name="value"></param>
       /// <returns></returns>
       public virtual SearchParametersConfigurator Stale(Stale value)
       {
           Parameters.Stale = value;

           return this;
       }

       /// <summary>
       /// A bookmark that was received from a previous search. This
       /// allows you to page through the results. If there are no more
       /// results after the bookmark, you will get a response with an
       /// empty rows array and the same bookmark. That way you can
       /// determine that you have reached the end of the result list.
       /// </summary>
       /// <param name="value"></param>
       /// <returns></returns>
       public virtual SearchParametersConfigurator Bookmark(string value)
       {
           Ensure.String.IsNotNullOrWhiteSpace(value, nameof(value));

           Parameters.Bookmark = value;

           return this;
       }

       /// <summary>
       /// Sort expressions used to sort the output.
       /// </summary>
       /// <param name="sortExpressions"></param>
       /// <returns></returns>
       public virtual SearchParametersConfigurator Sort(params string[] sortExpressions)
       {
           Ensure.That(sortExpressions, "sortExpressions").HasItems();

           Parameters.Sort = sortExpressions.ToList();

           return this;
       }

       /// <summary>
       /// Include the full content of the documents in the return;
       /// </summary>
       /// <param name="value"></param>
       /// <returns></returns>
       public virtual SearchParametersConfigurator IncludeDocs(bool value)
       {
           Parameters.IncludeDocs = value;

           return this;
       }

       /// <summary>
       /// Limit the number of the returned documents to the specified number.
       /// </summary>
       /// <param name="value"></param>
       /// <returns></returns>
       public virtual SearchParametersConfigurator Limit(int value)
       {
           Parameters.Limit = value;

           return this;
       }

       /// <summary>
       /// Expression to define ranges for faceted numeric search fields
       /// </summary>
       /// <param name="ranges"></param>
       /// <returns></returns>
       public virtual SearchParametersConfigurator Ranges(object ranges)
       {
           Ensure.That(ranges, "ranges").IsNotNull();

           Parameters.Ranges = ranges;

           return this;
       }



       /// <summary>
       /// List of field names for which counts should be produced.
       /// </summary>
       /// <param name="counts"></param>
       /// <returns></returns>
       public virtual SearchParametersConfigurator Counts(params string[] counts)
       {
           Ensure.That(counts, "value").HasItems();

           Parameters.Counts = counts.ToList();

           return this;
       }

       /// <summary>
       /// Field by which to group search matches.
       /// </summary>
       /// <param name="value"></param>
       /// <returns></returns>
       public virtual SearchParametersConfigurator GroupField(string value)
       {
           Ensure.String.IsNotNullOrWhiteSpace(value, nameof(value));

           Parameters.GroupField = value;

           return this;
       }

       /// <summary>
       /// Maximum group count. This field can only be used if group_field is specified.
       /// </summary>
       /// <param name="value"></param>
       /// <returns></returns>
       public virtual SearchParametersConfigurator GroupLimit(int value)
       {
           Parameters.GroupLimit = value;

           return this;
       }

       /// <summary>
       /// This field defines the order of the groups in a search using group_field.
       /// The default sort order is relevance.
       /// </summary>
       /// <param name="sortExpressions"></param>
       /// <returns></returns>
       public virtual SearchParametersConfigurator GroupSort(params string[] sortExpressions)
       {
           Ensure.That(sortExpressions, "sortExpressions").HasItems();

           Parameters.GroupSort = sortExpressions.ToList();

           return this;
       }

       /// <summary>
       /// Defines a pair of field name and value so that search only matches
       /// documents that that have the given value in the field name.
       /// </summary>
       public virtual SearchParametersConfigurator DrillDown(string name, string value)
       {
           EnsureArg.IsNotNullOrWhiteSpace(name, nameof(name));
           Ensure.String.IsNotNullOrWhiteSpace(value, nameof(value));

           Parameters.DrillDown = new KeyValuePair<string, string>(name, value);

           return this;
       }
   }
}