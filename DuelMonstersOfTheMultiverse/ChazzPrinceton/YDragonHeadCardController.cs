using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DeckardBaseMod;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace DMotM.ChazzPrinceton
{
    public class YDragonHeadCardController : CardController
    {
        public YDragonHeadCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override void AddTriggers()
        {
            // At end of turn, you may deal 1 target 1 radiant damage
            AddEndOfTurnTrigger(turnTaker => turnTaker.Equals(TurnTaker), EndOfTurnResponse, TriggerType.DealDamage);
        }

        private IEnumerator EndOfTurnResponse(PhaseChangeAction pca)
        {
            // Deal 1 target 1 radiant damage
            int numTargets = GetPowerNumeral(0, 1);
            int damageAmount = GetPowerNumeral(1, 1);
            return GameController.SelectTargetsAndDealDamage(DecisionMaker, new DamageSource(GameController, Card),
                damageAmount, DamageType.Radiant, numTargets, false, 0, cardSource: GetCardSource());
        }

        public override IEnumerator UsePower(int index = 0)
        {
            // Get the list of cards this hero currently has in the play area
            IList<Card> cardsInPlayArea = HeroTurnTaker.GetPlayAreaCards().ToList();

            // Check if any of them are Z-Metal Tank
            bool zInPlay = cardsInPlayArea.Any(card => card.Identifier.Equals(ChazzPrincetonConstants.ZMetalTank));

            // If Z-Metal Tank is in play...
            if (zInPlay)
            {
                // Have all ABC targets gain 1 HP
                int healAmount = GetPowerNumeral(0, 1);
                IEnumerator ghp = GameController.GainHP(HeroTurnTakerController, card => card.IsTarget && card.KeywordsContainEx(ChazzPrincetonConstants.ABC),
                    healAmount, optional: true, cardSource: GetCardSource());

                if (UseUnityCoroutines) { yield return GameController.StartCoroutine(ghp); }
                else { GameController.ExhaustCoroutine(ghp); }
            }

            // Check if any of them are X-Head Cannon
            bool xInPlay = cardsInPlayArea.Any(card => card.Identifier.Equals(ChazzPrincetonConstants.XHeadCannon));

            // If X-Head Cannon is in play...
            if (xInPlay)
            {
                // Use a power
                IEnumerator saup = GameController.SelectAndUsePower(HeroTurnTakerController, showMessage: true, cardSource: GetCardSource());

                if (UseUnityCoroutines) { yield return GameController.StartCoroutine(saup); }
                else { GameController.ExhaustCoroutine(saup); }
            }
        }
    }
}
