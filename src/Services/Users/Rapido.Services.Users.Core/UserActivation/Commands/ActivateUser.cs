using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Users.Core.UserActivation.Commands;

public sealed record ActivateUser(string ActivationToken) : ICommand;