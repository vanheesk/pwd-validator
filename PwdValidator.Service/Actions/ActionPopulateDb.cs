using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PwdValidator.Service.Utilities;

namespace PwdValidator.Service.Actions
{
    public class ActionPopulateDb 
    {
        
        private readonly ILogger<ActionPopulateDb> _logger;
        
        public IConfiguration Configuration { get; }
        
        public ActionPopulateDb(/*ILogger<RunSetupDb> logger*/)
        {
            //_logger = logger;

            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional:true, reloadOnChange:true)
                .AddEnvironmentVariables()
                .Build();   
        }

        public void Execute(string[] args)
        {
            new BatchReader().Read(args[0], Convert.ToInt32(args[1]), Convert.ToInt32(args[2])); }
    }
}