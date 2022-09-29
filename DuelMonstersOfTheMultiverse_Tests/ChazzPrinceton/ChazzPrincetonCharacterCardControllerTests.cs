using System.Collections.Generic;
using System.Linq;
using DeckardBaseMod;
using DMotM;
using DMotM.ChazzPrinceton;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using NUnit.Framework;

namespace DMotMTests.ChazzPrinceton
{
    [TestFixture]
    public class ChazzPrincetonCharacterCardControllerTests : ChazzPrincetonBaseTest
    {
        [Test]
        public void IsHeroCharacterCard()
        {
            // Assert that Chazz Princeton is a Hero Character Card
            Assert.That(ChazzPrinceton.CharacterCard.IsHeroCharacterCard);
        }

        [Test]
        public void OnSetup_HasCorrectHP()
        {
            // Assert that Chazz Princeton exists in this game
            Assert.That(ChazzPrinceton, Is.Not.Null);

            // Assert that Chazz Princeton is an instance of ChazzPrincetonCharacterCardController
            Assert.That(ChazzPrinceton.CharacterCardController, Is.TypeOf<ChazzPrincetonCharacterCardController>());

            // Assert that Chazz Princeton has exactly 27 HP maximum
            Assert.That(ChazzPrinceton.CharacterCard.MaximumHitPoints, Is.EqualTo(27));

            // Assert that Chazz Princeton has exactly 27 HP currently
            Assert.That(ChazzPrinceton.CharacterCard.HitPoints, Is.EqualTo(27));
        }

        [Test]
        public void UsePower_WhenNoTargetsInPlayArea_DrawsCard()
        {
            // Go to the Chazz Princeton's User Power Phase
            GoToUsePowerPhase(ChazzPrinceton);

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards = ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Assert that there are cards in the deck
            int numCardsInDeck = GetNumberOfCardsInDeck(ChazzPrinceton);
            Assert.That(numCardsInDeck, Is.GreaterThan(0));

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Assert that no decision will be presented to the player
            AssertNoDecision();

            // Use Chazz Princeton's power
            UsePower(ChazzPrinceton);

            // Assert that there is 1 more card in the hand than previously
            QuickHandCheck(1);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();
        }

        [Test]
        public void UsePower_WhenTargetInPlayAreaButNoValidCardInHand_DrawsCard()
        {
            // Go to the Chazz Princeton's User Power Phase
            GoToUsePowerPhase(ChazzPrinceton);

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards = ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Play Armed Dragon Lv3
            Card armedDragonLv3 = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, armedDragonLv3);

            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Assert that there are cards in the deck
            int numCardsInDeck = GetNumberOfCardsInDeck(ChazzPrinceton);
            Assert.That(numCardsInDeck, Is.GreaterThan(0));

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Assert that no decision will be presented to the player
            AssertNoDecision();

            // Use Chazz Princeton's power
            UsePower(ChazzPrinceton);

            // Assert that there is 1 more card in the hand than previously
            QuickHandCheck(1);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();
        }

