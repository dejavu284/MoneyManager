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
    public class DepositOperationRepositoryTests
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
        public async Task AddAsync_ShouldAddDepositOperation()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new DepositOperationRepository(context);
            var depositOperation = new DepositOperation
            {
                OperationAmount = 100,
                OperationDate = DateTime.Today,
                OperationType = "Deposit",
                Account = new Account { AccountName = "Test Account", Status = true },
                Deposit = new Deposit { DepositName = "Test Deposit", Status = true }
            };

            // Act
            await repository.AddAsync(depositOperation);
            var savedDepositOperation = await context.DepositOperation.FirstOrDefaultAsync(d => d.OperationAmount == 100);

            // Assert
            Assert.NotNull(savedDepositOperation);
            Assert.Equal(depositOperation.OperationAmount, savedDepositOperation.OperationAmount);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllDepositOperations()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new DepositOperationRepository(context);
            context.DepositOperation.AddRange(
                new DepositOperation { OperationAmount = 100, OperationDate = DateTime.Today, OperationType = "Deposit", Account = new Account { AccountName = "Account1", Status = true }, Deposit = new Deposit { DepositName = "Deposit1", Status = true } },
                new DepositOperation { OperationAmount = 200, OperationDate = DateTime.Today, OperationType = "Withdrawal", Account = new Account { AccountName = "Account2", Status = true }, Deposit = new Deposit { DepositName = "Deposit2", Status = true } }
            );
            await context.SaveChangesAsync();

            // Act
            var depositOperations = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, depositOperations.Count());
        }

        [Fact]
        public async Task GetOperationsForPeriodAsync_ShouldReturnCorrectDepositOperations()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new DepositOperationRepository(context);
            var depositOperation = new DepositOperation
            {
                OperationAmount = 100,
                OperationDate = DateTime.Today,
                OperationType = "Deposit",
                Account = new Account { AccountName = "Test Account", Status = true },
                Deposit = new Deposit { DepositName = "Test Deposit", Status = true }
            };
            context.DepositOperation.Add(depositOperation);
            await context.SaveChangesAsync();

            // Act
            var retrievedDepositOperations = await repository.GetOperationsForPeriodAsync(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1));

            // Assert
            Assert.Single(retrievedDepositOperations);
            Assert.Equal(depositOperation.OperationAmount, retrievedDepositOperations.First().OperationAmount);
        }

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteDepositOperation()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new DepositOperationRepository(context);
            var depositOperation = new DepositOperation
            {
                OperationAmount = 100,
                OperationDate = DateTime.Today,
                OperationType = "Deposit",
                Account = new Account { AccountName = "Test Account", Status = true },
                Deposit = new Deposit { DepositName = "Test Deposit", Status = true }
            };
            context.DepositOperation.Add(depositOperation);
            await context.SaveChangesAsync();

            // Act
            await repository.DeleteAsync(depositOperation.DepositOperationId);
            var deletedDepositOperation = await context.DepositOperation.FirstOrDefaultAsync(d => d.DepositOperationId == depositOperation.DepositOperationId);

            // Assert
            Assert.Null(deletedDepositOperation);
        }
    }
}
