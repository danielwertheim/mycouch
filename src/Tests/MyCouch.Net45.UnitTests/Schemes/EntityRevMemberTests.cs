using FluentAssertions;
using MyCouch.EntitySchemes;
using MyCouch.EntitySchemes.Reflections;
using Xunit;

namespace MyCouch.UnitTests.Schemes
{
    public class EntityRevMemberTestsWithLambdaPropertyFactoryTests : EntityRevMemberTests
    {
        public EntityRevMemberTestsWithLambdaPropertyFactoryTests()
        {
            SUT = new EntityRevMember(new LambdaDynamicPropertyFactory());
        }
    }
#if !NETFX_CORE
    public class EntityRevMemberTestsWithIlPropertyFactoryTests : EntityRevMemberTests
    {
        public EntityRevMemberTestsWithIlPropertyFactoryTests()
        {
            SUT = new EntityRevMember(new IlDynamicPropertyFactory());
        }
    }
#endif
    public abstract class EntityRevMemberTests : UnitTestsOf<EntityRevMember>
    {
        [Fact]
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

        [Fact]
        public void When_only_having_member_Rev_It_extracts_the_Rev()
        {
            var model = new ModelOne { Rev = "ModelOne:Rev:1" };

            SUT.GetValueFrom(model).Should().Be(model.Rev);
        }

        [Fact]
        public void When_having_members_Rev_EntityRev_It_extracts_EntityRev()
        {
            var model = new ModelTwo
            {
                Rev = "ModelTwo:Rev:1", 
                EntityRev = "ModelTwo:EntityRev:2"
            };

            SUT.GetValueFrom(model).Should().Be(model.EntityRev);
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
        public void When__rev_is_defined_in_base_It_can_extract_the_rev()
        {
            var model = new ModelInheritanceLowerCasedRevChild { _rev = "ModelInheritanceLowerCasedRevChild:_rev:1" };

            SUT.GetValueFrom(model).Should().Be(model._rev);
        }

        [Fact]
        public void When__Rev_is_defined_in_base_It_can_extract_the_rev()
        {
            var model = new ModelInheritanceProperCasedRevChild { _Rev = "ModelInheritanceProperCasedRevChild:_Rev:1" };

            SUT.GetValueFrom(model).Should().Be(model._Rev);
        }

        [Fact]
        public void When_ModelRev_is_defined_in_base_It_can_extract_the_rev()
        {
            var model = new ModelInheritanceModelRevChild { ModelInheritanceModelRevBaseRev = "ModelInheritanceModelRevChild:ModelInheritanceModelRevBaseRev:1" };

            SUT.GetValueFrom(model).Should().Be(model.ModelInheritanceModelRevBaseRev);
        }

        [Fact]
        public void When_ModelRev_is_defined_in_base_but_child_has_more_specific_ModelRev_It_extracts_the_child_ModelRev()
        {
            var model = new ModelInheritanceModelRevWithMoreSpecificChild { ModelInheritanceModelRevWithMoreSpecificChildRev = "ModelInheritanceModelRevWithMoreSpecificChild:ModelInheritanceModelRevWithMoreSpecificChildRev:1" };

            SUT.GetValueFrom(model).Should().Be(model.ModelInheritanceModelRevWithMoreSpecificChildRev);
        }

        [Fact]
        public void When_ModelRev_is_defined_in_base_but_child_has_DocumentRev_It_extracts_the_child_DocumentRev()
        {
            var model = new ModelInheritanceModelRevWithChildDocumentRev { DocumentRev = "ModelInheritanceModelRevWithChildDocumentRev:DocumentRev:1" };

            SUT.GetValueFrom(model).Should().Be(model.DocumentRev);
        }

        [Fact]
        public void When_ModelRev_is_defined_in_base_but_child_has_EntityRev_It_extracts_the_child_EntityRev()
        {
            var model = new ModelInheritanceModelRevWithChildEntityRev { EntityRev = "ModelInheritanceModelRevWithChildEntityRev:EntityRev:1" };

            SUT.GetValueFrom(model).Should().Be(model.EntityRev);
        }

        [Fact]
        public void When_ModelRev_is_defined_in_base_but_child_has_Rev_It_extracts_the_child_Rev()
        {
            var model = new ModelInheritanceModelRevWithChildRev { Rev = "ModelInheritanceModelRevWithChildRev:Rev:1" };

            SUT.GetValueFrom(model).Should().Be(model.Rev);
        }

        [Fact]
        public void When_DocumentRev_is_defined_in_base_It_can_extract_the_rev()
        {
            var model = new ModelInheritanceDocumentRevChild { DocumentRev = "ModelInheritanceDocumentRevChild:DocumentRev:1" };

            SUT.GetValueFrom(model).Should().Be(model.DocumentRev);
        }

        [Fact]
        public void When_EntityRev_is_defined_in_base_It_can_extract_the_rev()
        {
            var model = new ModelInheritanceEntityRevChild { EntityRev = "ModelInheritanceEntityRevChild:EntityRev:1" };

            SUT.GetValueFrom(model).Should().Be(model.EntityRev);
        }

        [Fact]
        public void When_Rev_is_defined_in_base_It_can_extract_the_rev()
        {
            var model = new ModelInheritanceRevChild { Rev = "ModelInheritanceRevChild:Rev:1" };

            SUT.GetValueFrom(model).Should().Be(model.Rev);
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

        private class ModelInheritanceModelRevBase
        {
            public string ModelInheritanceModelRevBaseRev { get; set; }
        }

        private class ModelInheritanceLowerCasedRevBase
        {
            public string _rev { get; set; }
        }

        private class ModelInheritanceProperCasedRevBase
        {
            public string _Rev { get; set; }
        }

        private class ModelInheritanceModelRevChild : ModelInheritanceModelRevBase
        {
        }

        private class ModelInheritanceModelRevWithMoreSpecificChild : ModelInheritanceModelRevBase
        {
            public string ModelInheritanceModelRevWithMoreSpecificChildRev { get; set; }
        }

        private class ModelInheritanceModelRevWithChildDocumentRev : ModelInheritanceModelRevBase
        {
            public string DocumentRev { get; set; }
        }

        private class ModelInheritanceModelRevWithChildEntityRev : ModelInheritanceModelRevBase
        {
            public string EntityRev { get; set; }
        }

        private class ModelInheritanceModelRevWithChildRev : ModelInheritanceModelRevBase
        {
            public string Rev { get; set; }
        }

        private class ModelInheritanceDocumentRevBase
        {
            public string DocumentRev { get; set; }
        }

        private class ModelInheritanceEntityRevBase
        {
            public string EntityRev { get; set; }
        }

        private class ModelInheritanceRevBase
        {
            public string Rev { get; set; }
        }

        private class ModelInheritanceLowerCasedRevChild : ModelInheritanceLowerCasedRevBase
        {
        }

        private class ModelInheritanceProperCasedRevChild : ModelInheritanceProperCasedRevBase
        {
        }

        private class ModelInheritanceDocumentRevChild : ModelInheritanceDocumentRevBase
        {
        }

        private class ModelInheritanceEntityRevChild : ModelInheritanceEntityRevBase
        {
        }

        private class ModelInheritanceRevChild : ModelInheritanceRevBase
        {
        }
    }
}