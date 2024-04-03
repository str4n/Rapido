using System.Net;

namespace Rapido.Framework.Api.Exceptions;

internal sealed record ExceptionResponse(HttpStatusCode StatusCode, Error Error);