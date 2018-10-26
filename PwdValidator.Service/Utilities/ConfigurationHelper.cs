using Microsoft.Extensions.Configuration;

namespace MCMSPasswordValidator.Utilities
{
    public class ConfigurationHelper
    {

        private static ConfigurationHelper _instance;
        private IConfiguration configuration;

        private ConfigurationHelper()
        { }

        public static ConfigurationHelper Instance()
        {
            _instance = _instance ?? new ConfigurationHelper();
            return _instance;
        }
        
        public void Init(string[] args)
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional:true, reloadOnChange:true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();            
        }

        public string GetValue(string key)
        {
            var result = configuration[key];
            return result;
        }
        
    }
    
}