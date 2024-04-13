using Microsoft.EntityFrameworkCore;
using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Services.Customers.Core.EF;
using Rapido.Services.Customers.Core.Entities.Lockout;
using Rapido.Services.Customers.Core.Exceptions;
using Rapido.Services.Customers.Core.Repositories;

namespace Rapido.Services.Customers.Core.Commands.Handler;

internal sealed class LockCustomerHandler : ICommandHandler<LockCustomer>
{
    private readonly CustomersDbContext _dbContext;
    private readonly IClock _clock;

    public LockCustomerHandler(CustomersDbContext dbContext, IClock clock)
    {
        _dbContext = dbContext;
        _clock = clock;
    }
    
    public async Task HandleAsync(LockCustomer command)
    {
        var customer = await _dbContext.Customers
            .Include(x => x.Lockouts)
            .SingleOrDefaultAsync(x => x.Id == command.CustomerId);

        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }

        var lockout = new Lockout(customer.Id, command.Reason, command.Description, _clock.Now());

        if (command.EndDate != default)
        {
            lockout.EndDate = command.EndDate;
        }
        
        customer.Lock(lockout);
        
        _dbContext.Attach(lockout);
        _dbContext.Entry(lockout).State = EntityState.Added;
        _dbContext.Customers.Update(customer);
        await _dbContext.SaveChangesAsync();
    }
}