using Microsoft.EntityFrameworkCore;
using Rapido.Services.Customers.Domain.Common.Customer;
using Rapido.Services.Customers.Domain.Common.Repositories;

namespace Rapido.Services.Customers.Infrastructure.EF.Repositories;

internal sealed class CustomerRepository : ICustomerRepository
{
    private readonly CustomersDbContext _dbContext;

    public CustomerRepository(CustomersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
        => await _dbContext.Customers.Include(x => x.Lockouts).ToListAsync();

    public async Task<Customer> GetAsync(Guid id)
        => await _dbContext.Customers.SingleOrDefaultAsync(x => x.Id == id);

    public async Task<bool> AnyAsync(string name)
        => await _dbContext.Customers.AnyAsync(x => x.Name == name);

    public async Task UpdateAsync(Customer customer)
    {
        _dbContext.Update(customer);
        await _dbContext.SaveChangesAsync();
    }
}