using Rapido.Framework.Common.Abstractions;
using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Users.Core.Commands;

public sealed record SignUp(Guid UserId, string Email, string Password) : ICommand;