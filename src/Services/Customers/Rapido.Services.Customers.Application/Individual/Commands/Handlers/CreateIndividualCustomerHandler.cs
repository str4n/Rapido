using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Services.Customers.Application.Common.Clients;
using Rapido.Services.Customers.Application.Common.Exceptions;
using Rapido.Services.Customers.Domain.Individual.Customer;
using Rapido.Services.Customers.Domain.Individual.Repositories;

namespace Rapido.Services.Customers.Application.Individual.Commands.Handlers;

internal sealed class CreateIndividualCustomerHandler : ICommandHandler<CreateIndividualCustomer>
{
    private readonly IIndividualCustomerRepository _repository;
    private readonly IClock _clock;
    private readonly IUserApiClient _apiClient;

    public CreateIndividualCustomerHandler(IIndividualCustomerRepository repository, IClock clock,
        IUserApiClient apiClient)
    {
        _repository = repository;
        _clock = clock;
        _apiClient = apiClient;
    }
    
    public async Task HandleAsync(CreateIndividualCustomer command)
    {
        var email = command.Email;
        

        if (await _repository.GetAsync(email) is not null)
        {
            throw new CustomerAlreadyExistsException($"Customer with email: {email} already exists.");
        }

        var user = await _apiClient.GetAsync(email);

        if (user is null)
        {
            throw new UserNotFoundException($"User with email: {email} was not found.");
        }

        var customerId = user.UserId;

        var customer = new IndividualCustomer(customerId, user.Email,_clock.Now());

        await _repository.AddAsync(customer);
    }
}