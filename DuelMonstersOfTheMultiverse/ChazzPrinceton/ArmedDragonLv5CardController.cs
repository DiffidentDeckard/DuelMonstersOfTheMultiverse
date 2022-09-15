using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace DMotM.ChazzPrinceton
{
    public class ArmedDragonLv5CardController : CardController
    {
        public ArmedDragonLv5CardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override IEnumerator Play()
        {
            // If this card is played during this hero's play or power phase...
            if (GameController.ActiveTurnTaker.Equals(TurnTaker) && (GameController.ActiveTurnPhase.IsPlayCard || GameController.ActiveTurnPhase.IsUsePower))
            {
                // Player chooses a non-character target in this play area to destroy
                IEnumerator sadc = GameController.SelectAndDestroyCard(DecisionMaker, new LinqCardCriteria(card => !card.IsCharacter && card.IsTarget && card.IsInLocation(HeroTurnTaker.PlayArea)),
                    false, cardSource: GetCardSource());

                if (UseUnityCoroutines)
                {
                    yield return GameController.StartCoroutine(sadc);
                }
                else
                {
                    GameController.ExhaustCoroutine(sadc);
                }
            }
        }

        public override void AddTriggers()
        {
            // At start of turn after this card was played, you may play an Armed Dragon Lv7 from your hand, and destroy this card
            AddStartOfTurnTrigger(turnTaker => turnTaker.Equals(TurnTaker) && GetCardPropertyJournalEntryBoolean(ModConstants.HasBeenInPlayAtLeastATurn) == true,
                StartOfTurnResponse, new List<TriggerType>() { TriggerType.PlayCard, TriggerType.DestroySelf });

            // At end of every turn, set HasBeenInPlayAtLeastATurn to true if it is false
            AddEndOfTurnTrigger(turnTaker => GetCardPropertyJournalEntryBoolean(ModConstants.HasBeenInPlayAtLeastATurn) != true,
                EndOfEveryTurnResponse, TriggerType.AddTrigger);

            // At end of turn, deal 1 target 2 projectile damage
            AddEndOfTurnTrigger(turnTaker => turnTaker.Equals(TurnTaker), EndOfTurnResponse, TriggerType.DealDamage);

            // Reset the CardProperty when this card leaves play
            AddAfterLeavesPlayAction(() => ResetFlagAfterLeavesPlay(ModConstants.HasBeenInPlayAtLeastATurn));
        }

        private IEnumerator StartOfTurnResponse(PhaseChangeAction pca)
        {
            // Get the list of playable cards in the hero's hand
            IEnumerable<Card> playableCards = GetPlayableCardsInHand(HeroTurnTakerController);

            // See if any of them are Armed Dragon Lv7
            bool hasLv7InHand = playableCards.Any(card => card.Identifier.Equals(ChazzPrincetonConstants.ArmedDragonLv7));

            // If Armed Dragon Lv7 is in the hand...
            if (hasLv7InHand)
            {
                // storedResults will store what the player did, so that we can check it to see if we should destroy this card
                List<PlayCardAction> storedResults = new List<PlayCardAction>();

                // Player may play it
                IEnumerator sapcfh = SelectAndPlayCardFromHand(DecisionMaker, storedResults: storedResults, associateCardSource: true,
                    cardCriteria: new LinqCardCriteria(card => card.Identifier.Equals(ChazzPrincetonConstants.ArmedDragonLv7), "Armed Dragon Lv7"));

                if (UseUnityCoroutines)
                {
                    yield return GameController.StartCoroutine(sapcfh);
                }
                else
                {
                    GameController.ExhaustCoroutine(sapcfh);
                }

                // If Armed Dragon Lv7 was played...
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

        private IEnumerator EndOfTurnResponse(PhaseChangeAction pca)
        {
            // Deal 1 target 2 projectile damage
            int numTargets = GetPowerNumeral(0, 1);
            int damageAmount = GetPowerNumeral(1, 2);
            return GameController.SelectTargetsAndDealDamage(DecisionMaker, new DamageSource(GameController, Card),
                    damageAmount, DamageType.Projectile, numTargets, false, 0, cardSource: GetCardSource());
        }
    }
}
