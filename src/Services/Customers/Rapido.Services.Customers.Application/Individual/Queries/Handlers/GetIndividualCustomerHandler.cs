using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Customers.Application.Individual.DTO;
using Rapido.Services.Customers.Application.Individual.Mappings;
using Rapido.Services.Customers.Domain.Individual.Repositories;

namespace Rapido.Services.Customers.Application.Individual.Queries.Handlers;

internal sealed class GetIndividualCustomerHandler : IQueryHandler<GetIndividualCustomer, IndividualCustomerDto>
{
    private readonly IIndividualCustomerRepository _repository;

    public GetIndividualCustomerHandler(IIndividualCustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IndividualCustomerDto> HandleAsync(GetIndividualCustomer query)
        => (await _repository.GetAsync(query.Id)).AsDto();
}