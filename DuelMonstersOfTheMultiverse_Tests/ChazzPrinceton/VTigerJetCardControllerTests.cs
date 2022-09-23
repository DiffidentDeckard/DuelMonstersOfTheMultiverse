using System.Collections.Generic;
using DeckardBaseMod;
using DMotM;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using NUnit.Framework;

namespace DMotMTests.ChazzPrinceton
{
    [TestFixture]
    public class VTigerJetCardControllerTests : ChazzPrincetonBaseTest
    {
        [Test]
        public void HasAbcKeyword()
        {
            // Put V Tiger Jet into hand
            Card vTigerJet = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.VTigerJet);
            AssertInHand(ChazzPrinceton, vTigerJet);

            // Assert that vTigerJet has the ABC keyword
            AssertCardHasKeyword(vTigerJet, ChazzPrincetonConstants.ABC, false);
        }

        [Test]
        public void IsATargetWith4MaxHP()
        {
            // Put V Tiger Jet into hand
            Card vTigerJet = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.VTigerJet);
            AssertInHand(ChazzPrinceton, vTigerJet);

            // Assert that V Tiger Jet is a target
            AssertIsTarget(vTigerJet);

            // Assert that the Maximum Hit Points is equal to 4
            AssertMaximumHitPoints(vTigerJet, 4);
        }

        [Test]
        public void AtYourEndOfTurn_WithMultipleTargetsInPlay_DealsDamageWithDecision()
        {
            // Play V Tiger Jet
            Card vTigerJet = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.VTigerJet);
            AssertIsInPlayAndNotUnderCard(vTigerJet);

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
            AssertIsInPlayAndNotUnderCard(vTigerJet);

            AssertNumberOfCardsInPlay(TestHero1, 4);
            AssertNumberOfCardsInPlay(TestHero2, 4);
            AssertNumberOfCardsInPlay(TestVillain, 4);
            AssertNumberOfCardsInPlay(TestEnvironment, 3);

            // Assert that all targets other than the villain are still at max health
            foreach (Card target in includedCards)
            {
                if (target.Identifier.Equals(TestVillainConstants.Villain))
                {
                    // Assert that the villain character took 1 damage
                    AssertHitPoints(TestVillain.CharacterCard, TestVillain.CharacterCard.MaximumHitPoints.Value - 1);
                }

                AssertHitPoints(target, target.MaximumHitPoints.Value);
            }
        }

        [Test]
        public void AtOtherEndOfTurn_DealsNoDamage()
        {
            // Play V Tiger Jet
            Card vTigerJet = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.VTigerJet);
            AssertIsInPlayAndNotUnderCard(vTigerJet);

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
            AssertIsInPlayAndNotUnderCard(vTigerJet);

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
