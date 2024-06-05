using MoneyManager.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            return await _context.Account.Include(a => a.Currency).ToListAsync();
        }

        public override async Task<Account> GetByIdAsync(int id)
        {
            return await _context.Account.Include(a => a.Currency).FirstOrDefaultAsync(a => a.AccountId == id);
        }
    }
}
