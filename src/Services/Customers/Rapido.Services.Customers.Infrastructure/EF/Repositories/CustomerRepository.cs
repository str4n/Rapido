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
    {
        var corporateCustomers = _dbContext.CorporateCustomers;
        var individualCustomers = _dbContext.IndividualCustomers;

        var result = individualCustomers.Union<Customer>(corporateCustomers);

        return await result.Include(x => x.Lockouts).ToListAsync();
    }

    public async Task<Customer> GetAsync(Guid id)
    {
        var corporateCustomers = await _dbContext.CorporateCustomers.ToListAsync();
        var individualCustomers = await _dbContext.IndividualCustomers.ToListAsync();

        var customer = (Customer)corporateCustomers.SingleOrDefault(x => x.Id == id) 
                             ?? individualCustomers.SingleOrDefault(x => x.Id == id);

        return customer;
    }

    public async Task UpdateAsync(Customer customer)
    {
        _dbContext.Update(customer);
        await _dbContext.SaveChangesAsync();
    }
}