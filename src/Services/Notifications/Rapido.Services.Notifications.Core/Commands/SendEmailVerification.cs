using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Notifications.Core.Commands;

public sealed record SendEmailVerification(Guid RecipientId, string VerificationToken) : ICommand;