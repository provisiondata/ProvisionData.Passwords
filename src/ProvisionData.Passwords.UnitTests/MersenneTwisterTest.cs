namespace ProvisionData.Passwords.UnitTests
{
    public class MersenneTwisterTest : RandomNumberGeneratorTests
    {
        protected override IRandomNumberGenerator RNG => new MersenneTwister();
    }
}
