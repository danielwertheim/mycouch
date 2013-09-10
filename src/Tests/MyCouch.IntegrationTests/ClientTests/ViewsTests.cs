using System.Linq;
using FluentAssertions;
using MyCouch.IntegrationTests.TestFixtures;
using MyCouch.Testing;
using MyCouch.Testing.Model;
using Xunit;

namespace MyCouch.IntegrationTests.ClientTests
{
    public class ViewsTests : ClientTestsOf<IViews>, IPreserveStatePerFixture, IUseFixture<ViewsFixture>
    {
        protected Artist[] Artists { get; set; }

        protected override void OnTestInit()
        {
            SUT = Client.Views;
        }

        public void SetFixture(ViewsFixture data)
        {
            Artists = data.Artists;
        }

        [Fact]
        public void When_IncludeDocs_and_no_value_is_returned_for_string_response_Then_the_included_docs_are_extracted()
        {
            var query = new ViewQuery(ClientTestData.Views.ArtistsNameNoValueViewId).Configure(cfg => cfg.IncludeDocs(true));

            var response = SUT.QueryAsync(query).Result;

            response.Should().BeSuccessfulGet(Artists.Length);
            for (var i = 0; i < response.RowCount; i++)
            {
                response.Rows[i].Value.Should().BeNull();
                CustomAsserts.AreValueEqual(Artists[i], Client.Entities.Serializer.Deserialize<Artist>(response.Rows[i].Doc));
            }
        }

        [Fact]
        public void When_IncludeDocs_and_no_value_is_returned_for_entity_response_Then_the_included_docs_are_extracted()
        {
            var query = new ViewQuery(ClientTestData.Views.ArtistsNameNoValueViewId).Configure(cfg => cfg.IncludeDocs(true));

            var response = SUT.QueryAsync<Artist>(query).Result;

            response.Should().BeSuccessfulGet(Artists.Length);
            for (var i = 0; i < response.RowCount; i++)
            {
                response.Rows[i].Value.Should().BeNull();
                CustomAsserts.AreValueEqual(Artists[i], response.Rows[i].Doc);
            }
        }

        [Fact]
        public void When_IncludeDocs_and_no_value_is_returned_but_non_array_doc_is_included_Then_the_included_docs_are_not_extracted()
        {
            var query = new ViewQuery(ClientTestData.Views.ArtistsNameNoValueViewId).Configure(cfg => cfg.IncludeDocs(true));

            var response = SUT.QueryAsync<string[]>(query).Result;

            response.Should().BeSuccessfulGet(Artists.Length);
            for (var i = 0; i < response.RowCount; i++)
            {
                response.Rows[i].Value.Should().BeNull();
                response.Rows[i].Doc.Should().BeNull();
            }
        }

        [Fact]
        public void When_Skipping_2_of_10_using_json_Then_8_rows_are_returned()
        {
            var artists = Artists.Skip(2);
            var query = new ViewQuery(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Skip(2));

            var response = SUT.QueryAsync(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        }

        [Fact]
        public void When_Skipping_2_of_10_using_json_array_Then_8_rows_are_returned()
        {
            var artists = Artists.Skip(2);
            var query = new ViewQuery(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Skip(2));

            var response = SUT.QueryAsync<string[]>(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [Fact]
        public void When_Skipping_2_of_10_using_entities_Then_8_rows_are_returned()
        {
            var artists = Artists.Skip(2);
            var query = new ViewQuery(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Skip(2));

            var response = SUT.QueryAsync<Album[]>(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums).ToArray());
        }

        [Fact]
        public void When_Limit_to_2_using_json_Then_2_rows_are_returned()
        {
            var artists = Artists.Take(2);
            var query = new ViewQuery(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Limit(2));

            var response = SUT.QueryAsync(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        }

        [Fact]
        public void When_Limit_to_2_using_json_array_Then_2_rows_are_returned()
        {
            var artists = Artists.Take(2);
            var query = new ViewQuery(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Limit(2));

            var response = SUT.QueryAsync<string[]>(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [Fact]
        public void When_Limit_to_2_using_entities_Then_2_rows_are_returned()
        {
            var artists = Artists.Take(2);
            var query = new ViewQuery(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Limit(2));

            var response = SUT.QueryAsync<Album[]>(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums).ToArray());
        }

        [Fact]
        public void When_Key_is_specified_using_json_Then_matching_row_is_returned()
        {
            var artist = Artists[2];
            var query = new ViewQuery(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Key(artist.Name));

            var response = SUT.QueryAsync(query).Result;

            response.Should().BeSuccessfulGet(new[] { Client.Serializer.Serialize(artist.Albums) });
        }

        [Fact]
        public void When_Key_is_specified_using_json_array_Then_matching_row_is_returned()
        {
            var artist = Artists[2];
            var query = new ViewQuery(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Key(artist.Name));

            var response = SUT.QueryAsync<string[]>(query).Result;

            response.Should().BeSuccessfulGet(new[] { artist.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray() });
        }

        [Fact]
        public void When_Key_is_specified_using_entities_Then_matching_row_is_returned()
        {
            var artist = Artists[2];
            var query = new ViewQuery(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Key(artist.Name));

            var response = SUT.QueryAsync<Album[]>(query).Result;

            response.Should().BeSuccessfulGet(new[] { artist.Albums });
        }

        [Fact]
        public void When_Keys_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new ViewQuery(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Keys(keys));

            var response = SUT.QueryAsync(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        }

        [Fact]
        public void When_Keys_are_specified_using_json_array_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new ViewQuery(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Keys(keys));

            var response = SUT.QueryAsync<string[]>(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [Fact]
        public void When_Keys_are_specified_using_entities_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new ViewQuery(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Keys(keys));

            var response = SUT.QueryAsync<Album[]>(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums).ToArray());
        }

        [Fact]
        public void When_StartKey_and_EndKey_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(5).ToArray();
            var query = new ViewQuery(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name));

            var response = SUT.QueryAsync(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        }

        [Fact]
        public void When_StartKey_and_EndKey_are_specified_using_json_array_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(5).ToArray();
            var query = new ViewQuery(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name));

            var response = SUT.QueryAsync<string[]>(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [Fact]
        public void When_StartKey_and_EndKey_are_specified_using_entities_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(5).ToArray();
            var query = new ViewQuery(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name));

            var response = SUT.QueryAsync<Album[]>(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums).ToArray());
        }

        [Fact]
        public void When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(5).ToArray();
            var query = new ViewQuery(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name)
                .InclusiveEnd(false));

            var response = SUT.QueryAsync(query).Result;

            response.Should().BeSuccessfulGet(artists.Take(artists.Length - 1).Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        }

        [Fact]
        public void When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_json_array_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(5).ToArray();
            var query = new ViewQuery(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name)
                .InclusiveEnd(false));

            var response = SUT.QueryAsync<string[]>(query).Result;

            response.Should().BeSuccessfulGet(artists.Take(artists.Length - 1).Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [Fact]
        public void When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_entities_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(5).ToArray();
            var query = new ViewQuery(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name)
                .InclusiveEnd(false));

            var response = SUT.QueryAsync<Album[]>(query).Result;

            response.Should().BeSuccessfulGet(artists.Take(artists.Length - 1).Select(a => a.Albums).ToArray());
        }
    }
}