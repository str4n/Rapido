﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rapido.Framework.Common.Time;
using Rapido.Services.Customers.Core.Common.Domain.Customer;
using Rapido.Services.Customers.Core.Common.Domain.Lockout;
using Rapido.Services.Customers.Core.Common.EF;

namespace Rapido.Services.Customers.Core.Common.Services;

internal sealed class CustomerLockoutService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly IClock _clock;
    private readonly ILogger<CustomerLockoutService> _logger;
    private Timer _timer;
    
    public CustomerLockoutService(IServiceProvider serviceProvider, IClock clock, ILogger<CustomerLockoutService> logger)
    {
        _serviceProvider = serviceProvider;
        _clock = clock;
        _logger = logger;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));
        
        return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
        try
        {
            await _semaphore.WaitAsync();

            using var scope = _serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<CustomersDbContext>();

            var now = _clock.Now();
            
            var lockedCustomers = await dbContext.Customers
                .Include(x => x.Lockouts)
                .Where(c => c.IsLocked && c.Lockouts.Any())
                .ToListAsync();

            var customersWithEndedLockout = lockedCustomers
                .Where(x => x.Lockouts.Last() is TemporaryLockout lockout && !lockout.IsActive(now)).ToList();
            
            foreach (var customer in customersWithEndedLockout)
            {
                customer.Unlock(now);
            }
            
            dbContext.UpdateRange(customersWithEndedLockout);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}