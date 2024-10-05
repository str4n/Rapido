using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Contracts.Customers.Events;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Customers.Application.Common.Exceptions;
using Rapido.Services.Customers.Domain.Common.Customer;
using Rapido.Services.Customers.Domain.Individual.Customer;
using Rapido.Services.Customers.Domain.Individual.Repositories;

namespace Rapido.Services.Customers.Application.Individual.Commands.Handlers;

internal sealed class CompleteIndividualCustomerHandler : ICommandHandler<CompleteIndividualCustomer>
{
    private readonly IIndividualCustomerRepository _individualCustomerRepository;
    private readonly IClock _clock;
    private readonly IMessageBroker _messageBroker;

    public CompleteIndividualCustomerHandler(IIndividualCustomerRepository individualCustomerRepository, IClock clock, IMessageBroker messageBroker)
    {
        _individualCustomerRepository = individualCustomerRepository;
        _clock = clock;
        _messageBroker = messageBroker;
    }

    public async Task HandleAsync(CompleteIndividualCustomer command)
    {
        var customer = await _individualCustomerRepository.GetAsync(command.CustomerId);

        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }

        var name = new Name(command.Name);
        var fullName = new FullName(command.FullName);
        var address = new Address(command.Country, command.Province, command.City, command.Street, command.Postalcode);
        var nationality = new Nationality(command.Nationality);

        if (await _individualCustomerRepository.AnyAsync(name))
        {
            throw new CustomerAlreadyExistsException($"Customer with name: {name} already exists.");
        }
        
        customer.Complete(name, fullName, address, nationality, _clock.Now());

        await _individualCustomerRepository.UpdateAsync(customer);
        await _messageBroker.PublishAsync(new CustomerCompleted(customer.Id, customer.Name, 
            customer.FullName, customer.Nationality));
    }
}