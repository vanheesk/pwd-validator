using Arke.PwdValidator.API.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Arke.PwdValidator.API.Infrastructure.Logging;

public class AppLogger<T> : IAppLogger<T>
{
    private readonly ILogger<T> _logger;

    public AppLogger(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<T>();
    }
    public void LogInformation(string message, params object[] args)
    {
        _logger.LogInformation(message, args);
    }

    public void LogWarning(string message, params object[] args)
    {
        _logger.LogWarning(message, args);
    }

    public void LogError(Exception ex, string message = null, params object[] args)
    {
        _logger.LogError(ex, message, args);
    }
}