using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Messages;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Customers.Core.Common.Domain.Lockout;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Common.Exceptions;

namespace Rapido.Services.Customers.Core.Common.Commands.Handlers;

internal sealed class LockCustomerPermanentlyHandler : ICommandHandler<LockCustomerPermanently>
{
    private readonly ICustomerRepository _repository;
    private readonly IClock _clock;
    private readonly IMessageBroker _messageBroker;

    public LockCustomerPermanentlyHandler(ICustomerRepository repository, IClock clock, IMessageBroker messageBroker)
    {
        _repository = repository;
        _clock = clock;
        _messageBroker = messageBroker;
    }
    
    public async Task HandleAsync(LockCustomerPermanently command)
    {
        var customer = await _repository.GetCustomerAsync(command.CustomerId);

        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }

        var now = _clock.Now();

        var lockout = new PermanentLockout(customer.Id, command.Reason, command.Description, now);
        
        customer.Lock(lockout);

        await _repository.UpdateAsync(customer);
        await _messageBroker.PublishAsync(new CustomerLocked(customer.Id));
    }
}