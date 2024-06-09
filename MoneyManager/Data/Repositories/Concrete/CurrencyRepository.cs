using MoneyManager.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using MoneyManager.Data.Repositories.Base;
using System.Runtime.Remoting.Contexts;
using System.Collections.Generic;
using System.Linq;

namespace MoneyManager.Data.Repositories.Concrete
{
    public class CurrencyRepository : Repository<Currency>
    {
        public CurrencyRepository(MoneyManagerContext context) : base(context)
        {
        }
        public override async Task<IEnumerable<Currency>> GetAllAsync()
        {
            return await _context.Currency
                .Where(c => c.Status)
                .ToListAsync();
        }

        public async Task<Currency> GetByCodeAsync(string code)
        {
            return await _context.Currency.FirstOrDefaultAsync(c => c.CurrencyCode == code);
        }
        public override async Task DeleteAsync(int id)
        {
            var currency = await _context.Currency.FindAsync(id);
            if (currency != null)
            {
                currency.Status = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}
