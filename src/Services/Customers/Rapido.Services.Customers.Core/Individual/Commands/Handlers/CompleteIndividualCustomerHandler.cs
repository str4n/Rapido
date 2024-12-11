using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Messages.Events;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Customers.Core.Common.Domain.Customer;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Common.Exceptions;
using Rapido.Services.Customers.Core.Individual.Domain.Customer;

namespace Rapido.Services.Customers.Core.Individual.Commands.Handlers;

internal sealed class CompleteIndividualCustomerHandler : ICommandHandler<CompleteIndividualCustomer>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IClock _clock;
    private readonly IMessageBroker _messageBroker;

    public CompleteIndividualCustomerHandler(ICustomerRepository customerRepository, IClock clock, 
        IMessageBroker messageBroker)
    {
        _customerRepository = customerRepository;
        _clock = clock;
        _messageBroker = messageBroker;
    }

    public async Task HandleAsync(CompleteIndividualCustomer command)
    {
        var customer = await _customerRepository.GetIndividualCustomerAsync(command.CustomerId);

        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }

        var name = new Name(command.Name);
        var fullName = new FullName(command.FullName);
        var address = new Address(command.Country, command.Province, command.City, command.Street, command.Postalcode);
        var nationality = new Nationality(command.Nationality);

        if (await _customerRepository.AnyWithNameAsync(name))
        {
            throw new CustomerAlreadyExistsException($"Customer with name: {name} already exists.");
        }
        
        customer.Complete(name, fullName, address, nationality, _clock.Now());

        await _customerRepository.UpdateAsync(customer);
        await _messageBroker.PublishAsync(new IndividualCustomerCompleted(customer.Id, customer.Name, 
            customer.FullName, customer.Nationality));
    }
}