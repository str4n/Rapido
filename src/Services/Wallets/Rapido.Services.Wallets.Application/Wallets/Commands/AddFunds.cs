using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Wallets.Application.Wallets.Commands;

public sealed record AddFunds(
    Guid WalletId, 
    string Currency, 
    double Amount, 
    string TransferName = "Funds transfer", 
    string TransferDescription = "Funds transfer") : ICommand;