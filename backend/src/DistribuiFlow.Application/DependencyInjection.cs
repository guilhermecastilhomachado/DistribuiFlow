using DistribuiFlow.Application.Interfaces.Services;
using DistribuiFlow.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DistribuiFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}