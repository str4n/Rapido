using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Messages;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Common.Exceptions;

namespace Rapido.Services.Customers.Core.Common.Commands.Handlers;

internal sealed class UnlockCustomerEndpoint(
    ICustomerRepository customerRepository,
    IClock clock,
    IMessageBroker messageBroker)
    : ICommandHandler<UnlockCustomer>
{
    public async Task HandleAsync(UnlockCustomer command, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetCustomerAsync(command.CustomerId);

        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }

        customer.Unlock(clock.Now());

        await customerRepository.UpdateAsync(customer);
        await messageBroker.PublishAsync(new CustomerUnlocked(customer.Id));
    }
}