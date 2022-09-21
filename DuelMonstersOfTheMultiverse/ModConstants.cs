namespace DMotM
{
    public static class ModConstants
    {
        // Keywords (SHOULD ALWAYS BE ALL LOWERCASE!!!)
        public const string Limited = "limited";
        public const string OneShot = "one-shot";
        public const string Ongoing = "ongoing";

        // Card Properties
        public const string HasBeenInPlayAtLeastATurn = nameof(HasBeenInPlayAtLeastATurn);
    }

    #region Heroes
    public static class ChazzPrincetonConstants
    {
        // Main
        public static string Deck = $"{nameof(DMotM)}.{Hero}";
        public const string Hero = "ChazzPrinceton";

        // Keywords (SHOULD ALWAYS BE ALL LOWERCASE!!!)
        public const string Armed = "armed";
        public const string ABC = "abc";
        public const string Ojama = "ojama";

        // Cards
        public const string ArmedDragonLv3 = nameof(ArmedDragonLv3);
        public const string ArmedDragonLv5 = nameof(ArmedDragonLv5);
        public const string ArmedDragonLv7 = nameof(ArmedDragonLv7);
        public const string ArmedDragonLv10 = nameof(ArmedDragonLv10);
        public const string VTigerJet = nameof(VTigerJet);
        public const string WWingCatapult = nameof(WWingCatapult);
        public const string XHeadCannon = nameof(XHeadCannon);
        public const string YDragonHead = nameof(YDragonHead);
        public const string ZMetalTank = nameof(ZMetalTank);
        public const string AbcUnion = nameof(AbcUnion);
        public const string OjamaYellow = nameof(OjamaYellow);
        public const string OjamaGreen = nameof(OjamaGreen);
        public const string OjamaBlack = nameof(OjamaBlack);
        public const string OjamaKing = nameof(OjamaKing);
        public const string Ojamuscle = nameof(Ojamuscle);
        public const string MagicalMallet = nameof(MagicalMallet);
        public const string YouGoByeBye = nameof(YouGoByeBye);
        public const string CallofTheHaunted = nameof(CallofTheHaunted);
        public const string ThePrincetonName = nameof(ThePrincetonName);
        public const string ItsGoTime = nameof(ItsGoTime);
    }

    public static class TestHero1Constants
    {
        // Main
        public static string Deck = $"{nameof(DMotM)}.{Hero}";
        public const string Hero = "TestHero1";

        // Cards
        public const string TestHero1Target = nameof(TestHero1Target);
        public const string TestHero1Ongoing = nameof(TestHero1Ongoing);
        public const string TestHero1Equipment = nameof(TestHero1Equipment);
        public const string TestHero1OneShot = nameof(TestHero1OneShot);
    }

    public static class TestHero2Constants
    {
        // Main
        public static string Deck = $"{nameof(DMotM)}.{Hero}";
        public const string Hero = "TestHero2";

        // Cards
        public const string TestHero2Target = nameof(TestHero2Target);
        public const string TestHero2Ongoing = nameof(TestHero2Ongoing);
        public const string TestHero2Equipment = nameof(TestHero2Equipment);
        public const string TestHero2OneShot = nameof(TestHero2OneShot);
    }
    #endregion Heroes

    #region Villains
    public static class MaximillionPegasusConstants
    {
        // Main
        public static string Deck = $"{nameof(DMotM)}.{Villain}";
        public const string Villain = "MaximillionPegasus";

        // Keywords (SHOULD ALWAYS BE ALL LOWERCASE!!!)


        // Cards

    }

    public static class TestVillainConstants
    {
        // Main
        public static string Deck = $"{nameof(DMotM)}.{Villain}";
        public const string Villain = "TestVillain";

        // Cards
        public const string TestVillainTarget = nameof(TestVillainTarget);
        public const string TestVillainOngoing = nameof(TestVillainOngoing);
        public const string TestVillainEquipment = nameof(TestVillainEquipment);
        public const string TestVillainOneShot = nameof(TestVillainOneShot);
    }
    #endregion Villains

    #region Environments
    public static class GearTownConstants
    {
        // Main
        public static string Deck = $"{nameof(DMotM)}.{Environment}";
        public const string Environment = "GearTown";

        // Keywords (SHOULD ALWAYS BE ALL LOWERCASE!!!)


        // Cards

    }

    public static class TestEnvironmentConstants
    {
        // Main
        public static string Deck = $"{nameof(DMotM)}.{Environment}";
        public const string Environment = "TestEnvironment";

        // Cards
        public const string TestEnvironmentTarget = nameof(TestEnvironmentTarget);
        public const string TestEnvironmentOngoing = nameof(TestEnvironmentOngoing);
        public const string TestEnvironmentEquipment = nameof(TestEnvironmentEquipment);
        public const string TestEnvironmentOneShot = nameof(TestEnvironmentOneShot);
    }
    #endregion Environments
}
