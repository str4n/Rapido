using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Services.Customers.Core.Common.Clients;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Common.Exceptions;
using Rapido.Services.Customers.Core.Individual.Domain.Customer;

namespace Rapido.Services.Customers.Core.Individual.Commands.Handlers;

internal sealed class CreateIndividualCustomerHandler : ICommandHandler<CreateIndividualCustomer>
{
    private readonly ICustomerRepository _repository;
    private readonly IClock _clock;
    private readonly IUserApiClient _apiClient;

    public CreateIndividualCustomerHandler(ICustomerRepository repository, IClock clock,
        IUserApiClient apiClient)
    {
        _repository = repository;
        _clock = clock;
        _apiClient = apiClient;
    }
    
    public async Task HandleAsync(CreateIndividualCustomer command)
    {
        var email = command.Email;

        if (await _repository.AnyWithEmailAsync(email))
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