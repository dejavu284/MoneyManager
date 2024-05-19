using MoneyManager.Models;

namespace MoneyManager.Data.Services
{
    public class CurrencyService : DataService<Currency>
    {
        public CurrencyService() : base(DbContextSingleton.Instance)
        {
        }
    }
}
