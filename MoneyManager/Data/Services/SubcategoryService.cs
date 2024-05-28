using MoneyManager.Models;

namespace MoneyManager.Data.Services
{
    public class SubcategoryService : DataService<Subcategory>
    {
        public SubcategoryService() : base(DbContextSingleton.Instance)
        {
        }
    }

}
