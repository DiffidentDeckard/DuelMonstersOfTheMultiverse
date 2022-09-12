using System;
using System.Reflection;
using DMotM.ChazzPrinceton;
using Handelabra;
using Handelabra.Sentinels.Engine.Model;
using NUnit.Framework;

namespace DMotM_Tests
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
            var ass = Assembly.GetAssembly(typeof(ChazzPrincetonCharacterCardController)); // replace with your own type
            ModHelper.AddAssembly("DMotM", ass); // replace with your own namespace
        }

        protected void Output(string message)
        {
            Console.WriteLine(message);
        }
    }
}
