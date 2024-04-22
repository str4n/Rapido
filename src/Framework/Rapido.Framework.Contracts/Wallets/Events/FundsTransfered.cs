using Rapido.Framework.Common.Abstractions.Events;

namespace Rapido.Framework.Contracts.Wallets.Events;

public sealed record FundsTransferred(Guid WalletId, Guid ReceiverWalletId, string TransferName, 
    string Currency, double Amount) : IEvent;