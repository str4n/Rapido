using Microsoft.EntityFrameworkCore;
using Rapido.Services.Customers.Core.Entities.Customer;
using Rapido.Services.Customers.Core.Repositories;

namespace Rapido.Services.Customers.Core.EF.Repositories;

internal sealed class CustomerRepository : ICustomerRepository
{
    private readonly CustomersDbContext _dbContext;

    public CustomerRepository(CustomersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Customer> GetAsync(Guid id, bool tracking = true)
        => tracking
            ? _dbContext.Customers.SingleOrDefaultAsync(x => x.Id == id)
            : _dbContext.Customers.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

    public Task<Customer> GetAsync(string email, bool tracking = true)
        => tracking
            ? _dbContext.Customers.SingleOrDefaultAsync(x => x.Email == email)
            : _dbContext.Customers.AsNoTracking().SingleOrDefaultAsync(x => x.Email == email);

    public Task<bool> AnyAsync(string name)
        => _dbContext.Customers.AnyAsync(x => x.Name == name);

    public async Task AddAsync(Customer customer)
    {
        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        _dbContext.Update(customer);
        await _dbContext.SaveChangesAsync();
    }
}