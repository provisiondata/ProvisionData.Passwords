namespace ProvisionData.Passwords.UnitTests
{

    public class CspRngTest : RandomNumberGeneratorTests
    {
        protected override IRandomNumberGenerator RNG => new CspRng();
    }
}
