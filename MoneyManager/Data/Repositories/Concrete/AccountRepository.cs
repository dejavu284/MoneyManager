using MoneyManager.Data.Repositories.Base;
using MoneyManager.Models;

namespace MoneyManager.Data.Repositories.Concrete
{
    public class AccountRepository : Repository<Account>
    {
        public AccountRepository(MoneyManagerContext context) : base(context)
        {
        }

        // Добавьте дополнительные методы для Account, если необходимо
    }
}
