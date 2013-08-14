using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyCouch.Querying;
using MyCouch.Testing;
using MyCouch.Testing.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyCouch.IntegrationTests.ClientTests
{
    [TestClass]
    public class ViewsTests : IntegrationTestsOf<IViews>, IDisposable
    {
        protected Artist[] Artists;

        public ViewsTests()
        {
            OnTestInitialize = () => SUT = Client.Views;

            Artists = TestData.Artists.CreateArtists(10);

            var tasks = new List<Task>();
            tasks.AddRange(Artists.Select(item => Client.Entities.PostAsync(item)));
            tasks.Add(Client.Documents.PostAsync(TestData.Views.ArtistsAlbums));

            Task.WaitAll(tasks.ToArray());
        }

        public virtual void Dispose()
        {
            IntegrationTestsRuntime.ClearAllDocuments();   
        }

        [TestMethod]
        public void When_Skipping_2_of_10_using_json_Then_8_rows_are_returned()
        {
            var artists = Artists.Skip(2);
            var query = new ViewQuery("artists", "albums").Configure(cfg => cfg.Skip(2));

            var response = SUT.RunQuery(query);

            response.Should().BeSuccessfulGet(artists.Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        }

        [TestMethod]
        public void When_Skipping_2_of_10_using_json_array_Then_8_rows_are_returned()
        {
            var artists = Artists.Skip(2);
            var query = new ViewQuery("artists", "albums").Configure(cfg => cfg.Skip(2));

            var response = SUT.RunQuery<string[]>(query);

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [TestMethod]
        public void When_Skipping_2_of_10_using_entities_Then_8_rows_are_returned()
        {
            var artists = Artists.Skip(2);
            var query = new ViewQuery("artists", "albums").Configure(cfg => cfg.Skip(2));

            var response = SUT.RunQuery<Album[]>(query);

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums).ToArray());
        }

        [TestMethod]
        public void When_Limit_to_2_using_json_Then_2_rows_are_returned()
        {
            var artists = Artists.Take(2);
            var query = new ViewQuery("artists", "albums").Configure(cfg => cfg.Limit(2));

            var response = SUT.RunQuery(query);

            response.Should().BeSuccessfulGet(artists.Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        }

        [TestMethod]
        public void When_Limit_to_2_using_json_array_Then_2_rows_are_returned()
        {
            var artists = Artists.Take(2);
            var query = new ViewQuery("artists", "albums").Configure(cfg => cfg.Limit(2));

            var response = SUT.RunQuery<string[]>(query);

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [TestMethod]
        public void When_Limit_to_2_using_entities_Then_2_rows_are_returned()
        {
            var artists = Artists.Take(2);
            var query = new ViewQuery("artists", "albums").Configure(cfg => cfg.Limit(2));
            
            var response = SUT.RunQuery<Album[]>(query);

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums).ToArray());
        }

        [TestMethod]
        public void When_Key_is_specified_using_json_Then_matching_row_is_returned()
        {
            var artist = Artists[2];
            var query = new ViewQuery("artists", "albums").Configure(cfg => cfg.Key(artist.Name));

            var response = SUT.RunQuery(query);

            response.Should().BeSuccessfulGet(new[] { Client.Serializer.Serialize(artist.Albums) });
        }

        [TestMethod]
        public void When_Key_is_specified_using_json_array_Then_matching_row_is_returned()
        {
            var artist = Artists[2];
            var query = new ViewQuery("artists", "albums").Configure(cfg => cfg.Key(artist.Name));

            var response = SUT.RunQuery<string[]>(query);

            response.Should().BeSuccessfulGet(new[] { artist.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray() });
        }

        [TestMethod]
        public void When_Key_is_specified_using_entities_Then_matching_row_is_returned()
        {
            var artist = Artists[2];
            var query = new ViewQuery("artists", "albums").Configure(cfg => cfg.Key(artist.Name));

            var response = SUT.RunQuery<Album[]>(query);

            response.Should().BeSuccessfulGet(new [] { artist.Albums });
        }

        [TestMethod]
        public void When_Keys_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new ViewQuery("artists", "albums").Configure(cfg => cfg.Keys(keys));

            var response = SUT.RunQuery(query);

            response.Should().BeSuccessfulGet(artists.Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        }

        [TestMethod]
        public void When_Keys_are_specified_using_json_array_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new ViewQuery("artists", "albums").Configure(cfg => cfg.Keys(keys));

            var response = SUT.RunQuery<string[]>(query);

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [TestMethod]
        public void When_Keys_are_specified_using_entities_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var query = new ViewQuery("artists", "albums").Configure(cfg => cfg.Keys(keys));

            var response = SUT.RunQuery<Album[]>(query);

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums).ToArray());
        }

        [TestMethod]
        public void When_StartKey_and_EndKey_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(5).ToArray();
            var query = new ViewQuery("artists", "albums").Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name));

            var response = SUT.RunQuery(query);

            response.Should().BeSuccessfulGet(artists.Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        }

        [TestMethod]
        public void When_StartKey_and_EndKey_are_specified_using_json_array_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(5).ToArray();
            var query = new ViewQuery("artists", "albums").Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name));

            var response = SUT.RunQuery<string[]>(query);

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        }

        [TestMethod]
        public void When_StartKey_and_EndKey_are_specified_using_entities_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(5).ToArray();
            var query = new ViewQuery("artists", "albums").Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name));

            var response = SUT.RunQuery<Album[]>(query);

            response.Should().BeSuccessfulGet(artists.Select(a => a.Albums).ToArray());
        }

        [TestMethod]
        public void When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_json_Then_matching_rows_are_returned()
        {
            var artists = Artists.Skip(2).Take(5).ToArray();
            var query = new ViewQuery("artists", "albums").Configure(cfg => cfg
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
            var query = new ViewQuery("artists", "albums").Configure(cfg => cfg
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
            var query = new ViewQuery("artists", "albums").Configure(cfg => cfg
                .StartKey(artists.First().Name)
                .EndKey(artists.Last().Name)
                .InclusiveEnd(false));

            var response = SUT.RunQuery<Album[]>(query);

            response.Should().BeSuccessfulGet(artists.Take(artists.Length - 1).Select(a => a.Albums).ToArray());
        }
    }
}