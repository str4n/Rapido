using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Urls.Core.Commands;

public sealed record ShortenUrl(string Scheme, string Host, string Url) : ICommand;