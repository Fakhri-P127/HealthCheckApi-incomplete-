using HealthCheck.DataService.IConfiguration;
using HealthCheck.DataService.IRepositories;
using HealthCheck.DataService.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCheck.DataService.Data
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private readonly AppDbContext _context;
        public readonly ILogger _logger;
        public IUsersRepository Users { get => new UsersRepository(_context,_logger) ?? throw new NotImplementedException(); }


        public IRefreshTokensRepository RefreshTokens { get=> new RefreshTokensRepository(_context,_logger) ?? throw new NotImplementedException(); }

        public UnitOfWork(AppDbContext context,ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("db_logs");
            _context = context;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
             _context.Dispose();
        }

      
    }
}
