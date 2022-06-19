using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleParser.Repository.Base
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class //1) Паттерны проектирования
    {
        private readonly DbContext _context;

        public BaseRepository(DbContext context)
        {
            _context = context;
        }

        public virtual IQueryable<T> DbSet => _context.Set<T>();

        public DbContext Context => _context;

        public virtual async Task<T?> FindByIdAsync(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task AddAsync(T item, bool autoSaveChanges = true)
        {
            await _context.Set<T>().AddAsync(item);
            await SaveChangesAsync(autoSaveChanges);
        }

        public virtual async Task AddAsync(IEnumerable<T> items, bool autoSaveChanges = true)
        {
            await _context.Set<T>().AddRangeAsync(items);
            await SaveChangesAsync(autoSaveChanges);
        }

        //public async Task AddBulkAsync(IList<T> items, CancellationToken token = default)
        //{
        //    await _context.BulkInsertAsync(items, cancellationToken: token);
        //}

        public virtual async Task UpdateAsync(T item, bool autoSaveChanges = true)
        {

            _context.Set<T>().Update(item);
            await SaveChangesAsync(autoSaveChanges);
        }

        public virtual async Task UpdateAsync(IEnumerable<T> items, bool autoSaveChanges = true)
        {
            _context.Set<T>().UpdateRange(items);
            await SaveChangesAsync(autoSaveChanges);
        }

        public virtual async Task DeleteAsync(T item, bool autoSaveChanges = true)
        {
            _context.Set<T>().Remove(item);
            await SaveChangesAsync(autoSaveChanges);
        }

        public virtual async Task DeleteAsync(IEnumerable<T> items, bool autoSaveChanges = true)
        {
            _context.Set<T>().RemoveRange(items);
            await SaveChangesAsync(autoSaveChanges);
        }

        public virtual async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private async Task SaveChangesAsync(bool autoSaveChanges)
        {
            if (autoSaveChanges)
            {
                await _context.SaveChangesAsync();
            }
        }

        public virtual void Dispose()
        {
        }
    }

}
