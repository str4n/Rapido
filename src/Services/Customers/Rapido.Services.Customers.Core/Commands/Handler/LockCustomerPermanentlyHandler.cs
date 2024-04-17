using Microsoft.EntityFrameworkCore;
using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Contracts.Customers.Events;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Customers.Core.EF;
using Rapido.Services.Customers.Core.Entities.Lockout;
using Rapido.Services.Customers.Core.Exceptions;

namespace Rapido.Services.Customers.Core.Commands.Handler;

internal sealed class LockCustomerPermanentlyHandler : ICommandHandler<LockCustomerPermanently>
{
    private readonly CustomersDbContext _dbContext;
    private readonly IClock _clock;
    private readonly IMessageBroker _messageBroker;

    public LockCustomerPermanentlyHandler(CustomersDbContext dbContext, IClock clock, IMessageBroker messageBroker)
    {
        _dbContext = dbContext;
        _clock = clock;
        _messageBroker = messageBroker;
    }
    
    public async Task HandleAsync(LockCustomerPermanently command)
    {
        var customer = await _dbContext.Customers
            .Include(x => x.Lockouts)
            .SingleOrDefaultAsync(x => x.Id == command.CustomerId);

        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }

        var now = _clock.Now();

        var lockout = new PermanentLockout(customer.Id, command.Reason, command.Description, now);
        
        customer.Lock(lockout);
        
        _dbContext.Attach(lockout);
        _dbContext.Entry(lockout).State = EntityState.Added;
        _dbContext.Customers.Update(customer);
        await _dbContext.SaveChangesAsync();
        await _messageBroker.PublishAsync(new CustomerLocked(customer.Id));
    }
}