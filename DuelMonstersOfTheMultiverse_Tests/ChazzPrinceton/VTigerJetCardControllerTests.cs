using System.Collections.Generic;
using System.Linq;
using DMotM;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.UnitTest;
using NUnit.Framework;

namespace DMotMTests.ChazzPrinceton
{
    [TestFixture]
    public class VTigerJetCardControllerTests : BaseTest
    {
        [Test]
        public void HasAbcKeyword()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Legacy", "Megalopolis");

            // Put V Tiger Jet into hand
            Card vTigerJet = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.VTigerJet);
            AssertInHand(ChazzPrinceton, vTigerJet);

            // Assert that vTigerJet has the ABC keyword
            AssertCardHasKeyword(vTigerJet, ChazzPrincetonConstants.ABC, false);
        }

        [Test]
        public void IsATargetWith4MaxHP()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Legacy", "Megalopolis");

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
            // Setup a sample game with Chazz Princeton and Luminary, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Luminary", "Megalopolis");
            StartGame();
            DestroyNonCharacterVillainCards();

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards =
                ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Play V Tiger Jet
            Card vTigerJet = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.VTigerJet);
            AssertIsInPlayAndNotUnderCard(vTigerJet);

            // Play All According To Plan
            Card allAccordingToPlan = PlayCard(luminary, "AllAccordingToPlan");
            AssertIsInPlayAndNotUnderCard(allAccordingToPlan);

            // Play Regression Turret
            Card regressionTurret = PlayCard(luminary, "RegressionTurret");
            AssertIsInPlayAndNotUnderCard(regressionTurret);

            // Play Traffic Pileup
            Card trafficPileup = PlayCard(FindEnvironment(), "TrafficPileup");
            AssertIsInPlayAndNotUnderCard(trafficPileup);

            // Play Elemental Redistributor
            Card elementalRedistributor = PlayCard(FindEnvironment(), "ElementalRedistributor");
            AssertIsInPlayAndNotUnderCard(elementalRedistributor);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Store the expected target choices
            IEnumerable<Card> includedCards = GameController.FindTargetsInPlay();
            IEnumerable<Card> notIncludedCards = new List<Card> { allAccordingToPlan };

            // Assert that we see the expected choices.
            // We will deal damage to the villain
            AssertNextDecisionChoices(includedCards, notIncludedCards);
            DecisionSelectCard = baron.CharacterCard;

            // Enter end of turn
            GoToEndOfTurn(ChazzPrinceton);

            // Assert that no changes were made in the hand or play area
            QuickHandCheck(0);

            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertIsInPlayAndNotUnderCard(vTigerJet);

            AssertNumberOfCardsInPlay(luminary, 3);
            AssertIsInPlayAndNotUnderCard(allAccordingToPlan);
            AssertIsInPlayAndNotUnderCard(regressionTurret);

            // Assert that all targets other than the villain are still at max health
            AssertHitPoints(ChazzPrinceton.CharacterCard, ChazzPrinceton.CharacterCard.MaximumHitPoints.Value);
            AssertHitPoints(vTigerJet, vTigerJet.MaximumHitPoints.Value);

            AssertHitPoints(luminary.CharacterCard, luminary.CharacterCard.MaximumHitPoints.Value);
            AssertHitPoints(regressionTurret, regressionTurret.MaximumHitPoints.Value);

            AssertHitPoints(trafficPileup, trafficPileup.MaximumHitPoints.Value);

            AssertHitPoints(elementalRedistributor, elementalRedistributor.MaximumHitPoints.Value);

            // Assert that the villain character took 1 damage
            AssertHitPoints(baron.CharacterCard, baron.CharacterCard.MaximumHitPoints.Value - 1);
        }

        [Test]
        public void AtOtherEndOfTurn_DealsNoDamage()
        {
            // Setup a sample game with Chazz Princeton and Luminary, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Luminary", "Megalopolis");
            StartGame();
            DestroyNonCharacterVillainCards();

            // Go to start of Luminary turn
            GoToStartOfTurn(luminary);

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards =
                ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Play V Tiger Jet
            Card vTigerJet = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.VTigerJet);
            AssertIsInPlayAndNotUnderCard(vTigerJet);

            // Play All According To Plan
            Card allAccordingToPlan = PlayCard(luminary, "AllAccordingToPlan");
            AssertIsInPlayAndNotUnderCard(allAccordingToPlan);

            // Play Regression Turret
            Card regressionTurret = PlayCard(luminary, "RegressionTurret");
            AssertIsInPlayAndNotUnderCard(regressionTurret);

            // Play Traffic Pileup
            Card trafficPileup = PlayCard(FindEnvironment(), "TrafficPileup");
            AssertIsInPlayAndNotUnderCard(trafficPileup);

            // Play Elemental Redistributor
            Card elementalRedistributor = PlayCard(FindEnvironment(), "ElementalRedistributor");
            AssertIsInPlayAndNotUnderCard(elementalRedistributor);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Assert that there is no choice of target to deal damage to
            AssertNoDecision();

            // Enter end of luminary turn
            GoToEndOfTurn(luminary);

            // Assert that no changes were made in the hand or play area
            QuickHandCheck(0);

            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertIsInPlayAndNotUnderCard(vTigerJet);

            AssertNumberOfCardsInPlay(luminary, 3);
            AssertIsInPlayAndNotUnderCard(allAccordingToPlan);
            AssertIsInPlayAndNotUnderCard(regressionTurret);

            // Assert that all targets including the villain are still at max health
            AssertHitPoints(ChazzPrinceton.CharacterCard, ChazzPrinceton.CharacterCard.MaximumHitPoints.Value);
            AssertHitPoints(vTigerJet, vTigerJet.MaximumHitPoints.Value);

            AssertHitPoints(luminary.CharacterCard, luminary.CharacterCard.MaximumHitPoints.Value);
            AssertHitPoints(regressionTurret, regressionTurret.MaximumHitPoints.Value);

            AssertHitPoints(trafficPileup, trafficPileup.MaximumHitPoints.Value);

            AssertHitPoints(baron.CharacterCard, baron.CharacterCard.MaximumHitPoints.Value);
            AssertHitPoints(elementalRedistributor, elementalRedistributor.MaximumHitPoints.Value);
        }
    }
}
