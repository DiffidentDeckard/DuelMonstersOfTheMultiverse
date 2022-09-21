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
    public class ArmedDragonLv10CardControllerTests : BaseTest
    {
        [Test]
        public void HasArmedKeyword()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Legacy", "Megalopolis");

            // Put Armed Dragon Lv10 into hand
            Card armedDragonLv10 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertInHand(ChazzPrinceton, armedDragonLv10);

            // Assert that Armed Dragon Lv10 has the Armed keyword
            AssertCardHasKeyword(armedDragonLv10, ChazzPrincetonConstants.Armed, false);
        }

        [Test]
        public void IsATargetWith10MaxHP()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Legacy", "Megalopolis");

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
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Legacy", "Megalopolis");
            StartGame();
            DestroyNonCharacterVillainCards();

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
        }

        [Test]
        public void DuringYourNotStartOfTurnPhases_CannotBePlayed(
            [Values(Phase.PlayCard, Phase.UsePower, Phase.DrawCard, Phase.End)] Phase phase)
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Legacy", "Megalopolis");
            StartGame();
            DestroyNonCharacterVillainCards();

            // Put Armed Dragon Lv10 into hand
            Card armedDragonLv10 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertInHand(ChazzPrinceton, armedDragonLv10);

            // Enter chazz phase
            GoToTurnTakerPhase(ChazzPrinceton, phase);

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
        }

        [Test]
        public void DuringOtherPlayerTurn_CannotBePlayed(
            [ValueSource(nameof(GetTestTurnPhases))] Phase phase)
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Legacy", "Megalopolis");
            StartGame();
            DestroyNonCharacterVillainCards();

            // Put Armed Dragon Lv10 into hand
            Card armedDragonLv10 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertInHand(ChazzPrinceton, armedDragonLv10);

            // Enter legacy phase
            GoToTurnTakerPhase(legacy, phase);

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

            // Play Armed Dragon Lv10
            GoToStartOfTurn(ChazzPrinceton);
            Card armedDragonLv10 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertIsInPlayAndNotUnderCard(armedDragonLv10);

            // Play ABC Union
            Card abcUnion = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.AbcUnion);
            AssertIsInPlayAndNotUnderCard(abcUnion);

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
            IEnumerable<Card> notIncludedCards = new List<Card> { abcUnion, allAccordingToPlan };

            // Assert that we see the expected choices.
            // We will deal damage to the villain
            AssertNextDecisionChoices(includedCards, notIncludedCards);
            DecisionSelectCard = baron.CharacterCard;

            // Enter end of turn
            GoToEndOfTurn(ChazzPrinceton);

            // Assert that no changes were made in the hand or play area
            QuickHandCheck(0);

            AssertNumberOfCardsInPlay(ChazzPrinceton, 3);
            AssertIsInPlayAndNotUnderCard(armedDragonLv10);
            AssertIsInPlayAndNotUnderCard(abcUnion);

            AssertNumberOfCardsInPlay(luminary, 3);
            AssertIsInPlayAndNotUnderCard(allAccordingToPlan);
            AssertIsInPlayAndNotUnderCard(regressionTurret);

            // Assert that all targets other than the villain are still at max health
            AssertHitPoints(ChazzPrinceton.CharacterCard, ChazzPrinceton.CharacterCard.MaximumHitPoints.Value);
            AssertHitPoints(armedDragonLv10, armedDragonLv10.MaximumHitPoints.Value);

            AssertHitPoints(luminary.CharacterCard, luminary.CharacterCard.MaximumHitPoints.Value);
            AssertHitPoints(regressionTurret, regressionTurret.MaximumHitPoints.Value);

            AssertHitPoints(trafficPileup, trafficPileup.MaximumHitPoints.Value);

            AssertHitPoints(elementalRedistributor, elementalRedistributor.MaximumHitPoints.Value);

            // Assert that the villain character took 4 damage
            AssertHitPoints(baron.CharacterCard, baron.CharacterCard.MaximumHitPoints.Value - 4);
        }

        [Test]
        public void AtOtherEndOfTurn_DealsNoDamage()
        {
            // Setup a sample game with Chazz Princeton and Luminary, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Luminary", "Megalopolis");
            StartGame();
            DestroyNonCharacterVillainCards();

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards =
                ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Play Armed Dragon Lv10
            GoToStartOfTurn(ChazzPrinceton);
            Card armedDragonLv10 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertIsInPlayAndNotUnderCard(armedDragonLv10);

            // Play ABC Union
            Card abcUnion = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.AbcUnion);
            AssertIsInPlayAndNotUnderCard(abcUnion);

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

            // Enter end of turn to go through the damage part, we'll skip the damage
            DecisionDoNotSelectCard = SelectionType.DealDamage;
            GoToEndOfTurn(ChazzPrinceton);

            // just in case any damage happened, restore all to max health
            SetAllTargetsToMaxHP();

            // Once we have skipped the damage dealing, assert that no more decisions will be presented to player
            AssertNoDecision();

            // Enter end of luminary turn
            GoToEndOfTurn(luminary);

            // Assert that no changes were made in the hand or play area
            QuickHandCheck(0);

            AssertNumberOfCardsInPlay(ChazzPrinceton, 3);
            AssertIsInPlayAndNotUnderCard(armedDragonLv10);
            AssertIsInPlayAndNotUnderCard(abcUnion);

            AssertNumberOfCardsInPlay(luminary, 3);
            AssertIsInPlayAndNotUnderCard(allAccordingToPlan);
            AssertIsInPlayAndNotUnderCard(regressionTurret);

            // Assert that all targets including the villain are still at max health
            AssertHitPoints(ChazzPrinceton.CharacterCard, ChazzPrinceton.CharacterCard.MaximumHitPoints.Value);
            AssertHitPoints(armedDragonLv10, armedDragonLv10.MaximumHitPoints.Value);

            AssertHitPoints(luminary.CharacterCard, luminary.CharacterCard.MaximumHitPoints.Value);
            AssertHitPoints(regressionTurret, regressionTurret.MaximumHitPoints.Value);

            AssertHitPoints(trafficPileup, trafficPileup.MaximumHitPoints.Value);

            AssertHitPoints(baron.CharacterCard, baron.CharacterCard.MaximumHitPoints.Value);
            AssertHitPoints(elementalRedistributor, elementalRedistributor.MaximumHitPoints.Value);
        }

        [Test]
        public void Usepower_DiscardsCardAndDealsAllNonHeroTargets4Damage()
        {
            // Setup a sample game with Chazz Princeton and Luminary, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Luminary", "Megalopolis");
            StartGame();
            DestroyNonCharacterVillainCards();

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards =
                ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Play Armed Dragon Lv10
            GoToStartOfTurn(ChazzPrinceton);
            Card armedDragonLv10 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertIsInPlayAndNotUnderCard(armedDragonLv10);

            // Play ABC Union
            Card abcUnion = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.AbcUnion);
            AssertIsInPlayAndNotUnderCard(abcUnion);

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

            // Assert that no changes were made in the play area
            AssertNumberOfCardsInPlay(ChazzPrinceton, 3);
            AssertIsInPlayAndNotUnderCard(armedDragonLv10);
            AssertIsInPlayAndNotUnderCard(abcUnion);

            AssertNumberOfCardsInPlay(luminary, 3);
            AssertIsInPlayAndNotUnderCard(allAccordingToPlan);
            AssertIsInPlayAndNotUnderCard(regressionTurret);

            // Assert that all hero targets are at max health
            AssertHitPoints(ChazzPrinceton.CharacterCard, ChazzPrinceton.CharacterCard.MaximumHitPoints.Value);
            AssertHitPoints(armedDragonLv10, armedDragonLv10.MaximumHitPoints.Value);

            AssertHitPoints(luminary.CharacterCard, luminary.CharacterCard.MaximumHitPoints.Value);
            AssertHitPoints(regressionTurret, regressionTurret.MaximumHitPoints.Value);

            // Assert that all non-hero targets took 4 damage
            AssertHitPoints(trafficPileup, trafficPileup.MaximumHitPoints.Value - 4);

            AssertHitPoints(baron.CharacterCard, baron.CharacterCard.MaximumHitPoints.Value - 4);
            AssertHitPoints(elementalRedistributor, elementalRedistributor.MaximumHitPoints.Value - 4);
        }
    }
}
