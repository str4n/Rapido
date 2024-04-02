using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Rapido.Framework.Api.Exceptions.Mappers;

namespace Rapido.Framework.Api.Exceptions.Middlewares;

internal sealed class ExceptionHandlerMiddleware : IMiddleware
{
    private readonly IExceptionToResponseMapper _mapper;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(IExceptionToResponseMapper mapper, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await HandleExceptionAsync(exception, context);
        }
    }

    private async Task HandleExceptionAsync(Exception exception, HttpContext context)
    {
        var response = _mapper.Map(exception);

        context.Response.StatusCode = (int)response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}