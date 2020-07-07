using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Components;
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
            
            DbSettingMapper.Add(typeof(SqlConnection), new SqlServerDbSetting(), true);
            return new SqlConnection(connectionString).EnsureOpen();
            
            // return connectionDb.ToUpper() switch
            // {
            //     "SQLSERVER" => new SqlConnection(connectionString),
            //     "SQLITE" => new SQLiteConnection(connectionString),
            //     _ => null
            // };
        }

        public void CloseConnection(IDbConnection connection)
        {
            connection.Close();
        }

    }
}