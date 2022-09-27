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
            AssertIsInPlayAndNotUnderCard(xHeadCannon);

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
            AssertIsInPlayAndNotUnderCard(xHeadCannon);

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
            AssertIsInPlayAndNotUnderCard(xHeadCannon);

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
            AssertIsInPlayAndNotUnderCard(xHeadCannon);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();

            // Assert that all targets including the villain are still at max health
            foreach (Card target in GameController.FindTargetsInPlay())
            {
                AssertHitPoints(target, target.MaximumHitPoints.Value);
            }
        }
    }
}
