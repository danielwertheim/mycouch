using System.Collections.Generic;

namespace MyCouch
{
    public interface IViewQueryOptions : IEnumerable<KeyValuePair<string, string>>
    {
        /// <summary>
        /// Return the documents in descending by key order.
        /// </summary>
        bool Descending { get; set; }
        
        /// <summary>
        /// Return only documents that match the specified key.
        /// </summary>
        string Key { get; set; }
        
        /// <summary>
        /// Return records starting with the specified key.
        /// </summary>
        string StartKey { get; set; }
        
        /// <summary>
        /// Return records starting with the specified document ID.
        /// </summary>
        string StartKeyDocId { get; set; }
        
        /// <summary>
        /// Stop returning records when the specified key is reached.
        /// </summary>
        string EndKey { get; set; }
        
        /// <summary>
        /// Stop returning records when the specified document ID is reached.
        /// </summary>
        string EndKeyDocId { get; set; }
        
        /// <summary>
        /// Skip this number of records before starting to return the results.
        /// </summary>
        int Skip { get; set; }
        
        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        int Limit { get; set; }
        
        /// <summary>
        /// Use the reduction function.
        /// </summary>
        bool Reduce { get; set; }
    }
}