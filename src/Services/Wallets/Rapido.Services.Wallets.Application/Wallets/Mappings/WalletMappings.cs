using Rapido.Services.Wallets.Application.Wallets.DTO;
using Rapido.Services.Wallets.Domain.Wallets.Balance;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Application.Wallets.Mappings;

internal static class WalletMappings
{
    public static WalletDto AsDto(this Wallet wallet)
        => new(
            wallet.Id,
            wallet.OwnerId, 
            wallet.Balances.OrderByDescending(x => x.IsPrimary).Select(x => x.AsDto()),
            wallet.Transfers.OrderByDescending(x => x.CreatedAt).Select(x => x.AsDto()), 
            wallet.CreatedAt);

    private static BalanceDto AsDto(this Balance balance)
        => new(
            balance.Id, 
            balance.WalletId, 
            Math.Round(balance.Amount.Value, 2), 
            balance.Currency, 
            balance.IsPrimary, 
            balance.CreatedAt);

    private static TransferDto AsDto(this Transfer transfer)
        => new(
            transfer.Id, 
            transfer.Name, 
            transfer.Amount, 
            transfer.Currency,
            transfer.GetTransferType(),
            transfer.CreatedAt);

    private static string GetTransferType(this Transfer transfer) 
        => transfer.GetType().ToString().Split(".").Last();
}