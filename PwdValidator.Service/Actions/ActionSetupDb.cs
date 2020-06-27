using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PwdValidator.Service.Utilities;

namespace PwdValidator.Service.Actions
{
    public class ActionSetupDb 
    {
        
        private readonly ILogger<ActionSetupDb> _logger;
        public IConfiguration Configuration { get; }
        
        public ActionSetupDb(/*ILogger<RunSetupDb> logger*/)
        {
            //_logger = logger;

            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional:true, reloadOnChange:true)
                .AddEnvironmentVariables()
                .Build();   
        }

        public void Execute()
        {
            //_logger.LogInformation("Process running at: {time}", DateTimeOffset.Now);
            
            var ddl = new DbAdminFactory(Configuration);
            ddl.Generate();
        }
    }
}