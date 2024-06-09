using MoneyManager.Models;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Data.Repositories.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Data.Repositories.Concrete
{
    public class SubcategoryRepository : Repository<Subcategory>
    {
        public SubcategoryRepository(MoneyManagerContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Subcategory>> GetAllAsync()
        {
            return await _context.Subcategory
                .Include(c => c.Category)
                .Where(c => c.Status)
                .ToListAsync();
        }
        public override async Task DeleteAsync(int id)
        {
            var subcategory = await _context.Subcategory.FindAsync(id);
            if (subcategory != null)
            {
                subcategory.Status = false;
                await _context.SaveChangesAsync();
            }
        }

    }
}
