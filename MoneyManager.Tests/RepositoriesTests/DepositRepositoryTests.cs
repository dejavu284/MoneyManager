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
    public class DepositRepositoryTests
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
        public async Task AddAsync_ShouldAddDeposit()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new DepositRepository(context);
            var deposit = new Deposit
            {
                DepositName = "Test Deposit",
                DepositAmount = 1000m,
                InterestRate = 5m,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddYears(1),
                Currency = new Currency { CurrencyCode = "USD" },
                Status = true
            };

            // Act
            await repository.AddAsync(deposit);
            var savedDeposit = await context.Deposit.FirstOrDefaultAsync(d => d.DepositName == "Test Deposit");

            // Assert
            Assert.NotNull(savedDeposit);
            Assert.Equal(deposit.DepositName, savedDeposit.DepositName);
            Assert.Equal(deposit.DepositAmount, savedDeposit.DepositAmount);
            Assert.Equal(deposit.InterestRate, savedDeposit.InterestRate);
            Assert.Equal(deposit.StartDate, savedDeposit.StartDate);
            Assert.Equal(deposit.EndDate, savedDeposit.EndDate);
            Assert.Equal(deposit.Currency.CurrencyCode, savedDeposit.Currency.CurrencyCode);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllDeposits()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new DepositRepository(context);
            context.Deposit.AddRange(
                new Deposit { DepositName = "Deposit1", Status = true, Currency = new Currency { CurrencyCode = "USD" } },
                new Deposit { DepositName = "Deposit2", Status = true, Currency = new Currency { CurrencyCode = "USD" } }
            );
            await context.SaveChangesAsync();

            // Act
            var deposits = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, deposits.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectDeposit()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new DepositRepository(context);
            var deposit = new Deposit
            {
                DepositName = "Deposit1",
                Status = true,
                Currency = new Currency { CurrencyCode = "USD" }
            };
            context.Deposit.Add(deposit);
            await context.SaveChangesAsync();

            // Act
            var retrievedDeposit = await repository.GetByIdAsync(deposit.DepositId);

            // Assert
            Assert.NotNull(retrievedDeposit);
            Assert.Equal(deposit.DepositName, retrievedDeposit.DepositName);
        }

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteDeposit()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new DepositRepository(context);
            var deposit = new Deposit
            {
                DepositName = "Deposit1",
                Status = true,
                Currency = new Currency { CurrencyCode = "USD" }
            };
            context.Deposit.Add(deposit);
            await context.SaveChangesAsync();

            // Act
            await repository.DeleteAsync(deposit.DepositId);
            var deletedDeposit = await context.Deposit.FirstOrDefaultAsync(d => d.DepositId == deposit.DepositId);

            // Assert
            Assert.NotNull(deletedDeposit);
            Assert.False(deletedDeposit.Status); // Verify that the deposit is soft-deleted
        }
    }
}
