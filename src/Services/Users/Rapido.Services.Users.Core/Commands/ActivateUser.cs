using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Users.Core.Commands;

public sealed record ActivateUser(string ActivationToken) : ICommand;