using HealthCheck.DataService.Data;
using HealthCheck.DataService.IRepositories;
using HealthCheck.Entities.DbSet;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCheck.DataService.Repositories
{
    public class RefreshTokensRepository : GenericRepository<RefreshToken>, IRefreshTokensRepository
    {
        public RefreshTokensRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
