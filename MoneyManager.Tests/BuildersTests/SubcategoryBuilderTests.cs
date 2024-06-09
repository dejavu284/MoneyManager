using MoneyManager.Models;
using MoneyManager.Models.Generators;

namespace MoneyManager.Tests.BuildersTests
{

    public class SubcategoryBuilderTests
    {
        [Fact]
        public void SubcategoryBuilder_ShouldBuildSubcategoryWithCorrectName()
        {
            // Arrange
            var builder = new SubcategoryBuilder();

            // Act
            var subcategory = builder.SetName("Test Subcategory").Build();

            // Assert
            Assert.Equal("Test Subcategory", subcategory.SubcategoryName);
        }

        [Fact]
        public void SubcategoryBuilder_ShouldBuildSubcategoryWithCorrectCategory()
        {
            // Arrange
            var builder = new SubcategoryBuilder();
            var category = new Category { CategoryId = 1, CategoryName = "Test Category" };

            // Act
            var subcategory = builder.SetCategory(category).Build();

            // Assert
            Assert.Equal(category, subcategory.Category);
            Assert.Equal(category.CategoryId, subcategory.CategoryId);
        }
    }

}
