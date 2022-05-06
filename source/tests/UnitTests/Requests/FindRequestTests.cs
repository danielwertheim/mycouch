namespace UnitTests.Requests
{
    using System.Collections.Generic;
    using FluentAssertions;
    using MyCouch;
    using MyCouch.Requests;
    using Xunit;

    public sealed class FindRequestTests : UnitTestsOf<FindRequest>
    {
        private const string Bookmark = "g1AAAADOeJzLYWBgYM5gTmGQT0lKzi9KdUhJMtbLSs1LLUst0kvOyS9NScwr0ctLLckBKmRKZEiy____f1YGk5v9l1kRDUCxRCaideexAEmGBiAFNGM_2JBvNSdBYomMJBpyAGLIfxRDmLIAxz9DAg";
        private const string IndexName = "_design/indexesByType";
        private const string DataField = "field";

        public FindRequestTests()
        {
            this.SUT = new FindRequest();
        }

        [Fact]
        public void When_Bookmark_are_null_It_returns_false_for_HasBookmark()
        {
            this.SUT.Bookmark = null;

            this.SUT.HasBookmark().Should().BeFalse();
        }

        [Fact]
        public void When_Bookmark_are_empty_It_returns_false_for_HasBookmark()
        {
            this.SUT.Bookmark = " ";

            this.SUT.HasBookmark().Should().BeFalse();
        }

        [Fact]
        public void When_Bookmark_are_specified_It_returns_true_for_HasBookmark()
        {
            this.SUT.Bookmark = Bookmark;

            this.SUT.HasBookmark().Should().BeTrue();
        }

        [Fact]
        public void When_UseIndex_are_null_It_returns_false_for_HasUseIndex()
        {
            this.SUT.UseIndex = null;

            this.SUT.HasUseIndex().Should().BeFalse();
        }

        [Fact]
        public void When_UseIndex_are_empty_It_returns_false_for_HasUseIndex()
        {
            this.SUT.UseIndex = " ";

            this.SUT.HasUseIndex().Should().BeFalse();
        }

        [Fact]
        public void When_UseIndex_are_specified_It_returns_true_for_HasUseIndex()
        {
            this.SUT.UseIndex = IndexName;

            this.SUT.HasUseIndex().Should().BeTrue();
        }

        [Fact]
        public void When_Fields_are_null_It_returns_false_for_HasFields()
        {
            this.SUT.Fields = null;

            this.SUT.HasFields().Should().BeFalse();
        }

        [Fact]
        public void When_Fields_are_empty_It_returns_false_for_HasFields()
        {
            this.SUT.Fields = new List<string>();

            this.SUT.HasFields().Should().BeFalse();
        }

        [Fact]
        public void When_Fields_are_specified_It_returns_true_for_HasFields()
        {
            this.SUT.Fields = new List<string> { DataField };

            this.SUT.HasFields().Should().BeTrue();
        }

        [Fact]
        public void When_UseIndex_are_null_It_returns_false_for_HasSortings()
        {
            this.SUT.Sort = null;

            this.SUT.HasSortings().Should().BeFalse();
        }

        [Fact]
        public void When_UseIndex_are_empty_It_returns_false_for_HasSortings()
        {
            this.SUT.Sort = new List<SortableField>();

            this.SUT.HasSortings().Should().BeFalse();
        }

        [Fact]
        public void When_UseIndex_are_specified_It_returns_true_for_HasSortings()
        {
            this.SUT.Sort = new List<SortableField>
            {
                new SortableField(DataField, SortDirection.Desc)
            };

            this.SUT.HasSortings().Should().BeTrue();
        }
    }
}
