using MoneyManager.Models;

namespace MoneyManager.Data.Services
{
    public class BankTransactionService : DataService<BankTransaction>
    {
        public BankTransactionService() : base(DbContextSingleton.Instance)
        {
        }
    }
}
