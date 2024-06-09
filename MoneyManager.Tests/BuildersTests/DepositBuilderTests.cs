using MoneyManager.Models;
using MoneyManager.Models.Generators;

namespace MoneyManager.Tests.BuildersTests
{
    public class DepositBuilderTests
    {
        [Fact]
        public void DepositBuilder_ShouldBuildDepositWithCorrectName()
        {
            // Arrange
            var builder = new DepositBuilder();

            // Act
            var deposit = builder.SetName("Test Deposit").Build();

            // Assert
            Assert.Equal("Test Deposit", deposit.DepositName);
        }

        [Fact]
        public void DepositBuilder_ShouldBuildDepositWithCorrectAmount()
        {
            // Arrange
            var builder = new DepositBuilder();

            // Act
            var deposit = builder.SetAmount(1000.50m).Build();

            // Assert
            Assert.Equal(1000.50m, deposit.DepositAmount);
        }

        [Fact]
        public void DepositBuilder_ShouldBuildDepositWithCorrectInterestRate()
        {
            // Arrange
            var builder = new DepositBuilder();

            // Act
            var deposit = builder.SetInterestRate(5.5m).Build();

            // Assert
            Assert.Equal(5.5m, deposit.InterestRate);
        }

        [Fact]
        public void DepositBuilder_ShouldBuildDepositWithCorrectStartDate()
        {
            // Arrange
            var builder = new DepositBuilder();
            var startDate = DateTime.Now;

            // Act
            var deposit = builder.SetStartDate(startDate).Build();

            // Assert
            Assert.Equal(startDate, deposit.StartDate);
        }

        [Fact]
        public void DepositBuilder_ShouldBuildDepositWithCorrectEndDate()
        {
            // Arrange
            var builder = new DepositBuilder();
            var endDate = DateTime.Now.AddYears(1);

            // Act
            var deposit = builder.SetEndDate(endDate).Build();

            // Assert
            Assert.Equal(endDate, deposit.EndDate);
        }

        [Fact]
        public void DepositBuilder_ShouldBuildDepositWithCorrectCurrency()
        {
            // Arrange
            var builder = new DepositBuilder();
            var currency = new Currency { CurrencyId = 1, CurrencyCode = "USD" };

            // Act
            var deposit = builder.SetCurrency(currency).Build();

            // Assert
            Assert.Equal(currency, deposit.Currency);
            Assert.Equal(currency.CurrencyId, deposit.CurrencyId);
        }
    }
}