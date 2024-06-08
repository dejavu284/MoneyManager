using MoneyManager.Models;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Data.Repositories.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace MoneyManager.Data.Repositories.Concrete
{
    public class DepositOperationRepository : Repository<DepositOperation>
    {
        public DepositOperationRepository(MoneyManagerContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<DepositOperation>> GetAllAsync()
        {
            return await _context.DepositOperation
                .Include(d => d.Account)
                .Include(d => d.Deposit)
                .ToListAsync();
        }

        public async Task<IEnumerable<DepositOperation>> GetOperationsForPeriodAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.DepositOperation
                .Where(dop => dop.OperationDate >= startDate && dop.OperationDate <= endDate)
                .Include(d => d.Account)
                .Include(d => d.Deposit)
                .ToListAsync();
        }
    }
}
