using System;
using FluentAssertions;
using MyCouch.Net;
using Xunit;

namespace MyCouch.UnitTests.Net
{
    public class DbClientConnectionDbRequestUrlGeneratorTests : UnitTestsOf<ConstantRequestUrlGenerator>
    {
        private readonly Uri _fakeDbUri;

        public DbClientConnectionDbRequestUrlGeneratorTests()
        {
            _fakeDbUri = new Uri("http://foo.com:5984/thedb");

            SUT = new ConstantRequestUrlGenerator(_fakeDbUri, "thedb");
        }

        [Fact]
        public void When_generating_for_same_db_It_returns_db_specific_url()
        {
            SUT.Generate("thedb").Should().Be(_fakeDbUri.AbsoluteUri);
        }

        [Fact]
        public void When_generating_for_other_db_It_throws_exception()
        {
            Action a = () => SUT.Generate("904dee5404b34537b61ba05ce3f683cf");

            a.ShouldThrow<InvalidOperationException>().WithMessage(ExceptionStrings.ConstantRequestUrlGenerationAgainstOtherDb);
        }
    }
}