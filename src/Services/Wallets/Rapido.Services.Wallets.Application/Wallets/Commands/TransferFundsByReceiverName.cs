using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Wallets.Application.Wallets.Commands;

public sealed record TransferFundsByReceiverName(
    Guid OwnerId, 
    Guid WalletId, 
    string ReceiverName, 
    string TransferName,
    string Currency, 
    double Amount) : ICommand;