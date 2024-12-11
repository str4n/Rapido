using Rapido.Framework.Common.Abstractions.Events;

// ReSharper disable once CheckNamespace
namespace Rapido.Messages.Events;

public sealed record WalletCreated(Guid WalletId, Guid OwnerId, string Currency) : IEvent;