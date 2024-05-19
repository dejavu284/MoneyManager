using MoneyManager.Models;

namespace MoneyManager.Data.Services
{
    public class AccountService : DataService<Account>
    {
        public AccountService() : base(DbContextSingleton.Instance)
        {
        }
    }
}
