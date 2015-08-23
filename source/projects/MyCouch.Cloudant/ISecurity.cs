using System.Threading.Tasks;
using MyCouch.Cloudant.Requests;
using MyCouch.Cloudant.Responses;

namespace MyCouch.Cloudant
{
    public interface ISecurity
    {
        Task<GenerateApiKeyResponse> GenerateApiKey();
        Task<GenerateApiKeyResponse> GenerateApiKey(GenerateApiKeyRequest request);
    }
}