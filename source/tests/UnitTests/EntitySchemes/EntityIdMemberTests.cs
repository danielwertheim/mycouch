﻿using FluentAssertions;
using MyCouch.EntitySchemes;
using MyCouch.EntitySchemes.Reflections;
using Xunit;

namespace UnitTests.EntitySchemes
{
    public class EntityIdMemberTestsWithLambdaPropertyFactoryTests : EntityIdMemberTests
    {
        public EntityIdMemberTestsWithLambdaPropertyFactoryTests()
        {
            SUT = new EntityIdMember(new LambdaDynamicPropertyFactory());
        }
    }

    public class EntityIdMemberTestsWithIlPropertyFactoryTests : EntityIdMemberTests
    {
        public EntityIdMemberTestsWithIlPropertyFactoryTests()
        {
            SUT = new EntityIdMember(new IlDynamicPropertyFactory());
        }
    }

    public abstract class EntityIdMemberTests : UnitTestsOf<EntityIdMember>
    {
        [Fact]
        public void Verify_MemberRanking()
        {
            var t = typeof(ModelForMemberRanking);

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
        public void When_ModelId_is_defined_in_base_It_can_extract_the_id()
        {
            var model = new ModelInheritanceModelIdChild { ModelInheritanceModelIdBaseId = "ModelInheritanceModelIdChild:ModelInheritanceModelIdBaseId:1" };

            SUT.GetValueFrom(model).Should().Be(model.ModelInheritanceModelIdBaseId);
        }

        [Fact]
        public void When_ModelId_is_defined_in_base_but_child_has_more_specific_ModelId_It_extracts_the_child_ModelId()
        {
            var model = new ModelInheritanceModelIdWithMoreSpecificChild { ModelInheritanceModelIdWithMoreSpecificChildId = "ModelInheritanceModelIdWithMoreSpecificChild:ModelInheritanceModelIdWithMoreSpecificChildId:1" };

            SUT.GetValueFrom(model).Should().Be(model.ModelInheritanceModelIdWithMoreSpecificChildId);
        }

        [Fact]
        public void When_ModelId_is_defined_in_base_but_child_has_DocumentId_It_extracts_the_child_DocumentId()
        {
            var model = new ModelInheritanceModelIdWithChildDocumentId { DocumentId = "ModelInheritanceModelIdWithChildDocumentId:DocumentId:1" };

            SUT.GetValueFrom(model).Should().Be(model.DocumentId);
        }

        [Fact]
        public void When_ModelId_is_defined_in_base_but_child_has_EntityId_It_extracts_the_child_EntityId()
        {
            var model = new ModelInheritanceModelIdWithChildEntityId { EntityId = "ModelInheritanceModelIdWithChildEntityId:EntityId:1" };

            SUT.GetValueFrom(model).Should().Be(model.EntityId);
        }

        [Fact]
        public void When_ModelId_is_defined_in_base_but_child_has_Id_It_extracts_the_child_Id()
        {
            var model = new ModelInheritanceModelIdWithChildId { Id = "ModelInheritanceModelIdWithChildId:Id:1" };

            SUT.GetValueFrom(model).Should().Be(model.Id);
        }

        [Fact]
        public void When_DocumentId_is_defined_in_base_It_can_extract_the_id()
        {
            var model = new ModelInheritanceDocumentIdChild { DocumentId = "ModelInheritanceDocumentIdChild:DocumentId:1" };

            SUT.GetValueFrom(model).Should().Be(model.DocumentId);
        }

        [Fact]
        public void When_EntityId_is_defined_in_base_It_can_extract_the_id()
        {
            var model = new ModelInheritanceEntityIdChild { EntityId = "ModelInheritanceEntityIdChild:EntityId:1" };

            SUT.GetValueFrom(model).Should().Be(model.EntityId);
        }

        [Fact]
        public void When_Id_is_defined_in_base_It_can_extract_the_id()
        {
            var model = new ModelInheritanceIdChild { Id = "ModelInheritanceIdChild:Id:1" };

            SUT.GetValueFrom(model).Should().Be(model.Id);
        }

        [Fact]
        public void When_anonymous_type_without_Id_It_still_functions_and_returns_null()
        {
            const string expectedId = null;

            var model = new { Value = 42 };

            SUT.GetValueFrom(model).Should().Be(expectedId);
        }

        [Fact]
        public void When_anonymous_type_with_Id_It_can_extract_the_id()
        {
            const string expectedId = "MyId";

            var model = new { Id = expectedId, Value = 42 };

            SUT.GetValueFrom(model).Should().Be(expectedId);
        }

        [Fact]
        public void When_anonymous_type_with_DocumentId_It_can_extract_the_id()
        {
            const string expectedId = "MyId";

            var model = new { DocumentId = expectedId, Value = 42 };

            SUT.GetValueFrom(model).Should().Be(expectedId);
        }

        [Fact]
        public void When_anonymous_type_with_EntityId_It_can_extract_the_id()
        {
            const string expectedId = "MyId";

            var model = new { EntityId = expectedId, Value = 42 };

            SUT.GetValueFrom(model).Should().Be(expectedId);
        }

        [Fact]
        public void When_anonymous_type_with__Id_It_can_extract_the_id()
        {
            const string expectedId = "MyId";

            var model = new { _Id = expectedId, Value = 42 };

            SUT.GetValueFrom(model).Should().Be(expectedId);
        }

        [Fact]
        public void When_private_setter_It_can_set_values()
        {
            var model = new ModelWithPrivateSetter();

            SUT.SetValueTo(model, "test");

            model.Id.Should().Be("test");
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

        private class ModelInheritanceModelIdBase
        {
            public string ModelInheritanceModelIdBaseId { get; set; }
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

        private class ModelInheritanceEntityIdBase
        {
            public string EntityId { get; set; }
        }

        private class ModelInheritanceIdBase
        {
            public string Id { get; set; }
        }

        private class ModelInheritanceLowerCasedIdChild : ModelInheritanceLowerCasedIdBase
        {
        }

        private class ModelInheritanceProperCasedIdChild : ModelInheritanceProperCasedIdBase
        {
        }

        private class ModelInheritanceModelIdChild : ModelInheritanceModelIdBase
        {
        }

        private class ModelInheritanceModelIdWithMoreSpecificChild : ModelInheritanceModelIdBase
        {
            public string ModelInheritanceModelIdWithMoreSpecificChildId { get; set; }
        }

        private class ModelInheritanceModelIdWithChildDocumentId : ModelInheritanceModelIdBase
        {
            public string DocumentId { get; set; }
        }

        private class ModelInheritanceModelIdWithChildEntityId : ModelInheritanceModelIdBase
        {
            public string EntityId { get; set; }
        }

        private class ModelInheritanceModelIdWithChildId : ModelInheritanceModelIdBase
        {
            public string Id { get; set; }
        }

        private class ModelInheritanceDocumentIdChild : ModelInheritanceDocumentIdBase
        {
        }

        private class ModelInheritanceEntityIdChild : ModelInheritanceEntityIdBase
        {
        }

        private class ModelInheritanceIdChild : ModelInheritanceIdBase
        {
        }

        private class ModelWithPrivateSetter
        {
            public string Id { get; private set; }
        }
    }
}