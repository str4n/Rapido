using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Customers.Core.Clients;
using Rapido.Services.Customers.Core.Entities.Customer;
using Rapido.Services.Customers.Core.Exceptions;
using Rapido.Services.Customers.Core.Repositories;

namespace Rapido.Services.Customers.Core.Commands.Handler;


//A backup method if for some reason the event consumer wouldn't work.
internal sealed class CreateCustomerHandler : ICommandHandler<CreateCustomer>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IClock _clock;
    private readonly IUserApiClient _apiClient;

    public CreateCustomerHandler(ICustomerRepository customerRepository, IClock clock,
        IUserApiClient apiClient)
    {
        _customerRepository = customerRepository;
        _clock = clock;
        _apiClient = apiClient;
    }
    
    public async Task HandleAsync(CreateCustomer command)
    {
        var email = command.Email;

        if (await _customerRepository.GetAsync(email) is not null)
        {
            throw new CustomerAlreadyExistsException($"Customer with email: {email} already exists.");
        }

        var user = await _apiClient.GetAsync(email);

        if (user is null)
        {
            throw new UserNotFoundException($"User with email: {email} was not found.");
        }

        var customerId = user.UserId;

        var customer = new Customer(customerId, user.Email, _clock.Now());

        await _customerRepository.AddAsync(customer);
    }
}