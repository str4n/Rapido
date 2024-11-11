using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Common.Abstractions.Dispatchers;

namespace Rapido.Framework.Testing;

public sealed class TestApp<T> : WebApplicationFactory<T> where T : class
{
    public HttpClient Client { get; }
    public IServiceScope Scope { get; private set; }
    public IDispatcher Dispatcher { get; private set; }

    public TestApp(Action<IServiceCollection> services = null, Dictionary<string, string> options = null)
    {
        Client = WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("test");
            
            if (services is not null)
            {
                builder.ConfigureTestServices(services);
            }

            if (options is not null)
            {
                var configuration = new ConfigurationBuilder().AddInMemoryCollection(options).Build();
                builder.UseConfiguration(configuration);
            }
            
            builder.ConfigureServices(s =>
            {
                var sp = s.BuildServiceProvider();
                Scope = sp.CreateScope();
                Dispatcher = Scope.ServiceProvider.GetRequiredService<IDispatcher>();
            });
        }).CreateClient();
    }
}