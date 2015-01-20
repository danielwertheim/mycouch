
using System.Collections.Generic;
namespace MyCouch
{
    public interface IShowParameters
    {
        ShowIdentity ShowIdentity { get; }
        string[] Accepts { get; set; }
        bool HasAccepts { get; }
        string DocId { get; set; }
        IDictionary<string, object> CustomQueryParameters { get; set; }
        bool HasCustomQueryParameters { get; }
    }
}
