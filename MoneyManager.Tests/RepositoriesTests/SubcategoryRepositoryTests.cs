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
    public class SubcategoryRepositoryTests
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
        public async Task AddAsync_ShouldAddSubcategory()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new SubcategoryRepository(context);
            var category = new Category { CategoryName = "Test Category", Status = true };
            var subcategory = new Subcategory
            {
                SubcategoryName = "Test Subcategory",
                Category = category,
                Status = true
            };
            context.Category.Add(category);
            await context.SaveChangesAsync();

            // Act
            await repository.AddAsync(subcategory);
            var savedSubcategory = await context.Subcategory.FirstOrDefaultAsync(s => s.SubcategoryName == "Test Subcategory");

            // Assert
            Assert.NotNull(savedSubcategory);
            Assert.Equal(subcategory.SubcategoryName, savedSubcategory.SubcategoryName);
            Assert.Equal(subcategory.Category.CategoryName, savedSubcategory.Category.CategoryName);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllSubcategories()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new SubcategoryRepository(context);
            var category = new Category { CategoryName = "Test Category", Status = true };
            context.Category.Add(category);
            context.Subcategory.AddRange(
                new Subcategory { SubcategoryName = "Subcategory1", Category = category, Status = true },
                new Subcategory { SubcategoryName = "Subcategory2", Category = category, Status = true }
            );
            await context.SaveChangesAsync();

            // Act
            var subcategories = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, subcategories.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectSubcategory()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new SubcategoryRepository(context);
            var category = new Category { CategoryName = "Test Category", Status = true };
            var subcategory = new Subcategory
            {
                SubcategoryName = "Subcategory1",
                Category = category,
                Status = true
            };
            context.Category.Add(category);
            context.Subcategory.Add(subcategory);
            await context.SaveChangesAsync();

            // Act
            var retrievedSubcategory = await repository.GetByIdAsync(subcategory.SubcategoryId);

            // Assert
            Assert.NotNull(retrievedSubcategory);
            Assert.Equal(subcategory.SubcategoryName, retrievedSubcategory.SubcategoryName);
            Assert.Equal(subcategory.Category.CategoryName, retrievedSubcategory.Category.CategoryName);
        }

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteSubcategory()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new SubcategoryRepository(context);
            var category = new Category { CategoryName = "Test Category", Status = true };
            var subcategory = new Subcategory
            {
                SubcategoryName = "Subcategory1",
                Category = category,
                Status = true
            };
            context.Category.Add(category);
            context.Subcategory.Add(subcategory);
            await context.SaveChangesAsync();

            // Act
            await repository.DeleteAsync(subcategory.SubcategoryId);
            var deletedSubcategory = await context.Subcategory.FirstOrDefaultAsync(s => s.SubcategoryId == subcategory.SubcategoryId);

            // Assert
            Assert.NotNull(deletedSubcategory);
            Assert.False(deletedSubcategory.Status); // Verify that the subcategory is soft-deleted
        }
    }
}
