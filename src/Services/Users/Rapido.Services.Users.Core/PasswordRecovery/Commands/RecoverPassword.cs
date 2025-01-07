using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Users.Core.PasswordRecovery.Commands;

public sealed record RecoverPassword(string Email, string RecoveryToken, string NewPassword) : ICommand;