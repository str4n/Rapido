using MassTransit.Initializers;
using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Customers.Core.DTO;
using Rapido.Services.Customers.Core.Mappings;
using Rapido.Services.Customers.Core.Repositories;

namespace Rapido.Services.Customers.Core.Queries.Handlers;

internal sealed class GetCustomersHandler : IQueryHandler<GetCustomers, IEnumerable<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomersHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<CustomerDto>> HandleAsync(GetCustomers query)
        => (await _customerRepository.GetAllAsync()).Select(x => x.AsDto());
}