using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace DMotM.ChazzPrinceton
{
    public class ChazzPrincetonCharacterCardController : HeroCharacterCardController
    {
        public ChazzPrincetonCharacterCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator UsePower(int index = 0)
        {
            // Get the list of cards this hero currently has in the play area
            IEnumerable<Card> cardsInThisPlayArea = HeroTurnTaker.GetPlayAreaCards();

            // Take only the targets from this list of cards
            IEnumerable<Card> targetsInThisPlayArea = cardsInThisPlayArea.Where(card => card.IsTarget);

            // Get all the distinct keywords of those targets, ignoring 'limited'
            IEnumerable<string> keywordsInThisPlayArea = targetsInThisPlayArea.SelectMany(card => card.GetKeywords()).Distinct()
                .Where(keyword => !keyword.Equals("limited", StringComparison.CurrentCultureIgnoreCase));

            // Get the list of playable cards in the hero's hand
            IEnumerable<Card> playableCards = GetPlayableCardsInHand(HeroTurnTakerController);

            // Get only the playable cards in hand that share a keyword with a target in the play area
            IEnumerable<Card> keywordSharingCards = playableCards.Where(card => card.KeywordsContainAnyOfEx(keywordsInThisPlayArea));

            // Do we have any valid cards to play?
            bool anyValidCardsToPlay = keywordSharingCards.Any();

            // storedResults will store what the player did, so that we can check it to see if we should draw a card
            List<PlayCardAction> storedResults = new List<PlayCardAction>();

            // If there are any valid card options...
            if (anyValidCardsToPlay)
            {
                // We are going to create two options for the player to select from
                List<Function> functions = new List<Function>();

                // This is the "play a card" option
                functions.Add(new Function(HeroTurnTakerController, "Play a card from your hand that shares a Keyword, other than 'limited', with a target you have in your play area.", SelectionType.PlayCard,
                    () =>
                    {
                        // Ask player to select a valid card to play
                        return SelectAndPlayCardFromHand(DecisionMaker, storedResults: storedResults, associateCardSource: true,
                            cardCriteria: new LinqCardCriteria(card => card.KeywordsContainAnyOfEx(keywordsInThisPlayArea), "keyword-sharing"));
                    }));

                // This is the "draw a card" option
                functions.Add(new Function(HeroTurnTakerController, "Draw 1 card.", SelectionType.DrawCard,
                    () =>
                    {
                        // Draw 1 card
                        int numCardsToDraw = GetPowerNumeral(0, 1);
                        return DrawCards(HeroTurnTakerController, numCardsToDraw);
                    }));

                // Create the SelectFunctionDecision object to perform
                SelectFunctionDecision sfd = new SelectFunctionDecision(GameController, HeroTurnTakerController, functions, true, cardSource: GetCardSource());

                // Ask the player to select one and perform it
                return GameController.SelectAndPerformFunction(sfd);
            }
            // If there are not any valid card options...
            else
            {
                // Draw 1 card
                int numCardsToDraw = GetPowerNumeral(0, 1);
                return DrawCards(HeroTurnTakerController, numCardsToDraw);
            }
        }

        public override IEnumerator UseIncapacitatedAbility(int index)
        {
            IEnumerator e = null;

            switch (index)
            {
                case 0:
                    // One player may play a card now.
                    e = SelectHeroToPlayCard(DecisionMaker);
                    break;
                case 1:
                    // One hero may use a power now.
                    e = GameController.SelectHeroToUsePower(DecisionMaker, cardSource: GetCardSource());
                    break;
                case 2:
                    // One player may draw a card now
                    e = GameController.SelectHeroToDrawCard(DecisionMaker, cardSource: GetCardSource());
                    break;
            }

            return e;
        }
    }
}
