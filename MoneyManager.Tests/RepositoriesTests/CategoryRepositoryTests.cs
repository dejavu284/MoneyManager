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
    public class CategoryRepositoryTests
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
        public async Task AddAsync_ShouldAddCategory()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CategoryRepository(context);
            var category = new Category
            {
                CategoryName = "Test Category",
                Status = true
            };

            // Act
            await repository.AddAsync(category);
            var savedCategory = await context.Category.FirstOrDefaultAsync(c => c.CategoryName == "Test Category");

            // Assert
            Assert.NotNull(savedCategory);
            Assert.Equal(category.CategoryName, savedCategory.CategoryName);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCategories()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CategoryRepository(context);
            context.Category.AddRange(
                new Category { CategoryName = "Category1", Status = true },
                new Category { CategoryName = "Category2", Status = true }
            );
            await context.SaveChangesAsync();

            // Act
            var categories = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, categories.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectCategory()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CategoryRepository(context);
            var category = new Category
            {
                CategoryName = "Category1",
                Status = true
            };
            context.Category.Add(category);
            await context.SaveChangesAsync();

            // Act
            var retrievedCategory = await repository.GetByIdAsync(category.CategoryId);

            // Assert
            Assert.NotNull(retrievedCategory);
            Assert.Equal(category.CategoryName, retrievedCategory.CategoryName);
        }

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteCategory()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CategoryRepository(context);
            var category = new Category
            {
                CategoryName = "Category1",
                Status = true
            };
            context.Category.Add(category);
            await context.SaveChangesAsync();

            // Act
            await repository.DeleteAsync(category.CategoryId);
            var deletedCategory = await context.Category.FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId);

            // Assert
            Assert.NotNull(deletedCategory);
            Assert.False(deletedCategory.Status); // Verify that the category is soft-deleted
        }
    }
}
