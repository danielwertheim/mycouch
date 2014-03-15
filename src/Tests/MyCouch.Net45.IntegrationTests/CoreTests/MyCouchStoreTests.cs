using System.Linq;
using System.Reactive.Linq;
using FluentAssertions;
using MyCouch.IntegrationTests.TestFixtures;
using MyCouch.Testing;
using MyCouch.Testing.Model;
using MyCouch.Testing.TestData;
using Xunit;

namespace MyCouch.IntegrationTests.CoreTests
{
    public class MyCouchStoreTests : ClientTestsOf<MyCouchStore>
    {
        public MyCouchStoreTests()
        {
            SUT = new MyCouchStore(Client);
        }

        [Fact]
        public virtual void FlowTestOfJson()
        {
            var storeJson = SUT.StoreAsync(ClientTestData.Artists.Artist1Json);
            storeJson.Result.Id.Should().Be(ClientTestData.Artists.Artist1Id);
            storeJson.Result.Rev.Should().NotBeNullOrWhiteSpace();
            
            var getJsonById = SUT.GetByIdAsync(storeJson.Result.Id);
            getJsonById.Result.Should().NotBeNullOrWhiteSpace();

            var getJsonByIdAndRev = SUT.GetByIdAsync(storeJson.Result.Id, storeJson.Result.Rev);
            getJsonByIdAndRev.Result.Should().NotBeNullOrWhiteSpace();

            var deleteByIdAndRev = SUT.DeleteAsync(storeJson.Result.Id, storeJson.Result.Rev);
            deleteByIdAndRev.Result.Should().BeTrue();
        }

        [Fact]
        public virtual void FlowTestOfEntities()
        {
            var storeEntity = SUT.StoreAsync(ClientTestData.Artists.Artist2);

            storeEntity.Result.ArtistId.Should().Be(ClientTestData.Artists.Artist2Id);
            storeEntity.Result.ArtistRev.Should().NotBeNullOrWhiteSpace();

            var getEntityById = SUT.GetByIdAsync<Artist>(storeEntity.Result.ArtistId);
            getEntityById.Result.Should().NotBeNull();
            getEntityById.Result.ArtistId.Should().Be(ClientTestData.Artists.Artist2Id);
            getEntityById.Result.ArtistRev.Should().Be(storeEntity.Result.ArtistRev);

            var getEntityByIdAndRev = SUT.GetByIdAsync<Artist>(storeEntity.Result.ArtistId, storeEntity.Result.ArtistRev);
            getEntityByIdAndRev.Result.Should().NotBeNull();
            getEntityByIdAndRev.Result.ArtistId.Should().Be(ClientTestData.Artists.Artist2Id);
            getEntityByIdAndRev.Result.ArtistRev.Should().Be(storeEntity.Result.ArtistRev);

            var deleteByEntity = SUT.DeleteAsync(getEntityById.Result);
            deleteByEntity.Result.Should().BeTrue();
        }
    }

