using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Users.Core.PasswordRecovery.Commands;

public sealed record CreateRecoveryToken(string Email) : ICommand;