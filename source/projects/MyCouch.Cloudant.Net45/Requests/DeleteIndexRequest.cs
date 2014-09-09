
using MyCouch.Requests;
using System;
namespace MyCouch.Cloudant.Requests
{
#if !PCL
    [Serializable]
#endif
    public class DeleteIndexRequest : Request
    {
        public string DesignDoc { get; private set; }
        public string Name { get; private set; }
        public IndexType Type { get; private set; }

        public DeleteIndexRequest(string designDoc, string name, IndexType type = IndexType.Json)
        {
            DesignDoc = designDoc;
            Name = name;
            Type = type;
        }
    }
}
