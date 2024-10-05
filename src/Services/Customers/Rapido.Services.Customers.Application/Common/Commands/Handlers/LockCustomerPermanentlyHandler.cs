using Microsoft.EntityFrameworkCore;
using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Contracts.Customers.Events;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Customers.Application.Common.Exceptions;
using Rapido.Services.Customers.Domain.Common.Lockout;
using Rapido.Services.Customers.Domain.Common.Repositories;

namespace Rapido.Services.Customers.Application.Common.Commands.Handlers;

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
        var customer = await _repository.GetAsync(command.CustomerId);

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