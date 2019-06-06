using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ProvisionData.Passwords.UnitTests
{
    public abstract class RandomNumberGeneratorTests
    {
        protected abstract IRandomNumberGenerator RNG { get; }

        [Fact]
        public void Can_Generate_Random_Int32()
        {
            for (var index = 0; index < 1000; index++)
            {
                var r = RNG.GetInt32();

                Assert.True(r >= 0, "Generated a value less than 0.0.");
                Assert.True(r <= Int32.MaxValue, String.Format("Generated a value greater than {0}.", Int32.MaxValue));
            }
        }

        [Fact]
        public void Can_Generate_Postive_Int64()
        {
            for (var index = 0; index < 1000; index++)
            {
                var r = RNG.GetInt64();

                Assert.True(r >= 0, "Generated a value less than 0.0.");
                Assert.True(r <= Int64.MaxValue, String.Format("Generated a value greater than {0}.", Int64.MaxValue));
            }
        }

        [Fact]
        public void Can_Generate_Double_Between_0_And_1()
        {
            var lastValue = Double.NaN;

            for (var index = 0; index < 1000; index++)
            {
                var r = RNG.GetDouble();

                // Assert
                Assert.True(r >= 0.0, String.Format("Generated a value less than 0.0: {0}", r));
                Assert.True(r <= 1.0, String.Format("Generated a value greater than 1.0: {0}", r));
                Assert.NotEqual(lastValue, r);
                lastValue = r;
            }
        }

        [Fact]
        public void Should_not_generate_a_duplicate_8_character_password_in_1000_Iterations()
        {
            const Int32 iterations = 1000;

            // Arrange
            var generator = new PronounceablePasswordGenerator(new GenerateOptions(RNG) { Length = 8 }, Graph.Default);

            // Act 100000 times
            var set = new HashSet<String>();
            for (var index = 0; index < iterations; index++)
            {
                Assert.True(set.Add(generator.Generate().First()));
            }
        }
    }
}
