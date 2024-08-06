using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Notifications.Core.Exceptions;

internal sealed class TemplateNotFoundException : CustomException
{
    public TemplateNotFoundException(string templateName) : base($"Template with name: {templateName} was not found.", ExceptionCategory.NotFound)
    {
    }
}