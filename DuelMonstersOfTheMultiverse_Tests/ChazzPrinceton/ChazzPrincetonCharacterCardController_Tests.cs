using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using NUnit.Framework;

namespace DuelMonstersOfTheMultiverse_Tests.ChazzPrinceton
{
    [TestFixture]
    public class ChazzPrincetonCharacterCardController_Tests : BaseTest
    {
        protected HeroTurnTakerController chazzPrinceton { get { return FindHero("ChazzPrinceton"); } }
    }
}
