using MassTransit;
using Rapido.Framework.Common;
using Rapido.Framework.Postgres;
using Rapido.Saga.Persistence;
using Rapido.Saga.Sagas;

namespace Rapido.Saga;

internal static class Extensions
{
    private const string SectionName = "sagaRabbitMQ";
    
    public static WebApplicationBuilder AddSagas(this WebApplicationBuilder builder)
    {
        var options = builder.Configuration.BindOptions<SagaOptions>(SectionName);
        
        if (!options.Enabled)
        {
            return builder;
        }
        
        builder.Services.AddPostgres<SagaDbContext>(builder.Configuration);
        
        builder.Services.AddMassTransit(busConfig =>
        {
            busConfig.SetKebabCaseEndpointNameFormatter();

            busConfig.AddSagaStateMachine<AccountSetUpSaga, AccountSetUpSagaData>()
                .EntityFrameworkRepository(c =>
                {
                    c.ExistingDbContext<SagaDbContext>();
                    c.UsePostgres();
                });
            
            busConfig.UsingRabbitMq((ctx, config) =>
            {
                config.Host(new Uri(options.Host), hostConfig =>
                {
                    hostConfig.Username(options.Username);
                    hostConfig.Password(options.Password);
                });
                
                config.UseInMemoryOutbox(ctx);
                
                config.ConfigureEndpoints(ctx);
            });
        });

        return builder;
    }
}