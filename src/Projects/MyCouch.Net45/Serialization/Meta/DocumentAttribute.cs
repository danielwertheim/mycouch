using System;

namespace MyCouch.Serialization.Meta
{
    public class DocumentAttribute : Attribute
    {
        public string DocType { get; set; }
    }
}