using MoneyManager.Models;

namespace MoneyManager.Data.Services
{
    public class CategoryService : DataService<Category>
    {
        public CategoryService() : base(DbContextSingleton.Instance)
        {
        }
    }
}
