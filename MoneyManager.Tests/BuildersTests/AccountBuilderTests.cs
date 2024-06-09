using MoneyManager.Models;
using MoneyManager.Models.Generators;

namespace MoneyManager.Tests.BuildersTests
{

    public class AccountBuilderTests
    {
        [Fact]
        public void AccountBuilder_ShouldBuildAccountWithCorrectName()
        {
            // Arrange
            var builder = new AccountBuilder();

            // Act
            var account = builder.SetName("Test Account").Build();

            // Assert
            Assert.Equal("Test Account", account.AccountName);
        }

        [Fact]
        public void AccountBuilder_ShouldBuildAccountWithCorrectAmount()
        {
            // Arrange
            var builder = new AccountBuilder();

            // Act
            var account = builder.SetBalance(2000.75m).Build();

            // Assert
            Assert.Equal(2000.75m, account.AccountBalance);
        }

        [Fact]
        public void AccountBuilder_ShouldBuildAccountWithCorrectCurrency()
        {
            // Arrange
            var builder = new AccountBuilder();
            var currency = new Currency { CurrencyId = 1, CurrencyCode = "EUR" };

            // Act
            var account = builder.SetCurrency(currency).Build();

            // Assert
            Assert.Equal(currency, account.Currency);
            Assert.Equal(currency.CurrencyId, account.CurrencyId);
        }
    }

}
