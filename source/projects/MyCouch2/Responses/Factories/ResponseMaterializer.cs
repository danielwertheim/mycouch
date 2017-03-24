using System.Net.Http;
using System.Threading.Tasks;

namespace MyCouch.Responses.Factories
{
    public delegate Task ResponseMaterializer<in T>(T response, HttpResponseMessage httpResponse) where T : Response;
}