using System.ComponentModel.Design;
using LetsVotes.Client.Services.Toast;

namespace LetsVotes.Client.Extensions;

public static class StartupExtensions
{
    /// <summary>
    /// Adds any services to the DI container that need to be the same instance on both the server and client side.
    /// </summary>
    /// <param name="services">the application service provider found on the Application Builder</param>
    /// <returns>the IServiceCollection for chaining</returns>
    public static IServiceCollection RegisterCommonServices(this IServiceCollection services)
    {
        services.AddScoped<IToastService, ToastService>();
        return services;
    }
}
