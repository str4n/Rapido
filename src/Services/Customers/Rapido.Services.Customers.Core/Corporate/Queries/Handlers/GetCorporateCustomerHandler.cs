using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Common.Exceptions;
using Rapido.Services.Customers.Core.Corporate.DTO;
using Rapido.Services.Customers.Core.Corporate.Mappings;

namespace Rapido.Services.Customers.Core.Corporate.Queries.Handlers;

internal sealed class GetCorporateCustomerHandler : IQueryHandler<GetCorporateCustomer, CorporateCustomerDto>
{
    private readonly ICustomerRepository _repository;

    public GetCorporateCustomerHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<CorporateCustomerDto> HandleAsync(GetCorporateCustomer query)
    { 
        var customer = await _repository.GetCorporateCustomerAsync(query.Id);

        if (customer is null)
        {
            throw new CustomerNotFoundException(query.Id);
        }

        return customer.AsDto();
    }
}