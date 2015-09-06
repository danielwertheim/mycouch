using System;
using MyCouch.Requests;

namespace MyCouch.Cloudant.Requests
{
#if !PCL && !vNext
    [Serializable]
#endif
    public class GenerateApiKeyRequest : Request { }
}