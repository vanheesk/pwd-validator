using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using PwdValidator.Service.Utilities;
using Serilog;

namespace PwdValidator.Service.Actions
{
    public class ActionRunAsService : IAction
    {
        
        public IConfiguration Configuration { get; }
        
        public ActionRunAsService()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional:true, reloadOnChange:true)
                .AddEnvironmentVariables()
                .Build();   
        }
        
        public void Execute(string[] args)
        {
            Log.Information($"Run-as-service requested.");

            DbContextFactory.GetInstance().Configuration = Configuration;
            
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()   
                .Build()
                .Run();
        }
        
    }
}