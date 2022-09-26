using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace DMotM.ChazzPrinceton
{
    public class ZMetalTankCardController : CardController
    {
        public ZMetalTankCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
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

            // Check if any of them are X-Head Cannon
            bool xInPlay = cardsInPlayArea.Any(card => card.Identifier.Equals(ChazzPrincetonConstants.XHeadCannon));

            // If X-Head Cannon is in play...
            if (xInPlay)
            {
                // Find all targets in play
                IEnumerable<Card> targetsInPlay = GameController.FindTargetsInPlay();

                // storedResults will store what the player did, so that we can apply the status effect to that target
                List<SelectTargetDecision> storedResults = new List<SelectTargetDecision>();

                // Have player select a target
                IEnumerator stasr = GameController.SelectTargetAndStoreResults(DecisionMaker, targetsInPlay, storedResults,
                    true, true, selectionType: SelectionType.SelectTargetNoDamage, cardSource: GetCardSource());

                if (UseUnityCoroutines) { yield return GameController.StartCoroutine(stasr); }
                else { GameController.ExhaustCoroutine(stasr); }

                // Get the target from the decision
                Card selectedTarget = storedResults.FirstOrDefault()?.SelectedCard;

                // If player selected a target...
                if (selectedTarget != null)
                {
                    // Damage dealt by that target is reduced by 1 until the start of your next turn
                    int damageReductionAmount = GetPowerNumeral(0, 1);
                    ReduceDamageStatusEffect rdse = new ReduceDamageStatusEffect(damageReductionAmount);
                    rdse.UntilStartOfNextTurn(HeroTurnTaker);
                    rdse.SourceCriteria.IsSpecificCard = selectedTarget;
                    rdse.CreateImplicitExpiryConditions();

                    // Apply the status effect
                    IEnumerator ase = GameController.AddStatusEffect(rdse, true, GetCardSource());

                    if (UseUnityCoroutines) { yield return GameController.StartCoroutine(ase); }
                    else { GameController.ExhaustCoroutine(ase); }
                }
            }

            // Check if any of them are Y-Dragon Head
            bool yInPlay = cardsInPlayArea.Any(card => card.Identifier.Equals(ChazzPrincetonConstants.YDragonHead));

            // If Y-Dragon Head is in play...
            if (yInPlay)
            {
                // Use a power
                IEnumerator saup = GameController.SelectAndUsePower(HeroTurnTakerController, showMessage: true, cardSource: GetCardSource());

                if (UseUnityCoroutines) { yield return GameController.StartCoroutine(saup); }
                else { GameController.ExhaustCoroutine(saup); }
            }
        }
    }
}
