namespace Rapido.Services.Urls.Core.Requests;

public sealed record ShortenUrl(string Scheme, string Host, string Url);