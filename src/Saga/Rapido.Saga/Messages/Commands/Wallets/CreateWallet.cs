using Rapido.Framework.Common.Abstractions.Commands;

// ReSharper disable once CheckNamespace
namespace Rapido.Messages.Commands;

public sealed record CreateWallet(Guid OwnerId, string Nationality) : ICommand;