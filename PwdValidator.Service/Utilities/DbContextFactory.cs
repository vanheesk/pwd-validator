using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using RepoDb;

namespace PwdValidator.Service.Utilities
{
    
    public class DbContextFactory
    {
        private static DbContextFactory _instance;

        private DbContextFactory()
        { }

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
            RepoDb.SqlServerBootstrap.Initialize();

            var connectionString = ConfigurationHelper.Instance().GetValue("database:SQLServer:ConnectionString");
            
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