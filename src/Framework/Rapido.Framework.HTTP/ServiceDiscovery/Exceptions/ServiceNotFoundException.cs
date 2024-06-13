using Rapido.Framework.Common.Exceptions;

namespace Rapido.Framework.HTTP.ServiceDiscovery.Exceptions;

internal sealed class ServiceNotFoundException : CustomException
{
    public ServiceNotFoundException(string serviceName) 
        : base($"Service with name: {serviceName} was not found.",ExceptionCategory.BadRequest)
    {
    }
}