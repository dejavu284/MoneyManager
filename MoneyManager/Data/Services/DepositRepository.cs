using MoneyManager.Models;

namespace MoneyManager.Data.Services
{
    public class DepositService : DataService<Deposit>
    {
        public DepositService() : base(DbContextSingleton.Instance)
        {
        }
    }
}
