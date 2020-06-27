using System;
using System.IO;
using System.Reflection;
using DbUp;
using Microsoft.Extensions.Configuration;

namespace PwdValidator.Service.Utilities
{
    
    public class DbAdminFactory
    {

        private readonly IConfiguration Configuration;

        public DbAdminFactory(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void Generate()
        {
            var connectionString = Configuration["database:SQLServer:ConnectionString"];
            EnsureDatabase.For.SqlDatabase(connectionString);

            var scriptLocation = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "data\\scripts");
            var upgrader = DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsFromFileSystem(scriptLocation)
                    .LogToConsole()
                    .Build();
            
            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                throw new Exception("Database creation failed...");
            }
        }
        
    }
    
}