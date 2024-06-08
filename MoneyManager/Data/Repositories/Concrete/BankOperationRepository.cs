using MoneyManager.Models;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Data.Repositories.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace MoneyManager.Data.Repositories.Concrete
{
    public class BankOperationRepository : Repository<BankOperation>
    {
        public BankOperationRepository(MoneyManagerContext context) : base(context)
        {
        }
        public override async Task<IEnumerable<BankOperation>> GetAllAsync()
        {
            return await _context.BankOperation
                .Include(a => a.Subcategory)
                .Include(a => a.Account)
                .ToListAsync();
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
        public async Task<IEnumerable<BankOperation>> GetOperationsForPeriodAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.BankOperation
                .Where(bo => bo.OperationDate >= startDate && bo.OperationDate <= endDate)
                .ToListAsync();
        }
    }
}
