namespace Rapido.Services.Notifications.Core.Clients.DTO;

internal sealed record ShortenedUrlDto(string LongUrl, string ShortUrl, DateTime Expires);