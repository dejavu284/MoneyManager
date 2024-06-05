using MoneyManager.Data.Repositories.Base;
using MoneyManager.Models;

namespace MoneyManager.Data.Repositories.Concrete
{
    public class DepositRepository : Repository<Deposit>
    {
        public DepositRepository(MoneyManagerContext context) : base(context)
        {
        }

        // Добавьте дополнительные методы для Deposit, если необходимо
    }
}
