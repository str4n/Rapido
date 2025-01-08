using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Common.Exceptions;
using Rapido.Services.Customers.Core.Individual.DTO;
using Rapido.Services.Customers.Core.Individual.Mappings;

namespace Rapido.Services.Customers.Core.Individual.Queries.Handlers;

internal sealed class GetIndividualCustomerHandler(ICustomerRepository repository)
    : IQueryHandler<GetIndividualCustomer, IndividualCustomerDto>
{
    public async Task<IndividualCustomerDto> HandleAsync(GetIndividualCustomer query, CancellationToken cancellationToken = default)
    {
        var customer = await repository.GetIndividualCustomerAsync(query.Id);

        if (customer is null)
        {
            throw new CustomerNotFoundException(query.Id);
        }

        return customer.AsDto();
    }
}