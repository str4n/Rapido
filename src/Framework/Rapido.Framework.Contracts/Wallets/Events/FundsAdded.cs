using Rapido.Framework.Common.Abstractions.Events;

namespace Rapido.Framework.Contracts.Wallets.Events;

public sealed record FundsAdded(
    Guid WalletId, 
    string TransferName, 
    string Currency, 
    double Amount) : IEvent;