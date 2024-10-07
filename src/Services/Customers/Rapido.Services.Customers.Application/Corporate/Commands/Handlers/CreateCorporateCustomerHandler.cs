using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Services.Customers.Application.Common.Clients;
using Rapido.Services.Customers.Application.Common.Exceptions;
using Rapido.Services.Customers.Domain.Corporate.Customer;
using Rapido.Services.Customers.Domain.Corporate.Repositories;

namespace Rapido.Services.Customers.Application.Corporate.Commands.Handlers;

internal sealed class CreateCorporateCustomerHandler : ICommandHandler<CreateCorporateCustomer>
{
    private readonly ICorporateCustomerRepository _repository;
    private readonly IUserApiClient _apiClient;
    private readonly IClock _clock;

    public CreateCorporateCustomerHandler(ICorporateCustomerRepository repository, IUserApiClient apiClient, IClock clock)
    {
        _repository = repository;
        _apiClient = apiClient;
        _clock = clock;
    }
    
    public async Task HandleAsync(CreateCorporateCustomer command)
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

        var customer = new CorporateCustomer(customerId, user.Email,_clock.Now());

        await _repository.AddAsync(customer);
    }
}