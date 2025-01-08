using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Messages;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Customers.Core.Common.Domain.Customer;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Common.Exceptions;

namespace Rapido.Services.Customers.Core.Common.Commands.Handlers;

internal sealed class ChangeAddressHandler(ICustomerRepository customerRepository, IMessageBroker messageBroker)
    : ICommandHandler<ChangeAddress>
{
    public async Task HandleAsync(ChangeAddress command, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetCustomerAsync(command.Id);

        if (customer is null)
        {
            throw new CustomerNotFoundException(command.Id);
        }

        var address = new Address(command.Country, command.Province, command.City, command.Street, command.PostalCode);
        
        customer.ChangeAddress(address);

        await customerRepository.UpdateAsync(customer);
        await messageBroker.PublishAsync(new CustomerAddressChanged(customer.Id));
    }
}