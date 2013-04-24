namespace MyCouch
{
    public interface IViewQueryConfigurator
    {
        /// <summary>
        ///  Include the full content of the documents in the return;
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IViewQueryConfigurator IncludeDocs(bool value);

        /// <summary>
        /// Return the documents in descending by key order.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IViewQueryConfigurator Descending(bool value);
        
        /// <summary>
        /// Return only documents that match the specified key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IViewQueryConfigurator Key(string value);
        
        /// <summary>
        /// Return records starting with the specified key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IViewQueryConfigurator StartKey(string value);
        
        /// <summary>
        /// Return records starting with the specified document ID.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IViewQueryConfigurator StartKeyDocId(string value);

        /// <summary>
        /// Stop returning records when the specified key is reached.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IViewQueryConfigurator EndKey(string value);

        /// <summary>
        /// Stop returning records when the specified document ID is reached.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IViewQueryConfigurator EndKeyDocId(string value);

        /// <summary>
        /// Specifies whether the specified end key should be included in the result.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IViewQueryConfigurator InclusiveEnd(bool value);

        /// <summary>
        /// Skip this number of records before starting to return the results.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IViewQueryConfigurator Skip(int value);

        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IViewQueryConfigurator Limit(int value);

        /// <summary>
        /// Use the reduction function.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IViewQueryConfigurator Reduce(bool value);
    }
}