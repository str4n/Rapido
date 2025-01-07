using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Users.Core.User.Commands;

public sealed record SignIn(string Email, string Password) : ICommand;