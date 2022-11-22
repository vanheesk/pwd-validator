using Arke.PwdValidator.API.Application.Contracts.Infrastructure;
using Arke.PwdValidator.API.Infrastructure.Logging;
using Arke.PwdValidator.API.Infrastructure.Services;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arke.PwdValidator.API.Infrastructure;

public static class InfrastructureServiceRegistration
{

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(typeof(IAppLogger<>), typeof(AppLogger<>));
        services.AddScoped<IBlobStorageService, BlobStorageService>();

        var blobStorageConnectionString = configuration.GetConnectionString("BlobStorage");
        if (blobStorageConnectionString != null)
        {
            services.AddAzureClients(builder =>
            {
#if DEBUG
                builder.AddBlobServiceClient(new Uri(blobStorageConnectionString));
#else
                var visualStudioTenantId = string.Empty;
                builder.AddBlobServiceClient(new Uri(blobStorageConnectionString))
                    .WithCredential(new DefaultAzureCredential(new DefaultAzureCredentialOptions { VisualStudioTenantId = visualStudioTenantId }));
#endif
            });
        }

        //services.AddHttpLogging(logging =>
        //{
        //    logging.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders | HttpLoggingFields.RequestBody | HttpLoggingFields.ResponsePropertiesAndHeaders;
        //});

        return services;
    }

}
