using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using MoneyManager.Data;
using MoneyManager.Data.Repositories.Concrete;
using MoneyManager.Models;

namespace MoneyManager.Tests.RepositoriesTests
{
    public class CurrencyRepositoryTests
    {
        private DbContextOptions<MoneyManagerContext> GetInMemoryDbOptions()
        {
            return new DbContextOptionsBuilder<MoneyManagerContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Используем уникальное имя базы данных для каждого теста
                .Options;
        }

        private MoneyManagerContext GetInMemoryDbContext()
        {
            var options = GetInMemoryDbOptions();
            return new MoneyManagerContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddCurrency()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CurrencyRepository(context);
            var currency = new Currency
            {
                CurrencyCode = "USD",
                Status = true
            };

            // Act
            await repository.AddAsync(currency);
            var savedCurrency = await context.Currency.FirstOrDefaultAsync(c => c.CurrencyCode == "USD");

            // Assert
            Assert.NotNull(savedCurrency);
            Assert.Equal(currency.CurrencyCode, savedCurrency.CurrencyCode);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCurrencies()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CurrencyRepository(context);
            context.Currency.AddRange(
                new Currency { CurrencyCode = "USD", Status = true },
                new Currency { CurrencyCode = "EUR", Status = true }
            );
            await context.SaveChangesAsync();

            // Act
            var currencies = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, currencies.Count());
        }

        [Fact]
        public async Task GetByCodeAsync_ShouldReturnCorrectCurrency()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CurrencyRepository(context);
            var currency = new Currency
            {
                CurrencyCode = "USD",
                Status = true
            };
            context.Currency.Add(currency);
            await context.SaveChangesAsync();

            // Act
            var retrievedCurrency = await repository.GetByCodeAsync("USD");

            // Assert
            Assert.NotNull(retrievedCurrency);
            Assert.Equal(currency.CurrencyCode, retrievedCurrency.CurrencyCode);
        }

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteCurrency()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CurrencyRepository(context);
            var currency = new Currency
            {
                CurrencyCode = "USD",
                Status = true
            };
            context.Currency.Add(currency);
            await context.SaveChangesAsync();

            // Act
            await repository.DeleteAsync(currency.CurrencyId);
            var deletedCurrency = await context.Currency.FirstOrDefaultAsync(c => c.CurrencyId == currency.CurrencyId);

            // Assert
            Assert.NotNull(deletedCurrency);
            Assert.False(deletedCurrency.Status); // Verify that the currency is soft-deleted
        }
    }
}
