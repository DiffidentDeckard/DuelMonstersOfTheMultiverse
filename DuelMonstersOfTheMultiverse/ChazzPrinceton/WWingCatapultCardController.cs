using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace DMotM.ChazzPrinceton
{
    public class WWingCatapultCardController : CardController
    {
        public WWingCatapultCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
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
            IEnumerable<Card> cardsInPlayArea = HeroTurnTaker.GetPlayAreaCards();

            // Check if any of them are "V-Tiger Jet"
            bool vInPlay = cardsInPlayArea.Any(card => card.Identifier.Equals(ChazzPrincetonConstants.VTigerJet));

            // If "V-Tiger Jet" is in play...
            if (vInPlay)
            {
                // Destroy an Environment
                IEnumerator sadc = GameController.SelectAndDestroyCard(DecisionMaker, new LinqCardCriteria(card => card.IsEnvironment), true, cardSource: GetCardSource());

                if (UseUnityCoroutines) { yield return GameController.StartCoroutine(sadc); }
                else { GameController.ExhaustCoroutine(sadc); }
            }
        }
    }
}
