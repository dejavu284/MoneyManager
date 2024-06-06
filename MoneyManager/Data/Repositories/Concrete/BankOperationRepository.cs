using MoneyManager.Models;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Data.Repositories.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Data.Repositories.Concrete
{
    public class BankOperationRepository : Repository<BankOperation>
    {
        public BankOperationRepository(MoneyManagerContext context) : base(context)
        {
        }

        public async Task<IEnumerable<BankOperation>> GetByAccountIdAsync(int accountId)
        {
            return await _context.BankOperation
                .Where(b => b.AccountId == accountId)
                .ToListAsync();
        }

        public async Task<IEnumerable<BankOperation>> GetBySubcategoryIdAsync(int subcategoryId)
        {
            return await _context.BankOperation
                .Where(b => b.SubcategoryId == subcategoryId)
                .ToListAsync();
        }
    }
}
