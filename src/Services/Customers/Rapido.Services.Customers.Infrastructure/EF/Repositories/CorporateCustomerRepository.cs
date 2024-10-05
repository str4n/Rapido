using Microsoft.EntityFrameworkCore;
using Rapido.Services.Customers.Domain.Corporate.Customer;
using Rapido.Services.Customers.Domain.Corporate.Repositories;

namespace Rapido.Services.Customers.Infrastructure.EF.Repositories;

internal sealed class CorporateCustomerRepository : ICorporateCustomerRepository
{
    private readonly CustomersDbContext _dbContext;

    public CorporateCustomerRepository(CustomersDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<CorporateCustomer>> GetAllAsync()
        => await _dbContext.CorporateCustomers.ToListAsync();
    
    public Task<CorporateCustomer> GetAsync(Guid id, bool tracking = true)
        => tracking
            ? _dbContext.CorporateCustomers
                .Include(x => x.Lockouts)
                .SingleOrDefaultAsync(x => x.Id == id)
            : _dbContext.CorporateCustomers
                .AsNoTracking()
                .Include(x => x.Lockouts)
                .SingleOrDefaultAsync(x => x.Id == id);

    public Task<CorporateCustomer> GetAsync(string email, bool tracking = true)
        => tracking
            ? _dbContext.CorporateCustomers
                .Include(x => x.Lockouts)
                .SingleOrDefaultAsync(x => x.Email == email)
            : _dbContext.CorporateCustomers
                .AsNoTracking()
                .Include(x => x.Lockouts)
                .SingleOrDefaultAsync(x => x.Email == email);

    public Task<bool> AnyAsync(string name)
        => _dbContext.CorporateCustomers.AnyAsync(x => x.Name == name);

    public async Task AddAsync(CorporateCustomer customer)
    {
        await _dbContext.CorporateCustomers.AddAsync(customer);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(CorporateCustomer customer)
    {
        _dbContext.CorporateCustomers.Update(customer);
        await _dbContext.SaveChangesAsync();
    }
}