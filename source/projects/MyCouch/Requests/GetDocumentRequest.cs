﻿using System;
using MyCouch.EnsureThat;

namespace MyCouch.Requests
{
#if !PCL && !vNext
    [Serializable]
#endif
    public class GetDocumentRequest : Request
    {
        public string Id { get; private set; }
        public string Rev { get; private set; }
        public bool? Conflicts { get; set; }

        public GetDocumentRequest(string id, string rev = null)
        {
            Ensure.That(id, "id").IsNotNullOrWhiteSpace();

            Id = id;
            Rev = rev;
        }
    }
}