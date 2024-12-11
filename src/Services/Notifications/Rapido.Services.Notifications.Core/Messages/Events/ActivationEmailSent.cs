using Rapido.Framework.Common.Abstractions.Events;

namespace Rapido.Services.Notifications.Core.Messages.Events;

public sealed record ActivationEmailSent(Guid UserId, string ActivationToken) : IEvent;