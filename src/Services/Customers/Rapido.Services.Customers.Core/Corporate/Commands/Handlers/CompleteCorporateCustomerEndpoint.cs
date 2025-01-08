using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Messages.Events;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Customers.Core.Common.Domain.Customer;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Common.Exceptions;
using Rapido.Services.Customers.Core.Corporate.Domain.Customer;

namespace Rapido.Services.Customers.Core.Corporate.Commands.Handlers;

internal sealed class CompleteCorporateCustomerEndpoint(
    ICustomerRepository customerRepository,
    IMessageBroker messageBroker,
    IClock clock)
    : ICommandHandler<CompleteCorporateCustomer>
{
    public async Task HandleAsync(CompleteCorporateCustomer command, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetCorporateCustomerAsync(command.CustomerId);

        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }

        var name = new Name(command.Name);
        var taxId = new TaxId(command.TaxId);
        var address = new Address(command.Country, command.Province, command.City, command.Street, command.Postalcode);
        var nationality = new Nationality(command.Nationality);

        if (await customerRepository.AnyWithNameAsync(name))
        {
            throw new CustomerAlreadyExistsException($"Customer with name: {name} already exists.");
        }
        
        customer.Complete(name, address, nationality, taxId, clock.Now());

        await customerRepository.UpdateAsync(customer);
        await messageBroker.PublishAsync(new CorporateCustomerCompleted(customer.Id, customer.Name, 
            customer.TaxId, customer.Nationality));
    }
}