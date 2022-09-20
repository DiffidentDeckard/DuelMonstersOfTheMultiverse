using System.Collections;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace DMotM.ChazzPrinceton
{
    public class ArmedDragonLv10CardController : CardController
    {
        public ArmedDragonLv10CardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        public override bool CanBePlayedInTurnPhase(TurnPhase turnPhase)
        {
            // This card can only be played during this hero's start of turn
            if (!GameController.ActiveTurnTaker.Equals(TurnTaker) || !turnPhase.IsStart)
            {
                return false;
            }

            return base.CanBePlayedInTurnPhase(turnPhase);
        }

        public override void AddTriggers()
        {
            // At end of turn, deal 1 target 3 projectile damage
            AddEndOfTurnTrigger(turnTaker => turnTaker.Equals(TurnTaker), EndOfTurnResponse, TriggerType.DealDamage);
        }

        private IEnumerator EndOfTurnResponse(PhaseChangeAction pca)
        {
            // Deal 1 target 4 projectile damage
            int numTargets = GetPowerNumeral(0, 1);
            int damageAmount = GetPowerNumeral(1, 4);
            return GameController.SelectTargetsAndDealDamage(DecisionMaker, new DamageSource(GameController, Card),
                damageAmount, DamageType.Projectile, numTargets, false, 0, cardSource: GetCardSource());
        }

        public override IEnumerator UsePower(int index = 0)
        {
            // When this power is used, discard a card.
            IEnumerator sadc = GameController.SelectAndDiscardCard(DecisionMaker, cardSource: GetCardSource());

            if (UseUnityCoroutines) { yield return GameController.StartCoroutine(sadc); }
            else { GameController.ExhaustCoroutine(sadc); }

            // Deal each non-hero target 4 projectile damage
            int damageAmount = GetPowerNumeral(0, 4);
            IEnumerator dd = GameController.DealDamage(DecisionMaker, Card, card => card.IsTarget && !card.IsHero, damageAmount, DamageType.Projectile, cardSource: GetCardSource());

            if (UseUnityCoroutines) { yield return GameController.StartCoroutine(dd); }
            else { GameController.ExhaustCoroutine(dd); }
        }
    }
}
