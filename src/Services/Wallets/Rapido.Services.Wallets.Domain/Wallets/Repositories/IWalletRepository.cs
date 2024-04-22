using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Domain.Wallets.Repositories;

public interface IWalletRepository
{
    Task<Wallet.Wallet> GetAsync(WalletId id);
    Task<Wallet.Wallet> GetAsync(OwnerId ownerId, Currency currency);
    Task<IEnumerable<Wallet.Wallet>> GetAllAsync(OwnerId ownerId, bool tracking = true);
    Task AddAsync(Wallet.Wallet wallet);
    Task UpdateAsync(Wallet.Wallet wallet);
}