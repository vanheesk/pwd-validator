using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Arke.PwdValidator.API.Application;

public static class ApplicationDependencyRegistration
{

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }

}
