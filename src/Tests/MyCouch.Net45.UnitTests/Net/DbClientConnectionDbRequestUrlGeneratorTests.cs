using System;
using FluentAssertions;
using MyCouch.Net;
using MyCouch.UnitTests.Fakes;
using Xunit;

namespace MyCouch.UnitTests.Net
{
    public class DbClientConnectionDbRequestUrlGeneratorTests : UnitTestsOf<DbClientConnectionDbRequestUrlGenerator>
    {
        private readonly DbClientConnectionFake _fakeConnection;

        public DbClientConnectionDbRequestUrlGeneratorTests()
        {
            _fakeConnection = new DbClientConnectionFake(new Uri("http://foo.com:5984/thedb"), "thedb");

            SUT = new DbClientConnectionDbRequestUrlGenerator(_fakeConnection);
        }

        [Fact]
        public void When_generating_for_same_db_It_returns_db_specific_url()
        {
            SUT.Generate("thedb").Should().Be(_fakeConnection.Address.AbsoluteUri);
        }

        [Fact]
        public void When_generating_for_other_db_It_throws_exception()
        {
            Action a = () => SUT.Generate("904dee5404b34537b61ba05ce3f683cf");

            a.ShouldThrow<InvalidOperationException>().WithMessage(ExceptionStrings.DbRequestUrlIsAgainstOtherDb);
        }
    }
}