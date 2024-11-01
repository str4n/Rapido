using Rapido.Framework.Common.Abstractions.Queries;

namespace Rapido.Services.Users.Core.Queries;

public sealed record CheckUserEmailUniqueness(string Email) : IQuery<bool>;