using System.Net;
using Humanizer;
using Rapido.Framework.Common.Exceptions;

namespace Rapido.Framework.Api.Exceptions.Mappers;

internal sealed class ExceptionToResponseMapper : IExceptionToResponseMapper
{
    public ExceptionResponse Map(Exception exception)
    {
        var error = new Error(exception.GetType().Name.Replace("Exception", string.Empty).Underscore(), exception.Message);

        if (exception is not CustomException)
        {
            return new ExceptionResponse(HttpStatusCode.InternalServerError, error);
        }

        var response = ((CustomException)exception).Category switch
        {
            ExceptionCategory.NotFound => new ExceptionResponse(HttpStatusCode.NotFound, error),
            ExceptionCategory.ValidationError => new ExceptionResponse(HttpStatusCode.BadRequest, error),
            ExceptionCategory.AlreadyExists => new ExceptionResponse(HttpStatusCode.BadRequest, error),
            ExceptionCategory.BadRequest => new ExceptionResponse(HttpStatusCode.BadRequest, error),
            ExceptionCategory.InternalError => new ExceptionResponse(HttpStatusCode.InternalServerError, error),
            _ => new ExceptionResponse(HttpStatusCode.BadRequest, error)
        };

        return response;
    }
}