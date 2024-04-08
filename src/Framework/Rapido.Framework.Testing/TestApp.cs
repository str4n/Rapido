using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Rapido.Framework.Testing;

public class TestApp<T> : WebApplicationFactory<T> where T : class 
{
    public HttpClient Client { get; }

    public TestApp(string environment = "test")
    {
        Client = WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment(environment);
        }).CreateClient();
    }
}