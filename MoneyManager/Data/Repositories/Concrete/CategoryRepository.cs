using MoneyManager.Data.Repositories.Base;
using MoneyManager.Models;

namespace MoneyManager.Data.Repositories.Concrete
{
    public class CategoryRepository : Repository<Account>
    {
        public CategoryRepository(MoneyManagerContext context) : base(context)
        {
        }

        // Добавьте дополнительные методы для Account, если необходимо
    }
}
