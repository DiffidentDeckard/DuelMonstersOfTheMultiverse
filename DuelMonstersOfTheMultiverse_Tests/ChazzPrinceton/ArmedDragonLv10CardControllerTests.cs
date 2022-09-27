using System.Collections.Generic;
using DMotM;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using NUnit.Framework;

namespace DMotMTests.ChazzPrinceton
{
    [TestFixture]
    public class ArmedDragonLv10CardControllerTests : ChazzPrincetonBaseTest
    {
        [Test]
        public void HasArmedKeyword()
        {
            // Put Armed Dragon Lv10 into hand
            Card armedDragonLv10 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertInHand(ChazzPrinceton, armedDragonLv10);

            // Assert that Armed Dragon Lv10 has the Armed keyword
            AssertCardHasKeyword(armedDragonLv10, ChazzPrincetonConstants.Armed, false);
        }

        [Test]
        public void IsATargetWith10MaxHP()
        {
            // Put Armed Dragon Lv10 into hand
            Card armedDragonLv10 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertInHand(ChazzPrinceton, armedDragonLv10);

            // Assert that Armed Dragon Lv10 is a target
            AssertIsTarget(armedDragonLv10);

            // Assert that the Maximum Hit Points is equal to 10
            AssertMaximumHitPoints(armedDragonLv10, 10);
        }

        [Test]
        public void DuringYourStartOfTurn_CanBePlayed()
        {
            // Put Armed Dragon Lv10 into hand
            Card armedDragonLv10 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertInHand(ChazzPrinceton, armedDragonLv10);

            // Enter start of turn
            GoToStartOfTurn(ChazzPrinceton);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Assert that no decision will be presented to the player
            AssertNoDecision();

            // Play Armed Dragon Lv10
            PlayCardFromHand(ChazzPrinceton, armedDragonLv10.Identifier);

            // Assert that Armed Dragon Lv10 was played
            QuickHandCheck(-1);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertIsInPlayAndNotUnderCard(armedDragonLv10);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();
        }

        [Test]
        public void DuringYourNotStartOfTurnPhases_CannotBePlayed(
            [Values(Phase.PlayCard, Phase.UsePower, Phase.DrawCard, Phase.End)] Phase phase)
        {
            // Put Armed Dragon Lv10 into hand
            Card armedDragonLv10 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertInHand(ChazzPrinceton, armedDragonLv10);

            // Enter Chazz Princeton phase
            GoToPhase(ChazzPrinceton, phase);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Assert that no decision will be presented to the player
            AssertNoDecision();

            // Attempt to play Armed Dragon Lv10
            PlayCardFromHand(ChazzPrinceton, armedDragonLv10.Identifier);

            // Assert that Armed Dragon Lv10 was not played
            QuickHandCheck(0);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 1);
            AssertInHand(armedDragonLv10);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();
        }

