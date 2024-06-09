using Microsoft.EntityFrameworkCore;
using MoneyManager.Models.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Data.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly MoneyManagerContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(MoneyManagerContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
