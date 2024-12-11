using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Messages.Events;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Customers.Core.Common.Domain.Customer;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Common.Exceptions;
using Rapido.Services.Customers.Core.Corporate.Domain.Customer;

namespace Rapido.Services.Customers.Core.Corporate.Commands.Handlers;

internal sealed class CompleteCorporateCustomerEndpoint : ICommandHandler<CompleteCorporateCustomer>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMessageBroker _messageBroker;
    private readonly IClock _clock;

    public CompleteCorporateCustomerEndpoint(ICustomerRepository customerRepository, IMessageBroker messageBroker, 
        IClock clock)
    {
        _customerRepository = customerRepository;
        _messageBroker = messageBroker;
        _clock = clock;
    }
    
    public async Task HandleAsync(CompleteCorporateCustomer command)
    {
        var customer = await _customerRepository.GetCorporateCustomerAsync(command.CustomerId);

        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }

        var name = new Name(command.Name);
        var taxId = new TaxId(command.TaxId);
        var address = new Address(command.Country, command.Province, command.City, command.Street, command.Postalcode);
        var nationality = new Nationality(command.Nationality);

        if (await _customerRepository.AnyWithNameAsync(name))
        {
            throw new CustomerAlreadyExistsException($"Customer with name: {name} already exists.");
        }
        
        customer.Complete(name, address, nationality, taxId, _clock.Now());

        await _customerRepository.UpdateAsync(customer);
        await _messageBroker.PublishAsync(new CorporateCustomerCompleted(customer.Id, customer.Name, 
            customer.TaxId, customer.Nationality));
    }
}