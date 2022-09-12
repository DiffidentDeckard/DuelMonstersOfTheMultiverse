using System.Linq;
using DMotM.ChazzPrinceton;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.UnitTest;
using NUnit.Framework;

namespace DMotM_Tests
{
    [TestFixture]
    public class ChazzPrincetonCharacterCardController_Tests : BaseTest
    {
        protected HeroTurnTakerController chazz { get { return FindHero("ChazzPrinceton"); } }

        [Test]
        public void Test_ChazzPrinceton_Loads()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", "DMotM.ChazzPrinceton", "Megalopolis");

            // Assert that there are exactly 3 turn takers
            Assert.That(GameController.TurnTakerControllers.Count(), Is.EqualTo(3));

            // Assert that Chazz Princeton exists in this game
            Assert.That(chazz, Is.Not.Null);

            // Assert that Chazz Princeton is an instance of ChazzPrincetonCharacterCardController
            Assert.That(chazz.CharacterCardController, Is.TypeOf<ChazzPrincetonCharacterCardController>());

            // Assert that Chazz Princeton has exactly 27 HP maximum
            Assert.That(chazz.CharacterCard.MaximumHitPoints, Is.EqualTo(27));

            // Assert that Chazz Princeton has exactly 27 HP currently
            Assert.That(chazz.CharacterCard.HitPoints, Is.EqualTo(27));
        }
    }
}
