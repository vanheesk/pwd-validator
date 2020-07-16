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

        public void Execute(IActionOptions options)
        {
            var actionOptions = (ActionSetupDbOptions) options;
            Log.Information($"Setup requested with option overwrite set to {actionOptions.Overwrite}");
            
            var ddl = new DbAdminFactory(Configuration);
            ddl.Generate();
        }
    }
}