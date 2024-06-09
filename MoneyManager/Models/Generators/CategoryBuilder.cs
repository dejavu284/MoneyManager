using System;

namespace MoneyManager.Models.Generators
{
    public class CategoryBuilder
    {
        private readonly Category _category;

        public CategoryBuilder()
        {
            _category = new Category();
        }

        public CategoryBuilder SetName(string name)
        {
            _category.CategoryName = name;
            return this;
        }

        public Category Build()
        {
            _category.Status = true;
            return _category;
        }
    }
}
