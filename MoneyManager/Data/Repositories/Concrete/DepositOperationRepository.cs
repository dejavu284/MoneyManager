using MoneyManager.Data.Repositories.Base;
using MoneyManager.Models;

namespace MoneyManager.Data.Repositories.Concrete
{
    public class DepositOperationRepository : Repository<Account>
    {
        public DepositOperationRepository(MoneyManagerContext context) : base(context)
        {
        }

        // Добавьте дополнительные методы для Account, если необходимо
    }
}
