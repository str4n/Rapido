namespace Rapido.Services.Notifications.Core.Clients.Requests;

public sealed record ShortenUrl(string Scheme, string Host, string Url);