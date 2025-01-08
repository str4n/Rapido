using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Messages;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Customers.Core.Common.Domain.Lockout;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Common.Exceptions;

namespace Rapido.Services.Customers.Core.Common.Commands.Handlers;

internal sealed class LockCustomerPermanentlyHandler(
    ICustomerRepository repository,
    IClock clock,
    IMessageBroker messageBroker)
    : ICommandHandler<LockCustomerPermanently>
{
    public async Task HandleAsync(LockCustomerPermanently command, CancellationToken cancellationToken = default)
    {
        var customer = await repository.GetCustomerAsync(command.CustomerId);

        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }

        var now = clock.Now();

        var lockout = new PermanentLockout(customer.Id, command.Reason, command.Description, now);
        
        customer.Lock(lockout);

        await repository.UpdateAsync(customer);
        await messageBroker.PublishAsync(new CustomerLocked(customer.Id));
    }
}