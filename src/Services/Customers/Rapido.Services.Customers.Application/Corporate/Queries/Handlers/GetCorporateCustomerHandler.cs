using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Customers.Application.Common.Exceptions;
using Rapido.Services.Customers.Application.Corporate.DTO;
using Rapido.Services.Customers.Application.Corporate.Mappings;
using Rapido.Services.Customers.Domain.Corporate.Repositories;

namespace Rapido.Services.Customers.Application.Corporate.Queries.Handlers;

internal sealed class GetCorporateCustomerHandler : IQueryHandler<GetCorporateCustomer, CorporateCustomerDto>
{
    private readonly ICorporateCustomerRepository _repository;

    public GetCorporateCustomerHandler(ICorporateCustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<CorporateCustomerDto> HandleAsync(GetCorporateCustomer query)
    { 
        var customer = await _repository.GetAsync(query.Id);

        if (customer is null)
        {
            throw new CustomerNotFoundException(query.Id);
        }

        return customer.AsDto();
    }
}