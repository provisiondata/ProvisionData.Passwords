namespace ProvisionData.Passwords.UnitTests
{
    public class StockRngTest : RandomNumberGeneratorTests
    {
        protected override IRandomNumberGenerator RNG => new StockRng();
    }
}
