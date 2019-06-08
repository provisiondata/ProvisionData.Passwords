using System.Linq;
using Xunit;

namespace ProvisionData.Passwords.UnitTests
{
    public class PronounceablePasswordGeneratorTests
    {
        [Fact]
        public void Generate_Should_Return_Password_Of_Specified_Length()
        {
            var generator = new PronounceablePasswordGenerator();

            Assert.Equal(5, generator.Generate(5).Length);
            Assert.Equal(6, generator.Generate(6).Length);
            Assert.Equal(10, generator.Generate(10).Length);
            Assert.Equal(30, generator.Generate(30).Length);
        }

        [Fact]
        public void Must_not_generate_a_password_shorter_than_MinLength()
        {
            // Arrange
            var generator = new PronounceablePasswordGenerator();

            // Act
            var password = generator.Generate(3);

            // Assert
            Assert.Equal(PronounceablePasswordGenerator.MinLength, password.Length);
        }

        [Fact]
        public void Must_not_generate_a_password_longer_than_MaxLength()
        {
            // Arrange
            var generator = new PronounceablePasswordGenerator();

            // Act
            var password = generator.Generate(100);

            // Assert
            Assert.Equal(PronounceablePasswordGenerator.MaxLength, password.Length);
        }

        [Fact]
        public void Generate_Must_Generate_1_Password_If_Count_Is_Less_Than_1()
        {
            // Arrange
            var generator = new PronounceablePasswordGenerator();

            // Act
            var password = generator.Generate(0, 4, 8);

            // Assert
            Assert.Single(password);
        }

        [Fact]
        public void Generate_Must_Generate_MaxCount_Passwords_If_Count_Is_Greater_Than_MaxCount()
        {
            // Arrange
            var generator = new PronounceablePasswordGenerator();

            // Act
            var password = generator.Generate(100, 4, 8);

            // Assert
            Assert.Equal(10, password.Count());
        }
    }
}
