using System.Collections.Generic;
using DMotM;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using NUnit.Framework;

namespace DMotMTests.ChazzPrinceton
{
    [TestFixture]
    public class ArmedDragonLv3CardControllerTests : ChazzPrincetonBaseTest
    {
        [Test]
        public void HasArmedKeyword()
        {
            // Put Armed Dragon Lv3 into hand
            Card armedDragonLv3 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertInHand(ChazzPrinceton, armedDragonLv3);

            // Assert that Armed Dragon Lv3 has the Armed keyword
            AssertCardHasKeyword(armedDragonLv3, ChazzPrincetonConstants.Armed, false);
        }

        [Test]
        public void IsATargetWith3MaxHP()
        {
            // Put Armed Dragon Lv3 into hand
            Card armedDragonLv3 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertInHand(ChazzPrinceton, armedDragonLv3);

            // Assert that Armed Dragon Lv3 is a target
            AssertIsTarget(armedDragonLv3);

            // Assert that the Maximum Hit Points is equal to 3
            AssertMaximumHitPoints(armedDragonLv3, 3);
        }

        [Test]
        public void AtYourSameStartOfTurn_WithLv5InHand_DoesNothing()
        {
            // Enter our Start of turn immediately, to ensure that Armed Dragon Lv3 does not activate
            GoToStartOfTurn(ChazzPrinceton);

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

            // Assert that no decision will be presented to the player
            AssertNoDecision();

            // Play Armed Dragon Lv3 from hand during this start of turn phase
            PlayCardFromHand(ChazzPrinceton, armedDragonLv3.Identifier);

            // Assert that we have one less card in hand, and Armed Dragon Lv3 is in play area
            QuickHandCheck(-1);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, armedDragonLv3);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();
        }

        [Test]
        public void AtOtherStartOfTurn_WithLv5InHand_DoesNothing()
        {
            // Enter our Start of turn immediately, to ensure that Armed Dragon Lv3 does not activate
            GoToStartOfTurn(ChazzPrinceton);

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

            // Assert that no decision will be presented to the player
            AssertNoDecision();

            // Enter Start of turn of next turn taker
            GoToStartOfTurn(TestHero1);

            // Assert that we have one less card in hand, and Armed Dragon Lv3 is in play area
            QuickHandCheck(-1);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, armedDragonLv3);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();
        }

        [Test]
        public void AtYourNextStartOfTurn_WithNoLv5InHand_DoesNothing()
        {
            // Play Armed Dragon Lv3
            Card armedDragonLv3 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, armedDragonLv3);

            // Move all Cards from Chazz Princeton's hand into the deck
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

            // Assert that there is no Armed Dragon Lv5 in hand
            AssertNotInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv5);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Assert that no decision will be presented to the player
            AssertNoDecision();

            // Enter end of Villain turn
            GoToEndOfTurn(TestVillain);

            // Enter start of next turn
            GoToStartOfTurn(ChazzPrinceton);

            // Assert that no changes were made in the hand or play area
            QuickHandCheck(0);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, armedDragonLv3);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();
        }

        [Test]
        public void AtYourNextStartOfTurn_WithLv5InHandButChooseNotToPlay_DoesNothing()
        {
            // Play Armed Dragon Lv3
            Card armedDragonLv3 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, armedDragonLv3);

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
            IEnumerable<Card> includedCards = new List<Card> { armedDragonLv5 };
            IEnumerable<Card> notIncludedCards = new List<Card> { armedDragonLv3, armedDragonLv7, armedDragonLv10, xheadCannon, ojamaGreen };

            // Assert the player sees the choices we expect.
            // We will be choosing not to play Armed Dragon Lv5
            AssertDecisionIsOptional(SelectionType.PlayCard);
            AssertNextDecisionChoices(includedCards, notIncludedCards);
            DecisionSelectCardToPlay = null;
            DecisionDoNotSelectCard = SelectionType.PlayCard;

            // Enter end of Villain turn
            GoToEndOfTurn(TestVillain);

            // Enter start of next turn
            GoToStartOfTurn(ChazzPrinceton);

            // Assert that no changes were made in the hand or play area
            QuickHandCheck(0);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, armedDragonLv3);
            AssertInHand(ChazzPrinceton, armedDragonLv5);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();
        }

        [Test]
        public void AtYourNextStartOfTurn_WithLv5InHandAndChooseToPlay_PlaysLv5AndDestroysSelf()
        {
            // Play Armed Dragon Lv3
            Card armedDragonLv3 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, armedDragonLv3);

            // Play X Head Cannon
            Card xheadCannon = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.XHeadCannon);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, xheadCannon);

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

            // Put Ojama Green into hand
            Card ojamaGreen = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.OjamaGreen);
            AssertInHand(ChazzPrinceton, ojamaGreen);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Store the expected choices the player should see
            IEnumerable<Card> includedCards = new List<Card> { armedDragonLv5 };
            IEnumerable<Card> notIncludedCards = new List<Card> { armedDragonLv3, armedDragonLv7, armedDragonLv10, xheadCannon, ojamaGreen };

            // Assert the player sees the choices we expect.
            // We will be choosing to play Armed Dragon Lv5
            AssertDecisionIsOptional(SelectionType.PlayCard);
            AssertNextDecisionChoices(includedCards, notIncludedCards);
            DecisionSelectCardToPlay = armedDragonLv5;

            // Enter end of Villain turn
            GoToEndOfTurn(TestVillain);

            // Enter start of next turn
            GoToStartOfTurn(ChazzPrinceton);

            // Assert that Armed Dragon Lv5 was played and that Armed Dragon Lv3 was destroyed
            // Since Armed Dragon Lv3 was destroyed we should have also drawn a card
            QuickHandCheck(0);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 3);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, armedDragonLv5);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, xheadCannon);
            AssertInTrash(ChazzPrinceton, armedDragonLv3);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();
        }

        [Test]
        public void WhenDestroyed_DrawsOneCard()
        {
            // Play Armed Dragon Lv3
            Card armedDragonLv3 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, armedDragonLv3);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Destroy Armed Dragon Lv3
            DestroyCard(armedDragonLv3, TestVillain.CharacterCard);
            AssertInTrash(ChazzPrinceton, armedDragonLv3);

            // Assert that one card was drawn
            QuickHandCheck(1);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();
        }
    }
}
