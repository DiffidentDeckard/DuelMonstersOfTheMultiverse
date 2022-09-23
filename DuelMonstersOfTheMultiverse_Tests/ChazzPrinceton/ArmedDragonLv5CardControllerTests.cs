﻿using System.Collections.Generic;
using DeckardBaseMod;
using DMotM;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using NUnit.Framework;

namespace DMotMTests.ChazzPrinceton
{
    [TestFixture]
    public class ArmedDragonLv5CardControllerTests : ChazzPrincetonBaseTest
    {
        [Test]
        public void HasArmedKeyword()
        {
            // Put Armed Dragon Lv5 into hand
            Card armedDragonLv5 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv5);
            AssertInHand(ChazzPrinceton, armedDragonLv5);

            // Assert that Armed Dragon Lv5 has the Armed keyword
            AssertCardHasKeyword(armedDragonLv5, ChazzPrincetonConstants.Armed, false);
        }

        [Test]
        public void IsATargetWith5MaxHP()
        {
            // Put Armed Dragon Lv5 into hand
            Card armedDragonLv5 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv5);
            AssertInHand(ChazzPrinceton, armedDragonLv5);

            // Assert that Armed Dragon Lv5 is a target
            AssertIsTarget(armedDragonLv5);

            // Assert that the Maximum Hit Points is equal to 5
            AssertMaximumHitPoints(armedDragonLv5, 5);
        }

        [Test]
        public void Play_DuringYourStartDrawCardEndPhases_DoesNotDestroy(
            [Values(Phase.Start, Phase.DrawCard, Phase.End)] Phase phase)
        {
            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Play V Tiger Jet
            Card vTigerJet = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.VTigerJet);
            AssertIsInPlayAndNotUnderCard(vTigerJet);

            // Put Armed Dragon Lv5 into hand
            Card armedDragonLv5 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv5);
            AssertInHand(ChazzPrinceton, armedDragonLv5);

            // Enter Chazz Princeton phase
            GoToPhase(ChazzPrinceton, phase);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Assert that no decision will be presented to the player
            AssertNoDecision();

            // Play Armed Dragon Lv5
            PlayCardFromHand(ChazzPrinceton, armedDragonLv5.Identifier);

            // Assert that Armed Dragon Lv5 did not destroy
            QuickHandCheck(-1);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 3);
            AssertIsInPlayAndNotUnderCard(vTigerJet);
            AssertIsInPlayAndNotUnderCard(armedDragonLv5);
        }

        [Test]
        public void Play_DuringYourPlayCardUsePowerPhasesWithNoOtherTargets_DestroysSelfNoDecision(
            [Values(Phase.PlayCard, Phase.UsePower)] Phase phase)
        {
            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Play ABC Union
            Card abcUnion = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.AbcUnion);
            AssertIsInPlayAndNotUnderCard(abcUnion);

            // Put Armed Dragon Lv5 into hand
            Card armedDragonLv5 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv5);
            AssertInHand(ChazzPrinceton, armedDragonLv5);

            // Enter Chazz Princeton phase
            GoToPhase(ChazzPrinceton, phase);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Assert that no decision will be presented to the player
            AssertNoDecision();

            // Play Armed Dragon Lv5
            PlayCardFromHand(ChazzPrinceton, armedDragonLv5.Identifier);

            // Assert that Armed Dragon Lv5 destroyed itself
            QuickHandCheck(-1);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertIsInPlayAndNotUnderCard(abcUnion);
            AssertInTrash(ChazzPrinceton, armedDragonLv5);
        }

        [Test]
        public void Play_DuringYourPlayCardUsePowerPhasesWithOtherTargets_DestroysTargetWithDecision(
            [Values(Phase.PlayCard, Phase.UsePower)] Phase phase)
        {
            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Play ABC Union
            Card abcUnion = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.AbcUnion);
            AssertIsInPlayAndNotUnderCard(abcUnion);

            // Play V Tiger Jet
            Card vTigerJet = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.VTigerJet);
            AssertIsInPlayAndNotUnderCard(vTigerJet);

            // Put Armed Dragon Lv5 into hand
            Card armedDragonLv5 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv5);
            AssertInHand(ChazzPrinceton, armedDragonLv5);

            // Enter Chazz Princeton phase
            GoToPhase(ChazzPrinceton, phase);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Store the expected choices for the player to destroy
            IEnumerable<Card> includedCards = new List<Card> { vTigerJet, armedDragonLv5 };
            IEnumerable<Card> notIncludedCards = new List<Card> { ChazzPrinceton.CharacterCard, abcUnion };

            // Assert that we get the correct choices
            // We will choose to destroy V Tiger jet
            AssertNextDecisionChoices(includedCards, notIncludedCards);
            DecisionSelectCard = vTigerJet;

            // Play Armed Dragon Lv5
            PlayCardFromHand(ChazzPrinceton, armedDragonLv5.Identifier);

            // Assert that Armed Dragon Lv5 destroyed V Tiger Jet
            QuickHandCheck(-1);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 3);
            AssertIsInPlayAndNotUnderCard(abcUnion);
            AssertIsInPlayAndNotUnderCard(armedDragonLv5);
            AssertInTrash(ChazzPrinceton, vTigerJet);
        }

        [Test]
        public void Play_DuringOtherPlayerTurn_DoesNotDestroy(
            [ValueSource(nameof(GetTestTurnPhases))] Phase phase)
        {
            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Play V Tiger Jet
            Card vTigerJet = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.VTigerJet);
            AssertIsInPlayAndNotUnderCard(vTigerJet);

            // Put Armed Dragon Lv5 into hand
            Card armedDragonLv5 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv5);
            AssertInHand(ChazzPrinceton, armedDragonLv5);

            // Enter TestHero1 phase
            GoToPhase(TestHero1, phase);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Assert that no decision will be presented to the player
            AssertNoDecision();

            // Play Armed Dragon Lv5
            PlayCardFromHand(ChazzPrinceton, armedDragonLv5.Identifier);

            // Assert that Armed Dragon Lv5 did not destroy
            QuickHandCheck(-1);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 3);
            AssertIsInPlayAndNotUnderCard(vTigerJet);
            AssertIsInPlayAndNotUnderCard(armedDragonLv5);
        }

        [Test]
        public void AtYourSameStartOfTurn_WithLv7InHand_DoesNothing()
        {
            // Enter our Start of turn immediately, to ensure that Armed Dragon Lv5 does not activate
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

            // Play Armed Dragon Lv5 from hand during this start of turn phase
            PlayCardFromHand(ChazzPrinceton, armedDragonLv5.Identifier);

            // Assert that we have one less card in hand, and Armed Dragon Lv5 is in play area
            QuickHandCheck(-1);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertIsInPlayAndNotUnderCard(armedDragonLv5);
        }

        [Test]
        public void AtOtherStartOfTurn_WithLv7InHand_DoesNothing()
        {
            // Enter our Start of turn immediately, to ensure that Armed Dragon Lv5 does not activate
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

            // Play Armed Dragon Lv5 from hand during this start of turn phase
            PlayCardFromHand(ChazzPrinceton, armedDragonLv5.Identifier);

            // Enter end of turn to go through the damage part, we'll skip the damage
            DecisionDoNotSelectCard = SelectionType.DealDamage;
            GoToEndOfTurn(ChazzPrinceton);

            // Once we have skipped the damage dealing, assert that no more decisions will be presented to player
            AssertNoDecision();

            // Go to next turn
            GoToNextTurn();

            // Assert that we have one less card in hand, and Armed Dragon Lv5 is in play area
            QuickHandCheck(-1);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertIsInPlayAndNotUnderCard(armedDragonLv5);
        }

        [Test]
        public void AtYourNextStartOfTurn_WithNoLv7InHand_DoesNothing()
        {
            // Play Armed Dragon Lv5
            Card armedDragonLv5 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv5);
            AssertIsInPlayAndNotUnderCard(armedDragonLv5);

            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Put Armed Dragon Lv3 into hand
            Card armedDragonLv3 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertInHand(ChazzPrinceton, armedDragonLv3);

            // Put Armed Dragon Lv10 into hand
            Card armedDragonLv10 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertInHand(ChazzPrinceton, armedDragonLv10);

            // Put X Head Cannon into hand
            Card xheadCannon = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.XHeadCannon);
            AssertInHand(ChazzPrinceton, xheadCannon);

            // Put Ojama Green into hand
            Card ojamaGreen = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.OjamaGreen);
            AssertInHand(ChazzPrinceton, ojamaGreen);

            // Assert that there is no Armed Dragon Lv7 in hand
            AssertNotInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv7);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Assert that no decision will be presented to the player
            AssertNoDecision();

            // Enter end of TestVillain turn
            GoToEndOfTurn(TestVillain);

            // Enter start of next turn
            GoToStartOfTurn(ChazzPrinceton);

            // Assert that no changes were made in the hand or play area
            QuickHandCheck(0);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertIsInPlayAndNotUnderCard(armedDragonLv5);
        }

        [Test]
        public void AtYourNextStartOfTurn_WithLv7InHandButChooseNotToPlay_DoesNothing()
        {
            // Play Armed Dragon Lv5
            Card armedDragonLv5 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv5);
            AssertIsInPlayAndNotUnderCard(armedDragonLv5);

            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Put Armed Dragon Lv3 into hand
            Card armedDragonLv3 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertInHand(ChazzPrinceton, armedDragonLv3);

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
            IEnumerable<Card> includedCards = new List<Card> { armedDragonLv7 };
            IEnumerable<Card> notIncludedCards = new List<Card> { armedDragonLv3, armedDragonLv5, armedDragonLv10, xheadCannon, ojamaGreen };

            // Assert the player sees the choices we expect.
            // We will be choosing not to play Armed Dragon Lv7
            AssertDecisionIsOptional(SelectionType.PlayCard);
            AssertNextDecisionChoices(includedCards, notIncludedCards);
            DecisionSelectCardToPlay = null;
            DecisionDoNotSelectCard = SelectionType.PlayCard;

            // Enter end of TestVillain turn
            GoToEndOfTurn(TestVillain);

            // Enter start of next turn
            GoToStartOfTurn(ChazzPrinceton);

            // Assert that no changes were made in the hand or play area
            QuickHandCheck(0);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertIsInPlayAndNotUnderCard(armedDragonLv5);
            AssertInHand(ChazzPrinceton, armedDragonLv7);
        }

        [Test]
        public void AtYourNextStartOfTurn_WithLv7InHandAndChooseToPlay_PlaysLv7AndDestroysSelf()
        {
            // Play Armed Dragon Lv5
            Card armedDragonLv5 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv5);
            AssertIsInPlayAndNotUnderCard(armedDragonLv5);

            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Put Armed Dragon Lv3 into hand
            Card armedDragonLv3 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertInHand(ChazzPrinceton, armedDragonLv3);

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
            IEnumerable<Card> includedCards = new List<Card> { armedDragonLv7 };
            IEnumerable<Card> notIncludedCards = new List<Card> { armedDragonLv3, armedDragonLv5, armedDragonLv10, ojamaGreen };

            // Assert the player sees the choices we expect.
            // We will be choosing to play Armed Dragon Lv7
            AssertDecisionIsOptional(SelectionType.PlayCard);
            AssertNextDecisionChoices(includedCards, notIncludedCards);
            DecisionSelectCardToPlay = armedDragonLv7;

            // Enter end of TestVillain turn
            GoToEndOfTurn(TestVillain);

            // Enter start of next turn
            GoToStartOfTurn(ChazzPrinceton);

            // Assert that Armed Dragon Lv7 was played and that Armed Dragon Lv5 was destroyed
            QuickHandCheck(-1);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertIsInPlayAndNotUnderCard(armedDragonLv7);
            AssertInTrash(ChazzPrinceton, armedDragonLv5);
        }

        [Test]
        public void AtYourEndOfTurn_WithMultipleTargetsInPlay_DealsDamageWithDecision()
        {
            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Play Armed Dragon Lv5
            Card armedDragonLv5 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv5);
            AssertIsInPlayAndNotUnderCard(armedDragonLv5);

            // Play ABC Union
            Card abcUnion = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.AbcUnion);
            AssertIsInPlayAndNotUnderCard(abcUnion);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Store the expected target choices
            IEnumerable<Card> includedCards = GameController.FindTargetsInPlay();
            IEnumerable<Card> notIncludedCards = GameController.FindCardsWhere(card => card.IsInPlay && !card.IsTarget);

            // Assert that we see the expected choices.
            // We will deal damage to the TestVillain
            AssertNextDecisionChoices(includedCards, notIncludedCards);
            DecisionSelectCard = TestVillain.CharacterCard;

            // Enter end of turn
            GoToEndOfTurn(ChazzPrinceton);

            // Assert that no changes were made in the hand or play area
            QuickHandCheck(0);

            AssertNumberOfCardsInPlay(ChazzPrinceton, 3);
            AssertIsInPlayAndNotUnderCard(armedDragonLv5);
            AssertIsInPlayAndNotUnderCard(abcUnion);

            AssertNumberOfCardsInPlay(TestHero1, 4);
            AssertNumberOfCardsInPlay(TestHero2, 4);
            AssertNumberOfCardsInPlay(TestVillain, 4);
            AssertNumberOfCardsInPlay(TestEnvironment, 3);

            // Assert that all targets other than the villain are still at max health
            foreach (Card target in includedCards)
            {
                if (target.Identifier.Equals(TestVillainConstants.Villain))
                {
                    // Assert that the villain character took 2 damage
                    AssertHitPoints(TestVillain.CharacterCard, TestVillain.CharacterCard.MaximumHitPoints.Value - 2);
                }

                AssertHitPoints(target, target.MaximumHitPoints.Value);
            }
        }

        [Test]
        public void AtOtherEndOfTurn_DealsNoDamage()
        {
            // Go to start of TestHero1 turn
            GoToStartOfTurn(TestHero1);

            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Play Armed Dragon Lv5
            Card armedDragonLv5 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv5);
            AssertIsInPlayAndNotUnderCard(armedDragonLv5);

            // Play ABC Union
            Card abcUnion = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.AbcUnion);
            AssertIsInPlayAndNotUnderCard(abcUnion);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Assert that there is no choice of target to deal damage to
            AssertNoDecision();

            // Enter end of TestHero1 turn
            GoToEndOfTurn(TestHero1);

            // Assert that no changes were made in the hand or play area
            QuickHandCheck(0);

            AssertNumberOfCardsInPlay(ChazzPrinceton, 3);
            AssertIsInPlayAndNotUnderCard(armedDragonLv5);
            AssertIsInPlayAndNotUnderCard(abcUnion);

            AssertNumberOfCardsInPlay(TestHero1, 4);
            AssertNumberOfCardsInPlay(TestHero2, 4);
            AssertNumberOfCardsInPlay(TestVillain, 4);
            AssertNumberOfCardsInPlay(TestEnvironment, 3);

            // Assert that all targets including the villain are still at max health
            foreach (Card target in GameController.FindTargetsInPlay())
            {
                AssertHitPoints(target, target.MaximumHitPoints.Value);
            }
        }
    }
}
