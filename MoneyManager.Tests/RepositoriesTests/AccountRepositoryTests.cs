using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using MoneyManager.Data;
using MoneyManager.Data.Repositories.Concrete;
using MoneyManager.Models;

namespace MoneyManager.Tests.RepositoriesTests
{
    public class AccountRepositoryTests
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
        public async Task AddAsync_ShouldAddAccount()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new AccountRepository(context);
            var account = new Account
            {
                AccountName = "Test Account",
                AccountBalance = 1000m,
                Currency = new Currency { CurrencyCode = "USD" },
                Status = true
            };

            // Act
            await repository.AddAsync(account);
            var savedAccount = await context.Account.FirstOrDefaultAsync(a => a.AccountName == "Test Account");

            // Assert
            Assert.NotNull(savedAccount);
            Assert.Equal(account.AccountName, savedAccount.AccountName);
            Assert.Equal(account.AccountBalance, savedAccount.AccountBalance);
            Assert.Equal(account.Currency.CurrencyCode, savedAccount.Currency.CurrencyCode);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllAccounts()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new AccountRepository(context);
            context.Account.AddRange(
                new Account { AccountName = "Account1", Status = true, Currency = new Currency { CurrencyCode = "USD" } },
                new Account { AccountName = "Account2", Status = true, Currency = new Currency { CurrencyCode = "USD" } }
            );
            await context.SaveChangesAsync();

            // Act
            var accounts = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, accounts.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectAccount()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new AccountRepository(context);
            var account = new Account
            {
                AccountName = "Account1",
                Status = true,
                Currency = new Currency { CurrencyCode = "USD" }
            };
            context.Account.Add(account);
            await context.SaveChangesAsync();

            // Act
            var retrievedAccount = await repository.GetByIdAsync(account.AccountId);

            // Assert
            Assert.NotNull(retrievedAccount);
            Assert.Equal(account.AccountName, retrievedAccount.AccountName);
        }

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteAccount()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new AccountRepository(context);
            var account = new Account
            {
                AccountName = "Account1",
                Status = true,
                Currency = new Currency { CurrencyCode = "USD" }
            };
            context.Account.Add(account);
            await context.SaveChangesAsync();

            // Act
            await repository.DeleteAsync(account.AccountId);
            var deletedAccount = await context.Account.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.AccountId == account.AccountId);

            // Assert
            Assert.NotNull(deletedAccount);
            Assert.False(deletedAccount.Status); // Verify that the account is soft-deleted
        }
    }
}
