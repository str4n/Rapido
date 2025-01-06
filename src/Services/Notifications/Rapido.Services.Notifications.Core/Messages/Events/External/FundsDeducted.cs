using Rapido.Framework.Common.Abstractions.Events;

// ReSharper disable once CheckNamespace
namespace Rapido.Messages.Events;

public sealed record FundsDeducted(Guid WalletId, Guid OwnerId, string TransactionId, 
    string TransferName, string Currency, double Amount, DateTime Date) : IEvent;