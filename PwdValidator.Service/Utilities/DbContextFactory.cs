
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RepoDb;

namespace PwdValidator.Service.Utilities
{
    
    public class DbContextFactory
    {
        
        private static DbContextFactory _instance;

        private DbContextFactory()
        {
            RepoDb.SqlServerBootstrap.Initialize();
        }
        
        public IConfiguration Configuration { get; set; }
        
        public static DbContextFactory GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DbContextFactory();
            }
            
            return _instance ?? new DbContextFactory();
        }    

        public IDbConnection GetConnection()
        {
            var connectionString = Configuration["database:SQLServer:ConnectionString"];
            return new SqlConnection(connectionString).EnsureOpen();
        }

    }
    
}