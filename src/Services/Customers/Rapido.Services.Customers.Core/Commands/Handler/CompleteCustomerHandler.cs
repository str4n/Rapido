using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Contracts.Customers.Events;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Customers.Core.Entities.Customer;
using Rapido.Services.Customers.Core.Exceptions;
using Rapido.Services.Customers.Core.Repositories;

namespace Rapido.Services.Customers.Core.Commands.Handler;

internal sealed class CompleteCustomerHandler : ICommandHandler<CompleteCustomer>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IClock _clock;
    private readonly IMessageBroker _messageBroker;

    public CompleteCustomerHandler(ICustomerRepository customerRepository, IClock clock, IMessageBroker messageBroker)
    {
        _customerRepository = customerRepository;
        _clock = clock;
        _messageBroker = messageBroker;
    }
    
    public async Task HandleAsync(CompleteCustomer command)
    {
        var customer = await _customerRepository.GetAsync(command.CustomerId);

        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }

        var name = new Name(command.Name);
        var fullName = new FullName(command.FullName);
        var address = new Address(command.Country, command.Province, command.City, command.Street, command.Postalcode);
        var nationality = new Nationality(command.Nationality);
        var identity = new Identity(command.IdentityType, command.IdentitySeries);

        if (await _customerRepository.AnyAsync(name))
        {
            throw new CustomerAlreadyExistsException($"Customer with name: {name} already exists.");
        }
        
        customer.Complete(name, fullName, address, nationality, identity, _clock.Now());

        await _customerRepository.UpdateAsync(customer);
        await _messageBroker.PublishAsync(new CustomerCompleted(customer.Id, customer.Name, 
            customer.FullName, customer.Nationality));
    }
}