    public class ObservableQueryTests :
        ClientTestsOf<MyCouchStore>,
        IPreserveStatePerFixture,
        IUseFixture<ViewsFixture>
    {
        protected Artist[] Artists { get; set; }

        public ObservableQueryTests()
        {
            SUT = new MyCouchStore(Client);
        }

        public void SetFixture(ViewsFixture data)
        {
            data.Init(Environment);
            Artists = data.Artists;
        }

        [Fact]
        public void When_no_key_with_sum_reduce_for_string_response_It_will_be_able_to_sum()
        {
            var expectedSum = Artists.Sum(a => a.Albums.Count());
            var query = new Query(ClientTestData.Views.ArtistsTotalNumOfAlbumsViewId).Reduce(true);

            SUT.ObserveQuery(query).ForEachAsync((row, i) =>
            {
                i.Should().Be(0);
                row.Value.Should().Be(expectedSum.ToString(MyCouchRuntime.NumberFormat));
            });
        }

        [Fact]
        public void When_no_key_with_sum_reduce_for_dynamic_response_It_will_be_able_to_sum()
        {
            var expectedSum = Artists.Sum(a => a.Albums.Count());
            var query = new Query(ClientTestData.Views.ArtistsTotalNumOfAlbumsViewId).Reduce(true);

            SUT.ObserveQuery<dynamic>(query).ForEachAsync((row, i) =>
            {
                i.Should().Be(0);
                ((long)row.Value).Should().Be(expectedSum);
            });
        }

        [Fact]
        public void When_no_key_with_sum_reduce_for_typed_response_It_will_be_able_to_sum()
        {
            var expectedSum = Artists.Sum(a => a.Albums.Count());
            var query = new Query(ClientTestData.Views.ArtistsTotalNumOfAlbumsViewId).Reduce(true);

            SUT.ObserveQuery<int>(query).ForEachAsync((row, i) =>
            {
                i.Should().Be(0);
                row.Value.Should().Be(expectedSum);
            });
        }

        [Fact]
        public void When_IncludeDocs_and_no_value_is_returned_for_string_response_Then_the_included_docs_are_extracted()
        {
            var query = new Query(ClientTestData.Views.ArtistsNameNoValueViewId).IncludeDocs(true);

            var len = 0;
            SUT.ObserveQuery(query).ForEachAsync((row, i) =>
            {
                len++;
                CustomAsserts.AreValueEqual(Artists[i], Client.Entities.Serializer.Deserialize<Artist>(row.IncludedDoc));
            }).ContinueWith(t => len.Should().Be(Artists.Length));
        }

        [Fact]
        public void When_IncludeDocs_and_no_value_is_returned_for_entity_response_Then_the_included_docs_are_extracted()
        {
            var query = new Query(ClientTestData.Views.ArtistsNameNoValueViewId).IncludeDocs(true);

            var len = 0;
            SUT.ObserveQuery<string, Artist>(query).ForEachAsync((row, i) =>
            {
                len++;
                CustomAsserts.AreValueEqual(Artists[i], row.IncludedDoc);
            }).ContinueWith(t => len.Should().Be(Artists.Length));
        }

        [Fact]
        public void When_IncludeDocs_of_non_array_doc_and_null_value_is_returned_Then_the_neither_included_docs_nor_value_is_extracted()
        {
            var query = new Query(ClientTestData.Views.ArtistsNameNoValueViewId).IncludeDocs(true);

            var len = 0;
            SUT.ObserveQuery<string[], string[]>(query).ForEachAsync((row, i) =>
            {
                len++;
                row.Value.Should().BeNull();
                row.IncludedDoc.Should().BeNull();
            }).ContinueWith(t => len.Should().Be(Artists.Length));
        }

        //[Fact]
        //public void When_Skipping_2_of_10_using_json_Then_8_rows_are_returned()
        //{
        //    var artists = Artists.Skip(2);
        //    var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Skip(2));

        //    var response = SUT.QueryAsync(query).Result;

        //    response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
        //    response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        //}

        //[Fact]
        //public void When_Skipping_2_of_10_using_json_array_Then_8_rows_are_returned()
        //{
        //    var artists = Artists.Skip(2);
        //    var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Skip(2));

        //    var response = SUT.QueryAsync<string[]>(query).Result;

        //    response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
        //    response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        //}

        //[Fact]
        //public void When_Skipping_2_of_10_using_entities_Then_8_rows_are_returned()
        //{
        //    var artists = Artists.Skip(2);
        //    var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Skip(2));

        //    var response = SUT.QueryAsync<Album[]>(query).Result;

        //    response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
        //    response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Select(a => a.Albums).ToArray());
        //}

        //[Fact]
        //public void When_Limit_to_2_using_json_Then_2_rows_are_returned()
        //{
        //    var artists = Artists.Take(2);
        //    var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Limit(2));

        //    var response = SUT.QueryAsync(query).Result;

        //    response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
        //    response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        //}

        //[Fact]
        //public void When_Limit_to_2_using_json_array_Then_2_rows_are_returned()
        //{
        //    var artists = Artists.Take(2);
        //    var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Limit(2));

        //    var response = SUT.QueryAsync<string[]>(query).Result;

        //    response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
        //    response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        //}

        //[Fact]
        //public void When_Limit_to_2_using_entities_Then_2_rows_are_returned()
        //{
        //    var artists = Artists.Take(2);
        //    var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Limit(2));

        //    var response = SUT.QueryAsync<Album[]>(query).Result;

        //    response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
        //    response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Select(a => a.Albums).ToArray());
        //}

        //[Fact]
        //public void When_Key_is_specified_using_json_Then_the_matching_row_is_returned()
        //{
        //    var artist = Artists[2];
        //    var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Key(artist.Name));

        //    var response = SUT.QueryAsync(query).Result;

        //    response.Should().BeSuccessfulGet(new[] { Client.Serializer.Serialize(artist.Albums) });
        //}

        //[Fact]
        //public void When_Key_is_specified_using_json_array_Then_the_matching_row_is_returned()
        //{
        //    var artist = Artists[2];
        //    var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Key(artist.Name));

        //    var response = SUT.QueryAsync<string[]>(query).Result;

        //    response.Should().BeSuccessfulGet(new[] { artist.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray() });
        //}

        //[Fact]
        //public void When_Key_is_specified_using_entities_Then_the_matching_row_is_returned()
        //{
        //    var artist = Artists[2];
        //    var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Key(artist.Name));

        //    var response = SUT.QueryAsync<Album[]>(query).Result;

        //    response.Should().BeSuccessfulGet(new[] { artist.Albums });
        //}

        //[Fact]
        //public void When_Keys_are_specified_using_json_Then_matching_rows_are_returned()
        //{
        //    var artists = Artists.Skip(2).Take(3).ToArray();
        //    var keys = artists.Select(a => a.Name).ToArray();
        //    var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Keys(keys));

        //    var response = SUT.QueryAsync(query).Result;

        //    response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
        //    response.Should().BeSuccessfulPost(artists.OrderBy(a => a.ArtistId).Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        //}

        //[Fact]
        //public void When_Keys_are_specified_using_json_array_Then_matching_rows_are_returned()
        //{
        //    var artists = Artists.Skip(2).Take(3).ToArray();
        //    var keys = artists.Select(a => a.Name).ToArray();
        //    var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Keys(keys));

        //    var response = SUT.QueryAsync<string[]>(query).Result;

        //    response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
        //    response.Should().BeSuccessfulPost(artists.OrderBy(a => a.ArtistId).Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        //}

        //[Fact]
        //public void When_Keys_are_specified_using_entities_Then_matching_rows_are_returned()
        //{
        //    var artists = Artists.Skip(2).Take(3).ToArray();
        //    var keys = artists.Select(a => a.Name).ToArray();
        //    var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg.Keys(keys));

        //    var response = SUT.QueryAsync<Album[]>(query).Result;

        //    response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
        //    response.Should().BeSuccessfulPost(artists.OrderBy(a => a.ArtistId).Select(a => a.Albums).ToArray());
        //}

        //[Fact]
        //public void When_StartKey_and_EndKey_are_specified_using_json_Then_matching_rows_are_returned()
        //{
        //    var artists = Artists.Skip(2).Take(5).ToArray();
        //    var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
        //        .StartKey(artists.First().Name)
        //        .EndKey(artists.Last().Name));

        //    var response = SUT.QueryAsync(query).Result;

        //    response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
        //    response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        //}

        //[Fact]
        //public void When_StartKey_and_EndKey_are_specified_using_json_array_Then_matching_rows_are_returned()
        //{
        //    var artists = Artists.Skip(2).Take(5).ToArray();
        //    var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
        //        .StartKey(artists.First().Name)
        //        .EndKey(artists.Last().Name));

        //    var response = SUT.QueryAsync<string[]>(query).Result;

        //    response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
        //    response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        //}

        //[Fact]
        //public void When_StartKey_and_EndKey_are_specified_using_entities_Then_matching_rows_are_returned()
        //{
        //    var artists = Artists.Skip(2).Take(5).ToArray();
        //    var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
        //        .StartKey(artists.First().Name)
        //        .EndKey(artists.Last().Name));

        //    var response = SUT.QueryAsync<Album[]>(query).Result;

        //    response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
        //    response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Select(a => a.Albums).ToArray());
        //}

        //[Fact]
        //public void When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_json_Then_matching_rows_are_returned()
        //{
        //    var artists = Artists.Skip(2).Take(5).ToArray();
        //    var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
        //        .StartKey(artists.First().Name)
        //        .EndKey(artists.Last().Name)
        //        .InclusiveEnd(false));

        //    var response = SUT.QueryAsync(query).Result;

        //    response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
        //    response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Take(artists.Length - 1).Select(a => Client.Serializer.Serialize(a.Albums)).ToArray());
        //}

        //[Fact]
        //public void When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_json_array_Then_matching_rows_are_returned()
        //{
        //    var artists = Artists.Skip(2).Take(5).ToArray();
        //    var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
        //        .StartKey(artists.First().Name)
        //        .EndKey(artists.Last().Name)
        //        .InclusiveEnd(false));

        //    var response = SUT.QueryAsync<string[]>(query).Result;

        //    response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
        //    response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Take(artists.Length - 1).Select(a => a.Albums.Select(i => Client.Serializer.Serialize(i)).ToArray()).ToArray());
        //}

        //[Fact]
        //public void When_StartKey_and_EndKey_with_non_inclusive_end_are_specified_using_entities_Then_matching_rows_are_returned()
        //{
        //    var artists = Artists.Skip(2).Take(5).ToArray();
        //    var query = new QueryViewRequest(ClientTestData.Views.ArtistsAlbumsViewId).Configure(cfg => cfg
        //        .StartKey(artists.First().Name)
        //        .EndKey(artists.Last().Name)
        //        .InclusiveEnd(false));

        //    var response = SUT.QueryAsync<Album[]>(query).Result;

        //    response.Rows = response.Rows.OrderBy(r => r.Id).ToArray();
        //    response.Should().BeSuccessfulGet(artists.OrderBy(a => a.ArtistId).Take(artists.Length - 1).Select(a => a.Albums).ToArray());
        //}

        //[Fact]
        //public void When_skip_two_of_ten_It_should_return_the_other_eight()
        //{
        //    var query = new QueryViewRequest(ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId).Configure(cfg => cfg
        //        .Skip(2));

        //    var response = SUT.QueryAsync<Artist>(query).Result;

        //    response.Should().BeSuccessfulGet(8);
        //}

        //[Fact]
        //public void When_limit_is_two_of_ten_It_should_return_two()
        //{
        //    var query = new QueryViewRequest(ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId).Configure(cfg => cfg
        //        .Limit(2));

        //    var response = SUT.QueryAsync<Artist>(query).Result;

        //    response.Should().BeSuccessfulGet(2);
        //}

        //[Fact]
        //public void When_getting_all_artists_It_can_deserialize_artists_properly()
        //{
        //    var query = new QueryViewRequest(ClientTestData.Views.ArtistsNameAsKeyAndDocAsValueId);

        //    var response = SUT.QueryAsync<Artist>(query).Result;

        //    response.Should().BeSuccessfulGet(
        //        Artists.OrderBy(a => a.ArtistId).ToArray(),
        //        i => i.Id);
        //}
    }
}