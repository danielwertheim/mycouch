using FluentAssertions;
using MyCouch.EntitySchemes;
using MyCouch.EntitySchemes.Reflections;
using Xunit;

namespace MyCouch.UnitTests.Schemes
{
    public class EntityIdMemberTestsWithLambdaPropertyFactoryTests : EntityIdMemberTests
    {
        public EntityIdMemberTestsWithLambdaPropertyFactoryTests()
        {
            SUT = new EntityIdMember(new LambdaDynamicPropertyFactory());
        }
    }
#if !NETFX_CORE
    public class EntityIdMemberTestsWithIlPropertyFactoryTests : EntityIdMemberTests
    {
        public EntityIdMemberTestsWithIlPropertyFactoryTests()
        {
            SUT = new EntityIdMember(new IlDynamicPropertyFactory());
        }
    }
#endif
    public abstract class EntityIdMemberTests : UnitTestsOf<EntityIdMember>
    {
        [Fact]
        public void Verify_MemberRanking()
        {
            var t = typeof (ModelForMemberRanking);

            SUT.GetMemberRankingIndex(t, "FDE29AC2-C452-4493-8D61-5349E2E5B5D5").Should().Be(null);
            SUT.GetMemberRankingIndex(t, "_Id").Should().Be(0);
            SUT.GetMemberRankingIndex(t, "ModelForMemberRankingId").Should().Be(1);
            SUT.GetMemberRankingIndex(t, "DocumentId").Should().Be(2);
            SUT.GetMemberRankingIndex(t, "EntityId").Should().Be(3);
            SUT.GetMemberRankingIndex(t, "Id").Should().Be(4);
        }

        [Fact]
        public void When_only_having_member_Id_It_extracts_the_id()
        {
            var model = new ModelOne { Id = "ModelOne:Id:1" };

            SUT.GetValueFrom(model).Should().Be(model.Id);
        }

        [Fact]
        public void When_having_members_Id_EntityId_It_extracts_EntityId()
        {
            var model = new ModelTwo
            {
                Id = "ModelTwo:Id:1", 
                EntityId = "ModelTwo:EntityId:2"
            };

            SUT.GetValueFrom(model).Should().Be(model.EntityId);
        }

        [Fact]
        public void When_having_members_Id_EntityId_DocumentId_It_extracts_DocumentId()
        {
            var model = new ModelThree
            {
                Id = "ModelThree:Id:1",
                EntityId = "ModelThree:EntityId:2",
                DocumentId = "ModelThree:DocumentId:3"
            };

            SUT.GetValueFrom(model).Should().Be(model.DocumentId);
        }

        [Fact]
        public void When_having_members_Id_EntityId_DocumentId_ModelId_It_extracts_ModelId()
        {
            var model = new ModelFour
            {
                Id = "ModelFour:Id:1",
                EntityId = "ModelFour:EntityId:2",
                DocumentId = "ModelFour:DocumentId:3",
                ModelFourId = "ModelFour:ModelFourId:4"
            };

            SUT.GetValueFrom(model).Should().Be(model.ModelFourId);
        }

        [Fact]
        public void When_having_members_Id_EntityId_DocumentId_ModelId__Id_It_extracts__Id()
        {
            var model = new ModelFive
            {
                Id = "ModelFive:Id:1",
                EntityId = "ModelFive:EntityId:2",
                DocumentId = "ModelFive:DocumentId:3",
                ModelFourId = "ModelFive:ModelFourId:4",
                ModelFiveId = "ModelFive:ModelFiveId:5",
                _Id = "ModelFive:_Id:6"
            };

            SUT.GetValueFrom(model).Should().Be(model._Id);
        }

        [Fact]
        public void When__id_is_defined_in_base_It_can_extract_the_id()
        {
            var model = new ModelInheritanceLowerCasedIdChild { _id = "ModelInheritanceLowerCasedIdChild:_id:1" };

            SUT.GetValueFrom(model).Should().Be(model._id);
        }

        [Fact]
        public void When__Id_is_defined_in_base_It_can_extract_the_id()
        {
            var model = new ModelInheritanceProperCasedIdChild { _Id = "ModelInheritanceProperCasedIdChild:_Id:1" };

            SUT.GetValueFrom(model).Should().Be(model._Id);
        }

        [Fact]
        public void When_DocumentId_is_defined_in_base_It_can_extract_the_id()
        {
            var model = new ModelInheritanceDocumentIdChild { DocumentId = "ModelInheritanceDocumentIdChild:DocumentId:1" };

            SUT.GetValueFrom(model).Should().Be(model.DocumentId);
        }

        private class ModelForMemberRanking
        {
            public string Id { get; set; }
            public string EntityId { get; set; }
            public string DocumentId { get; set; }
            public string ModelForMemberRankingId { get; set; }
            public string _Id { get; set; }
        }

        private class ModelOne
        {
            public string Id { get; set; }
        }

        private class ModelTwo : ModelOne
        {
            public string EntityId { get; set; }
        }

        private class ModelThree : ModelTwo
        {
            public string DocumentId { get; set; }
        }

        private class ModelFour : ModelThree
        {
            public string ModelFourId { get; set; }
        }

        private class ModelFive : ModelFour
        {
            public string _Id { get; set; }
            public string ModelFiveId { get; set; }
        }

        private class ModelInheritanceLowerCasedIdBase
        {
            public string _id { get; set; }
        }

        private class ModelInheritanceProperCasedIdBase
        {
            public string _Id { get; set; }
        }

        private class ModelInheritanceDocumentIdBase
        {
            public string DocumentId { get; set; }
        }

        private class ModelInheritanceLowerCasedIdChild : ModelInheritanceLowerCasedIdBase
        {
        }

        private class ModelInheritanceProperCasedIdChild : ModelInheritanceProperCasedIdBase
        {
        }

        private class ModelInheritanceDocumentIdChild : ModelInheritanceDocumentIdBase
        {
        }
    }
}