using Rapido.Services.Notifications.Core.Entities;

namespace Rapido.Services.Notifications.Core.Facades;

// Temporary solution. It will be replaced with sendgrid

internal sealed class FakeEmailSenderFacade : IEmailSenderFacade
{
    public FakeEmailSenderFacade()
    {
    }
    
    public async Task SendEmail(string emailAddress, Template template)
    {
        //
    }
}