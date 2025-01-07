using Rapido.Framework.Common.Abstractions.Queries;

namespace Rapido.Services.Users.Core.User.Queries;

public sealed record CheckUserEmailUniqueness(string Email) : IQuery<bool>;