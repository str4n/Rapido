using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Common;
using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Postgres.Decorators;
using Rapido.Framework.Postgres.Initializers;
using Rapido.Framework.Postgres.UnitOfWork;

namespace Rapido.Framework.Postgres;

public static class Extensions
{
    private const string SectionName = "Postgres";

    public static IServiceCollection AddPostgres<T>(this IServiceCollection services, IConfiguration configuration) where T : DbContext
    {
        var section = configuration.GetSection(SectionName);
        var options = section.BindOptions<PostgresOptions>();
        services.Configure<PostgresOptions>(section);

        services.AddDbContext<T>(x => x.UseNpgsql(options.ConnectionString));
        services.AddHostedService<DatabaseInitializer<T>>();
        services.AddHostedService<DataInitializer>();
        
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        return services;
    }
    
    public static IServiceCollection AddInitializer<T>(this IServiceCollection services) where T : class, IDataInitializer
        => services.AddTransient<IDataInitializer, T>();
    
    public static IServiceCollection AddUnitOfWork<T>(this IServiceCollection services) where T : DbContext
    {
        services.AddScoped<IUnitOfWork, PostgresUnitOfWork<T>>();
        services.TryDecorate(typeof(ICommandHandler<>), typeof(TransactionalCommandHandlerDecorator<>));

        return services;
    }
}