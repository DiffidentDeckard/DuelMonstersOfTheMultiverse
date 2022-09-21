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
    public class ArmedDragonLv7CardControllerTests : BaseTest
    {
        [Test]
        public void HasArmedKeyword()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Legacy", "Megalopolis");

            // Put Armed Dragon Lv7 into hand
            Card armedDragonLv7 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv7);
            AssertInHand(ChazzPrinceton, armedDragonLv7);

            // Assert that Armed Dragon Lv7 has the Armed keyword
            AssertCardHasKeyword(armedDragonLv7, ChazzPrincetonConstants.Armed, false);
        }

        [Test]
        public void IsATargetWith7MaxHP()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Legacy", "Megalopolis");

            // Put Armed Dragon Lv7 into hand
            Card armedDragonLv7 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv7);
            AssertInHand(ChazzPrinceton, armedDragonLv7);

            // Assert that Armed Dragon Lv7 is a target
            AssertIsTarget(armedDragonLv7);

            // Assert that the Maximum Hit Points is equal to 7
            AssertMaximumHitPoints(armedDragonLv7, 7);
        }

        [Test]
        public void DuringYourStartDrawCardEndPhases_CanBePlayed(
            [Values(Phase.Start, Phase.DrawCard, Phase.End)] Phase phase)
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Legacy", "Megalopolis");
            StartGame();
            DestroyNonCharacterVillainCards();

            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Put Armed Dragon Lv7 into hand
            Card armedDragonLv7 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv7);
            AssertInHand(ChazzPrinceton, armedDragonLv7);

            // Enter chazz phase
            GoToTurnTakerPhase(ChazzPrinceton, phase);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Assert that no decision will be presented to the player
            AssertNoDecision();

            // Play Armed Dragon Lv7
            PlayCardFromHand(ChazzPrinceton, armedDragonLv7.Identifier);

            // Assert that Armed Dragon Lv7 was played
            QuickHandCheck(-1);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertIsInPlayAndNotUnderCard(armedDragonLv7);
        }

        [Test]
        public void DuringYourPlayCardUsePowerPhases_CannotBePlayed(
            [Values(Phase.PlayCard, Phase.UsePower)] Phase phase)
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Legacy", "Megalopolis");
            StartGame();
            DestroyNonCharacterVillainCards();

            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Put Armed Dragon Lv7 into hand
            Card armedDragonLv7 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv7);
            AssertInHand(ChazzPrinceton, armedDragonLv7);

            // Enter chazz phase
            GoToTurnTakerPhase(ChazzPrinceton, phase);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Assert that no decision will be presented to the player
            AssertNoDecision();

            // Attempt to play Armed Dragon Lv7
            PlayCardFromHand(ChazzPrinceton, armedDragonLv7.Identifier);

            // Assert that Armed Dragon Lv7 was not played
            QuickHandCheck(0);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 1);
            AssertInHand(armedDragonLv7);
        }

        [Test]
        public void DuringOtherPlayerTurn_CanBePlayed(
            [ValueSource(nameof(GetTestTurnPhases))] Phase phase)
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Legacy", "Megalopolis");
            StartGame();
            DestroyNonCharacterVillainCards();

            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Put Armed Dragon Lv7 into hand
            Card armedDragonLv7 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv7);
            AssertInHand(ChazzPrinceton, armedDragonLv7);

            // Enter legacy phase
            GoToTurnTakerPhase(legacy, phase);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Assert that no decision will be presented to the player
            AssertNoDecision();

            // Play Armed Dragon Lv7
            PlayCardFromHand(ChazzPrinceton, armedDragonLv7.Identifier);

            // Assert that Armed Dragon Lv7 was played
            QuickHandCheck(-1);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertIsInPlayAndNotUnderCard(armedDragonLv7);
        }

        [Test]
        public void AtYourSameStartOfTurn_WithLv10InHand_DoesNothing()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Legacy", "Megalopolis");
            StartGame();
            DestroyNonCharacterVillainCards();

            // Enter our Start of turn immediately, to ensure that Armed Dragon Lv7 does not activate
            GoToStartOfTurn(ChazzPrinceton);

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards =
                ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
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

            // Assert that no decision will be presented to the player
            AssertNoDecision();

            // Play Armed Dragon Lv7 from hand during this start of turn phase
            PlayCardFromHand(ChazzPrinceton, armedDragonLv7.Identifier);

            // Assert that we have one less card in hand, and Armed Dragon Lv7 is in play area
            QuickHandCheck(-1);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertIsInPlayAndNotUnderCard(armedDragonLv7);
        }

        [Test]
        public void AtOtherStartOfTurn_WithLv10InHand_DoesNothing()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Legacy", "Megalopolis");
            StartGame();
            DestroyNonCharacterVillainCards();

            // Enter our Start of turn immediately, to ensure that Armed Dragon Lv7 does not activate
            GoToStartOfTurn(ChazzPrinceton);

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards =
                ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
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

            // Play Armed Dragon Lv7 from hand during this start of turn phase
            PlayCardFromHand(ChazzPrinceton, armedDragonLv7.Identifier);

            // Enter end of turn to go through the damage part, we'll skip the damage
            DecisionDoNotSelectCard = SelectionType.DealDamage;
            GoToEndOfTurn(ChazzPrinceton);

            // Once we have skipped the damage dealing, assert that no more decisions will be presented to player
            AssertNoDecision();

            // Go to next turn
            GoToNextTurn();

            // Assert that we have one less card in hand, and Armed Dragon Lv7 is in play area
            QuickHandCheck(-1);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertIsInPlayAndNotUnderCard(armedDragonLv7);
        }

        [Test]
        public void AtYourNextStartOfTurn_WithNoLv10InHand_DoesNothing()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Legacy", "Megalopolis");
            StartGame();
            DestroyNonCharacterVillainCards();

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards =
                ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Play Armed Dragon Lv7
            Card armedDragonLv7 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv7);
            AssertIsInPlayAndNotUnderCard(armedDragonLv7);

            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Put Armed Dragon Lv3 into hand
            Card armedDragonLv3 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertInHand(ChazzPrinceton, armedDragonLv3);

            // Put Armed Dragon Lv5 into hand
            Card armedDragonLv5 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv5);
            AssertInHand(ChazzPrinceton, armedDragonLv5);

            // Put X Head Cannon into hand
            Card xheadCannon = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.XHeadCannon);
            AssertInHand(ChazzPrinceton, xheadCannon);

            // Put Ojama Green into hand
            Card ojamaGreen = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.OjamaGreen);
            AssertInHand(ChazzPrinceton, ojamaGreen);

            // Assert that there is no Armed Dragon Lv10 in hand
            AssertNotInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Assert that no decision will be presented to the player
            AssertNoDecision();

            // Enter end of Villain turn
            GoToEndOfTurn(baron);

            // Enter start of next turn
            GoToStartOfTurn(ChazzPrinceton);

            // Assert that no changes were made in the hand or play area
            QuickHandCheck(0);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertIsInPlayAndNotUnderCard(armedDragonLv7);
        }

        [Test]
        public void AtYourNextStartOfTurn_WithLv10InHandButChooseNotToPlay_DoesNothing()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Legacy", "Megalopolis");
            StartGame();
            DestroyNonCharacterVillainCards();

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards =
                ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Play Armed Dragon Lv7
            Card armedDragonLv7 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv7);
            AssertIsInPlayAndNotUnderCard(armedDragonLv7);

            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Put Armed Dragon Lv3 into hand
            Card armedDragonLv3 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertInHand(ChazzPrinceton, armedDragonLv3);

            // Put Armed Dragon Lv5 into hand
            Card armedDragonLv5 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv5);
            AssertInHand(ChazzPrinceton, armedDragonLv5);

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
            IEnumerable<Card> includedCards = new List<Card> { armedDragonLv10 };
            IEnumerable<Card> notIncludedCards = new List<Card> { armedDragonLv3, armedDragonLv5, armedDragonLv7, xheadCannon, ojamaGreen };

            // Assert the player sees the choices we expect.
            // We will be choosing not to play Armed Dragon Lv10
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
            AssertIsInPlayAndNotUnderCard(armedDragonLv7);
            AssertInHand(ChazzPrinceton, armedDragonLv10);
        }

        [Test]
        public void AtYourNextStartOfTurn_WithLv10InHandAndChooseToPlay_PlaysLv10AndDestroysSelf()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Legacy", "Megalopolis");
            StartGame();
            DestroyNonCharacterVillainCards();

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards =
                ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Play Armed Dragon Lv7
            Card armedDragonLv7 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv7);
            AssertIsInPlayAndNotUnderCard(armedDragonLv7);

            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Put Armed Dragon Lv3 into hand
            Card armedDragonLv3 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertInHand(ChazzPrinceton, armedDragonLv3);

            // Put Armed Dragon Lv5 into hand
            Card armedDragonLv5 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv5);
            AssertInHand(ChazzPrinceton, armedDragonLv5);

            // Put Armed Dragon Lv10 into hand
            Card armedDragonLv10 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv10);
            AssertInHand(ChazzPrinceton, armedDragonLv10);

            // Put Ojama Green into hand
            Card ojamaGreen = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.OjamaGreen);
            AssertInHand(ChazzPrinceton, ojamaGreen);

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Store the expected choices the player should see
            IEnumerable<Card> includedCards = new List<Card> { armedDragonLv10 };
            IEnumerable<Card> notIncludedCards = new List<Card> { armedDragonLv3, armedDragonLv5, armedDragonLv7, ojamaGreen };

            // Assert the player sees the choices we expect.
            // We will be choosing to play Armed Dragon Lv10
            AssertDecisionIsOptional(SelectionType.PlayCard);
            AssertNextDecisionChoices(includedCards, notIncludedCards);
            DecisionSelectCardToPlay = armedDragonLv10;

            // Enter end of Villain turn
            GoToEndOfTurn(baron);

            // Enter start of next turn
            GoToStartOfTurn(ChazzPrinceton);

            // Assert that Armed Dragon Lv10 was played and that Armed Dragon Lv7 was destroyed
            QuickHandCheck(-1);
            AssertNumberOfCardsInPlay(ChazzPrinceton, 2);
            AssertIsInPlayAndNotUnderCard(armedDragonLv10);
            AssertInTrash(ChazzPrinceton, armedDragonLv7);
        }

        [Test]
        public void AtYourEndOfTurn_WithMultipleTargetsInPlay_DealsDamageWithDecision()
        {
            // Setup a sample game with Chazz Princeton and Luminary, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonConstants.Deck, "Luminary", "Megalopolis");
            StartGame();
            DestroyNonCharacterVillainCards();

            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards =
                ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Play Armed Dragon Lv7
            Card armedDragonLv7 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv7);
            AssertIsInPlayAndNotUnderCard(armedDragonLv7);

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
            AssertIsInPlayAndNotUnderCard(armedDragonLv7);
            AssertIsInPlayAndNotUnderCard(abcUnion);

            AssertNumberOfCardsInPlay(luminary, 3);
            AssertIsInPlayAndNotUnderCard(allAccordingToPlan);
            AssertIsInPlayAndNotUnderCard(regressionTurret);

            // Assert that all targets other than the villain are still at max health
            AssertHitPoints(ChazzPrinceton.CharacterCard, ChazzPrinceton.CharacterCard.MaximumHitPoints.Value);
            AssertHitPoints(armedDragonLv7, armedDragonLv7.MaximumHitPoints.Value);

            AssertHitPoints(luminary.CharacterCard, luminary.CharacterCard.MaximumHitPoints.Value);
            AssertHitPoints(regressionTurret, regressionTurret.MaximumHitPoints.Value);

            AssertHitPoints(trafficPileup, trafficPileup.MaximumHitPoints.Value);

            AssertHitPoints(elementalRedistributor, elementalRedistributor.MaximumHitPoints.Value);

            // Assert that the villain character took 3 damage
            AssertHitPoints(baron.CharacterCard, baron.CharacterCard.MaximumHitPoints.Value - 3);
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

            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards =
                ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Play Armed Dragon Lv7
            Card armedDragonLv7 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv7);
            AssertIsInPlayAndNotUnderCard(armedDragonLv7);

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

            // Assert that there is no choice of target to deal damage to
            AssertNoDecision();

            // Enter end of luminary turn
            GoToEndOfTurn(luminary);

            // Assert that no changes were made in the hand or play area
            QuickHandCheck(0);

            AssertNumberOfCardsInPlay(ChazzPrinceton, 3);
            AssertIsInPlayAndNotUnderCard(armedDragonLv7);
            AssertIsInPlayAndNotUnderCard(abcUnion);

            AssertNumberOfCardsInPlay(luminary, 3);
            AssertIsInPlayAndNotUnderCard(allAccordingToPlan);
            AssertIsInPlayAndNotUnderCard(regressionTurret);

            // Assert that all targets including the villain are still at max health
            AssertHitPoints(ChazzPrinceton.CharacterCard, ChazzPrinceton.CharacterCard.MaximumHitPoints.Value);
            AssertHitPoints(armedDragonLv7, armedDragonLv7.MaximumHitPoints.Value);

            AssertHitPoints(luminary.CharacterCard, luminary.CharacterCard.MaximumHitPoints.Value);
            AssertHitPoints(regressionTurret, regressionTurret.MaximumHitPoints.Value);

            AssertHitPoints(trafficPileup, trafficPileup.MaximumHitPoints.Value);

            AssertHitPoints(baron.CharacterCard, baron.CharacterCard.MaximumHitPoints.Value);
            AssertHitPoints(elementalRedistributor, elementalRedistributor.MaximumHitPoints.Value);
        }
    }
}
