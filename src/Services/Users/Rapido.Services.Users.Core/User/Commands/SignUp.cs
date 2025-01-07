using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Users.Core.User.Commands;

public sealed record SignUp(Guid UserId, string Email, string Password, string AccountType) : ICommand;