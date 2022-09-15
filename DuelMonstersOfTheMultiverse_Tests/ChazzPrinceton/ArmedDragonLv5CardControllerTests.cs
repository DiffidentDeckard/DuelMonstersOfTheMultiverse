using System.Collections.Generic;
using System.Linq;
using DMotM;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.UnitTest;
using NUnit.Framework;

namespace DMotMTests.ChazzPrinceton
{
    [TestFixture]
    public class ArmedDragonLv5CardControllerTests : BaseTest
    {
        [Test]
        public void AtStartOfTurn_WithNoLv7InHand_DoesNothing()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Megalopolis");
            StartGame();

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards = ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Play Armed Dragon Lv5
            Card armedDragonLv5 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv5);
            AssertIsInPlayAndNotUnderCard(armedDragonLv5);

            // Move all Cards from Chazz Princeton's hand into the deck, to get rid of any possible 'armed' cards
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Put Armed Dragon Lv3 into hand
            Card armedDragonLv3 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertInHand(ChazzPrinceton, armedDragonLv3);

            // Put Armed Dragon Lv10 into hand
            Card armedDragonLv10 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertInHand(ChazzPrinceton, armedDragonLv10);

            // Put V Tiger Jet into hand
            Card vTigerjet = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.VTigerJet);
            AssertInHand(ChazzPrinceton, vTigerjet);

            // Put Ojama King into hand
            Card ojamaKing = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.OjamaKing);
            AssertInHand(ChazzPrinceton, ojamaKing);

            // Assert that there is no Armed Dragon Lv 7 in hand
            AssertNotInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv7);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Enter start of turn
            GoToStartOfTurn(ChazzPrinceton);

            // Assert that no decision was presented to the player
            AssertNoDecision();

            // Assert that no changes were made in the hand or play area
            QuickHandCheck(0);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertInPlayArea(ChazzPrinceton, armedDragonLv5);
        }

        [Test]
        public void AtStartOfTurn_WithLv7InHandButChooseNotToPlay_DoesNothing()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Megalopolis");
            StartGame();

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards = ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Play Armed Dragon Lv5
            Card armedDragonLv5 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv5);
            AssertIsInPlayAndNotUnderCard(armedDragonLv5);

            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Put Armed Dragon Lv5 into hand
            Card armedDragonLv3 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertInHand(ChazzPrinceton, armedDragonLv3);

            // Put Armed Dragon Lv7 into hand
            Card armedDragonLv7 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv7);
            AssertInHand(ChazzPrinceton, armedDragonLv7);

            // Put Armed Dragon Lv10 into hand
            Card armedDragonLv10 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertInHand(ChazzPrinceton, armedDragonLv10);

            // Put V Tiger Jet into hand
            Card vTigerjet = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.VTigerJet);
            AssertInHand(ChazzPrinceton, vTigerjet);

            // Put Ojama King into hand
            Card ojamaKing = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.OjamaKing);
            AssertInHand(ChazzPrinceton, ojamaKing);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Store the expected choices the player should see
            IEnumerable<Card> includedCards = new List<Card>() { armedDragonLv7 };
            IEnumerable<Card> notIncludedCards = new List<Card>() { armedDragonLv3, armedDragonLv5, armedDragonLv10, vTigerjet, ojamaKing };

            // Assert the player sees the choices we expect.
            // We will be choosing not to play Armed Dragon Lv7
            AssertDecisionIsOptional(SelectionType.PlayCard);
            AssertNextDecisionChoices(includedCards, notIncludedCards);
            DecisionSelectCardToPlay = null;
            DecisionDoNotSelectCard = SelectionType.PlayCard;

            // Enter start of turn
            GoToStartOfTurn(ChazzPrinceton);

            // Assert that no changes were made in the hand or play area
            QuickHandCheck(0);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertInPlayArea(ChazzPrinceton, armedDragonLv5);
            AssertNotInPlayArea(ChazzPrinceton, armedDragonLv7);
        }

        [Test]
        public void AtStartOfTurn_WithLv7InHandAndChooseToPlay_PlaysLv7AndDestroysLv5()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Megalopolis");
            StartGame();

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards = ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Play Armed Dragon Lv5
            Card armedDragonLv5 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv5);
            AssertIsInPlayAndNotUnderCard(armedDragonLv5);

            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Put Armed Dragon Lv5 into hand
            Card armedDragonLv3 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertInHand(ChazzPrinceton, armedDragonLv3);

            // Put Armed Dragon Lv7 into hand
            Card armedDragonLv7 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv7);
            AssertInHand(ChazzPrinceton, armedDragonLv7);

            // Put Armed Dragon Lv10 into hand
            Card armedDragonLv10 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertInHand(ChazzPrinceton, armedDragonLv10);

            // Put V Tiger Jet into hand
            Card vTigerjet = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.VTigerJet);
            AssertInHand(ChazzPrinceton, vTigerjet);

            // Put Ojama King into hand
            Card ojamaKing = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.OjamaKing);
            AssertInHand(ChazzPrinceton, ojamaKing);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Store the expected choices the player should see
            IEnumerable<Card> includedCards = new List<Card>() { armedDragonLv7 };
            IEnumerable<Card> notIncludedCards = new List<Card>() { armedDragonLv3, armedDragonLv5, armedDragonLv10, vTigerjet, ojamaKing };

            // Assert the player sees the choices we expect.
            // We will be choosing to play Armed Dragon Lv7
            AssertDecisionIsOptional(SelectionType.PlayCard);
            AssertNextDecisionChoices(includedCards, notIncludedCards);
            DecisionSelectCardToPlay = armedDragonLv7;

            // Enter start of turn
            GoToStartOfTurn(ChazzPrinceton);

            // Assert that Armed Dragon Lv7 was played and that Armed Dragon Lv5 was destroyed
            QuickHandCheck(-1);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertInPlayArea(ChazzPrinceton, armedDragonLv7);
            AssertInTrash(ChazzPrinceton, armedDragonLv5);
        }
    }
}
