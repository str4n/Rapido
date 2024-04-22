using Rapido.Services.Wallets.Application.Wallets.DTO;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Application.Wallets.Mappings;

internal static class WalletMappings
{
    public static WalletDto AsDto(this Wallet wallet)
        => new(
            wallet.Id,
            wallet.OwnerId, 
            wallet.Amount, 
            wallet.Currency, 
            wallet.Transfers.Select(x => x.AsDto()), 
            wallet.CreatedAt);

    private static TransferDto AsDto(this Transfer transfer)
        => new(
            transfer.Id, 
            transfer.Name, 
            transfer.Description, 
            transfer.Amount, 
            transfer.Currency,
            transfer.GetTransferType(),
            transfer.CreatedAt);

    private static string GetTransferType(this Transfer transfer) 
        => transfer.GetType().ToString().Split(".").Last();
}