using Rapido.Framework.Common.Abstractions.Events;

namespace Rapido.Framework.Contracts.Users.Events;

public sealed record UserActivated(Guid UserId) : IEvent;