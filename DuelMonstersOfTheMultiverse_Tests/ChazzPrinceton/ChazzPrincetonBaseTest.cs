using DMotM;
using NUnit.Framework;

namespace DMotMTests.ChazzPrinceton
{
    public class ChazzPrincetonBaseTest : DMotMBaseTest
    {
        [SetUp]
        public override void Setup()
        {
            // Setup test game with Chazz Princeton
            SetupTestGameWithHero(ChazzPrincetonConstants.Deck);
            base.Setup();
        }
    }
}
