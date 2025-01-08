using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Domain.Wallets.Repositories;

public interface IWalletRepository
{
    Task<Wallet.Wallet> GetAsync(WalletId id, CancellationToken cancellationToken = default);
    Task<Wallet.Wallet> GetAsync(OwnerId ownerId, CancellationToken cancellationToken = default);
    Task AddAsync(Wallet.Wallet wallet, CancellationToken cancellationToken = default);
    Task UpdateAsync(Wallet.Wallet wallet, CancellationToken cancellationToken = default);
}