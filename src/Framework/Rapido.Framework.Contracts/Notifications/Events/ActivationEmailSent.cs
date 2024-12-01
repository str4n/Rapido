using Rapido.Framework.Common.Abstractions.Events;

namespace Rapido.Framework.Contracts.Notifications.Events;

public sealed record ActivationEmailSent(Guid UserId) : IEvent;