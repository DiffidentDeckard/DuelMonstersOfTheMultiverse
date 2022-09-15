using System.Collections.Generic;
using System.Linq;
using DMotM.ChazzPrinceton;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.UnitTest;
using NUnit.Framework;

namespace DMotMTests.ChazzPrinceton
{
    [TestFixture]
    public class ChazzPrincetonCharacterCardControllerTests : BaseTest
    {
        [Test]
        public void ChazzPrinceton_OnSetup_HasCorrectHP()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonNamespace, "Megalopolis");

            // Assert that there are exactly 3 turn takers
            Assert.That(GameController.TurnTakerControllers.Count(), Is.EqualTo(3));

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
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonNamespace, "Megalopolis");
            StartGame();
            GoToUsePowerPhase(ChazzPrinceton);

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards = ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Assert that there are cards in the deck
            int numCardsInDeck = GetNumberOfCardsInDeck(ChazzPrinceton);
            Assert.That(numCardsInDeck, Is.GreaterThan(0));

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Use Chazz Princeton's power
            UsePower(ChazzPrinceton);

            // Assert that no decision was presented to the player
            AssertNoDecision();

            // Assert that there is 1 more card in the hand than previously
            QuickHandCheck(1);
        }

        [Test]
        public void UsePower_WhenTargetInPlayAreaButNoValidCardInHand_DrawsCard()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonNamespace, "Megalopolis");
            StartGame();
            GoToUsePowerPhase(ChazzPrinceton);

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards = ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Play Armed Dragon Lv3
            PlayCard(ChazzPrinceton, "ArmedDragonLv3");
            AssertIsInPlayAndNotUnderCard("ArmedDragonLv3");

            // Move all Cards from Chazz Princeton's hand into the deck, to get rid of any possible 'armed' cards
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Assert that there are cards in the deck
            int numCardsInDeck = GetNumberOfCardsInDeck(ChazzPrinceton);
            Assert.That(numCardsInDeck, Is.GreaterThan(0));

            // Store the cards currently in hand
            QuickHandStorage(ChazzPrinceton);

            // Use Chazz Princeton's power
            UsePower(ChazzPrinceton);

            // Assert that no decision was presented to the player
            AssertNoDecision();

            // Assert that there is 1 more card in the hand than previously
            QuickHandCheck(1);
        }

        [Test]
        public void UsePower_WhenValidAndSelectPlayCard_PresentsCorrectOptions()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonNamespace, "Megalopolis");
            StartGame();
            GoToUsePowerPhase(ChazzPrinceton);

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards = ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Play Ojama Yellow
            Card ojamaYellow = PlayCard(ChazzPrinceton, "OjamaYellow");
            AssertIsInPlayAndNotUnderCard("OjamaYellow");

            // Put Armed Dragon Lv3 into hand
            Card armedDragonLv3 = PutInHand(ChazzPrinceton, "ArmedDragonLv3");
            AssertInHand(ChazzPrinceton, "ArmedDragonLv3");
            AssertIsInPlayAndNotUnderCard("OjamaYellow");

            // Put Y Dragon Head into hand
            Card yDragonHead = PutInHand(ChazzPrinceton, "YDragonHead");
            AssertInHand(ChazzPrinceton, "YDragonHead");

            // Put Ojama Black into hand
            Card ojamaBlack = PutInHand(ChazzPrinceton, "OjamaBlack");
            AssertInHand(ChazzPrinceton, "OjamaBlack");

            // Put Ojamuscle into hand
            Card ojamuscle = PutInHand(ChazzPrinceton, "Ojamuscle");
            AssertInHand(ChazzPrinceton, "Ojamuscle");

            // Assert that there are cards in the deck
            int numCardsInDeck = GetNumberOfCardsInDeck(ChazzPrinceton);
            Assert.That(numCardsInDeck, Is.GreaterThan(0));

            // Prepare which cards should be shown in the choices, and which should not
            IEnumerable<Card> includedCards = new List<Card>() { ojamaBlack, ojamuscle };
            IEnumerable<Card> notIncludedCards = new List<Card>() { ojamaYellow, armedDragonLv3, yDragonHead };

            // Assert that the appropriate card choices will be displayed to player
            AssertNextDecisionChoices(includedCards, notIncludedCards);

            // Use Chazz Princeton's power, selecting the first option (to play a card from hand)
            // We will opt to play Ojama Black
            DecisionSelectFunction = 0;
            DecisionSelectCardToPlay = ojamaBlack;
            UsePower(ChazzPrinceton);

            // Assert that Ojama Black was played
            AssertIsInPlayAndNotUnderCard(ojamaBlack);
        }

        [Test]
        public void UsePower_WhenValidAndSelectDrawCard_DrawsCard()
        {
            // Setup a sample game with Chazz Princeton, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonNamespace, "Megalopolis");
            StartGame();
            GoToUsePowerPhase(ChazzPrinceton);

            // Assert that there are no cards in Chazz Princeton's play area other than the hero character card
            IEnumerable<Card> chazzPlayAreaCards = ChazzPrinceton.HeroTurnTaker.GetPlayAreaCards().Where(card => !card.IsHeroCharacterCard);
            Assert.That(chazzPlayAreaCards, Is.Empty);

            // Move all Cards from Chazz Princeton's hand into the deck
            MoveAllCardsFromHandToDeck(ChazzPrinceton);
            AssertNumberOfCardsInHand(ChazzPrinceton, 0);

            // Play Ojama Yellow
            Card ojamaYellow = PlayCard(ChazzPrinceton, "OjamaYellow");
            AssertIsInPlayAndNotUnderCard("OjamaYellow");

            // Put Armed Dragon Lv3 into hand
            Card armedDragonLv3 = PutInHand(ChazzPrinceton, "ArmedDragonLv3");
            AssertInHand(ChazzPrinceton, "ArmedDragonLv3");
            AssertIsInPlayAndNotUnderCard("OjamaYellow");

            // Put Y Dragon Head into hand
            Card yDragonHead = PutInHand(ChazzPrinceton, "YDragonHead");
            AssertInHand(ChazzPrinceton, "YDragonHead");

            // Put Ojama Black into hand
            Card ojamaBlack = PutInHand(ChazzPrinceton, "OjamaBlack");
            AssertInHand(ChazzPrinceton, "OjamaBlack");

            // Put Ojamuscle into hand
            Card ojamuscle = PutInHand(ChazzPrinceton, "Ojamuscle");
            AssertInHand(ChazzPrinceton, "Ojamuscle");

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
        }

        [Test]
        public void UsePower_Incapacitated_AllowsHeroToPlayCard()
        {
            // Setup a sample game with Chazz Princeton and some other heroes, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonNamespace, "Legacy", "Haka", "Megalopolis");
            StartGame();

            // Incapacitate Chazz Princeton
            DealDamage(baron, ChazzPrinceton, 9999, DamageType.Energy);
            AssertIncapacitated(ChazzPrinceton);
            GoToUseIncapacitatedAbilityPhase(ChazzPrinceton);

            // Put the card that will be played into Legacy's hand
            string theLegacyRing = "TheLegacyRing";
            PutInHand(legacy, theLegacyRing);
            AssertInHand(legacy, theLegacyRing);

            // Prepare which turnTakers should be shown in the choices, and which should not
            IEnumerable<TurnTakerController> includedTTC = new List<TurnTakerController>() { legacy, haka };
            IEnumerable<TurnTakerController> notIncludedTTC = new List<TurnTakerController>() { baron, ChazzPrinceton };

            // Assert the possible choices for the player, we'll select Legacy
            AssertNextDecisionChoices(includedTTC, notIncludedTTC);
            DecisionSelectTurnTaker = legacy.HeroTurnTaker;

            // Assert that Chazz Princeton's incap power allows Legacy to play The Legacy Ring
            AssertIncapLetsHeroPlayCard(ChazzPrinceton, 0, legacy, theLegacyRing);
        }

        [Test]
        public void UsePower_Incapacitated_AllowsHeroToUsePower()
        {
            // Setup a sample game with Chazz Princeton and some other heroes, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonNamespace, "Legacy", "Haka", "Megalopolis");
            StartGame();

            // Incapacitate Chazz Princeton
            DealDamage(baron, ChazzPrinceton, 9999, DamageType.Energy);
            AssertIncapacitated(ChazzPrinceton);
            GoToUseIncapacitatedAbilityPhase(ChazzPrinceton);

            // Prepare which turnTakers should be shown in the choices, and which should not
            IEnumerable<TurnTakerController> includedTTC = new List<TurnTakerController>() { legacy, haka };
            IEnumerable<TurnTakerController> notIncludedTTC = new List<TurnTakerController>() { baron, ChazzPrinceton };

            // Assert the possible choices for the player, we'll select Legacy
            AssertNextDecisionChoices(includedTTC, notIncludedTTC);
            DecisionSelectTurnTaker = legacy.HeroTurnTaker;

            // Assert that Chazz Princeton's incap power allows Legacy to use a power
            AssertIncapLetsHeroUsePower(ChazzPrinceton, 1, legacy);
        }

        [Test]
        public void UsePower_Incapacitated_AllowsHeroToDrawCard()
        {
            // Setup a sample game with Chazz Princeton and some other heroes, the villain and environment don't matter
            SetupGameController("BaronBlade", ChazzPrincetonNamespace, "Legacy", "Haka", "Megalopolis");
            StartGame();

            // Incapacitate Chazz Princeton
            DealDamage(baron, ChazzPrinceton, 9999, DamageType.Energy);
            AssertIncapacitated(ChazzPrinceton);
            GoToUseIncapacitatedAbilityPhase(ChazzPrinceton);

            // Prepare which turnTakers should be shown in the choices, and which should not
            IEnumerable<TurnTakerController> includedTTC = new List<TurnTakerController>() { legacy, haka };
            IEnumerable<TurnTakerController> notIncludedTTC = new List<TurnTakerController>() { baron, ChazzPrinceton };

            // Assert the possible choices for the player, we'll select Legacy
            AssertNextDecisionChoices(includedTTC, notIncludedTTC);
            DecisionSelectTurnTaker = legacy.HeroTurnTaker;

            // Assert that Chazz Princeton's incap power allows Legacy to draw 1 card
            AssertIncapLetsHeroDrawCard(ChazzPrinceton, 2, legacy, 1);
        }
    }
}
