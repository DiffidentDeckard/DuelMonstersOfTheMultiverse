using System.Collections;
using System.Collections.Generic;
using DMotM;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using NUnit.Framework;

namespace DMotMTests.ChazzPrinceton
{
    [TestFixture]
    public class XHeadCannonCardControllerTests : ChazzPrincetonBaseTest
    {
        [Test]
        public void HasAbcKeyword()
        {
            // Put X Head Cannon into hand
            Card xHeadCannon = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.XHeadCannon);
            AssertInHand(ChazzPrinceton, xHeadCannon);

            // Assert that X Head Cannon  has the ABC keyword
            AssertCardHasKeyword(xHeadCannon, ChazzPrincetonConstants.ABC, false);
        }

        [Test]
        public void IsATargetWith4MaxHP()
        {
            // Put X Head Cannon into hand
            Card xHeadCannon = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.XHeadCannon);
            AssertInHand(ChazzPrinceton, xHeadCannon);

            // Assert that X Head Cannon  is a target
            AssertIsTarget(xHeadCannon);

            // Assert that the Maximum Hit Points is equal to 4
            AssertMaximumHitPoints(xHeadCannon, 4);
        }

        [Test]
        public void IsLimited()
        {
            // Put X Head Cannon into hand
            Card xHeadCannon = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.XHeadCannon);
            AssertInHand(ChazzPrinceton, xHeadCannon);

            // Assert that X Head Cannon  is Limited
            Assert.That(xHeadCannon.IsLimited, Is.True);
        }

        [Test]
        public void AtYourEndOfTurn_WithMultipleTargetsInPlay_DealsDamageWithDecision()
        {
            // Play X Head Cannon
            Card xHeadCannon = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.XHeadCannon);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, xHeadCannon);

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

            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, xHeadCannon);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();

            // For each target in play...
            foreach (Card target in includedCards)
            {
                // If it is the villain character card...
                if (target.IsVillainCharacterCard)
                {
                    // Assert that it took 1 damage
                    AssertHitPoints(TestVillain.CharacterCard, TestVillain.CharacterCard.MaximumHitPoints.Value - 1);
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
            // Play X Head Cannon
            Card xHeadCannon = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.XHeadCannon);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, xHeadCannon);

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

            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, xHeadCannon);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();

            // Assert that all targets including the villain are still at max health
            foreach (Card target in GameController.FindTargetsInPlay())
            {
                AssertHitPoints(target, target.MaximumHitPoints.Value);
            }
        }

        [Test]
        public void UsePower_WithNoYInPlay_AppliesNoIrreducibleStatusEffect()
        {
            // Play X Head Cannon
            Card xHeadCannon = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.XHeadCannon);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, xHeadCannon);

            // Assert that Y Dragon Head is not in the play area
            AssertNotInPlayArea(ChazzPrinceton, ChazzPrincetonConstants.YDragonHead);

            // Go to Chazz Princeton Use Power Phase
            GoToUsePowerPhase(ChazzPrinceton);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Assert that no decision will be presented to the player
            AssertNoDecision();

            // Use X Head Cannon power
            UsePower(xHeadCannon);

            // Assert that no status effect was applied
            AssertNumberOfStatusEffectsInPlay(0);

            // Apply a DR status effect to Test Villain
            ReduceDamageStatusEffect rdse = new ReduceDamageStatusEffect(1);
            rdse.TargetCriteria.IsSpecificCard = TestVillain.CharacterCard;
            IEnumerator ase = GameController.AddStatusEffect(rdse, false, TestVillain.CharacterCardController.GetCardSource());
            GameController.ExhaustCoroutine(ase);
            AssertNumberOfStatusEffectsInPlay(1);

            // Attempt to deal 2 damage to Test Villain
            DealDamage(ChazzPrinceton.CharacterCard, TestVillain.CharacterCard, 2, DamageType.Energy);

            // For each target in play...
            foreach (Card target in GameController.FindTargetsInPlay())
            {
                // If it is Test Villain...
                if (target.IsVillainCharacterCard)
                {
                    // Assert that reduced damage was dealt
                    AssertHitPoints(target, target.MaximumHitPoints.Value - 1);
                }
                // For any other target...
                else
                {
                    // Assert that the target is at max health
                    AssertHitPoints(target, target.MaximumHitPoints.Value);
                }
            }

            // Assert that no changes were made in the hand or play area
            QuickHandCheck(0);

            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, xHeadCannon);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();
        }

        [Test]
        public void UsePower_WithYInPlay_AppliesIrreducibleStatusEffect()
        {
            // Play X Head Cannon
            Card xHeadCannon = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.XHeadCannon);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, xHeadCannon);

            // Play Y Dragon Head
            Card yDragonHead = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.YDragonHead);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, yDragonHead);

            // Go to Chazz Princeton Use Power Phase
            GoToUsePowerPhase(ChazzPrinceton);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Store the expected target choices
            IEnumerable<Card> includedCards = GameController.FindTargetsInPlay();
            IEnumerable<Card> notIncludedCards = GameController.FindCardsWhere(card => card.IsInPlay && !card.IsTarget);

            // Assert that we see the expected choices.
            // We will deal damage to the TestVillain
            AssertNextDecisionChoices(includedCards, notIncludedCards);
            DecisionSelectCard = TestVillain.CharacterCard;

            // Use X Head Cannon power
            UsePower(xHeadCannon);

            // Assert that one status effect was applied
            AssertNumberOfStatusEffectsInPlay(1);

            // Apply a DR status effect to Test Villain
            ReduceDamageStatusEffect rdse = new ReduceDamageStatusEffect(1);
            rdse.TargetCriteria.IsSpecificCard = TestVillain.CharacterCard;
            IEnumerator ase = GameController.AddStatusEffect(rdse, false, TestVillain.CharacterCardController.GetCardSource());
            GameController.ExhaustCoroutine(ase);
            AssertNumberOfStatusEffectsInPlay(2);

            // Attempt to deal 2 damage to Test Villain
            DealDamage(ChazzPrinceton.CharacterCard, TestVillain.CharacterCard, 2, DamageType.Energy);

            // For each target in play...
            foreach (Card target in GameController.FindTargetsInPlay())
            {
                // If it is Test Villain...
                if (target.IsVillainCharacterCard)
                {
                    // Assert that full damage was dealt
                    AssertHitPoints(target, target.MaximumHitPoints.Value - 2);
                }
                // For any other target...
                else
                {
                    // Assert that the target is at max health
                    AssertHitPoints(target, target.MaximumHitPoints.Value);
                }
            }

            // Assert that no changes were made in the hand or play area
            QuickHandCheck(0);

            AssertNumberOfCardsInPlay(ChazzPrinceton, 3);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, xHeadCannon);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, yDragonHead);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();
        }

        [Test]
        public void UsePower_WithNoZInPlay_DoesNotLetYouUseAnotherPower()
        {
            // Play X Head Cannon
            Card xHeadCannon = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.XHeadCannon);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, xHeadCannon);

            // Assert that Z Metal Tank is not in the play area
            AssertNotInPlayArea(ChazzPrinceton, ChazzPrincetonConstants.ZMetalTank);

            // Go to Chazz Princeton Use Power Phase
            GoToUsePowerPhase(ChazzPrinceton);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Assert that no decision will be presented to the player
            AssertNoDecision();

            // Use X Head Cannon power
            UsePower(xHeadCannon);
        }
    }
}
