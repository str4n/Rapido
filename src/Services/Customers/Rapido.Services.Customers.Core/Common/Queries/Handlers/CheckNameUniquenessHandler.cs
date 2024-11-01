using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;

namespace Rapido.Services.Customers.Core.Common.Queries.Handlers;

internal sealed class CheckNameUniquenessHandler : IQueryHandler<CheckNameUniqueness, bool>
{
    private readonly ICustomerRepository _repository;

    public CheckNameUniquenessHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> HandleAsync(CheckNameUniqueness query)
        => await _repository.AnyWithNameAsync(query.Name);
}