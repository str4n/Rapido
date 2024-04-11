using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Customers.Core.DTO;
using Rapido.Services.Customers.Core.Exceptions;
using Rapido.Services.Customers.Core.Mappings;
using Rapido.Services.Customers.Core.Repositories;

namespace Rapido.Services.Customers.Core.Queries.Handlers;

internal sealed class GetCustomerHandler : IQueryHandler<GetCustomer, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    
    public async Task<CustomerDto> HandleAsync(GetCustomer query)
    {
        var customer = await _customerRepository.GetAsync(query.CustomerId, false);

        if (customer is null)
        {
            throw new CustomerNotFoundException(query.CustomerId);
        }

        return customer.AsDto();
    }
}