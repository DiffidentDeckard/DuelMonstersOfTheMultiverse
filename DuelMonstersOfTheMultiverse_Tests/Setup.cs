using System;
using System.Reflection;
using DeckardBaseMod.TestHero1;
using DMotM.ChazzPrinceton;
using Handelabra;
using Handelabra.Sentinels.Engine.Model;
using NUnit.Framework;

namespace DMotMTests
{
    [SetUpFixture]
    public class Setup
    {
        [OneTimeSetUp]
        public void DoSetup()
        {
            Log.DebugDelegate += Output;
            Log.WarningDelegate += Output;
            Log.ErrorDelegate += Output;

            // Tell the engine about our mod assembly so it can load up our code.
            // It doesn't matter which type as long as it comes from the mod's assembly.

            var dbmAssembly = Assembly.GetAssembly(typeof(TestHero1CharacterCardController));
            ModHelper.AddAssembly(nameof(DeckardBaseMod), dbmAssembly);

            var dmotmAssembly = Assembly.GetAssembly(typeof(ChazzPrincetonCharacterCardController));
            ModHelper.AddAssembly(nameof(DMotM), dmotmAssembly);
        }

        protected void Output(string message)
        {
            Console.WriteLine(message);
        }
    }
}
