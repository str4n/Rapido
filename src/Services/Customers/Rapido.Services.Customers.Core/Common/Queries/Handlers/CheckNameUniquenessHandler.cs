using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;

namespace Rapido.Services.Customers.Core.Common.Queries.Handlers;

internal sealed class CheckNameUniquenessHandler(ICustomerRepository repository)
    : IQueryHandler<CheckNameUniqueness, bool>
{
    public async Task<bool> HandleAsync(CheckNameUniqueness query, CancellationToken cancellationToken = default)
        => await repository.AnyWithNameAsync(query.Name);
}