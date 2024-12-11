using Rapido.Framework.Common.Abstractions.Events;

// ReSharper disable once CheckNamespace
namespace Rapido.Messages.Events;

public sealed record FundsTransferred(Guid WalletId, Guid ReceiverWalletId, string TransferName, string Currency, double Amount) : IEvent;