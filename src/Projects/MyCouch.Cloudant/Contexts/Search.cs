using MyCouch.Contexts;
using MyCouch.Net;

namespace MyCouch.Cloudant.Contexts
{
    public class Search : ApiContextBase, ISearch
    {
        public Search(IConnection connection) 
            : base(connection)
        {
        }
    }
}