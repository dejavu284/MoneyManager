using MoneyManager.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MoneyManager.Data.Repositories.Base;
using System.Runtime.Remoting.Contexts;

namespace MoneyManager.Data.Repositories.Concrete
{
    public class AccountRepository : Repository<Account>
    {
        public AccountRepository(MoneyManagerContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _context.Account
                .Include(a => a.Currency)
                .Where(a => a.Status)  // Фильтрация только активных счетов
                .ToListAsync();
        }

        public override async Task<Account> GetByIdAsync(int id)
        {
            return await _context.Account
                .Include(a => a.Currency)
                .FirstOrDefaultAsync(a => a.AccountId == id && a.Status);  // Фильтрация только активного счета
        }

        public override async Task DeleteAsync(int id)
        {
            var account = await _context.Account.FindAsync(id);
            if (account != null)
            {
                account.Status = false;  // Логическое удаление счета
                await _context.SaveChangesAsync();
            }
        }
    }
}
