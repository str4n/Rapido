using Rapido.Framework.Common.Abstractions.Commands;

// ReSharper disable once CheckNamespace
namespace Rapido.Messages.Events;

public sealed record PasswordRecovered(Guid UserId) : ICommand;