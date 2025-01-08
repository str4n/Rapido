using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Messages.Events;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Customers.Core.Common.Domain.Customer;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Common.Exceptions;
using Rapido.Services.Customers.Core.Individual.Domain.Customer;

namespace Rapido.Services.Customers.Core.Individual.Commands.Handlers;

internal sealed class CompleteIndividualCustomerHandler(
    ICustomerRepository customerRepository,
    IClock clock,
    IMessageBroker messageBroker)
    : ICommandHandler<CompleteIndividualCustomer>
{
    public async Task HandleAsync(CompleteIndividualCustomer command, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetIndividualCustomerAsync(command.CustomerId);

        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }

        var name = new Name(command.Name);
        var fullName = new FullName(command.FullName);
        var address = new Address(command.Country, command.Province, command.City, command.Street, command.Postalcode);
        var nationality = new Nationality(command.Nationality);

        if (await customerRepository.AnyWithNameAsync(name))
        {
            throw new CustomerAlreadyExistsException($"Customer with name: {name} already exists.");
        }
        
        customer.Complete(name, fullName, address, nationality, clock.Now());

        await customerRepository.UpdateAsync(customer);
        await messageBroker.PublishAsync(new IndividualCustomerCompleted(customer.Id, customer.Name, 
            customer.FullName, customer.Nationality));
    }
}