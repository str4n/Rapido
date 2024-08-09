using Rapido.Services.Notifications.Core.Clients.DTO;

namespace Rapido.Services.Notifications.Core.Clients;

internal interface IUrlShortenerApiClient
{
    Task<ShortenedUrlDto> ShortenUrl(string url);
}