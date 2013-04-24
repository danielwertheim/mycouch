using FluentAssertions;
using MyCouch.Schemes;
using NUnit.Framework;

namespace MyCouch.UnitTests.Schemes
{
    [TestFixture]
    public class EntityIdMemberTests : UnitTestsOf<EntityIdMember>
    {
        protected override void OnTestInitialize()
        {
            SUT = new EntityIdMember();
        }

        [Test]
        public void When_only_having_member_Id_It_extracts_the_id()
        {
            var model = new ModelOne { Id = "ModelOne:Id:1" };

            SUT.GetValueFrom(model).Should().Be(model.Id);
        }

        [Test]
        public void When_having_members_Id_EntityId_It_extracts_EntityId()
        {
            var model = new ModelTwo
            {
                Id = "ModelTwo:Id:1", 
                EntityId = "ModelTwo:EntityId:2"
            };

            SUT.GetValueFrom(model).Should().Be(model.EntityId);
        }

        [Test]
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

        [Test]
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

        [Test]
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
    }
}