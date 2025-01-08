using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Common.Exceptions;
using Rapido.Services.Customers.Core.Corporate.DTO;
using Rapido.Services.Customers.Core.Corporate.Mappings;

namespace Rapido.Services.Customers.Core.Corporate.Queries.Handlers;

internal sealed class GetCorporateCustomerHandler(ICustomerRepository repository)
    : IQueryHandler<GetCorporateCustomer, CorporateCustomerDto>
{
    public async Task<CorporateCustomerDto> HandleAsync(GetCorporateCustomer query, CancellationToken cancellationToken = default)
    { 
        var customer = await repository.GetCorporateCustomerAsync(query.Id);

        if (customer is null)
        {
            throw new CustomerNotFoundException(query.Id);
        }

        return customer.AsDto();
    }
}