        [Test]
        public void UsePower_WhenValidAndSelectPlayCard_PresentsCorrectOptions()
        {
            // Go to the Chazz Princeton's User Power Phase
            GoToUsePowerPhase(ChazzPrinceton);

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards = ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Play Ojama Yellow
            Card ojamaYellow = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.OjamaYellow);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, ojamaYellow);

            // Put Armed Dragon Lv3 into hand
            Card armedDragonLv3 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertInHand(ChazzPrinceton, armedDragonLv3);

            // Put Y Dragon Head into hand
            Card yDragonHead = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.YDragonHead);
            AssertInHand(ChazzPrinceton, yDragonHead);

            // Put Ojama Black into hand
            Card ojamaBlack = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.OjamaBlack);
            AssertInHand(ChazzPrinceton, ojamaBlack);

            // Put Ojamuscle into hand
            Card ojamuscle = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.Ojamuscle);
            AssertInHand(ChazzPrinceton, ojamuscle);

            // Assert that there are cards in the deck
            int numCardsInDeck = GetNumberOfCardsInDeck(ChazzPrinceton);
            Assert.That(numCardsInDeck, Is.GreaterThan(0));

            // Prepare which cards should be shown in the choices, and which should not
            IEnumerable<Card> includedCards = new List<Card> { ojamaBlack, ojamuscle };
            IEnumerable<Card> notIncludedCards = new List<Card> { ojamaYellow, armedDragonLv3, yDragonHead };

            // Assert that the appropriate card choices will be displayed to player
            AssertNextDecisionChoices(includedCards, notIncludedCards);

            // Use Chazz Princeton's power, selecting the first option (to play a card from hand)
            // We will opt to play Ojama Black
            DecisionSelectFunction = 0;
            DecisionSelectCardToPlay = ojamaBlack;
            UsePower(ChazzPrinceton);

            // Assert that Ojama Black was played
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, ojamaBlack);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();
        }

        [Test]
        public void UsePower_WhenValidAndSelectDrawCard_DrawsCard()
        {
            // Go to the Chazz Princeton's User Power Phase
            GoToUsePowerPhase(ChazzPrinceton);

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards = ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Play Ojama Yellow
            Card ojamaYellow = PlayCard(ChazzPrinceton, ChazzPrincetonConstants.OjamaYellow);
            AssertInPlayAreaAndHasGameText(ChazzPrinceton, ojamaYellow);

            // Put Armed Dragon Lv3 into hand
            Card armedDragonLv3 = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.ArmedDragonLv3);
            AssertInHand(ChazzPrinceton, armedDragonLv3);

            // Put Y Dragon Head into hand
            Card yDragonHead = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.YDragonHead);
            AssertInHand(ChazzPrinceton, yDragonHead);

            // Put Ojama Black into hand
            Card ojamaBlack = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.OjamaBlack);
            AssertInHand(ChazzPrinceton, ojamaBlack);

            // Put Ojamuscle into hand
            Card ojamuscle = PutInHand(ChazzPrinceton, ChazzPrincetonConstants.Ojamuscle);
            AssertInHand(ChazzPrinceton, ojamuscle);

            // Assert that there are cards in the deck
            int numCardsInDeck = GetNumberOfCardsInDeck(ChazzPrinceton);
            Assert.That(numCardsInDeck, Is.GreaterThan(0));

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Use Chazz Princeton's power, selecting the second option (to draw a card)
            DecisionSelectFunction = 1;
            UsePower(ChazzPrinceton);

            // Assert that there is 1 more card in the hand than previously
            QuickHandCheck(1);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();
        }

        [Test]
        public void UsePower_Incapacitated_AllowsHeroToPlayCard()
        {
            // Incapacitate Chazz Princeton
            DealDamage(TestVillain, ChazzPrinceton, 9999, DamageType.Energy);
            AssertIncapacitated(ChazzPrinceton);
            GoToUseIncapacitatedAbilityPhase(ChazzPrinceton);

            // Put the card that will be played into TestHero1's hand
            PutInHand(TestHero1, TestHero1Constants.TestHero1OneShot);
            AssertInHand(TestHero1, TestHero1Constants.TestHero1OneShot);

            // Prepare which turnTakers should be shown in the choices, and which should not
            IEnumerable<TurnTakerController> includedTTC = new List<TurnTakerController> { TestHero1, TestHero2 };
            IEnumerable<TurnTakerController> notIncludedTTC = new List<TurnTakerController> { TestVillain, ChazzPrinceton, TestEnvironment };

            // Assert the possible choices for the player, we'll select TestHero1
            AssertNextDecisionChoices(includedTTC, notIncludedTTC);
            DecisionSelectTurnTaker = TestHero1.HeroTurnTaker;

            // Assert that Chazz Princeton's incap power allows TestHero1 to play a card
            AssertIncapLetsHeroPlayCard(ChazzPrinceton, 0, TestHero1, TestHero1Constants.TestHero1OneShot);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();
        }

        [Test]
        public void UsePower_Incapacitated_AllowsHeroToUsePower()
        {
            // Incapacitate Chazz Princeton
            DealDamage(TestVillain, ChazzPrinceton, 9999, DamageType.Energy);
            AssertIncapacitated(ChazzPrinceton);
            GoToUseIncapacitatedAbilityPhase(ChazzPrinceton);

            // Prepare which turnTakers should be shown in the choices, and which should not
            IEnumerable<TurnTakerController> includedTTC = new List<TurnTakerController> { TestHero1, TestHero2 };
            IEnumerable<TurnTakerController> notIncludedTTC = new List<TurnTakerController> { TestVillain, ChazzPrinceton, TestEnvironment };

            // Assert the possible choices for the player, we'll select TestHero1
            AssertNextDecisionChoices(includedTTC, notIncludedTTC);
            DecisionSelectTurnTaker = TestHero1.HeroTurnTaker;

            // Assert that Chazz Princeton's incap power allows TestHero1 to use a power
            AssertIncapLetsHeroUsePower(ChazzPrinceton, 1, TestHero1);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();
        }

        [Test]
        public void UsePower_Incapacitated_AllowsHeroToDrawCard()
        {
            // Incapacitate Chazz Princeton
            DealDamage(TestVillain, ChazzPrinceton, 9999, DamageType.Energy);
            AssertIncapacitated(ChazzPrinceton);
            GoToUseIncapacitatedAbilityPhase(ChazzPrinceton);

            // Prepare which turnTakers should be shown in the choices, and which should not
            IEnumerable<TurnTakerController> includedTTC = new List<TurnTakerController> { TestHero1, TestHero2 };
            IEnumerable<TurnTakerController> notIncludedTTC = new List<TurnTakerController> { TestVillain, ChazzPrinceton, TestEnvironment };

            // Assert the possible choices for the player, we'll select TestHero1
            AssertNextDecisionChoices(includedTTC, notIncludedTTC);
            DecisionSelectTurnTaker = TestHero1.HeroTurnTaker;

            // Assert that Chazz Princeton's incap power allows TestHero1 to draw 1 card
            AssertIncapLetsHeroDrawCard(ChazzPrinceton, 2, TestHero1, 1);

            // Assert no other changes in any of the other play areas
            AssertAllTestKeepersInPlayForAllTestTurnTakers();
        }
    }
}
