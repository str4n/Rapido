using Rapido.Services.Wallets.Domain.Wallets.Transfer;

namespace Rapido.Services.Wallets.Application.Wallets.Services;

internal interface ITransactionIdGenerator
{
    Task<TransactionId> Generate();
}