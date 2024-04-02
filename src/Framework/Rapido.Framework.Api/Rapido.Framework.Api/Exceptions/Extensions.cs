using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Api.Exceptions.Mappers;
using Rapido.Framework.Api.Exceptions.Middlewares;

namespace Rapido.Framework.Api.Exceptions;

public static class Extensions
{
    public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
        => services
            .AddSingleton<ExceptionHandlerMiddleware>()
            .AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>();
    
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
        => app.UseMiddleware<ExceptionHandlerMiddleware>();
}