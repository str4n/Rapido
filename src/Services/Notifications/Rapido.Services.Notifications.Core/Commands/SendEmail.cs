using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Notifications.Core.Commands;

public sealed record SendEmail(Guid RecipientId, string TemplateName) : ICommand;