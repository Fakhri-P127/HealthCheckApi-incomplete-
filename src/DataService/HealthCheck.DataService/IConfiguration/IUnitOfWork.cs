using HealthCheck.DataService.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCheck.DataService.IConfiguration
{
    public interface IUnitOfWork
    {
        IUsersRepository Users{ get; }
        IRefreshTokensRepository RefreshTokens { get; }
        Task SaveChangesAsync();
    }
}
