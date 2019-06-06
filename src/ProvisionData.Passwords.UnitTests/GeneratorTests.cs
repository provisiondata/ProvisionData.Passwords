using System;
using System.Linq;
using Xunit;

namespace ProvisionData.Passwords.UnitTests
{
    public class GeneratorTests
    {
        [Fact]
        public void Generate_Should_Return_Password_Of_Specified_Length()
        {
            Assert.Equal(5, new PronounceablePasswordGenerator(new GenerateOptions() { Length = 5 }, Graph.Default).Generate().First().Length);
            Assert.Equal(6, new PronounceablePasswordGenerator(new GenerateOptions() { Length = 6 }, Graph.Default).Generate().First().Length);
            Assert.Equal(10, new PronounceablePasswordGenerator(new GenerateOptions() { Length = 10 }, Graph.Default).Generate().First().Length);
            Assert.Equal(30, new PronounceablePasswordGenerator(new GenerateOptions() { Length = 30 }, Graph.Default).Generate().First().Length);
        }

        [Fact]
        public void Must_not_generate_a_password_shorter_than_GenerateOptions_MinLength()
        {
            // Arrange
            var generator = new PronounceablePasswordGenerator(new GenerateOptions() { Length = 3 }, Graph.Default);

            // Act
            var password = generator.Generate();

            // Assert
            Assert.Equal(GenerateOptions.MinLength, password.Single().Length);
        }

        [Fact]
        public void Must_not_generate_a_password_longer_than_GenerateOptions_MaxLength()
        {
            // Arrange
            var generator = new PronounceablePasswordGenerator(new GenerateOptions() { Length = 100 }, Graph.Default);

            // Act
            var password = generator.Generate();

            // Assert
            Assert.Equal(GenerateOptions.MaxLength, password.Single().Length);
        }

        [Fact]
        public void Generate_Must_Generate_1_Password_If_Count_Is_Less_Than_1()
        {
            // Arrange
            var generator = new PronounceablePasswordGenerator(new GenerateOptions() { Count = 0 }, Graph.Default);

            // Act
            var password = generator.Generate();

            // Assert
            Assert.Single(password);
        }

        [Fact]
        public void Generate_Must_Generate_10_Passwords_If_Count_Is_Greater_Than_10()
        {
            // Arrange
            var generator = new PronounceablePasswordGenerator(new GenerateOptions() { Count = 11 }, Graph.Default);

            // Act
            var password = generator.Generate();

            // Assert
            Assert.Equal(10, password.Count());
        }

        [Fact]
        public void Generate_Should_Apply_Password_Modifiers()
        {
            // Arrange
            var options = new GenerateOptions()
            {
                Length = 30,
                Count = 1,
                Frequency = 100,
                UseBrackets = true,
                UseDigits = true,
                UseMinus = true,
                UseSpace = true,
                UseSpecial = true,
                UseUnderscore = true,
                UseUppercase = true
            };
            var generator = new PronounceablePasswordGenerator(options, Graph.Default);

            // Act
            var password = generator.Generate().Single();

            // Assert
            Assert.Matches(String.Format("[{0}]", GenerateOptions.Brackets.Replace("[", @"\[").Replace("]", @"\]")), password);
            Assert.Matches(String.Format("[{0}]", GenerateOptions.Digits), password);
            Assert.Matches(String.Format("[{0}]", GenerateOptions.Minus), password);
            Assert.Matches(String.Format("[{0}]", GenerateOptions.Space), password);
            Assert.Matches(String.Format("[{0}]", GenerateOptions.Special), password);
            Assert.Matches(String.Format("[{0}]", GenerateOptions.Underscore), password);
        }
    }
}
