using Microsoft.EntityFrameworkCore;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;
using Rapido.Services.Wallets.Infrastructure.EF;

namespace Rapido.Services.Wallets.Application.Wallets.Services;

internal sealed class TransactionIdGenerator : ITransactionIdGenerator
{
    private readonly WalletsDbContext _dbContext;

    public TransactionIdGenerator(WalletsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<TransactionId> Generate()
    {
        TransactionId transactionId;
        var isValid = false;

        do
        {
            transactionId = TransactionId.Create();

            if (!await _dbContext.Transfers.AnyAsync(x => x.TransactionId == transactionId))
            {
                isValid = true;
            }
        } while (!isValid);

        return transactionId;
    }
}