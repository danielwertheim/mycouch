using System;
using FluentAssertions;
using MyCouch.Net;
using Xunit;

namespace MyCouch.UnitTests.Net
{
    public class AppendingDbRequestUrlGeneratorTests : UnitTestsOf<AppendingDbRequestUrlGenerator>
    {
        private readonly Uri _fakeUri;

        public AppendingDbRequestUrlGeneratorTests()
        {
            _fakeUri = new Uri("http://foo.com:5984");

            SUT = new AppendingDbRequestUrlGenerator(_fakeUri);
        }

        [Fact]
        public void When_generating_for_same_db_It_returns_db_specific_url()
        {
            SUT.Generate("thedb").Should().Be(_fakeUri.AbsoluteUri + "thedb");
        }
    }
}