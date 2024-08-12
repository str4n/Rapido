using Rapido.Services.Urls.Core.DTO;
using Rapido.Services.Urls.Core.Requests;

namespace Rapido.Services.Urls.Core.Services;

public interface IUrlShortenerService
{
    Task<ShortenedUrlDto> ShortenUrl(ShortenUrl request);
    Task<string> GetRedirection(string alias);
}