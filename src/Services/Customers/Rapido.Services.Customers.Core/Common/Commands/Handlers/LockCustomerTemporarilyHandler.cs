using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Messages;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Customers.Core.Common.Domain.Lockout;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Common.Exceptions;

namespace Rapido.Services.Customers.Core.Common.Commands.Handlers;

internal sealed class LockCustomerTemporarilyHandler(
    ICustomerRepository repository,
    IClock clock,
    IMessageBroker messageBroker)
    : ICommandHandler<LockCustomerTemporarily>
{
    public async Task HandleAsync(LockCustomerTemporarily command, CancellationToken cancellationToken = default)
    {
        var customer = await repository.GetCustomerAsync(command.CustomerId);

        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }

        var now = clock.Now();

        if (now >= command.EndDate)
        {
            throw new CannotLockCustomerException("End date must be from future.");
        }

        var lockout = new TemporaryLockout(customer.Id, command.Reason, command.Description, now, command.EndDate);
        
        customer.Lock(lockout);

        await repository.UpdateAsync(customer);
        await messageBroker.PublishAsync(new CustomerLocked(customer.Id));
    }
}