        [Test]
        public void DuringOtherPlayerTurn_CannotBePlayed(
            [ValueSource(nameof(GetTestTurnPhases))] Phase phase)
        {
            // Put Armed Dragon Lv10 into hand
            Card armedDragonLv10 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertInHand(ChazzPrinceton, armedDragonLv10);

            // Enter TestHero1 phase
            GoToPhase(TestHero1, phase);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Assert that no decision will be presented to the player
            AssertNoDecision();

            // Attempt to play Armed Dragon Lv10
            PlayCardFromHand(ChazzPrinceton, armedDragonLv10.Identifier);

            // Assert that Armed Dragon Lv10 was not played
            QuickHandCheck(0);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 1);
            AssertInHand(armedDragonLv10);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();
        }

        [Test]
        public void AtYourEndOfTurn_WithMultipleTargetsInPlay_DealsDamageWithDecision()
        {
            // Play Armed Dragon Lv10
            GoToStartOfTurn(ChazzPrinceton);
            Card armedDragonLv10 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertIsInPlayAndNotUnderCard(armedDragonLv10);

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
            AssertIsInPlayAndNotUnderCard(armedDragonLv10);
            AssertIsInPlayAndNotUnderCard(abcUnion);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();

            // For each target in play...
            foreach (Card target in includedCards)
            {
                // If it is the villain character card...
                if (target.IsVillainCharacterCard)
                {
                    // Assert that it took 4 damage
                    AssertHitPoints(TestVillain.CharacterCard, TestVillain.CharacterCard.MaximumHitPoints.Value - 4);
                }
                else
                {
                    // Assert that it took 0 damage
                    AssertHitPoints(target, target.MaximumHitPoints.Value);
                }
            }
        }

        [Test]
        public void AtOtherEndOfTurn_DealsNoDamage()
        {
            // Play Armed Dragon Lv10
            GoToStartOfTurn(ChazzPrinceton);
            Card armedDragonLv10 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertIsInPlayAndNotUnderCard(armedDragonLv10);

            // Play ABC Union
            Card abcUnion = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.AbcUnion);
            AssertIsInPlayAndNotUnderCard(abcUnion);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Enter end of turn to go through the damage part, we'll skip the damage
            DecisionDoNotSelectCard = SelectionType.DealDamage;
            GoToEndOfTurn(ChazzPrinceton);

            // Just in case any damage happened, restore all to max health
            SetAllTargetsToMaxHP();

            // Once we have skipped the damage dealing, assert that no more decisions will be presented to player
            AssertNoDecision();

            // Enter end of TestHero1 turn
            GoToEndOfTurn(TestHero1);

            // Assert that no changes were made in the hand or play area
            QuickHandCheck(0);

            AssertNumberOfCardsInPlay(ChazzPrinceton, 3);
            AssertIsInPlayAndNotUnderCard(armedDragonLv10);
            AssertIsInPlayAndNotUnderCard(abcUnion);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();

            // Assert that all targets including the villain are still at max health
            foreach (Card target in GameController.FindTargetsInPlay())
            {
                AssertHitPoints(target, target.MaximumHitPoints.Value);
            }
        }

        [Test]
        public void Usepower_DiscardsCardAndDealsAllNonHeroTargets4Damage()
        {
            // Play Armed Dragon Lv10
            GoToStartOfTurn(ChazzPrinceton);
            Card armedDragonLv10 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertIsInPlayAndNotUnderCard(armedDragonLv10);

            // Play ABC Union
            Card abcUnion = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.AbcUnion);
            AssertIsInPlayAndNotUnderCard(abcUnion);

            // Added Armed Dragon Lv3 to our hand just so we have a card to discard
            Card armedDragonLv3 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertInHand(ChazzPrinceton, armedDragonLv3);

            // Enter the use power phase
            GoToUsePowerPhase(ChazzPrinceton);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Assert that all cards in our hand are discard options
            // We will discard Armed Dragon Lv3
            IEnumerable<Card> includedCards = GameController.GetAllCardsInHand(ChazzPrinceton);
            IEnumerable<Card> notIncludedCards = GameController.FindCardsWhere(card => card.IsInPlayAndNotUnderCard, true);
            DecisionSelectCard = armedDragonLv3;

            // Use Armed Dragon Lv10's power
            UsePower(armedDragonLv10);

            // Assert that we discarded Armed Dragon Lv3
            QuickHandCheck(-1);
            AssertInTrash(armedDragonLv3);

            // Assert that no changes were made in the hand or play area
            QuickHandCheck(0);

            AssertNumberOfCardsInPlay(ChazzPrinceton, 3);
            AssertIsInPlayAndNotUnderCard(armedDragonLv10);
            AssertIsInPlayAndNotUnderCard(abcUnion);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();

            // For each target in play...
            foreach (Card target in GameController.FindTargetsInPlay())
            {
                // If it is a Hero target...
                if (target.IsHero)
                {
                    // Assert that this target is at max HP
                    AssertHitPoints(target, target.MaximumHitPoints.Value);
                }
                // If it is a non-Hero target...
                else
                {
                    // Assert that this target took 4 damage from Armed Dragon Lv10's power
                    AssertHitPoints(target, target.MaximumHitPoints.Value - 4);
                }
            }
        }
    }
}
