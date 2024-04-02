using Rapido.Framework.CQRS.Commands;

namespace Rapido.Services.Users.Core.Commands;

public sealed record SignIn(string Email, string Password) : ICommand;