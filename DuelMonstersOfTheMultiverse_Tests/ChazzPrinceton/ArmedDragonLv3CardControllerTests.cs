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
    public class ArmedDragonLv3CardControllerTests : BaseTest
    {

        [Test]
        public void AtStartOfSameTurn_WithLv5InHand_DoesNothing()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Legacy", "Megalopolis");
            StartGame();

            // Enter our Start of turn immediately, to ensure that Armed Dragon Lv3 does not activate
            GoToStartOfTurn(ChazzPrinceton);

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards = ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Put Armed Dragon Lv3 into hand
            Card armedDragonLv3 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertInHand(ChazzPrinceton, armedDragonLv3);

            // Put Armed Dragon Lv5 into hand
            Card armedDragonLv5 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv5);
            AssertInHand(ChazzPrinceton, armedDragonLv5);

            // Put Armed Dragon Lv7 into hand
            Card armedDragonLv7 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv7);
            AssertInHand(ChazzPrinceton, armedDragonLv7);

            // Put Armed Dragon Lv10 into hand
            Card armedDragonLv10 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertInHand(ChazzPrinceton, armedDragonLv10);

            // Put X Head Cannon into hand
            Card xheadCannon = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.XHeadCannon);
            AssertInHand(ChazzPrinceton, xheadCannon);

            // Put Ojama Green into hand
            Card ojamaGreen = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.OjamaGreen);
            AssertInHand(ChazzPrinceton, ojamaGreen);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Play Armed Dragon Lv3 from hand during this start of turn phase
            PlayCardFromHand(ChazzPrinceton, armedDragonLv3.Identifier);

            // Assert that no decision was presented to the player
            AssertNoDecision();

            // Assert that we have one less card in hand, and Armed Dragon Lv3 is in play area
            QuickHandCheck(-1);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertInPlayArea(ChazzPrinceton, armedDragonLv3);
        }

        [Test]
        public void AtStartOfNextTurn_WithNoLv5InHand_DoesNothing()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Legacy", "Megalopolis");
            StartGame();

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards = ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Play Armed Dragon Lv3
            Card armedDragonLv3 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertIsInPlayAndNotUnderCard(armedDragonLv3);

            // Move all Cards from Chazz Princeton's hand into the deck, to get rid of any possible 'armed' cards
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Put Armed Dragon Lv7 into hand
            Card armedDragonLv7 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv7);
            AssertInHand(ChazzPrinceton, armedDragonLv7);

            // Put Armed Dragon Lv10 into hand
            Card armedDragonLv10 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertInHand(ChazzPrinceton, armedDragonLv10);

            // Put X Head Cannon into hand
            Card xheadCannon = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.XHeadCannon);
            AssertInHand(ChazzPrinceton, xheadCannon);

            // Put Ojama Green into hand
            Card ojamaGreen = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.OjamaGreen);
            AssertInHand(ChazzPrinceton, ojamaGreen);

            // Assert that there is no Armed Dragon Lv 5 in hand
            AssertNotInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv5);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Enter end of Villain turn
            GoToEndOfTurn(baron);

            // Enter start of next turn
            GoToStartOfTurn(ChazzPrinceton);

            // Assert that no decision was presented to the player
            AssertNoDecision();

            // Assert that no changes were made in the hand or play area
            QuickHandCheck(0);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertInPlayArea(ChazzPrinceton, armedDragonLv3);
        }

        [Test]
        public void AtStartOfNextTurn_WithLv5InHandButChooseNotToPlay_DoesNothing()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Legacy", "Megalopolis");
            StartGame();

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards = ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Play Armed Dragon Lv3
            Card armedDragonLv3 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertIsInPlayAndNotUnderCard(armedDragonLv3);

            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Put Armed Dragon Lv5 into hand
            Card armedDragonLv5 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv5);
            AssertInHand(ChazzPrinceton, armedDragonLv5);

            // Put Armed Dragon Lv7 into hand
            Card armedDragonLv7 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv7);
            AssertInHand(ChazzPrinceton, armedDragonLv7);

            // Put Armed Dragon Lv10 into hand
            Card armedDragonLv10 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertInHand(ChazzPrinceton, armedDragonLv10);

            // Put X Head Cannon into hand
            Card xheadCannon = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.XHeadCannon);
            AssertInHand(ChazzPrinceton, xheadCannon);

            // Put Ojama Green into hand
            Card ojamaGreen = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.OjamaGreen);
            AssertInHand(ChazzPrinceton, ojamaGreen);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Store the expected choices the player should see
            IEnumerable<Card> includedCards = new List<Card>() { armedDragonLv5 };
            IEnumerable<Card> notIncludedCards = new List<Card>() { armedDragonLv3, armedDragonLv7, armedDragonLv10, xheadCannon, ojamaGreen };

            // Assert the player sees the choices we expect.
            // We will be choosing not to play Armed Dragon Lv5
            AssertDecisionIsOptional(SelectionType.PlayCard);
            AssertNextDecisionChoices(includedCards, notIncludedCards);
            DecisionSelectCardToPlay = null;
            DecisionDoNotSelectCard = SelectionType.PlayCard;

            // Enter end of Villain turn
            GoToEndOfTurn(baron);

            // Enter start of next turn
            GoToStartOfTurn(ChazzPrinceton);

            // Assert that no changes were made in the hand or play area
            QuickHandCheck(0);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertInPlayArea(ChazzPrinceton, armedDragonLv3);
            AssertNotInPlayArea(ChazzPrinceton, armedDragonLv5);
        }

        [Test]
        public void AtStartOfNextTurn_WithLv5InHandAndChooseToPlay_PlaysLv5AndDestroysLv3()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Legacy", "Megalopolis");
            StartGame();

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards = ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Play Armed Dragon Lv3
            Card armedDragonLv3 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertIsInPlayAndNotUnderCard(armedDragonLv3);

            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Put Armed Dragon Lv5 into hand
            Card armedDragonLv5 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv5);
            AssertInHand(ChazzPrinceton, armedDragonLv5);

            // Put Armed Dragon Lv7 into hand
            Card armedDragonLv7 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv7);
            AssertInHand(ChazzPrinceton, armedDragonLv7);

            // Put Armed Dragon Lv10 into hand
            Card armedDragonLv10 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertInHand(ChazzPrinceton, armedDragonLv10);

            // Put X Head Cannon into hand
            Card xheadCannon = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.XHeadCannon);
            AssertInHand(ChazzPrinceton, xheadCannon);

            // Put Ojama Green into hand
            Card ojamaGreen = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.OjamaGreen);
            AssertInHand(ChazzPrinceton, ojamaGreen);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Store the expected choices the player should see
            IEnumerable<Card> includedCards = new List<Card>() { armedDragonLv5 };
            IEnumerable<Card> notIncludedCards = new List<Card>() { armedDragonLv3, armedDragonLv7, armedDragonLv10, xheadCannon, ojamaGreen };

            // Assert the player sees the choices we expect.
            // We will be choosing to play Armed Dragon Lv5
            AssertDecisionIsOptional(SelectionType.PlayCard);
            AssertNextDecisionChoices(includedCards, notIncludedCards);
            DecisionSelectCardToPlay = armedDragonLv5;

            // Enter end of Villain turn
            GoToEndOfTurn(baron);

            // Enter start of next turn
            GoToStartOfTurn(ChazzPrinceton);

            // Assert that Armed Dragon Lv5 was played and that Armed Dragon Lv3 was destroyed
            // Since Armed Dragon Lv3 was destroyed we should have also drawn a card
            QuickHandCheck(0);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertInPlayArea(ChazzPrinceton, armedDragonLv5);
            AssertInTrash(ChazzPrinceton, armedDragonLv3);
        }

        [Test]
        public void WhenDestroyed_DrawsOneCard()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Legacy", "Megalopolis");
            StartGame();

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards = ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Play Armed Dragon Lv3
            Card armedDragonLv3 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertIsInPlayAndNotUnderCard(armedDragonLv3);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Destroy Armed Dragon Lv3
            DestroyCard(armedDragonLv3, baron.CharacterCard);
            AssertInTrash(ChazzPrinceton, armedDragonLv3);

            // Assert that one card was drawn
            QuickHandCheck(1);
        }
    }
}
