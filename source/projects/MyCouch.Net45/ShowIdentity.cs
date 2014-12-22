﻿using System;
using EnsureThat;

namespace MyCouch
{
    /// <summary>
    /// Used to identify a certain search index in a design document.
    /// </summary>
#if !PCL
    [Serializable]
#endif
    public class ShowIdentity
    {
        public string DesignDocument { get; private set; }
        public string Name { get; private set; }

        public ShowIdentity(string designDocument, string name)
        {
            Ensure.That(designDocument, "designDocument").IsNotNullOrWhiteSpace();
            Ensure.That(name, "name").IsNotNullOrWhiteSpace();

            DesignDocument = designDocument;
            Name = name;
        }
    }
}