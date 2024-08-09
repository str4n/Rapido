using Rapido.Services.Notifications.Core.Clients;
using Rapido.Services.Notifications.Core.Entities;

namespace Rapido.Services.Notifications.Core.Facades;

// Temporary solution. It will be replaced with sendgrid

internal sealed class FakeEmailSenderFacade : IEmailSenderFacade
{
    private readonly IUrlShortenerApiClient _client;

    public FakeEmailSenderFacade(IUrlShortenerApiClient client)
    {
        _client = client;
    }
    
    public async Task SendEmail(string emailAddress, Template template)
    {
        //
    }
}