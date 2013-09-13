using System.Collections.Generic;
using MyCouch.Responses;
using Newtonsoft.Json;

namespace MyCouch.Serialization
{
    /// <summary>
    /// Responsible for materializing <see cref="QueryResponse{TRow}"/> rows
    /// for assigning to <see cref="QueryResponse{TRow}.Rows"/>.
    /// </summary>
    public interface IQueryResponseRowsDeserializer
    {
        /// <summary>
        /// Takes a <see cref="JsonReader"/>, which should point to a node being
        /// an array. Traverses the tree and yields T
        /// of <see cref="QueryResponseRow"/> from it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jr"></param>
        /// <returns></returns>
        IEnumerable<T> Deserialize<T>(JsonReader jr) where T : QueryResponseRow;
    }
}