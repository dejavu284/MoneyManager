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
                .Where(c => c.Status)
                .ToListAsync();
        }
    }
}
