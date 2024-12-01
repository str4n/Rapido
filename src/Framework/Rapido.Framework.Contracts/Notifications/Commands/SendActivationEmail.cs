using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Framework.Contracts.Notifications.Commands;

public sealed record SendActivationEmail(Guid UserId, string ActivationToken) : ICommand;