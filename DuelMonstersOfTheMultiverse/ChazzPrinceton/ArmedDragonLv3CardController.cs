using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace DMotM.ChazzPrinceton
{
    public class ArmedDragonLv3CardController : CardController
    {
        public ArmedDragonLv3CardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            // At start of turn after this card was played, you may play an Armed Dragon Lv5 from your hand, and destroy this card
            AddStartOfTurnTrigger(turnTaker => turnTaker.Equals(TurnTaker) && GetCardPropertyJournalEntryBoolean(ModConstants.HasBeenInPlayAtLeastATurn) == true,
                StartOfTurnResponse, new List<TriggerType>() { TriggerType.PlayCard, TriggerType.DestroySelf });

            // At end of every turn, set HasBeenInPlayAtLeastATurn to true if it is false
            AddEndOfTurnTrigger(turnTaker => GetCardPropertyJournalEntryBoolean(ModConstants.HasBeenInPlayAtLeastATurn) != true,
                EndOfEveryTurnResponse, TriggerType.AddTrigger);

            // When this card is destroyed, you may draw 1 card
            AddWhenDestroyedTrigger(DestroyedResponse, TriggerType.DrawCard);

            // Reset the CardProperty when this card leaves play
            AddAfterLeavesPlayAction(() => ResetFlagAfterLeavesPlay(ModConstants.HasBeenInPlayAtLeastATurn));
        }

        private IEnumerator StartOfTurnResponse(PhaseChangeAction pca)
        {
            // Get the list of playable cards in the hero's hand
            IEnumerable<Card> playableCards = GetPlayableCardsInHand(HeroTurnTakerController);

            // See if any of them are Armed Dragon Lv5
            bool hasLv5InHand = playableCards.Any(card => card.Identifier.Equals(ChazzPrincetonConstants.ArmedDragonLv5));

            // If Armed Dragon Lv5 is in the hand...
            if (hasLv5InHand)
            {
                // storedResults will store what the player did, so that we can check it to see if we should destroy this card
                List<PlayCardAction> storedResults = new List<PlayCardAction>();

                // Player may play it
                IEnumerator sapcfh = SelectAndPlayCardFromHand(DecisionMaker, storedResults: storedResults, associateCardSource: true,
                    cardCriteria: new LinqCardCriteria(card => card.Identifier.Equals(ChazzPrincetonConstants.ArmedDragonLv5), "Armed Dragon Lv5"));

                if (UseUnityCoroutines)
                {
                    yield return GameController.StartCoroutine(sapcfh);
                }
                else
                {
                    GameController.ExhaustCoroutine(sapcfh);
                }

                // If Armed Dragon Lv5 was played...
                if (DidPlayCards(storedResults))
                {
                    // Destroy this card
                    IEnumerator dc = GameController.DestroyCard(HeroTurnTakerController, Card, showOutput: true, cardSource: GetCardSource());

                    if (UseUnityCoroutines)
                    {
                        yield return GameController.StartCoroutine(dc);
                    }
                    else
                    {
                        GameController.ExhaustCoroutine(dc);
                    }
                }
            }
        }

        private IEnumerator EndOfEveryTurnResponse(PhaseChangeAction pca)
        {
            // If HasBeenInPlayAtLeastATurn is false...
            if (GetCardPropertyJournalEntryBoolean(ModConstants.HasBeenInPlayAtLeastATurn) != true)
            {
                // Set it to true because we reach an end of turn
                SetCardProperty(ModConstants.HasBeenInPlayAtLeastATurn, true);
            }

            return null;
        }

        private IEnumerator DestroyedResponse(DestroyCardAction dca)
        {
            // Draw 1 card
            int numCardsToDraw = GetPowerNumeral(0, 1);
            return DrawCards(HeroTurnTakerController, numCardsToDraw);
        }
    }
}
