using Rapido.Framework.Common.Abstractions.Commands;

// ReSharper disable once CheckNamespace
namespace Rapido.Messages.Commands;

public sealed record CreateRecipient(Guid Id, string Email) : ICommand;