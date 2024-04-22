using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Wallets.Application.Wallets.Commands;

public sealed record DeductFunds(
    Guid WalletId, 
    string Currency, 
    double Amount, 
    string TransferName = "Deduction of funds", 
    string TransferDescription = "Deduction of funds") : ICommand;