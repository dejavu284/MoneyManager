using MoneyManager.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MoneyManager.Data.Repositories.Base;
using System.Runtime.Remoting.Contexts;

namespace MoneyManager.Data.Repositories.Concrete
{
    public class CategoryRepository : Repository<Category>
    {
        public CategoryRepository(MoneyManagerContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Category
                .Where(c => c.Status)  // Фильтрация только активных категорий
                .ToListAsync();
        }

        public override async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Category
                .FirstOrDefaultAsync(c => c.CategoryId == id && c.Status);  // Фильтрация только активной категории
        }

        public override async Task DeleteAsync(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category != null)
            {
                category.Status = false;  // Логическое удаление категории
                await _context.SaveChangesAsync();
            }
        }
    }
}
