using MoneyManager.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MoneyManager.Data.Repositories.Base;
using System.Runtime.Remoting.Contexts;

namespace MoneyManager.Data.Repositories.Concrete
{
    public class DepositRepository : Repository<Deposit>
    {
        public DepositRepository(MoneyManagerContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Deposit>> GetAllAsync()
        {
            return await _context.Deposit
                .Include(d => d.Currency)
                .Where(d => d.Status)  // Фильтрация только активных вкладов
                .ToListAsync();
        }

        public override async Task<Deposit> GetByIdAsync(int id)
        {
            return await _context.Deposit
                .Include(d => d.Currency)
                .FirstOrDefaultAsync(d => d.DepositId == id && d.Status);  // Фильтрация только активного вклада
        }

        public override async Task DeleteAsync(int id)
        {
            var deposit = await _context.Deposit.FindAsync(id);
            if (deposit != null)
            {
                deposit.Status = false;  // Логическое удаление вклада
                await _context.SaveChangesAsync();
            }
        }
    }
}
