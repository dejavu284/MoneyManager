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
    public class BankOperationRepositoryTests
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
        public async Task AddAsync_ShouldAddBankOperation()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new BankOperationRepository(context);
            var bankOperation = new BankOperation
            {
                OperationAmount = 100,
                OperationDate = DateTime.Today,
                OperationType = "Deposit",
                Account = new Account { AccountName = "Test Account", Status = true },
                Subcategory = new Subcategory { SubcategoryName = "Test Subcategory", Status = true }
            };

            // Act
            await repository.AddAsync(bankOperation);
            var savedBankOperation = await context.BankOperation.FirstOrDefaultAsync(b => b.OperationAmount == 100);

            // Assert
            Assert.NotNull(savedBankOperation);
            Assert.Equal(bankOperation.OperationAmount, savedBankOperation.OperationAmount);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllBankOperations()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new BankOperationRepository(context);
            context.BankOperation.AddRange(
                new BankOperation { OperationAmount = 100, OperationDate = DateTime.Today, OperationType = "Deposit", Account = new Account { AccountName = "Account1", Status = true }, Subcategory = new Subcategory { SubcategoryName = "Subcategory1", Status = true } },
                new BankOperation { OperationAmount = 200, OperationDate = DateTime.Today, OperationType = "Withdrawal", Account = new Account { AccountName = "Account2", Status = true }, Subcategory = new Subcategory { SubcategoryName = "Subcategory2", Status = true } }
            );
            await context.SaveChangesAsync();

            // Act
            var bankOperations = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, bankOperations.Count());
        }

        [Fact]
        public async Task GetByAccountIdAsync_ShouldReturnCorrectBankOperations()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new BankOperationRepository(context);
            var account = new Account { AccountName = "Test Account", Status = true };
            var bankOperation = new BankOperation
            {
                OperationAmount = 100,
                OperationDate = DateTime.Today,
                OperationType = "Deposit",
                Account = account,
                Subcategory = new Subcategory { SubcategoryName = "Test Subcategory", Status = true }
            };
            context.Account.Add(account);
            context.BankOperation.Add(bankOperation);
            await context.SaveChangesAsync();

            // Act
            var retrievedBankOperations = await repository.GetByAccountIdAsync(account.AccountId);

            // Assert
            Assert.Single(retrievedBankOperations);
            Assert.Equal(bankOperation.OperationAmount, retrievedBankOperations.First().OperationAmount);
        }

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteBankOperation()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new BankOperationRepository(context);
            var bankOperation = new BankOperation
            {
                OperationAmount = 100,
                OperationDate = DateTime.Today,
                OperationType = "Deposit",
                Account = new Account { AccountName = "Test Account", Status = true },
                Subcategory = new Subcategory { SubcategoryName = "Test Subcategory", Status = true }
            };
            context.BankOperation.Add(bankOperation);
            await context.SaveChangesAsync();

            // Act
            await repository.DeleteAsync(bankOperation.OperationId);
            var deletedBankOperation = await context.BankOperation.FirstOrDefaultAsync(b => b.OperationId == bankOperation.OperationId);

            // Assert
            Assert.Null(deletedBankOperation);
        }
    }
}
