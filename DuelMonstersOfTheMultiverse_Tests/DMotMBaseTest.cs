using DMotM;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.UnitTest;
using NUnit.Framework;

namespace DMotMTests
{
    public class DMotMBaseTest : DeckardBaseTest
    {
        // Chazz Princeton
        protected HeroTurnTakerController ChazzPrinceton { get { return GameController.FindTurnTakerController(ChazzPrincetonConstants.Hero)?.ToHero(); } }

        /// <summary>
        /// Virtual Setup method to be called before every single unit test.
        /// Can be overriden to add addition logic.
        /// </summary>
        [SetUp]
        public virtual void Setup()
        {
            SetupTestTargetsOngoingsEquipmentsForAllTestTurnTakers();
            StartGame();
        }
    }
}
