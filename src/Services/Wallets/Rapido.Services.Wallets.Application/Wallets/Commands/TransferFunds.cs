using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Wallets.Application.Wallets.Commands;

public sealed record TransferFunds(
    Guid OwnerId, 
    Guid WalletId, 
    Guid ReceiverWalletId, 
    string TransferName,
    string Currency, 
    double Amount) : ICommand;