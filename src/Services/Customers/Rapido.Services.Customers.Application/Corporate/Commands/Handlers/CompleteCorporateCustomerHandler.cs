using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Contracts.Customers.Events;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Customers.Application.Common.Exceptions;
using Rapido.Services.Customers.Domain.Common.Customer;
using Rapido.Services.Customers.Domain.Common.Repositories;
using Rapido.Services.Customers.Domain.Corporate.Customer;
using Rapido.Services.Customers.Domain.Corporate.Repositories;
using Rapido.Services.Customers.Domain.Individual.Customer;

namespace Rapido.Services.Customers.Application.Corporate.Commands.Handlers;

internal sealed class CompleteCorporateCustomerHandler : ICommandHandler<CompleteCorporateCustomer>
{
    private readonly ICorporateCustomerRepository _repository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMessageBroker _messageBroker;
    private readonly IClock _clock;

    public CompleteCorporateCustomerHandler(ICorporateCustomerRepository repository, ICustomerRepository customerRepository, 
        IMessageBroker messageBroker, IClock clock)
    {
        _repository = repository;
        _customerRepository = customerRepository;
        _messageBroker = messageBroker;
        _clock = clock;
    }
    
    public async Task HandleAsync(CompleteCorporateCustomer command)
    {
        var customer = await _repository.GetAsync(command.CustomerId);

        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }

        var name = new Name(command.Name);
        var taxId = new TaxId(command.TaxId);
        var address = new Address(command.Country, command.Province, command.City, command.Street, command.Postalcode);
        var nationality = new Nationality(command.Nationality);

        if (await _customerRepository.AnyAsync(name))
        {
            throw new CustomerAlreadyExistsException($"Customer with name: {name} already exists.");
        }
        
        customer.Complete(name, address, nationality, taxId, _clock.Now());

        await _repository.UpdateAsync(customer);
        await _messageBroker.PublishAsync(new CorporateCustomerCompleted(customer.Id, customer.Name, 
            customer.TaxId, customer.Nationality));
    }
}