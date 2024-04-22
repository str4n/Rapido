using Rapido.Framework.Common.Abstractions.Events;

namespace Rapido.Framework.Contracts.Wallets.Events;

public sealed record FundsDeducted(
    Guid WalletId, 
    string TransferName, 
    string Currency, 
    double Amount) : IEvent;