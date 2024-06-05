using MoneyManager.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using MoneyManager.Data.Repositories.Base;
using System.Runtime.Remoting.Contexts;

namespace MoneyManager.Data.Repositories.Concrete
{
    public class CurrencyRepository : Repository<Currency>
    {
        public CurrencyRepository(MoneyManagerContext context) : base(context)
        {
        }

        public async Task<Currency> GetByCodeAsync(string code)
        {
            return await _context.Currency.FirstOrDefaultAsync(c => c.CurrencyCode == code);
        }
    }
}
