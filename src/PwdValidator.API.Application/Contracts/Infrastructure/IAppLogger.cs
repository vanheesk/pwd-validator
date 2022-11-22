namespace Arke.PwdValidator.API.Application.Contracts.Infrastructure;

public interface IAppLogger<T>
{
    void LogInformation(string message, params object[] args);
    void LogWarning(string message, params object[] args);
    void LogError(Exception ex, string message = null, params object[] args);
}