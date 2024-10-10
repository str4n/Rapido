using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Common.Exceptions;
using Rapido.Services.Customers.Core.Individual.DTO;
using Rapido.Services.Customers.Core.Individual.Mappings;

namespace Rapido.Services.Customers.Core.Individual.Queries.Handlers;

internal sealed class GetIndividualCustomerHandler : IQueryHandler<GetIndividualCustomer, IndividualCustomerDto>
{
    private readonly ICustomerRepository _repository;

    public GetIndividualCustomerHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IndividualCustomerDto> HandleAsync(GetIndividualCustomer query)
    {
        var customer = await _repository.GetIndividualCustomerAsync(query.Id);

        if (customer is null)
        {
            throw new CustomerNotFoundException(query.Id);
        }

        return customer.AsDto();
    }
}