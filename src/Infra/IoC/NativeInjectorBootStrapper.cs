using Mttechne.Domain.Interfaces;
using Mttechne.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Mttechne.Infra.Data.Repository;
using Mttechne.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Mttechne.Toolkit.OutBox.Producer;

namespace Mttechne.Infra.IoC;

public static class NativeInjectorBootStrapper
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, string stringConnection)
    {
        // Application
        services.AddScoped<IMovementAppService, MovementAppService>();
        // Infra - Data
        services.AddScoped<IMovementRepository, MovementRepository>();
        services.AddDbContext<MttechneContext>(opt => opt.UseNpgsql(stringConnection));
        services.AddHostedService(o => new RecreateDbHostedService<MttechneContext>(false, o));
        return services;
    }
}