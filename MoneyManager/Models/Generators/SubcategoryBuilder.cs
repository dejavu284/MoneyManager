namespace MoneyManager.Models.Generators
{
    public class SubcategoryBuilder
    {
        private readonly Subcategory _subcategory;

        public SubcategoryBuilder()
        {
            _subcategory = new Subcategory();
        }

        public SubcategoryBuilder SetName(string name)
        {
            _subcategory.SubcategoryName = name;
            return this;
        }

        public SubcategoryBuilder SetCategory(Category category)
        {
            _subcategory.Category = category;
            _subcategory.CategoryId = category.CategoryId;
            return this;
        }

        public Subcategory Build()
        {
            _subcategory.Status = true;
            return _subcategory;
        }
    }
}
