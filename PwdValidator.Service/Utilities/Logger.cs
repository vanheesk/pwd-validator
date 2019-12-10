using Serilog;

namespace PasswordValidatorService.Utilities
{
    
    public static class FileLogger
    {

        static FileLogger()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.File("logs\\log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public static void Write(LogLevel logLevel, string messageTemplate, object[] values = null)
        {
            switch (logLevel)
            {
                case LogLevel.DEBUG: 
                    Log.Debug(messageTemplate, values);
                    break;
                case LogLevel.INFO: 
                    Log.Information(messageTemplate, values);
                    break;
                case LogLevel.WARNING: 
                    Log.Warning(messageTemplate, values);
                    break;
                case LogLevel.ERROR: 
                    Log.Error(messageTemplate, values);
                    break;
                case LogLevel.FATAL: 
                    Log.Fatal(messageTemplate, values);
                    break;
                case LogLevel.VERBOSE:
                    Log.Verbose(messageTemplate, values);
                    break;
            }            
        }
    }

    public enum LogLevel
    {
        VERBOSE,
        DEBUG,
        INFO,
        WARNING,
        ERROR,
        FATAL
    }
    
}