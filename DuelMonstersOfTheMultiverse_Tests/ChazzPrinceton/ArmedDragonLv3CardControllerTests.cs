using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMotM;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.UnitTest;
using NUnit.Framework;

namespace DMotMTests.ChazzPrinceton
{
    [TestFixture]
    public class ArmedDragonLv3CardControllerTests : BaseTest
    {
        [Test]
        public void AtStartOfTurnWithNoLv5InHand_DoesNothing()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Megalopolis");
            StartGame();

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards = ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Play Armed Dragon Lv3
            PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertIsInPlayAndNotUnderCard(ChazzPrincetonConstants.ArmedDragonLv3);

            // Move all Cards from Chazz Princeton's hand into the deck, to get rid of any possible 'armed' cards
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Put Armed Dragon Lv7 into hand
            Card armedDragonLv7 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv7);
            AssertInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv7);

            // Put Armed Dragon Lv10 into hand
            Card armedDragonLv10 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);

            // Put X Head Cannon into hand
            Card yDragonHead = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.XHeadCannon);
            AssertInHand(ChazzPrinceton, ChazzPrincetonConstants.XHeadCannon);

            // Put Ojama Green into hand
            Card ojamaBlack = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.OjamaGreen);
            AssertInHand(ChazzPrinceton, ChazzPrincetonConstants.OjamaGreen);

            // Assert that there is no Armed Dragon Lv 5 in hand
            AssertNotInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv5);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Enter start of turn
            GoToStartOfTurn(ChazzPrinceton);

            // Assert that no decision was presented to the player
            AssertNoDecision();

            // Assert that no changes were made in the hand or play area
            QuickHandCheck(0);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
        }
    }
}
