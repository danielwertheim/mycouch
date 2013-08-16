using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyCouch.Querying;
using MyCouch.Testing;
using MyCouch.Testing.Model;
#if !NETFX_CORE
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif
using MyCouch.Extensions;

namespace MyCouch.IntegrationTests.ClientTests
{
    [TestClass]
    public class ViewsTests : IntegrationTestsOf<IViews>
    {
        protected static readonly Artist[] Artists;

        static ViewsTests()
        {
            Artists = TestData.Artists.CreateArtists(10);
        }

        public ViewsTests()
        {
            OnTestInitialize = () => SUT = Client.Views;
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var tasks = new List<Task>();
            tasks.AddRange(Artists.Select(item => IntegrationTestsRuntime.Client.Entities.PostAsync(item)));
            Task.WaitAll(tasks.ToArray());

            tasks.Clear();
            tasks.Add(IntegrationTestsRuntime.Client.Documents.PostAsync(TestData.Views.Artists));
            Task.WaitAll(tasks.ToArray());

            var touchView1 = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(q => q.Stale(Stale.UpdateAfter));
            var touchView2 = new ViewQuery(TestData.Views.ArtistsNamesNoValueViewId).Configure(q => q.Stale(Stale.UpdateAfter));
            IntegrationTestsRuntime.Client.Views.RunQuery(touchView1);
            IntegrationTestsRuntime.Client.Views.RunQuery(touchView2);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            IntegrationTestsRuntime.ClearAllDocuments();
        }

        [TestMethod]
        public void When_IncludeDocs_and_no_value_is_returned_for_string_response_Then_the_included_docs_are_extracted()
        {
            var query = new ViewQuery(TestData.Views.ArtistsNamesNoValueViewId).Configure(cfg => cfg.IncludeDocs(true));

            var response = SUT.RunQuery(query);

            response.Should().BeSuccessfulGet(Artists.Length);
            for (var i = 0; i < response.RowCount; i++)
            {
                Assert.IsNull(response.Rows[i].Value);
                CustomAsserts.AreValueEqual(Artists[i], Client.Serializer.Deserialize<Artist>(response.Rows[i].Doc));
            }
        }

        [TestMethod]
        public void When_IncludeDocs_and_no_value_is_returned_but_non_array_doc_is_included_Then_the_included_docs_are_not_extracted()
        {
            var query = new ViewQuery(TestData.Views.ArtistsNamesNoValueViewId).Configure(cfg => cfg.IncludeDocs(true));

            var response = SUT.RunQuery<string[]>(query);

            response.Should().BeSuccessfulGet(Artists.Length);
            for (var i = 0; i < response.RowCount; i++)
            {
                Assert.IsNull(response.Rows[i].Value);
                Assert.IsNull(response.Rows[i].Doc);
            }
        }

        [TestMethod]
        public void When_Skipping_2_of_10_using_json_Then_8_rows_are_returned()
        {
            var artists = Artists.Skip(2);
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Skip(2));

            var response = SUT.RunQuery(query);

            response.Should().BeSuccessfulGet(artists.Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        }

        [TestMethod]
        public void When_Skipping_2_of_10_using_json_array_Then_8_rows_are_returned()
        {
            var artists = Artists.Skip(2);
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Skip(2));

            var response = SUT.RunQuery<string[]>(query);

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [TestMethod]
        public void When_Skipping_2_of_10_using_entities_Then_8_rows_are_returned()
        {
            var artists = Artists.Skip(2);
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Skip(2));

            var response = SUT.RunQuery<Album[]>(query);

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums).ToArray());
        }

        [TestMethod]
        public void When_Limit_to_2_using_json_Then_2_rows_are_returned()
        {
            var artists = Artists.Take(2);
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Limit(2));

            var response = SUT.RunQuery(query);

            response.Should().BeSuccessfulGet(artists.Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        }

        [TestMethod]
        public void When_Limit_to_2_using_json_array_Then_2_rows_are_returned()
        {
            var artists = Artists.Take(2);
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Limit(2));

            var response = SUT.RunQuery<string[]>(query);

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [TestMethod]
        public void When_Limit_to_2_using_entities_Then_2_rows_are_returned()
        {
            var artists = Artists.Take(2);
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Limit(2));
            
            var response = SUT.RunQuery<Album[]>(query);

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums).ToArray());
        }

        [TestMethod]
        public void When_Key_is_specified_using_json_Then_matching_row_is_returned()
        {
            var artist = Artists[2];
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Key(artist.Name));

            var response = SUT.RunQuery(query);

            response.Should().BeSuccessfulGet(new[] { Client.Serializer.Serialize(artist.Albums) });
        }

        [TestMethod]
        public void When_Key_is_specified_using_json_array_Then_matching_row_is_returned()
        {
            var artist = Artists[2];
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Key(artist.Name));

            var response = SUT.RunQuery<string[]>(query);

            response.Should().BeSuccessfulGet(new[] { artist.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray() });
        }

        [TestMethod]
        public void When_Key_is_specified_using_entities_Then_matching_row_is_returned()
        {
            var artist = Artists[2];
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Key(artist.Name));

            var response = SUT.RunQuery<Album[]>(query);

            response.Should().BeSuccessfulGet(new [] { artist.Albums });
        }

        [TestMethod]
        public void When_Keys_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Keys(keys));

            var response = SUT.RunQuery(query);

            response.Should().BeSuccessfulGet(artists.Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        }

        [TestMethod]
        public void When_Keys_are_specified_using_json_array_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Keys(keys));

            var response = SUT.RunQuery<string[]>(query);

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [TestMethod]
        public void When_Keys_are_specified_using_entities_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Keys(keys));

            var response = SUT.RunQuery<Album[]>(query);

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums).ToArray());
        }

        [TestMethod]
        public void When_StartKey_and_EndKey_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(5).ToArray();
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name));

            var response = SUT.RunQuery(query);

            response.Should().BeSuccessfulGet(artists.Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        }

        [TestMethod]
        public void When_StartKey_and_EndKey_are_specified_using_json_array_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(5).ToArray();
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name));

            var response = SUT.RunQuery<string[]>(query);

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [TestMethod]
        public void When_StartKey_and_EndKey_are_specified_using_entities_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(5).ToArray();
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name));

            var response = SUT.RunQuery<Album[]>(query);

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums).ToArray());
        }

        [TestMethod]
        public void When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(5).ToArray();
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name)
                .InclusiveEnd(false));

            var response = SUT.RunQuery(query);

            response.Should().BeSuccessfulGet(artists.Take(artists.Length - 1).Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        }

        [TestMethod]
        public void When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_json_array_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(5).ToArray();
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name)
                .InclusiveEnd(false));

            var response = SUT.RunQuery<string[]>(query);

            response.Should().BeSuccessfulGet(artists.Take(artists.Length - 1).Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [TestMethod]
        public void When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_entities_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(5).ToArray();
            var query = new ViewQuery(TestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name)
                .InclusiveEnd(false));

            var response = SUT.RunQuery<Album[]>(query);

            response.Should().BeSuccessfulGet(artists.Take(artists.Length - 1).Select(a => a.Albums).ToArray());
        }
    }
}