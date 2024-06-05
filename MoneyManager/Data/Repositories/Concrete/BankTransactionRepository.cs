using MoneyManager.Data.Repositories.Base;
using MoneyManager.Models;

namespace MoneyManager.Data.Repositories.Concrete
{
    public class BankTransactionRepository : Repository<Account>
    {
        public BankTransactionRepository(MoneyManagerContext context) : base(context)
        {
        }

        // Добавьте дополнительные методы для Account, если необходимо
    }
}
