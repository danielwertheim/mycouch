using FluentAssertions;
using MyCouch.Schemes;
using NUnit.Framework;

namespace MyCouch.UnitTests.Schemes
{
    [TestFixture]
    public class EntityRevMemberTests : UnitTestsOf<EntityRevMember>
    {
        protected override void OnTestInitialize()
        {
            SUT = new EntityRevMember();
        }

        [Test]
        public void When_only_having_member_Rev_It_extracts_the_Rev()
        {
            var model = new ModelOne { Rev = "ModelOne:Rev:1" };

            SUT.GetValueFrom(model).Should().Be(model.Rev);
        }

        [Test]
        public void When_having_members_Rev_EntityRev_It_extracts_EntityRev()
        {
            var model = new ModelTwo
            {
                Rev = "ModelTwo:Rev:1", 
                EntityRev = "ModelTwo:EntityRev:2"
            };

            SUT.GetValueFrom(model).Should().Be(model.EntityRev);
        }

        [Test]
        public void When_having_members_Rev_EntityRev_DocumentRev_It_extracts_DocumentRev()
        {
            var model = new ModelThree
            {
                Rev = "ModelThree:Rev:1",
                EntityRev = "ModelThree:EntityRev:2",
                DocumentRev = "ModelThree:DocumentRev:3"
            };

            SUT.GetValueFrom(model).Should().Be(model.DocumentRev);
        }

        [Test]
        public void When_having_members_Rev_EntityRev_DocumentRev_ModelRev_It_extracts_ModelRev()
        {
            var model = new ModelFour
            {
                Rev = "ModelFour:Rev:1",
                EntityRev = "ModelFour:EntityRev:2",
                DocumentRev = "ModelFour:DocumentRev:3",
                ModelFourRev = "ModelFour:ModelFourRev:4"
            };

            SUT.GetValueFrom(model).Should().Be(model.ModelFourRev);
        }

        [Test]
        public void When_having_members_Rev_EntityRev_DocumentRev_ModelRev__Rev_It_extracts__Rev()
        {
            var model = new ModelFive
            {
                Rev = "ModelFive:Rev:1",
                EntityRev = "ModelFive:EntityRev:2",
                DocumentRev = "ModelFive:DocumentRev:3",
                ModelFourRev = "ModelFive:ModelFourRev:4",
                ModelFiveRev = "ModelFive:ModelFiveRev:5",
                _Rev = "ModelFive:_Rev:6"
            };

            SUT.GetValueFrom(model).Should().Be(model._Rev);
        }

        private class ModelOne
        {
            public string Rev { get; set; }
        }

        private class ModelTwo : ModelOne
        {
            public string EntityRev { get; set; }
        }

        private class ModelThree : ModelTwo
        {
            public string DocumentRev { get; set; }
        }

        private class ModelFour : ModelThree
        {
            public string ModelFourRev { get; set; }
        }

        private class ModelFive : ModelFour
        {
            public string _Rev { get; set; }
            public string ModelFiveRev { get; set; }
        }
    }
}