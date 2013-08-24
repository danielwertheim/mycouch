using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MyCouch.Querying;
using MyCouch.Testing;
using MyCouch.Testing.Model;
using Xunit;

namespace MyCouch.IntegrationTests.ClientTests
{
    public class ViewsTests : ClientTestsOf<IViews>, IPreserveStatePerFixture, IUseFixture<ViewsTests.ViewsFixture>
    {
        protected Artist[] Artists { get; set; }

        public ViewsTests()
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
            var query = new ViewQuery(TestData.Views.ArtistsNameNoValueViewId).Configure(cfg => cfg.IncludeDocs(true));

            var response = SUT.RunQueryAsync(query).Result;

            response.Should().BeSuccessfulGet(Artists.Length);
            for (var i = 0; i < response.RowCount; i++)
            {
                response.Rows[i].Value.Should().BeNull();
                response.Rows[i].Doc.Should().NotBeNullOrEmpty();
            }
        }

        [Fact]
        public void When_IncludeDocs_and_no_value_is_returned_for_entity_response_Then_the_included_docs_are_extracted()
        {
            var query = new ViewQuery(TestData.Views.ArtistsNameNoValueViewId).Configure(cfg => cfg.IncludeDocs(true));

            var response = SUT.RunQueryAsync<Artist>(query).Result;

            response.Should().BeSuccessfulGet(Artists.Length);
            for (var i = 0; i < response.RowCount; i++)
            {
                response.Rows[i].Value.Should().BeNull();
                response.Rows[i].Doc.Should().NotBeNull();
            }
        }

        [Fact]
        public void When_IncludeDocs_and_no_value_is_returned_but_non_array_doc_is_included_Then_the_included_docs_are_not_extracted()
        {
            var query = new ViewQuery(TestData.Views.ArtistsNameNoValueViewId).Configure(cfg => cfg.IncludeDocs(true));

            var response = SUT.RunQueryAsync<string[]>(query).Result;

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
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Skip(2));

            var response = SUT.RunQueryAsync(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        }

        [Fact]
        public void When_Skipping_2_of_10_using_json_array_Then_8_rows_are_returned()
        {
            var artists = Artists.Skip(2);
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Skip(2));

            var response = SUT.RunQueryAsync<string[]>(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [Fact]
        public void When_Skipping_2_of_10_using_entities_Then_8_rows_are_returned()
        {
            var artists = Artists.Skip(2);
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Skip(2));

            var response = SUT.RunQueryAsync<Album[]>(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums).ToArray());
        }

        [Fact]
        public void When_Limit_to_2_using_json_Then_2_rows_are_returned()
        {
            var artists = Artists.Take(2);
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Limit(2));

            var response = SUT.RunQueryAsync(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        }

        [Fact]
        public void When_Limit_to_2_using_json_array_Then_2_rows_are_returned()
        {
            var artists = Artists.Take(2);
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Limit(2));

            var response = SUT.RunQueryAsync<string[]>(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [Fact]
        public void When_Limit_to_2_using_entities_Then_2_rows_are_returned()
        {
            var artists = Artists.Take(2);
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Limit(2));

            var response = SUT.RunQueryAsync<Album[]>(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums).ToArray());
        }

        [Fact]
        public void When_Key_is_specified_using_json_Then_matching_row_is_returned()
        {
            var artist = Artists[2];
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Key(artist.Name));

            var response = SUT.RunQueryAsync(query).Result;

            response.Should().BeSuccessfulGet(new[] { Client.Serializer.Serialize(artist.Albums) });
        }

        [Fact]
        public void When_Key_is_specified_using_json_array_Then_matching_row_is_returned()
        {
            var artist = Artists[2];
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Key(artist.Name));

            var response = SUT.RunQueryAsync<string[]>(query).Result;

            response.Should().BeSuccessfulGet(new[] { artist.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray() });
        }

        [Fact]
        public void When_Key_is_specified_using_entities_Then_matching_row_is_returned()
        {
            var artist = Artists[2];
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Key(artist.Name));

            var response = SUT.RunQueryAsync<Album[]>(query).Result;

            response.Should().BeSuccessfulGet(new[] { artist.Albums });
        }

        [Fact]
        public void When_Keys_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Keys(keys));

            var response = SUT.RunQueryAsync(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        }

        [Fact]
        public void When_Keys_are_specified_using_json_array_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Keys(keys));

            var response = SUT.RunQueryAsync<string[]>(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [Fact]
        public void When_Keys_are_specified_using_entities_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Keys(keys));

            var response = SUT.RunQueryAsync<Album[]>(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums).ToArray());
        }

        [Fact]
        public void When_StartKey_and_EndKey_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(5).ToArray();
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name));

            var response = SUT.RunQueryAsync(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        }

        [Fact]
        public void When_StartKey_and_EndKey_are_specified_using_json_array_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(5).ToArray();
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name));

            var response = SUT.RunQueryAsync<string[]>(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [Fact]
        public void When_StartKey_and_EndKey_are_specified_using_entities_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(5).ToArray();
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name));

            var response = SUT.RunQueryAsync<Album[]>(query).Result;

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums).ToArray());
        }

        [Fact]
        public void When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(5).ToArray();
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name)
                .InclusiveEnd(false));

            var response = SUT.RunQueryAsync(query).Result;

            response.Should().BeSuccessfulGet(artists.Take(artists.Length - 1).Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        }

        [Fact]
        public void When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_json_array_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(5).ToArray();
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name)
                .InclusiveEnd(false));

            var response = SUT.RunQueryAsync<string[]>(query).Result;

            response.Should().BeSuccessfulGet(artists.Take(artists.Length - 1).Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [Fact]
        public void When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_entities_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(5).ToArray();
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name)
                .InclusiveEnd(false));

            var response = SUT.RunQueryAsync<Album[]>(query).Result;

            response.Should().BeSuccessfulGet(artists.Take(artists.Length - 1).Select(a => a.Albums).ToArray());
        }

        public class ViewsFixture : IDisposable
        {
            public Artist[] Artists { get; protected set; }

            public ViewsFixture()
            {
                using (var client = IntegrationTestsRuntime.CreateRichClient())
                {
                    Artists = TestData.Artists.CreateArtists(10);

                    var tasks = new List<Task>();
                    tasks.AddRange(Artists.Select(item => client.Entities.PostAsync(item)));
                    Task.WaitAll(tasks.ToArray());

                    tasks.Clear();
                    tasks.Add(client.Documents.PostAsync(TestData.Views.Artists));
                    Task.WaitAll(tasks.ToArray());

                    var touchView1 = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(q => q.Stale(Stale.UpdateAfter));
                    var touchView2 = new ViewQuery(TestData.Views.ArtistsNameNoValueViewId).Configure(q => q.Stale(Stale.UpdateAfter));

                    Task.WaitAll(
                        client.Views.RunQueryAsync(touchView1),
                        client.Views.RunQueryAsync(touchView2));
                }
            }

            public virtual void Dispose()
            {
                using (var client = IntegrationTestsRuntime.CreateClient())
                {
                    client.ClearAllDocuments();
                }
            }
        }
    }
}