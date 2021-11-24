using System.Net;
using System.Net.Http;
using FluentAssertions;
using MyCouch;
using MyCouch.Responses;
using MyCouch.Responses.Factories;
using Xunit;

namespace UnitTests.Responses.Factories
{
    public class PurgeResponseFactoryTests : UnitTestsOf<PurgeResponseFactory>
    {
        public PurgeResponseFactoryTests()
        {
            var boostrapper = new MyCouchClientBootstrapper();
            SUT = new PurgeResponseFactory(boostrapper.DocumentSerializerFn());
        }

        [Fact]
        public void When_response_for_purge_success_It_especifies_an_id_and_rev()
        {
            var message = CreateResponse(HttpStatusCode.Accepted, HttpMethod.Post, "/_purge", "{\"purge_seq\":null,\"purged\":{\"my_doc_id\":[\"my_doc_rev\"]}}");
            var r = SUT.CreateAsync(message);

            r.Result.IsSuccess.Should().BeTrue();
            r.Result.PurgeSeq.Should().BeNull();
            r.Result.Purged.SeqsById.Should().ContainKey("my_doc_id");
            r.Result.Purged.SeqsById["my_doc_id"].Should().Contain("my_doc_rev");
        }

        protected virtual HttpResponseMessage CreateResponse(HttpStatusCode statusCode, HttpMethod method, string requestUri, string content)
        {
            return new HttpResponseMessage 
            {
                StatusCode = statusCode,
                RequestMessage = new HttpRequestMessage(method, requestUri),
                Content = new StringContent(content)
            };
        }
    }
}