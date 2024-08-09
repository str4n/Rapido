using Rapido.Framework.Common.Abstractions.Queries;

namespace Rapido.Services.Urls.Core.Queries;

public sealed record GetRedirection(string Alias) : IQuery<string>;