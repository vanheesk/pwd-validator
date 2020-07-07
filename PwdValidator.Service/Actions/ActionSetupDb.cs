using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PwdValidator.Service.Utilities;
using Serilog;

namespace PwdValidator.Service.Actions
{
    public class ActionSetupDb : IAction
    {
        
        public IConfiguration Configuration { get; }
        
        public ActionSetupDb()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional:true, reloadOnChange:true)
                .AddEnvironmentVariables()
                .Build();   
        }

        public void Execute(string[] args)
        {
            Log.Information($"Setup requested with option overwrite set to {args[0]}");
            
            var ddl = new DbAdminFactory(Configuration);
            ddl.Generate();
        }
    }
}