namespace Rapido.Services.Urls.Core.DTO;

public sealed record ShortenedUrlDto(string ShortUrl, string Alias, DateTime Expiry);