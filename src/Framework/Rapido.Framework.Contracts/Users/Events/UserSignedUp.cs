using Rapido.Framework.Common.Abstractions.Events;

namespace Rapido.Framework.Contracts.Users.Events;

public sealed record UserSignedUp(Guid UserId, string Email, DateTime CreatedAt) : IEvent;