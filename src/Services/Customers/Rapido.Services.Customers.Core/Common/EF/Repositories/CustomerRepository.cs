using Microsoft.EntityFrameworkCore;
using Rapido.Services.Customers.Core.Common.Domain.Customer;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Corporate.Domain.Customer;
using Rapido.Services.Customers.Core.Individual.Domain.Customer;

namespace Rapido.Services.Customers.Core.Common.EF.Repositories;

internal sealed class CustomerRepository : ICustomerRepository
{
    private readonly CustomersDbContext _dbContext;

    public CustomerRepository(CustomersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Customer>> GetCustomersAllAsync()
        => await _dbContext.Customers.Include(x => x.Lockouts).ToListAsync();

    public async Task<IndividualCustomer> GetIndividualCustomerAsync(Guid id)
        => await _dbContext.Customers.OfType<IndividualCustomer>().SingleOrDefaultAsync(x => x.Id == id);

    public async Task<CorporateCustomer> GetCorporateCustomerAsync(Guid id)
        => await _dbContext.Customers.OfType<CorporateCustomer>().SingleOrDefaultAsync(x => x.Id == id);

    public async Task<Customer> GetCustomerAsync(Guid id)
        => await _dbContext.Customers.SingleOrDefaultAsync(x => x.Id == id);

    public async Task<bool> AnyWithNameAsync(string name)
        => await _dbContext.Customers.AnyAsync(x => x.Name == name);

    public async Task<bool> AnyWithEmailAsync(string email)
        => await _dbContext.Customers.AnyAsync(x => x.Email == email);

    public async Task UpdateAsync(Customer customer)
    {
        _dbContext.Update(customer);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddAsync(Customer customer)
    {
        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.SaveChangesAsync();
    }
}