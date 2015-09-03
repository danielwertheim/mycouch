using System;
using MyCouch.Requests;

namespace MyCouch.Cloudant.Requests
{
#if !PCL
    [Serializable]
#endif
    public class GenerateApiKeyRequest : Request { }
}