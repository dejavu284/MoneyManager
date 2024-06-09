using MoneyManager.Models;
using MoneyManager.Models.Generators;
using Xunit;

namespace MoneyManager.Tests.BuildersTests
{
    public class CurrencyBuilderTests
    {
        [Fact]
        public void CurrencyBuilder_ShouldBuildCurrencyWithCorrectCode()
        {
            // Arrange
            var builder = new CurrencyBuilder();

            // Act
            var currency = builder.SetCode("USD").Build();

            // Assert
            Assert.Equal("USD", currency.CurrencyCode);
        }

        [Fact]
        public void CurrencyBuilder_ShouldBuildCurrencyWithActiveStatus()
        {
            // Arrange
            var builder = new CurrencyBuilder();

            // Act
            var currency = builder.Build();

            // Assert
            Assert.True(currency.Status);
        }
    }
}
