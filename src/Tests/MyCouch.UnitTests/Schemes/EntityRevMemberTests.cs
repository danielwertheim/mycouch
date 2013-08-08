using FluentAssertions;
using MyCouch.Schemes;
using MyCouch.Schemes.Reflections;
using NUnit.Framework;

namespace MyCouch.UnitTests.Schemes
{
    [TestFixture]
    public class EntityRevMemberTestsWithLambdaPropertyFactoryTests : EntityRevMemberTests
    {
        protected override void OnTestInitialize()
        {
            base.OnTestInitialize();

            SUT = new EntityRevMember(new LambdaDynamicPropertyFactory());
        }
    }

    [TestFixture]
    public class EntityRevMemberTestsWithIlPropertyFactoryTests : EntityRevMemberTests
    {
        protected override void OnTestInitialize()
        {
            base.OnTestInitialize();

            SUT = new EntityRevMember(new IlDynamicPropertyFactory());
        }
    }

    [TestFixture]
    public abstract class EntityRevMemberTests : UnitTestsOf<EntityRevMember>
    {
        [Test]
        public void Verify_MemberRanking()
        {
            var t = typeof(ModelForMemberRanking);

            SUT.GetMemberRankingIndex(t, "FDE29AC2-C452-4493-8D61-5349E2E5B5D5").Should().Be(null);
            SUT.GetMemberRankingIndex(t, "_Rev").Should().Be(0);
            SUT.GetMemberRankingIndex(t, "ModelForMemberRankingRev").Should().Be(1);
            SUT.GetMemberRankingIndex(t, "DocumentRev").Should().Be(2);
            SUT.GetMemberRankingIndex(t, "EntityRev").Should().Be(3);
            SUT.GetMemberRankingIndex(t, "Rev").Should().Be(4);
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

        private class ModelForMemberRanking
        {
            public string Rev { get; set; }
            public string EntityRev { get; set; }
            public string DocumentRev { get; set; }
            public string ModelForMemberRankingRev { get; set; }
            public string _Rev { get; set; }
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