using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DMotM.ChazzPrinceton;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Controller.Legacy;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.UnitTest;
using NUnit.Framework;

namespace DMotMTests.ChazzPrinceton
{
    [TestFixture]
    public class ChazzPrincetonCharacterCardControllerTests : BaseTest
    {
        protected const string ChazzPrincetonNamespace = "DMotM.ChazzPrinceton";
        protected HeroTurnTakerController ChazzPrinceton { get { return FindHero("ChazzPrinceton"); } }

        [Test]
        public void ChazzPrinceton_OnSetup_HasCorrectHP()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonNamespace, "Megalopolis");

            // Assert that there are exactly 3 turn takers
            Assert.That(GameController.TurnTakerControllers.Count(), Is.EqualTo(3));

            // Assert that Chazz Princeton exists in this game
            Assert.That(ChazzPrinceton, Is.Not.Null);

            // Assert that Chazz Princeton is an instance of ChazzPrincetonCharacterCardController
            Assert.That(ChazzPrinceton.CharacterCardController, Is.TypeOf<ChazzPrincetonCharacterCardController>());

            // Assert that Chazz Princeton has exactly 27 HP maximum
            Assert.That(ChazzPrinceton.CharacterCard.MaximumHitPoints, Is.EqualTo(27));

            // Assert that Chazz Princeton has exactly 27 HP currently
            Assert.That(ChazzPrinceton.CharacterCard.HitPoints, Is.EqualTo(27));
        }

        [Test]
        public void UsePower_WhenNoTargetsInPlayArea_DrawsCard()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonNamespace, "Megalopolis");
            StartGame();
            GoToUsePowerPhase(ChazzPrinceton);

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards = ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Assert that there are cards in the deck
            int numCardsInDeck = GetNumberOfCardsInDeck(ChazzPrinceton);
            Assert.That(numCardsInDeck, Is.GreaterThan(0));

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Use Chazz Princeton's power
            UsePower(ChazzPrinceton);

            // Assert that there is 1 more card in the hand than previously
            QuickHandCheck(1);
        }
    }
}
