using HealthCheck.DataService.Data;
using HealthCheck.DataService.IRepositories;
using HealthCheck.Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCheck.DataService.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly ILogger _logger;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context,ILogger logger)
        {
            _context = context;
            _logger = logger;
            _dbSet = _context.Set<T>();

        }
        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> GetById(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task<bool> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            return true;
        }

        public virtual async Task<bool> Delete(Guid id, string userId)
        {
            throw new NotImplementedException();
        }

   
        public Task<bool> Upsert(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
