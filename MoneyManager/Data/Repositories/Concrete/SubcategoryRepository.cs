using MoneyManager.Data.Repositories.Base;
using MoneyManager.Models;

namespace MoneyManager.Data.Repositories.Concrete
{
    public class SubcategoryRepository : Repository<Account>
    {
        public SubcategoryRepository(MoneyManagerContext context) : base(context)
        {
        }

        // Добавьте дополнительные методы для Account, если необходимо`
    }
}
