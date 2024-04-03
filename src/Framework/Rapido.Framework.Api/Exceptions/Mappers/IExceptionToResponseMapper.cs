namespace Rapido.Framework.Api.Exceptions.Mappers;

internal interface IExceptionToResponseMapper
{
    ExceptionResponse Map(Exception exception);
}