using MoneyManager.Models;
using MoneyManager.Models.Generators;
using Xunit;

namespace MoneyManager.Tests.BuildersTests
{
    public class CategoryBuilderTests
    {
        [Fact]
        public void CategoryBuilder_ShouldBuildCategoryWithCorrectName()
        {
            // Arrange
            var builder = new CategoryBuilder();

            // Act
            var category = builder.SetName("Test Category").Build();

            // Assert
            Assert.Equal("Test Category", category.CategoryName);
        }

        [Fact]
        public void CategoryBuilder_ShouldBuildCategoryWithActiveStatus()
        {
            // Arrange
            var builder = new CategoryBuilder();

            // Act
            var category = builder.Build();

            // Assert
            Assert.True(category.Status);
        }
    }
}
