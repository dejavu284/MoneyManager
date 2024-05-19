using MoneyManager.Models;

namespace MoneyManager.Data.Services
{
    public class DepositOperationService : DataService<DepositOperation>
    {
        public DepositOperationService() : base(DbContextSingleton.Instance)
        {
        }
    }
}
