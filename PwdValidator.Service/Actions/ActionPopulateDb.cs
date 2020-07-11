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

        public void Execute(string[] args)
        {
            Log.Information(args[1] == int.MaxValue.ToString()
                ? $"Populate DB requested without record limit"
                : $"Populate DB requested with option recordcount set to {args[1]}");
            
            Log.Information(args[2] == "1"
                ? $"Minimal occurence count set to include all"
                : $"Minimal occurence count set to {args[2]}");
            
            DbContextFactory.GetInstance().Configuration = Configuration;
            new BatchReader().Read(args[0], Convert.ToInt32(args[1]), Convert.ToInt32(args[2])); }
    }
}