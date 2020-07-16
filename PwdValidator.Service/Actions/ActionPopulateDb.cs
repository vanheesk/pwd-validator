using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PwdValidator.Service.Utilities;
using Serilog;

namespace PwdValidator.Service.Actions
{
    public class ActionPopulateDb : IAction
    {
        private IConfiguration Configuration { get; }
        
        public ActionPopulateDb()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional:true, reloadOnChange:true)
                .AddEnvironmentVariables()
                .Build();   
        }

        public void Execute(IActionOptions options)
        {
            var actionOptions = (ActionPopulateDbOptions) options;
            
            Log.Information(actionOptions.Limit == int.MaxValue
                ? $"Populate DB requested without record limit"
                : $"Populate DB requested with option recordcount set to {actionOptions.Limit}");
            
            Log.Information(actionOptions.MinPrevalance == Constants.DEFAULT_MIN_PREVALENCE
                ? $"Minimal occurence count set to include all"
                : $"Minimal occurence count set to {actionOptions.MinPrevalance}");
            
            DbContextFactory.GetInstance().Configuration = Configuration;
            new BatchReader().Read(actionOptions); }
    }
}