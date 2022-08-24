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
    public class UsersRepository : GenericRepository<User>, IUsersRepository
    {
        
        public UsersRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }


        public override async Task<IEnumerable<User>> GetAll()
        {
            try
            {
                return await _dbSet.Where(x => x.Status == 1)
                    .AsNoTracking().ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"{Repo} GetAll method has generated an error",typeof(UsersRepository));
                return new List<User>();
            }
        }
    }
}
