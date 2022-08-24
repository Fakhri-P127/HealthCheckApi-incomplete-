using HealthCheck.Entities.DbSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCheck.DataService.IRepositories
{
    public interface IRefreshTokensRepository:IGenericRepository<RefreshToken>
    {
        
    }
}
